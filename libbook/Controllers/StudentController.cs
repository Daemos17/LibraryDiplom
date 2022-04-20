using datamodel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace libbook.Controllers
{
    public class StudentController : Controller
    {
        Services.Student m_student = new Services.Student(); 
        public ActionResult Index()
        {
            var student = m_student.GetAllStudent();
            return View(student);
        }
        public JsonResult Autocomplete(string term)
        {
            var student = m_student.GetStudent(term);
            return Json(
                new
                {
                    student = student
                }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add()
        {
            return View();
        }

        public ActionResult AddTeacher(vStudent student)
        {
            int id = m_student.AddStudent(student);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            ViewBag.StudentId = id;
            return View();
        }

        public ActionResult Edit(int id)
        {
            var teacher = m_student.GetStudentById(id);
            return View(teacher);
        }

        public ActionResult EditTeacher(vStudent student)
        {
            m_student.EditStudent(student);
            return RedirectToAction("Index");

        }
        [HttpPost]
        public JsonResult DeleteStudent(int id)
        {
            m_student.DeleteStudent(id);
            return Json(true);
        }

        public ActionResult LoadList()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload()
        {
            if (Request.Files.Count > 0)
            {
                string fileName = System.IO.Path.GetFileName(Request.Files[0].FileName);
                //
                StreamReader stream = new StreamReader(Request.Files[0].InputStream);
                string x = stream.ReadToEnd();
                string[] lines = x.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    var parts = line.Split(';');

                    if (parts.Length >= 2)
                    {
                        vStudent student = new vStudent
                        {
                            FirstName = parts[0],
                            SecondName = parts[1],
                            LastName = parts.Length >= 3 ? parts[2] : "",
                            
                        };
                        m_student.AddStudent(student);
                    }
                }
            }

            return RedirectToAction("Index");
        }
    }
}