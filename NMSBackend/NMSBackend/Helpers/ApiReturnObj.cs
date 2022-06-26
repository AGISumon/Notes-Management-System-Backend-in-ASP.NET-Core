namespace NMSBackend.Helpers
{
    public class ApiReturnObj
    {
        public dynamic ApiData { get; set; }
        public string Message { get; set; }
        public bool IsExecute { get; set; }
        public int TotalRecord { get; set; }
        public string MessageCode { get; set; }
    }
}
