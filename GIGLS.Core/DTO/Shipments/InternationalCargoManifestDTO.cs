using GIGLS.Core.Domain;
using GIGLS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO
{
    public class InternationalCargoManifestDTO : BaseDomain
    {

        public InternationalCargoManifestDTO()
        {
            InternationalCargoManifestDetails = new List<InternationalCargoManifestDetailDTO>();
        }
        public int InternationalCargoManifestId { get; set; }
       
        public string ManifestNo { get; set; }
       
        public string AirlineWaybillNo { get; set; }
       
        public string FlightNo { get; set; }
        public int DestinationCountry { get; set; }

        public int DepartureCountry { get; set; }
        public DateTime FlightDate { get; set; }
      
        public string UserId { get; set; }
        
        public string CargoedBy { get; set; }
        public List<InternationalCargoManifestDetailDTO> InternationalCargoManifestDetails { get; set; }
        public string DestinationCountryName { get; set; }

        public string DepartureCountryName { get; set; }
    }


    public class InternationalCargoManifestDetailDTO
    {
        public int InternationalCargoManifestDetailId { get; set; }

        public string RequestNumber { get; set; }

        public string Waybill { get; set; }
        public double Weight { get; set; }
        public int Quantity { get; set; }
        
        public string ItemUniqueNo { get; set; }
        
        public string CourierService { get; set; }
        
        public string ItemName { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public ItemState ItemState { get; set; }
        public int InternationalCargoManifestId { get; set; }
        public int ShipmentExportId { get; set; }
        public string ItemRequestCode { get; set; }
        public int NoOfPackageReceived { get; set; }
    }
}
