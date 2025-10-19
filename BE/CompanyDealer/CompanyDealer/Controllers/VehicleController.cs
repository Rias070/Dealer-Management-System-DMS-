using System;
using System.Threading.Tasks;
using CompanyDealer.DAL.Models;
using CompanyDealer.DAL.Repository;
using CompanyDealer.DAL.Repository.VehicleRepo;
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
            var dto = vehicles.ConvertAll(v => new VehicleResponseDto
            {
                Id = v.Id,
                Make = v.Make,
                Model = v.Model,
                Year = v.Year,
                VIN = v.VIN,
                Color = v.Color,
                Price = v.Price,
                Description = v.Description,
                IsAvailable = v.IsAvailable,
                
                CategoryId = v.CategoryId
            });
            return Ok(dto);
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
        public class VehicleResponseDto
        {
            public Guid Id { get; set; }
            public string Make { get; set; } = string.Empty;
            public string Model { get; set; } = string.Empty;
            public int Year { get; set; }
            public string VIN { get; set; } = string.Empty;
            public string Color { get; set; } = string.Empty;
            public decimal Price { get; set; }
            public string Description { get; set; } = string.Empty;
            public bool IsAvailable { get; set; }
            public Guid CategoryId { get; set; }
        }

        // POST: api/vehicle //Create a new vehicle
        [HttpPost]
        public async Task<ActionResult<VehicleResponseDto>> Create([FromBody] VehicleCreateUpdateDto request)
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
                
                CategoryId = request.CategoryId
            };

            var created = await _vehicleRepository.CreateAsync(vehicle);
            var dto = new VehicleResponseDto
            {
                Id = created.Id,
                Make = created.Make,
                Model = created.Model,
                Year = created.Year,
                VIN = created.VIN,
                Color = created.Color,
                Price = created.Price,
                Description = created.Description,
                IsAvailable = created.IsAvailable,
                
                CategoryId = created.CategoryId
            };
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, dto);
        }

        // GET: api/vehicle/{id} //View a vehicle by its ID
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<VehicleResponseDto>> GetById([FromRoute] Guid id)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            var dto = new VehicleResponseDto
            {
                Id = vehicle.Id,
                Make = vehicle.Make,
                Model = vehicle.Model,
                Year = vehicle.Year,
                VIN = vehicle.VIN,
                Color = vehicle.Color,
                Price = vehicle.Price,
                Description = vehicle.Description,
                IsAvailable = vehicle.IsAvailable,
                
                CategoryId = vehicle.CategoryId
            };
            return Ok(dto);
        }

        // PUT: api/vehicle/{id} //Update a vehicle by its ID
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<VehicleResponseDto>> Update([FromRoute] Guid id, [FromBody] VehicleCreateUpdateDto request)
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
                
                CategoryId = request.CategoryId
            };

            var updated = await _vehicleRepository.UpdateAsync(vehicle);
            if (updated == null)
            {
                return NotFound();
            }

            var dto = new VehicleResponseDto
            {
                Id = updated.Id,
                Make = updated.Make,
                Model = updated.Model,
                Year = updated.Year,
                VIN = updated.VIN,
                Color = updated.Color,
                Price = updated.Price,
                Description = updated.Description,
                IsAvailable = updated.IsAvailable,
                
                CategoryId = updated.CategoryId
            };
            return Ok(dto);
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


