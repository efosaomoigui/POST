using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.Utility;
using GIGLS.Core.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Utility
{
    public interface IQRAndBarcodeService : IServiceDependencyMarker
    {

       Task<string> ConverWaybillToQRCodeImage(string waybill);
       Task<string> ConverWaybillToBarCodeImage(string waybill);
       Task<byte[]> MergeImages(string path1, string path2, string path3, string waybill);
       Task<PreShipmentMobileThirdPartyDTO> AddImage(string waybill);
       Task<byte[]> AddTextToImage(string waybill, string imgPath);


    }
}
