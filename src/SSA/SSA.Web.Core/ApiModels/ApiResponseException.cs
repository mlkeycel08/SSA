using System;
using System.Runtime.Serialization;

namespace SSA.Web.Core.ApiModels
{ 
    [Serializable]
    public sealed class ApiResponseException : Exception
    {
        public int ErrorCode { get; }
        public ApiResponseException(int errorCode, string message) : base($"{errorCode}_{message}")
        {
            ErrorCode = errorCode;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ErrorCode", ErrorCode);

            base.GetObjectData(info, context);

        }
    }
}
