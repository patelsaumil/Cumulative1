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
        public IActionResult List()
        {
            // get this information from the API
            List<string> TeacherList = _api.ListAllTeacher(); // Assuming ListTeacherNames method returns a list of Teacher objects

            // Return the list of teachers to the View
            return View(TeacherList);
        }

        // GET: /TeacherPage/Show/{id} -> A webpage which shows one specific teacher by its id
        [HttpGet]
        public IActionResult Show(int id)
        {
            // Get specific teacher details
            Teacher SelectedTeacher = _api.FindTeacherID(id);

            // Return the teacher details to the View
            return View(SelectedTeacher);
        }
    }
}