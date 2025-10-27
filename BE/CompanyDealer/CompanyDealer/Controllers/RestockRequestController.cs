using CompanyDealer.BLL.DTOs.RestockRequestDTOs;
using CompanyDealer.BLL.Services;
using CompanyDealer.BLL.ExceptionHandle;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Security.Claims;

namespace CompanyDealer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RestockRequestController : ControllerBase
    {
        private readonly RestockRequestService _service;
        private readonly AuthService _authService;

        public RestockRequestController(RestockRequestService service, AuthService authService)
        {
            _service = service;
            _authService = authService;
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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FillRestockDto requestDto)
        {
            var userId = GetUserIdFromToken();
            var dealerId = await _authService.GetDealerIdByUserIdAsync(userId);
            var dto = new CreateRestockRequestDto
            {
                AccountId = userId,
                DealerId = dealerId.Value,
                VehicleId = requestDto.VehicleId,
                Quantity = requestDto.Quantity,
                Description = requestDto.Description
            };
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
            var userId = GetUserIdFromToken();
            var success = await _service.AcceptAndEscalateAsync(id, userId);
            if (!success)
                return BadRequest(ApiResponse<object>.FailResponse("BAD_REQUEST", "Cannot escalate this request."));
            return Ok(ApiResponse<object>.SuccessResponse(null, "Request escalated to company."));
        }

        // DealerManager: View all restock requests for their dealer (dealerId from user)
        [Authorize(Roles = "DealerManager")]
        [HttpGet("dealer/requests")]
        public async Task<IActionResult> GetRequestsByDealerManager()
        {
            var userId = GetUserIdFromToken();
            var dealerId = await _authService.GetDealerIdByUserIdAsync(userId);
            if (dealerId == null)
                return BadRequest(ApiResponse<object>.FailResponse("BAD_REQUEST", "Dealer not found for this user."));
            var requests = await _service.GetRequestsByDealerManager(dealerId.Value);
            return Ok(ApiResponse<object>.SuccessResponse(requests, "Fetched dealer restock requests"));
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

        //CompanyStaff: View all restock requests for company staff
        [Authorize(Roles = "CompanyStaff")]
        [HttpGet("/companystaff")]
        public async Task<IActionResult> GetRequestsForCompanyStaff()
        {
            var requests = await _service.GetRestockRequestForCompany();
            return Ok(ApiResponse<object>.SuccessResponse(requests, "Fetched restock requests for company staff"));
        }
    }
}
