using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Core.DTO.OnlinePayment
{
    public class CellulantPayloadDTO
    {
        public string merchantTransactionID { get; set; }

        public string customerFirstName { get; set; }

        public string customerLastName { get; set; }

        public string MSISDN { get; set; }

        public string customerEmail { get; set; }

        public string requestAmount { get; set; }

        public string currencyCode { get; set; }

        public string accountNumber { get; set; }

        public string serviceCode { get; set; }

        public string dueDate { get; set; }

        public string requestDescription { get; set; }

        public string countryCode { get; set; }

        public string languageCode { get; set; }

        public string successRedirectUrl { get; set; }

        public string failRedirectUrl { get; set; }

        public string paymentWebhookUrl { get; set; }
    }

    public class CellulantResponseDTO
    {
        public string param { get; set; }

        public string accessKey { get; set; }

        public string countryCode { get; set; }
    }
}
