namespace POST.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Efetype : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountSummary",
                c => new
                    {
                        AccountSummaryId = c.Int(nullable: false, identity: true),
                        Balance = c.Double(nullable: false),
                        AccountType = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.AccountSummaryId);
            
            CreateTable(
                "dbo.AccountTransaction",
                c => new
                    {
                        AccountTransactionId = c.Int(nullable: false, identity: true),
                        Amount = c.Double(nullable: false),
                        DateOfTransaction = c.DateTime(nullable: false),
                        Narration = c.String(),
                        TransactionReference = c.String(),
                        AccountType = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.AccountTransactionId);
            
            CreateTable(
                "dbo.ActivationCampaignEmail",
                c => new
                    {
                        ActivationCampaignEmailId = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.ActivationCampaignEmailId);
            
            CreateTable(
                "dbo.AuditTrailEvent",
                c => new
                    {
                        EventId = c.Int(nullable: false, identity: true),
                        InsertedDate = c.DateTime(),
                        LastUpdatedDate = c.DateTime(),
                        Data = c.String(),
                    })
                .PrimaryKey(t => t.EventId);
            
            CreateTable(
                "dbo.Bank",
                c => new
                    {
                        BankId = c.Int(nullable: false, identity: true),
                        BankName = c.String(maxLength: 100),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.BankId)
                .Index(t => t.BankName, unique: true);
            
            CreateTable(
                "dbo.BankProcessingOrderCodes",
                c => new
                    {
                        CodeId = c.Int(nullable: false, identity: true),
                        Code = c.String(maxLength: 100),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateAndTimeOfDeposit = c.DateTime(nullable: false),
                        UserId = c.String(maxLength: 128),
                        FullName = c.String(),
                        ServiceCenter = c.Int(nullable: false),
                        ScName = c.String(),
                        DepositType = c.Int(nullable: false),
                        StartDateTime = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        BankName = c.String(),
                        VerifiedBy = c.String(maxLength: 128),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.CodeId);
            
            CreateTable(
                "dbo.BankProcessingOrderForShipmentAndCOD",
                c => new
                    {
                        ProcessingOrderId = c.Int(nullable: false, identity: true),
                        Waybill = c.String(maxLength: 100),
                        GrandTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CODAmount = c.Decimal(precision: 18, scale: 2),
                        DemurrageAmount = c.Decimal(precision: 18, scale: 2),
                        RefCode = c.String(maxLength: 100),
                        DepositType = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        ServiceCenterId = c.Int(nullable: false),
                        ServiceCenter = c.String(),
                        Status = c.Int(nullable: false),
                        VerifiedBy = c.String(maxLength: 128),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ProcessingOrderId);
            
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
                "dbo.CaptainBonusByZoneMaping",
                c => new
                    {
                        CaptainBonusByZoneMapingId = c.Int(nullable: false, identity: true),
                        Zone = c.Int(nullable: false),
                        BonusAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsActivated = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.CaptainBonusByZoneMapingId);
            
            CreateTable(
                "dbo.CashOnDeliveryAccount",
                c => new
                    {
                        CashOnDeliveryAccountId = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreditDebitType = c.Int(nullable: false),
                        WalletId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        CODStatus = c.Int(nullable: false),
                        CountryId = c.Int(nullable: false),
                        Waybill = c.String(maxLength: 100),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.CashOnDeliveryAccountId)
                .ForeignKey("dbo.Wallet", t => t.WalletId, cascadeDelete: true)
                .Index(t => t.WalletId);
            
            CreateTable(
                "dbo.Wallet",
                c => new
                    {
                        WalletId = c.Int(nullable: false, identity: true),
                        WalletNumber = c.String(maxLength: 100),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CustomerId = c.Int(nullable: false),
                        CustomerType = c.Int(nullable: false),
                        IsSystem = c.Boolean(nullable: false),
                        CustomerCode = c.String(maxLength: 100),
                        CompanyType = c.String(maxLength: 100),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.WalletId);
            
            CreateTable(
                "dbo.CashOnDeliveryBalance",
                c => new
                    {
                        CashOnDeliveryBalanceId = c.Int(nullable: false, identity: true),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        WalletId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.CashOnDeliveryBalanceId)
                .ForeignKey("dbo.Wallet", t => t.WalletId, cascadeDelete: true)
                .Index(t => t.WalletId);
            
            CreateTable(
                "dbo.CashOnDeliveryRegisterAccount",
                c => new
                    {
                        CashOnDeliveryRegisterAccountId = c.Int(nullable: false, identity: true),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UserId = c.String(maxLength: 128),
                        Waybill = c.String(maxLength: 100),
                        CODStatusHistory = c.Int(nullable: false),
                        ServiceCenterId = c.Int(nullable: false),
                        ServiceCenterCode = c.String(maxLength: 50),
                        Description = c.String(),
                        PaymentType = c.Int(nullable: false),
                        PaymentTypeReference = c.String(maxLength: 100),
                        DepositStatus = c.Int(nullable: false),
                        RefCode = c.String(maxLength: 100),
                        DepartureServiceCenterId = c.Int(nullable: false),
                        DestinationCountryId = c.Int(nullable: false),
                        TransferAccount = c.String(maxLength: 300),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.CashOnDeliveryRegisterAccountId);
            
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.ClientNode",
                c => new
                    {
                        ClientNodeId = c.String(nullable: false, maxLength: 32),
                        Base64Secret = c.String(nullable: false, maxLength: 80),
                        Name = c.String(nullable: false, maxLength: 100),
                        Status = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ClientNodeId);
            
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
                "dbo.CodPayOutList",
                c => new
                    {
                        CodPayOutId = c.Int(nullable: false, identity: true),
                        Waybill = c.String(maxLength: 100),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateAndTimeOfDeposit = c.DateTime(nullable: false),
                        UserId = c.String(maxLength: 128),
                        CustomerCode = c.String(maxLength: 100),
                        Name = c.String(),
                        ServiceCenter = c.Int(nullable: false),
                        ScName = c.String(),
                        IsCODPaidOut = c.Boolean(nullable: false),
                        VerifiedBy = c.String(maxLength: 128),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.CodPayOutId);
            
            CreateTable(
                "dbo.CODSettlementSheet",
                c => new
                    {
                        CODSettlementSheetId = c.Int(nullable: false, identity: true),
                        Waybill = c.String(maxLength: 100),
                        ReceiverAgentId = c.String(maxLength: 100),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateSettled = c.DateTime(),
                        CollectionAgentId = c.String(maxLength: 100),
                        ReceivedCOD = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.CODSettlementSheetId)
                .Index(t => t.Waybill, unique: true);
            
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
                "dbo.Company",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 500),
                        RcNumber = c.String(maxLength: 100),
                        Email = c.String(maxLength: 500),
                        City = c.String(maxLength: 100),
                        State = c.String(maxLength: 100),
                        Address = c.String(maxLength: 500),
                        UserActiveCountryId = c.Int(nullable: false),
                        PhoneNumber = c.String(maxLength: 20),
                        Industry = c.String(maxLength: 100),
                        CompanyType = c.Int(nullable: false),
                        CompanyStatus = c.Int(nullable: false),
                        CustomerCode = c.String(maxLength: 100),
                        Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SettlementPeriod = c.Int(nullable: false),
                        CustomerCategory = c.Int(nullable: false),
                        ReturnOption = c.String(maxLength: 100),
                        ReturnServiceCentre = c.Int(nullable: false),
                        ReturnAddress = c.String(maxLength: 500),
                        Password = c.String(maxLength: 100),
                        FirstName = c.String(maxLength: 100),
                        LastName = c.String(maxLength: 100),
                        IsRegisteredFromMobile = c.Boolean(nullable: false),
                        isCodNeeded = c.Boolean(nullable: false),
                        WalletAmount = c.Decimal(precision: 18, scale: 2),
                        IsEligible = c.Boolean(),
                        AccountName = c.String(maxLength: 500),
                        AccountNumber = c.String(maxLength: 100),
                        ProductType = c.String(),
                        BankName = c.String(maxLength: 100),
                        Rank = c.Int(nullable: false),
                        BVN = c.String(maxLength: 50),
                        AssignedCustomerRep = c.String(),
                        IdentificationNumber = c.String(maxLength: 500),
                        IdentificationImageUrl = c.String(maxLength: 500),
                        IdentificationType = c.Int(nullable: false),
                        IsInternational = c.Boolean(nullable: false),
                        RankModificationDate = c.DateTime(nullable: false),
                        TransactionType = c.Int(nullable: false),
                        NUBANAccountNo = c.String(maxLength: 100),
                        PrefferedNubanBank = c.String(maxLength: 128),
                        NUBANCustomerId = c.Int(nullable: false),
                        NUBANCustomerCode = c.String(maxLength: 128),
                        NUBANCustomerName = c.String(maxLength: 300),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.CompanyId)
                .Index(t => t.PhoneNumber, unique: true);
            
            CreateTable(
                "dbo.CompanyContactPerson",
                c => new
                    {
                        CompanyContactPersonId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(maxLength: 100),
                        LastName = c.String(maxLength: 100),
                        Email = c.String(maxLength: 100),
                        Designation = c.String(maxLength: 100),
                        PhoneNumber = c.String(maxLength: 100),
                        CompanyId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.CompanyContactPersonId)
                .ForeignKey("dbo.Company", t => t.CompanyId, cascadeDelete: true)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.Country",
                c => new
                    {
                        CountryId = c.Int(nullable: false, identity: true),
                        CountryName = c.String(maxLength: 100),
                        CountryCode = c.String(maxLength: 100),
                        CurrencySymbol = c.String(maxLength: 100),
                        CurrencyCode = c.String(),
                        CurrencyRatio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsActive = c.Boolean(nullable: false),
                        PhoneNumberCode = c.String(maxLength: 10),
                        ContactNumber = c.String(maxLength: 100),
                        ContactEmail = c.String(maxLength: 100),
                        TermAndConditionAmount = c.String(maxLength: 100),
                        TermAndConditionCountry = c.String(maxLength: 100),
                        CountryFlag = c.String(maxLength: 300),
                        IsInternationalShippingCountry = c.Boolean(nullable: false),
                        CourierEnable = c.String(maxLength: 100),
                        CountryShortCode = c.String(maxLength: 10),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.CountryId);
            
            CreateTable(
                "dbo.CountryRouteZoneMap",
                c => new
                    {
                        CountryRouteZoneMapId = c.Int(nullable: false, identity: true),
                        ZoneId = c.Int(nullable: false),
                        DepartureId = c.Int(),
                        DestinationId = c.Int(),
                        Rate = c.Double(nullable: false),
                        Status = c.Boolean(nullable: false),
                        CompanyMap = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.CountryRouteZoneMapId)
                .ForeignKey("dbo.Country", t => t.DepartureId)
                .ForeignKey("dbo.Country", t => t.DestinationId)
                .ForeignKey("dbo.Zone", t => t.ZoneId, cascadeDelete: true)
                .Index(t => t.ZoneId)
                .Index(t => t.DepartureId)
                .Index(t => t.DestinationId);
            
            CreateTable(
                "dbo.Zone",
                c => new
                    {
                        ZoneId = c.Int(nullable: false, identity: true),
                        ZoneName = c.String(maxLength: 100),
                        Status = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ZoneId);
            
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
                "dbo.CustomerInvoice",
                c => new
                    {
                        CustomerInvoiceId = c.Int(nullable: false, identity: true),
                        InvoiceRefNo = c.String(maxLength: 128),
                        CustomerName = c.String(maxLength: 128),
                        PhoneNumber = c.String(maxLength: 128),
                        Email = c.String(maxLength: 128),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalVat = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Waybills = c.String(),
                        UserID = c.String(maxLength: 128),
                        CreatedBy = c.String(maxLength: 128),
                        CustomerCode = c.String(maxLength: 128),
                        PaymentStatus = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.CustomerInvoiceId);
            
            CreateTable(
                "dbo.InvoiceCharge",
                c => new
                    {
                        InvoiceChargeId = c.Int(nullable: false, identity: true),
                        CustomerInvoiceId = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Description = c.String(maxLength: 300),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.InvoiceChargeId)
                .ForeignKey("dbo.CustomerInvoice", t => t.CustomerInvoiceId, cascadeDelete: true)
                .Index(t => t.CustomerInvoiceId);
            
            CreateTable(
                "dbo.DeliveryLocation",
                c => new
                    {
                        DeliveryLocationId = c.Int(nullable: false, identity: true),
                        Location = c.String(maxLength: 100),
                        Tariff = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.DeliveryLocationId);
            
            CreateTable(
                "dbo.DeliveryNumber",
                c => new
                    {
                        DeliveryNumberId = c.Int(nullable: false, identity: true),
                        Number = c.String(maxLength: 20),
                        SenderCode = c.String(maxLength: 20),
                        ReceiverCode = c.String(maxLength: 20),
                        UserId = c.String(maxLength: 128),
                        IsUsed = c.Boolean(nullable: false),
                        Waybill = c.String(maxLength: 100),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.DeliveryNumberId);
            
            CreateTable(
                "dbo.DeliveryOption",
                c => new
                    {
                        DeliveryOptionId = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Description = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CustomerType = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.DeliveryOptionId);
            
            CreateTable(
                "dbo.DeliveryOptionPrice",
                c => new
                    {
                        DeliveryOptionPriceId = c.Int(nullable: false, identity: true),
                        ZoneId = c.Int(nullable: false),
                        DeliveryOptionId = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CountryId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.DeliveryOptionPriceId)
                .ForeignKey("dbo.DeliveryOption", t => t.DeliveryOptionId, cascadeDelete: true)
                .ForeignKey("dbo.Zone", t => t.ZoneId, cascadeDelete: true)
                .Index(t => t.ZoneId)
                .Index(t => t.DeliveryOptionId);
            
            CreateTable(
                "dbo.Demurrage",
                c => new
                    {
                        DemurrageId = c.Int(nullable: false, identity: true),
                        DayCount = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        WaybillNumber = c.String(maxLength: 100),
                        AmountPaid = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ApprovedBy = c.String(),
                        ApprovedId = c.String(),
                        UserId = c.String(maxLength: 128),
                        ServiceCenterId = c.Int(nullable: false),
                        ServiceCenterCode = c.String(maxLength: 50),
                        CountryId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.DemurrageId);
            
            CreateTable(
                "dbo.DemurrageRegisterAccount",
                c => new
                    {
                        DemurrageAccountId = c.Int(nullable: false, identity: true),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UserId = c.String(maxLength: 128),
                        Waybill = c.String(maxLength: 100),
                        DEMStatusHistory = c.Int(nullable: false),
                        ServiceCenterId = c.Int(nullable: false),
                        ServiceCenterCode = c.String(maxLength: 50),
                        Description = c.String(),
                        PaymentType = c.Int(nullable: false),
                        PaymentTypeReference = c.String(maxLength: 100),
                        DepositStatus = c.Int(nullable: false),
                        RefCode = c.String(maxLength: 100),
                        CountryId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.DemurrageAccountId);
            
            CreateTable(
                "dbo.Device",
                c => new
                    {
                        DeviceId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SerialNumber = c.String(maxLength: 100),
                        IMEI = c.String(maxLength: 100),
                        IMEI2 = c.String(maxLength: 100),
                        HandStarp = c.Boolean(nullable: false),
                        UsbCable = c.Boolean(nullable: false),
                        PowerAdapter = c.Boolean(nullable: false),
                        SimCardNumber = c.String(maxLength: 100),
                        NetworkProvider = c.String(maxLength: 100),
                        Description = c.String(),
                        Active = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.DeviceId);
            
            CreateTable(
                "dbo.DeviceManagement",
                c => new
                    {
                        DeviceManagementId = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        DeviceId = c.Int(nullable: false),
                        LocationId = c.Int(nullable: false),
                        DataSimCardNumber = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        Location_ServiceCentreId = c.Int(),
                    })
                .PrimaryKey(t => t.DeviceManagementId)
                .ForeignKey("dbo.Device", t => t.DeviceId, cascadeDelete: true)
                .ForeignKey("dbo.ServiceCentre", t => t.Location_ServiceCentreId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.DeviceId)
                .Index(t => t.Location_ServiceCentreId);
            
            CreateTable(
                "dbo.ServiceCentre",
                c => new
                    {
                        ServiceCentreId = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        Code = c.String(maxLength: 100),
                        Address = c.String(maxLength: 500),
                        City = c.String(maxLength: 100),
                        PhoneNumber = c.String(maxLength: 100),
                        Email = c.String(maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                        StationId = c.Int(nullable: false),
                        TargetAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TargetOrder = c.Int(nullable: false),
                        IsDefault = c.Boolean(nullable: false),
                        Latitude = c.Double(),
                        Longitude = c.Double(),
                        IsHUB = c.Boolean(nullable: false),
                        IsGateway = c.Boolean(nullable: false),
                        FormattedServiceCentreName = c.String(maxLength: 128),
                        IsPublic = c.Boolean(nullable: false),
                        LGAId = c.Int(nullable: false),
                        IsConsignable = c.Boolean(nullable: false),
                        CrAccount = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ServiceCentreId)
                .ForeignKey("dbo.Station", t => t.StationId, cascadeDelete: true)
                .Index(t => t.Name, unique: true)
                .Index(t => t.Code, unique: true)
                .Index(t => t.StationId);
            
            CreateTable(
                "dbo.Station",
                c => new
                    {
                        StationId = c.Int(nullable: false, identity: true),
                        StationName = c.String(maxLength: 100),
                        StationCode = c.String(maxLength: 100),
                        StateId = c.Int(nullable: false),
                        SuperServiceCentreId = c.Int(nullable: false),
                        StationPickupPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GIGGoActive = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.StationId)
                .ForeignKey("dbo.State", t => t.StateId, cascadeDelete: true)
                .Index(t => t.StateId);
            
            CreateTable(
                "dbo.State",
                c => new
                    {
                        StateId = c.Int(nullable: false, identity: true),
                        StateName = c.String(maxLength: 100),
                        StateCode = c.String(maxLength: 100),
                        CountryId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.StateId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Gender = c.Int(nullable: false),
                        Designation = c.String(),
                        Department = c.String(),
                        PictureUrl = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        Organisation = c.String(),
                        Status = c.Int(nullable: false),
                        UserType = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        UserChannelCode = c.String(),
                        UserChannelPassword = c.String(),
                        UserChannelType = c.Int(nullable: false),
                        SystemUserId = c.String(),
                        SystemUserRole = c.String(),
                        PasswordExpireDate = c.DateTime(nullable: false),
                        UserActiveCountryId = c.Int(nullable: false),
                        IsRegisteredFromMobile = c.Boolean(nullable: false),
                        AppType = c.String(),
                        IsUniqueInstalled = c.Boolean(),
                        RegistrationReferrercode = c.String(),
                        IsMagaya = c.Boolean(nullable: false),
                        IsInternational = c.Boolean(nullable: false),
                        IdentificationType = c.Int(nullable: false),
                        IdentificationImage = c.String(),
                        IdentificationNumber = c.String(maxLength: 100),
                        DashboardAccess = c.Boolean(nullable: false),
                        CountryType = c.String(),
                        AssignedEcommerceCustomer = c.Int(nullable: false),
                        IsRequestNewPassword = c.Boolean(nullable: false),
                        WalletAddress = c.String(),
                        PrivateKey = c.String(),
                        PublicKey = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SystemRoleId = c.String(),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Dispatch",
                c => new
                    {
                        DispatchId = c.Int(nullable: false, identity: true),
                        RegistrationNumber = c.String(),
                        ManifestNumber = c.String(maxLength: 50),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RescuedDispatchId = c.Int(nullable: false),
                        DriverDetail = c.String(),
                        DispatchedBy = c.String(),
                        ReceivedBy = c.String(),
                        DispatchCategory = c.Int(nullable: false),
                        ServiceCentreId = c.Int(),
                        DepartureId = c.Int(),
                        DestinationId = c.Int(),
                        DepartureServiceCenterId = c.Int(nullable: false),
                        DestinationServiceCenterId = c.Int(nullable: false),
                        IsSuperManifest = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.DispatchId)
                .ForeignKey("dbo.Station", t => t.DepartureId)
                .ForeignKey("dbo.Station", t => t.DestinationId)
                .ForeignKey("dbo.ServiceCentre", t => t.ServiceCentreId)
                .Index(t => t.ManifestNumber, unique: true)
                .Index(t => t.ServiceCentreId)
                .Index(t => t.DepartureId)
                .Index(t => t.DestinationId);
            
            CreateTable(
                "dbo.DispatchActivity",
                c => new
                    {
                        DispatchActivityId = c.Int(nullable: false, identity: true),
                        DispatchId = c.Int(nullable: false),
                        Description = c.String(),
                        Location = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.DispatchActivityId)
                .ForeignKey("dbo.Dispatch", t => t.DispatchId, cascadeDelete: true)
                .Index(t => t.DispatchId);
            
            CreateTable(
                "dbo.DomesticRouteZoneMap",
                c => new
                    {
                        DomesticRouteZoneMapId = c.Int(nullable: false, identity: true),
                        ZoneId = c.Int(nullable: false),
                        DepartureId = c.Int(),
                        DestinationId = c.Int(),
                        Status = c.Boolean(nullable: false),
                        ETA = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.DomesticRouteZoneMapId)
                .ForeignKey("dbo.Station", t => t.DepartureId)
                .ForeignKey("dbo.Station", t => t.DestinationId)
                .ForeignKey("dbo.Zone", t => t.ZoneId, cascadeDelete: true)
                .Index(t => t.ZoneId)
                .Index(t => t.DepartureId)
                .Index(t => t.DestinationId);
            
            CreateTable(
                "dbo.DomesticZonePrice",
                c => new
                    {
                        DomesticZonePriceId = c.Int(nullable: false, identity: true),
                        Weight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ZoneId = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RegularEcommerceType = c.Int(nullable: false),
                        CountryId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.DomesticZonePriceId)
                .ForeignKey("dbo.Zone", t => t.ZoneId, cascadeDelete: true)
                .Index(t => t.ZoneId);
            
            CreateTable(
                "dbo.EcommerceAgreement",
                c => new
                    {
                        EcommerceAgreementId = c.Int(nullable: false, identity: true),
                        BusinessEmail = c.String(maxLength: 100),
                        BusinessOwnerName = c.String(maxLength: 200),
                        OfficeAddress = c.String(maxLength: 500),
                        City = c.String(maxLength: 50),
                        State = c.String(maxLength: 50),
                        CountryId = c.Int(nullable: false),
                        ContactName = c.String(maxLength: 200),
                        ReturnAddress = c.String(maxLength: 500),
                        ContactEmail = c.String(maxLength: 100),
                        ContactPhoneNumber = c.String(maxLength: 100),
                        AgreementDate = c.DateTime(),
                        EcommerceSignature = c.String(maxLength: 500),
                        NatureOfBusiness = c.String(maxLength: 500),
                        Status = c.Int(nullable: false),
                        IsCod = c.Boolean(nullable: false),
                        AccountName = c.String(maxLength: 500),
                        AccountNumber = c.String(maxLength: 100),
                        BankName = c.String(maxLength: 100),
                        IsApi = c.Boolean(nullable: false),
                        Source = c.String(maxLength: 100),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.EcommerceAgreementId);
            
            CreateTable(
                "dbo.EmailSendLog",
                c => new
                    {
                        EmailSendLogId = c.Int(nullable: false, identity: true),
                        To = c.String(maxLength: 128),
                        From = c.String(),
                        Message = c.String(),
                        Status = c.Int(nullable: false),
                        User = c.String(maxLength: 128),
                        ResultStatus = c.String(),
                        ResultDescription = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.EmailSendLogId);
            
            CreateTable(
                "dbo.Expenditure",
                c => new
                    {
                        ExpenditureId = c.Int(nullable: false, identity: true),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Description = c.String(),
                        ExpenseTypeId = c.Int(nullable: false),
                        ServiceCentreId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ExpenditureId)
                .ForeignKey("dbo.ExpenseType", t => t.ExpenseTypeId, cascadeDelete: true)
                .ForeignKey("dbo.ServiceCentre", t => t.ServiceCentreId, cascadeDelete: true)
                .Index(t => t.ExpenseTypeId)
                .Index(t => t.ServiceCentreId);
            
            CreateTable(
                "dbo.ExpenseType",
                c => new
                    {
                        ExpenseTypeId = c.Int(nullable: false, identity: true),
                        ExpenseTypeName = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ExpenseTypeId);
            
            CreateTable(
                "dbo.FinancialReport",
                c => new
                    {
                        FinancialReportId = c.Int(nullable: false, identity: true),
                        Source = c.Int(nullable: false),
                        Waybill = c.String(maxLength: 50),
                        GrandTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PartnerEarnings = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Earnings = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Demurrage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CountryId = c.Int(nullable: false),
                        ConversionRate = c.Double(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.FinancialReportId);
            
            CreateTable(
                "dbo.Fleet",
                c => new
                    {
                        FleetId = c.Int(nullable: false, identity: true),
                        RegistrationNumber = c.String(maxLength: 100),
                        ChassisNumber = c.String(),
                        EngineNumber = c.String(),
                        Status = c.Boolean(nullable: false),
                        FleetType = c.Int(nullable: false),
                        Capacity = c.Int(nullable: false),
                        Description = c.String(),
                        ModelId = c.Int(nullable: false),
                        PartnerId = c.Int(nullable: false),
                        FleetName = c.String(),
                        EnterprisePartnerId = c.String(maxLength: 128),
                        IsFixed = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        FleetMake_MakeId = c.Int(),
                    })
                .PrimaryKey(t => t.FleetId)
                .ForeignKey("dbo.AspNetUsers", t => t.EnterprisePartnerId)
                .ForeignKey("dbo.FleetMake", t => t.FleetMake_MakeId)
                .ForeignKey("dbo.FleetModel", t => t.ModelId, cascadeDelete: true)
                .ForeignKey("dbo.Partner", t => t.PartnerId, cascadeDelete: true)
                .Index(t => t.ModelId)
                .Index(t => t.PartnerId)
                .Index(t => t.EnterprisePartnerId)
                .Index(t => t.FleetMake_MakeId);
            
            CreateTable(
                "dbo.FleetModel",
                c => new
                    {
                        ModelId = c.Int(nullable: false, identity: true),
                        ModelName = c.String(),
                        MakeId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ModelId)
                .ForeignKey("dbo.FleetMake", t => t.MakeId, cascadeDelete: true)
                .Index(t => t.MakeId);
            
            CreateTable(
                "dbo.FleetMake",
                c => new
                    {
                        MakeId = c.Int(nullable: false, identity: true),
                        MakeName = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.MakeId);
            
            CreateTable(
                "dbo.Partner",
                c => new
                    {
                        PartnerId = c.Int(nullable: false, identity: true),
                        PartnerName = c.String(maxLength: 100),
                        PartnerCode = c.String(maxLength: 100),
                        FirstName = c.String(maxLength: 100),
                        LastName = c.String(maxLength: 100),
                        Email = c.String(maxLength: 100),
                        PhoneNumber = c.String(maxLength: 100),
                        OptionalPhoneNumber = c.String(maxLength: 100),
                        Address = c.String(),
                        PartnerType = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        IsActivated = c.Boolean(nullable: false),
                        VehicleType = c.String(maxLength: 100),
                        PictureUrl = c.String(),
                        BankName = c.String(maxLength: 100),
                        AccountNumber = c.String(maxLength: 100),
                        AccountName = c.String(maxLength: 100),
                        VehicleLicenseNumber = c.String(maxLength: 100),
                        VehicleLicenseExpiryDate = c.DateTime(),
                        VehicleLicenseImageDetails = c.String(),
                        UserActiveCountryId = c.Int(nullable: false),
                        FleetPartnerCode = c.String(maxLength: 100),
                        ActivityStatus = c.Int(nullable: false),
                        ActivityDate = c.DateTime(nullable: false),
                        Contacted = c.Boolean(nullable: false),
                        CaptainBankName = c.String(maxLength: 100),
                        CaptainAccountNumber = c.String(maxLength: 100),
                        CaptainAccountName = c.String(maxLength: 100),
                        Age = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.PartnerId);
            
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
                "dbo.FleetPart",
                c => new
                    {
                        PartId = c.Int(nullable: false, identity: true),
                        PartName = c.String(),
                        ModelId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.PartId)
                .ForeignKey("dbo.FleetModel", t => t.ModelId, cascadeDelete: true)
                .Index(t => t.ModelId);
            
            CreateTable(
                "dbo.FleetPartInventory",
                c => new
                    {
                        FleetPartInventoryId = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PartId = c.Int(nullable: false),
                        StoreId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.FleetPartInventoryId)
                .ForeignKey("dbo.FleetPart", t => t.PartId, cascadeDelete: true)
                .ForeignKey("dbo.Store", t => t.StoreId, cascadeDelete: true)
                .Index(t => t.PartId)
                .Index(t => t.StoreId);
            
            CreateTable(
                "dbo.Store",
                c => new
                    {
                        StoreId = c.Int(nullable: false, identity: true),
                        StoreName = c.String(maxLength: 50),
                        Address = c.String(maxLength: 500),
                        City = c.String(maxLength: 50),
                        State = c.String(maxLength: 50),
                        URL = c.String(maxLength: 200),
                        storeImage = c.String(maxLength: 300),
                        CountryId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.StoreId);
            
            CreateTable(
                "dbo.FleetPartInventoryHistory",
                c => new
                    {
                        FleetPartInventoryHistoryId = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Remark = c.String(),
                        InventoryType = c.Int(nullable: false),
                        InitialBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CurrentBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PartId = c.Int(nullable: false),
                        StoreId = c.Int(nullable: false),
                        VendorId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        MovedBy_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.FleetPartInventoryHistoryId)
                .ForeignKey("dbo.FleetPart", t => t.PartId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.MovedBy_Id)
                .ForeignKey("dbo.Store", t => t.StoreId, cascadeDelete: true)
                .ForeignKey("dbo.Vendor", t => t.VendorId, cascadeDelete: true)
                .Index(t => t.PartId)
                .Index(t => t.StoreId)
                .Index(t => t.VendorId)
                .Index(t => t.MovedBy_Id);
            
            CreateTable(
                "dbo.Vendor",
                c => new
                    {
                        VendorId = c.Int(nullable: false, identity: true),
                        VendorName = c.String(),
                        ContactName = c.String(),
                        Address = c.String(),
                        Email = c.String(),
                        PhoneNumber = c.String(maxLength: 100),
                        CompanyRegistrationNumber = c.String(),
                        BankName = c.String(),
                        BankAccountNumber = c.String(),
                        VendorType = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.VendorId);
            
            CreateTable(
                "dbo.FleetPartner",
                c => new
                    {
                        FleetPartnerId = c.Int(nullable: false, identity: true),
                        FleetPartnerCode = c.String(maxLength: 100),
                        FirstName = c.String(maxLength: 100),
                        LastName = c.String(maxLength: 100),
                        Email = c.String(maxLength: 100),
                        PhoneNumber = c.String(maxLength: 100),
                        Address = c.String(),
                        UserId = c.String(maxLength: 128),
                        UserActiveCountryId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.FleetPartnerId);
            
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
                "dbo.FleetTrip",
                c => new
                    {
                        FleetTripId = c.Int(nullable: false, identity: true),
                        DepartureDestination = c.String(),
                        ActualDestination = c.String(),
                        ExpectedDestination = c.String(),
                        DepartureTime = c.DateTime(nullable: false),
                        ArrivalTime = c.DateTime(nullable: false),
                        DistanceTravelled = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FuelCosts = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FuelUsed = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FleetId = c.Int(nullable: false),
                        CaptainId = c.String(maxLength: 128),
                        MovementManifestId = c.Int(nullable: false),
                        FleetRegistrationNumber = c.String(),
                        DispatchAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DepartureStationId = c.Int(),
                        DestinationStationId = c.Int(),
                        DepartureServiceCenterId = c.Int(nullable: false),
                        DestinationServiceCenterId = c.Int(nullable: false),
                        TripAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.FleetTripId)
                .ForeignKey("dbo.AspNetUsers", t => t.CaptainId)
                .ForeignKey("dbo.Station", t => t.DepartureStationId)
                .ForeignKey("dbo.Station", t => t.DestinationStationId)
                .ForeignKey("dbo.Fleet", t => t.FleetId, cascadeDelete: true)
                .Index(t => t.FleetId)
                .Index(t => t.CaptainId)
                .Index(t => t.DepartureStationId)
                .Index(t => t.DestinationStationId);
            
            CreateTable(
                "dbo.GeneralLedger",
                c => new
                    {
                        GeneralLedgerId = c.Int(nullable: false, identity: true),
                        DateOfEntry = c.DateTime(nullable: false),
                        ServiceCentreId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreditDebitType = c.Int(nullable: false),
                        Description = c.String(),
                        IsDeferred = c.Boolean(nullable: false),
                        Waybill = c.String(maxLength: 100),
                        ClientNodeId = c.String(),
                        PaymentType = c.Int(nullable: false),
                        PaymentTypeReference = c.String(),
                        PaymentServiceType = c.Int(nullable: false),
                        IsInternational = c.Boolean(nullable: false),
                        CountryId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.GeneralLedgerId)
                .ForeignKey("dbo.ServiceCentre", t => t.ServiceCentreId, cascadeDelete: true)
                .Index(t => t.ServiceCentreId);
            
            CreateTable(
                "dbo.GeneralLedger_Archive",
                c => new
                    {
                        GeneralLedgerId = c.Int(nullable: false, identity: true),
                        DateOfEntry = c.DateTime(nullable: false),
                        ServiceCentreId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreditDebitType = c.Int(nullable: false),
                        Description = c.String(),
                        IsDeferred = c.Boolean(nullable: false),
                        Waybill = c.String(maxLength: 100),
                        ClientNodeId = c.String(),
                        PaymentType = c.Int(nullable: false),
                        PaymentTypeReference = c.String(),
                        PaymentServiceType = c.Int(nullable: false),
                        IsInternational = c.Boolean(nullable: false),
                        CountryId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.GeneralLedgerId);
            
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
                "dbo.GiglgoStation",
                c => new
                    {
                        GiglgoStationId = c.Int(nullable: false, identity: true),
                        StationId = c.Int(nullable: false),
                        StateName = c.String(),
                        StationCode = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.GiglgoStationId);
            
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
                "dbo.GlobalProperty",
                c => new
                    {
                        GlobalPropertyId = c.Int(nullable: false, identity: true),
                        Key = c.String(maxLength: 100),
                        Value = c.String(maxLength: 500),
                        Description = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CountryId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.GlobalPropertyId);
            
            CreateTable(
                "dbo.GroupWaybillNumber",
                c => new
                    {
                        GroupWaybillNumberId = c.Int(nullable: false, identity: true),
                        GroupWaybillCode = c.String(maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                        UserId = c.String(maxLength: 128),
                        ServiceCentreId = c.Int(nullable: false),
                        HasManifest = c.Boolean(nullable: false),
                        DepartureServiceCentreId = c.Int(nullable: false),
                        ExpressDelivery = c.Boolean(nullable: false),
                        IsBulky = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.GroupWaybillNumberId)
                .ForeignKey("dbo.ServiceCentre", t => t.ServiceCentreId, cascadeDelete: true)
                .Index(t => t.GroupWaybillCode, unique: true)
                .Index(t => t.ServiceCentreId);
            
            CreateTable(
                "dbo.GroupWaybillNumberMapping",
                c => new
                    {
                        GroupWaybillNumberMappingId = c.Int(nullable: false, identity: true),
                        DateMapped = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        GroupWaybillNumber = c.String(maxLength: 100),
                        WaybillNumber = c.String(maxLength: 100),
                        DepartureServiceCentreId = c.Int(nullable: false),
                        DestinationServiceCentreId = c.Int(nullable: false),
                        OriginalDepartureServiceCentreId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        ExpressDelivery = c.Boolean(nullable: false),
                        IsBulky = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        DepartureServiceCentre_ServiceCentreId = c.Int(),
                        DestinationServiceCentre_ServiceCentreId = c.Int(),
                        OriginalDepartureServiceCentre_ServiceCentreId = c.Int(),
                    })
                .PrimaryKey(t => t.GroupWaybillNumberMappingId)
                .ForeignKey("dbo.ServiceCentre", t => t.DepartureServiceCentre_ServiceCentreId)
                .ForeignKey("dbo.ServiceCentre", t => t.DestinationServiceCentre_ServiceCentreId)
                .ForeignKey("dbo.ServiceCentre", t => t.OriginalDepartureServiceCentre_ServiceCentreId)
                .Index(t => t.DepartureServiceCentre_ServiceCentreId)
                .Index(t => t.DestinationServiceCentre_ServiceCentreId)
                .Index(t => t.OriginalDepartureServiceCentre_ServiceCentreId);
            
            CreateTable(
                "dbo.Haulage",
                c => new
                    {
                        HaulageId = c.Int(nullable: false, identity: true),
                        Tonne = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Description = c.String(),
                        Status = c.Boolean(nullable: false),
                        FixedRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AdditionalRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CountryId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.HaulageId);
            
            CreateTable(
                "dbo.HaulageDistanceMapping",
                c => new
                    {
                        HaulageDistanceMappingId = c.Int(nullable: false, identity: true),
                        Distance = c.Int(nullable: false),
                        DepartureId = c.Int(),
                        DestinationId = c.Int(),
                        Status = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.HaulageDistanceMappingId)
                .ForeignKey("dbo.Station", t => t.DepartureId)
                .ForeignKey("dbo.Station", t => t.DestinationId)
                .Index(t => t.DepartureId)
                .Index(t => t.DestinationId);
            
            CreateTable(
                "dbo.HaulageDistanceMappingPrice",
                c => new
                    {
                        HaulageDistanceMappingPriceId = c.Int(nullable: false, identity: true),
                        StartRange = c.Int(nullable: false),
                        EndRange = c.Int(nullable: false),
                        HaulageId = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CountryId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.HaulageDistanceMappingPriceId)
                .ForeignKey("dbo.Haulage", t => t.HaulageId, cascadeDelete: true)
                .Index(t => t.HaulageId);
            
            CreateTable(
                "dbo.HUBManifestWaybillMapping",
                c => new
                    {
                        HUBManifestWaybillMappingId = c.Int(nullable: false, identity: true),
                        IsActive = c.Boolean(nullable: false),
                        ManifestCode = c.String(maxLength: 100),
                        Waybill = c.String(maxLength: 100),
                        ServiceCentreId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.HUBManifestWaybillMappingId)
                .ForeignKey("dbo.ServiceCentre", t => t.ServiceCentreId, cascadeDelete: true)
                .Index(t => t.ServiceCentreId);
            
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
                "dbo.IndividualCustomer",
                c => new
                    {
                        IndividualCustomerId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(maxLength: 500),
                        LastName = c.String(maxLength: 500),
                        Gender = c.Int(nullable: false),
                        Email = c.String(maxLength: 500),
                        City = c.String(),
                        State = c.String(),
                        Address = c.String(),
                        UserActiveCountryId = c.Int(nullable: false),
                        PhoneNumber = c.String(maxLength: 20),
                        PictureUrl = c.String(),
                        CustomerCode = c.String(maxLength: 100),
                        Password = c.String(),
                        IsRegisteredFromMobile = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.IndividualCustomerId)
                .Index(t => t.PhoneNumber, unique: true);
            
            CreateTable(
                "dbo.Insurance",
                c => new
                    {
                        InsuranceId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CountryId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.InsuranceId);
            
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
                "dbo.InternationalRequestReceiverItem",
                c => new
                    {
                        InternationalRequestReceiverItemId = c.Int(nullable: false, identity: true),
                        InternationalRequestReceiverId = c.Int(nullable: false),
                        Description = c.String(),
                        Quantity = c.String(),
                        Weight = c.String(),
                        Width = c.String(),
                        Length = c.String(),
                        Height = c.String(),
                        Value = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.InternationalRequestReceiverItemId)
                .ForeignKey("dbo.InternationalRequestReceiver", t => t.InternationalRequestReceiverId, cascadeDelete: true)
                .Index(t => t.InternationalRequestReceiverId);
            
            CreateTable(
                "dbo.InternationalRequestReceiver",
                c => new
                    {
                        InternationalRequestReceiverId = c.Int(nullable: false, identity: true),
                        CustomerId = c.String(),
                        GenerateCode = c.String(),
                        Name = c.String(),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                        Address = c.String(),
                        City = c.String(),
                        Country = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.InternationalRequestReceiverId);
            
            CreateTable(
                "dbo.InternationalShipmentWaybill",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Waybill = c.String(maxLength: 100),
                        ShipmentIdentificationNumber = c.String(maxLength: 100),
                        PackageResult = c.String(),
                        InternationalShipmentStatus = c.Int(nullable: false),
                        ResponseResult = c.String(),
                        OutBoundChannel = c.Int(nullable: false),
                        IsFromMobile = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IntlShipmentRequest",
                c => new
                    {
                        IntlShipmentRequestId = c.Int(nullable: false, identity: true),
                        RequestNumber = c.String(maxLength: 100),
                        UserId = c.String(maxLength: 128),
                        CustomerFirstName = c.String(maxLength: 50),
                        CustomerLastName = c.String(maxLength: 50),
                        CustomerType = c.String(maxLength: 50),
                        CustomerId = c.Int(nullable: false),
                        CustomerCountryId = c.Int(nullable: false),
                        CustomerAddress = c.String(maxLength: 500),
                        CustomerEmail = c.String(maxLength: 100),
                        CustomerPhoneNumber = c.String(maxLength: 100),
                        CustomerCity = c.String(maxLength: 50),
                        CustomerState = c.String(maxLength: 50),
                        ItemSenderfullName = c.String(maxLength: 150),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DestinationServiceCentreId = c.Int(nullable: false),
                        DestinationCountryId = c.Int(nullable: false),
                        ReceiverName = c.String(maxLength: 200),
                        ReceiverPhoneNumber = c.String(maxLength: 100),
                        ReceiverEmail = c.String(maxLength: 100),
                        ReceiverAddress = c.String(maxLength: 500),
                        ReceiverCity = c.String(maxLength: 50),
                        ReceiverState = c.String(maxLength: 50),
                        ReceiverCountry = c.String(maxLength: 50),
                        PickupOptions = c.Int(nullable: false),
                        ApproximateItemsWeight = c.Double(nullable: false),
                        GrandTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Total = c.Decimal(precision: 18, scale: 2),
                        PaymentMethod = c.String(maxLength: 20),
                        SenderAddress = c.String(maxLength: 500),
                        SenderState = c.String(maxLength: 50),
                        IsProcessed = c.Boolean(nullable: false),
                        IsMobile = c.Boolean(nullable: false),
                        Consolidated = c.Boolean(nullable: false),
                        ConsolidationId = c.String(maxLength: 128),
                        RequestProcessingCountryId = c.Int(nullable: false),
                        DeliveryType = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        DestinationServiceCentre_ServiceCentreId = c.Int(),
                    })
                .PrimaryKey(t => t.IntlShipmentRequestId)
                .ForeignKey("dbo.ServiceCentre", t => t.DestinationServiceCentre_ServiceCentreId)
                .Index(t => t.RequestNumber, unique: true)
                .Index(t => t.DestinationServiceCentre_ServiceCentreId);
            
            CreateTable(
                "dbo.IntlShipmentRequestItem",
                c => new
                    {
                        IntlShipmentRequestItemId = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        ItemName = c.String(),
                        TrackingId = c.String(),
                        storeName = c.String(),
                        ShipmentType = c.Int(nullable: false),
                        Weight = c.Double(nullable: false),
                        Nature = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        SerialNumber = c.Int(nullable: false),
                        IsVolumetric = c.Boolean(nullable: false),
                        Length = c.Double(nullable: false),
                        Width = c.Double(nullable: false),
                        Height = c.Double(nullable: false),
                        ItemSenderfullName = c.String(),
                        IntlShipmentRequestId = c.Int(nullable: false),
                        RequiresInsurance = c.Boolean(nullable: false),
                        ItemValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemCount = c.String(maxLength: 128),
                        Received = c.Boolean(nullable: false),
                        ReceivedBy = c.String(maxLength: 128),
                        CourierService = c.String(maxLength: 300),
                        ItemUniqueNo = c.String(maxLength: 300),
                        ReceivedDate = c.DateTime(),
                        ItemState = c.Int(nullable: false),
                        ItemRequestCode = c.String(maxLength: 128),
                        ItemStateDescription = c.String(maxLength: 500),
                        NoOfPackageReceived = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.IntlShipmentRequestItemId)
                .ForeignKey("dbo.IntlShipmentRequest", t => t.IntlShipmentRequestId, cascadeDelete: true)
                .Index(t => t.IntlShipmentRequestId);
            
            CreateTable(
                "dbo.Invoice",
                c => new
                    {
                        InvoiceId = c.Int(nullable: false, identity: true),
                        InvoiceNo = c.String(maxLength: 100),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaymentStatus = c.Int(nullable: false),
                        PaymentMethod = c.String(maxLength: 100),
                        PaymentDate = c.DateTime(nullable: false),
                        Waybill = c.String(maxLength: 100),
                        DueDate = c.DateTime(nullable: false),
                        ServiceCentreId = c.Int(nullable: false),
                        IsInternational = c.Boolean(nullable: false),
                        IsShipmentCollected = c.Boolean(nullable: false),
                        PaymentTypeReference = c.String(maxLength: 100),
                        CountryId = c.Int(nullable: false),
                        Cash = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Transfer = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Pos = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Note = c.String(maxLength: 450),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.InvoiceId)
                .Index(t => t.InvoiceNo, unique: true);
            
            CreateTable(
                "dbo.Invoice_Archive",
                c => new
                    {
                        InvoiceId = c.Int(nullable: false, identity: true),
                        InvoiceNo = c.String(maxLength: 100),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaymentStatus = c.Int(nullable: false),
                        PaymentMethod = c.String(maxLength: 100),
                        PaymentDate = c.DateTime(nullable: false),
                        Waybill = c.String(maxLength: 100),
                        DueDate = c.DateTime(nullable: false),
                        ServiceCentreId = c.Int(nullable: false),
                        IsInternational = c.Boolean(nullable: false),
                        IsShipmentCollected = c.Boolean(nullable: false),
                        PaymentTypeReference = c.String(maxLength: 100),
                        CountryId = c.Int(nullable: false),
                        Cash = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Transfer = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Pos = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.InvoiceId);
            
            CreateTable(
                "dbo.InvoiceShipment",
                c => new
                    {
                        InvoiceShipmentId = c.Int(nullable: false, identity: true),
                        InvoiceId = c.Int(nullable: false),
                        ShipmentId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.InvoiceShipmentId);
            
            CreateTable(
                "dbo.JobCard",
                c => new
                    {
                        JobCardId = c.Int(nullable: false, identity: true),
                        JobDescription = c.String(),
                        JobCardType = c.Int(nullable: false),
                        JobCardStatus = c.Int(nullable: false),
                        MaintenanceType = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        Approver_Id = c.String(maxLength: 128),
                        Fleet_FleetId = c.Int(),
                        Requester_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.JobCardId)
                .ForeignKey("dbo.AspNetUsers", t => t.Approver_Id)
                .ForeignKey("dbo.Fleet", t => t.Fleet_FleetId)
                .ForeignKey("dbo.AspNetUsers", t => t.Requester_Id)
                .Index(t => t.Approver_Id)
                .Index(t => t.Fleet_FleetId)
                .Index(t => t.Requester_Id);
            
            CreateTable(
                "dbo.JobCardManagement",
                c => new
                    {
                        JobCardManagementId = c.Int(nullable: false, identity: true),
                        SupervisorComment = c.String(),
                        MechanicComment = c.String(),
                        EntryDate = c.DateTime(nullable: false),
                        ReleaseDate = c.DateTime(nullable: false),
                        EstimatedCompletionDate = c.DateTime(nullable: false),
                        DateStarted = c.DateTime(nullable: false),
                        DateCompleted = c.DateTime(nullable: false),
                        JobCardMaintenanceStatus = c.Int(nullable: false),
                        WorkshopId = c.Int(nullable: false),
                        JobCardId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        MechanicSupervisor_Id = c.String(maxLength: 128),
                        MechanicUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.JobCardManagementId)
                .ForeignKey("dbo.JobCard", t => t.JobCardId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.MechanicSupervisor_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.MechanicUser_Id)
                .ForeignKey("dbo.Workshop", t => t.WorkshopId, cascadeDelete: true)
                .Index(t => t.WorkshopId)
                .Index(t => t.JobCardId)
                .Index(t => t.MechanicSupervisor_Id)
                .Index(t => t.MechanicUser_Id);
            
            CreateTable(
                "dbo.Workshop",
                c => new
                    {
                        WorkshopId = c.Int(nullable: false, identity: true),
                        WorkshopName = c.String(maxLength: 100),
                        Address = c.String(maxLength: 500),
                        City = c.String(maxLength: 100),
                        State = c.String(maxLength: 100),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        WorkshopSupervisor_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.WorkshopId)
                .ForeignKey("dbo.AspNetUsers", t => t.WorkshopSupervisor_Id)
                .Index(t => t.WorkshopSupervisor_Id);
            
            CreateTable(
                "dbo.JobCardManagementPart",
                c => new
                    {
                        JobCardManagementPartId = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        PartId = c.Int(nullable: false),
                        JobCardManagementId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.JobCardManagementPartId)
                .ForeignKey("dbo.FleetPart", t => t.PartId, cascadeDelete: true)
                .ForeignKey("dbo.JobCardManagement", t => t.JobCardManagementId, cascadeDelete: true)
                .Index(t => t.PartId)
                .Index(t => t.JobCardManagementId);
            
            CreateTable(
                "dbo.LGA",
                c => new
                    {
                        LGAId = c.Int(nullable: false, identity: true),
                        LGAName = c.String(maxLength: 100),
                        LGAState = c.String(maxLength: 100),
                        Status = c.Boolean(nullable: false),
                        StateId = c.Int(nullable: false),
                        HomeDeliveryStatus = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.LGAId);
            
            CreateTable(
                "dbo.Location",
                c => new
                    {
                        LocationId = c.Int(nullable: false, identity: true),
                        Latitude = c.Double(),
                        Longitude = c.Double(),
                        Name = c.String(maxLength: 500),
                        FormattedAddress = c.String(maxLength: 500),
                        LGA = c.String(maxLength: 128),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.LocationId);
            
            CreateTable(
                "dbo.LogEntry",
                c => new
                    {
                        LogEntryId = c.Int(nullable: false, identity: true),
                        CallSite = c.String(),
                        DateTime = c.String(),
                        Level = c.String(),
                        Logger = c.String(),
                        MachineName = c.String(),
                        Username = c.String(),
                        ErrorSource = c.String(),
                        ErrorClass = c.String(),
                        ErrorMethod = c.String(),
                        ErrorMessage = c.String(),
                        InnerErrorMessage = c.String(),
                        Exception = c.String(),
                        StackTrace = c.String(),
                        Thread = c.String(),
                    })
                .PrimaryKey(t => t.LogEntryId);
            
            CreateTable(
                "dbo.LogVisitReason",
                c => new
                    {
                        LogVisitReasonId = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.LogVisitReasonId);
            
            CreateTable(
                "dbo.MagayaShipmentAgility",
                c => new
                    {
                        MagayaShipmentId = c.Int(nullable: false, identity: true),
                        ServiceCenterCountryId = c.Int(nullable: false),
                        ServiceCenterId = c.Int(nullable: false),
                        SealNumber = c.String(maxLength: 20),
                        Waybill = c.String(),
                        Value = c.Decimal(precision: 18, scale: 2),
                        DeliveryTime = c.DateTime(),
                        PaymentStatus = c.Int(nullable: false),
                        Type = c.String(maxLength: 100),
                        CompanyType = c.String(maxLength: 100),
                        CustomerCode = c.String(maxLength: 100),
                        OriginPort = c.String(),
                        DestinationPort = c.String(),
                        ShipperName = c.String(maxLength: 200),
                        ShipperPhoneNumber = c.String(maxLength: 100),
                        ShipperEmail = c.String(maxLength: 100),
                        ShipperAddress = c.String(maxLength: 500),
                        ShipperCity = c.String(maxLength: 50),
                        ShipperState = c.String(maxLength: 50),
                        ShipperCountry = c.String(maxLength: 50),
                        ConsigneeName = c.String(maxLength: 200),
                        ConsigneePhoneNumber = c.String(maxLength: 100),
                        ConsigneeEmail = c.String(maxLength: 100),
                        ConsigneeAddress = c.String(maxLength: 500),
                        ConsigneeCity = c.String(maxLength: 50),
                        ConsigneeState = c.String(maxLength: 50),
                        ConsigneeCountry = c.String(maxLength: 50),
                        PickupOptions = c.Int(nullable: false),
                        ExpectedDateOfArrival = c.DateTime(),
                        ActualDateOfArrival = c.DateTime(),
                        MagayaShipmentItemsXml = c.String(),
                        ShipmentType = c.String(),
                        ApproximateItemsWeight = c.Double(nullable: false),
                        GrandTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsCashOnDelivery = c.Boolean(nullable: false),
                        CashOnDeliveryAmount = c.Decimal(precision: 18, scale: 2),
                        ExpectedAmountToCollect = c.Decimal(precision: 18, scale: 2),
                        ActualAmountCollected = c.Decimal(precision: 18, scale: 2),
                        ShipmentGUID = c.Guid(nullable: false),
                        IsdeclaredVal = c.Boolean(nullable: false),
                        DeclarationOfValueCheck = c.Decimal(precision: 18, scale: 2),
                        AppliedDiscount = c.Decimal(precision: 18, scale: 2),
                        DiscountValue = c.Decimal(precision: 18, scale: 2),
                        Insurance = c.Decimal(precision: 18, scale: 2),
                        Vat = c.Decimal(precision: 18, scale: 2),
                        Total = c.Decimal(precision: 18, scale: 2),
                        ShipmentPackagePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ShipmentPickupPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        vatvalue_display = c.Decimal(precision: 18, scale: 2),
                        InvoiceDiscountValue_display = c.Decimal(precision: 18, scale: 2),
                        offInvoiceDiscountvalue_display = c.Decimal(precision: 18, scale: 2),
                        PaymentMethod = c.String(maxLength: 20),
                        IsCancelled = c.Boolean(nullable: false),
                        IsInternational = c.Boolean(nullable: false),
                        Description = c.String(maxLength: 500),
                        DepositStatus = c.Int(nullable: false),
                        ReprintCounterStatus = c.Boolean(nullable: false),
                        SenderAddress = c.String(maxLength: 500),
                        IsCODPaidOut = c.Boolean(nullable: false),
                        ShipmentScanStatus = c.Int(nullable: false),
                        IsGrouped = c.Boolean(nullable: false),
                        CurrencyRatio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DeliveryNumber = c.String(maxLength: 20),
                        IsFromMobile = c.Boolean(nullable: false),
                        IsShipmentCollected = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.MagayaShipmentId);
            
            CreateTable(
                "dbo.MagayaShipmentItem",
                c => new
                    {
                        MagayaShipmentItemId = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Description_s = c.String(),
                        Weight = c.Double(nullable: false),
                        Nature = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Double(nullable: false),
                        Length = c.Double(nullable: false),
                        Width = c.Double(nullable: false),
                        Height = c.Double(nullable: false),
                        VolumeWeight = c.Double(nullable: false),
                        MagayaShipmentId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.MagayaShipmentItemId)
                .ForeignKey("dbo.MagayaShipmentAgility", t => t.MagayaShipmentId, cascadeDelete: true)
                .Index(t => t.MagayaShipmentId);
            
            CreateTable(
                "dbo.MainNav",
                c => new
                    {
                        MainNavId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        State = c.String(),
                        Param = c.String(),
                        Position = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.MainNavId);
            
            CreateTable(
                "dbo.SubNav",
                c => new
                    {
                        SubNavId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        State = c.String(),
                        Param = c.String(),
                        MainNavId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.SubNavId)
                .ForeignKey("dbo.MainNav", t => t.MainNavId, cascadeDelete: true)
                .Index(t => t.MainNavId);
            
            CreateTable(
                "dbo.SubSubNav",
                c => new
                    {
                        SubSubNavId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        State = c.String(),
                        Param = c.String(),
                        SubNavId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.SubSubNavId)
                .ForeignKey("dbo.SubNav", t => t.SubNavId, cascadeDelete: true)
                .Index(t => t.SubNavId);
            
            CreateTable(
                "dbo.Manifest",
                c => new
                    {
                        ManifestId = c.Int(nullable: false, identity: true),
                        ManifestCode = c.String(maxLength: 100),
                        DateTime = c.DateTime(nullable: false),
                        ShipmentId = c.Int(),
                        DispatchedById = c.String(),
                        ReceiverById = c.String(),
                        FleetTripId = c.Int(),
                        IsDispatched = c.Boolean(nullable: false),
                        IsReceived = c.Boolean(nullable: false),
                        ServiceCentreId = c.Int(nullable: false),
                        ManifestType = c.Int(nullable: false),
                        DepartureServiceCentreId = c.Int(nullable: false),
                        DestinationServiceCentreId = c.Int(nullable: false),
                        HasSuperManifest = c.Boolean(nullable: false),
                        SuperManifestStatus = c.Int(nullable: false),
                        SuperManifestCode = c.String(),
                        MovementStatus = c.Int(nullable: false),
                        CargoStatus = c.Int(nullable: false),
                        ExpressDelivery = c.Boolean(nullable: false),
                        IsBulky = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ManifestId)
                .ForeignKey("dbo.FleetTrip", t => t.FleetTripId)
                .ForeignKey("dbo.Shipment", t => t.ShipmentId)
                .Index(t => t.ManifestCode, unique: true)
                .Index(t => t.ShipmentId)
                .Index(t => t.FleetTripId);
            
            CreateTable(
                "dbo.Shipment",
                c => new
                    {
                        ShipmentId = c.Int(nullable: false, identity: true),
                        SealNumber = c.String(maxLength: 20),
                        Waybill = c.String(maxLength: 100),
                        AwaitingCollectionCount = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DeliveryTime = c.DateTime(),
                        PaymentStatus = c.Int(nullable: false),
                        CustomerType = c.String(maxLength: 100),
                        CustomerId = c.Int(nullable: false),
                        CompanyType = c.String(maxLength: 100),
                        CustomerCode = c.String(maxLength: 100),
                        DepartureServiceCentreId = c.Int(nullable: false),
                        DestinationServiceCentreId = c.Int(nullable: false),
                        ReceiverName = c.String(maxLength: 200),
                        ReceiverPhoneNumber = c.String(maxLength: 100),
                        ReceiverEmail = c.String(maxLength: 100),
                        ReceiverAddress = c.String(maxLength: 500),
                        ReceiverCity = c.String(maxLength: 50),
                        ReceiverState = c.String(maxLength: 50),
                        ReceiverCountry = c.String(maxLength: 50),
                        DeliveryOptionId = c.Int(nullable: false),
                        PickupOptions = c.Int(nullable: false),
                        ExpectedDateOfArrival = c.DateTime(),
                        ActualDateOfArrival = c.DateTime(),
                        ApproximateItemsWeight = c.Double(nullable: false),
                        GrandTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsCashOnDelivery = c.Boolean(nullable: false),
                        CashOnDeliveryAmount = c.Decimal(precision: 18, scale: 2),
                        ExpectedAmountToCollect = c.Decimal(precision: 18, scale: 2),
                        ActualAmountCollected = c.Decimal(precision: 18, scale: 2),
                        UserId = c.String(maxLength: 128),
                        IsdeclaredVal = c.Boolean(nullable: false),
                        DeclarationOfValueCheck = c.Decimal(precision: 18, scale: 2),
                        AppliedDiscount = c.Decimal(precision: 18, scale: 2),
                        DiscountValue = c.Decimal(precision: 18, scale: 2),
                        Insurance = c.Decimal(precision: 18, scale: 2),
                        Vat = c.Decimal(precision: 18, scale: 2),
                        Total = c.Decimal(precision: 18, scale: 2),
                        ShipmentPackagePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ShipmentPickupPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        vatvalue_display = c.Decimal(precision: 18, scale: 2),
                        InvoiceDiscountValue_display = c.Decimal(precision: 18, scale: 2),
                        offInvoiceDiscountvalue_display = c.Decimal(precision: 18, scale: 2),
                        PaymentMethod = c.String(maxLength: 20),
                        IsCancelled = c.Boolean(nullable: false),
                        IsInternational = c.Boolean(nullable: false),
                        Description = c.String(maxLength: 500),
                        DepositStatus = c.Int(nullable: false),
                        ReprintCounterStatus = c.Boolean(nullable: false),
                        SenderAddress = c.String(maxLength: 500),
                        SenderState = c.String(maxLength: 50),
                        IsCODPaidOut = c.Boolean(nullable: false),
                        ShipmentScanStatus = c.Int(nullable: false),
                        IsGrouped = c.Boolean(nullable: false),
                        DepartureCountryId = c.Int(nullable: false),
                        DestinationCountryId = c.Int(nullable: false),
                        CurrencyRatio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DeliveryNumber = c.String(maxLength: 20),
                        IsFromMobile = c.Boolean(nullable: false),
                        isInternalShipment = c.Boolean(nullable: false),
                        IsCargoed = c.Boolean(nullable: false),
                        InternationalShipmentType = c.Int(nullable: false),
                        IsClassShipment = c.Boolean(nullable: false),
                        FileNameUrl = c.String(maxLength: 500),
                        InternationalWayBill = c.String(maxLength: 100),
                        InternationalShippingCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Courier = c.String(maxLength: 50),
                        ExpressDelivery = c.Boolean(nullable: false),
                        IsExported = c.Boolean(nullable: false),
                        RequestNumber = c.String(maxLength: 128),
                        ExpressCharge = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsExpressDropoff = c.Boolean(nullable: false),
                        IsBulky = c.Boolean(nullable: false),
                        IsGIGGOExtension = c.Boolean(nullable: false),
                        CODStatus = c.Int(nullable: false),
                        CODStatusDate = c.DateTime(),
                        CODDescription = c.String(maxLength: 300),
                        blackBookSerialNumber = c.String(),
                        ReceiverPostalCode = c.String(maxLength: 128),
                        ReceiverStateOrProvinceCode = c.String(maxLength: 128),
                        ReceiverCompanyName = c.String(maxLength: 128),
                        ExtraCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        ShipmentReroute_WaybillNew = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.ShipmentId)
                .ForeignKey("dbo.DeliveryOption", t => t.DeliveryOptionId, cascadeDelete: true)
                .ForeignKey("dbo.ShipmentReroute", t => t.ShipmentReroute_WaybillNew)
                .Index(t => t.Waybill, unique: true)
                .Index(t => t.DeliveryOptionId)
                .Index(t => t.ShipmentReroute_WaybillNew);
            
            CreateTable(
                "dbo.ShipmentItem",
                c => new
                    {
                        ShipmentItemId = c.Int(nullable: false, identity: true),
                        Description = c.String(maxLength: 500),
                        Description_s = c.String(maxLength: 500),
                        ShipmentType = c.Int(nullable: false),
                        Weight = c.Double(nullable: false),
                        Nature = c.String(maxLength: 100),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        SerialNumber = c.Int(nullable: false),
                        ShipmentPackagePriceId = c.Int(nullable: false),
                        PackageQuantity = c.Int(nullable: false),
                        IsVolumetric = c.Boolean(nullable: false),
                        Length = c.Double(nullable: false),
                        Width = c.Double(nullable: false),
                        Height = c.Double(nullable: false),
                        ShipmentId = c.Int(nullable: false),
                        InternationalShipmentItemCategory = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ShipmentItemId)
                .ForeignKey("dbo.Shipment", t => t.ShipmentId, cascadeDelete: true)
                .Index(t => t.ShipmentId);
            
            CreateTable(
                "dbo.ShipmentReroute",
                c => new
                    {
                        WaybillNew = c.String(nullable: false, maxLength: 100),
                        WaybillOld = c.String(maxLength: 100),
                        RerouteBy = c.String(maxLength: 128),
                        RerouteReason = c.String(maxLength: 500),
                        ShipmentRerouteInitiator = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.WaybillNew)
                .Index(t => t.WaybillNew, unique: true)
                .Index(t => t.WaybillOld, unique: true);
            
            CreateTable(
                "dbo.ManifestGroupWaybillNumberMapping",
                c => new
                    {
                        ManifestGroupWaybillNumberMappingId = c.Int(nullable: false, identity: true),
                        DateMapped = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        ManifestCode = c.String(maxLength: 100),
                        GroupWaybillNumber = c.String(maxLength: 100),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ManifestGroupWaybillNumberMappingId);
            
            CreateTable(
                "dbo.ManifestVisitMonitoring",
                c => new
                    {
                        ManifestVisitMonitoringId = c.Int(nullable: false, identity: true),
                        Waybill = c.String(maxLength: 100),
                        Address = c.String(),
                        ReceiverName = c.String(),
                        ReceiverPhoneNumber = c.String(),
                        Status = c.String(),
                        Signature = c.String(),
                        UserId = c.String(maxLength: 128),
                        ServiceCentreId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ManifestVisitMonitoringId)
                .ForeignKey("dbo.ServiceCentre", t => t.ServiceCentreId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.ServiceCentreId);
            
            CreateTable(
                "dbo.ManifestWaybillMapping",
                c => new
                    {
                        ManifestWaybillMappingId = c.Int(nullable: false, identity: true),
                        IsActive = c.Boolean(nullable: false),
                        ManifestCode = c.String(maxLength: 100),
                        Waybill = c.String(maxLength: 100),
                        ServiceCentreId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ManifestWaybillMappingId)
                .ForeignKey("dbo.ServiceCentre", t => t.ServiceCentreId, cascadeDelete: true)
                .Index(t => t.ServiceCentreId);
            
            CreateTable(
                "dbo.Message",
                c => new
                    {
                        MessageId = c.Int(nullable: false, identity: true),
                        Body = c.String(),
                        Subject = c.String(),
                        From = c.String(),
                        To = c.String(maxLength: 100),
                        EmailSmsType = c.Int(nullable: false),
                        MessageType = c.Int(nullable: false),
                        SMSSenderPlatform = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.MessageId);
            
            CreateTable(
                "dbo.MissingShipment",
                c => new
                    {
                        MissingShipmentId = c.Int(nullable: false, identity: true),
                        Waybill = c.String(maxLength: 100),
                        SettlementAmount = c.Double(nullable: false),
                        Comment = c.String(),
                        Reason = c.String(),
                        Status = c.String(),
                        Feedback = c.String(),
                        CreatedBy = c.String(),
                        ResolvedBy = c.String(),
                        ServiceCentreId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.MissingShipmentId);
            
            CreateTable(
                "dbo.MobileGroupCodeWaybillMapping",
                c => new
                    {
                        MobileGroupCodeWaybillMappingId = c.Int(nullable: false, identity: true),
                        DateMapped = c.DateTime(nullable: false),
                        GroupCodeNumber = c.String(maxLength: 100),
                        WaybillNumber = c.String(maxLength: 100),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.MobileGroupCodeWaybillMappingId);
            
            CreateTable(
                "dbo.MobilePickUpRequests",
                c => new
                    {
                        MobilePickUpRequestsId = c.Int(nullable: false, identity: true),
                        Status = c.String(maxLength: 100),
                        Waybill = c.String(maxLength: 100),
                        UserId = c.String(maxLength: 128),
                        Reason = c.String(),
                        PartnerCode = c.String(maxLength: 100),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.MobilePickUpRequestsId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.MobileRating",
                c => new
                    {
                        MobileRatingId = c.Int(nullable: false, identity: true),
                        Waybill = c.String(maxLength: 100),
                        CommentByCustomer = c.String(),
                        CommentByPartner = c.String(),
                        PartnerCode = c.String(maxLength: 100),
                        CustomerCode = c.String(maxLength: 100),
                        CustomerRating = c.Double(),
                        PartnerRating = c.Double(),
                        IsRatedByCustomer = c.Boolean(nullable: false),
                        IsRatedByPartner = c.Boolean(nullable: false),
                        DateCustomerRated = c.DateTime(),
                        DatePartnerRated = c.DateTime(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.MobileRatingId);
            
            CreateTable(
                "dbo.MobileScanStatus",
                c => new
                    {
                        MobileScanStatusId = c.Int(nullable: false, identity: true),
                        Code = c.String(maxLength: 10),
                        Incident = c.String(),
                        Reason = c.String(),
                        Comment = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.MobileScanStatusId)
                .Index(t => t.Code, unique: true);
            
            CreateTable(
                "dbo.MobileShipmentTracking",
                c => new
                    {
                        MobileShipmentTrackingId = c.Int(nullable: false, identity: true),
                        Waybill = c.String(maxLength: 100),
                        Location = c.String(),
                        Status = c.String(maxLength: 100),
                        DateTime = c.DateTime(nullable: false),
                        TrackingType = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        ServiceCentreId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.MobileShipmentTrackingId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.MovementDispatch",
                c => new
                    {
                        DispatchId = c.Int(nullable: false, identity: true),
                        RegistrationNumber = c.String(),
                        MovementManifestNumber = c.String(maxLength: 50),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RescuedDispatchId = c.Int(nullable: false),
                        DriverDetail = c.String(),
                        DispatchedBy = c.String(),
                        ReceivedBy = c.String(),
                        DispatchCategory = c.Int(nullable: false),
                        ServiceCentreId = c.Int(),
                        DepartureId = c.Int(),
                        DestinationId = c.Int(),
                        DepartureServiceCenterId = c.Int(nullable: false),
                        DestinationServiceCenterId = c.Int(nullable: false),
                        IsSuperManifest = c.Boolean(nullable: false),
                        BonusAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.DispatchId)
                .ForeignKey("dbo.Station", t => t.DepartureId)
                .ForeignKey("dbo.Station", t => t.DestinationId)
                .ForeignKey("dbo.ServiceCentre", t => t.ServiceCentreId)
                .Index(t => t.MovementManifestNumber, unique: true)
                .Index(t => t.ServiceCentreId)
                .Index(t => t.DepartureId)
                .Index(t => t.DestinationId);
            
            CreateTable(
                "dbo.MovementManifestNumber",
                c => new
                    {
                        MovementManifestNumberId = c.Int(nullable: false, identity: true),
                        MovementManifestCode = c.String(maxLength: 100),
                        DepartureServiceCentreId = c.Int(nullable: false),
                        DestinationServiceCentreId = c.Int(nullable: false),
                        DriverCode = c.String(),
                        DestinationServiceCentreCode = c.String(),
                        MovementStatus = c.Int(nullable: false),
                        IsDriverValid = c.Boolean(nullable: false),
                        IsDestinationServiceCentreValid = c.Boolean(nullable: false),
                        UserId = c.String(maxLength: 128),
                        DriverUserId = c.String(),
                        DestinationServiceCentreUserId = c.String(),
                        IsAutomated = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        DepartureServiceCentre_ServiceCentreId = c.Int(),
                        DestinationServiceCentre_ServiceCentreId = c.Int(),
                    })
                .PrimaryKey(t => t.MovementManifestNumberId)
                .ForeignKey("dbo.ServiceCentre", t => t.DepartureServiceCentre_ServiceCentreId)
                .ForeignKey("dbo.ServiceCentre", t => t.DestinationServiceCentre_ServiceCentreId)
                .Index(t => t.MovementManifestCode, unique: true)
                .Index(t => t.DepartureServiceCentre_ServiceCentreId)
                .Index(t => t.DestinationServiceCentre_ServiceCentreId);
            
            CreateTable(
                "dbo.MovementManifestNumberMapping",
                c => new
                    {
                        MovementManifestNumberMappingId = c.Int(nullable: false, identity: true),
                        MovementManifestCode = c.String(maxLength: 100),
                        ManifestNumber = c.String(maxLength: 100),
                        UserId = c.String(maxLength: 128),
                        IsAutomated = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.MovementManifestNumberMappingId);
            
            CreateTable(
                "dbo.Notification",
                c => new
                    {
                        NotificationId = c.Int(nullable: false, identity: true),
                        Subject = c.String(maxLength: 250),
                        Message = c.String(maxLength: 500),
                        UserId = c.String(maxLength: 500),
                        IsRead = c.Boolean(nullable: false),
                        MesageActions = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.NotificationId);
            
            CreateTable(
                "dbo.NumberGeneratorMonitor",
                c => new
                    {
                        NumberGeneratorMonitorId = c.Int(nullable: false, identity: true),
                        ServiceCentreCode = c.String(maxLength: 100),
                        NumberGeneratorType = c.Int(nullable: false),
                        Number = c.String(maxLength: 100),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.NumberGeneratorMonitorId);
            
            CreateTable(
                "dbo.OTP",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.String(maxLength: 100),
                        Otp = c.Int(nullable: false),
                        IsValid = c.Boolean(nullable: false),
                        PhoneNumber = c.String(maxLength: 100),
                        EmailAddress = c.String(maxLength: 100),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OverdueShipment",
                c => new
                    {
                        Waybill = c.String(nullable: false, maxLength: 100),
                        OverdueShipmentStatus = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Waybill)
                .Index(t => t.Waybill, unique: true);
            
            CreateTable(
                "dbo.PackingList",
                c => new
                    {
                        PackingListId = c.Int(nullable: false, identity: true),
                        Waybill = c.String(maxLength: 100),
                        Items = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.PackingListId);
            
            CreateTable(
                "dbo.PartnerApplication",
                c => new
                    {
                        PartnerApplicationId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(maxLength: 500),
                        LastName = c.String(maxLength: 500),
                        CompanyName = c.String(maxLength: 500),
                        Email = c.String(),
                        PhoneNumber = c.String(maxLength: 100),
                        Address = c.String(),
                        CompanyRcNumber = c.String(),
                        IdentificationNumber = c.String(),
                        PartnerType = c.Int(nullable: false),
                        TellAboutYou = c.String(),
                        IsRegistered = c.Boolean(nullable: false),
                        PartnerApplicationStatus = c.Int(nullable: false),
                        IdentificationTypeId = c.Int(),
                        ApproverId = c.String(maxLength: 128),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.PartnerApplicationId);
            
            CreateTable(
                "dbo.PartnerPayout",
                c => new
                    {
                        PartnerPayoutId = c.Int(nullable: false, identity: true),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProcessedBy = c.String(maxLength: 100),
                        DateProcessed = c.DateTime(nullable: false),
                        PartnerName = c.String(maxLength: 100),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        PartnerType = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.PartnerPayoutId);
            
            CreateTable(
                "dbo.PartnerTransactions",
                c => new
                    {
                        PartnerTransactionsID = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        Destination = c.String(),
                        Departure = c.String(),
                        AmountReceived = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Waybill = c.String(maxLength: 100),
                        IsProcessed = c.Boolean(nullable: false),
                        Manifest = c.String(maxLength: 100),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.PartnerTransactionsID);
            
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
                "dbo.PaymentPartialTransaction",
                c => new
                    {
                        PaymentPartialTransactionId = c.Int(nullable: false, identity: true),
                        Waybill = c.String(maxLength: 100),
                        TransactionCode = c.String(maxLength: 100),
                        PaymentStatus = c.Int(nullable: false),
                        PaymentType = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UserId = c.String(maxLength: 128),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.PaymentPartialTransactionId);
            
            CreateTable(
                "dbo.PaymentTransaction",
                c => new
                    {
                        PaymentTransactionId = c.Int(nullable: false, identity: true),
                        Waybill = c.String(maxLength: 100),
                        TransactionCode = c.String(maxLength: 100),
                        PaymentStatus = c.Int(nullable: false),
                        PaymentTypes = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.PaymentTransactionId);
            
            CreateTable(
                "dbo.PickupManifest",
                c => new
                    {
                        PickupManifestId = c.Int(nullable: false, identity: true),
                        ManifestCode = c.String(maxLength: 100),
                        DateTime = c.DateTime(nullable: false),
                        ShipmentId = c.Int(),
                        DispatchedById = c.String(maxLength: 128),
                        ReceiverById = c.String(maxLength: 128),
                        FleetTripId = c.Int(),
                        IsDispatched = c.Boolean(nullable: false),
                        IsReceived = c.Boolean(nullable: false),
                        ServiceCentreId = c.Int(nullable: false),
                        ManifestType = c.Int(nullable: false),
                        ManifestStatus = c.Int(nullable: false),
                        DepartureServiceCentreId = c.Int(nullable: false),
                        DestinationServiceCentreId = c.Int(nullable: false),
                        Picked = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        Shipment_PreShipmentMobileId = c.Int(),
                    })
                .PrimaryKey(t => t.PickupManifestId)
                .ForeignKey("dbo.FleetTrip", t => t.FleetTripId)
                .ForeignKey("dbo.PreShipmentMobile", t => t.Shipment_PreShipmentMobileId)
                .Index(t => t.ManifestCode, unique: true)
                .Index(t => t.FleetTripId)
                .Index(t => t.Shipment_PreShipmentMobileId);
            
            CreateTable(
                "dbo.PreShipmentMobile",
                c => new
                    {
                        PreShipmentMobileId = c.Int(nullable: false, identity: true),
                        Waybill = c.String(maxLength: 100),
                        SenderName = c.String(maxLength: 500),
                        SenderPhoneNumber = c.String(maxLength: 100),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DeliveryTime = c.DateTime(),
                        CustomerType = c.String(maxLength: 100),
                        CompanyType = c.String(maxLength: 100),
                        CustomerCode = c.String(maxLength: 100),
                        ReceiverName = c.String(maxLength: 500),
                        ReceiverPhoneNumber = c.String(maxLength: 100),
                        ReceiverEmail = c.String(maxLength: 500),
                        ReceiverAddress = c.String(maxLength: 500),
                        ReceiverCity = c.String(maxLength: 500),
                        ReceiverState = c.String(maxLength: 500),
                        ReceiverCountry = c.String(maxLength: 100),
                        InputtedReceiverAddress = c.String(maxLength: 500),
                        SenderLocality = c.String(maxLength: 500),
                        SenderAddress = c.String(maxLength: 500),
                        InputtedSenderAddress = c.String(maxLength: 500),
                        SenderStationId = c.Int(nullable: false),
                        ReceiverStationId = c.Int(nullable: false),
                        IsHomeDelivery = c.Boolean(nullable: false),
                        ExpectedDateOfArrival = c.DateTime(),
                        ActualDateOfArrival = c.DateTime(),
                        GrandTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsCashOnDelivery = c.Boolean(nullable: false),
                        CashOnDeliveryAmount = c.Decimal(precision: 18, scale: 2),
                        ExpectedAmountToCollect = c.Decimal(precision: 18, scale: 2),
                        ActualAmountCollected = c.Decimal(precision: 18, scale: 2),
                        UserId = c.String(maxLength: 128),
                        IsdeclaredVal = c.Boolean(nullable: false),
                        Total = c.Decimal(precision: 18, scale: 2),
                        DiscountValue = c.Decimal(precision: 18, scale: 2),
                        Vat = c.Decimal(precision: 18, scale: 2),
                        InsuranceValue = c.Decimal(precision: 18, scale: 2),
                        DeliveryPrice = c.Decimal(precision: 18, scale: 2),
                        ShipmentPackagePrice = c.Decimal(precision: 18, scale: 2),
                        vatvalue_display = c.Decimal(precision: 18, scale: 2),
                        InvoiceDiscountValue_display = c.Decimal(precision: 18, scale: 2),
                        offInvoiceDiscountvalue_display = c.Decimal(precision: 18, scale: 2),
                        IsCancelled = c.Boolean(nullable: false),
                        IsConfirmed = c.Boolean(nullable: false),
                        IsDelivered = c.Boolean(nullable: false),
                        DeclinedReason = c.String(maxLength: 500),
                        CalculatedTotal = c.Double(),
                        shipmentstatus = c.String(maxLength: 500),
                        VehicleType = c.String(maxLength: 500),
                        ZoneMapping = c.Int(),
                        ActualReceiverFirstName = c.String(maxLength: 500),
                        ActualReceiverLastName = c.String(maxLength: 500),
                        ActualReceiverPhoneNumber = c.String(maxLength: 500),
                        CountryId = c.Int(nullable: false),
                        ServiceCentreAddress = c.String(maxLength: 500),
                        IsApproved = c.Boolean(),
                        ShipmentPickupPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DeliveryNumber = c.String(maxLength: 20),
                        TimeAssigned = c.DateTime(),
                        TimePickedUp = c.DateTime(),
                        TimeDelivered = c.DateTime(),
                        IndentificationUrl = c.String(maxLength: 500),
                        DeliveryAddressImageUrl = c.String(maxLength: 500),
                        IsScheduled = c.Boolean(nullable: false),
                        ScheduledDate = c.DateTime(),
                        DestinationServiceCenterId = c.Int(nullable: false),
                        IsBatchPickUp = c.Boolean(nullable: false),
                        WaybillImageUrl = c.String(maxLength: 500),
                        IsFromAgility = c.Boolean(nullable: false),
                        Haulageid = c.Int(nullable: false),
                        ReceiverCompanyName = c.String(maxLength: 200),
                        ReceiverPostalCode = c.String(maxLength: 50),
                        ReceiverStateOrProvinceCode = c.String(maxLength: 5),
                        ReceiverCountryCode = c.String(maxLength: 200),
                        InternationalShippingCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ManufacturerCountry = c.String(maxLength: 5),
                        ItemDetails = c.String(maxLength: 170),
                        CompanyMap = c.Int(nullable: false),
                        IsInternationalShipment = c.Boolean(nullable: false),
                        DeclarationOfValueCheck = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DepartureCountryId = c.Int(nullable: false),
                        DestinationCountryId = c.Int(nullable: false),
                        CustomerCancelReason = c.String(maxLength: 300),
                        IsCoupon = c.Boolean(nullable: false),
                        CouponCode = c.String(maxLength: 50),
                        IsAlpha = c.Boolean(nullable: false),
                        CODStatus = c.Int(nullable: false),
                        CODStatusDate = c.DateTime(),
                        CODDescription = c.String(maxLength: 300),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        ReceiverLocation_LocationId = c.Int(),
                        SenderLocation_LocationId = c.Int(),
                        serviceCentreLocation_LocationId = c.Int(),
                    })
                .PrimaryKey(t => t.PreShipmentMobileId)
                .ForeignKey("dbo.Location", t => t.ReceiverLocation_LocationId)
                .ForeignKey("dbo.Location", t => t.SenderLocation_LocationId)
                .ForeignKey("dbo.Location", t => t.serviceCentreLocation_LocationId)
                .Index(t => t.Waybill, unique: true)
                .Index(t => t.ReceiverLocation_LocationId)
                .Index(t => t.SenderLocation_LocationId)
                .Index(t => t.serviceCentreLocation_LocationId);
            
            CreateTable(
                "dbo.PreShipmentItemMobile",
                c => new
                    {
                        PreShipmentItemMobileId = c.Int(nullable: false, identity: true),
                        Description = c.String(maxLength: 500),
                        Weight = c.Double(nullable: false),
                        ItemType = c.String(maxLength: 100),
                        ItemCode = c.String(maxLength: 100),
                        ItemName = c.String(maxLength: 100),
                        EstimatedPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Value = c.String(maxLength: 100),
                        ImageUrl = c.String(maxLength: 500),
                        Quantity = c.Int(nullable: false),
                        SerialNumber = c.Int(nullable: false),
                        IsVolumetric = c.Boolean(nullable: false),
                        Length = c.Double(),
                        Width = c.Double(),
                        Height = c.Double(),
                        PreShipmentMobileId = c.Int(nullable: false),
                        CalculatedPrice = c.Decimal(precision: 18, scale: 2),
                        SpecialPackageId = c.Int(),
                        ShipmentType = c.Int(nullable: false),
                        IsCancelled = c.Boolean(nullable: false),
                        PictureName = c.String(maxLength: 100),
                        PictureDate = c.DateTime(),
                        WeightRange = c.String(maxLength: 100),
                        InternationalShipmentItemCategory = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.PreShipmentItemMobileId)
                .ForeignKey("dbo.PreShipmentMobile", t => t.PreShipmentMobileId, cascadeDelete: true)
                .Index(t => t.PreShipmentMobileId);
            
            CreateTable(
                "dbo.PickupManifestWaybillMapping",
                c => new
                    {
                        PickupManifestWaybillMappingId = c.Int(nullable: false, identity: true),
                        IsActive = c.Boolean(nullable: false),
                        ManifestCode = c.String(maxLength: 100),
                        Waybill = c.String(maxLength: 100),
                        ServiceCentreId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.PickupManifestWaybillMappingId)
                .ForeignKey("dbo.ServiceCentre", t => t.ServiceCentreId, cascadeDelete: true)
                .Index(t => t.ServiceCentreId);
            
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
                "dbo.PreShipment",
                c => new
                    {
                        PreShipmentId = c.Int(nullable: false, identity: true),
                        TempCode = c.String(maxLength: 100),
                        Waybill = c.String(maxLength: 100),
                        CompanyType = c.String(maxLength: 100),
                        CustomerCode = c.String(maxLength: 100),
                        SenderUserId = c.String(maxLength: 128),
                        SenderCity = c.String(maxLength: 100),
                        SenderName = c.String(maxLength: 100),
                        SenderPhoneNumber = c.String(maxLength: 100),
                        ReceiverName = c.String(maxLength: 100),
                        ReceiverPhoneNumber = c.String(maxLength: 100),
                        ReceiverAddress = c.String(maxLength: 500),
                        ReceiverCity = c.String(maxLength: 100),
                        LGA = c.String(maxLength: 100),
                        PickupOptions = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DepartureStationId = c.Int(nullable: false),
                        DestinationStationId = c.Int(nullable: false),
                        DestinationServiceCenterId = c.Int(nullable: false),
                        ApproximateItemsWeight = c.Double(nullable: false),
                        GrandTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsProcessed = c.Boolean(nullable: false),
                        IsAgent = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        DeliveryType = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.PreShipmentId)
                .Index(t => t.TempCode, unique: true);
            
            CreateTable(
                "dbo.PreShipmentItem",
                c => new
                    {
                        PreShipmentItemId = c.Int(nullable: false, identity: true),
                        Description = c.String(maxLength: 500),
                        Description_s = c.String(maxLength: 500),
                        ShipmentType = c.Int(nullable: false),
                        Weight = c.Double(nullable: false),
                        Nature = c.String(maxLength: 100),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        SerialNumber = c.Int(nullable: false),
                        IsVolumetric = c.Boolean(nullable: false),
                        Length = c.Double(nullable: false),
                        Width = c.Double(nullable: false),
                        Height = c.Double(nullable: false),
                        SpecialPackageId = c.Int(),
                        ItemValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PreShipmentId = c.Int(nullable: false),
                        CalculatedPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.PreShipmentItemId)
                .ForeignKey("dbo.PreShipment", t => t.PreShipmentId, cascadeDelete: true)
                .Index(t => t.PreShipmentId);
            
            CreateTable(
                "dbo.PreShipmentManifestMapping",
                c => new
                    {
                        PreShipmentManifestMappingId = c.Int(nullable: false, identity: true),
                        ManifestCode = c.String(maxLength: 100),
                        PreShipmentId = c.Int(nullable: false),
                        Waybill = c.String(maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                        RegistrationNumber = c.String(maxLength: 100),
                        DriverDetail = c.String(maxLength: 100),
                        DispatchedBy = c.String(maxLength: 100),
                        ReceivedBy = c.String(maxLength: 100),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.PreShipmentManifestMappingId)
                .ForeignKey("dbo.PreShipment", t => t.PreShipmentId, cascadeDelete: true)
                .Index(t => t.PreShipmentId);
            
            CreateTable(
                "dbo.PriceCategory",
                c => new
                    {
                        PriceCategoryId = c.Int(nullable: false, identity: true),
                        CountryId = c.Int(nullable: false),
                        PriceCategoryName = c.String(),
                        CategoryMinimumWeight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PricePerWeight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CategoryMinimumPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsActive = c.Boolean(nullable: false),
                        DepartureCountryId = c.Int(nullable: false),
                        SubminimumWeight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SubminimumPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsHazardous = c.Boolean(nullable: false),
                        DeliveryType = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.PriceCategoryId);
            
            CreateTable(
                "dbo.RankHistory",
                c => new
                    {
                        RankHistoryId = c.Int(nullable: false, identity: true),
                        CustomerName = c.String(maxLength: 100),
                        CustomerCode = c.String(maxLength: 100),
                        RankType = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.RankHistoryId);
            
            CreateTable(
                "dbo.ReferrerCode",
                c => new
                    {
                        ReferrerCodeId = c.Int(nullable: false, identity: true),
                        Referrercode = c.String(maxLength: 50),
                        UserId = c.String(maxLength: 128),
                        UserCode = c.String(maxLength: 100),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ReferrerCodeId);
            
            CreateTable(
                "dbo.Region",
                c => new
                    {
                        RegionId = c.Int(nullable: false, identity: true),
                        RegionName = c.String(maxLength: 100),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.RegionId)
                .Index(t => t.RegionName, unique: true);
            
            CreateTable(
                "dbo.RegionServiceCentreMapping",
                c => new
                    {
                        RegionServiceCentreMappingId = c.Int(nullable: false, identity: true),
                        RegionId = c.Int(nullable: false),
                        ServiceCentreId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.RegionServiceCentreMappingId);
            
            CreateTable(
                "dbo.RiderDelivery",
                c => new
                    {
                        RiderDeliveryId = c.Int(nullable: false, identity: true),
                        Waybill = c.String(maxLength: 100),
                        DriverId = c.String(maxLength: 128),
                        CostOfDelivery = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Address = c.String(maxLength: 500),
                        DeliveryDate = c.DateTime(nullable: false),
                        Area = c.String(maxLength: 100),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.RiderDeliveryId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.Route",
                c => new
                    {
                        RouteId = c.Int(nullable: false, identity: true),
                        RouteName = c.String(),
                        DepartureCentreId = c.Int(nullable: false),
                        DestinationCentreId = c.Int(nullable: false),
                        IsSubRoute = c.Boolean(nullable: false),
                        DispatchFee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LoaderFee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CaptainFee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MainRouteId = c.Int(),
                        RouteType = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        DepartureCenter_ServiceCentreId = c.Int(),
                        DestinationCenter_ServiceCentreId = c.Int(),
                    })
                .PrimaryKey(t => t.RouteId)
                .ForeignKey("dbo.ServiceCentre", t => t.DepartureCenter_ServiceCentreId)
                .ForeignKey("dbo.ServiceCentre", t => t.DestinationCenter_ServiceCentreId)
                .Index(t => t.DepartureCenter_ServiceCentreId)
                .Index(t => t.DestinationCenter_ServiceCentreId);
            
            CreateTable(
                "dbo.ScanStatus",
                c => new
                    {
                        ScanStatusId = c.Int(nullable: false, identity: true),
                        Code = c.String(maxLength: 10),
                        Incident = c.String(),
                        Reason = c.String(),
                        Comment = c.String(),
                        HiddenFlag = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ScanStatusId)
                .Index(t => t.Code, unique: true);
            
            CreateTable(
                "dbo.ServiceCenterPackage",
                c => new
                    {
                        ServiceCenterPackageId = c.Int(nullable: false, identity: true),
                        ShipmentPackageId = c.Int(nullable: false),
                        ServiceCenterId = c.Int(nullable: false),
                        InventoryOnHand = c.Int(nullable: false),
                        MinimunRequired = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ServiceCenterPackageId);
            
            CreateTable(
                "dbo.Shipment_Archive",
                c => new
                    {
                        ShipmentId = c.Int(nullable: false, identity: true),
                        SealNumber = c.String(maxLength: 20),
                        Waybill = c.String(maxLength: 100),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DeliveryTime = c.DateTime(),
                        PaymentStatus = c.Int(nullable: false),
                        CustomerType = c.String(maxLength: 100),
                        CustomerId = c.Int(nullable: false),
                        CompanyType = c.String(maxLength: 100),
                        CustomerCode = c.String(maxLength: 100),
                        DepartureServiceCentreId = c.Int(nullable: false),
                        DestinationServiceCentreId = c.Int(nullable: false),
                        ReceiverName = c.String(maxLength: 200),
                        ReceiverPhoneNumber = c.String(maxLength: 100),
                        ReceiverEmail = c.String(maxLength: 100),
                        ReceiverAddress = c.String(maxLength: 500),
                        ReceiverCity = c.String(maxLength: 50),
                        ReceiverState = c.String(maxLength: 50),
                        ReceiverCountry = c.String(maxLength: 50),
                        DeliveryOptionId = c.Int(nullable: false),
                        PickupOptions = c.Int(nullable: false),
                        ExpectedDateOfArrival = c.DateTime(),
                        ActualDateOfArrival = c.DateTime(),
                        ApproximateItemsWeight = c.Double(nullable: false),
                        GrandTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsCashOnDelivery = c.Boolean(nullable: false),
                        CashOnDeliveryAmount = c.Decimal(precision: 18, scale: 2),
                        ExpectedAmountToCollect = c.Decimal(precision: 18, scale: 2),
                        ActualAmountCollected = c.Decimal(precision: 18, scale: 2),
                        UserId = c.String(maxLength: 128),
                        IsdeclaredVal = c.Boolean(nullable: false),
                        DeclarationOfValueCheck = c.Decimal(precision: 18, scale: 2),
                        AppliedDiscount = c.Decimal(precision: 18, scale: 2),
                        DiscountValue = c.Decimal(precision: 18, scale: 2),
                        Insurance = c.Decimal(precision: 18, scale: 2),
                        Vat = c.Decimal(precision: 18, scale: 2),
                        Total = c.Decimal(precision: 18, scale: 2),
                        ShipmentPackagePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ShipmentPickupPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        vatvalue_display = c.Decimal(precision: 18, scale: 2),
                        InvoiceDiscountValue_display = c.Decimal(precision: 18, scale: 2),
                        offInvoiceDiscountvalue_display = c.Decimal(precision: 18, scale: 2),
                        PaymentMethod = c.String(maxLength: 20),
                        IsCancelled = c.Boolean(nullable: false),
                        IsInternational = c.Boolean(nullable: false),
                        Description = c.String(maxLength: 500),
                        DepositStatus = c.Int(nullable: false),
                        ReprintCounterStatus = c.Boolean(nullable: false),
                        SenderAddress = c.String(maxLength: 500),
                        SenderState = c.String(maxLength: 50),
                        ShipmentReroute_WaybillNew = c.String(maxLength: 100),
                        IsCODPaidOut = c.Boolean(nullable: false),
                        ShipmentScanStatus = c.Int(nullable: false),
                        IsGrouped = c.Boolean(nullable: false),
                        DepartureCountryId = c.Int(nullable: false),
                        DestinationCountryId = c.Int(nullable: false),
                        CurrencyRatio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DeliveryNumber = c.String(maxLength: 20),
                        IsFromMobile = c.Boolean(nullable: false),
                        isInternalShipment = c.Boolean(nullable: false),
                        IsCargoed = c.Boolean(nullable: false),
                        InternationalShipmentType = c.Int(nullable: false),
                        IsClassShipment = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.ShipmentId);
            
            CreateTable(
                "dbo.ShipmentCancel",
                c => new
                    {
                        Waybill = c.String(nullable: false, maxLength: 100),
                        CreatedBy = c.String(maxLength: 128),
                        ShipmentCreatedDate = c.DateTime(nullable: false),
                        CancelledBy = c.String(maxLength: 128),
                        CancelReason = c.String(maxLength: 500),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Waybill)
                .Index(t => t.Waybill, unique: true);
            
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
                "dbo.ShipmentCollection",
                c => new
                    {
                        Waybill = c.String(nullable: false, maxLength: 100),
                        Name = c.String(maxLength: 200),
                        PhoneNumber = c.String(maxLength: 100),
                        Email = c.String(maxLength: 100),
                        Address = c.String(maxLength: 500),
                        City = c.String(maxLength: 50),
                        State = c.String(maxLength: 50),
                        IndentificationUrl = c.String(maxLength: 500),
                        ShipmentScanStatus = c.Int(nullable: false),
                        DepartureServiceCentreId = c.Int(nullable: false),
                        DestinationServiceCentreId = c.Int(nullable: false),
                        IsCashOnDelivery = c.Boolean(nullable: false),
                        UserId = c.String(maxLength: 128),
                        DeliveryAddressImageUrl = c.String(maxLength: 500),
                        ActualDeliveryAddress = c.String(maxLength: 500),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Waybill)
                .Index(t => t.Waybill, unique: true);
            
            CreateTable(
                "dbo.ShipmentContact",
                c => new
                    {
                        ShipmentContactId = c.Int(nullable: false, identity: true),
                        Waybill = c.String(maxLength: 100),
                        Status = c.Int(nullable: false),
                        ContactedBy = c.String(maxLength: 128),
                        NoOfContact = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ShipmentContactId)
                .Index(t => t.Waybill, unique: true);
            
            CreateTable(
                "dbo.ShipmentContactHistory",
                c => new
                    {
                        ShipmentContactHistoryId = c.Int(nullable: false, identity: true),
                        Waybill = c.String(maxLength: 100),
                        ContactedBy = c.String(maxLength: 128),
                        NoOfContact = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ShipmentContactHistoryId)
                .Index(t => t.Waybill);
            
            CreateTable(
                "dbo.ShipmentDeliveryOptionMapping",
                c => new
                    {
                        ShipmentDeliveryOptionMappingId = c.Int(nullable: false, identity: true),
                        Waybill = c.String(maxLength: 100),
                        DeliveryOptionId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ShipmentDeliveryOptionMappingId)
                .ForeignKey("dbo.DeliveryOption", t => t.DeliveryOptionId, cascadeDelete: true)
                .Index(t => t.Waybill)
                .Index(t => t.DeliveryOptionId);
            
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
                "dbo.ShipmentHash",
                c => new
                    {
                        ShipmentHashId = c.Int(nullable: false, identity: true),
                        HashedShipment = c.String(maxLength: 500),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ShipmentHashId);
            
            CreateTable(
                "dbo.ShipmentItem_Archive",
                c => new
                    {
                        ShipmentItemId = c.Int(nullable: false, identity: true),
                        Description = c.String(maxLength: 500),
                        Description_s = c.String(maxLength: 500),
                        ShipmentType = c.Int(nullable: false),
                        Weight = c.Double(nullable: false),
                        Nature = c.String(maxLength: 100),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        SerialNumber = c.Int(nullable: false),
                        ShipmentPackagePriceId = c.Int(nullable: false),
                        PackageQuantity = c.Int(nullable: false),
                        IsVolumetric = c.Boolean(nullable: false),
                        Length = c.Double(nullable: false),
                        Width = c.Double(nullable: false),
                        Height = c.Double(nullable: false),
                        ShipmentId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.ShipmentItemId);
            
            CreateTable(
                "dbo.ShipmentPackagePrice",
                c => new
                    {
                        ShipmentPackagePriceId = c.Int(nullable: false, identity: true),
                        Description = c.String(maxLength: 100),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CountryId = c.Int(nullable: false),
                        InventoryOnHand = c.Int(nullable: false),
                        MinimunRequired = c.Int(nullable: false),
                        Length = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Height = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Width = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Weight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ShipmentPackagePriceId);
            
            CreateTable(
                "dbo.ShipmentPackagingTransactions",
                c => new
                    {
                        ShipmentPackageTransactionsId = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        ShipmentPackageId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        ServiceCenterId = c.Int(nullable: false),
                        Waybill = c.String(),
                        PackageTransactionType = c.Int(nullable: false),
                        ReceiverServiceCenterId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ShipmentPackageTransactionsId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ShipmentReturn",
                c => new
                    {
                        WaybillNew = c.String(nullable: false, maxLength: 100),
                        WaybillOld = c.String(maxLength: 100),
                        Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OriginalPayment = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ServiceCentreId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.WaybillNew)
                .Index(t => t.WaybillNew, unique: true)
                .Index(t => t.WaybillOld, unique: true);
            
            CreateTable(
                "dbo.ShipmentTimeMonitor",
                c => new
                    {
                        ShipmentTimeMonitorId = c.Int(nullable: false, identity: true),
                        Waybill = c.String(maxLength: 100),
                        UserName = c.String(maxLength: 128),
                        TimeInSeconds = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        UserServiceCentreId = c.Int(nullable: false),
                        UserServiceCentreName = c.String(maxLength: 128),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ShipmentTimeMonitorId)
                .Index(t => t.Waybill, unique: true);
            
            CreateTable(
                "dbo.ShipmentTracking",
                c => new
                    {
                        ShipmentTrackingId = c.Int(nullable: false, identity: true),
                        Waybill = c.String(maxLength: 100),
                        Location = c.String(),
                        Status = c.String(maxLength: 100),
                        DateTime = c.DateTime(nullable: false),
                        TrackingType = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        ServiceCentreId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ShipmentTrackingId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.Waybill)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.SLA",
                c => new
                    {
                        SLAId = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        SLAType = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.SLAId);
            
            CreateTable(
                "dbo.SLASignedUser",
                c => new
                    {
                        SLASignedUserId = c.Int(nullable: false, identity: true),
                        SLAId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.SLASignedUserId)
                .ForeignKey("dbo.SLA", t => t.SLAId, cascadeDelete: true)
                .Index(t => t.SLAId);
            
            CreateTable(
                "dbo.SmsSendLog",
                c => new
                    {
                        SmsSendLogId = c.Int(nullable: false, identity: true),
                        To = c.String(maxLength: 100),
                        From = c.String(),
                        Message = c.String(),
                        Status = c.Int(nullable: false),
                        User = c.String(maxLength: 128),
                        ResultStatus = c.String(),
                        ResultDescription = c.String(),
                        Waybill = c.String(maxLength: 100),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.SmsSendLogId);
            
            CreateTable(
                "dbo.SpecialDomesticPackage",
                c => new
                    {
                        SpecialDomesticPackageId = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 500),
                        Status = c.Boolean(nullable: false),
                        Weight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SpecialDomesticPackageType = c.Int(nullable: false),
                        WeightRange = c.String(maxLength: 100),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        SubCategory_SubCategoryId = c.Int(),
                    })
                .PrimaryKey(t => t.SpecialDomesticPackageId)
                .ForeignKey("dbo.SubCategory", t => t.SubCategory_SubCategoryId)
                .Index(t => t.SubCategory_SubCategoryId);
            
            CreateTable(
                "dbo.SubCategory",
                c => new
                    {
                        SubCategoryId = c.Int(nullable: false, identity: true),
                        CategoryId = c.Int(nullable: false),
                        SubCategoryName = c.String(maxLength: 500),
                        Weight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        WeightRange = c.String(maxLength: 100),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.SubCategoryId)
                .ForeignKey("dbo.Category", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.SpecialDomesticZonePrice",
                c => new
                    {
                        SpecialDomesticZonePriceId = c.Int(nullable: false, identity: true),
                        Weight = c.Decimal(precision: 18, scale: 2),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Description = c.String(),
                        ZoneId = c.Int(nullable: false),
                        SpecialDomesticPackageId = c.Int(nullable: false),
                        CountryId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.SpecialDomesticZonePriceId)
                .ForeignKey("dbo.SpecialDomesticPackage", t => t.SpecialDomesticPackageId, cascadeDelete: true)
                .ForeignKey("dbo.Zone", t => t.ZoneId, cascadeDelete: true)
                .Index(t => t.ZoneId)
                .Index(t => t.SpecialDomesticPackageId);
            
            CreateTable(
                "dbo.StockRequest",
                c => new
                    {
                        StockRequestId = c.Int(nullable: false, identity: true),
                        IsSupplied = c.Boolean(nullable: false),
                        SourceId = c.Int(nullable: false),
                        StockRequestSourceType = c.Int(nullable: false),
                        StockRequestStatus = c.Int(nullable: false),
                        Remark = c.String(),
                        VendorAddress = c.String(),
                        StockRequestDestinationType = c.Int(nullable: false),
                        DateIssued = c.DateTime(nullable: false),
                        DateReceived = c.DateTime(nullable: false),
                        ConveyingFleetId = c.Int(nullable: false),
                        Receiver = c.String(),
                        Requester = c.String(),
                        Issuer = c.String(),
                        StockInApprover = c.String(),
                        StockOutApprover = c.String(),
                        DestinationId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        ConveyingFleet_FleetId = c.Int(),
                        Destination_ServiceCentreId = c.Int(),
                    })
                .PrimaryKey(t => t.StockRequestId)
                .ForeignKey("dbo.Fleet", t => t.ConveyingFleet_FleetId)
                .ForeignKey("dbo.ServiceCentre", t => t.Destination_ServiceCentreId)
                .Index(t => t.ConveyingFleet_FleetId)
                .Index(t => t.Destination_ServiceCentreId);
            
            CreateTable(
                "dbo.StockRequestPart",
                c => new
                    {
                        StockRequestPartId = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        QuantitySupplied = c.Int(nullable: false),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SerialNumber = c.String(),
                        PartId = c.Int(nullable: false),
                        StockRequestId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.StockRequestPartId)
                .ForeignKey("dbo.FleetPart", t => t.PartId, cascadeDelete: true)
                .ForeignKey("dbo.StockRequest", t => t.StockRequestId, cascadeDelete: true)
                .Index(t => t.PartId)
                .Index(t => t.StockRequestId);
            
            CreateTable(
                "dbo.StockSupplyDetails",
                c => new
                    {
                        StockSupplyDetailsId = c.Int(nullable: false, identity: true),
                        InvoiceNumber = c.String(),
                        LPONumber = c.String(),
                        WaybillNumber = c.String(),
                        ScannedInvoiceURL = c.String(),
                        StockRequestId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.StockSupplyDetailsId)
                .ForeignKey("dbo.StockRequest", t => t.StockRequestId, cascadeDelete: true)
                .Index(t => t.StockRequestId);
            
            CreateTable(
                "dbo.TransferDetails",
                c => new
                    {
                        TransferDetailsId = c.Int(nullable: false, identity: true),
                        OriginatorAccountNumber = c.String(maxLength: 50),
                        Amount = c.String(maxLength: 50),
                        OriginatorName = c.String(maxLength: 150),
                        Narration = c.String(),
                        CrAccountName = c.String(maxLength: 100),
                        PaymentReference = c.String(),
                        BankName = c.String(maxLength: 100),
                        SessionId = c.String(),
                        CrAccount = c.String(maxLength: 50),
                        BankCode = c.String(maxLength: 50),
                        CreatedAt = c.String(maxLength: 50),
                        ResponseCode = c.String(maxLength: 25),
                        TransactionStatus = c.String(maxLength: 25),
                        IsVerified = c.Boolean(nullable: false),
                        Id = c.String(maxLength: 50),
                        ModifiedAt = c.String(maxLength: 50),
                        FromKey = c.String(maxLength: 50),
                        ToKey = c.String(maxLength: 50),
                        SenderName = c.String(maxLength: 100),
                        SenderBank = c.String(maxLength: 100),
                        Charge = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Note = c.String(maxLength: 150),
                        Status = c.String(maxLength: 50),
                        RefId = c.String(maxLength: 100),
                        CustomerRef = c.String(maxLength: 100),
                        SetRefId = c.String(maxLength: 100),
                        Type = c.String(maxLength: 50),
                        Settled = c.Boolean(nullable: false),
                        DeviceId = c.String(maxLength: 50),
                        TimedAccNo = c.String(maxLength: 50),
                        ManagerName = c.String(maxLength: 50),
                        IsPaymentGateway = c.Boolean(nullable: false),
                        ProcessingPartner = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.TransferDetailsId);
            
            CreateTable(
                "dbo.TransitWaybillNumber",
                c => new
                    {
                        TransitWaybillNumberId = c.Int(nullable: false, identity: true),
                        WaybillNumber = c.String(maxLength: 100),
                        ServiceCentreId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        IsGrouped = c.Boolean(nullable: false),
                        IsTransitCompleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.TransitWaybillNumberId)
                .ForeignKey("dbo.ServiceCentre", t => t.ServiceCentreId, cascadeDelete: true)
                .Index(t => t.WaybillNumber, unique: true)
                .Index(t => t.ServiceCentreId);
            
            CreateTable(
                "dbo.TransitWaybillNumber_Archive",
                c => new
                    {
                        TransitWaybillNumberId = c.Int(nullable: false, identity: true),
                        WaybillNumber = c.String(maxLength: 100),
                        ServiceCentreId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        IsGrouped = c.Boolean(nullable: false),
                        IsTransitCompleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.TransitWaybillNumberId);
            
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
                "dbo.UserLoginEmail",
                c => new
                    {
                        UserLoginEmailId = c.Int(nullable: false, identity: true),
                        Email = c.String(maxLength: 100),
                        DateLastSent = c.DateTime(nullable: false),
                        NumberOfEmailsSent = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.UserLoginEmailId)
                .Index(t => t.Email, unique: true);
            
            CreateTable(
                "dbo.UserServiceCentreMapping",
                c => new
                    {
                        UserServiceCentreMappingId = c.Int(nullable: false, identity: true),
                        IsActive = c.Boolean(nullable: false),
                        UserId = c.String(maxLength: 128),
                        ServiceCentreId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.UserServiceCentreMappingId)
                .ForeignKey("dbo.ServiceCentre", t => t.ServiceCentreId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.ServiceCentreId);
            
            CreateTable(
                "dbo.VAT",
                c => new
                    {
                        VATId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Type = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CountryId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.VATId);
            
            CreateTable(
                "dbo.VehicleType",
                c => new
                    {
                        VehicleTypeId = c.Int(nullable: false, identity: true),
                        Partnercode = c.String(maxLength: 100),
                        Vehicletype = c.String(maxLength: 100),
                        VehiclePlateNumber = c.String(maxLength: 100),
                        VehicleInsurancePolicyDetails = c.String(),
                        VehicleRoadWorthinessDetails = c.String(),
                        VehicleParticularsDetails = c.String(),
                        IsVerified = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.VehicleTypeId);
            
            CreateTable(
                "dbo.WalletNumber",
                c => new
                    {
                        WalletNumberId = c.Int(nullable: false, identity: true),
                        WalletPan = c.String(maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.WalletNumberId);
            
            CreateTable(
                "dbo.WalletPaymentLog",
                c => new
                    {
                        WalletPaymentLogId = c.Int(nullable: false, identity: true),
                        WalletId = c.Int(nullable: false),
                        Reference = c.String(maxLength: 100),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TransactionStatus = c.String(),
                        TransactionResponse = c.String(),
                        UserId = c.String(maxLength: 128),
                        IsWalletCredited = c.Boolean(nullable: false),
                        OnlinePaymentType = c.Int(nullable: false),
                        PaymentCountryId = c.Int(nullable: false),
                        ExternalReference = c.String(maxLength: 50),
                        PhoneNumber = c.String(maxLength: 20),
                        TransactionType = c.Int(nullable: false),
                        isConverted = c.Boolean(nullable: false),
                        CardType = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.WalletPaymentLogId)
                .ForeignKey("dbo.Wallet", t => t.WalletId, cascadeDelete: true)
                .Index(t => t.WalletId)
                .Index(t => t.Reference, unique: true);
            
            CreateTable(
                "dbo.WalletTransaction",
                c => new
                    {
                        WalletTransactionId = c.Int(nullable: false, identity: true),
                        DateOfEntry = c.DateTime(nullable: false),
                        ServiceCentreId = c.Int(nullable: false),
                        WalletId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreditDebitType = c.Int(nullable: false),
                        Description = c.String(),
                        IsDeferred = c.Boolean(nullable: false),
                        Waybill = c.String(maxLength: 100),
                        ClientNodeId = c.String(),
                        PaymentType = c.Int(nullable: false),
                        PaymentTypeReference = c.String(maxLength: 100),
                        BalanceAfterTransaction = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Manifest = c.String(maxLength: 100),
                        TransactionCountryId = c.Int(nullable: false),
                        ServiceCharge = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.WalletTransactionId)
                .ForeignKey("dbo.ServiceCentre", t => t.ServiceCentreId, cascadeDelete: true)
                .ForeignKey("dbo.Wallet", t => t.WalletId, cascadeDelete: true)
                .Index(t => t.ServiceCentreId)
                .Index(t => t.WalletId);
            
            CreateTable(
                "dbo.WaybillCharge",
                c => new
                    {
                        WaybillChargeId = c.Int(nullable: false, identity: true),
                        Waybill = c.String(maxLength: 128),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Description = c.String(maxLength: 300),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.WaybillChargeId);
            
            CreateTable(
                "dbo.WaybillNumber",
                c => new
                    {
                        WaybillNumberId = c.Int(nullable: false, identity: true),
                        WaybillCode = c.String(maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                        UserId = c.String(maxLength: 128),
                        ServiceCentreId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.WaybillNumberId)
                .ForeignKey("dbo.ServiceCentre", t => t.ServiceCentreId, cascadeDelete: true)
                .Index(t => t.WaybillCode, unique: true)
                .Index(t => t.ServiceCentreId);
            
            CreateTable(
                "dbo.WaybillPaymentLog",
                c => new
                    {
                        WaybillPaymentLogId = c.Int(nullable: false, identity: true),
                        Waybill = c.String(),
                        Reference = c.String(maxLength: 100),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TransactionStatus = c.String(),
                        TransactionResponse = c.String(),
                        Currency = c.String(maxLength: 10),
                        OnlinePaymentType = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        IsWaybillSettled = c.Boolean(nullable: false),
                        IsPaymentSuccessful = c.Boolean(nullable: false),
                        PhoneNumber = c.String(maxLength: 100),
                        Email = c.String(maxLength: 100),
                        NetworkProvider = c.String(maxLength: 50),
                        PaymentCountryId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.WaybillPaymentLogId)
                .Index(t => t.Reference, unique: true);
            
            CreateTable(
                "dbo.WeightLimitPrice",
                c => new
                    {
                        WeightLimitPriceId = c.Int(nullable: false, identity: true),
                        ZoneId = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Weight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RegularEcommerceType = c.Int(nullable: false),
                        CountryId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.WeightLimitPriceId)
                .ForeignKey("dbo.Zone", t => t.ZoneId, cascadeDelete: true)
                .Index(t => t.ZoneId);
            
            CreateTable(
                "dbo.WeightLimit",
                c => new
                    {
                        WeightLimitId = c.Int(nullable: false, identity: true),
                        Weight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.WeightLimitId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WeightLimitPrice", "ZoneId", "dbo.Zone");
            DropForeignKey("dbo.WaybillNumber", "ServiceCentreId", "dbo.ServiceCentre");
            DropForeignKey("dbo.WalletTransaction", "WalletId", "dbo.Wallet");
            DropForeignKey("dbo.WalletTransaction", "ServiceCentreId", "dbo.ServiceCentre");
            DropForeignKey("dbo.WalletPaymentLog", "WalletId", "dbo.Wallet");
            DropForeignKey("dbo.UserServiceCentreMapping", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserServiceCentreMapping", "ServiceCentreId", "dbo.ServiceCentre");
            DropForeignKey("dbo.TransitWaybillNumber", "ServiceCentreId", "dbo.ServiceCentre");
            DropForeignKey("dbo.StockSupplyDetails", "StockRequestId", "dbo.StockRequest");
            DropForeignKey("dbo.StockRequestPart", "StockRequestId", "dbo.StockRequest");
            DropForeignKey("dbo.StockRequestPart", "PartId", "dbo.FleetPart");
            DropForeignKey("dbo.StockRequest", "Destination_ServiceCentreId", "dbo.ServiceCentre");
            DropForeignKey("dbo.StockRequest", "ConveyingFleet_FleetId", "dbo.Fleet");
            DropForeignKey("dbo.SpecialDomesticZonePrice", "ZoneId", "dbo.Zone");
            DropForeignKey("dbo.SpecialDomesticZonePrice", "SpecialDomesticPackageId", "dbo.SpecialDomesticPackage");
            DropForeignKey("dbo.SpecialDomesticPackage", "SubCategory_SubCategoryId", "dbo.SubCategory");
            DropForeignKey("dbo.SubCategory", "CategoryId", "dbo.Category");
            DropForeignKey("dbo.SLASignedUser", "SLAId", "dbo.SLA");
            DropForeignKey("dbo.ShipmentTracking", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ShipmentPackagingTransactions", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ShipmentDeliveryOptionMapping", "DeliveryOptionId", "dbo.DeliveryOption");
            DropForeignKey("dbo.Route", "DestinationCenter_ServiceCentreId", "dbo.ServiceCentre");
            DropForeignKey("dbo.Route", "DepartureCenter_ServiceCentreId", "dbo.ServiceCentre");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.PreShipmentManifestMapping", "PreShipmentId", "dbo.PreShipment");
            DropForeignKey("dbo.PreShipmentItem", "PreShipmentId", "dbo.PreShipment");
            DropForeignKey("dbo.PickupManifestWaybillMapping", "ServiceCentreId", "dbo.ServiceCentre");
            DropForeignKey("dbo.PickupManifest", "Shipment_PreShipmentMobileId", "dbo.PreShipmentMobile");
            DropForeignKey("dbo.PreShipmentMobile", "serviceCentreLocation_LocationId", "dbo.Location");
            DropForeignKey("dbo.PreShipmentMobile", "SenderLocation_LocationId", "dbo.Location");
            DropForeignKey("dbo.PreShipmentMobile", "ReceiverLocation_LocationId", "dbo.Location");
            DropForeignKey("dbo.PreShipmentItemMobile", "PreShipmentMobileId", "dbo.PreShipmentMobile");
            DropForeignKey("dbo.PickupManifest", "FleetTripId", "dbo.FleetTrip");
            DropForeignKey("dbo.MovementManifestNumber", "DestinationServiceCentre_ServiceCentreId", "dbo.ServiceCentre");
            DropForeignKey("dbo.MovementManifestNumber", "DepartureServiceCentre_ServiceCentreId", "dbo.ServiceCentre");
            DropForeignKey("dbo.MovementDispatch", "ServiceCentreId", "dbo.ServiceCentre");
            DropForeignKey("dbo.MovementDispatch", "DestinationId", "dbo.Station");
            DropForeignKey("dbo.MovementDispatch", "DepartureId", "dbo.Station");
            DropForeignKey("dbo.MobileShipmentTracking", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.MobilePickUpRequests", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ManifestWaybillMapping", "ServiceCentreId", "dbo.ServiceCentre");
            DropForeignKey("dbo.ManifestVisitMonitoring", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ManifestVisitMonitoring", "ServiceCentreId", "dbo.ServiceCentre");
            DropForeignKey("dbo.Manifest", "ShipmentId", "dbo.Shipment");
            DropForeignKey("dbo.Shipment", "ShipmentReroute_WaybillNew", "dbo.ShipmentReroute");
            DropForeignKey("dbo.ShipmentItem", "ShipmentId", "dbo.Shipment");
            DropForeignKey("dbo.Shipment", "DeliveryOptionId", "dbo.DeliveryOption");
            DropForeignKey("dbo.Manifest", "FleetTripId", "dbo.FleetTrip");
            DropForeignKey("dbo.SubSubNav", "SubNavId", "dbo.SubNav");
            DropForeignKey("dbo.SubNav", "MainNavId", "dbo.MainNav");
            DropForeignKey("dbo.MagayaShipmentItem", "MagayaShipmentId", "dbo.MagayaShipmentAgility");
            DropForeignKey("dbo.JobCardManagementPart", "JobCardManagementId", "dbo.JobCardManagement");
            DropForeignKey("dbo.JobCardManagementPart", "PartId", "dbo.FleetPart");
            DropForeignKey("dbo.JobCardManagement", "WorkshopId", "dbo.Workshop");
            DropForeignKey("dbo.Workshop", "WorkshopSupervisor_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.JobCardManagement", "MechanicUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.JobCardManagement", "MechanicSupervisor_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.JobCardManagement", "JobCardId", "dbo.JobCard");
            DropForeignKey("dbo.JobCard", "Requester_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.JobCard", "Fleet_FleetId", "dbo.Fleet");
            DropForeignKey("dbo.JobCard", "Approver_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.IntlShipmentRequestItem", "IntlShipmentRequestId", "dbo.IntlShipmentRequest");
            DropForeignKey("dbo.IntlShipmentRequest", "DestinationServiceCentre_ServiceCentreId", "dbo.ServiceCentre");
            DropForeignKey("dbo.InternationalRequestReceiverItem", "InternationalRequestReceiverId", "dbo.InternationalRequestReceiver");
            DropForeignKey("dbo.InternationalCargoManifestDetail", "InternationalCargoManifestId", "dbo.InternationalCargoManifest");
            DropForeignKey("dbo.HUBManifestWaybillMapping", "ServiceCentreId", "dbo.ServiceCentre");
            DropForeignKey("dbo.HaulageDistanceMappingPrice", "HaulageId", "dbo.Haulage");
            DropForeignKey("dbo.HaulageDistanceMapping", "DestinationId", "dbo.Station");
            DropForeignKey("dbo.HaulageDistanceMapping", "DepartureId", "dbo.Station");
            DropForeignKey("dbo.GroupWaybillNumberMapping", "OriginalDepartureServiceCentre_ServiceCentreId", "dbo.ServiceCentre");
            DropForeignKey("dbo.GroupWaybillNumberMapping", "DestinationServiceCentre_ServiceCentreId", "dbo.ServiceCentre");
            DropForeignKey("dbo.GroupWaybillNumberMapping", "DepartureServiceCentre_ServiceCentreId", "dbo.ServiceCentre");
            DropForeignKey("dbo.GroupWaybillNumber", "ServiceCentreId", "dbo.ServiceCentre");
            DropForeignKey("dbo.GeneralLedger", "ServiceCentreId", "dbo.ServiceCentre");
            DropForeignKey("dbo.FleetTrip", "FleetId", "dbo.Fleet");
            DropForeignKey("dbo.FleetTrip", "DestinationStationId", "dbo.Station");
            DropForeignKey("dbo.FleetTrip", "DepartureStationId", "dbo.Station");
            DropForeignKey("dbo.FleetTrip", "CaptainId", "dbo.AspNetUsers");
            DropForeignKey("dbo.FleetPartnerTransaction", "FleetId", "dbo.Fleet");
            DropForeignKey("dbo.FleetPartInventoryHistory", "VendorId", "dbo.Vendor");
            DropForeignKey("dbo.FleetPartInventoryHistory", "StoreId", "dbo.Store");
            DropForeignKey("dbo.FleetPartInventoryHistory", "MovedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.FleetPartInventoryHistory", "PartId", "dbo.FleetPart");
            DropForeignKey("dbo.FleetPartInventory", "StoreId", "dbo.Store");
            DropForeignKey("dbo.FleetPartInventory", "PartId", "dbo.FleetPart");
            DropForeignKey("dbo.FleetPart", "ModelId", "dbo.FleetModel");
            DropForeignKey("dbo.FleetJobCard", "FleetOwnerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.FleetJobCard", "FleetId", "dbo.Fleet");
            DropForeignKey("dbo.FleetDisputeMessage", "FleetOwnerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Fleet", "PartnerId", "dbo.Partner");
            DropForeignKey("dbo.Fleet", "ModelId", "dbo.FleetModel");
            DropForeignKey("dbo.Fleet", "FleetMake_MakeId", "dbo.FleetMake");
            DropForeignKey("dbo.FleetModel", "MakeId", "dbo.FleetMake");
            DropForeignKey("dbo.Fleet", "EnterprisePartnerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Expenditure", "ServiceCentreId", "dbo.ServiceCentre");
            DropForeignKey("dbo.Expenditure", "ExpenseTypeId", "dbo.ExpenseType");
            DropForeignKey("dbo.DomesticZonePrice", "ZoneId", "dbo.Zone");
            DropForeignKey("dbo.DomesticRouteZoneMap", "ZoneId", "dbo.Zone");
            DropForeignKey("dbo.DomesticRouteZoneMap", "DestinationId", "dbo.Station");
            DropForeignKey("dbo.DomesticRouteZoneMap", "DepartureId", "dbo.Station");
            DropForeignKey("dbo.DispatchActivity", "DispatchId", "dbo.Dispatch");
            DropForeignKey("dbo.Dispatch", "ServiceCentreId", "dbo.ServiceCentre");
            DropForeignKey("dbo.Dispatch", "DestinationId", "dbo.Station");
            DropForeignKey("dbo.Dispatch", "DepartureId", "dbo.Station");
            DropForeignKey("dbo.DeviceManagement", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.DeviceManagement", "Location_ServiceCentreId", "dbo.ServiceCentre");
            DropForeignKey("dbo.Station", "StateId", "dbo.State");
            DropForeignKey("dbo.ServiceCentre", "StationId", "dbo.Station");
            DropForeignKey("dbo.DeviceManagement", "DeviceId", "dbo.Device");
            DropForeignKey("dbo.DeliveryOptionPrice", "ZoneId", "dbo.Zone");
            DropForeignKey("dbo.DeliveryOptionPrice", "DeliveryOptionId", "dbo.DeliveryOption");
            DropForeignKey("dbo.InvoiceCharge", "CustomerInvoiceId", "dbo.CustomerInvoice");
            DropForeignKey("dbo.CountryRouteZoneMap", "ZoneId", "dbo.Zone");
            DropForeignKey("dbo.CountryRouteZoneMap", "DestinationId", "dbo.Country");
            DropForeignKey("dbo.CountryRouteZoneMap", "DepartureId", "dbo.Country");
            DropForeignKey("dbo.CompanyContactPerson", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.CashOnDeliveryBalance", "WalletId", "dbo.Wallet");
            DropForeignKey("dbo.CashOnDeliveryAccount", "WalletId", "dbo.Wallet");
            DropIndex("dbo.WeightLimitPrice", new[] { "ZoneId" });
            DropIndex("dbo.WaybillPaymentLog", new[] { "Reference" });
            DropIndex("dbo.WaybillNumber", new[] { "ServiceCentreId" });
            DropIndex("dbo.WaybillNumber", new[] { "WaybillCode" });
            DropIndex("dbo.WalletTransaction", new[] { "WalletId" });
            DropIndex("dbo.WalletTransaction", new[] { "ServiceCentreId" });
            DropIndex("dbo.WalletPaymentLog", new[] { "Reference" });
            DropIndex("dbo.WalletPaymentLog", new[] { "WalletId" });
            DropIndex("dbo.UserServiceCentreMapping", new[] { "ServiceCentreId" });
            DropIndex("dbo.UserServiceCentreMapping", new[] { "UserId" });
            DropIndex("dbo.UserLoginEmail", new[] { "Email" });
            DropIndex("dbo.TransitWaybillNumber", new[] { "ServiceCentreId" });
            DropIndex("dbo.TransitWaybillNumber", new[] { "WaybillNumber" });
            DropIndex("dbo.StockSupplyDetails", new[] { "StockRequestId" });
            DropIndex("dbo.StockRequestPart", new[] { "StockRequestId" });
            DropIndex("dbo.StockRequestPart", new[] { "PartId" });
            DropIndex("dbo.StockRequest", new[] { "Destination_ServiceCentreId" });
            DropIndex("dbo.StockRequest", new[] { "ConveyingFleet_FleetId" });
            DropIndex("dbo.SpecialDomesticZonePrice", new[] { "SpecialDomesticPackageId" });
            DropIndex("dbo.SpecialDomesticZonePrice", new[] { "ZoneId" });
            DropIndex("dbo.SubCategory", new[] { "CategoryId" });
            DropIndex("dbo.SpecialDomesticPackage", new[] { "SubCategory_SubCategoryId" });
            DropIndex("dbo.SLASignedUser", new[] { "SLAId" });
            DropIndex("dbo.ShipmentTracking", new[] { "UserId" });
            DropIndex("dbo.ShipmentTracking", new[] { "Waybill" });
            DropIndex("dbo.ShipmentTimeMonitor", new[] { "Waybill" });
            DropIndex("dbo.ShipmentReturn", new[] { "WaybillOld" });
            DropIndex("dbo.ShipmentReturn", new[] { "WaybillNew" });
            DropIndex("dbo.ShipmentPackagingTransactions", new[] { "UserId" });
            DropIndex("dbo.ShipmentDeliveryOptionMapping", new[] { "DeliveryOptionId" });
            DropIndex("dbo.ShipmentDeliveryOptionMapping", new[] { "Waybill" });
            DropIndex("dbo.ShipmentContactHistory", new[] { "Waybill" });
            DropIndex("dbo.ShipmentContact", new[] { "Waybill" });
            DropIndex("dbo.ShipmentCollection", new[] { "Waybill" });
            DropIndex("dbo.ShipmentCancel", new[] { "Waybill" });
            DropIndex("dbo.ScanStatus", new[] { "Code" });
            DropIndex("dbo.Route", new[] { "DestinationCenter_ServiceCentreId" });
            DropIndex("dbo.Route", new[] { "DepartureCenter_ServiceCentreId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Region", new[] { "RegionName" });
            DropIndex("dbo.PreShipmentManifestMapping", new[] { "PreShipmentId" });
            DropIndex("dbo.PreShipmentItem", new[] { "PreShipmentId" });
            DropIndex("dbo.PreShipment", new[] { "TempCode" });
            DropIndex("dbo.PickupManifestWaybillMapping", new[] { "ServiceCentreId" });
            DropIndex("dbo.PreShipmentItemMobile", new[] { "PreShipmentMobileId" });
            DropIndex("dbo.PreShipmentMobile", new[] { "serviceCentreLocation_LocationId" });
            DropIndex("dbo.PreShipmentMobile", new[] { "SenderLocation_LocationId" });
            DropIndex("dbo.PreShipmentMobile", new[] { "ReceiverLocation_LocationId" });
            DropIndex("dbo.PreShipmentMobile", new[] { "Waybill" });
            DropIndex("dbo.PickupManifest", new[] { "Shipment_PreShipmentMobileId" });
            DropIndex("dbo.PickupManifest", new[] { "FleetTripId" });
            DropIndex("dbo.PickupManifest", new[] { "ManifestCode" });
            DropIndex("dbo.OverdueShipment", new[] { "Waybill" });
            DropIndex("dbo.MovementManifestNumber", new[] { "DestinationServiceCentre_ServiceCentreId" });
            DropIndex("dbo.MovementManifestNumber", new[] { "DepartureServiceCentre_ServiceCentreId" });
            DropIndex("dbo.MovementManifestNumber", new[] { "MovementManifestCode" });
            DropIndex("dbo.MovementDispatch", new[] { "DestinationId" });
            DropIndex("dbo.MovementDispatch", new[] { "DepartureId" });
            DropIndex("dbo.MovementDispatch", new[] { "ServiceCentreId" });
            DropIndex("dbo.MovementDispatch", new[] { "MovementManifestNumber" });
            DropIndex("dbo.MobileShipmentTracking", new[] { "UserId" });
            DropIndex("dbo.MobileScanStatus", new[] { "Code" });
            DropIndex("dbo.MobilePickUpRequests", new[] { "UserId" });
            DropIndex("dbo.ManifestWaybillMapping", new[] { "ServiceCentreId" });
            DropIndex("dbo.ManifestVisitMonitoring", new[] { "ServiceCentreId" });
            DropIndex("dbo.ManifestVisitMonitoring", new[] { "UserId" });
            DropIndex("dbo.ShipmentReroute", new[] { "WaybillOld" });
            DropIndex("dbo.ShipmentReroute", new[] { "WaybillNew" });
            DropIndex("dbo.ShipmentItem", new[] { "ShipmentId" });
            DropIndex("dbo.Shipment", new[] { "ShipmentReroute_WaybillNew" });
            DropIndex("dbo.Shipment", new[] { "DeliveryOptionId" });
            DropIndex("dbo.Shipment", new[] { "Waybill" });
            DropIndex("dbo.Manifest", new[] { "FleetTripId" });
            DropIndex("dbo.Manifest", new[] { "ShipmentId" });
            DropIndex("dbo.Manifest", new[] { "ManifestCode" });
            DropIndex("dbo.SubSubNav", new[] { "SubNavId" });
            DropIndex("dbo.SubNav", new[] { "MainNavId" });
            DropIndex("dbo.MagayaShipmentItem", new[] { "MagayaShipmentId" });
            DropIndex("dbo.JobCardManagementPart", new[] { "JobCardManagementId" });
            DropIndex("dbo.JobCardManagementPart", new[] { "PartId" });
            DropIndex("dbo.Workshop", new[] { "WorkshopSupervisor_Id" });
            DropIndex("dbo.JobCardManagement", new[] { "MechanicUser_Id" });
            DropIndex("dbo.JobCardManagement", new[] { "MechanicSupervisor_Id" });
            DropIndex("dbo.JobCardManagement", new[] { "JobCardId" });
            DropIndex("dbo.JobCardManagement", new[] { "WorkshopId" });
            DropIndex("dbo.JobCard", new[] { "Requester_Id" });
            DropIndex("dbo.JobCard", new[] { "Fleet_FleetId" });
            DropIndex("dbo.JobCard", new[] { "Approver_Id" });
            DropIndex("dbo.Invoice", new[] { "InvoiceNo" });
            DropIndex("dbo.IntlShipmentRequestItem", new[] { "IntlShipmentRequestId" });
            DropIndex("dbo.IntlShipmentRequest", new[] { "DestinationServiceCentre_ServiceCentreId" });
            DropIndex("dbo.IntlShipmentRequest", new[] { "RequestNumber" });
            DropIndex("dbo.InternationalRequestReceiverItem", new[] { "InternationalRequestReceiverId" });
            DropIndex("dbo.InternationalCargoManifestDetail", new[] { "InternationalCargoManifestId" });
            DropIndex("dbo.InternationalCargoManifest", new[] { "ManifestNo" });
            DropIndex("dbo.IndividualCustomer", new[] { "PhoneNumber" });
            DropIndex("dbo.HUBManifestWaybillMapping", new[] { "ServiceCentreId" });
            DropIndex("dbo.HaulageDistanceMappingPrice", new[] { "HaulageId" });
            DropIndex("dbo.HaulageDistanceMapping", new[] { "DestinationId" });
            DropIndex("dbo.HaulageDistanceMapping", new[] { "DepartureId" });
            DropIndex("dbo.GroupWaybillNumberMapping", new[] { "OriginalDepartureServiceCentre_ServiceCentreId" });
            DropIndex("dbo.GroupWaybillNumberMapping", new[] { "DestinationServiceCentre_ServiceCentreId" });
            DropIndex("dbo.GroupWaybillNumberMapping", new[] { "DepartureServiceCentre_ServiceCentreId" });
            DropIndex("dbo.GroupWaybillNumber", new[] { "ServiceCentreId" });
            DropIndex("dbo.GroupWaybillNumber", new[] { "GroupWaybillCode" });
            DropIndex("dbo.GeneralLedger", new[] { "ServiceCentreId" });
            DropIndex("dbo.FleetTrip", new[] { "DestinationStationId" });
            DropIndex("dbo.FleetTrip", new[] { "DepartureStationId" });
            DropIndex("dbo.FleetTrip", new[] { "CaptainId" });
            DropIndex("dbo.FleetTrip", new[] { "FleetId" });
            DropIndex("dbo.FleetPartnerTransaction", new[] { "FleetId" });
            DropIndex("dbo.FleetPartInventoryHistory", new[] { "MovedBy_Id" });
            DropIndex("dbo.FleetPartInventoryHistory", new[] { "VendorId" });
            DropIndex("dbo.FleetPartInventoryHistory", new[] { "StoreId" });
            DropIndex("dbo.FleetPartInventoryHistory", new[] { "PartId" });
            DropIndex("dbo.FleetPartInventory", new[] { "StoreId" });
            DropIndex("dbo.FleetPartInventory", new[] { "PartId" });
            DropIndex("dbo.FleetPart", new[] { "ModelId" });
            DropIndex("dbo.FleetJobCard", new[] { "FleetOwnerId" });
            DropIndex("dbo.FleetJobCard", new[] { "FleetId" });
            DropIndex("dbo.FleetDisputeMessage", new[] { "FleetOwnerId" });
            DropIndex("dbo.FleetModel", new[] { "MakeId" });
            DropIndex("dbo.Fleet", new[] { "FleetMake_MakeId" });
            DropIndex("dbo.Fleet", new[] { "EnterprisePartnerId" });
            DropIndex("dbo.Fleet", new[] { "PartnerId" });
            DropIndex("dbo.Fleet", new[] { "ModelId" });
            DropIndex("dbo.Expenditure", new[] { "ServiceCentreId" });
            DropIndex("dbo.Expenditure", new[] { "ExpenseTypeId" });
            DropIndex("dbo.DomesticZonePrice", new[] { "ZoneId" });
            DropIndex("dbo.DomesticRouteZoneMap", new[] { "DestinationId" });
            DropIndex("dbo.DomesticRouteZoneMap", new[] { "DepartureId" });
            DropIndex("dbo.DomesticRouteZoneMap", new[] { "ZoneId" });
            DropIndex("dbo.DispatchActivity", new[] { "DispatchId" });
            DropIndex("dbo.Dispatch", new[] { "DestinationId" });
            DropIndex("dbo.Dispatch", new[] { "DepartureId" });
            DropIndex("dbo.Dispatch", new[] { "ServiceCentreId" });
            DropIndex("dbo.Dispatch", new[] { "ManifestNumber" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Station", new[] { "StateId" });
            DropIndex("dbo.ServiceCentre", new[] { "StationId" });
            DropIndex("dbo.ServiceCentre", new[] { "Code" });
            DropIndex("dbo.ServiceCentre", new[] { "Name" });
            DropIndex("dbo.DeviceManagement", new[] { "Location_ServiceCentreId" });
            DropIndex("dbo.DeviceManagement", new[] { "DeviceId" });
            DropIndex("dbo.DeviceManagement", new[] { "UserId" });
            DropIndex("dbo.DeliveryOptionPrice", new[] { "DeliveryOptionId" });
            DropIndex("dbo.DeliveryOptionPrice", new[] { "ZoneId" });
            DropIndex("dbo.InvoiceCharge", new[] { "CustomerInvoiceId" });
            DropIndex("dbo.CountryRouteZoneMap", new[] { "DestinationId" });
            DropIndex("dbo.CountryRouteZoneMap", new[] { "DepartureId" });
            DropIndex("dbo.CountryRouteZoneMap", new[] { "ZoneId" });
            DropIndex("dbo.CompanyContactPerson", new[] { "CompanyId" });
            DropIndex("dbo.Company", new[] { "PhoneNumber" });
            DropIndex("dbo.CODSettlementSheet", new[] { "Waybill" });
            DropIndex("dbo.CashOnDeliveryBalance", new[] { "WalletId" });
            DropIndex("dbo.CashOnDeliveryAccount", new[] { "WalletId" });
            DropIndex("dbo.Bank", new[] { "BankName" });
            DropTable("dbo.WeightLimit");
            DropTable("dbo.WeightLimitPrice");
            DropTable("dbo.WaybillPaymentLog");
            DropTable("dbo.WaybillNumber");
            DropTable("dbo.WaybillCharge");
            DropTable("dbo.WalletTransaction");
            DropTable("dbo.WalletPaymentLog");
            DropTable("dbo.WalletNumber");
            DropTable("dbo.VehicleType");
            DropTable("dbo.VAT");
            DropTable("dbo.UserServiceCentreMapping");
            DropTable("dbo.UserLoginEmail");
            DropTable("dbo.UnidentifiedItemsForInternationalShipping");
            DropTable("dbo.TransitWaybillNumber_Archive");
            DropTable("dbo.TransitWaybillNumber");
            DropTable("dbo.TransferDetails");
            DropTable("dbo.StockSupplyDetails");
            DropTable("dbo.StockRequestPart");
            DropTable("dbo.StockRequest");
            DropTable("dbo.SpecialDomesticZonePrice");
            DropTable("dbo.SubCategory");
            DropTable("dbo.SpecialDomesticPackage");
            DropTable("dbo.SmsSendLog");
            DropTable("dbo.SLASignedUser");
            DropTable("dbo.SLA");
            DropTable("dbo.ShipmentTracking");
            DropTable("dbo.ShipmentTimeMonitor");
            DropTable("dbo.ShipmentReturn");
            DropTable("dbo.ShipmentPackagingTransactions");
            DropTable("dbo.ShipmentPackagePrice");
            DropTable("dbo.ShipmentItem_Archive");
            DropTable("dbo.ShipmentHash");
            DropTable("dbo.ShipmentExport");
            DropTable("dbo.ShipmentDeliveryOptionMapping");
            DropTable("dbo.ShipmentContactHistory");
            DropTable("dbo.ShipmentContact");
            DropTable("dbo.ShipmentCollection");
            DropTable("dbo.ShipmentCategory");
            DropTable("dbo.ShipmentCancel");
            DropTable("dbo.Shipment_Archive");
            DropTable("dbo.ServiceCenterPackage");
            DropTable("dbo.ScanStatus");
            DropTable("dbo.Route");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.RiderDelivery");
            DropTable("dbo.RegionServiceCentreMapping");
            DropTable("dbo.Region");
            DropTable("dbo.ReferrerCode");
            DropTable("dbo.RankHistory");
            DropTable("dbo.PriceCategory");
            DropTable("dbo.PreShipmentManifestMapping");
            DropTable("dbo.PreShipmentItem");
            DropTable("dbo.PreShipment");
            DropTable("dbo.PlaceLocation");
            DropTable("dbo.PickupManifestWaybillMapping");
            DropTable("dbo.PreShipmentItemMobile");
            DropTable("dbo.PreShipmentMobile");
            DropTable("dbo.PickupManifest");
            DropTable("dbo.PaymentTransaction");
            DropTable("dbo.PaymentPartialTransaction");
            DropTable("dbo.PaymentMethod");
            DropTable("dbo.PartnerTransactions");
            DropTable("dbo.PartnerPayout");
            DropTable("dbo.PartnerApplication");
            DropTable("dbo.PackingList");
            DropTable("dbo.OverdueShipment");
            DropTable("dbo.OTP");
            DropTable("dbo.NumberGeneratorMonitor");
            DropTable("dbo.Notification");
            DropTable("dbo.MovementManifestNumberMapping");
            DropTable("dbo.MovementManifestNumber");
            DropTable("dbo.MovementDispatch");
            DropTable("dbo.MobileShipmentTracking");
            DropTable("dbo.MobileScanStatus");
            DropTable("dbo.MobileRating");
            DropTable("dbo.MobilePickUpRequests");
            DropTable("dbo.MobileGroupCodeWaybillMapping");
            DropTable("dbo.MissingShipment");
            DropTable("dbo.Message");
            DropTable("dbo.ManifestWaybillMapping");
            DropTable("dbo.ManifestVisitMonitoring");
            DropTable("dbo.ManifestGroupWaybillNumberMapping");
            DropTable("dbo.ShipmentReroute");
            DropTable("dbo.ShipmentItem");
            DropTable("dbo.Shipment");
            DropTable("dbo.Manifest");
            DropTable("dbo.SubSubNav");
            DropTable("dbo.SubNav");
            DropTable("dbo.MainNav");
            DropTable("dbo.MagayaShipmentItem");
            DropTable("dbo.MagayaShipmentAgility");
            DropTable("dbo.LogVisitReason");
            DropTable("dbo.LogEntry");
            DropTable("dbo.Location");
            DropTable("dbo.LGA");
            DropTable("dbo.JobCardManagementPart");
            DropTable("dbo.Workshop");
            DropTable("dbo.JobCardManagement");
            DropTable("dbo.JobCard");
            DropTable("dbo.InvoiceShipment");
            DropTable("dbo.Invoice_Archive");
            DropTable("dbo.Invoice");
            DropTable("dbo.IntlShipmentRequestItem");
            DropTable("dbo.IntlShipmentRequest");
            DropTable("dbo.InternationalShipmentWaybill");
            DropTable("dbo.InternationalRequestReceiver");
            DropTable("dbo.InternationalRequestReceiverItem");
            DropTable("dbo.InternationalCargoManifestDetail");
            DropTable("dbo.InternationalCargoManifest");
            DropTable("dbo.Insurance");
            DropTable("dbo.IndividualCustomer");
            DropTable("dbo.InboundShipmentCategory");
            DropTable("dbo.InboundCategory");
            DropTable("dbo.HUBManifestWaybillMapping");
            DropTable("dbo.HaulageDistanceMappingPrice");
            DropTable("dbo.HaulageDistanceMapping");
            DropTable("dbo.Haulage");
            DropTable("dbo.GroupWaybillNumberMapping");
            DropTable("dbo.GroupWaybillNumber");
            DropTable("dbo.GlobalProperty");
            DropTable("dbo.GIGXUserDetail");
            DropTable("dbo.GiglgoStation");
            DropTable("dbo.GIGGOCODTransfer");
            DropTable("dbo.GeneralLedger_Archive");
            DropTable("dbo.GeneralLedger");
            DropTable("dbo.FleetTrip");
            DropTable("dbo.FleetPartnerTransaction");
            DropTable("dbo.FleetPartner");
            DropTable("dbo.Vendor");
            DropTable("dbo.FleetPartInventoryHistory");
            DropTable("dbo.Store");
            DropTable("dbo.FleetPartInventory");
            DropTable("dbo.FleetPart");
            DropTable("dbo.FleetJobCard");
            DropTable("dbo.FleetDisputeMessage");
            DropTable("dbo.Partner");
            DropTable("dbo.FleetMake");
            DropTable("dbo.FleetModel");
            DropTable("dbo.Fleet");
            DropTable("dbo.FinancialReport");
            DropTable("dbo.ExpenseType");
            DropTable("dbo.Expenditure");
            DropTable("dbo.EmailSendLog");
            DropTable("dbo.EcommerceAgreement");
            DropTable("dbo.DomesticZonePrice");
            DropTable("dbo.DomesticRouteZoneMap");
            DropTable("dbo.DispatchActivity");
            DropTable("dbo.Dispatch");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.State");
            DropTable("dbo.Station");
            DropTable("dbo.ServiceCentre");
            DropTable("dbo.DeviceManagement");
            DropTable("dbo.Device");
            DropTable("dbo.DemurrageRegisterAccount");
            DropTable("dbo.Demurrage");
            DropTable("dbo.DeliveryOptionPrice");
            DropTable("dbo.DeliveryOption");
            DropTable("dbo.DeliveryNumber");
            DropTable("dbo.DeliveryLocation");
            DropTable("dbo.InvoiceCharge");
            DropTable("dbo.CustomerInvoice");
            DropTable("dbo.CouponCodeManagement");
            DropTable("dbo.Zone");
            DropTable("dbo.CountryRouteZoneMap");
            DropTable("dbo.Country");
            DropTable("dbo.CompanyContactPerson");
            DropTable("dbo.Company");
            DropTable("dbo.CODWallet");
            DropTable("dbo.CODTransferRegister");
            DropTable("dbo.CODTransferLog");
            DropTable("dbo.CODSettlementSheet");
            DropTable("dbo.CodPayOutList");
            DropTable("dbo.CODGeneratedAccountNo");
            DropTable("dbo.ClientNode");
            DropTable("dbo.Category");
            DropTable("dbo.CashOnDeliveryRegisterAccount");
            DropTable("dbo.CashOnDeliveryBalance");
            DropTable("dbo.Wallet");
            DropTable("dbo.CashOnDeliveryAccount");
            DropTable("dbo.CaptainBonusByZoneMaping");
            DropTable("dbo.BillsPaymentManagement");
            DropTable("dbo.BankProcessingOrderForShipmentAndCOD");
            DropTable("dbo.BankProcessingOrderCodes");
            DropTable("dbo.Bank");
            DropTable("dbo.AuditTrailEvent");
            DropTable("dbo.ActivationCampaignEmail");
            DropTable("dbo.AccountTransaction");
            DropTable("dbo.AccountSummary");
        }
    }
}
