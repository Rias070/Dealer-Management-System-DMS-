using System;

namespace CompanyDealer.DAL.Models
{
    public class Account
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ContactPerson { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        public int Role { get; set; } // 0: admin, 1: dealer staff, 2: manager staff, 3: evm staff

        // Đăng nhập cơ bản
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        // Navigation property
        public Guid DealerId { get; set; }
        public Dealer Dealer { get; set; } = null!;
    }
}
