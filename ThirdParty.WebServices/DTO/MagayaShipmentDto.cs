using Newtonsoft.Json;
using System.Collections.Generic;
using System.Xml.Serialization;
using ThirdParty.WebServices.Magaya.Business.New;

namespace ThirdParty.WebServices.Magaya.DTO
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class MagayaShipmentDto
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

        private MeasurementUnitsType measurementUnits;

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
        [JsonProperty(PropertyName = "Status")]
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
        [JsonProperty(PropertyName = "IssuedBy")]
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
        [JsonProperty(PropertyName = "Shipper")]
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
        public MeasurementUnitsType MeasurementUnits
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

	public class EntityDto 
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

}
