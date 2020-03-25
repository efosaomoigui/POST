using System;
using System.Collections.Generic;

namespace GIGLS.Core.DTO.Utility
{
    public class MultipleShipmentResultComparer : IEqualityComparer<MultipleShipmentResult>
    {
        public bool Equals(MultipleShipmentResult x, MultipleShipmentResult y)
        {
            return x.Waybill.Equals(y.Waybill, StringComparison.InvariantCultureIgnoreCase);
        }

        public int GetHashCode(MultipleShipmentResult obj)
        {
            return obj.Waybill.GetHashCode();
        }
    }
}
