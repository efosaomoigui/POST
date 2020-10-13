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
        public string Method { get; set; }
        public string StatusCode { get; set; }
        public string ApiId { get; set; }
        public string completeUrl { get; set; }
        public string baseUrl { get; set; }
        public string Url { get; set; }
        public string Reason { get; set; }
        public DateTime CreationDate { get; set; }
        public Body Body { get; set; }
    }


    public class Body
    {
        public string PartnerId { get; set; }
        public string waybillNumber { get; set; }
    }


}
