using AutoMapper;
using POST.Core;
using POST.CORE.Domain;
using POST.CORE.DTO.Nav;
using POST.CORE.IServices.Nav;
using POST.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Services.Implementation.Nav
{
    public class MainNavService : IMainNavService
    {
        private readonly IUnitOfWork _uow;

        public MainNavService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<List<MainNavDTO>> GetMainNavs()
        {
            var mainNavs = await _uow.MainNav.GetMainNavsAsync();
            return mainNavs;
        }

        public async Task<MainNavDTO> GetMainNavById(int mainNavId)
        {
            var mainNav = await _uow.MainNav.GetAsync(mainNavId);

            if (mainNav == null)
            {
                throw new GenericException("MAIN NAVIGATION INFORMATION DOES NOT EXIST");
            }
            return Mapper.Map<MainNavDTO>(mainNav);
        }

        public async Task<object> AddMainNav(MainNavDTO mainNavDto)
        {
            mainNavDto.Name = mainNavDto.Name.Trim();
            var mainNavName = mainNavDto.Name.ToLower();

            if (await _uow.MainNav.ExistAsync(v => v.Name.ToLower() == mainNavName))
            {
                throw new GenericException($"Main Navigation {mainNavDto.Name} already exist");
            }

            var newMainNav = Mapper.Map<MainNav>(mainNavDto);
            _uow.MainNav.Add(newMainNav);
            await _uow.CompleteAsync();
            return new { id = newMainNav.MainNavId };
        }

        public async Task UpdateMainNav(int mainNavId, MainNavDTO mainNavDto)
        {
            var mainNav = await _uow.MainNav.GetAsync(mainNavId);

            if (mainNav == null)
            {
                throw new GenericException("MAIN NAVIGATION INFORMATION DOES NOT EXIST");
            }
            mainNav.Name = mainNavDto.Name.Trim();
            mainNav.State = mainNavDto.State;
            mainNav.Param = mainNavDto.Param.ToString();
            mainNav.Position = mainNavDto.Position;
            await _uow.CompleteAsync();
        }

        public async Task RemoveMainNav(int mainNavId)
        {
            var MainNav = await _uow.MainNav.GetAsync(mainNavId);

            if (MainNav == null)
            {
                throw new GenericException("MAIN NAVIGATION INFORMATION DOES NOT EXIST");
            }
            _uow.MainNav.Remove(MainNav);
            await _uow.CompleteAsync();
        }
    }
}
