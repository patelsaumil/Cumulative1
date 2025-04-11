using Cumilative1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Cumilative1.Controllers
{
    [Route("api/Teacher")]
    [ApiController]
    public class TeacherAPIController : ControllerBase
    {
        private readonly SchoolDbContext _context;
        public TeacherAPIController(SchoolDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns a list of Teachers in the system
        /// </summary>
        /// <example>
        /// GET /localhost:7044/ListTeacherNames -> ["alex"," Cummings", "Linda "..]
        /// </example>
        /// <returns>
        /// A list of strings, formatted "{First Name} {Last Name}"
        /// </returns>
        [HttpGet]
        [Route(template: "ListTeachers")]
        public List<Teacher> ListTeachers(string searchKey = null)
        {
            List<Teacher> teacherResults = new List<Teacher>();

            using (MySqlConnection dbConnection = _context.AccessDatabase())
            {
                dbConnection.Open();
                MySqlCommand dbCommand = dbConnection.CreateCommand();

                string sqlQuery = "SELECT * FROM teachers";

                if (searchKey != null)
                {
                    sqlQuery += @" WHERE lower(teacherfname) LIKE lower(@key)
                        OR lower(teacherlname) LIKE lower(@key)
                        OR lower(CONCAT(teacherfname, ' ', teacherlname)) LIKE lower(@key)";
                    dbCommand.Parameters.AddWithValue("@key", $"%{searchKey}%");
                }

                dbCommand.CommandText = sqlQuery;
                dbCommand.Prepare();

                using (MySqlDataReader result = dbCommand.ExecuteReader())
                {
                    while (result.Read())
                    {
                        Teacher singleTeacher = new Teacher()
                        {
                            TeacherId = Convert.ToInt32(result["teacherid"]),
                            TeacherFirstName = result["teacherfname"].ToString(),
                            TeacherLastName = result["teacherlname"].ToString(),
                            HireDate = Convert.ToDateTime(result["hiredate"]),
                            Salary = Convert.ToDecimal(result["salary"])
                        };

                        teacherResults.Add(singleTeacher);
                    }
                }
            }

            return teacherResults;
        }


        /// <summary>
        /// Returns an Teacher in the database by their ID
        /// </summary>
        /// <example>
        /// GET api/Teacher/FindTeacher/4 -> {"TeacherId":4,"TeacherFname":"Linda","TeacherLName":"Chan"}
        /// </example>
        /// <returns>
        /// A matching Teacher object by its ID. Empty object if Teacher not found
        /// </returns>
        [HttpGet(template: "FindTeacher/{id}")]
        public Teacher FindTeacher(int id)
        {

            Teacher SelectedTeacher = new Teacher();

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
                        string FirstName = ResultSet["teacherfname"]?.ToString() ?? "";
                        string LastName = ResultSet["teacherlname"]?.ToString() ?? "";

                        DateTime HireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                        decimal Salary = Convert.ToDecimal(ResultSet["salary"]);


                        SelectedTeacher.TeacherId = Id;
                        SelectedTeacher.TeacherFirstName = FirstName;
                        SelectedTeacher.TeacherLastName = LastName;
                        SelectedTeacher.HireDate = HireDate;
                        SelectedTeacher.Salary = Salary;

                    }
                }
            }


            //output the final list of teacher names
            return SelectedTeacher;
        }


        // <summary>
        // Add an Teacher to the database
        // </summary>
        // <param name="TeacherData">Teacher Object</param>
        // <example>
        // POST: api/TeacherData/AddTeacher
        // Headers: Content-Type: application/json
        // Request Body:
        // {
        //      "TeacherId":"11",
        //	    "TeacherFname":"saumil",
        //	    "TeacherLname":"patel",
        //	    "EmployeeNumber":"t000",
        //	    "TeacherHireDate":"2024-10-15 00:00:00",
        // } -> 409
        // </example>
        // <returns>
        // The inserted Teacher Id from the database if successful. 0 if Unsuccessful
        // </returns>
        [HttpPost(template: "AddTeacher")]
        public int AddTeacher([FromBody] Teacher TeacherData)
        {
            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();


                Command.CommandText = "insert into teachers (teacherid, teacherfname, teacherlname, employeenumber, hiredate, salary) values (@teacherid, @teacherfname, @teacherlname, @employeenumber, CURRENT_DATE(), @salary)";
                Command.Parameters.AddWithValue("@teacherid", TeacherData.TeacherId);
                Command.Parameters.AddWithValue("@teacherfname", TeacherData.TeacherFirstName);
                Command.Parameters.AddWithValue("@teacherlname", TeacherData.TeacherLastName);
                Command.Parameters.AddWithValue("@employeenumber", TeacherData.EmpNumber);
                Command.Parameters.AddWithValue("@hiredate", TeacherData.HireDate);
                Command.Parameters.AddWithValue("@salary", TeacherData.Salary);

                Command.ExecuteNonQuery();

                return Convert.ToInt32(Command.LastInsertedId);

            }
            // if failure
            return 0;
        }


        // <summary>
        // Deletes an Teacher from the database
        // </summary>
        // <param name="TeacherId">Primary key of the teacher to delete</param>
        // <example>
        // DELETE: api/TeacherData/DeleteTeacher -> 2
        // </example
        // <returns>
        // Number of rows affected by delete operation.
        // </returns>
        [HttpDelete(template: "DeleteTeacher/{TeacherId}")]
        public int DeleteTeacher(int TeacherId)
        {
            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();


                Command.CommandText = "delete from teachers where teacherid=@id";
                Command.Parameters.AddWithValue("@id", TeacherId);
                return Command.ExecuteNonQuery();

            }
            // if failure
            return 0;
        }



    }
}


