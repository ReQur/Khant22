using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnetserver.Controllers
{
	[ApiController]
	[Authorize]
	[Route("api/[controller]")]
	public class RequestController : ControllerBase
	{
	}
}
