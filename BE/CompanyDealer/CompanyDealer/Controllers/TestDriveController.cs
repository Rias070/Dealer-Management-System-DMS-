using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CompanyDealer.BLL.DTOs.TestDriveDTOs;
using CompanyDealer.BLL.ExceptionHandle;
using CompanyDealer.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyDealer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TestDriveController : ControllerBase
    {
        private readonly ITestDriveService _testDriveService;
        private readonly AuthService _authService;

        public TestDriveController(ITestDriveService testDriveService, AuthService authService)
        {
            _testDriveService = testDriveService;
            _authService = authService;
        }

        /// <summary>
        /// Get all test drives with optional filters
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] Guid? dealerId = null,
            [FromQuery] Guid? vehicleId = null,
            [FromQuery] string? status = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            try
            {
                var testDrives = await _testDriveService.GetAllAsync(dealerId, vehicleId, status, fromDate, toDate);
                return Ok(ApiResponse<IEnumerable<TestDriveResponse>>.SuccessResponse(testDrives, "Test drives retrieved successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
        }

        [Authorize]
        [HttpGet("my")]
        public async Task<IActionResult> GetMyDealerTestDrives()
        {
            try
            {
                var userId = GetUserIdFromToken();
                var dealerId = await _authService.GetDealerIdByUserIdAsync(userId);
                var testDrives = await _testDriveService.GetTestDriveByDealerIdAsync(dealerId.Value);
                return Ok(ApiResponse<IEnumerable<TestDriveResponse>>.SuccessResponse(testDrives, "User's test drives retrieved successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
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

        /// <summary>
        /// Get test drive by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var testDrive = await _testDriveService.GetByIdAsync(id);
                return Ok(ApiResponse<TestDriveResponse>.SuccessResponse(testDrive, "Test drive retrieved successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Create new test drive (DealerStaff, DealerAdmin)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "DealerStaff,DealerAdmin")]
        public async Task<IActionResult> Create([FromBody] CreateTestDriveRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<object>.ErrorResponse("Invalid request data", ModelState));

                var userId = GetCurrentUserId();
                var userName = GetCurrentUserName();

                var testDrive = await _testDriveService.CreateAsync(request, userId, userName);
                return CreatedAtAction(
                    nameof(GetById), 
                    new { id = testDrive.Id }, 
                    ApiResponse<TestDriveResponse>.SuccessResponse(testDrive, "Test drive created successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse($"An error occurred: {ex.Message}"));
            }
        }

        /// <summary>
        /// Update test drive (DealerStaff can update Pending/Rejected, DealerAdmin can update all)
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "DealerStaff,DealerAdmin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTestDriveRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<object>.ErrorResponse("Invalid request data", ModelState));

                var testDrive = await _testDriveService.UpdateAsync(id, request);
                return Ok(ApiResponse<TestDriveResponse>.SuccessResponse(testDrive, "Test drive updated successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse($"An error occurred: {ex.Message}"));
            }
        }

        /// <summary>
        /// Delete test drive (DealerStaff can delete Pending, DealerAdmin can delete all)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "DealerStaff,DealerAdmin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var result = await _testDriveService.DeleteAsync(id);
                return Ok(ApiResponse<bool>.SuccessResponse(result, "Test drive deleted successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse($"An error occurred: {ex.Message}"));
            }
        }

        /// <summary>
        /// Approve test drive (DealerAdmin only)
        /// </summary>
        [HttpPost("{id}/approve")]
        [Authorize(Roles = "DealerAdmin,DealerManager")]
        public async Task<IActionResult> Approve(Guid id, [FromBody] ApproveTestDriveRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<object>.ErrorResponse("Invalid request data", ModelState));

                var userName = GetCurrentUserName();
                var testDrive = await _testDriveService.ApproveAsync(id, request, userName);
                return Ok(ApiResponse<TestDriveResponse>.SuccessResponse(testDrive, "Test drive approved successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse($"An error occurred: {ex.Message}"));
            }
        }

        /// <summary>
        /// Reject test drive (DealerAdmin only)
        /// </summary>
        [HttpPost("{id}/reject")]
        [Authorize(Roles = "DealerAdmin, DealerManager")]
        public async Task<IActionResult> Reject(Guid id, [FromBody] RejectTestDriveRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<object>.ErrorResponse("Invalid request data", ModelState));

                var userName = GetCurrentUserName();
                var testDrive = await _testDriveService.RejectAsync(id, request, userName);
                return Ok(ApiResponse<TestDriveResponse>.SuccessResponse(testDrive, "Test drive rejected successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse($"An error occurred: {ex.Message}"));
            }
        }

        /// <summary>
        /// Get test drives by dealer
        /// </summary>
        [HttpGet("dealer/{dealerId}")]
        public async Task<IActionResult> GetByDealer(Guid dealerId)
        {
            try
            {
                var testDrives = await _testDriveService.GetAllAsync(dealerId: dealerId);
                return Ok(ApiResponse<IEnumerable<TestDriveResponse>>.SuccessResponse(testDrives, "Test drives retrieved successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Get test drives by vehicle
        /// </summary>
        [HttpGet("vehicle/{vehicleId}")]
        public async Task<IActionResult> GetByVehicle(Guid vehicleId)
        {
            try
            {
                var testDrives = await _testDriveService.GetAllAsync(vehicleId: vehicleId);
                return Ok(ApiResponse<IEnumerable<TestDriveResponse>>.SuccessResponse(testDrives, "Test drives retrieved successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
        }

        #region Helper Methods

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedAccessException("User ID not found in token");
            }
            return userId;
        }

        private string GetCurrentUserName()
        {
            var name = User.FindFirst(ClaimTypes.Name)?.Value;
            return name ?? "Unknown User";
        }

        #endregion
    }
}
