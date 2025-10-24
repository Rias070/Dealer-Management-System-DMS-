using System;

namespace CompanyDealer.BLL.DTOs.AuthDTOs
{
    public class RegisterRequestDto
    {
        public string Username { get; set; }         // Tên đăng nhập
        public string Password { get; set; }         // Mật khẩu
        public string Name { get; set; }             // Họ tên đầy đủ
        public string Phone { get; set; }            // Số điện thoại
        public string ContactPerson { get; set; }   // Người liên hệ
        public string Email { get; set; }            // Email
        public string Address { get; set; }          // Địa chỉ
        public DateTime? Dob { get; set; }           // Ngày sinh (có thể null)\
        public Guid DealerId { get; set; }     // Tên đại lý
        public Guid RoleId { get; set; }    // Vai trò 
    }
}
