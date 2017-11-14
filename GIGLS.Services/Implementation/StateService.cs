using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using GIGLS.Core.IServices;
using GIGLS.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

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
            return states;
        }


        public async Task<StateDTO> GetStateById(int stateId)
        {
            var state = await _uow.State.GetAsync(stateId);

            if (state == null)
            {
                throw new GenericException("STATE INFORMATION DOES NOT EXIST");
            }
            return Mapper.Map<StateDTO>(state);
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
                Country = stateDto.Country
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
            //states.Country = state.Country.Trim();
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
