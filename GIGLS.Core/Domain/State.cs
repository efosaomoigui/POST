using System.Collections.Generic;

namespace GIGLS.Core.Domain
{
    public class State : BaseDomain, IAuditable
    {
        public State()
        {
            Stations = new HashSet<Station>();
        }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public string StateCode { get; set; }
        public int CountryId { get; set; }        
        public virtual ICollection<Station> Stations { get; set; }        
    }
}
