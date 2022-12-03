using dotnetserver.Models;
using dotnetserver.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotnetserver.Controllers
{
	[ApiController]
	[Authorize]
	[Route("api/[controller]")]
	public class RequestController : ControllerBase
	{
		private readonly IRequestService _RequestService;

		public RequestController(IRequestService RequestService)
		{
			_RequestService = RequestService;
		}

		[HttpGet(nameof(GetRequests) + "/organizationId")]
		public Task<IEnumerable<Request>> GetRequests(int organizationId)
		{
			return default;
		}

		[HttpGet(nameof(GetRequests))]
		public Task<IEnumerable<Request>> GetRequests()
		{
			return default;
		}

		[HttpPost(nameof(EditRequest))]
		public Task<Request> EditRequest([FromBody] Request request)
		{
			return default;
		}
	}
}
