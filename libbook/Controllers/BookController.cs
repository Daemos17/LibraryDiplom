using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using libbook.ViewModels;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace libbook.Controllers
{
 
    public class BookController : Controller
    {
     
        Services.Book bookServ = new Services.Book();

        private static Logger logger = LogManager.GetLogger("f");

        // GET: Book
        public ActionResult Index(string searchString)
        {

            logger.Info("Пользователь " + "'" + User.Identity.Name + "'" + " перешел на страницу 'Справочник книг'");

            var books = new Services.Book().GetAllBooks();
            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(s => s.BookName.Contains(searchString));
            }
            return View(books);
        }

        public ActionResult GenerateBarCode()
        {
            Byte[] byteArray;

            var list = new List<BarCodeViewModel>();

            var books = bookServ.GetAllBooks();

            foreach (var b in books)
            {
                if (b.GUID != null)
                {
                    var model = new BarCodeViewModel();

                    var pixelData = bookServ.GenerateCode(b.Id);
                    using (var bitmap = new System.Drawing.Bitmap(pixelData.Width, pixelData.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb))
                    {
                        using (var ms = new MemoryStream())
                        {
                            var bitmapData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, pixelData.Width, pixelData.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                            try
                            {
                                // we assume that the row stride of the bitmap is aligned to 4 byte multiplied by the width of the image   
                                System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
                            }
                            finally
                            {
                                bitmap.UnlockBits(bitmapData);
                            }
                            // save to stream as PNG   
                            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                            byteArray = ms.ToArray();
                        }
                    }

                    model.Book = b;
                    model.QrCodeImage = byteArray;

                    list.Add(model);
                }
            }

            return View(list);
        }


        public ActionResult GenerateGuid(int id)
        {
            bookServ.AddGuid(id);

            TempData["message"] = "QR - код успешно сгенерирован";
            return RedirectToAction("Index");

        }



       


        [HttpGet]
        public ActionResult Edit(int id)
        {
            logger.Info("'Справочник книг' Ожидание редактирования книги...");
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
            logger.Info("'Справочник книг' Успешное редактирование!");
            bookServ.EditBook(book);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Add()
        {
            logger.Info("'Справочник книг' Ожидание добавления книги...");
            var authors = bookServ.GetAllAuthors();
            var makers = bookServ.GetAllMakers();
            var categories = bookServ.GetAllCategories();
            var book = new datamodel.Book();


            SelectList author = new SelectList(authors, "Id", "FullName", book.Author_id);
            SelectList maker = new SelectList(makers, "Id_maker", "MakerName", book.Maker_id);
            SelectList category = new SelectList(categories, "Id_category", "CategoryName", book.Category_id);


            ViewBag.Authors = author;
            ViewBag.Categories = category;
            ViewBag.Makers = maker;

            return View(book);

        }




        [HttpPost]
        public ActionResult Add(datamodel.Book book)
        {
            logger.Info("'Справочник книг' Успешное добавление!");
            bookServ.CreateBook(book);


            return RedirectToAction("Index");
        }



        public ActionResult Delete(int id)
        {
            logger.Info("'Справочник книг' Ожидание удаления книги...");
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
