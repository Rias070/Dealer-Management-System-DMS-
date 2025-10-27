using System;
using System.Threading.Tasks;
using CompanyDealer.BLL.DTOs.DealerDTOs;
using CompanyDealer.BLL.ExceptionHandle;
using CompanyDealer.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyDealer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DealerController : ControllerBase
    {
        private readonly DealerService _dealerService;

        public DealerController(DealerService dealerService)
        {
            _dealerService = dealerService ?? throw new ArgumentNullException(nameof(dealerService));
        }

        // GET: api/dealer
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var dealers = await _dealerService.GetAllAsync();
                return Ok(ApiResponse<object>.SuccessResponse(dealers, "Fetched all dealers"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.FailResponse("INTERNAL_ERROR", ex.Message));
            }
        }

        // GET: api/dealer/active
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveDealers()
        {
            try
            {
                var dealers = await _dealerService.GetActiveDealersAsync();
                return Ok(ApiResponse<object>.SuccessResponse(dealers, "Fetched active dealers"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.FailResponse("INTERNAL_ERROR", ex.Message));
            }
        }

        // GET: api/dealer/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(ApiResponse<object>.FailResponse("BAD_REQUEST", "Invalid dealer ID"));

            try
            {
                var result = await _dealerService.GetByIdAsync(id);
                return Ok(ApiResponse<object>.SuccessResponse(result.Dealer, result.Message));
            }
            catch (ApiException.NotFoundException)
            {
                return NotFound(ApiResponse<object>.FailResponse("NOT_FOUND", "Dealer not found"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.FailResponse("INTERNAL_ERROR", ex.Message));
            }
        }

        // POST: api/dealer
        [HttpPost]
        [Authorize(Policy = "RequireDealerAdminRole")]
        public async Task<IActionResult> Create([FromBody] DealerRequestDto request)
        {
            if (request == null)
                return BadRequest(ApiResponse<object>.FailResponse("BAD_REQUEST", "Request body is required"));

            try
            {
                var result = await _dealerService.CreateAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = result.DealerId }, 
                    ApiResponse<object>.SuccessResponse(result.Dealer, result.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.FailResponse("INTERNAL_ERROR", ex.Message));
            }
        }

        // PUT: api/dealer/{id}
        [HttpPut("{id:guid}")]
        [Authorize(Policy = "RequireDealerAdminRole")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] DealerRequestDto request)
        {
            if (id == Guid.Empty)
                return BadRequest(ApiResponse<object>.FailResponse("BAD_REQUEST", "Invalid dealer ID"));

            if (request == null)
                return BadRequest(ApiResponse<object>.FailResponse("BAD_REQUEST", "Request body is required"));

            try
            {
                var result = await _dealerService.UpdateAsync(id, request);
                return Ok(ApiResponse<object>.SuccessResponse(result.Dealer, result.Message));
            }
            catch (ApiException.NotFoundException)
            {
                return NotFound(ApiResponse<object>.FailResponse("NOT_FOUND", "Dealer not found"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.FailResponse("INTERNAL_ERROR", ex.Message));
            }
        }

        // DELETE: api/dealer/{id}
        [HttpDelete("{id:guid}")]
        [Authorize(Policy = "RequireDealerAdminRole")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(ApiResponse<object>.FailResponse("BAD_REQUEST", "Invalid dealer ID"));

            try
            {
                var result = await _dealerService.DeleteAsync(id);
                if (result)
                    return Ok(ApiResponse<object>.SuccessResponse(null, "Dealer deleted successfully"));
                else
                    return StatusCode(500, ApiResponse<object>.FailResponse("INTERNAL_ERROR", "Failed to delete dealer"));
            }
            catch (ApiException.NotFoundException)
            {
                return NotFound(ApiResponse<object>.FailResponse("NOT_FOUND", "Dealer not found"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.FailResponse("INTERNAL_ERROR", ex.Message));
            }
        }
    }
}
