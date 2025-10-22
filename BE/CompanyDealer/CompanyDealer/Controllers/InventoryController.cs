using System;
using System.Threading.Tasks;
using CompanyDealer.BLL.DTOs.InventoryDTOs;
using CompanyDealer.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace CompanyDealer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryService _service;

        public InventoryController(InventoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAsync();
            return Ok(list);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var res = await _service.GetByIdAsync(id);
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] InventoryRequestDto dto)
        {
            var res = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = res.InventoryId }, res);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] InventoryRequestDto dto)
        {
            var res = await _service.UpdateAsync(dto);
            return Ok(res);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var res = await _service.DeleteAsync(id);
            return Ok(res);
        }

        [HttpGet("dealer/{dealerId:guid}/vehicles")]
        public async Task<IActionResult> GetVehiclesByDealerId(Guid dealerId)
        {
            var vehicles = await _service.GetVehicleInInventory(dealerId);
            return Ok(vehicles);
        }
    }
}