using System;
using System.Threading.Tasks;
using POST.Core.DTO.JobCards;
using POST.Core.IServices.JobCards;

namespace POST.Services.Implementation.JobCards
{
    public class JobCardManagementPartService : IJobCardManagementPartService
    {
        public Task<object> AddJobCardManagementPart(JobCardManagementPartDTO JobCardDTO)
        {
            throw new NotImplementedException();
        }

        public Task DeleteJobCardManagementPart(int JobCardId)
        {
            throw new NotImplementedException();
        }

        public Task<JobCardManagementPartDTO> GetJobCardManagementPartById(int jobCardId)
        {
            throw new NotImplementedException();
        }

        public Task<JobCardManagementPartDTO> GetJobCardManagementParts()
        {
            throw new NotImplementedException();
        }

        public Task UpdateJobCardManagementPart(int JobCardId, JobCardManagementPartDTO JobCardDTO)
        {
            throw new NotImplementedException();
        }
    }
}
