using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Registrar.Models;
using Microsoft.AspNetCore.Mvc;

namespace Registrar.Controllers
{
    public class StudentsController : Controller
    {
        [HttpGet("/students")]
        public ActionResult Index()
        {
            List<Student> allStudents = Student.GetAll();
            return View(allStudents);
        }

        [HttpGet("/students/create")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost("/students")]
        public ActionResult CreateStudent(string firstName, string lastName, string startDate)
        {
            DateTime newDate = DateTime.Parse(startDate);
            Student newStudent = new Student(firstName, lastName, newDate);
            newStudent.Save();
            return RedirectToAction("Index");
        }
    
    }
}
