using GIGLS.Core.DTO.BankSettlement;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;

namespace GIGLS.Core.IServices
{
    public interface IServiceResponse<TResponse>
    {
        string Code { get; set; }
        string ShortDescription { get; set; }
        TResponse Object { get; set; }
        decimal Total { get; set; }
        string RefCode { get; set; }
        BankProcessingOrderCodesDTO Shipmentcodref { get; set; }
        Dictionary<string, IEnumerable<string>> ValidationErrors { get; set; }
        List<string> VehicleType { get; set; }

}
}