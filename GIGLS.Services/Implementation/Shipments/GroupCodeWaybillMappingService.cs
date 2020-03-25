using GIGLS.Core;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Shipments
{
    public class GroupCodeWaybillMappingService : IGroupCodeWaybillMappingService
    {
        private readonly IUnitOfWork _uow;
        private readonly IPreShipmentMobileService _preShipmentMobileService;

        public GroupCodeWaybillMappingService(IUnitOfWork uow, IPreShipmentMobileService preShipmentMobileService)
        {
            _uow = uow;
            _preShipmentMobileService = preShipmentMobileService;
            MapperConfig.Initialize();
        }

        //Get WayBillNumbers In Group
        public async Task<GroupCodeWaybillMappingDTO> GetWaybillNumbersInGroup (string groupCodeNumber)
        {
            try
            {
                var groupList = await _uow.GroupCodeWaybillMapping.FindAsync(x => x.GroupCodeNumber == groupCodeNumber);
                var preShipmentList = new List<PreShipmentMobileDTO>();
                var waybillList = new List<string>();
                if(groupList != null)
                {
                    foreach(var item in groupList)
                    {
                        var preShipmentDTO = await _preShipmentMobileService.GetPreShipmentDetail(item.WaybillNumber);
                        preShipmentList.Add(preShipmentDTO);
                        waybillList.Add(preShipmentDTO.Waybill);
                    }
                    var groupCodeWaybillMappingDTO = new GroupCodeWaybillMappingDTO
                    {
                        GroupCodeNumber = groupCodeNumber,
                        PreShipmentMobile = preShipmentList,
                        WaybillNumbers = waybillList
                    };
                    return groupCodeWaybillMappingDTO;
                }
                else
                {
                    throw new GenericException($"No GroupCode exists for this : {groupCodeNumber}");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
