using System;
using System.ComponentModel.DataAnnotations;

namespace CompanyDealer.BLL.DTOs.TestDriveDTOs
{
    public class RejectTestDriveRequest
    {
        [Required(ErrorMessage = "Rejected by is required")]
        public Guid RejectedBy { get; set; }

        [Required(ErrorMessage = "Rejection reason is required")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Rejection reason must be between 10 and 500 characters")]
        public string RejectionReason { get; set; } = string.Empty;
    }
}
