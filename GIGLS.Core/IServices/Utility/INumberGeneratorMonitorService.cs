using GIGLS.Core.IServices;
using GIGLS.Core.Enums;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Utility
{
    public interface INumberGeneratorMonitorService : IServiceDependencyMarker
    {
        Task<string> GenerateNextNumber(NumberGeneratorType numberGeneratorType, string serviceCenterCode);
        Task<string> GenerateNextNumber(NumberGeneratorType numberGeneratorType);
    }
}
