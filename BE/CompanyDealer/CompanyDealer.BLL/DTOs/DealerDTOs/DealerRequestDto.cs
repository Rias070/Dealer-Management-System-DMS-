using System;

namespace CompanyDealer.BLL.DTOs.DealerDTOs
{
    public class DealerRequestDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string ContactInfo { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
