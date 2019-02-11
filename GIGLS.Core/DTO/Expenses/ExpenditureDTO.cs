using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.Expenses
{
    public class ExpenditureDTO : BaseDomainDTO
    {
        public int ExpenditureId { get; set; }

        public decimal Amount { get; set; }
        public string Description { get; set; }

        public int ExpenseTypeId { get; set; }
        public ExpenseTypeDTO ExpenseType { get; set; }

        public int ServiceCentreId { get; set; }
        public ServiceCentreDTO ServiceCentre { get; set; }

        public string UserId { get; set; }
        public string CreatedBy { get; set; }
    }
}
