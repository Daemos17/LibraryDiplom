using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wangkanai.Detection;
using libbook.ViewModels;

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

        public ActionResult GenerateBarCode(int id)
        {
            Byte[] byteArray;

            var pixelData = bookServ.GenerateCode(id);

            using (var bitmap= new System.Drawing.Bitmap(pixelData.Width, pixelData.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb))
            {
                using (var ms=new MemoryStream())
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
          
                return View(new BarCodeViewModel 
                {                 
                    QrCodeImage=byteArray
                });
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
            var categories = bookServ.GetAllCategories();
            var book = new datamodel.Book();


            SelectList author = new SelectList(authors, "Id", "FullName", book.Author_id);
            SelectList maker = new SelectList(makers, "Id_maker", "MakerName", book.Maker_id);
            SelectList category = new SelectList(categories, "Id_category", "CategoryName", book.Author_id);


            ViewBag.Authors = author;
            ViewBag.Categories = category;
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
