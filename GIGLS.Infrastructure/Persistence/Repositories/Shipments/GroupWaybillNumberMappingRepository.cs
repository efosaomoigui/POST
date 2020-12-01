using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Shipments;
using GIGLS.CORE.DTO.Shipments;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.CORE.DTO.Report;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Shipments
{
    public class GroupWaybillNumberMappingRepository : Repository<GroupWaybillNumberMapping, GIGLSContext>, IGroupWaybillNumberMappingRepository
    {
        private GIGLSContext _context;
        public GroupWaybillNumberMappingRepository(GIGLSContext context)
            : base(context)
        {
            _context = context;
        }

        public Task<List<GroupWaybillNumberMappingDTO>> GetGroupWaybillMappings(int[] serviceCentreIds)
        {
            var groupwaybillMapping = Context.GroupWaybillNumberMapping.AsQueryable();
            
            if (serviceCentreIds.Length > 0)
            {
                groupwaybillMapping = groupwaybillMapping.Where(s => serviceCentreIds.Contains(s.DepartureServiceCentreId));
            }

            var groupwaybillMappingDto = from gw in groupwaybillMapping
                                         select new GroupWaybillNumberMappingDTO
                                         {
                                             GroupWaybillNumber = gw.GroupWaybillNumber,
                                             WaybillNumber = gw.WaybillNumber,
                                             GroupWaybillNumberMappingId = gw.GroupWaybillNumberMappingId,
                                             IsActive = gw.IsActive,
                                             DateMapped = gw.DateMapped,
                                             DateCreated = gw.DateCreated,
                                             DateModified = gw.DateModified,
                                             DepartureServiceCentreId = gw.DepartureServiceCentreId,
                                             DestinationServiceCentreId = gw.DestinationServiceCentreId,
                                             DepartureServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == gw.DepartureServiceCentreId).Select(x => new ServiceCentreDTO
                                             {
                                                 ServiceCentreId = gw.DepartureServiceCentreId,
                                                 Code = x.Code,
                                                 Name = x.Name
                                             }).FirstOrDefault(),
                                             DestinationServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == gw.DestinationServiceCentreId).Select(x => new ServiceCentreDTO
                                             {
                                                 ServiceCentreId = gw.DestinationServiceCentreId,
                                                 Code = x.Code,
                                                 Name = x.Name
                                             }).FirstOrDefault()
                                         };
            return Task.FromResult(groupwaybillMappingDto.ToList());
        }

        public Task<List<GroupWaybillNumberMappingDTO>> GetGroupWaybillMappings(int[] serviceCentreIds, DateFilterCriteria dateFilterCriteria)
        {
            //get startDate and endDate
            var queryDate = dateFilterCriteria.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;

            var groupwaybillMapping = _context.GroupWaybillNumberMapping.Where(s => s.IsDeleted == false && s.DateCreated >= startDate && s.DateCreated < endDate).AsQueryable();
            
            if (serviceCentreIds.Length > 0)
            {
                groupwaybillMapping = groupwaybillMapping.Where(s => serviceCentreIds.Contains(s.DepartureServiceCentreId));
            }

            var groupwaybillMappingDto = from gw in groupwaybillMapping
                                         select new GroupWaybillNumberMappingDTO
                                         {
                                             GroupWaybillNumber = gw.GroupWaybillNumber,
                                             WaybillNumber = gw.WaybillNumber,
                                             GroupWaybillNumberMappingId = gw.GroupWaybillNumberMappingId,
                                             IsActive = gw.IsActive,
                                             DateMapped = gw.DateMapped,
                                             DateCreated = gw.DateCreated,
                                             DateModified = gw.DateModified,
                                             DepartureServiceCentreId = gw.DepartureServiceCentreId,
                                             DestinationServiceCentreId = gw.DestinationServiceCentreId,
                                             DepartureServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == gw.DepartureServiceCentreId).Select(x => new ServiceCentreDTO
                                             {
                                                 ServiceCentreId = gw.DepartureServiceCentreId,
                                                 Code = x.Code,
                                                 Name = x.Name
                                             }).FirstOrDefault(),
                                             DestinationServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == gw.DestinationServiceCentreId).Select(x => new ServiceCentreDTO
                                             {
                                                 ServiceCentreId = gw.DestinationServiceCentreId,
                                                 Code = x.Code,
                                                 Name = x.Name
                                             }).FirstOrDefault()
                                         };
            return Task.FromResult(groupwaybillMappingDto.OrderByDescending(x => x.DateCreated).ToList());
        }

        public Task<List<GroupWaybillNumberMappingDTO>> GetGroupWaybillMappings(FilterOptionsDto filterOptionsDto, int[] serviceCentreIds)
        {
            var groupwaybillMapping = Context.GroupWaybillNumberMapping.AsQueryable();
            
            if (serviceCentreIds.Length > 0)
            {
                groupwaybillMapping = groupwaybillMapping.Where(s => serviceCentreIds.Contains(s.DepartureServiceCentreId));
            }

            var groupwaybillMappingDto = (from gw in groupwaybillMapping
                                          select new GroupWaybillNumberMappingDTO
                                          {
                                              GroupWaybillNumber = gw.GroupWaybillNumber,
                                              WaybillNumber = gw.WaybillNumber,
                                              GroupWaybillNumberMappingId = gw.GroupWaybillNumberMappingId,
                                              IsActive = gw.IsActive,
                                              DateMapped = gw.DateMapped,
                                              DateCreated = gw.DateCreated,
                                              DateModified = gw.DateModified,
                                              DepartureServiceCentreId = gw.DepartureServiceCentreId,
                                              DestinationServiceCentreId = gw.DestinationServiceCentreId,
                                              DepartureServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == gw.DepartureServiceCentreId).Select(x => new ServiceCentreDTO
                                              {
                                                  ServiceCentreId = gw.DepartureServiceCentreId,
                                                  Code = x.Code,
                                                  Name = x.Name
                                              }).FirstOrDefault(),
                                              DestinationServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == gw.DestinationServiceCentreId).Select(x => new ServiceCentreDTO
                                              {
                                                  ServiceCentreId = gw.DestinationServiceCentreId,
                                                  Code = x.Code,
                                                  Name = x.Name
                                              }).FirstOrDefault()
                                          }).ToList();


            //filter
            var filter = filterOptionsDto.filter;
            var filterValue = filterOptionsDto.filterValue;
            if (!string.IsNullOrEmpty(filter) && !string.IsNullOrEmpty(filterValue))
            {
                groupwaybillMappingDto = groupwaybillMappingDto.Where(s => (s.GetType().GetProperty(filter).GetValue(s)).ToString() == filterValue).ToList();
            }

            //sort
            var sortorder = filterOptionsDto.sortorder;
            var sortvalue = filterOptionsDto.sortvalue;

            if (!string.IsNullOrEmpty(sortorder) && !string.IsNullOrEmpty(sortvalue))
            {
                System.Reflection.PropertyInfo prop = typeof(Shipment).GetProperty(sortvalue);

                if (sortorder == "0")
                {
                    groupwaybillMappingDto = groupwaybillMappingDto.OrderBy(x => x.GetType().GetProperty(prop.Name).GetValue(x)).ToList();
                    //shipment = shipment.OrderBy(x => prop.Name); ;
                }
                else
                {
                    groupwaybillMappingDto = groupwaybillMappingDto.OrderByDescending(x => x.GetType().GetProperty(prop.Name).GetValue(x)).ToList();
                    //shipment = shipment.OrderByDescending(x => prop.Name); 
                }
            }

            return Task.FromResult(groupwaybillMappingDto.ToList());
        }

        public Task<List<string>> GetGroupWaybillMappingWaybills(int[] serviceCentreIds)
        {
            var groupwaybillMapping = Context.GroupWaybillNumberMapping.AsQueryable();
            
            if (serviceCentreIds.Length > 0)
            {
                groupwaybillMapping = groupwaybillMapping.Where(s => serviceCentreIds.Contains(s.DepartureServiceCentreId) || serviceCentreIds.Contains(s.OriginalDepartureServiceCentreId));
            }
            
            var groupwaybillMappingDto = groupwaybillMapping.Select(x => x.WaybillNumber).Distinct().ToList();

            return Task.FromResult(groupwaybillMappingDto);
        }
    }

    public class MovementManifestNumberMappingRepository : Repository<MovementManifestNumberMapping, GIGLSContext>, IMovementManifestNumberMappingRepository
    {
        private GIGLSContext _context;
        public MovementManifestNumberMappingRepository(GIGLSContext context) 
            : base(context)
        {
            _context = context;
        }

        public Task<List<MovementManifestNumberMappingDTO>> GetMovmentManifestMappings(int[] serviceCentreIds)
        {
            var movementmanifestMapping = Context.MovementManifestNumberMapping.AsQueryable(); 

            if (serviceCentreIds.Length > 0)
            {
                //movementmanifestMapping = movementmanifestMapping.Where(s => serviceCentreIds.Contains(s.DepartureServiceCentreId));
            }

            var movementmanifestMappingDto = from gw in movementmanifestMapping 
                                         select new MovementManifestNumberMappingDTO
                                         {
                                             MovementManifestCode = gw.MovementManifestCode,
                                             MovementManifestNumberMappingId = gw.MovementManifestNumberMappingId,
                                             DateCreated = gw.DateCreated,
                                             DateModified = gw.DateModified,
                                             //DepartureServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == gw.DepartureServiceCentreId).Select(x => new ServiceCentreDTO
                                             //{
                                             //    ServiceCentreId = gw.DepartureServiceCentreId,
                                             //    Code = x.Code,
                                             //    Name = x.Name
                                             //}).FirstOrDefault(),
                                             //DestinationServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == gw.DestinationServiceCentreId).Select(x => new ServiceCentreDTO
                                             //{
                                             //    ServiceCentreId = gw.DestinationServiceCentreId,
                                             //    Code = x.Code,
                                             //    Name = x.Name
                                             //}).FirstOrDefault()
                                         };
            return Task.FromResult(movementmanifestMappingDto.ToList()); 
        }

        public Task<List<MovementManifestNumberMappingDTO>> GetMovmentManifestMappings(int[] serviceCentreIds, DateFilterCriteria dateFilterCriteria)
        {
            //get startDate and endDate
            var queryDate = dateFilterCriteria.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;

            var movementmanifestMapping = _context.MovementManifestNumberMapping.Where(s => s.IsDeleted == false && s.DateCreated >= startDate && s.DateCreated < endDate).AsQueryable();

            if (serviceCentreIds.Length > 0)
            {
                //movementmanifestMapping = movementmanifestMapping.Where(s => serviceCentreIds.Contains(s.DepartureServiceCentreId));
            }

            var movementmanifestMappingDto = from gw in movementmanifestMapping 
                                         select new MovementManifestNumberMappingDTO
                                         {
                                             MovementManifestCode = gw.MovementManifestCode,
                                             MovementManifestNumberMappingId = gw.MovementManifestNumberMappingId,
                                             DateCreated = gw.DateCreated,
                                             DateModified = gw.DateModified,
                                             //DepartureServiceCentreId = gw.DepartureServiceCentreId,
                                             //DestinationServiceCentreId = gw.DestinationServiceCentreId,
                                             //DepartureServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == gw.DepartureServiceCentreId).Select(x => new ServiceCentreDTO
                                             //{
                                             //    ServiceCentreId = gw.DepartureServiceCentreId,
                                             //    Code = x.Code,
                                             //    Name = x.Name
                                             //}).FirstOrDefault(),
                                             //DestinationServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == gw.DestinationServiceCentreId).Select(x => new ServiceCentreDTO
                                             //{
                                             //    ServiceCentreId = gw.DestinationServiceCentreId,
                                             //    Code = x.Code,
                                             //    Name = x.Name
                                             //}).FirstOrDefault()
                                         };
            return Task.FromResult(movementmanifestMappingDto.OrderByDescending(x => x.DateCreated).ToList());
        }

        public Task<List<MovementManifestNumberMappingDTO>> GetMovmentManifestMappings(FilterOptionsDto filterOptionsDto, int[] serviceCentreIds)
        {
            var movementmanifestMapping = Context.MovementManifestNumberMapping.AsQueryable();

            if (serviceCentreIds.Length > 0)
            {
                //movementmanifestMapping = movementmanifestMapping.Where(s => serviceCentreIds.Contains(s.DepartureServiceCentreId));
            }


            var movementmanifestMappingDto = (from gw in movementmanifestMapping
                                          select new MovementManifestNumberMappingDTO
                                          {
                                              MovementManifestCode = gw.MovementManifestCode,
                                              MovementManifestNumberMappingId = gw.MovementManifestNumberMappingId,
                                              DateCreated = gw.DateCreated,
                                              DateModified = gw.DateModified,
                                              //DepartureServiceCentreId = gw.DepartureServiceCentreId,
                                              //DestinationServiceCentreId = gw.DestinationServiceCentreId,
                                              //DepartureServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == gw.DepartureServiceCentreId).Select(x => new ServiceCentreDTO
                                              //{
                                              //    ServiceCentreId = gw.DepartureServiceCentreId,
                                              //    Code = x.Code,
                                              //    Name = x.Name
                                              //}).FirstOrDefault(),
                                              //DestinationServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == gw.DestinationServiceCentreId).Select(x => new ServiceCentreDTO
                                              //{
                                              //    ServiceCentreId = gw.DestinationServiceCentreId,
                                              //    Code = x.Code,
                                              //    Name = x.Name
                                              //}).FirstOrDefault()
                                          }).ToList();


            //filter
            var filter = filterOptionsDto.filter;
            var filterValue = filterOptionsDto.filterValue;
            if (!string.IsNullOrEmpty(filter) && !string.IsNullOrEmpty(filterValue))
            {
                movementmanifestMappingDto = movementmanifestMappingDto.Where(s => (s.GetType().GetProperty(filter).GetValue(s)).ToString() == filterValue).ToList();
            }

            //sort
            var sortorder = filterOptionsDto.sortorder;
            var sortvalue = filterOptionsDto.sortvalue;

            if (!string.IsNullOrEmpty(sortorder) && !string.IsNullOrEmpty(sortvalue))
            {
                System.Reflection.PropertyInfo prop = typeof(Shipment).GetProperty(sortvalue);

                if (sortorder == "0")
                {
                    movementmanifestMappingDto = movementmanifestMappingDto.OrderBy(x => x.GetType().GetProperty(prop.Name).GetValue(x)).ToList();
                    //shipment = shipment.OrderBy(x => prop.Name); ;
                }
                else
                {
                    movementmanifestMappingDto = movementmanifestMappingDto.OrderByDescending(x => x.GetType().GetProperty(prop.Name).GetValue(x)).ToList();
                    //shipment = shipment.OrderByDescending(x => prop.Name); 
                }
            }

            return Task.FromResult(movementmanifestMappingDto.ToList());
        }

        public Task<List<string>> GetMovmentManifestMappingsManifest(int[] serviceCentreIds)  
        {
            var movementmanifestMapping = Context.GroupWaybillNumberMapping.AsQueryable();

            if (serviceCentreIds.Length > 0)
            {
                movementmanifestMapping = movementmanifestMapping.Where(s => serviceCentreIds.Contains(s.DepartureServiceCentreId) || serviceCentreIds.Contains(s.OriginalDepartureServiceCentreId));
            }

            var movementmanifestMappingDto = movementmanifestMapping.Select(x => x.WaybillNumber).Distinct().ToList();

            return Task.FromResult(movementmanifestMappingDto);
        }
    }
}
