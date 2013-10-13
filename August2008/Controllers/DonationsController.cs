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
    [Authorize]
    public class DonationsController : BaseController
    {
        private IDonationRepository _donationRepository;
        private IDonationService _donationService;
         
        public DonationsController(IDonationService donationService, IDonationRepository donationRepository)
        {
            _donationRepository = donationRepository;
            _donationService = donationService;
        }
        [AllowAnonymous]
        public ActionResult Index()
        {
            var criteria = _donationRepository.SearchDonations(new DonationSearchCriteria());
            var model = Mapper.Map(criteria, new DonationSearchModel());
            return View(model);
        }
        [AllowAnonymous]
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
            donation.UserId = Me.UserId;            
            _donationRepository.UpdateUserMessage(donation);
            return new EmptyResult();
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
                Logger.Error(ex);
            }
            catch (Exception ex)
            {                
                ViewBag.DisplayMessage = "Oops! Something went wrong... :(";
                Logger.Error(ex);
            }
            return PartialView("DonationsListPartial", model.Result);
        }
        [AllowAnonymous]
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
        [AllowAnonymous]
        public ActionResult PayPalDonationIpn(PayPalTransaction model)
        {
            var httpStatus = new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            if (ValidateTransaction(model))
            {           
                       
                var ipnBytes = Request.BinaryRead(Request.ContentLength);

                model.ReplyEmail = ReplyEmail;
                model.EmailSubject = Resources.Donations.Strings.ThankYou;
                model.EmailMessage = Resources.Donations.Strings.ThankYouEmailMessage;

                if (_donationService.ProcessPayPalDonation(ipnBytes, model))
                {
                    httpStatus = new HttpStatusCodeResult(HttpStatusCode.OK);
                }
            }
            Response.ContentType = "text/html";
            return httpStatus;
        }
        private bool ValidateTransaction(PayPalTransaction model)
        {
            Logger.Info(model.ToXml());

            if (!_donationRepository.TransactionCompleted(model.txn_id))
            {
                if (!model.receiver_email.Equals(PayPalEmail, StringComparison.OrdinalIgnoreCase))
                {
                    Logger.WarnFormat("Email 'receiver_email' value '{0}' does not match our email '{1}'.", model.receiver_email, PayPalEmail);
                    return false;
                }
                Logger.InfoFormat("{0} - {1}", model.txn_id, model.payment_status);
                if (model.txn_id == null)
                {
                    Logger.Warn("Parameter 'txn_id' is empty.");
                    return false;
                }
                if (string.IsNullOrWhiteSpace(model.custom))
                {
                    Logger.Warn("Parameter 'custom' is empty.");
                    return false;
                }                
                return true;
            }
            return false;
        }
    }
}