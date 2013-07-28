using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.WebPages.OAuth;
using August2008.Models;

namespace August2008
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            //OAuthWebSecurity.RegisterMicrosoftClient("00000000401017BA", "HiS50OVaZl93jQM60op5r5FSBGDe6JOR");
            //OAuthWebSecurity.RegisterTwitterClient("48WJcrkiwrv0pa2dJgbvw", "Rewzwm8e42m8OyCftG9vGtW9XyEEoDbMhdbwe1ds8aA");
            OAuthWebSecurity.RegisterFacebookClient("551104464941114", "3189a843e5829d465a52da1c862a191f");
            OAuthWebSecurity.RegisterGoogleClient();
            OAuthWebSecurity.RegisterYahooClient("Yahoo!");
        }
    }
}
