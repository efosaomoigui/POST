using System;
using System.Threading.Tasks;
using GIGLS.Core.DTO.JobCards;
using GIGLS.Core.IServices.JobCards;

namespace GIGLS.Services.Implementation.JobCards
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
