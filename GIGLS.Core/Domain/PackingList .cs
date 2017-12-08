namespace GIGLS.Core.Domain
{
    public class PackingList : BaseDomain, IAuditable
    {
        public int PackingListId  { get; set; }
        public string Waybill { get; set; }
        public string Items { get; set; }
    }
}
