using POST.Core;
using POST.Core.Domain;
using System;

namespace GIGL.POST.Core.Domain
{
    public class StockSupplyDetails : BaseDomain, IAuditable
    {
        public int StockSupplyDetailsId { get; set; }
        public string InvoiceNumber { get; set; }
        public string LPONumber { get; set; }
        public string WaybillNumber { get; set; }
        public string ScannedInvoiceURL { get; set; }        

        public int StockRequestId { get; set; }
        public StockRequest StockRequest { get; set; }
    }
}