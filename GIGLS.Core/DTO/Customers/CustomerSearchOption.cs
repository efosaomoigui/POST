using GIGLS.CORE.Enums;

namespace GIGLS.Core.DTO.Customers
{
    public class CustomerSearchOption
    {
        public FilterCustomerType CustomerType { get; set; }
        public string SearchData { get; set; }
    }
}
