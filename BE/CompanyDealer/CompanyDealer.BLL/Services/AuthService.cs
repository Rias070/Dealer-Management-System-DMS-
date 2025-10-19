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
                return new LoginResponseDto
                {
                    Success = false,
                    Message = "Invalid username or password"
                };
            }

            bool passwordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
            if (!passwordValid)
            {
                return new LoginResponseDto
                {
                    Success = false,
                    Message = "Invalid username or password"
                };
            }

            var (accessToken, refreshToken) = await _jwtService.SaveTokensAsync(user.Id);

            // Assuming the first role is the main role (adjust as needed)
            int role = user.Roles?.FirstOrDefault() != null ? Convert.ToInt32(user.Roles.First().Id) : 0;

            return new LoginResponseDto
            {
                Success = true,
                Message = "Login successful",
                UserId = user.Id,
                Token = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            using (var transaction = await _userRepository.BeginTransactionAsync())
            {
                try
                {
                    var existingUser = await _userRepository.FindAsync(u => u.Username == request.Username);
                    if (existingUser.Any())
                    {
                        return new RegisterResponseDto
                        {
                            Success = false,
                            Message = "Username already exists"
                        };
                    }

                    string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

                    var user = new DAL.Models.Account
                    {
                        Id = Guid.NewGuid(),
                        Username = request.Username,
                        Password = passwordHash,
                        Name = request.Name,
                        Email = request.Email,
                        Address = request.Address,
                        CreatedAt = DateTime.UtcNow,
                        IsActive = request.IsActive
                    };

                    // Nếu Account có trường Dob
                    var dobProp = user.GetType().GetProperty("Dob");
                    if (dobProp != null && request.Dob.HasValue)
                        dobProp.SetValue(user, request.Dob.Value);

                    await _userRepository.AddAsync(user);

                    // Kiểm tra role có tồn tại không
                    string roleName = !string.IsNullOrEmpty(request.Role) ? request.Role : "User";
                    var role = await _roleRepository.GetByNameAsync(roleName);
                    if (role == null)
                    {
                        await transaction.RollbackAsync();
                        return new RegisterResponseDto
                        {
                            Success = false,
                            Message = $"Role '{roleName}' does not exist"
                        };
                    }

                    // Gán role cho user
                    await _userRepository.AssignRoleToUserAsync(user.Id, roleName);

                    await transaction.CommitAsync();

                    return new RegisterResponseDto
                    {
                        Success = true,
                        Message = "Register successful",
                        UserId = user.Id
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return new RegisterResponseDto
                    {
                        Success = false,
                        Message = $"Registration failed: {ex.Message}"
                    };
                }
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
            try
            {
                // Validate the expired access token
                var principal = _jwtService.GetPrincipalFromExpiredToken(request.AccessToken);
                var userId = Guid.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                // Refresh the token
                var (success, newAccessToken, newRefreshToken) = await _jwtService.RefreshTokenAsync(request.RefreshToken);

                if (!success)
                {
                    return new RefreshTokenResponseDto
                    {
                        Success = false,
                        Message = "Invalid refresh token"
                    };
                }

                return new RefreshTokenResponseDto
                {
                    Success = true,
                    Message = "Token refreshed successfully",
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken
                };
            }
            catch (Exception ex)
            {
                return new RefreshTokenResponseDto
                {
                    Success = false,
                    Message = "Error refreshing token: " + ex.Message
                };
            }
        }
    }
}
