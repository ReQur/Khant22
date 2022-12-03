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
		private readonly IRequestService _requestService;

		public RequestController(IRequestService RequestService)
		{
			_requestService = RequestService;
		}

		[HttpGet(nameof(GetRequests) + "/organizationId")]
		public async Task<IEnumerable<Request>> GetRequests(int organizationId)
		{
			return await _requestService.GetRequests(organizationId);
		}

		[HttpGet(nameof(GetRequests))]
		public async Task<IEnumerable<Request>> GetRequests()
		{
			return await _requestService.GetRequests();
		}

		[HttpPost(nameof(EditRequest))]
		public Task<Request> EditRequest([FromBody] Request request)
		{
			return default;
		}

		[HttpPost(nameof(AddRequest))]
		public Task<Request> AddRequest([FromBody] Request request)
		{
			return default;
		}
	}
}
