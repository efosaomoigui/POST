using POST.CORE.Enums;

namespace POST.Core.DTO.Customers
{
    public class CustomerSearchOption
    {
        public FilterCustomerType CustomerType { get; set; }
        public string SearchData { get; set; }
    }
}
