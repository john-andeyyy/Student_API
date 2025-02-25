
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Student_API.Models.Entities
{
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentId { get; set; }

        [Required]
        public required string Name { get; set; } 

        public required DateOnly Birthday { get; set; }

        [Required]
        public required string Email { get; set; }

        public required string PhoneNumber { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [ForeignKey("Address")]
        public required int AddressId { get; set; }
        //public Address? Address { get; set; }
    }
}