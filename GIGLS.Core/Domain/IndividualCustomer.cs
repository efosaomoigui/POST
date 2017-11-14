using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.Enums;
using System;
using System.Collections.Generic;

namespace GIGL.GIGLS.Core.Domain
{
    public class IndividualCustomer : BaseDomain, IAuditable
    {
        public IndividualCustomer()
        {
            Shipments = new HashSet<Shipment>();
        }
        public int IndividualCustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string PictureUrl { get; set; }

        public virtual ICollection<Shipment> Shipments { get; set; }
    }
}