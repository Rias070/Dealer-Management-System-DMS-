using System;
using System.Threading.Tasks;
using CompanyDealer.BLL.DTOs;
using CompanyDealer.BLL.DTOs.VehicleDTOs;
using CompanyDealer.BLL.ExceptionHandle;
using CompanyDealer.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace CompanyDealer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly VehicleService _vehicleService;

        public VehicleController(VehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        // GET: api/vehicle //View all vehicles
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var vehicles = await _vehicleService.GetAllAsync();
            return Ok(vehicles);
        }

        public class VehicleCreateUpdateDto
        {
            public string Make { get; set; } = string.Empty;
            public string Model { get; set; } = string.Empty;
            public int Year { get; set; }
            public string VIN { get; set; } = string.Empty;
            public string Color { get; set; } = string.Empty;
            public decimal Price { get; set; }
            public string Description { get; set; } = string.Empty;
            public bool IsAvailable { get; set; } = true;
            public Guid InventoryId { get; set; }
            public Guid CategoryId { get; set; }
        }

        // POST: api/vehicle //Create a new vehicle
        [HttpPost]
        public async Task<ActionResult<VehicleDto>> Create([FromBody] VehicleCreateUpdateDto request)
        {
            if (request == null)
                return BadRequest();

            var svcRequest = new VehicleRequestDto
            {
                Id = null,
                Make = request.Make,
                Model = request.Model,
                Year = request.Year,
                VIN = request.VIN,
                Color = request.Color,
                Price = request.Price,
                Description = request.Description,
                IsAvailable = request.IsAvailable,
                CategoryId = request.CategoryId
            };

            var result = await _vehicleService.CreateAsync(svcRequest);
            if (result == null || !result.Success)
                return BadRequest(result?.Message);

            return CreatedAtAction(nameof(GetById), new { id = result.VehicleId }, result.Vehicle);
        }

        // GET: api/vehicle/{id} //View a vehicle by its ID
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<VehicleDto>> GetById([FromRoute] Guid id)
        {
            try
            {
                var result = await _vehicleService.GetByIdAsync(id);
                return Ok(result.Vehicle);
            }
            catch (ApiException.NotFoundException)
            {
                return NotFound();
            }
        }

        // PUT: api/vehicle/{id} //Update a vehicle by its ID
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<VehicleDto>> Update([FromRoute] Guid id, [FromBody] VehicleCreateUpdateDto request)
        {
            if (request == null || id == Guid.Empty)
                return BadRequest();

            var svcRequest = new VehicleRequestDto
            {
                Id = id,
                Make = request.Make,
                Model = request.Model,
                Year = request.Year,
                VIN = request.VIN,
                Color = request.Color,
                Price = request.Price,
                Description = request.Description,
                IsAvailable = request.IsAvailable,
                CategoryId = request.CategoryId
            };

            try
            {
                var result = await _vehicleService.UpdateAsync(svcRequest);
                if (result == null || !result.Success)
                    return NotFound();

                return Ok(result.Vehicle);
            }
            catch (ApiException.NotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE: api/vehicle/{id} //Delete a vehicle by its ID
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest();

            try
            {
                var result = await _vehicleService.DeleteAsync(id);
                if (result == null || !result.Success)
                    return NotFound();

                return NoContent();
            }
            catch (ApiException.NotFoundException)
            {
                return NotFound();
            }
        }
    }
}


