using System;

namespace CompanyDealer.BLL.DTOs.FeedbackDTOs
{
    public class FeedbackRequestDto
    {
        public string Content { get; set; }
        public int Rating { get; set; }
        public Guid DealerId { get; set; }
        public Guid CustomerId { get; set; }
    }
}
