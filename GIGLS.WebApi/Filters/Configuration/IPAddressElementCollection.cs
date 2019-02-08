using System.Configuration;

namespace GIGLS.WebApi.Filters.Configuration
{
    [ConfigurationCollection(typeof(IPAddressElement))]
    public class IPAddressElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new IPAddressElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((IPAddressElement)element).Address;
        }
    }
}