using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using SchoolChallenge.Models;
using SchoolChallenge.ViewModels;

namespace SchoolChallenge.Controllers
{
    [Route("[controller]/[action]")]
    public class StudentsController : Controller
    {
        public IActionResult Index()
        {
            var students = LoadStudents().ToList();
            return View(students);
        }

        public IActionResult Add()
        {
            var student = new Student();
            var viewModel = new StudentFormViewModel
            {
                Student = student
            };

            return View("StudentForm", viewModel);
        }

        public IActionResult Edit(int id)
        {
            var student = LoadStudent(id);

            var viewModel = new StudentFormViewModel
            {
                Student = student
            };

            return View("StudentForm", viewModel);
        }

        public IActionResult Save(Student student)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new StudentFormViewModel
                {
                    Student = student
                };
                return View("StudentForm", viewModel);
            }

            if (student.Id == 0)
            {

                var sqlStudentList = new List<Student>();
                sqlStudentList = LoadStudents();

                if (!sqlStudentList.Any(x => x.FirstName == student.FirstName && x.LastName == student.LastName &&
                                             x.StudentNumber == student.StudentNumber))
                {
                    InsertStudent(student);
                }
                else
                {
                    return RedirectToAction("Index", "Students");
                }

            }
            else
            {
                UpdateStudent(student);
            }

            return RedirectToAction("Index", "Students");
        }

        public IActionResult Import()
        {
            string filePath = @"C:\SampleData\Interview-Data-Students.csv";
            if (!(System.IO.File.Exists(filePath)))
            {
                return RedirectToAction("Index", "Students");
            }
            else
            {
                ImportCSV(filePath);
            }

            return RedirectToAction("Index", "Students");
        }

        // method to load all students from the database
        public List<Student> LoadStudents()
        {
            var studentList = new List<Student>();

            using (var connection =
                new SqlConnection(
                    @"Data Source=(LOCALDB)\MSSQLLocalDB;Initial Catalog=SchoolChallengeDB;Trusted_Connection=True;MultipleActiveResultSets=true;")
            )
            {
                connection.Open();
                string sql = "Select * from Students";
                using (var command = new SqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var student = new Student();
                            student.Id = Int32.Parse(reader["Id"].ToString());
                            student.StudentNumber = reader["StudentNumber"].ToString();
                            student.FirstName = reader["FirstName"].ToString();
                            student.LastName = reader["LastName"].ToString();
                            student.HasScholarship = Boolean.Parse(reader["HasScholarship"].ToString());

                            studentList.Add(student);
                        }
                    }
                }
            }
            return studentList;
        }

        public void InsertStudent(Student student)
        {
            using (var connection =
                new SqlConnection(
                    @"Data Source=(LOCALDB)\MSSQLLocalDB;Initial Catalog=SchoolChallengeDB;Trusted_Connection=True;MultipleActiveResultSets=true;")
            )
            {
                string sql = "Insert into Students " +
                             "(StudentNumber, FirstName, LastName, HasScholarship) " +
                             "Values ('" +
                             student.StudentNumber.ToString() + "','" +
                             student.FirstName.ToString() + "','" +
                             student.LastName.ToString() + "','" +
                             student.HasScholarship.ToString() + "');";

                var command = new SqlCommand();
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = sql;
                command.Connection = connection;

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        // update a student in the database
        public void UpdateStudent(Student student)
        {
            using (var connection =
                new SqlConnection(
                    @"Data Source=(LOCALDB)\MSSQLLocalDB;Initial Catalog=SchoolChallengeDB;Trusted_Connection=True;MultipleActiveResultSets=true;")
            )
            {
                string sql = "Update Students Set " +
                             "StudentNumber='" + student.StudentNumber.ToString() + "'," +
                             "FirstName='" + student.FirstName.ToString() + "'," +
                             "LastName='" + student.LastName.ToString() + "'," +
                             "HasScholarship='" + student.HasScholarship.ToString() + "' where id =" +
                             student.Id.ToString();

                var command = new SqlCommand();
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = sql;
                command.Connection = connection;

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        // load one student form the database
        public Student LoadStudent(int id)
        {
            var student = new Student();

            using (var connection =
                new SqlConnection(
                    @"Data Source=(LOCALDB)\MSSQLLocalDB;Initial Catalog=SchoolChallengeDB;Trusted_Connection=True;MultipleActiveResultSets=true;")
            )
            {
                connection.Open();
                string sql = "Select * from Students where Id =" + id.ToString();
                using (var command = new SqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            student.Id = Int32.Parse(reader["Id"].ToString());
                            student.StudentNumber = reader["StudentNumber"].ToString();
                            student.FirstName = reader["FirstName"].ToString();
                            student.LastName = reader["LastName"].ToString();
                            student.HasScholarship = Boolean.Parse(reader["HasScholarship"].ToString());

                        }
                    }
                }
            }
            return student;
        }

        // method to import items from csv in the database.
        // method will only import entries that aren't in the database
        public void ImportCSV(string filePath)
        {

            var lines = System.IO.File.ReadAllLines(filePath).Skip(1);
            var csvStudent = new Student();

            foreach (string item in lines)
            {
                var values = item.Split(',');

                csvStudent.StudentNumber = values[1];
                csvStudent.FirstName = values[2];
                csvStudent.LastName = values[3];

                if (values[4] == "yes")
                {
                    csvStudent.HasScholarship = true;
                }
                else
                {
                    csvStudent.HasScholarship = false;
                }

                var sqlStudentList = new List<Student>();
                sqlStudentList = LoadStudents();

                if (!sqlStudentList.Any(x => x.FirstName == csvStudent.FirstName && x.LastName == csvStudent.LastName &&
                                             x.StudentNumber == csvStudent.StudentNumber))
                {
                    InsertStudent(csvStudent);
                }
                else
                {
                    continue;
                }

            }
        }
    }
}
