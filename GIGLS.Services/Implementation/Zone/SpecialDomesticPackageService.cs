using GIGLS.Core.IServices.Zone;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Zone;
using GIGLS.Core;
using GIGLS.Infrastructure;
using GIGLS.Core.Domain;
using AutoMapper;

namespace GIGLS.Services.Implementation.Zone
{
    public class SpecialDomesticPackageService : ISpecialDomesticPackageService
    {

        private readonly IUnitOfWork _uow;
        
        public SpecialDomesticPackageService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }

        private async Task<bool> _ConfirmIsNewPackage(string packageName) {
            var exists =   _uow.SpecialDomesticPackage
                                    .ExistAsync( p => p.Name.Trim().ToLower() == packageName.ToLower().Trim());
            if(true == await exists)
            {
                throw new GenericException($"{packageName.ToUpper()} already exist");
            }
            return exists.Result;
        }

        public async Task<object> AddSpecialDomesticPackage(SpecialDomesticPackageDTO specialDomestic)
        {
            // var package = Mapper.Map<SpecialDomesticPackage>(specialDomestic);
            // Confirm Package does not already exist in the database
            //await _ConfirmIsNewPackage(package.Name);

            await _ConfirmIsNewPackage(specialDomestic.Name);

            var newPackage = new SpecialDomesticPackage
            {
                Name = specialDomestic.Name,
                Status = true,
                Weight = specialDomestic.Weight,
                SpecialDomesticPackageType = specialDomestic.SpecialDomesticPackageType                
            };

            _uow.SpecialDomesticPackage.Add(newPackage);

            await _uow.CompleteAsync();

            return new { Id = newPackage.SpecialDomesticPackageId };

        }

        public async Task DeleteSpecialDomesticPackage(int specialDomesticPackageId)
        {
            var package = await  _uow.SpecialDomesticPackage.GetAsync(specialDomesticPackageId);

            if (package == null)
                throw new GenericException("Special Package does not exist");

            _uow.SpecialDomesticPackage.Remove(package);
            _uow.Complete();

        }

        public async Task<SpecialDomesticPackageDTO> GetSpecialDomesticPackageById(int packageId)
        {
            var package = await _uow.SpecialDomesticPackage.GetAsync(packageId);

            if (package == null)
                throw new GenericException("Special Package does not exist");

            return Mapper.Map<SpecialDomesticPackageDTO>(package); 
        }

        public Task<IEnumerable<SpecialDomesticPackageDTO>> GetSpecialDomesticPackages()
        {
            return Task.FromResult( Mapper.Map<IEnumerable<SpecialDomesticPackage>, IEnumerable<SpecialDomesticPackageDTO>>(_uow.SpecialDomesticPackage.GetAll()));
        }

        public async Task UpdateSpecialDomesticPackage(int specialDomesticPackageId, SpecialDomesticPackageDTO specialDomestic)
        {
            var package = await _uow.SpecialDomesticPackage.GetAsync(specialDomesticPackageId);

            if (package == null)
                throw new GenericException("Special Package does not exist");

            if (package.SpecialDomesticPackageId != specialDomestic.SpecialDomesticPackageId)
                throw new GenericException("Invalid Package update request");

            package.Name = specialDomestic.Name;
            package.Status = specialDomestic.Status;
            package.Weight = specialDomestic.Weight;
            package.SpecialDomesticPackageType = specialDomestic.SpecialDomesticPackageType;

            await _uow.CompleteAsync();

        }

        public async Task UpdateSpecialDomesticPackage(int specialDomesticPackageId, bool status)
        {
            var package = await _uow.SpecialDomesticPackage.GetAsync(specialDomesticPackageId);

            if (package == null)
                throw new GenericException("Special Package does not exist");
                        
            package.Status = status;
            await _uow.CompleteAsync();
        }

       
    }
}
