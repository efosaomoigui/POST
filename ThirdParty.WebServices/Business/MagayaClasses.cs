using Newtonsoft.Json;
using System.Collections.Generic;
using System.Xml.Serialization;
using ThirdParty.WebServices.Business;

namespace ThirdParty.WebServices.Magaya.Business.New
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("Event", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class EventType
    {
        private System.DateTime date;

        private string details;

        private string location;

        private EventDefinitionType eventDefinition;

        private bool includeInTracking;

        private bool includeInTrackingSpecified;

        private string ownerType;

        private string ownerGUID;

        private string ownerNumber;

        private string ownerURL;

        /// <remarks/>
        public System.DateTime Date
        {
            get
            {
                return this.date;
            }
            set
            {
                this.date = value;
            }
        }

        /// <remarks/>
        public string Details
        {
            get
            {
                return this.details;
            }
            set
            {
                this.details = value;
            }
        }

        /// <remarks/>
        public string Location
        {
            get
            {
                return this.location;
            }
            set
            {
                this.location = value;
            }
        }

        /// <remarks/>
        public EventDefinitionType EventDefinition
        {
            get
            {
                return this.eventDefinition;
            }
            set
            {
                this.eventDefinition = value;
            }
        }

        /// <remarks/>
        public bool IncludeInTracking
        {
            get
            {
                return this.includeInTracking;
            }
            set
            {
                this.includeInTracking = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IncludeInTrackingSpecified
        {
            get
            {
                return this.includeInTrackingSpecified;
            }
            set
            {
                this.includeInTrackingSpecified = value;
            }
        }

        /// <remarks/>
        public string OwnerType
        {
            get
            {
                return this.ownerType;
            }
            set
            {
                this.ownerType = value;
            }
        }

        /// <remarks/>
        public string OwnerGUID
        {
            get
            {
                return this.ownerGUID;
            }
            set
            {
                this.ownerGUID = value;
            }
        }

        /// <remarks/>
        public string OwnerNumber
        {
            get
            {
                return this.ownerNumber;
            }
            set
            {
                this.ownerNumber = value;
            }
        }

        /// <remarks/>
        public string OwnerURL
        {
            get
            {
                return this.ownerURL;
            }
            set
            {
                this.ownerURL = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class EventDefinitionType
    {

        private string name;

        private string location;

        private string details;

        private bool includeInTracking;

        private bool includeInTrackingSpecified;

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        /// <remarks/>
        public string Location
        {
            get
            {
                return this.location;
            }
            set
            {
                this.location = value;
            }
        }

        /// <remarks/>
        public string Details
        {
            get
            {
                return this.details;
            }
            set
            {
                this.details = value;
            }
        }

        /// <remarks/>
        public bool IncludeInTracking
        {
            get
            {
                return this.includeInTracking;
            }
            set
            {
                this.includeInTracking = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IncludeInTrackingSpecified
        {
            get
            {
                return this.includeInTrackingSpecified;
            }
            set
            {
                this.includeInTrackingSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class PODItem
    {

        private string gUID;

        private string quantity;

        /// <remarks/>
        public string GUID
        {
            get
            {
                return this.gUID;
            }
            set
            {
                this.gUID = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string Quantity
        {
            get
            {
                return this.quantity;
            }
            set
            {
                this.quantity = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class RepackingSettings
    {

        private bool allowRepackingOnReceive;

        private bool allowRepackingOnReceiveSpecified;

        private bool allowRepackingOnPick;

        private bool allowRepackingOnPickSpecified;

        private bool allowRepackingOnMove;

        private bool allowRepackingOnMoveSpecified;

        /// <remarks/>
        public bool AllowRepackingOnReceive
        {
            get
            {
                return this.allowRepackingOnReceive;
            }
            set
            {
                this.allowRepackingOnReceive = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AllowRepackingOnReceiveSpecified
        {
            get
            {
                return this.allowRepackingOnReceiveSpecified;
            }
            set
            {
                this.allowRepackingOnReceiveSpecified = value;
            }
        }

        /// <remarks/>
        public bool AllowRepackingOnPick
        {
            get
            {
                return this.allowRepackingOnPick;
            }
            set
            {
                this.allowRepackingOnPick = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AllowRepackingOnPickSpecified
        {
            get
            {
                return this.allowRepackingOnPickSpecified;
            }
            set
            {
                this.allowRepackingOnPickSpecified = value;
            }
        }

        /// <remarks/>
        public bool AllowRepackingOnMove
        {
            get
            {
                return this.allowRepackingOnMove;
            }
            set
            {
                this.allowRepackingOnMove = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AllowRepackingOnMoveSpecified
        {
            get
            {
                return this.allowRepackingOnMoveSpecified;
            }
            set
            {
                this.allowRepackingOnMoveSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class ReceivingSettings
    {

        private bool enforceDefaultLocation;

        private bool enforceDefaultLocationSpecified;

        private bool skipEntities;

        private bool skipEntitiesSpecified;

        private bool confirmNewItemCreation;

        private bool confirmNewItemCreationSpecified;

        private bool promptPropertiesWhenItemIsCreated;

        private bool promptPropertiesWhenItemIsCreatedSpecified;

        private bool allowReceptionOfEmptyPallets;

        private bool allowReceptionOfEmptyPalletsSpecified;

        /// <remarks/>
        public bool EnforceDefaultLocation
        {
            get
            {
                return this.enforceDefaultLocation;
            }
            set
            {
                this.enforceDefaultLocation = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool EnforceDefaultLocationSpecified
        {
            get
            {
                return this.enforceDefaultLocationSpecified;
            }
            set
            {
                this.enforceDefaultLocationSpecified = value;
            }
        }

        /// <remarks/>
        public bool SkipEntities
        {
            get
            {
                return this.skipEntities;
            }
            set
            {
                this.skipEntities = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool SkipEntitiesSpecified
        {
            get
            {
                return this.skipEntitiesSpecified;
            }
            set
            {
                this.skipEntitiesSpecified = value;
            }
        }

        /// <remarks/>
        public bool ConfirmNewItemCreation
        {
            get
            {
                return this.confirmNewItemCreation;
            }
            set
            {
                this.confirmNewItemCreation = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ConfirmNewItemCreationSpecified
        {
            get
            {
                return this.confirmNewItemCreationSpecified;
            }
            set
            {
                this.confirmNewItemCreationSpecified = value;
            }
        }

        /// <remarks/>
        public bool PromptPropertiesWhenItemIsCreated
        {
            get
            {
                return this.promptPropertiesWhenItemIsCreated;
            }
            set
            {
                this.promptPropertiesWhenItemIsCreated = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool PromptPropertiesWhenItemIsCreatedSpecified
        {
            get
            {
                return this.promptPropertiesWhenItemIsCreatedSpecified;
            }
            set
            {
                this.promptPropertiesWhenItemIsCreatedSpecified = value;
            }
        }

        /// <remarks/>
        public bool AllowReceptionOfEmptyPallets
        {
            get
            {
                return this.allowReceptionOfEmptyPallets;
            }
            set
            {
                this.allowReceptionOfEmptyPallets = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AllowReceptionOfEmptyPalletsSpecified
        {
            get
            {
                return this.allowReceptionOfEmptyPalletsSpecified;
            }
            set
            {
                this.allowReceptionOfEmptyPalletsSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class ServicesType
    {

        private bool allowVINDecode;

        private bool allowVINDecodeSpecified;

        /// <remarks/>
        public bool AllowVINDecode
        {
            get
            {
                return this.allowVINDecode;
            }
            set
            {
                this.allowVINDecode = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AllowVINDecodeSpecified
        {
            get
            {
                return this.allowVINDecodeSpecified;
            }
            set
            {
                this.allowVINDecodeSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class TaskObjectType
    {

        private object item;

        private ItemChoiceType2 itemElementName;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("AirBooking", typeof(MagayaShipment))]
        [System.Xml.Serialization.XmlElementAttribute("AirShipment", typeof(MagayaShipment))]
        [System.Xml.Serialization.XmlElementAttribute("Bill", typeof(AccountingItem))]
        [System.Xml.Serialization.XmlElementAttribute("BillCredit", typeof(AccountingItem))]
        [System.Xml.Serialization.XmlElementAttribute("CargoCount", typeof(CargoCountType))]
        [System.Xml.Serialization.XmlElementAttribute("CargoMove", typeof(CargoMoveType))]
        [System.Xml.Serialization.XmlElementAttribute("CargoRelease", typeof(CargoReleaseType))]
        [System.Xml.Serialization.XmlElementAttribute("CreditMemo", typeof(AccountingItem))]
        [System.Xml.Serialization.XmlElementAttribute("GroundBooking", typeof(MagayaShipment))]
        [System.Xml.Serialization.XmlElementAttribute("GroundShipment", typeof(MagayaShipment))]
        [System.Xml.Serialization.XmlElementAttribute("Invoice", typeof(AccountingItem))]
        [System.Xml.Serialization.XmlElementAttribute("Job", typeof(JobType))]
        [System.Xml.Serialization.XmlElementAttribute("OceanBooking", typeof(MagayaShipment))]
        [System.Xml.Serialization.XmlElementAttribute("OceanShipment", typeof(MagayaShipment))]
        [System.Xml.Serialization.XmlElementAttribute("PickupOrder", typeof(PickupOrderType))]
        [System.Xml.Serialization.XmlElementAttribute("PurchaseOrder", typeof(OrderType))]
        [System.Xml.Serialization.XmlElementAttribute("Quotation", typeof(QuotationType))]
        [System.Xml.Serialization.XmlElementAttribute("SalesOrder", typeof(OrderType))]
        [System.Xml.Serialization.XmlElementAttribute("WarehouseReceipt", typeof(WarehouseReceipt))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
        public object Item
        {
            get
            {
                return this.item;
            }
            set
            {
                this.item = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemChoiceType2 ItemElementName
        {
            get
            {
                return this.itemElementName;
            }
            set
            {
                this.itemElementName = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("AirShipment", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class MagayaShipment 
    {

        private System.DateTime createdOn;

        private string number;

        private string createdByName;

        private sbyte version;

        private bool versionSpecified;

        private ModeOfTransportation modeOfTransportation;

        private string modeOfTransportCode;

        private Entity issuedBy;

        private Address issuedByAddress;

        private string issuedByName;

        private string shipperName;

        private Address shipperAddress;

        private Entity shipper;

        private string consigneeName;

        private Address consigneeAddress;

        private Entity consignee;

        private string destinationAgentName;

        private Address destinationAgentAddress;

        private Entity destinationAgent;

        private Entity carrier;

        private string carrierName;

        private Address carrierAddress;

        private string notes;

        private string waybillNotes;

        private Item[] items;

        private MeasurementUnits measurementUnits;

        private long creatorNetworkID;

        private bool creatorNetworkIDSpecified;

        private ChargeList charges;

        private EventType[] events;

        private Entity division;

        private string totalPieces;

        private WeightValue totalWeight;

        private VolumeValue totalVolume;

        private MoneyValue totalValue;

        private VolumeWeightValue totalVolumeWeight;

        private WeightValue chargeableWeight;

        private string descriptionOfGoods;

        private Port originPort;

        private Port destinationPort;

        private Port receiptPort;

        private Port deliveryPort;

        private ShipmentStatusType status;

        private bool statusSpecified;

        private ShipmentDirectionType direction;

        private ShipmentLayoutType layoutType;

        private bool layoutTypeSpecified;

        private System.DateTime estimatedArrivalDate;

        private bool estimatedArrivalDateSpecified;

        private System.DateTime actualArrivalDate;

        private bool actualArrivalDateSpecified;

        private System.DateTime estimatedDepartureDate;

        private bool estimatedDepartureDateSpecified;

        private System.DateTime actualDepartureDate;

        private bool actualDepartureDateSpecified;

        private Entity notifyParty;

        private string notifyPartyName;

        private Address notifyPartyAddress;

        private Entity intermediateConsignee;

        private string intermediateConsigneeName;

        private Address intermediateConsigneeAddress;

        private Entity billingClient;

        private string executedPlace;

        private string executedCityCode;

        private string declaredValue;

        private string bookingNumber;

        private string masterGUID;

        private string masterNumber;

        private long receivedAtNetworkID;

        private bool receivedAtNetworkIDSpecified;

        private string originPortSchedule;

        private string destinationPortSchedule;

        private string aESXTNNumber;

        private System.DateTime aESFilingDate;

        private bool aESFilingDateSpecified;

        private string aESITNNumber;

        private string aESStatus;

        private string fromQuoteNumber;

        private InBondDataType inBondData;

        private Port lastPortOfDeparture;

        private System.DateTime lastDateOfDeparture;

        private bool lastDateOfDepartureSpecified;

        private string lastPortOfDepartureSchedule;

        private Entity aMSNotifyParty1;

        private string aMSNotifyParty1Name;

        private string aMSNotifyParty1SCAC;

        private Entity aMSNotifyParty2;

        private string aMSNotifyParty2Name;

        private string aMSNotifyParty2SCAC;

        private PODDataType pODData;

        private System.DateTime spottingDate;

        private bool spottingDateSpecified;

        private System.DateTime pickupDate;

        private bool pickupDateSpecified;

        private System.DateTime cutoffDate;

        private bool cutoffDateSpecified;

        private Address cargoLocation;

        private WeightValue maximumWeight;

        private VolumeValue maximumVolume;

        private string maximumPieces;

        private string masterWayBillNumber;

        private ShipmentList houseShipments;

        private DocumentType[] documents;

        private bool hasAttachments;

        private bool hasAttachmentsSpecified;

        private AttachmentType[] attachments;

        private BookingStatusType bookingStatus;

        private bool bookingStatusSpecified;

        private ShipmentOptionsType shipmentOptions;

        private AirShipment airShipmentInfo;

        private OceanShipment oceanShipmentInfo;

        private GroundShipment groundShipmentInfo;

        private Item[] tripEquipments;

        private string uRL;

        private string deliveryAgentName;

        private Entity deliveryAgent;

        private Address destinationPortAddress;

        private string routingInstructions;

        private bool isLessThanFullLoad;

        private bool isLessThanFullLoadSpecified;

        private ServiceType service;

        private bool serviceSpecified;

        private bool isOnlineBooking;

        private bool isOnlineBookingSpecified;

        private string entryNumber;

        private string insuranceNumber;

        private System.DateTime insuranceDate;

        private bool insuranceDateSpecified;

        private CustomType[] customs;

        private bool isOnline;

        private bool isOnlineSpecified;

        private EntityRoleType[] entityRoles;

        private ISFDataType iSFData;

        private GUIDItem createdFrom;

        private string containersNumbers;

        private string totalContainers;

        private string containers20Foot;

        private string containers40Foot;

        private string containers45Foot;

        private string tankContainers;

        private RouteSegment[] routeSegments;

        private DeniedPartyScreeningResultType deniedPartyScreeningResultData;

        private Entity destinationPickupEntity;

        private string destinationPickupEntityName;

        private bool isLiquidated;

        private bool isLiquidatedSpecified;

        private ExpressLinkInfoType expressLinkInfo;

        private string gUID;

        private string type;

        /// <remarks/>
        public System.DateTime CreatedOn
        {
            get
            {
                return this.createdOn;
            }
            set
            {
                this.createdOn = value;
            }
        }

        /// <remarks/>
        public string Number
        {
            get
            {
                return this.number;
            }
            set
            {
                this.number = value;
            }
        }

        /// <remarks/>
        public string CreatedByName
        {
            get
            {
                return this.createdByName;
            }
            set
            {
                this.createdByName = value;
            }
        }

        /// <remarks/>
        public sbyte Version
        {
            get
            {
                return this.version;
            }
            set
            {
                this.version = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool VersionSpecified
        {
            get
            {
                return this.versionSpecified;
            }
            set
            {
                this.versionSpecified = value;
            }
        }

        /// <remarks/>
        public ModeOfTransportation ModeOfTransportation
        {
            get
            {
                return this.modeOfTransportation;
            }
            set
            {
                this.modeOfTransportation = value;
            }
        }

        /// <remarks/>
        public string ModeOfTransportCode
        {
            get
            {
                return this.modeOfTransportCode;
            }
            set
            {
                this.modeOfTransportCode = value;
            }
        }

        /// <remarks/>
        public Entity IssuedBy
        {
            get
            {
                return this.issuedBy;
            }
            set
            {
                this.issuedBy = value;
            }
        }

        /// <remarks/>
        public Address IssuedByAddress
        {
            get
            {
                return this.issuedByAddress;
            }
            set
            {
                this.issuedByAddress = value;
            }
        }

        /// <remarks/>
        public string IssuedByName
        {
            get
            {
                return this.issuedByName;
            }
            set
            {
                this.issuedByName = value;
            }
        }

        /// <remarks/>
        public string ShipperName
        {
            get
            {
                return this.shipperName;
            }
            set
            {
                this.shipperName = value;
            }
        }

        /// <remarks/>
        public Address ShipperAddress
        {
            get
            {
                return this.shipperAddress;
            }
            set
            {
                this.shipperAddress = value;
            }
        }

        /// <remarks/>
        public Entity Shipper
        {
            get
            {
                return this.shipper;
            }
            set
            {
                this.shipper = value;
            }
        }

        /// <remarks/>
        public string ConsigneeName
        {
            get
            {
                return this.consigneeName;
            }
            set
            {
                this.consigneeName = value;
            }
        }

        /// <remarks/>
        public Address ConsigneeAddress
        {
            get
            {
                return this.consigneeAddress;
            }
            set
            {
                this.consigneeAddress = value;
            }
        }

        /// <remarks/>
        public Entity Consignee
        {
            get
            {
                return this.consignee;
            }
            set
            {
                this.consignee = value;
            }
        }

        /// <remarks/>
        public string DestinationAgentName
        {
            get
            {
                return this.destinationAgentName;
            }
            set
            {
                this.destinationAgentName = value;
            }
        }

        /// <remarks/>
        public Address DestinationAgentAddress
        {
            get
            {
                return this.destinationAgentAddress;
            }
            set
            {
                this.destinationAgentAddress = value;
            }
        }

        /// <remarks/>
        public Entity DestinationAgent
        {
            get
            {
                return this.destinationAgent;
            }
            set
            {
                this.destinationAgent = value;
            }
        }

        /// <remarks/>
        public Entity Carrier
        {
            get
            {
                return this.carrier;
            }
            set
            {
                this.carrier = value;
            }
        }

        /// <remarks/>
        public string CarrierName
        {
            get
            {
                return this.carrierName;
            }
            set
            {
                this.carrierName = value;
            }
        }

        /// <remarks/>
        public Address CarrierAddress
        {
            get
            {
                return this.carrierAddress;
            }
            set
            {
                this.carrierAddress = value;
            }
        }

        /// <remarks/>
        public string Notes
        {
            get
            {
                return this.notes;
            }
            set
            {
                this.notes = value;
            }
        }

        /// <remarks/>
        public string WaybillNotes
        {
            get
            {
                return this.waybillNotes;
            }
            set
            {
                this.waybillNotes = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Item", IsNullable = false)]
        public Item[] Items
        {
            get
            {
                return this.items;
            }
            set
            {
                this.items = value;
            }
        }

        /// <remarks/>
        public MeasurementUnits MeasurementUnits
        {
            get
            {
                return this.measurementUnits;
            }
            set
            {
                this.measurementUnits = value;
            }
        }

        /// <remarks/>
        public long CreatorNetworkID
        {
            get
            {
                return this.creatorNetworkID;
            }
            set
            {
                this.creatorNetworkID = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CreatorNetworkIDSpecified
        {
            get
            {
                return this.creatorNetworkIDSpecified;
            }
            set
            {
                this.creatorNetworkIDSpecified = value;
            }
        }

        /// <remarks/>
        public ChargeList Charges
        {
            get
            {
                return this.charges;
            }
            set
            {
                this.charges = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Event", IsNullable = false)]
        public EventType[] Events
        {
            get
            {
                return this.events;
            }
            set
            {
                this.events = value;
            }
        }

        /// <remarks/>
        public Entity Division
        {
            get
            {
                return this.division;
            }
            set
            {
                this.division = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string TotalPieces
        {
            get
            {
                return this.totalPieces;
            }
            set
            {
                this.totalPieces = value;
            }
        }

        /// <remarks/>
        public WeightValue TotalWeight
        {
            get
            {
                return this.totalWeight;
            }
            set
            {
                this.totalWeight = value;
            }
        }

        /// <remarks/>
        public VolumeValue TotalVolume
        {
            get
            {
                return this.totalVolume;
            }
            set
            {
                this.totalVolume = value;
            }
        }

        /// <remarks/>
        public MoneyValue TotalValue
        {
            get
            {
                return this.totalValue;
            }
            set
            {
                this.totalValue = value;
            }
        }

        /// <remarks/>
        public VolumeWeightValue TotalVolumeWeight
        {
            get
            {
                return this.totalVolumeWeight;
            }
            set
            {
                this.totalVolumeWeight = value;
            }
        }

        /// <remarks/>
        public WeightValue ChargeableWeight
        {
            get
            {
                return this.chargeableWeight;
            }
            set
            {
                this.chargeableWeight = value;
            }
        }

        /// <remarks/>
        public string DescriptionOfGoods
        {
            get
            {
                return this.descriptionOfGoods;
            }
            set
            {
                this.descriptionOfGoods = value;
            }
        }

        /// <remarks/>
        public Port OriginPort
        {
            get
            {
                return this.originPort;
            }
            set
            {
                this.originPort = value;
            }
        }

        /// <remarks/>
        public Port DestinationPort
        {
            get
            {
                return this.destinationPort;
            }
            set
            {
                this.destinationPort = value;
            }
        }

        /// <remarks/>
        public Port ReceiptPort
        {
            get
            {
                return this.receiptPort;
            }
            set
            {
                this.receiptPort = value;
            }
        }

        /// <remarks/>
        public Port DeliveryPort
        {
            get
            {
                return this.deliveryPort;
            }
            set
            {
                this.deliveryPort = value;
            }
        }

        /// <remarks/>
        public ShipmentStatusType Status
        {
            get
            {
                return this.status;
            }
            set
            {
                this.status = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool StatusSpecified
        {
            get
            {
                return this.statusSpecified;
            }
            set
            {
                this.statusSpecified = value;
            }
        }

        /// <remarks/>
        public ShipmentDirectionType Direction
        {
            get
            {
                return this.direction;
            }
            set
            {
                this.direction = value;
            }
        }

        /// <remarks/>
        public ShipmentLayoutType LayoutType
        {
            get
            {
                return this.layoutType;
            }
            set
            {
                this.layoutType = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool LayoutTypeSpecified
        {
            get
            {
                return this.layoutTypeSpecified;
            }
            set
            {
                this.layoutTypeSpecified = value;
            }
        }

        /// <remarks/>
        public System.DateTime EstimatedArrivalDate
        {
            get
            {
                return this.estimatedArrivalDate;
            }
            set
            {
                this.estimatedArrivalDate = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool EstimatedArrivalDateSpecified
        {
            get
            {
                return this.estimatedArrivalDateSpecified;
            }
            set
            {
                this.estimatedArrivalDateSpecified = value;
            }
        }

        /// <remarks/>
        public System.DateTime ActualArrivalDate
        {
            get
            {
                return this.actualArrivalDate;
            }
            set
            {
                this.actualArrivalDate = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ActualArrivalDateSpecified
        {
            get
            {
                return this.actualArrivalDateSpecified;
            }
            set
            {
                this.actualArrivalDateSpecified = value;
            }
        }

        /// <remarks/>
        public System.DateTime EstimatedDepartureDate
        {
            get
            {
                return this.estimatedDepartureDate;
            }
            set
            {
                this.estimatedDepartureDate = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool EstimatedDepartureDateSpecified
        {
            get
            {
                return this.estimatedDepartureDateSpecified;
            }
            set
            {
                this.estimatedDepartureDateSpecified = value;
            }
        }

        /// <remarks/>
        public System.DateTime ActualDepartureDate
        {
            get
            {
                return this.actualDepartureDate;
            }
            set
            {
                this.actualDepartureDate = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ActualDepartureDateSpecified
        {
            get
            {
                return this.actualDepartureDateSpecified;
            }
            set
            {
                this.actualDepartureDateSpecified = value;
            }
        }

        /// <remarks/>
        public Entity NotifyParty
        {
            get
            {
                return this.notifyParty;
            }
            set
            {
                this.notifyParty = value;
            }
        }

        /// <remarks/>
        public string NotifyPartyName
        {
            get
            {
                return this.notifyPartyName;
            }
            set
            {
                this.notifyPartyName = value;
            }
        }

        /// <remarks/>
        public Address NotifyPartyAddress
        {
            get
            {
                return this.notifyPartyAddress;
            }
            set
            {
                this.notifyPartyAddress = value;
            }
        }

        /// <remarks/>
        public Entity IntermediateConsignee
        {
            get
            {
                return this.intermediateConsignee;
            }
            set
            {
                this.intermediateConsignee = value;
            }
        }

        /// <remarks/>
        public string IntermediateConsigneeName
        {
            get
            {
                return this.intermediateConsigneeName;
            }
            set
            {
                this.intermediateConsigneeName = value;
            }
        }

        /// <remarks/>
        public Address IntermediateConsigneeAddress
        {
            get
            {
                return this.intermediateConsigneeAddress;
            }
            set
            {
                this.intermediateConsigneeAddress = value;
            }
        }

        /// <remarks/>
        public Entity BillingClient
        {
            get
            {
                return this.billingClient;
            }
            set
            {
                this.billingClient = value;
            }
        }

        /// <remarks/>
        public string ExecutedPlace
        {
            get
            {
                return this.executedPlace;
            }
            set
            {
                this.executedPlace = value;
            }
        }

        /// <remarks/>
        public string ExecutedCityCode
        {
            get
            {
                return this.executedCityCode;
            }
            set
            {
                this.executedCityCode = value;
            }
        }

        /// <remarks/>
        public string DeclaredValue
        {
            get
            {
                return this.declaredValue;
            }
            set
            {
                this.declaredValue = value;
            }
        }

        /// <remarks/>
        public string BookingNumber
        {
            get
            {
                return this.bookingNumber;
            }
            set
            {
                this.bookingNumber = value;
            }
        }

        /// <remarks/>
        public string MasterGUID
        {
            get
            {
                return this.masterGUID;
            }
            set
            {
                this.masterGUID = value;
            }
        }

        /// <remarks/>
        public string MasterNumber
        {
            get
            {
                return this.masterNumber;
            }
            set
            {
                this.masterNumber = value;
            }
        }

        /// <remarks/>
        public long ReceivedAtNetworkID
        {
            get
            {
                return this.receivedAtNetworkID;
            }
            set
            {
                this.receivedAtNetworkID = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ReceivedAtNetworkIDSpecified
        {
            get
            {
                return this.receivedAtNetworkIDSpecified;
            }
            set
            {
                this.receivedAtNetworkIDSpecified = value;
            }
        }

        /// <remarks/>
        public string OriginPortSchedule
        {
            get
            {
                return this.originPortSchedule;
            }
            set
            {
                this.originPortSchedule = value;
            }
        }

        /// <remarks/>
        public string DestinationPortSchedule
        {
            get
            {
                return this.destinationPortSchedule;
            }
            set
            {
                this.destinationPortSchedule = value;
            }
        }

        /// <remarks/>
        public string AESXTNNumber
        {
            get
            {
                return this.aESXTNNumber;
            }
            set
            {
                this.aESXTNNumber = value;
            }
        }

        /// <remarks/>
        public System.DateTime AESFilingDate
        {
            get
            {
                return this.aESFilingDate;
            }
            set
            {
                this.aESFilingDate = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AESFilingDateSpecified
        {
            get
            {
                return this.aESFilingDateSpecified;
            }
            set
            {
                this.aESFilingDateSpecified = value;
            }
        }

        /// <remarks/>
        public string AESITNNumber
        {
            get
            {
                return this.aESITNNumber;
            }
            set
            {
                this.aESITNNumber = value;
            }
        }

        /// <remarks/>
        public string AESStatus
        {
            get
            {
                return this.aESStatus;
            }
            set
            {
                this.aESStatus = value;
            }
        }

        /// <remarks/>
        public string FromQuoteNumber
        {
            get
            {
                return this.fromQuoteNumber;
            }
            set
            {
                this.fromQuoteNumber = value;
            }
        }

        /// <remarks/>
        public InBondDataType InBondData
        {
            get
            {
                return this.inBondData;
            }
            set
            {
                this.inBondData = value;
            }
        }

        /// <remarks/>
        public Port LastPortOfDeparture
        {
            get
            {
                return this.lastPortOfDeparture;
            }
            set
            {
                this.lastPortOfDeparture = value;
            }
        }

        /// <remarks/>
        public System.DateTime LastDateOfDeparture
        {
            get
            {
                return this.lastDateOfDeparture;
            }
            set
            {
                this.lastDateOfDeparture = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool LastDateOfDepartureSpecified
        {
            get
            {
                return this.lastDateOfDepartureSpecified;
            }
            set
            {
                this.lastDateOfDepartureSpecified = value;
            }
        }

        /// <remarks/>
        public string LastPortOfDepartureSchedule
        {
            get
            {
                return this.lastPortOfDepartureSchedule;
            }
            set
            {
                this.lastPortOfDepartureSchedule = value;
            }
        }

        /// <remarks/>
        public Entity AMSNotifyParty1
        {
            get
            {
                return this.aMSNotifyParty1;
            }
            set
            {
                this.aMSNotifyParty1 = value;
            }
        }

        /// <remarks/>
        public string AMSNotifyParty1Name
        {
            get
            {
                return this.aMSNotifyParty1Name;
            }
            set
            {
                this.aMSNotifyParty1Name = value;
            }
        }

        /// <remarks/>
        public string AMSNotifyParty1SCAC
        {
            get
            {
                return this.aMSNotifyParty1SCAC;
            }
            set
            {
                this.aMSNotifyParty1SCAC = value;
            }
        }

        /// <remarks/>
        public Entity AMSNotifyParty2
        {
            get
            {
                return this.aMSNotifyParty2;
            }
            set
            {
                this.aMSNotifyParty2 = value;
            }
        }

        /// <remarks/>
        public string AMSNotifyParty2Name
        {
            get
            {
                return this.aMSNotifyParty2Name;
            }
            set
            {
                this.aMSNotifyParty2Name = value;
            }
        }

        /// <remarks/>
        public string AMSNotifyParty2SCAC
        {
            get
            {
                return this.aMSNotifyParty2SCAC;
            }
            set
            {
                this.aMSNotifyParty2SCAC = value;
            }
        }

        /// <remarks/>
        public PODDataType PODData
        {
            get
            {
                return this.pODData;
            }
            set
            {
                this.pODData = value;
            }
        }

        /// <remarks/>
        public System.DateTime SpottingDate
        {
            get
            {
                return this.spottingDate;
            }
            set
            {
                this.spottingDate = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool SpottingDateSpecified
        {
            get
            {
                return this.spottingDateSpecified;
            }
            set
            {
                this.spottingDateSpecified = value;
            }
        }

        /// <remarks/>
        public System.DateTime PickupDate
        {
            get
            {
                return this.pickupDate;
            }
            set
            {
                this.pickupDate = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool PickupDateSpecified
        {
            get
            {
                return this.pickupDateSpecified;
            }
            set
            {
                this.pickupDateSpecified = value;
            }
        }

        /// <remarks/>
        public System.DateTime CutoffDate
        {
            get
            {
                return this.cutoffDate;
            }
            set
            {
                this.cutoffDate = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CutoffDateSpecified
        {
            get
            {
                return this.cutoffDateSpecified;
            }
            set
            {
                this.cutoffDateSpecified = value;
            }
        }

        /// <remarks/>
        public Address CargoLocation
        {
            get
            {
                return this.cargoLocation;
            }
            set
            {
                this.cargoLocation = value;
            }
        }

        /// <remarks/>
        public WeightValue MaximumWeight
        {
            get
            {
                return this.maximumWeight;
            }
            set
            {
                this.maximumWeight = value;
            }
        }

        /// <remarks/>
        public VolumeValue MaximumVolume
        {
            get
            {
                return this.maximumVolume;
            }
            set
            {
                this.maximumVolume = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string MaximumPieces
        {
            get
            {
                return this.maximumPieces;
            }
            set
            {
                this.maximumPieces = value;
            }
        }

        /// <remarks/>
        public string MasterWayBillNumber
        {
            get
            {
                return this.masterWayBillNumber;
            }
            set
            {
                this.masterWayBillNumber = value;
            }
        }

        /// <remarks/>
        public ShipmentList HouseShipments
        {
            get
            {
                return this.houseShipments;
            }
            set
            {
                this.houseShipments = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Document", IsNullable = false)]
        public DocumentType[] Documents
        {
            get
            {
                return this.documents;
            }
            set
            {
                this.documents = value;
            }
        }

        /// <remarks/>
        public bool HasAttachments
        {
            get
            {
                return this.hasAttachments;
            }
            set
            {
                this.hasAttachments = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool HasAttachmentsSpecified
        {
            get
            {
                return this.hasAttachmentsSpecified;
            }
            set
            {
                this.hasAttachmentsSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Attachment", IsNullable = false)]
        public AttachmentType[] Attachments
        {
            get
            {
                return this.attachments;
            }
            set
            {
                this.attachments = value;
            }
        }

        /// <remarks/>
        public BookingStatusType BookingStatus
        {
            get
            {
                return this.bookingStatus;
            }
            set
            {
                this.bookingStatus = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool BookingStatusSpecified
        {
            get
            {
                return this.bookingStatusSpecified;
            }
            set
            {
                this.bookingStatusSpecified = value;
            }
        }

        /// <remarks/>
        public ShipmentOptionsType ShipmentOptions
        {
            get
            {
                return this.shipmentOptions;
            }
            set
            {
                this.shipmentOptions = value;
            }
        }

        /// <remarks/>
        public AirShipment AirShipmentInfo
        {
            get
            {
                return this.airShipmentInfo;
            }
            set
            {
                this.airShipmentInfo = value;
            }
        }

        /// <remarks/>
        public OceanShipment OceanShipmentInfo
        {
            get
            {
                return this.oceanShipmentInfo;
            }
            set
            {
                this.oceanShipmentInfo = value;
            }
        }

        /// <remarks/>
        public GroundShipment GroundShipmentInfo
        {
            get
            {
                return this.groundShipmentInfo;
            }
            set
            {
                this.groundShipmentInfo = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Item", IsNullable = false)]
        public Item[] TripEquipments
        {
            get
            {
                return this.tripEquipments;
            }
            set
            {
                this.tripEquipments = value;
            }
        }

        /// <remarks/>
        public string URL
        {
            get
            {
                return this.uRL;
            }
            set
            {
                this.uRL = value;
            }
        }

        /// <remarks/>
        public string DeliveryAgentName
        {
            get
            {
                return this.deliveryAgentName;
            }
            set
            {
                this.deliveryAgentName = value;
            }
        }

        /// <remarks/>
        public Entity DeliveryAgent
        {
            get
            {
                return this.deliveryAgent;
            }
            set
            {
                this.deliveryAgent = value;
            }
        }

        /// <remarks/>
        public Address DestinationPortAddress
        {
            get
            {
                return this.destinationPortAddress;
            }
            set
            {
                this.destinationPortAddress = value;
            }
        }

        /// <remarks/>
        public string RoutingInstructions
        {
            get
            {
                return this.routingInstructions;
            }
            set
            {
                this.routingInstructions = value;
            }
        }

        /// <remarks/>
        public bool IsLessThanFullLoad
        {
            get
            {
                return this.isLessThanFullLoad;
            }
            set
            {
                this.isLessThanFullLoad = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsLessThanFullLoadSpecified
        {
            get
            {
                return this.isLessThanFullLoadSpecified;
            }
            set
            {
                this.isLessThanFullLoadSpecified = value;
            }
        }

        /// <remarks/>
        public ServiceType Service
        {
            get
            {
                return this.service;
            }
            set
            {
                this.service = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ServiceSpecified
        {
            get
            {
                return this.serviceSpecified;
            }
            set
            {
                this.serviceSpecified = value;
            }
        }

        /// <remarks/>
        public bool IsOnlineBooking
        {
            get
            {
                return this.isOnlineBooking;
            }
            set
            {
                this.isOnlineBooking = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsOnlineBookingSpecified
        {
            get
            {
                return this.isOnlineBookingSpecified;
            }
            set
            {
                this.isOnlineBookingSpecified = value;
            }
        }

        /// <remarks/>
        public string EntryNumber
        {
            get
            {
                return this.entryNumber;
            }
            set
            {
                this.entryNumber = value;
            }
        }

        /// <remarks/>
        public string InsuranceNumber
        {
            get
            {
                return this.insuranceNumber;
            }
            set
            {
                this.insuranceNumber = value;
            }
        }

        /// <remarks/>
        public System.DateTime InsuranceDate
        {
            get
            {
                return this.insuranceDate;
            }
            set
            {
                this.insuranceDate = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool InsuranceDateSpecified
        {
            get
            {
                return this.insuranceDateSpecified;
            }
            set
            {
                this.insuranceDateSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Custom", IsNullable = false)]
        public CustomType[] Customs
        {
            get
            {
                return this.customs;
            }
            set
            {
                this.customs = value;
            }
        }

        /// <remarks/>
        public bool IsOnline
        {
            get
            {
                return this.isOnline;
            }
            set
            {
                this.isOnline = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsOnlineSpecified
        {
            get
            {
                return this.isOnlineSpecified;
            }
            set
            {
                this.isOnlineSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("EntityRole", IsNullable = false)]
        public EntityRoleType[] EntityRoles
        {
            get
            {
                return this.entityRoles;
            }
            set
            {
                this.entityRoles = value;
            }
        }

        /// <remarks/>
        public ISFDataType ISFData
        {
            get
            {
                return this.iSFData;
            }
            set
            {
                this.iSFData = value;
            }
        }

        /// <remarks/>
        public GUIDItem CreatedFrom
        {
            get
            {
                return this.createdFrom;
            }
            set
            {
                this.createdFrom = value;
            }
        }

        /// <remarks/>
        public string ContainersNumbers
        {
            get
            {
                return this.containersNumbers;
            }
            set
            {
                this.containersNumbers = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string TotalContainers
        {
            get
            {
                return this.totalContainers;
            }
            set
            {
                this.totalContainers = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string Containers20Foot
        {
            get
            {
                return this.containers20Foot;
            }
            set
            {
                this.containers20Foot = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string Containers40Foot
        {
            get
            {
                return this.containers40Foot;
            }
            set
            {
                this.containers40Foot = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string Containers45Foot
        {
            get
            {
                return this.containers45Foot;
            }
            set
            {
                this.containers45Foot = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string TankContainers
        {
            get
            {
                return this.tankContainers;
            }
            set
            {
                this.tankContainers = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("RouteSegment", IsNullable = false)]
        public RouteSegment[] RouteSegments
        {
            get
            {
                return this.routeSegments;
            }
            set
            {
                this.routeSegments = value;
            }
        }

        /// <remarks/>
        public DeniedPartyScreeningResultType DeniedPartyScreeningResultData
        {
            get
            {
                return this.deniedPartyScreeningResultData;
            }
            set
            {
                this.deniedPartyScreeningResultData = value;
            }
        }

        /// <remarks/>
        public Entity DestinationPickupEntity
        {
            get
            {
                return this.destinationPickupEntity;
            }
            set
            {
                this.destinationPickupEntity = value;
            }
        }

        /// <remarks/>
        public string DestinationPickupEntityName
        {
            get
            {
                return this.destinationPickupEntityName;
            }
            set
            {
                this.destinationPickupEntityName = value;
            }
        }

        /// <remarks/>
        public bool IsLiquidated
        {
            get
            {
                return this.isLiquidated;
            }
            set
            {
                this.isLiquidated = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsLiquidatedSpecified
        {
            get
            {
                return this.isLiquidatedSpecified;
            }
            set
            {
                this.isLiquidatedSpecified = value;
            }
        }

        /// <remarks/>
        public ExpressLinkInfoType ExpressLinkInfo
        {
            get
            {
                return this.expressLinkInfo;
            }
            set
            {
                this.expressLinkInfo = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string GUID
        {
            get
            {
                return this.gUID;
            }
            set
            {
                this.gUID = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class ModeOfTransportation
    {

        private string description;

        private MethodType method;

        //private bool? defaultField;

        private bool defaultSpecified;

        private CustomType[] customs;

        private string code;

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        /// <remarks/>
        public MethodType Method
        {
            get
            {
                return this.method;
            }
            set
            {
                this.method = value;
            }
        }

        /// <remarks/>
        //public bool? DefaultField
        //{
        //    get
        //    {
        //        return this.defaultField;
        //    }
        //    set
        //    {
        //        this.defaultField = value;
        //    }
        //}

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DefaultSpecified
        {
            get
            {
                return this.defaultSpecified;
            }
            set
            {
                this.defaultSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Custom", IsNullable = false)]
        public CustomType[] Customs
        {
            get
            {
                return this.customs;
            }
            set
            {
                this.customs = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Code
        {
            get
            {
                return this.code;
            }
            set
            {
                this.code = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum MethodType
    {

        /// <remarks/>
        Air,

        /// <remarks/>
        Ocean,

        /// <remarks/>
        Ground,

        /// <remarks/>
        Mail,

        /// <remarks/>
        Pipe,

        /// <remarks/>
        Rail,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class CustomType
    {

        private CustomDefinitionType customDefinition;

        private string value;

        private CustomObjectType relatedObject;

        /// <remarks/>
        public CustomDefinitionType CustomDefinition
        {
            get
            {
                return this.customDefinition;
            }
            set
            {
                this.customDefinition = value;
            }
        }

        /// <remarks/>
        public string Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }

        /// <remarks/>
        public CustomObjectType RelatedObject
        {
            get
            {
                return this.relatedObject;
            }
            set
            {
                this.relatedObject = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class CustomDefinitionType
    {

        private CustomDefType type;

        private string internalName;

        private string displayName;

        private string description;

        private string category;

        private bool internalUse;

        private bool internalUseSpecified;

        private bool buildReports;

        private bool buildReportsSpecified;

        private bool inactive;

        private bool inactiveSpecified;

        private CustomDefaultValueType defaultValue;

        private bool dateTimeFormat;

        private bool dateTimeFormatSpecified;

        private long maximumLength;

        private bool maximumLengthSpecified;

        private sbyte precision;

        private bool precisionSpecified;

        private string[] pickItems;

        private EntityDesc[] lookupItems;

        private bool isMultiline;

        private bool isMultilineSpecified;

        private bool isCalculated;

        private bool isCalculatedSpecified;

        private string code;

        private bool dateSystemTime;

        private bool dateSystemTimeSpecified;

        private bool isReadOnly;

        private bool isReadOnlySpecified;

        /// <remarks/>
        public CustomDefType Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }

        /// <remarks/>
        public string InternalName
        {
            get
            {
                return this.internalName;
            }
            set
            {
                this.internalName = value;
            }
        }

        /// <remarks/>
        public string DisplayName
        {
            get
            {
                return this.displayName;
            }
            set
            {
                this.displayName = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        /// <remarks/>
        public string Category
        {
            get
            {
                return this.category;
            }
            set
            {
                this.category = value;
            }
        }

        /// <remarks/>
        public bool InternalUse
        {
            get
            {
                return this.internalUse;
            }
            set
            {
                this.internalUse = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool InternalUseSpecified
        {
            get
            {
                return this.internalUseSpecified;
            }
            set
            {
                this.internalUseSpecified = value;
            }
        }

        /// <remarks/>
        public bool BuildReports
        {
            get
            {
                return this.buildReports;
            }
            set
            {
                this.buildReports = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool BuildReportsSpecified
        {
            get
            {
                return this.buildReportsSpecified;
            }
            set
            {
                this.buildReportsSpecified = value;
            }
        }

        /// <remarks/>
        public bool Inactive
        {
            get
            {
                return this.inactive;
            }
            set
            {
                this.inactive = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool InactiveSpecified
        {
            get
            {
                return this.inactiveSpecified;
            }
            set
            {
                this.inactiveSpecified = value;
            }
        }

        /// <remarks/>
        public CustomDefaultValueType DefaultValue
        {
            get
            {
                return this.defaultValue;
            }
            set
            {
                this.defaultValue = value;
            }
        }

        /// <remarks/>
        public bool DateTimeFormat
        {
            get
            {
                return this.dateTimeFormat;
            }
            set
            {
                this.dateTimeFormat = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DateTimeFormatSpecified
        {
            get
            {
                return this.dateTimeFormatSpecified;
            }
            set
            {
                this.dateTimeFormatSpecified = value;
            }
        }

        /// <remarks/>
        public long MaximumLength
        {
            get
            {
                return this.maximumLength;
            }
            set
            {
                this.maximumLength = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MaximumLengthSpecified
        {
            get
            {
                return this.maximumLengthSpecified;
            }
            set
            {
                this.maximumLengthSpecified = value;
            }
        }

        /// <remarks/>
        public sbyte Precision
        {
            get
            {
                return this.precision;
            }
            set
            {
                this.precision = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool PrecisionSpecified
        {
            get
            {
                return this.precisionSpecified;
            }
            set
            {
                this.precisionSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("PickItem", IsNullable = false)]
        public string[] PickItems
        {
            get
            {
                return this.pickItems;
            }
            set
            {
                this.pickItems = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("LookupItem", IsNullable = false)]
        public EntityDesc[] LookupItems
        {
            get
            {
                return this.lookupItems;
            }
            set
            {
                this.lookupItems = value;
            }
        }

        /// <remarks/>
        public bool IsMultiline
        {
            get
            {
                return this.isMultiline;
            }
            set
            {
                this.isMultiline = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsMultilineSpecified
        {
            get
            {
                return this.isMultilineSpecified;
            }
            set
            {
                this.isMultilineSpecified = value;
            }
        }

        /// <remarks/>
        public bool IsCalculated
        {
            get
            {
                return this.isCalculated;
            }
            set
            {
                this.isCalculated = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsCalculatedSpecified
        {
            get
            {
                return this.isCalculatedSpecified;
            }
            set
            {
                this.isCalculatedSpecified = value;
            }
        }

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.code;
            }
            set
            {
                this.code = value;
            }
        }

        /// <remarks/>
        public bool DateSystemTime
        {
            get
            {
                return this.dateSystemTime;
            }
            set
            {
                this.dateSystemTime = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DateSystemTimeSpecified
        {
            get
            {
                return this.dateSystemTimeSpecified;
            }
            set
            {
                this.dateSystemTimeSpecified = value;
            }
        }

        /// <remarks/>
        public bool IsReadOnly
        {
            get
            {
                return this.isReadOnly;
            }
            set
            {
                this.isReadOnly = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsReadOnlySpecified
        {
            get
            {
                return this.isReadOnlySpecified;
            }
            set
            {
                this.isReadOnlySpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum CustomDefType
    {

        /// <remarks/>
        Text,

        /// <remarks/>
        Integer,

        /// <remarks/>
        Decimal,

        /// <remarks/>
        PickList,

        /// <remarks/>
        Logical,

        /// <remarks/>
        Date,

        /// <remarks/>
        Money,

        /// <remarks/>
        Lookup,

        /// <remarks/>
        LookupAddress,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class CustomDefaultValueType
    {

        private CustomDefinitionType customDefinition;

        private string value;

        private CustomObjectType relatedObject;

        /// <remarks/>
        public CustomDefinitionType CustomDefinition
        {
            get
            {
                return this.customDefinition;
            }
            set
            {
                this.customDefinition = value;
            }
        }

        /// <remarks/>
        public string Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }

        /// <remarks/>
        public CustomObjectType RelatedObject
        {
            get
            {
                return this.relatedObject;
            }
            set
            {
                this.relatedObject = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class CustomObjectType
    {

        private object item;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Entity", typeof(Entity))]
        [System.Xml.Serialization.XmlElementAttribute("EntityAddress", typeof(EntityAddress))]
        public object Item
        {
            get
            {
                return this.item;
            }
            set
            {
                this.item = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("Entity", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class Entity
    {

        private EntityDesc type;

        private string name;

        private string entityID;

        private System.DateTime createdOn;

        private bool createdOnSpecified;

        private Address address;

        private Address billingAddress;

        private string email;

        private string website;

        private string phone;

        private string phoneExtension;

        private string fax;

        private string accountNumber;

        private string contactFirstName;

        private string contactLastName;

        private string exporterID;

        private ExporterIDTypeDesc exporterIDType;

        private bool exporterIDTypeSpecified;

        private long networkID;

        private bool networkIDSpecified;

        private string notes;

        private MoneyValue creditLimit;

        private MoneyValue balance;

        private string parentName;

        private string transactionDueDays;

        private PaymentTerm paymentTerms;

        private bool isKnownShipper;

        private bool isKnownShipperSpecified;

        private System.DateTime knownShipperExpirationDate;

        private bool knownShipperExpirationDateSpecified;

        private Agent agentInfo;

        private Carrier carrierInfo;

        private DivisionType divisionInfo;

        private Address[] otherAddresses;

        private System.DateTime dateOfBirth;

        private bool dateOfBirthSpecified;

        private Country countryOfCitizenship;

        private Entity division;

        private bool isInactive;

        private bool isInactiveSpecified;

        private IncotermType incoterm;

        private DeniedPartyScreeningResultType deniedPartyScreeningResultData;

        private bool isPrepaid;

        private bool isPrepaidSpecified;

        private bool is1099Eligible;

        private bool is1099EligibleSpecified;

        private string mobilePhone;

        private CustomType[] customs;

        private string gUID;

        /// <remarks/>
        public EntityDesc Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        /// <remarks/>
        public string EntityID
        {
            get
            {
                return this.entityID;
            }
            set
            {
                this.entityID = value;
            }
        }

        /// <remarks/>
        public System.DateTime CreatedOn
        {
            get
            {
                return this.createdOn;
            }
            set
            {
                this.createdOn = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CreatedOnSpecified
        {
            get
            {
                return this.createdOnSpecified;
            }
            set
            {
                this.createdOnSpecified = value;
            }
        }

        /// <remarks/>
        public Address Address
        {
            get
            {
                return this.address;
            }
            set
            {
                this.address = value;
            }
        }

        /// <remarks/>
        public Address BillingAddress
        {
            get
            {
                return this.billingAddress;
            }
            set
            {
                this.billingAddress = value;
            }
        }

        /// <remarks/>
        public string Email
        {
            get
            {
                return this.email;
            }
            set
            {
                this.email = value;
            }
        }

        /// <remarks/>
        public string Website
        {
            get
            {
                return this.website;
            }
            set
            {
                this.website = value;
            }
        }

        /// <remarks/>
        public string Phone
        {
            get
            {
                return this.phone;
            }
            set
            {
                this.phone = value;
            }
        }

        /// <remarks/>
        public string PhoneExtension
        {
            get
            {
                return this.phoneExtension;
            }
            set
            {
                this.phoneExtension = value;
            }
        }

        /// <remarks/>
        public string Fax
        {
            get
            {
                return this.fax;
            }
            set
            {
                this.fax = value;
            }
        }

        /// <remarks/>
        public string AccountNumber
        {
            get
            {
                return this.accountNumber;
            }
            set
            {
                this.accountNumber = value;
            }
        }

        /// <remarks/>
        public string ContactFirstName
        {
            get
            {
                return this.contactFirstName;
            }
            set
            {
                this.contactFirstName = value;
            }
        }

        /// <remarks/>
        public string ContactLastName
        {
            get
            {
                return this.contactLastName;
            }
            set
            {
                this.contactLastName = value;
            }
        }

        /// <remarks/>
        public string ExporterID
        {
            get
            {
                return this.exporterID;
            }
            set
            {
                this.exporterID = value;
            }
        }

        /// <remarks/>
        public ExporterIDTypeDesc ExporterIDType
        {
            get
            {
                return this.exporterIDType;
            }
            set
            {
                this.exporterIDType = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ExporterIDTypeSpecified
        {
            get
            {
                return this.exporterIDTypeSpecified;
            }
            set
            {
                this.exporterIDTypeSpecified = value;
            }
        }

        /// <remarks/>
        public long NetworkID
        {
            get
            {
                return this.networkID;
            }
            set
            {
                this.networkID = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool NetworkIDSpecified
        {
            get
            {
                return this.networkIDSpecified;
            }
            set
            {
                this.networkIDSpecified = value;
            }
        }

        /// <remarks/>
        public string Notes
        {
            get
            {
                return this.notes;
            }
            set
            {
                this.notes = value;
            }
        }

        /// <remarks/>
        public MoneyValue CreditLimit
        {
            get
            {
                return this.creditLimit;
            }
            set
            {
                this.creditLimit = value;
            }
        }

        /// <remarks/>
        public MoneyValue Balance
        {
            get
            {
                return this.balance;
            }
            set
            {
                this.balance = value;
            }
        }

        /// <remarks/>
        public string ParentName
        {
            get
            {
                return this.parentName;
            }
            set
            {
                this.parentName = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string TransactionDueDays
        {
            get
            {
                return this.transactionDueDays;
            }
            set
            {
                this.transactionDueDays = value;
            }
        }

        /// <remarks/>
        public PaymentTerm PaymentTerms
        {
            get
            {
                return this.paymentTerms;
            }
            set
            {
                this.paymentTerms = value;
            }
        }

        /// <remarks/>
        public bool IsKnownShipper
        {
            get
            {
                return this.isKnownShipper;
            }
            set
            {
                this.isKnownShipper = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsKnownShipperSpecified
        {
            get
            {
                return this.isKnownShipperSpecified;
            }
            set
            {
                this.isKnownShipperSpecified = value;
            }
        }

        /// <remarks/>
        public System.DateTime KnownShipperExpirationDate
        {
            get
            {
                return this.knownShipperExpirationDate;
            }
            set
            {
                this.knownShipperExpirationDate = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool KnownShipperExpirationDateSpecified
        {
            get
            {
                return this.knownShipperExpirationDateSpecified;
            }
            set
            {
                this.knownShipperExpirationDateSpecified = value;
            }
        }

        /// <remarks/>
        public Agent AgentInfo
        {
            get
            {
                return this.agentInfo;
            }
            set
            {
                this.agentInfo = value;
            }
        }

        /// <remarks/>
        public Carrier CarrierInfo
        {
            get
            {
                return this.carrierInfo;
            }
            set
            {
                this.carrierInfo = value;
            }
        }

        /// <remarks/>
        public DivisionType DivisionInfo
        {
            get
            {
                return this.divisionInfo;
            }
            set
            {
                this.divisionInfo = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Address", IsNullable = false)]
        public Address[] OtherAddresses
        {
            get
            {
                return this.otherAddresses;
            }
            set
            {
                this.otherAddresses = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime DateOfBirth
        {
            get
            {
                return this.dateOfBirth;
            }
            set
            {
                this.dateOfBirth = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DateOfBirthSpecified
        {
            get
            {
                return this.dateOfBirthSpecified;
            }
            set
            {
                this.dateOfBirthSpecified = value;
            }
        }

        /// <remarks/>
        public Country CountryOfCitizenship
        {
            get
            {
                return this.countryOfCitizenship;
            }
            set
            {
                this.countryOfCitizenship = value;
            }
        }

        /// <remarks/>
        public Entity Division
        {
            get
            {
                return this.division;
            }
            set
            {
                this.division = value;
            }
        }

        /// <remarks/>
        public bool IsInactive
        {
            get
            {
                return this.isInactive;
            }
            set
            {
                this.isInactive = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsInactiveSpecified
        {
            get
            {
                return this.isInactiveSpecified;
            }
            set
            {
                this.isInactiveSpecified = value;
            }
        }

        /// <remarks/>
        public IncotermType Incoterm
        {
            get
            {
                return this.incoterm;
            }
            set
            {
                this.incoterm = value;
            }
        }

        /// <remarks/>
        public DeniedPartyScreeningResultType DeniedPartyScreeningResultData
        {
            get
            {
                return this.deniedPartyScreeningResultData;
            }
            set
            {
                this.deniedPartyScreeningResultData = value;
            }
        }

        /// <remarks/>
        public bool IsPrepaid
        {
            get
            {
                return this.isPrepaid;
            }
            set
            {
                this.isPrepaid = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsPrepaidSpecified
        {
            get
            {
                return this.isPrepaidSpecified;
            }
            set
            {
                this.isPrepaidSpecified = value;
            }
        }

        /// <remarks/>
        public bool Is1099Eligible
        {
            get
            {
                return this.is1099Eligible;
            }
            set
            {
                this.is1099Eligible = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Is1099EligibleSpecified
        {
            get
            {
                return this.is1099EligibleSpecified;
            }
            set
            {
                this.is1099EligibleSpecified = value;
            }
        }

        /// <remarks/>
        public string MobilePhone
        {
            get
            {
                return this.mobilePhone;
            }
            set
            {
                this.mobilePhone = value;
            }
        }

        /// <remarks/>
        //[System.Xml.Serialization.XmlArrayItemAttribute("Custom", IsNullable = false)]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public CustomType[] Customs
        {
            get
            {
                return this.customs;
            }
            set
            {
                this.customs = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string GUID
        {
            get
            {
                return this.gUID;
            }
            set
            {
                this.gUID = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum EntityDesc
    {

        /// <remarks/>
        Client,

        /// <remarks/>
        WarehouseProvider,

        /// <remarks/>
        ForwardingAgent,

        /// <remarks/>
        Carrier,

        /// <remarks/>
        Vendor,

        /// <remarks/>
        Employee,

        /// <remarks/>
        Salesperson,

        /// <remarks/>
        Division,

        /// <remarks/>
        EntityContact,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class Address
    {

        private string[] street;

        private string city;

        private string state;

        private string zipCode;

        private Country country;

        private string description;

        private string contactName;

        private string contactPhone;

        private string contactPhoneExtension;

        private string contactFax;

        private string contactEmail;

        private string portCode;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Street")]
        public string[] Street
        {
            get
            {
                return this.street;
            }
            set
            {
                this.street = value;
            }
        }

        /// <remarks/>
        public string City
        {
            get
            {
                return this.city;
            }
            set
            {
                this.city = value;
            }
        }

        /// <remarks/>
        public string State
        {
            get
            {
                return this.state;
            }
            set
            {
                this.state = value;
            }
        }

        /// <remarks/>
        public string ZipCode
        {
            get
            {
                return this.zipCode;
            }
            set
            {
                this.zipCode = value;
            }
        }

        /// <remarks/>
        public Country Country
        {
            get
            {
                return this.country;
            }
            set
            {
                this.country = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        /// <remarks/>
        public string ContactName
        {
            get
            {
                return this.contactName;
            }
            set
            {
                this.contactName = value;
            }
        }

        /// <remarks/>
        public string ContactPhone
        {
            get
            {
                return this.contactPhone;
            }
            set
            {
                this.contactPhone = value;
            }
        }

        /// <remarks/>
        public string ContactPhoneExtension
        {
            get
            {
                return this.contactPhoneExtension;
            }
            set
            {
                this.contactPhoneExtension = value;
            }
        }

        /// <remarks/>
        public string ContactFax
        {
            get
            {
                return this.contactFax;
            }
            set
            {
                this.contactFax = value;
            }
        }

        /// <remarks/>
        public string ContactEmail
        {
            get
            {
                return this.contactEmail;
            }
            set
            {
                this.contactEmail = value;
            }
        }

        /// <remarks/>
        public string PortCode
        {
            get
            {
                return this.portCode;
            }
            set
            {
                this.portCode = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class Country
    {

        private string code;

        private string value;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Code
        {
            get
            {
                return this.code;
            }
            set
            {
                this.code = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum ExporterIDTypeDesc
    {

        /// <remarks/>
        Other,

        /// <remarks/>
        EIN,

        /// <remarks/>
        SSN,

        /// <remarks/>
        DUNS,

        /// <remarks/>
        Foreign,

        /// <remarks/>
        CNPJ,

        /// <remarks/>
        License,

        /// <remarks/>
        Passport,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class MoneyValue
    {

        private string currency;

        private double value;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Currency
        {
            get
            {
                return this.currency;
            }
            set
            {
                this.currency = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public double Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class PaymentTerm
    {

        private string description;

        private string netDueDays;

        private double discountPercentage;

        private bool discountPercentageSpecified;

        private string discountPaidDays;

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string NetDueDays
        {
            get
            {
                return this.netDueDays;
            }
            set
            {
                this.netDueDays = value;
            }
        }

        /// <remarks/>
        public double DiscountPercentage
        {
            get
            {
                return this.discountPercentage;
            }
            set
            {
                this.discountPercentage = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DiscountPercentageSpecified
        {
            get
            {
                return this.discountPercentageSpecified;
            }
            set
            {
                this.discountPercentageSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string DiscountPaidDays
        {
            get
            {
                return this.discountPaidDays;
            }
            set
            {
                this.discountPaidDays = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class Agent
    {

        private string iATAAccountNumber;

        private string fMCNumber;

        private string sCACNumber;

        private string tSANumber;

        /// <remarks/>
        public string IATAAccountNumber
        {
            get
            {
                return this.iATAAccountNumber;
            }
            set
            {
                this.iATAAccountNumber = value;
            }
        }

        /// <remarks/>
        public string FMCNumber
        {
            get
            {
                return this.fMCNumber;
            }
            set
            {
                this.fMCNumber = value;
            }
        }

        /// <remarks/>
        public string SCACNumber
        {
            get
            {
                return this.sCACNumber;
            }
            set
            {
                this.sCACNumber = value;
            }
        }

        /// <remarks/>
        public string TSANumber
        {
            get
            {
                return this.tSANumber;
            }
            set
            {
                this.tSANumber = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class Carrier
    {

        private MethodType Code; 

        private string iATAAccountNumber;

        private string airlineCode;

        private string airlineCodeNumber;

        private string fMCNumber;

        private string sCACNumber;

        /// <remarks/>
        public MethodType CarrierCode
        {
            get
            {
                return this.Code;
            }
            set
            {
                this.Code = value;
            }
        }

        /// <remarks/>
        public string IATAAccountNumber
        {
            get
            {
                return this.iATAAccountNumber;
            }
            set
            {
                this.iATAAccountNumber = value;
            }
        }

        /// <remarks/>
        public string AirlineCode
        {
            get
            {
                return this.airlineCode;
            }
            set
            {
                this.airlineCode = value;
            }
        }

        /// <remarks/>
        public string AirlineCodeNumber
        {
            get
            {
                return this.airlineCodeNumber;
            }
            set
            {
                this.airlineCodeNumber = value;
            }
        }

        /// <remarks/>
        public string FMCNumber
        {
            get
            {
                return this.fMCNumber;
            }
            set
            {
                this.fMCNumber = value;
            }
        }

        /// <remarks/>
        public string SCACNumber
        {
            get
            {
                return this.sCACNumber;
            }
            set
            {
                this.sCACNumber = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class DivisionType
    {

        private bool useInHeaders;

        private bool useInHeadersSpecified;

        /// <remarks/>
        public bool UseInHeaders
        {
            get
            {
                return this.useInHeaders;
            }
            set
            {
                this.useInHeaders = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool UseInHeadersSpecified
        {
            get
            {
                return this.useInHeadersSpecified;
            }
            set
            {
                this.useInHeadersSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class IncotermType
    {

        private string code;

        private string description;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.code;
            }
            set
            {
                this.code = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class DeniedPartyScreeningResultType
    {

        private string screeningStatus;

        private System.DateTime screeningTime;

        /// <remarks/>
        public string ScreeningStatus
        {
            get
            {
                return this.screeningStatus;
            }
            set
            {
                this.screeningStatus = value;
            }
        }

        /// <remarks/>
        public System.DateTime ScreeningTime
        {
            get
            {
                return this.screeningTime;
            }
            set
            {
                this.screeningTime = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class EntityAddress
    {

        private Entity entity;

        private Address address;

        /// <remarks/>
        public Entity Entity
        {
            get
            {
                return this.entity;
            }
            set
            {
                this.entity = value;
            }
        }

        /// <remarks/>
        public Address Address
        {
            get
            {
                return this.address;
            }
            set
            {
                this.address = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("Item", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class Item
    {

        private sbyte version;

        private bool versionSpecified;

        //item status : Hold status
        private ItemStatusType status;

        private bool statusSpecified;

        //Package Type Field : Box or Bag
        private PackageType package;

        private string packageName;

        //Piece field : piece = 1
        private string pieces;
        //(LxWxH)
        private LenghtValue length;
        //(LxWxH)
        private LenghtValue height;
        //(LxWxH)
        private LenghtValue width;
        //unit weight
        private WeightValue pieceWeight;
        //unit volume
        private VolumeValue pieceVolume;
        
        private bool containedPiecesWeightIncluded;

        private bool containedPiecesWeightIncludedSpecified;
        //piece x pieceWeight
        private WeightValue weight;
        //piece x pieceVolume
        private VolumeValue volume;
        //unit and value to be collected
        private VolumeWeightValue volumeWeight;

        private double pieceQuantity;

        private bool pieceQuantitySpecified;
        //input quantity
        private string quantityUnit;

        private CurrencyType currency;

        private MoneyValue unitaryValue;
        //unitaryvalue x quantityunit
        private MoneyValue totalValue;

        private bool isSummarized;

        private bool isSummarizedSpecified;

        private string[] serialNumbers;

        private string serialNumber;

        private string model;

        private string description;

        private string partNumber;

        private string lotNumber;

        private string supplierInvoiceNumber;

        private string supplierPONumber;

        private ItemDefinition itemDefinition;

        private Item[] containedItems;

        private string upItemGUID;

        private string upItemSerialNumber;

        private string upItemPartNumber;

        private string upItemLotNumber;

        private string parentItemGUID;

        private string warehouseReceiptGUID;

        private string purchaseOrderGUID;

        private string salesOrderGUID;

        private string jobGUID;

        private string wHRItemID;

        private string trackingNumber;

        private TrackingNumberInfoType[] externalTrackingNumbers;

        private string palletID;

        private string locationCode;

        private LocationType location;

        private LocationType previousLocation;

        private AESDataType aESData;

        private bool includeInSED;

        private bool includeInSEDSpecified;

        private AMSDataType aMSData;

        private HazardousType hazardous;

        private string notes;

        private string waybillNotes;

        private AttachmentType[] attachments;

        private string pickupOrderGUID;

        private string outShipmentGUID;

        private string inShipmentGUID;

        private string cargoReleaseGUID;

        private string outMasterWayBillNumber;

        private string outHouseWayBillNumber;

        private string inMasterWayBillNumber;

        private string inHouseWayBillNumber;

        private string warehouseReceiptNumber;

        private string pickupOrderNumber;

        private string cargoReleaseNumber;

        private string salesOrderNumber;

        private string jobNumber;

        private System.DateTime arrivalDate;

        private bool arrivalDateSpecified;

        private System.DateTime outDate;

        private bool outDateSpecified;

        private long lastReceptionNetworkID;

        private bool lastReceptionNetworkIDSpecified;

        //private MethodType shipmentMethod; 

        private bool shipmentSpecified;

        private bool isContainer;

        private bool isContainerSpecified;

        private ContainerType containerInfo;

        private string orderIndex;

        private bool isPallet;

        private bool isPalletSpecified;

        private bool isOverstock;

        private bool isOverstockSpecified;

        private bool notLoaded;

        private bool notLoadedSpecified;

        private bool inTask;

        private bool inTaskSpecified;

        private string warehouseReceiptURL;

        private string pickupOrderURL;

        private string outShipmentURL;

        private string inShipmentURL;

        private string cargoReleaseURL;

        private string salesOrderURL;

        private string jobURL;

        private string purchaseOrderURL;

        private Charge purchaseCharge;

        private Charge salesCharge;

        private string commodityTypeName;

        private Entity manufacturer;

        private string pOLineID;

        private string sOLineID;

        private bool isCancel;

        private bool isCancelSpecified;

        private bool isKitItem;

        private bool isKitItemSpecified;

        private bool isInKit;

        private bool isInKitSpecified;

        private bool showKitItems;

        private bool showKitItemsSpecified;

        private HoldStatusType holdStatus;

        private System.DateTime expirationDate;

        private bool expirationDateSpecified;

        private System.DateTime requestedDate;

        private bool requestedDateSpecified;

        private System.DateTime promisedDate;

        private bool promisedDateSpecified;

        private string nCMCode;

        private System.DateTime entryDate;

        private bool entryDateSpecified;

        private VGMDataType vGMData;

        private CustomType[] customs;

        private string gUID;

        private string type;

        /// <remarks/>
        public sbyte Version
        {
            get
            {
                return this.version;
            }
            set
            {
                this.version = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool VersionSpecified
        {
            get
            {
                return this.versionSpecified;
            }
            set
            {
                this.versionSpecified = value;
            }
        }

        /// <remarks/>
        public ItemStatusType Status
        {
            get
            {
                return this.status;
            }
            set
            {
                this.status = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool StatusSpecified
        {
            get
            {
                return this.statusSpecified;
            }
            set
            {
                this.statusSpecified = value;
            }
        }

        /// <remarks/>
        public PackageType Package
        {
            get
            {
                return this.package;
            }
            set
            {
                this.package = value;
            }
        }

        /// <remarks/>
        public string PackageName
        {
            get
            {
                return this.packageName;
            }
            set
            {
                this.packageName = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "nonNegativeInteger")]
        public string Pieces
        {
            get
            {
                return this.pieces;
            }
            set
            {
                this.pieces = value;
            }
        }

        /// <remarks/>
        public LenghtValue Length
        {
            get
            {
                return this.length;
            }
            set
            {
                this.length = value;
            }
        }

        /// <remarks/>
        public LenghtValue Height
        {
            get
            {
                return this.height;
            }
            set
            {
                this.height = value;
            }
        }

        /// <remarks/>
        public LenghtValue Width
        {
            get
            {
                return this.width;
            }
            set
            {
                this.width = value;
            }
        }

        /// <remarks/>
        public WeightValue PieceWeight
        {
            get
            {
                return this.pieceWeight;
            }
            set
            {
                this.pieceWeight = value;
            }
        }

        /// <remarks/>
        public VolumeValue PieceVolume
        {
            get
            {
                return this.pieceVolume;
            }
            set
            {
                this.pieceVolume = value;
            }
        }

        /// <remarks/>
        public bool ContainedPiecesWeightIncluded
        {
            get
            {
                return this.containedPiecesWeightIncluded;
            }
            set
            {
                this.containedPiecesWeightIncluded = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ContainedPiecesWeightIncludedSpecified
        {
            get
            {
                return this.containedPiecesWeightIncludedSpecified;
            }
            set
            {
                this.containedPiecesWeightIncludedSpecified = value;
            }
        }

        /// <remarks/>
        public WeightValue Weight
        {
            get
            {
                return this.weight;
            }
            set
            {
                this.weight = value;
            }
        }

        /// <remarks/>
        public VolumeValue Volume
        {
            get
            {
                return this.volume;
            }
            set
            {
                this.volume = value;
            }
        }

        /// <remarks/>
        public VolumeWeightValue VolumeWeight
        {
            get
            {
                return this.volumeWeight;
            }
            set
            {
                this.volumeWeight = value;
            }
        }

        /// <remarks/>
        public double PieceQuantity
        {
            get
            {
                return this.pieceQuantity;
            }
            set
            {
                this.pieceQuantity = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool PieceQuantitySpecified
        {
            get
            {
                return this.pieceQuantitySpecified;
            }
            set
            {
                this.pieceQuantitySpecified = value;
            }
        }

        /// <remarks/>
        public string QuantityUnit
        {
            get
            {
                return this.quantityUnit;
            }
            set
            {
                this.quantityUnit = value;
            }
        }

        /// <remarks/>
        public CurrencyType Currency
        {
            get
            {
                return this.currency;
            }
            set
            {
                this.currency = value;
            }
        }

        /// <remarks/>
        public MoneyValue UnitaryValue
        {
            get
            {
                return this.unitaryValue;
            }
            set
            {
                this.unitaryValue = value;
            }
        }

        /// <remarks/>
        public MoneyValue TotalValue
        {
            get
            {
                return this.totalValue;
            }
            set
            {
                this.totalValue = value;
            }
        }

        /// <remarks/>
        public bool IsSummarized
        {
            get
            {
                return this.isSummarized;
            }
            set
            {
                this.isSummarized = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsSummarizedSpecified
        {
            get
            {
                return this.isSummarizedSpecified;
            }
            set
            {
                this.isSummarizedSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("SerialNumber", IsNullable = false)]
        public string[] SerialNumbers
        {
            get
            {
                return this.serialNumbers;
            }
            set
            {
                this.serialNumbers = value;
            }
        }

        /// <remarks/>
        public string SerialNumber
        {
            get
            {
                return this.serialNumber;
            }
            set
            {
                this.serialNumber = value;
            }
        }

        /// <remarks/>
        public string Model
        {
            get
            {
                return this.model;
            }
            set
            {
                this.model = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        /// <remarks/>
        public string PartNumber
        {
            get
            {
                return this.partNumber;
            }
            set
            {
                this.partNumber = value;
            }
        }

        /// <remarks/>
        public string LotNumber
        {
            get
            {
                return this.lotNumber;
            }
            set
            {
                this.lotNumber = value;
            }
        }

        /// <remarks/>
        public string SupplierInvoiceNumber
        {
            get
            {
                return this.supplierInvoiceNumber;
            }
            set
            {
                this.supplierInvoiceNumber = value;
            }
        }

        /// <remarks/>
        public string SupplierPONumber
        {
            get
            {
                return this.supplierPONumber;
            }
            set
            {
                this.supplierPONumber = value;
            }
        }

        /// <remarks/>
        public ItemDefinition ItemDefinition
        {
            get
            {
                return this.itemDefinition;
            }
            set
            {
                this.itemDefinition = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Item", IsNullable = false)]
        public Item[] ContainedItems
        {
            get
            {
                return this.containedItems;
            }
            set
            {
                this.containedItems = value;
            }
        }

        /// <remarks/>
        public string UpItemGUID
        {
            get
            {
                return this.upItemGUID;
            }
            set
            {
                this.upItemGUID = value;
            }
        }

        /// <remarks/>
        public string UpItemSerialNumber
        {
            get
            {
                return this.upItemSerialNumber;
            }
            set
            {
                this.upItemSerialNumber = value;
            }
        }

        /// <remarks/>
        public string UpItemPartNumber
        {
            get
            {
                return this.upItemPartNumber;
            }
            set
            {
                this.upItemPartNumber = value;
            }
        }

        /// <remarks/>
        public string UpItemLotNumber
        {
            get
            {
                return this.upItemLotNumber;
            }
            set
            {
                this.upItemLotNumber = value;
            }
        }

        /// <remarks/>
        public string ParentItemGUID
        {
            get
            {
                return this.parentItemGUID;
            }
            set
            {
                this.parentItemGUID = value;
            }
        }

        /// <remarks/>
        public string WarehouseReceiptGUID
        {
            get
            {
                return this.warehouseReceiptGUID;
            }
            set
            {
                this.warehouseReceiptGUID = value;
            }
        }

        /// <remarks/>
        public string PurchaseOrderGUID
        {
            get
            {
                return this.purchaseOrderGUID;
            }
            set
            {
                this.purchaseOrderGUID = value;
            }
        }

        /// <remarks/>
        public string SalesOrderGUID
        {
            get
            {
                return this.salesOrderGUID;
            }
            set
            {
                this.salesOrderGUID = value;
            }
        }

        /// <remarks/>
        public string JobGUID
        {
            get
            {
                return this.jobGUID;
            }
            set
            {
                this.jobGUID = value;
            }
        }

        /// <remarks/>
        public string WHRItemID
        {
            get
            {
                return this.wHRItemID;
            }
            set
            {
                this.wHRItemID = value;
            }
        }

        /// <remarks/>
        public string TrackingNumber
        {
            get
            {
                return this.trackingNumber;
            }
            set
            {
                this.trackingNumber = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("TrackingNumberInfo", IsNullable = false)]
        public TrackingNumberInfoType[] ExternalTrackingNumbers
        {
            get
            {
                return this.externalTrackingNumbers;
            }
            set
            {
                this.externalTrackingNumbers = value;
            }
        }

        /// <remarks/>
        public string PalletID
        {
            get
            {
                return this.palletID;
            }
            set
            {
                this.palletID = value;
            }
        }

        /// <remarks/>
        public string LocationCode
        {
            get
            {
                return this.locationCode;
            }
            set
            {
                this.locationCode = value;
            }
        }

        /// <remarks/>
        public LocationType Location
        {
            get
            {
                return this.location;
            }
            set
            {
                this.location = value;
            }
        }

        /// <remarks/>
        public LocationType PreviousLocation
        {
            get
            {
                return this.previousLocation;
            }
            set
            {
                this.previousLocation = value;
            }
        }

        /// <remarks/>
        public AESDataType AESData
        {
            get
            {
                return this.aESData;
            }
            set
            {
                this.aESData = value;
            }
        }

        /// <remarks/>
        public bool IncludeInSED
        {
            get
            {
                return this.includeInSED;
            }
            set
            {
                this.includeInSED = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IncludeInSEDSpecified
        {
            get
            {
                return this.includeInSEDSpecified;
            }
            set
            {
                this.includeInSEDSpecified = value;
            }
        }

        /// <remarks/>
        public AMSDataType AMSData
        {
            get
            {
                return this.aMSData;
            }
            set
            {
                this.aMSData = value;
            }
        }

        /// <remarks/>
        public HazardousType Hazardous
        {
            get
            {
                return this.hazardous;
            }
            set
            {
                this.hazardous = value;
            }
        }

        /// <remarks/>
        public string Notes
        {
            get
            {
                return this.notes;
            }
            set
            {
                this.notes = value;
            }
        }

        /// <remarks/>
        public string WaybillNotes
        {
            get
            {
                return this.waybillNotes;
            }
            set
            {
                this.waybillNotes = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Attachment", IsNullable = false)]
        public AttachmentType[] Attachments
        {
            get
            {
                return this.attachments;
            }
            set
            {
                this.attachments = value;
            }
        }

        /// <remarks/>
        public string PickupOrderGUID
        {
            get
            {
                return this.pickupOrderGUID;
            }
            set
            {
                this.pickupOrderGUID = value;
            }
        }

        /// <remarks/>
        public string OutShipmentGUID
        {
            get
            {
                return this.outShipmentGUID;
            }
            set
            {
                this.outShipmentGUID = value;
            }
        }

        /// <remarks/>
        public string InShipmentGUID
        {
            get
            {
                return this.inShipmentGUID;
            }
            set
            {
                this.inShipmentGUID = value;
            }
        }

        /// <remarks/>
        public string CargoReleaseGUID
        {
            get
            {
                return this.cargoReleaseGUID;
            }
            set
            {
                this.cargoReleaseGUID = value;
            }
        }

        /// <remarks/>
        public string OutMasterWayBillNumber
        {
            get
            {
                return this.outMasterWayBillNumber;
            }
            set
            {
                this.outMasterWayBillNumber = value;
            }
        }

        /// <remarks/>
        public string OutHouseWayBillNumber
        {
            get
            {
                return this.outHouseWayBillNumber;
            }
            set
            {
                this.outHouseWayBillNumber = value;
            }
        }

        /// <remarks/>
        public string InMasterWayBillNumber
        {
            get
            {
                return this.inMasterWayBillNumber;
            }
            set
            {
                this.inMasterWayBillNumber = value;
            }
        }

        /// <remarks/>
        public string InHouseWayBillNumber
        {
            get
            {
                return this.inHouseWayBillNumber;
            }
            set
            {
                this.inHouseWayBillNumber = value;
            }
        }

        /// <remarks/>
        public string WarehouseReceiptNumber
        {
            get
            {
                return this.warehouseReceiptNumber;
            }
            set
            {
                this.warehouseReceiptNumber = value;
            }
        }

        /// <remarks/>
        public string PickupOrderNumber
        {
            get
            {
                return this.pickupOrderNumber;
            }
            set
            {
                this.pickupOrderNumber = value;
            }
        }

        /// <remarks/>
        public string CargoReleaseNumber
        {
            get
            {
                return this.cargoReleaseNumber;
            }
            set
            {
                this.cargoReleaseNumber = value;
            }
        }

        /// <remarks/>
        public string SalesOrderNumber
        {
            get
            {
                return this.salesOrderNumber;
            }
            set
            {
                this.salesOrderNumber = value;
            }
        }

        /// <remarks/>
        public string JobNumber
        {
            get
            {
                return this.jobNumber;
            }
            set
            {
                this.jobNumber = value;
            }
        }

        /// <remarks/>
        public System.DateTime ArrivalDate
        {
            get
            {
                return this.arrivalDate;
            }
            set
            {
                this.arrivalDate = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ArrivalDateSpecified
        {
            get
            {
                return this.arrivalDateSpecified;
            }
            set
            {
                this.arrivalDateSpecified = value;
            }
        }

        /// <remarks/>
        public System.DateTime OutDate
        {
            get
            {
                return this.outDate;
            }
            set
            {
                this.outDate = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool OutDateSpecified
        {
            get
            {
                return this.outDateSpecified;
            }
            set
            {
                this.outDateSpecified = value;
            }
        }

        /// <remarks/>
        public long LastReceptionNetworkID
        {
            get
            {
                return this.lastReceptionNetworkID;
            }
            set
            {
                this.lastReceptionNetworkID = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool LastReceptionNetworkIDSpecified
        {
            get
            {
                return this.lastReceptionNetworkIDSpecified;
            }
            set
            {
                this.lastReceptionNetworkIDSpecified = value;
            }
        }

        /// <remarks/>
        //public MethodType ShipmentMethod
        //{
        //    get
        //    {
        //        return this.shipmentMethod;
        //    }
        //    set
        //    {
        //        this.shipmentMethod = value;
        //    }
        //}

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ShipmentSpecified
        {
            get
            {
                return this.shipmentSpecified;
            }
            set
            {
                this.shipmentSpecified = value;
            }
        }

        /// <remarks/>
        public bool IsContainer
        {
            get
            {
                return this.isContainer;
            }
            set
            {
                this.isContainer = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsContainerSpecified
        {
            get
            {
                return this.isContainerSpecified;
            }
            set
            {
                this.isContainerSpecified = value;
            }
        }

        /// <remarks/>
        public ContainerType ContainerInfo
        {
            get
            {
                return this.containerInfo;
            }
            set
            {
                this.containerInfo = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string OrderIndex
        {
            get
            {
                return this.orderIndex;
            }
            set
            {
                this.orderIndex = value;
            }
        }

        /// <remarks/>
        public bool IsPallet
        {
            get
            {
                return this.isPallet;
            }
            set
            {
                this.isPallet = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsPalletSpecified
        {
            get
            {
                return this.isPalletSpecified;
            }
            set
            {
                this.isPalletSpecified = value;
            }
        }

        /// <remarks/>
        public bool IsOverstock
        {
            get
            {
                return this.isOverstock;
            }
            set
            {
                this.isOverstock = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsOverstockSpecified
        {
            get
            {
                return this.isOverstockSpecified;
            }
            set
            {
                this.isOverstockSpecified = value;
            }
        }

        /// <remarks/>
        public bool NotLoaded
        {
            get
            {
                return this.notLoaded;
            }
            set
            {
                this.notLoaded = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool NotLoadedSpecified
        {
            get
            {
                return this.notLoadedSpecified;
            }
            set
            {
                this.notLoadedSpecified = value;
            }
        }

        /// <remarks/>
        public bool InTask
        {
            get
            {
                return this.inTask;
            }
            set
            {
                this.inTask = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool InTaskSpecified
        {
            get
            {
                return this.inTaskSpecified;
            }
            set
            {
                this.inTaskSpecified = value;
            }
        }

        /// <remarks/>
        public string WarehouseReceiptURL
        {
            get
            {
                return this.warehouseReceiptURL;
            }
            set
            {
                this.warehouseReceiptURL = value;
            }
        }

        /// <remarks/>
        public string PickupOrderURL
        {
            get
            {
                return this.pickupOrderURL;
            }
            set
            {
                this.pickupOrderURL = value;
            }
        }

        /// <remarks/>
        public string OutShipmentURL
        {
            get
            {
                return this.outShipmentURL;
            }
            set
            {
                this.outShipmentURL = value;
            }
        }

        /// <remarks/>
        public string InShipmentURL
        {
            get
            {
                return this.inShipmentURL;
            }
            set
            {
                this.inShipmentURL = value;
            }
        }

        /// <remarks/>
        public string CargoReleaseURL
        {
            get
            {
                return this.cargoReleaseURL;
            }
            set
            {
                this.cargoReleaseURL = value;
            }
        }

        /// <remarks/>
        public string SalesOrderURL
        {
            get
            {
                return this.salesOrderURL;
            }
            set
            {
                this.salesOrderURL = value;
            }
        }

        /// <remarks/>
        public string JobURL
        {
            get
            {
                return this.jobURL;
            }
            set
            {
                this.jobURL = value;
            }
        }

        /// <remarks/>
        public string PurchaseOrderURL
        {
            get
            {
                return this.purchaseOrderURL;
            }
            set
            {
                this.purchaseOrderURL = value;
            }
        }

        /// <remarks/>
        public Charge PurchaseCharge
        {
            get
            {
                return this.purchaseCharge;
            }
            set
            {
                this.purchaseCharge = value;
            }
        }

        /// <remarks/>
        public Charge SalesCharge
        {
            get
            {
                return this.salesCharge;
            }
            set
            {
                this.salesCharge = value;
            }
        }

        /// <remarks/>
        public string CommodityTypeName
        {
            get
            {
                return this.commodityTypeName;
            }
            set
            {
                this.commodityTypeName = value;
            }
        }

        /// <remarks/>
        public Entity Manufacturer
        {
            get
            {
                return this.manufacturer;
            }
            set
            {
                this.manufacturer = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string POLineID
        {
            get
            {
                return this.pOLineID;
            }
            set
            {
                this.pOLineID = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string SOLineID
        {
            get
            {
                return this.sOLineID;
            }
            set
            {
                this.sOLineID = value;
            }
        }

        /// <remarks/>
        public bool IsCancel
        {
            get
            {
                return this.isCancel;
            }
            set
            {
                this.isCancel = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsCancelSpecified
        {
            get
            {
                return this.isCancelSpecified;
            }
            set
            {
                this.isCancelSpecified = value;
            }
        }

        /// <remarks/>
        public bool IsKitItem
        {
            get
            {
                return this.isKitItem;
            }
            set
            {
                this.isKitItem = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsKitItemSpecified
        {
            get
            {
                return this.isKitItemSpecified;
            }
            set
            {
                this.isKitItemSpecified = value;
            }
        }

        /// <remarks/>
        public bool IsInKit
        {
            get
            {
                return this.isInKit;
            }
            set
            {
                this.isInKit = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsInKitSpecified
        {
            get
            {
                return this.isInKitSpecified;
            }
            set
            {
                this.isInKitSpecified = value;
            }
        }

        /// <remarks/>
        public bool ShowKitItems
        {
            get
            {
                return this.showKitItems;
            }
            set
            {
                this.showKitItems = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ShowKitItemsSpecified
        {
            get
            {
                return this.showKitItemsSpecified;
            }
            set
            {
                this.showKitItemsSpecified = value;
            }
        }

        /// <remarks/>
        public HoldStatusType HoldStatus
        {
            get
            {
                return this.holdStatus;
            }
            set
            {
                this.holdStatus = value;
            }
        }

        /// <remarks/>
        public System.DateTime ExpirationDate
        {
            get
            {
                return this.expirationDate;
            }
            set
            {
                this.expirationDate = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ExpirationDateSpecified
        {
            get
            {
                return this.expirationDateSpecified;
            }
            set
            {
                this.expirationDateSpecified = value;
            }
        }

        /// <remarks/>
        public System.DateTime RequestedDate
        {
            get
            {
                return this.requestedDate;
            }
            set
            {
                this.requestedDate = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool RequestedDateSpecified
        {
            get
            {
                return this.requestedDateSpecified;
            }
            set
            {
                this.requestedDateSpecified = value;
            }
        }

        /// <remarks/>
        public System.DateTime PromisedDate
        {
            get
            {
                return this.promisedDate;
            }
            set
            {
                this.promisedDate = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool PromisedDateSpecified
        {
            get
            {
                return this.promisedDateSpecified;
            }
            set
            {
                this.promisedDateSpecified = value;
            }
        }

        /// <remarks/>
        public string NCMCode
        {
            get
            {
                return this.nCMCode;
            }
            set
            {
                this.nCMCode = value;
            }
        }

        /// <remarks/>
        public System.DateTime EntryDate
        {
            get
            {
                return this.entryDate;
            }
            set
            {
                this.entryDate = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool EntryDateSpecified
        {
            get
            {
                return this.entryDateSpecified;
            }
            set
            {
                this.entryDateSpecified = value;
            }
        }

        /// <remarks/>
        public VGMDataType VGMData
        {
            get
            {
                return this.vGMData;
            }
            set
            {
                this.vGMData = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Custom", IsNullable = false)]
        public CustomType[] Customs
        {
            get
            {
                return this.customs;
            }
            set
            {
                this.customs = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string GUID
        {
            get
            {
                return this.gUID;
            }
            set
            {
                this.gUID = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum ItemStatusType
    {

        /// <remarks/>
        OnHand,

        /// <remarks/>
        ToLoad,

        /// <remarks/>
        Loaded,

        /// <remarks/>
        InTransit,

        /// <remarks/>
        AtDestination,

        /// <remarks/>
        Delivered,

        /// <remarks/>
        Arriving,

        /// <remarks/>
        InQuote,

        /// <remarks/>
        Available,

        /// <remarks/>
        Booked,

        /// <remarks/>
        Processed,

        /// <remarks/>
        Pending,

        /// <remarks/>
        Ordered,

        /// <remarks/>
        Backordered,

        /// <remarks/>
        Canceled,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class PackageType
    {

        private PackageTypeDesc type;

        private string code;

        private LenghtValue length;

        private LenghtValue height;

        private LenghtValue width;

        private VolumeValue volume;

        private WeightValue weight;

        private WeightValue maxWeight;

        private string name;

        private string containerCode;

        private string containerEquipType;

        private MethodType[] methods;

        private CustomType[] customs;

        /// <remarks/>
        public PackageTypeDesc Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.code;
            }
            set
            {
                this.code = value;
            }
        }

        /// <remarks/>
        public LenghtValue Length
        {
            get
            {
                return this.length;
            }
            set
            {
                this.length = value;
            }
        }

        /// <remarks/>
        public LenghtValue Height
        {
            get
            {
                return this.height;
            }
            set
            {
                this.height = value;
            }
        }

        /// <remarks/>
        public LenghtValue Width
        {
            get
            {
                return this.width;
            }
            set
            {
                this.width = value;
            }
        }

        /// <remarks/>
        public VolumeValue Volume
        {
            get
            {
                return this.volume;
            }
            set
            {
                this.volume = value;
            }
        }

        /// <remarks/>
        public WeightValue Weight
        {
            get
            {
                return this.weight;
            }
            set
            {
                this.weight = value;
            }
        }

        /// <remarks/>
        public WeightValue MaxWeight
        {
            get
            {
                return this.maxWeight;
            }
            set
            {
                this.maxWeight = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        /// <remarks/>
        public string ContainerCode
        {
            get
            {
                return this.containerCode;
            }
            set
            {
                this.containerCode = value;
            }
        }

        /// <remarks/>
        public string ContainerEquipType
        {
            get
            {
                return this.containerEquipType;
            }
            set
            {
                this.containerEquipType = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Method", IsNullable = false)]
        public MethodType[] Methods
        {
            get
            {
                return this.methods;
            }
            set
            {
                this.methods = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Custom", IsNullable = false)]
        public CustomType[] Customs
        {
            get
            {
                return this.customs;
            }
            set
            {
                this.customs = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum PackageTypeDesc
    {

        /// <remarks/>
        Container,

        /// <remarks/>
        Other,

        /// <remarks/>
        Pallet,

        /// <remarks/>
        Box,

        /// <remarks/>
        Bag,

        /// <remarks/>
        Drum,

        /// <remarks/>
        Skid,

        /// <remarks/>
        Tank,

        /// <remarks/>
        Crate,

        /// <remarks/>
        Barrel,

        /// <remarks/>
        Bottle,

        /// <remarks/>
        Basket,

        /// <remarks/>
        Cabinet,

        /// <remarks/>
        Cones,

        /// <remarks/>
        Cylinder,

        /// <remarks/>
        Envelope,

        /// <remarks/>
        Frame,

        /// <remarks/>
        Gallon,

        /// <remarks/>
        Parcel,

        /// <remarks/>
        Pieces,

        /// <remarks/>
        Package,

        /// <remarks/>
        Rack,

        /// <remarks/>
        Roll,

        /// <remarks/>
        Sheet,

        /// <remarks/>
        Tube,

        /// <remarks/>
        Tray,

        /// <remarks/>
        Vehicles,

        /// <remarks/>
        Wrapped,

        /// <remarks/>
        Bundle,

        /// <remarks/>
        BingChest,

        /// <remarks/>
        Bin,

        /// <remarks/>
        Bucket,

        /// <remarks/>
        Bale,

        /// <remarks/>
        Can,

        /// <remarks/>
        Carcass,

        /// <remarks/>
        Case,

        /// <remarks/>
        ContainerBulkCargo,

        /// <remarks/>
        Carboy,

        /// <remarks/>
        CanCase,

        /// <remarks/>
        Chest,

        /// <remarks/>
        Coil,

        /// <remarks/>
        Cord,

        /// <remarks/>
        Cask,

        /// <remarks/>
        Carton,

        /// <remarks/>
        DryBulk,

        /// <remarks/>
        HeadOfBeef,

        /// <remarks/>
        Hamper,

        /// <remarks/>
        Keg,

        /// <remarks/>
        LiquidBulk,

        /// <remarks/>
        Log,

        /// <remarks/>
        Lug,

        /// <remarks/>
        LiftVan,

        /// <remarks/>
        Pail,

        /// <remarks/>
        PrivatelyOwnedVehicle,

        /// <remarks/>
        QuarterOfBeef,

        /// <remarks/>
        ToteBin,

        /// <remarks/>
        Tin,

        /// <remarks/>
        Unit,

        /// <remarks/>
        Pack,

        /// <remarks/>
        WoodenCase,

        /// <remarks/>
        Reel,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class LenghtValue
    {

        private LengthUnitType unit;

        private double value;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public LengthUnitType Unit
        {
            get
            {
                return this.unit;
            }
            set
            {
                this.unit = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public double Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum LengthUnitType
    {

        /// <remarks/>
        m,

        /// <remarks/>
        @in,

        /// <remarks/>
        ft,

        /// <remarks/>
        mm,

        /// <remarks/>
        cm,

        /// <remarks/>
        dm,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class VolumeValue
    {

        private VolumeUnitType unit;

        private double value;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public VolumeUnitType Unit
        {
            get
            {
                return this.unit;
            }
            set
            {
                this.unit = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public double Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum VolumeUnitType
    {

        /// <remarks/>
        m3,

        /// <remarks/>
        in3,

        /// <remarks/>
        ft3,

        /// <remarks/>
        cm3,

        /// <remarks/>
        dm3,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class WeightValue
    {

        private WeightUnitType unit;

        private double value;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public WeightUnitType Unit
        {
            get
            {
                return this.unit;
            }
            set
            {
                this.unit = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public double Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum WeightUnitType
    {

        /// <remarks/>
        kg,

        /// <remarks/>
        g,

        /// <remarks/>
        lb,

        /// <remarks/>
        t,

        /// <remarks/>
        oz,

        /// <remarks/>
        ozt,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class VolumeWeightValue
    {

        private VolumeWeightUnitType unit;

        private double value;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public VolumeWeightUnitType Unit
        {
            get
            {
                return this.unit;
            }
            set
            {
                this.unit = value;
            }
        }

        //public void dd()
        //{
        //    Unit = VolumeWeightUnitType.vlb;
        //}

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public double Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum VolumeWeightUnitType
    {

        /// <remarks/>
        m3,

        /// <remarks/>
        ft3,

        /// <remarks/>
        vlb,

        /// <remarks/>
        vkg,

        /// <remarks/>
        vg,

        /// <remarks/>
        vt,

        /// <remarks/>
        voz,

        /// <remarks/>
        vozt,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class CurrencyType
    {

        private string name;

        private double exchangeRate;

        private bool exchangeRateSpecified;

        private sbyte decimalPlaces;

        private bool decimalPlacesSpecified;

        private bool isHomeCurrency;

        private bool isHomeCurrencySpecified;

        private string code;

        private string symbol;

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        /// <remarks/>
        public double ExchangeRate
        {
            get
            {
                return this.exchangeRate;
            }
            set
            {
                this.exchangeRate = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ExchangeRateSpecified
        {
            get
            {
                return this.exchangeRateSpecified;
            }
            set
            {
                this.exchangeRateSpecified = value;
            }
        }

        /// <remarks/>
        public sbyte DecimalPlaces
        {
            get
            {
                return this.decimalPlaces;
            }
            set
            {
                this.decimalPlaces = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DecimalPlacesSpecified
        {
            get
            {
                return this.decimalPlacesSpecified;
            }
            set
            {
                this.decimalPlacesSpecified = value;
            }
        }

        /// <remarks/>
        public bool IsHomeCurrency
        {
            get
            {
                return this.isHomeCurrency;
            }
            set
            {
                this.isHomeCurrency = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsHomeCurrencySpecified
        {
            get
            {
                return this.isHomeCurrencySpecified;
            }
            set
            {
                this.isHomeCurrencySpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Code
        {
            get
            {
                return this.code;
            }
            set
            {
                this.code = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Symbol
        {
            get
            {
                return this.symbol;
            }
            set
            {
                this.symbol = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("ItemDefinition", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class ItemDefinition
    {

        private Item item;

        private bool itemSpecified; 

        private string partNumber;

        private string model;

        private string description;

        private System.DateTime createdOn;

        private bool createdOnSpecified;

        private string pieces;

        private string requestedPieces;

        private AESDataType aESData;

        private Entity client;

        private Entity manufacturer;

        private PackageType package;

        private HazardousType hazardous;

        private string amountPerPallet;

        private string minimumStock;

        private SKUType[] sKUNumbers;

        private InventoryTypeType inventoryType;

        private bool inventoryTypeSpecified;

        private MoneyValue unitaryValue;

        private ChargeDefinition incomeChargeDefinition;

        private ChargeDefinition costChargeDefinition;

        private string notes;

        private string arrivingPieces;

        private string requestedArrivingPieces;

        private string piecesInCompoundItems;

        private string arrivingPiecesInCompoundItems;

        private string availablePiecesForSale;

        private bool keepSerialNumbers;

        private bool keepSerialNumbersSpecified;

        private bool keepSerialWhenOut;

        private bool keepSerialWhenOutSpecified;

        private LenghtValue length;

        private LenghtValue height;

        private LenghtValue width;

        private WeightValue weight;

        private VolumeValue volume;

        private AttachmentType[] images;

        private string mainImageIdentifier;

        private bool autoPlacePurchaseOrders;

        private bool autoPlacePurchaseOrdersSpecified;

        private string defaultQuantityToOrder;

        private string commodityTypeName;

        private CommodityTypeType commodityType;

        private bool addKitItemsUngrouped;

        private bool addKitItemsUngroupedSpecified;

        private bool showKitItemsInDocs;

        private bool showKitItemsInDocsSpecified;

        private string nCMCode;

        private bool handledWithVariableWeight;

        private bool handledWithVariableWeightSpecified;

        private string unitOfMeasurement;

        private BOMItem[] billOfMaterials;

        private CategoryRefType[] categoryRefs;

        private CustomType[] customs;

        private string gUID;

        private string type;

        /// <remarks/>
        public Item Item
        {
            get
            {
                return this.item;
            }
            set
            {
                this.item = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ItemSpecified
        {
            get
            {
                return this.itemSpecified;
            }
            set
            {
                this.itemSpecified = value;
            }
        }

        /// <remarks/>
        public string PartNumber
        {
            get
            {
                return this.partNumber;
            }
            set
            {
                this.partNumber = value;
            }
        }

        /// <remarks/>
        public string Model
        {
            get
            {
                return this.model;
            }
            set
            {
                this.model = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        /// <remarks/>
        public System.DateTime CreatedOn
        {
            get
            {
                return this.createdOn;
            }
            set
            {
                this.createdOn = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CreatedOnSpecified
        {
            get
            {
                return this.createdOnSpecified;
            }
            set
            {
                this.createdOnSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "nonNegativeInteger")]
        public string Pieces
        {
            get
            {
                return this.pieces;
            }
            set
            {
                this.pieces = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "nonNegativeInteger")]
        public string RequestedPieces
        {
            get
            {
                return this.requestedPieces;
            }
            set
            {
                this.requestedPieces = value;
            }
        }

        /// <remarks/>
        public AESDataType AESData
        {
            get
            {
                return this.aESData;
            }
            set
            {
                this.aESData = value;
            }
        }

        /// <remarks/>
        public Entity Client
        {
            get
            {
                return this.client;
            }
            set
            {
                this.client = value;
            }
        }

        /// <remarks/>
        public Entity Manufacturer
        {
            get
            {
                return this.manufacturer;
            }
            set
            {
                this.manufacturer = value;
            }
        }

        /// <remarks/>
        public PackageType Package
        {
            get
            {
                return this.package;
            }
            set
            {
                this.package = value;
            }
        }

        /// <remarks/>
        public HazardousType Hazardous
        {
            get
            {
                return this.hazardous;
            }
            set
            {
                this.hazardous = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string AmountPerPallet
        {
            get
            {
                return this.amountPerPallet;
            }
            set
            {
                this.amountPerPallet = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string MinimumStock
        {
            get
            {
                return this.minimumStock;
            }
            set
            {
                this.minimumStock = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("SKU", IsNullable = false)]
        public SKUType[] SKUNumbers
        {
            get
            {
                return this.sKUNumbers;
            }
            set
            {
                this.sKUNumbers = value;
            }
        }

        /// <remarks/>
        public InventoryTypeType InventoryType
        {
            get
            {
                return this.inventoryType;
            }
            set
            {
                this.inventoryType = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool InventoryTypeSpecified
        {
            get
            {
                return this.inventoryTypeSpecified;
            }
            set
            {
                this.inventoryTypeSpecified = value;
            }
        }

        /// <remarks/>
        public MoneyValue UnitaryValue
        {
            get
            {
                return this.unitaryValue;
            }
            set
            {
                this.unitaryValue = value;
            }
        }

        /// <remarks/>
        public ChargeDefinition IncomeChargeDefinition
        {
            get
            {
                return this.incomeChargeDefinition;
            }
            set
            {
                this.incomeChargeDefinition = value;
            }
        }

        /// <remarks/>
        public ChargeDefinition CostChargeDefinition
        {
            get
            {
                return this.costChargeDefinition;
            }
            set
            {
                this.costChargeDefinition = value;
            }
        }

        /// <remarks/>
        public string Notes
        {
            get
            {
                return this.notes;
            }
            set
            {
                this.notes = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "nonNegativeInteger")]
        public string ArrivingPieces
        {
            get
            {
                return this.arrivingPieces;
            }
            set
            {
                this.arrivingPieces = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "nonNegativeInteger")]
        public string RequestedArrivingPieces
        {
            get
            {
                return this.requestedArrivingPieces;
            }
            set
            {
                this.requestedArrivingPieces = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "nonNegativeInteger")]
        public string PiecesInCompoundItems
        {
            get
            {
                return this.piecesInCompoundItems;
            }
            set
            {
                this.piecesInCompoundItems = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "nonNegativeInteger")]
        public string ArrivingPiecesInCompoundItems
        {
            get
            {
                return this.arrivingPiecesInCompoundItems;
            }
            set
            {
                this.arrivingPiecesInCompoundItems = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "nonNegativeInteger")]
        public string AvailablePiecesForSale
        {
            get
            {
                return this.availablePiecesForSale;
            }
            set
            {
                this.availablePiecesForSale = value;
            }
        }

        /// <remarks/>
        public bool KeepSerialNumbers
        {
            get
            {
                return this.keepSerialNumbers;
            }
            set
            {
                this.keepSerialNumbers = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool KeepSerialNumbersSpecified
        {
            get
            {
                return this.keepSerialNumbersSpecified;
            }
            set
            {
                this.keepSerialNumbersSpecified = value;
            }
        }

        /// <remarks/>
        public bool KeepSerialWhenOut
        {
            get
            {
                return this.keepSerialWhenOut;
            }
            set
            {
                this.keepSerialWhenOut = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool KeepSerialWhenOutSpecified
        {
            get
            {
                return this.keepSerialWhenOutSpecified;
            }
            set
            {
                this.keepSerialWhenOutSpecified = value;
            }
        }

        /// <remarks/>
        public LenghtValue Length
        {
            get
            {
                return this.length;
            }
            set
            {
                this.length = value;
            }
        }

        /// <remarks/>
        public LenghtValue Height
        {
            get
            {
                return this.height;
            }
            set
            {
                this.height = value;
            }
        }

        /// <remarks/>
        public LenghtValue Width
        {
            get
            {
                return this.width;
            }
            set
            {
                this.width = value;
            }
        }

        /// <remarks/>
        public WeightValue Weight
        {
            get
            {
                return this.weight;
            }
            set
            {
                this.weight = value;
            }
        }

        /// <remarks/>
        public VolumeValue Volume
        {
            get
            {
                return this.volume;
            }
            set
            {
                this.volume = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Attachment", IsNullable = false)]
        public AttachmentType[] Images
        {
            get
            {
                return this.images;
            }
            set
            {
                this.images = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string MainImageIdentifier
        {
            get
            {
                return this.mainImageIdentifier;
            }
            set
            {
                this.mainImageIdentifier = value;
            }
        }

        /// <remarks/>
        public bool AutoPlacePurchaseOrders
        {
            get
            {
                return this.autoPlacePurchaseOrders;
            }
            set
            {
                this.autoPlacePurchaseOrders = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AutoPlacePurchaseOrdersSpecified
        {
            get
            {
                return this.autoPlacePurchaseOrdersSpecified;
            }
            set
            {
                this.autoPlacePurchaseOrdersSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string DefaultQuantityToOrder
        {
            get
            {
                return this.defaultQuantityToOrder;
            }
            set
            {
                this.defaultQuantityToOrder = value;
            }
        }

        /// <remarks/>
        public string CommodityTypeName
        {
            get
            {
                return this.commodityTypeName;
            }
            set
            {
                this.commodityTypeName = value;
            }
        }

        /// <remarks/>
        public CommodityTypeType CommodityType
        {
            get
            {
                return this.commodityType;
            }
            set
            {
                this.commodityType = value;
            }
        }

        /// <remarks/>
        public bool AddKitItemsUngrouped
        {
            get
            {
                return this.addKitItemsUngrouped;
            }
            set
            {
                this.addKitItemsUngrouped = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AddKitItemsUngroupedSpecified
        {
            get
            {
                return this.addKitItemsUngroupedSpecified;
            }
            set
            {
                this.addKitItemsUngroupedSpecified = value;
            }
        }

        /// <remarks/>
        public bool ShowKitItemsInDocs
        {
            get
            {
                return this.showKitItemsInDocs;
            }
            set
            {
                this.showKitItemsInDocs = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ShowKitItemsInDocsSpecified
        {
            get
            {
                return this.showKitItemsInDocsSpecified;
            }
            set
            {
                this.showKitItemsInDocsSpecified = value;
            }
        }

        /// <remarks/>
        public string NCMCode
        {
            get
            {
                return this.nCMCode;
            }
            set
            {
                this.nCMCode = value;
            }
        }

        /// <remarks/>
        public bool HandledWithVariableWeight
        {
            get
            {
                return this.handledWithVariableWeight;
            }
            set
            {
                this.handledWithVariableWeight = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool HandledWithVariableWeightSpecified
        {
            get
            {
                return this.handledWithVariableWeightSpecified;
            }
            set
            {
                this.handledWithVariableWeightSpecified = value;
            }
        }

        /// <remarks/>
        public string UnitOfMeasurement
        {
            get
            {
                return this.unitOfMeasurement;
            }
            set
            {
                this.unitOfMeasurement = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("BOMItem", IsNullable = false)]
        public BOMItem[] BillOfMaterials
        {
            get
            {
                return this.billOfMaterials;
            }
            set
            {
                this.billOfMaterials = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("CategoryRef", IsNullable = false)]
        public CategoryRefType[] CategoryRefs
        {
            get
            {
                return this.categoryRefs;
            }
            set
            {
                this.categoryRefs = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Custom", IsNullable = false)]
        public CustomType[] Customs
        {
            get
            {
                return this.customs;
            }
            set
            {
                this.customs = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string GUID
        {
            get
            {
                return this.gUID;
            }
            set
            {
                this.gUID = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum ItemType
    {

        /// <remarks/>
        StockItem,

        /// <remarks/>
        KitItem,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class AESDataType
    {

        private ScheduleBType scheduleB;

        private VehicleDataType vehicleData;

        private MilitaryDataType militaryData;

        private string exportCode;

        private string quantity1;

        private string quantity2;

        private string licenseType;

        private string licenseValue;

        private string licenseNumber;

        private string eCCN;

        private CurrencyType currency;

        private MoneyValue value;

        private OriginType origin;

        private bool originSpecified;

        private string stateOfOrigin;

        /// <remarks/>
        public ScheduleBType ScheduleB
        {
            get
            {
                return this.scheduleB;
            }
            set
            {
                this.scheduleB = value;
            }
        }

        /// <remarks/>
        public VehicleDataType VehicleData
        {
            get
            {
                return this.vehicleData;
            }
            set
            {
                this.vehicleData = value;
            }
        }

        /// <remarks/>
        public MilitaryDataType MilitaryData
        {
            get
            {
                return this.militaryData;
            }
            set
            {
                this.militaryData = value;
            }
        }

        /// <remarks/>
        public string ExportCode
        {
            get
            {
                return this.exportCode;
            }
            set
            {
                this.exportCode = value;
            }
        }

        /// <remarks/>
        public string Quantity1
        {
            get
            {
                return this.quantity1;
            }
            set
            {
                this.quantity1 = value;
            }
        }

        /// <remarks/>
        public string Quantity2
        {
            get
            {
                return this.quantity2;
            }
            set
            {
                this.quantity2 = value;
            }
        }

        /// <remarks/>
        public string LicenseType
        {
            get
            {
                return this.licenseType;
            }
            set
            {
                this.licenseType = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string LicenseValue
        {
            get
            {
                return this.licenseValue;
            }
            set
            {
                this.licenseValue = value;
            }
        }

        /// <remarks/>
        public string LicenseNumber
        {
            get
            {
                return this.licenseNumber;
            }
            set
            {
                this.licenseNumber = value;
            }
        }

        /// <remarks/>
        public string ECCN
        {
            get
            {
                return this.eCCN;
            }
            set
            {
                this.eCCN = value;
            }
        }

        /// <remarks/>
        public CurrencyType Currency
        {
            get
            {
                return this.currency;
            }
            set
            {
                this.currency = value;
            }
        }

        /// <remarks/>
        public MoneyValue Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }

        /// <remarks/>
        public OriginType Origin
        {
            get
            {
                return this.origin;
            }
            set
            {
                this.origin = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool OriginSpecified
        {
            get
            {
                return this.originSpecified;
            }
            set
            {
                this.originSpecified = value;
            }
        }

        /// <remarks/>
        public string StateOfOrigin
        {
            get
            {
                return this.stateOfOrigin;
            }
            set
            {
                this.stateOfOrigin = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class ScheduleBType
    {

        private string description;

        private string quantity1;

        private string quantity2;

        private CustomType[] customs;

        private string code;

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        /// <remarks/>
        public string Quantity1
        {
            get
            {
                return this.quantity1;
            }
            set
            {
                this.quantity1 = value;
            }
        }

        /// <remarks/>
        public string Quantity2
        {
            get
            {
                return this.quantity2;
            }
            set
            {
                this.quantity2 = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Custom", IsNullable = false)]
        public CustomType[] Customs
        {
            get
            {
                return this.customs;
            }
            set
            {
                this.customs = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Code
        {
            get
            {
                return this.code;
            }
            set
            {
                this.code = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class VehicleDataType
    {

        private VehicleIDTypeDesc vehicleIDType;

        private string vehicleID;

        private string vehicleTitle;

        private string vehicleState;

        private string cardOrderNumber;

        private bool isNewVehicle;

        private bool isNewVehicleSpecified;

        private string modelYear;

        private string make;

        private string model;

        private string trimLevel;

        private string vehicleClass;

        private string vehicleType;

        private string manufacturer;

        private string bodyType;

        private string country;

        private string engineType;

        /// <remarks/>
        public VehicleIDTypeDesc VehicleIDType
        {
            get
            {
                return this.vehicleIDType;
            }
            set
            {
                this.vehicleIDType = value;
            }
        }

        /// <remarks/>
        public string VehicleID
        {
            get
            {
                return this.vehicleID;
            }
            set
            {
                this.vehicleID = value;
            }
        }

        /// <remarks/>
        public string VehicleTitle
        {
            get
            {
                return this.vehicleTitle;
            }
            set
            {
                this.vehicleTitle = value;
            }
        }

        /// <remarks/>
        public string VehicleState
        {
            get
            {
                return this.vehicleState;
            }
            set
            {
                this.vehicleState = value;
            }
        }

        /// <remarks/>
        public string CardOrderNumber
        {
            get
            {
                return this.cardOrderNumber;
            }
            set
            {
                this.cardOrderNumber = value;
            }
        }

        /// <remarks/>
        public bool IsNewVehicle
        {
            get
            {
                return this.isNewVehicle;
            }
            set
            {
                this.isNewVehicle = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsNewVehicleSpecified
        {
            get
            {
                return this.isNewVehicleSpecified;
            }
            set
            {
                this.isNewVehicleSpecified = value;
            }
        }

        /// <remarks/>
        public string ModelYear
        {
            get
            {
                return this.modelYear;
            }
            set
            {
                this.modelYear = value;
            }
        }

        /// <remarks/>
        public string Make
        {
            get
            {
                return this.make;
            }
            set
            {
                this.make = value;
            }
        }

        /// <remarks/>
        public string Model
        {
            get
            {
                return this.model;
            }
            set
            {
                this.model = value;
            }
        }

        /// <remarks/>
        public string TrimLevel
        {
            get
            {
                return this.trimLevel;
            }
            set
            {
                this.trimLevel = value;
            }
        }

        /// <remarks/>
        public string VehicleClass
        {
            get
            {
                return this.vehicleClass;
            }
            set
            {
                this.vehicleClass = value;
            }
        }

        /// <remarks/>
        public string VehicleType
        {
            get
            {
                return this.vehicleType;
            }
            set
            {
                this.vehicleType = value;
            }
        }

        /// <remarks/>
        public string Manufacturer
        {
            get
            {
                return this.manufacturer;
            }
            set
            {
                this.manufacturer = value;
            }
        }

        /// <remarks/>
        public string BodyType
        {
            get
            {
                return this.bodyType;
            }
            set
            {
                this.bodyType = value;
            }
        }

        /// <remarks/>
        public string Country
        {
            get
            {
                return this.country;
            }
            set
            {
                this.country = value;
            }
        }

        /// <remarks/>
        public string EngineType
        {
            get
            {
                return this.engineType;
            }
            set
            {
                this.engineType = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum VehicleIDTypeDesc
    {

        /// <remarks/>
        VIN,

        /// <remarks/>
        Product,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class MilitaryDataType
    {

        private string exemptionNumber;

        private string registrationNumber;

        private bool isSignificantMilitaryEquipment;

        private bool isSignificantMilitaryEquipmentSpecified;

        private bool isElegiblePartyCertification;

        private bool isElegiblePartyCertificationSpecified;

        private string uSMLCategoryCode;

        private string unitOfMeasureCode;

        private string quantity;

        /// <remarks/>
        public string ExemptionNumber
        {
            get
            {
                return this.exemptionNumber;
            }
            set
            {
                this.exemptionNumber = value;
            }
        }

        /// <remarks/>
        public string RegistrationNumber
        {
            get
            {
                return this.registrationNumber;
            }
            set
            {
                this.registrationNumber = value;
            }
        }

        /// <remarks/>
        public bool IsSignificantMilitaryEquipment
        {
            get
            {
                return this.isSignificantMilitaryEquipment;
            }
            set
            {
                this.isSignificantMilitaryEquipment = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsSignificantMilitaryEquipmentSpecified
        {
            get
            {
                return this.isSignificantMilitaryEquipmentSpecified;
            }
            set
            {
                this.isSignificantMilitaryEquipmentSpecified = value;
            }
        }

        /// <remarks/>
        public bool IsElegiblePartyCertification
        {
            get
            {
                return this.isElegiblePartyCertification;
            }
            set
            {
                this.isElegiblePartyCertification = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsElegiblePartyCertificationSpecified
        {
            get
            {
                return this.isElegiblePartyCertificationSpecified;
            }
            set
            {
                this.isElegiblePartyCertificationSpecified = value;
            }
        }

        /// <remarks/>
        public string USMLCategoryCode
        {
            get
            {
                return this.uSMLCategoryCode;
            }
            set
            {
                this.uSMLCategoryCode = value;
            }
        }

        /// <remarks/>
        public string UnitOfMeasureCode
        {
            get
            {
                return this.unitOfMeasureCode;
            }
            set
            {
                this.unitOfMeasureCode = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string Quantity
        {
            get
            {
                return this.quantity;
            }
            set
            {
                this.quantity = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum OriginType
    {

        /// <remarks/>
        Domestic,

        /// <remarks/>
        Foreign,

        /// <remarks/>
        Military,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class HazardousType
    {

        private string materialCode;

        private string materialClass;

        private string classQualifier;

        private string materialDescription;

        private string materialPage;

        private string flashpointTemp;

        private string classDescription;

        private string specialInstructions;

        private string emergencyContact;

        private Entity emergencyEntity;

        private HazardousLevelType hazardousLevel;

        private bool hazardousLevelSpecified;

        /// <remarks/>
        public string MaterialCode
        {
            get
            {
                return this.materialCode;
            }
            set
            {
                this.materialCode = value;
            }
        }

        /// <remarks/>
        public string MaterialClass
        {
            get
            {
                return this.materialClass;
            }
            set
            {
                this.materialClass = value;
            }
        }

        /// <remarks/>
        public string ClassQualifier
        {
            get
            {
                return this.classQualifier;
            }
            set
            {
                this.classQualifier = value;
            }
        }

        /// <remarks/>
        public string MaterialDescription
        {
            get
            {
                return this.materialDescription;
            }
            set
            {
                this.materialDescription = value;
            }
        }

        /// <remarks/>
        public string MaterialPage
        {
            get
            {
                return this.materialPage;
            }
            set
            {
                this.materialPage = value;
            }
        }

        /// <remarks/>
        public string FlashpointTemp
        {
            get
            {
                return this.flashpointTemp;
            }
            set
            {
                this.flashpointTemp = value;
            }
        }

        /// <remarks/>
        public string ClassDescription
        {
            get
            {
                return this.classDescription;
            }
            set
            {
                this.classDescription = value;
            }
        }

        /// <remarks/>
        public string SpecialInstructions
        {
            get
            {
                return this.specialInstructions;
            }
            set
            {
                this.specialInstructions = value;
            }
        }

        /// <remarks/>
        public string EmergencyContact
        {
            get
            {
                return this.emergencyContact;
            }
            set
            {
                this.emergencyContact = value;
            }
        }

        /// <remarks/>
        public Entity EmergencyEntity
        {
            get
            {
                return this.emergencyEntity;
            }
            set
            {
                this.emergencyEntity = value;
            }
        }

        /// <remarks/>
        public HazardousLevelType HazardousLevel
        {
            get
            {
                return this.hazardousLevel;
            }
            set
            {
                this.hazardousLevel = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool HazardousLevelSpecified
        {
            get
            {
                return this.hazardousLevelSpecified;
            }
            set
            {
                this.hazardousLevelSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum HazardousLevelType
    {

        /// <remarks/>
        MinorDanger,

        /// <remarks/>
        MediumDanger,

        /// <remarks/>
        GreatDanger,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class SKUType
    {

        private string code;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Code
        {
            get
            {
                return this.code;
            }
            set
            {
                this.code = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum InventoryTypeType
    {

        /// <remarks/>
        FIFO,

        /// <remarks/>
        LIFO,

        /// <remarks/>
        FEFO,

        /// <remarks/>
        LEFO,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class ChargeDefinition
    {

        private ChargeDefType type;

        private string description;

        private string code;

        private AccountDefinition accountDefinition;

        private MoneyValue amount;

        private string notes;

        private CurrencyType currency;

        private TaxDefinition taxDefinition;

        private ChargeDefinition resellCharge;

        private Entity preferredVendor;

        private AccountDefinition assetAccount;

        private ItemDefinition itemDefinition;

        private sbyte prioritySortingOrder;

        private bool prioritySortingOrderSpecified;

        private string iATACode;

        private bool enforce3rdPartyBilling;

        private bool enforce3rdPartyBillingSpecified;

        private CustomType[] customs;

        /// <remarks/>
        public ChargeDefType Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.code;
            }
            set
            {
                this.code = value;
            }
        }

        /// <remarks/>
        public AccountDefinition AccountDefinition
        {
            get
            {
                return this.accountDefinition;
            }
            set
            {
                this.accountDefinition = value;
            }
        }

        /// <remarks/>
        public MoneyValue Amount
        {
            get
            {
                return this.amount;
            }
            set
            {
                this.amount = value;
            }
        }

        /// <remarks/>
        public string Notes
        {
            get
            {
                return this.notes;
            }
            set
            {
                this.notes = value;
            }
        }

        /// <remarks/>
        public CurrencyType Currency
        {
            get
            {
                return this.currency;
            }
            set
            {
                this.currency = value;
            }
        }

        /// <remarks/>
        public TaxDefinition TaxDefinition
        {
            get
            {
                return this.taxDefinition;
            }
            set
            {
                this.taxDefinition = value;
            }
        }

        /// <remarks/>
        public ChargeDefinition ResellCharge
        {
            get
            {
                return this.resellCharge;
            }
            set
            {
                this.resellCharge = value;
            }
        }

        /// <remarks/>
        public Entity PreferredVendor
        {
            get
            {
                return this.preferredVendor;
            }
            set
            {
                this.preferredVendor = value;
            }
        }

        /// <remarks/>
        public AccountDefinition AssetAccount
        {
            get
            {
                return this.assetAccount;
            }
            set
            {
                this.assetAccount = value;
            }
        }

        /// <remarks/>
        public ItemDefinition ItemDefinition
        {
            get
            {
                return this.itemDefinition;
            }
            set
            {
                this.itemDefinition = value;
            }
        }

        /// <remarks/>
        public sbyte PrioritySortingOrder
        {
            get
            {
                return this.prioritySortingOrder;
            }
            set
            {
                this.prioritySortingOrder = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool PrioritySortingOrderSpecified
        {
            get
            {
                return this.prioritySortingOrderSpecified;
            }
            set
            {
                this.prioritySortingOrderSpecified = value;
            }
        }

        /// <remarks/>
        public string IATACode
        {
            get
            {
                return this.iATACode;
            }
            set
            {
                this.iATACode = value;
            }
        }

        /// <remarks/>
        public bool Enforce3rdPartyBilling
        {
            get
            {
                return this.enforce3rdPartyBilling;
            }
            set
            {
                this.enforce3rdPartyBilling = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Enforce3rdPartyBillingSpecified
        {
            get
            {
                return this.enforce3rdPartyBillingSpecified;
            }
            set
            {
                this.enforce3rdPartyBillingSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Custom", IsNullable = false)]
        public CustomType[] Customs
        {
            get
            {
                return this.customs;
            }
            set
            {
                this.customs = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum ChargeDefType
    {

        /// <remarks/>
        Freight,

        /// <remarks/>
        OtherFreight,

        /// <remarks/>
        Valuation,

        /// <remarks/>
        Tax,

        /// <remarks/>
        Other,

        /// <remarks/>
        Inventory,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class AccountDefinition
    {

        private AccountType type;

        private string name;

        private string number;

        private string notes;

        private CurrencyType currency;

        private CustomType[] customs;

        private AccountDefinition parentAccount;

        /// <remarks/>
        public AccountType Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        /// <remarks/>
        public string Number
        {
            get
            {
                return this.number;
            }
            set
            {
                this.number = value;
            }
        }

        /// <remarks/>
        public string Notes
        {
            get
            {
                return this.notes;
            }
            set
            {
                this.notes = value;
            }
        }

        /// <remarks/>
        public CurrencyType Currency
        {
            get
            {
                return this.currency;
            }
            set
            {
                this.currency = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Custom", IsNullable = false)]
        public CustomType[] Customs
        {
            get
            {
                return this.customs;
            }
            set
            {
                this.customs = value;
            }
        }

        /// <remarks/>
        public AccountDefinition ParentAccount
        {
            get
            {
                return this.parentAccount;
            }
            set
            {
                this.parentAccount = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum AccountType
    {

        /// <remarks/>
        AccountReceivable,

        /// <remarks/>
        AccountPayable,

        /// <remarks/>
        Income,

        /// <remarks/>
        Expense,

        /// <remarks/>
        CostOfGoodsSold,

        /// <remarks/>
        BankAccount,

        /// <remarks/>
        FixedAssets,

        /// <remarks/>
        OtherAssets,

        /// <remarks/>
        OtherCurrentAssets,

        /// <remarks/>
        LongTermLiability,

        /// <remarks/>
        OtherCurrentLiability,

        /// <remarks/>
        Equity,

        /// <remarks/>
        CreditCard,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class TaxDefinition
    {

        private string code;

        private TaxDefType type;

        private bool typeSpecified;

        private TaxDefLayoutType layout;

        private bool layoutSpecified;

        private string name;

        private double rate;

        private bool rateSpecified;

        private Entity taxAuthority;

        private MoneyValueEx minimumAmount;

        private MoneyValueEx maximumAmount;

        private bool minimumAppliedToTaxableTotal;

        private bool minimumAppliedToTaxableTotalSpecified;

        private bool minimumAppliedToInvoiceTotal;

        private bool minimumAppliedToInvoiceTotalSpecified;

        private AccountDefinition assetAccount;

        private AccountDefinition liabilityAccount;

        private TaxDefinition[] taxDefinitions;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.code;
            }
            set
            {
                this.code = value;
            }
        }

        /// <remarks/>
        public TaxDefType Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TypeSpecified
        {
            get
            {
                return this.typeSpecified;
            }
            set
            {
                this.typeSpecified = value;
            }
        }

        /// <remarks/>
        public TaxDefLayoutType Layout
        {
            get
            {
                return this.layout;
            }
            set
            {
                this.layout = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool LayoutSpecified
        {
            get
            {
                return this.layoutSpecified;
            }
            set
            {
                this.layoutSpecified = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        /// <remarks/>
        public double Rate
        {
            get
            {
                return this.rate;
            }
            set
            {
                this.rate = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool RateSpecified
        {
            get
            {
                return this.rateSpecified;
            }
            set
            {
                this.rateSpecified = value;
            }
        }

        /// <remarks/>
        public Entity TaxAuthority
        {
            get
            {
                return this.taxAuthority;
            }
            set
            {
                this.taxAuthority = value;
            }
        }

        /// <remarks/>
        public MoneyValueEx MinimumAmount
        {
            get
            {
                return this.minimumAmount;
            }
            set
            {
                this.minimumAmount = value;
            }
        }

        /// <remarks/>
        public MoneyValueEx MaximumAmount
        {
            get
            {
                return this.maximumAmount;
            }
            set
            {
                this.maximumAmount = value;
            }
        }

        /// <remarks/>
        public bool MinimumAppliedToTaxableTotal
        {
            get
            {
                return this.minimumAppliedToTaxableTotal;
            }
            set
            {
                this.minimumAppliedToTaxableTotal = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MinimumAppliedToTaxableTotalSpecified
        {
            get
            {
                return this.minimumAppliedToTaxableTotalSpecified;
            }
            set
            {
                this.minimumAppliedToTaxableTotalSpecified = value;
            }
        }

        /// <remarks/>
        public bool MinimumAppliedToInvoiceTotal
        {
            get
            {
                return this.minimumAppliedToInvoiceTotal;
            }
            set
            {
                this.minimumAppliedToInvoiceTotal = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MinimumAppliedToInvoiceTotalSpecified
        {
            get
            {
                return this.minimumAppliedToInvoiceTotalSpecified;
            }
            set
            {
                this.minimumAppliedToInvoiceTotalSpecified = value;
            }
        }

        /// <remarks/>
        public AccountDefinition AssetAccount
        {
            get
            {
                return this.assetAccount;
            }
            set
            {
                this.assetAccount = value;
            }
        }

        /// <remarks/>
        public AccountDefinition LiabilityAccount
        {
            get
            {
                return this.liabilityAccount;
            }
            set
            {
                this.liabilityAccount = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("TaxDefinition", IsNullable = false)]
        public TaxDefinition[] TaxDefinitions
        {
            get
            {
                return this.taxDefinitions;
            }
            set
            {
                this.taxDefinitions = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum TaxDefType
    {

        /// <remarks/>
        Tax,

        /// <remarks/>
        Retention,

        /// <remarks/>
        FlatValue,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum TaxDefLayoutType
    {

        /// <remarks/>
        Simple,

        /// <remarks/>
        TaxGroup,

        /// <remarks/>
        Cascade,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class MoneyValueEx
    {

        private string currency;

        private double exchangeRate;

        private double value;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Currency
        {
            get
            {
                return this.currency;
            }
            set
            {
                this.currency = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public double ExchangeRate
        {
            get
            {
                return this.exchangeRate;
            }
            set
            {
                this.exchangeRate = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public double Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("Attachment", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class AttachmentType
    {

        private string name;

        private string extension;

        private byte[] data;

        private long size;

        private bool sizeSpecified;

        private bool isImage;

        private bool isImageSpecified;

        private long identifier;

        private bool identifierSpecified;

        private string ownerType;

        private string ownerGUID;

        private bool isInternal;

        private bool isInternalSpecified;

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        /// <remarks/>
        public string Extension
        {
            get
            {
                return this.extension;
            }
            set
            {
                this.extension = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "base64Binary")]
        public byte[] Data
        {
            get
            {
                return this.data;
            }
            set
            {
                this.data = value;
            }
        }

        /// <remarks/>
        public long Size
        {
            get
            {
                return this.size;
            }
            set
            {
                this.size = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool SizeSpecified
        {
            get
            {
                return this.sizeSpecified;
            }
            set
            {
                this.sizeSpecified = value;
            }
        }

        /// <remarks/>
        public bool IsImage
        {
            get
            {
                return this.isImage;
            }
            set
            {
                this.isImage = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsImageSpecified
        {
            get
            {
                return this.isImageSpecified;
            }
            set
            {
                this.isImageSpecified = value;
            }
        }

        /// <remarks/>
        public long Identifier
        {
            get
            {
                return this.identifier;
            }
            set
            {
                this.identifier = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IdentifierSpecified
        {
            get
            {
                return this.identifierSpecified;
            }
            set
            {
                this.identifierSpecified = value;
            }
        }

        /// <remarks/>
        public string OwnerType
        {
            get
            {
                return this.ownerType;
            }
            set
            {
                this.ownerType = value;
            }
        }

        /// <remarks/>
        public string OwnerGUID
        {
            get
            {
                return this.ownerGUID;
            }
            set
            {
                this.ownerGUID = value;
            }
        }

        /// <remarks/>
        public bool IsInternal
        {
            get
            {
                return this.isInternal;
            }
            set
            {
                this.isInternal = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsInternalSpecified
        {
            get
            {
                return this.isInternalSpecified;
            }
            set
            {
                this.isInternalSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class CommodityTypeType
    {

        private string description;

        private CustomType[] customs;

        private string code;

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Custom", IsNullable = false)]
        public CustomType[] Customs
        {
            get
            {
                return this.customs;
            }
            set
            {
                this.customs = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Code
        {
            get
            {
                return this.code;
            }
            set
            {
                this.code = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class BOMItem
    {

        private string itemDefinitionGUID;

        private string quantity;

        /// <remarks/>
        public string ItemDefinitionGUID
        {
            get
            {
                return this.itemDefinitionGUID;
            }
            set
            {
                this.itemDefinitionGUID = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string Quantity
        {
            get
            {
                return this.quantity;
            }
            set
            {
                this.quantity = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class CategoryRefType
    {

        private string gUID;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string GUID
        {
            get
            {
                return this.gUID;
            }
            set
            {
                this.gUID = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class TrackingNumberInfoType
    {

        private string trackingNumber;

        private string type;

        /// <remarks/>
        public string TrackingNumber
        {
            get
            {
                return this.trackingNumber;
            }
            set
            {
                this.trackingNumber = value;
            }
        }

        /// <remarks/>
        public string Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("Location", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    [JsonObject]
    public partial class LocationType
    {

        private long networkID;

        private bool networkIDSpecified;

        private string description;

        private LocationTypeDesc type;

        private bool typeSpecified;

        private LenghtValue length;

        private LenghtValue width;

        private LenghtValue height;

        private Item[] items;

        private string warehouseZoneName;

        private LocationDefinitionType locationDefinition;

        private bool isDisabled;

        private bool isDisabledSpecified;

        private Entity customer;

        private CustomType[] customs;

        private string sequencePosition;

        private string code;

        /// <remarks/>
        public long NetworkID
        {
            get
            {
                return this.networkID;
            }
            set
            {
                this.networkID = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool NetworkIDSpecified
        {
            get
            {
                return this.networkIDSpecified;
            }
            set
            {
                this.networkIDSpecified = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        /// <remarks/>
        public LocationTypeDesc Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TypeSpecified
        {
            get
            {
                return this.typeSpecified;
            }
            set
            {
                this.typeSpecified = value;
            }
        }

        /// <remarks/>
        public LenghtValue Length
        {
            get
            {
                return this.length;
            }
            set
            {
                this.length = value;
            }
        }

        /// <remarks/>
        public LenghtValue Width
        {
            get
            {
                return this.width;
            }
            set
            {
                this.width = value;
            }
        }

        /// <remarks/>
        public LenghtValue Height
        {
            get
            {
                return this.height;
            }
            set
            {
                this.height = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Item", IsNullable = false)]
        public Item[] Items
        {
            get
            {
                return this.items;
            }
            set
            {
                this.items = value;
            }
        }

        /// <remarks/>
        public string WarehouseZoneName
        {
            get
            {
                return this.warehouseZoneName;
            }
            set
            {
                this.warehouseZoneName = value;
            }
        }

        /// <remarks/>
        public LocationDefinitionType LocationDefinition
        {
            get
            {
                return this.locationDefinition;
            }
            set
            {
                this.locationDefinition = value;
            }
        }

        /// <remarks/>
        public bool IsDisabled
        {
            get
            {
                return this.isDisabled;
            }
            set
            {
                this.isDisabled = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsDisabledSpecified
        {
            get
            {
                return this.isDisabledSpecified;
            }
            set
            {
                this.isDisabledSpecified = value;
            }
        }

        /// <remarks/>
        public Entity Customer
        {
            get
            {
                return this.customer;
            }
            set
            {
                this.customer = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Custom", IsNullable = false)]
        public CustomType[] Customs
        {
            get
            {
                return this.customs;
            }
            set
            {
                this.customs = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string SequencePosition
        {
            get
            {
                return this.sequencePosition;
            }
            set
            {
                this.sequencePosition = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Code
        {
            get
            {
                return this.code;
            }
            set
            {
                this.code = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum LocationTypeDesc
    {

        /// <remarks/>
        Storage,

        /// <remarks/>
        Receiving,

        /// <remarks/>
        Shipping,

        /// <remarks/>
        QualityControl,

        /// <remarks/>
        Mobile,

        /// <remarks/>
        Other,

        /// <remarks/>
        Replenishment,

        /// <remarks/>
        Picking,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Replenishment")]
        Replenishment1,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Picking")]
        Picking1,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("LocationDefinition", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class LocationDefinitionType
    {

        private string description;

        private LocationTypeDesc type;

        private bool typeSpecified;

        private WeightValue maxWeight;

        private DimensionsType dimensions;

        private string gUID;

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        /// <remarks/>
        public LocationTypeDesc Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TypeSpecified
        {
            get
            {
                return this.typeSpecified;
            }
            set
            {
                this.typeSpecified = value;
            }
        }

        /// <remarks/>
        public WeightValue MaxWeight
        {
            get
            {
                return this.maxWeight;
            }
            set
            {
                this.maxWeight = value;
            }
        }

        /// <remarks/>
        public DimensionsType Dimensions
        {
            get
            {
                return this.dimensions;
            }
            set
            {
                this.dimensions = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string GUID
        {
            get
            {
                return this.gUID;
            }
            set
            {
                this.gUID = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class DimensionsType
    {

        private LenghtValue length;

        private LenghtValue width;

        private LenghtValue height;

        /// <remarks/>
        public LenghtValue Length
        {
            get
            {
                return this.length;
            }
            set
            {
                this.length = value;
            }
        }

        /// <remarks/>
        public LenghtValue Width
        {
            get
            {
                return this.width;
            }
            set
            {
                this.width = value;
            }
        }

        /// <remarks/>
        public LenghtValue Height
        {
            get
            {
                return this.height;
            }
            set
            {
                this.height = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class AMSDataType
    {

        private ScheduleBType harmonizedTariff;

        private VehicleDataType vehicle;

        private Country country;

        private string typeOfService;

        private CurrencyType currency;

        private MoneyValue value;

        /// <remarks/>
        public ScheduleBType HarmonizedTariff
        {
            get
            {
                return this.harmonizedTariff;
            }
            set
            {
                this.harmonizedTariff = value;
            }
        }

        /// <remarks/>
        public VehicleDataType Vehicle
        {
            get
            {
                return this.vehicle;
            }
            set
            {
                this.vehicle = value;
            }
        }

        /// <remarks/>
        public Country Country
        {
            get
            {
                return this.country;
            }
            set
            {
                this.country = value;
            }
        }

        /// <remarks/>
        public string TypeOfService
        {
            get
            {
                return this.typeOfService;
            }
            set
            {
                this.typeOfService = value;
            }
        }

        /// <remarks/>
        public CurrencyType Currency
        {
            get
            {
                return this.currency;
            }
            set
            {
                this.currency = value;
            }
        }

        /// <remarks/>
        public MoneyValue Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class ContainerType
    {

        private string ventilationSetup;

        private bool generatorSetup;

        private bool generatorSetupSpecified;

        private TemperatureStringValue temperatureSetup;

        private bool isNonOperatingReefer;

        private bool isNonOperatingReeferSpecified;

        /// <remarks/>
        public string VentilationSetup
        {
            get
            {
                return this.ventilationSetup;
            }
            set
            {
                this.ventilationSetup = value;
            }
        }

        /// <remarks/>
        public bool GeneratorSetup
        {
            get
            {
                return this.generatorSetup;
            }
            set
            {
                this.generatorSetup = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool GeneratorSetupSpecified
        {
            get
            {
                return this.generatorSetupSpecified;
            }
            set
            {
                this.generatorSetupSpecified = value;
            }
        }

        /// <remarks/>
        public TemperatureStringValue TemperatureSetup
        {
            get
            {
                return this.temperatureSetup;
            }
            set
            {
                this.temperatureSetup = value;
            }
        }

        /// <remarks/>
        public bool IsNonOperatingReefer
        {
            get
            {
                return this.isNonOperatingReefer;
            }
            set
            {
                this.isNonOperatingReefer = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsNonOperatingReeferSpecified
        {
            get
            {
                return this.isNonOperatingReeferSpecified;
            }
            set
            {
                this.isNonOperatingReeferSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class TemperatureStringValue
    {

        private TemperatureUnitType unit;

        private bool unitSpecified;

        private string value;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public TemperatureUnitType Unit
        {
            get
            {
                return this.unit;
            }
            set
            {
                this.unit = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool UnitSpecified
        {
            get
            {
                return this.unitSpecified;
            }
            set
            {
                this.unitSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum TemperatureUnitType
    {

        /// <remarks/>
        C,

        /// <remarks/>
        F,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class Charge
    {

        private ChargeDesc type;

        private ChargeDefinition chargeDefinition;

        private Entity entity;

        private double quantity;

        private MoneyValue price;

        private MoneyValue amount;

        private MoneyValue taxAmount;

        private MoneyValue retentionAmount;

        private CurrencyType homeCurrency;

        private CurrencyType currency;

        private double exchangeRate;

        private MoneyValue priceInCurrency;

        private MoneyValue amountInCurrency;

        private MoneyValue taxAmountInCurrency;

        private MoneyValue retentionAmountInCurrency;

        private bool isPrepaid;

        private bool isPrepaidSpecified;

        private bool isThirdPartyCharge;

        private bool isThirdPartyChargeSpecified;

        private ChargeStatusType status;

        private bool statusSpecified;

        private string description;

        private string notes;

        private string units;

        private TaxDefinition taxDefinition;

        private bool showInDocuments;

        private bool showInDocumentsSpecified;

        private bool isCredit;

        private bool isCreditSpecified;

        private GUIDItem liquidatedAt;

        private GUIDItem createdAt;

        private GUIDItem parent;

        private GUIDItem salesOrder;

        private GUIDItem purchaseOrder;

        private FreightCharge freightChargeInfo;

        private CustomType[] customs;

        private bool isFromSegment;

        private bool isFromSegmentSpecified;

        private ModeOfTransportation modeOfTransportation;

        private Port pointOfOrigin;

        private Port pointOfDestination;

        /// <remarks/>
        public ChargeDesc Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }

        /// <remarks/>
        public ChargeDefinition ChargeDefinition
        {
            get
            {
                return this.chargeDefinition;
            }
            set
            {
                this.chargeDefinition = value;
            }
        }

        /// <remarks/>
        public Entity Entity
        {
            get
            {
                return this.entity;
            }
            set
            {
                this.entity = value;
            }
        }

        /// <remarks/>
        public double Quantity
        {
            get
            {
                return this.quantity;
            }
            set
            {
                this.quantity = value;
            }
        }

        /// <remarks/>
        public MoneyValue Price
        {
            get
            {
                return this.price;
            }
            set
            {
                this.price = value;
            }
        }

        /// <remarks/>
        public MoneyValue Amount
        {
            get
            {
                return this.amount;
            }
            set
            {
                this.amount = value;
            }
        }

        /// <remarks/>
        public MoneyValue TaxAmount
        {
            get
            {
                return this.taxAmount;
            }
            set
            {
                this.taxAmount = value;
            }
        }

        /// <remarks/>
        public MoneyValue RetentionAmount
        {
            get
            {
                return this.retentionAmount;
            }
            set
            {
                this.retentionAmount = value;
            }
        }

        /// <remarks/>
        public CurrencyType HomeCurrency
        {
            get
            {
                return this.homeCurrency;
            }
            set
            {
                this.homeCurrency = value;
            }
        }

        /// <remarks/>
        public CurrencyType Currency
        {
            get
            {
                return this.currency;
            }
            set
            {
                this.currency = value;
            }
        }

        /// <remarks/>
        public double ExchangeRate
        {
            get
            {
                return this.exchangeRate;
            }
            set
            {
                this.exchangeRate = value;
            }
        }

        /// <remarks/>
        public MoneyValue PriceInCurrency
        {
            get
            {
                return this.priceInCurrency;
            }
            set
            {
                this.priceInCurrency = value;
            }
        }

        /// <remarks/>
        public MoneyValue AmountInCurrency
        {
            get
            {
                return this.amountInCurrency;
            }
            set
            {
                this.amountInCurrency = value;
            }
        }

        /// <remarks/>
        public MoneyValue TaxAmountInCurrency
        {
            get
            {
                return this.taxAmountInCurrency;
            }
            set
            {
                this.taxAmountInCurrency = value;
            }
        }

        /// <remarks/>
        public MoneyValue RetentionAmountInCurrency
        {
            get
            {
                return this.retentionAmountInCurrency;
            }
            set
            {
                this.retentionAmountInCurrency = value;
            }
        }

        /// <remarks/>
        public bool IsPrepaid
        {
            get
            {
                return this.isPrepaid;
            }
            set
            {
                this.isPrepaid = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsPrepaidSpecified
        {
            get
            {
                return this.isPrepaidSpecified;
            }
            set
            {
                this.isPrepaidSpecified = value;
            }
        }

        /// <remarks/>
        public bool IsThirdPartyCharge
        {
            get
            {
                return this.isThirdPartyCharge;
            }
            set
            {
                this.isThirdPartyCharge = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsThirdPartyChargeSpecified
        {
            get
            {
                return this.isThirdPartyChargeSpecified;
            }
            set
            {
                this.isThirdPartyChargeSpecified = value;
            }
        }

        /// <remarks/>
        public ChargeStatusType Status
        {
            get
            {
                return this.status;
            }
            set
            {
                this.status = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool StatusSpecified
        {
            get
            {
                return this.statusSpecified;
            }
            set
            {
                this.statusSpecified = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        /// <remarks/>
        public string Notes
        {
            get
            {
                return this.notes;
            }
            set
            {
                this.notes = value;
            }
        }

        /// <remarks/>
        public string Units
        {
            get
            {
                return this.units;
            }
            set
            {
                this.units = value;
            }
        }

        /// <remarks/>
        public TaxDefinition TaxDefinition
        {
            get
            {
                return this.taxDefinition;
            }
            set
            {
                this.taxDefinition = value;
            }
        }

        /// <remarks/>
        public bool ShowInDocuments
        {
            get
            {
                return this.showInDocuments;
            }
            set
            {
                this.showInDocuments = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ShowInDocumentsSpecified
        {
            get
            {
                return this.showInDocumentsSpecified;
            }
            set
            {
                this.showInDocumentsSpecified = value;
            }
        }

        /// <remarks/>
        public bool IsCredit
        {
            get
            {
                return this.isCredit;
            }
            set
            {
                this.isCredit = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsCreditSpecified
        {
            get
            {
                return this.isCreditSpecified;
            }
            set
            {
                this.isCreditSpecified = value;
            }
        }

        /// <remarks/>
        public GUIDItem LiquidatedAt
        {
            get
            {
                return this.liquidatedAt;
            }
            set
            {
                this.liquidatedAt = value;
            }
        }

        /// <remarks/>
        public GUIDItem CreatedAt
        {
            get
            {
                return this.createdAt;
            }
            set
            {
                this.createdAt = value;
            }
        }

        /// <remarks/>
        public GUIDItem Parent
        {
            get
            {
                return this.parent;
            }
            set
            {
                this.parent = value;
            }
        }

        /// <remarks/>
        public GUIDItem SalesOrder
        {
            get
            {
                return this.salesOrder;
            }
            set
            {
                this.salesOrder = value;
            }
        }

        /// <remarks/>
        public GUIDItem PurchaseOrder
        {
            get
            {
                return this.purchaseOrder;
            }
            set
            {
                this.purchaseOrder = value;
            }
        }

        /// <remarks/>
        public FreightCharge FreightChargeInfo
        {
            get
            {
                return this.freightChargeInfo;
            }
            set
            {
                this.freightChargeInfo = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Custom", IsNullable = false)]
        public CustomType[] Customs
        {
            get
            {
                return this.customs;
            }
            set
            {
                this.customs = value;
            }
        }

        /// <remarks/>
        public bool IsFromSegment
        {
            get
            {
                return this.isFromSegment;
            }
            set
            {
                this.isFromSegment = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsFromSegmentSpecified
        {
            get
            {
                return this.isFromSegmentSpecified;
            }
            set
            {
                this.isFromSegmentSpecified = value;
            }
        }

        /// <remarks/>
        public ModeOfTransportation ModeOfTransportation
        {
            get
            {
                return this.modeOfTransportation;
            }
            set
            {
                this.modeOfTransportation = value;
            }
        }

        /// <remarks/>
        public Port PointOfOrigin
        {
            get
            {
                return this.pointOfOrigin;
            }
            set
            {
                this.pointOfOrigin = value;
            }
        }

        /// <remarks/>
        public Port PointOfDestination
        {
            get
            {
                return this.pointOfDestination;
            }
            set
            {
                this.pointOfDestination = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum ChargeDesc
    {

        /// <remarks/>
        Standard,

        /// <remarks/>
        Freight,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum ChargeStatusType
    {

        /// <remarks/>
        Open,

        /// <remarks/>
        Posted,

        /// <remarks/>
        Paid,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class GUIDItem
    {

        private string gUID;

        private TransactionType type;

        private string number;

        /// <remarks/>
        public string GUID
        {
            get
            {
                return this.gUID;
            }
            set
            {
                this.gUID = value;
            }
        }

        /// <remarks/>
        public TransactionType Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }

        /// <remarks/>
        public string Number
        {
            get
            {
                return this.number;
            }
            set
            {
                this.number = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum TransactionType
    {

        /// <remarks/>
        PickupOrder,

        /// <remarks/>
        WarehouseReceipt,

        /// <remarks/>
        CargoRelease,

        /// <remarks/>
        AirShipment,

        /// <remarks/>
        OceanShipment,

        /// <remarks/>
        GroundShipment,

        /// <remarks/>
        Invoice,

        /// <remarks/>
        CreditMemo,

        /// <remarks/>
        Bill,

        /// <remarks/>
        BillCredit,

        /// <remarks/>
        Quotation,

        /// <remarks/>
        AirBooking,

        /// <remarks/>
        OceanBooking,

        /// <remarks/>
        GroundBooking,

        /// <remarks/>
        AirTrip,

        /// <remarks/>
        OceanTrip,

        /// <remarks/>
        GroundTrip,

        /// <remarks/>
        Job,

        /// <remarks/>
        PurchaseOrder,

        /// <remarks/>
        SalesOrder,

        /// <remarks/>
        CargoTask,

        /// <remarks/>
        Payment,

        /// <remarks/>
        RefundPayment,

        /// <remarks/>
        Deposit,

        /// <remarks/>
        Check,

        /// <remarks/>
        JournalEntry,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class FreightCharge
    {

        private sbyte version;

        private bool versionSpecified;

        private double pieces;

        private WeightValue weight;

        private VolumeValue volume;

        private WeightValue chargeableWeight;

        private ChargeFlagsType flags;

        private ApplyByType applyBy;

        private MethodType method;

        private MeasurementUnits measurementUnits;

        private bool useGrossWeight;

        private bool useGrossWeightSpecified;

        /// <remarks/>
        public sbyte Version
        {
            get
            {
                return this.version;
            }
            set
            {
                this.version = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool VersionSpecified
        {
            get
            {
                return this.versionSpecified;
            }
            set
            {
                this.versionSpecified = value;
            }
        }

        /// <remarks/>
        public double Pieces
        {
            get
            {
                return this.pieces;
            }
            set
            {
                this.pieces = value;
            }
        }

        /// <remarks/>
        public WeightValue Weight
        {
            get
            {
                return this.weight;
            }
            set
            {
                this.weight = value;
            }
        }

        /// <remarks/>
        public VolumeValue Volume
        {
            get
            {
                return this.volume;
            }
            set
            {
                this.volume = value;
            }
        }

        /// <remarks/>
        public WeightValue ChargeableWeight
        {
            get
            {
                return this.chargeableWeight;
            }
            set
            {
                this.chargeableWeight = value;
            }
        }

        /// <remarks/>
        public ChargeFlagsType Flags
        {
            get
            {
                return this.flags;
            }
            set
            {
                this.flags = value;
            }
        }

        /// <remarks/>
        public ApplyByType ApplyBy
        {
            get
            {
                return this.applyBy;
            }
            set
            {
                this.applyBy = value;
            }
        }

        /// <remarks/>
        public MethodType Method
        {
            get
            {
                return this.method;
            }
            set
            {
                this.method = value;
            }
        }

        /// <remarks/>
        public MeasurementUnits MeasurementUnits
        {
            get
            {
                return this.measurementUnits;
            }
            set
            {
                this.measurementUnits = value;
            }
        }

        /// <remarks/>
        public bool UseGrossWeight
        {
            get
            {
                return this.useGrossWeight;
            }
            set
            {
                this.useGrossWeight = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool UseGrossWeightSpecified
        {
            get
            {
                return this.useGrossWeightSpecified;
            }
            set
            {
                this.useGrossWeightSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum ChargeFlagsType
    {

        /// <remarks/>
        Rate,

        /// <remarks/>
        Minimum,

        /// <remarks/>
        Maximum,

        /// <remarks/>
        Formula,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum ApplyByType
    {

        /// <remarks/>
        Pieces,

        /// <remarks/>
        Weight,

        /// <remarks/>
        Volume,

        /// <remarks/>
        Package,

        /// <remarks/>
        Formula,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("MeasurementUnits", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class MeasurementUnits
    {
        [XmlElement("lengthUnit", IsNullable = false)]
        private LengthUnitType lengthUnit;

        private bool lengthUnitSpecified;

        [XmlElement("volumeUnit")]
        private VolumeUnitType volumeUnit;

        private bool volumeUnitSpecified;

        private WeightUnitType weightUnit;

        private bool weightUnitSpecified;

        private VolumeWeightUnitType volumeWeightUnit;

        private bool volumeWeightUnitSpecified;

        private AreaUnitType areaUnit;

        private bool areaUnitSpecified;

        private sbyte lengthPrecision;

        private bool lengthPrecisionSpecified;

        private sbyte volumePrecision;

        private bool volumePrecisionSpecified;

        private sbyte weightPrecision;

        private bool weightPrecisionSpecified;

        private sbyte volumeWeightPrecision;

        private bool volumeWeightPrecisionSpecified;

        private sbyte areaPrecision;

        private bool areaPrecisionSpecified;

        private double volumeWeightFactor;

        private bool volumeWeightFactorSpecified;

        /// <remarks/>
        public LengthUnitType LengthUnit
        {
            get
            {
                return this.lengthUnit;
            }
            set
            {
                this.lengthUnit = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool LengthUnitSpecified
        {
            get
            {
                return this.lengthUnitSpecified;
            }
            set
            {
                this.lengthUnitSpecified = value;
            }
        }

        /// <remarks/>
        public VolumeUnitType VolumeUnit
        {
            get
            {
                return this.volumeUnit;
            }
            set
            {
                this.volumeUnit = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool VolumeUnitSpecified
        {
            get
            {
                return this.volumeUnitSpecified;
            }
            set
            {
                this.volumeUnitSpecified = value;
            }
        }

        /// <remarks/>
        public WeightUnitType WeightUnit
        {
            get
            {
                return this.weightUnit;
            }
            set
            {
                this.weightUnit = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool WeightUnitSpecified
        {
            get
            {
                return this.weightUnitSpecified;
            }
            set
            {
                this.weightUnitSpecified = value;
            }
        }

        /// <remarks/>
        public VolumeWeightUnitType VolumeWeightUnit
        {
            get
            {
                return this.volumeWeightUnit;
            }
            set
            {
                this.volumeWeightUnit = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool VolumeWeightUnitSpecified
        {
            get
            {
                return this.volumeWeightUnitSpecified;
            }
            set
            {
                this.volumeWeightUnitSpecified = value;
            }
        }

        /// <remarks/>
        public AreaUnitType AreaUnit
        {
            get
            {
                return this.areaUnit;
            }
            set
            {
                this.areaUnit = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AreaUnitSpecified
        {
            get
            {
                return this.areaUnitSpecified;
            }
            set
            {
                this.areaUnitSpecified = value;
            }
        }

        /// <remarks/>
        public sbyte LengthPrecision
        {
            get
            {
                return this.lengthPrecision;
            }
            set
            {
                this.lengthPrecision = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool LengthPrecisionSpecified
        {
            get
            {
                return this.lengthPrecisionSpecified;
            }
            set
            {
                this.lengthPrecisionSpecified = value;
            }
        }

        /// <remarks/>
        public sbyte VolumePrecision
        {
            get
            {
                return this.volumePrecision;
            }
            set
            {
                this.volumePrecision = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool VolumePrecisionSpecified
        {
            get
            {
                return this.volumePrecisionSpecified;
            }
            set
            {
                this.volumePrecisionSpecified = value;
            }
        }

        /// <remarks/>
        public sbyte WeightPrecision
        {
            get
            {
                return this.weightPrecision;
            }
            set
            {
                this.weightPrecision = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool WeightPrecisionSpecified
        {
            get
            {
                return this.weightPrecisionSpecified;
            }
            set
            {
                this.weightPrecisionSpecified = value;
            }
        }

        /// <remarks/>
        public sbyte VolumeWeightPrecision
        {
            get
            {
                return this.volumeWeightPrecision;
            }
            set
            {
                this.volumeWeightPrecision = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool VolumeWeightPrecisionSpecified
        {
            get
            {
                return this.volumeWeightPrecisionSpecified;
            }
            set
            {
                this.volumeWeightPrecisionSpecified = value;
            }
        }

        /// <remarks/>
        public sbyte AreaPrecision
        {
            get
            {
                return this.areaPrecision;
            }
            set
            {
                this.areaPrecision = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AreaPrecisionSpecified
        {
            get
            {
                return this.areaPrecisionSpecified;
            }
            set
            {
                this.areaPrecisionSpecified = value;
            }
        }

        /// <remarks/>
        public double VolumeWeightFactor
        {
            get
            {
                return this.volumeWeightFactor;
            }
            set
            {
                this.volumeWeightFactor = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool VolumeWeightFactorSpecified
        {
            get
            {
                return this.volumeWeightFactorSpecified;
            }
            set
            {
                this.volumeWeightFactorSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum AreaUnitType
    {

        /// <remarks/>
        m2,

        /// <remarks/>
        in2,

        /// <remarks/>
        ft2,

        /// <remarks/>
        km2,

        /// <remarks/>
        cm2,

        /// <remarks/>
        dm2,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("Port", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class Port
    {

        private Country country;

        private MethodType[] method;

        private string name;

        private string subdivision;

        private string remarks;

        private CustomType[] customs;

        private string code;

        /// <remarks/>
        public Country Country
        {
            get
            {
                return this.country;
            }
            set
            {
                this.country = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Method")]
        public MethodType[] Method
        {
            get
            {
                return this.method;
            }
            set
            {
                this.method = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        /// <remarks/>
        public string Subdivision
        {
            get
            {
                return this.subdivision;
            }
            set
            {
                this.subdivision = value;
            }
        }

        /// <remarks/>
        public string Remarks
        {
            get
            {
                return this.remarks;
            }
            set
            {
                this.remarks = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Custom", IsNullable = false)]
        public CustomType[] Customs
        {
            get
            {
                return this.customs;
            }
            set
            {
                this.customs = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Code
        {
            get
            {
                return this.code;
            }
            set
            {
                this.code = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class HoldStatusType
    {

        private bool isOnHold;

        private string holdReason;

        /// <remarks/>
        public bool IsOnHold
        {
            get
            {
                return this.isOnHold;
            }
            set
            {
                this.isOnHold = value;
            }
        }

        /// <remarks/>
        public string HoldReason
        {
            get
            {
                return this.holdReason;
            }
            set
            {
                this.holdReason = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class VGMDataType
    {

        private VerificationMethodType verificationMethod;

        private bool verificationMethodSpecified;

        private WeightValue vGM;

        private WeightValue containerTareWeight;

        private WeightValue totalCargoWeight;

        private System.DateTime verificationDate;

        private bool verificationDateSpecified;

        private Entity verifyingParty;

        private string verificationSignature;

        private string phoneNumber;

        /// <remarks/>
        public VerificationMethodType VerificationMethod
        {
            get
            {
                return this.verificationMethod;
            }
            set
            {
                this.verificationMethod = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool VerificationMethodSpecified
        {
            get
            {
                return this.verificationMethodSpecified;
            }
            set
            {
                this.verificationMethodSpecified = value;
            }
        }

        /// <remarks/>
        public WeightValue VGM
        {
            get
            {
                return this.vGM;
            }
            set
            {
                this.vGM = value;
            }
        }

        /// <remarks/>
        public WeightValue ContainerTareWeight
        {
            get
            {
                return this.containerTareWeight;
            }
            set
            {
                this.containerTareWeight = value;
            }
        }

        /// <remarks/>
        public WeightValue TotalCargoWeight
        {
            get
            {
                return this.totalCargoWeight;
            }
            set
            {
                this.totalCargoWeight = value;
            }
        }

        /// <remarks/>
        public System.DateTime VerificationDate
        {
            get
            {
                return this.verificationDate;
            }
            set
            {
                this.verificationDate = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool VerificationDateSpecified
        {
            get
            {
                return this.verificationDateSpecified;
            }
            set
            {
                this.verificationDateSpecified = value;
            }
        }

        /// <remarks/>
        public Entity VerifyingParty
        {
            get
            {
                return this.verifyingParty;
            }
            set
            {
                this.verifyingParty = value;
            }
        }

        /// <remarks/>
        public string VerificationSignature
        {
            get
            {
                return this.verificationSignature;
            }
            set
            {
                this.verificationSignature = value;
            }
        }

        /// <remarks/>
        public string PhoneNumber
        {
            get
            {
                return this.phoneNumber;
            }
            set
            {
                this.phoneNumber = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum VerificationMethodType
    {

        /// <remarks/>
        MethodOne,

        /// <remarks/>
        MethodTwo,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("Charges", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class ChargeList
    {

        private Charge[] charge;

        private bool useSequenceOrder;

        private bool useSequenceOrderSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Charge")]
        public Charge[] Charge
        {
            get
            {
                return this.charge;
            }
            set
            {
                this.charge = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool UseSequenceOrder
        {
            get
            {
                return this.useSequenceOrder;
            }
            set
            {
                this.useSequenceOrder = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool UseSequenceOrderSpecified
        {
            get
            {
                return this.useSequenceOrderSpecified;
            }
            set
            {
                this.useSequenceOrderSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum ShipmentStatusType
    {

        /// <remarks/>
        WaitingForInstructions,

        /// <remarks/>
        Loading,

        /// <remarks/>
        Loaded,

        /// <remarks/>
        InTransit,

        /// <remarks/>
        Received,

        /// <remarks/>
        Delivered,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum ShipmentDirectionType
    {

        /// <remarks/>
        Outgoing,

        /// <remarks/>
        Incoming,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum ShipmentLayoutType
    {

        /// <remarks/>
        Master,

        /// <remarks/>
        House,

        /// <remarks/>
        Straight,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class InBondDataType
    {

        private bool bTDFDAIndicator;

        private bool bTDFDAIndicatorSpecified;

        private InbondEntryTypeDesc inbondEntryType;

        private string conventionalInbondNumber;

        private string inbondCarrierSCACCode;

        private string uSDestinationPort;

        private string foreignDestinationPort;

        private string foreignPlaceOfDelivery;

        private MoneyValue value;

        private string bondedCarrierIDNumber;

        private string paperlessInbondNumber;

        private string uSPortOfInbond1;

        private string uSPortOfInbond2;

        private string uSPortOfInbond3;

        private string uSPortOfInbond4;

        private string uSPortOfInbond5;

        /// <remarks/>
        public bool BTDFDAIndicator
        {
            get
            {
                return this.bTDFDAIndicator;
            }
            set
            {
                this.bTDFDAIndicator = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool BTDFDAIndicatorSpecified
        {
            get
            {
                return this.bTDFDAIndicatorSpecified;
            }
            set
            {
                this.bTDFDAIndicatorSpecified = value;
            }
        }

        /// <remarks/>
        public InbondEntryTypeDesc InbondEntryType
        {
            get
            {
                return this.inbondEntryType;
            }
            set
            {
                this.inbondEntryType = value;
            }
        }

        /// <remarks/>
        public string ConventionalInbondNumber
        {
            get
            {
                return this.conventionalInbondNumber;
            }
            set
            {
                this.conventionalInbondNumber = value;
            }
        }

        /// <remarks/>
        public string InbondCarrierSCACCode
        {
            get
            {
                return this.inbondCarrierSCACCode;
            }
            set
            {
                this.inbondCarrierSCACCode = value;
            }
        }

        /// <remarks/>
        public string USDestinationPort
        {
            get
            {
                return this.uSDestinationPort;
            }
            set
            {
                this.uSDestinationPort = value;
            }
        }

        /// <remarks/>
        public string ForeignDestinationPort
        {
            get
            {
                return this.foreignDestinationPort;
            }
            set
            {
                this.foreignDestinationPort = value;
            }
        }

        /// <remarks/>
        public string ForeignPlaceOfDelivery
        {
            get
            {
                return this.foreignPlaceOfDelivery;
            }
            set
            {
                this.foreignPlaceOfDelivery = value;
            }
        }

        /// <remarks/>
        public MoneyValue Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }

        /// <remarks/>
        public string BondedCarrierIDNumber
        {
            get
            {
                return this.bondedCarrierIDNumber;
            }
            set
            {
                this.bondedCarrierIDNumber = value;
            }
        }

        /// <remarks/>
        public string PaperlessInbondNumber
        {
            get
            {
                return this.paperlessInbondNumber;
            }
            set
            {
                this.paperlessInbondNumber = value;
            }
        }

        /// <remarks/>
        public string USPortOfInbond1
        {
            get
            {
                return this.uSPortOfInbond1;
            }
            set
            {
                this.uSPortOfInbond1 = value;
            }
        }

        /// <remarks/>
        public string USPortOfInbond2
        {
            get
            {
                return this.uSPortOfInbond2;
            }
            set
            {
                this.uSPortOfInbond2 = value;
            }
        }

        /// <remarks/>
        public string USPortOfInbond3
        {
            get
            {
                return this.uSPortOfInbond3;
            }
            set
            {
                this.uSPortOfInbond3 = value;
            }
        }

        /// <remarks/>
        public string USPortOfInbond4
        {
            get
            {
                return this.uSPortOfInbond4;
            }
            set
            {
                this.uSPortOfInbond4 = value;
            }
        }

        /// <remarks/>
        public string USPortOfInbond5
        {
            get
            {
                return this.uSPortOfInbond5;
            }
            set
            {
                this.uSPortOfInbond5 = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum InbondEntryTypeDesc
    {

        /// <remarks/>
        ImmediateTransportation,

        /// <remarks/>
        TransportationAndExportation,

        /// <remarks/>
        ImmediateExportation,

        /// <remarks/>
        Transit,

        /// <remarks/>
        MultiTransit,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("PODData", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class PODDataType
    {

        private System.DateTime deliveryDate;

        private Entity receivedBy;

        private string receivedByName;

        private string comments;

        private byte[] signatureImage;

        private string signatureURL;

        private string ownerType;

        private string ownerGUID;

        /// <remarks/>
        public System.DateTime DeliveryDate
        {
            get
            {
                return this.deliveryDate;
            }
            set
            {
                this.deliveryDate = value;
            }
        }

        /// <remarks/>
        public Entity ReceivedBy
        {
            get
            {
                return this.receivedBy;
            }
            set
            {
                this.receivedBy = value;
            }
        }

        /// <remarks/>
        public string ReceivedByName
        {
            get
            {
                return this.receivedByName;
            }
            set
            {
                this.receivedByName = value;
            }
        }

        /// <remarks/>
        public string Comments
        {
            get
            {
                return this.comments;
            }
            set
            {
                this.comments = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "base64Binary")]
        public byte[] SignatureImage
        {
            get
            {
                return this.signatureImage;
            }
            set
            {
                this.signatureImage = value;
            }
        }

        /// <remarks/>
        public string SignatureURL
        {
            get
            {
                return this.signatureURL;
            }
            set
            {
                this.signatureURL = value;
            }
        }

        /// <remarks/>
        public string OwnerType
        {
            get
            {
                return this.ownerType;
            }
            set
            {
                this.ownerType = value;
            }
        }

        /// <remarks/>
        public string OwnerGUID
        {
            get
            {
                return this.ownerGUID;
            }
            set
            {
                this.ownerGUID = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("Shipments", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class ShipmentList
    {

        private MagayaShipment[] items;

        private ItemsChoiceType1[] itemsElementName;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("AirShipment", typeof(MagayaShipment))]
        [System.Xml.Serialization.XmlElementAttribute("GroundShipment", typeof(MagayaShipment))]
        [System.Xml.Serialization.XmlElementAttribute("OceanShipment", typeof(MagayaShipment))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
        public MagayaShipment[] Items
        {
            get
            {
                return this.items;
            }
            set
            {
                this.items = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ItemsElementName")]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemsChoiceType1[] ItemsElementName
        {
            get
            {
                return this.itemsElementName;
            }
            set
            {
                this.itemsElementName = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1", IncludeInSchema = false)]
    public enum ItemsChoiceType1
    {

        /// <remarks/>
        AirShipment,

        /// <remarks/>
        GroundShipment,

        /// <remarks/>
        OceanShipment,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class DocumentType
    {

        private string name;

        private string extension;

        private byte[] data;

        private bool isMagayaDoc;

        private bool isMagayaDocSpecified;

        private bool isOLEDoc;

        private bool isOLEDocSpecified;

        private long identifier;

        private bool identifierSpecified;

        private string ownerType;

        private string ownerGUID;

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        /// <remarks/>
        public string Extension
        {
            get
            {
                return this.extension;
            }
            set
            {
                this.extension = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "base64Binary")]
        public byte[] Data
        {
            get
            {
                return this.data;
            }
            set
            {
                this.data = value;
            }
        }

        /// <remarks/>
        public bool IsMagayaDoc
        {
            get
            {
                return this.isMagayaDoc;
            }
            set
            {
                this.isMagayaDoc = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsMagayaDocSpecified
        {
            get
            {
                return this.isMagayaDocSpecified;
            }
            set
            {
                this.isMagayaDocSpecified = value;
            }
        }

        /// <remarks/>
        public bool IsOLEDoc
        {
            get
            {
                return this.isOLEDoc;
            }
            set
            {
                this.isOLEDoc = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsOLEDocSpecified
        {
            get
            {
                return this.isOLEDocSpecified;
            }
            set
            {
                this.isOLEDocSpecified = value;
            }
        }

        /// <remarks/>
        public long Identifier
        {
            get
            {
                return this.identifier;
            }
            set
            {
                this.identifier = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IdentifierSpecified
        {
            get
            {
                return this.identifierSpecified;
            }
            set
            {
                this.identifierSpecified = value;
            }
        }

        /// <remarks/>
        public string OwnerType
        {
            get
            {
                return this.ownerType;
            }
            set
            {
                this.ownerType = value;
            }
        }

        /// <remarks/>
        public string OwnerGUID
        {
            get
            {
                return this.ownerGUID;
            }
            set
            {
                this.ownerGUID = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum BookingStatusType
    {

        /// <remarks/>
        Open,

        /// <remarks/>
        Closed,

        /// <remarks/>
        Canceled,

        /// <remarks/>
        InProcess,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class ShipmentOptionsType
    {

        private bool isShipperConsigneeRelated;

        private bool isShipperConsigneeRelatedSpecified;

        private bool isRoutedTransaction;

        private bool isRoutedTransactionSpecified;

        private bool isAirWayBillDetailExpanded;

        private bool isAirWayBillDetailExpandedSpecified;

        private bool isBillOfLadingDetailExpanded;

        private bool isBillOfLadingDetailExpandedSpecified;

        private bool isCargoManifestDetailExpanded;

        private bool isCargoManifestDetailExpandedSpecified;

        private bool fillDocsWithAccountingInfo;

        private bool fillDocsWithAccountingInfoSpecified;

        private bool showContainerDetails;

        private bool showContainerDetailsSpecified;

        private bool showWHRsInCargoManifest;

        private bool showWHRsInCargoManifestSpecified;

        private bool showDimensionsInAWB;

        private bool showDimensionsInAWBSpecified;

        private bool showDimensionsDetailedInAWB;

        private bool showDimensionsDetailedInAWBSpecified;

        private AWBUnitType airWayBillWeightUnit;

        private bool airWayBillWeightUnitSpecified;

        private AWBUnitType billOfLadingWeightUnit;

        private bool billOfLadingWeightUnitSpecified;

        private bool useNotesAsMarksNumbersInBL;

        private bool useNotesAsMarksNumbersInBLSpecified;

        private bool billOfLadingGroupSimilarItems;

        private bool billOfLadingGroupSimilarItemsSpecified;

        /// <remarks/>
        public bool IsShipperConsigneeRelated
        {
            get
            {
                return this.isShipperConsigneeRelated;
            }
            set
            {
                this.isShipperConsigneeRelated = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsShipperConsigneeRelatedSpecified
        {
            get
            {
                return this.isShipperConsigneeRelatedSpecified;
            }
            set
            {
                this.isShipperConsigneeRelatedSpecified = value;
            }
        }

        /// <remarks/>
        public bool IsRoutedTransaction
        {
            get
            {
                return this.isRoutedTransaction;
            }
            set
            {
                this.isRoutedTransaction = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsRoutedTransactionSpecified
        {
            get
            {
                return this.isRoutedTransactionSpecified;
            }
            set
            {
                this.isRoutedTransactionSpecified = value;
            }
        }

        /// <remarks/>
        public bool IsAirWayBillDetailExpanded
        {
            get
            {
                return this.isAirWayBillDetailExpanded;
            }
            set
            {
                this.isAirWayBillDetailExpanded = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsAirWayBillDetailExpandedSpecified
        {
            get
            {
                return this.isAirWayBillDetailExpandedSpecified;
            }
            set
            {
                this.isAirWayBillDetailExpandedSpecified = value;
            }
        }

        /// <remarks/>
        public bool IsBillOfLadingDetailExpanded
        {
            get
            {
                return this.isBillOfLadingDetailExpanded;
            }
            set
            {
                this.isBillOfLadingDetailExpanded = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsBillOfLadingDetailExpandedSpecified
        {
            get
            {
                return this.isBillOfLadingDetailExpandedSpecified;
            }
            set
            {
                this.isBillOfLadingDetailExpandedSpecified = value;
            }
        }

        /// <remarks/>
        public bool IsCargoManifestDetailExpanded
        {
            get
            {
                return this.isCargoManifestDetailExpanded;
            }
            set
            {
                this.isCargoManifestDetailExpanded = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsCargoManifestDetailExpandedSpecified
        {
            get
            {
                return this.isCargoManifestDetailExpandedSpecified;
            }
            set
            {
                this.isCargoManifestDetailExpandedSpecified = value;
            }
        }

        /// <remarks/>
        public bool FillDocsWithAccountingInfo
        {
            get
            {
                return this.fillDocsWithAccountingInfo;
            }
            set
            {
                this.fillDocsWithAccountingInfo = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool FillDocsWithAccountingInfoSpecified
        {
            get
            {
                return this.fillDocsWithAccountingInfoSpecified;
            }
            set
            {
                this.fillDocsWithAccountingInfoSpecified = value;
            }
        }

        /// <remarks/>
        public bool ShowContainerDetails
        {
            get
            {
                return this.showContainerDetails;
            }
            set
            {
                this.showContainerDetails = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ShowContainerDetailsSpecified
        {
            get
            {
                return this.showContainerDetailsSpecified;
            }
            set
            {
                this.showContainerDetailsSpecified = value;
            }
        }

        /// <remarks/>
        public bool ShowWHRsInCargoManifest
        {
            get
            {
                return this.showWHRsInCargoManifest;
            }
            set
            {
                this.showWHRsInCargoManifest = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ShowWHRsInCargoManifestSpecified
        {
            get
            {
                return this.showWHRsInCargoManifestSpecified;
            }
            set
            {
                this.showWHRsInCargoManifestSpecified = value;
            }
        }

        /// <remarks/>
        public bool ShowDimensionsInAWB
        {
            get
            {
                return this.showDimensionsInAWB;
            }
            set
            {
                this.showDimensionsInAWB = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ShowDimensionsInAWBSpecified
        {
            get
            {
                return this.showDimensionsInAWBSpecified;
            }
            set
            {
                this.showDimensionsInAWBSpecified = value;
            }
        }

        /// <remarks/>
        public bool ShowDimensionsDetailedInAWB
        {
            get
            {
                return this.showDimensionsDetailedInAWB;
            }
            set
            {
                this.showDimensionsDetailedInAWB = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ShowDimensionsDetailedInAWBSpecified
        {
            get
            {
                return this.showDimensionsDetailedInAWBSpecified;
            }
            set
            {
                this.showDimensionsDetailedInAWBSpecified = value;
            }
        }

        /// <remarks/>
        public AWBUnitType AirWayBillWeightUnit
        {
            get
            {
                return this.airWayBillWeightUnit;
            }
            set
            {
                this.airWayBillWeightUnit = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AirWayBillWeightUnitSpecified
        {
            get
            {
                return this.airWayBillWeightUnitSpecified;
            }
            set
            {
                this.airWayBillWeightUnitSpecified = value;
            }
        }

        /// <remarks/>
        public AWBUnitType BillOfLadingWeightUnit
        {
            get
            {
                return this.billOfLadingWeightUnit;
            }
            set
            {
                this.billOfLadingWeightUnit = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool BillOfLadingWeightUnitSpecified
        {
            get
            {
                return this.billOfLadingWeightUnitSpecified;
            }
            set
            {
                this.billOfLadingWeightUnitSpecified = value;
            }
        }

        /// <remarks/>
        public bool UseNotesAsMarksNumbersInBL
        {
            get
            {
                return this.useNotesAsMarksNumbersInBL;
            }
            set
            {
                this.useNotesAsMarksNumbersInBL = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool UseNotesAsMarksNumbersInBLSpecified
        {
            get
            {
                return this.useNotesAsMarksNumbersInBLSpecified;
            }
            set
            {
                this.useNotesAsMarksNumbersInBLSpecified = value;
            }
        }

        /// <remarks/>
        public bool BillOfLadingGroupSimilarItems
        {
            get
            {
                return this.billOfLadingGroupSimilarItems;
            }
            set
            {
                this.billOfLadingGroupSimilarItems = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool BillOfLadingGroupSimilarItemsSpecified
        {
            get
            {
                return this.billOfLadingGroupSimilarItemsSpecified;
            }
            set
            {
                this.billOfLadingGroupSimilarItemsSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum AWBUnitType
    {

        /// <remarks/>
        Kg,

        /// <remarks/>
        Lb,

        /// <remarks/>
        Both,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class AirShipment
    {

        private string airWayBillNumber;

        private string declareValueForCustoms;

        private string flightNumber;

        private Entity firstTransferCarrier;

        private string firstTransferCarrierName;

        private Address firstTransferCarrierAddress;

        private Port firstTransferPort;

        private string firstTransferFlightNumber;

        private System.DateTime firstTransferDate;

        private bool firstTransferDateSpecified;

        private Entity secondTransferCarrier;

        private string secondTransferCarrierName;

        private Address secondTransferCarrierAddress;

        private Port secondTransferPort;

        private string secondTransferFlightNumber;

        private System.DateTime secondTransferDate;

        private bool secondTransferDateSpecified;

        private AirAMSInfoType airAMSInfo;

        /// <remarks/>
        public string AirWayBillNumber
        {
            get
            {
                return this.airWayBillNumber;
            }
            set
            {
                this.airWayBillNumber = value;
            }
        }

        /// <remarks/>
        public string DeclareValueForCustoms
        {
            get
            {
                return this.declareValueForCustoms;
            }
            set
            {
                this.declareValueForCustoms = value;
            }
        }

        /// <remarks/>
        public string FlightNumber
        {
            get
            {
                return this.flightNumber;
            }
            set
            {
                this.flightNumber = value;
            }
        }

        /// <remarks/>
        public Entity FirstTransferCarrier
        {
            get
            {
                return this.firstTransferCarrier;
            }
            set
            {
                this.firstTransferCarrier = value;
            }
        }

        /// <remarks/>
        public string FirstTransferCarrierName
        {
            get
            {
                return this.firstTransferCarrierName;
            }
            set
            {
                this.firstTransferCarrierName = value;
            }
        }

        /// <remarks/>
        public Address FirstTransferCarrierAddress
        {
            get
            {
                return this.firstTransferCarrierAddress;
            }
            set
            {
                this.firstTransferCarrierAddress = value;
            }
        }

        /// <remarks/>
        public Port FirstTransferPort
        {
            get
            {
                return this.firstTransferPort;
            }
            set
            {
                this.firstTransferPort = value;
            }
        }

        /// <remarks/>
        public string FirstTransferFlightNumber
        {
            get
            {
                return this.firstTransferFlightNumber;
            }
            set
            {
                this.firstTransferFlightNumber = value;
            }
        }

        /// <remarks/>
        public System.DateTime FirstTransferDate
        {
            get
            {
                return this.firstTransferDate;
            }
            set
            {
                this.firstTransferDate = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool FirstTransferDateSpecified
        {
            get
            {
                return this.firstTransferDateSpecified;
            }
            set
            {
                this.firstTransferDateSpecified = value;
            }
        }

        /// <remarks/>
        public Entity SecondTransferCarrier
        {
            get
            {
                return this.secondTransferCarrier;
            }
            set
            {
                this.secondTransferCarrier = value;
            }
        }

        /// <remarks/>
        public string SecondTransferCarrierName
        {
            get
            {
                return this.secondTransferCarrierName;
            }
            set
            {
                this.secondTransferCarrierName = value;
            }
        }

        /// <remarks/>
        public Address SecondTransferCarrierAddress
        {
            get
            {
                return this.secondTransferCarrierAddress;
            }
            set
            {
                this.secondTransferCarrierAddress = value;
            }
        }

        /// <remarks/>
        public Port SecondTransferPort
        {
            get
            {
                return this.secondTransferPort;
            }
            set
            {
                this.secondTransferPort = value;
            }
        }

        /// <remarks/>
        public string SecondTransferFlightNumber
        {
            get
            {
                return this.secondTransferFlightNumber;
            }
            set
            {
                this.secondTransferFlightNumber = value;
            }
        }

        /// <remarks/>
        public System.DateTime SecondTransferDate
        {
            get
            {
                return this.secondTransferDate;
            }
            set
            {
                this.secondTransferDate = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool SecondTransferDateSpecified
        {
            get
            {
                return this.secondTransferDateSpecified;
            }
            set
            {
                this.secondTransferDateSpecified = value;
            }
        }

        /// <remarks/>
        public AirAMSInfoType AirAMSInfo
        {
            get
            {
                return this.airAMSInfo;
            }
            set
            {
                this.airAMSInfo = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class AirAMSInfoType
    {

        private bool fDAIndicator;

        private bool fDAIndicatorSpecified;

        private string bondedCarrierID;

        private string inbondControlNumber;

        private string bondedPremisesID;

        private DomInterIDType domesticInternationalID;

        private bool domesticInternationalIDSpecified;

        private string destinationAirport;

        private string permitToProceedDestAirport;

        private System.DateTime arrivalDateAtDestAirport;

        private bool arrivalDateAtDestAirportSpecified;

        private string originOfGoodsCountry;

        private MoneyValue declaredValue;

        private string currency;

        private string harmonizedTariff;

        private string entryType;

        private string entryNumber;

        private string nominatedAgent;

        private string splitIndicator;

        private string boardedPieceQuantity;

        private WeightCodeType weightCode;

        private bool weightCodeSpecified;

        private string boardedWeight;

        /// <remarks/>
        public bool FDAIndicator
        {
            get
            {
                return this.fDAIndicator;
            }
            set
            {
                this.fDAIndicator = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool FDAIndicatorSpecified
        {
            get
            {
                return this.fDAIndicatorSpecified;
            }
            set
            {
                this.fDAIndicatorSpecified = value;
            }
        }

        /// <remarks/>
        public string BondedCarrierID
        {
            get
            {
                return this.bondedCarrierID;
            }
            set
            {
                this.bondedCarrierID = value;
            }
        }

        /// <remarks/>
        public string InbondControlNumber
        {
            get
            {
                return this.inbondControlNumber;
            }
            set
            {
                this.inbondControlNumber = value;
            }
        }

        /// <remarks/>
        public string BondedPremisesID
        {
            get
            {
                return this.bondedPremisesID;
            }
            set
            {
                this.bondedPremisesID = value;
            }
        }

        /// <remarks/>
        public DomInterIDType DomesticInternationalID
        {
            get
            {
                return this.domesticInternationalID;
            }
            set
            {
                this.domesticInternationalID = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DomesticInternationalIDSpecified
        {
            get
            {
                return this.domesticInternationalIDSpecified;
            }
            set
            {
                this.domesticInternationalIDSpecified = value;
            }
        }

        /// <remarks/>
        public string DestinationAirport
        {
            get
            {
                return this.destinationAirport;
            }
            set
            {
                this.destinationAirport = value;
            }
        }

        /// <remarks/>
        public string PermitToProceedDestAirport
        {
            get
            {
                return this.permitToProceedDestAirport;
            }
            set
            {
                this.permitToProceedDestAirport = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime ArrivalDateAtDestAirport
        {
            get
            {
                return this.arrivalDateAtDestAirport;
            }
            set
            {
                this.arrivalDateAtDestAirport = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ArrivalDateAtDestAirportSpecified
        {
            get
            {
                return this.arrivalDateAtDestAirportSpecified;
            }
            set
            {
                this.arrivalDateAtDestAirportSpecified = value;
            }
        }

        /// <remarks/>
        public string OriginOfGoodsCountry
        {
            get
            {
                return this.originOfGoodsCountry;
            }
            set
            {
                this.originOfGoodsCountry = value;
            }
        }

        /// <remarks/>
        public MoneyValue DeclaredValue
        {
            get
            {
                return this.declaredValue;
            }
            set
            {
                this.declaredValue = value;
            }
        }

        /// <remarks/>
        public string Currency
        {
            get
            {
                return this.currency;
            }
            set
            {
                this.currency = value;
            }
        }

        /// <remarks/>
        public string HarmonizedTariff
        {
            get
            {
                return this.harmonizedTariff;
            }
            set
            {
                this.harmonizedTariff = value;
            }
        }

        /// <remarks/>
        public string EntryType
        {
            get
            {
                return this.entryType;
            }
            set
            {
                this.entryType = value;
            }
        }

        /// <remarks/>
        public string EntryNumber
        {
            get
            {
                return this.entryNumber;
            }
            set
            {
                this.entryNumber = value;
            }
        }

        /// <remarks/>
        public string NominatedAgent
        {
            get
            {
                return this.nominatedAgent;
            }
            set
            {
                this.nominatedAgent = value;
            }
        }

        /// <remarks/>
        public string SplitIndicator
        {
            get
            {
                return this.splitIndicator;
            }
            set
            {
                this.splitIndicator = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string BoardedPieceQuantity
        {
            get
            {
                return this.boardedPieceQuantity;
            }
            set
            {
                this.boardedPieceQuantity = value;
            }
        }

        /// <remarks/>
        public WeightCodeType WeightCode
        {
            get
            {
                return this.weightCode;
            }
            set
            {
                this.weightCode = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool WeightCodeSpecified
        {
            get
            {
                return this.weightCodeSpecified;
            }
            set
            {
                this.weightCodeSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string BoardedWeight
        {
            get
            {
                return this.boardedWeight;
            }
            set
            {
                this.boardedWeight = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum DomInterIDType
    {

        /// <remarks/>
        D,

        /// <remarks/>
        I,

        /// <remarks/>
        R,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum WeightCodeType
    {

        /// <remarks/>
        K,

        /// <remarks/>
        L,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class OceanShipment
    {

        private string billOfLadingNumber;

        private VesselType vessel;

        private string vesselName;

        private string vesselFlagCode;

        private string voyageIdentification;

        private string pointOfOriginFTZNumber;

        private string loadingPierOrTerminal;

        private string preCarriageBy;

        private Entity preCarriageByEntity;

        private string placeOfDeliveryByOnCarrier;

        private string placeOfReceiptByPreCarrier;

        private string onCarriageBy;

        private Entity onCarriageByEntity;

        /// <remarks/>
        public string BillOfLadingNumber
        {
            get
            {
                return this.billOfLadingNumber;
            }
            set
            {
                this.billOfLadingNumber = value;
            }
        }

        /// <remarks/>
        public VesselType Vessel
        {
            get
            {
                return this.vessel;
            }
            set
            {
                this.vessel = value;
            }
        }

        /// <remarks/>
        public string VesselName
        {
            get
            {
                return this.vesselName;
            }
            set
            {
                this.vesselName = value;
            }
        }

        /// <remarks/>
        public string VesselFlagCode
        {
            get
            {
                return this.vesselFlagCode;
            }
            set
            {
                this.vesselFlagCode = value;
            }
        }

        /// <remarks/>
        public string VoyageIdentification
        {
            get
            {
                return this.voyageIdentification;
            }
            set
            {
                this.voyageIdentification = value;
            }
        }

        /// <remarks/>
        public string PointOfOriginFTZNumber
        {
            get
            {
                return this.pointOfOriginFTZNumber;
            }
            set
            {
                this.pointOfOriginFTZNumber = value;
            }
        }

        /// <remarks/>
        public string LoadingPierOrTerminal
        {
            get
            {
                return this.loadingPierOrTerminal;
            }
            set
            {
                this.loadingPierOrTerminal = value;
            }
        }

        /// <remarks/>
        public string PreCarriageBy
        {
            get
            {
                return this.preCarriageBy;
            }
            set
            {
                this.preCarriageBy = value;
            }
        }

        /// <remarks/>
        public Entity PreCarriageByEntity
        {
            get
            {
                return this.preCarriageByEntity;
            }
            set
            {
                this.preCarriageByEntity = value;
            }
        }

        /// <remarks/>
        public string PlaceOfDeliveryByOnCarrier
        {
            get
            {
                return this.placeOfDeliveryByOnCarrier;
            }
            set
            {
                this.placeOfDeliveryByOnCarrier = value;
            }
        }

        /// <remarks/>
        public string PlaceOfReceiptByPreCarrier
        {
            get
            {
                return this.placeOfReceiptByPreCarrier;
            }
            set
            {
                this.placeOfReceiptByPreCarrier = value;
            }
        }

        /// <remarks/>
        public string OnCarriageBy
        {
            get
            {
                return this.onCarriageBy;
            }
            set
            {
                this.onCarriageBy = value;
            }
        }

        /// <remarks/>
        public Entity OnCarriageByEntity
        {
            get
            {
                return this.onCarriageByEntity;
            }
            set
            {
                this.onCarriageByEntity = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class VesselType
    {

        private string name;

        private string lloydsCode;

        private string flagCode;

        private DeniedPartyScreeningResultType deniedPartyScreeningResultData;

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        /// <remarks/>
        public string LloydsCode
        {
            get
            {
                return this.lloydsCode;
            }
            set
            {
                this.lloydsCode = value;
            }
        }

        /// <remarks/>
        public string FlagCode
        {
            get
            {
                return this.flagCode;
            }
            set
            {
                this.flagCode = value;
            }
        }

        /// <remarks/>
        public DeniedPartyScreeningResultType DeniedPartyScreeningResultData
        {
            get
            {
                return this.deniedPartyScreeningResultData;
            }
            set
            {
                this.deniedPartyScreeningResultData = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class GroundShipment
    {

        private string billOfLadingNumber;

        private string vehicleNumber;

        private string carrierTrakingNumber;

        private string carrierPRONumber;

        private string driverName;

        private string driverLicenseNumber;

        /// <remarks/>
        public string BillOfLadingNumber
        {
            get
            {
                return this.billOfLadingNumber;
            }
            set
            {
                this.billOfLadingNumber = value;
            }
        }

        /// <remarks/>
        public string VehicleNumber
        {
            get
            {
                return this.vehicleNumber;
            }
            set
            {
                this.vehicleNumber = value;
            }
        }

        /// <remarks/>
        public string CarrierTrakingNumber
        {
            get
            {
                return this.carrierTrakingNumber;
            }
            set
            {
                this.carrierTrakingNumber = value;
            }
        }

        /// <remarks/>
        public string CarrierPRONumber
        {
            get
            {
                return this.carrierPRONumber;
            }
            set
            {
                this.carrierPRONumber = value;
            }
        }

        /// <remarks/>
        public string DriverName
        {
            get
            {
                return this.driverName;
            }
            set
            {
                this.driverName = value;
            }
        }

        /// <remarks/>
        public string DriverLicenseNumber
        {
            get
            {
                return this.driverLicenseNumber;
            }
            set
            {
                this.driverLicenseNumber = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum ServiceType
    {

        /// <remarks/>
        Undefined,

        /// <remarks/>
        DoorToDoor,

        /// <remarks/>
        DoorToPort,

        /// <remarks/>
        PortToPort,

        /// <remarks/>
        PortToDoor,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class EntityRoleType
    {

        private Entity entity;

        private EntityRoleDesc role;

        /// <remarks/>
        public Entity Entity
        {
            get
            {
                return this.entity;
            }
            set
            {
                this.entity = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public EntityRoleDesc Role
        {
            get
            {
                return this.role;
            }
            set
            {
                this.role = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum EntityRoleDesc
    {

        /// <remarks/>
        Other,

        /// <remarks/>
        Manufacturer,

        /// <remarks/>
        SellingParty,

        /// <remarks/>
        BuyingParty,

        /// <remarks/>
        ShipToParty,

        /// <remarks/>
        StuffingLocation,

        /// <remarks/>
        Consolidator,

        /// <remarks/>
        BookingParty,

        /// <remarks/>
        Consignee,

        /// <remarks/>
        ImporterOfRecord,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class ISFDataType
    {

        private ISFFilingTypeDesc filingType;

        private bool filingTypeSpecified;

        private Entity bondedHolder;

        private string bondActivityCode;

        private ISFBondTypeDesc bondType;

        private bool bondTypeSpecified;

        private string iSFTransactionNumber;

        private string iSFShipType;

        private string actionType;

        /// <remarks/>
        public ISFFilingTypeDesc FilingType
        {
            get
            {
                return this.filingType;
            }
            set
            {
                this.filingType = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool FilingTypeSpecified
        {
            get
            {
                return this.filingTypeSpecified;
            }
            set
            {
                this.filingTypeSpecified = value;
            }
        }

        /// <remarks/>
        public Entity BondedHolder
        {
            get
            {
                return this.bondedHolder;
            }
            set
            {
                this.bondedHolder = value;
            }
        }

        /// <remarks/>
        public string BondActivityCode
        {
            get
            {
                return this.bondActivityCode;
            }
            set
            {
                this.bondActivityCode = value;
            }
        }

        /// <remarks/>
        public ISFBondTypeDesc BondType
        {
            get
            {
                return this.bondType;
            }
            set
            {
                this.bondType = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool BondTypeSpecified
        {
            get
            {
                return this.bondTypeSpecified;
            }
            set
            {
                this.bondTypeSpecified = value;
            }
        }

        /// <remarks/>
        public string ISFTransactionNumber
        {
            get
            {
                return this.iSFTransactionNumber;
            }
            set
            {
                this.iSFTransactionNumber = value;
            }
        }

        /// <remarks/>
        public string ISFShipType
        {
            get
            {
                return this.iSFShipType;
            }
            set
            {
                this.iSFShipType = value;
            }
        }

        /// <remarks/>
        public string ActionType
        {
            get
            {
                return this.actionType;
            }
            set
            {
                this.actionType = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum ISFFilingTypeDesc
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("ISF-5")]
        ISF5,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("ISF-10")]
        ISF10,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum ISFBondTypeDesc
    {

        /// <remarks/>
        BondTypeContinuos,

        /// <remarks/>
        BondTypeSingleTrans,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class RouteSegment
    {

        private Entity carrier;

        private ModeOfTransportation modeOfTransportation;

        private Port pointOfOrigin;

        private Port pointOfDestination;

        /// <remarks/>
        public Entity Carrier
        {
            get
            {
                return this.carrier;
            }
            set
            {
                this.carrier = value;
            }
        }

        /// <remarks/>
        public ModeOfTransportation ModeOfTransportation
        {
            get
            {
                return this.modeOfTransportation;
            }
            set
            {
                this.modeOfTransportation = value;
            }
        }

        /// <remarks/>
        public Port PointOfOrigin
        {
            get
            {
                return this.pointOfOrigin;
            }
            set
            {
                this.pointOfOrigin = value;
            }
        }

        /// <remarks/>
        public Port PointOfDestination
        {
            get
            {
                return this.pointOfDestination;
            }
            set
            {
                this.pointOfDestination = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class ExpressLinkInfoType
    {

        private string courierName;

        private MoneyValue cost;

        private string trackingNumber;

        private string trackingLink;

        private string serviceType;

        private string payorName;

        private ExpressLinkStatusType status;

        private bool returnField;

        private System.DateTime date;

        private System.DateTime deliveryDate;

        private bool deliveryDateSpecified;

        private System.DateTime estimatedDeliveryDate;

        private bool estimatedDeliveryDateSpecified;

        private System.DateTime estimatedPickupDate;

        private bool estimatedPickupDateSpecified;

        public ExpressLinkInfoType()
        {
            this.returnField = false;
        }

        /// <remarks/>
        public string CourierName
        {
            get
            {
                return this.courierName;
            }
            set
            {
                this.courierName = value;
            }
        }

        /// <remarks/>
        public MoneyValue Cost
        {
            get
            {
                return this.cost;
            }
            set
            {
                this.cost = value;
            }
        }

        /// <remarks/>
        public string TrackingNumber
        {
            get
            {
                return this.trackingNumber;
            }
            set
            {
                this.trackingNumber = value;
            }
        }

        /// <remarks/>
        public string TrackingLink
        {
            get
            {
                return this.trackingLink;
            }
            set
            {
                this.trackingLink = value;
            }
        }

        /// <remarks/>
        public string ServiceType
        {
            get
            {
                return this.serviceType;
            }
            set
            {
                this.serviceType = value;
            }
        }

        /// <remarks/>
        public string PayorName
        {
            get
            {
                return this.payorName;
            }
            set
            {
                this.payorName = value;
            }
        }

        /// <remarks/>
        public ExpressLinkStatusType Status
        {
            get
            {
                return this.status;
            }
            set
            {
                this.status = value;
            }
        }

        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool Return
        {
            get
            {
                return this.returnField;
            }
            set
            {
                this.returnField = value;
            }
        }

        /// <remarks/>
        public System.DateTime Date
        {
            get
            {
                return this.date;
            }
            set
            {
                this.date = value;
            }
        }

        /// <remarks/>
        public System.DateTime DeliveryDate
        {
            get
            {
                return this.deliveryDate;
            }
            set
            {
                this.deliveryDate = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DeliveryDateSpecified
        {
            get
            {
                return this.deliveryDateSpecified;
            }
            set
            {
                this.deliveryDateSpecified = value;
            }
        }

        /// <remarks/>
        public System.DateTime EstimatedDeliveryDate
        {
            get
            {
                return this.estimatedDeliveryDate;
            }
            set
            {
                this.estimatedDeliveryDate = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool EstimatedDeliveryDateSpecified
        {
            get
            {
                return this.estimatedDeliveryDateSpecified;
            }
            set
            {
                this.estimatedDeliveryDateSpecified = value;
            }
        }

        /// <remarks/>
        public System.DateTime EstimatedPickupDate
        {
            get
            {
                return this.estimatedPickupDate;
            }
            set
            {
                this.estimatedPickupDate = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool EstimatedPickupDateSpecified
        {
            get
            {
                return this.estimatedPickupDateSpecified;
            }
            set
            {
                this.estimatedPickupDateSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum ExpressLinkStatusType
    {

        /// <remarks/>
        Canceled,

        /// <remarks/>
        Delivered,

        /// <remarks/>
        Exception,

        /// <remarks/>
        Failed,

        /// <remarks/>
        Pending,

        /// <remarks/>
        Requested,

        /// <remarks/>
        InTransit,

        /// <remarks/>
        PickedUp,

        /// <remarks/>
        ReturnedToShipper,

        /// <remarks/>
        FirstAttempt,

        /// <remarks/>
        SecondAttempt,

        /// <remarks/>
        ThirdAttempt,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("Invoice", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class AccountingItem
    {

        private System.DateTime createdOn;

        private System.DateTime dueDate;

        private bool dueDateSpecified;

        private string number;

        private Entity createdBy;

        private Entity issuedBy;

        private AccountDefinition account;

        private Entity entity;

        private Address billingAddress;

        private MoneyValue totalAmount;

        private CurrencyType homeCurrency;

        private CurrencyType currency;

        private double exchangeRate;

        private MoneyValue totalAmountInCurrency;

        private Entity division;

        private ChargeList charges;

        private AccountItemLineType[] accountItemLines;

        private string notes;

        private string description;

        private AccountingItemStatusType status;

        private bool statusSpecified;

        private AccountingItemApprovalStatusType approvalStatus;

        private bool approvalStatusSpecified;

        private MoneyValue amountPaid;

        private MoneyValue amountPaidInCurrency;

        private MoneyValue taxAmount;

        private MoneyValue taxAmountInCurrency;

        private MoneyValue retentionAmount;

        private MoneyValue retentionAmountInCurrency;

        private bool isPrepaid;

        private bool isPrepaidSpecified;

        private bool isFiscalPrinted;

        private bool isFiscalPrintedSpecified;

        private FiscalPrintResultType fiscalPrintResult;

        private bool isPeriodic;

        private bool isPeriodicSpecified;

        private bool isPrinted;

        private bool isPrintedSpecified;

        private string objectElementName;

        private string containerNumber;

        private ObjectType relatedObject;

        private string sourceQuotationGUID;

        private bool hasAttachments;

        private bool hasAttachmentsSpecified;

        private AttachmentType[] attachments;

        private EventType[] events;

        private CustomType[] customs;

        private string paymentTerms;

        private PaymentTerm paymentTermsRef;

        private string gUID;

        private string type;

        /// <remarks/>
        public System.DateTime CreatedOn
        {
            get
            {
                return this.createdOn;
            }
            set
            {
                this.createdOn = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime DueDate
        {
            get
            {
                return this.dueDate;
            }
            set
            {
                this.dueDate = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DueDateSpecified
        {
            get
            {
                return this.dueDateSpecified;
            }
            set
            {
                this.dueDateSpecified = value;
            }
        }

        /// <remarks/>
        public string Number
        {
            get
            {
                return this.number;
            }
            set
            {
                this.number = value;
            }
        }

        /// <remarks/>
        public Entity CreatedBy
        {
            get
            {
                return this.createdBy;
            }
            set
            {
                this.createdBy = value;
            }
        }

        /// <remarks/>
        public Entity IssuedBy
        {
            get
            {
                return this.issuedBy;
            }
            set
            {
                this.issuedBy = value;
            }
        }

        /// <remarks/>
        public AccountDefinition Account
        {
            get
            {
                return this.account;
            }
            set
            {
                this.account = value;
            }
        }

        /// <remarks/>
        public Entity Entity
        {
            get
            {
                return this.entity;
            }
            set
            {
                this.entity = value;
            }
        }

        /// <remarks/>
        public Address BillingAddress
        {
            get
            {
                return this.billingAddress;
            }
            set
            {
                this.billingAddress = value;
            }
        }

        /// <remarks/>
        public MoneyValue TotalAmount
        {
            get
            {
                return this.totalAmount;
            }
            set
            {
                this.totalAmount = value;
            }
        }

        /// <remarks/>
        public CurrencyType HomeCurrency
        {
            get
            {
                return this.homeCurrency;
            }
            set
            {
                this.homeCurrency = value;
            }
        }

        /// <remarks/>
        public CurrencyType Currency
        {
            get
            {
                return this.currency;
            }
            set
            {
                this.currency = value;
            }
        }

        /// <remarks/>
        public double ExchangeRate
        {
            get
            {
                return this.exchangeRate;
            }
            set
            {
                this.exchangeRate = value;
            }
        }

        /// <remarks/>
        public MoneyValue TotalAmountInCurrency
        {
            get
            {
                return this.totalAmountInCurrency;
            }
            set
            {
                this.totalAmountInCurrency = value;
            }
        }

        /// <remarks/>
        public Entity Division
        {
            get
            {
                return this.division;
            }
            set
            {
                this.division = value;
            }
        }

        /// <remarks/>
        public ChargeList Charges
        {
            get
            {
                return this.charges;
            }
            set
            {
                this.charges = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("AccountItemLine", IsNullable = false)]
        public AccountItemLineType[] AccountItemLines
        {
            get
            {
                return this.accountItemLines;
            }
            set
            {
                this.accountItemLines = value;
            }
        }

        /// <remarks/>
        public string Notes
        {
            get
            {
                return this.notes;
            }
            set
            {
                this.notes = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        /// <remarks/>
        public AccountingItemStatusType Status
        {
            get
            {
                return this.status;
            }
            set
            {
                this.status = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool StatusSpecified
        {
            get
            {
                return this.statusSpecified;
            }
            set
            {
                this.statusSpecified = value;
            }
        }

        /// <remarks/>
        public AccountingItemApprovalStatusType ApprovalStatus
        {
            get
            {
                return this.approvalStatus;
            }
            set
            {
                this.approvalStatus = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ApprovalStatusSpecified
        {
            get
            {
                return this.approvalStatusSpecified;
            }
            set
            {
                this.approvalStatusSpecified = value;
            }
        }

        /// <remarks/>
        public MoneyValue AmountPaid
        {
            get
            {
                return this.amountPaid;
            }
            set
            {
                this.amountPaid = value;
            }
        }

        /// <remarks/>
        public MoneyValue AmountPaidInCurrency
        {
            get
            {
                return this.amountPaidInCurrency;
            }
            set
            {
                this.amountPaidInCurrency = value;
            }
        }

        /// <remarks/>
        public MoneyValue TaxAmount
        {
            get
            {
                return this.taxAmount;
            }
            set
            {
                this.taxAmount = value;
            }
        }

        /// <remarks/>
        public MoneyValue TaxAmountInCurrency
        {
            get
            {
                return this.taxAmountInCurrency;
            }
            set
            {
                this.taxAmountInCurrency = value;
            }
        }

        /// <remarks/>
        public MoneyValue RetentionAmount
        {
            get
            {
                return this.retentionAmount;
            }
            set
            {
                this.retentionAmount = value;
            }
        }

        /// <remarks/>
        public MoneyValue RetentionAmountInCurrency
        {
            get
            {
                return this.retentionAmountInCurrency;
            }
            set
            {
                this.retentionAmountInCurrency = value;
            }
        }

        /// <remarks/>
        public bool IsPrepaid
        {
            get
            {
                return this.isPrepaid;
            }
            set
            {
                this.isPrepaid = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsPrepaidSpecified
        {
            get
            {
                return this.isPrepaidSpecified;
            }
            set
            {
                this.isPrepaidSpecified = value;
            }
        }

        /// <remarks/>
        public bool IsFiscalPrinted
        {
            get
            {
                return this.isFiscalPrinted;
            }
            set
            {
                this.isFiscalPrinted = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsFiscalPrintedSpecified
        {
            get
            {
                return this.isFiscalPrintedSpecified;
            }
            set
            {
                this.isFiscalPrintedSpecified = value;
            }
        }

        /// <remarks/>
        public FiscalPrintResultType FiscalPrintResult
        {
            get
            {
                return this.fiscalPrintResult;
            }
            set
            {
                this.fiscalPrintResult = value;
            }
        }

        /// <remarks/>
        public bool IsPeriodic
        {
            get
            {
                return this.isPeriodic;
            }
            set
            {
                this.isPeriodic = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsPeriodicSpecified
        {
            get
            {
                return this.isPeriodicSpecified;
            }
            set
            {
                this.isPeriodicSpecified = value;
            }
        }

        /// <remarks/>
        public bool IsPrinted
        {
            get
            {
                return this.isPrinted;
            }
            set
            {
                this.isPrinted = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsPrintedSpecified
        {
            get
            {
                return this.isPrintedSpecified;
            }
            set
            {
                this.isPrintedSpecified = value;
            }
        }

        /// <remarks/>
        public string ObjectElementName
        {
            get
            {
                return this.objectElementName;
            }
            set
            {
                this.objectElementName = value;
            }
        }

        /// <remarks/>
        public string ContainerNumber
        {
            get
            {
                return this.containerNumber;
            }
            set
            {
                this.containerNumber = value;
            }
        }

        /// <remarks/>
        public ObjectType RelatedObject
        {
            get
            {
                return this.relatedObject;
            }
            set
            {
                this.relatedObject = value;
            }
        }

        /// <remarks/>
        public string SourceQuotationGUID
        {
            get
            {
                return this.sourceQuotationGUID;
            }
            set
            {
                this.sourceQuotationGUID = value;
            }
        }

        /// <remarks/>
        public bool HasAttachments
        {
            get
            {
                return this.hasAttachments;
            }
            set
            {
                this.hasAttachments = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool HasAttachmentsSpecified
        {
            get
            {
                return this.hasAttachmentsSpecified;
            }
            set
            {
                this.hasAttachmentsSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Attachment", IsNullable = false)]
        public AttachmentType[] Attachments
        {
            get
            {
                return this.attachments;
            }
            set
            {
                this.attachments = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Event", IsNullable = false)]
        public EventType[] Events
        {
            get
            {
                return this.events;
            }
            set
            {
                this.events = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Custom", IsNullable = false)]
        public CustomType[] Customs
        {
            get
            {
                return this.customs;
            }
            set
            {
                this.customs = value;
            }
        }

        /// <remarks/>
        public string PaymentTerms
        {
            get
            {
                return this.paymentTerms;
            }
            set
            {
                this.paymentTerms = value;
            }
        }

        /// <remarks/>
        public PaymentTerm PaymentTermsRef
        {
            get
            {
                return this.paymentTermsRef;
            }
            set
            {
                this.paymentTermsRef = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string GUID
        {
            get
            {
                return this.gUID;
            }
            set
            {
                this.gUID = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class AccountItemLineType
    {

        private AccountDefinition account;

        private MoneyValue amount;

        private MoneyValue amountInCurrency;

        private string description;

        private Entity entity;

        /// <remarks/>
        public AccountDefinition Account
        {
            get
            {
                return this.account;
            }
            set
            {
                this.account = value;
            }
        }

        /// <remarks/>
        public MoneyValue Amount
        {
            get
            {
                return this.amount;
            }
            set
            {
                this.amount = value;
            }
        }

        /// <remarks/>
        public MoneyValue AmountInCurrency
        {
            get
            {
                return this.amountInCurrency;
            }
            set
            {
                this.amountInCurrency = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        /// <remarks/>
        public Entity Entity
        {
            get
            {
                return this.entity;
            }
            set
            {
                this.entity = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum AccountingItemStatusType
    {

        /// <remarks/>
        Open,

        /// <remarks/>
        Paid,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum AccountingItemApprovalStatusType
    {

        /// <remarks/>
        None,

        /// <remarks/>
        Approved,

        /// <remarks/>
        Disputed,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class FiscalPrintResultType
    {

        private string printerRegisterNumber;

        private string fiscalTransactionNumber;

        private System.DateTime printTime;

        private double totalPrintedAmount;

        private Entity printedBy;

        /// <remarks/>
        public string PrinterRegisterNumber
        {
            get
            {
                return this.printerRegisterNumber;
            }
            set
            {
                this.printerRegisterNumber = value;
            }
        }

        /// <remarks/>
        public string FiscalTransactionNumber
        {
            get
            {
                return this.fiscalTransactionNumber;
            }
            set
            {
                this.fiscalTransactionNumber = value;
            }
        }

        /// <remarks/>
        public System.DateTime PrintTime
        {
            get
            {
                return this.printTime;
            }
            set
            {
                this.printTime = value;
            }
        }

        /// <remarks/>
        public double TotalPrintedAmount
        {
            get
            {
                return this.totalPrintedAmount;
            }
            set
            {
                this.totalPrintedAmount = value;
            }
        }

        /// <remarks/>
        public Entity PrintedBy
        {
            get
            {
                return this.printedBy;
            }
            set
            {
                this.printedBy = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class ObjectType
    {

        private object item;

        private ItemChoiceType itemElementName;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("AirShipment", typeof(MagayaShipment))]
        [System.Xml.Serialization.XmlElementAttribute("CargoRelease", typeof(CargoReleaseType))]
        [System.Xml.Serialization.XmlElementAttribute("GroundShipment", typeof(MagayaShipment))]
        [System.Xml.Serialization.XmlElementAttribute("OceanShipment", typeof(MagayaShipment))]
        [System.Xml.Serialization.XmlElementAttribute("PickupOrder", typeof(PickupOrderType))]
        [System.Xml.Serialization.XmlElementAttribute("PurchaseOrder", typeof(OrderType))]
        [System.Xml.Serialization.XmlElementAttribute("SalesOrder", typeof(OrderType))]
        [System.Xml.Serialization.XmlElementAttribute("WarehouseReceipt", typeof(WarehouseReceipt))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
        public object Item
        {
            get
            {
                return this.item;
            }
            set
            {
                this.item = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemChoiceType ItemElementName
        {
            get
            {
                return this.itemElementName;
            }
            set
            {
                this.itemElementName = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("CargoRelease", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class CargoReleaseType
    {

        private System.DateTime createdOn;

        private System.DateTime releaseDate;

        private bool releaseDateSpecified;

        private string number;

        private string createdByName;

        private sbyte version;

        private bool versionSpecified;

        private Entity issuedBy;

        private Address issuedByAddress;

        private string issuedByName;

        private Entity carrier;

        private string carrierName;

        private string notes;

        private Item[] items;

        private MeasurementUnits measurementUnits;

        private long creatorNetworkID;

        private bool creatorNetworkIDSpecified;

        private ChargeList charges;

        private EventType[] events;

        private Entity division;

        private string totalPieces;

        private WeightValue totalWeight;

        private VolumeValue totalVolume;

        private MoneyValue totalValue;

        private VolumeWeightValue totalVolumeWeight;

        private Entity releasedTo;

        private string releasedToName;

        private Address releasedToAddress;

        private string carrierTrackingNumber;

        private string carrierPRONumber;

        private string driverName;

        private string driverLicenseNumber;

        private string supplierPONumber;

        private PODDataType pODData;

        private CRStatusType status;

        private bool statusSpecified;

        private string uRL;

        private Entity billingClient;

        private bool hasAttachments;

        private bool hasAttachmentsSpecified;

        private AttachmentType[] attachments;

        private CustomType[] customs;

        private bool isOnline;

        private bool isOnlineSpecified;

        private bool isLiquidated;

        private bool isLiquidatedSpecified;

        private ExpressLinkInfoType expressLinkInfo;

        private string gUID;

        private string type;

        /// <remarks/>
        public System.DateTime CreatedOn
        {
            get
            {
                return this.createdOn;
            }
            set
            {
                this.createdOn = value;
            }
        }

        /// <remarks/>
        public System.DateTime ReleaseDate
        {
            get
            {
                return this.releaseDate;
            }
            set
            {
                this.releaseDate = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ReleaseDateSpecified
        {
            get
            {
                return this.releaseDateSpecified;
            }
            set
            {
                this.releaseDateSpecified = value;
            }
        }

        /// <remarks/>
        public string Number
        {
            get
            {
                return this.number;
            }
            set
            {
                this.number = value;
            }
        }

        /// <remarks/>
        public string CreatedByName
        {
            get
            {
                return this.createdByName;
            }
            set
            {
                this.createdByName = value;
            }
        }

        /// <remarks/>
        public sbyte Version
        {
            get
            {
                return this.version;
            }
            set
            {
                this.version = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool VersionSpecified
        {
            get
            {
                return this.versionSpecified;
            }
            set
            {
                this.versionSpecified = value;
            }
        }

        /// <remarks/>
        public Entity IssuedBy
        {
            get
            {
                return this.issuedBy;
            }
            set
            {
                this.issuedBy = value;
            }
        }

        /// <remarks/>
        public Address IssuedByAddress
        {
            get
            {
                return this.issuedByAddress;
            }
            set
            {
                this.issuedByAddress = value;
            }
        }

        /// <remarks/>
        public string IssuedByName
        {
            get
            {
                return this.issuedByName;
            }
            set
            {
                this.issuedByName = value;
            }
        }

        /// <remarks/>
        public Entity Carrier
        {
            get
            {
                return this.carrier;
            }
            set
            {
                this.carrier = value;
            }
        }

        /// <remarks/>
        public string CarrierName
        {
            get
            {
                return this.carrierName;
            }
            set
            {
                this.carrierName = value;
            }
        }

        /// <remarks/>
        public string Notes
        {
            get
            {
                return this.notes;
            }
            set
            {
                this.notes = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Item", IsNullable = false)]
        public Item[] Items
        {
            get
            {
                return this.items;
            }
            set
            {
                this.items = value;
            }
        }

        /// <remarks/>
        public MeasurementUnits MeasurementUnits
        {
            get
            {
                return this.measurementUnits;
            }
            set
            {
                this.measurementUnits = value;
            }
        }

        /// <remarks/>
        public long CreatorNetworkID
        {
            get
            {
                return this.creatorNetworkID;
            }
            set
            {
                this.creatorNetworkID = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CreatorNetworkIDSpecified
        {
            get
            {
                return this.creatorNetworkIDSpecified;
            }
            set
            {
                this.creatorNetworkIDSpecified = value;
            }
        }

        /// <remarks/>
        public ChargeList Charges
        {
            get
            {
                return this.charges;
            }
            set
            {
                this.charges = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Event", IsNullable = false)]
        public EventType[] Events
        {
            get
            {
                return this.events;
            }
            set
            {
                this.events = value;
            }
        }

        /// <remarks/>
        public Entity Division
        {
            get
            {
                return this.division;
            }
            set
            {
                this.division = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string TotalPieces
        {
            get
            {
                return this.totalPieces;
            }
            set
            {
                this.totalPieces = value;
            }
        }

        /// <remarks/>
        public WeightValue TotalWeight
        {
            get
            {
                return this.totalWeight;
            }
            set
            {
                this.totalWeight = value;
            }
        }

        /// <remarks/>
        public VolumeValue TotalVolume
        {
            get
            {
                return this.totalVolume;
            }
            set
            {
                this.totalVolume = value;
            }
        }

        /// <remarks/>
        public MoneyValue TotalValue
        {
            get
            {
                return this.totalValue;
            }
            set
            {
                this.totalValue = value;
            }
        }

        /// <remarks/>
        public VolumeWeightValue TotalVolumeWeight
        {
            get
            {
                return this.totalVolumeWeight;
            }
            set
            {
                this.totalVolumeWeight = value;
            }
        }

        /// <remarks/>
        public Entity ReleasedTo
        {
            get
            {
                return this.releasedTo;
            }
            set
            {
                this.releasedTo = value;
            }
        }

        /// <remarks/>
        public string ReleasedToName
        {
            get
            {
                return this.releasedToName;
            }
            set
            {
                this.releasedToName = value;
            }
        }

        /// <remarks/>
        public Address ReleasedToAddress
        {
            get
            {
                return this.releasedToAddress;
            }
            set
            {
                this.releasedToAddress = value;
            }
        }

        /// <remarks/>
        public string CarrierTrackingNumber
        {
            get
            {
                return this.carrierTrackingNumber;
            }
            set
            {
                this.carrierTrackingNumber = value;
            }
        }

        /// <remarks/>
        public string CarrierPRONumber
        {
            get
            {
                return this.carrierPRONumber;
            }
            set
            {
                this.carrierPRONumber = value;
            }
        }

        /// <remarks/>
        public string DriverName
        {
            get
            {
                return this.driverName;
            }
            set
            {
                this.driverName = value;
            }
        }

        /// <remarks/>
        public string DriverLicenseNumber
        {
            get
            {
                return this.driverLicenseNumber;
            }
            set
            {
                this.driverLicenseNumber = value;
            }
        }

        /// <remarks/>
        public string SupplierPONumber
        {
            get
            {
                return this.supplierPONumber;
            }
            set
            {
                this.supplierPONumber = value;
            }
        }

        /// <remarks/>
        public PODDataType PODData
        {
            get
            {
                return this.pODData;
            }
            set
            {
                this.pODData = value;
            }
        }

        /// <remarks/>
        public CRStatusType Status
        {
            get
            {
                return this.status;
            }
            set
            {
                this.status = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool StatusSpecified
        {
            get
            {
                return this.statusSpecified;
            }
            set
            {
                this.statusSpecified = value;
            }
        }

        /// <remarks/>
        public string URL
        {
            get
            {
                return this.uRL;
            }
            set
            {
                this.uRL = value;
            }
        }

        /// <remarks/>
        public Entity BillingClient
        {
            get
            {
                return this.billingClient;
            }
            set
            {
                this.billingClient = value;
            }
        }

        /// <remarks/>
        public bool HasAttachments
        {
            get
            {
                return this.hasAttachments;
            }
            set
            {
                this.hasAttachments = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool HasAttachmentsSpecified
        {
            get
            {
                return this.hasAttachmentsSpecified;
            }
            set
            {
                this.hasAttachmentsSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Attachment", IsNullable = false)]
        public AttachmentType[] Attachments
        {
            get
            {
                return this.attachments;
            }
            set
            {
                this.attachments = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Custom", IsNullable = false)]
        public CustomType[] Customs
        {
            get
            {
                return this.customs;
            }
            set
            {
                this.customs = value;
            }
        }

        /// <remarks/>
        public bool IsOnline
        {
            get
            {
                return this.isOnline;
            }
            set
            {
                this.isOnline = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsOnlineSpecified
        {
            get
            {
                return this.isOnlineSpecified;
            }
            set
            {
                this.isOnlineSpecified = value;
            }
        }

        /// <remarks/>
        public bool IsLiquidated
        {
            get
            {
                return this.isLiquidated;
            }
            set
            {
                this.isLiquidated = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsLiquidatedSpecified
        {
            get
            {
                return this.isLiquidatedSpecified;
            }
            set
            {
                this.isLiquidatedSpecified = value;
            }
        }

        /// <remarks/>
        public ExpressLinkInfoType ExpressLinkInfo
        {
            get
            {
                return this.expressLinkInfo;
            }
            set
            {
                this.expressLinkInfo = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string GUID
        {
            get
            {
                return this.gUID;
            }
            set
            {
                this.gUID = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum CRStatusType
    {

        /// <remarks/>
        Empty,

        /// <remarks/>
        Loaded,

        /// <remarks/>
        InTransit,

        /// <remarks/>
        Delivered,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("PickupOrder", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class PickupOrderType
    {

        private System.DateTime createdOn;

        private string number;

        private string createdByName;

        private sbyte version;

        private bool versionSpecified;

        private Entity issuedBy;

        private Address issuedByAddress;

        private string issuedByName;

        private string shipperName;

        private Address shipperAddress;

        private Entity shipper;

        private string consigneeName;

        private Address consigneeAddress;

        private Entity consignee;

        private string destinationAgentName;

        private Entity destinationAgent;

        private Entity carrier;

        private string carrierName;

        private string notes;

        private Item[] items;

        private MeasurementUnits measurementUnits;

        private long creatorNetworkID;

        private bool creatorNetworkIDSpecified;

        private ChargeList charges;

        private EventType[] events;

        private Entity division;

        private string totalPieces;

        private WeightValue totalWeight;

        private VolumeValue totalVolume;

        private MoneyValue totalValue;

        private VolumeWeightValue totalVolumeWeight;

        private WeightValue chargeableWeight;

        private Port originPort;

        private Port destinationPort;

        private string supplierName;

        private Address supplierAddress;

        private Entity supplier;

        private string supplierInvoiceNumber;

        private string supplierPONumber;

        private string carrierTrackingNumber;

        private string carrierPRONumber;

        private string driverName;

        private string driverLicenseNumber;

        private WHRStatusType status;

        private bool statusSpecified;

        private string fromQuoteNumber;

        private bool hasAttachments;

        private bool hasAttachmentsSpecified;

        private AttachmentType[] attachments;

        private BondedEntryType bondedEntry;

        private bool bondedEntrySpecified;

        private string bondedEntryNumber;

        private string carrierBookingNumber;

        private string fromBookingNumber;

        private Entity mainCarrier;

        private Entity billingClient;

        private short lastItemID;

        private bool lastItemIDSpecified;

        private Entity pickup;

        private string pickupName;

        private Address pickupAddress;

        private System.DateTime pickupDate;

        private bool pickupDateSpecified;

        private Entity delivery;

        private string deliveryName;

        private Address deliveryAddress;

        private System.DateTime deliveryDate;

        private bool deliveryDateSpecified;

        private PODDataType pODData;

        private Address returnAddress;

        private string uRL;

        private CustomType[] customs;

        private bool isOnline;

        private bool isOnlineSpecified;

        private ModeOfTransportation preferredModeOfTransportation;

        private bool isLiquidated;

        private bool isLiquidatedSpecified;

        private ExpressLinkInfoType expressLinkInfo;

        private string gUID;

        private string type;

        /// <remarks/>
        public System.DateTime CreatedOn
        {
            get
            {
                return this.createdOn;
            }
            set
            {
                this.createdOn = value;
            }
        }

        /// <remarks/>
        public string Number
        {
            get
            {
                return this.number;
            }
            set
            {
                this.number = value;
            }
        }

        /// <remarks/>
        public string CreatedByName
        {
            get
            {
                return this.createdByName;
            }
            set
            {
                this.createdByName = value;
            }
        }

        /// <remarks/>
        public sbyte Version
        {
            get
            {
                return this.version;
            }
            set
            {
                this.version = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool VersionSpecified
        {
            get
            {
                return this.versionSpecified;
            }
            set
            {
                this.versionSpecified = value;
            }
        }

        /// <remarks/>
        public Entity IssuedBy
        {
            get
            {
                return this.issuedBy;
            }
            set
            {
                this.issuedBy = value;
            }
        }

        /// <remarks/>
        public Address IssuedByAddress
        {
            get
            {
                return this.issuedByAddress;
            }
            set
            {
                this.issuedByAddress = value;
            }
        }

        /// <remarks/>
        public string IssuedByName
        {
            get
            {
                return this.issuedByName;
            }
            set
            {
                this.issuedByName = value;
            }
        }

        /// <remarks/>
        public string ShipperName
        {
            get
            {
                return this.shipperName;
            }
            set
            {
                this.shipperName = value;
            }
        }

        /// <remarks/>
        public Address ShipperAddress
        {
            get
            {
                return this.shipperAddress;
            }
            set
            {
                this.shipperAddress = value;
            }
        }

        /// <remarks/>
        public Entity Shipper
        {
            get
            {
                return this.shipper;
            }
            set
            {
                this.shipper = value;
            }
        }

        /// <remarks/>
        public string ConsigneeName
        {
            get
            {
                return this.consigneeName;
            }
            set
            {
                this.consigneeName = value;
            }
        }

        /// <remarks/>
        public Address ConsigneeAddress
        {
            get
            {
                return this.consigneeAddress;
            }
            set
            {
                this.consigneeAddress = value;
            }
        }

        /// <remarks/>
        public Entity Consignee
        {
            get
            {
                return this.consignee;
            }
            set
            {
                this.consignee = value;
            }
        }

        /// <remarks/>
        public string DestinationAgentName
        {
            get
            {
                return this.destinationAgentName;
            }
            set
            {
                this.destinationAgentName = value;
            }
        }

        /// <remarks/>
        public Entity DestinationAgent
        {
            get
            {
                return this.destinationAgent;
            }
            set
            {
                this.destinationAgent = value;
            }
        }

        /// <remarks/>
        public Entity Carrier
        {
            get
            {
                return this.carrier;
            }
            set
            {
                this.carrier = value;
            }
        }

        /// <remarks/>
        public string CarrierName
        {
            get
            {
                return this.carrierName;
            }
            set
            {
                this.carrierName = value;
            }
        }

        /// <remarks/>
        public string Notes
        {
            get
            {
                return this.notes;
            }
            set
            {
                this.notes = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Item", IsNullable = false)]
        public Item[] Items
        {
            get
            {
                return this.items;
            }
            set
            {
                this.items = value;
            }
        }

        /// <remarks/>
        public MeasurementUnits MeasurementUnits
        {
            get
            {
                return this.measurementUnits;
            }
            set
            {
                this.measurementUnits = value;
            }
        }

        /// <remarks/>
        public long CreatorNetworkID
        {
            get
            {
                return this.creatorNetworkID;
            }
            set
            {
                this.creatorNetworkID = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CreatorNetworkIDSpecified
        {
            get
            {
                return this.creatorNetworkIDSpecified;
            }
            set
            {
                this.creatorNetworkIDSpecified = value;
            }
        }

        /// <remarks/>
        public ChargeList Charges
        {
            get
            {
                return this.charges;
            }
            set
            {
                this.charges = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Event", IsNullable = false)]
        public EventType[] Events
        {
            get
            {
                return this.events;
            }
            set
            {
                this.events = value;
            }
        }

        /// <remarks/>
        public Entity Division
        {
            get
            {
                return this.division;
            }
            set
            {
                this.division = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string TotalPieces
        {
            get
            {
                return this.totalPieces;
            }
            set
            {
                this.totalPieces = value;
            }
        }

        /// <remarks/>
        public WeightValue TotalWeight
        {
            get
            {
                return this.totalWeight;
            }
            set
            {
                this.totalWeight = value;
            }
        }

        /// <remarks/>
        public VolumeValue TotalVolume
        {
            get
            {
                return this.totalVolume;
            }
            set
            {
                this.totalVolume = value;
            }
        }

        /// <remarks/>
        public MoneyValue TotalValue
        {
            get
            {
                return this.totalValue;
            }
            set
            {
                this.totalValue = value;
            }
        }

        /// <remarks/>
        public VolumeWeightValue TotalVolumeWeight
        {
            get
            {
                return this.totalVolumeWeight;
            }
            set
            {
                this.totalVolumeWeight = value;
            }
        }

        /// <remarks/>
        public WeightValue ChargeableWeight
        {
            get
            {
                return this.chargeableWeight;
            }
            set
            {
                this.chargeableWeight = value;
            }
        }

        /// <remarks/>
        public Port OriginPort
        {
            get
            {
                return this.originPort;
            }
            set
            {
                this.originPort = value;
            }
        }

        /// <remarks/>
        public Port DestinationPort
        {
            get
            {
                return this.destinationPort;
            }
            set
            {
                this.destinationPort = value;
            }
        }

        /// <remarks/>
        public string SupplierName
        {
            get
            {
                return this.supplierName;
            }
            set
            {
                this.supplierName = value;
            }
        }

        /// <remarks/>
        public Address SupplierAddress
        {
            get
            {
                return this.supplierAddress;
            }
            set
            {
                this.supplierAddress = value;
            }
        }

        /// <remarks/>
        public Entity Supplier
        {
            get
            {
                return this.supplier;
            }
            set
            {
                this.supplier = value;
            }
        }

        /// <remarks/>
        public string SupplierInvoiceNumber
        {
            get
            {
                return this.supplierInvoiceNumber;
            }
            set
            {
                this.supplierInvoiceNumber = value;
            }
        }

        /// <remarks/>
        public string SupplierPONumber
        {
            get
            {
                return this.supplierPONumber;
            }
            set
            {
                this.supplierPONumber = value;
            }
        }

        /// <remarks/>
        public string CarrierTrackingNumber
        {
            get
            {
                return this.carrierTrackingNumber;
            }
            set
            {
                this.carrierTrackingNumber = value;
            }
        }

        /// <remarks/>
        public string CarrierPRONumber
        {
            get
            {
                return this.carrierPRONumber;
            }
            set
            {
                this.carrierPRONumber = value;
            }
        }

        /// <remarks/>
        public string DriverName
        {
            get
            {
                return this.driverName;
            }
            set
            {
                this.driverName = value;
            }
        }

        /// <remarks/>
        public string DriverLicenseNumber
        {
            get
            {
                return this.driverLicenseNumber;
            }
            set
            {
                this.driverLicenseNumber = value;
            }
        }

        /// <remarks/>
        public WHRStatusType Status
        {
            get
            {
                return this.status;
            }
            set
            {
                this.status = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool StatusSpecified
        {
            get
            {
                return this.statusSpecified;
            }
            set
            {
                this.statusSpecified = value;
            }
        }

        /// <remarks/>
        public string FromQuoteNumber
        {
            get
            {
                return this.fromQuoteNumber;
            }
            set
            {
                this.fromQuoteNumber = value;
            }
        }

        /// <remarks/>
        public bool HasAttachments
        {
            get
            {
                return this.hasAttachments;
            }
            set
            {
                this.hasAttachments = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool HasAttachmentsSpecified
        {
            get
            {
                return this.hasAttachmentsSpecified;
            }
            set
            {
                this.hasAttachmentsSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Attachment", IsNullable = false)]
        public AttachmentType[] Attachments
        {
            get
            {
                return this.attachments;
            }
            set
            {
                this.attachments = value;
            }
        }

        /// <remarks/>
        public BondedEntryType BondedEntry
        {
            get
            {
                return this.bondedEntry;
            }
            set
            {
                this.bondedEntry = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool BondedEntrySpecified
        {
            get
            {
                return this.bondedEntrySpecified;
            }
            set
            {
                this.bondedEntrySpecified = value;
            }
        }

        /// <remarks/>
        public string BondedEntryNumber
        {
            get
            {
                return this.bondedEntryNumber;
            }
            set
            {
                this.bondedEntryNumber = value;
            }
        }

        /// <remarks/>
        public string CarrierBookingNumber
        {
            get
            {
                return this.carrierBookingNumber;
            }
            set
            {
                this.carrierBookingNumber = value;
            }
        }

        /// <remarks/>
        public string FromBookingNumber
        {
            get
            {
                return this.fromBookingNumber;
            }
            set
            {
                this.fromBookingNumber = value;
            }
        }

        /// <remarks/>
        public Entity MainCarrier
        {
            get
            {
                return this.mainCarrier;
            }
            set
            {
                this.mainCarrier = value;
            }
        }

        /// <remarks/>
        public Entity BillingClient
        {
            get
            {
                return this.billingClient;
            }
            set
            {
                this.billingClient = value;
            }
        }

        /// <remarks/>
        public short LastItemID
        {
            get
            {
                return this.lastItemID;
            }
            set
            {
                this.lastItemID = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool LastItemIDSpecified
        {
            get
            {
                return this.lastItemIDSpecified;
            }
            set
            {
                this.lastItemIDSpecified = value;
            }
        }

        /// <remarks/>
        public Entity Pickup
        {
            get
            {
                return this.pickup;
            }
            set
            {
                this.pickup = value;
            }
        }

        /// <remarks/>
        public string PickupName
        {
            get
            {
                return this.pickupName;
            }
            set
            {
                this.pickupName = value;
            }
        }

        /// <remarks/>
        public Address PickupAddress
        {
            get
            {
                return this.pickupAddress;
            }
            set
            {
                this.pickupAddress = value;
            }
        }

        /// <remarks/>
        public System.DateTime PickupDate
        {
            get
            {
                return this.pickupDate;
            }
            set
            {
                this.pickupDate = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool PickupDateSpecified
        {
            get
            {
                return this.pickupDateSpecified;
            }
            set
            {
                this.pickupDateSpecified = value;
            }
        }

        /// <remarks/>
        public Entity Delivery
        {
            get
            {
                return this.delivery;
            }
            set
            {
                this.delivery = value;
            }
        }

        /// <remarks/>
        public string DeliveryName
        {
            get
            {
                return this.deliveryName;
            }
            set
            {
                this.deliveryName = value;
            }
        }

        /// <remarks/>
        public Address DeliveryAddress
        {
            get
            {
                return this.deliveryAddress;
            }
            set
            {
                this.deliveryAddress = value;
            }
        }

        /// <remarks/>
        public System.DateTime DeliveryDate
        {
            get
            {
                return this.deliveryDate;
            }
            set
            {
                this.deliveryDate = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DeliveryDateSpecified
        {
            get
            {
                return this.deliveryDateSpecified;
            }
            set
            {
                this.deliveryDateSpecified = value;
            }
        }

        /// <remarks/>
        public PODDataType PODData
        {
            get
            {
                return this.pODData;
            }
            set
            {
                this.pODData = value;
            }
        }

        /// <remarks/>
        public Address ReturnAddress
        {
            get
            {
                return this.returnAddress;
            }
            set
            {
                this.returnAddress = value;
            }
        }

        /// <remarks/>
        public string URL
        {
            get
            {
                return this.uRL;
            }
            set
            {
                this.uRL = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Custom", IsNullable = false)]
        public CustomType[] Customs
        {
            get
            {
                return this.customs;
            }
            set
            {
                this.customs = value;
            }
        }

        /// <remarks/>
        public bool IsOnline
        {
            get
            {
                return this.isOnline;
            }
            set
            {
                this.isOnline = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsOnlineSpecified
        {
            get
            {
                return this.isOnlineSpecified;
            }
            set
            {
                this.isOnlineSpecified = value;
            }
        }

        /// <remarks/>
        public ModeOfTransportation PreferredModeOfTransportation
        {
            get
            {
                return this.preferredModeOfTransportation;
            }
            set
            {
                this.preferredModeOfTransportation = value;
            }
        }

        /// <remarks/>
        public bool IsLiquidated
        {
            get
            {
                return this.isLiquidated;
            }
            set
            {
                this.isLiquidated = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsLiquidatedSpecified
        {
            get
            {
                return this.isLiquidatedSpecified;
            }
            set
            {
                this.isLiquidatedSpecified = value;
            }
        }

        /// <remarks/>
        public ExpressLinkInfoType ExpressLinkInfo
        {
            get
            {
                return this.expressLinkInfo;
            }
            set
            {
                this.expressLinkInfo = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string GUID
        {
            get
            {
                return this.gUID;
            }
            set
            {
                this.gUID = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum WHRStatusType
    {

        /// <remarks/>
        OnHand,

        /// <remarks/>
        InProcess,

        /// <remarks/>
        InTransit,

        /// <remarks/>
        AtDestination,

        /// <remarks/>
        Delivered,

        /// <remarks/>
        Empty,

        /// <remarks/>
        Arriving,

        /// <remarks/>
        Pending,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum BondedEntryType
    {

        /// <remarks/>
        Other,

        /// <remarks/>
        Domestic,

        /// <remarks/>
        Bonded,

        /// <remarks/>
        ContainerFreightStation,

        /// <remarks/>
        GeneralOrder,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("PurchaseOrder", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class OrderType
    {

        private System.DateTime createdOn;

        private string number;

        private string createdByName;

        private sbyte version;

        private bool versionSpecified;

        private OrderStatusType status;

        private bool statusSpecified;

        private string referenceNumber;

        private System.DateTime shippingDate;

        private bool shippingDateSpecified;

        private System.DateTime approvedDate;

        private bool approvedDateSpecified;

        private string approvedByName;

        private string buyerName;

        private Entity buyer;

        private Address buyerBillingAddress;

        private Address buyerShippingAddress;

        private Entity billingClient;

        private string sellerName;

        private Entity seller;

        private Address sellerAddress;

        private string paymentTerms;

        private PaymentTerm paymentTermsRef;

        private Entity salesperson;

        private OrderLineType[] lines;

        private Item[] items;

        private string notes;

        private Entity division;

        private ModeOfTransportation modeOfTransportation;

        private IncotermType incoterm;

        private string jobGUID;

        private string jobNumber;

        private AttachmentType[] attachments;

        private long creatorNetworkID;

        private bool creatorNetworkIDSpecified;

        private CurrencyType currency;

        private CurrencyType homeCurrency;

        private double exchangeRate;

        private bool exchangeRateSpecified;

        private MoneyValue itemsAmount;

        private MoneyValue itemsAmountInCurrency;

        private MoneyValue chargesAmount;

        private MoneyValue chargesAmountInCurrency;

        private MoneyValue taxAmount;

        private MoneyValue taxAmountInCurrency;

        private MoneyValue totalAmount;

        private MoneyValue totalAmountInCurrency;

        private string totalPieces;

        private WeightValue totalWeight;

        private VolumeValue totalVolume;

        private bool isAcceptedBySeller;

        private bool isAcceptedBySellerSpecified;

        private EventType[] events;

        private string uRL;

        private string jobURL;

        private MeasurementUnits measurementUnits;

        private ChargeList charges;

        private CustomType[] customs;

        private GUIDItem createdFrom;

        private bool isOnline;

        private bool isOnlineSpecified;

        private bool isCancelled;

        private bool isCancelledSpecified;

        private string gUID;

        private string type;

        /// <remarks/>
        public System.DateTime CreatedOn
        {
            get
            {
                return this.createdOn;
            }
            set
            {
                this.createdOn = value;
            }
        }

        /// <remarks/>
        public string Number
        {
            get
            {
                return this.number;
            }
            set
            {
                this.number = value;
            }
        }

        /// <remarks/>
        public string CreatedByName
        {
            get
            {
                return this.createdByName;
            }
            set
            {
                this.createdByName = value;
            }
        }

        /// <remarks/>
        public sbyte Version
        {
            get
            {
                return this.version;
            }
            set
            {
                this.version = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool VersionSpecified
        {
            get
            {
                return this.versionSpecified;
            }
            set
            {
                this.versionSpecified = value;
            }
        }

        /// <remarks/>
        public OrderStatusType Status
        {
            get
            {
                return this.status;
            }
            set
            {
                this.status = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool StatusSpecified
        {
            get
            {
                return this.statusSpecified;
            }
            set
            {
                this.statusSpecified = value;
            }
        }

        /// <remarks/>
        public string ReferenceNumber
        {
            get
            {
                return this.referenceNumber;
            }
            set
            {
                this.referenceNumber = value;
            }
        }

        /// <remarks/>
        public System.DateTime ShippingDate
        {
            get
            {
                return this.shippingDate;
            }
            set
            {
                this.shippingDate = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ShippingDateSpecified
        {
            get
            {
                return this.shippingDateSpecified;
            }
            set
            {
                this.shippingDateSpecified = value;
            }
        }

        /// <remarks/>
        public System.DateTime ApprovedDate
        {
            get
            {
                return this.approvedDate;
            }
            set
            {
                this.approvedDate = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ApprovedDateSpecified
        {
            get
            {
                return this.approvedDateSpecified;
            }
            set
            {
                this.approvedDateSpecified = value;
            }
        }

        /// <remarks/>
        public string ApprovedByName
        {
            get
            {
                return this.approvedByName;
            }
            set
            {
                this.approvedByName = value;
            }
        }

        /// <remarks/>
        public string BuyerName
        {
            get
            {
                return this.buyerName;
            }
            set
            {
                this.buyerName = value;
            }
        }

        /// <remarks/>
        public Entity Buyer
        {
            get
            {
                return this.buyer;
            }
            set
            {
                this.buyer = value;
            }
        }

        /// <remarks/>
        public Address BuyerBillingAddress
        {
            get
            {
                return this.buyerBillingAddress;
            }
            set
            {
                this.buyerBillingAddress = value;
            }
        }

        /// <remarks/>
        public Address BuyerShippingAddress
        {
            get
            {
                return this.buyerShippingAddress;
            }
            set
            {
                this.buyerShippingAddress = value;
            }
        }

        /// <remarks/>
        public Entity BillingClient
        {
            get
            {
                return this.billingClient;
            }
            set
            {
                this.billingClient = value;
            }
        }

        /// <remarks/>
        public string SellerName
        {
            get
            {
                return this.sellerName;
            }
            set
            {
                this.sellerName = value;
            }
        }

        /// <remarks/>
        public Entity Seller
        {
            get
            {
                return this.seller;
            }
            set
            {
                this.seller = value;
            }
        }

        /// <remarks/>
        public Address SellerAddress
        {
            get
            {
                return this.sellerAddress;
            }
            set
            {
                this.sellerAddress = value;
            }
        }

        /// <remarks/>
        public string PaymentTerms
        {
            get
            {
                return this.paymentTerms;
            }
            set
            {
                this.paymentTerms = value;
            }
        }

        /// <remarks/>
        public PaymentTerm PaymentTermsRef
        {
            get
            {
                return this.paymentTermsRef;
            }
            set
            {
                this.paymentTermsRef = value;
            }
        }

        /// <remarks/>
        public Entity Salesperson
        {
            get
            {
                return this.salesperson;
            }
            set
            {
                this.salesperson = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Line", IsNullable = false)]
        public OrderLineType[] Lines
        {
            get
            {
                return this.lines;
            }
            set
            {
                this.lines = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Item", IsNullable = false)]
        public Item[] Items
        {
            get
            {
                return this.items;
            }
            set
            {
                this.items = value;
            }
        }

        /// <remarks/>
        public string Notes
        {
            get
            {
                return this.notes;
            }
            set
            {
                this.notes = value;
            }
        }

        /// <remarks/>
        public Entity Division
        {
            get
            {
                return this.division;
            }
            set
            {
                this.division = value;
            }
        }

        /// <remarks/>
        public ModeOfTransportation ModeOfTransportation
        {
            get
            {
                return this.modeOfTransportation;
            }
            set
            {
                this.modeOfTransportation = value;
            }
        }

        /// <remarks/>
        public IncotermType Incoterm
        {
            get
            {
                return this.incoterm;
            }
            set
            {
                this.incoterm = value;
            }
        }

        /// <remarks/>
        public string JobGUID
        {
            get
            {
                return this.jobGUID;
            }
            set
            {
                this.jobGUID = value;
            }
        }

        /// <remarks/>
        public string JobNumber
        {
            get
            {
                return this.jobNumber;
            }
            set
            {
                this.jobNumber = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Attachment", IsNullable = false)]
        public AttachmentType[] Attachments
        {
            get
            {
                return this.attachments;
            }
            set
            {
                this.attachments = value;
            }
        }

        /// <remarks/>
        public long CreatorNetworkID
        {
            get
            {
                return this.creatorNetworkID;
            }
            set
            {
                this.creatorNetworkID = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CreatorNetworkIDSpecified
        {
            get
            {
                return this.creatorNetworkIDSpecified;
            }
            set
            {
                this.creatorNetworkIDSpecified = value;
            }
        }

        /// <remarks/>
        public CurrencyType Currency
        {
            get
            {
                return this.currency;
            }
            set
            {
                this.currency = value;
            }
        }

        /// <remarks/>
        public CurrencyType HomeCurrency
        {
            get
            {
                return this.homeCurrency;
            }
            set
            {
                this.homeCurrency = value;
            }
        }

        /// <remarks/>
        public double ExchangeRate
        {
            get
            {
                return this.exchangeRate;
            }
            set
            {
                this.exchangeRate = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ExchangeRateSpecified
        {
            get
            {
                return this.exchangeRateSpecified;
            }
            set
            {
                this.exchangeRateSpecified = value;
            }
        }

        /// <remarks/>
        public MoneyValue ItemsAmount
        {
            get
            {
                return this.itemsAmount;
            }
            set
            {
                this.itemsAmount = value;
            }
        }

        /// <remarks/>
        public MoneyValue ItemsAmountInCurrency
        {
            get
            {
                return this.itemsAmountInCurrency;
            }
            set
            {
                this.itemsAmountInCurrency = value;
            }
        }

        /// <remarks/>
        public MoneyValue ChargesAmount
        {
            get
            {
                return this.chargesAmount;
            }
            set
            {
                this.chargesAmount = value;
            }
        }

        /// <remarks/>
        public MoneyValue ChargesAmountInCurrency
        {
            get
            {
                return this.chargesAmountInCurrency;
            }
            set
            {
                this.chargesAmountInCurrency = value;
            }
        }

        /// <remarks/>
        public MoneyValue TaxAmount
        {
            get
            {
                return this.taxAmount;
            }
            set
            {
                this.taxAmount = value;
            }
        }

        /// <remarks/>
        public MoneyValue TaxAmountInCurrency
        {
            get
            {
                return this.taxAmountInCurrency;
            }
            set
            {
                this.taxAmountInCurrency = value;
            }
        }

        /// <remarks/>
        public MoneyValue TotalAmount
        {
            get
            {
                return this.totalAmount;
            }
            set
            {
                this.totalAmount = value;
            }
        }

        /// <remarks/>
        public MoneyValue TotalAmountInCurrency
        {
            get
            {
                return this.totalAmountInCurrency;
            }
            set
            {
                this.totalAmountInCurrency = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string TotalPieces
        {
            get
            {
                return this.totalPieces;
            }
            set
            {
                this.totalPieces = value;
            }
        }

        /// <remarks/>
        public WeightValue TotalWeight
        {
            get
            {
                return this.totalWeight;
            }
            set
            {
                this.totalWeight = value;
            }
        }

        /// <remarks/>
        public VolumeValue TotalVolume
        {
            get
            {
                return this.totalVolume;
            }
            set
            {
                this.totalVolume = value;
            }
        }

        /// <remarks/>
        public bool IsAcceptedBySeller
        {
            get
            {
                return this.isAcceptedBySeller;
            }
            set
            {
                this.isAcceptedBySeller = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsAcceptedBySellerSpecified
        {
            get
            {
                return this.isAcceptedBySellerSpecified;
            }
            set
            {
                this.isAcceptedBySellerSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Event", IsNullable = false)]
        public EventType[] Events
        {
            get
            {
                return this.events;
            }
            set
            {
                this.events = value;
            }
        }

        /// <remarks/>
        public string URL
        {
            get
            {
                return this.uRL;
            }
            set
            {
                this.uRL = value;
            }
        }

        /// <remarks/>
        public string JobURL
        {
            get
            {
                return this.jobURL;
            }
            set
            {
                this.jobURL = value;
            }
        }

        /// <remarks/>
        public MeasurementUnits MeasurementUnits
        {
            get
            {
                return this.measurementUnits;
            }
            set
            {
                this.measurementUnits = value;
            }
        }

        /// <remarks/>
        public ChargeList Charges
        {
            get
            {
                return this.charges;
            }
            set
            {
                this.charges = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Custom", IsNullable = false)]
        public CustomType[] Customs
        {
            get
            {
                return this.customs;
            }
            set
            {
                this.customs = value;
            }
        }

        /// <remarks/>
        public GUIDItem CreatedFrom
        {
            get
            {
                return this.createdFrom;
            }
            set
            {
                this.createdFrom = value;
            }
        }

        /// <remarks/>
        public bool IsOnline
        {
            get
            {
                return this.isOnline;
            }
            set
            {
                this.isOnline = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsOnlineSpecified
        {
            get
            {
                return this.isOnlineSpecified;
            }
            set
            {
                this.isOnlineSpecified = value;
            }
        }

        /// <remarks/>
        public bool IsCancelled
        {
            get
            {
                return this.isCancelled;
            }
            set
            {
                this.isCancelled = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsCancelledSpecified
        {
            get
            {
                return this.isCancelledSpecified;
            }
            set
            {
                this.isCancelledSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string GUID
        {
            get
            {
                return this.gUID;
            }
            set
            {
                this.gUID = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum OrderStatusType
    {

        /// <remarks/>
        Empty,

        /// <remarks/>
        Open,

        /// <remarks/>
        Approved,

        /// <remarks/>
        Accepted,

        /// <remarks/>
        Arriving,

        /// <remarks/>
        OnHand,

        /// <remarks/>
        Loading,

        /// <remarks/>
        InTransit,

        /// <remarks/>
        AtDestination,

        /// <remarks/>
        Delivered,

        /// <remarks/>
        Canceled,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class OrderLineType
    {

        private string lineNumber;

        private string description;

        private string unitOfMeasurement;

        private ItemDefinition itemDefinition;

        private string quantity;

        private System.DateTime createdOn;

        private bool createdOnSpecified;

        private System.DateTime lastModifiedOn;

        private bool lastModifiedOnSpecified;

        private System.DateTime promisedDate;

        private bool promisedDateSpecified;

        private PrimaryOrderLineStatusType status;

        private bool statusSpecified;

        private SecondaryOrderLineStatusType saleStatus;

        private bool saleStatusSpecified;

        private ItemRefType[] allocatedItems;

        private Charge salesCharge;

        private Charge invoiceCharge;

        private bool isKitLine;

        private bool isKitLineSpecified;

        private string kitReferenceLineNumber;

        private string notes;

        private CustomType[] customs;

        private string gUID;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string LineNumber
        {
            get
            {
                return this.lineNumber;
            }
            set
            {
                this.lineNumber = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        /// <remarks/>
        public string UnitOfMeasurement
        {
            get
            {
                return this.unitOfMeasurement;
            }
            set
            {
                this.unitOfMeasurement = value;
            }
        }

        /// <remarks/>
        public ItemDefinition ItemDefinition
        {
            get
            {
                return this.itemDefinition;
            }
            set
            {
                this.itemDefinition = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string Quantity
        {
            get
            {
                return this.quantity;
            }
            set
            {
                this.quantity = value;
            }
        }

        /// <remarks/>
        public System.DateTime CreatedOn
        {
            get
            {
                return this.createdOn;
            }
            set
            {
                this.createdOn = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CreatedOnSpecified
        {
            get
            {
                return this.createdOnSpecified;
            }
            set
            {
                this.createdOnSpecified = value;
            }
        }

        /// <remarks/>
        public System.DateTime LastModifiedOn
        {
            get
            {
                return this.lastModifiedOn;
            }
            set
            {
                this.lastModifiedOn = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool LastModifiedOnSpecified
        {
            get
            {
                return this.lastModifiedOnSpecified;
            }
            set
            {
                this.lastModifiedOnSpecified = value;
            }
        }

        /// <remarks/>
        public System.DateTime PromisedDate
        {
            get
            {
                return this.promisedDate;
            }
            set
            {
                this.promisedDate = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool PromisedDateSpecified
        {
            get
            {
                return this.promisedDateSpecified;
            }
            set
            {
                this.promisedDateSpecified = value;
            }
        }

        /// <remarks/>
        public PrimaryOrderLineStatusType Status
        {
            get
            {
                return this.status;
            }
            set
            {
                this.status = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool StatusSpecified
        {
            get
            {
                return this.statusSpecified;
            }
            set
            {
                this.statusSpecified = value;
            }
        }

        /// <remarks/>
        public SecondaryOrderLineStatusType SaleStatus
        {
            get
            {
                return this.saleStatus;
            }
            set
            {
                this.saleStatus = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool SaleStatusSpecified
        {
            get
            {
                return this.saleStatusSpecified;
            }
            set
            {
                this.saleStatusSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("ItemRef", IsNullable = false)]
        public ItemRefType[] AllocatedItems
        {
            get
            {
                return this.allocatedItems;
            }
            set
            {
                this.allocatedItems = value;
            }
        }

        /// <remarks/>
        public Charge SalesCharge
        {
            get
            {
                return this.salesCharge;
            }
            set
            {
                this.salesCharge = value;
            }
        }

        /// <remarks/>
        public Charge InvoiceCharge
        {
            get
            {
                return this.invoiceCharge;
            }
            set
            {
                this.invoiceCharge = value;
            }
        }

        /// <remarks/>
        public bool IsKitLine
        {
            get
            {
                return this.isKitLine;
            }
            set
            {
                this.isKitLine = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsKitLineSpecified
        {
            get
            {
                return this.isKitLineSpecified;
            }
            set
            {
                this.isKitLineSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string KitReferenceLineNumber
        {
            get
            {
                return this.kitReferenceLineNumber;
            }
            set
            {
                this.kitReferenceLineNumber = value;
            }
        }

        /// <remarks/>
        public string Notes
        {
            get
            {
                return this.notes;
            }
            set
            {
                this.notes = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Custom", IsNullable = false)]
        public CustomType[] Customs
        {
            get
            {
                return this.customs;
            }
            set
            {
                this.customs = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string GUID
        {
            get
            {
                return this.gUID;
            }
            set
            {
                this.gUID = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum PrimaryOrderLineStatusType
    {

        /// <remarks/>
        Backordered,

        /// <remarks/>
        Canceled,

        /// <remarks/>
        Allocated,

        /// <remarks/>
        Reserved,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum SecondaryOrderLineStatusType
    {

        /// <remarks/>
        Empty,

        /// <remarks/>
        Open,

        /// <remarks/>
        Arriving,

        /// <remarks/>
        OnHand,

        /// <remarks/>
        Loaded,

        /// <remarks/>
        InTransit,

        /// <remarks/>
        AtDestination,

        /// <remarks/>
        Delivered,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class ItemRefType
    {

        private string gUID;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string GUID
        {
            get
            {
                return this.gUID;
            }
            set
            {
                this.gUID = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("WarehouseReceipt", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class WarehouseReceipt
    {

        private System.DateTime createdOn;

        private string number;

        private string createdByName;

        private sbyte version;

        private bool versionSpecified;

        private WHRStatusType status;

        private bool statusSpecified;

        private ModeOfTransportation modeOfTransportation;

        private string modeOfTransportCode;

        private Entity issuedBy;

        private Address issuedByAddress;

        private string issuedByName;

        private string shipperName;

        private Address shipperAddress;

        private Entity shipper;

        private string consigneeName;

        private Address consigneeAddress;

        private Entity consignee;

        private string destinationAgentName;

        private Entity destinationAgent;

        private Entity carrier;

        private string carrierName;

        private string carrierTrackingNumber;

        private string carrierPRONumber;

        private string driverName;

        private string driverLicenseNumber;

        private string notes;

        private Item[] items;

        private MeasurementUnits measurementUnits;

        private long creatorNetworkID;

        private bool creatorNetworkIDSpecified;

        private ChargeList charges;

        private EventType[] events;

        private Entity division;

        private string totalPieces;

        private WeightValue totalWeight;

        private VolumeValue totalVolume;

        private MoneyValue totalValue;

        private VolumeWeightValue totalVolumeWeight;

        private WeightValue chargeableWeight;

        private Port originPort;

        private Port destinationPort;

        private string supplierName;

        private Address supplierAddress;

        private Entity supplier;

        private string supplierInvoiceNumber;

        private string supplierPONumber;

        private string fromQuoteNumber;

        private bool hasAttachments;

        private bool hasAttachmentsSpecified;

        private AttachmentType[] attachments;

        private BondedEntryType bondedEntry;

        private bool bondedEntrySpecified;

        private string bondedEntryNumber;

        private System.DateTime bondedEntryDate;

        private bool bondedEntryDateSpecified;

        private string carrierBookingNumber;

        private string fromBookingNumber;

        private Entity mainCarrier;

        private Entity billingClient;

        private short lastItemID;

        private bool lastItemIDSpecified;

        private string uRL;

        private CustomType[] customs;

        private bool isOnline;

        private bool isOnlineSpecified;

        private HoldStatusType holdStatus;

        private bool isLiquidated;

        private bool isLiquidatedSpecified;

        private string gUID;

        private string type;

        /// <remarks/>
        public System.DateTime CreatedOn
        {
            get
            {
                return this.createdOn;
            }
            set
            {
                this.createdOn = value;
            }
        }

        /// <remarks/>
        public string Number
        {
            get
            {
                return this.number;
            }
            set
            {
                this.number = value;
            }
        }

        /// <remarks/>
        public string CreatedByName
        {
            get
            {
                return this.createdByName;
            }
            set
            {
                this.createdByName = value;
            }
        }

        /// <remarks/>
        public sbyte Version
        {
            get
            {
                return this.version;
            }
            set
            {
                this.version = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool VersionSpecified
        {
            get
            {
                return this.versionSpecified;
            }
            set
            {
                this.versionSpecified = value;
            }
        }

        /// <remarks/>
        public WHRStatusType Status
        {
            get
            {
                return this.status;
            }
            set
            {
                this.status = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool StatusSpecified
        {
            get
            {
                return this.statusSpecified;
            }
            set
            {
                this.statusSpecified = value;
            }
        }

        /// <remarks/>
        public ModeOfTransportation ModeOfTransportation
        {
            get
            {
                return this.modeOfTransportation;
            }
            set
            {
                this.modeOfTransportation = value;
            }
        }

        /// <remarks/>
        public string ModeOfTransportCode
        {
            get
            {
                return this.modeOfTransportCode;
            }
            set
            {
                this.modeOfTransportCode = value;
            }
        }

        /// <remarks/>
        public Entity IssuedBy
        {
            get
            {
                return this.issuedBy;
            }
            set
            {
                this.issuedBy = value;
            }
        }

        /// <remarks/>
        public Address IssuedByAddress
        {
            get
            {
                return this.issuedByAddress;
            }
            set
            {
                this.issuedByAddress = value;
            }
        }

        /// <remarks/>
        public string IssuedByName
        {
            get
            {
                return this.issuedByName;
            }
            set
            {
                this.issuedByName = value;
            }
        }

        /// <remarks/>
        public string ShipperName
        {
            get
            {
                return this.shipperName;
            }
            set
            {
                this.shipperName = value;
            }
        }

        /// <remarks/>
        public Address ShipperAddress
        {
            get
            {
                return this.shipperAddress;
            }
            set
            {
                this.shipperAddress = value;
            }
        }

        /// <remarks/>
        public Entity Shipper
        {
            get
            {
                return this.shipper;
            }
            set
            {
                this.shipper = value;
            }
        }

        /// <remarks/>
        public string ConsigneeName
        {
            get
            {
                return this.consigneeName;
            }
            set
            {
                this.consigneeName = value;
            }
        }

        /// <remarks/>
        public Address ConsigneeAddress
        {
            get
            {
                return this.consigneeAddress;
            }
            set
            {
                this.consigneeAddress = value;
            }
        }

        /// <remarks/>
        public Entity Consignee
        {
            get
            {
                return this.consignee;
            }
            set
            {
                this.consignee = value;
            }
        }

        /// <remarks/>
        public string DestinationAgentName
        {
            get
            {
                return this.destinationAgentName;
            }
            set
            {
                this.destinationAgentName = value;
            }
        }

        /// <remarks/>
        public Entity DestinationAgent
        {
            get
            {
                return this.destinationAgent;
            }
            set
            {
                this.destinationAgent = value;
            }
        }

        /// <remarks/>
        public Entity Carrier
        {
            get
            {
                return this.carrier;
            }
            set
            {
                this.carrier = value;
            }
        }

        /// <remarks/>
        public string CarrierName
        {
            get
            {
                return this.carrierName;
            }
            set
            {
                this.carrierName = value;
            }
        }

        /// <remarks/>
        public string CarrierTrackingNumber
        {
            get
            {
                return this.carrierTrackingNumber;
            }
            set
            {
                this.carrierTrackingNumber = value;
            }
        }

        /// <remarks/>
        public string CarrierPRONumber
        {
            get
            {
                return this.carrierPRONumber;
            }
            set
            {
                this.carrierPRONumber = value;
            }
        }

        /// <remarks/>
        public string DriverName
        {
            get
            {
                return this.driverName;
            }
            set
            {
                this.driverName = value;
            }
        }

        /// <remarks/>
        public string DriverLicenseNumber
        {
            get
            {
                return this.driverLicenseNumber;
            }
            set
            {
                this.driverLicenseNumber = value;
            }
        }

        /// <remarks/>
        public string Notes
        {
            get
            {
                return this.notes;
            }
            set
            {
                this.notes = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Item", IsNullable = false)]
        public Item[] Items
        {
            get
            {
                return this.items;
            }
            set
            {
                this.items = value;
            }
        }


        /// <remarks/>
        public MeasurementUnits MeasurementUnits
        {
            get
            {
                return this.measurementUnits;
            }
            set
            {
                this.measurementUnits = value;
            }
        }

        /// <remarks/>
        public long CreatorNetworkID
        {
            get
            {
                return this.creatorNetworkID;
            }
            set
            {
                this.creatorNetworkID = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CreatorNetworkIDSpecified
        {
            get
            {
                return this.creatorNetworkIDSpecified;
            }
            set
            {
                this.creatorNetworkIDSpecified = value;
            }
        }

        /// <remarks/>
        public ChargeList Charges
        {
            get
            {
                return this.charges;
            }
            set
            {
                this.charges = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Event", IsNullable = false)]
        public EventType[] Events
        {
            get
            {
                return this.events;
            }
            set
            {
                this.events = value;
            }
        }

        /// <remarks/>
        public Entity Division
        {
            get
            {
                return this.division;
            }
            set
            {
                this.division = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string TotalPieces
        {
            get
            {
                return this.totalPieces;
            }
            set
            {
                this.totalPieces = value;
            }
        }

        /// <remarks/>
        public WeightValue TotalWeight
        {
            get
            {
                return this.totalWeight;
            }
            set
            {
                this.totalWeight = value;
            }
        }

        /// <remarks/>
        public VolumeValue TotalVolume
        {
            get
            {
                return this.totalVolume;
            }
            set
            {
                this.totalVolume = value;
            }
        }

        /// <remarks/>
        public MoneyValue TotalValue
        {
            get
            {
                return this.totalValue;
            }
            set
            {
                this.totalValue = value;
            }
        }

        /// <remarks/>
        public VolumeWeightValue TotalVolumeWeight
        {
            get
            {
                return this.totalVolumeWeight;
            }
            set
            {
                this.totalVolumeWeight = value;
            }
        }

        /// <remarks/>
        public WeightValue ChargeableWeight
        {
            get
            {
                return this.chargeableWeight;
            }
            set
            {
                this.chargeableWeight = value;
            }
        }

        /// <remarks/>
        public Port OriginPort
        {
            get
            {
                return this.originPort;
            }
            set
            {
                this.originPort = value;
            }
        }

        /// <remarks/>
        public Port DestinationPort
        {
            get
            {
                return this.destinationPort;
            }
            set
            {
                this.destinationPort = value;
            }
        }

        /// <remarks/>
        public string SupplierName
        {
            get
            {
                return this.supplierName;
            }
            set
            {
                this.supplierName = value;
            }
        }

        /// <remarks/>
        public Address SupplierAddress
        {
            get
            {
                return this.supplierAddress;
            }
            set
            {
                this.supplierAddress = value;
            }
        }

        /// <remarks/>
        public Entity Supplier
        {
            get
            {
                return this.supplier;
            }
            set
            {
                this.supplier = value;
            }
        }

        /// <remarks/>
        public string SupplierInvoiceNumber
        {
            get
            {
                return this.supplierInvoiceNumber;
            }
            set
            {
                this.supplierInvoiceNumber = value;
            }
        }

        /// <remarks/>
        public string SupplierPONumber
        {
            get
            {
                return this.supplierPONumber;
            }
            set
            {
                this.supplierPONumber = value;
            }
        }

        /// <remarks/>
        public string FromQuoteNumber
        {
            get
            {
                return this.fromQuoteNumber;
            }
            set
            {
                this.fromQuoteNumber = value;
            }
        }

        /// <remarks/>
        public bool HasAttachments
        {
            get
            {
                return this.hasAttachments;
            }
            set
            {
                this.hasAttachments = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool HasAttachmentsSpecified
        {
            get
            {
                return this.hasAttachmentsSpecified;
            }
            set
            {
                this.hasAttachmentsSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Attachment", IsNullable = false)]
        public AttachmentType[] Attachments
        {
            get
            {
                return this.attachments;
            }
            set
            {
                this.attachments = value;
            }
        }

        /// <remarks/>
        public BondedEntryType BondedEntry
        {
            get
            {
                return this.bondedEntry;
            }
            set
            {
                this.bondedEntry = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool BondedEntrySpecified
        {
            get
            {
                return this.bondedEntrySpecified;
            }
            set
            {
                this.bondedEntrySpecified = value;
            }
        }

        /// <remarks/>
        public string BondedEntryNumber
        {
            get
            {
                return this.bondedEntryNumber;
            }
            set
            {
                this.bondedEntryNumber = value;
            }
        }

        /// <remarks/>
        public System.DateTime BondedEntryDate
        {
            get
            {
                return this.bondedEntryDate;
            }
            set
            {
                this.bondedEntryDate = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool BondedEntryDateSpecified
        {
            get
            {
                return this.bondedEntryDateSpecified;
            }
            set
            {
                this.bondedEntryDateSpecified = value;
            }
        }

        /// <remarks/>
        public string CarrierBookingNumber
        {
            get
            {
                return this.carrierBookingNumber;
            }
            set
            {
                this.carrierBookingNumber = value;
            }
        }

        /// <remarks/>
        public string FromBookingNumber
        {
            get
            {
                return this.fromBookingNumber;
            }
            set
            {
                this.fromBookingNumber = value;
            }
        }

        /// <remarks/>
        public Entity MainCarrier
        {
            get
            {
                return this.mainCarrier;
            }
            set
            {
                this.mainCarrier = value;
            }
        }

        /// <remarks/>
        public Entity BillingClient
        {
            get
            {
                return this.billingClient;
            }
            set
            {
                this.billingClient = value;
            }
        }

        /// <remarks/>
        public short LastItemID
        {
            get
            {
                return this.lastItemID;
            }
            set
            {
                this.lastItemID = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool LastItemIDSpecified
        {
            get
            {
                return this.lastItemIDSpecified;
            }
            set
            {
                this.lastItemIDSpecified = value;
            }
        }

        /// <remarks/>
        public string URL
        {
            get
            {
                return this.uRL;
            }
            set
            {
                this.uRL = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Custom", IsNullable = false)]
        public CustomType[] Customs
        {
            get
            {
                return this.customs;
            }
            set
            {
                this.customs = value;
            }
        }

        /// <remarks/>
        public bool IsOnline
        {
            get
            {
                return this.isOnline;
            }
            set
            {
                this.isOnline = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsOnlineSpecified
        {
            get
            {
                return this.isOnlineSpecified;
            }
            set
            {
                this.isOnlineSpecified = value;
            }
        }

        /// <remarks/>
        public HoldStatusType HoldStatus
        {
            get
            {
                return this.holdStatus;
            }
            set
            {
                this.holdStatus = value;
            }
        }

        /// <remarks/>
        public bool IsLiquidated
        {
            get
            {
                return this.isLiquidated;
            }
            set
            {
                this.isLiquidated = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsLiquidatedSpecified
        {
            get
            {
                return this.isLiquidatedSpecified;
            }
            set
            {
                this.isLiquidatedSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string GUID
        {
            get
            {
                return this.gUID;
            }
            set
            {
                this.gUID = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1", IncludeInSchema = false)]
    public enum ItemChoiceType
    {

        /// <remarks/>
        AirShipment,

        /// <remarks/>
        CargoRelease,

        /// <remarks/>
        GroundShipment,

        /// <remarks/>
        OceanShipment,

        /// <remarks/>
        PickupOrder,

        /// <remarks/>
        PurchaseOrder,

        /// <remarks/>
        SalesOrder,

        /// <remarks/>
        WarehouseReceipt,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("CargoCount", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class CargoCountType
    {

        private CargoCountTypeDesc type;

        private CountItem[] countItems;

        private ItemDefinition itemDefinition;

        /// <remarks/>
        public CargoCountTypeDesc Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("CountItem", IsNullable = false)]
        public CountItem[] CountItems
        {
            get
            {
                return this.countItems;
            }
            set
            {
                this.countItems = value;
            }
        }

        /// <remarks/>
        public ItemDefinition ItemDefinition
        {
            get
            {
                return this.itemDefinition;
            }
            set
            {
                this.itemDefinition = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum CargoCountTypeDesc
    {

        /// <remarks/>
        Informed,

        /// <remarks/>
        Blind,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("CountItem", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class CountItem
    {

        private string locationCode;

        private CountResultType[] countResults;

        private System.DateTime countedOn;

        private bool countedOnSpecified;

        private CountItemStatusType status;

        private bool statusSpecified;

        private string currentUserName;

        /// <remarks/>
        public string LocationCode
        {
            get
            {
                return this.locationCode;
            }
            set
            {
                this.locationCode = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("CountResult", IsNullable = false)]
        public CountResultType[] CountResults
        {
            get
            {
                return this.countResults;
            }
            set
            {
                this.countResults = value;
            }
        }

        /// <remarks/>
        public System.DateTime CountedOn
        {
            get
            {
                return this.countedOn;
            }
            set
            {
                this.countedOn = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CountedOnSpecified
        {
            get
            {
                return this.countedOnSpecified;
            }
            set
            {
                this.countedOnSpecified = value;
            }
        }

        /// <remarks/>
        public CountItemStatusType Status
        {
            get
            {
                return this.status;
            }
            set
            {
                this.status = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool StatusSpecified
        {
            get
            {
                return this.statusSpecified;
            }
            set
            {
                this.statusSpecified = value;
            }
        }

        /// <remarks/>
        public string CurrentUserName
        {
            get
            {
                return this.currentUserName;
            }
            set
            {
                this.currentUserName = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("CountResult", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class CountResultType
    {

        private string palletID;

        private string itemCode;

        private string serialNumber;

        private bool isNewSerialNumber;

        private bool isNewSerialNumberSpecified;

        private string databaseQuantity;

        private string countQuantity;

        private CountReasonType countReason;

        private Entity client;

        /// <remarks/>
        public string PalletID
        {
            get
            {
                return this.palletID;
            }
            set
            {
                this.palletID = value;
            }
        }

        /// <remarks/>
        public string ItemCode
        {
            get
            {
                return this.itemCode;
            }
            set
            {
                this.itemCode = value;
            }
        }

        /// <remarks/>
        public string SerialNumber
        {
            get
            {
                return this.serialNumber;
            }
            set
            {
                this.serialNumber = value;
            }
        }

        /// <remarks/>
        public bool IsNewSerialNumber
        {
            get
            {
                return this.isNewSerialNumber;
            }
            set
            {
                this.isNewSerialNumber = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsNewSerialNumberSpecified
        {
            get
            {
                return this.isNewSerialNumberSpecified;
            }
            set
            {
                this.isNewSerialNumberSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string DatabaseQuantity
        {
            get
            {
                return this.databaseQuantity;
            }
            set
            {
                this.databaseQuantity = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string CountQuantity
        {
            get
            {
                return this.countQuantity;
            }
            set
            {
                this.countQuantity = value;
            }
        }

        /// <remarks/>
        public CountReasonType CountReason
        {
            get
            {
                return this.countReason;
            }
            set
            {
                this.countReason = value;
            }
        }

        /// <remarks/>
        public Entity Client
        {
            get
            {
                return this.client;
            }
            set
            {
                this.client = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("CountReason", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class CountReasonType
    {

        private string description;

        private string name;

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum CountItemStatusType
    {

        /// <remarks/>
        Pending,

        /// <remarks/>
        Counted,

        /// <remarks/>
        Accepted,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("CargoMove", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class CargoMoveType
    {

        private string number;

        private System.DateTime createdOn;

        private bool createdOnSpecified;

        private System.DateTime startDate;

        private bool startDateSpecified;

        private System.DateTime completionDate;

        private bool completionDateSpecified;

        private Entity createdBy;

        private Entity issuedBy;

        private Entity division;

        private Item[] items;

        private CargoMoveItemRefType[] cargoMoveItemRefs;

        private LocationType destinationLocation;

        private Item destinationPallet;

        private LocationType mobileLocation;

        private CargoMoveStatusType status;

        private bool statusSpecified;

        private CargoMoveTypeDesc cargoMoveType1;

        private bool cargoMoveType1Specified;

        private string notes;

        private CustomType[] customs;

        private string gUID;

        /// <remarks/>
        public string Number
        {
            get
            {
                return this.number;
            }
            set
            {
                this.number = value;
            }
        }

        /// <remarks/>
        public System.DateTime CreatedOn
        {
            get
            {
                return this.createdOn;
            }
            set
            {
                this.createdOn = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CreatedOnSpecified
        {
            get
            {
                return this.createdOnSpecified;
            }
            set
            {
                this.createdOnSpecified = value;
            }
        }

        /// <remarks/>
        public System.DateTime StartDate
        {
            get
            {
                return this.startDate;
            }
            set
            {
                this.startDate = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool StartDateSpecified
        {
            get
            {
                return this.startDateSpecified;
            }
            set
            {
                this.startDateSpecified = value;
            }
        }

        /// <remarks/>
        public System.DateTime CompletionDate
        {
            get
            {
                return this.completionDate;
            }
            set
            {
                this.completionDate = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CompletionDateSpecified
        {
            get
            {
                return this.completionDateSpecified;
            }
            set
            {
                this.completionDateSpecified = value;
            }
        }

        /// <remarks/>
        public Entity CreatedBy
        {
            get
            {
                return this.createdBy;
            }
            set
            {
                this.createdBy = value;
            }
        }

        /// <remarks/>
        public Entity IssuedBy
        {
            get
            {
                return this.issuedBy;
            }
            set
            {
                this.issuedBy = value;
            }
        }

        /// <remarks/>
        public Entity Division
        {
            get
            {
                return this.division;
            }
            set
            {
                this.division = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Item", IsNullable = false)]
        public Item[] Items
        {
            get
            {
                return this.items;
            }
            set
            {
                this.items = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("CargoMoveItemRef", IsNullable = false)]
        public CargoMoveItemRefType[] CargoMoveItemRefs
        {
            get
            {
                return this.cargoMoveItemRefs;
            }
            set
            {
                this.cargoMoveItemRefs = value;
            }
        }

        /// <remarks/>
        public LocationType DestinationLocation
        {
            get
            {
                return this.destinationLocation;
            }
            set
            {
                this.destinationLocation = value;
            }
        }

        /// <remarks/>
        public Item DestinationPallet
        {
            get
            {
                return this.destinationPallet;
            }
            set
            {
                this.destinationPallet = value;
            }
        }

        /// <remarks/>
        public LocationType MobileLocation
        {
            get
            {
                return this.mobileLocation;
            }
            set
            {
                this.mobileLocation = value;
            }
        }

        /// <remarks/>
        public CargoMoveStatusType Status
        {
            get
            {
                return this.status;
            }
            set
            {
                this.status = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool StatusSpecified
        {
            get
            {
                return this.statusSpecified;
            }
            set
            {
                this.statusSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CargoMoveType")]
        public CargoMoveTypeDesc CargoMoveType1
        {
            get
            {
                return this.cargoMoveType1;
            }
            set
            {
                this.cargoMoveType1 = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CargoMoveType1Specified
        {
            get
            {
                return this.cargoMoveType1Specified;
            }
            set
            {
                this.cargoMoveType1Specified = value;
            }
        }

        /// <remarks/>
        public string Notes
        {
            get
            {
                return this.notes;
            }
            set
            {
                this.notes = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Custom", IsNullable = false)]
        public CustomType[] Customs
        {
            get
            {
                return this.customs;
            }
            set
            {
                this.customs = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string GUID
        {
            get
            {
                return this.gUID;
            }
            set
            {
                this.gUID = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("CargoMoveItemRef", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class CargoMoveItemRefType
    {

        private Item item;

        private LocationType originLocation;

        private string pieces;

        /// <remarks/>
        public Item Item
        {
            get
            {
                return this.item;
            }
            set
            {
                this.item = value;
            }
        }

        /// <remarks/>
        public LocationType OriginLocation
        {
            get
            {
                return this.originLocation;
            }
            set
            {
                this.originLocation = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string Pieces
        {
            get
            {
                return this.pieces;
            }
            set
            {
                this.pieces = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum CargoMoveStatusType
    {

        /// <remarks/>
        Pending,

        /// <remarks/>
        Loading,

        /// <remarks/>
        InTransit,

        /// <remarks/>
        Unloading,

        /// <remarks/>
        Completed,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum CargoMoveTypeDesc
    {

        /// <remarks/>
        Local,

        /// <remarks/>
        Transfer,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("Job", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class JobType
    {

        private System.DateTime createdOn;

        private string number;

        private string createdByName;

        private sbyte version;

        private bool versionSpecified;

        private JobStatusType status;

        private bool statusSpecified;

        private Entity division;

        private System.DateTime dueDate;

        private bool dueDateSpecified;

        private System.DateTime endDate;

        private bool endDateSpecified;

        private string customerName;

        private Entity customer;

        private string completionPercent;

        private string description;

        private Item[] items;

        private OrderType[] purchaseOrders;

        private OrderType[] salesOrders;

        private OperationType[] operations;

        private AccountingItemList accountingItems;

        private AttachmentType[] attachments;

        private EventType[] events;

        private string uRL;

        private CustomType[] customs;

        private string gUID;

        private string type;

        /// <remarks/>
        public System.DateTime CreatedOn
        {
            get
            {
                return this.createdOn;
            }
            set
            {
                this.createdOn = value;
            }
        }

        /// <remarks/>
        public string Number
        {
            get
            {
                return this.number;
            }
            set
            {
                this.number = value;
            }
        }

        /// <remarks/>
        public string CreatedByName
        {
            get
            {
                return this.createdByName;
            }
            set
            {
                this.createdByName = value;
            }
        }

        /// <remarks/>
        public sbyte Version
        {
            get
            {
                return this.version;
            }
            set
            {
                this.version = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool VersionSpecified
        {
            get
            {
                return this.versionSpecified;
            }
            set
            {
                this.versionSpecified = value;
            }
        }

        /// <remarks/>
        public JobStatusType Status
        {
            get
            {
                return this.status;
            }
            set
            {
                this.status = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool StatusSpecified
        {
            get
            {
                return this.statusSpecified;
            }
            set
            {
                this.statusSpecified = value;
            }
        }

        /// <remarks/>
        public Entity Division
        {
            get
            {
                return this.division;
            }
            set
            {
                this.division = value;
            }
        }

        /// <remarks/>
        public System.DateTime DueDate
        {
            get
            {
                return this.dueDate;
            }
            set
            {
                this.dueDate = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DueDateSpecified
        {
            get
            {
                return this.dueDateSpecified;
            }
            set
            {
                this.dueDateSpecified = value;
            }
        }

        /// <remarks/>
        public System.DateTime EndDate
        {
            get
            {
                return this.endDate;
            }
            set
            {
                this.endDate = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool EndDateSpecified
        {
            get
            {
                return this.endDateSpecified;
            }
            set
            {
                this.endDateSpecified = value;
            }
        }

        /// <remarks/>
        public string CustomerName
        {
            get
            {
                return this.customerName;
            }
            set
            {
                this.customerName = value;
            }
        }

        /// <remarks/>
        public Entity Customer
        {
            get
            {
                return this.customer;
            }
            set
            {
                this.customer = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string CompletionPercent
        {
            get
            {
                return this.completionPercent;
            }
            set
            {
                this.completionPercent = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Item", IsNullable = false)]
        public Item[] Items
        {
            get
            {
                return this.items;
            }
            set
            {
                this.items = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("PurchaseOrder", IsNullable = false)]
        public OrderType[] PurchaseOrders
        {
            get
            {
                return this.purchaseOrders;
            }
            set
            {
                this.purchaseOrders = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("SalesOrder", IsNullable = false)]
        public OrderType[] SalesOrders
        {
            get
            {
                return this.salesOrders;
            }
            set
            {
                this.salesOrders = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Operation", IsNullable = false)]
        public OperationType[] Operations
        {
            get
            {
                return this.operations;
            }
            set
            {
                this.operations = value;
            }
        }

        /// <remarks/>
        public AccountingItemList AccountingItems
        {
            get
            {
                return this.accountingItems;
            }
            set
            {
                this.accountingItems = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Attachment", IsNullable = false)]
        public AttachmentType[] Attachments
        {
            get
            {
                return this.attachments;
            }
            set
            {
                this.attachments = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Event", IsNullable = false)]
        public EventType[] Events
        {
            get
            {
                return this.events;
            }
            set
            {
                this.events = value;
            }
        }

        /// <remarks/>
        public string URL
        {
            get
            {
                return this.uRL;
            }
            set
            {
                this.uRL = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Custom", IsNullable = false)]
        public CustomType[] Customs
        {
            get
            {
                return this.customs;
            }
            set
            {
                this.customs = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string GUID
        {
            get
            {
                return this.gUID;
            }
            set
            {
                this.gUID = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum JobStatusType
    {

        /// <remarks/>
        Open,

        /// <remarks/>
        Closed,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class OperationType
    {

        private object item;

        private ItemChoiceType1 itemElementName;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("AirShipment", typeof(MagayaShipment))]
        [System.Xml.Serialization.XmlElementAttribute("CargoRelease", typeof(CargoReleaseType))]
        [System.Xml.Serialization.XmlElementAttribute("GroundShipment", typeof(MagayaShipment))]
        [System.Xml.Serialization.XmlElementAttribute("OceanShipment", typeof(MagayaShipment))]
        [System.Xml.Serialization.XmlElementAttribute("PickupOrder", typeof(PickupOrderType))]
        [System.Xml.Serialization.XmlElementAttribute("WarehouseReceipt", typeof(WarehouseReceipt))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
        public object Item
        {
            get
            {
                return this.item;
            }
            set
            {
                this.item = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemChoiceType1 ItemElementName
        {
            get
            {
                return this.itemElementName;
            }
            set
            {
                this.itemElementName = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1", IncludeInSchema = false)]
    public enum ItemChoiceType1
    {

        /// <remarks/>
        AirShipment,

        /// <remarks/>
        CargoRelease,

        /// <remarks/>
        GroundShipment,

        /// <remarks/>
        OceanShipment,

        /// <remarks/>
        PickupOrder,

        /// <remarks/>
        WarehouseReceipt,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("AccountingItems", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class AccountingItemList
    {

        private AccountingItem[] items;

        private ItemsChoiceType6[] itemsElementName;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Bill", typeof(AccountingItem))]
        [System.Xml.Serialization.XmlElementAttribute("BillCredit", typeof(AccountingItem))]
        [System.Xml.Serialization.XmlElementAttribute("CreditMemo", typeof(AccountingItem))]
        [System.Xml.Serialization.XmlElementAttribute("Invoice", typeof(AccountingItem))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
        public AccountingItem[] Items
        {
            get
            {
                return this.items;
            }
            set
            {
                this.items = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ItemsElementName")]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemsChoiceType6[] ItemsElementName
        {
            get
            {
                return this.itemsElementName;
            }
            set
            {
                this.itemsElementName = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1", IncludeInSchema = false)]
    public enum ItemsChoiceType6
    {

        /// <remarks/>
        Bill,

        /// <remarks/>
        BillCredit,

        /// <remarks/>
        CreditMemo,

        /// <remarks/>
        Invoice,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("Quotation", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class QuotationType
    {

        private bool isCommerceQuotation;

        private bool isCommerceQuotationSpecified;

        private System.DateTime createdOn;

        private string number;

        private string createdByName;

        private sbyte version;

        private bool versionSpecified;

        private ModeOfTransportation modeOfTransportation;

        private string modeOfTransportCode;

        private Entity issuedBy;

        private Address issuedByAddress;

        private string issuedByName;

        private string shipperName;

        private Address shipperAddress;

        private Entity shipper;

        private string consigneeName;

        private Address consigneeAddress;

        private Entity consignee;

        private string notes;

        private Item[] items;

        private MeasurementUnits measurementUnits;

        private ChargeList charges;

        private Entity division;

        private string totalPieces;

        private WeightValue totalWeight;

        private VolumeValue totalVolume;

        private MoneyValue totalValue;

        private VolumeWeightValue totalVolumeWeight;

        private WeightValue chargeableWeight;

        private CurrencyType currency;

        private CurrencyType homeCurrency;

        private double exchangeRate;

        private bool exchangeRateSpecified;

        private MoneyValue itemsAmount;

        private MoneyValue itemsAmountInCurrency;

        private MoneyValue chargesAmount;

        private MoneyValue chargesAmountInCurrency;

        private MoneyValue taxAmount;

        private MoneyValue taxAmountInCurrency;

        private MoneyValue totalAmount;

        private MoneyValue totalAmountInCurrency;

        private string descriptionOfGoods;

        private ServiceType service;

        private bool serviceSpecified;

        private Port originPort;

        private Port destinationPort;

        private Port receiptPort;

        private Port deliveryPort;

        private QuotationStatusType status;

        private bool statusSpecified;

        private System.DateTime expirationDate;

        private bool expirationDateSpecified;

        private string contactName;

        private Address contactAddress;

        private Entity contact;

        private string uRL;

        private EventType[] events;

        private bool hasAttachments;

        private bool hasAttachmentsSpecified;

        private AttachmentType[] attachments;

        private CustomType[] customs;

        private GUIDItem convertedTo;

        private string paymentTerms;

        private PaymentTerm paymentTermsRef;

        private IncotermType incoterm;

        private QuotationDirectionType direction;

        private bool directionSpecified;

        private bool isHazardous;

        private bool isHazardousSpecified;

        private Entity representative;

        private string representativeName;

        private Entity salesperson;

        private string salespersonName;

        private FrequencyType frequency;

        private bool frequencySpecified;

        private Entity carrier;

        private Entity preCarriageByEntity;

        private Entity onCarriageByEntity;

        private string gUID;

        private string type;

        /// <remarks/>
        public bool IsCommerceQuotation
        {
            get
            {
                return this.isCommerceQuotation;
            }
            set
            {
                this.isCommerceQuotation = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsCommerceQuotationSpecified
        {
            get
            {
                return this.isCommerceQuotationSpecified;
            }
            set
            {
                this.isCommerceQuotationSpecified = value;
            }
        }

        /// <remarks/>
        public System.DateTime CreatedOn
        {
            get
            {
                return this.createdOn;
            }
            set
            {
                this.createdOn = value;
            }
        }

        /// <remarks/>
        public string Number
        {
            get
            {
                return this.number;
            }
            set
            {
                this.number = value;
            }
        }

        /// <remarks/>
        public string CreatedByName
        {
            get
            {
                return this.createdByName;
            }
            set
            {
                this.createdByName = value;
            }
        }

        /// <remarks/>
        public sbyte Version
        {
            get
            {
                return this.version;
            }
            set
            {
                this.version = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool VersionSpecified
        {
            get
            {
                return this.versionSpecified;
            }
            set
            {
                this.versionSpecified = value;
            }
        }

        /// <remarks/>
        public ModeOfTransportation ModeOfTransportation
        {
            get
            {
                return this.modeOfTransportation;
            }
            set
            {
                this.modeOfTransportation = value;
            }
        }

        /// <remarks/>
        public string ModeOfTransportCode
        {
            get
            {
                return this.modeOfTransportCode;
            }
            set
            {
                this.modeOfTransportCode = value;
            }
        }

        /// <remarks/>
        public Entity IssuedBy
        {
            get
            {
                return this.issuedBy;
            }
            set
            {
                this.issuedBy = value;
            }
        }

        /// <remarks/>
        public Address IssuedByAddress
        {
            get
            {
                return this.issuedByAddress;
            }
            set
            {
                this.issuedByAddress = value;
            }
        }

        /// <remarks/>
        public string IssuedByName
        {
            get
            {
                return this.issuedByName;
            }
            set
            {
                this.issuedByName = value;
            }
        }

        /// <remarks/>
        public string ShipperName
        {
            get
            {
                return this.shipperName;
            }
            set
            {
                this.shipperName = value;
            }
        }

        /// <remarks/>
        public Address ShipperAddress
        {
            get
            {
                return this.shipperAddress;
            }
            set
            {
                this.shipperAddress = value;
            }
        }

        /// <remarks/>
        public Entity Shipper
        {
            get
            {
                return this.shipper;
            }
            set
            {
                this.shipper = value;
            }
        }

        /// <remarks/>
        public string ConsigneeName
        {
            get
            {
                return this.consigneeName;
            }
            set
            {
                this.consigneeName = value;
            }
        }

        /// <remarks/>
        public Address ConsigneeAddress
        {
            get
            {
                return this.consigneeAddress;
            }
            set
            {
                this.consigneeAddress = value;
            }
        }

        /// <remarks/>
        public Entity Consignee
        {
            get
            {
                return this.consignee;
            }
            set
            {
                this.consignee = value;
            }
        }

        /// <remarks/>
        public string Notes
        {
            get
            {
                return this.notes;
            }
            set
            {
                this.notes = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Item", IsNullable = false)]
        public Item[] Items
        {
            get
            {
                return this.items;
            }
            set
            {
                this.items = value;
            }
        }

        /// <remarks/>
        public MeasurementUnits MeasurementUnits
        {
            get
            {
                return this.measurementUnits;
            }
            set
            {
                this.measurementUnits = value;
            }
        }

        /// <remarks/>
        public ChargeList Charges
        {
            get
            {
                return this.charges;
            }
            set
            {
                this.charges = value;
            }
        }

        /// <remarks/>
        public Entity Division
        {
            get
            {
                return this.division;
            }
            set
            {
                this.division = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string TotalPieces
        {
            get
            {
                return this.totalPieces;
            }
            set
            {
                this.totalPieces = value;
            }
        }

        /// <remarks/>
        public WeightValue TotalWeight
        {
            get
            {
                return this.totalWeight;
            }
            set
            {
                this.totalWeight = value;
            }
        }

        /// <remarks/>
        public VolumeValue TotalVolume
        {
            get
            {
                return this.totalVolume;
            }
            set
            {
                this.totalVolume = value;
            }
        }

        /// <remarks/>
        public MoneyValue TotalValue
        {
            get
            {
                return this.totalValue;
            }
            set
            {
                this.totalValue = value;
            }
        }

        /// <remarks/>
        public VolumeWeightValue TotalVolumeWeight
        {
            get
            {
                return this.totalVolumeWeight;
            }
            set
            {
                this.totalVolumeWeight = value;
            }
        }

        /// <remarks/>
        public WeightValue ChargeableWeight
        {
            get
            {
                return this.chargeableWeight;
            }
            set
            {
                this.chargeableWeight = value;
            }
        }

        /// <remarks/>
        public CurrencyType Currency
        {
            get
            {
                return this.currency;
            }
            set
            {
                this.currency = value;
            }
        }

        /// <remarks/>
        public CurrencyType HomeCurrency
        {
            get
            {
                return this.homeCurrency;
            }
            set
            {
                this.homeCurrency = value;
            }
        }

        /// <remarks/>
        public double ExchangeRate
        {
            get
            {
                return this.exchangeRate;
            }
            set
            {
                this.exchangeRate = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ExchangeRateSpecified
        {
            get
            {
                return this.exchangeRateSpecified;
            }
            set
            {
                this.exchangeRateSpecified = value;
            }
        }

        /// <remarks/>
        public MoneyValue ItemsAmount
        {
            get
            {
                return this.itemsAmount;
            }
            set
            {
                this.itemsAmount = value;
            }
        }

        /// <remarks/>
        public MoneyValue ItemsAmountInCurrency
        {
            get
            {
                return this.itemsAmountInCurrency;
            }
            set
            {
                this.itemsAmountInCurrency = value;
            }
        }

        /// <remarks/>
        public MoneyValue ChargesAmount
        {
            get
            {
                return this.chargesAmount;
            }
            set
            {
                this.chargesAmount = value;
            }
        }

        /// <remarks/>
        public MoneyValue ChargesAmountInCurrency
        {
            get
            {
                return this.chargesAmountInCurrency;
            }
            set
            {
                this.chargesAmountInCurrency = value;
            }
        }

        /// <remarks/>
        public MoneyValue TaxAmount
        {
            get
            {
                return this.taxAmount;
            }
            set
            {
                this.taxAmount = value;
            }
        }

        /// <remarks/>
        public MoneyValue TaxAmountInCurrency
        {
            get
            {
                return this.taxAmountInCurrency;
            }
            set
            {
                this.taxAmountInCurrency = value;
            }
        }

        /// <remarks/>
        public MoneyValue TotalAmount
        {
            get
            {
                return this.totalAmount;
            }
            set
            {
                this.totalAmount = value;
            }
        }

        /// <remarks/>
        public MoneyValue TotalAmountInCurrency
        {
            get
            {
                return this.totalAmountInCurrency;
            }
            set
            {
                this.totalAmountInCurrency = value;
            }
        }

        /// <remarks/>
        public string DescriptionOfGoods
        {
            get
            {
                return this.descriptionOfGoods;
            }
            set
            {
                this.descriptionOfGoods = value;
            }
        }

        /// <remarks/>
        public ServiceType Service
        {
            get
            {
                return this.service;
            }
            set
            {
                this.service = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ServiceSpecified
        {
            get
            {
                return this.serviceSpecified;
            }
            set
            {
                this.serviceSpecified = value;
            }
        }

        /// <remarks/>
        public Port OriginPort
        {
            get
            {
                return this.originPort;
            }
            set
            {
                this.originPort = value;
            }
        }

        /// <remarks/>
        public Port DestinationPort
        {
            get
            {
                return this.destinationPort;
            }
            set
            {
                this.destinationPort = value;
            }
        }

        /// <remarks/>
        public Port ReceiptPort
        {
            get
            {
                return this.receiptPort;
            }
            set
            {
                this.receiptPort = value;
            }
        }

        /// <remarks/>
        public Port DeliveryPort
        {
            get
            {
                return this.deliveryPort;
            }
            set
            {
                this.deliveryPort = value;
            }
        }

        /// <remarks/>
        public QuotationStatusType Status
        {
            get
            {
                return this.status;
            }
            set
            {
                this.status = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool StatusSpecified
        {
            get
            {
                return this.statusSpecified;
            }
            set
            {
                this.statusSpecified = value;
            }
        }

        /// <remarks/>
        public System.DateTime ExpirationDate
        {
            get
            {
                return this.expirationDate;
            }
            set
            {
                this.expirationDate = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ExpirationDateSpecified
        {
            get
            {
                return this.expirationDateSpecified;
            }
            set
            {
                this.expirationDateSpecified = value;
            }
        }

        /// <remarks/>
        public string ContactName
        {
            get
            {
                return this.contactName;
            }
            set
            {
                this.contactName = value;
            }
        }

        /// <remarks/>
        public Address ContactAddress
        {
            get
            {
                return this.contactAddress;
            }
            set
            {
                this.contactAddress = value;
            }
        }

        /// <remarks/>
        public Entity Contact
        {
            get
            {
                return this.contact;
            }
            set
            {
                this.contact = value;
            }
        }

        /// <remarks/>
        public string URL
        {
            get
            {
                return this.uRL;
            }
            set
            {
                this.uRL = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Event", IsNullable = false)]
        public EventType[] Events
        {
            get
            {
                return this.events;
            }
            set
            {
                this.events = value;
            }
        }

        /// <remarks/>
        public bool HasAttachments
        {
            get
            {
                return this.hasAttachments;
            }
            set
            {
                this.hasAttachments = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool HasAttachmentsSpecified
        {
            get
            {
                return this.hasAttachmentsSpecified;
            }
            set
            {
                this.hasAttachmentsSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Attachment", IsNullable = false)]
        public AttachmentType[] Attachments
        {
            get
            {
                return this.attachments;
            }
            set
            {
                this.attachments = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Custom", IsNullable = false)]
        public CustomType[] Customs
        {
            get
            {
                return this.customs;
            }
            set
            {
                this.customs = value;
            }
        }

        /// <remarks/>
        public GUIDItem ConvertedTo
        {
            get
            {
                return this.convertedTo;
            }
            set
            {
                this.convertedTo = value;
            }
        }

        /// <remarks/>
        public string PaymentTerms
        {
            get
            {
                return this.paymentTerms;
            }
            set
            {
                this.paymentTerms = value;
            }
        }

        /// <remarks/>
        public PaymentTerm PaymentTermsRef
        {
            get
            {
                return this.paymentTermsRef;
            }
            set
            {
                this.paymentTermsRef = value;
            }
        }

        /// <remarks/>
        public IncotermType Incoterm
        {
            get
            {
                return this.incoterm;
            }
            set
            {
                this.incoterm = value;
            }
        }

        /// <remarks/>
        public QuotationDirectionType Direction
        {
            get
            {
                return this.direction;
            }
            set
            {
                this.direction = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DirectionSpecified
        {
            get
            {
                return this.directionSpecified;
            }
            set
            {
                this.directionSpecified = value;
            }
        }

        /// <remarks/>
        public bool IsHazardous
        {
            get
            {
                return this.isHazardous;
            }
            set
            {
                this.isHazardous = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsHazardousSpecified
        {
            get
            {
                return this.isHazardousSpecified;
            }
            set
            {
                this.isHazardousSpecified = value;
            }
        }

        /// <remarks/>
        public Entity Representative
        {
            get
            {
                return this.representative;
            }
            set
            {
                this.representative = value;
            }
        }

        /// <remarks/>
        public string RepresentativeName
        {
            get
            {
                return this.representativeName;
            }
            set
            {
                this.representativeName = value;
            }
        }

        /// <remarks/>
        public Entity Salesperson
        {
            get
            {
                return this.salesperson;
            }
            set
            {
                this.salesperson = value;
            }
        }

        /// <remarks/>
        public string SalespersonName
        {
            get
            {
                return this.salespersonName;
            }
            set
            {
                this.salespersonName = value;
            }
        }

        /// <remarks/>
        public FrequencyType Frequency
        {
            get
            {
                return this.frequency;
            }
            set
            {
                this.frequency = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool FrequencySpecified
        {
            get
            {
                return this.frequencySpecified;
            }
            set
            {
                this.frequencySpecified = value;
            }
        }

        /// <remarks/>
        public Entity Carrier
        {
            get
            {
                return this.carrier;
            }
            set
            {
                this.carrier = value;
            }
        }

        /// <remarks/>
        public Entity PreCarriageByEntity
        {
            get
            {
                return this.preCarriageByEntity;
            }
            set
            {
                this.preCarriageByEntity = value;
            }
        }

        /// <remarks/>
        public Entity OnCarriageByEntity
        {
            get
            {
                return this.onCarriageByEntity;
            }
            set
            {
                this.onCarriageByEntity = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string GUID
        {
            get
            {
                return this.gUID;
            }
            set
            {
                this.gUID = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum QuotationStatusType
    {

        /// <remarks/>
        Empty,

        /// <remarks/>
        Open,

        /// <remarks/>
        Posted,

        /// <remarks/>
        Lost,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum QuotationDirectionType
    {

        /// <remarks/>
        Outgoing,

        /// <remarks/>
        Incoming,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum FrequencyType
    {

        /// <remarks/>
        Other,

        /// <remarks/>
        Daily,

        /// <remarks/>
        Weekly,

        /// <remarks/>
        Biweekly,

        /// <remarks/>
        Monthly,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1", IncludeInSchema = false)]
    public enum ItemChoiceType2
    {

        /// <remarks/>
        AirBooking,

        /// <remarks/>
        AirShipment,

        /// <remarks/>
        Bill,

        /// <remarks/>
        BillCredit,

        /// <remarks/>
        CargoCount,

        /// <remarks/>
        CargoMove,

        /// <remarks/>
        CargoRelease,

        /// <remarks/>
        CreditMemo,

        /// <remarks/>
        GroundBooking,

        /// <remarks/>
        GroundShipment,

        /// <remarks/>
        Invoice,

        /// <remarks/>
        Job,

        /// <remarks/>
        OceanBooking,

        /// <remarks/>
        OceanShipment,

        /// <remarks/>
        PickupOrder,

        /// <remarks/>
        PurchaseOrder,

        /// <remarks/>
        Quotation,

        /// <remarks/>
        SalesOrder,

        /// <remarks/>
        WarehouseReceipt,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class TaskDefinitionType
    {

        private TaskType type;

        private string className;

        private string description;

        private bool isSystemTask;

        private bool isSystemTaskSpecified;

        /// <remarks/>
        public TaskType Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }

        /// <remarks/>
        public string ClassName
        {
            get
            {
                return this.className;
            }
            set
            {
                this.className = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        /// <remarks/>
        public bool IsSystemTask
        {
            get
            {
                return this.isSystemTask;
            }
            set
            {
                this.isSystemTask = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsSystemTaskSpecified
        {
            get
            {
                return this.isSystemTaskSpecified;
            }
            set
            {
                this.isSystemTaskSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum TaskType
    {

        /// <remarks/>
        CustomTask,

        /// <remarks/>
        WMS_Receive,

        /// <remarks/>
        WMS_Pick,

        /// <remarks/>
        WMS_Move,

        /// <remarks/>
        WMS_Count,

        /// <remarks/>
        WMS_Load,

        /// <remarks/>
        InventoryReplenishment,

        /// <remarks/>
        Delivery,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class JournalEntryLineType
    {

        private AccountDefinition account;

        private MoneyValue debitAmount;

        private MoneyValue creditAmount;

        private double exchangeRate;

        private bool exchangeRateSpecified;

        private string description;

        private Entity entity;

        /// <remarks/>
        public AccountDefinition Account
        {
            get
            {
                return this.account;
            }
            set
            {
                this.account = value;
            }
        }

        /// <remarks/>
        public MoneyValue DebitAmount
        {
            get
            {
                return this.debitAmount;
            }
            set
            {
                this.debitAmount = value;
            }
        }

        /// <remarks/>
        public MoneyValue CreditAmount
        {
            get
            {
                return this.creditAmount;
            }
            set
            {
                this.creditAmount = value;
            }
        }

        /// <remarks/>
        public double ExchangeRate
        {
            get
            {
                return this.exchangeRate;
            }
            set
            {
                this.exchangeRate = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ExchangeRateSpecified
        {
            get
            {
                return this.exchangeRateSpecified;
            }
            set
            {
                this.exchangeRateSpecified = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        /// <remarks/>
        public Entity Entity
        {
            get
            {
                return this.entity;
            }
            set
            {
                this.entity = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class DepositLineType
    {

        private AccountDefinition account;

        private MoneyValue depositAmount;

        private MoneyValue depositAmountInCurrency;

        private string description;

        private Entity entity;

        /// <remarks/>
        public AccountDefinition Account
        {
            get
            {
                return this.account;
            }
            set
            {
                this.account = value;
            }
        }

        /// <remarks/>
        public MoneyValue DepositAmount
        {
            get
            {
                return this.depositAmount;
            }
            set
            {
                this.depositAmount = value;
            }
        }

        /// <remarks/>
        public MoneyValue DepositAmountInCurrency
        {
            get
            {
                return this.depositAmountInCurrency;
            }
            set
            {
                this.depositAmountInCurrency = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        /// <remarks/>
        public Entity Entity
        {
            get
            {
                return this.entity;
            }
            set
            {
                this.entity = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class DepositItem
    {

        private string paymentGUID;

        private MoneyValue paymentAmount;

        private MoneyValue paymentAmountInCurrency;

        private System.DateTime createdOn;

        private bool createdOnSpecified;

        private string number;

        private Entity entity;

        private string references;

        /// <remarks/>
        public string PaymentGUID
        {
            get
            {
                return this.paymentGUID;
            }
            set
            {
                this.paymentGUID = value;
            }
        }

        /// <remarks/>
        public MoneyValue PaymentAmount
        {
            get
            {
                return this.paymentAmount;
            }
            set
            {
                this.paymentAmount = value;
            }
        }

        /// <remarks/>
        public MoneyValue PaymentAmountInCurrency
        {
            get
            {
                return this.paymentAmountInCurrency;
            }
            set
            {
                this.paymentAmountInCurrency = value;
            }
        }

        /// <remarks/>
        public System.DateTime CreatedOn
        {
            get
            {
                return this.createdOn;
            }
            set
            {
                this.createdOn = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CreatedOnSpecified
        {
            get
            {
                return this.createdOnSpecified;
            }
            set
            {
                this.createdOnSpecified = value;
            }
        }

        /// <remarks/>
        public string Number
        {
            get
            {
                return this.number;
            }
            set
            {
                this.number = value;
            }
        }

        /// <remarks/>
        public Entity Entity
        {
            get
            {
                return this.entity;
            }
            set
            {
                this.entity = value;
            }
        }

        /// <remarks/>
        public string References
        {
            get
            {
                return this.references;
            }
            set
            {
                this.references = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class CheckLineType
    {

        private AccountDefinition account;

        private MoneyValue amountPaid;

        private MoneyValue amountPaidInCurrency;

        private string description;

        private Entity entity;

        /// <remarks/>
        public AccountDefinition Account
        {
            get
            {
                return this.account;
            }
            set
            {
                this.account = value;
            }
        }

        /// <remarks/>
        public MoneyValue AmountPaid
        {
            get
            {
                return this.amountPaid;
            }
            set
            {
                this.amountPaid = value;
            }
        }

        /// <remarks/>
        public MoneyValue AmountPaidInCurrency
        {
            get
            {
                return this.amountPaidInCurrency;
            }
            set
            {
                this.amountPaidInCurrency = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        /// <remarks/>
        public Entity Entity
        {
            get
            {
                return this.entity;
            }
            set
            {
                this.entity = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class PaymentLineType
    {

        private AccountDefinition account;

        private MoneyValue paymentAmount;

        private MoneyValue paymentAmountInCurrency;

        private string description;

        private Entity entity;

        /// <remarks/>
        public AccountDefinition Account
        {
            get
            {
                return this.account;
            }
            set
            {
                this.account = value;
            }
        }

        /// <remarks/>
        public MoneyValue PaymentAmount
        {
            get
            {
                return this.paymentAmount;
            }
            set
            {
                this.paymentAmount = value;
            }
        }

        /// <remarks/>
        public MoneyValue PaymentAmountInCurrency
        {
            get
            {
                return this.paymentAmountInCurrency;
            }
            set
            {
                this.paymentAmountInCurrency = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        /// <remarks/>
        public Entity Entity
        {
            get
            {
                return this.entity;
            }
            set
            {
                this.entity = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class PaymentItem
    {

        private string itemPaidGUID;

        private TransactionType type;

        private MoneyValue originalAmount;

        private MoneyValue originalAmountInCurrency;

        private MoneyValue amountDue;

        private MoneyValue amountDueInCurrency;

        private MoneyValue amountPaid;

        private MoneyValue amountPaidInCurrency;

        private System.DateTime createdOn;

        private bool createdOnSpecified;

        private string number;

        private string references;

        /// <remarks/>
        public string ItemPaidGUID
        {
            get
            {
                return this.itemPaidGUID;
            }
            set
            {
                this.itemPaidGUID = value;
            }
        }

        /// <remarks/>
        public TransactionType Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }

        /// <remarks/>
        public MoneyValue OriginalAmount
        {
            get
            {
                return this.originalAmount;
            }
            set
            {
                this.originalAmount = value;
            }
        }

        /// <remarks/>
        public MoneyValue OriginalAmountInCurrency
        {
            get
            {
                return this.originalAmountInCurrency;
            }
            set
            {
                this.originalAmountInCurrency = value;
            }
        }

        /// <remarks/>
        public MoneyValue AmountDue
        {
            get
            {
                return this.amountDue;
            }
            set
            {
                this.amountDue = value;
            }
        }

        /// <remarks/>
        public MoneyValue AmountDueInCurrency
        {
            get
            {
                return this.amountDueInCurrency;
            }
            set
            {
                this.amountDueInCurrency = value;
            }
        }

        /// <remarks/>
        public MoneyValue AmountPaid
        {
            get
            {
                return this.amountPaid;
            }
            set
            {
                this.amountPaid = value;
            }
        }

        /// <remarks/>
        public MoneyValue AmountPaidInCurrency
        {
            get
            {
                return this.amountPaidInCurrency;
            }
            set
            {
                this.amountPaidInCurrency = value;
            }
        }

        /// <remarks/>
        public System.DateTime CreatedOn
        {
            get
            {
                return this.createdOn;
            }
            set
            {
                this.createdOn = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CreatedOnSpecified
        {
            get
            {
                return this.createdOnSpecified;
            }
            set
            {
                this.createdOnSpecified = value;
            }
        }

        /// <remarks/>
        public string Number
        {
            get
            {
                return this.number;
            }
            set
            {
                this.number = value;
            }
        }

        /// <remarks/>
        public string References
        {
            get
            {
                return this.references;
            }
            set
            {
                this.references = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class CustomChargeDefinition
    {

        private ChargeDefinition chargeDefinition;

        private MoneyValue amount;

        /// <remarks/>
        public ChargeDefinition ChargeDefinition
        {
            get
            {
                return this.chargeDefinition;
            }
            set
            {
                this.chargeDefinition = value;
            }
        }

        /// <remarks/>
        public MoneyValue Amount
        {
            get
            {
                return this.amount;
            }
            set
            {
                this.amount = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class FormulaType
    {

        private bool useScript;

        private bool useScriptSpecified;

        private string text;

        private WeightUnitType weightUnit;

        private bool weightUnitSpecified;

        private VolumeUnitType volumeUnit;

        private bool volumeUnitSpecified;

        private VolumeWeightUnitType volumeWeightUnit;

        private bool volumeWeightUnitSpecified;

        private TimeUnitType timeUnit;

        private bool timeUnitSpecified;

        /// <remarks/>
        public bool UseScript
        {
            get
            {
                return this.useScript;
            }
            set
            {
                this.useScript = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool UseScriptSpecified
        {
            get
            {
                return this.useScriptSpecified;
            }
            set
            {
                this.useScriptSpecified = value;
            }
        }

        /// <remarks/>
        public string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                this.text = value;
            }
        }

        /// <remarks/>
        public WeightUnitType WeightUnit
        {
            get
            {
                return this.weightUnit;
            }
            set
            {
                this.weightUnit = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool WeightUnitSpecified
        {
            get
            {
                return this.weightUnitSpecified;
            }
            set
            {
                this.weightUnitSpecified = value;
            }
        }

        /// <remarks/>
        public VolumeUnitType VolumeUnit
        {
            get
            {
                return this.volumeUnit;
            }
            set
            {
                this.volumeUnit = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool VolumeUnitSpecified
        {
            get
            {
                return this.volumeUnitSpecified;
            }
            set
            {
                this.volumeUnitSpecified = value;
            }
        }

        /// <remarks/>
        public VolumeWeightUnitType VolumeWeightUnit
        {
            get
            {
                return this.volumeWeightUnit;
            }
            set
            {
                this.volumeWeightUnit = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool VolumeWeightUnitSpecified
        {
            get
            {
                return this.volumeWeightUnitSpecified;
            }
            set
            {
                this.volumeWeightUnitSpecified = value;
            }
        }

        /// <remarks/>
        public TimeUnitType TimeUnit
        {
            get
            {
                return this.timeUnit;
            }
            set
            {
                this.timeUnit = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TimeUnitSpecified
        {
            get
            {
                return this.timeUnitSpecified;
            }
            set
            {
                this.timeUnitSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum TimeUnitType
    {

        /// <remarks/>
        sec,

        /// <remarks/>
        min,

        /// <remarks/>
        hour,

        /// <remarks/>
        day,

        /// <remarks/>
        week,

        /// <remarks/>
        month,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class FormulaRateType
    {

        private FormulaType formula;

        /// <remarks/>
        public FormulaType Formula
        {
            get
            {
                return this.formula;
            }
            set
            {
                this.formula = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class PackageRateType
    {

        private PackageType package;

        private MoneyValue price;

        /// <remarks/>
        public PackageType Package
        {
            get
            {
                return this.package;
            }
            set
            {
                this.package = value;
            }
        }

        /// <remarks/>
        public MoneyValue Price
        {
            get
            {
                return this.price;
            }
            set
            {
                this.price = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public partial class RangeRateType
    {

        private double moreThan;

        private MoneyValue price;

        /// <remarks/>
        public double MoreThan
        {
            get
            {
                return this.moreThan;
            }
            set
            {
                this.moreThan = value;
            }
        }

        /// <remarks/>
        public MoneyValue Price
        {
            get
            {
                return this.price;
            }
            set
            {
                this.price = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("Events", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class EventList
    {

        private EventType[] eventField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Event")]
        public EventType[] Event
        {
            get
            {
                return this.eventField;
            }
            set
            {
                this.eventField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("Ports", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class PortList
    {

        private Port[] port;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Port")]
        public Port[] Port
        {
            get
            {
                return this.port;
            }
            set
            {
                this.port = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("LocationDefinitions", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class LocationDefinitionList
    {

        private LocationDefinitionType[] locationDefinition;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("LocationDefinition")]
        public LocationDefinitionType[] LocationDefinition
        {
            get
            {
                return this.locationDefinition;
            }
            set
            {
                this.locationDefinition = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("ItemDefinitions", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class ItemDefinitionList
    {

        private ItemDefinition[] itemDefinition;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ItemDefinition")]
        public ItemDefinition[] ItemDefinition
        {
            get
            {
                return this.itemDefinition;
            }
            set
            {
                this.itemDefinition = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("Entities", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class EntityList
    {

        private Entity[] items;

        private ItemsChoiceType[] itemsElementName;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Carrier", typeof(Entity))]
        [System.Xml.Serialization.XmlElementAttribute("Client", typeof(Entity))]
        [System.Xml.Serialization.XmlElementAttribute("Employee", typeof(Entity))]
        [System.Xml.Serialization.XmlElementAttribute("ForwardingAgent", typeof(Entity))]
        [System.Xml.Serialization.XmlElementAttribute("Salesperson", typeof(Entity))]
        [System.Xml.Serialization.XmlElementAttribute("Vendor", typeof(Entity))]
        [System.Xml.Serialization.XmlElementAttribute("WarehouseProvider", typeof(Entity))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
        public Entity[] Items
        {
            get
            {
                return this.items;
            }
            set
            {
                this.items = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ItemsElementName")]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemsChoiceType[] ItemsElementName
        {
            get
            {
                return this.itemsElementName;
            }
            set
            {
                this.itemsElementName = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1", IncludeInSchema = false)]
    public enum ItemsChoiceType
    {

        /// <remarks/>
        Carrier,

        /// <remarks/>
        Client,

        /// <remarks/>
        Employee,

        /// <remarks/>
        ForwardingAgent,

        /// <remarks/>
        Salesperson,

        /// <remarks/>
        Vendor,

        /// <remarks/>
        WarehouseProvider,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("Carriers", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class CarrierList
    {

        private Entity[] carrier;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Carrier")]
        public Entity[] Carrier
        {
            get
            {
                return this.carrier;
            }
            set
            {
                this.carrier = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("ForwardingAgents", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class ForwardingAgentList
    {

        private Entity[] forwardingAgent;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ForwardingAgent")]
        public Entity[] ForwardingAgent
        {
            get
            {
                return this.forwardingAgent;
            }
            set
            {
                this.forwardingAgent = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("WarehouseProviders", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class WarehouseProviderList
    {

        private Entity[] warehouseProvider;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("WarehouseProvider")]
        public Entity[] WarehouseProvider
        {
            get
            {
                return this.warehouseProvider;
            }
            set
            {
                this.warehouseProvider = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("Clients", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class ClientList
    {

        private Entity[] client;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Client")]
        public Entity[] Client
        {
            get
            {
                return this.client;
            }
            set
            {
                this.client = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("Vendors", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class VendorList
    {

        private Entity[] vendor;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Vendor")]
        public Entity[] Vendor
        {
            get
            {
                return this.vendor;
            }
            set
            {
                this.vendor = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("Salespersons", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class SalespersonList
    {

        private Entity[] salesperson;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Salesperson")]
        public Entity[] Salesperson
        {
            get
            {
                return this.salesperson;
            }
            set
            {
                this.salesperson = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("Employees", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class EmployeeList
    {

        private Entity[] employee;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Employee")]
        public Entity[] Employee
        {
            get
            {
                return this.employee;
            }
            set
            {
                this.employee = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("ChargeDefinitions", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class ChargeDefinitionList
    {

        private ChargeDefinition[] chargeDefinition;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ChargeDefinition")]
        public ChargeDefinition[] ChargeDefinition
        {
            get
            {
                return this.chargeDefinition;
            }
            set
            {
                this.chargeDefinition = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("Locations", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class LocationList
    {

        private LocationType[] location;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Location")]
        public LocationType[] Location
        {
            get
            {
                return this.location;
            }
            set
            {
                this.location = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("AccountDefinitions", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class AccountDefinitionList
    {

        private AccountDefinition[] accountDefinition;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("AccountDefinition")]
        public AccountDefinition[] AccountDefinition
        {
            get
            {
                return this.accountDefinition;
            }
            set
            {
                this.accountDefinition = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("StandardRates", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class RateList
    {

        private RateType[] rate;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Rate")]
        public RateType[] Rate
        {
            get
            {
                return this.rate;
            }
            set
            {
                this.rate = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("Rate", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class RateType
    {

        private RateTypeDesc type;

        private string description;

        private CommodityTypeType commodityType;

        private ChargeDefinition chargeDefinition;

        private ServiceType service;

        private bool serviceSpecified;

        private Country originCountry;

        private Country destinationCountry;

        private Port originPort;

        private Port destinationPort;

        private Port receiptPort;

        private Port deliveryPort;

        private ModeOfTransportation modeOfTransportation;

        private MoneyValue minimum;

        private MoneyValue maximum;

        private ApplyByType applyBy;

        private WeightUnitType weightUnit;

        private bool weightUnitSpecified;

        private VolumeUnitType volumeUnit;

        private bool volumeUnitSpecified;

        private Entity entity;

        private Entity carrier;

        private bool pricePerRange;

        private bool pricePerRangeSpecified;

        private double rateMultiple;

        private bool rateMultipleSpecified;

        private string notes;

        private RangeRateType[] pieceRates;

        private RangeRateType[] weightRates;

        private RangeRateType[] volumeRates;

        private PackageRateType[] packageRates;

        private FormulaRateType formulaRate;

        private FrequencyType frequency;

        private bool frequencySpecified;

        private System.DateTime createdOn;

        private bool createdOnSpecified;

        private System.DateTime expirationDate;

        private bool expirationDateSpecified;

        private System.DateTime effectiveDate;

        private bool effectiveDateSpecified;

        private string contractNumber;

        private string contractAmendmentNumber;

        private CustomType[] customs;

        private double transitTime;

        private bool transitTimeSpecified;

        private bool useGrossWeight;

        private bool useGrossWeightSpecified;

        private bool isHazardous;

        private bool isHazardousSpecified;

        private bool isAutomaticCreateCharge;

        private bool isAutomaticCreateChargeSpecified;

        private string gUID;

        /// <remarks/>
        public RateTypeDesc Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        /// <remarks/>
        public CommodityTypeType CommodityType
        {
            get
            {
                return this.commodityType;
            }
            set
            {
                this.commodityType = value;
            }
        }

        /// <remarks/>
        public ChargeDefinition ChargeDefinition
        {
            get
            {
                return this.chargeDefinition;
            }
            set
            {
                this.chargeDefinition = value;
            }
        }

        /// <remarks/>
        public ServiceType Service
        {
            get
            {
                return this.service;
            }
            set
            {
                this.service = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ServiceSpecified
        {
            get
            {
                return this.serviceSpecified;
            }
            set
            {
                this.serviceSpecified = value;
            }
        }

        /// <remarks/>
        public Country OriginCountry
        {
            get
            {
                return this.originCountry;
            }
            set
            {
                this.originCountry = value;
            }
        }

        /// <remarks/>
        public Country DestinationCountry
        {
            get
            {
                return this.destinationCountry;
            }
            set
            {
                this.destinationCountry = value;
            }
        }

        /// <remarks/>
        public Port OriginPort
        {
            get
            {
                return this.originPort;
            }
            set
            {
                this.originPort = value;
            }
        }

        /// <remarks/>
        public Port DestinationPort
        {
            get
            {
                return this.destinationPort;
            }
            set
            {
                this.destinationPort = value;
            }
        }

        /// <remarks/>
        public Port ReceiptPort
        {
            get
            {
                return this.receiptPort;
            }
            set
            {
                this.receiptPort = value;
            }
        }

        /// <remarks/>
        public Port DeliveryPort
        {
            get
            {
                return this.deliveryPort;
            }
            set
            {
                this.deliveryPort = value;
            }
        }

        /// <remarks/>
        public ModeOfTransportation ModeOfTransportation
        {
            get
            {
                return this.modeOfTransportation;
            }
            set
            {
                this.modeOfTransportation = value;
            }
        }

        /// <remarks/>
        public MoneyValue Minimum
        {
            get
            {
                return this.minimum;
            }
            set
            {
                this.minimum = value;
            }
        }

        /// <remarks/>
        public MoneyValue Maximum
        {
            get
            {
                return this.maximum;
            }
            set
            {
                this.maximum = value;
            }
        }

        /// <remarks/>
        public ApplyByType ApplyBy
        {
            get
            {
                return this.applyBy;
            }
            set
            {
                this.applyBy = value;
            }
        }

        /// <remarks/>
        public WeightUnitType WeightUnit
        {
            get
            {
                return this.weightUnit;
            }
            set
            {
                this.weightUnit = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool WeightUnitSpecified
        {
            get
            {
                return this.weightUnitSpecified;
            }
            set
            {
                this.weightUnitSpecified = value;
            }
        }

        /// <remarks/>
        public VolumeUnitType VolumeUnit
        {
            get
            {
                return this.volumeUnit;
            }
            set
            {
                this.volumeUnit = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool VolumeUnitSpecified
        {
            get
            {
                return this.volumeUnitSpecified;
            }
            set
            {
                this.volumeUnitSpecified = value;
            }
        }

        /// <remarks/>
        public Entity Entity
        {
            get
            {
                return this.entity;
            }
            set
            {
                this.entity = value;
            }
        }

        /// <remarks/>
        public Entity Carrier
        {
            get
            {
                return this.carrier;
            }
            set
            {
                this.carrier = value;
            }
        }

        /// <remarks/>
        public bool PricePerRange
        {
            get
            {
                return this.pricePerRange;
            }
            set
            {
                this.pricePerRange = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool PricePerRangeSpecified
        {
            get
            {
                return this.pricePerRangeSpecified;
            }
            set
            {
                this.pricePerRangeSpecified = value;
            }
        }

        /// <remarks/>
        public double RateMultiple
        {
            get
            {
                return this.rateMultiple;
            }
            set
            {
                this.rateMultiple = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool RateMultipleSpecified
        {
            get
            {
                return this.rateMultipleSpecified;
            }
            set
            {
                this.rateMultipleSpecified = value;
            }
        }

        /// <remarks/>
        public string Notes
        {
            get
            {
                return this.notes;
            }
            set
            {
                this.notes = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("RangeRate", IsNullable = false)]
        public RangeRateType[] PieceRates
        {
            get
            {
                return this.pieceRates;
            }
            set
            {
                this.pieceRates = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("RangeRate", IsNullable = false)]
        public RangeRateType[] WeightRates
        {
            get
            {
                return this.weightRates;
            }
            set
            {
                this.weightRates = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("RangeRate", IsNullable = false)]
        public RangeRateType[] VolumeRates
        {
            get
            {
                return this.volumeRates;
            }
            set
            {
                this.volumeRates = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("PackageRate", IsNullable = false)]
        public PackageRateType[] PackageRates
        {
            get
            {
                return this.packageRates;
            }
            set
            {
                this.packageRates = value;
            }
        }

        /// <remarks/>
        public FormulaRateType FormulaRate
        {
            get
            {
                return this.formulaRate;
            }
            set
            {
                this.formulaRate = value;
            }
        }

        /// <remarks/>
        public FrequencyType Frequency
        {
            get
            {
                return this.frequency;
            }
            set
            {
                this.frequency = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool FrequencySpecified
        {
            get
            {
                return this.frequencySpecified;
            }
            set
            {
                this.frequencySpecified = value;
            }
        }

        /// <remarks/>
        public System.DateTime CreatedOn
        {
            get
            {
                return this.createdOn;
            }
            set
            {
                this.createdOn = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CreatedOnSpecified
        {
            get
            {
                return this.createdOnSpecified;
            }
            set
            {
                this.createdOnSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime ExpirationDate
        {
            get
            {
                return this.expirationDate;
            }
            set
            {
                this.expirationDate = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ExpirationDateSpecified
        {
            get
            {
                return this.expirationDateSpecified;
            }
            set
            {
                this.expirationDateSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime EffectiveDate
        {
            get
            {
                return this.effectiveDate;
            }
            set
            {
                this.effectiveDate = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool EffectiveDateSpecified
        {
            get
            {
                return this.effectiveDateSpecified;
            }
            set
            {
                this.effectiveDateSpecified = value;
            }
        }

        /// <remarks/>
        public string ContractNumber
        {
            get
            {
                return this.contractNumber;
            }
            set
            {
                this.contractNumber = value;
            }
        }

        /// <remarks/>
        public string ContractAmendmentNumber
        {
            get
            {
                return this.contractAmendmentNumber;
            }
            set
            {
                this.contractAmendmentNumber = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Custom", IsNullable = false)]
        public CustomType[] Customs
        {
            get
            {
                return this.customs;
            }
            set
            {
                this.customs = value;
            }
        }

        /// <remarks/>
        public double TransitTime
        {
            get
            {
                return this.transitTime;
            }
            set
            {
                this.transitTime = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TransitTimeSpecified
        {
            get
            {
                return this.transitTimeSpecified;
            }
            set
            {
                this.transitTimeSpecified = value;
            }
        }

        /// <remarks/>
        public bool UseGrossWeight
        {
            get
            {
                return this.useGrossWeight;
            }
            set
            {
                this.useGrossWeight = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool UseGrossWeightSpecified
        {
            get
            {
                return this.useGrossWeightSpecified;
            }
            set
            {
                this.useGrossWeightSpecified = value;
            }
        }

        /// <remarks/>
        public bool IsHazardous
        {
            get
            {
                return this.isHazardous;
            }
            set
            {
                this.isHazardous = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsHazardousSpecified
        {
            get
            {
                return this.isHazardousSpecified;
            }
            set
            {
                this.isHazardousSpecified = value;
            }
        }

        /// <remarks/>
        public bool IsAutomaticCreateCharge
        {
            get
            {
                return this.isAutomaticCreateCharge;
            }
            set
            {
                this.isAutomaticCreateCharge = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsAutomaticCreateChargeSpecified
        {
            get
            {
                return this.isAutomaticCreateChargeSpecified;
            }
            set
            {
                this.isAutomaticCreateChargeSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string GUID
        {
            get
            {
                return this.gUID;
            }
            set
            {
                this.gUID = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum RateTypeDesc
    {

        /// <remarks/>
        Standard,

        /// <remarks/>
        Client,

        /// <remarks/>
        Carrier,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("Items", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class ItemList
    {

        private Item[] item;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Item")]
        public Item[] Item
        {
            get
            {
                return this.item;
            }
            set
            {
                this.item = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("PackageTypes", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class PackageList
    {

        private PackageType[] package;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Package")]
        public PackageType[] Package
        {
            get
            {
                return this.package;
            }
            set
            {
                this.package = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("CustomDefinitions", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class CustomDefinitionList
    {

        private CustomDefinitionType[] customDefinition;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CustomDefinition")]
        public CustomDefinitionType[] CustomDefinition
        {
            get
            {
                return this.customDefinition;
            }
            set
            {
                this.customDefinition = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("GUIDItems", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class GUIDItemList
    {

        private GUIDItem[] gUIDItem;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("GUIDItem")]
        public GUIDItem[] GUIDItem
        {
            get
            {
                return this.gUIDItem;
            }
            set
            {
                this.gUIDItem = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("Currencies", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class CurrencyList
    {

        private CurrencyType[] currency;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Currency")]
        public CurrencyType[] Currency
        {
            get
            {
                return this.currency;
            }
            set
            {
                this.currency = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("Clause", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class ClauseType
    {

        private string name;

        private string description;

        private string categoryName;

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        /// <remarks/>
        public string CategoryName
        {
            get
            {
                return this.categoryName;
            }
            set
            {
                this.categoryName = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("Clauses", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class ClauseList
    {

        private ClauseType[] clause;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Clause")]
        public ClauseType[] Clause
        {
            get
            {
                return this.clause;
            }
            set
            {
                this.clause = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("CustomChargeDefinitions", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class CustomChargeDefinitionList
    {

        private CustomChargeDefinition[] customChargeDefinition;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CustomChargeDefinition")]
        public CustomChargeDefinition[] CustomChargeDefinition
        {
            get
            {
                return this.customChargeDefinition;
            }
            set
            {
                this.customChargeDefinition = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("EntityContacts", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class EntityContactList
    {

        private Entity[] entityContact;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("EntityContact")]
        public Entity[] EntityContact
        {
            get
            {
                return this.entityContact;
            }
            set
            {
                this.entityContact = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("EntityRoles", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class EntityRoleList
    {

        private EntityRoleType[] entityRole;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("EntityRole")]
        public EntityRoleType[] EntityRole
        {
            get
            {
                return this.entityRole;
            }
            set
            {
                this.entityRole = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("ItemDefCategory", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class ItemDefCategoryType
    {

        private string name;

        private string description;

        private ItemDefCategoryType[] subcategories;

        private string parentCategoryGUID;

        private string gUID;

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("ItemDefCategory", IsNullable = false)]
        public ItemDefCategoryType[] Subcategories
        {
            get
            {
                return this.subcategories;
            }
            set
            {
                this.subcategories = value;
            }
        }

        /// <remarks/>
        public string ParentCategoryGUID
        {
            get
            {
                return this.parentCategoryGUID;
            }
            set
            {
                this.parentCategoryGUID = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string GUID
        {
            get
            {
                return this.gUID;
            }
            set
            {
                this.gUID = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("ItemDefCategories", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class ItemDefCategoryList
    {

        private ItemDefCategoryType[] itemDefCategory;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ItemDefCategory")]
        public ItemDefCategoryType[] ItemDefCategory
        {
            get
            {
                return this.itemDefCategory;
            }
            set
            {
                this.itemDefCategory = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("RemoteUsers", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class RemoteUserList
    {

        private RemoteUserType[] remoteUser;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RemoteUser")]
        public RemoteUserType[] RemoteUser
        {
            get
            {
                return this.remoteUser;
            }
            set
            {
                this.remoteUser = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("RemoteUser", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class RemoteUserType
    {

        private string userName;

        private string password;

        private bool createDisabled;

        private bool createDisabledSpecified;

        private RemoteUserPermissionDesc[] permissionList;

        /// <remarks/>
        public string UserName
        {
            get
            {
                return this.userName;
            }
            set
            {
                this.userName = value;
            }
        }

        /// <remarks/>
        public string Password
        {
            get
            {
                return this.password;
            }
            set
            {
                this.password = value;
            }
        }

        /// <remarks/>
        public bool CreateDisabled
        {
            get
            {
                return this.createDisabled;
            }
            set
            {
                this.createDisabled = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CreateDisabledSpecified
        {
            get
            {
                return this.createDisabledSpecified;
            }
            set
            {
                this.createDisabledSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Permission", IsNullable = false)]
        public RemoteUserPermissionDesc[] PermissionList
        {
            get
            {
                return this.permissionList;
            }
            set
            {
                this.permissionList = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum RemoteUserPermissionDesc
    {

        /// <remarks/>
        TrackPickupOrders,

        /// <remarks/>
        TrackWarehouseReceipts,

        /// <remarks/>
        TrackCargoReleases,

        /// <remarks/>
        TrackInventoryItems,

        /// <remarks/>
        TrackInvoices,

        /// <remarks/>
        TrackShipments,

        /// <remarks/>
        AllowChat,

        /// <remarks/>
        TrackBookings,

        /// <remarks/>
        TrackQuotations,

        /// <remarks/>
        TrackJobs,

        /// <remarks/>
        TrackPurchaseOrders,

        /// <remarks/>
        TrackSalesOrders,

        /// <remarks/>
        SendOnlineBookings,

        /// <remarks/>
        QueryRates,

        /// <remarks/>
        SendOnlineSalesOrder,

        /// <remarks/>
        SendOnlineShippingInstructions,

        /// <remarks/>
        ApproveDisputeInvoice,

        /// <remarks/>
        SendOnlinePayment,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("WarehouseReceipts", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class WarehouseReceiptList
    {

        private WarehouseReceipt[] warehouseReceipt;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("WarehouseReceipt")]
        public WarehouseReceipt[] WarehouseReceipt
        {
            get
            {
                return this.warehouseReceipt;
            }
            set
            {
                this.warehouseReceipt = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("TheWarehouseReceiptCombo", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class TheWarehouseReceiptCombo 
    { 
        private WarehouseReceipt warehouseReceipt;
        private string actualAmountCollected;
        private string expectedAmountToCollect;
        private string magayaPaymentOption;
        private string magayaPaymentType;
        private IntlShipmentRequestDTL intlShipmentRequest;

        private int serviceCenterId;

        public int ServiceCenterId 
        {
            get { return serviceCenterId; }
            set { serviceCenterId = value; }
        }


        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("WarehouseReceipt", IsNullable = false)]
        public WarehouseReceipt WarehouseReceipt
        {
            get
            {
                return this.warehouseReceipt;
            }
            set
            {
                this.warehouseReceipt = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("ActualAmountCollected")]
        public string ActualAmountCollected
        {
            get 
            {
                return this.actualAmountCollected; 
            }
            set 
            { 
                this.actualAmountCollected = value; 
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("ExpectedAmountToCollect")]
        public string ExpectedAmountToCollect
        {
            get 
            { 
                return expectedAmountToCollect; 
            }
            set 
            { 
                expectedAmountToCollect = value; 
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("MagayaPaymentOption")]
        public string MagayaPaymentOption
        {
            get 
            { 
                return magayaPaymentOption; 
            }
            set 
            { 
                magayaPaymentOption = value; 
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("MagayaPaymentType")]
        public string MagayaPaymentType
        {
            get 
            { 
                return magayaPaymentType; 
            }
            set 
            { 
                magayaPaymentType = value; 
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("IntlShipmentRequest")]
        public IntlShipmentRequestDTL IntlShipmentRequest
        {
            get
            {
                return intlShipmentRequest;
            }
            set
            {
                intlShipmentRequest = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("PickupOrders", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class PickupOrderList
    {

        private PickupOrderType[] pickupOrder;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PickupOrder")]
        public PickupOrderType[] PickupOrder
        {
            get
            {
                return this.pickupOrder;
            }
            set
            {
                this.pickupOrder = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("CargoReleases", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class CargoReleaseList
    {

        private CargoReleaseType[] cargoRelease;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CargoRelease")]
        public CargoReleaseType[] CargoRelease
        {
            get
            {
                return this.cargoRelease;
            }
            set
            {
                this.cargoRelease = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("Bookings", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class BookingList
    {

        private MagayaShipment[] items;

        private ItemsChoiceType2[] itemsElementName;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("AirBooking", typeof(MagayaShipment))]
        [System.Xml.Serialization.XmlElementAttribute("GroundBooking", typeof(MagayaShipment))]
        [System.Xml.Serialization.XmlElementAttribute("OceanBooking", typeof(MagayaShipment))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
        public MagayaShipment[] Items
        {
            get
            {
                return this.items;
            }
            set
            {
                this.items = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ItemsElementName")]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemsChoiceType2[] ItemsElementName
        {
            get
            {
                return this.itemsElementName;
            }
            set
            {
                this.itemsElementName = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1", IncludeInSchema = false)]
    public enum ItemsChoiceType2
    {

        /// <remarks/>
        AirBooking,

        /// <remarks/>
        GroundBooking,

        /// <remarks/>
        OceanBooking,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("Trips", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class TripList
    {

        private MagayaShipment[] items;

        private ItemsChoiceType3[] itemsElementName;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("AirTrip", typeof(ItemsChoiceType3))]
        [System.Xml.Serialization.XmlElementAttribute("GroundTrip", typeof(ItemsChoiceType3))]
        [System.Xml.Serialization.XmlElementAttribute("OceanTrip", typeof(ItemsChoiceType3))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
        public MagayaShipment[] Items
        {
            get
            {
                return this.items;
            }
            set
            {
                this.items = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ItemsElementName")]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemsChoiceType3[] ItemsElementName
        {
            get
            {
                return this.itemsElementName;
            }
            set
            {
                this.itemsElementName = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1", IncludeInSchema = false)]
    public enum ItemsChoiceType3
    {

        /// <remarks/>
        AirTrip,

        /// <remarks/>
        GroundTrip,

        /// <remarks/>
        OceanTrip,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("Folder", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class FolderType
    {

        private string name;

        private FolderType[] subFolders;

        private DocumentType[] documents;

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Folder", IsNullable = false)]
        public FolderType[] SubFolders
        {
            get
            {
                return this.subFolders;
            }
            set
            {
                this.subFolders = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Document", IsNullable = false)]
        public DocumentType[] Documents
        {
            get
            {
                return this.documents;
            }
            set
            {
                this.documents = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("Folders", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class FolderList
    {

        private FolderType[] folder;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Folder")]
        public FolderType[] Folder
        {
            get
            {
                return this.folder;
            }
            set
            {
                this.folder = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("PurchaseOrders", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class PurchaseOrderList
    {

        private OrderType[] purchaseOrder;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PurchaseOrder")]
        public OrderType[] PurchaseOrder
        {
            get
            {
                return this.purchaseOrder;
            }
            set
            {
                this.purchaseOrder = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("SalesOrders", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class SalesOrderList
    {

        private OrderType[] salesOrder;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SalesOrder")]
        public OrderType[] SalesOrder
        {
            get
            {
                return this.salesOrder;
            }
            set
            {
                this.salesOrder = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("Invoices", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class InvoiceList
    {

        private AccountingItem[] items;

        private ItemsChoiceType4[] itemsElementName;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CreditMemo", typeof(AccountingItem))]
        [System.Xml.Serialization.XmlElementAttribute("Invoice", typeof(AccountingItem))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
        public AccountingItem[] Items
        {
            get
            {
                return this.items;
            }
            set
            {
                this.items = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ItemsElementName")]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemsChoiceType4[] ItemsElementName
        {
            get
            {
                return this.itemsElementName;
            }
            set
            {
                this.itemsElementName = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1", IncludeInSchema = false)]
    public enum ItemsChoiceType4
    {

        /// <remarks/>
        CreditMemo,

        /// <remarks/>
        Invoice,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("Bills", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class BillList
    {

        private AccountingItem[] items;

        private ItemsChoiceType5[] itemsElementName;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Bill", typeof(AccountingItem))]
        [System.Xml.Serialization.XmlElementAttribute("BillCredit", typeof(AccountingItem))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
        public AccountingItem[] Items
        {
            get
            {
                return this.items;
            }
            set
            {
                this.items = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ItemsElementName")]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemsChoiceType5[] ItemsElementName
        {
            get
            {
                return this.itemsElementName;
            }
            set
            {
                this.itemsElementName = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1", IncludeInSchema = false)]
    public enum ItemsChoiceType5
    {

        /// <remarks/>
        Bill,

        /// <remarks/>
        BillCredit,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("Payment", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class PaymentType
    {

        private System.DateTime createdOn;

        private string number;

        private Entity createdBy;

        private Entity issuedBy;

        private AccountDefinition gLAccount;

        private AccountDefinition bankAccount;

        private Entity entity;

        private MoneyValue totalAmount;

        private CurrencyType homeCurrency;

        private CurrencyType currency;

        private double exchangeRate;

        private MoneyValue totalAmountInCurrency;

        private Entity division;

        private PaymentItem[] paymentItems;

        private PaymentLineType[] paymentLines;

        private string notes;

        private string description;

        private MoneyValue creditAmount;

        private MoneyValue creditAmountInCurrency;

        private MoneyValue usedCreditAmount;

        private MoneyValue usedCreditAmountInCurrency;

        private string depositGUID;

        private bool isPrinted;

        private bool isPrintedSpecified;

        private bool hasAttachments;

        private bool hasAttachmentsSpecified;

        private AttachmentType[] attachments;

        private CustomType[] customs;

        private string gUID;

        private string type;

        /// <remarks/>
        public System.DateTime CreatedOn
        {
            get
            {
                return this.createdOn;
            }
            set
            {
                this.createdOn = value;
            }
        }

        /// <remarks/>
        public string Number
        {
            get
            {
                return this.number;
            }
            set
            {
                this.number = value;
            }
        }

        /// <remarks/>
        public Entity CreatedBy
        {
            get
            {
                return this.createdBy;
            }
            set
            {
                this.createdBy = value;
            }
        }

        /// <remarks/>
        public Entity IssuedBy
        {
            get
            {
                return this.issuedBy;
            }
            set
            {
                this.issuedBy = value;
            }
        }

        /// <remarks/>
        public AccountDefinition GLAccount
        {
            get
            {
                return this.gLAccount;
            }
            set
            {
                this.gLAccount = value;
            }
        }

        /// <remarks/>
        public AccountDefinition BankAccount
        {
            get
            {
                return this.bankAccount;
            }
            set
            {
                this.bankAccount = value;
            }
        }

        /// <remarks/>
        public Entity Entity
        {
            get
            {
                return this.entity;
            }
            set
            {
                this.entity = value;
            }
        }

        /// <remarks/>
        public MoneyValue TotalAmount
        {
            get
            {
                return this.totalAmount;
            }
            set
            {
                this.totalAmount = value;
            }
        }

        /// <remarks/>
        public CurrencyType HomeCurrency
        {
            get
            {
                return this.homeCurrency;
            }
            set
            {
                this.homeCurrency = value;
            }
        }

        /// <remarks/>
        public CurrencyType Currency
        {
            get
            {
                return this.currency;
            }
            set
            {
                this.currency = value;
            }
        }

        /// <remarks/>
        public double ExchangeRate
        {
            get
            {
                return this.exchangeRate;
            }
            set
            {
                this.exchangeRate = value;
            }
        }

        /// <remarks/>
        public MoneyValue TotalAmountInCurrency
        {
            get
            {
                return this.totalAmountInCurrency;
            }
            set
            {
                this.totalAmountInCurrency = value;
            }
        }

        /// <remarks/>
        public Entity Division
        {
            get
            {
                return this.division;
            }
            set
            {
                this.division = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("PaymentItem", IsNullable = false)]
        public PaymentItem[] PaymentItems
        {
            get
            {
                return this.paymentItems;
            }
            set
            {
                this.paymentItems = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("PaymentLine", IsNullable = false)]
        public PaymentLineType[] PaymentLines
        {
            get
            {
                return this.paymentLines;
            }
            set
            {
                this.paymentLines = value;
            }
        }

        /// <remarks/>
        public string Notes
        {
            get
            {
                return this.notes;
            }
            set
            {
                this.notes = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        /// <remarks/>
        public MoneyValue CreditAmount
        {
            get
            {
                return this.creditAmount;
            }
            set
            {
                this.creditAmount = value;
            }
        }

        /// <remarks/>
        public MoneyValue CreditAmountInCurrency
        {
            get
            {
                return this.creditAmountInCurrency;
            }
            set
            {
                this.creditAmountInCurrency = value;
            }
        }

        /// <remarks/>
        public MoneyValue UsedCreditAmount
        {
            get
            {
                return this.usedCreditAmount;
            }
            set
            {
                this.usedCreditAmount = value;
            }
        }

        /// <remarks/>
        public MoneyValue UsedCreditAmountInCurrency
        {
            get
            {
                return this.usedCreditAmountInCurrency;
            }
            set
            {
                this.usedCreditAmountInCurrency = value;
            }
        }

        /// <remarks/>
        public string DepositGUID
        {
            get
            {
                return this.depositGUID;
            }
            set
            {
                this.depositGUID = value;
            }
        }

        /// <remarks/>
        public bool IsPrinted
        {
            get
            {
                return this.isPrinted;
            }
            set
            {
                this.isPrinted = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsPrintedSpecified
        {
            get
            {
                return this.isPrintedSpecified;
            }
            set
            {
                this.isPrintedSpecified = value;
            }
        }

        /// <remarks/>
        public bool HasAttachments
        {
            get
            {
                return this.hasAttachments;
            }
            set
            {
                this.hasAttachments = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool HasAttachmentsSpecified
        {
            get
            {
                return this.hasAttachmentsSpecified;
            }
            set
            {
                this.hasAttachmentsSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Attachment", IsNullable = false)]
        public AttachmentType[] Attachments
        {
            get
            {
                return this.attachments;
            }
            set
            {
                this.attachments = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Custom", IsNullable = false)]
        public CustomType[] Customs
        {
            get
            {
                return this.customs;
            }
            set
            {
                this.customs = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string GUID
        {
            get
            {
                return this.gUID;
            }
            set
            {
                this.gUID = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("Payments", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class PaymentList
    {

        private PaymentType[] items;

        private ItemsChoiceType7[] itemsElementName;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Payment", typeof(PaymentType))]
        [System.Xml.Serialization.XmlElementAttribute("RefundPayment", typeof(PaymentType))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
        public PaymentType[] Items
        {
            get
            {
                return this.items;
            }
            set
            {
                this.items = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ItemsElementName")]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemsChoiceType7[] ItemsElementName
        {
            get
            {
                return this.itemsElementName;
            }
            set
            {
                this.itemsElementName = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1", IncludeInSchema = false)]
    public enum ItemsChoiceType7
    {

        /// <remarks/>
        Payment,

        /// <remarks/>
        RefundPayment,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("Check", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class CheckType
    {

        private CheckSubType type;

        private System.DateTime createdOn;

        private string number;

        private Entity createdBy;

        private Entity issuedBy;

        private AccountDefinition gLAccount;

        private AccountDefinition bankAccount;

        private Entity entity;

        private MoneyValue totalAmount;

        private CurrencyType homeCurrency;

        private CurrencyType currency;

        private double exchangeRate;

        private MoneyValue totalAmountInCurrency;

        private Entity division;

        private PaymentItem[] paymentItems;

        private CheckLineType[] checkLines;

        private string notes;

        private string description;

        private bool isPrinted;

        private bool isPrintedSpecified;

        private bool hasAttachments;

        private bool hasAttachmentsSpecified;

        private AttachmentType[] attachments;

        private CustomType[] customs;

        private string gUID;

        private string type1;

        /// <remarks/>
        public CheckSubType Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }

        /// <remarks/>
        public System.DateTime CreatedOn
        {
            get
            {
                return this.createdOn;
            }
            set
            {
                this.createdOn = value;
            }
        }

        /// <remarks/>
        public string Number
        {
            get
            {
                return this.number;
            }
            set
            {
                this.number = value;
            }
        }

        /// <remarks/>
        public Entity CreatedBy
        {
            get
            {
                return this.createdBy;
            }
            set
            {
                this.createdBy = value;
            }
        }

        /// <remarks/>
        public Entity IssuedBy
        {
            get
            {
                return this.issuedBy;
            }
            set
            {
                this.issuedBy = value;
            }
        }

        /// <remarks/>
        public AccountDefinition GLAccount
        {
            get
            {
                return this.gLAccount;
            }
            set
            {
                this.gLAccount = value;
            }
        }

        /// <remarks/>
        public AccountDefinition BankAccount
        {
            get
            {
                return this.bankAccount;
            }
            set
            {
                this.bankAccount = value;
            }
        }

        /// <remarks/>
        public Entity Entity
        {
            get
            {
                return this.entity;
            }
            set
            {
                this.entity = value;
            }
        }

        /// <remarks/>
        public MoneyValue TotalAmount
        {
            get
            {
                return this.totalAmount;
            }
            set
            {
                this.totalAmount = value;
            }
        }

        /// <remarks/>
        public CurrencyType HomeCurrency
        {
            get
            {
                return this.homeCurrency;
            }
            set
            {
                this.homeCurrency = value;
            }
        }

        /// <remarks/>
        public CurrencyType Currency
        {
            get
            {
                return this.currency;
            }
            set
            {
                this.currency = value;
            }
        }

        /// <remarks/>
        public double ExchangeRate
        {
            get
            {
                return this.exchangeRate;
            }
            set
            {
                this.exchangeRate = value;
            }
        }

        /// <remarks/>
        public MoneyValue TotalAmountInCurrency
        {
            get
            {
                return this.totalAmountInCurrency;
            }
            set
            {
                this.totalAmountInCurrency = value;
            }
        }

        /// <remarks/>
        public Entity Division
        {
            get
            {
                return this.division;
            }
            set
            {
                this.division = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("PaymentItem", IsNullable = false)]
        public PaymentItem[] PaymentItems
        {
            get
            {
                return this.paymentItems;
            }
            set
            {
                this.paymentItems = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("CheckLine", IsNullable = false)]
        public CheckLineType[] CheckLines
        {
            get
            {
                return this.checkLines;
            }
            set
            {
                this.checkLines = value;
            }
        }

        /// <remarks/>
        public string Notes
        {
            get
            {
                return this.notes;
            }
            set
            {
                this.notes = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        /// <remarks/>
        public bool IsPrinted
        {
            get
            {
                return this.isPrinted;
            }
            set
            {
                this.isPrinted = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsPrintedSpecified
        {
            get
            {
                return this.isPrintedSpecified;
            }
            set
            {
                this.isPrintedSpecified = value;
            }
        }

        /// <remarks/>
        public bool HasAttachments
        {
            get
            {
                return this.hasAttachments;
            }
            set
            {
                this.hasAttachments = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool HasAttachmentsSpecified
        {
            get
            {
                return this.hasAttachmentsSpecified;
            }
            set
            {
                this.hasAttachmentsSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Attachment", IsNullable = false)]
        public AttachmentType[] Attachments
        {
            get
            {
                return this.attachments;
            }
            set
            {
                this.attachments = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Custom", IsNullable = false)]
        public CustomType[] Customs
        {
            get
            {
                return this.customs;
            }
            set
            {
                this.customs = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string GUID
        {
            get
            {
                return this.gUID;
            }
            set
            {
                this.gUID = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("Type")]
        public string Type1
        {
            get
            {
                return this.type1;
            }
            set
            {
                this.type1 = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum CheckSubType
    {

        /// <remarks/>
        BillPaymentCheck,

        /// <remarks/>
        ExpenseCheck,

        /// <remarks/>
        RefundCheck,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("Checks", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class CheckList
    {

        private CheckType[] check;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Check")]
        public CheckType[] Check
        {
            get
            {
                return this.check;
            }
            set
            {
                this.check = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("Deposit", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class DepositType
    {

        private DepositSubType type;

        private System.DateTime createdOn;

        private Entity createdBy;

        private Entity issuedBy;

        private AccountDefinition bankAccount;

        private MoneyValue totalAmount;

        private CurrencyType homeCurrency;

        private CurrencyType currency;

        private double exchangeRate;

        private MoneyValue totalAmountInCurrency;

        private Entity division;

        private DepositItem[] depositItems;

        private DepositLineType[] depositLines;

        private string notes;

        private string description;

        private bool isPrinted;

        private bool isPrintedSpecified;

        private bool hasAttachments;

        private bool hasAttachmentsSpecified;

        private AttachmentType[] attachments;

        private CustomType[] customs;

        private string gUID;

        private string type1;

        /// <remarks/>
        public DepositSubType Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }

        /// <remarks/>
        public System.DateTime CreatedOn
        {
            get
            {
                return this.createdOn;
            }
            set
            {
                this.createdOn = value;
            }
        }

        /// <remarks/>
        public Entity CreatedBy
        {
            get
            {
                return this.createdBy;
            }
            set
            {
                this.createdBy = value;
            }
        }

        /// <remarks/>
        public Entity IssuedBy
        {
            get
            {
                return this.issuedBy;
            }
            set
            {
                this.issuedBy = value;
            }
        }

        /// <remarks/>
        public AccountDefinition BankAccount
        {
            get
            {
                return this.bankAccount;
            }
            set
            {
                this.bankAccount = value;
            }
        }

        /// <remarks/>
        public MoneyValue TotalAmount
        {
            get
            {
                return this.totalAmount;
            }
            set
            {
                this.totalAmount = value;
            }
        }

        /// <remarks/>
        public CurrencyType HomeCurrency
        {
            get
            {
                return this.homeCurrency;
            }
            set
            {
                this.homeCurrency = value;
            }
        }

        /// <remarks/>
        public CurrencyType Currency
        {
            get
            {
                return this.currency;
            }
            set
            {
                this.currency = value;
            }
        }

        /// <remarks/>
        public double ExchangeRate
        {
            get
            {
                return this.exchangeRate;
            }
            set
            {
                this.exchangeRate = value;
            }
        }

        /// <remarks/>
        public MoneyValue TotalAmountInCurrency
        {
            get
            {
                return this.totalAmountInCurrency;
            }
            set
            {
                this.totalAmountInCurrency = value;
            }
        }

        /// <remarks/>
        public Entity Division
        {
            get
            {
                return this.division;
            }
            set
            {
                this.division = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("DepositItem", IsNullable = false)]
        public DepositItem[] DepositItems
        {
            get
            {
                return this.depositItems;
            }
            set
            {
                this.depositItems = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("DepositLine", IsNullable = false)]
        public DepositLineType[] DepositLines
        {
            get
            {
                return this.depositLines;
            }
            set
            {
                this.depositLines = value;
            }
        }

        /// <remarks/>
        public string Notes
        {
            get
            {
                return this.notes;
            }
            set
            {
                this.notes = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        /// <remarks/>
        public bool IsPrinted
        {
            get
            {
                return this.isPrinted;
            }
            set
            {
                this.isPrinted = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsPrintedSpecified
        {
            get
            {
                return this.isPrintedSpecified;
            }
            set
            {
                this.isPrintedSpecified = value;
            }
        }

        /// <remarks/>
        public bool HasAttachments
        {
            get
            {
                return this.hasAttachments;
            }
            set
            {
                this.hasAttachments = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool HasAttachmentsSpecified
        {
            get
            {
                return this.hasAttachmentsSpecified;
            }
            set
            {
                this.hasAttachmentsSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Attachment", IsNullable = false)]
        public AttachmentType[] Attachments
        {
            get
            {
                return this.attachments;
            }
            set
            {
                this.attachments = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Custom", IsNullable = false)]
        public CustomType[] Customs
        {
            get
            {
                return this.customs;
            }
            set
            {
                this.customs = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string GUID
        {
            get
            {
                return this.gUID;
            }
            set
            {
                this.gUID = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("Type")]
        public string Type1
        {
            get
            {
                return this.type1;
            }
            set
            {
                this.type1 = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum DepositSubType
    {

        /// <remarks/>
        PaymentsDeposit,

        /// <remarks/>
        StraightDeposit,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("Deposits", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class DepositList
    {

        private DepositType[] deposit;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Deposit")]
        public DepositType[] Deposit
        {
            get
            {
                return this.deposit;
            }
            set
            {
                this.deposit = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("JournalEntry", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class JournalEntryType
    {

        private System.DateTime createdOn;

        private string number;

        private Entity createdBy;

        private Entity issuedBy;

        private CurrencyType homeCurrency;

        private MoneyValue totalDebitAmount;

        private MoneyValue totalCreditAmount;

        private Entity division;

        private JournalEntryLineType[] journalEntryLines;

        private string notes;

        private bool isPrinted;

        private bool isPrintedSpecified;

        private bool hasAttachments;

        private bool hasAttachmentsSpecified;

        private AttachmentType[] attachments;

        private CustomType[] customs;

        private string gUID;

        private string type;

        /// <remarks/>
        public System.DateTime CreatedOn
        {
            get
            {
                return this.createdOn;
            }
            set
            {
                this.createdOn = value;
            }
        }

        /// <remarks/>
        public string Number
        {
            get
            {
                return this.number;
            }
            set
            {
                this.number = value;
            }
        }

        /// <remarks/>
        public Entity CreatedBy
        {
            get
            {
                return this.createdBy;
            }
            set
            {
                this.createdBy = value;
            }
        }

        /// <remarks/>
        public Entity IssuedBy
        {
            get
            {
                return this.issuedBy;
            }
            set
            {
                this.issuedBy = value;
            }
        }

        /// <remarks/>
        public CurrencyType HomeCurrency
        {
            get
            {
                return this.homeCurrency;
            }
            set
            {
                this.homeCurrency = value;
            }
        }

        /// <remarks/>
        public MoneyValue TotalDebitAmount
        {
            get
            {
                return this.totalDebitAmount;
            }
            set
            {
                this.totalDebitAmount = value;
            }
        }

        /// <remarks/>
        public MoneyValue TotalCreditAmount
        {
            get
            {
                return this.totalCreditAmount;
            }
            set
            {
                this.totalCreditAmount = value;
            }
        }

        /// <remarks/>
        public Entity Division
        {
            get
            {
                return this.division;
            }
            set
            {
                this.division = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("JournalEntryLine", IsNullable = false)]
        public JournalEntryLineType[] JournalEntryLines
        {
            get
            {
                return this.journalEntryLines;
            }
            set
            {
                this.journalEntryLines = value;
            }
        }

        /// <remarks/>
        public string Notes
        {
            get
            {
                return this.notes;
            }
            set
            {
                this.notes = value;
            }
        }

        /// <remarks/>
        public bool IsPrinted
        {
            get
            {
                return this.isPrinted;
            }
            set
            {
                this.isPrinted = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsPrintedSpecified
        {
            get
            {
                return this.isPrintedSpecified;
            }
            set
            {
                this.isPrintedSpecified = value;
            }
        }

        /// <remarks/>
        public bool HasAttachments
        {
            get
            {
                return this.hasAttachments;
            }
            set
            {
                this.hasAttachments = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool HasAttachmentsSpecified
        {
            get
            {
                return this.hasAttachmentsSpecified;
            }
            set
            {
                this.hasAttachmentsSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Attachment", IsNullable = false)]
        public AttachmentType[] Attachments
        {
            get
            {
                return this.attachments;
            }
            set
            {
                this.attachments = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Custom", IsNullable = false)]
        public CustomType[] Customs
        {
            get
            {
                return this.customs;
            }
            set
            {
                this.customs = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string GUID
        {
            get
            {
                return this.gUID;
            }
            set
            {
                this.gUID = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("JournalEntries", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class JournalEntryList
    {

        private JournalEntryType[] journalEntry;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("JournalEntry")]
        public JournalEntryType[] JournalEntry
        {
            get
            {
                return this.journalEntry;
            }
            set
            {
                this.journalEntry = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("Operations", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class OperationList
    {

        private OperationType[] operation;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Operation")]
        public OperationType[] Operation
        {
            get
            {
                return this.operation;
            }
            set
            {
                this.operation = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("Quotations", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class QuotationList
    {

        private QuotationType[] quotation;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Quotation")]
        public QuotationType[] Quotation
        {
            get
            {
                return this.quotation;
            }
            set
            {
                this.quotation = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("CargoMoves", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class CargoMoveList
    {

        private CargoMoveType[] cargoMove;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CargoMove")]
        public CargoMoveType[] CargoMove
        {
            get
            {
                return this.cargoMove;
            }
            set
            {
                this.cargoMove = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("CargoTask", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class CargoTaskType
    {

        private System.DateTime createdOn;

        private System.DateTime startDate;

        private bool startDateSpecified;

        private System.DateTime reminderDate;

        private bool reminderDateSpecified;

        private System.DateTime completionDate;

        private bool completionDateSpecified;

        private System.DateTime dueDate;

        private bool dueDateSpecified;

        private Entity createdBy;

        private Entity assignedTo;

        private CargoTaskStatusType status;

        private string subject;

        private string notes;

        private TaskDefinitionType taskDefinition;

        private LocationType location;

        private string objectElementName;

        private TaskObjectType relatedObject;

        private CustomType[] customs;

        private string gUID;

        /// <remarks/>
        public System.DateTime CreatedOn
        {
            get
            {
                return this.createdOn;
            }
            set
            {
                this.createdOn = value;
            }
        }

        /// <remarks/>
        public System.DateTime StartDate
        {
            get
            {
                return this.startDate;
            }
            set
            {
                this.startDate = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool StartDateSpecified
        {
            get
            {
                return this.startDateSpecified;
            }
            set
            {
                this.startDateSpecified = value;
            }
        }

        /// <remarks/>
        public System.DateTime ReminderDate
        {
            get
            {
                return this.reminderDate;
            }
            set
            {
                this.reminderDate = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ReminderDateSpecified
        {
            get
            {
                return this.reminderDateSpecified;
            }
            set
            {
                this.reminderDateSpecified = value;
            }
        }

        /// <remarks/>
        public System.DateTime CompletionDate
        {
            get
            {
                return this.completionDate;
            }
            set
            {
                this.completionDate = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CompletionDateSpecified
        {
            get
            {
                return this.completionDateSpecified;
            }
            set
            {
                this.completionDateSpecified = value;
            }
        }

        /// <remarks/>
        public System.DateTime DueDate
        {
            get
            {
                return this.dueDate;
            }
            set
            {
                this.dueDate = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DueDateSpecified
        {
            get
            {
                return this.dueDateSpecified;
            }
            set
            {
                this.dueDateSpecified = value;
            }
        }

        /// <remarks/>
        public Entity CreatedBy
        {
            get
            {
                return this.createdBy;
            }
            set
            {
                this.createdBy = value;
            }
        }

        /// <remarks/>
        public Entity AssignedTo
        {
            get
            {
                return this.assignedTo;
            }
            set
            {
                this.assignedTo = value;
            }
        }

        /// <remarks/>
        public CargoTaskStatusType Status
        {
            get
            {
                return this.status;
            }
            set
            {
                this.status = value;
            }
        }

        /// <remarks/>
        public string Subject
        {
            get
            {
                return this.subject;
            }
            set
            {
                this.subject = value;
            }
        }

        /// <remarks/>
        public string Notes
        {
            get
            {
                return this.notes;
            }
            set
            {
                this.notes = value;
            }
        }

        /// <remarks/>
        public TaskDefinitionType TaskDefinition
        {
            get
            {
                return this.taskDefinition;
            }
            set
            {
                this.taskDefinition = value;
            }
        }

        /// <remarks/>
        public LocationType Location
        {
            get
            {
                return this.location;
            }
            set
            {
                this.location = value;
            }
        }

        /// <remarks/>
        public string ObjectElementName
        {
            get
            {
                return this.objectElementName;
            }
            set
            {
                this.objectElementName = value;
            }
        }

        /// <remarks/>
        public TaskObjectType RelatedObject
        {
            get
            {
                return this.relatedObject;
            }
            set
            {
                this.relatedObject = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Custom", IsNullable = false)]
        public CustomType[] Customs
        {
            get
            {
                return this.customs;
            }
            set
            {
                this.customs = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string GUID
        {
            get
            {
                return this.gUID;
            }
            set
            {
                this.gUID = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum CargoTaskStatusType
    {

        /// <remarks/>
        Pending,

        /// <remarks/>
        InProcess,

        /// <remarks/>
        Completed,

        /// <remarks/>
        Canceled,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("CargoTasks", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class CargoTaskList
    {

        private CargoTaskType[] cargoTask;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CargoTask")]
        public CargoTaskType[] CargoTask
        {
            get
            {
                return this.cargoTask;
            }
            set
            {
                this.cargoTask = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("CountReasons", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class CountReasonList
    {

        private CountReasonType[] countReason;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CountReason")]
        public CountReasonType[] CountReason
        {
            get
            {
                return this.countReason;
            }
            set
            {
                this.countReason = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("CountResults", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class CountResultList
    {

        private CountResultType[] countResult;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CountResult")]
        public CountResultType[] CountResult
        {
            get
            {
                return this.countResult;
            }
            set
            {
                this.countResult = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("WMSConfiguration", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class WMSConfigurationType
    {

        private MeasurementUnits measurementUnits;

        private bool askUserIdentification;

        private bool askUserIdentificationSpecified;

        private TaskType[] taskPermissions;

        private PackageType defaultPalletPackage;

        private LanguageType language;

        private bool languageSpecified;

        private bool enableInitialInventory;

        private bool enableInitialInventorySpecified;

        private bool isBondedWarehouse;

        private bool isBondedWarehouseSpecified;

        private ServicesType services;

        private ReceivingSettings receivingSettings;

        private RepackingSettings repackingSettings;

        /// <remarks/>
        public MeasurementUnits MeasurementUnits
        {
            get
            {
                return this.measurementUnits;
            }
            set
            {
                this.measurementUnits = value;
            }
        }

        /// <remarks/>
        public bool AskUserIdentification
        {
            get
            {
                return this.askUserIdentification;
            }
            set
            {
                this.askUserIdentification = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AskUserIdentificationSpecified
        {
            get
            {
                return this.askUserIdentificationSpecified;
            }
            set
            {
                this.askUserIdentificationSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("TaskPermission", IsNullable = false)]
        public TaskType[] TaskPermissions
        {
            get
            {
                return this.taskPermissions;
            }
            set
            {
                this.taskPermissions = value;
            }
        }

        /// <remarks/>
        public PackageType DefaultPalletPackage
        {
            get
            {
                return this.defaultPalletPackage;
            }
            set
            {
                this.defaultPalletPackage = value;
            }
        }

        /// <remarks/>
        public LanguageType Language
        {
            get
            {
                return this.language;
            }
            set
            {
                this.language = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool LanguageSpecified
        {
            get
            {
                return this.languageSpecified;
            }
            set
            {
                this.languageSpecified = value;
            }
        }

        /// <remarks/>
        public bool EnableInitialInventory
        {
            get
            {
                return this.enableInitialInventory;
            }
            set
            {
                this.enableInitialInventory = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool EnableInitialInventorySpecified
        {
            get
            {
                return this.enableInitialInventorySpecified;
            }
            set
            {
                this.enableInitialInventorySpecified = value;
            }
        }

        /// <remarks/>
        public bool IsBondedWarehouse
        {
            get
            {
                return this.isBondedWarehouse;
            }
            set
            {
                this.isBondedWarehouse = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsBondedWarehouseSpecified
        {
            get
            {
                return this.isBondedWarehouseSpecified;
            }
            set
            {
                this.isBondedWarehouseSpecified = value;
            }
        }

        /// <remarks/>
        public ServicesType Services
        {
            get
            {
                return this.services;
            }
            set
            {
                this.services = value;
            }
        }

        /// <remarks/>
        public ReceivingSettings ReceivingSettings
        {
            get
            {
                return this.receivingSettings;
            }
            set
            {
                this.receivingSettings = value;
            }
        }

        /// <remarks/>
        public RepackingSettings RepackingSettings
        {
            get
            {
                return this.repackingSettings;
            }
            set
            {
                this.repackingSettings = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum LanguageType
    {

        /// <remarks/>
        English,

        /// <remarks/>
        Spanish,

        /// <remarks/>
        Portuguese,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("WarehouseEquipment", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class WarehouseEquipmentType
    {

        private string description;

        private string make;

        private string model;

        private string serial;

        private bool isAvailable;

        private bool isAvailableSpecified;

        private string currentUserName;

        private TaskType[] tasks;

        private string gUID;

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        /// <remarks/>
        public string Make
        {
            get
            {
                return this.make;
            }
            set
            {
                this.make = value;
            }
        }

        /// <remarks/>
        public string Model
        {
            get
            {
                return this.model;
            }
            set
            {
                this.model = value;
            }
        }

        /// <remarks/>
        public string Serial
        {
            get
            {
                return this.serial;
            }
            set
            {
                this.serial = value;
            }
        }

        /// <remarks/>
        public bool IsAvailable
        {
            get
            {
                return this.isAvailable;
            }
            set
            {
                this.isAvailable = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsAvailableSpecified
        {
            get
            {
                return this.isAvailableSpecified;
            }
            set
            {
                this.isAvailableSpecified = value;
            }
        }

        /// <remarks/>
        public string CurrentUserName
        {
            get
            {
                return this.currentUserName;
            }
            set
            {
                this.currentUserName = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("TaskPermission", IsNullable = false)]
        public TaskType[] Tasks
        {
            get
            {
                return this.tasks;
            }
            set
            {
                this.tasks = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string GUID
        {
            get
            {
                return this.gUID;
            }
            set
            {
                this.gUID = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("WarehouseEquipments", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class WarehouseEquipmentList
    {

        private WarehouseEquipmentType[] warehouseEquipment;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("WarehouseEquipment")]
        public WarehouseEquipmentType[] WarehouseEquipment
        {
            get
            {
                return this.warehouseEquipment;
            }
            set
            {
                this.warehouseEquipment = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("WarehouseZone", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class WarehouseZoneType
    {

        private string name;

        private string description;

        private string parentZoneName;

        private Entity division;

        private string gUID;

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        /// <remarks/>
        public string ParentZoneName
        {
            get
            {
                return this.parentZoneName;
            }
            set
            {
                this.parentZoneName = value;
            }
        }

        /// <remarks/>
        public Entity Division
        {
            get
            {
                return this.division;
            }
            set
            {
                this.division = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string GUID
        {
            get
            {
                return this.gUID;
            }
            set
            {
                this.gUID = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("WarehouseZones", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class WarehouseZoneList
    {

        private WarehouseZoneType[] warehouseZone;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("WarehouseZone")]
        public WarehouseZoneType[] WarehouseZone
        {
            get
            {
                return this.warehouseZone;
            }
            set
            {
                this.warehouseZone = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("Unit", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class UnitType
    {

        private double factorToBaseUnit;

        private string symbol;

        /// <remarks/>
        public double FactorToBaseUnit
        {
            get
            {
                return this.factorToBaseUnit;
            }
            set
            {
                this.factorToBaseUnit = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Symbol
        {
            get
            {
                return this.symbol;
            }
            set
            {
                this.symbol = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("UnitConversionTable", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class UnitConversionTableType
    {

        private string baseUnit;

        private UnitType[] units;

        private UnitNameType name;

        /// <remarks/>
        public string BaseUnit
        {
            get
            {
                return this.baseUnit;
            }
            set
            {
                this.baseUnit = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Unit", IsNullable = false)]
        public UnitType[] Units
        {
            get
            {
                return this.units;
            }
            set
            {
                this.units = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public UnitNameType Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    public enum UnitNameType
    {

        /// <remarks/>
        Length,

        /// <remarks/>
        Volume,

        /// <remarks/>
        Weight,

        /// <remarks/>
        VolumeWeight,

        /// <remarks/>
        Area,

        /// <remarks/>
        Time,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("UnitConversionTables", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class UnitConversionTableList
    {

        private UnitConversionTableType[] unitConversionTable;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("UnitConversionTable")]
        public UnitConversionTableType[] UnitConversionTable
        {
            get
            {
                return this.unitConversionTable;
            }
            set
            {
                this.unitConversionTable = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("PODConfiguration", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class PODConfigurationType
    {

        private MeasurementUnits measurementUnits;

        private LanguageType language;

        private bool languageSpecified;

        /// <remarks/>
        public MeasurementUnits MeasurementUnits
        {
            get
            {
                return this.measurementUnits;
            }
            set
            {
                this.measurementUnits = value;
            }
        }

        /// <remarks/>
        public LanguageType Language
        {
            get
            {
                return this.language;
            }
            set
            {
                this.language = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool LanguageSpecified
        {
            get
            {
                return this.languageSpecified;
            }
            set
            {
                this.languageSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.magaya.com/XMLSchema/V1")]
    [System.Xml.Serialization.XmlRootAttribute("PODItems", Namespace = "http://www.magaya.com/XMLSchema/V1", IsNullable = false)]
    public partial class PODItemList
    {

        private PODItem[] pODItem;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PODItem")]
        public PODItem[] PODItem
        {
            get
            {
                return this.pODItem;
            }
            set
            {
                this.pODItem = value;
            }
        }
    }

    [XmlRoot(ElementName = "DescriptionType")]
    public class DescriptionType
    {
        [XmlElement(ElementName = "itemNo")]
        public string ItemNo { get; set; }
        [XmlElement(ElementName = "description")]
        public string Description { get; set; }
    }

    [XmlRoot(ElementName = "Description")]
    public class Description
    {
        [XmlElement(ElementName = "DescriptionType")]
        public List<DescriptionType> DescriptionType { get; set; }
    }

    [XmlRoot(ElementName = "Type")]
    public class Type
    {
        [XmlElement(ElementName = "Code")]
        public string Code { get; set; }
        [XmlElement(ElementName = "Description")]
        public string Description { get; set; }
    }

    [XmlRoot(ElementName = "TransactionTypes")]
    public class TransactionTypes
    {
        [XmlElement(ElementName = "Type")]
        public List<Type> Type { get; set; }
    }

    public class QuerylogDt0
    {
        public int access_key { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public int log_entry_type { get; set; }
        public string trans_type { get; set; }
        public int flags { get; set; }
        public string trans_list_xml { get; set; }
        public int record_quatity { get; set; }
        public int backwards_order { get; set; }
    }

    public class TransactionResults
    {
        public WarehouseReceiptList warehousereceipt { get; set; }
        public ShipmentList shipmentlist { get; set; }
        public InvoiceList invoicelist { get; set; }
        public PaymentList paymentlist { get; set; } 

    }
}
