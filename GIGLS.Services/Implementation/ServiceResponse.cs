using GIGLS.Core.DTO.BankSettlement;
using GIGLS.Core.IServices;
using System.Collections.Generic;
using GIGLS.Core.DTO.Partnership;
using ThirdParty.WebServices.Magaya.Services;

namespace GIGLS.Services.Implementation
{
    public class ServiceResponse<TResponse> : IServiceResponse<TResponse>
    {
        public ServiceResponse(TResponse response) : this()
        {
            Object = response;
        }

        public ServiceResponse()
        {
            ValidationErrors = new Dictionary<string, IEnumerable<string>>();
        }

        public string Code { get; set; }
        public string ShortDescription { get; set; }
        public TResponse Object { get; set; }
        public api_session_error magayaErrorMessage { get; set; }
        public string Cookies { get; set; }
        public int more_reults { get; set; }
        public decimal Total { get; set; } 
        public string RefCode { get; set; }

        //added to display list of shipments for a wallet code
        public BankProcessingOrderCodesDTO Shipmentcodref { get; set; }   

        public Dictionary<string, IEnumerable<string>> ValidationErrors { get; set; }
        public List<string> VehicleType { get; set; }

        public string ReferrerCode { get; set; }

        public double AverageRatings { get; set; }

        public bool IsVerified { get; set; }
        public string PartnerType { get; set; }
        public bool IsEligible { get; set; }
        public List<VehicleTypeDTO> VehicleDetails { get; set; }
        public string BankName { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
    }
}
