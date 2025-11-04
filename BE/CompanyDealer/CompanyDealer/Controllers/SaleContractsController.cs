using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CompanyDealer.BLL.DTOs.SaleContractDTOs;
using CompanyDealer.BLL.Services;

namespace CompanyDealer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SaleContractController : ControllerBase
    {
        private readonly ISaleContractService _saleContractService;

        public SaleContractController(ISaleContractService saleContractService)
        {
            _saleContractService = saleContractService;
        }

        /// <summary>
        /// Get all sale contracts with optional filters.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllAsync(
            [FromQuery] Guid? orderId,
            [FromQuery] string? contractNumber,
            [FromQuery] DateTime? signDate,
            [FromQuery] string? terms,
            [FromQuery] string? customerSignature,
            [FromQuery] string? dealerSignature,
            [FromQuery] bool? isActive)
        {
            var result = await _saleContractService.GetAllAsync(orderId, contractNumber, signDate, terms, customerSignature, dealerSignature, isActive);
            return Ok(result);
        }

        /// <summary>
        /// Get a sale contract by its unique ID.
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var result = await _saleContractService.GetByIdAsync(id);
            if (result == null)
                return NotFound(new { Message = $"Sale contract with ID {id} not found." });

            return Ok(result);
        }

        /// <summary>
        /// Get a sale contract by its contract number.
        /// </summary>
        [HttpGet("by-number/{contractNumber}")]
        public async Task<IActionResult> GetByContractNumberAsync(string contractNumber)
        {
            var result = await _saleContractService.GetByContractNumberAsync(contractNumber);
            if (result == null)
                return NotFound(new { Message = $"Contract number '{contractNumber}' not found." });

            return Ok(result);
        }

        /// <summary>
        /// Create a new sale contract.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateSaleContractRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var created = await _saleContractService.CreateAsync(request);
                return CreatedAtAction("GetById", new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { Message = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing sale contract.
        /// </summary>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateSaleContractRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _saleContractService.UpdateAsync(id, request);
            if (updated == null)
                return NotFound(new { Message = $"Sale contract with ID {id} not found." });

            return Ok(updated);
        }

        /// <summary>
        /// Delete a sale contract (hard delete by default).
        /// </summary>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var deleted = await _saleContractService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { Message = $"Sale contract with ID {id} not found." });

            return NoContent();
        }
    }
}
