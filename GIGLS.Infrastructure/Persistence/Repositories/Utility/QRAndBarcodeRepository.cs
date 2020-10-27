using GIGLS.Core.Domain.Utility;
using GIGLS.Core.IRepositories.Utility;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.Infrastructure.Persistence.Repositories.Utility
{
    public class QRAndBarcodeRepository : Repository<QRAndBarcode, GIGLSContext>, IQRAndBarcodeRepository
    {
        public QRAndBarcodeRepository(GIGLSContext context) : base(context)
        {
        }
    }
}
