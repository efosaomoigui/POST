using System;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Workshops;
using GIGLS.Core.IServices.Workshops;

namespace GIGLS.Services.Implementation.Workshops
{
    public class WorkshopService : IWorkshopService
    {
        public Task<object> AddWorkshop(WorkshopDTO workshop)
        {
            throw new NotImplementedException();
        }

        public Task DeleteWorkshop(int workshopId)
        {
            throw new NotImplementedException();
        }

        public Task<WorkshopDTO> GetWorkshopById(int workshopId)
        {
            throw new NotImplementedException();
        }

        public Task<WorkshopDTO> GetWorkshops()
        {
            throw new NotImplementedException();
        }

        public Task UpdateWorkshop(int workshopId, WorkshopDTO workshop)
        {
            throw new NotImplementedException();
        }
    }
}
