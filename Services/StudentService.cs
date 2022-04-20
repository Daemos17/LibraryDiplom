using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using datamodel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
//using System.Data.Entity;

namespace Services
{
    //студенты
    public class StudentService
    {
        Entities db = new Entities();

        //public  ActionResult test() { }
        /// <summary>
        /// Возвращает список студетов для поиска
        /// </summary>
        /// <param name="term">фраза для поиска</param>
        /// <returns></returns>
        public List<vStudent> GetStudent(string term)
        {
            
            var t = db.vStudents.Where(p=>p.FirstName.StartsWith(term)).ToList();
            if (t==null)
            {
                throw new ArgumentNullException();
            }
            //Dictionary<int, string> students = new Dictionary<int, string>();
            //foreach(var student in t)
            //{
            //    string fullname = student.FirstName.TrimEnd(' ') + " " + student.SecondName.TrimEnd(' ') + " " + student.LastName.TrimEnd(' ');
            //    students.Add(student.Id,fullname);
            //}
            return t;
        }

        /// <summary>
        /// Получение студента по id
        /// </summary>
        /// <param name="id">номер</param>
        /// <returns></returns>
        public vStudent GetStudentById(int id)
        {
            return db.vStudents.FirstOrDefault(p => p.Id == id);
        }
    }
}
