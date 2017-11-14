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
    public class SubNavService : ISubNavService
    {
        private readonly IUnitOfWork _uow;

        public SubNavService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<List<SubNavDTO>> GetSubNavs()
        {
            var subNavs = await _uow.SubNav.GetSubNavsAsync();
            return subNavs;
        }

        public async Task<SubNavDTO> GetSubNavById(int subNavId)
        {
            var subNav = await _uow.SubNav.GetAsync(subNavId);

            if (subNav == null)
            {
                throw new GenericException("SUB MAIN NAVIGATION INFORMATION DOES NOT EXIST");
            }
            return Mapper.Map<SubNavDTO>(subNav);
        }

        public async Task<object> AddSubNav(SubNavDTO subNavDto)
        {
            subNavDto.Title = subNavDto.Title.Trim();
            var SubNavTitle = subNavDto.Title.ToLower();

            if (await _uow.SubNav.ExistAsync(v => v.Title.ToLower() == SubNavTitle))
            {
                throw new GenericException($"Sub Main Navigation {subNavDto.Title} already exist");
            }

            var newSubNav = Mapper.Map<SubNav>(subNavDto);
            _uow.SubNav.Add(newSubNav);
            await _uow.CompleteAsync();
            return new { id = newSubNav.SubNavId };
        }

        public async Task UpdateSubNav(int subNavId, SubNavDTO subNavDto)
        {
            var subNav = await _uow.SubNav.GetAsync(subNavId);

            if (subNav == null)
            {
                throw new GenericException("Sub NAVIGATION INFORMATION DOES NOT EXIST");
            }
            subNav.MainNavId = subNavDto.MainNavId;
            subNav.Title = subNavDto.Title.Trim();
            subNav.State = subNavDto.State;
            subNav.Param = subNavDto.Param.ToString();
            await _uow.CompleteAsync();
        }

        public async Task RemoveSubNav(int SubNavId)
        {
            var SubNav = await _uow.SubNav.GetAsync(SubNavId);

            if (SubNav == null)
            {
                throw new GenericException("Sub NAVIGATION INFORMATION DOES NOT EXIST");
            }
            _uow.SubNav.Remove(SubNav);
            await _uow.CompleteAsync();
        }
    }
}
