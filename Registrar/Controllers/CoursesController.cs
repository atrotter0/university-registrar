using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Registrar.Models;
using Registrar.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Registrar.Controllers
{
    public class CoursesController : Controller
    {

        [HttpGet("/courses")]
        public ActionResult Index()
        {
            List<Course> allCourses = Course.GetAll();
            return View(allCourses);
        }

        [HttpGet("/courses/new")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost("/courses")]
        public ActionResult CreateCourse(string courseName, string courseNumber)
        {
            Course course = new Course(courseName, courseNumber);
            course.Save();
            return RedirectToAction("Index");
        }

        [HttpGet("/courses/{id}")]
        public ActionResult Details(int id)
        {
            StudentsCourses allCoursesStudents = new StudentsCourses();
            allCoursesStudents.FindCourse(id);
            return View(allCoursesStudents);
        }

        [HttpPost("/course/{id}/student/{studentId}/delete")]
        public ActionResult Delete(int id, int studentId)
        {
            Student student = Student.Find(studentId);
            Course course = Course.Find(id);
            course.DeleteStudent(student);
            return RedirectToAction("Details");
        }
        
    }
}
