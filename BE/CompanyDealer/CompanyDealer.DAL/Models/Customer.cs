using System;

namespace CompanyDealer.DAL.Models
{
    public class Customer
    {
        public Guid Id { get; set; }                // Unique identifier
        public string Name { get; set; }            // Full name
        public string Email { get; set; }           // Email address
        public string Phone { get; set; }           // Phone number
        public string Address { get; set; }         // Address
        public DateTime? Dob { get; set; }          // Date of birth (nullable)
        public DateTime CreatedAt { get; set; }     // Registration date
        public bool IsActive { get; set; }          // Status
    }
}
