using AutoMapper;
using POST.Core;
using POST.Core.Domain;
using POST.Core.Domain.Utility;
using POST.Core.Domain.Wallet;
using POST.Core.DTO;
using POST.Core.DTO.Customers;
using POST.Core.DTO.Partnership;
using POST.Core.DTO.Wallet;
using POST.Core.Enums;
using POST.Core.IServices.User;
using POST.Core.IServices.Utility;
using POST.Core.IServices.Wallet;
using POST.CORE.Enums;
using POST.Infrastructure;
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

namespace POST.Services.Implementation.Wallet
{
    public class CODTransferLogService : ICODTransferLogService
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _uow;
        private readonly IGlobalPropertyService _globalPropertyService;
        private readonly IStellasService _stellasService;

        public CODTransferLogService(IUserService userService, IUnitOfWork uow, IGlobalPropertyService globalPropertyService, IStellasService stellasService)
        {
            _userService = userService;
            _uow = uow;
            _globalPropertyService = globalPropertyService;
            _stellasService = stellasService;
            MapperConfig.Initialize();
        }


    }
}