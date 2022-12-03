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
		Task<Request> AddRequest(Request request);
		Task<Request> EditRequest(Request request);
	}

	public class RequestService : WithDbAccess, IRequestService
	{
		public RequestService(ConnectionContext context) : base(context) { }

		public async Task<IEnumerable<Request>> GetRequests(int organizationId)
		{
			var parameters = new { OrganizationId = organizationId };
			var query = @"select r.*, u.*, o.*, v.*
							from request r
							left join user u on r.userId = u.userId
							left join organization o on r.organizationId = o.organizationId
							left join vehicle v on r.vehicleId = v.vehicleId
							where r.organizationId = @OrganizationId;";
			using (var db = _context.GenericConnection())
			{
				db.Open();
				try
				{
					var requests = await db.QueryAsync<Request, User, Organization, Vehicle, Request>(
						query,
						(req, user, org, veh) =>
						{
							req.user = user;
							req.organization = org;
							req.vehicle = veh;
							return req;
						}, parameters, splitOn: "userId, organizationId, vehicleId"
					);
					return requests;
				}
				catch (Exception e)
				{
					throw;
				}
			}
		}

		public async Task<IEnumerable<Request>> GetRequests()
		{
			var query = @"select r.*, u.*, o.*, v.*
							from request r
							left join user u on r.userId = u.userId
							left join organization o on r.organizationId = o.organizationId
							left join vehicle v on r.vehicleId = v.vehicleId;";
			using (var db = _context.GenericConnection())
			{
				db.Open();
				try
				{
					var requests = await db.QueryAsync<Request, User, Organization, Vehicle, Request>(
						query,
						(req, user, org, veh) =>
						{
							req.user = user;
							req.organization = org;
							req.vehicle = veh;
							return req;
						}, splitOn: "userId, organizationId, vehicleId"
					);
					return requests;
				}
				catch (Exception e)
				{
					throw;
				}
			}
		}

		public async Task<Request> AddRequest(Request request)
		{
			var query = "insert into request (status, userId, vehicleId, organizationId, comment) " +
			            "values (@Status, @UserId, @VehicleId, @OrganizationId, @comment)";
			var parameters = new { Status = request.status, UserId = request.user.userId, 
				OrganizationId = request.organization.organizationId, Comment = request.comment};

			using (var db = _context.GenericConnection())
			{
				db.Open();
				try
				{
					var result = await db.QueryAsync(query, parameters);
					return request;
				}
				catch (Exception e)
				{
					throw;
				}
			}
		}

		public async Task<Request> EditRequest(Request request)
		{
			var query = "update request set (status = @Status, userId = @UserId, vehicleId = @VehicleId, organizationId = @OrganizationId, comment = @Comment) where request == @Id";
			var parameters = new {Id = request.requestId, Status = request.status, 
				UserId = request.user.userId, OrganizationId = request.organization.organizationId, Comment = request.comment };

			using (var db = _context.GenericConnection())
			{
				db.Open();
				try
				{
					var result = await db.QueryAsync(query, parameters);
					return request;
				}
				catch (Exception e)
				{
					throw;
				}
			}
		}
	}
}
