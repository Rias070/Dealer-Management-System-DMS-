using CompanyDealer.BLL.DTOs.AuthDTOs;
using CompanyDealer.BLL.ExceptionHandle;
using CompanyDealer.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly JwtService _jwtService;

    public AuthController(AuthService authService, JwtService jwtService)
    {
        _authService = authService;
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var response = await _authService.LoginAsync(request);
        if (!response.Success)
            return Unauthorized(ApiResponse<object>.FailResponse("UNAUTHORIZED", response.Message));

        return Ok(ApiResponse<LoginResponseDto>.SuccessResponse(response, "Login successfully"));
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        var response = await _authService.RegisterAsync(request);
        if (!response.Success)
            return BadRequest(ApiResponse<object>.FailResponse("BAD_REQUEST", response.Message));

        return Ok(ApiResponse<RegisterResponseDto>.SuccessResponse(response, "Register successfully"));
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        string refreshToken = Request.Headers["X-Refresh-Token"];
        if (string.IsNullOrEmpty(refreshToken))
        {
            return Unauthorized(ApiResponse<object>.FailResponse("UNAUTHORIZED", "Refresh token is required"));
        }

        string authHeader = Request.Headers["Authorization"];
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            return Unauthorized(ApiResponse<object>.FailResponse("UNAUTHORIZED", "Access token is required"));
        }

        string accessToken = authHeader.Substring("Bearer ".Length).Trim();

        var request = new RefreshTokenRequestDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };

        var response = await _authService.RefreshTokenAsync(request);
        if (!response.Success)
            return Unauthorized(ApiResponse<object>.FailResponse("UNAUTHORIZED", response.Message));

        return Ok(ApiResponse<object>.SuccessResponse(response, "Refresh token successfully"));
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        string authHeader = Request.Headers["Authorization"];
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            return Unauthorized(ApiResponse<object>.FailResponse("UNAUTHORIZED", "Invalid token"));
        }

        string accessToken = authHeader.Substring("Bearer ".Length).Trim();

        var principal = _jwtService.GetPrincipalFromExpiredToken(accessToken);
        var userId = System.Guid.Parse(principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
        var response = await _authService.LogoutAsync(userId);

        if (!response.Success)
            return BadRequest(ApiResponse<object>.FailResponse("BAD_REQUEST", response.Message));

        return Ok(ApiResponse<object>.SuccessResponse(response, "Logout successfully"));
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto request)
    {
        string authHeader = Request.Headers["Authorization"];
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            return Unauthorized(ApiResponse<object>.FailResponse("UNAUTHORIZED", "Invalid token"));

        string accessToken = authHeader.Substring("Bearer ".Length).Trim();
        var principal = _jwtService.GetPrincipalFromExpiredToken(accessToken);
        var userId = Guid.Parse(principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

        await _authService.ChangePasswordAsync(userId, request);
        return Ok(ApiResponse<object>.SuccessResponse(null, "Password changed successfully"));
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto request)
    {
        await _authService.ForgotPasswordAsync(request);
        return Ok(ApiResponse<object>.SuccessResponse(null, "Password reset successfully"));
    }
}