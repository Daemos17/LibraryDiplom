using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using datamodel;
using ZXing;
using ZXing.QrCode;
using ZXing.Rendering;

namespace Services
{
    public class Book
    {
        datamodel.Entities2 db = new datamodel.Entities2();

        public IEnumerable<datamodel.Book> GetAllBooks()
        {
            //var m_books = db.Books;
            //формировать строку из автора книги + название + год
            //db.Books.Load(); vBooks
            return db.Books;
        }

        public IEnumerable<datamodel.Author> GetAllAuthors()
        {
            
            return db.Authors;
        }

        public IEnumerable<datamodel.Maker> GetAllMakers()
        {
            
            return db.Makers;
        }

        public IEnumerable<datamodel.Category> GetAllCategories()
        {

            return db.Categories;
        }

        public  void CreateBook(datamodel.Book book)
        {
         
            var newBook = new datamodel.Book()
            {
                Author_id = book.Author_id,
                BookName = book.BookName,
                Maker_id = book.Maker_id,
                Year = book.Year,
                InventoryNum = "ИНВ №"+book.InventoryNum,
                Comment = book.Comment,        
                IsReserved=false,
                Category_id=book.Category_id
                
            };
            db.Books.Add(newBook);

         db.SaveChangesAsync();
        }

        public PixelData GenerateCode(int bookId)
        {
        
            var book = db.Books.Find(bookId);

            if (book != null)
            {
                var qrCodeWriter = new ZXing.BarcodeWriterPixelData
                {
                    Format = ZXing.BarcodeFormat.QR_CODE,
                    Options = new QrCodeEncodingOptions
                    {
                        Height = 250,
                        Width = 250,
                        Margin = 0
                    }
                };

                var result = qrCodeWriter.Write(book.GUID);
                
                return result;
            }

            return null;

           
        }


        public void AddGuid(int id)
        {
            var book = db.Books.Find(id);
            var guid = Guid.NewGuid().ToString();
            book.GUID = guid;
            db.Entry(book).State = EntityState.Modified;
            db.SaveChanges();

        }


        public datamodel.Book GetById(int id)
        {
            return db.Books.FirstOrDefault(p => p.Id == id);

        }

        public datamodel.Book FindByGuid(string guid)
        {
            var book = db.Books.FirstOrDefault(g => g.GUID == guid);
            if (book != null)
            {
                return book;
            }
            else
            {
                return null;
            }
        }


        public List<datamodel.vBook> GetBooks(string term)
        {
            var isGuid = Guid.TryParse(term, out Guid result);
            //если guid
            if (isGuid)
            {
                var t = db.vBooks.FirstOrDefault(p => p.GUID == term);
                //guid нашли в базе, возвратим книгу с этим guid
                if(t!=null)
                {
                    List<datamodel.vBook> lst = new List<datamodel.vBook>();
                    lst.Add(t);
                    return lst;
                }
                //guid не нашли
                else
                {
                    return null;
                }
            }
            else
            {
                var parts = term.Split(new char[] { ' ' },
                                 StringSplitOptions.RemoveEmptyEntries);

                var books = db.vBooks.AsEnumerable();
                List<datamodel.vBook> lst = new List<datamodel.vBook>();
                foreach (var part in parts)
                {
                    lst.AddRange(books.Where(
                        p => p.FirstName.Contains(part) ||
                        p.SecondName.Contains(part) ||
                        p.BookName.Contains(part) ||
                        p.MakerName.Contains(part) ||
                        p.CategoryName.Contains(part)
                        ));
                }
                return lst;
            }
        }


        public void EditBook(datamodel.Book b)
        {
            datamodel.Book book = new datamodel.Book {
                Id=b.Id, 
                Author_id=b.Author_id,
                BookName=b.BookName,
                Maker_id=b.Maker_id,
                Year=b.Year,
                InventoryNum= "ИНВ №"+b.InventoryNum
            };
            db.Entry(book).State = EntityState.Modified;
            db.SaveChanges();
        }



        public void DeleteBook(int? id)
        {
            if (id != null)
            {
                datamodel.Book book =db.Books.FirstOrDefault(p => p.Id == id);
                db.Books.Remove(book);
                db.SaveChanges();
            }
        }
    }
}
