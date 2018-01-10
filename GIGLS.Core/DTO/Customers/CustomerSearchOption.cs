using GIGLS.Core.Enums;

namespace GIGLS.Core.DTO.Customers
{
    public class CustomerSearchOption
    {
        public CustomerFilterOption CustomerType { get; set; }
        public string SearchData { get; set; }
    }
}
