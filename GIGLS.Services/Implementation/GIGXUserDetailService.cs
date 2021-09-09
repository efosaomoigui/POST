using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.BankSettlement;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.BankSettlement;
using GIGLS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.BankSettlement
{
    public class GIGXUserDetailService : IGIGXUserDetailService
    {
        private readonly IUnitOfWork _uow;

        public GIGXUserDetailService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }

      
    }
}
