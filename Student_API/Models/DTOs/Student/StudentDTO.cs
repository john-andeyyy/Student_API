using Student_API.Models.DTOs.Address;

namespace Student_API.Models.DTOs.Student
{
    public class StudentDTO
    {
        public int StudentId { get; set; }
        public required string Name { get; set; } 
        public required DateOnly Birthday { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int? AddressId { get; set; }
        public AddressDTO? Address { get; set; } 
    }
}   