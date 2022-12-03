using System;
using dotnetserver.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
		Task<string> GetQrCode(int requestId);
		Task<Request> DecodeQrCode(string hash);
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
			var query = "insert into request (status, userId, vehicleId, organizationId, comment) values (@Status, @UserId, @VehicleId, @OrganizationId, @comment)";
			var parameters = new { Status = request.status, UserId = request.user.userId, 
				VehicleId = request.vehicle.vehicleId, OrganizationId = request.organization.organizationId, Comment = request.comment};

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
			var query = "update request set status = @Status, userId = @UserId, vehicleId = @VehicleId, organizationId = @OrganizationId, comment = @Comment where requestId = @Id";
			var parameters = new {Id = request.requestId, Status = request.status, 
				UserId = request.user.userId, VehicleId = request.vehicle.vehicleId, OrganizationId = request.organization.organizationId, Comment = request.comment };

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

		public async Task<string> GetQrCode(int requestId)
		{
			var query = "select * from request where requestId = @Id";
			var parameters = new { Id = requestId };

			using (var db = _context.GenericConnection())
			{
				db.Open();
				try
				{
					var result = await db.QueryAsync<Request>(query, parameters);
					if (result.Count() != 0)
					{
						var stringToEncode = "request" + result.First().requestId.ToString();
						return stringToEncode;
					}
					else
					{
						return "";
					}
				}
				catch (Exception e)
				{
					throw;
				}
			}
		}

		public async Task<Request> DecodeQrCode(string encodedString)
		{
			var requestId = encodedString.Substring("request".Length);
			var query = @"select r.*, u.*, o.*, v.*
							from request r
							left join user u on r.userId = u.userId
							left join organization o on r.organizationId = o.organizationId
							left join vehicle v on r.vehicleId = v.vehicleId
							where requestId = @Id;";
			var parameters = new { Id = requestId };

			using (var db = _context.GenericConnection())
			{
				db.Open();
				try
				{
					var result = await db.QueryAsync<Request, User, Organization, Vehicle, Request>(
						query,
						(req, user, org, veh) =>
						{
							req.user = user;
							req.organization = org;
							req.vehicle = veh;
							return req;
						},parameters, splitOn: "userId, organizationId, vehicleId"
					);
					if (result.Count() != 0)
					{
						return result.First();
					}
					else
					{
						return null;
					}
				}
				catch (Exception e)
				{
					throw;
				}
			}
		}
	}
}
