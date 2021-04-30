using GIGLS.Core.IServices;
using GIGLS.Core.Enums;
using System.Threading.Tasks;
using System;

namespace GIGLS.Core.IServices.Utility
{
    public interface INumberGeneratorMonitorService : IServiceDependencyMarker
    {
        Task<string> GenerateNextNumber(NumberGeneratorType numberGeneratorType, string serviceCenterCode);
        Task<string> GenerateNextNumber(NumberGeneratorType numberGeneratorType);
        Task<string>GenerateInvoiceRefNoWithDate(NumberGeneratorType numberGeneratorType, string customerCode, DateTime startDate, DateTime endDate);
    }
}
