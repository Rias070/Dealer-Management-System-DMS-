using CompanyDealer.BLL.DTOs.InventoryDTOs;
using CompanyDealer.BLL.ExceptionHandle;
using CompanyDealer.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CompanyDealer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryService _service;
        private readonly AuthService _authService;

        public InventoryController(InventoryService service, AuthService authService)
        {
            _service = service;
            _authService = authService;
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

        [HttpGet("dealer/vehicles")]
        public async Task<IActionResult> GetVehiclesByDealerId()
        {
            var userId = GetUserIdFromToken();
            var userDealerId = await _authService.GetDealerIdByUserIdAsync(userId);
            var vehicles = await _service.GetVehicleInInventory(userDealerId.Value);
            return Ok(vehicles);
        }
        private Guid GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")?.Value
                ?? User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                throw new ApiException.UnauthorizedException("Invalid user token");
            return userId;
        }
    }
}