using System;
using System.Threading.Tasks;
using GIGLS.Core.DTO.JobCards;
using GIGLS.Core.IServices.JobCards;

namespace GIGLS.Services.Implementation.JobCards
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
