using GIGLS.Core.IServices;
using System.Threading.Tasks;
using GIGLS.Core.DTO;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System.Web.Http;
using GIGLS.Core.DTO.Captains;
using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using GIGLS.CORE.DTO.Report;
using GIGLS.Services.Implementation.Shipments;

namespace GIGLS.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/captain")]
    public class CaptainController : BaseWebApiController
    {

        private readonly ICaptainService _captainService;

        public CaptainController(ICaptainService captainService) : base(nameof(CaptainController))
        {
            _captainService = captainService;
        }

        //[GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("register")]
        public async Task<IServiceResponse<object>> RegisterCaptain(RegCaptainDTO captainDTO)
        {
            byte[] bytes = Convert.FromBase64String(captainDTO.PictureUrl);
            
            //Save to AzureBlobStorage
            var picUrl = await AzureBlobServiceUtil.UploadAsync(bytes, $"{captainDTO.FirstName}-{captainDTO.LastName}.png");
            captainDTO.PictureUrl = picUrl;
            

            return await HandleApiOperationAsync(async () =>
            {
                var result = await _captainService.RegisterCaptainAsync(captainDTO);

                return new ServiceResponse<object>
                {
                    Object = result
                };
            });
        }

        //[GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("register/captains/inrange")]
        public async Task<IServiceResponse<object>> RegisterCaptainsInRange(List<RegCaptainDTO> captainsDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _captainService.RegisterCaptainsInRangeAsync(captainsDto);

                return new ServiceResponse<object>
                {
                    Object = result
                };
            });
        }

        //[GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{date}")]
        public async Task<IServiceResponse<object>> GetAllCaptainsByDate(DateTime date)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _captainService.GetCaptainsByDateAsync(date);

                return new ServiceResponse<object>
                {
                    Object = result
                };
            });
        }

        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<object>> GetAllCaptainsByDate()
        {
            
            return await HandleApiOperationAsync(async () =>
            {
                DateTime? date = null;
                var result = await _captainService.GetCaptainsByDateAsync(date);

                return new ServiceResponse<object>
                {
                    Object = result
                };
            });
        }

        [HttpGet]
        [Route("captainid/{captainId}")]
        public async Task<IServiceResponse<object>> GetCaptainById(int captainId)
        {

            return await HandleApiOperationAsync(async () =>
            {
                var result = await _captainService.GetCaptainByIdAsync(captainId);

                return new ServiceResponse<object>
                {
                    Object = result
                };
            });
        }

        [HttpDelete]
        [Route("{captainId}")]
        public async Task<IServiceResponse<bool>> DeleteCaptainById(int captainId)
        {

            return await HandleApiOperationAsync(async () =>
            {
                await _captainService.DeleteCaptainByIdAsync(captainId);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [HttpPut]
        [Route("")]
        public async Task<IServiceResponse<bool>> EditCaptain(UpdateCaptainDTO captainInfo)
        {
            if (captainInfo.PictureUrl != "default.jpg" && !string.IsNullOrEmpty(captainInfo.PictureUrl) && !string.IsNullOrEmpty(captainInfo.PictureUrl))
            {
                var picsCheck = captainInfo.PictureUrl.Split(':')[0];
                if (picsCheck.ToLower().Trim() != "https")
                {
                    byte[] bytes = Convert.FromBase64String(captainInfo.PictureUrl);

                    //Save to AzureBlobStorage
                    var picUrl = await AzureBlobServiceUtil.UploadAsync(bytes, $"{captainInfo.FirstName}-{captainInfo.LastName}-updated.png");
                    captainInfo.PictureUrl = picUrl;
                }
            }

            return await HandleApiOperationAsync(async () =>
            {
                await _captainService.EditCaptainAsync(captainInfo);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [HttpPost]
        [Route("register/vehicle")]
        public async Task<IServiceResponse<object>> RegisterVehicle(RegisterVehicleDTO vehicleDTO)
        {
            vehicleDTO.PartnerEmail = vehicleDTO.AssignedCaptain;
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _captainService.RegisterVehicleAsync(vehicleDTO);

                return new ServiceResponse<object>
                {
                    Object = result
                };
            });
        }

        [HttpPost]
        [Route("register/vehicleinrange")]
        public async Task<IServiceResponse<object>> RegisterVehicleInRange(List<RegisterVehicleDTO> vehicleDTO)
        {
            //vehicleDTO.PartnerEmail = vehicleDTO.AssignedCaptain;
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _captainService.RegisterVehicleInRangeAsync(vehicleDTO);

                return new ServiceResponse<object>
                {
                    Object = result
                };
            });
        }

        [HttpGet]
        [Route("allcaptains")]
        public async Task<IServiceResponse<object>> GetAllCaptains()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _captainService.GetAllCaptainsAsync();

                return new ServiceResponse<object>
                {
                    Object = result
                };
            });
        }

        [HttpGet]
        [Route("allcaptains/paginated/{currentPage}/{pageSize}")]
        public async Task<IServiceResponse<object>> GetAllCaptainsPaginated(int currentPage, int pageSize)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _captainService.GetAllCaptainsPaginatedAsync(currentPage, pageSize);

                return new ServiceResponse<object>
                {
                    Object = result
                };
            });
        }

        [HttpGet]
        [Route("vehicles/bydate")]
        public async Task<IServiceResponse<object>> GetAllVehiclesByDate()
        {
            DateTime? date = null;
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _captainService.GetVehiclesByDateAsync(date);

                return new ServiceResponse<object>
                {
                    Object = result
                };
            });
        }

        [HttpGet]
        [Route("vehicles/bydate/{date}")]
        public async Task<IServiceResponse<object>> GetAllVehiclesByDate(DateTime? date = null)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _captainService.GetVehiclesByDateAsync(date);

                return new ServiceResponse<object>
                {
                    Object = result
                };
            });
        }
        
        [HttpGet]
        [Route("vehicles/{fleetId}")]
        public async Task<IServiceResponse<object>> GetVehicleById(int fleetId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _captainService.GetVehicleByIdAsync(fleetId);

                return new ServiceResponse<object>
                {
                    Object = result
                };
            });
        }

        [HttpPut]
        [Route("vehicles")]
        public async Task<IServiceResponse<object>> EditVehicleById(VehicleDetailsDTO vehicle)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _captainService.EditVehicleAsync(vehicle);

                return new ServiceResponse<object>
                {
                    Object = result
                };
            });
        }

        [HttpDelete]
        [Route("vehicles/{fleetId}")]
        public async Task<IServiceResponse<object>> DeleteVehicleById(int fleetId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _captainService.DeleteVehicleByIdAsync(fleetId);

                return new ServiceResponse<object>
                {
                    Object = result
                };
            });
        }

        [HttpGet]
        [Route("vehicles")]
        public async Task<IServiceResponse<object>> GetAllVehicle()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _captainService.GetAllVehiclesAsync();

                return new ServiceResponse<object>
                {
                    Object = result
                };
            });
        }

        [HttpGet]
        [Route("vehicles/byregnum/{regnum}")]
        public async Task<IServiceResponse<object>> GetVehicleByRegistrationNumber(string regnum)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _captainService.GetVehicleByRegistrationNumberAsync(regnum);

                return new ServiceResponse<object>
                {
                    Object = result
                };
            });
        }

        [HttpGet]
        [Route("vehiclesanalytics/{vehiclenumber}")]
        public async Task<IServiceResponse<object>> GetVehicleAnalytics(string vehiclenumber)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _captainService.GetVehicleAnalyticsAsync(vehiclenumber);

                return new ServiceResponse<object>
                {
                    Object = result
                };
            });
        }
        
        [HttpPost]
        [Route("getvehicles/bydaterange")]
        public async Task<IServiceResponse<object>> GetVehiclesByDateRange(DateFilterCriteria dateRange)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _captainService.GetVehiclesByDateRangeAsync(dateRange);

                return new ServiceResponse<object>
                {
                    Object = result
                };
            });
        }
        
        [HttpPost]
        [Route("getcaptains/bydaterange")]
        public async Task<IServiceResponse<object>> GetCaptainsByDateRange(DateFilterCriteria dateRange)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _captainService.GetCaptainsByDateRangeAsync(dateRange);

                return new ServiceResponse<object>
                {
                    Object = result
                };
            });
        }
    }
}