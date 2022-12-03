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
		Task<IEnumerable<Vehicle>> GetVehicles(string organizationName);
		Task<Vehicle> GetVehicle(int vehicleId);
	}

	public class VehicleService : WithDbAccess, IVehicleService
	{
		public VehicleService(ConnectionContext context) : base(context) 
		{
		}
		public async Task<IEnumerable<Vehicle>> GetVehicles(string organizationName)
		{
			var parameters = new { OrganizationName = organizationName };
			var query = @"select * from vehicle where organization = @OrganizationName;";
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
			var query = @"select * from vehicle where vehicleId = @VehicleId;";
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
