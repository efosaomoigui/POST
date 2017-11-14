namespace GIGLS.CORE.DTO.Nav
{
    public class SubSubNavDTO : BaseDomainDTO
    {
        public int SubSubNavId { get; set; }
        public string Title { get; set; }
        public string State { get; set; }
        public string Param { get; set; }
        public int SubNavId { get; set; }
        public string SubNavTitle { get; set; }
    }
}
