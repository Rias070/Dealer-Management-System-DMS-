using System;
using System.ComponentModel.DataAnnotations;

namespace CompanyDealer.BLL.DTOs.TestDriveDTOs
{
    public class ApproveTestDriveRequest
    {
        [Required(ErrorMessage = "Approved by is required")]
        public Guid ApprovedBy { get; set; }
    }
}
