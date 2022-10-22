using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using POST.Core.DTO.Captains;
using POST.Infrastructure;

namespace POST.Services.Implementation.Utility
{
    public static class InputsValidator
    {
        private const string emailExpre = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
        private const string phoneExpre = @"/((^090)([23589]))|((^070)([1-9]))|((^080)([2-9]))|((^081)([0-9]))(\d{7})|((^091)([0-9]))(\d{7})/";
        private const string accountNumberExpre = @"(\d{10})";
        private const string ageExpre = @"(\d{2})";

        public static bool ValidateEmail(string email)
        {
            var result = Regex.IsMatch(email, emailExpre);
            return result;
        }

        public static bool ValidatePhoneNumber(string number)
        {
            var result = Regex.IsMatch(number, phoneExpre);
            return result;
        }

        public static string ValidateRegisterCaptainExcelFile(List<RegCaptainDTO> dto)
        {
            //string output = string.Empty;
            foreach (var regCaptainDto in dto)
            {
                if (regCaptainDto == null)
                {
                    throw new GenericException($"No blank row required!");
                }

                if (string.IsNullOrEmpty(regCaptainDto.Email) || !Regex.IsMatch(regCaptainDto.Email, emailExpre))
                {
                    throw new GenericException($"Email: {regCaptainDto.Email} is not in right format");
                }

                if (regCaptainDto.Age.GetType() != typeof(int) || regCaptainDto.Age <= 0 || regCaptainDto.Age > 999)
                {
                    throw new GenericException($"Age: {regCaptainDto.Age} is not valid");
                }

                if (regCaptainDto.AccountNumber.GetType() != typeof(string) || !Regex.IsMatch(regCaptainDto.AccountNumber, accountNumberExpre))
                {
                    throw new GenericException($"Account Number: {regCaptainDto.AccountNumber} is not valid");
                }

                if (regCaptainDto.PhoneNumber.GetType() != typeof(string) || !Regex.IsMatch(regCaptainDto.PhoneNumber, phoneExpre))
                {
                    throw new GenericException($"Phone Number: {regCaptainDto.PhoneNumber} is not valid");
                }

                if (regCaptainDto.EmploymentDate.GetType() != typeof(DateTime) || regCaptainDto.EmploymentDate == null)
                {
                    throw new GenericException($"Employment Date: {regCaptainDto.EmploymentDate} is not in right format");
                }

                if (regCaptainDto.AccountName.GetType() != typeof(string) || string.IsNullOrEmpty(regCaptainDto.AccountName) || regCaptainDto.BankName.GetType() != typeof(string) || string.IsNullOrEmpty(regCaptainDto.BankName))
                {
                    throw new GenericException($"Account name: {regCaptainDto.AccountName} or Bank name: {regCaptainDto.BankName} is not valid");
                }

                if (regCaptainDto.FirstName.GetType() != typeof(string) || string.IsNullOrEmpty(regCaptainDto.FirstName) || regCaptainDto.LastName.GetType() != typeof(string) || string.IsNullOrEmpty(regCaptainDto.LastName))
                {
                    throw new GenericException($"Firstname: {regCaptainDto.FirstName} or Lastname: {regCaptainDto.LastName} is not valid");
                }

                if (regCaptainDto.Designation.GetType() != typeof(string) || string.IsNullOrEmpty(regCaptainDto.Designation) || regCaptainDto.Department.GetType() != typeof(string) || string.IsNullOrEmpty(regCaptainDto.Department))
                {
                    throw new GenericException($"Department: {regCaptainDto.Department} or Designation: {regCaptainDto.Designation} is not valid");
                }

                if (regCaptainDto.Address.GetType() != typeof(string) || string.IsNullOrEmpty(regCaptainDto.Address) || regCaptainDto.HomeAddress.GetType() != typeof(string) || string.IsNullOrEmpty(regCaptainDto.HomeAddress))
                {
                    throw new GenericException($"Address or HomeAddress is not valid");
                }

                if (regCaptainDto.Organisation.GetType() != typeof(string) || string.IsNullOrEmpty(regCaptainDto.Organisation))
                {
                    throw new GenericException($"Organization: {regCaptainDto.Organisation} is not valid");
                }
            }
            return null;
        }

        public static string ValidateRegisterVehicleExcelFile(List<RegisterVehicleDTO> dto)
        {
            //string output = string.Empty;
            foreach (var vehicle in dto)
            {
                if (vehicle == null)
                {
                    throw new GenericException($"No blank row required!");
                }

                if (string.IsNullOrEmpty(vehicle.PartnerEmail) || !Regex.IsMatch(vehicle.PartnerEmail, emailExpre))
                {
                    throw new GenericException($"Email: {vehicle.PartnerEmail} is not in right format");
                }

                if (vehicle.DateOfCommission.GetType() != typeof(DateTime) || vehicle.DateOfCommission == null)
                {
                    throw new GenericException($"Employment Date: {vehicle.DateOfCommission} is not in right format");
                }

                if (string.IsNullOrEmpty(vehicle.RegistrationNumber) || vehicle.RegistrationNumber.Length <= 0 || vehicle.RegistrationNumber.Length > 10 )
                {
                    throw new GenericException($"Vehicle Number: {vehicle.RegistrationNumber} is not valid, can not be empty and must be 10 digits");
                }

                if (string.IsNullOrEmpty(vehicle.VehicleType) || string.IsNullOrEmpty(vehicle.VehicleName))
                {
                    throw new GenericException($"Vehicle owner: {vehicle.VehicleOwner} or Vehicle name: {vehicle.VehicleName} is not valid");
                }

                if (vehicle.IsFixed.GetType() != typeof(string) || string.IsNullOrEmpty(vehicle.IsFixed) || vehicle.IsFixed.ToLower() != "fixed" || vehicle.IsFixed.ToLower() != "variable")
                {
                    throw new GenericException($"Vehicle Revenue Status is not valid");
                }

                if (vehicle.VehicleOwner.GetType() != typeof(string) || string.IsNullOrEmpty(vehicle.VehicleOwner))
                {
                    throw new GenericException($"Vehicle Owner: {vehicle.VehicleOwner} is not valid");
                }
            }
            return null;
        }
    }
}
