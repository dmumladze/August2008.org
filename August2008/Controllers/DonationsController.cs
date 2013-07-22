using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using August2008.Common.Interfaces;
using August2008.Model;
using August2008.Models;
using AutoMapper;

namespace August2008.Controllers
{
    [Authorize]
    public class DonationsController : BaseController
    {
        private IDonationRepository _donationRepository;

        public DonationsController(IDonationRepository donationRepository)
        {
            _donationRepository = donationRepository;
        }
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Donate(string provider)
        {
            ViewBag.Provider = provider;
            return View();
        }
        [HttpPost]
        public ActionResult PayPal(PayPalModel transaction) 
        {
            var donation = new Donation 
                {
                    UserId = Me.UserId,
                    ProviderName = "PayPal",
                    DisplayName = Me.Identity.Name,
                    DonationProviderId = 1 // PayPal 
                };
            Mapper.Map(transaction, donation);
            donation = _donationRepository.CreateDonation(donation);
            var model = new DonationModel();
            Mapper.Map(donation, model);
            return View("Confirm", model);
        }
        [HttpPost]
        public ActionResult Confirm(DonationModel model)
        {
            var donation = new Donation();
            Mapper.Map(model, donation);

            _donationRepository.UpdateDonation(donation);

            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Cancel(DonationProvider provider)
        {
            return View("Index");
        }
    }
}