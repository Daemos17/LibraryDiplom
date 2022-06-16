using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using datamodel;
using Services;
using Microsoft.EntityFrameworkCore;
using NLog;


namespace libbook.Controllers
{
   
    public class LendingBookController : Controller
    {
        Services.Book bookServ = new Services.Book();

        datamodel.Entities2 db=new Entities2();


        private static Logger logger = LogManager.GetLogger("f");

        // GET: Student
        public ActionResult Index()
        {
            logger.Info("Пользователь " + "'" + User.Identity.Name + "'" + " перешел на страницу 'Выдача книг'");
            return View();
           
        }


        public ActionResult LendStudent(int studentId)
        {
            var rb = new ReaderBook();
            var student = db.Students.FirstOrDefault(s => s.Id == studentId);
            rb.Student = student;
            TempData["stId"] = studentId;

            return View(rb);
        }

        [HttpGet]
       public ActionResult LendBook(int bookId)
        {
            var lend = new ReaderBook();

            int studentId = Convert.ToInt32(TempData["stId"]);

            var book = bookServ.GetById(bookId);
            var student = db.vStudents.FirstOrDefault(p=>p.Id==studentId);

            lend.BookId = book.Id;
            lend.Book = book;
            lend.ReaderId = student.Id;


            return View(lend);
        }


        [HttpPost]
        public ActionResult LendBook(datamodel.ReaderBook reader)
        {
            var book = db.Books.FirstOrDefault(b=>b.Id==reader.Book.Id);
            var student = db.Students.Find(reader.ReaderId);
            string ms = "";
            var readers = new List<ReaderBook>();
            if (reader.CounOfBooks > 0)
            {
                var books = db.Books.Where(b => b.BookName == book.BookName && b.IsReserved==false);

                if (books.Count() >= reader.CounOfBooks)
                {
                    foreach(var b in books)
                    {
                        b.IsReserved = true;
                        db.Entry(b).State = System.Data.Entity.EntityState.Modified;
                        var r = new ReaderBook() { DateOfTaking = DateTime.Now, Book=b,ReaderId=student.Id,Student=student,BookId=b.Id};
                        readers.Add(r);
                        ms += "\n" + b.InventoryNum + " ";
                    }
                    db.ReaderBooks.AddRange(readers);
                    db.SaveChanges();
                    TempData["message"] = "Выдайте книги с номерами: "+ms;
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["message"] = "Недостаточно книг в базе!";
                    return Redirect(Request.UrlReferrer.ToString());
                   
                }

            }
            else
            {
                TempData["message"] = "Выберите число книг больше 0!";
                return Redirect(Request.UrlReferrer.ToString());
            }


           
        }
    

    }
}