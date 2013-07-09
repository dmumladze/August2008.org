using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using August2008.Model;
using August2008.Models;

namespace August2008.Controllers
{
    public class BaseController : Controller
    {
        protected override void Initialize(RequestContext requestContext)
        {
            Me = requestContext.HttpContext.User as FormsPrincipal;
            base.Initialize(requestContext);
        }
        protected FormsPrincipal Me { get; private set; } 
    }
}
