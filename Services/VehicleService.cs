using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetserver.Models;
using Microsoft.AspNetCore.Mvc;

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
		public VehicleService(ConnectionContext context) : base(context) { }

		public async Task<IEnumerable<Vehicle>> GetVehicles(int organizationId)
		{
			var parameters = new { OrganizationId = organizationId };
			var query = "select * from vehicle where organizationId=@OrganizationId";

			try
			{
				var vehicles = await DbQueryAsync<Vehicle>(query, parameters);
				return vehicles;
			}
			catch (Exception e)
			{
				throw;
			}
		}

		public async Task<Vehicle> GetVehicle(int vehicleId)
		{
			var parameters = new { VehicleId = vehicleId };
			var query = "select * from vehicle where vehicleId=@VehicleId";

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
