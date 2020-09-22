using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.Shipments
{
    public class ShipmentPackagePriceDTO : BaseDomainDTO
    {
        public int ShipmentPackagePriceId { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CountryId { get; set; }
        public int Balance { get; set; }
        public int QuantityToBeAdded { get; set; }

        public int StartingInventory { get; set; }
        public int InventoryReceived { get; set; }
        public int InventoryShipped { get; set; }
        public int InventoryOnHand { get; set; }
        public int MinimunRequired { get; set; }

        public CountryDTO Country { get; set; }
    }
}
