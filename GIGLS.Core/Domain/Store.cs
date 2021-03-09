using GIGLS.Core;
using GIGLS.Core.Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace GIGL.GIGLS.Core.Domain
{
    public class Store : BaseDomain, IAuditable
    {
        public int StoreId { get; set; }
        [MaxLength(50)]
        public string StoreName { get; set; }
        [MaxLength(500)]
        public string Address { get; set; }
        [MaxLength(50)]
        public string City { get; set; }
        [MaxLength(50)]
        public string State { get; set; }
        [MaxLength(200)]
        public string URL { get; set; }
        [MaxLength(300)]
        public string storeImage { get; set; }
        public int CountryId { get; set; }

    }
}