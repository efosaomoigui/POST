using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IRepositories.Shipments;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Linq;
using GIGLS.Core.DTO.ServiceCentres;

namespace GIGLS.Infrastructure.Persistence.Repositories.Shipments
{
    public class MissingShipmentRepository : Repository<MissingShipment, GIGLSContext>, IMissingShipmentRepository
    {
        private GIGLSContext _context;

        public MissingShipmentRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<MissingShipmentDTO>> GetMissingShipments()
        {
            var missings = _context.MissingShipment.AsQueryable();

            List<MissingShipmentDTO> missingDto = (from m in missings
                                                   select new MissingShipmentDTO()
                                                   {
                                                       MissingShipmentId = m.MissingShipmentId,
                                                       Comment = m.Comment,
                                                       Feedback = m.Feedback,
                                                       Reason = m.Reason,
                                                       SettlementAmount = m.SettlementAmount,
                                                       Waybill = m.Waybill,
                                                       Status = m.Status,
                                                       DateModified = m.DateModified,
                                                       DateCreated = m.DateCreated,
                                                       CreatedBy = Context.Users.Where(u => u.Id == m.CreatedBy).Select(s => s.LastName + " " + s.FirstName).FirstOrDefault(),
                                                       ResolvedBy = Context.Users.Where(u => u.Id == m.ResolvedBy).Select(s => s.LastName + " " + s.FirstName).FirstOrDefault(),
                                                       ServiceCentreId = m.ServiceCentreId,
                                                       ServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == m.ServiceCentreId).Select(x => new ServiceCentreDTO
                                                       {
                                                           Code = x.Code,
                                                           Name = x.Name
                                                       }).FirstOrDefault()
                                                   }).OrderByDescending(x =>x.DateCreated).ToList();

            return Task.FromResult(missingDto);
        }

        public Task<MissingShipmentDTO> GetMissingShipmentById(int missingShipmentId)
        {
            var missing = _context.MissingShipment.Where(x => x.MissingShipmentId == missingShipmentId);

            MissingShipmentDTO missingDto = (from m in missing
                                             select new MissingShipmentDTO()
                                             {
                                                 MissingShipmentId = m.MissingShipmentId,
                                                 Comment = m.Comment,
                                                 Feedback = m.Feedback,
                                                 Reason = m.Reason,
                                                 SettlementAmount = m.SettlementAmount,
                                                 Waybill = m.Waybill,
                                                 Status = m.Status,
                                                 DateModified = m.DateModified,
                                                 DateCreated = m.DateCreated,
                                                 CreatedBy = Context.Users.Where(u => u.Id == m.CreatedBy).Select(s => s.LastName + " " + s.FirstName).FirstOrDefault(),
                                                 ResolvedBy = Context.Users.Where(u => u.Id == m.ResolvedBy).Select(s => s.LastName + " " + s.FirstName).FirstOrDefault(),
                                                 ServiceCentreId = m.ServiceCentreId,
                                                 ServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == m.ServiceCentreId).Select(x => new ServiceCentreDTO
                                                 {
                                                     Code = x.Code,
                                                     Name = x.Name
                                                 }).FirstOrDefault()
                                             }).FirstOrDefault();

            return Task.FromResult(missingDto);
        }
    }
}
