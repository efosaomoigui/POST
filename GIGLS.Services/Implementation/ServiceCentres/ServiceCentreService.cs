﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core;
using GIGL.GIGLS.Core.Domain;
using AutoMapper;
using GIGLS.Core.IServices.ServiceCentres;
using GIGLS.Core.Domain;
using GIGLS.Infrastructure;

namespace GIGLS.Services.IServices.ServiceCentres
{
    public class ServiceCentreService : IServiceCentreService
    {
        private readonly IUnitOfWork _uow;

        public ServiceCentreService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<object> AddServiceCentre(ServiceCentreDTO service)
        {
            try
            {
                if (!await _uow.Station.ExistAsync(c => c.StationId == service.StationId))
                {
                    throw new GenericException("STATION_SELECTED_DOES_NOT_EXIST");
                }

                service.Name = service.Name.Trim();
                service.Name = service.Name.ToLower();
                service.Code = service.Code.Trim();
                service.Code = service.Code.ToLower();

                if (await _uow.ServiceCentre.ExistAsync(c => c.Name.ToLower() == service.Name || c.Code.ToLower() == service.Code))
                {
                    throw new GenericException($"{service.Name} Service Centre Already Exist");
                }
                //var newCentre = new ServiceCentre
                //{
                //    Name = service.Name,
                //    Address = service.Address,
                //    City = service.City,
                //    Email = service.Email,
                //    StationId = service.StationId,
                //    PhoneNumber = service.PhoneNumber,                    
                //    IsActive = true,
                //    Code = service.Code
                //    //user loggen on Id here
                //};
                var newCentre = Mapper.Map<ServiceCentre>(service);
                _uow.ServiceCentre.Add(newCentre);
                await _uow.CompleteAsync();
                return new { Id = newCentre.ServiceCentreId};
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
                    throw new GenericException("Service Centre does not exist");
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
                    throw new GenericException("Service Centre does not exist");
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

        public async Task<ServiceCentreDTO> GetServiceCentreById(int serviceCentreId)
        {
            try
            {
                var centre = await _uow.ServiceCentre.GetAsync( s => s.ServiceCentreId == serviceCentreId, "Station");
                if (centre == null)
                {
                    throw new GenericException("Service Centre does not exist");
                }

                var centreDto =  Mapper.Map<ServiceCentreDTO>(centre);
                centreDto.StationName = centre.Station.StationName;
                centreDto.StationCode = centre.Station.StationCode;
                return centreDto;
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
                    throw new GenericException("Service Centre does not exist");
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
                    throw new GenericException("Service Centre does not exist");
                }

                centre.Name = service.Name;
                centre.PhoneNumber = service.PhoneNumber;
                centre.Address = service.Address;
                centre.City = service.City;
                centre.Email = service.Email;
                centre.StationId = service.StationId;
                centre.IsActive = true;
                centre.Code = service.Code;
                centre.TargetAmount = service.TargetAmount;
                centre.TargetOrder = service.TargetOrder;
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
