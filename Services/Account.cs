using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using datamodel;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;

namespace Services
{
    public class Account
    {
        Entities2 db = new Entities2();
        public datamodel.Account GetAccountByLoginPassword(string login, string password)
        {
            var account = db.Accounts.FirstOrDefault(p => p.Login == login && p.Password == password);

            return account;
        }

        public IEnumerable<datamodel.vAccount> GetAllAccounts()
        {
            //var m_books = db.Books;
            //формировать строку из автора книги + название + год
            //db.Books.Load(); vBooks
            return db.vAccounts;
        }

        public datamodel.Account GetAccountByLogin(string login)
        {
            var account = db.Accounts.FirstOrDefault(p => p.Login == login);
            return account;
        }

        public datamodel.vAccount GetAccountById(int id)
        {
            var account = db.vAccounts.FirstOrDefault(p => p.Id == id);
            return account;
        }

        public int SetLoginDateTime(int userId)
        {
            int ret = db.SetLoginDateTime(userId);
            return ret;
        }

        public int SetLogoutDateTime(int userId)
        {
            int ret = db.SetLogOutDateTime(userId);
            return ret;
        }

        public int Delete(int id)
        {
            var account = db.vAccounts.FirstOrDefault(p => p.Id == id);
            var admins = db.vAccounts.Where(p => p.IsAdmin == true);
            //проверим что не удаляем последнего админа
            if (admins.Count() == 1 && admins.FirstOrDefault().Id == id)
                return 1;
            //удалим
            var res = db.DeleteAccount(id);
            return 0;
        }

        public bool EditUser(vAccount account)
        {
            var res = db.EditUser(account.Id, account.FirstName, account.LastName, account.SecondName, account.Login, account.Email, account.Phone);
            if (res>0)
            {
                return true;
            }
            return false;
        }
    }
}
