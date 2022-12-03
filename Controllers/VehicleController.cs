using System.Collections.Generic;
using System.Threading.Tasks;
using dotnetserver.Models;
using dotnetserver.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnetserver.Controllers
{
	[ApiController]
	[Authorize]
	[Route("api/[controller]")]
	public class VehicleController : ControllerBase
	{
		private readonly IVehicleService _vehicleService;

		public VehicleController(IVehicleService vehicleService)
		{
			_vehicleService = vehicleService;
		}

		[HttpGet(nameof(GetVehicles) + "/organizationId")]
		public async Task<IEnumerable<Vehicle>> GetVehicles(int organizationId)
		{
			return await _vehicleService.GetVehicles(organizationId);
		}

		[HttpGet(nameof(GetVehicle) + "/vehicleId")]
		public async Task<Vehicle> GetVehicle(int vehicleId)
		{
			return await _vehicleService.GetVehicle(vehicleId);
		}

		[HttpGet(nameof(RemoveVehicle) + "/vehicleId")]
		public async Task<ActionResult<Vehicle>> RemoveVehicle(int vehicleId)
		{
			return await _vehicleService.RemoveVehicle(vehicleId);
		}
	}
}
