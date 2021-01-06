using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Fleets;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Fleets;
using System.Linq;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.User;
using GIGLS.Core.Enums;

namespace GIGLS.Infrastructure.Persistence.Repositories.Fleets
{
    public class DispatchRepository : Repository<Dispatch, GIGLSContext>, IDispatchRepository
    {
        private GIGLSContext _context;

        public DispatchRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<DispatchDTO>> GetDispatchAsync(int[] serviceCentreIds)
        {
            try
            {
                var dispatchs = _context.Dispatch.AsQueryable();
                if (serviceCentreIds.Length > 0)
                {
                    dispatchs = dispatchs.Where(s => s.ServiceCentreId != null);
                    dispatchs = dispatchs.Where(s => serviceCentreIds.Contains((int)s.ServiceCentreId));
                }

                var dispatchDto = from r in dispatchs
                                  select new DispatchDTO
                                  {
                                      DispatchId = r.DispatchId,
                                      RegistrationNumber = r.RegistrationNumber,
                                      ManifestNumber = r.ManifestNumber,
                                      Amount = r.Amount,
                                      RescuedDispatchId = r.RescuedDispatchId,
                                      DriverDetail = r.DriverDetail,
                                      DispatchedBy = r.DispatchedBy,
                                      ReceivedBy = r.ReceivedBy,
                                      DispatchCategory = r.DispatchCategory,
                                      DepartureId = r.DepartureId,
                                      DestinationId = r.DestinationId,
                                      DateCreated = r.DateCreated,
                                      DateModified = r.DateModified,
                                      ServiceCentreId = r.ServiceCentreId,
                                      ServiceCentre = new ServiceCentreDTO
                                      {
                                          Code = r.ServiceCentre.Code,
                                          Name = r.ServiceCentre.Name
                                      },
                                      UserDetail = Context.Users.Where(c => c.Id == r.DriverDetail).Select(x => new UserDTO
                                      {
                                          FirstName = x.FirstName,
                                          LastName = x.LastName,
                                          Department = x.Department,
                                          Designation = x.Designation,
                                          Email = x.Email,
                                          PhoneNumber = x.PhoneNumber,
                                          Organisation = x.Organisation,
                                          PictureUrl = x.PictureUrl,
                                          Gender = x.Gender                                          
                                      }).FirstOrDefault()
                                  };
                return Task.FromResult(dispatchDto.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<DispatchDTO>> CheckForOutstandingDispatch(string driverId)
        {
            try
            {
                DateTime saturday = new DateTime(2020, 01, 25);
                var dispatch = _context.Dispatch;
                var dispatchs = from s in dispatch
                                join sc in _context.Manifest on s.ManifestNumber equals sc.ManifestCode
                                where s.DriverDetail == driverId && s.ReceivedBy == null && s.DateCreated > saturday
                                && sc.ManifestType == Core.Enums.ManifestType.Delivery
                                select new DispatchDTO
                                {
                                    DateModified = s.DateModified
                                };
                return Task.FromResult(dispatchs.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<DispatchDTO>> GetDeliveryDispatchForPartner(string userId)
        {
            try
            {
                var dispatchs = _context.Dispatch.Where(x => x.DriverDetail == userId && x.ReceivedBy == null);

                var dispatchDto = (from r in dispatchs
                                   join m in _context.Manifest on r.ManifestNumber equals m.ManifestCode
                                   where m.ManifestType == ManifestType.Delivery
                                   select new DispatchDTO
                                   {
                                       DispatchId = r.DispatchId,
                                       RegistrationNumber = r.RegistrationNumber,
                                       ManifestNumber = r.ManifestNumber,
                                       Amount = r.Amount,
                                       RescuedDispatchId = r.RescuedDispatchId,
                                       DriverDetail = r.DriverDetail,
                                       DispatchedBy = r.DispatchedBy,
                                       ServiceCentreId = r.ServiceCentreId,
                                       DepartureId = r.DepartureId,
                                       DestinationId = r.DestinationId,
                                       DateCreated = r.DateCreated,
                                       DateModified = r.DateModified,
                                       DepartureServiceCenterId = r.DepartureServiceCenterId,
                                       DestinationServiceCenterId = r.DestinationServiceCenterId,
                                       IsSuperManifest = r.IsSuperManifest
                                   }).ToList();

                return Task.FromResult(dispatchDto.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<DispatchDTO>> GetPickupForDeliveryDispatchForPartner(string userId)
        {
            try
            {
                var dispatchs = _context.Dispatch.Where(x => x.DriverDetail == userId && x.ReceivedBy == null);

                var dispatchDto = (from r in dispatchs
                                   join m in _context.PickupManifest on r.ManifestNumber equals m.ManifestCode
                                   where m.ManifestType == ManifestType.PickupForDelivery
                                   select new DispatchDTO
                                   {
                                       DispatchId = r.DispatchId,
                                       RegistrationNumber = r.RegistrationNumber,
                                       ManifestNumber = r.ManifestNumber,
                                       Amount = r.Amount,
                                       RescuedDispatchId = r.RescuedDispatchId,
                                       DriverDetail = r.DriverDetail,
                                       DispatchedBy = r.DispatchedBy,
                                       ServiceCentreId = r.ServiceCentreId,
                                       DepartureId = r.DepartureId,
                                       DestinationId = r.DestinationId,
                                       DateCreated = r.DateCreated,
                                       DateModified = r.DateModified,
                                       DepartureServiceCenterId = r.DepartureServiceCenterId,
                                       DestinationServiceCenterId = r.DestinationServiceCenterId,
                                       IsSuperManifest = r.IsSuperManifest
                                   }).ToList();

                return Task.FromResult(dispatchDto.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class MovementDispatchRepository : Repository<MovementDispatch, GIGLSContext>, IMovementDispatchRepository
    {
        private GIGLSContext _context;

        public MovementDispatchRepository(GIGLSContext context) : base(context) 
        {
            _context = context;
        }

        public Task<List<MovementDispatchDTO>> GetMovementmanifestDispatchForPartner(string userId)
        {
            try
            {
                var dispatchs = _context.MovementDispatch.Where(x => x.DriverDetail == userId && x.ReceivedBy == null);

                var dispatchDto = (from r in dispatchs
                                   join m in _context.MovementManifestNumber on r.MovementManifestNumber equals m.MovementManifestCode
                                   select new MovementDispatchDTO
                                   {
                                       DispatchId = r.DispatchId,
                                       RegistrationNumber = r.RegistrationNumber,
                                       MovementManifestNumber = r.MovementManifestNumber,
                                       Amount = r.Amount,
                                       RescuedDispatchId = r.RescuedDispatchId,
                                       DriverDetail = r.DriverDetail,
                                       DispatchedBy = r.DispatchedBy,
                                       ServiceCentreId = r.ServiceCentreId,
                                       DepartureId = r.DepartureId,
                                       DestinationId = r.DestinationId,
                                       DateCreated = r.DateCreated,
                                       DateModified = r.DateModified,
                                       DepartureServiceCenterId = r.DepartureServiceCenterId,
                                       DestinationServiceCenterId = r.DestinationServiceCenterId,
                                       IsSuperManifest = r.IsSuperManifest
                                   }).ToList();

                return Task.FromResult(dispatchDto.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<MovementDispatchDTO>> GetDispatchAsync(int[] serviceCentreIds)
        {
            try
            {
                var dispatchs = _context.MovementDispatch.AsQueryable();
                if (serviceCentreIds.Length > 0)
                {
                    dispatchs = dispatchs.Where(s => s.ServiceCentreId != null);
                    dispatchs = dispatchs.Where(s => serviceCentreIds.Contains((int)s.ServiceCentreId));
                }

                var dispatchDto = from r in dispatchs
                                  select new MovementDispatchDTO
                                  {
                                      DispatchId = r.DispatchId,
                                      RegistrationNumber = r.RegistrationNumber,
                                      MovementManifestNumber = r.MovementManifestNumber,
                                      Amount = r.Amount,
                                      RescuedDispatchId = r.RescuedDispatchId,
                                      DriverDetail = r.DriverDetail,
                                      DispatchedBy = r.DispatchedBy,
                                      ReceivedBy = r.ReceivedBy,
                                      DispatchCategory = r.DispatchCategory,
                                      DepartureId = r.DepartureId,
                                      DestinationId = r.DestinationId,
                                      DateCreated = r.DateCreated,
                                      DateModified = r.DateModified,
                                      ServiceCentreId = r.ServiceCentreId,
                                      ServiceCentre = new ServiceCentreDTO
                                      {
                                          Code = r.ServiceCentre.Code,
                                          Name = r.ServiceCentre.Name
                                      },
                                      UserDetail = Context.Users.Where(c => c.Id == r.DriverDetail).Select(x => new UserDTO
                                      {
                                          FirstName = x.FirstName,
                                          LastName = x.LastName,
                                          Department = x.Department,
                                          Designation = x.Designation,
                                          Email = x.Email,
                                          PhoneNumber = x.PhoneNumber,
                                          Organisation = x.Organisation,
                                          PictureUrl = x.PictureUrl,
                                          Gender = x.Gender
                                      }).FirstOrDefault()
                                  };
                return Task.FromResult(dispatchDto.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public Task<List<MovementDispatchDTO>> CheckForOutstandingDispatch(string driverId)
        //{
        //    try
        //    {
        //        DateTime saturday = new DateTime(2020, 01, 25);
        //        var dispatch = _context.MovementDispatch;
        //        var dispatchs = from s in dispatch
        //                        join sc in _context.MovementManifestNumber on s.MovementManifestNumber equals sc.MovementManifestCode
        //                        where s.DriverDetail == driverId && s.ReceivedBy == null && s.DateCreated > saturday
        //                        && sc.ManifestType == Core.Enums.ManifestType.Delivery
        //                        select new DispatchDTO
        //                        {
        //                            DateModified = s.DateModified
        //                        };
        //        return Task.FromResult(dispatchs.ToList());
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
    }

}
