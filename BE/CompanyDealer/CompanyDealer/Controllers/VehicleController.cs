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

        // POST: api/vehicle
        [HttpPost]
        public async Task<ActionResult<Vehicle>> Create([FromBody] Vehicle vehicle)
        {
            if (vehicle == null)
            {
                return BadRequest();
            }

            vehicle.Id = Guid.NewGuid();
            var created = await _vehicleRepository.CreateAsync(vehicle);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // GET: api/vehicle/{id}
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

        // PUT: api/vehicle/{id}
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Vehicle>> Update([FromRoute] Guid id, [FromBody] Vehicle vehicle)
        {
            if (vehicle == null || id == Guid.Empty)
            {
                return BadRequest();
            }

            vehicle.Id = id;
            var updated = await _vehicleRepository.UpdateAsync(vehicle);
            if (updated == null)
            {
                return NotFound();
            }

            return Ok(updated);
        }

        // DELETE: api/vehicle/{id}
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


