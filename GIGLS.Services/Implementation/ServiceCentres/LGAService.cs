using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.IServices.ServiceCentres;
using GIGLS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.ServiceCentres
{
    public class LGAService : ILGAService
    {
        private readonly IUnitOfWork _uow;

        public LGAService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<object> AddLGA(LGADTO lgaDto)
        {
            try
            {
                var state = await _uow.State.GetAsync(lgaDto.StateId);
                if (state == null)
                {
                    throw new GenericException("State does not exist");
                }

                var lga = await _uow.LGA.GetAsync(x => x.LGAName.ToLower() == lgaDto.LGAName.ToLower() && x.StateId == lgaDto.StateId);

                if (lga != null)
                {
                    throw new GenericException("LGA Information already exists");
                }
                lgaDto.LGAState = state.StateName;
                var newlga = Mapper.Map<LGA>(lgaDto);
                _uow.LGA.Add(newlga);
                await _uow.CompleteAsync();
                return new { Id = newlga.LGAId };
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<LGADTO> GetLGAById(int lgaId)
        {
            try
            {
                var lga = await _uow.LGA.GetAsync(lgaId);
                if (lga == null)
                {
                    throw new GenericException("LGA information does not exist");
                }

                var lgaDto = Mapper.Map<LGADTO>(lga);
                return lgaDto;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Task<IEnumerable<LGADTO>> GetLGAs()
        {
            var lgas = _uow.LGA.GetAll().OrderBy(x => x.LGAName);
            return Task.FromResult(Mapper.Map<IEnumerable<LGADTO>>(lgas));
        }

        public async Task UpdateLGA(int lgaId, LGADTO lgaDto)
        {
            try
            {
                var lga = await _uow.LGA.GetAsync(lgaId);

                var state = await _uow.State.GetAsync(lgaDto.StateId);
                if (state == null)
                {
                    throw new GenericException("State does not exist");
                }

                //To check if the update already exists
                var lgas = await _uow.LGA.ExistAsync(c => c.LGAName.ToLower() == lgaDto.LGAName.ToLower() && c.StateId == lgaDto.StateId);
                if (lga == null || lgaDto.LGAId != lgaId)
                {
                    throw new GenericException("LGA Information does not exist");
                }
                else if (lgas == true)
                {
                    throw new GenericException("LGA Information already exists");
                }
                lga.LGAName = lgaDto.LGAName;
                lga.LGAState = state.StateName;
                lga.StateId = state.StateId;
                lga.Status = lgaDto.Status;

                _uow.Complete();

            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task UpdateLGA(int lgaId, bool status)
        {
            try
            {
                var lga = await _uow.LGA.GetAsync(lgaId);
                if (lga == null)
                {
                    throw new GenericException("LGA Information does not exist");
                }
                lga.Status = status;
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteLGA(int lgaId)
        {
            try
            {
                var lga = await _uow.LGA.GetAsync(lgaId);
                if (lga == null)
                {
                    throw new GenericException("LGA information does not exist");
                }
                _uow.LGA.Remove(lga);
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<LGADTO>> GetActiveLGAs()
        {
            try
            {
                return await _uow.LGA.GetActiveLGAs();
            }
            catch (Exception)
            {
                throw;
            }
        }




        public async Task UpdateHomeDeliveryLocation(int lgaId, bool status)
        {
            try
            {
                var location = await _uow.LGA.GetAsync(lgaId);
                if (location == null)
                {
                    throw new GenericException("LGA Information does not exist");
                }
                location.HomeDeliveryStatus = status;
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<LGADTO>> GetActiveHomeDeliveryLocations()
        {
            try
            {
                return await _uow.LGA.GetActiveHomeDeliveryLocations();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<LGADTO>> GetLGAByState(int stateId)
        {
            var lgas = new List<LGADTO>();
            try
            {
                var items = _uow.LGA.GetAll().Where(x => x.StateId == stateId).ToList();
                if (items.Any())
                {
                    lgas = (Mapper.Map<List<LGADTO>>(items));
                }
                return lgas;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
