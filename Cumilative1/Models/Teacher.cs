namespace Cumilative1.Models
{
    public class Teacher
    {
        public int TeacherId { get; set; }
        public string TeacherFName { get; set; }
        public string TeacherLName { get; set; }
    }

    public class Courses
    {
        public int CourseId { get; set; }
        public string? CourseCode { get; set; }
        public int TeacherId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public string? CourseName { get; set; }
    }
}
