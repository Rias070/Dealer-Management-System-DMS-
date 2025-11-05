using System;

namespace CompanyDealer.DAL.Models
{
    public class Feedback
    {
        public Guid Id { get; set; }

        public string Content { get; set; } = string.Empty;
        public int Rating { get; set; } // e.g., 1 to 5
        public DateTime SubmissionDate { get; set; } = DateTime.UtcNow;

        // Navigation property
        public Guid DealerId { get; set; }
        public Dealer Dealer { get; set; } = null!;

        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;
    }
}
