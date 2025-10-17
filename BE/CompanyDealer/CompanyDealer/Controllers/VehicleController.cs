using System;
using System.Threading.Tasks;
using CompanyDealer.DAL.Models;
using CompanyDealer.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CompanyDealer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly VehicleRepository _vehicleRepository;

        public VehicleController(VehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        // GET: api/vehicle //View all vehicles
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var vehicles = await _vehicleRepository.GetAllAsync();
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
        public async Task<ActionResult<Vehicle>> Create([FromBody] VehicleCreateUpdateDto request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            var vehicle = new Vehicle
            {
                Id = Guid.NewGuid(),
                Make = request.Make,
                Model = request.Model,
                Year = request.Year,
                VIN = request.VIN,
                Color = request.Color,
                Price = request.Price,
                Description = request.Description,
                IsAvailable = request.IsAvailable,
                InventoryId = request.InventoryId,
                CategoryId = request.CategoryId
            };

            var created = await _vehicleRepository.CreateAsync(vehicle);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // GET: api/vehicle/{id} //View a vehicle by its ID
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Vehicle>> GetById([FromRoute] Guid id)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return Ok(vehicle);
        }

        // PUT: api/vehicle/{id} //Update a vehicle by its ID
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Vehicle>> Update([FromRoute] Guid id, [FromBody] VehicleCreateUpdateDto request)
        {
            if (request == null || id == Guid.Empty)
            {
                return BadRequest();
            }

            var vehicle = new Vehicle
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
                InventoryId = request.InventoryId,
                CategoryId = request.CategoryId
            };

            var updated = await _vehicleRepository.UpdateAsync(vehicle);
            if (updated == null)
            {
                return NotFound();
            }

            return Ok(updated);
        }

        // DELETE: api/vehicle/{id} //Delete a vehicle by its ID
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var deleted = await _vehicleRepository.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}


