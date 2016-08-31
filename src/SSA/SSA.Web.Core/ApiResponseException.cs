using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSA.Web.Core
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
