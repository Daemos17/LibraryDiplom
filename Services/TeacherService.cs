using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using datamodel;

namespace Services
{
    //преподаватели
    public class TeacherService
    {
        Entities db = new Entities();
        public IEnumerable<vTeacher> GetAllTeachers()
        {
            return db.vTeachers;
        }
    }
}
