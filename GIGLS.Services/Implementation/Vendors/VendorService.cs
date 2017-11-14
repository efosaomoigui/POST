using System;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Vendors;
using GIGLS.Core.IServices.Vendors;

namespace GIGLS.Services.Implementation.Vendors
{
    public class VendorService : IVendorService
    {
        public Task<object> AddFleet(VendorDTO vendor)
        {
            throw new NotImplementedException();
        }

        public Task DeleteVendor(int vendorId)
        {
            throw new NotImplementedException();
        }

        public Task<VendorDTO> GetVendorById(int vendorId)
        {
            throw new NotImplementedException();
        }

        public Task<VendorDTO> GetVendors()
        {
            throw new NotImplementedException();
        }

        public Task UpdateVendor(int vendorId, VendorDTO vendor)
        {
            throw new NotImplementedException();
        }
    }
}
