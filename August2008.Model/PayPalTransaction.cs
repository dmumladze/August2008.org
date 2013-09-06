using System;

namespace August2008.Model
{
    public class PayPalTransaction
    {
        public string mc_gross { get; set; }
        public string protection_eligibility { get; set; }
        public string address_status { get; set; }
        public string payer_id { get; set; }
        public string tax { get; set; }
        public string address_street { get; set; }
        public string payment_date { get; set; }
        public string payment_status { get; set; }
        public string charset { get; set; }
        public string address_zip { get; set; }
        public string first_name { get; set; }
        public string address_country_code { get; set; }
        public string address_name { get; set; }
        public string notify_version { get; set; }
        public string custom { get; set; }
        public string payer_status { get; set; }
        public string address_country { get; set; }
        public string address_city { get; set; }
        public string quantity { get; set; }
        public string payer_email { get; set; }
        public string verify_sign { get; set; }
        public string txn_id { get; set; }
        public string payment_type { get; set; }
        public string payer_business_name { get; set; }
        public string last_name { get; set; }
        public string address_state { get; set; }
        public string receiver_email { get; set; }
        public string pending_reason { get; set; }
        public string txn_type { get; set; }
        public string item_name { get; set; }
        public string mc_currency { get; set; }
        public string item_number { get; set; } 
        public string item_number1 { get; set; }
        public string item_number2 { get; set; }
        public string item_number3 { get; set; } 
        public string residence_country { get; set; }
        public string test_ipn { get; set; }
        public string transaction_subject { get; set; }
        public string payment_gross { get; set; }
        public string merchant_return_link { get; set; }
        public string auth { get; set; }
        public string ipn_track_id { get; set; }
    }
}