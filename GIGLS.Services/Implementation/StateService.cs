using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using GIGLS.Core.IServices;
using GIGLS.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation
{
    public class StateService : IStateService
    {
        private readonly IUnitOfWork _uow;

        public StateService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<IEnumerable<StateDTO>> GetStates(int pageSize, int page)
        {
            var states = await _uow.State.GetStatesAsync(pageSize, page);
            return states.OrderBy(x => x.StateName).ToList();
        }


        public async Task<StateDTO> GetStateById(int stateId)
        {
            return await _uow.State.GetStateById(stateId);
        }

        public async Task<object> AddState(StateDTO stateDto)
        {
            stateDto.StateName = stateDto.StateName.Trim();
            var stateName = stateDto.StateName.ToLower();

            if (await _uow.State.ExistAsync(v => v.StateName.ToLower() == stateName))
            {
                throw new GenericException($"STATE {stateDto.StateName} ALREADY EXIST");
            }

            var newState = new State
            {
                StateName = stateDto.StateName,
                StateCode = stateDto.StateCode,
                CountryId = stateDto.CountryId
            };

            _uow.State.Add(newState);
            await _uow.CompleteAsync();
            return new { id = newState.StateId };
        }

        public async Task UpdateState(int stateId, StateDTO stateDto)
        {
            var state = await _uow.State.GetAsync(stateId);

            if (state == null)
            {
                throw new GenericException("STATE INFORMATION DOES NOT EXIST");
            }
            state.StateName = stateDto.StateName.Trim();
            state.StateCode = stateDto.StateCode;
            state.CountryId = stateDto.CountryId;
            await _uow.CompleteAsync();
        }

        public async Task RemoveState(int stateId)
        {
            var state = await _uow.State.GetAsync(stateId);

            if (state == null)
            {
                throw new GenericException("STATE INFORMATION DOES NOT EXIST");
            }
            _uow.State.Remove(state);
            await _uow.CompleteAsync();
        }

        public int GetStatesTotal()
        {
            var states = _uow.State.GetStatesTotal();
            return states;
        }
    }
}
