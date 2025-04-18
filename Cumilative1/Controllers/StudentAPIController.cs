using Cumilative1.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Cumilative1.Controllers
{
    public class StudentAPIController : Controller
    {
        private readonly SchoolDbContext _context;
        public StudentAPIController(SchoolDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns a list of Students in the system
        /// </summary>
        /// <example>
        /// GET /localhost:7044/ListStudents -> [27","dav","Larson", "N1744", "2018-07-19"]
        /// </example>
        /// <returns>
        /// A list of strings, formatted "{StudentId} {StudentFname} {StudentLname} {StudentNumber} {EnrolDate}"
        /// </returns>


        [HttpGet]
        [Route(template: "ListStudent")]
        public List<Student> ListStudent()
        {
            // Create an empty list of Students
            List<Student> Students = new List<Student>();

            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();

                // SQL QUERY
                Command.CommandText = "select * from students";

                // Gather Result Set of Query into a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    while (ResultSet.Read())
                    {

                        //Access Column information by the DB column name as an index
                        int Id = Convert.ToInt32(ResultSet["studentid"]);
                        string Fname = ResultSet["studentfname"].ToString();
                        string Lname = ResultSet["studentlname"].ToString();
                        string Number = ResultSet["studentnumber"].ToString();

                        DateTime EDate = Convert.ToDateTime(ResultSet["enroldate"]);


                        //short form for setting all properties while creating the object
                        Student CurrentStudent = new Student()
                        {
                            StudentId = Id,
                            StudentFirstName = Fname,
                            StudentLastName = Lname,
                            StudentNumber = Number,
                            EnrollmentDate = EDate
                        };

                        Students.Add(CurrentStudent);
                    }
                }
            }

            // Return the final list of Student
            return Students;
        }


        // <summary>
        // Output a Student associated with the input Student id.
        // </summary>
        // <param name="StudentId">The primary key of the Student</param>
        // <returns>An object associated with the Student</returns>
        // <example>
        // GET: api/Student/FindStudent/4 -> {"StudentId":"4","StudentFName":"Austin","StudentLName":"Simon","StudentNumber":"N1682","EnrolDate":"2018-06-14"}
        // </example>
        [HttpGet]
        [Route(template: "FindStudent/{StudentId}")]
        public Student FindStudent(int StudentId)
        {
            Student SelectedStudent = new Student();

            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                // opening the connection
                Connection.Open();

                // setting up an sql query
                string query = "select * from students where studentid=" + StudentId;

                // setting the command text to the query
                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = query;

                // use the result set to get information about the Student
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {

                    while (ResultSet.Read())
                    {


                        //get information about the Student
                        SelectedStudent.StudentId = Convert.ToInt32(ResultSet["studentid"]);
                        SelectedStudent.StudentFirstName = ResultSet["studentfname"].ToString();
                        SelectedStudent.StudentLastName = ResultSet["studentlname"].ToString();
                        SelectedStudent.StudentNumber = ResultSet["studentnumber"].ToString();
                        SelectedStudent.EnrollmentDate = Convert.ToDateTime(ResultSet["enroldate"]);

                    }

                }
            }

            return SelectedStudent;
        }



        // <summary>
        // Adds an Student to the database
        // </summary>
        // <param name="StudentData">Student Object</param>
        // <example>
        // POST: api/StudentData/AddStudent
        // Headers: Content-Type: application/json
        // Request Body:
        // {
        //      "StudentId":"15",
        //	    "StudentFName":"saumil",
        //	    "StudentLName":"patel",
        //	    "StudentNumber":"n1500",
        //	    "StudentDate":"2023-06-15",
        // }
        // </example>
        // <returns>
        // The inserted Student Id from the database if successful. 0 if Unsuccessful
        // </returns>
        [HttpPost(template: "AddStudent")]
        public int AddStudent([FromBody] Student StudentData)
        {
            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();


                Command.CommandText = "insert into students (studentid, studentfname, studentlname, studentnumber, enroldate) values (@studentid, @studentfname, @studentlname, @studentnumber, @enroldate)";

                Command.Parameters.AddWithValue("@studentid", StudentData.StudentId);
                Command.Parameters.AddWithValue("@studentfname", StudentData.StudentFirstName);
                Command.Parameters.AddWithValue("@studentlname", StudentData.StudentLastName);
                Command.Parameters.AddWithValue("@studentnumber", StudentData.StudentNumber);
                Command.Parameters.AddWithValue("@enroldate", StudentData.EnrollmentDate);

                Command.ExecuteNonQuery();

                return Convert.ToInt32(Command.LastInsertedId);

            }
            // if failure
            return 0;
        }


        // <summary>
        // Deletes an Student from the database
        // </summary>
        // <param name="StudentId">Primary key of the Student to delete</param>
        // <example>
        // DELETE: api/StudentData/DeleteStudent -> 1
        // </example
        // <returns>
        // Number of rows affected by delete operation.
        // </returns>
        [HttpDelete(template: "DeleteStudent/{StudentId}")]
        public int DeleteStudent(int StudentId)
        {
            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();


                Command.CommandText = "delete from students where studentid=@id";
                Command.Parameters.AddWithValue("@id", StudentId);
                return Command.ExecuteNonQuery();

            }
            // if failure
            return 0;
        }

        // <summary>
        // Updates an Student in the database. Data is Student object, request query contains ID
        // </summary>
        // <param name="StudentData">Student Object</param>
        // <param name="Studentid">The Student ID primary key</param>
        // <example>
        // PUT: api/Student/UpdateStudent/5
        // Headers: Content-Type: application/json
        // Request Body:
        // {
        //	    "StudentFirstName":"saumil",
        //	    "StudentLastName":"patel", 
        // } -> 
        // {
        //     "StudentId":5,
        //	    "StudentFirstName":"saumil",
        //	    "StudentLastName":"patel",
        //	    "StudentNumber":"N1234",
        //	    "EnrollmentDate":"12-02-2023"
        // }
        // </example>
        // <returns>
        // The updated Student object
        // </returns>
        [HttpPut(template: "UpdateStudent/{StudentId}")]
        public Student UpdateStudent(int StudentId, [FromBody] Student StudentData)
        {
            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                // parameterize query

                Command.CommandText = "update students set studentfname=@studentfname, studentlname=@studentlname, studentnumber=@studentnumber, enroldate=@enroldate where studentid=@id";
                Command.Parameters.AddWithValue("@studentid", StudentData.StudentId);
                Command.Parameters.AddWithValue("@studentfname", StudentData.StudentFirstName);
                Command.Parameters.AddWithValue("@studentlname", StudentData.StudentLastName);
                Command.Parameters.AddWithValue("@studentnumber", StudentData.StudentNumber);
                Command.Parameters.AddWithValue("@enroldate", StudentData.EnrollmentDate);


                Command.Parameters.AddWithValue("@id", StudentId);

                Command.ExecuteNonQuery();

            }

            return FindStudent(StudentId);
        }
    }
}