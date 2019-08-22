using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.Fleets;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GIGLS.Core.Enums;
using System;

namespace GIGLS.Core.DTO.User
{
    public class UserDTO
    {
        public UserDTO()
        {
            ServiceCentres = new List<ServiceCentreDTO>();
            Shipments = new List<ShipmentDTO>();
            FleetTrips = new List<FleetTripDTO>();
        }

        public string Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public Gender Gender { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string PictureUrl { get; set; }
        public int Status { get; set; }
        public string Organisation { get; set; }
        public bool IsActive { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserActiveServiceCentre { get; set; }
        public UserType UserType { get; set; }
        public List<ServiceCentreDTO> ServiceCentres { get; set; }
        public List<ShipmentDTO> Shipments { get; set; }
        public List<FleetTripDTO> FleetTrips { get; set; }
        public DateTime DateCreated { get; set; }

        //for system user
        public string SystemUserId { get; set; }
        public string SystemUserRole { get; set; }

        // UserChannel
        public string UserChannelCode { get; set; }
        public string UserChannelPassword { get; set; }
        public UserChannelType UserChannelType { get; set; }
        
        public DateTime PasswordExpireDate { get; set; }

        public string CustomerId { get; set; }

        //user priviledge countries
        public List<CountryDTO> Country { get; set; }
        public List<string> CountryName { get; set; }

        public int UserActiveCountryId { get; set; }
        public CountryDTO UserActiveCountry { get; set; }
        public List<string> VehicleType { get; set; }
        public bool IsFromMobile { get; set; }

        public string Referrercode { get; set; }

        public double AverageRatings { get; set; }
    }
}
