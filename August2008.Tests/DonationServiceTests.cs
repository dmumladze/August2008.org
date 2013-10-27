using System;
using System.Collections.Specialized;
using System.Reflection;
using System.Web;
using System.Web.Routing;
using August2008.Controllers;
using August2008.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Fasterflect;
using August2008.Tests.Helpers;
using August2008.Common.Interfaces;
using System.Text;
using August2008.Tests.Fakes;

namespace August2008.Tests
{
    [TestClass]
    public class DonationServiceTests
    {
        private const string web_accept = "mc_gross=10.00&protection_eligibility=Partially Eligible - INR Only&address_status=unconfirmed&payer_id=ZTJXR7PV9B5XE&tax=0.00&address_street=10 marion ave&payment_date=14:54:07 Oct 01, 2013 PDT&payment_status=Completed&charset=windows-1252&address_zip=07081&first_name=valerian&mc_fee=0.59&address_country_code=US&address_name=valerian kapanadze&notify_version=3.7&custom={\"UserId\":\"15\",\"Amount\":\"10.00\"}&payer_status=unverified&business=admin@august2008.org&address_country=United States&address_city=springfield&quantity=0&verify_sign=ADvj-tIuMJxr3YEo-QPTwZC-2h.3AxZ.rJGoM6WYQBtB.M6Sr0yVPvIv&payer_email=valeriirma@yahoo.com&txn_id=65V630709A475661H&payment_type=instant&last_name=kapanadze&address_state=NJ&receiver_email=admin@august2008.org&payment_fee=0.59&receiver_id=AWFLS4EPFP8QA&txn_type=web_accept&item_name=In memory of Georgian heroes who died in Russia-Georgia War of August 2008&mc_currency=USD&item_number=&residence_country=US&receipt_id=3762-9314-8199-6898&transaction_subject={\"UserId\":\"15\",\"Amount\":\"10.00\"}&payment_gross=10.00&ipn_track_id=faadfe53a942a";
        private const string subscr_signup = "amount3=5.00&address_status=confirmed&recur_times=12&subscr_date=19:43:15 Oct 14, 2013 PDT&payer_id=JU4BSXZ6NMX8N&address_street=1 Dorothy Ct&mc_amount3=5.00&charset=windows-1252&address_zip=07930&first_name=David&reattempt=1&address_country_code=US&address_name=Business Objectd&notify_version=3.7&subscr_id=I-02JM045RAWH4&custom={\"UserId\":\"15\",\"Amount\":\"5.00\"}&payer_status=unverified&business=admin@august2008.org&address_country=United States&address_city=Chester&verify_sign=A6bEGiMZQGz.QXDgnVNb5PilXTCbAxTHD2KvxuaY5bzSj3Lb179yfij1&payer_email=davidlars99@gmail.com&payer_business_name=Business Objectd&last_name=Mumladze&address_state=NJ&receiver_email=admin@august2008.org&recurring=1&txn_type=subscr_signup&item_name=In memory of Georgian heroes who died in Russia-Georgia War of August 2008&mc_currency=USD&residence_country=US&period3=1 M&ipn_track_id=e3eceba9ac99";
        private const string subscr_payment = "mc_gross=5.00&protection_eligibility=Eligible&address_status=confirmed&payer_id=JU4BSXZ6NMX8N&address_street=1+Dorothy+Ct&payment_date=19%3A43%3A18+Oct+14%2C+2013+PDT&payment_status=Completed&charset=windows-1252&address_zip=07930&first_name=David&mc_fee=0.45&address_country_code=US&address_name=Business+Objectd&notify_version=3.7&subscr_id=I-02JM045RAWH4&custom={\"UserId\":\"15\",\"Amount\":\"10.00\"}&payer_status=unverified&business=admin%40august2008.org&address_country=United+States&address_city=Chester&verify_sign=AHpQoZvXI0TkHdUPDGIqSGUbhDcDAPyzL7vETcusODrm3udMGq0Ki34H&payer_email=davidlars99%40gmail.com&txn_id=1F811911UP9949934&payment_type=instant&payer_business_name=Business+Objectd&last_name=Mumladze&address_state=NJ&receiver_email=admin%40august2008.org&payment_fee=0.45&receiver_id=AWFLS4EPFP8QA&txn_type=subscr_payment&item_name=In+memory+of+Georgian+heroes+who+died+in+Russia-Georgia+War+of+August+2008&mc_currency=USD&residence_country=US&transaction_subject=In+memory+of+Georgian+heroes+who+died+in+Russia-Georgia+War+of+August+2008&payment_gross=5.00&ipn_track_id=e3eceba9ac99";

