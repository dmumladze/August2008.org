using System;
using System.Collections.Generic;
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

        public JsonResult OkJson(string message = null)
        {
            return new JsonResult { Data = new {Ok = true, Message = message}};
        }
        public JsonResult WtfJson(string message = null) 
        {
            return new JsonResult { Data = new {Ok = false, Message = message}};
        }
    }
}
