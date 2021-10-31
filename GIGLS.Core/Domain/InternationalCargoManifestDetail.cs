using GIGLS.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIGLS.Core.Domain
{
    public class InternationalCargoManifestDetail : BaseDomain, IAuditable
    {
        [Key]
        public int InternationalCargoManifestDetailId { get; set; }
        [MaxLength(128)]
        public string RequestNumber { get; set; }
        [MaxLength(128)]
        public string Waybill { get; set; }
        public double Weight { get; set; }
        public int Quantity { get; set; }
        [MaxLength(500)]
        public string ItemUniqueNo { get; set; }
        [MaxLength(500)]
        public string CourierService { get; set; }
        [MaxLength(500)]
        public string ItemName { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public int InternationalCargoManifestId { get; set; }
        [MaxLength(500)]
        public string ItemRequestCode { get; set; }
        public int NoOfPackageReceived { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal DeclaredValue { get; set; }
        public virtual InternationalCargoManifest InternationalCargoManifest { get; set; }

    }
}

