using System;
using System.Xml.Serialization;

namespace August2008.Model
{
    // https://www.paypal.com/cgi-bin/webscr?cmd=p/acc/ipn-subscriptions-outside
    // https://cms.paypal.com/uk/cgi-bin/?cmd=_render-content&content_ID=developer/e_howto_html_subscribe_buttons
    // https://www.paypal.com/cgi-bin/webscr?cmd=_pdn_subscr_techview_outside
    // http://www.mixedwaves.com/2010/11/paypal-subscriptions-ipn-demystified/
    [Serializable]
    public class PayPalVariables
    {
        // basic information
        public string business { get; set; }
        public string receiver_email { get; set; }
        public string receiver_id { get; set; }
        public string item_name { get; set; }
        public string item_number { get; set; }

        // advanced and custom information
        public string invoice { get; set; }
        [XmlIgnore]
        public string custom { get; set; }
        public string option_name1 { get; set; }
        public string option_selection1 { get; set; }
        public string option_name2 { get; set; }
        public string option_selection2 { get; set; }

        // transaction information
        public string payment_status { get; set; }
        public string pending_reason { get; set; }
        public string reason_code { get; set; }
        public string payment_date { get; set; }
        public string txn_id { get; set; }
        public string parent_txn_id { get; set; }
        public string txn_type { get; set; }

        // currency and exchange information
        public virtual string mc_gross { get; set; }
        public string mc_fee { get; set; }
        public string mc_currency { get; set; }
        public string settle_amount { get; set; }
        public string settle_currency { get; set; }
        public string exchange_rate { get; set; }
        public string payment_gross { get; set; }
        public string payment_fee { get; set; }

        // customer information
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string payer_business_name { get; set; }
        public string address_name { get; set; }
        public string address_street { get; set; }
        public string address_city { get; set; }
        public string address_state { get; set; }
        public string address_zip { get; set; }
        public string address_country { get; set; }
        public string address_country_code { get; set; }
        public string address_status { get; set; }
        public string payer_email { get; set; }
        public string payer_id { get; set; }
        public string payer_status { get; set; }
        public string payment_type { get; set; }

        // IPN information
        public string notify_version { get; set; }

        // security information
        public string verify_sign { get; set; }

        // subscription information
        public string subscr_date { get; set; }
        public string subscr_effective { get; set; }
        public string period1 { get; set; }
        public string period2 { get; set; }
        public string period3 { get; set; }
        public string amount1 { get; set; }
        public string amount2 { get; set; }
        public string amount3 { get; set; }
        public string mc_amount1 { get; set; }
        public string mc_amount2 { get; set; }
        public string mc_amount3 { get; set; }
        public string recurring { get; set; }
        public string reattempt { get; set; }
        public string retry_at { get; set; }
        public string recur_times { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string subscr_id { get; set; }

        // API fields
        [XmlIgnore]
        public string ReplyEmail { get; set; }
        [XmlIgnore]
        public string EmailSubject { get; set; }
        [XmlIgnore]
        public string EmailMessage { get; set; }
    }
}