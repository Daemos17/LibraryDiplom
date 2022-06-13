using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wangkanai.Detection;
using NLog;

namespace libbook.Controllers
{
    public class LendingBookController : Controller
    {
        private static Logger logger = LogManager.GetLogger("f");

        // GET: Student
        public ActionResult Index()
        {
            logger.Info("Пользователь " + "'" + User.Identity.Name + "'" + " перешел на страницу 'Выдача книг'");
            return View();

        }

        public ActionResult MobileIndex()
        {
            return View();
        }

    }
}
