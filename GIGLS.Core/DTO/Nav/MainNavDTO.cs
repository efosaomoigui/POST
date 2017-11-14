using System.Collections.Generic;

namespace GIGLS.CORE.DTO.Nav
{
    public class MainNavDTO : BaseDomainDTO
    {
        public MainNavDTO()
        {
            SubNavs = new List<SubNavDTO>();
        }
        public int MainNavId { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public string Param { get; set; }
        public string Position { get; set; }
        public List<SubNavDTO> SubNavs { get; set; }
    }
}
