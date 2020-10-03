using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core;
using GIGL.GIGLS.Core.Domain;
using AutoMapper;
using GIGLS.Core.IServices.ServiceCentres;
using GIGLS.Infrastructure;
using GIGLS.Core.IServices.Utility;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.User;
using System.Net;

namespace GIGLS.Services.IServices.ServiceCentres
{
    public class ServiceCentreService : IServiceCentreService
    {
        private readonly IGlobalPropertyService _globalPropertyService;
        private readonly IUnitOfWork _uow;

        public ServiceCentreService()
        {

        }
        public ServiceCentreService(IGlobalPropertyService globalPropertyService, IUnitOfWork uow)
        {
            _globalPropertyService = globalPropertyService;
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<object> AddServiceCentre(ServiceCentreDTO service)
        {
            try
            {
                if (!await _uow.Station.ExistAsync(c => c.StationId == service.StationId))
                {
                    throw new GenericException("STATION SELECTED DOES NOT EXIST", $"{(int)HttpStatusCode.NotFound}");
                }

                service.Name = service.Name.Trim();
                service.Name = service.Name.ToLower();
                service.Code = service.Code.Trim();
                service.Code = service.Code.ToLower();

                if (await _uow.ServiceCentre.ExistAsync(c => c.Name.ToLower() == service.Name || c.Code.ToLower() == service.Code))
                {
                    throw new GenericException($"{service.Name} Service Centre Already Exist", $"{(int)HttpStatusCode.Forbidden}");
                }

                service.Name = service.Name.ToUpper();
                service.Code = service.Code.ToUpper();
                service.IsActive = true;
                var newCentre = Mapper.Map<ServiceCentre>(service);
                _uow.ServiceCentre.Add(newCentre);
                await _uow.CompleteAsync();

                return new { Id = newCentre.ServiceCentreId };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteServiceCentre(int serviceCentreId)
        {
            try
            {
                var centre = await _uow.ServiceCentre.GetAsync(serviceCentreId);
                if (centre == null)
                {
                    throw new GenericException("Service Centre does not exist", $"{(int)HttpStatusCode.NotFound}");
                }
                _uow.ServiceCentre.Remove(centre);
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ServiceCentreDTO> GetServiceCentreByCode(string serviceCentreCode)
        {
            try
            {
                var centre = await _uow.ServiceCentre.GetAsync(s => s.Code == serviceCentreCode, "Station");
                if (centre == null)
                {
                    throw new GenericException("Service Centre does not exist", $"{(int)HttpStatusCode.NotFound}");
                }

                var centreDto = Mapper.Map<ServiceCentreDTO>(centre);
                centreDto.StationName = centre.Station.StationName;
                centreDto.StationCode = centre.Station.StationCode;
                return centreDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ServiceCentreDTO>> GetServiceCentreByCode(string[] code)
        {
            return await _uow.ServiceCentre.GetServiceCentreByCode(code);
        }

        public async Task<ServiceCentreDTO> GetServiceCentreById(int serviceCentreId)
        {
            try
            {
                //for international, gets the country information
                var serviceCentre = await _uow.ServiceCentre.GetServiceCentresByIdForInternational(serviceCentreId);
                if (serviceCentre != null)
                {
                    return serviceCentre;
                }

                // for local information
                var centre = await _uow.ServiceCentre.GetAsync(s => s.ServiceCentreId == serviceCentreId, "Station");
                if (centre == null)
                {
                    throw new GenericException("Service Centre does not exist", $"{(int)HttpStatusCode.NotFound}");
                }

                var centreDto = Mapper.Map<ServiceCentreDTO>(centre);
                centreDto.StationName = centre.Station.StationName;
                centreDto.StationCode = centre.Station.StationCode;
                return centreDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ServiceCentreDTO> GetServiceCentreByIdForInternational(int serviceCentreId)
        {
            try
            {
                var centre = await _uow.ServiceCentre.GetServiceCentresByIdForInternational(serviceCentreId);
                if (centre == null)
                {
                    throw new GenericException("Service Centre does not exist", $"{(int)HttpStatusCode.NotFound}");
                }
                return centre;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ServiceCentreDTO>> GetServiceCentres()
        {
            try
            {
                return await _uow.ServiceCentre.GetServiceCentres();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<ServiceCentreDTO> GetDefaultServiceCentre()
        {
            try
            {
                var centre = await _uow.ServiceCentre.GetAsync(s => s.IsDefault == true, "Station");
                if (centre == null)
                {
                    throw new GenericException("Error: A Default Service Center has not been configured on this system. Please see the administrator.", $"{(int)HttpStatusCode.NotFound}");
                }

                var centreDto = Mapper.Map<ServiceCentreDTO>(centre);
                centreDto.StationName = centre.Station.StationName;
                centreDto.StationCode = centre.Station.StationCode;
                return centreDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ServiceCentreDTO> GetGIGGOServiceCentre()
        {
            try
            {
                var centre = await _uow.ServiceCentre.GetAsync(s => s.Code.ToLower() == "giggo", "Station");
                if (centre == null)
                {
                    throw new GenericException("Error: A GIGGO Service Center has not been configured on this system. Please see the administrator.", $"{(int)HttpStatusCode.NotFound}");
                }

                var centreDto = Mapper.Map<ServiceCentreDTO>(centre);
                centreDto.StationName = centre.Station.StationName;
                centreDto.StationCode = centre.Station.StationCode;
                return centreDto;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<List<ServiceCentreDTO>> GetLocalServiceCentres()
        {
            try
            {
                return await _uow.ServiceCentre.GetLocalServiceCentres();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<ServiceCentreDTO>> GetLocalServiceCentres(int[] countryIds)
        {
            try
            {
                return await _uow.ServiceCentre.GetLocalServiceCentres(countryIds);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ServiceCentreDTO>> GetInternationalServiceCentres()
        {
            try
            {
                return await _uow.ServiceCentre.GetInternationalServiceCentres();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task ServiceCentreStatus(int serviceCentreId, bool status)
        {
            try
            {
                var centre = await _uow.ServiceCentre.GetAsync(serviceCentreId);
                if (centre == null)
                {
                    throw new GenericException("Service Centre does not exist", $"{(int)HttpStatusCode.NotFound}");
                }
                centre.IsActive = status;
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateServiceCentre(int serviceCentreId, ServiceCentreDTO service)
        {
            try
            {
                var centre = await _uow.ServiceCentre.GetAsync(serviceCentreId);
                if (centre == null || serviceCentreId != service.ServiceCentreId)
                {
                    throw new GenericException("Service Centre does not exist", $"{(int)HttpStatusCode.NotFound}");
                }

                //1. update the old service centre code to the new one in Number Generator Monitor if they are different
                if(centre.Code.ToLower() != service.Code.ToLower())
                {
                    var numberGenerator = await _uow.NumberGeneratorMonitor.FindAsync(x => x.ServiceCentreCode == centre.Code);

                    foreach (var number in numberGenerator)
                    {
                        number.ServiceCentreCode = service.Code;
                    }
                }

                var station = await _uow.Station.GetAsync(service.StationId);

                //2. Update the service centre details
                centre.Name = service.Name;
                centre.PhoneNumber = service.PhoneNumber;
                centre.Address = service.Address;
                centre.City = service.City;
                centre.Email = service.Email;
                centre.StationId = station.StationId;
                centre.IsActive = true;
                centre.Code = service.Code;
                centre.TargetAmount = service.TargetAmount;
                centre.TargetOrder = service.TargetOrder;
                centre.IsHUB = service.IsHUB;
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ServiceCentreDTO> GetServiceCentreForHomeDelivery(int serviceCentreId)
        {
            try
            {
                //Get input service centre
                var inputServiceCentre = await _uow.ServiceCentre.GetAsync(s => s.ServiceCentreId == serviceCentreId, "Station");
                if (inputServiceCentre == null)
                {
                    throw new GenericException("Service Centre does not exist", $"{(int)HttpStatusCode.NotFound}");
                }

                //Get Service Centre from SuperServiceCentreId
                ServiceCentreDTO serviceCentreForHomeDelivery = null;
                if (inputServiceCentre.Station.SuperServiceCentreId > 0)
                {
                    serviceCentreForHomeDelivery = await GetServiceCentreById(inputServiceCentre.Station.SuperServiceCentreId);
                }
                else
                {
                    serviceCentreForHomeDelivery = Mapper.Map<ServiceCentreDTO>(inputServiceCentre);
                };

                return serviceCentreForHomeDelivery;
            }
            catch (Exception)
            {
                throw;
                //throw new GenericException("ServiceCentreForHomeDelivery has not been configured for the Input Service Centre.");
            }
        }

        public async Task<IEnumerable<ServiceCentreDTO>> GetServiceCentresByStationId(int stationId)
        {
            try
            {
                return await _uow.ServiceCentre.GetServiceCentresByStationId(stationId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ServiceCentreDTO>> GetServiceCentresByCountryId(int countryId)
        {
            int[] countryIds = new int[] { countryId };
            return await _uow.ServiceCentre.GetLocalServiceCentres(countryIds);
        }

        public async Task<List<ServiceCentreDTO>> GetHUBServiceCentres()
        {
            try
            {
                var services = await _uow.ServiceCentre.FindAsync(x => x.IsHUB == true);
                var servicesDTO = Mapper.Map<List<ServiceCentreDTO>>(services);
                return servicesDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ServiceCentreDTO>> GetServiceCentresWithoutHUBForNonLagosStation(int usersServiceCentresId, int countryId)
        {
            try
            {
                var serviceCenterIds = await _uow.ServiceCentre.GetAsync(usersServiceCentresId);

                //Get all service centre
                int[] countryIds = new int[] { countryId };
                bool excludehub = true;
                
                if (serviceCenterIds.StationId == 4)
                {
                    excludehub = false;
                }

                return await _uow.ServiceCentre.GetServiceCentres(countryIds, excludehub);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<List<ServiceCentreDTO>> GetServiceCentresBySingleCountry(int countryId)
        {
            return await _uow.ServiceCentre.GetServiceCentresBySingleCountry(countryId);
        }
    }
}
