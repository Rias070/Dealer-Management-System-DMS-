using System;
using System.Collections.Generic;
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
    public class VehiclesController : ControllerBase
    {
        private readonly VehicleService _vehicleService;

        public VehiclesController(VehicleService vehicleService)
        {
            _vehicleService = vehicleService ?? throw new ArgumentNullException(nameof(vehicleService));
        }

        // GET: api/vehicles
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var vehicles = await _vehicleService.GetAllAsync();
            return Ok(vehicles);
        }

        // GET: api/vehicles/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest();

            try
            {
                var result = await _vehicleService.GetByIdAsync(id);
                if (result == null || result.Vehicle == null)
                    return NotFound();

                return Ok(result.Vehicle);
            }
            catch (ApiException.NotFoundException)
            {
                return NotFound();
            }
        }

        // POST: api/vehicles
        [HttpPost]
        public async Task<ActionResult<VehicleDto>> Create([FromBody] VehicleCreateUpdateDto request)
        {
            if (request == null)
                return BadRequest();

            var svcRequest = new VehicleRequestDto
            {
                // Id left unset for create; service should generate new Id
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

            // return created resource
            return CreatedAtAction(nameof(GetById), new { id = result.VehicleId }, result.Vehicle);
        }

        // PUT: api/vehicles/{id}
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

        // DELETE: api/vehicles/{id}
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