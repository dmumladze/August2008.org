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

namespace August2008.Controllers
{
    public class BaseController : Controller
    {
        protected BaseController()
        {            
        }
        protected BaseController(ICacheProvider cacheProvider)
        {
            Cache = cacheProvider;
        }
        protected override void Initialize(RequestContext requestContext)
        {
            Me = requestContext.HttpContext.User as FormsPrincipal;
            base.Initialize(requestContext);
        }

        protected FormsPrincipal Me { get; private set; }
        protected ICacheProvider Cache { get; private set; }

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
