using AutoMapper;
using GIGL.POST.Core.Domain;
using POST.Core;
using POST.Core.Domain;
using POST.Core.Domain.DHL;
using POST.Core.Domain.Wallet;
using POST.Core.DTO;
using POST.Core.DTO.Account;
using POST.Core.DTO.Customers;
using POST.Core.DTO.DHL;
using POST.Core.DTO.PaymentTransactions;
using POST.Core.DTO.Report;
using POST.Core.DTO.ServiceCentres;
using POST.Core.DTO.Shipments;
using POST.Core.DTO.User;
using POST.Core.DTO.Wallet;
using POST.Core.DTO.Zone;
using POST.Core.Enums;
using POST.Core.IMessageService;
using POST.Core.IServices;
using POST.Core.IServices.Business;
using POST.Core.IServices.Customers;
using POST.Core.IServices.DHL;
using POST.Core.IServices.Node;
using POST.Core.IServices.ServiceCentres;
using POST.Core.IServices.Shipments;
using POST.Core.IServices.UPS;
using POST.Core.IServices.User;
using POST.Core.IServices.Utility;
using POST.Core.IServices.Wallet;
using POST.Core.IServices.Zone;
using POST.Core.View;
using POST.CORE.DTO.Report;
using POST.CORE.DTO.Shipments;
using POST.Infrastructure;
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

namespace POST.Services.Implementation.Shipments
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