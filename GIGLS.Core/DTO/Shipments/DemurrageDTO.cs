namespace GIGLS.Core.DTO.Shipments
{
    public class DemurrageDTO
    {
        public int DayCount { get; set; }
        public decimal Amount { get; set; }
        public string WaybillNumber { get; set; }
        public decimal AmountPaid { get; set; }

        //who approved the amount of demurrage
        public string ApprovedBy { get; set; }
        public string ApprovedId { get; set; }

        //who processed the release
        public string UserId { get; set; }

        public int ServiceCenterId { get; set; }
        public string ServiceCenterCode { get; set; }
        public int CountryId { get; set; }
        public CountryDTO Country { get; set; }
    }


    public class NewDemurrageDTO
    {
        public decimal Amount { get; set; }
        public string WaybillNumber { get; set; }
        public decimal AmountPaid { get; set; }

        //who approved the amount of demurrage
        public string ApprovedBy { get; set; }
        public string ApprovedId { get; set; }
        public int ServiceCenterId { get; set; }
        public int CountryId { get; set; }
    }
}
