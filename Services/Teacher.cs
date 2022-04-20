using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extentions;
using datamodel;

namespace Services
{
    public class Teacher
    {
        Entities2 db = new Entities2();

        /// <summary>
        /// Возвращает список преподавателей для поиска
        /// </summary>
        /// <param name="term">фраза для поиска</param>
        /// <returns></returns>
        public List<vTeacher> GetTeacher(string term)
        {
            if (term.IsNullOrEmptyOrWhitespace())
            {
                return null;
            }

            var parts = term.Split(new char[] { ' ', '.' },
                             StringSplitOptions.RemoveEmptyEntries);

            var teachers = db.vTeachers.AsEnumerable();

            foreach (var part in parts)
            {
                teachers = teachers.Where(
                    p => p.FirstName.ToLower().Contains(part) ||
                    p.LastName.ToLower().Contains(part) ||
                    p.SecondName.ToLower().Contains(part)
                    );
            }

            return teachers.ToList();
        }

        /// <summary>
        /// Получение преподавателя по id
        /// </summary>
        /// <param name="id">номер</param>
        /// <returns></returns>
        public vTeacher GetTeacherById(int id)
        {
            return db.vTeachers.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<vTeacher> GetAllTeachers()
        {
            return db.vTeachers;
        }

        public void EditTeacher(vTeacher teacher)
        {
            User user = db.Users.Find(teacher.Id);
            user.FirstName = teacher.FirstName;
            user.SecondName = teacher.SecondName;
            user.LastName = teacher.LastName;

            var teach = db.Teachers.FirstOrDefault(p => p.User_id == teacher.Id);
            teach.Comment = teacher.Comment;
            db.SaveChanges();
        }

        public int AddTeacher(vTeacher teacher)
        {
            User user = new User
            {
                FirstName = teacher.FirstName,
                SecondName = teacher.SecondName,
                LastName = teacher.LastName
            };

            db.Users.Add(user);
            db.SaveChanges();

            datamodel.Teacher teach = new datamodel.Teacher
            {
                Comment = teacher.Comment,
                User_id = user.Id
            };

            db.Teachers.Add(teach);
            db.SaveChanges();

            return teach.Id;
        }

        public void DeleteTeacher(int id)
        {
            var user = db.Users.Find(id);
            if (user != null)
            {
                db.Users.Remove(user);
                db.Teachers.Remove(db.Teachers.FirstOrDefault(p => p.User_id == id));
                db.SaveChanges();
            }
        }
    }
}
