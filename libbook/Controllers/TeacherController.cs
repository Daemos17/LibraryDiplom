using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using datamodel;
using NLog;

namespace libbook.Controllers
{
    public class TeacherController : Controller
    {
        Services.Teacher m_teacher = new Services.Teacher();

        private static Logger logger = LogManager.GetLogger("f");

        public ActionResult Index()
        {
            logger.Info("Пользователь " + "'" + User.Identity.Name + "'" + " перешел на страницу 'Справочник преподавателей'");

            var teachers = m_teacher.GetAllTeachers();
            return View(teachers);
        }
        //автозаполнение на форме
        public JsonResult Autocomplete(string term)
        {
            var teacher = m_teacher.GetTeacher(term);
            return Json(
                new
                {
                    teacher = teacher
                }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add()
        {
            logger.Info("'Справочник преподавателей' Ожидание добавления преподавателя...");

            return View();
        }
        public ActionResult AddTeacher(datamodel.vTeacher teacher)
        {
            logger.Info("'Справочник преподавателей' Пользователь " + "'" + User.Identity.Name + "'" + " добавил преподавателя: " + "'" + teacher.FirstName + " " + teacher.SecondName + " " + teacher.LastName + "'" + ". ");
            int id = m_teacher.AddTeacher(teacher);
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            logger.Info("'Справочник преподавателей' Ожидание удаления преподавателя...");
            ViewBag.TeacherId = id;
            return View();
        }
        public ActionResult Edit(int id)
        {
            logger.Info("'Справочник преподавателей' Ожидание редактирования преподавателя...");
            var teacher = m_teacher.GetTeacherById(id);
            return View(teacher);
        }
        public ActionResult EditTeacher(vTeacher teacher)
        {
            logger.Info("'Справочник преподавателей' Пользователь " + "'" + User.Identity.Name + "'" + " редактировал преподавателя: " + "'" + teacher.FirstName + " " + teacher.SecondName + " " + teacher.LastName + "'" + ". ");
            m_teacher.EditTeacher(teacher);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public JsonResult DeleteTeacher(int id)
        {
            logger.Info("'Справочник преподавателей' Пользователь " + "'" + User.Identity.Name + "'" + " удалил преподавателя");
            m_teacher.DeleteTeacher(id);
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
                        vTeacher teacher = new vTeacher
                        {
                            FirstName = parts[0],
                            SecondName = parts[1],
                            LastName = parts.Length >= 3 ? parts[2] : "",
                            Comment = parts.Length >= 4 ? parts[3] : ""
                        };
                        m_teacher.AddTeacher(teacher);
                    }
                }
            }
            return RedirectToAction("Index");
        }
    }
}
