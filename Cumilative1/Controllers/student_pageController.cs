using Cumilative1.Controllers;
using Cumilative1.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cumilative1.Controllers
{
    public class student_pageController : Controller
    {
        // get access to the component which retrieves data
        // the component is the StudentAPIController
        private readonly StudentAPIController _api;

        public student_pageController(StudentAPIController api)
        {
            _api = api;
        }

        // GET: /StudentPage/List -> A webpage which shows all Student in the system
        [HttpGet]
        public IActionResult List()
        {
            // get this information from the API
            List<Student> Students = _api.ListStudent(); // Assuming List Student method returns a list of Student objects

            // Return the list of Student to the View
            return View(Students);
        }

        // GET: /StudentPage/Show/{id} -> A webpage which shows one specific Student by its id
        [HttpGet]
        public IActionResult Show(int StudentId)
        {
            // Get specific Student details
            Student SelectedStudent = _api.FindStudent(StudentId);

            // Return the Course details to the View
            return View(SelectedStudent);
        }

        // GET : StudentPage/New
        [HttpGet]
        public IActionResult New(int id)
        {
            return View();
        }

        // POST: StudentPage/Create
        [HttpPost]
        public IActionResult Create(Student NewStudent)
        {
            int StudentId = _api.AddStudent(NewStudent);

            // redirects to "Show" action on "Student" cotroller with id parameter supplied
            return RedirectToAction("Show", new { id = StudentId });
        }

        // GET : StudentPage/DeleteConfirm/{id}
        [HttpGet]
        public IActionResult DeleteConfirm(int id)
        {
            Student SelectedStudent = _api.FindStudent(id);
            return View(SelectedStudent);
        }

        // POST: StudentPage/Delete/{id}
        [HttpPost]
        public IActionResult Delete(int id)
        {
            int StudentId = _api.DeleteStudent(id);
            // redirects to list action
            return RedirectToAction("List");
        }
    }
}