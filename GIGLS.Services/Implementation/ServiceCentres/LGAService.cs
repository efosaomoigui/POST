﻿using AutoMapper;
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
    public class LGAService: ILGAService
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
                var lga = await _uow.LGA.GetAsync(x => x.LGAName.ToLower() == lgaDto.LGAName.ToLower() && x.LGAState.ToLower() == lgaDto.LGAState.ToLower());

                if(lga != null)
                {
                    throw new GenericException("LGA Information already exists");
                }
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
        public async Task<LGADTO> GetLGAById (int lgaId)
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

                //To check if the update already exists
                var lgas = await _uow.LGA.ExistAsync(c =>c.LGAName.ToLower() == lgaDto.LGAName.ToLower() && c.LGAState.ToLower() == lgaDto.LGAState.ToLower());
                if (lga == null || lgaDto.LGAId != lgaId)
                {
                    throw new GenericException("LGA Information does not exist");
                }
                else if(lgas == true)
                {
                    throw new GenericException("LGA Information already exists");
                }
                lga.LGAName = lgaDto.LGAName;
                lga.LGAState = lgaDto.LGAState;
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
    }
}