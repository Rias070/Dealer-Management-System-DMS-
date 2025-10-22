using System;
using CompanyDealer.DAL.Models;

namespace CompanyDealer.BLL.DTOs.TestDriveDTOs
{
    public class TestDriveResponse
    {
        public Guid Id { get; set; }
        public DateTime TestDate { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerContact { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

        // Creator information
        public Guid? CreatedBy { get; set; }
        public string CreatedByName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        // Approver information
        public Guid? ApprovedBy { get; set; }
        public string ApprovedByName { get; set; } = string.Empty;
        public DateTime? ApprovedAt { get; set; }

        // Rejection information
        public string RejectionReason { get; set; } = string.Empty;
        public DateTime? RejectedAt { get; set; }

        // Dealer information
        public Guid DealerId { get; set; }
        public DealerInfo? Dealer { get; set; }

        // Vehicle information
        public Guid VehicleId { get; set; }
        public VehicleInfo? Vehicle { get; set; }
    }

    public class DealerInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
    }

    public class VehicleInfo
    {
        public Guid Id { get; set; }
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Color { get; set; } = string.Empty;
        public string VIN { get; set; } = string.Empty;
    }
}
