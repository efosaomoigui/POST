using System;
using System.Threading.Tasks;
using POST.Core.DTO.Workshops;
using POST.Core.IServices.Workshops;

namespace POST.Services.Implementation.Workshops
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
