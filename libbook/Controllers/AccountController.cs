using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace libbook.Controllers
{
    public class AccountController : Controller
    {
        Services.Account m_account = new Services.Account();
        // GET: Student
        public ActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(datamodel.Account account, string returnUrl)
        {
            var user = m_account.GetAccountByLoginPassword(account.Login, account.Password);
            if (user == null)
            {
                ModelState.AddModelError("", "Некорректное имя или пароль.");
            }
            else
            {
                FormsAuthentication.SetAuthCookie(account.Login, true);
                //логируем время
                m_account.SetLoginDateTime(user.Id);
                return RedirectToAction("Index", "LendingBook");
            }
            return View(account);
        }

        public ActionResult Index()
        {
            var user = m_account.GetAccountByLogin(User.Identity.Name);
            datamodel.Entities2 db = new datamodel.Entities2();

            var account = db.vAccounts.FirstOrDefault(p => p.Id == user.User_id);
            return View(account);
        }

        public ActionResult Edit(int id)
        {
            var account = m_account.GetAccountById(id);
            return View(account);
        }

        public ActionResult Delete(int id)
        {
            var account = m_account.GetAccountById(id);
            if (account != null)
                return PartialView(account);
            return View(account);
        }

        public ActionResult DeleteAccount(int id)
        {
            //удаление
            return View();
        }

        public ActionResult Logout()
        {
            var user = User.Identity.Name;
            var account = m_account.GetAccountByLogin(user);
            FormsAuthentication.SignOut();
            m_account.SetLogoutDateTime(account.Id);
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public ActionResult Edit(datamodel.vAccount account)
        {
            var result = m_account.EditUser(account);
            if(result)
                return RedirectToAction("Index");//всё норм
            return RedirectToAction("Index");//должна быть инфа об ошибке
        }

        // 
        public JsonResult GetAllAccounts()
        {
            var accounts = new Services.Account().GetAllAccounts();
            return Json(
                new
                {
                    accounts = accounts
                }, JsonRequestBehavior.AllowGet);
        }
    }
}