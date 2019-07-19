using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.IRepositories.ServiceCentres;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Linq;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.ServiceCentres
{
    public class ServiceCentreRepository : Repository<ServiceCentre, GIGLSContext>, IServiceCentreRepository
    {
        private GIGLSContext _context;
        public ServiceCentreRepository(GIGLSContext context)
            : base(context)
        {
            _context = context;
        }

        public Task<List<ServiceCentreDTO>> GetServiceCentres()
        {
            try
            {
                var centres = _context.ServiceCentre;
                var centreDto = from s in centres
                                join sc in _context.Station on s.StationId equals sc.StationId
                                select new ServiceCentreDTO
                                {
                                    Name = s.Name,
                                    Address = s.Address,
                                    City = s.City,
                                    Email = s.Email,
                                    PhoneNumber = s.PhoneNumber,
                                    ServiceCentreId = s.ServiceCentreId,
                                    Code = s.Code,
                                    IsActive = s.IsActive,
                                    TargetAmount = s.TargetAmount,
                                    TargetOrder = s.TargetOrder,
                                    StationId = s.StationId,
                                    StationName = sc.StationName,
                                    StationCode = sc.StationCode,
                                    IsDefault = s.IsDefault,
                                    IsHUB = s.IsHUB
                                };
                return Task.FromResult(centreDto.OrderBy(x => x.Name).ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<ServiceCentreDTO>> GetServiceCentresForInternational()
        {
            try
            {
                var centres = _context.ServiceCentre;
                var centreDto = from s in centres
                                join sc in _context.Station on s.StationId equals sc.StationId
                                join st in _context.State on sc.StateId equals st.StateId
                                join c in _context.Country on st.CountryId equals c.CountryId
                                select new ServiceCentreDTO
                                {
                                    Name = s.Name,
                                    Address = s.Address,
                                    City = s.City,
                                    Email = s.Email,
                                    PhoneNumber = s.PhoneNumber,
                                    ServiceCentreId = s.ServiceCentreId,
                                    Code = s.Code,
                                    IsActive = s.IsActive,
                                    TargetAmount = s.TargetAmount,
                                    TargetOrder = s.TargetOrder,
                                    StationId = s.StationId,
                                    StationName = sc.StationName,
                                    StationCode = sc.StationCode,
                                    CountryId = c.CountryId,
                                    Country = c.CountryName,
                                    IsDefault = s.IsDefault,
                                    IsHUB = s.IsHUB
                                };
                return Task.FromResult(centreDto.OrderBy(x => x.Name).ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<ServiceCentreDTO>> GetLocalServiceCentres()
        {
            try
            {
                var centres = _context.ServiceCentre;
                var centreDto = from s in centres
                                join sc in _context.Station on s.StationId equals sc.StationId
                                join st in _context.State on sc.StateId equals st.StateId
                                join c in _context.Country on st.CountryId equals c.CountryId
                                where c.CountryName == "Nigeria"
                                select new ServiceCentreDTO
                                {
                                    Name = s.Name,
                                    Address = s.Address,
                                    City = s.City,
                                    Email = s.Email,
                                    PhoneNumber = s.PhoneNumber,
                                    ServiceCentreId = s.ServiceCentreId,
                                    Code = s.Code,
                                    IsActive = s.IsActive,
                                    TargetAmount = s.TargetAmount,
                                    TargetOrder = s.TargetOrder,
                                    StationId = s.StationId,
                                    StationName = sc.StationName,
                                    StationCode = sc.StationCode,
                                    CountryId = c.CountryId,
                                    Country = c.CountryName,
                                    IsDefault = s.IsDefault,
                                    Longitude = s.Longitude,
                                    Latitude = s.Latitude
                                };
                return Task.FromResult(centreDto.OrderBy(x => x.Name).ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<ServiceCentreDTO>> GetLocalServiceCentres(int[] countryIds)
        {
            try
            {
                var centres = _context.ServiceCentre;

                //1. countryIds not empty
                if(countryIds.Length > 0)
                {
                    var centreDto = from s in centres
                                    join sc in _context.Station on s.StationId equals sc.StationId
                                    join st in _context.State on sc.StateId equals st.StateId
                                    join c in _context.Country on st.CountryId equals c.CountryId
                                    where countryIds.Contains(c.CountryId)
                                    select new ServiceCentreDTO
                                    {
                                        Name = s.Name,
                                        Address = s.Address,
                                        City = s.City,
                                        Email = s.Email,
                                        PhoneNumber = s.PhoneNumber,
                                        ServiceCentreId = s.ServiceCentreId,
                                        Code = s.Code,
                                        IsActive = s.IsActive,
                                        TargetAmount = s.TargetAmount,
                                        TargetOrder = s.TargetOrder,
                                        StationId = s.StationId,
                                        StationName = sc.StationName,
                                        StationCode = sc.StationCode,
                                        CountryId = c.CountryId,
                                        Country = c.CountryName,
                                        IsDefault = s.IsDefault,
                                        Longitude = s.Longitude,
                                        Latitude = s.Latitude
                                    };
                    return Task.FromResult(centreDto.OrderBy(x => x.Name).ToList());
                }
                else
                {
                    var centreDto = from s in centres
                                    join sc in _context.Station on s.StationId equals sc.StationId
                                    join st in _context.State on sc.StateId equals st.StateId
                                    join c in _context.Country on st.CountryId equals c.CountryId
                                    select new ServiceCentreDTO
                                    {
                                        Name = s.Name,
                                        Address = s.Address,
                                        City = s.City,
                                        Email = s.Email,
                                        PhoneNumber = s.PhoneNumber,
                                        ServiceCentreId = s.ServiceCentreId,
                                        Code = s.Code,
                                        IsActive = s.IsActive,
                                        TargetAmount = s.TargetAmount,
                                        TargetOrder = s.TargetOrder,
                                        StationId = s.StationId,
                                        StationName = sc.StationName,
                                        StationCode = sc.StationCode,
                                        CountryId = c.CountryId,
                                        Country = c.CountryName,
                                        IsDefault = s.IsDefault,
                                        IsHUB = s.IsHUB,
                                        Longitude = s.Longitude,
                                        Latitude = s.Latitude
                                    };
                    return Task.FromResult(centreDto.OrderBy(x => x.Name).ToList());
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<ServiceCentreDTO>> GetInternationalServiceCentres()
        {
            try
            {
                var centres = _context.ServiceCentre;
                var centreDto = from s in centres
                                join sc in _context.Station on s.StationId equals sc.StationId
                                join st in _context.State on sc.StateId equals st.StateId
                                join c in _context.Country on st.CountryId equals c.CountryId
                                where c.CountryName != "Nigeria"
                                select new ServiceCentreDTO
                                {
                                    Name = s.Name,
                                    Address = s.Address,
                                    City = s.City,
                                    Email = s.Email,
                                    PhoneNumber = s.PhoneNumber,
                                    ServiceCentreId = s.ServiceCentreId,
                                    Code = s.Code,
                                    IsActive = s.IsActive,
                                    TargetAmount = s.TargetAmount,
                                    TargetOrder = s.TargetOrder,
                                    StationId = s.StationId,
                                    StationName = sc.StationName,
                                    StationCode = sc.StationCode,
                                    CountryId = c.CountryId,
                                    Country = c.CountryName,
                                    IsDefault = s.IsDefault,
                                    IsHUB = s.IsHUB
                                };
                return Task.FromResult(centreDto.OrderBy(x => x.Name).ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<ServiceCentreDTO> GetServiceCentresByIdForInternational(int serviceCentreId)
        {
            try
            {
                var centres = _context.ServiceCentre;
                var centreDto = from s in centres.Where(x => x.ServiceCentreId == serviceCentreId)
                                join sc in _context.Station on s.StationId equals sc.StationId
                                join st in _context.State on sc.StateId equals st.StateId
                                join c in _context.Country on st.CountryId equals c.CountryId
                                select new ServiceCentreDTO
                                {
                                    Name = s.Name,
                                    Address = s.Address,
                                    City = s.City,
                                    Email = s.Email,
                                    PhoneNumber = s.PhoneNumber,
                                    ServiceCentreId = s.ServiceCentreId,
                                    Code = s.Code,
                                    IsActive = s.IsActive,
                                    TargetAmount = s.TargetAmount,
                                    TargetOrder = s.TargetOrder,
                                    StationId = s.StationId,
                                    StationName = sc.StationName,
                                    StationCode = sc.StationCode,
                                    CountryId = c.CountryId,
                                    Country = c.CountryName,
                                    IsDefault = s.IsDefault,
                                    DateCreated = s.DateCreated,
                                    DateModified = s.DateModified,
                                    IsHUB = s.IsHUB
                                };
                return Task.FromResult(centreDto.FirstOrDefault());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<ServiceCentreDTO>> GetServiceCentresByStationId(int stationId)
        {
            try
            {
                var centres = _context.ServiceCentre.Where(x => x.StationId == stationId);
                var centreDto = from s in centres
                                join sc in _context.Station on s.StationId equals sc.StationId
                                select new ServiceCentreDTO
                                {
                                    Name = s.Name,
                                    Address = s.Address,
                                    City = s.City,
                                    Email = s.Email,
                                    PhoneNumber = s.PhoneNumber,
                                    ServiceCentreId = s.ServiceCentreId,
                                    Code = s.Code,
                                    IsActive = s.IsActive,
                                    TargetAmount = s.TargetAmount,
                                    TargetOrder = s.TargetOrder,
                                    StationId = s.StationId,
                                    StationName = sc.StationName,
                                    StationCode = sc.StationCode,
                                    IsDefault = s.IsDefault,
                                    IsHUB = s.IsHUB
                                };
                return Task.FromResult(centreDto.OrderBy(x => x.Name).ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<ServiceCentreDTO>> GetServiceCentreByCode(string[] code)
        {
            try
            {
                var centres = _context.ServiceCentre.Where(x => code.Contains(x.Code));

                var centreDto = from s in centres
                                join sc in _context.Station on s.StationId equals sc.StationId
                                select new ServiceCentreDTO
                                {
                                    Name = s.Name,
                                    Address = s.Address,
                                    City = s.City,
                                    Email = s.Email,
                                    PhoneNumber = s.PhoneNumber,
                                    ServiceCentreId = s.ServiceCentreId,
                                    Code = s.Code,
                                    IsActive = s.IsActive,
                                    TargetAmount = s.TargetAmount,
                                    TargetOrder = s.TargetOrder,
                                    StationId = s.StationId,
                                    StationName = sc.StationName,
                                    StationCode = sc.StationCode,
                                    IsDefault = s.IsDefault,
                                    IsHUB = s.IsHUB
                                };
                return Task.FromResult(centreDto.OrderBy(x => x.Name).ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}