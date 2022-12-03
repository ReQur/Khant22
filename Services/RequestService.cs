using System;
using dotnetserver.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;

namespace dotnetserver.Services
{
	public interface IRequestService
	{
		Task<IEnumerable<Request>> GetRequests(int organizationId);
		Task<IEnumerable<Request>> GetRequests();
		Task<Request> EditRequest(Request request);
		Task<Request> AddRequest(Request request);
	}

	public class RequestService : WithDbAccess, IRequestService
	{
		public RequestService(ConnectionContext context) : base(context) { }

		public async Task<IEnumerable<Request>> GetRequests(int organizationId)
		{
			var parameters = new { OrganizationId = organizationId };
			var query = @"select r.*, u.*, o.*
							from request r
							left join user u on r.userId = u.userId
							left join organization o on r.organizationId = o.organizationId
							where r.organizationId = @OrganizationId;";
			using (var db = _context.GenericConnection())
			{
				db.Open();
				try
				{
					var vehicles = await db.QueryAsync<Request, User, Organization, Request>(
						query,
						(req, user, org) =>
						{
							req.user = user;
							req.organization = org;
							return req;
						}, parameters, splitOn: "userId, organizationId"
					);
					return vehicles;
				}
				catch (Exception e)
				{
					throw;
				}
			}
		}

		public Task<IEnumerable<Request>> GetRequests()
		{
			throw new System.NotImplementedException();
		}

		public Task<Request> EditRequest(Request request)
		{
			throw new System.NotImplementedException();
		}

		public Task<Request> AddRequest(Request request)
		{
			throw new System.NotImplementedException();
		}
	}
}
