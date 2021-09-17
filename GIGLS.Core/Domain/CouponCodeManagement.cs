using GIGLS.Core.Enums;
using System;

namespace GIGLS.Core.Domain
{
    public class CouponCodeManagement : BaseDomain, IAuditable
    {
        public int CouponCodeManagementId { get; set; }
        public string CouponCode { get; set; }
        public CouponDiscountType DiscountType { get; set; }
        public bool IsCouponCodeUsed { get; set; }
        public DateTime ExpiryDay { get; set; }
    }
}
