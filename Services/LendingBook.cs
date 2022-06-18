using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using datamodel;

namespace Services
{
    public class LendingBook
    {
        datamodel.Entities2 db = new datamodel.Entities2();
        
        public void LendBook(ReaderBook r)
        {
            db.LendBook(r.BookId, r.ReaderId, r.DateOfTaking);
        }

        public void ReturnBook(int bookId)
        {
            db.ReturnBook(bookId, DateTime.Now);
        }
    }
}
