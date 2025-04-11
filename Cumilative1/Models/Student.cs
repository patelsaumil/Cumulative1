using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Cumilative1.Models
{

    public class Student
    {
        public int StudentId { get; set; }
        public string? StudentFirstName { get; set; }
        public string? StudentLastName { get; set; }
        public string? StudentNumber { get; set; }
        public DateTime EnrollmentDate { get; set; }

    }

}