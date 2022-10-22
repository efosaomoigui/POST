using System.ComponentModel.DataAnnotations;

namespace POST.Core.Domain.Expenses
{
    public class ExpenseType : BaseDomain, IAuditable
    {
        [Key]
        public int ExpenseTypeId { get; set; }
        public string ExpenseTypeName { get; set; }
    }
}
