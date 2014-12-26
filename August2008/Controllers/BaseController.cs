using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using August2008.Common.Interfaces;
using August2008.Model;
using August2008.Models;
using log4net;
using Microsoft.Practices.Unity;
using August2008.Helpers;

namespace August2008.Controllers
{
    public class BaseController : Controller
    {
        protected readonly static Dictionary<string, int> AppCultures = new Dictionary<string,int>(StringComparer.OrdinalIgnoreCase);

        static BaseController()
        {
            AppCultures.Add("ka", 1);
            AppCultures.Add("ka-GE", 1);
            AppCultures.Add("en", 2);
            AppCultures.Add("en-Us", 2);
        }
        protected BaseController()
        {            
        }
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext); 
            AppUser = requestContext.HttpContext.User as FormsPrincipal;
            var culture = CultureHelper.GetCurrentCulture();
            int languageId;
            if (AppCultures.TryGetValue(culture, out languageId))
            {
                AppUser.LanguageId = languageId;
            }
            base.Initialize(requestContext);
        }

        [Dependency]
        protected ICacheProvider Cache { get; set; }

        [Dependency]
        protected ILog Log { get; set; }

        [Dependency]
        protected IMetadataRepository Metadata { get; set; }

        protected FormsPrincipal AppUser { get; private set; }

        protected string ContactEmail
        {
            get { return ConfigurationManager.AppSettings["August2008:ContactEmail"]; }
        }
        protected string SmtpServer
        {
            get { return ConfigurationManager.AppSettings["August2008:SmtpServer"]; }
        }
        protected string SmtpUsername
        {
            get { return ConfigurationManager.AppSettings["August2008:SmtpUsername"]; }
        }
        protected string SmtpPassword
        {
            get { return ConfigurationManager.AppSettings["August2008:SmtpPassword"]; }
        }
        protected string ReplyEmail
        {
            get { return ConfigurationManager.AppSettings["August2008:ReplyEmail"]; }
        }
        protected string PayPalEmail 
        {
            get { return ConfigurationManager.AppSettings["PayPal:PrimaryEmail"]; }
        }
        protected string PayPalWebScrUrl 
        {
            get { return ConfigurationManager.AppSettings["PayPal:WebScrUrl"]; }
        }
        protected ActionResult RedirectToLocal(string returnUrl)
        {
            // prevents from "open redirection attack"
            if (Url.IsLocalUrl(returnUrl)) 
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
