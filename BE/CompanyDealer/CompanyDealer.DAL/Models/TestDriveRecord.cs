using System;

namespace CompanyDealer.DAL.Models
{
    public enum TestDriveStatus
    {
        Pending,
        Approved,
        Rejected,
        ChangeRequested,
        Completed,
        Cancelled
    }

    public class TestDriveRecord
    {
        public Guid Id { get; set; }
        public DateTime TestDate { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerContact { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;

        // Status and workflow
        public TestDriveStatus Status { get; set; } = TestDriveStatus.Pending;
        
        // Creator information
        public Guid? CreatedBy { get; set; }
        public string CreatedByName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Approver information
        public Guid? ApprovedBy { get; set; }
        public string ApprovedByName { get; set; } = string.Empty;
        public DateTime? ApprovedAt { get; set; }
        
        // Rejection information
        public string RejectionReason { get; set; } = string.Empty;
        public DateTime? RejectedAt { get; set; }

        // Navigation properties
        public Guid DealerId { get; set; }
        public Dealer Dealer { get; set; } = null!;
        
        public Guid VehicleId { get; set; }
        public Vehicle Vehicle { get; set; } = null!;
    }
}
