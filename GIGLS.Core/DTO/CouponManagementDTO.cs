using GIGLS.Core.Enums;
using System;
using System.Collections.Generic;

namespace GIGLS.Core.DTO
{
    public class CouponManagementDTO
    {
        public int CouponCodeManagementId { get; set; }
        public int Number { get; set; }
        public List<string> CouponCode { get; set; }
        public float CouponCodeValue { get; set; }
        public CouponDiscountType DiscountType { get; set; }
        public bool IsCouponCodeUsed { get; set; }
        public DateTime ExpiryDay { get; set; }
    }
}
