using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using datamodel;
using Services;
using Microsoft.EntityFrameworkCore;


namespace libbook.Controllers
{
    public class LendingBookController : Controller
    {
        Services.Book bookServ = new Services.Book();
        Services.Student studentServ = new Services.Student();
        Services.LendingBook m_lending = new Services.LendingBook();
        //datamodel.Entities2 db=new Entities2();
       
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult Index(int? idStudent, int? idBook)
        {
            if (idStudent.HasValue)
            {
                ViewBag.Student = studentServ.GetStudentById(idStudent.Value);
            }
            if (idBook.HasValue)
            {
                ViewBag.Book = bookServ.GetById(idBook.Value).GUID;
            }
            return View();
        }

        //public ActionResult LendStudent(int studentId)
        //{
        //    var rb = new ReaderBook();
        //    var student = db.Students.FirstOrDefault(s => s.Id == studentId);
        //    //rb.Student = student;
        //    TempData["stId"] = studentId;

        //    return View(rb);
        //}

        // [HttpGet]
        //public ActionResult LendBook(int bookId)
        // {
        //     var lend = new ReaderBook();

        //     int studentId = Convert.ToInt32(TempData["stId"]);

        //     var book = bookServ.GetById(4);
        //     var student = db.vStudents.FirstOrDefault(p=>p.Id==2);

        //     lend.BookId = book.Id;
        //    // lend.Book = book;
        //     lend.ReaderId = student.Id;


        //     return View(lend);
        // }


        //[HttpPost]
        //public ActionResult LendBook(datamodel.ReaderBook reader)
        //{
        //    //var book = db.Books.FirstOrDefault(b=>b.Id==reader.Book.Id);
        //    //var student = db.Students.Find(reader.ReaderId);

        //    //var readers = new List<ReaderBook>();
        //    //if (reader.CounOfBooks > 0)
        //    //{
        //    //    var books = db.Books.Where(b => b.BookName == book.BookName && b.IsReserved==false);

        //    //    if (books.Count() >= reader.CounOfBooks)
        //    //    {
        //    //        foreach(var b in books)
        //    //        {
        //    //            b.IsReserved = true;
        //    //            db.Entry(b).State = System.Data.Entity.EntityState.Modified;
        //    //            var r = new ReaderBook() { DateOfTaking = DateTime.Now, Book=b,ReaderId=student.Id,Student=student,BookId=b.Id};
        //    //            readers.Add(r);

        //    //        }
        //    //        db.ReaderBooks.AddRange(readers);
        //    //        db.SaveChanges();
        //    //        return RedirectToAction("Index");
        //    //    }
        //    //    else
        //    //    {
        //    //        TempData["message"] = "Недостаточно книг в базе!";
        //    //        return Redirect(Request.UrlReferrer.ToString());

        //    //    }

        //    //}
        //    //else
        //    //{
        //    //    TempData["message"] = "Выберите число книг больше 0!";
        //    //    return Redirect(Request.UrlReferrer.ToString());
        //    //}



        //}


        public JsonResult LendingBookForStudent(int idSt, int idBook)
        {
            if (idBook <=0)
                return Json(
                new
                {
                    res = "error"
                }, JsonRequestBehavior.AllowGet);
            //если есть книга, а студента нет - студент сдает книгу
            if(idBook >0 && idSt ==0)
            {
                 m_lending.ReturnBook(idBook);
                return Json(
                new
                {
                    res = "ok"
                }, JsonRequestBehavior.AllowGet);
            }
            var book = bookServ.GetById(idBook);
            var student = studentServ.GetStudentById(idSt);

            var r = new ReaderBook() 
            { 
                DateOfTaking = DateTime.Now, 
                ReaderId = student.Id, 
                BookId = book.Id,
                CounOfBooks = 1
            };
            m_lending.LendBook(r);

            return Json(
                new
                {
                    res = "ok"
                }, JsonRequestBehavior.AllowGet);
        }

    }
}