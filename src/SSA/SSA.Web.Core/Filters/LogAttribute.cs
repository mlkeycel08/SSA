using System;
using System.Text;
using System.Web.Mvc;
using log4net;
using SSA.Core.Extensions;

namespace SSA.Web.Core.Filters
{
    public class LogAttribute : ActionFilterAttribute
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(LogManager));

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var guid = Guid.NewGuid();
            var startTime = DateTime.Now;
            var routeData = filterContext.RouteData;
            var controller = (string) routeData.Values["controller"];
            var action = (string) routeData.Values["action"];
            var stream = filterContext.HttpContext.Request.InputStream;
            var data2 = new byte[stream.Length];
            stream.Read(data2, 0, data2.Length);
            var str = Encoding.UTF8.GetString(data2) + " IP: " + filterContext.HttpContext.Request.UserHostAddress;
            _log.Info($"Controller: {controller} Action: {action} Body: {str} Date: {startTime}  Guid:{guid}");
            filterContext.HttpContext.Items["__start_time__"] = startTime;
            filterContext.HttpContext.Items["__req_guid__"] = guid;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var startTime = (DateTime) filterContext.HttpContext.Items["__start_time__"];
            var guid = (Guid) filterContext.HttpContext.Items["__req_guid__"];
            var routeData = filterContext.RouteData;
            var duration = DateTime.Now - startTime;
            var controller = (string) routeData.Values["controller"];
            var action = (string) routeData.Values["action"];
            var createdAt = DateTime.Now;
            var ip = filterContext.HttpContext.Request.UserHostAddress;
            var result = filterContext.Result;
            if (result is JsonResult)
            {
                var j = result as JsonResult;
                _log.Info($"Controller: {controller} Action: {action} Body: {j.Data.PropertyList()} Duration: {duration} Date: {createdAt} IP: {ip} Guid:{guid}");
            }
            else
            {
                _log.Info(
                    $"Controller: {controller} Action: {action} Status: {filterContext.HttpContext.Response.Status} Duration: {duration} Date: {createdAt} IP: {ip} Guid:{guid}");

            }
        }
    }
}