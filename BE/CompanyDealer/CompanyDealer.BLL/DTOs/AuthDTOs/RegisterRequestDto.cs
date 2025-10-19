using System;

namespace CompanyDealer.BLL.DTOs.AuthDTOs
{
    public class RegisterRequestDto
    {
        public string Username { get; set; }         // Tên đăng nhập
        public string Password { get; set; }         // Mật khẩu
        public string Name { get; set; }             // Họ tên đầy đủ
        public string Email { get; set; }            // Email
        public string Address { get; set; }          // Địa chỉ
        public DateTime? Dob { get; set; }           // Ngày sinh (có thể null)
        public bool IsActive { get; set; } = true;   // Trạng thái hoạt động
        public string Role { get; set; } = "User";   // Vai trò (mặc định là User)
    }
}
