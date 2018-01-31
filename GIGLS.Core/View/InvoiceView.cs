using GIGLS.Core.DTO.Customers;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.Zone;
using GIGLS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.View
{
    public class InvoiceView
    {
        [Key]
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Waybill { get; set; }
        public DateTime DueDate { get; set; }

        ////////////SHIPMENT
        //Shipment Information
        public int ShipmentId { get; set; }

        private CustomerType customerType;

        ////Senders' Information
        //public string GetCustomerType()
        //{
        //    return customerType;
        //}

        ////Senders' Information
        //public void SetCustomerType(string value)
        //{
        //    customerType = value;
        //}

        public int CustomerId { get; set; }

        //Receivers Information
        public int DepartureServiceCentreId { get; set; }
        public string DepartureServiceCentre { get; set; }

        public int DestinationServiceCentreId { get; set; }
        public string DestinationServiceCentre { get; set; }

        public string ReceiverName { get; set; }
        public string ReceiverPhoneNumber { get; set; }
        public string ReceiverEmail { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverCity { get; set; }
        public string ReceiverState { get; set; }
        public string ReceiverCountry { get; set; }

        //Delivery Options
        public int DeliveryOptionId { get; set; }
        public string DeliveryOptionCode { get; set; }
        public string DeliveryOptionDescription { get; set; }

        //PickUp Options
        public PickupOptions PickupOptions { get; set; }

        public decimal GrandTotal { get; set; }

        //Invoice parameters: Helps generate invoice for ecomnerce customers  by customerType
        public bool IsCashOnDelivery { get; set; }
        public decimal? CashOnDeliveryAmount { get; set; }

        public decimal? ExpectedAmountToCollect { get; set; }
        public decimal? ActualAmountCollected { get; set; }

        //General Details comes with role user
        public string UserId { get; set; }

        //
        public bool IsdeclaredVal { get; set; }
        public decimal? DeclarationOfValueCheck { get; set; }

        //discount information
        public decimal? AppliedDiscount { get; set; }
        public decimal? DiscountValue { get; set; }

        public decimal? Insurance { get; set; }
        public decimal? Vat { get; set; }
        public decimal? Total { get; set; }

        //wallet information
        public string WalletNumber { get; set; }

        //from client
        public decimal? vatvalue_display { get; set; }
        public decimal? InvoiceDiscountValue_display { get; set; }
        public decimal? offInvoiceDiscountvalue_display { get; set; }

        //ShipmentCollection
        //public ShipmentCollectionDTO ShipmentCollection { get; set; }

        //Demurrage Information
        public DemurrageDTO Demurrage { get; set; }
        ///////////////////////


        ////////// CUSTOMER
        public CustomerType CustomerType { get; set; }
        //public string CustomerName
        //{
        //    get
        //    {
        //        if (GetCustomerType().Equals(GetCustomerType().Company))
        //        {
        //            return Name;
        //        }
        //        else
        //        {
        //            return string.Format($"{FirstName} {LastName}");
        //        }
        //    }
        //}

        // CompanyDTO
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string RcNumber { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Industry { get; set; }
        public CompanyType CompanyType { get; set; }
        public CompanyStatus CompanyStatus { get; set; }
        public decimal Discount { get; set; }

        // IndividualCustomerDTO
        public int IndividualCustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        //public string Email { get; set; }
        //public string City { get; set; }
        //public string State { get; set; }
        //public string Address { get; set; }
        //public string PhoneNumber { get; set; }
        public string PictureUrl { get; set; }
        public string PicData { get; set; }
        //public DateTime DateCreated { get; set; }
        //public DateTime DateModified { get; set; }
        public string CustomerCode { get; set; }
        ////////////////////////////////////////
    }
}
