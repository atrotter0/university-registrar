using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Registrar.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime StartDate { get; set; }

        public Student(string firstName, string lastName, DateTime startDate, int id = 0)
        {
            FirstName = firstName;
            LastName = lastName;
            StartDate = startDate;
            Id = id;
        }

        public override bool Equals(System.Object otherStudent)
        {
            if (!(otherStudent is Student))
            {
                return false;
            }
            else
            {
                Student newStudent = (Student)otherStudent;
                bool idEquality = Id == newStudent.Id;
                bool firstNameEquality = FirstName == newStudent.FirstName;
                bool lastNameEquality = LastName == newStudent.LastName;
                bool startDateEquality = StartDate == newStudent.StartDate;
                return (idEquality && firstNameEquality && lastNameEquality && startDateEquality);
            }
        }


        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO students (first_name, last_name, start_date) VALUES (@firstName, @lastName, @startDate);";

            cmd.Parameters.AddWithValue("@firstName", this.FirstName);
            cmd.Parameters.AddWithValue("@lastName", this.LastName);
            cmd.Parameters.AddWithValue("@startDate", this.StartDate);

            cmd.ExecuteNonQuery();
            Id = (int)cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void Delete()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM students WHERE id = @studentId; DELETE FROM students_courses WHERE student_id = @studentId";
            cmd.Parameters.AddWithValue("@studentId", this.Id);
            cmd.ExecuteNonQuery();
            conn.Close();

            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static List<Student> GetAll()
        {
            List<Student> allStudents = new List<Student> { };
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM students;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while (rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string firstName = rdr.GetString(1);
                string lastName = rdr.GetString(2);
                DateTime startDate = rdr.GetDateTime(3);

                Student newStudent = new Student(firstName, lastName, startDate, id);
                allStudents.Add(newStudent);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allStudents;
        }

        public static Student Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM students WHERE id = (@searchId);";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int studentId = 0;
            string firstName = "";
            string lastName = "";
            DateTime startDate = new DateTime();

            while (rdr.Read())
            {
                studentId = rdr.GetInt32(0);
                firstName = rdr.GetString(1);
                lastName = rdr.GetString(2);
                startDate = rdr.GetDateTime(3);
            }

            Student newStudent = new Student(firstName, lastName, startDate, studentId);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return newStudent;
        }


        public void AddCourse(Course newCourse)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO students_courses (course_id, student_id) VALUES (@courseId, @studentId);";

            cmd.Parameters.AddWithValue("@studentId", this.Id);
            cmd.Parameters.AddWithValue("@courseId", newCourse.Id);

            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void DeleteCourse(Course newCourse)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM students_courses WHERE course_id = @courseId AND student_id = @studentId;";
            cmd.Parameters.AddWithValue("@courseId", newCourse.Id);
            cmd.Parameters.AddWithValue("@studentId", this.Id);

            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public List<Course> GetCourses()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT courses.* FROM students
            JOIN students_courses ON (students.id = students_courses.student_id)
            JOIN courses ON (students_courses.course_id = courses.id)
            WHERE students.id = @studentId;";

            cmd.Parameters.AddWithValue("@studentId", this.Id);

            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<Course> courses = new List<Course> { };

            while (rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string courseName = rdr.GetString(1);
                string courseNumber = rdr.GetString(2);

                Course newCourse = new Course(courseName, courseNumber, id);
                courses.Add(newCourse);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return courses;
        }
    }
}