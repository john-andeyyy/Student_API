namespace Student_API.Models.DTOs.Address
{
    public class AddressDTO
    {
        public int Id { get; set; }
        public required string Barangay { get; set; }
        public required string Province { get; set; } 
        public required string Municipality { get; set; }
        public required string Country { get; set; } 

    }

}
