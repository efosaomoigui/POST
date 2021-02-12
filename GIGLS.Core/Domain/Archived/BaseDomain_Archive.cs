using System;
using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain.Archived
{
    public class BaseDomain_Archive
    {
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public bool IsDeleted { get; set; }

        [MaxLength(20)]
        public string RowVersion { get; set; }
    }
}
