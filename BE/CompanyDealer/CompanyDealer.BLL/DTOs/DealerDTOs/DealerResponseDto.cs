using System;

namespace CompanyDealer.BLL.DTOs.DealerDTOs
{
    public class DealerResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid DealerId { get; set; }
        public DealerDto Dealer { get; set; } = null!;
    }
}
