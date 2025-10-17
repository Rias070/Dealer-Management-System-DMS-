using System;
using System.Threading.Tasks;
using CompanyDealer.DAL.Repositories;

namespace CompanyDealer.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAccountRepository _accountRepository;

        public AuthService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<LoginResult> LoginAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return new LoginResult { Success = false, Error = "Username and password are required" };
            }

            var account = await _accountRepository.GetByUsernameAndPasswordAsync(username, password);
            if (account == null)
            {
                return new LoginResult { Success = false, Error = "Invalid username or password" };
            }

            if (!account.IsActive)
            {
                return new LoginResult { Success = false, Error = "Account is inactive" };
            }

            return new LoginResult
            {
                Success = true,
                AccountId = account.Id,
                Name = account.Name,
                Email = account.Email,
                Role = account.Role,
                Username = account.Username,
                IsActive = account.IsActive
            };
        }
    }
}


