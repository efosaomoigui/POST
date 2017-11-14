using System;
using System.Threading.Tasks;
using GIGLS.Core.DTO.JobCards;
using GIGLS.Core.IServices.JobCards;

namespace GIGLS.Services.Implementation.JobCards
{
    public class JobCardService : IJobCardService
    {
        public Task<object> AddJobCard(JobCardDTO JobCardDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteJobCard(int JobCardId)
        {
            throw new NotImplementedException();
        }

        public Task<JobCardDTO> GetJobCardById(int JobCardId)
        {
            throw new NotImplementedException();
        }

        public Task<JobCardDTO> GetJobCards()
        {
            throw new NotImplementedException();
        }

        public Task UpdateJobCard(int JobCardId, JobCardManagementPartDTO JobCardDTO)
        {
            throw new NotImplementedException();
        }
    }
}
