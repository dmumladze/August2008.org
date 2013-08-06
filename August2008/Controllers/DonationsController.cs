using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using August2008.Common;
using August2008.Common.Interfaces;
using August2008.Filters;
using August2008.Helpers;
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
            var criteria = new DonationSearchCriteria
                {
                    FromDate = DateTime.Now.AddDays(-30),
                    ToDate = DateTime.Now
                };
            criteria = _donationRepository.SearchDonations(criteria);
            var model = Mapper.Map(criteria, new DonationSearchModel());
            return View(model);
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
            SiteHelper.SendEmail(ReplyEmail, 
                Me.Email, 
                Resources.Donations.Strings.ThankYou,
                Resources.Donations.Strings.ThankYouEmailMessage);                
            var model = new DonationSearchModel { ConfirmDonation = Mapper.Map(donation, new DonationModel()) }; 
            return View("Index", model);
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
        [HttpGet]
        [NoCache]
        [AllowAnonymous]
        public ActionResult Search(DonationSearchModel model)
        {
            var criteria = Mapper.Map(model, new DonationSearchCriteria());
            try
            {
                criteria = _donationRepository.SearchDonations(criteria);                
                Mapper.Map(criteria.Result, model.Result);
            }
            catch (RepositoryException ex)
            {
                ViewBag.DisplayMessage = ex.Message;
            }
            catch (Exception ex)
            {
                ViewBag.DisplayMessage = "Oops! Something went wrong... :(";
            }            
            return PartialView("DonationsListPartial", model.Result);
        }
    }
}