using dotnetserver.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotnetserver.Services
{
	public interface IRequestService
	{
		Task<IEnumerable<Request>> GetRequests(int organizationId);
		Task<IEnumerable<Request>> GetRequests();
		Task<Request> EditRequest(Request request);
	}

	public class RequestService : IRequestService
	{
		public Task<IEnumerable<Request>> GetRequests(int organizationId)
		{
			throw new System.NotImplementedException();
		}

		public Task<IEnumerable<Request>> GetRequests()
		{
			throw new System.NotImplementedException();
		}

		public Task<Request> EditRequest(Request request)
		{
			throw new System.NotImplementedException();
		}
	}
}
