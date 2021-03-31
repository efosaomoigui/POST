namespace GIGLS.Core.DTO.UPS
{
    public class UPSSecurity
    {
        public UPSSecurity()
        {
            UsernameToken = new UsernameToken();
            ServiceAccessToken = new ServiceAccessToken();
        }
        public UsernameToken UsernameToken { get; set; }
        public ServiceAccessToken ServiceAccessToken { get; set; }
    }

    public class UsernameToken
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class ServiceAccessToken
    {
        public string AccessLicenseNumber { get; set; }
    }

}
