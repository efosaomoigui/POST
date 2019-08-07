using GIGL.GIGLS.Core.Domain;
using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain.Expenses
{
    public class Expenditure : BaseDomain, IAuditable
    {
        [Key]
        public int ExpenditureId { get; set; }

        public decimal Amount { get; set; }
        public string Description { get; set; }

        public int ExpenseTypeId { get; set; }
        public virtual ExpenseType ExpenseType { get; set; }

        public int ServiceCentreId { get; set; }
        public virtual ServiceCentre ServiceCentre { get; set; }

        [MaxLength(128)]
        public string UserId { get; set; }
    }
}