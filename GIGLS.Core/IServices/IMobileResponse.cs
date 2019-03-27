using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;

namespace GIGLS.Core.IServices
{
    public interface IMobileResponse<TResponse>
    {
        
        TResponse Object { get; set; }
        int Total { get; set; }
        Dictionary<string, IEnumerable<string>> ValidationErrors { get; set; }

    }
}