using GIGLS.CORE.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.Shipments
{
    [Serializable()]
    public class ShipmentHashDTO 
    {
        public int DestServId { get; set; }
        public int DeptServId { get; set; }
        public string Description { get; set; }
        public string SenderPhoneNumber { get; set; }
        public string ReceiverPhoneNumber { get; set; }
        public List<double> Weight { get; set; }
    }
}
