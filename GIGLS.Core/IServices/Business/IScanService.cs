using GIGLS.Core.IServices;
using GIGLS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Business
{
    public interface IScanService : IServiceDependencyMarker
    {
        Task<bool> ScanShipment(string waybillNumber, ShipmentScanStatus scanStatus);
    }
}
