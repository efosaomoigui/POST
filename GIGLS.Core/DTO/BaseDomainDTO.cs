using System;

namespace GIGLS.CORE.DTO
{
    public class BaseDomainDTO
    {
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public bool IsDeleted { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
