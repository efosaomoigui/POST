using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.Domain
{
    public class ShipmentPackagingTransactions : BaseDomain, IAuditable
    {
        [Key]
        public int ShipmentPackageTransactionsId { get; set; }
        [MaxLength(500)]
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public int ShipmentPackageId { get; set; }
        public int Quantity { get; set; }
        public int ServiceCenterId { get; set; }
        public string Waybill { get; set; }
        public PackageTransactionType PackageTransactionType { get; set; }
        public int ReceiverServiceCenterId { get; set; }
    }
}
