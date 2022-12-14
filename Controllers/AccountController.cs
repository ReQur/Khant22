using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using dotnetserver.Models;
using dotnetserver.Services.JWT;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace dotnetserver.Controllers
{
    [ApiController]
   // [Authorize]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IUserService _userService;
        private readonly IJwtAuthManager _jwtAuthManager;

        public AccountController(ILogger<AccountController> logger, IUserService userService, IJwtAuthManager jwtAuthManager)
        {
            _logger = logger;
            _userService = userService;
            _jwtAuthManager = jwtAuthManager;
        }

        /// <summary>
        /// Authorize user by given credentials
        /// </summary>
        /// <remarks>
        /// That method generates new pair access/refresh tokens,
        /// Adds user to server claims,
        /// Returns all user data.
        /// </remarks>
        /// <returns>Returns all user data</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request if model state is invalid</response>
        /// <response code="401">Unauthorized if got incorrect credentials</response>
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResult), 200)]
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!await _userService.IsValidUserCredentials(request.Login, request.Password))
            {
                return Unauthorized();
            }

            var user = await _userService.GetUserData(request.Login);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.login),
                new Claim(ClaimTypes.NameIdentifier, user.userId.ToString())
            };

            var jwtResult = _jwtAuthManager.GenerateTokens(request.Login, claims, DateTime.Now);
            _logger.LogInformation($"User [{request.Login}] logged in the system.");
            return Ok(new LoginResult
            {
                login = request.Login,
                userId = user.userId,
                firstName = user.firstName,
                lastName = user.lastName,
                organization = user.organization,
                jobTitle = user.jobTitle,
				AccessToken = jwtResult.AccessToken,
                RefreshToken = jwtResult.RefreshToken.TokenString
            });
        }

        /// <summary>
        /// Register user by given credentials
        /// </summary>
        /// <remarks>
        /// That method add new user data to database,
        /// Generates new pair access/refresh tokens,
        /// Adds user to server claims,
        /// Returns all user data.
        /// </remarks>
        /// <returns>Returns all user data</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request if model state is invalid</response>
        /// <response code="401">Unauthorized if got already registered email</response>
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResult), 200)]
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] User request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (await _userService.IsAnExistingUser(request.login))
            {
                return Unauthorized();
            }

            await _userService.RegisterUser(request);
            
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,request.login),
                new Claim(ClaimTypes.NameIdentifier, request.userId.ToString())
            };

            var jwtResult = _jwtAuthManager.GenerateTokens(request.login, claims, DateTime.Now);
            _logger.LogInformation($"User [{request.login}] logged in the system.");
            return Ok(new LoginResult
            {
                login = request.login,
                userId = request.userId,
                firstName = request.firstName,
                lastName = request.lastName,
                AccessToken = jwtResult.AccessToken,
                RefreshToken = jwtResult.RefreshToken.TokenString
            });
        }

        /// <summary>
        /// Get user ID from server claims and returns it
        /// </summary>
        /// <remarks>
        /// Use that method to get current session user ID if its needed
        /// </remarks>
        /// <returns>User ID</returns>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthorized if get request from unauthorized client</response>
        [ProducesResponseType(typeof(LoginResult), 200)]
        [HttpGet("user")]
        //[Authorize]
        public async Task<ActionResult> GetCurrentUser()
        {
            //var Login = User.Identity?.Name;
            var Login = "login";
            var user = await _userService.GetUserData(Login);

            return Ok(new LoginResult
            {
                login = user.login,
                userId = user.userId,
                firstName = user.firstName,
                lastName = user.lastName,
                organization = user.organization,
            });
        }

        /// <summary>
        /// Remove user from server claims and delete his access and refresh tokens.
        /// </summary>
        /// <returns>Nothing</returns>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthorized if get request from unauthorized client</response>
        [HttpPost("logout")]
        [Authorize]
        public ActionResult Logout()
        {
            // optionally "revoke" JWT token on the server side --> add the current token to a block-list
            // https://github.com/auth0/node-jsonwebtoken/issues/375

            var Login = User.Identity?.Name;
            _jwtAuthManager.RemoveRefreshTokenByUserName(Login);
            _logger.LogInformation($"User [{Login}] logged out the system.");
            return Ok();
        }

        /// <summary>
        /// reAuthorize user by given refresh token
        /// </summary>
        /// <remarks>
        /// That method generates new pair access/refresh tokens,
        /// Returns all user data.
        /// If got invalid token, method returns Unauthorized status code
        /// </remarks>
        /// <returns>Returns all user data</returns>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthorized if got broken refresh token</response>
        [ProducesResponseType(typeof(LoginResult), 200)]
        [HttpPost("refresh-token")]
        [Authorize]
        public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var Login = User.Identity?.Name;
                _logger.LogInformation($"User [{Login}] is trying to refresh JWT token.");

                if (string.IsNullOrWhiteSpace(request.RefreshToken))
                {
                    return Unauthorized();
                }

                var user = await _userService.GetUserData(Login);

                var accessToken = await HttpContext.GetTokenAsync("Bearer", "access_token");
                var jwtResult = _jwtAuthManager.Refresh(request.RefreshToken, accessToken, DateTime.Now);
                _logger.LogInformation($"User [{Login}] has refreshed JWT token.");
                return Ok(new LoginResult
                {
                    userId = user.userId,
                    firstName = user.firstName,
                    lastName = user.lastName,
                    login = user.login,
                    AccessToken = jwtResult.AccessToken,
                    RefreshToken = jwtResult.RefreshToken.TokenString
                });
            }
            catch (SecurityTokenException e)
            {
                return Unauthorized(e.Message); // return 401 so that the client side can redirect the user to login page
            }
        }
    }


    public class LoginRequest
    {
        [Required]
        [JsonPropertyName("login")]
        public string Login { get; set; }

        [Required]
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }

    public class LoginResult: User
    {

        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; }

        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }
    }

    public class RefreshTokenRequest
    {
        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }
    }
}
