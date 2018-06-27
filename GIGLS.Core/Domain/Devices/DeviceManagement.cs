﻿using GIGL.GIGLS.Core.Domain;

namespace GIGLS.Core.Domain.Devices
{
    public class DeviceManagement : BaseDomain
    {
        public int DeviceManagementId { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public int DeviceId { get; set; }
        public virtual Device Device { get; set; }

        public bool IsActive { get; set; }
    }
}