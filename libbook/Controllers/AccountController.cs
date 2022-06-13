using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using NLog;


namespace libbook.Controllers
{
    public class AccountController : Controller
    {
        Services.Account m_account = new Services.Account();
        // GET: Student
        private static Logger logger = LogManager.GetLogger("f");
        public ActionResult Login()
        {
            logger.Info("Ожидание авторизации...");
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(datamodel.Account account)
        {
            var user = m_account.GetAccountByLoginPassword(account.Login, account.Password);
            if (user == null)
            {
                logger.Warn("Ошибка авторизации! Введен неверный логин и/или пароль.");
                ModelState.AddModelError("", "Некорректное имя или пароль.");
            }
            else
            {
                logger.Info("Успешная авторизация! Пользователь " + "'"+ user.Login +"'" + " выполнил вход в аккаунт."); 
                FormsAuthentication.SetAuthCookie(account.Login, true);
                
                //логируем время
                m_account.SetLoginDateTime(user.Id);
                
                return RedirectToAction("Index", "LendingBook");
            }
            return View(account);
        }

        public ActionResult Index()
        {
            logger.Info("Пользователь " + "'" + User.Identity.Name + "'" + " перешел на страницу 'Профиль'");

            var user = m_account.GetAccountByLogin(User.Identity.Name);
            datamodel.Entities2 db = new datamodel.Entities2();

            var account = db.vAccounts.FirstOrDefault(p => p.Id == user.User_id);
            return View(account);
        }

        public ActionResult Edit(int id)
        {
            logger.Info("'Профиль' Изменение персональных данных");
            var account = m_account.GetAccountById(id);
            return View(account);
        }

        public ActionResult Delete(int id)
        {
            logger.Info("'Профиль' Удаление сотрудника");
            var account = m_account.GetAccountById(id);
            if (account != null)
                return PartialView(account);
            return View(account);
        }

        public ActionResult DeleteAccount(int id)
        {
            logger.Info("'Профиль' Сотрудник удалён");
            //удаление
            return View();
        }

        public ActionResult Logout()
        {
            logger.Info("Пользователь " + "'" + User.Identity.Name + "'" + " вышел из аккаунта.");
            var user = User.Identity.Name;
            var account = m_account.GetAccountByLogin(user);
            FormsAuthentication.SignOut();
            m_account.SetLogoutDateTime(account.Id);
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public ActionResult Edit(datamodel.vAccount account)
        {
            logger.Info("'Профиль' Ожидание редактирования пользователей...");
            var result = m_account.EditUser(account);
            if (result)
            {
                logger.Info("'Профиль' Успешное редактирование!");
            }
            else
            {
                logger.Warn("'Профиль' Ошибка редактирования!");
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
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
