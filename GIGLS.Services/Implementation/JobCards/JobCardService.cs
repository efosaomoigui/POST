using System;
using System.Threading.Tasks;
using POST.Core.DTO.JobCards;
using POST.Core.IServices.JobCards;

namespace POST.Services.Implementation.JobCards
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
