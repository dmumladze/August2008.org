using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using August2008.Common;
using August2008.Common.Interfaces;
using August2008.Filters;
using August2008.Helpers;
using August2008.Model;
using August2008.Models;
using AutoMapper; 
using Newtonsoft.Json;

namespace August2008.Controllers
{
    [AllowAnonymous]
    public class DonationsController : BaseController
    {
        private IDonationRepository _donationRepository;
        private IDonationService _donationService;
        private IAccountRepository _accountRepository;
         
        public DonationsController(IDonationService donationService, IDonationRepository donationRepository, IAccountRepository accountRepository)
        {
            _donationRepository = donationRepository;
            _donationService = donationService;
            _accountRepository = accountRepository;
        }

        public ActionResult Index()
        {
            var criteria = _donationRepository.SearchDonations(new DonationSearchCriteria());
            var model = Mapper.Map(criteria, new DonationSearchModel());
            return View(model);
        }

        public ActionResult ThankYou()   
        {
            var criteria = _donationRepository.SearchDonations(new DonationSearchCriteria());
            var model = Mapper.Map(criteria, new DonationSearchModel());
            model.ConfirmDonation = true;
            return View("Index", model);
        }
        [NoCache]
        [HttpGet]
        public PartialViewResult UserMessage(int id)
        {
            var message = _donationRepository.GetUserMessage(id);
            return PartialView("UserMessagePartial", new DonationModel { DonationId = id, UserMessage = message });
        }
        [HttpPost]
        public ActionResult UserMessage(DonationModel model)
        {
            var donation = Mapper.Map(model, new Donation()); 
            donation.UserId = AppUser.UserId;            
            _donationRepository.UpdateUserMessage(donation);
            return new EmptyResult();
        }
        [HttpGet]
        public ActionResult Cancel()
        {
            var criteria = _donationRepository.SearchDonations(new DonationSearchCriteria());
            var model = Mapper.Map(criteria, new DonationSearchModel());
            return View("Index", model);
        }
        [HttpGet]
        [NoCache]

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
                Log.Error(ex);
            }
            catch (Exception ex)
            {                
                ViewBag.DisplayMessage = "Oops! Something went wrong... :(";
                Log.Error(ex);
            }
            return PartialView("DonationsListPartial", model.Result);
        }

        [NoCache]
        public JsonResult Locations(ZoomLevel? zoom) 
        {
            var p1 = new MapPoint(null, null);
            List<MapPoint> model;
            switch (zoom)
            {
                case ZoomLevel.City:
                    model = _donationRepository.GetDonationsByCity(p1, p1);
                    break;

                case ZoomLevel.State:
                    model = _donationRepository.GetDonationsByState(p1, p1);
                    break;

                case ZoomLevel.Country:
                default:
                    model = _donationRepository.GetDonationsByCity(p1, p1);
                    break;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult PayPalDonationIpn(PayPalVariables model)
        {
            this.ResolveUserId(model);

            Response.ContentType = "text/html";
            if (_donationService.ProcessPayPalDonation(Request.BinaryRead(Request.ContentLength), model))
            {
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }

        [HttpPost]
        public ActionResult PayPalSubscriptionIpn(PayPalVariables model)
        {
            this.ResolveUserId(model);

            Response.ContentType = "text/html";
            if (_donationService.ProcessPayPalSubscription(Request.BinaryRead(Request.ContentLength), model))
            {
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }

        private void ResolveUserId(PayPalVariables vars)
        {
            try
            {
                int? userId;
                bool isOAuthUser;

                Log.InfoFormat("IPN received: {0}", vars.ToJson());

                if (!_accountRepository.TryGetUserRegistered(vars.payer_email, "PayPal", out userId, out isOAuthUser))
                {
                    var user = new User();
                    user.DisplayName = string.Format("{0} {0}", vars.first_name, vars.last_name);
                    user.Email = vars.payer_email;

                    user.Profile = new UserProfile();
                    user.Profile.Lang = new Language { LanguageId = 1 };

                    user = _accountRepository.CreateUser(user);

                    vars.UserId = user.UserId;

                    SiteHelper.SendEmail(ReplyEmail,
                        user.Email,
                        Resources.Global.Strings.EmailSubject,
                        Resources.Account.Strings.WelcomeRegistrationMessage);
                }
                else
                {
                    vars.UserId = userId.GetValueOrDefault(0); //fallback to 0 as anonymous user
                }
            }
            catch (Exception ex)
            {
                vars.UserId = 0;
                Log.Error(ex);
            }
        }
    }
}