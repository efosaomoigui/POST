using GIGLS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GIGLS.Core.DTO.Captains
{
    public class RegCaptainDTO
    {
        public string LoggedInUserId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public Gender Gender { get; set; }
        public UserType UserType { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string PictureUrl { get; set; }
        public int Status { get; set; }
        public string Organisation { get; set; }
        public bool IsActive { get; set; }

        // Captain other info
        public string HomeAddress { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public int Age { get; set; }
        public DateTime EmploymentDate { get; set; }
    }

    public class ViewCaptainsDTO
    {
        public int PartnerId { get; set; }
        public string Name { get; set; }
        public string CaptainCode { get; set; }
        public string Email { get; set; }
        public string VehicleAssigned { get; set; }
        public string Status { get; set; }
        public DateTime EmploymentDate { get; set; }
    }

    public class CaptainDetailsDTO
    {
        public int PartnerId { get; set; }
        public string CaptainName { get; set; }
        public string CaptainLastName { get; set; }
        public string CaptainFirstName { get; set; }
        public string CaptainPhoneNumber { get; set; }
        public string PictureUrl { get; set; }
        public int CaptainAge { get; set; }
        public string CaptainCode { get; set; }
        public string Email { get; set; }
        public string AssignedVehicleName { get; set; }
        public string AssignedVehicleNumber { get; set; }
        public string Status { get; set; }
        public DateTime EmploymentDate { get; set; }
    }

    public class UpdateCaptainDTO
    {
        public int PartnerId { get; set; }
        public string PictureUrl { get; set; }
        public string CaptainCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string CaptainPhoneNumber { get; set; }
        public int CaptainAge { get; set; }
        public string Status { get; set; }
        public string EmploymentDate { get; set; }
        public string AssignedVehicleType {get; set; }
        public string AssignedVehicleNumber {get; set; }
    }
    
    public class RegisterVehicleDTO
    {
        public string RegistrationNumber { get; set; }
        public DateTime DateOfCommission { get; set; }
        public string Status { get; set; }
        public string VehicleOwner { get; set; }
        public string AssignedCaptain { get; set; }
        public string VehicleType { get; set; }
        public string VehicleName { get; set; }
        public int VehicleCapacity { get; set; }
        public string PartnerEmail { get; set; }
        public int PartnerId { get; set; }
        public string IsFixed { get; set; }
    }

    public class CurrentMonthDetailsDTO
    {
        public DateTime FirstDay { get; set; }
        public DateTime LastDay { get; set; }
    }

    public class VehicleDTO
    {
        public int FleetId { get; set; }
        public string RegistrationNumber { get; set; }
        public string FleetName { get; set; }
        public string VehicleOwner { get; set; }
        public string AssignedCaptain { get; set; }
        public string Status { get; set; }
        public int VehicleAge { get; set; }
        public string VehicleOwnerId { get; set; }
        public string IsFixed { get; set; }
    }
    
    public class VehicleDetailsDTO
    {
        public int FleetId { get; set; }
        public string FleetName { get; set; }
        public string RegistrationNumber { get; set; }
        public string AssignedCaptain { get; set; }
        public int VehicleAge { get; set; }
        public string Status { get; set; }
        public string VehicleOwner { get; set; }
        public string VehicleType { get; set; }
        public int Capacity { get; set; }
        public int PartnerId { get; set; }
        public string VehicleOwnerId { get; set; }
        public string IsFixed { get; set; }
    }
}
