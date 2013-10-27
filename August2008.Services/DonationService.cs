using System;
using System.Configuration;
using August2008.Common;
using August2008.Common.Interfaces;
using August2008.Model;
using AutoMapper;
using log4net;
using Newtonsoft.Json;

namespace August2008.Services
{
    public class DonationService : IDonationService
    {
        private IPayPalService _paypalService;
        private IGeocodeService _geocodeService;
        private IDonationRepository _donationRepository;
        private IAccountRepository _accountRepository;
        private IEmailService _emailService;
        private string _paypalEmail;
         
        private ILog Log;

        public DonationService(IPayPalService paypalService, IGeocodeService geocodeService, IDonationRepository donationRepository, 
            IAccountRepository accountRepositoty, IEmailService emailService, ILog log)
        {
            _paypalService = paypalService;
            _geocodeService = geocodeService;
            _donationRepository = donationRepository;
            _accountRepository = accountRepositoty;
            _emailService = emailService;
            _paypalEmail = ConfigurationManager.AppSettings["PayPal:PrimaryEmail"];
            Log = log;
        }
        public bool ProcessPayPalDonation(byte[] ipnBytes, PayPalVariables variables)
        {
            if (variables.payment_status.Equals("refunded", StringComparison.OrdinalIgnoreCase))
            {
                return this.HandleRefunded(ipnBytes, variables);
            }
            if (!this.ValidateDonation(variables))
            {
                return false;
            }
            try
            {
                var donation = Mapper.Map(variables, new Donation());
                if (variables.txn_type.Equals("subscr_payment", StringComparison.OrdinalIgnoreCase))
                {
                    var subscription = _donationRepository.GetDonationSubscription(variables.subscr_id);
                    donation.DonationSubscriptionId = subscription.DonationSubscriptionId;
                }
                donation = _donationRepository.CreateDonation(donation);
                string response;
                if (_paypalService.TryReplyToIpn(ipnBytes, out response))
                {
                    donation.ExternalStatus = variables.payment_status;
                    if (response.Equals("verified", StringComparison.OrdinalIgnoreCase))
                    {
                        return this.HandleVerified(variables, donation);
                    }
                    else
                    {
                        this.LogFailure(ipnBytes, response);
                        _donationRepository.CompleteTransaction(donation);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error while processing PayPal donation IPN.", ex);
            }
            return false;
        }
        public bool ProcessPayPalSubscription(byte[] ipnBytes, PayPalVariables variables)
        {
            switch (variables.txn_type)
            {
                case "subscr_payment":
                    return this.ProcessPayPalDonation(ipnBytes, variables);

                case "subscr_signup":
                    return this.HandleSignup(ipnBytes, variables);                
            }
            if (variables.payment_status.Equals("refunded", StringComparison.OrdinalIgnoreCase))
            {
                return this.HandleRefunded(ipnBytes, variables);
            }
            return false;
        }
        private bool HandleVerified(PayPalVariables variables, Donation donation) 
        {
            Log.InfoFormat("{0} - {1}, {2} - OK", variables.txn_id, variables.mc_gross, variables.mc_currency);

            if (variables.payment_status.Equals("completed", StringComparison.OrdinalIgnoreCase))
            {
                donation.IsCompleted = true;
                donation.ExternalStatus = variables.payment_status;
                try
                {
                    GeoLocation location;
                    if (_geocodeService.TryGetGeoLocation(variables, out location))
                    {
                        donation.CityId = location.City.CityId;
                        donation.StateId = location.State.StateId;
                        donation.CountryId = location.Country.CountryId;
                    }
                    var custom = JsonConvert.DeserializeObject<PayPalCustom>(variables.custom);
                    _donationRepository.CompleteTransaction(donation);
                    if (location != null && location.Address != null)
                    {
                        _accountRepository.UpdateUserProfileAddress(custom.UserId, location.Address);
                    }
                    //var contactInfo = _accountRepository.GetUserContactInfo(custom.UserId);
                    //if (contactInfo != null)
                    //{
                    //    _emailService.SendEmail(variables.ReplyEmail, contactInfo.Email, variables.EmailSubject, variables.EmailMessage);
                    //}
                    return true;
                }
                catch (Exception ex)
                {
                    Log.Error("Manual investigation required.", ex);
                }
            }
            return false;
        }
        private bool HandleSignup(byte[] ipnBytes, PayPalVariables variables)
        {
            if (!this.ValidateSubscription(variables))
            {
                return false;
            }
            try
            {
                var subscription = Mapper.Map(variables, new DonationSubscription());
                subscription.EndDate = subscription.StartDate.AddMonths(subscription.RecurrenceTimes);
                _donationRepository.CreateDonationSubscription(subscription);
                string response;
                if (_paypalService.TryReplyToIpn(ipnBytes, out response))
                {
                    if (response.Equals("verified", StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                    else
                    {
                        this.LogFailure(ipnBytes, response);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Manual investigation required.", ex);
            }
            return false;
        }
        private bool HandleRefunded(byte[] ipnBytes, PayPalVariables variables) 
        {
            string response;
            if (_paypalService.TryReplyToIpn(ipnBytes, out response))
            {
                if (response.Equals("verified", StringComparison.OrdinalIgnoreCase))
                {
                    _donationRepository.RefundDonation(variables.parent_txn_id, variables.payment_status);
                    return true;
                }
                else
                {
                    this.LogFailure(ipnBytes, response);
                }
            }
            return false;
        }
        private void LogFailure(byte[] ipnBytes, string response)
        {
            if (response.Equals("invalid", StringComparison.OrdinalIgnoreCase))
            {
                Log.ErrorFormat("Invalid response: {0}.", ipnBytes.ToASCIIString());                
            }
            else
            {
                Log.ErrorFormat("Empty status from PayPal... Failed, {0}.", ipnBytes.ToASCIIString());
            }
        }
        public bool ValidateDonation(PayPalVariables variables)
        {
            if (!_donationRepository.DonationCompleted(variables.txn_id))
            {
                if (!variables.receiver_email.Equals(_paypalEmail, StringComparison.OrdinalIgnoreCase))
                {
                    Log.WarnFormat("Email 'receiver_email' value '{0}' does not match our email '{1}'.", variables.receiver_email, _paypalEmail);
                    return false;
                }
                Log.InfoFormat("{0} - {1}", variables.txn_id, variables.payment_status);
                if (string.IsNullOrWhiteSpace(variables.txn_id))
                {
                    Log.Warn("Parameter 'txn_id' is empty.");
                    return false;
                }
                if (string.IsNullOrWhiteSpace(variables.custom))
                {
                    Log.Warn("Parameter 'custom' is empty.");
                    return false;
                }
                return true;
            }
            return false;
        }
        public bool ValidateSubscription(PayPalVariables variables)
        {
            if (!_donationRepository.SubscriptionCompleted(variables.subscr_id))
            {
                if (!variables.receiver_email.Equals(_paypalEmail, StringComparison.OrdinalIgnoreCase))
                {
                    Log.WarnFormat("Email 'receiver_email' value '{0}' does not match our email '{1}'.", variables.receiver_email, _paypalEmail);
                    return false;
                }
                if (string.IsNullOrWhiteSpace(variables.subscr_id))
                {
                    Log.Warn("Parameter 'txn_id' is empty.");
                    return false;
                }
                if (string.IsNullOrWhiteSpace(variables.custom))
                {
                    Log.Warn("Parameter 'custom' is empty.");
                    return false;
                }
                return true;
            }
            return false;
        }
    }
}
