using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain
{
    public class State : BaseDomain, IAuditable
    {
        public State()
        {
            Stations = new HashSet<Station>();
        }
        public int StateId { get; set; }

        [MaxLength(100)]
        public string StateName { get; set; }

        [MaxLength(100)]
        public string StateCode { get; set; }
        public int CountryId { get; set; }        
        public virtual ICollection<Station> Stations { get; set; }        
    }
}
