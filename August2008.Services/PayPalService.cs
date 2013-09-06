using System;
using August2008.Common.Interfaces;
using log4net;
using Microsoft.Practices.Unity;

namespace August2008.Services
{
    public class PayPalService : IDonationService
    {
        private IDonationRepository _donationRepository;
        private IGeocodeService _geocodeService;
        private IMetadataRepository _metadataRepository;
        private IAccountRepository _accountRepository;

        public PayPalService(IDonationRepository donationRepository, IGeocodeService geocodeService, IMetadataRepository metadataReposity, IAccountRepository accountRepository)
        {
            _donationRepository = donationRepository;
            _geocodeService = geocodeService;
            _metadataRepository = metadataReposity;
            _accountRepository = accountRepository;
        }

        [Dependency]
        private ILog Logger { get; set; }

        public 
    }
}
