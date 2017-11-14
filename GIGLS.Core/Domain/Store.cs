using GIGLS.Core;
using GIGLS.Core.Domain;
using System;

namespace GIGL.GIGLS.Core.Domain
{
    public class Store : BaseDomain, IAuditable
    {
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}