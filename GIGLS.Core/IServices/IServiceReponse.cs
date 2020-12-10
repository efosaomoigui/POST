using GIGLS.Core.DTO.BankSettlement;
using System.Collections.Generic;
using ThirdParty.WebServices.Magaya.Services;

namespace GIGLS.Core.IServices
{
    public interface IServiceResponse<TResponse>
    {
        string Code { get; set; }
        string ShortDescription { get; set; }
        TResponse Object { get; set; }
        api_session_error magayaErrorMessage { get; set; }
        string Cookies { get; set; }
        int more_reults { get; set; }
        decimal Total { get; set; }
        string RefCode { get; set; }
        BankProcessingOrderCodesDTO Shipmentcodref { get; set; }
        Dictionary<string, IEnumerable<string>> ValidationErrors { get; set; }
        List<string> VehicleType { get; set; }

    }
}