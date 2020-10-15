using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.Domain.Utility
{
    public class QRAndBarcode
    {
        public string Waybill { get; set; }
        public byte[] Image { get; set; }
        public string ImageLink { get; set; }
    }
}
