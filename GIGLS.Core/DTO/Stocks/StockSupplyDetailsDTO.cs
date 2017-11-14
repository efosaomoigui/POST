using GIGLS.CORE.DTO;
using System;
using System.Collections.Generic;

namespace GIGLS.Core.DTO.Stocks
{
    public class StockSupplyDetailsDTO : BaseDomainDTO
    {
        public StockSupplyDetailsDTO()
        {
            StockRequest = new List<StockRequestDTO>();
        }
        public int StockSupplyDetailsId { get; set; }
        public string InvoiceNumber { get; set; }
        public string LPONumber { get; set; }
        public string WaybillNumber { get; set; }
        public string ScannedInvoiceURL { get; set; }

        public List<StockRequestDTO> StockRequest { get; set; }
    }
}
