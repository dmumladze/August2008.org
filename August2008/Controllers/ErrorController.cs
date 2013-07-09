using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace August2008.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Forbidden()
        {
            Response.StatusCode = 403;
            return View();
        }
        public ActionResult NotFound()
        {
            return View();
        }
    }
}