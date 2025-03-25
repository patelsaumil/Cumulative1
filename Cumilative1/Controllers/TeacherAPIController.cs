using Cumilative1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Cumilative1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherAPIController : ControllerBase
    {

        private readonly SchoolDbContext _context;
        public TeacherAPIController(SchoolDbContext context)
        {
            _context = context;
        }

       
        [HttpGet]
        [Route(template: "ListAllTeacher")]
        public List<string> ListAllTeacher()
        {
            // Create an empty list of Teacher Names
            List<string> TeacherAllNames = new List<string>();


            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();

                MySqlCommand Command = Connection.CreateCommand();

                //SQL QUERY
                Command.CommandText = "select * from teachers";

                // Gather Result Set of Query into a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    //Loop Through Each Row the Result Set
                    while (ResultSet.Read())
                    {
                        string FirstName = ResultSet["teacherfname"].ToString();
                        string LastName = ResultSet["teacherlname"].ToString();

                        //Access Column information by the DB column name as an index
                        string TeacherNames = $"{FirstName} {LastName}";
                        //Add the Teacher all Name to the List
                        TeacherAllNames.Add(TeacherNames);
                    }
                }
            }


            //Return the final list of Teacher names
            return TeacherAllNames;
        }


        /// <summary>
        /// Returns an Teacher in the database by their ID
        /// </summary>
        /// <example>
        /// GET api/Teachers_page/FindTeacher/1 -> {"TeacherId":1}
        /// </example>
        /// <returns>
        /// A matching Teacher object by its ID. Empty object if Teacher not found
        /// </returns>
        [HttpGet(template: "FindTeacherID/{id}")]
        public Teacher FindTeacherID(int id)
        {

            Teacher TeacherIDSelect = new Teacher();

            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                // @id is replaced with a 'sanitized' id
                Command.CommandText = "select * from teachers where teacherid=@id";
                Command.Parameters.AddWithValue("@id", id);

                // Gather Result Set of Query into a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    //Loop Through Each Row the Result Set
                    while (ResultSet.Read())
                    {
                        //Access Column information by the DB column name as an index
                        int Id = Convert.ToInt32(ResultSet["teacherid"]);
                        string FName = ResultSet["teacherfname"].ToString();
                        string LName = ResultSet["teacherlname"].ToString();

                        TeacherIDSelect.TeacherId = Id;
                        TeacherIDSelect.TeacherFName = FName;
                        TeacherIDSelect.TeacherLName = LName;
                  

                    }
                }
            }


            //Return the final list of teacher names
            return TeacherIDSelect;
        }

        /// <summary>
        /// Returns a list of Students in the system
        /// </summary>
        /// <example>
        /// GET /localhost:xx/student -> ["6","Kevin","Williams",..]
        /// </example>
        /// <returns>
        /// A list of strings, formatted "{id} {First Name} {Last Name}"
        /// </returns>
        [HttpGet]
        [Route(template: "StudentList")]
        public List<string> StudentList()
        {
            // Create an empty list of Student Names
            List<string> StudentAllNames = new List<string>();


            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();

                MySqlCommand Command = Connection.CreateCommand();

                //SQL QUERY
                Command.CommandText = "select * from students";

                // Gather Result Set of Query into a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    //Loop Through Each Row the Result Set
                    while (ResultSet.Read())
                    {
                        string Id = ResultSet["studentid"].ToString();
                        string FName = ResultSet["studentfname"].ToString();
                        string LName = ResultSet["studentlname"].ToString();


                        //Access Column information by the DB column name as an index
                        string StudentNames = $"{Id} {FName} {LName}";
                        //Add the Student Name to the List
                        StudentAllNames.Add(StudentNames);
                    }
                }
            }


            //Return the final list of Students
            return StudentAllNames;
        }


        /// <summary>
        /// Returns a list of Courses in the system
        /// </summary>
        /// <example>
        /// GET /localhost:xx/ListCourses -> ["4","http5104","7","2018-09-04","2018-12-14","Digital Design"]
        /// </example>
        /// <returns>
        /// A list of strings, formatted "{CourseId} {CourseCode} {TeacherId} {StartDate} {FinishDate} {CourseName}"
        /// </returns>


        [HttpGet]
        [Route(template: "ListCourse")]
        public List<Courses> ListCourse()
        {
            // Create an empty list of Teachers
            List<Courses> CourseList = new List<Courses>();   

            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();

                // SQL QUERY
                Command.CommandText = "select * from courses";

                // Gather Result Set of Query into a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    while (ResultSet.Read())
                    {
                        Courses course = new Courses
                        {
                            CourseId = Convert.ToInt32(ResultSet["courseid"]),
                            CourseCode = ResultSet["coursecode"].ToString(),
                            TeacherId = Convert.ToInt32(ResultSet["teacherid"]),
                            StartDate = Convert.ToDateTime(ResultSet["startdate"]),
                            FinishDate = Convert.ToDateTime(ResultSet["finishdate"]),
                            CourseName = ResultSet["coursename"].ToString(),

                        };

                        // Add the Course object to the list
                        CourseList.Add(course);
                    }
                }
            }

            // Return the final list of Course
            return CourseList;
        }


    }
}
