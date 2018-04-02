using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Chunks;
using SchoolChallenge.Models;
using SchoolChallenge.ViewModels;

namespace SchoolChallenge.Controllers
{
    [Route("[controller]/[action]")]
    public class TeachersController : Controller
    {
        public IActionResult Index()
        {
            var teachers = LoadTeachers().ToList();
            return View(teachers);
        }

        public IActionResult Add()
        {
            var teacher = new Teacher();
            var viewModel = new TeacherFormViewModel
            {
                Teacher = teacher
            };

            return View("TeacherForm", viewModel);
        }

        public IActionResult Edit(int id)
        {
            var teacher = LoadTeacher(id);

            var viewModel = new TeacherFormViewModel
            {
                Teacher = teacher
            };

            return View("TeacherForm", viewModel);
        }

        public IActionResult Save(Teacher teacher)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new TeacherFormViewModel
                {
                    Teacher = teacher
                };
                return View("TeacherForm", viewModel);
            }

            if (teacher.Id == 0)
            {

                var sqlTeacherList = new List<Teacher>();
                sqlTeacherList = LoadTeachers();

                if (!sqlTeacherList.Any(x => x.FirstName == teacher.FirstName && x.LastName == teacher.LastName))
                {
                    InsertTeacher(teacher);
                }
                else
                {
                    return RedirectToAction("Index", "Teachers");
                }

            }
            else
            {
                UpdateTeacher(teacher);
            }

            return RedirectToAction("Index", "Teachers");
        }

        public IActionResult Import()
        {
            string filePath = @"C:\SampleData\Interview-Data-Teachers.csv";
            if (!(System.IO.File.Exists(filePath)))
            {
                return RedirectToAction("Index", "Teachers");
            }
            else
            {
                ImportCSV(filePath);
            }

            return RedirectToAction("Index", "Teachers");
        }

        // method to load all students from the database
        public List<Teacher> LoadTeachers()
        {
            var teacherList = new List<Teacher>();

            using (var connection =
                new SqlConnection(
                    @"Data Source=(LOCALDB)\MSSQLLocalDB;Initial Catalog=SchoolChallengeDB;Trusted_Connection=True;MultipleActiveResultSets=true;")
            )
            {
                connection.Open();
                string sql = "Select * from Teachers";
                using (var command = new SqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var teacher = new Teacher();
                            teacher.Id = Int32.Parse(reader["Id"].ToString());
                            teacher.FirstName = reader["FirstName"].ToString();
                            teacher.LastName = reader["LastName"].ToString();
                            if (teacher.NumberOfStudents == null)
                            {
                                teacher.NumberOfStudents = 0;
                            }
                            else
                            {
                                teacher.NumberOfStudents = Int32.Parse(reader["NumberOfStudents"].ToString());
                            }


                            teacherList.Add(teacher);
                        }
                    }
                }
            }
            return teacherList;
        }

        public void InsertTeacher(Teacher teacher)
        {
            using (var connection =
                new SqlConnection(
                    @"Data Source=(LOCALDB)\MSSQLLocalDB;Initial Catalog=SchoolChallengeDB;Trusted_Connection=True;MultipleActiveResultSets=true;")
            )
            {
                string sql = "Insert into Teachers " +
                             "(FirstName, LastName) " +
                             "Values ('" +
                             teacher.FirstName.ToString() + "','" +
                             teacher.LastName.ToString() + "');";

                var command = new SqlCommand();
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = sql;
                command.Connection = connection;

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        // update a teracher in the database
        public void UpdateTeacher(Teacher teacher)
        {
            using (var connection =
                new SqlConnection(
                    @"Data Source=(LOCALDB)\MSSQLLocalDB;Initial Catalog=SchoolChallengeDB;Trusted_Connection=True;MultipleActiveResultSets=true;")
            )
            {
                string sql = "Update Teachers Set " +
                             "FirstName='" + teacher.FirstName.ToString() + "'," +
                             "LastName='" + teacher.LastName.ToString() + "'," +
                             "NumberOfStudents='" + teacher.NumberOfStudents.ToString() +
                             "' where id =" +
                             teacher.Id.ToString();

                var command = new SqlCommand();
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = sql;
                command.Connection = connection;

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        // load one teacher form the database
        public Teacher LoadTeacher(int id)
        {
            var teacher = new Teacher();

            using (var connection =
                new SqlConnection(
                    @"Data Source=(LOCALDB)\MSSQLLocalDB;Initial Catalog=SchoolChallengeDB;Trusted_Connection=True;MultipleActiveResultSets=true;")
            )
            {
                connection.Open();
                string sql = "Select * from Teachers where Id =" + id.ToString();
                using (var command = new SqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            teacher.Id = Int32.Parse(reader["Id"].ToString());
                            teacher.FirstName = reader["FirstName"].ToString();
                            teacher.LastName = reader["LastName"].ToString();
                            if (teacher.NumberOfStudents == null)
                            {
                                teacher.NumberOfStudents = 0;
                            }
                            else
                            {
                                teacher.NumberOfStudents = Int32.Parse(reader["NumberOfStudents"].ToString());
                            }

                        }
                    }
                }
            }
            return teacher;
        }

        // method to import items from csv in the database.
        // method will only import entries that aren't in the database
        public void ImportCSV(string filePath)
        {

            var lines = System.IO.File.ReadAllLines(filePath).Skip(1);
            var csvTeacher = new Teacher();

            foreach (string item in lines)
            {
                var values = item.Split(',');

                csvTeacher.FirstName = values[1];
                csvTeacher.LastName = values[2];

                var sqlTeacherList = new List<Teacher>();
                sqlTeacherList = LoadTeachers();

                if (!sqlTeacherList.Any(x => x.FirstName == csvTeacher.FirstName && x.LastName == csvTeacher.LastName))
                {
                    InsertTeacher(csvTeacher);
                }
                else
                {
                    continue;
                }

            }
        }
    }
}