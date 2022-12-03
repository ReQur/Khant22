using System.Collections.Generic;
using System.Threading.Tasks;
using dotnetserver.Models;
using dotnetserver.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnetserver.Controllers
{
	[ApiController]
	// [Authorize]
	[Route("api/[controller]")]
	public class VehicleController : ControllerBase
	{
		private readonly IVehicleService _vehicleService;

		public VehicleController(IVehicleService vehicleService)
		{
			_vehicleService = vehicleService;
		}

		[HttpPost(nameof(GetVehicles))]
		public async Task<IEnumerable<Vehicle>> GetVehicles([FromBody] Organization organization)
		{
			return await _vehicleService.GetVehicles(organization.name);
		}

		[HttpGet(nameof(GetVehicle) + "/vehicleId")]
		public async Task<Vehicle> GetVehicle(int vehicleId)
		{
			return await _vehicleService.GetVehicle(vehicleId);
		}

	}
}
