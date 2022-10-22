using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Core.DTO.Alpha
{
    public class AlphaUserLoginDTO
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("user_type")]
        public string UserType { get; set; }
    }


    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class User
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("customer_code")]
        public string CustomerCode { get; set; }

        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }

        [JsonProperty("country_name")]
        public string CountryName { get; set; }

        [JsonProperty("user_type")]
        public string UserType { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("is_onboarded")]
        public bool IsOnboarded { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("has_store")]
        public bool HasStore { get; set; }

        [JsonProperty("subscription_status")]
        public string SubscriptionStatus { get; set; }

        [JsonProperty("subscription_amount")]
        public int SubscriptionAmount { get; set; }

        [JsonProperty("subscription_expiry_date")]
        public DateTime SubscriptionExpiryDate { get; set; }

        [JsonProperty("subscription_start_date")]
        public DateTime SubscriptionStartDate { get; set; }

        [JsonProperty("subscription_plan")]
        public string SubscriptionPlan { get; set; }
    }

    public class Results
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }
    }

    public class AlphaLoginResponseDTO
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("error")]
        public bool Error { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("results")]
        public Results Results { get; set; }
    }

    public class AlphaSubscriptionUpdateDTO
    {
        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("customer_code")]
        public string CustomerCode { get; set; }

        [JsonProperty("subscription_plan")]
        public string SubscriptionPlan { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("expiry_date")]
        public DateTime ExpiryDate { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }
    }

    public class ResponseDTO
    {
        [JsonProperty("expiry_date")]
        public DateTime ExpiryDate { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("customer_code")]
        public string CustomerCode { get; set; }

        [JsonProperty("subscription_plan")]
        public string SubscriptionPlan { get; set; }

        [JsonProperty("subscription_status")]
        public string SubscriptionStatus { get; set; }
    }

    public class AlphaSubscriptionUpdateResponseDTO
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("error")]
        public bool Error { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("results")]
        public ResponseDTO Results { get; set; }
    }

    public class AlphaUpdateOrderStatusDTO
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("way_bill")]
        public string WayBill { get; set; }
    }
}
