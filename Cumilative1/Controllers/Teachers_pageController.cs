using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cumilative1.Data;
using Cumilative1.Models;

namespace Cumilative1.Controllers
{
    public class Teachers_pageController : Controller
    {

        // get access to the component which retrieves data
        // the component is the TeacherAPIController
        private readonly TeacherAPIController _api;

        public Teachers_pageController(TeacherAPIController api)
        {
            _api = api;
        }

        // GET: /TeacherPage/List -> A webpage which shows all teachers in the system
        [HttpGet]
        public IActionResult List(string SearchKey)
        {
            // get this information from the API
            List<Teacher> Teachers = _api.ListTeachers(SearchKey); // Assuming ListTeacherNames method returns a list of Teacher objects

            // Return the list of teachers to the View
            return View(Teachers);
        }


        // GET: /TeacherPage/Show/{id} -> A webpage which shows one specific teacher by its id
        [HttpGet]
        public IActionResult Show(int id)
        {
            // Get specific teacher details
            Teacher SelectedTeacher = _api.FindTeacher(id);

            // Return the teacher details to the View
            return View(SelectedTeacher);
        }


        // GET : TeacherPage/New
        [HttpGet]
        public IActionResult New(int id)
        {
            return View();
        }

        // POST: TeacherPage/Create
        [HttpPost]
        public IActionResult Create(Teacher NewTeacher)
        {
            int TeacherId = _api.AddTeacher(NewTeacher);

            // redirects to "Show" action on "Teacher" cotroller with id parameter supplied
            return RedirectToAction("Show", new { id = TeacherId });
        }

        // GET : TeacherPage/DeleteConfirm/{id}
        [HttpGet]
        public IActionResult DeleteConfirm(int id)
        {
            Teacher SelectedTeacher = _api.FindTeacher(id);
            return View(SelectedTeacher);
        }

        // POST: TeacherPage/Delete/{id}
        [HttpPost]
        public IActionResult Delete(int id)
        {
            int TeacherId = _api.DeleteTeacher(id);
            // redirects to list action
            return RedirectToAction("List");
        }

        // GET : TeacherPage/Edit/{id}
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Teacher SelectedTeacher = _api.FindTeacher(id);
            return View(SelectedTeacher);
        }


        // POST: TeacherPage/Update/{id}
        [HttpPost]
        public IActionResult Update(int id, string TeacherFirstName, string TeacherLastName, string EmpNumber, DateTime HireDate, decimal Salary)
        {
            Teacher UpdatedTeacher = new Teacher();
            UpdatedTeacher.TeacherFirstName = TeacherFirstName;
            UpdatedTeacher.TeacherLastName = TeacherLastName;
            UpdatedTeacher.EmpNumber = EmpNumber;
            UpdatedTeacher.HireDate = HireDate;
            UpdatedTeacher.Salary = Salary;



            // not doing anything with the response
            _api.UpdateTeacher(id, UpdatedTeacher);
            // redirects to show teacher
            return RedirectToAction("Show", new { id = id });
        }
    }
}