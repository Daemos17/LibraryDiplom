using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using datamodel;

namespace Services
{
    
    //книги
    public class BookService
    {
        Entities db = new Entities();

        public IEnumerable<Book> GetAllBooks()
        {
            //var m_books = db.Books;
            //формировать строку из автора книги + название + год
            //db.Books.Load(); vBooks
            return db.Books;
        }

        public Book GetById(int id)
        {
            return db.Books.FirstOrDefault(p => p.Id == id);

        }
        public List<Book> GetBooks(string term)
        {
            var t = db.Books.Where(p => p.BookName.StartsWith(term));
            
            return t.ToList();
        }
    }
}