        private string ExternalId = Guid.NewGuid().ToString();
        private string SubscriptionId = Guid.NewGuid().ToString();

        [ClassInitialize]
        public static void OnInitialize(TestContext context)
        {
            MapperConfig.RegisterMapper();
            UnityHelper.Setup();
        }
        [ClassCleanup]
        public static void OnCleanup()
        {
            UnityHelper.Destroy();
        }
        [TestMethod]
        public void can_process_new_web_accept_ipn()  
        {
            var service = UnityHelper.Resolve<IDonationService>(); 
            var vars = this.CreatePayPalVariables(web_accept);
            vars.txn_id = ExternalId;          
            var result = service.ProcessPayPalDonation(Encoding.ASCII.GetBytes(web_accept), vars);
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void can_process_new_subscr_signup_ipn() 
        {
            var service = UnityHelper.Resolve<IDonationService>();
            var vars = this.CreatePayPalVariables(subscr_signup);
            vars.subscr_id = ExternalId;
            var result = service.ProcessPayPalSubscription(Encoding.ASCII.GetBytes(subscr_signup), vars);
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void can_process_new_subscr_payment_ipn() 
        {
            var service = UnityHelper.Resolve<IDonationService>();

            var signupVars = this.CreatePayPalVariables(subscr_signup);
            signupVars.subscr_id = SubscriptionId;
            var result = service.ProcessPayPalSubscription(Encoding.ASCII.GetBytes(subscr_signup), signupVars);
            Assert.IsTrue(result);

            var paymentVars = this.CreatePayPalVariables(subscr_payment);
            paymentVars.txn_id = ExternalId + "-1";
            paymentVars.subscr_id = SubscriptionId;
            result = service.ProcessPayPalSubscription(Encoding.ASCII.GetBytes(subscr_payment), paymentVars);
            Assert.IsTrue(result);

            paymentVars = this.CreatePayPalVariables(subscr_payment);
            paymentVars.txn_id = ExternalId + "-2";
            paymentVars.subscr_id = SubscriptionId;
            result = service.ProcessPayPalSubscription(Encoding.ASCII.GetBytes(subscr_payment), paymentVars);
            Assert.IsTrue(result);
        }
        private NameValueCollection ParseFormVars(string formVars)
        {
            var vars = new NameValueCollection();
            var splits = formVars.Split('&');

            foreach (var item in splits)
            {
                var itemSplit = item.Split('=');
                var left = itemSplit[0];
                var right = itemSplit[1];
                vars.Add(left, HttpUtility.HtmlDecode(right));
            }
            return vars;
        }
        private PayPalVariables CreatePayPalVariables(string formVars)
        {
            var vars = new PayPalVariables();
            var type = typeof(PayPalVariables);
            var split = formVars.Split('&');

            foreach (var item in split)
            {
                var itemSplit = item.Split('=');
                var left = itemSplit[0];
                var right = itemSplit[1];
                var prop = type.GetProperty(left, BindingFlags.Public | BindingFlags.Instance);
                if (prop != null)
                {
                    prop.SetValue(vars, HttpUtility.UrlDecode(right));
                }
            }
            return vars;
        }
    }
}
