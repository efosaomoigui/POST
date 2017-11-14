using System;
using System.Threading.Tasks;
using GIGLS.Core.IServices.ServiceCentres;
using GIGLS.Core;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.User;
using AutoMapper;
using System.Collections.Generic;
using GIGLS.Infrastructure;
using GIGLS.Core.IServices.User;
using System.Linq;

namespace GIGLS.Services.Implementation.ServiceCentres
{
    public class UserServiceCentreMappingService : IUserServiceCentreMappingService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        private readonly IServiceCentreService _centreService;

        public UserServiceCentreMappingService(IUnitOfWork uow, IUserService userService, IServiceCentreService centreService)
        {
            _uow = uow;
            _userService = userService;
            _centreService = centreService;
            MapperConfig.Initialize();
            //Mapper.Initialize(cfg =>
            //{
            //    cfg.CreateMap<ServiceCentre, ServiceCentreDTO>();
            //});
        }

        //GetUsersInServiceCentre

        public async Task<List<UserDTO>> GetUsersInServiceCentre()
        {
            try
            {
                var activeUser = await _uow.UserServiceCentreMapping.FindAsync(x => x.IsActive == true, "User,ServiceCentre");

                if (activeUser == null)
                {
                    throw new GenericException("No Active User Assign to any Service Centre");
                }
                
                List<UserDTO> userDto = new List<UserDTO>();

                //loop through the active User result to get the user details
                foreach (var user in activeUser.ToList())
                {
                    var data = Mapper.Map<UserDTO>(user.User);
                    data.UserActiveServiceCentre = user.ServiceCentre.Name;
                    //data.ServiceCentres.Add(Mapper.Map<ServiceCentreDTO>(user.ServiceCentre));
                    userDto.Add(data);
                }

                return userDto.OrderBy(x => x.Username).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Get user Active Service Centre
        public async Task<ServiceCentreDTO> GetUserActiveServiceCentre(string userId)
        {
            try
            {
                await _userService.GetUserById(userId);

                var activeCentre = _uow.UserServiceCentreMapping.SingleOrDefault(x => x.IsActive == true && x.User.Id.Equals(userId));
                if (activeCentre == null)
                {
                    throw new GenericException("User Not Assign to any Service Centre");
                }
                var serviceCentre = await _centreService.GetServiceCentreById(activeCentre.ServiceCentreId);
                return serviceCentre;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Get all service centres of the user (both active and inactive)
        public async Task<List<ServiceCentreDTO>> GetUserServiceCentres(string userId)
        {
            try
            {
                await _userService.GetUserById(userId);

                var activeCentre = await _uow.UserServiceCentreMapping.FindAsync(x => x.User.Id.Equals(userId));

                if (activeCentre == null)
                {
                    throw new GenericException("User Not Assign to any Service Centre");
                }
                
                List<ServiceCentreDTO> serviceCentre = new List<ServiceCentreDTO>();
                
                foreach (var centre in activeCentre)
                {
                    var data = await _uow.ServiceCentre.GetAsync(centre.ServiceCentreId); //_centreService.GetServiceCentreById(centre.ServiceCentreId);
                    if(data != null)
                    {
                        serviceCentre.Add(Mapper.Map<ServiceCentreDTO>(data));
                    }
                }

                return serviceCentre.OrderBy(x=>x.Name).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Get users under the service centre that is active
        public async Task<List<UserDTO>> GetActiveUsersMapToServiceCentre(int serviceCentreId)
        {
            try
            {                
                var activeUser = await _uow.UserServiceCentreMapping.FindAsync(x => x.ServiceCentreId == serviceCentreId && x.IsActive == true, "User");

                if (activeUser == null)
                {
                    throw new GenericException("No Active User Assign to the Service Centre");
                }
                                
                List<UserDTO> userDto = new List<UserDTO>();

                //loop through the active User result to get the user details
                foreach (var user in activeUser)
                {
                    var data = await _userService.GetUserById(user.User.Id);
                    userDto.Add(data);
                }
                return userDto.OrderBy(x => x.Username).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Get users under the service centre both active and inactive
        public async Task<List<UserDTO>> GetUsersMapToServiceCentre(int serviceCentreId)
        {
            try
            {
                await _centreService.GetServiceCentreById(serviceCentreId);

                var activeUser = await _uow.UserServiceCentreMapping.FindAsync(x => x.ServiceCentreId == serviceCentreId, "User");

                if (activeUser == null)
                {
                    throw new GenericException("No Active User Assign to the Service Centre");
                }

                List<UserDTO> userDto = new List<UserDTO>();

                foreach (var user in activeUser)
                {
                    var data = await _userService.GetUserById(user.User.Id);
                    userDto.Add(data);
                }
                return userDto.OrderBy(x => x.Username).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //map user to service centre
        public async Task MappingUserToServiceCentre(string id, int serviceCentreId)
        {
            try
            {
                var userDetail = await _uow.User.GetUserById(id);
                if(userDetail == null)
                {
                    throw new GenericException("User does not exist");
                }
                await _centreService.GetServiceCentreById(serviceCentreId);

                //where user is active should change to Inactive 
                var userActive = _uow.UserServiceCentreMapping.SingleOrDefault(x => x.IsActive == true && x.User.Id.Equals(id));

                if (userActive != null)
                {
                    userActive.IsActive = false;
                    _uow.Complete();
                }

                //Add new Mapping
                var newMapping = new UserServiceCentreMapping
                {
                    ServiceCentreId = serviceCentreId,
                    UserId = userDetail.Id,
                    IsActive = true
                };
                _uow.UserServiceCentreMapping.Add(newMapping);
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }        
    }
}
