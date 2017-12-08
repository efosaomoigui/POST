using GIGLS.Core.IServices.Haulage;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Haulage;
using GIGLS.Core;
using GIGLS.Infrastructure;
using AutoMapper;
using GIGLS.Core.Domain;

namespace GIGLS.Services.Implementation.Haulage
{
    public class PackingListService : IPackingListService
    {
        private readonly IUnitOfWork _uow;

        public PackingListService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<object> AddPackingList(PackingListDTO packingListDto)
        {
            if (await _uow.PackingList.ExistAsync(x => x.Waybill == packingListDto.Waybill))
            {
                throw new GenericException($"{packingListDto.Waybill} Waybill already exist");
            }

            var newPackingList = new PackingList
            {
                Waybill = packingListDto.Waybill,
                Items = packingListDto.Items
            };

            _uow.PackingList.Add(newPackingList);
            await _uow.CompleteAsync();
            return new { id = newPackingList.PackingListId };
        }

        public async Task<PackingListDTO> GetPackingListById(int packingListId)
        {
            var packingList = await _uow.PackingList.GetAsync(packingListId);

            if (packingList == null)
            {
                throw new GenericException("PACKING LIST INFORMATION DOES NOT EXIST");
            }
            return Mapper.Map<PackingListDTO>(packingList);
        }

        public async Task<PackingListDTO> GetPackingListByWaybill(string waybill)
        {
            var packingList = await _uow.PackingList.GetAsync(x => x.Waybill.Equals(waybill));

            if (packingList == null)
            {
                throw new GenericException("PACKING LIST INFORMATION DOES NOT EXIST");
            }
            return Mapper.Map<PackingListDTO>(packingList);
        }


        public async Task<IEnumerable<PackingListDTO>> GetPackingLists()
        {
            var packingLists = await _uow.PackingList.GetPackingListAsync();
            return packingLists;
        }

        public async Task RemovePackingList(int packingListId)
        {
            var packingList = await _uow.PackingList.GetAsync(packingListId);

            if (packingList == null)
            {
                throw new GenericException("PACKING LIST INFORMATION DOES NOT EXIST");
            }

            _uow.PackingList.Remove(packingList);
            await _uow.CompleteAsync();
        }

        public async Task UpdatePackingList(int packingListId, PackingListDTO packingListDto)
        {
            var packingList = await _uow.PackingList.GetAsync(packingListId);

            if (packingList == null)
            {
                throw new GenericException("PACKING LIST INFORMATION DOES NOT EXIST");
            }
            packingList.Waybill = packingListDto.Waybill;
            packingList.Items = packingListDto.Items;
            await _uow.CompleteAsync();
        }
    }
}
