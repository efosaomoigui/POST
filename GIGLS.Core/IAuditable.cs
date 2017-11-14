using System;

namespace GIGLS.Core
{
    public interface IAuditable
    {
        DateTime DateCreated { get; set; }
        DateTime DateModified { get; set; }
        bool IsDeleted { get; set; }

        byte[] RowVersion { get; set; }
    }
}
