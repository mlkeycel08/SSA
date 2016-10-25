namespace SSA.Core.Models
{
    public class ResponseModel
    {
        public int? ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
        public object Data { get; set; }
        public bool IsValid() => ErrorCode == null;
    }
}