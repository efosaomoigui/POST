using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;

namespace GIGLS.Core.IServices
{
    public interface IServiceResponse<TResponse>
    {
        string Code { get; set; }
        string ShortDescription { get; set; }
        TResponse Object { get; set; }
        int Total { get; set; }
        Dictionary<string, IEnumerable<string>> ValidationErrors { get; set; }
  
    }
}