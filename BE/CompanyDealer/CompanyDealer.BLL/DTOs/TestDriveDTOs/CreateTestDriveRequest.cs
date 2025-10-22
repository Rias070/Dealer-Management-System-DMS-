using System;
using System.ComponentModel.DataAnnotations;

namespace CompanyDealer.BLL.DTOs.TestDriveDTOs
{
    public class CreateTestDriveRequest
    {
        [Required(ErrorMessage = "Test date is required")]
        public DateTime TestDate { get; set; }

        [Required(ErrorMessage = "Customer name is required")]
        [StringLength(100, ErrorMessage = "Customer name cannot exceed 100 characters")]
        public string CustomerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Customer contact is required")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        [StringLength(20, ErrorMessage = "Contact number cannot exceed 20 characters")]
        public string CustomerContact { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
        public string? Notes { get; set; }

        [Required(ErrorMessage = "Dealer ID is required")]
        public Guid DealerId { get; set; }

        [Required(ErrorMessage = "Vehicle ID is required")]
        public Guid VehicleId { get; set; }
    }
}
