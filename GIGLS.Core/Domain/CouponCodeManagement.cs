using GIGLS.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain
{
    public class CouponCodeManagement : BaseDomain, IAuditable
    {
        public int CouponCodeManagementId { get; set; }
        [MaxLength(50)]
        public string CouponCode { get; set; }
        public float CouponCodeValue { get; set; }
        public CouponDiscountType DiscountType { get; set; }
        public bool IsCouponCodeUsed { get; set; }
        public DateTime ExpiryDay { get; set; }
    }
}
