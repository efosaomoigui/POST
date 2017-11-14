using GIGLS.Core;
using GIGLS.Core.Domain;
using System.Collections.Generic;

namespace GIGLS.CORE.Domain
{
    public class SubNav : BaseDomain, IAuditable
    {
        public SubNav()
        {
            SubSubNavs = new HashSet<SubSubNav>();
        }
        public int SubNavId { get; set; }
        public string Title { get; set; }
        public string State { get; set; }
        public string Param { get; set; }

        public int MainNavId { get; set; }
        public virtual MainNav MainNav { get; set; }
        
        public virtual ICollection<SubSubNav> SubSubNavs { get; set; }
    }
}
