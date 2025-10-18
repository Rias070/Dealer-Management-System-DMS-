using System;
using System.Threading.Tasks;

namespace CompanyDealer.Services
{
    public class LoginResult
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public Guid? AccountId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Role { get; set; }
        public string Username { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public interface IAuthService
    {
        Task<LoginResult> LoginAsync(string username, string password);
    }
}


