using System.Configuration;

namespace POST.WebApi.Filters.Configuration
{
    public class IPFilteringSection : ConfigurationSection
    {
        [ConfigurationProperty("ipAddresses", IsDefaultCollection = true)]
        public IPAddressElementCollection IPAdresses
        {
            get { return (IPAddressElementCollection)this["ipAddresses"]; }
            set { this["ipAddresses"] = value;  }
        }
    }
}