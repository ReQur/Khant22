using dotnetserver.Models;
using dotnetserver.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotnetserver.Controllers
{
	[ApiController]
	//[Authorize]
	[Route("api/[controller]")]
	public class RequestController : ControllerBase
	{
		private readonly IRequestService _requestService;

		public RequestController(IRequestService RequestService)
		{
			_requestService = RequestService;
		}

		[HttpGet(nameof(GetRequests) + "{organizationId}")]
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
		public async Task<Request> EditRequest([FromBody] Request request)
		{
			return await _requestService.EditRequest(request);
		}

		[HttpPost(nameof(AddRequest))]
		public async Task<Request> AddRequest([FromBody] Request request)
		{
			return await _requestService.AddRequest(request);
		}

		[HttpGet(nameof(GetQrCode) + "{requestId}")]
		public async Task<ActionResult> GetQrCode(int requestId)
		{
			var result =  await _requestService.GetQrCode(requestId);
			if (result == "")
			{
				return BadRequest();
			}

			return Ok(result);
		}

		[HttpGet(nameof(DecodeQrCode) + "{encoded}")]
		public async Task<ActionResult<Request>> DecodeQrCode(string encoded)
		{
			var result = await _requestService.DecodeQrCode(encoded);
			if (result == null)
			{
				return BadRequest();
			}

			return Ok(result);
		}
	}
}
