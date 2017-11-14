using AutoMapper;
using GIGLS.Core;
using GIGLS.CORE.Domain;
using GIGLS.CORE.DTO.Nav;
using GIGLS.CORE.IServices.Nav;
using GIGLS.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Nav
{
    public class SubSubNavService : ISubSubNavService
    {
        private readonly IUnitOfWork _uow;

        public SubSubNavService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<List<SubSubNavDTO>> GetSubSubNavs()
        {
            var subSubNavs = await _uow.SubSubNav.GetSubSubNavsAsync();
            return subSubNavs;
        }

        public async Task<SubSubNavDTO> GetSubSubNavById(int SubSubNavId)
        {
            var subSubNav = await _uow.SubSubNav.GetAsync(SubSubNavId);

            if (subSubNav == null)
            {
                throw new GenericException("SUB-SUB MAIN NAVIGATION INFORMATION DOES NOT EXIST");
            }
            return Mapper.Map<SubSubNavDTO>(subSubNav);
        }

        public async Task<object> AddSubSubNav(SubSubNavDTO subSubNavDto)
        {
            subSubNavDto.Title = subSubNavDto.Title.Trim();
            var subSubNavTitle = subSubNavDto.Title.ToLower();

            if (await _uow.SubSubNav.ExistAsync(v => v.Title.ToLower() == subSubNavTitle))
            {
                throw new GenericException($"Sub-Sub Main Navigation {subSubNavDto.Title} already exist");
            }

            var newSubSubNav = Mapper.Map<SubSubNav>(subSubNavDto);
            _uow.SubSubNav.Add(newSubSubNav);
            await _uow.CompleteAsync();
            return new { id = newSubSubNav.SubSubNavId };
        }

        public async Task UpdateSubSubNav(int SubSubNavId, SubSubNavDTO subSubNavDto)
        {
            var SubSubNav = await _uow.SubSubNav.GetAsync(SubSubNavId);

            if (SubSubNav == null)
            {
                throw new GenericException("SubSub NAVIGATION INFORMATION DOES NOT EXIST");
            }
            SubSubNav.SubNavId = subSubNavDto.SubNavId;
            SubSubNav.Title = subSubNavDto.Title.Trim();
            SubSubNav.State = subSubNavDto.State;
            SubSubNav.Param = subSubNavDto.Param.ToString();
            await _uow.CompleteAsync();
        }

        public async Task RemoveSubSubNav(int subSubNavId)
        {
            var subSubNav = await _uow.SubSubNav.GetAsync(subSubNavId);

            if (subSubNav == null)
            {
                throw new GenericException("SubSub NAVIGATION INFORMATION DOES NOT EXIST");
            }
            _uow.SubSubNav.Remove(subSubNav);
            await _uow.CompleteAsync();
        }
    }
}
