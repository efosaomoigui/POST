using GIGLS.Core.DTO.JobCards;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.JobCards
{
    public interface IJobCardService : IServiceDependencyMarker
    {
        Task<JobCardDTO> GetJobCards();
        Task<JobCardDTO> GetJobCardById(int JobCardId);
        Task<object> AddJobCard(JobCardDTO JobCardDto);
        Task UpdateJobCard(int JobCardId, JobCardManagementPartDTO JobCardDTO);
        Task DeleteJobCard(int JobCardId);
    }
}
