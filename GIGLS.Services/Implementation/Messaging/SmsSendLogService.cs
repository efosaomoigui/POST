﻿using GIGLS.Core.IServices.MessagingLog;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.MessagingLog;
using GIGLS.Core;
using GIGLS.Core.Domain.MessagingLog;
using AutoMapper;
using GIGLS.Infrastructure;
using GIGLS.Core.IServices.User;
using GIGLS.CORE.DTO.Shipments;
using System.Linq;
using System;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace GIGLS.Services.Implementation.Messaging
{
    public class SmsSendLogService : ISmsSendLogService
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _uow;

        public SmsSendLogService(IUserService userService, IUnitOfWork uow)
        {
            _userService = userService;
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<object> AddSmsSendLog(SmsSendLogDTO messageDto)
        {
            var message = Mapper.Map<SmsSendLog>(messageDto);

            if (message.User == null)
            {
                message.User = await _userService.GetCurrentUserId();
            }
            _uow.SmsSendLog.Add(message);
            await _uow.CompleteAsync();
            return new { id = message.SmsSendLogId };
        }

        public Task<List<SmsSendLogDTO>> GetSmsSendLogAsync(MessageFilterOption filter)
        {
            var messages = _uow.SmsSendLog.GetSmsSendLogsAsync(filter);
            return messages;
        }

        public Tuple<Task<List<SmsSendLogDTO>>, int> GetSmsSendLogAsync(FilterOptionsDto filterOptionsDto)
        {
            var messages = _uow.SmsSendLog.GetSmsSendLogsAsync(filterOptionsDto);
            return messages;
        }

        public Tuple<Task<List<SmsSendLogDTO>>, int> GetSmsSendLogAsync_OLD(FilterOptionsDto filterOptionsDto)
        {
            try
            {
                var smsCollection = _uow.SmsSendLog.FindAsync(s => s.IsDeleted == false).Result;
                var smsCollectionDto = Mapper.Map<IEnumerable<SmsSendLogDTO>>(smsCollection);
                smsCollectionDto = smsCollectionDto.OrderByDescending(x => x.DateCreated);

                var count = smsCollectionDto.ToList().Count();

                if (filterOptionsDto != null)
                {
                    //filter
                    var filter = filterOptionsDto.filter;
                    var filterValue = filterOptionsDto.filterValue;
                    if (!string.IsNullOrEmpty(filter) && !string.IsNullOrEmpty(filterValue))
                    {
                        smsCollectionDto = smsCollectionDto.Where(s => (s.GetType().GetProperty(filter).GetValue(s)) != null
                            && (s.GetType().GetProperty(filter).GetValue(s)).ToString().Contains(filterValue)).ToList();
                    }

                    //sort
                    var sortorder = filterOptionsDto.sortorder;
                    var sortvalue = filterOptionsDto.sortvalue;

                    if (!string.IsNullOrEmpty(sortorder) && !string.IsNullOrEmpty(sortvalue))
                    {
                        System.Reflection.PropertyInfo prop = typeof(SmsSendLog).GetProperty(sortvalue);

                        if (sortorder == "0")
                        {
                            smsCollectionDto = smsCollectionDto.OrderBy(x => x.GetType().GetProperty(prop.Name).GetValue(x)).ToList();
                        }
                        else
                        {
                            smsCollectionDto = smsCollectionDto.OrderByDescending(x => x.GetType().GetProperty(prop.Name).GetValue(x)).ToList();
                        }
                    }

                    smsCollectionDto = smsCollectionDto.Skip(filterOptionsDto.count * (filterOptionsDto.page - 1)).Take(filterOptionsDto.count).ToList();
                }

                return new Tuple<Task<List<SmsSendLogDTO>>, int>(Task.FromResult(smsCollectionDto.ToList()), count);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SmsSendLogDTO> GetSmsSendLogById(int messageId)
        {
            var message = await _uow.SmsSendLog.GetAsync(messageId);

            if (message == null)
            {
                throw new GenericException("MESSAGE INFORMATION DOES NOT EXIST");
            }
            return Mapper.Map<SmsSendLogDTO>(message);
        }

        public async Task<List<SmsSendLogDTO>> GetSmsSendLog(string phoneNumber)
        {
            var message = await _uow.SmsSendLog.FindAsync(x => x.To == phoneNumber);
            return Mapper.Map<List<SmsSendLogDTO>>(message.OrderByDescending(x => x.DateCreated));
        }

        public async Task<List<SmsSendLogDTO>> GetSmsSendLogs(string waybill)
        {
            var message = await _uow.SmsSendLog.FindAsync(x => x.Waybill == waybill);
            return Mapper.Map<List<SmsSendLogDTO>>(message.OrderByDescending(x => x.DateCreated));
        }

        public async Task RemoveSmsSendLog(int messageId)
        {
            var message = await _uow.SmsSendLog.GetAsync(messageId);

            if (message == null)
            {
                throw new GenericException("MESSAGE INFORMATION DOES NOT EXIST");
            }
            _uow.SmsSendLog.Remove(message);
            await _uow.CompleteAsync();
        }

        public async Task UpdateSmsSendLog(int messageId, SmsSendLogDTO messageDto)
        {
            var message = await _uow.SmsSendLog.GetAsync(messageId);

            if (message == null)
            {
                throw new GenericException("MESSAGE INFORMATION DOES NOT EXIST");
            }

            message.From = messageDto.From;
            message.To = messageDto.To;
            message.Status = messageDto.Status;
            message.Message = messageDto.Message;
            message.User = messageDto.User;
            message.ResultStatus = messageDto.ResultStatus;
            message.ResultDescription = messageDto.ResultDescription;

            await _uow.CompleteAsync();
        }

        public async Task<SmsDeliveryDTO> SmsDeliveryLog(string phonenumber)
        {
            var Response = new SmsDeliveryDTO();
            try
            {
                var phoneNumber = phonenumber.Substring(1);
                
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var httpWebRequest = (HttpWebRequest)WebRequest.Create($"https://ogosms.net/dlr.php?num=234{phoneNumber}");
                using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    Stream result = httpResponse.GetResponseStream();
                    StreamReader reader = new StreamReader(result);
                    string responseFromServer = reader.ReadToEnd();
                    Response = JsonConvert.DeserializeObject<SmsDeliveryDTO>(responseFromServer);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return await Task.FromResult(Response);


        }

    }
}
