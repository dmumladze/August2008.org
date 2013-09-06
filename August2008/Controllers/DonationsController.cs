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
        private IGeocodeService _geocodeService;
        private IMetadataRepository _metadataRepository;
        private IAccountRepository _accountRepository;
        private IPayPalService _paypalService;

        public DonationsController(IPayPalService paypalService, IDonationRepository donationRepository, IGeocodeService geocodeService, IMetadataRepository metadataReposity, IAccountRepository accountRepository)
        {
            _donationRepository = donationRepository;
            _geocodeService = geocodeService;
            _metadataRepository = metadataReposity;
            _accountRepository = accountRepository;
            _paypalService = paypalService;
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
            }
            catch (Exception ex)
            {
                ViewBag.DisplayMessage = "Oops! Something went wrong... :(";
            }
            return PartialView("DonationsListPartial", model.Result);
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult PayPalDonationIpn(PayPalTransaction model)
        {
            if (ValidateTransaction(model))
            {
                Task.Factory.StartNew(() => {
                    try
                    {
                        var donation = Mapper.Map(model, new Donation());
                        donation = _donationRepository.CreateDonation(donation);

                        var bytes = Request.BinaryRead(Request.ContentLength);
                        string response;
                        if (_paypalService.TryReplyToIpn(PayPalWebScrUrl, bytes, out response))
                        {
                            donation.ExternalStatus = model.payment_status;

                            if (response.Equals("VERIFIED", StringComparison.OrdinalIgnoreCase))                               
                            {
                                Logger.InfoFormat("{0} - {1}, {2}, {3}", model.txn_id, model.mc_gross, model.mc_currency, response);

                                if (string.Equals(model.payment_status, "Completed", StringComparison.OrdinalIgnoreCase))
                                {
                                    try
                                    {
                                        var geo = _geocodeService.GetGeoLocation(model);

                                        donation.IsCompleted = true;
                                        donation.ExternalStatus = model.payment_status;
                                        donation.CityId = geo.City.CityId;
                                        donation.StateId = geo.State.StateId;
                                        donation.CountryId = geo.Country.CountryId;

                                        var ipnItem = JsonConvert.DeserializeObject<PayPalCustom>(model.custom);

                                        _donationRepository.CompleteTransaction(donation);
                                        _accountRepository.UpdateUserProfileAddress(ipnItem.UserId, geo.Address);

                                        var contactInfo = _accountRepository.GetUserContactInfo(ipnItem.UserId);
                                        if (contactInfo != null)
                                        {
                                            SiteHelper.SendEmail(ReplyEmail,
                                                contactInfo.Email,
                                                Resources.Donations.Strings.ThankYou,
                                                Resources.Donations.Strings.ThankYouEmailMessage);
                                        }
                                        //var country = _geocodeService.GetCountry(model.residence_country);
                                        //check the payment_status is Completed
                                        //check that txn_id has not been previously processed
                                        //check that payment_amount/payment_currency are correct
                                        //process payment
                                    }
                                    catch (Exception ex)
                                    {
                                        Logger.Error("Manual investigation required.", ex);
                                    }
                                }
                            }
                            else if (response.Equals("INVALID", StringComparison.OrdinalIgnoreCase))
                            {
                                Logger.WarnFormat("INVALID response: {0}.", bytes.ToASCIIString());
                                _donationRepository.CompleteTransaction(donation);
                            }
                            else
                            {
                                Logger.WarnFormat("Empty status from PayPal... {0}, {1}.", response, bytes.ToASCIIString());
                                _donationRepository.CompleteTransaction(donation);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("Error while talking to PayPal", ex);
                    }
                });
            }
            Response.ContentType = "text/html";
            return new HttpStatusCodeResult(HttpStatusCode.OK);
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