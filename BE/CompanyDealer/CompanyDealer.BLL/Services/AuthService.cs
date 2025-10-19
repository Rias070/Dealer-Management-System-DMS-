using CompanyDealer.DAL.Repository;
using CompanyDealer.DAL.Repository.RoleRepo;
using CompanyDealer.DAL.Repository.UserRepo;
using CompanyDealer.BLL.DTOs.AuthDTOs;
using CompanyDealer.BLL.Services;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BCrypt.Net;
using CompanyDealer.DAL.Models;
using CompanyDealer.BLL.ExceptionHandle;

namespace CompanyDealer.BLL.Services
{
    public class AuthService
    {
        private readonly IAccountRepository _userRepository;
        private readonly JwtService _jwtService;
        private readonly IRoleRepository _roleRepository;

        public AuthService(IAccountRepository userRepository, JwtService jwtService, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _roleRepository = roleRepository;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _userRepository.GetByUserNameWithRolesAsync(request.UserName);
            if (user == null)
            {
                throw new ApiException.BadRequestException("Invalid username or password");
            }

            bool passwordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
            if (!passwordValid)
            {
                throw new ApiException.BadRequestException("Invalid username or password");
            }

            var (accessToken, refreshToken) = await _jwtService.SaveTokensAsync(user.Id);

            var roles = user.Roles?.Select(r => r.RoleName).ToList() ?? new List<string>();

            return new LoginResponseDto
            {
                Success = true,
                Message = "Login successful",
                UserId = user.Id,
                Token = accessToken,
                RefreshToken = refreshToken,
                Roles = roles
            };
        }

        public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            using (var transaction = await _userRepository.BeginTransactionAsync())
            {
                var existingUser = await _userRepository.FindAsync(u => u.Username == request.Username);
                if (existingUser.Any())
                {
                    throw new ApiException.BadRequestException("Username already exists");
                }

                string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

                var dealerId = await _userRepository.GetDealerIdByNameAsync(request.DealerName);
                if (dealerId == null)
                {
                    await transaction.RollbackAsync();
                    throw new ApiException.NotFoundException($"Dealer '{request.DealerName}' does not exist");
                }

                var user = new DAL.Models.Account
                {
                    Id = Guid.NewGuid(),
                    Username = request.Username,
                    Password = passwordHash,
                    Name = request.Name,
                    Email = request.Email,
                    Address = request.Address,
                    Phone = request.Phone,
                    ContactPerson = request.ContactPerson,
                    CreatedAt = DateTime.UtcNow,
                    DealerId = dealerId.Value,
                    Roles = new List<Role>()
                };

                var dobProp = user.GetType().GetProperty("Dob");
                if (dobProp != null && request.Dob.HasValue)
                    dobProp.SetValue(user, request.Dob.Value);

                await _userRepository.AddAsync(user);

                string roleName = !string.IsNullOrEmpty(request.Role) ? request.Role : "CompanyStaff";
                var role = await _roleRepository.GetByNameAsync(roleName);
                if (role == null)
                {
                    await transaction.RollbackAsync();
                    throw new ApiException.NotFoundException($"Role '{roleName}' does not exist");
                }

                await _userRepository.AssignRoleToUserAsync(user.Id, roleName);

                await transaction.CommitAsync();

                return new RegisterResponseDto
                {
                    Success = true,
                    Message = "Register successful",
                    UserId = user.Id
                };
            }
        }

        public async Task<LogoutResponseDto> LogoutAsync(Guid userId)
        {
            await _jwtService.RevokeRefreshTokenAsync(userId);

            return new LogoutResponseDto
            {
                Success = true,
                Message = "Logged out successfully"
            };
        }

        public async Task<RefreshTokenResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request)
        {
            // Validate the expired access token
            var principal = _jwtService.GetPrincipalFromExpiredToken(request.AccessToken);
            if (principal == null)
            {
                throw new ApiException.BadRequestException("Invalid access token");
            }

            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                throw new ApiException.BadRequestException("Invalid user identifier in token");
            }

            var (success, newAccessToken, newRefreshToken) = await _jwtService.RefreshTokenAsync(request.RefreshToken);

            if (!success)
            {
                throw new ApiException.BadRequestException("Invalid refresh token");
            }

            return new RefreshTokenResponseDto
            {
                Success = true,
                Message = "Token refreshed successfully",
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }
    }
}
