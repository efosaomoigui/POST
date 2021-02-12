using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIGLS.Core.Domain.Archived
{
    public class BaseDomain_Archive
    {
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public bool IsDeleted { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string RowVersion { get; set; }
    }
}
