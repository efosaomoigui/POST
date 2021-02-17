using GIGLS.CORE.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.Shipments
{
    public class ShipmentActivityDTO : BaseDomainDTO
    {
        public string Waybill { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Action { get; set; }
        public string ActionBy { get; set; }
        public string ActionReason { get; set; }
        public DateTime ActionTime { get; set; }
        public string ActionResult { get; set; }
        public string PhoneNo { get; set; }
    }

    public class DataResponse
    {
        public Data Data { get; set; }
    }

    public class Data
    {
        public Data()
        {
            ApiList = new List<ApiList>();
        }
        public List<ApiList> ApiList { get; set; }
    }

    public class ApiList
    {
        public ApiList()
        {
            Body = new Body();
        }
        public string StatusCode { get; set; }
        public string Url { get; set; }
        public string ReasonText { get; set; }
        public DateTime CreationDate { get; set; }
        public Body Body { get; set; }
    }


    public class Body
    {
        public string PartnerId { get; set; }
        public string waybillNumber { get; set; }
        public string assignedPartner { get; set; }
    }



    public class DataInfo
    {
        public DataInfo()
        {
            PartnerInfo = new PartnerInfo();
        }

        public string apiId { get; set; }
        public int statusCode { get; set; }
        public string message { get; set; }
        public PartnerInfo PartnerInfo { get; set; }
    }

    public class PartnerInfo
    {
        public string assignedPartner { get; set; }
        public bool isReassigned { get; set; }
    }
}
