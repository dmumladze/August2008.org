using System;
using System.IO;
using System.Net;
using System.Text;
using August2008.Common;
using August2008.Common.Interfaces;
using log4net;
using Microsoft.Practices.Unity;

namespace August2008.Services
{
    public class PayPalService : IPayPalService
    {
        private IGeocodeService _geocodeService;
        private IMetadataRepository _metadataRepository;

        public PayPalService(IGeocodeService geocodeService, IMetadataRepository metadataReposity)
        {
            _geocodeService = geocodeService;
            _metadataRepository = metadataReposity;
        }

        [Dependency]
        public ILog Logger { get; set; }

        public bool TryReplyToIpn(string webSrcUrl, byte[] bytes, out string response)       
        {
            response = string.Empty;
            try
            {
                var formVars = string.Concat(bytes.ToASCIIString(), "&cmd=_notify-validate");
                var request = (HttpWebRequest)WebRequest.Create(webSrcUrl);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";                

                using (var streamOut = new StreamWriter(request.GetRequestStream(), Encoding.ASCII))
                {
                    streamOut.Write(formVars);
                    streamOut.Close();
                    using (var streamIn = new StreamReader(request.GetResponse().GetResponseStream()))
                    {
                        response = streamIn.ReadToEnd();
                        streamIn.Close();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("Error while trying to handle IPN.", ex);
                return false;
            }
        }
    }
}
