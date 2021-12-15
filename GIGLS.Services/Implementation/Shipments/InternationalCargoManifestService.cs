using AutoMapper;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.Domain.DHL;
using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.Customers;
using GIGLS.Core.DTO.DHL;
using GIGLS.Core.DTO.PaymentTransactions;
using GIGLS.Core.DTO.Report;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.User;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.DTO.Zone;
using GIGLS.Core.Enums;
using GIGLS.Core.IMessageService;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Business;
using GIGLS.Core.IServices.Customers;
using GIGLS.Core.IServices.DHL;
using GIGLS.Core.IServices.Node;
using GIGLS.Core.IServices.ServiceCentres;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Core.IServices.UPS;
using GIGLS.Core.IServices.User;
using GIGLS.Core.IServices.Utility;
using GIGLS.Core.IServices.Wallet;
using GIGLS.Core.IServices.Zone;
using GIGLS.Core.View;
using GIGLS.CORE.DTO.Report;
using GIGLS.CORE.DTO.Shipments;
using GIGLS.Infrastructure;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Shipments
{
    public class InternationalCargoManifestService : IInternationalCargoManifestService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
       


        public InternationalCargoManifestService(IUnitOfWork uow, IUserService userService
           )
        {
            _uow = uow;
            _userService = userService;
            MapperConfig.Initialize();
        }

        public async Task<InternationalCargoManifestDTO> AddCargoManifest(InternationalCargoManifestDTO cargoManifest)
        {
            var newCargoManifest = Mapper.Map<InternationalCargoManifest>(cargoManifest);
            _uow.InternationalCargoManifest.Add(newCargoManifest);
            _uow.Complete();
            return cargoManifest;
        }

        public async Task<InternationalCargoManifestDTO> GetIntlCargoManifestByID(int cargoID)
        {
            return await _uow.InternationalCargoManifest.GetIntlCargoManifestByID(cargoID);
        }

        public async Task<List<InternationalCargoManifestDTO>> GetIntlCargoManifests(NewFilterOptionsDto filter)
        {
            if (filter != null && filter.StartDate == null && filter.EndDate == null)
            {
                var now = DateTime.Now;
                DateTime firstDay = new DateTime(now.Year, now.Month, 1);
                DateTime lastDay = firstDay.AddMonths(1).AddDays(-1);
                filter.StartDate = firstDay;
                filter.EndDate = lastDay;
            }
            if (filter != null && filter.StartDate != null && filter.EndDate != null)
            {
                filter.StartDate = filter.StartDate.Value.ToUniversalTime();
                filter.StartDate = filter.StartDate.Value.AddHours(12).AddMinutes(00);
                filter.EndDate = filter.EndDate.Value.ToUniversalTime();
                filter.EndDate = filter.EndDate.Value.AddHours(23).AddMinutes(59);
            }
            return await _uow.InternationalCargoManifest.GetIntlCargoManifests(filter);
        }
    }
}