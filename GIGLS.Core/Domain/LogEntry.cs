namespace GIGLS.Core.Domain
{
    public class LogEntry
    {
        public int LogEntryId { get; set; }
        public string CallSite { get; set; }
        public string DateTime { get; set; }
        public string Level { get; set; }
        public string Logger { get; set; }
        public string MachineName { get; set; }
        public string Username { get; set; }
        public string ErrorSource { get; set; }
        public string ErrorClass { get; set; }
        public string ErrorMethod { get; set; }
        public string ErrorMessage { get; set; }
        public string InnerErrorMessage { get; set; }
        public string Exception { get; set; }
        public string StackTrace { get; set; }
        public string Thread { get; set; }
    }
}
