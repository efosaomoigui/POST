﻿using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GIGLS.Core.DTO.Wallet
{
    public class CODWalletDTO : BaseDomainDTO
    {
        public int CODWalletId { get; set; }
        public string AccountNo { get; set; }
        public string AvailableBalance { get; set; }
        public string CustomerId { get; set; }
        public string UserId { get; set; }
        public CustomerType CustomerType { get; set; }
        public string CustomerCode { get; set; }
        public string CompanyType { get; set; }
        public string AccountType { get; set; }
        public string WithdrawableBalance { get; set; }
        public string CustomerAccountId { get; set; }
        public string DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }
        public string Address { get; set; }
        public string NationalIdentityNo { get; set; }
    }

    public class CreateStellaAccountDTO 
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string otherNames { get; set; }
        public string bvn { get; set; }
        public string phoneNo { get; set; }
        public string gender { get; set; }
        public string email { get; set; }
        public string dateOfBirth { get; set; }
        public string placeOfBirth { get; set; }
        public string address { get; set; }
        public string nationalIdentityNo { get; set; }
        public string CustomerCode { get; set; }

    }

    public class CreateStellaAccounResponsetDTO
    {
       
        public bool status { get; set; }
        public string message { get; set; }
        public ResponseObj data { get; set; }

    }

    public class ResponseObj
    {
        public CustomerDetails customerDetails { get; set; }
        public AccountDetails account_details { get; set; }
    }

    public class CustomerDetails
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string otherNames { get; set; }
        public string bvn { get; set; }
        public string phoneNo { get; set; }
        public string gender { get; set; }
        public string email { get; set; }
        public string dateOfBirth { get; set; }
        public string placeOfBirth { get; set; }
        public string address { get; set; }
        public string nationalIdentityNo { get; set; }


    }

    public class AccountDetails
    {
        public string accountNumber { get; set; }
        public string accountType { get; set; }
        public string availableBalance { get; set; }
        public string withdrawableBalance { get; set; }
        public string customerId { get; set; }
    }

    public class GetCustomerBalanceDTO
    {

        public bool status { get; set; }
        public string message { get; set; }
        public CustomerAccountDetailDTO data { get; set; }
    }

    public class CustomerAccountDetailDTO
    {
        public string accountNumber { get; set; }
        public string availableBalance { get; set; }
        public DateTime financialDate { get; set; }
    }




    public class StellasWithdrawalDTO
    {
        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("receiverAccountNumber")]
        public string ReceiverAccountNumber { get; set; }

        [JsonProperty("receiverBankCode")]
        public string ReceiverBankCode { get; set; }

        [JsonProperty("retrievalReference")]
        public string RetrievalReference { get; set; }

        [JsonProperty("narration")]
        public string Narration { get; set; }
    }


    public class Datum
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }
    }

    public class StellassBankResponse
    {
        [JsonProperty("status")]
        public bool Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public List<Datum> Data { get; set; }
    }



    public class Error
    {
        [JsonProperty("message")]
        public string Message { get; set; }
    }

    public class StellasWithdrawalResponse
    {
        [JsonProperty("errors")]
        public List<Error> Errors { get; set; }
    }



}
