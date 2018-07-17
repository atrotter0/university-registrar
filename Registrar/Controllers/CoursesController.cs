using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Registrar.Models;
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

        
    }
}
