using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.User;
using GIGLS.Core.IRepositories;
using GIGLS.Core.IRepositories.BankSettlement;
using GIGLS.CORE.DTO.Shipments;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.Infrastructure.Persistence.Repositories.BankSettlement
{
    public class ShipmentExportRepository : Repository<ShipmentExport, GIGLSContext>, IShipmentExportRepository
    {
        private GIGLSContext _context;
        public ShipmentExportRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<ShipmentExportDTO>> GetShipmentExport()
        {
            try
            {
                var exports = _context.ShipmentExport.AsQueryable();

                List<ShipmentExportDTO> dtos = new List<ShipmentExportDTO>();

                dtos = (from r in exports
                        select new ShipmentExportDTO()
                                  {
                                    ShipmentExportId = r.ShipmentExportId,
                                    RequestNumber = r.RequestNumber,
                                    ItemUniqueNo = r.ItemUniqueNo,
                                    Quantity = r.Quantity,
                                    Weight = r.Weight,
                                    DateCreated = r.DateCreated,
                                    CourierService = r.CourierService,   
                                    IsExported = r.IsExported,
                                    Length = r.Length,
                                    Width = r.Width,
                                    Height = r.Height,
                                    ItemName = r.ItemName,
                                    ItemRequestCode = r.ItemRequestCode ,
                                    GrandTotal = r.GrandTotal,
                                    Description = r.Description,
                                    DeclaredValue = r.DeclaredValue
                        }).OrderByDescending(x => x.DateCreated).ToList();

                return Task.FromResult(dtos.OrderByDescending(x => x.DateCreated).ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<ShipmentExportDTO>> GetShipmentExportNotYetExported()
        {
            try
            {
                var exports = _context.ShipmentExport.AsQueryable().Where(x => !x.IsExported);

                List<ShipmentExportDTO> dtos = new List<ShipmentExportDTO>();

                dtos = (from r in exports
                        select new ShipmentExportDTO()
                        {
                            ShipmentExportId = r.ShipmentExportId,
                            RequestNumber = r.RequestNumber,
                            ItemUniqueNo = r.ItemUniqueNo,
                            Quantity = r.Quantity,
                            Weight = r.Weight,
                            DateCreated = r.DateCreated,
                            CourierService = r.CourierService,
                            IsExported = r.IsExported,
                            Waybill = r.Waybill,
                            Length = r.Length,
                            Width = r.Width,
                            Height = r.Height,
                            ItemName = r.ItemName,
                            ItemRequestCode = r.ItemRequestCode,
                            NoOfPackageReceived = r.NoOfPackageReceived,
                            GrandTotal = r.GrandTotal,
                            Description = r.Description,
                            DeclaredValue = r.DeclaredValue
                        }).OrderByDescending(x => x.DateCreated).ToList();

                return Task.FromResult(dtos.OrderByDescending(x => x.DateCreated).ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<ShipmentExportDTO>> GetShipmentExportNotYetExported(NewFilterOptionsDto filterOptionsDto)
        {
            try
            {
                var exports = _context.ShipmentExport.AsQueryable().Where(x => !x.IsExported);
                if (filterOptionsDto != null && !String.IsNullOrEmpty(filterOptionsDto.FilterType))
                {
                    exports = _context.ShipmentExport.AsQueryable().Where(x => x.Waybill == filterOptionsDto.FilterType || x.RequestNumber == filterOptionsDto.FilterType);
                }
                else
                {
                    exports = exports.Where(x => x.DateCreated >= filterOptionsDto.StartDate && x.DateCreated <= filterOptionsDto.EndDate);
                }

                List<ShipmentExportDTO> dtos = new List<ShipmentExportDTO>();

                dtos = (from r in exports
                        select new ShipmentExportDTO()
                        {
                            ShipmentExportId = r.ShipmentExportId,
                            RequestNumber = r.RequestNumber,
                            ItemUniqueNo = r.ItemUniqueNo,
                            Quantity = r.Quantity,
                            Weight = r.Weight,
                            DateCreated = r.DateCreated,
                            CourierService = r.CourierService,
                            IsExported = r.IsExported,
                            Waybill = r.Waybill,
                            Length = r.Length,
                            Width = r.Width,
                            Height = r.Height,
                            ItemName = r.ItemName,
                            ItemRequestCode = r.ItemRequestCode,
                            NoOfPackageReceived = r.NoOfPackageReceived,
                            GrandTotal = r.GrandTotal,
                            Description = r.Description,
                            DeclaredValue = r.DeclaredValue
                            

                        }).OrderByDescending(x => x.DateCreated).ToList();

                return Task.FromResult(dtos.OrderByDescending(x => x.DateCreated).ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
