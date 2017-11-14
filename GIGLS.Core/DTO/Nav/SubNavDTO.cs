using System.Collections.Generic;

namespace GIGLS.CORE.DTO.Nav
{
    public class SubNavDTO : BaseDomainDTO
    {
        public SubNavDTO()
        {
            SubSubNavs = new List<SubSubNavDTO>();
        }
        public int SubNavId { get; set; }
        public string Title { get; set; }
        public string State { get; set; }
        public string Param { get; set; }
        public int MainNavId { get; set; }
        public string MainNavName { get; set; }
        public List<SubSubNavDTO> SubSubNavs { get; set; }
    }
}
