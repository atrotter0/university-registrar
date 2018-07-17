using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Registrar.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string CourseName { get; set; }
        public string CourseNumber { get; set; }

        public Course(string courseName, string courseNumber, int id = 0)
        {
            CourseName = courseName;
            CourseNumber = courseNumber;
            Id = id;
        }


        public override bool Equals(System.Object otherCourse)
        {
            if (!(otherCourse is Course))
            {
                return false;
            }
            else
            {
                Course newCourse = (Course)otherCourse;
                bool idEquality = Id == newCourse.Id;
                bool courseNameEquality = CourseName == newCourse.CourseName;
                bool courseNumberEquality = CourseNumber == newCourse.CourseNumber;
                return (idEquality && courseNameEquality && courseNumberEquality);
            }
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO courses (course_name, course_number) VALUES (@courseName, @courseNumber);";

            cmd.Parameters.AddWithValue("@courseName", this.CourseName);
            cmd.Parameters.AddWithValue("@courseNumber", this.CourseNumber);

            cmd.ExecuteNonQuery();
            Id = (int)cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static List<Course> GetAll()
        {
            List<Course> allCourses = new List<Course> { };
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM courses;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while (rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string courseName = rdr.GetString(1);
                string courseNumber = rdr.GetString(2);


                Course newCourse = new Course(courseName, courseNumber, id);
                allCourses.Add(newCourse);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allCourses;
        }

        public static Course Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM courses WHERE id = (@searchId);";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int courseId = 0;
            string courseName = "";
            string courseNumber = "";

            while (rdr.Read())
            {
                courseId = rdr.GetInt32(0);
                courseName = rdr.GetString(1);
                courseNumber = rdr.GetString(2);
            }

            Course newCourse = new Course(courseName, courseNumber, courseId);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return newCourse;
        }


        public void AddStudent(Student newStudent)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO students_courses (course_id, student_id) VALUES (@courseId, @studentId);";

            cmd.Parameters.AddWithValue("@courseId", this.Id);
            cmd.Parameters.AddWithValue("@studentId", newStudent.Id);

            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void DeleteStudent(Student newStudent)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM students_courses WHERE course_id = @courseId AND student_id = @studentId;";
            cmd.Parameters.AddWithValue("@courseId", this.Id);
            cmd.Parameters.AddWithValue("@studentId", newStudent.Id);

            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public List<Student> GetStudents()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT students.* FROM courses
            JOIN students_courses ON (courses.id = students_courses.course_id)
            JOIN students ON (students_courses.student_id = students.id)
            WHERE courses.id = @courseId;";

            cmd.Parameters.AddWithValue("@courseId", this.Id);

            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<Student> students = new List<Student> { };

            while (rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string firstName = rdr.GetString(1);
                string lastName = rdr.GetString(2);
                DateTime time = rdr.GetDateTime(3);

                Student newStudent = new Student(firstName, lastName, time, id);
                students.Add(newStudent);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return students;
        }
    }
}
