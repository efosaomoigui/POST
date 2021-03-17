namespace GIGLS.Core.DTO.PaymentTransactions
{
    public class PriceCategoryDTO
    {
        public int PriceCategoryId { get; set; }
        public int CountryId { get; set; }
        public string PriceCategoryName { get; set; }
        public decimal CategoryMinimumWeight { get; set; }
        public decimal PricePerWeight { get; set; }
        public decimal CategoryMinimumPrice { get; set; }
        public bool IsActive { get; set; }
    }
}
