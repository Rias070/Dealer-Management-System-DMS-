using Microsoft.AspNetCore.Mvc;
using CompanyDealer.BLL.DTOs.SaleContractDTOs;
using CompanyDealer.BLL.Services;

namespace CompanyDealer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SaleContractController : ControllerBase
    {
        private readonly ISaleContractService _service;

        public SaleContractController(ISaleContractService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSaleContractRequest dto)
        {
            try
            {
                var created = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound(new { message = "Contract not found" });

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var contracts = await _service.GetAllAsync();
            return Ok(contracts);
        }

        [HttpPatch("{id}/deactivate")]
        public async Task<IActionResult> Deactivate(Guid id)
        {
            var success = await _service.DeactivateAsync(id);
            if (!success)
                return NotFound(new { message = "Contract not found" });

            return Ok(new { message = "Contract deactivated successfully" });
        }

        [HttpPatch("{id}/approve")]
        public async Task<IActionResult> Approve(Guid id, [FromBody] string dealerSignature)
        {
            try
            {
                var result = await _service.ApproveAsync(id, dealerSignature);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        public class RejectRequest
        {
            public string Reason { get; set; } = string.Empty;
        }


        [HttpPatch("{id}/reject")]
        public async Task<IActionResult> Reject(Guid id, [FromBody] RejectRequest request)
        {
            try
            {
                var result = await _service.RejectAsync(id, request.Reason);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


    }
}
