using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Student_API.Models.Entities
{
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Barangay { get; set; } = string.Empty;
        public string Province { get; set; } = string.Empty;
        public string Municipality { get; set; } = string.Empty;
        public string Country { get; set; } = "Philippines";
      
    }
}