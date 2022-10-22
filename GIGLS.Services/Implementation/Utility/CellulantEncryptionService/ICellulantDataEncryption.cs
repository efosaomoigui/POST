using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Services.Implementation.Utility.CellulantEncryptionService
{
    public interface ICellulantDataEncryption 
    {
        string EncryptData(string payload);
    }
}
