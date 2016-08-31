using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SSA.Web.Core
{
    public class JsonErrorResult : ActionResult
    {
        private readonly IEnumerable<string> _validationErrors;

        public JsonErrorResult(string message)
        {
            _validationErrors = new List<string> {message};

        }
        public JsonErrorResult(Exception ex)
        {
            _validationErrors = new List<string> {ex.ToString()};
        }

        public JsonErrorResult(IEnumerable<string> validationErrors)
        {
            _validationErrors = validationErrors;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.StatusCode = 400;
            var jsonResult = new JsonResult
            {
                Data = new {Errors = _validationErrors},
                ContentType = null,
                ContentEncoding = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
            jsonResult.ExecuteResult(context);
        }
    }
}