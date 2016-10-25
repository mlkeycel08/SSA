using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Script.Serialization;
using SSA.Web.Core.ApiModels;

namespace SSA.Web.Core.Filters
{
    public class ExceptionHandlingAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is ApiResponseException)
            {
                var exp = context.Exception as ApiResponseException;
                var obj = new {exp.ErrorCode, exp.Message};
                var jsSerializer = new JavaScriptSerializer();
                var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(jsSerializer.Serialize(obj)),
                    ReasonPhrase = "ApiException"
                };
                throw new HttpResponseException(resp);
            }
        }
    }
}