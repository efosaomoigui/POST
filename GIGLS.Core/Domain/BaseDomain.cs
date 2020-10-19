using System;
using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain
{
    public class BaseDomain : IAuditable
    {
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public bool IsDeleted { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }

}
