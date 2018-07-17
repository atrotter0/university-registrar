using System;
using System.Collections.Generic;
using Registrar.Models;

namespace Registrar.ViewModels
{
    public class StudentsCourses
    {
        public List<Course> AllCourses { get; set; }
        public List<Student> AllStudents { get; set; }
        public Student FoundStudent { get; set; }
        public Course FoundCourse { get; set; }

        public StudentsCourses()
        {
            AllCourses = Course.GetAll();
            AllStudents = Student.GetAll();
        }

        public void FindStudent(int id)
        {
            FoundStudent = Student.Find(id);
        }

        public void FindCourse(int id)
        {
            FoundCourse = Course.Find(id);
        }
    }
}
