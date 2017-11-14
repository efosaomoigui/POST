using GIGLS.Core;
using GIGLS.Core.Domain;

namespace GIGLS.CORE.Domain
{
    public class SubSubNav : BaseDomain, IAuditable
    {
        public int SubSubNavId { get; set; }
        public string Title { get; set; }
        public string State { get; set; }
        public string Param { get; set; }
        public int SubNavId { get; set; }
        public virtual SubNav SubNav { get; set; }
    }
}
