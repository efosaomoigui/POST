using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.Utility;
using GIGLS.Infrastructure;
using System;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Utility
{
    public class NumberGeneratorMonitorService : INumberGeneratorMonitorService
    {
        private readonly IUnitOfWork _uow;
        public NumberGeneratorMonitorService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<string> GenerateNextNumber(NumberGeneratorType numberGeneratorType, string serviceCenterCode)
        {
            try
            {
                string numberGenerated = null;
                //1. Get the last numberGenerated for the serviceCenter and numberGeneratorType
                //from the NumberGeneratorMonitor Table
                var monitor = _uow.NumberGeneratorMonitor.SingleOrDefault(x =>
                    x.ServiceCentreCode == serviceCenterCode && x.NumberGeneratorType == numberGeneratorType);


                // At this point, monitor can only be null if it's the first time we're
                // creating a number for the service centre and numberGeneratorType. 
                //If that's the case, we assume our numberCode to be "0".
                var numberCode = monitor?.Number ?? "0";

                //2. Increment lastcode to get the next available numberCode by 1
                var number = long.Parse(numberCode) + 1;
                var numberStr = number.ToString("00000000");

                //Add or update the NumberGeneratorMonitor Table for the Service Centre and numberGeneratorType
                if (monitor != null)
                {
                    await UpdateNumberGeneratorMonitor(serviceCenterCode, numberGeneratorType, numberStr);
                }
                else
                {
                    await AddNumberGeneratorMonitor(serviceCenterCode, numberGeneratorType, numberStr);
                }

                //pad the service centre
                var serviceCenter = await _uow.ServiceCentre.GetAsync(s => s.Code == serviceCenterCode);
                var serviceCentreId = (serviceCenter == null) ? 0 : serviceCenter.ServiceCentreId;
                var codeStr = serviceCentreId.ToString("000");

                //Add the numberCode with the serviceCenterCode and numberGeneratorType
                numberGenerated = ResolvePrefixFromNumberGeneratorType(numberGeneratorType) + codeStr + numberStr;
                return numberGenerated;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> GenerateNextNumber(NumberGeneratorType numberGeneratorType)
        {
            var defaultServiceCenterCode = "SYS";
            return await GenerateNextNumber(numberGeneratorType, defaultServiceCenterCode);
        }


        private async Task AddNumberGeneratorMonitor(string serviceCenterCode, NumberGeneratorType numberGeneratorType, string number)
        {
            try
            {
                _uow.NumberGeneratorMonitor.Add(new NumberGeneratorMonitor
                {
                    ServiceCentreCode = serviceCenterCode,
                    NumberGeneratorType = numberGeneratorType,
                    Number = number
                });
                await _uow.CompleteAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task UpdateNumberGeneratorMonitor(string serviceCenterCode, NumberGeneratorType numberGeneratorType, string number)
        {
            try
            {
                var monitor = _uow.NumberGeneratorMonitor.SingleOrDefault(x =>
                    x.ServiceCentreCode == serviceCenterCode && x.NumberGeneratorType == numberGeneratorType);

                if (monitor == null)
                {
                    throw new GenericException("NumberGeneratorMonitor failed to update!");
                }
                monitor.Number = number;
                await _uow.CompleteAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }


        private int ResolvePrefixFromNumberGeneratorType(NumberGeneratorType numberGeneratorType)
        {
            switch (numberGeneratorType)
            {
                case NumberGeneratorType.WaybillNumber:
                    {
                        return (int)NumberGeneratorType.WaybillNumber;
                    }
                case NumberGeneratorType.GroupWaybillNumber:
                    {
                        return (int)NumberGeneratorType.GroupWaybillNumber;
                    }
                case NumberGeneratorType.Manifest:
                    {
                        return (int)NumberGeneratorType.Manifest;
                    }
                case NumberGeneratorType.Invoice:
                    {
                        return (int)NumberGeneratorType.Invoice;
                    }
                case NumberGeneratorType.Wallet:
                    {
                        return (int)NumberGeneratorType.Wallet;
                    }
                default:
                    {
                        return (int)NumberGeneratorType.WaybillNumber;
                    }
            }
        }

    }
}
