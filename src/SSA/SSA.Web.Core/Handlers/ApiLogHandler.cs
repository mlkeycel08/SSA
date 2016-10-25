using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Routing;
using log4net;
using Newtonsoft.Json;
using SSA.Core.Extensions;
using SSA.Web.Core.ApiModels;

namespace SSA.Web.Core.Handlers
{
    public class ApiLogHandler : DelegatingHandler
    {
        private readonly JsonSerializerSettings _jSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var apiLog = CreateApiLogWithRequestData(request);
            if (request.Content != null)
            {
                await request.Content.ReadAsStringAsync()
                    .ContinueWith(task => { apiLog.RequestContentBody = task.Result; }, cancellationToken);
            }

            return await base.SendAsync(request, cancellationToken)
                .ContinueWith(task =>
                {
                    var response = task.Result;

                    // Update the API log entry with response info
                    apiLog.ResponseStatusCode = (int)response.StatusCode;
                    apiLog.ResponseTimestamp = DateTime.Now;

                    if (response.Content != null)
                    {
                        apiLog.ResponseContentBody = response.Content.ReadAsStringAsync().Result;
                        apiLog.ResponseContentType = response.Content.Headers.ContentType.MediaType;
                        apiLog.ResponseHeaders = SerializeHeaders(response.Content.Headers);
                    }


                    // Save the API log entry to the database
                    //var logServ = Container.Get<ILogService>();
                    //logServ.AddLog(apiLog);
                    var duration = apiLog.ResponseTimestamp - apiLog.RequestTimestamp;
                    var logger = LogManager.GetLogger(typeof(ApiLogHandler));
                    logger.InfoFormat("ApiLog: {0}  Duration : {1}", apiLog.SerializeXML(), duration);
                    return response;
                }, cancellationToken);
        }

        private ApiLog CreateApiLogWithRequestData(HttpRequestMessage request)
        {
            var context = (HttpContextBase)request.Properties["MS_HttpContext"];
            var routeData = request.GetRouteData();

            return new ApiLog
            {
                Application = "BabyEye",
                User = context.User.Identity.Name,
                Machine = Environment.MachineName,
                RequestContentType = context.Request.ContentType,
                RequestRouteTemplate = routeData.Route.RouteTemplate,
                //RequestRouteData = SerializeRouteData(routeData),
                RequestIpAddress = context.Request.UserHostAddress,
                RequestMethod = request.Method.Method,
                RequestHeaders = SerializeHeaders(request.Headers),
                RequestTimestamp = DateTime.Now,
                RequestUri = request.RequestUri.ToString()
            };
        }

        // Yavaş çalışıyor
        private string SerializeRouteData(IHttpRouteData routeData)
        {
            return JsonConvert.SerializeObject(routeData, Formatting.Indented, _jSettings);
        }

        private string SerializeHeaders(HttpHeaders headers)
        {
            var dict = new Dictionary<string, string>();

            foreach (var item in headers.ToList())
            {
                if (item.Value != null)
                {
                    var header = string.Empty;
                    foreach (var value in item.Value)
                    {
                        header += value + " ";
                    }

                    // Trim the trailing space and add item to the dictionary
                    header = header.TrimEnd(" ".ToCharArray());
                    dict.Add(item.Key, header);
                }
            }

            return JsonConvert.SerializeObject(dict, Formatting.Indented, _jSettings);
        }
    }
}
