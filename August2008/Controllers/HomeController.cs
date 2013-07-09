using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using August2008.Common.Interfaces;

namespace August2008.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult About()
        {
            return Index();
        }
        public ActionResult Contact()
        {
            return Index();
        }
    }
}
