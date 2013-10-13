using System;
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
         
        private ILog Log;

        public DonationService(IPayPalService paypalService, IGeocodeService geocodeService, IDonationRepository donationRepository, IAccountRepository accountRepositoty, IEmailService emailService, ILog log)
        {
            _paypalService = paypalService;
            _geocodeService = geocodeService;
            _donationRepository = donationRepository;
            _accountRepository = accountRepositoty;
            _emailService = emailService;
            Log = log;
        }
        public bool ProcessPayPalDonation(byte[] ipnBytes, PayPalTransaction transaction) 
        {
            try
            {
                var donation = Mapper.Map(transaction, new Donation());  
                donation = _donationRepository.CreateDonation(donation);
                string response;
                if (_paypalService.TryReplyToIpn(ipnBytes, out response))
                {
                    donation.ExternalStatus = transaction.payment_status;

                    if (response.Equals("VERIFIED", StringComparison.OrdinalIgnoreCase))
                    {
                        Log.InfoFormat("{0} - {1}, {2}, {3}", transaction.txn_id, transaction.mc_gross, transaction.mc_currency, response);

                        if (string.Equals(transaction.payment_status, "Completed", StringComparison.OrdinalIgnoreCase))
                        {
                            donation.IsCompleted = true;
                            donation.ExternalStatus = transaction.payment_status;
                            try
                            {
                                GeoLocation location;
                                if (_geocodeService.TryGetGeoLocation(transaction, out location))
                                {
                                    donation.CityId = location.City.CityId;
                                    donation.StateId = location.State.StateId;
                                    donation.CountryId = location.Country.CountryId;
                                }
                                var ipnItem = JsonConvert.DeserializeObject<PayPalCustom>(transaction.custom);
                                _donationRepository.CompleteTransaction(donation);
                                if (location.Address != null)
                                {
                                    _accountRepository.UpdateUserProfileAddress(ipnItem.UserId, location.Address);
                                }
                                var contactInfo = _accountRepository.GetUserContactInfo(ipnItem.UserId);
                                if (contactInfo != null)
                                {
                                    _emailService.SendEmail(transaction.ReplyEmail, contactInfo.Email, transaction.EmailSubject, transaction.EmailMessage);
                                }
                                return true;
                            }
                            catch (Exception ex)
                            {
                                Log.Error("Manual investigation required.", ex);
                            }
                        }
                    }
                    else if (response.Equals("INVALID", StringComparison.OrdinalIgnoreCase))
                    {
                        Log.WarnFormat("INVALID response: {0}.", ipnBytes.ToASCIIString());
                        _donationRepository.CompleteTransaction(donation);
                    }
                    else
                    {
                        Log.WarnFormat("Empty status from PayPal... {0}, {1}.", response, ipnBytes.ToASCIIString());
                        _donationRepository.CompleteTransaction(donation);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error while processing PayPal IPN.", ex);
            }
            return false;
        }
    }
}
