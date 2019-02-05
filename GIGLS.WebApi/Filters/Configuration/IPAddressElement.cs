using System.Configuration;

namespace GIGLS.WebApi.Filters.Configuration
{
    public class IPAddressElement : ConfigurationElement
    {
        [ConfigurationProperty("address", IsKey = true, IsRequired = true)]
        public string Address
        {
            get { return (string)this["address"]; }
            set { this["address"] = value; }
        }

        [ConfigurationProperty("denied", IsRequired = false)]
        public bool Denied
        {
            get { return (bool)this["denied"]; }
            set { this["denied"] = value;  }
        }
    }
}