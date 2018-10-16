namespace GIGLS.Core.Domain
{
    public class Notification : BaseDomain
    {
        public int NotificationId { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
