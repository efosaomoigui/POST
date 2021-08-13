using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.Common.Helpers
{
    public interface IQueryCommand<out TResult>
    {
        TResult Execute();
    }
}
