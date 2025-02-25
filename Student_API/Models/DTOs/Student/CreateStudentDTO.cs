namespace Student_API.Models.DTOs.Student
{
    public class CreateStudentDTO
    {
        public required string Name { get; set; }
        public required DateOnly Birthday { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required int AddressId { get; set; }
    }
}
