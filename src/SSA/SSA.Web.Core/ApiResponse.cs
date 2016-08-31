using System.Net;
using System.Runtime.Serialization;

namespace SSA.Web.Core
{
    [DataContract]
    public class ApiResponse<T>
    {
        public ApiResponse(HttpStatusCode statusCode, T result, int errorCode = 0, string errorMessage = null)
        {
            StatusCode = (int) statusCode;
            Data = result;
            ErrorDescription = errorMessage;
            ErrorCode = errorCode;
        }

        public ApiResponse()
        {
        }

        [DataMember]
        public string Version => "1.0";

        [DataMember]
        public int StatusCode { get; set; }

        [DataMember]
        public int ErrorCode { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string ErrorDescription { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public T Data { get; set; }
    }
}