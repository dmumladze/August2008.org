using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using August2008.Common.Interfaces;
using August2008.Helpers;
using August2008.Models;
using AutoMapper;

namespace August2008.Controllers
{
    public class HomeController : BaseController
    {
        private IHeroRepository _heroRepository;

        public HomeController(IHeroRepository heroRepository)
        {
            _heroRepository = heroRepository;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Mission() 
        {
            return Index();
        }
        public ActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Contact(ContactModel model)
        {
            SiteHelper.SendEmail(model.Email,
                ContactEmail,
                string.Format("{0}: {1}", model.Name, model.Subject),
                model.Message);
            ViewBag.Notification = "Thank you, message has been delivered.";
            return View();
        }
        public ActionResult Faq()
        {
            return View();
        }
    }
}
