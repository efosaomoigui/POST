using POST.Core.Domain.Utility;
using POST.Core.IRepositories.Utility;
using POST.Infrastructure.Persistence.Repository;

namespace POST.Infrastructure.Persistence.Repositories.Utility
{
    public class QRAndBarcodeRepository : Repository<QRAndBarcode, GIGLSContext>, IQRAndBarcodeRepository
    {
        public QRAndBarcodeRepository(GIGLSContext context) : base(context)
        {
        }
    }
}
