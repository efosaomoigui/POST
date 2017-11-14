using GIGLS.Core.DTO.JobCards;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.JobCards
{
    public interface IJobCardManagementPartService : IServiceDependencyMarker
    {
        Task<JobCardManagementPartDTO> GetJobCardManagementParts();
        Task<JobCardManagementPartDTO> GetJobCardManagementPartById(int jobCardId);
        Task<object> AddJobCardManagementPart(JobCardManagementPartDTO JobCardDTO);
        Task UpdateJobCardManagementPart(int JobCardId, JobCardManagementPartDTO JobCardDTO);
        Task DeleteJobCardManagementPart(int JobCardId);
    }
}
