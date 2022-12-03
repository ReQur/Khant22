using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetserver.Models;
using Microsoft.AspNetCore.Mvc;
using Dapper;

namespace dotnetserver.Services
{
	public interface IVehicleService
	{
		Task<IEnumerable<Vehicle>> GetVehicles(int organizationId);
		Task<Vehicle> GetVehicle(int vehicleId);
		Task<Vehicle> RemoveVehicle(int vehicleId);
	}

	public class VehicleService : WithDbAccess, IVehicleService
	{
		private readonly ConnectionContext _context;

		public VehicleService(ConnectionContext context) : base(context) 
		{
			_context = context;
		}

		public async Task<IEnumerable<Vehicle>> GetVehicles(int organizationId)
		{
			var parameters = new { OrganizationId = organizationId };
			var query = @"select v.*, vT.*, sT.*, vTE.*, o.*
						from vehicle v
						left join vehicleType vT on v.vehicleTypeId = vT.vehicleTypeId
						left join serviceType sT on v.serviceTypeId = sT.serviceTypeId
						left join organization o on v.organizationId = o.organizationId
						left join vehicleTypeExt vTE on v.vehicleTypeExtId = vTE.vehicleTypeExtId where v.organizationId = @OrganizationId;";
			using (var db = _context.GenericConnection())
			{
				db.Open();
				try
				{
					var vehicles = await db.QueryAsync<Vehicle, vehicleType, serviceType, vehicleTypeExt, Organization, Vehicle>(
							query,
							(vehicle, veh_type, ser_type, veh_type_ext, org) =>
							{
								vehicle.vehicleType = veh_type;
								vehicle.serviceType = ser_type;
								vehicle.vehicleTypeExt = veh_type_ext;
								vehicle.organization = org;
								return vehicle;
							}, parameters, splitOn: "vehicleTypeId, serviceTypeId, vehicleTypeExtId, organizationId"
						);
					return vehicles;
				}
				catch (Exception e)
				{
					throw;
				}
			}
		}

		public async Task<Vehicle> GetVehicle(int vehicleId)
		{
			var parameters = new { VehicleId = vehicleId };
			var query = @"select v.*, vT.*, sT.*, vTE.*, o.*
						from vehicle v
						left join vehicleType vT on v.vehicleTypeId = vT.vehicleTypeId
						left join serviceType sT on v.serviceTypeId = sT.serviceTypeId
						left join organization o on v.organizationId = o.organizationId
						left join vehicleTypeExt vTE on v.vehicleTypeExtId = vTE.vehicleTypeExtId where v.vehicleId = @VehicleId;";
			using (var db = _context.GenericConnection())
            {
				db.Open();
				try
				{
					var vehicle = await db.QueryAsync<Vehicle, vehicleType, serviceType, vehicleTypeExt, Organization, Vehicle>(
						query,
						(vehicle, veh_type, ser_type, veh_type_ext, org) =>
                        {
							vehicle.vehicleType = veh_type;
							vehicle.serviceType = ser_type;
							vehicle.vehicleTypeExt= veh_type_ext;
							vehicle.organization = org;
							return vehicle;
						}, parameters, splitOn: "vehicleTypeId, serviceTypeId, vehicleTypeExtId, organizationId"
					);
					return vehicle.FirstOrDefault();
				}
				catch (Exception e)
				{
					throw;
				}
			}
		}

		public async Task<Vehicle> RemoveVehicle(int vehicleId)
		{
			var parameters = new { VehicleId = vehicleId };
			var query = "delete from vehicle where vehicleId=@VehicleId";

			try
			{
				var vehicle = await DbQueryAsync<Vehicle>(query, parameters);
				return vehicle.FirstOrDefault();
			}
			catch (Exception e)
			{
				throw;
			}
		}
	}
}
