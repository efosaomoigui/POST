using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Shipments;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIGL.GIGLS.Core.Domain;
using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.DTO.Shipments;
using GIGLS.CORE.DTO.Shipments;
using System.Linq.Expressions;

namespace GIGLS.Infrastructure.Persistence.Repositories.Shipments
{
    public class PreShipmentMobileRepository : Repository<PreShipmentMobile, GIGLSContext>, IPreShipmentMobileRepository
    {
        private GIGLSContext _context;
        public PreShipmentMobileRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<PreShipmentMobileDTO>> GetPreShipmentsForMobile()
        {
            try
            {
                var preShipmentMobile = _context.PresShipmentMobile.AsQueryable();

                
                List<PreShipmentMobileDTO> preShipmentDto = new List<PreShipmentMobileDTO>();
                //filter
                 preShipmentDto = (from r in preShipmentMobile
                                      select new PreShipmentMobileDTO()
                                      {
                                          PreShipmentMobileId = r.PreShipmentMobileId,
                                          Waybill = r.Waybill,
                                          CustomerType = r.CustomerType,
                                          ActualDateOfArrival = r.ActualDateOfArrival,
                                          DateCreated = r.DateCreated,
                                          DateModified = r.DateModified,
                                          ExpectedDateOfArrival = r.ExpectedDateOfArrival,
                                          ReceiverAddress = r.ReceiverAddress,
                                          SenderAddress = r.SenderAddress,
                                          ReceiverCity = r.ReceiverCity,
                                          ReceiverCountry = r.ReceiverCountry,
                                          ReceiverEmail = r.ReceiverEmail,
                                          ReceiverName = r.ReceiverName,
                                          ReceiverPhoneNumber = r.ReceiverPhoneNumber,
                                          ReceiverState = r.ReceiverState,
                                          SenderName = r.SenderName,
                                          UserId = r.UserId,
                                          Value = r.Value,
                                          GrandTotal = r.GrandTotal,
                                          DeliveryPrice = r.DeliveryPrice,
                                          CalculatedTotal = r.CalculatedTotal,
                                          InsuranceValue = r.InsuranceValue,
                                          DiscountValue = r.DiscountValue,
                                          CompanyType = r.CompanyType,
                                          CustomerCode = r.CustomerCode
                                      }).OrderByDescending(x => x.DateCreated).Take(20).ToList();

                    return Task.FromResult(preShipmentDto.OrderByDescending(x => x.DateCreated).ToList());
           }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
