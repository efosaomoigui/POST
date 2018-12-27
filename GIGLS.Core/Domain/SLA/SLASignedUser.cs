﻿namespace GIGLS.Core.Domain.SLA
{
    public class SLASignedUser : BaseDomain, IAuditable
    {
        public int SLASignedUserId { get; set; }

        public int SLAId { get; set; }
        public virtual SLA SLA { get; set; }

        public string UserId { get; set; }
    }
}