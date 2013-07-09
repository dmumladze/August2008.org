using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using August2008.Models;

namespace August2008.Controllers
{
    public class DonationsController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PayPalDataTransfer(PayPalDataModel model)
        {
            return View("Index");
        }
        [HttpGet]
        public ActionResult Cancel(DonationProvider provider)
        {
            return View("Index");
        }
    }
}