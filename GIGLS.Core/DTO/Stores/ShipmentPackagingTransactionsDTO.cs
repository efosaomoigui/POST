using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.Stores
{
    public class ShipmentPackagingTransactionsDTO : BaseDomainDTO
    {
        public string UserId { get; set; }
        public string User { get; set; }
        public int ShipmentPackageId { get; set; }
        public string ShipmentPackageName { get; set; }
        public int Quantity { get; set; }
        public int ServiceCenterId { get; set; }
        public string ServiceCenterName { get; set; }
        public string ReceiverServiceCenterName { get; set; }
        public string Waybill { get; set; }
        public PackageTransactionType PackageTransactionType { get; set; }
    }
}
