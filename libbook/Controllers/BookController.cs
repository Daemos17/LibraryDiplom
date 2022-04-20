using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wangkanai.Detection;

namespace libbook.Controllers
{
    public class BookController : Controller
    {
        Services.Book bookServ = new Services.Book();
       


        // GET: Book
        public ActionResult Index(string searchString)
        {
            var books = new Services.Book().GetAllBooks();
            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(s => s.BookName.Contains(searchString));
            }
            return View(books);
        }


       


        [HttpGet]
        public ActionResult Edit(int id)
        {

            var authors = bookServ.GetAllAuthors();
            var makers = bookServ.GetAllMakers();


            var book = bookServ.GetById(id);
            SelectList author = new SelectList(authors, "Id", "FullName", book.Author_id);
            SelectList maker = new SelectList(makers, "Id_maker", "MakerName", book.Maker_id);

            ViewBag.Authors = author;
            ViewBag.Makers = maker;

            return View(book);
        }

        [HttpPost]
        public ActionResult Edit(datamodel.Book book)
        {
            bookServ.EditBook(book);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Add()
        {
            var authors = bookServ.GetAllAuthors();
            var makers = bookServ.GetAllMakers();
            var book = new datamodel.Book();


            SelectList author = new SelectList(authors, "Id", "FullName", book.Author_id);
            SelectList maker = new SelectList(makers, "Id_maker", "MakerName", book.Maker_id);

            ViewBag.Authors = author;
            ViewBag.Makers = maker;

            return View(book);

        }




        [HttpPost]
        public ActionResult Add(datamodel.Book book)
        {

            bookServ.CreateBook(book);


            return RedirectToAction("Index");
        }



        public ActionResult Delete(int id)
        {
            if (id != null)
            {
                datamodel.Book book = bookServ.GetById(id);
                if (book != null)
                {
                    bookServ.DeleteBook(id);

                    return RedirectToAction("Index");
                }
            }
            return HttpNotFound();
        }

        public JsonResult Autocomplete(string term)
        {
            var books = new Services.Book().GetBooks(term);
            return Json(
                new
                {
                    books = books
                }, JsonRequestBehavior.AllowGet);
        }
    }
}
