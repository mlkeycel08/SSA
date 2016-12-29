using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using log4net;
using SSA.Core.Extensions;
using SSA.Web.Core.ApiModels;

namespace SSA.Web.Core.Handlers
{
    public class ApiResponseHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);
            if (!request.RequestUri.AbsolutePath.Contains("/swagger"))
                return BuildApiResponse(request, response);

            return response;
        }

        private static HttpResponseMessage BuildApiResponse(HttpRequestMessage request, HttpResponseMessage response)
        {
            object content = null;
            string errorMessage = null;
            var errorCode = 0;

            ValidateResponse(response, ref content, ref errorCode, ref errorMessage);

            // Yeni response'u custom olarak oluşturmuş olduğumuz wrapper sınıf ile baştan oluşturuyoruz.
            var newResponse = CreateHttpResponseMessage(request, response, content, errorCode, errorMessage);

            // Header key'lerini baştan set et.
            foreach (var loopHeader in response.Headers)
            {
                newResponse.Headers.Add(loopHeader.Key, loopHeader.Value);
            }

            return newResponse;
        }

        private static HttpResponseMessage CreateHttpResponseMessage<T>(HttpRequestMessage request,
            HttpResponseMessage response, T content, int errorCode, string errorMessage)
        {
            return request.CreateResponse(response.StatusCode,
                new ApiResponse<T>(response.StatusCode, content, errorCode, errorMessage));
        }

        private static void ValidateResponse(HttpResponseMessage response, ref object content, ref int errorCode,
            ref string errorMessage)
        {
            if (response.TryGetContentValue(out content) && !response.IsSuccessStatusCode)
            {
                var logger = LogManager.GetLogger(typeof(ApiResponseHandler));
                logger.Error($"Response content Error: {content.SerializeXML()}");
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    errorCode = (int) HttpStatusCode.Unauthorized;
                    errorMessage = "İşlemi yapmak için yetkiniz bulunmamaktadır.";
                    content = null;
                    return;
                }
                var error = content as HttpError;

                if (error != null)
                {
                    content = null;
                    errorCode = 500;
                    errorMessage = "İşleminiz şuan gerçekleştirilemiyor.";

                    var err = error.FirstOrDefault(w => w.Key.Equals("ExceptionMessage"));
                    if (err.Key == null) return;

                    var splitData = err.Value.ToString().Split('_');
                    if (splitData.Length != 2) return;

                    errorCode = int.Parse(splitData[0]);
                    errorMessage = splitData[1];
                    //content = null;
                    //StringBuilder sb = new StringBuilder();

                    //foreach (var loopError in error)
                    //{
                    //    sb.Append($"{loopError.Key}: {loopError.Value} ");
                    //}

                    //errorMessage = sb.ToString();
                }
            }
        }
    }
}