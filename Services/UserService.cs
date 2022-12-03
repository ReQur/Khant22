using System;
using System.Linq;
using System.Threading.Tasks;
using dotnetserver.Models;
using Microsoft.Extensions.Logging;
using Dapper;

namespace dotnetserver
{
    public interface IUserService
    {
        Task<bool> IsAnExistingUser(string userName);
        Task<bool> IsValidUserCredentials(string userName, string password);
        Task<string> GetUserId(string userName);
        Task<User> GetUserData(string userName);
        Task<bool> RegisterUser(User user);
    }

    public class UserService : WithDbAccess, IUserService
    {
        private readonly ILogger<UserService> _logger;

        public UserService(ILogger<UserService> logger, ConnectionContext context) : base(context)
        {
            _logger = logger;
        }

        public async Task<bool> IsValidUserCredentials(string login, string password)
        {
            _logger.LogInformation($"Validating user [{login}] with password [{password}]");
            if (string.IsNullOrWhiteSpace(login))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            var parameters = new { Login = login, Password = password };
            var sql = "SELECT * FROM user WHERE login=@login and password=@Password";

            try
            {
                var user = await DbQueryAsync<User>(sql, parameters);
                var _ = user.First();
            }
            catch (Exception e)
            {
                return false;
            }
            
            
            return true;

        }

        public async Task<bool> IsAnExistingUser(string login)
        {
            var parameters = new { login };
            var sql = "SELECT * FROM user WHERE login=@login";
            try
            {
                var user = await DbQueryAsync<User>(sql, parameters);
                var _ = user.First();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public async Task<string> GetUserId(string login)
        {
            var parameters = new { login };
            var sql = "SELECT userId FROM user WHERE login=@login";
            try
            {
                 var user = await DbQueryAsync<string>(sql, parameters);
                 return user.First();

            }
            catch (Exception e)
            {
                throw(new Exception("Was try to get userId of unknown user"));
            }
            
        }

        public async Task<User> GetUserData(string login)
        {
            var parameters = new { login };
            var sql = "SELECT u.*, o.* FROM user u LEFT JOIN organization o on u.organizationId = o.organizationId WHERE login=@login";

            using (var db = _context.GenericConnection())
            {
                db.Open();
                try
                {
                    var user = await db.QueryAsync<User, Organization, User>(
                           sql,
                           (user, org) =>
                           {
                               user.organization = org;
                               return user;
                           }, parameters, splitOn: "organizationId"
                       );
                    return user.First();

                }
                catch (Exception e)
                {
                    throw (new Exception("Was try to get data of unknown user"));
                }
            }
        }

        public async Task<bool> RegisterUser(User user)
        {
            var sql = @"INSERT INTO user(login, firstName, lastName, password, organizationId, jobTitle) 
                            VALUES(@login,  @firstName, @lastName, @password, @organizationId, @jobTitle);";
            try
            {
                await DbExecuteAsync(sql, user);
            }
            catch (Exception e)
            {
                _logger.LogError($"Unexpected error while register new user {e}");
                return false;
            }
            
            return true;
        }
    }

}