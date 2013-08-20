using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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
        [HttpGet]
        public ActionResult PayPal(PayPalModel2 transaction)
        {
            try
            {
                var donation = new Donation
                    {
                        UserId = Me.UserId,
                        ProviderName = "PayPal",
                        DisplayName = Me.Identity.Name,
                        DonationProviderId = 1, // PayPal 
                        ProviderData = transaction.ToDictionary()
                    };
                if (donation.Amount == 0)
                {
                    string val;
                    if (!string.IsNullOrWhiteSpace(val = Request.QueryString["amt"]))
                    {
                        donation.Amount = val.ToDecimal();
                    }
                }
                donation = _donationRepository.CreateDonation(donation);
                SiteHelper.SendEmail(ReplyEmail,
                    Me.Email,
                    Resources.Donations.Strings.ThankYou,
                    Resources.Donations.Strings.ThankYouEmailMessage);
                var model = new DonationSearchModel { ConfirmDonation = Mapper.Map(donation, new DonationModel()) };
                return View("Index", model);
            }
            catch
            {
            }
            return View("Index");
        }
        [HttpPost]
        public ActionResult PayPal(PayPalModel transaction)
        {
            try
            {
                var donation = new Donation
                {
                    UserId = Me.UserId,
                    ProviderName = "PayPal",
                    DisplayName = Me.Identity.Name,
                    DonationProviderId = 1, // PayPal 
                    ProviderData = transaction.ToDictionary()
                };
                Mapper.Map(transaction, donation);
                if (donation.Amount == 0)
                {
                    string val;
                    if (!string.IsNullOrWhiteSpace(val = Request.QueryString["amt"]))
                    {
                        donation.Amount = val.ToDecimal();
                    }
                }
                donation = _donationRepository.CreateDonation(donation);
                SiteHelper.SendEmail(ReplyEmail,
                    Me.Email,
                    Resources.Donations.Strings.ThankYou,
                    Resources.Donations.Strings.ThankYouEmailMessage);
                var model = new DonationSearchModel { ConfirmDonation = Mapper.Map(donation, new DonationModel()) };
                return View("Index", model);
            }
            catch
            {
            }
            return View("Index");
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
        [HttpPost]
        [AllowAnonymous]
        public ActionResult PayPalDonationIpn(PayPalModel model)
        {
            Logger.InfoFormat("{0} - {1}", model.txn_id, model.payment_status);
            Logger.Info(model.ToXml());

            if (!model.receiver_email.Equals(base.PayPalEmail, StringComparison.OrdinalIgnoreCase))
            {
                Logger.WarnFormat("Email 'receiver_email' value '{0}' does not match our email '{1}'.", model.receiver_email, base.PayPalEmail);
            }
            if (model.txn_id == null)
            {
                Logger.Warn("Parameter 'txn_id' is empty.");
            }
            var request = (HttpWebRequest)WebRequest.Create(base.PayPalPostUri);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            var param = Request.BinaryRead(Request.ContentLength);
            var message = Encoding.ASCII.GetString(param);
            message += "&cmd=_notify-validate";
            request.ContentLength = message.Length;

            //send the request to PayPal and get the response
            using (var streamOut = new StreamWriter(request.GetRequestStream(), Encoding.ASCII))
            {
                streamOut.Write(message);
                streamOut.Close();

                using (var streamIn = new StreamReader(request.GetResponse().GetResponseStream()))
                {
                    var response = streamIn.ReadToEnd();
                    streamIn.Close();

                    Logger.Info(response);

                    if (response.Equals("VERIFIED", StringComparison.OrdinalIgnoreCase))
                    {
                        Logger.InfoFormat("{0} - {1}, {2}", model.txn_id, model.mc_gross, model.mc_currency);
                        //check the payment_status is Completed
                        //check that txn_id has not been previously processed
                        //check that payment_amount/payment_currency are correct
                        //process payment
                    }
                    else if (response.Equals("INVALID", StringComparison.OrdinalIgnoreCase))
                    {
                        Logger.WarnFormat("INVALID response: {0}.", message);
                    }
                    else
                    {
                        Logger.WarnFormat("No status. Manual investinagation needed for {0}.", message);
                    }
                }
            }
            return new EmptyResult();
        }
    }
}