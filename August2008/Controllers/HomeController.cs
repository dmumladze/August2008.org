using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using August2008.Common.Interfaces;
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
            var hero = _heroRepository.GetRandomHero(Me.LanguageId);
            ViewBag.RandomHero = Mapper.Map(hero, new HeroModel());
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
            var smtp = new SmtpClient();
            var message = new MailMessage(base.ContactEmail, model.Email, model.Subject, model.Message);
            smtp.Send(message);
            ViewBag.Notification = "Thank you, message has been delivered.";
            return View();
        }
    }
}
