using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using datamodel;
using Extentions;

namespace Services
{
    public class Student:baseService
    {
        datamodel.Entities2 db = new datamodel.Entities2();

        /// <summary>
        /// Возвращает список студетов для поиска
        /// </summary>
        /// <param name="term">фраза для поиска</param>
        /// <returns></returns>
        public List<datamodel.vStudent> GetStudent(string term)
        {
            if (term.IsNullOrEmptyOrWhitespace())
            {
                return null;
            }

            var parts = term.Split(new char[] { ' ', '.' },
                             StringSplitOptions.RemoveEmptyEntries);

            var students = db.vStudents.AsEnumerable();
            List<datamodel.vStudent> lst = new List<vStudent>();
            foreach (var part in parts)
            {
                lst.AddRange(students.Where(
                    p => p.FirstName.Contains(part) ||
                    p.LastName.Contains(part) ||
                    p.SecondName.Contains(part) ||
                    p.GroupFullName.Contains(part) ||
                    p.GroupLittleName.Contains(part)
                    ));
            }

            return lst;
        }

        
        public IEnumerable<vStudent> GetAllStudent()
        {
            return db.vStudents;
        }

        /// <summary>
        /// Получение студента по id
        /// </summary>
        /// <param name="id">номер</param>
        /// <returns></returns>
        public datamodel.vStudent GetStudentById(int id)
        {
            return db.vStudents.FirstOrDefault(p => p.Id == id);
        }


        public void EditStudent(vStudent student)
        {
            datamodel.Student st = db.Students.Find(student.Id);
            st.FirstName = student.FirstName;
            st.SecondName = student.SecondName;
            st.LastName = student.LastName;

            var groupStudent = db.Groups.FirstOrDefault(p => p.GroupLittleName == student.GroupLittleName);
            if (groupStudent != null)
            {
                st.Group_id = groupStudent.Id;
            }
            db.SaveChanges();
        }

        public int AddStudent(vStudent student)
        {
            datamodel.Student st = new datamodel.Student
            {
                FirstName = student.FirstName,
                SecondName = student.SecondName,
                LastName = student.LastName,
            };

            var groupStudent = db.Groups.FirstOrDefault(p => p.GroupLittleName == student.GroupLittleName);
            if (groupStudent != null)
            {
                st.Group_id = groupStudent.Id;
            }
            else
            {
                Group group = new Group
                {
                    GroupLittleName = student.GroupLittleName,
                    GroupFullName = student.GroupFullName
                };

                db.Groups.Add(group);
            }

            db.Students.Add(st);
            db.SaveChanges();

            return st.Id;
        }

        public void DeleteStudent(int id)
        {
            var student = db.Students.Find(id);
            if (student != null)
            {
                db.Students.Remove(student);
                //Если удален последний студент из группы, то удалить группу

                db.SaveChanges();
            }
        }
    }
}
