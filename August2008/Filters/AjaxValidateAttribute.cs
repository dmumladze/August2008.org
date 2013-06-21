using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace August2008.Filters
{
    public class AjaxValidateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                return;
            }
            var modelState = filterContext.Controller.ViewData.ModelState;
            if (!modelState.IsValid)
            {
                var errorModel = from x in modelState.Keys
                                 let item = modelState[x]
                                 where item.Errors.Count > 0
                                 select new { key = x, errors = item.Errors.Select(y => y.ErrorMessage).ToArray() };

                filterContext.Result = new JsonResult { Data = errorModel };
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
        }
    }
}