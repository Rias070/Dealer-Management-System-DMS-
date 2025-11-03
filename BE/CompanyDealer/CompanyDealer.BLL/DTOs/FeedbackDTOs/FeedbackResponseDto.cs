using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyDealer.BLL.DTOs.FeedbackDTOs
{
    public class FeedbackResponseDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
        public DateTime SubmissionDate { get; set; }
        public Guid DealerId { get; set; }
        public Guid CustomerId { get; set; }
    }
}
