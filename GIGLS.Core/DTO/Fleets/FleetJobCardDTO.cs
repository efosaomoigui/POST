using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.Fleets
{
    public class FleetJobCardDto
    {
        public int FleetJobCardId { get; set; }
        public string VehicleNumber { get; set; }
        public decimal Amount { get; set; }
        public string VehiclePartToFix { get; set; }
        public string Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string FleetManagerId { get; set; }
        public string FleetManager { get; set; }
        public string FleetOwnerId { get; set; }
        public string FleetOwner { get; set; }
        public string RevenueStatus { get; set; }
        public string PaymentReceiptUrl { get; set; }
    }

    public class FleetJobCardMailDto
    {
        public int FleetJobCardId { get; set; }
        public string VehicleNumber { get; set; }
        public string VehicleOwner { get; set; }
        public decimal Amount { get; set; }
        public string VehiclePartToFix { get; set; }
        public string Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string FleetManager { get; set; }
        public string EnterpriseEmail { get; set; }
    }

    public class OpenFleetJobCardDto
    {
        public string VehicleNumber { get; set; }
        public decimal Amount { get; set; }
        public string VehiclePartToFix { get; set; }
        public string Status { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public string EnterprisePartnerId { get; set; }
    }
    
    public class FleetJobCardByDateDto
    {
        public int FleetJobCardId { get; set; }
        public string VehicleNumber { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
        public string VehiclePartToFix { get; set; }
        public DateTime DateCreated { get; set; }
    }
    
    public class GetFleetJobCardByDateRangeDto
    {
        public int FleetJobCardId { get; set; }
        public string VehicleNumber { get; set; }
        public string FleetManagerId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsAdmin { get; set; }
    }

    public class NewJobCard
    {
        public List<OpenFleetJobCardDto> JobCardItems { get; set; }
    }

    public class CloseJobCardDto
    {
        public int FleetJobCardId { get; set; }
        public string ReceiptUrl { get; set; }
        public string VehicleNumber { get; set; }
    }
}
