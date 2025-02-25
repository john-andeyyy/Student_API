using System.ComponentModel;

namespace Student_API.Models.DTOs.Address
{
    public class UpdateAddressDTO
    {
        public required string Barangay { get; set; } 
        public required string Province { get; set; }
        public required string Municipality { get; set; }
        public required string Country { get; set; } 

    }
}
