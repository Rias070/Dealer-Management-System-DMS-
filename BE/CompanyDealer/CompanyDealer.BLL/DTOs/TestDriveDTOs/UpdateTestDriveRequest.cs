using System;
using System.ComponentModel.DataAnnotations;

namespace CompanyDealer.BLL.DTOs.TestDriveDTOs
{
    public class UpdateTestDriveRequest
    {
        public DateTime? TestDate { get; set; }

        [StringLength(100, ErrorMessage = "Customer name cannot exceed 100 characters")]
        public string? CustomerName { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format")]
        [StringLength(20, ErrorMessage = "Contact number cannot exceed 20 characters")]
        public string? CustomerContact { get; set; }

        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
        public string? Notes { get; set; }

        public Guid? DealerId { get; set; }

        public Guid? VehicleId { get; set; }
    }
}
