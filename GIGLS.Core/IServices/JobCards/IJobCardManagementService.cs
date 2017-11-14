using GIGLS.Core.DTO.JobCards;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.JobCards
{
    public interface IJobCardManagementService : IServiceDependencyMarker
    {
        Task<JobCardManagementDTO> GetJobCardManagements();
        Task<JobCardManagementDTO> GetJobCardManagementById(int JobCardId);
        Task<object> AddJobCardManagement(JobCardManagementDTO JobCardManagementDTO);
        Task UpdateJobCardManagement(int JobCardId, JobCardManagementDTO JobCardDTO);
        Task DeleteJobCardManagement(int JobCardId);
    }
}
