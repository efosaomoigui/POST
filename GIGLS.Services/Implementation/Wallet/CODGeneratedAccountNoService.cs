using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.Domain.Utility;
using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Customers;
using GIGLS.Core.DTO.Partnership;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.User;
using GIGLS.Core.IServices.Utility;
using GIGLS.Core.IServices.Wallet;
using GIGLS.CORE.Enums;
using GIGLS.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Wallet
{
    public class CODGeneratedAccountNoService : ICODGeneratedAccountNoService
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _uow;
        private readonly IGlobalPropertyService _globalPropertyService;
        private readonly IStellasService _stellasService;

        public CODGeneratedAccountNoService(IUserService userService, IUnitOfWork uow, IGlobalPropertyService globalPropertyService, IStellasService stellasService)
        {
            _userService = userService;
            _uow = uow;
            _globalPropertyService = globalPropertyService;
            _stellasService = stellasService;
            MapperConfig.Initialize();
        }


    }
}