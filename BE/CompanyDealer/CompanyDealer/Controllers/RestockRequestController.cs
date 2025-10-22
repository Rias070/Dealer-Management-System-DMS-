using CompanyDealer.BLL.DTOs.RestockRequestDTOs;
using CompanyDealer.BLL.Services;
using CompanyDealer.BLL.ExceptionHandle;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CompanyDealer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RestockRequestController : ControllerBase
    {
        private readonly RestockRequestService _service;

        public RestockRequestController(RestockRequestService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAsync();
            return Ok(ApiResponse<object>.SuccessResponse(list, "Fetched all restock requests"));
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var res = await _service.GetByIdAsync(id);
            if (res == null)
                return NotFound(ApiResponse<object>.FailResponse("NOT_FOUND", "Restock request not found"));
            return Ok(ApiResponse<object>.SuccessResponse(res, "Fetched restock request"));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRestockRequestDto dto)
        {
            var res = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = res.Id }, ApiResponse<object>.SuccessResponse(res, "Created restock request"));
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] RestockRequestDto dto)
        {
            var success = await _service.UpdateAsync(id, dto);
            if (!success)
                return NotFound(ApiResponse<object>.FailResponse("NOT_FOUND", "Restock request not found"));
            return Ok(ApiResponse<object>.SuccessResponse(null, "Updated restock request"));
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success)
                return NotFound(ApiResponse<object>.FailResponse("NOT_FOUND", "Restock request not found"));
            return Ok(ApiResponse<object>.SuccessResponse(null, "Deleted restock request"));
        }

        // DealerManager: Accept and escalate to company
        [Authorize(Roles = "DealerManager")]
        [HttpPost("{id:guid}/accept")]
        public async Task<IActionResult> AcceptAndEscalate(Guid id)
        {
            var managerAccountId = Guid.Parse(User.FindFirst("sub")?.Value ?? User.FindFirst("id")?.Value ?? Guid.Empty.ToString());
            var success = await _service.AcceptAndEscalateAsync(id, managerAccountId);
            if (!success)
                return BadRequest(ApiResponse<object>.FailResponse("BAD_REQUEST", "Cannot escalate this request."));
            return Ok(ApiResponse<object>.SuccessResponse(null, "Request escalated to company."));
        }

        // DealerManager: View all restock requests for their dealer
        [Authorize(Roles = "DealerManager")]
        [HttpGet("dealer/{dealerId:guid}")]
        public async Task<IActionResult> GetRequestsByDealerManager(Guid dealerId)
        {
            var requests = await _service.GetRequestsByDealerManager(dealerId);
            return Ok(ApiResponse<object>.SuccessResponse(requests, "Fetched dealer restock requests"));
        }
    }
}
