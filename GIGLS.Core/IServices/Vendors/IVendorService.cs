using GIGLS.Core.DTO.Vendors;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Vendors
{
    public interface IVendorService : IServiceDependencyMarker
    {
        Task<VendorDTO> GetVendors();
        Task<VendorDTO> GetVendorById(int vendorId);
        Task<object> AddFleet(VendorDTO vendor);
        Task UpdateVendor(int vendorId, VendorDTO vendor);
        Task DeleteVendor(int vendorId);
    }
}
