namespace GIGLS.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deltalinetest : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Fleet", "EnterprisePartnerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.FleetDisputeMessage", "FleetOwnerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.FleetJobCard", "FleetId", "dbo.Fleet");
            DropForeignKey("dbo.FleetJobCard", "FleetOwnerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.FleetPartnerTransaction", "FleetId", "dbo.Fleet");
            DropForeignKey("dbo.FleetTrip", "DepartureStationId", "dbo.Station");
            DropForeignKey("dbo.FleetTrip", "DestinationStationId", "dbo.Station");
            DropForeignKey("dbo.InternationalCargoManifestDetail", "InternationalCargoManifestId", "dbo.InternationalCargoManifest");
            DropIndex("dbo.Fleet", new[] { "EnterprisePartnerId" });
            DropIndex("dbo.FleetDisputeMessage", new[] { "FleetOwnerId" });
            DropIndex("dbo.FleetJobCard", new[] { "FleetId" });
            DropIndex("dbo.FleetJobCard", new[] { "FleetOwnerId" });
            DropIndex("dbo.FleetPartnerTransaction", new[] { "FleetId" });
            DropIndex("dbo.FleetTrip", new[] { "DepartureStationId" });
            DropIndex("dbo.FleetTrip", new[] { "DestinationStationId" });
            DropIndex("dbo.InternationalCargoManifest", new[] { "ManifestNo" });
            DropIndex("dbo.InternationalCargoManifestDetail", new[] { "InternationalCargoManifestId" });
            DropColumn("dbo.CashOnDeliveryAccount", "Waybill");
            DropColumn("dbo.CashOnDeliveryRegisterAccount", "TransferAccount");
            DropColumn("dbo.Company", "NUBANCustomerName");
            DropColumn("dbo.Fleet", "FleetName");
            DropColumn("dbo.Fleet", "EnterprisePartnerId");
            DropColumn("dbo.Fleet", "IsFixed");
            DropColumn("dbo.Partner", "CaptainBankName");
            DropColumn("dbo.Partner", "CaptainAccountNumber");
            DropColumn("dbo.Partner", "CaptainAccountName");
            DropColumn("dbo.Partner", "Age");
            DropColumn("dbo.FleetTrip", "MovementManifestId");
            DropColumn("dbo.FleetTrip", "FleetRegistrationNumber");
            DropColumn("dbo.FleetTrip", "DispatchAmount");
            DropColumn("dbo.FleetTrip", "DepartureStationId");
            DropColumn("dbo.FleetTrip", "DestinationStationId");
            DropColumn("dbo.FleetTrip", "DepartureServiceCenterId");
            DropColumn("dbo.FleetTrip", "DestinationServiceCenterId");
            DropColumn("dbo.FleetTrip", "TripAmount");
            DropColumn("dbo.FleetTrip", "Status");
            DropColumn("dbo.GroupWaybillNumber", "ExpressDelivery");
            DropColumn("dbo.GroupWaybillNumber", "IsBulky");
            DropColumn("dbo.GroupWaybillNumberMapping", "ExpressDelivery");
            DropColumn("dbo.GroupWaybillNumberMapping", "IsBulky");
            DropColumn("dbo.InternationalShipmentWaybill", "IsFromMobile");
            DropColumn("dbo.IntlShipmentRequest", "DeliveryType");
            DropColumn("dbo.IntlShipmentRequestItem", "CourierService");
            DropColumn("dbo.IntlShipmentRequestItem", "ItemUniqueNo");
            DropColumn("dbo.IntlShipmentRequestItem", "ReceivedDate");
            DropColumn("dbo.IntlShipmentRequestItem", "ItemState");
            DropColumn("dbo.IntlShipmentRequestItem", "ItemRequestCode");
            DropColumn("dbo.IntlShipmentRequestItem", "ItemStateDescription");
            DropColumn("dbo.IntlShipmentRequestItem", "NoOfPackageReceived");
            DropColumn("dbo.Invoice", "Note");
            DropColumn("dbo.Manifest", "ExpressDelivery");
            DropColumn("dbo.Manifest", "IsBulky");
            DropColumn("dbo.Shipment", "AwaitingCollectionCount");
            DropColumn("dbo.Shipment", "ExpressDelivery");
            DropColumn("dbo.Shipment", "IsExported");
            DropColumn("dbo.Shipment", "RequestNumber");
            DropColumn("dbo.Shipment", "ExpressCharge");
            DropColumn("dbo.Shipment", "IsExpressDropoff");
            DropColumn("dbo.Shipment", "IsBulky");
            DropColumn("dbo.Shipment", "IsGIGGOExtension");
            DropColumn("dbo.Shipment", "CODStatus");
            DropColumn("dbo.Shipment", "CODStatusDate");
            DropColumn("dbo.Shipment", "CODDescription");
            DropColumn("dbo.Shipment", "blackBookSerialNumber");
            DropColumn("dbo.Shipment", "ReceiverPostalCode");
            DropColumn("dbo.Shipment", "ReceiverStateOrProvinceCode");
            DropColumn("dbo.Shipment", "ReceiverCompanyName");
            DropColumn("dbo.Shipment", "ExtraCost");
            DropColumn("dbo.ShipmentItem", "InternationalShipmentItemCategory");
            DropColumn("dbo.MovementManifestNumber", "IsAutomated");
            DropColumn("dbo.MovementManifestNumberMapping", "IsAutomated");
            DropColumn("dbo.PartnerPayout", "PartnerType");
            DropColumn("dbo.PreShipmentMobile", "IsCoupon");
            DropColumn("dbo.PreShipmentMobile", "CouponCode");
            DropColumn("dbo.PreShipmentMobile", "IsAlpha");
            DropColumn("dbo.PreShipmentMobile", "CODStatus");
            DropColumn("dbo.PreShipmentMobile", "CODStatusDate");
            DropColumn("dbo.PreShipmentMobile", "CODDescription");
            DropColumn("dbo.PreShipment", "DeliveryType");
            DropColumn("dbo.PriceCategory", "IsHazardous");
            DropColumn("dbo.PriceCategory", "DeliveryType");
            DropColumn("dbo.ShipmentCollection", "ActualDeliveryAddress");
            DropColumn("dbo.ShipmentPackagePrice", "Length");
            DropColumn("dbo.ShipmentPackagePrice", "Height");
            DropColumn("dbo.ShipmentPackagePrice", "Width");
            DropColumn("dbo.ShipmentPackagePrice", "Weight");
            DropColumn("dbo.TransferDetails", "IsVerified");
            DropColumn("dbo.TransferDetails", "Id");
            DropColumn("dbo.TransferDetails", "ModifiedAt");
            DropColumn("dbo.TransferDetails", "FromKey");
            DropColumn("dbo.TransferDetails", "ToKey");
            DropColumn("dbo.TransferDetails", "SenderName");
            DropColumn("dbo.TransferDetails", "SenderBank");
            DropColumn("dbo.TransferDetails", "Charge");
            DropColumn("dbo.TransferDetails", "Note");
            DropColumn("dbo.TransferDetails", "Status");
            DropColumn("dbo.TransferDetails", "RefId");
            DropColumn("dbo.TransferDetails", "CustomerRef");
            DropColumn("dbo.TransferDetails", "SetRefId");
            DropColumn("dbo.TransferDetails", "Type");
            DropColumn("dbo.TransferDetails", "Settled");
            DropColumn("dbo.TransferDetails", "DeviceId");
            DropColumn("dbo.TransferDetails", "TimedAccNo");
            DropColumn("dbo.TransferDetails", "ManagerName");
            DropColumn("dbo.TransferDetails", "IsPaymentGateway");
            DropColumn("dbo.TransferDetails", "ProcessingPartner");
            DropColumn("dbo.WalletPaymentLog", "isConverted");
            DropColumn("dbo.WalletPaymentLog", "CardType");
            DropColumn("dbo.WalletTransaction", "ServiceCharge");
            DropTable("dbo.BillsPaymentManagement");
            DropTable("dbo.CODGeneratedAccountNo");
            DropTable("dbo.CODTransferLog");
            DropTable("dbo.CODTransferRegister");
            DropTable("dbo.CODWallet");
            DropTable("dbo.CouponCodeManagement");
            DropTable("dbo.FleetDisputeMessage");
            DropTable("dbo.FleetJobCard");
            DropTable("dbo.FleetPartnerTransaction");
            DropTable("dbo.GIGGOCODTransfer");
            DropTable("dbo.GIGXUserDetail");
            DropTable("dbo.InboundCategory");
            DropTable("dbo.InboundShipmentCategory");
            DropTable("dbo.InternationalCargoManifest");
            DropTable("dbo.InternationalCargoManifestDetail");
            DropTable("dbo.PaymentMethod");
            DropTable("dbo.PlaceLocation");
            DropTable("dbo.ShipmentCategory");
            DropTable("dbo.ShipmentExport");
            DropTable("dbo.UnidentifiedItemsForInternationalShipping");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.InternationalCargoManifestDetailId);
            
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
                .PrimaryKey(t => t.InternationalCargoManifestId);
            
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
                .PrimaryKey(t => t.FleetPartnerTransactionId);
            
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
                .PrimaryKey(t => t.FleetJobCardId);
            
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
                .PrimaryKey(t => t.FleetDisputeMessageId);
            
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
                        Narration = c.String(maxLength: 300),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.CODTransferLogId);
            
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
            
            AddColumn("dbo.WalletTransaction", "ServiceCharge", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.WalletPaymentLog", "CardType", c => c.Int(nullable: false));
            AddColumn("dbo.WalletPaymentLog", "isConverted", c => c.Boolean(nullable: false));
            AddColumn("dbo.TransferDetails", "ProcessingPartner", c => c.Int(nullable: false));
            AddColumn("dbo.TransferDetails", "IsPaymentGateway", c => c.Boolean(nullable: false));
            AddColumn("dbo.TransferDetails", "ManagerName", c => c.String(maxLength: 50));
            AddColumn("dbo.TransferDetails", "TimedAccNo", c => c.String(maxLength: 50));
            AddColumn("dbo.TransferDetails", "DeviceId", c => c.String(maxLength: 50));
            AddColumn("dbo.TransferDetails", "Settled", c => c.Boolean(nullable: false));
            AddColumn("dbo.TransferDetails", "Type", c => c.String(maxLength: 50));
            AddColumn("dbo.TransferDetails", "SetRefId", c => c.String(maxLength: 100));
            AddColumn("dbo.TransferDetails", "CustomerRef", c => c.String(maxLength: 100));
            AddColumn("dbo.TransferDetails", "RefId", c => c.String(maxLength: 100));
            AddColumn("dbo.TransferDetails", "Status", c => c.String(maxLength: 50));
            AddColumn("dbo.TransferDetails", "Note", c => c.String(maxLength: 150));
            AddColumn("dbo.TransferDetails", "Charge", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.TransferDetails", "SenderBank", c => c.String(maxLength: 100));
            AddColumn("dbo.TransferDetails", "SenderName", c => c.String(maxLength: 100));
            AddColumn("dbo.TransferDetails", "ToKey", c => c.String(maxLength: 50));
            AddColumn("dbo.TransferDetails", "FromKey", c => c.String(maxLength: 50));
            AddColumn("dbo.TransferDetails", "ModifiedAt", c => c.String(maxLength: 50));
            AddColumn("dbo.TransferDetails", "Id", c => c.String(maxLength: 50));
            AddColumn("dbo.TransferDetails", "IsVerified", c => c.Boolean(nullable: false));
            AddColumn("dbo.ShipmentPackagePrice", "Weight", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ShipmentPackagePrice", "Width", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ShipmentPackagePrice", "Height", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ShipmentPackagePrice", "Length", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ShipmentCollection", "ActualDeliveryAddress", c => c.String(maxLength: 500));
            AddColumn("dbo.PriceCategory", "DeliveryType", c => c.Int(nullable: false));
            AddColumn("dbo.PriceCategory", "IsHazardous", c => c.Boolean(nullable: false));
            AddColumn("dbo.PreShipment", "DeliveryType", c => c.Int(nullable: false));
            AddColumn("dbo.PreShipmentMobile", "CODDescription", c => c.String(maxLength: 300));
            AddColumn("dbo.PreShipmentMobile", "CODStatusDate", c => c.DateTime());
            AddColumn("dbo.PreShipmentMobile", "CODStatus", c => c.Int(nullable: false));
            AddColumn("dbo.PreShipmentMobile", "IsAlpha", c => c.Boolean(nullable: false));
            AddColumn("dbo.PreShipmentMobile", "CouponCode", c => c.String(maxLength: 50));
            AddColumn("dbo.PreShipmentMobile", "IsCoupon", c => c.Boolean(nullable: false));
            AddColumn("dbo.PartnerPayout", "PartnerType", c => c.Int(nullable: false));
            AddColumn("dbo.MovementManifestNumberMapping", "IsAutomated", c => c.Boolean(nullable: false));
            AddColumn("dbo.MovementManifestNumber", "IsAutomated", c => c.Boolean(nullable: false));
            AddColumn("dbo.ShipmentItem", "InternationalShipmentItemCategory", c => c.Int(nullable: false));
            AddColumn("dbo.Shipment", "ExtraCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Shipment", "ReceiverCompanyName", c => c.String(maxLength: 128));
            AddColumn("dbo.Shipment", "ReceiverStateOrProvinceCode", c => c.String(maxLength: 128));
            AddColumn("dbo.Shipment", "ReceiverPostalCode", c => c.String(maxLength: 128));
            AddColumn("dbo.Shipment", "blackBookSerialNumber", c => c.String());
            AddColumn("dbo.Shipment", "CODDescription", c => c.String(maxLength: 300));
            AddColumn("dbo.Shipment", "CODStatusDate", c => c.DateTime());
            AddColumn("dbo.Shipment", "CODStatus", c => c.Int(nullable: false));
            AddColumn("dbo.Shipment", "IsGIGGOExtension", c => c.Boolean(nullable: false));
            AddColumn("dbo.Shipment", "IsBulky", c => c.Boolean(nullable: false));
            AddColumn("dbo.Shipment", "IsExpressDropoff", c => c.Boolean(nullable: false));
            AddColumn("dbo.Shipment", "ExpressCharge", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Shipment", "RequestNumber", c => c.String(maxLength: 128));
            AddColumn("dbo.Shipment", "IsExported", c => c.Boolean(nullable: false));
            AddColumn("dbo.Shipment", "ExpressDelivery", c => c.Boolean(nullable: false));
            AddColumn("dbo.Shipment", "AwaitingCollectionCount", c => c.Int(nullable: false));
            AddColumn("dbo.Manifest", "IsBulky", c => c.Boolean(nullable: false));
            AddColumn("dbo.Manifest", "ExpressDelivery", c => c.Boolean(nullable: false));
            AddColumn("dbo.Invoice", "Note", c => c.String(maxLength: 450));
            AddColumn("dbo.IntlShipmentRequestItem", "NoOfPackageReceived", c => c.Int(nullable: false));
            AddColumn("dbo.IntlShipmentRequestItem", "ItemStateDescription", c => c.String(maxLength: 500));
            AddColumn("dbo.IntlShipmentRequestItem", "ItemRequestCode", c => c.String(maxLength: 128));
            AddColumn("dbo.IntlShipmentRequestItem", "ItemState", c => c.Int(nullable: false));
            AddColumn("dbo.IntlShipmentRequestItem", "ReceivedDate", c => c.DateTime());
            AddColumn("dbo.IntlShipmentRequestItem", "ItemUniqueNo", c => c.String(maxLength: 300));
            AddColumn("dbo.IntlShipmentRequestItem", "CourierService", c => c.String(maxLength: 300));
            AddColumn("dbo.IntlShipmentRequest", "DeliveryType", c => c.Int(nullable: false));
            AddColumn("dbo.InternationalShipmentWaybill", "IsFromMobile", c => c.Boolean(nullable: false));
            AddColumn("dbo.GroupWaybillNumberMapping", "IsBulky", c => c.Boolean(nullable: false));
            AddColumn("dbo.GroupWaybillNumberMapping", "ExpressDelivery", c => c.Boolean(nullable: false));
            AddColumn("dbo.GroupWaybillNumber", "IsBulky", c => c.Boolean(nullable: false));
            AddColumn("dbo.GroupWaybillNumber", "ExpressDelivery", c => c.Boolean(nullable: false));
            AddColumn("dbo.FleetTrip", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.FleetTrip", "TripAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.FleetTrip", "DestinationServiceCenterId", c => c.Int(nullable: false));
            AddColumn("dbo.FleetTrip", "DepartureServiceCenterId", c => c.Int(nullable: false));
            AddColumn("dbo.FleetTrip", "DestinationStationId", c => c.Int());
            AddColumn("dbo.FleetTrip", "DepartureStationId", c => c.Int());
            AddColumn("dbo.FleetTrip", "DispatchAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.FleetTrip", "FleetRegistrationNumber", c => c.String());
            AddColumn("dbo.FleetTrip", "MovementManifestId", c => c.Int(nullable: false));
            AddColumn("dbo.Partner", "Age", c => c.Int(nullable: false));
            AddColumn("dbo.Partner", "CaptainAccountName", c => c.String(maxLength: 100));
            AddColumn("dbo.Partner", "CaptainAccountNumber", c => c.String(maxLength: 100));
            AddColumn("dbo.Partner", "CaptainBankName", c => c.String(maxLength: 100));
            AddColumn("dbo.Fleet", "IsFixed", c => c.Int(nullable: false));
            AddColumn("dbo.Fleet", "EnterprisePartnerId", c => c.String(maxLength: 128));
            AddColumn("dbo.Fleet", "FleetName", c => c.String());
            AddColumn("dbo.Company", "NUBANCustomerName", c => c.String(maxLength: 300));
            AddColumn("dbo.CashOnDeliveryRegisterAccount", "TransferAccount", c => c.String(maxLength: 300));
            AddColumn("dbo.CashOnDeliveryAccount", "Waybill", c => c.String(maxLength: 100));
            CreateIndex("dbo.InternationalCargoManifestDetail", "InternationalCargoManifestId");
            CreateIndex("dbo.InternationalCargoManifest", "ManifestNo", unique: true);
            CreateIndex("dbo.FleetTrip", "DestinationStationId");
            CreateIndex("dbo.FleetTrip", "DepartureStationId");
            CreateIndex("dbo.FleetPartnerTransaction", "FleetId");
            CreateIndex("dbo.FleetJobCard", "FleetOwnerId");
            CreateIndex("dbo.FleetJobCard", "FleetId");
            CreateIndex("dbo.FleetDisputeMessage", "FleetOwnerId");
            CreateIndex("dbo.Fleet", "EnterprisePartnerId");
            AddForeignKey("dbo.InternationalCargoManifestDetail", "InternationalCargoManifestId", "dbo.InternationalCargoManifest", "InternationalCargoManifestId", cascadeDelete: true);
            AddForeignKey("dbo.FleetTrip", "DestinationStationId", "dbo.Station", "StationId");
            AddForeignKey("dbo.FleetTrip", "DepartureStationId", "dbo.Station", "StationId");
            AddForeignKey("dbo.FleetPartnerTransaction", "FleetId", "dbo.Fleet", "FleetId", cascadeDelete: true);
            AddForeignKey("dbo.FleetJobCard", "FleetOwnerId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.FleetJobCard", "FleetId", "dbo.Fleet", "FleetId", cascadeDelete: true);
            AddForeignKey("dbo.FleetDisputeMessage", "FleetOwnerId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Fleet", "EnterprisePartnerId", "dbo.AspNetUsers", "Id");
        }
    }
}
