using System;

namespace CompanyDealer.BLL.DTOs.RestockRequestDTOs
{
    public class RestockRequestDto
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public Guid DealerId { get; set; }
        public Guid VehicleId { get; set; }
        public string VehicleName { get; set; }
        public int Quantity { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? ResponseDate { get; set; }
        public string AcceptenceLevel { get; set; }
        public string AcceptedBy { get; set; }
        public string ReasonRejected { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
    }
}