using System;
using System.Threading.Tasks;
using POST.Core.DTO.JobCards;
using POST.Core.IServices.JobCards;

namespace POST.Services.Implementation.JobCards
{
    public class JobCardManagementService : IJobCardManagementService
    {
        public Task<object> AddJobCardManagement(JobCardManagementDTO JobCardManagementDTO)
        {
            throw new NotImplementedException();
        }

        public Task DeleteJobCardManagement(int JobCardId)
        {
            throw new NotImplementedException();
        }

        public Task<JobCardManagementDTO> GetJobCardManagementById(int JobCardId)
        {
            throw new NotImplementedException();
        }

        public Task<JobCardManagementDTO> GetJobCardManagements()
        {
            throw new NotImplementedException();
        }

        public Task UpdateJobCardManagement(int JobCardId, JobCardManagementDTO JobCardDTO)
        {
            throw new NotImplementedException();
        }
    }
}
