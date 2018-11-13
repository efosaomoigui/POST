namespace GIGLS.CORE.DTO.Shipments
{
    public class FilterOptionsDto
    {
        public static int DefaultCount { get; } = 10;
        public static int DefaultPageNumber { get; } = 1;
        public int count { get; set; }
        public int page { get; set; }
        public string sortorder { get; set; }
        public string sortvalue { get; set; }
        public string filter { get; set; }
        public string filterValue { get; set; }
        public bool? IsInternational { get; set; }
    }
}
