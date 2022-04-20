using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using datamodel;

namespace Services
{
    public class AccountService
    {

        Entities db = new Entities();
        public Account GetAccountByLoginPassword(string login, string password)
        {
            var account = db.Accounts.FirstOrDefault(p => p.Login == login && p.Password == password);

            return account;
        }
    }
}
