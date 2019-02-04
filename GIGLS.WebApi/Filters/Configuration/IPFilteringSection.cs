using System.Configuration;

namespace GIGLS.WebApi.Filters.Configuration
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