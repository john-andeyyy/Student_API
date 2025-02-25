using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_API.Data;
using Student_API.Models.DTOs.Address;
using Student_API.Models.DTOs.Student;
using Student_API.Models.Entities;

namespace Student_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public StudentController(ApplicationDbContext dbContext) 
        {
            this.dbContext = dbContext;
        }

        [HttpGet("GetAllStudents")]
        public IActionResult GetAllStudents()
        {
            var students = dbContext.Students
                .Include(s => s.Address)
                .ToList(); 

            var studentsList = students.Select(s => FullStudentData(s));

            return Ok(studentsList);
        }


        [HttpGet("GetStudent/{id}")]
        public IActionResult GetStudent(int id)
        {
            var student = dbContext.Students
                .Include(s => s.Address)
                .FirstOrDefault(s => s.StudentId == id);

            if (student == null) return NotFound("Student not found");

            var studentslist = FullStudentData(student);

            return Ok(studentslist);
        }

        [HttpPost("CreateStudent")]
        public IActionResult AddStudent([FromBody] CreateStudentDTO NewStudentDTO)
        {
            if (NewStudentDTO == null) return BadRequest(new { message = "Invalid student data" });

            var address = dbContext.Address.FirstOrDefault(a => a.Id == NewStudentDTO.AddressId);
            if (address == null)  return NotFound(new { message = "Address not found" });

            var student = new Student
            {
                Name = NewStudentDTO.Name,
                Birthday = NewStudentDTO.Birthday,
                Email = NewStudentDTO.Email,
                PhoneNumber = NewStudentDTO.PhoneNumber,
                AddressId = NewStudentDTO.AddressId
            };

            dbContext.Students.Add(student);
            dbContext.SaveChanges();

            var result = FullStudentData(student);
            return Ok(result);
        }

        [HttpPut]
        [Route("UpdateStudent/{id}")]
        public IActionResult UpdateStudent([FromBody] UpdateStudentDTO UpdateStudentDTO,int id)
        {
            var student = dbContext.Students.Find(id);
            if (student == null) return NotFound("Student Not Found");

            student.Name = UpdateStudentDTO.Name;
            student.AddressId = UpdateStudentDTO.AddressId;
            student.Birthday = UpdateStudentDTO.Birthday;
            student.Email = UpdateStudentDTO.Email;
            student.PhoneNumber = UpdateStudentDTO.PhoneNumber;
            dbContext.SaveChanges();
            return Ok(FullStudentData(student));
        }

        [HttpDelete("DeleteStudent/{id}")]
        public IActionResult DeleteStudent( int id)
        {
            var student = dbContext.Students.Find(id);
            if (student == null) return NotFound("Student Not FOund");
            dbContext.Remove(student);
            dbContext.SaveChanges();
            return Ok($"Successfully deleted the student with ID {id}");
        }



        // function to get full data of the Student to 
        // Student bcuse it accept the Student object
        private StudentDTO FullStudentData(Student student)
        {
            return new StudentDTO
            {
                StudentId = student.StudentId,
                Name = student.Name,
                Birthday = student.Birthday,
                Email = student.Email,
                PhoneNumber = student.PhoneNumber,
                CreatedDate = student.CreatedDate,
                AddressId = student.AddressId,
                Address = new AddressDTO
                {
                    Id = student.Address.Id,
                    Barangay = student.Address.Barangay,
                    Province = student.Address.Province,
                    Municipality = student.Address.Municipality,
                    Country = student.Address.Country
                }
            };
        }



    }
}
