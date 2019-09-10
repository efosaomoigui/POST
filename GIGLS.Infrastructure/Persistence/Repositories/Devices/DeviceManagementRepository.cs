using GIGLS.Core.Domain.Devices;
using GIGLS.Core.DTO.Devices;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.User;
using GIGLS.Core.IRepositories.Devices;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.Infrastructure.Persistence.Repositories.Devices
{
    public class DeviceManagementRepository : Repository<DeviceManagement, GIGLSContext>, IDeviceManagementRepository
    {
        public DeviceManagementRepository(GIGLSContext context) : base(context)
        {
        }

        //Active Device Management
        public Task<List<DeviceManagementDTO>> GetActiveDeviceManagementAsync()
        {
            var managements = Context.DeviceManagement.Where(x => x.IsActive == true).AsQueryable();

            List<DeviceManagementDTO> managementDto = (from s in managements
                                       select new DeviceManagementDTO
                                       {
                                           DeviceManagementId = s.DeviceManagementId,
                                           DateCreated = s.DateCreated,
                                           DateModified = s.DateModified,
                                           IsActive = s.IsActive,
                                           DeviceId = s.DeviceId,
                                           Device = Context.Device.Where(c => c.DeviceId == s.DeviceId).Select(x => new DeviceDTO
                                           {
                                               Active = x.Active,
                                               DateCreated = x.DateCreated,
                                               DateModified = x.DateModified,
                                               Description = x.Description,
                                               DeviceId = x.DeviceId,
                                               HandStarp = x.HandStarp,
                                               Name = x.Name,
                                               PowerAdapter = x.PowerAdapter,
                                               SerialNumber = x.SerialNumber,
                                               SimCardNumber = x.SimCardNumber,
                                               UsbCable = x.UsbCable,
                                               NetworkProvider = x.NetworkProvider,
                                               IMEI = x.IMEI,
                                               IMEI2 = x.IMEI2
                                           }).FirstOrDefault(),
                                           UserId = s.UserId,
                                           User = Context.Users.Where(c => c.Id == s.UserId).Select(x => new UserDTO
                                           {
                                               FirstName = x.FirstName,
                                               LastName = x.LastName,
                                               Department = x.Department,
                                               Designation = x.Designation,
                                               Email = x.Email,
                                               PhoneNumber = x.PhoneNumber,
                                               Organisation = x.Organisation,
                                               PictureUrl = x.PictureUrl,
                                               Gender = x.Gender,
                                               SystemUserRole = x.SystemUserRole
                                           }).FirstOrDefault(),
                                           StationId = s.StationId,
                                           Location = Context.ServiceCentre.Where(c => c.ServiceCentreId == s.StationId).Select(x => new ServiceCentreDTO
                                           {
                                               Address = x.Address,
                                               Name = x.Name,
                                               Code = x.Code,
                                               ServiceCentreId = x.ServiceCentreId
                                               
                                           }).FirstOrDefault(),
                                           
                                           DataSimCardNumber = s.DataSimCardNumber
                                       }).ToList();
           
            return Task.FromResult(managementDto);
        }

        //All Device Management Assignment
        public Task<List<DeviceManagementDTO>> GetDeviceManagementAsync()
        {
            var managements = Context.DeviceManagement.AsQueryable();

            List<DeviceManagementDTO> managementDto = (from s in managements
                                                       select new DeviceManagementDTO
                                                       {
                                                           DeviceManagementId = s.DeviceManagementId,
                                                           DateCreated = s.DateCreated,
                                                           DateModified = s.DateModified,
                                                           IsActive = s.IsActive,
                                                           DeviceId = s.DeviceId,
                                                           Device = Context.Device.Where(c => c.DeviceId == s.DeviceId).Select(x => new DeviceDTO
                                                           {
                                                               Active = x.Active,
                                                               DateCreated = x.DateCreated,
                                                               DateModified = x.DateModified,
                                                               Description = x.Description,
                                                               DeviceId = x.DeviceId,
                                                               HandStarp = x.HandStarp,
                                                               Name = x.Name,
                                                               PowerAdapter = x.PowerAdapter,
                                                               SerialNumber = x.SerialNumber,
                                                               SimCardNumber = x.SimCardNumber,
                                                               UsbCable = x.UsbCable,
                                                               NetworkProvider = x.NetworkProvider,
                                                                IMEI = x.IMEI,
                                                               IMEI2 = x.IMEI2
                                                           }).FirstOrDefault(),
                                                           UserId = s.UserId,
                                                           User = Context.Users.Where(c => c.Id == s.UserId).Select(x => new UserDTO
                                                           {
                                                               FirstName = x.FirstName,
                                                               LastName = x.LastName,
                                                               Department = x.Department,
                                                               Designation = x.Designation,
                                                               Email = x.Email,
                                                               PhoneNumber = x.PhoneNumber,
                                                               Organisation = x.Organisation,
                                                               PictureUrl = x.PictureUrl,
                                                               Gender = x.Gender
                                                           }).FirstOrDefault(),
                                                           StationId = s.StationId,
                                                           Location = Context.ServiceCentre.Where(c => c.ServiceCentreId == s.StationId).Select(x => new ServiceCentreDTO
                                                           {
                                                               Address = x.Address,
                                                               Name = x.Name,
                                                               Code = x.Code,
                                                               ServiceCentreId = x.ServiceCentreId

                                                           }).FirstOrDefault(),
                                                           
                                                           DataSimCardNumber = s.DataSimCardNumber
                                                       }).ToList();

            return Task.FromResult(managementDto);
        }

        public Task<DeviceManagementDTO> GetDeviceManagementById(int deviceManagementId)
        {
            var management = Context.DeviceManagement.Where(x => x.DeviceManagementId ==  deviceManagementId);

            DeviceManagementDTO managementDto = (from s in management
                                                 select new DeviceManagementDTO
                                                 {
                                                     DeviceManagementId = s.DeviceManagementId,
                                                     DateCreated = s.DateCreated,
                                                     DateModified = s.DateModified,
                                                     IsActive = s.IsActive,
                                                     DeviceId = s.DeviceId,
                                                     Device = Context.Device.Where(c => c.DeviceId == s.DeviceId).Select(x => new DeviceDTO
                                                     {
                                                         Active = x.Active,
                                                         DateCreated = x.DateCreated,
                                                         DateModified = x.DateModified,
                                                         Description = x.Description,
                                                         DeviceId = x.DeviceId,
                                                         HandStarp = x.HandStarp,
                                                         Name = x.Name,
                                                         PowerAdapter = x.PowerAdapter,
                                                         SerialNumber = x.SerialNumber,
                                                         SimCardNumber = x.SimCardNumber,
                                                         UsbCable = x.UsbCable,
                                                         NetworkProvider = x.NetworkProvider,
                                                         IMEI = x.IMEI,
                                                         IMEI2 = x.IMEI2
                                                     }).FirstOrDefault(),
                                                     UserId = s.UserId,
                                                     User = Context.Users.Where(c => c.Id == s.UserId).Select(x => new UserDTO
                                                     {
                                                         FirstName = x.FirstName,
                                                         LastName = x.LastName,
                                                         Department = x.Department,
                                                         Designation = x.Designation,
                                                         Email = x.Email,
                                                         PhoneNumber = x.PhoneNumber,
                                                         Organisation = x.Organisation,
                                                         PictureUrl = x.PictureUrl,
                                                         Gender = x.Gender
                                                     }).FirstOrDefault(),
                                                     StationId = s.StationId,
                                                     Location = Context.ServiceCentre.Where(c => c.ServiceCentreId == s.StationId).Select(x => new ServiceCentreDTO
                                                     {
                                                         Address = x.Address,
                                                         Name = x.Name,
                                                         Code = x.Code,
                                                         ServiceCentreId = x.ServiceCentreId

                                                     }).FirstOrDefault(),
                                                     DataSimCardNumber = s.DataSimCardNumber
                                                 }).FirstOrDefault();

            return Task.FromResult(managementDto);
        }
    }
}
