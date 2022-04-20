using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using datamodel;

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

        public  void CreateBook(datamodel.Book book)
        {
            var newBook = new datamodel.Book()
            {
                Author_id = book.Author_id,
                BookName = book.BookName,
                Maker_id = book.Maker_id,
                Year = book.Year,
                Count = book.Count,
                Comment=book.Comment
                
            };


            db.Books.Add(newBook);

         db.SaveChangesAsync();
        }



        public datamodel.Book GetById(int id)
        {
            return db.Books.FirstOrDefault(p => p.Id == id);

        }
        public List<datamodel.Book> GetBooks(string term)
        {
            var t = db.Books.Where(p => p.BookName.StartsWith(term));

            return t.ToList();
        }


        public void EditBook(datamodel.Book b)
        {
            datamodel.Book book = new datamodel.Book {Id=b.Id, Author_id=b.Author_id,BookName=b.BookName,
            Maker_id=b.Maker_id,Year=b.Year,Count=b.Count};
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
