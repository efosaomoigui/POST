using GIGLS.Core.Domain;
using GIGLS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO
{
    public class UnidentifiedItemsForInternationalShippingDTO : BaseDomain
    {
        public int UnidentifiedItemsForInternationalShippingId { get; set; }
        public string TrackingNo { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhoneNo { get; set; }
        public string UserId { get; set; }
        public bool IsProcessed { get; set; }
        public string ItemName { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public int NoOfPackageReceived { get; set; }
        public int Quantity { get; set; }
    }


}
