using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Registrar.Models;
using Registrar.ViewModels;
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

        [HttpGet("/students/{id}")]
        public ActionResult Details(int id)
        {
            StudentsCourses allCoursesStudents = new StudentsCourses();
            allCoursesStudents.FindStudent(id);
            return View(allCoursesStudents);
        }

        [HttpPost("/students/{id}/update")]
        public ActionResult Update(int id, int courseId)
        {
            Student student = Student.Find(id);
            Course course = Course.Find(courseId);
            student.AddCourse(course);
            return RedirectToAction("Details");
        }
    }
}
