namespace POST.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class efe_dev : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BillsPaymentManagement",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 100),
                        PurchasedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PurchasedDate = c.DateTime(nullable: false),
                        FraudRating = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CODGeneratedAccountNo",
                c => new
                    {
                        CODGeneratedAccountNoId = c.Int(nullable: false, identity: true),
                        Waybill = c.String(maxLength: 100),
                        AccountNo = c.String(maxLength: 128),
                        AccountName = c.String(maxLength: 100),
                        Amount = c.String(),
                        BankName = c.String(maxLength: 300),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.CODGeneratedAccountNoId);
            
            CreateTable(
                "dbo.CODTransferLog",
                c => new
                    {
                        CODTransferLogId = c.Int(nullable: false, identity: true),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OriginatingBankName = c.String(maxLength: 300),
                        OriginatingBankAccount = c.String(maxLength: 300),
                        DestinationBankName = c.String(maxLength: 300),
                        DestinationBankAccount = c.String(maxLength: 300),
                        CustomerCode = c.String(maxLength: 100),
                        PaymentStatus = c.Int(nullable: false),
                        StatusCode = c.String(maxLength: 10),
                        StatusDescription = c.String(),
                        ReferenceNo = c.String(maxLength: 300),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.CODTransferLogId);
            
            CreateTable(
                "dbo.CODTransferRegister",
                c => new
                    {
                        CODTransferRegisterId = c.Int(nullable: false, identity: true),
                        Waybill = c.String(maxLength: 100),
                        AccountNo = c.String(maxLength: 100),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RefNo = c.String(maxLength: 300),
                        ClientRefNo = c.String(maxLength: 300),
                        CustomerCode = c.String(maxLength: 100),
                        PaymentStatus = c.Int(nullable: false),
                        StatusCode = c.String(maxLength: 10),
                        StatusDescription = c.String(maxLength: 500),
                        ReceiverNarration = c.String(),
                        TransferDate = c.DateTime(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.CODTransferRegisterId);
            
            CreateTable(
                "dbo.CODWallet",
                c => new
                    {
                        CODWalletId = c.Int(nullable: false, identity: true),
                        AccountNo = c.String(maxLength: 100),
                        AvailableBalance = c.String(maxLength: 100),
                        CustomerId = c.String(maxLength: 100),
                        CustomerType = c.Int(nullable: false),
                        CustomerCode = c.String(maxLength: 100),
                        CompanyType = c.String(maxLength: 100),
                        AccountType = c.String(maxLength: 100),
                        WithdrawableBalance = c.String(maxLength: 100),
                        CustomerAccountId = c.String(maxLength: 100),
                        UserId = c.String(maxLength: 100),
                        DateOfBirth = c.String(maxLength: 100),
                        PlaceOfBirth = c.String(maxLength: 300),
                        Address = c.String(maxLength: 300),
                        NationalIdentityNo = c.String(maxLength: 128),
                        FirstName = c.String(maxLength: 300),
                        LastName = c.String(maxLength: 300),
                        UserName = c.String(maxLength: 300),
                        Password = c.String(maxLength: 300),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.CODWalletId);
            
            CreateTable(
                "dbo.CouponCodeManagement",
                c => new
                    {
                        CouponCodeManagementId = c.Int(nullable: false, identity: true),
                        CouponCode = c.String(maxLength: 50),
                        CouponCodeValue = c.Single(nullable: false),
                        DiscountType = c.Int(nullable: false),
                        IsCouponCodeUsed = c.Boolean(nullable: false),
                        ExpiryDay = c.DateTime(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.CouponCodeManagementId);
            
            CreateTable(
                "dbo.FleetDisputeMessage",
                c => new
                    {
                        FleetDisputeMessageId = c.Int(nullable: false, identity: true),
                        VehicleNumber = c.String(),
                        DisputeMessage = c.String(),
                        FleetOwnerId = c.String(maxLength: 128),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.FleetDisputeMessageId)
                .ForeignKey("dbo.AspNetUsers", t => t.FleetOwnerId)
                .Index(t => t.FleetOwnerId);
            
            CreateTable(
                "dbo.FleetJobCard",
                c => new
                    {
                        FleetJobCardId = c.Int(nullable: false, identity: true),
                        VehicleNumber = c.String(maxLength: 12),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        VehiclePartToFix = c.String(),
                        Status = c.String(),
                        PaymentReceiptUrl = c.String(),
                        FleetId = c.Int(nullable: false),
                        FleetManagerId = c.String(),
                        FleetOwnerId = c.String(maxLength: 128),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.FleetJobCardId)
                .ForeignKey("dbo.Fleet", t => t.FleetId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.FleetOwnerId)
                .Index(t => t.FleetId)
                .Index(t => t.FleetOwnerId);
            
            CreateTable(
                "dbo.FleetPartnerTransaction",
                c => new
                    {
                        FleetPartnerTransactionId = c.Int(nullable: false, identity: true),
                        FleetId = c.Int(nullable: false),
                        FleetRegistrationNumber = c.String(),
                        DateOfEntry = c.DateTime(nullable: false),
                        UserId = c.String(maxLength: 128),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreditDebitType = c.Int(nullable: false),
                        Description = c.String(),
                        IsDeferred = c.Boolean(nullable: false),
                        MovementManifestNumber = c.String(maxLength: 100),
                        PaymentType = c.Int(nullable: false),
                        PaymentTypeReference = c.String(maxLength: 100),
                        TransactionCountryId = c.Int(nullable: false),
                        IsSettled = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.FleetPartnerTransactionId)
                .ForeignKey("dbo.Fleet", t => t.FleetId, cascadeDelete: true)
                .Index(t => t.FleetId);
            
            CreateTable(
                "dbo.GIGGOCODTransfer",
                c => new
                    {
                        GIGGOCODTransferID = c.String(nullable: false, maxLength: 128),
                        Waybill = c.String(),
                        AccountNo = c.String(),
                        AccountName = c.String(),
                        Amount = c.String(),
                        BankName = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.GIGGOCODTransferID);
            
            CreateTable(
                "dbo.GIGXUserDetail",
                c => new
                    {
                        GIGXUserDetailId = c.Int(nullable: false, identity: true),
                        CustomerCode = c.String(maxLength: 128),
                        CustomerPin = c.String(maxLength: 7),
                        WalletAddress = c.String(),
                        PrivateKey = c.String(),
                        PublicKey = c.String(),
                        GIGXEmail = c.String(maxLength: 128),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.GIGXUserDetailId);
            
            CreateTable(
                "dbo.InboundCategory",
                c => new
                    {
                        CategoryId = c.String(nullable: false, maxLength: 128),
                        CategoryName = c.String(),
                        IsGoStandard = c.Boolean(nullable: false),
                        IsGoFaster = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.InboundShipmentCategory",
                c => new
                    {
                        InboundShipmentCategoryId = c.String(nullable: false, maxLength: 128),
                        ShipmentCategoryId = c.Int(nullable: false),
                        CountryId = c.Int(nullable: false),
                        IsGoStandard = c.Boolean(nullable: false),
                        IsGoFaster = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.InboundShipmentCategoryId);
            
            CreateTable(
                "dbo.InternationalCargoManifest",
                c => new
                    {
                        InternationalCargoManifestId = c.Int(nullable: false, identity: true),
                        ManifestNo = c.String(maxLength: 128),
                        AirlineWaybillNo = c.String(maxLength: 128),
                        FlightNo = c.String(maxLength: 300),
                        DestinationCountry = c.Int(nullable: false),
                        DepartureCountry = c.Int(nullable: false),
                        FlightDate = c.DateTime(nullable: false),
                        UserId = c.String(maxLength: 128),
                        CargoedBy = c.String(maxLength: 128),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.InternationalCargoManifestId)
                .Index(t => t.ManifestNo, unique: true);
            
            CreateTable(
                "dbo.InternationalCargoManifestDetail",
                c => new
                    {
                        InternationalCargoManifestDetailId = c.Int(nullable: false, identity: true),
                        RequestNumber = c.String(maxLength: 128),
                        Waybill = c.String(maxLength: 128),
                        Weight = c.Double(nullable: false),
                        Quantity = c.Int(nullable: false),
                        ItemUniqueNo = c.String(maxLength: 500),
                        CourierService = c.String(maxLength: 500),
                        ItemName = c.String(maxLength: 500),
                        Length = c.Double(nullable: false),
                        Width = c.Double(nullable: false),
                        Height = c.Double(nullable: false),
                        InternationalCargoManifestId = c.Int(nullable: false),
                        ItemRequestCode = c.String(maxLength: 500),
                        NoOfPackageReceived = c.Int(nullable: false),
                        Description = c.String(maxLength: 500),
                        GrandTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DeclaredValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.InternationalCargoManifestDetailId)
                .ForeignKey("dbo.InternationalCargoManifest", t => t.InternationalCargoManifestId, cascadeDelete: true)
                .Index(t => t.InternationalCargoManifestId);
            
            CreateTable(
                "dbo.PaymentMethod",
                c => new
                    {
                        PaymentMethodId = c.Int(nullable: false, identity: true),
                        PaymentMethodName = c.String(maxLength: 100),
                        CountryId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.PaymentMethodId);
            
            CreateTable(
                "dbo.PlaceLocation",
                c => new
                    {
                        PlaceLocationId = c.Int(nullable: false, identity: true),
                        PlaceLocationName = c.String(maxLength: 100),
                        StateName = c.String(maxLength: 100),
                        StateId = c.Int(nullable: false),
                        BaseStation = c.String(maxLength: 100),
                        BaseStationId = c.Int(nullable: false),
                        IsHomeDelivery = c.Boolean(nullable: false),
                        IsNormalHomeDelivery = c.Boolean(nullable: false),
                        IsExpressHomeDelivery = c.Boolean(nullable: false),
                        IsExtraMileDelivery = c.Boolean(nullable: false),
                        IsGIGGO = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.PlaceLocationId);
            
            CreateTable(
                "dbo.ShipmentCategory",
                c => new
                    {
                        ShipmentCategoryId = c.Int(nullable: false, identity: true),
                        ShipmentCategoryName = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ShipmentCategoryId);
            
            CreateTable(
                "dbo.ShipmentExport",
                c => new
                    {
                        ShipmentExportId = c.Int(nullable: false, identity: true),
                        RequestNumber = c.String(maxLength: 128),
                        Waybill = c.String(maxLength: 128),
                        Weight = c.Double(nullable: false),
                        Quantity = c.Int(nullable: false),
                        ItemUniqueNo = c.String(maxLength: 500),
                        CourierService = c.String(maxLength: 500),
                        ItemName = c.String(maxLength: 500),
                        IsExported = c.Boolean(nullable: false),
                        UserId = c.String(maxLength: 128),
                        ExportedBy = c.String(maxLength: 128),
                        Length = c.Double(nullable: false),
                        Width = c.Double(nullable: false),
                        Height = c.Double(nullable: false),
                        ItemRequestCode = c.String(maxLength: 500),
                        NoOfPackageReceived = c.Int(nullable: false),
                        Description = c.String(maxLength: 500),
                        GrandTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DeclaredValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemState = c.Int(nullable: false),
                        CustomerName = c.String(maxLength: 500),
                        ItemValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ShipmentExportId);
            
            CreateTable(
                "dbo.UnidentifiedItemsForInternationalShipping",
                c => new
                    {
                        UnidentifiedItemsForInternationalShippingId = c.Int(nullable: false, identity: true),
                        TrackingNo = c.String(maxLength: 128),
                        CustomerName = c.String(maxLength: 128),
                        CustomerEmail = c.String(maxLength: 128),
                        CustomerPhoneNo = c.String(maxLength: 128),
                        UserId = c.String(maxLength: 128),
                        IsProcessed = c.Boolean(nullable: false),
                        ItemName = c.String(maxLength: 300),
                        Length = c.Double(nullable: false),
                        Width = c.Double(nullable: false),
                        Height = c.Double(nullable: false),
                        Weight = c.Double(nullable: false),
                        NoOfPackageReceived = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        ItemDescription = c.String(maxLength: 300),
                        StoreName = c.String(maxLength: 300),
                        ItemUniqueNo = c.String(maxLength: 128),
                        CourierService = c.String(maxLength: 300),
                        ItemStateDescription = c.String(maxLength: 300),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.UnidentifiedItemsForInternationalShippingId);
            
            AddColumn("dbo.CashOnDeliveryAccount", "Waybill", c => c.String(maxLength: 100));
            AddColumn("dbo.CashOnDeliveryRegisterAccount", "TransferAccount", c => c.String(maxLength: 300));
            AddColumn("dbo.Company", "NUBANCustomerName", c => c.String(maxLength: 300));
            AddColumn("dbo.Fleet", "FleetName", c => c.String());
            AddColumn("dbo.Fleet", "EnterprisePartnerId", c => c.String(maxLength: 128));
            AddColumn("dbo.Fleet", "IsFixed", c => c.Int(nullable: false));
            AddColumn("dbo.Partner", "CaptainBankName", c => c.String(maxLength: 100));
            AddColumn("dbo.Partner", "CaptainAccountNumber", c => c.String(maxLength: 100));
            AddColumn("dbo.Partner", "CaptainAccountName", c => c.String(maxLength: 100));
            AddColumn("dbo.Partner", "Age", c => c.Int(nullable: false));
            AddColumn("dbo.FleetTrip", "MovementManifestId", c => c.Int(nullable: false));
            AddColumn("dbo.FleetTrip", "FleetRegistrationNumber", c => c.String());
            AddColumn("dbo.FleetTrip", "DispatchAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.FleetTrip", "DepartureStationId", c => c.Int());
            AddColumn("dbo.FleetTrip", "DestinationStationId", c => c.Int());
            AddColumn("dbo.FleetTrip", "DepartureServiceCenterId", c => c.Int(nullable: false));
            AddColumn("dbo.FleetTrip", "DestinationServiceCenterId", c => c.Int(nullable: false));
            AddColumn("dbo.FleetTrip", "TripAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.FleetTrip", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.GroupWaybillNumber", "ExpressDelivery", c => c.Boolean(nullable: false));
            AddColumn("dbo.GroupWaybillNumber", "IsBulky", c => c.Boolean(nullable: false));
            AddColumn("dbo.GroupWaybillNumberMapping", "ExpressDelivery", c => c.Boolean(nullable: false));
            AddColumn("dbo.GroupWaybillNumberMapping", "IsBulky", c => c.Boolean(nullable: false));
            AddColumn("dbo.InternationalShipmentWaybill", "IsFromMobile", c => c.Boolean(nullable: false));
            AddColumn("dbo.IntlShipmentRequest", "DeliveryType", c => c.Int(nullable: false));
            AddColumn("dbo.IntlShipmentRequestItem", "CourierService", c => c.String(maxLength: 300));
            AddColumn("dbo.IntlShipmentRequestItem", "ItemUniqueNo", c => c.String(maxLength: 300));
            AddColumn("dbo.IntlShipmentRequestItem", "ReceivedDate", c => c.DateTime());
            AddColumn("dbo.IntlShipmentRequestItem", "ItemState", c => c.Int(nullable: false));
            AddColumn("dbo.IntlShipmentRequestItem", "ItemRequestCode", c => c.String(maxLength: 128));
            AddColumn("dbo.IntlShipmentRequestItem", "ItemStateDescription", c => c.String(maxLength: 500));
            AddColumn("dbo.IntlShipmentRequestItem", "NoOfPackageReceived", c => c.Int(nullable: false));
            AddColumn("dbo.Invoice", "Note", c => c.String(maxLength: 450));
            AddColumn("dbo.Manifest", "ExpressDelivery", c => c.Boolean(nullable: false));
            AddColumn("dbo.Manifest", "IsBulky", c => c.Boolean(nullable: false));
            AddColumn("dbo.Shipment", "AwaitingCollectionCount", c => c.Int(nullable: false));
            AddColumn("dbo.Shipment", "ExpressDelivery", c => c.Boolean(nullable: false));
            AddColumn("dbo.Shipment", "IsExported", c => c.Boolean(nullable: false));
            AddColumn("dbo.Shipment", "RequestNumber", c => c.String(maxLength: 128));
            AddColumn("dbo.Shipment", "ExpressCharge", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Shipment", "IsExpressDropoff", c => c.Boolean(nullable: false));
            AddColumn("dbo.Shipment", "IsBulky", c => c.Boolean(nullable: false));
            AddColumn("dbo.Shipment", "IsGIGGOExtension", c => c.Boolean(nullable: false));
            AddColumn("dbo.Shipment", "CODStatus", c => c.Int(nullable: false));
            AddColumn("dbo.Shipment", "CODStatusDate", c => c.DateTime());
            AddColumn("dbo.Shipment", "CODDescription", c => c.String(maxLength: 300));
            AddColumn("dbo.Shipment", "blackBookSerialNumber", c => c.String());
            AddColumn("dbo.Shipment", "ReceiverPostalCode", c => c.String(maxLength: 128));
            AddColumn("dbo.Shipment", "ReceiverStateOrProvinceCode", c => c.String(maxLength: 128));
            AddColumn("dbo.Shipment", "ReceiverCompanyName", c => c.String(maxLength: 128));
            AddColumn("dbo.Shipment", "ExtraCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ShipmentItem", "InternationalShipmentItemCategory", c => c.Int(nullable: false));
            AddColumn("dbo.MovementManifestNumber", "IsAutomated", c => c.Boolean(nullable: false));
            AddColumn("dbo.MovementManifestNumberMapping", "IsAutomated", c => c.Boolean(nullable: false));
            AddColumn("dbo.PartnerPayout", "PartnerType", c => c.Int(nullable: false));
            AddColumn("dbo.PreShipmentMobile", "IsCoupon", c => c.Boolean(nullable: false));
            AddColumn("dbo.PreShipmentMobile", "CouponCode", c => c.String(maxLength: 50));
            AddColumn("dbo.PreShipmentMobile", "IsAlpha", c => c.Boolean(nullable: false));
            AddColumn("dbo.PreShipmentMobile", "CODStatus", c => c.Int(nullable: false));
            AddColumn("dbo.PreShipmentMobile", "CODStatusDate", c => c.DateTime());
            AddColumn("dbo.PreShipmentMobile", "CODDescription", c => c.String(maxLength: 300));
            AddColumn("dbo.PreShipment", "DeliveryType", c => c.Int(nullable: false));
            AddColumn("dbo.PriceCategory", "IsHazardous", c => c.Boolean(nullable: false));
            AddColumn("dbo.PriceCategory", "DeliveryType", c => c.Int(nullable: false));
            AddColumn("dbo.ShipmentCollection", "ActualDeliveryAddress", c => c.String(maxLength: 500));
            AddColumn("dbo.ShipmentPackagePrice", "Length", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ShipmentPackagePrice", "Height", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ShipmentPackagePrice", "Width", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ShipmentPackagePrice", "Weight", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.TransferDetails", "IsVerified", c => c.Boolean(nullable: false));
            AddColumn("dbo.TransferDetails", "Id", c => c.String(maxLength: 50));
            AddColumn("dbo.TransferDetails", "ModifiedAt", c => c.String(maxLength: 50));
            AddColumn("dbo.TransferDetails", "FromKey", c => c.String(maxLength: 50));
            AddColumn("dbo.TransferDetails", "ToKey", c => c.String(maxLength: 50));
            AddColumn("dbo.TransferDetails", "SenderName", c => c.String(maxLength: 100));
            AddColumn("dbo.TransferDetails", "SenderBank", c => c.String(maxLength: 100));
            AddColumn("dbo.TransferDetails", "Charge", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.TransferDetails", "Note", c => c.String(maxLength: 150));
            AddColumn("dbo.TransferDetails", "Status", c => c.String(maxLength: 50));
            AddColumn("dbo.TransferDetails", "RefId", c => c.String(maxLength: 100));
            AddColumn("dbo.TransferDetails", "CustomerRef", c => c.String(maxLength: 100));
            AddColumn("dbo.TransferDetails", "SetRefId", c => c.String(maxLength: 100));
            AddColumn("dbo.TransferDetails", "Type", c => c.String(maxLength: 50));
            AddColumn("dbo.TransferDetails", "Settled", c => c.Boolean(nullable: false));
            AddColumn("dbo.TransferDetails", "DeviceId", c => c.String(maxLength: 50));
            AddColumn("dbo.TransferDetails", "TimedAccNo", c => c.String(maxLength: 50));
            AddColumn("dbo.TransferDetails", "ManagerName", c => c.String(maxLength: 50));
            AddColumn("dbo.TransferDetails", "IsPaymentGateway", c => c.Boolean(nullable: false));
            AddColumn("dbo.TransferDetails", "ProcessingPartner", c => c.Int(nullable: false));
            AddColumn("dbo.WalletPaymentLog", "isConverted", c => c.Boolean(nullable: false));
            AddColumn("dbo.WalletPaymentLog", "CardType", c => c.Int(nullable: false));
            AddColumn("dbo.WalletTransaction", "ServiceCharge", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            CreateIndex("dbo.Fleet", "EnterprisePartnerId");
            CreateIndex("dbo.FleetTrip", "DepartureStationId");
            CreateIndex("dbo.FleetTrip", "DestinationStationId");
            AddForeignKey("dbo.Fleet", "EnterprisePartnerId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.FleetTrip", "DepartureStationId", "dbo.Station", "StationId");
            AddForeignKey("dbo.FleetTrip", "DestinationStationId", "dbo.Station", "StationId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InternationalCargoManifestDetail", "InternationalCargoManifestId", "dbo.InternationalCargoManifest");
            DropForeignKey("dbo.FleetTrip", "DestinationStationId", "dbo.Station");
            DropForeignKey("dbo.FleetTrip", "DepartureStationId", "dbo.Station");
            DropForeignKey("dbo.FleetPartnerTransaction", "FleetId", "dbo.Fleet");
            DropForeignKey("dbo.FleetJobCard", "FleetOwnerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.FleetJobCard", "FleetId", "dbo.Fleet");
            DropForeignKey("dbo.FleetDisputeMessage", "FleetOwnerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Fleet", "EnterprisePartnerId", "dbo.AspNetUsers");
            DropIndex("dbo.InternationalCargoManifestDetail", new[] { "InternationalCargoManifestId" });
            DropIndex("dbo.InternationalCargoManifest", new[] { "ManifestNo" });
            DropIndex("dbo.FleetTrip", new[] { "DestinationStationId" });
            DropIndex("dbo.FleetTrip", new[] { "DepartureStationId" });
            DropIndex("dbo.FleetPartnerTransaction", new[] { "FleetId" });
            DropIndex("dbo.FleetJobCard", new[] { "FleetOwnerId" });
            DropIndex("dbo.FleetJobCard", new[] { "FleetId" });
            DropIndex("dbo.FleetDisputeMessage", new[] { "FleetOwnerId" });
            DropIndex("dbo.Fleet", new[] { "EnterprisePartnerId" });
            DropColumn("dbo.WalletTransaction", "ServiceCharge");
            DropColumn("dbo.WalletPaymentLog", "CardType");
            DropColumn("dbo.WalletPaymentLog", "isConverted");
            DropColumn("dbo.TransferDetails", "ProcessingPartner");
            DropColumn("dbo.TransferDetails", "IsPaymentGateway");
            DropColumn("dbo.TransferDetails", "ManagerName");
            DropColumn("dbo.TransferDetails", "TimedAccNo");
            DropColumn("dbo.TransferDetails", "DeviceId");
            DropColumn("dbo.TransferDetails", "Settled");
            DropColumn("dbo.TransferDetails", "Type");
            DropColumn("dbo.TransferDetails", "SetRefId");
            DropColumn("dbo.TransferDetails", "CustomerRef");
            DropColumn("dbo.TransferDetails", "RefId");
            DropColumn("dbo.TransferDetails", "Status");
            DropColumn("dbo.TransferDetails", "Note");
            DropColumn("dbo.TransferDetails", "Charge");
            DropColumn("dbo.TransferDetails", "SenderBank");
            DropColumn("dbo.TransferDetails", "SenderName");
            DropColumn("dbo.TransferDetails", "ToKey");
            DropColumn("dbo.TransferDetails", "FromKey");
            DropColumn("dbo.TransferDetails", "ModifiedAt");
            DropColumn("dbo.TransferDetails", "Id");
            DropColumn("dbo.TransferDetails", "IsVerified");
            DropColumn("dbo.ShipmentPackagePrice", "Weight");
            DropColumn("dbo.ShipmentPackagePrice", "Width");
            DropColumn("dbo.ShipmentPackagePrice", "Height");
            DropColumn("dbo.ShipmentPackagePrice", "Length");
            DropColumn("dbo.ShipmentCollection", "ActualDeliveryAddress");
            DropColumn("dbo.PriceCategory", "DeliveryType");
            DropColumn("dbo.PriceCategory", "IsHazardous");
            DropColumn("dbo.PreShipment", "DeliveryType");
            DropColumn("dbo.PreShipmentMobile", "CODDescription");
            DropColumn("dbo.PreShipmentMobile", "CODStatusDate");
            DropColumn("dbo.PreShipmentMobile", "CODStatus");
            DropColumn("dbo.PreShipmentMobile", "IsAlpha");
            DropColumn("dbo.PreShipmentMobile", "CouponCode");
            DropColumn("dbo.PreShipmentMobile", "IsCoupon");
            DropColumn("dbo.PartnerPayout", "PartnerType");
            DropColumn("dbo.MovementManifestNumberMapping", "IsAutomated");
            DropColumn("dbo.MovementManifestNumber", "IsAutomated");
            DropColumn("dbo.ShipmentItem", "InternationalShipmentItemCategory");
            DropColumn("dbo.Shipment", "ExtraCost");
            DropColumn("dbo.Shipment", "ReceiverCompanyName");
            DropColumn("dbo.Shipment", "ReceiverStateOrProvinceCode");
            DropColumn("dbo.Shipment", "ReceiverPostalCode");
            DropColumn("dbo.Shipment", "blackBookSerialNumber");
            DropColumn("dbo.Shipment", "CODDescription");
            DropColumn("dbo.Shipment", "CODStatusDate");
            DropColumn("dbo.Shipment", "CODStatus");
            DropColumn("dbo.Shipment", "IsGIGGOExtension");
            DropColumn("dbo.Shipment", "IsBulky");
            DropColumn("dbo.Shipment", "IsExpressDropoff");
            DropColumn("dbo.Shipment", "ExpressCharge");
            DropColumn("dbo.Shipment", "RequestNumber");
            DropColumn("dbo.Shipment", "IsExported");
            DropColumn("dbo.Shipment", "ExpressDelivery");
            DropColumn("dbo.Shipment", "AwaitingCollectionCount");
            DropColumn("dbo.Manifest", "IsBulky");
            DropColumn("dbo.Manifest", "ExpressDelivery");
            DropColumn("dbo.Invoice", "Note");
            DropColumn("dbo.IntlShipmentRequestItem", "NoOfPackageReceived");
            DropColumn("dbo.IntlShipmentRequestItem", "ItemStateDescription");
            DropColumn("dbo.IntlShipmentRequestItem", "ItemRequestCode");
            DropColumn("dbo.IntlShipmentRequestItem", "ItemState");
            DropColumn("dbo.IntlShipmentRequestItem", "ReceivedDate");
            DropColumn("dbo.IntlShipmentRequestItem", "ItemUniqueNo");
            DropColumn("dbo.IntlShipmentRequestItem", "CourierService");
            DropColumn("dbo.IntlShipmentRequest", "DeliveryType");
            DropColumn("dbo.InternationalShipmentWaybill", "IsFromMobile");
            DropColumn("dbo.GroupWaybillNumberMapping", "IsBulky");
            DropColumn("dbo.GroupWaybillNumberMapping", "ExpressDelivery");
            DropColumn("dbo.GroupWaybillNumber", "IsBulky");
            DropColumn("dbo.GroupWaybillNumber", "ExpressDelivery");
            DropColumn("dbo.FleetTrip", "Status");
            DropColumn("dbo.FleetTrip", "TripAmount");
            DropColumn("dbo.FleetTrip", "DestinationServiceCenterId");
            DropColumn("dbo.FleetTrip", "DepartureServiceCenterId");
            DropColumn("dbo.FleetTrip", "DestinationStationId");
            DropColumn("dbo.FleetTrip", "DepartureStationId");
            DropColumn("dbo.FleetTrip", "DispatchAmount");
            DropColumn("dbo.FleetTrip", "FleetRegistrationNumber");
            DropColumn("dbo.FleetTrip", "MovementManifestId");
            DropColumn("dbo.Partner", "Age");
            DropColumn("dbo.Partner", "CaptainAccountName");
            DropColumn("dbo.Partner", "CaptainAccountNumber");
            DropColumn("dbo.Partner", "CaptainBankName");
            DropColumn("dbo.Fleet", "IsFixed");
            DropColumn("dbo.Fleet", "EnterprisePartnerId");
            DropColumn("dbo.Fleet", "FleetName");
            DropColumn("dbo.Company", "NUBANCustomerName");
            DropColumn("dbo.CashOnDeliveryRegisterAccount", "TransferAccount");
            DropColumn("dbo.CashOnDeliveryAccount", "Waybill");
            DropTable("dbo.UnidentifiedItemsForInternationalShipping");
            DropTable("dbo.ShipmentExport");
            DropTable("dbo.ShipmentCategory");
            DropTable("dbo.PlaceLocation");
            DropTable("dbo.PaymentMethod");
            DropTable("dbo.InternationalCargoManifestDetail");
            DropTable("dbo.InternationalCargoManifest");
            DropTable("dbo.InboundShipmentCategory");
            DropTable("dbo.InboundCategory");
            DropTable("dbo.GIGXUserDetail");
            DropTable("dbo.GIGGOCODTransfer");
            DropTable("dbo.FleetPartnerTransaction");
            DropTable("dbo.FleetJobCard");
            DropTable("dbo.FleetDisputeMessage");
            DropTable("dbo.CouponCodeManagement");
            DropTable("dbo.CODWallet");
            DropTable("dbo.CODTransferRegister");
            DropTable("dbo.CODTransferLog");
            DropTable("dbo.CODGeneratedAccountNo");
            DropTable("dbo.BillsPaymentManagement");
        }
    }
}
