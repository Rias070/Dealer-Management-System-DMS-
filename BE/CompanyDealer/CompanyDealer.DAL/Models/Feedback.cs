using System;

namespace CompanyDealer.DAL.Models
{
    public class Feedback
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public int Rating { get; set; }
        public DateTime SubmissionDate { get; set; } = DateTime.UtcNow;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerContact { get; set; } = string.Empty;

        // Navigation property
        public Guid DealerId { get; set; }
        public Dealer Dealer { get; set; } = null!;
    }
}
