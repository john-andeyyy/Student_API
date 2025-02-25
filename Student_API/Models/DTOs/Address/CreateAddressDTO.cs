using System.ComponentModel;

namespace Student_API.Models.DTOs.Address
{
    public class CreateAddressDTO
    {
        public required string Barangay { get; set; } 
        public required string Province { get; set; }
        public required string Municipality { get; set; } 
        // for swagger only 
        [DefaultValue("Philippines")]
        public string Country { get; set; }

        // for api the default value is phippines even no Country key send
        public CreateAddressDTO()
        {
            Country = "Philippines";
        }
    }
}
