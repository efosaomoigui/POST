using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.IRepositories.ServiceCentres;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Linq;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Dashboard;

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
                                    SupperServiceCentreId = sc.SuperServiceCentreId,
                                    IsHUB = s.IsHUB,
                                    FormattedServiceCentreName = s.FormattedServiceCentreName,
                                    IsPublic = s.IsPublic,
                                    IsGateway = s.IsGateway
                                };

                return Task.FromResult(centreDto.OrderBy(x => x.Name).ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<ServiceCentreDTO>> GetServiceCentresWithoutStation()
        {
            try
            {
                var centres = _context.ServiceCentre;
                var centreDto = from s in centres
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
                                    IsDefault = s.IsDefault,
                                    IsHUB = s.IsHUB,
                                    IsPublic = s.IsPublic,
                                    IsGateway = s.IsGateway
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
                                    IsHUB = s.IsHUB,
                                    IsPublic = s.IsPublic,
                                    IsGateway = s.IsGateway
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
                                where c.CountryId == 1
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
                                    Latitude = s.Latitude,
                                    FormattedServiceCentreName = s.FormattedServiceCentreName,
                                    IsPublic = s.IsPublic
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
                var centres = _context.ServiceCentre.Where(s => s.IsActive == true);

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
                                        Latitude = s.Latitude,
                                        FormattedServiceCentreName = s.FormattedServiceCentreName,
                                        IsPublic = s.IsPublic
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
                                        Latitude = s.Latitude,
                                        FormattedServiceCentreName = s.FormattedServiceCentreName,
                                        IsPublic = s.IsPublic
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
                                    IsHUB = s.IsHUB,
                                    IsPublic = s.IsPublic,
                                    IsGateway = s.IsGateway
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
                                    FormattedServiceCentreName = s.FormattedServiceCentreName,
                                    IsHUB = s.IsHUB,
                                    IsGateway = s.IsGateway,
                                    IsPublic = s.IsPublic,
                                    LGAId = s.LGAId,
                                    CountryDTO = new CountryDTO
                                    {
                                        CountryId = c.CountryId,
                                        CountryCode = c.CountryCode,
                                        CountryName = c.CountryName,
                                        CurrencySymbol = c.CurrencySymbol
                                    },
                                    Longitude = s.Longitude,
                                    Latitude = s.Latitude
                                };
                return Task.FromResult(centreDto.FirstOrDefault());
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task<List<ServiceCentreDTO>> GetServiceCentresByStationId(int stationId)
        {
            try
            {
                var centres = _context.ServiceCentre.Where(x => x.StationId == stationId).ToList();
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
                                    IsHUB = s.IsHUB,
                                    Latitude = s.Latitude,
                                    Longitude = s.Longitude,
                                    FormattedServiceCentreName = s.FormattedServiceCentreName,
                                    SupperServiceCentreId = sc.SuperServiceCentreId,
                                    IsPublic = s.IsPublic
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
                                    IsHUB = s.IsHUB,
                                    FormattedServiceCentreName = s.FormattedServiceCentreName,
                                    IsPublic = s.IsPublic
                                };
                return Task.FromResult(centreDto.OrderBy(x => x.Name).ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<ServiceCentreDTO>> GetServiceCentres(int[] countryIds, bool excludeHub, int stationId)
        {
            try
            {
                var centres = _context.ServiceCentre.Where(s => s.IsActive == true);

                if(excludeHub == true)
                {
                    centres = centres.Where(x => x.IsHUB == false);
                }

                if(stationId > 0)
                {
                    centres = centres.Where(x => x.StationId != stationId);
                }

                var centreDto = from s in centres
                                join sc in _context.Station on s.StationId equals sc.StationId
                                join st in _context.State on sc.StateId equals st.StateId
                                join c in _context.Country on st.CountryId equals c.CountryId
                                join t in _context.LGA on s.LGAId equals t.LGAId
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
                                    Latitude = s.Latitude,
                                    FormattedServiceCentreName = s.FormattedServiceCentreName,
                                    IsPublic = s.IsPublic,
                                    HomeDeliveryStatus = t.HomeDeliveryStatus,
                                    IsGateway = s.IsGateway
                                };
                return Task.FromResult(centreDto.OrderBy(x => x.Name).ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<ServiceCentreDTO>> GetServiceCentresBySingleCountry(int countryId)
        {
            try
            {
                var centres = _context.ServiceCentre.Where(s => s.IsActive == true && s.IsHUB == false && s.IsPublic == true);
              var centreDto = new List<ServiceCentreDTO>();
                if (countryId == 1)
                {
                    var centreDtos = from s in centres
                                     join sc in _context.Station on s.StationId equals sc.StationId
                                     join st in _context.State on sc.StateId equals st.StateId
                                     join c in _context.Country on st.CountryId equals c.CountryId
                                     join t in _context.LGA on s.LGAId equals t.LGAId
                                     where c.CountryId == countryId
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
                                         Latitude = s.Latitude,
                                         FormattedServiceCentreName = s.FormattedServiceCentreName,
                                         IsPublic = s.IsPublic,
                                         HomeDeliveryStatus = t.HomeDeliveryStatus
                                     };
                    centreDto = centreDtos.ToList();
                }
                else
                {
                    var centreDtos = from s in centres
                                     join sc in _context.Station on s.StationId equals sc.StationId
                                     join st in _context.State on sc.StateId equals st.StateId
                                     join c in _context.Country on st.CountryId equals c.CountryId
                                     where c.CountryId == countryId
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
                                         Latitude = s.Latitude,
                                         FormattedServiceCentreName = s.FormattedServiceCentreName,
                                         IsPublic = s.IsPublic,
                                     };
                    centreDto = centreDtos.ToList();
                }
                return Task.FromResult(centreDto.OrderBy(x => x.Name).ToList());

            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<ServiceCentreDTO>> GetActiveServiceCentresBySingleCountry(int countryId, int stationId)
        {
            try
            {
                var centres = _context.ServiceCentre.Where(s => s.IsActive == true);

                if (stationId > 0)
                {
                    centres = centres.Where(x => x.StationId != stationId);
                }

                var centreDto = from s in centres
                                join sc in _context.Station on s.StationId equals sc.StationId
                                join st in _context.State on sc.StateId equals st.StateId
                                join c in _context.Country on st.CountryId equals c.CountryId
                                join t in _context.LGA on s.LGAId equals t.LGAId
                                where c.CountryId == countryId
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
                                    Latitude = s.Latitude,
                                    FormattedServiceCentreName = s.FormattedServiceCentreName,
                                    IsPublic = s.IsPublic,
                                    HomeDeliveryStatus = t.HomeDeliveryStatus
                                };
                return Task.FromResult(centreDto.OrderBy(x => x.Name).ToList());

            }
            catch (Exception)
            {
                throw;
            }
        }


        public Task<List<ServiceCentreDTO>> GetActiveServiceCentres()
        {
            try
            {
                var centres = _context.ServiceCentre.Where(s => s.IsActive == true);
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
                                    Longitude = s.Longitude,
                                    Latitude = s.Latitude,
                                    FormattedServiceCentreName = s.FormattedServiceCentreName,
                                    IsPublic = s.IsPublic
                                };
                return Task.FromResult(centreDto.OrderBy(x => x.Name).ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<ServiceCentreBreakdownDTO> GetServiceCentresData(int countryId)
        {
            try
            {
                var result = new ServiceCentreBreakdownDTO();

                var centres = _context.ServiceCentre;

                var centresData = from s in centres
                                  join sc in _context.Station on s.StationId equals sc.StationId
                                  join st in _context.State on sc.StateId equals st.StateId
                                  join c in _context.Country on st.CountryId equals c.CountryId
                                  where c.CountryId == countryId
                                  select s;

                var walkInCenters = centresData.Where(x => x.IsHUB == false && x.IsGateway == false && x.IsActive == true && x.IsPublic == true).Count();
                var hubs = centresData.Where(x => x.IsHUB == true && x.IsGateway == false).Count();
                var gateway = centresData.Where(x => x.IsGateway == true && x.IsHUB == false).Count();

                result.Total = walkInCenters + hubs + gateway;
                result.WalkIn = walkInCenters;
                result.Hub = hubs;
                result.Gateway = gateway;

                return Task.FromResult(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}