CREATE TABLE [dbo].[AccountSummary] (
    [AccountSummaryId] [int] NOT NULL IDENTITY,
    [Balance] [float] NOT NULL,
    [AccountType] [int] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.AccountSummary] PRIMARY KEY ([AccountSummaryId])
)
CREATE TABLE [dbo].[AccountTransaction] (
    [AccountTransactionId] [int] NOT NULL IDENTITY,
    [Amount] [float] NOT NULL,
    [DateOfTransaction] [datetime] NOT NULL,
    [Narration] [nvarchar](max),
    [TransactionReference] [nvarchar](max),
    [AccountType] [int] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.AccountTransaction] PRIMARY KEY ([AccountTransactionId])
)
CREATE TABLE [dbo].[AuditTrailEvent] (
    [EventId] [int] NOT NULL IDENTITY,
    [InsertedDate] [datetime],
    [LastUpdatedDate] [datetime],
    [Data] [nvarchar](max),
    CONSTRAINT [PK_dbo.AuditTrailEvent] PRIMARY KEY ([EventId])
)
CREATE TABLE [dbo].[CashOnDeliveryAccount] (
    [CashOnDeliveryAccountId] [int] NOT NULL IDENTITY,
    [Description] [nvarchar](max),
    [Amount] [decimal](18, 2) NOT NULL,
    [CreditDebitType] [int] NOT NULL,
    [WalletId] [int] NOT NULL,
    [UserId] [nvarchar](max),
    [CODStatus] [int] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.CashOnDeliveryAccount] PRIMARY KEY ([CashOnDeliveryAccountId])
)
CREATE TABLE [dbo].[Wallet] (
    [WalletId] [int] NOT NULL IDENTITY,
    [WalletNumber] [nvarchar](max),
    [Balance] [decimal](18, 2) NOT NULL,
    [CustomerId] [int] NOT NULL,
    [CustomerType] [int] NOT NULL,
    [IsSystem] [bit] NOT NULL,
    [CustomerCode] [nvarchar](max),
    [CompanyType] [nvarchar](max),
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.Wallet] PRIMARY KEY ([WalletId])
)
CREATE TABLE [dbo].[CashOnDeliveryBalance] (
    [CashOnDeliveryBalanceId] [int] NOT NULL IDENTITY,
    [Balance] [decimal](18, 2) NOT NULL,
    [WalletId] [int] NOT NULL,
    [UserId] [nvarchar](max),
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.CashOnDeliveryBalance] PRIMARY KEY ([CashOnDeliveryBalanceId])
)
CREATE TABLE [dbo].[ClientNode] (
    [ClientNodeId] [nvarchar](32) NOT NULL,
    [Base64Secret] [nvarchar](80) NOT NULL,
    [Name] [nvarchar](100) NOT NULL,
    [Status] [bit] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.ClientNode] PRIMARY KEY ([ClientNodeId])
)
CREATE TABLE [dbo].[CODSettlementSheet] (
    [CODSettlementSheetId] [int] NOT NULL IDENTITY,
    [Waybill] [nvarchar](100),
    [ReceiverAgentId] [nvarchar](max),
    [Amount] [decimal](18, 2) NOT NULL,
    [DateSettled] [datetime],
    [CollectionAgentId] [nvarchar](max),
    [ReceivedCOD] [bit] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.CODSettlementSheet] PRIMARY KEY ([CODSettlementSheetId])
)
CREATE TABLE [dbo].[Company] (
    [CompanyId] [int] NOT NULL IDENTITY,
    [Name] [nvarchar](max),
    [RcNumber] [nvarchar](max),
    [Email] [nvarchar](max),
    [City] [nvarchar](max),
    [State] [nvarchar](max),
    [Address] [nvarchar](max),
    [PhoneNumber] [nvarchar](20),
    [Industry] [nvarchar](max),
    [CompanyType] [int] NOT NULL,
    [CompanyStatus] [int] NOT NULL,
    [CustomerCode] [nvarchar](max),
    [Discount] [decimal](18, 2) NOT NULL,
    [SettlementPeriod] [int] NOT NULL,
    [CustomerCategory] [int] NOT NULL,
    [ReturnOption] [nvarchar](max),
    [ReturnServiceCentre] [int] NOT NULL,
    [ReturnAddress] [nvarchar](max),
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.Company] PRIMARY KEY ([CompanyId])
)
CREATE TABLE [dbo].[CompanyContactPerson] (
    [CompanyContactPersonId] [int] NOT NULL IDENTITY,
    [FirstName] [nvarchar](max),
    [LastName] [nvarchar](max),
    [Email] [nvarchar](max),
    [Designation] [nvarchar](max),
    [PhoneNumber] [nvarchar](max),
    [CompanyId] [int] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.CompanyContactPerson] PRIMARY KEY ([CompanyContactPersonId])
)
CREATE TABLE [dbo].[Country] (
    [CountryId] [int] NOT NULL IDENTITY,
    [CountryName] [nvarchar](max),
    [CountryCode] [nvarchar](max),
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.Country] PRIMARY KEY ([CountryId])
)
CREATE TABLE [dbo].[CountryRouteZoneMap] (
    [CountryRouteZoneMapId] [int] NOT NULL IDENTITY,
    [ZoneId] [int] NOT NULL,
    [DepartureId] [int],
    [DestinationId] [int],
    [Status] [bit] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.CountryRouteZoneMap] PRIMARY KEY ([CountryRouteZoneMapId])
)
CREATE TABLE [dbo].[Zone] (
    [ZoneId] [int] NOT NULL IDENTITY,
    [ZoneName] [nvarchar](max),
    [Status] [bit] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.Zone] PRIMARY KEY ([ZoneId])
)
CREATE TABLE [dbo].[DeliveryOption] (
    [DeliveryOptionId] [int] NOT NULL IDENTITY,
    [Code] [nvarchar](max),
    [Description] [nvarchar](max),
    [IsActive] [bit] NOT NULL,
    [CustomerType] [int] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.DeliveryOption] PRIMARY KEY ([DeliveryOptionId])
)
CREATE TABLE [dbo].[DeliveryOptionPrice] (
    [DeliveryOptionPriceId] [int] NOT NULL IDENTITY,
    [ZoneId] [int] NOT NULL,
    [DeliveryOptionId] [int] NOT NULL,
    [Price] [decimal](18, 2) NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.DeliveryOptionPrice] PRIMARY KEY ([DeliveryOptionPriceId])
)
CREATE TABLE [dbo].[Device] (
    [DeviceId] [int] NOT NULL IDENTITY,
    [Name] [nvarchar](max),
    [SerialNumber] [nvarchar](max),
    [IMEI] [nvarchar](max),
    [IMEI2] [nvarchar](max),
    [HandStarp] [bit] NOT NULL,
    [UsbCable] [bit] NOT NULL,
    [PowerAdapter] [bit] NOT NULL,
    [SimCardNumber] [nvarchar](max),
    [NetworkProvider] [nvarchar](max),
    [Description] [nvarchar](max),
    [Active] [bit] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.Device] PRIMARY KEY ([DeviceId])
)
CREATE TABLE [dbo].[DeviceManagement] (
    [DeviceManagementId] [int] NOT NULL IDENTITY,
    [UserId] [nvarchar](128),
    [DeviceId] [int] NOT NULL,
    [IsActive] [bit] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.DeviceManagement] PRIMARY KEY ([DeviceManagementId])
)
CREATE TABLE [dbo].[AspNetUsers] (
    [Id] [nvarchar](128) NOT NULL,
    [FirstName] [nvarchar](max),
    [LastName] [nvarchar](max),
    [Gender] [int] NOT NULL,
    [Designation] [nvarchar](max),
    [Department] [nvarchar](max),
    [PictureUrl] [nvarchar](max),
    [IsActive] [bit] NOT NULL,
    [Organisation] [nvarchar](max),
    [Status] [int] NOT NULL,
    [UserType] [int] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [UserChannelCode] [nvarchar](max),
    [UserChannelPassword] [nvarchar](max),
    [UserChannelType] [int] NOT NULL,
    [SystemUserId] [nvarchar](max),
    [SystemUserRole] [nvarchar](max),
    [PasswordExpireDate] [datetime] NOT NULL,
    [Email] [nvarchar](256),
    [EmailConfirmed] [bit] NOT NULL,
    [PasswordHash] [nvarchar](max),
    [SecurityStamp] [nvarchar](max),
    [PhoneNumber] [nvarchar](max),
    [PhoneNumberConfirmed] [bit] NOT NULL,
    [TwoFactorEnabled] [bit] NOT NULL,
    [LockoutEndDateUtc] [datetime],
    [LockoutEnabled] [bit] NOT NULL,
    [AccessFailedCount] [int] NOT NULL,
    [UserName] [nvarchar](256) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY ([Id])
)
CREATE TABLE [dbo].[AspNetUserClaims] (
    [Id] [int] NOT NULL IDENTITY,
    [SystemRoleId] [nvarchar](max),
    [UserId] [nvarchar](128) NOT NULL,
    [ClaimType] [nvarchar](max),
    [ClaimValue] [nvarchar](max),
    CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY ([Id])
)
CREATE TABLE [dbo].[AspNetUserLogins] (
    [LoginProvider] [nvarchar](128) NOT NULL,
    [ProviderKey] [nvarchar](128) NOT NULL,
    [UserId] [nvarchar](128) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey], [UserId])
)
CREATE TABLE [dbo].[AspNetUserRoles] (
    [UserId] [nvarchar](128) NOT NULL,
    [RoleId] [nvarchar](128) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId])
)
CREATE TABLE [dbo].[Dispatch] (
    [DispatchId] [int] NOT NULL IDENTITY,
    [RegistrationNumber] [nvarchar](max),
    [ManifestNumber] [nvarchar](50),
    [Amount] [decimal](18, 2) NOT NULL,
    [RescuedDispatchId] [int] NOT NULL,
    [DriverDetail] [nvarchar](max),
    [DispatchedBy] [nvarchar](max),
    [ReceivedBy] [nvarchar](max),
    [DispatchCategory] [int] NOT NULL,
    [ServiceCentreId] [int],
    [DepartureId] [int],
    [DestinationId] [int],
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.Dispatch] PRIMARY KEY ([DispatchId])
)
CREATE TABLE [dbo].[Station] (
    [StationId] [int] NOT NULL IDENTITY,
    [StationName] [nvarchar](max),
    [StationCode] [nvarchar](max),
    [StateId] [int] NOT NULL,
    [SuperServiceCentreId] [int] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.Station] PRIMARY KEY ([StationId])
)
CREATE TABLE [dbo].[ServiceCentre] (
    [ServiceCentreId] [int] NOT NULL IDENTITY,
    [Name] [nvarchar](100),
    [Code] [nvarchar](100),
    [Address] [nvarchar](max),
    [City] [nvarchar](max),
    [PhoneNumber] [nvarchar](max),
    [Email] [nvarchar](max),
    [IsActive] [bit] NOT NULL,
    [StationId] [int] NOT NULL,
    [TargetAmount] [decimal](18, 2) NOT NULL,
    [TargetOrder] [int] NOT NULL,
    [IsDefault] [bit] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.ServiceCentre] PRIMARY KEY ([ServiceCentreId])
)
CREATE TABLE [dbo].[State] (
    [StateId] [int] NOT NULL IDENTITY,
    [StateName] [nvarchar](max),
    [StateCode] [nvarchar](max),
    [CountryId] [int] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.State] PRIMARY KEY ([StateId])
)
CREATE TABLE [dbo].[DispatchActivity] (
    [DispatchActivityId] [int] NOT NULL IDENTITY,
    [DispatchId] [int] NOT NULL,
    [Description] [nvarchar](max),
    [Location] [nvarchar](max),
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.DispatchActivity] PRIMARY KEY ([DispatchActivityId])
)
CREATE TABLE [dbo].[DomesticRouteZoneMap] (
    [DomesticRouteZoneMapId] [int] NOT NULL IDENTITY,
    [ZoneId] [int] NOT NULL,
    [DepartureId] [int],
    [DestinationId] [int],
    [Status] [bit] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.DomesticRouteZoneMap] PRIMARY KEY ([DomesticRouteZoneMapId])
)
CREATE TABLE [dbo].[DomesticZonePrice] (
    [DomesticZonePriceId] [int] NOT NULL IDENTITY,
    [Weight] [decimal](18, 2) NOT NULL,
    [ZoneId] [int] NOT NULL,
    [Price] [decimal](18, 2) NOT NULL,
    [RegularEcommerceType] [int] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.DomesticZonePrice] PRIMARY KEY ([DomesticZonePriceId])
)
CREATE TABLE [dbo].[EmailSendLog] (
    [EmailSendLogId] [int] NOT NULL IDENTITY,
    [To] [nvarchar](max),
    [From] [nvarchar](max),
    [Message] [nvarchar](max),
    [Status] [int] NOT NULL,
    [User] [nvarchar](max),
    [ResultStatus] [nvarchar](max),
    [ResultDescription] [nvarchar](max),
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.EmailSendLog] PRIMARY KEY ([EmailSendLogId])
)
CREATE TABLE [dbo].[Fleet] (
    [FleetId] [int] NOT NULL IDENTITY,
    [RegistrationNumber] [nvarchar](max),
    [ChassisNumber] [nvarchar](max),
    [EngineNumber] [nvarchar](max),
    [Status] [bit] NOT NULL,
    [FleetType] [int] NOT NULL,
    [Capacity] [int] NOT NULL,
    [Description] [nvarchar](max),
    [ModelId] [int] NOT NULL,
    [PartnerId] [int] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    [FleetMake_MakeId] [int],
    CONSTRAINT [PK_dbo.Fleet] PRIMARY KEY ([FleetId])
)
CREATE TABLE [dbo].[FleetModel] (
    [ModelId] [int] NOT NULL IDENTITY,
    [ModelName] [nvarchar](max),
    [MakeId] [int] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.FleetModel] PRIMARY KEY ([ModelId])
)
CREATE TABLE [dbo].[FleetMake] (
    [MakeId] [int] NOT NULL IDENTITY,
    [MakeName] [nvarchar](max),
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.FleetMake] PRIMARY KEY ([MakeId])
)
CREATE TABLE [dbo].[Partner] (
    [PartnerId] [int] NOT NULL IDENTITY,
    [PartnerName] [nvarchar](max),
    [PartnerCode] [nvarchar](max),
    [FirstName] [nvarchar](max),
    [LastName] [nvarchar](max),
    [Email] [nvarchar](max),
    [PhoneNumber] [nvarchar](max),
    [OptionalPhoneNumber] [nvarchar](max),
    [Address] [nvarchar](max),
    [PartnerType] [int] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.Partner] PRIMARY KEY ([PartnerId])
)
CREATE TABLE [dbo].[FleetPart] (
    [PartId] [int] NOT NULL IDENTITY,
    [PartName] [nvarchar](max),
    [ModelId] [int] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.FleetPart] PRIMARY KEY ([PartId])
)
CREATE TABLE [dbo].[FleetPartInventory] (
    [FleetPartInventoryId] [int] NOT NULL IDENTITY,
    [Quantity] [int] NOT NULL,
    [UnitPrice] [decimal](18, 2) NOT NULL,
    [PartId] [int] NOT NULL,
    [StoreId] [int] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.FleetPartInventory] PRIMARY KEY ([FleetPartInventoryId])
)
CREATE TABLE [dbo].[Store] (
    [StoreId] [int] NOT NULL IDENTITY,
    [StoreName] [nvarchar](max),
    [Address] [nvarchar](max),
    [City] [nvarchar](max),
    [State] [nvarchar](max),
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.Store] PRIMARY KEY ([StoreId])
)
CREATE TABLE [dbo].[FleetPartInventoryHistory] (
    [FleetPartInventoryHistoryId] [int] NOT NULL IDENTITY,
    [Quantity] [int] NOT NULL,
    [UnitPrice] [decimal](18, 2) NOT NULL,
    [Remark] [nvarchar](max),
    [InventoryType] [int] NOT NULL,
    [InitialBalance] [decimal](18, 2) NOT NULL,
    [CurrentBalance] [decimal](18, 2) NOT NULL,
    [PartId] [int] NOT NULL,
    [StoreId] [int] NOT NULL,
    [VendorId] [int] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    [MovedBy_Id] [nvarchar](128),
    CONSTRAINT [PK_dbo.FleetPartInventoryHistory] PRIMARY KEY ([FleetPartInventoryHistoryId])
)
CREATE TABLE [dbo].[Vendor] (
    [VendorId] [int] NOT NULL IDENTITY,
    [VendorName] [nvarchar](max),
    [ContactName] [nvarchar](max),
    [Address] [nvarchar](max),
    [Email] [nvarchar](max),
    [PhoneNumber] [nvarchar](max),
    [CompanyRegistrationNumber] [nvarchar](max),
    [BankName] [nvarchar](max),
    [BankAccountNumber] [nvarchar](max),
    [VendorType] [int] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.Vendor] PRIMARY KEY ([VendorId])
)
CREATE TABLE [dbo].[FleetTrip] (
    [FleetTripId] [int] NOT NULL IDENTITY,
    [DepartureDestination] [nvarchar](max),
    [ActualDestination] [nvarchar](max),
    [ExpectedDestination] [nvarchar](max),
    [DepartureTime] [datetime] NOT NULL,
    [ArrivalTime] [datetime] NOT NULL,
    [DistanceTravelled] [decimal](18, 2) NOT NULL,
    [FuelCosts] [decimal](18, 2) NOT NULL,
    [FuelUsed] [decimal](18, 2) NOT NULL,
    [FleetId] [int] NOT NULL,
    [CaptainId] [int] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    [Captain_Id] [nvarchar](128),
    CONSTRAINT [PK_dbo.FleetTrip] PRIMARY KEY ([FleetTripId])
)
CREATE TABLE [dbo].[GeneralLedger] (
    [GeneralLedgerId] [int] NOT NULL IDENTITY,
    [DateOfEntry] [datetime] NOT NULL,
    [ServiceCentreId] [int] NOT NULL,
    [UserId] [nvarchar](max),
    [Amount] [decimal](18, 2) NOT NULL,
    [CreditDebitType] [int] NOT NULL,
    [Description] [nvarchar](max),
    [IsDeferred] [bit] NOT NULL,
    [Waybill] [nvarchar](max),
    [ClientNodeId] [nvarchar](max),
    [PaymentType] [int] NOT NULL,
    [PaymentTypeReference] [nvarchar](max),
    [PaymentServiceType] [int] NOT NULL,
    [IsInternational] [bit] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.GeneralLedger] PRIMARY KEY ([GeneralLedgerId])
)
CREATE TABLE [dbo].[GlobalProperty] (
    [GlobalPropertyId] [int] NOT NULL IDENTITY,
    [Key] [nvarchar](max),
    [Value] [nvarchar](max),
    [Description] [nvarchar](max),
    [IsActive] [bit] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.GlobalProperty] PRIMARY KEY ([GlobalPropertyId])
)
CREATE TABLE [dbo].[GroupWaybillNumber] (
    [GroupWaybillNumberId] [int] NOT NULL IDENTITY,
    [GroupWaybillCode] [nvarchar](100),
    [IsActive] [bit] NOT NULL,
    [UserId] [nvarchar](max),
    [ServiceCentreId] [int] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.GroupWaybillNumber] PRIMARY KEY ([GroupWaybillNumberId])
)
CREATE TABLE [dbo].[GroupWaybillNumberMapping] (
    [GroupWaybillNumberMappingId] [int] NOT NULL IDENTITY,
    [DateMapped] [datetime] NOT NULL,
    [IsActive] [bit] NOT NULL,
    [GroupWaybillNumber] [nvarchar](max),
    [WaybillNumber] [nvarchar](max),
    [DepartureServiceCentreId] [int] NOT NULL,
    [DestinationServiceCentreId] [int] NOT NULL,
    [OriginalDepartureServiceCentreId] [int] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    [DepartureServiceCentre_ServiceCentreId] [int],
    [DestinationServiceCentre_ServiceCentreId] [int],
    [OriginalDepartureServiceCentre_ServiceCentreId] [int],
    CONSTRAINT [PK_dbo.GroupWaybillNumberMapping] PRIMARY KEY ([GroupWaybillNumberMappingId])
)
CREATE TABLE [dbo].[Haulage] (
    [HaulageId] [int] NOT NULL IDENTITY,
    [Tonne] [decimal](18, 2) NOT NULL,
    [Description] [nvarchar](max),
    [Status] [bit] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.Haulage] PRIMARY KEY ([HaulageId])
)
CREATE TABLE [dbo].[HaulageDistanceMapping] (
    [HaulageDistanceMappingId] [int] NOT NULL IDENTITY,
    [Distance] [int] NOT NULL,
    [DepartureId] [int],
    [DestinationId] [int],
    [Status] [bit] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.HaulageDistanceMapping] PRIMARY KEY ([HaulageDistanceMappingId])
)
CREATE TABLE [dbo].[HaulageDistanceMappingPrice] (
    [HaulageDistanceMappingPriceId] [int] NOT NULL IDENTITY,
    [StartRange] [int] NOT NULL,
    [EndRange] [int] NOT NULL,
    [HaulageId] [int] NOT NULL,
    [Price] [decimal](18, 2) NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.HaulageDistanceMappingPrice] PRIMARY KEY ([HaulageDistanceMappingPriceId])
)
CREATE TABLE [dbo].[IdentificationType] (
    [IdentificationTypeId] [int] NOT NULL IDENTITY,
    [IdentificationTypeName] [nvarchar](max),
    [IdentificationTypeDescription] [nvarchar](max),
    CONSTRAINT [PK_dbo.IdentificationType] PRIMARY KEY ([IdentificationTypeId])
)
CREATE TABLE [dbo].[IndividualCustomer] (
    [IndividualCustomerId] [int] NOT NULL IDENTITY,
    [FirstName] [nvarchar](max),
    [LastName] [nvarchar](max),
    [Gender] [int] NOT NULL,
    [Email] [nvarchar](max),
    [City] [nvarchar](max),
    [State] [nvarchar](max),
    [Address] [nvarchar](max),
    [PhoneNumber] [nvarchar](20),
    [PictureUrl] [nvarchar](max),
    [CustomerCode] [nvarchar](max),
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.IndividualCustomer] PRIMARY KEY ([IndividualCustomerId])
)
CREATE TABLE [dbo].[Shipment] (
    [ShipmentId] [int] NOT NULL IDENTITY,
    [SealNumber] [nvarchar](max),
    [Waybill] [nvarchar](100),
    [Value] [decimal](18, 2) NOT NULL,
    [DeliveryTime] [datetime],
    [PaymentStatus] [int] NOT NULL,
    [CustomerType] [nvarchar](max),
    [CustomerId] [int] NOT NULL,
    [CompanyType] [nvarchar](max),
    [CustomerCode] [nvarchar](max),
    [DepartureServiceCentreId] [int] NOT NULL,
    [DestinationServiceCentreId] [int] NOT NULL,
    [ReceiverName] [nvarchar](max),
    [ReceiverPhoneNumber] [nvarchar](max),
    [ReceiverEmail] [nvarchar](max),
    [ReceiverAddress] [nvarchar](max),
    [ReceiverCity] [nvarchar](max),
    [ReceiverState] [nvarchar](max),
    [ReceiverCountry] [nvarchar](max),
    [DeliveryOptionId] [int] NOT NULL,
    [PickupOptions] [int] NOT NULL,
    [ExpectedDateOfArrival] [datetime],
    [ActualDateOfArrival] [datetime],
    [GrandTotal] [decimal](18, 2) NOT NULL,
    [IsCashOnDelivery] [bit] NOT NULL,
    [CashOnDeliveryAmount] [decimal](18, 2),
    [ExpectedAmountToCollect] [decimal](18, 2),
    [ActualAmountCollected] [decimal](18, 2),
    [UserId] [nvarchar](max),
    [IsdeclaredVal] [bit] NOT NULL,
    [DeclarationOfValueCheck] [decimal](18, 2),
    [AppliedDiscount] [decimal](18, 2),
    [DiscountValue] [decimal](18, 2),
    [Insurance] [decimal](18, 2),
    [Vat] [decimal](18, 2),
    [Total] [decimal](18, 2),
    [ShipmentPackagePrice] [decimal](18, 2) NOT NULL,
    [vatvalue_display] [decimal](18, 2),
    [InvoiceDiscountValue_display] [decimal](18, 2),
    [offInvoiceDiscountvalue_display] [decimal](18, 2),
    [IsCancelled] [bit] NOT NULL,
    [IsInternational] [bit] NOT NULL,
    [Description] [nvarchar](max),
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    [DepartureServiceCentre_ServiceCentreId] [int],
    [DestinationServiceCentre_ServiceCentreId] [int],
    [IndividualCustomer_IndividualCustomerId] [int],
    CONSTRAINT [PK_dbo.Shipment] PRIMARY KEY ([ShipmentId])
)
CREATE TABLE [dbo].[ShipmentItem] (
    [ShipmentItemId] [int] NOT NULL IDENTITY,
    [Description] [nvarchar](max),
    [Description_s] [nvarchar](max),
    [ShipmentType] [int] NOT NULL,
    [Weight] [float] NOT NULL,
    [Nature] [nvarchar](max),
    [Price] [decimal](18, 2) NOT NULL,
    [Quantity] [int] NOT NULL,
    [SerialNumber] [int] NOT NULL,
    [IsVolumetric] [bit] NOT NULL,
    [Length] [float] NOT NULL,
    [Width] [float] NOT NULL,
    [Height] [float] NOT NULL,
    [ShipmentId] [int] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.ShipmentItem] PRIMARY KEY ([ShipmentItemId])
)
CREATE TABLE [dbo].[Insurance] (
    [InsuranceId] [int] NOT NULL IDENTITY,
    [Name] [nvarchar](max),
    [Value] [decimal](18, 2) NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.Insurance] PRIMARY KEY ([InsuranceId])
)
CREATE TABLE [dbo].[InternationalRequestReceiverItem] (
    [InternationalRequestReceiverItemId] [int] NOT NULL IDENTITY,
    [InternationalRequestReceiverId] [int] NOT NULL,
    [Description] [nvarchar](max),
    [Quantity] [nvarchar](max),
    [Weight] [nvarchar](max),
    [Width] [nvarchar](max),
    [Length] [nvarchar](max),
    [Height] [nvarchar](max),
    [Value] [nvarchar](max),
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.InternationalRequestReceiverItem] PRIMARY KEY ([InternationalRequestReceiverItemId])
)
CREATE TABLE [dbo].[InternationalRequestReceiver] (
    [InternationalRequestReceiverId] [int] NOT NULL IDENTITY,
    [CustomerId] [nvarchar](max),
    [GenerateCode] [nvarchar](max),
    [Name] [nvarchar](max),
    [Email] [nvarchar](max),
    [PhoneNumber] [nvarchar](max),
    [Address] [nvarchar](max),
    [City] [nvarchar](max),
    [Country] [nvarchar](max),
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.InternationalRequestReceiver] PRIMARY KEY ([InternationalRequestReceiverId])
)
CREATE TABLE [dbo].[Invoice] (
    [InvoiceId] [int] NOT NULL IDENTITY,
    [InvoiceNo] [nvarchar](100),
    [Amount] [decimal](18, 2) NOT NULL,
    [PaymentStatus] [int] NOT NULL,
    [PaymentMethod] [nvarchar](max),
    [PaymentDate] [datetime] NOT NULL,
    [Waybill] [nvarchar](max),
    [DueDate] [datetime] NOT NULL,
    [ServiceCentreId] [int] NOT NULL,
    [IsInternational] [bit] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.Invoice] PRIMARY KEY ([InvoiceId])
)
CREATE TABLE [dbo].[InvoiceShipment] (
    [InvoiceShipmentId] [int] NOT NULL IDENTITY,
    [InvoiceId] [int] NOT NULL,
    [ShipmentId] [int] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.InvoiceShipment] PRIMARY KEY ([InvoiceShipmentId])
)
CREATE TABLE [dbo].[JobCard] (
    [JobCardId] [int] NOT NULL IDENTITY,
    [JobDescription] [nvarchar](max),
    [JobCardType] [int] NOT NULL,
    [JobCardStatus] [int] NOT NULL,
    [MaintenanceType] [int] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    [Approver_Id] [nvarchar](128),
    [Fleet_FleetId] [int],
    [Requester_Id] [nvarchar](128),
    CONSTRAINT [PK_dbo.JobCard] PRIMARY KEY ([JobCardId])
)
CREATE TABLE [dbo].[JobCardManagement] (
    [JobCardManagementId] [int] NOT NULL IDENTITY,
    [SupervisorComment] [nvarchar](max),
    [MechanicComment] [nvarchar](max),
    [EntryDate] [datetime] NOT NULL,
    [ReleaseDate] [datetime] NOT NULL,
    [EstimatedCompletionDate] [datetime] NOT NULL,
    [DateStarted] [datetime] NOT NULL,
    [DateCompleted] [datetime] NOT NULL,
    [JobCardMaintenanceStatus] [int] NOT NULL,
    [WorkshopId] [int] NOT NULL,
    [JobCardId] [int] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    [MechanicSupervisor_Id] [nvarchar](128),
    [MechanicUser_Id] [nvarchar](128),
    CONSTRAINT [PK_dbo.JobCardManagement] PRIMARY KEY ([JobCardManagementId])
)
CREATE TABLE [dbo].[Workshop] (
    [WorkshopId] [int] NOT NULL IDENTITY,
    [WorkshopName] [nvarchar](max),
    [Address] [nvarchar](max),
    [City] [nvarchar](max),
    [State] [nvarchar](max),
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    [WorkshopSupervisor_Id] [nvarchar](128),
    CONSTRAINT [PK_dbo.Workshop] PRIMARY KEY ([WorkshopId])
)
CREATE TABLE [dbo].[JobCardManagementPart] (
    [JobCardManagementPartId] [int] NOT NULL IDENTITY,
    [Quantity] [int] NOT NULL,
    [PartId] [int] NOT NULL,
    [JobCardManagementId] [int] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.JobCardManagementPart] PRIMARY KEY ([JobCardManagementPartId])
)
CREATE TABLE [dbo].[LogEntry] (
    [LogEntryId] [int] NOT NULL IDENTITY,
    [CallSite] [nvarchar](max),
    [DateTime] [nvarchar](max),
    [Level] [nvarchar](max),
    [Logger] [nvarchar](max),
    [MachineName] [nvarchar](max),
    [Username] [nvarchar](max),
    [ErrorSource] [nvarchar](max),
    [ErrorClass] [nvarchar](max),
    [ErrorMethod] [nvarchar](max),
    [ErrorMessage] [nvarchar](max),
    [InnerErrorMessage] [nvarchar](max),
    [Exception] [nvarchar](max),
    [StackTrace] [nvarchar](max),
    [Thread] [nvarchar](max),
    CONSTRAINT [PK_dbo.LogEntry] PRIMARY KEY ([LogEntryId])
)
CREATE TABLE [dbo].[LogVisitReason] (
    [LogVisitReasonId] [int] NOT NULL IDENTITY,
    [Message] [nvarchar](max),
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.LogVisitReason] PRIMARY KEY ([LogVisitReasonId])
)
CREATE TABLE [dbo].[MainNav] (
    [MainNavId] [int] NOT NULL IDENTITY,
    [Name] [nvarchar](max),
    [State] [nvarchar](max),
    [Param] [nvarchar](max),
    [Position] [nvarchar](max),
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.MainNav] PRIMARY KEY ([MainNavId])
)
CREATE TABLE [dbo].[SubNav] (
    [SubNavId] [int] NOT NULL IDENTITY,
    [Title] [nvarchar](max),
    [State] [nvarchar](max),
    [Param] [nvarchar](max),
    [MainNavId] [int] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.SubNav] PRIMARY KEY ([SubNavId])
)
CREATE TABLE [dbo].[SubSubNav] (
    [SubSubNavId] [int] NOT NULL IDENTITY,
    [Title] [nvarchar](max),
    [State] [nvarchar](max),
    [Param] [nvarchar](max),
    [SubNavId] [int] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.SubSubNav] PRIMARY KEY ([SubSubNavId])
)
CREATE TABLE [dbo].[Manifest] (
    [ManifestId] [int] NOT NULL IDENTITY,
    [ManifestCode] [nvarchar](100),
    [DateTime] [datetime] NOT NULL,
    [ShipmentId] [int],
    [DispatchedById] [nvarchar](max),
    [ReceiverById] [nvarchar](max),
    [FleetTripId] [int],
    [IsDispatched] [bit] NOT NULL,
    [IsReceived] [bit] NOT NULL,
    [ServiceCentreId] [int] NOT NULL,
    [ManifestType] [int] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.Manifest] PRIMARY KEY ([ManifestId])
)
CREATE TABLE [dbo].[ManifestGroupWaybillNumberMapping] (
    [ManifestGroupWaybillNumberMappingId] [int] NOT NULL IDENTITY,
    [DateMapped] [datetime] NOT NULL,
    [IsActive] [bit] NOT NULL,
    [ManifestCode] [nvarchar](max),
    [GroupWaybillNumber] [nvarchar](max),
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.ManifestGroupWaybillNumberMapping] PRIMARY KEY ([ManifestGroupWaybillNumberMappingId])
)
CREATE TABLE [dbo].[ManifestVisitMonitoring] (
    [ManifestVisitMonitoringId] [int] NOT NULL IDENTITY,
    [Waybill] [nvarchar](max),
    [Address] [nvarchar](max),
    [ReceiverName] [nvarchar](max),
    [ReceiverPhoneNumber] [nvarchar](max),
    [Status] [nvarchar](max),
    [Signature] [nvarchar](max),
    [UserId] [nvarchar](128),
    [ServiceCentreId] [int] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.ManifestVisitMonitoring] PRIMARY KEY ([ManifestVisitMonitoringId])
)
CREATE TABLE [dbo].[ManifestWaybillMapping] (
    [ManifestWaybillMappingId] [int] NOT NULL IDENTITY,
    [IsActive] [bit] NOT NULL,
    [ManifestCode] [nvarchar](max),
    [Waybill] [nvarchar](max),
    [ServiceCentreId] [int] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.ManifestWaybillMapping] PRIMARY KEY ([ManifestWaybillMappingId])
)
CREATE TABLE [dbo].[Message] (
    [MessageId] [int] NOT NULL IDENTITY,
    [Body] [nvarchar](max),
    [Subject] [nvarchar](max),
    [From] [nvarchar](max),
    [To] [nvarchar](max),
    [EmailSmsType] [int] NOT NULL,
    [MessageType] [int] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.Message] PRIMARY KEY ([MessageId])
)
CREATE TABLE [dbo].[MissingShipment] (
    [MissingShipmentId] [int] NOT NULL IDENTITY,
    [Waybill] [nvarchar](max),
    [SettlementAmount] [float] NOT NULL,
    [Comment] [nvarchar](max),
    [Reason] [nvarchar](max),
    [Status] [nvarchar](max),
    [Feedback] [nvarchar](max),
    [CreatedBy] [nvarchar](max),
    [ResolvedBy] [nvarchar](max),
    [ServiceCentreId] [int] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.MissingShipment] PRIMARY KEY ([MissingShipmentId])
)
CREATE TABLE [dbo].[Notification] (
    [NotificationId] [int] NOT NULL IDENTITY,
    [Subject] [nvarchar](max),
    [Message] [nvarchar](max),
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.Notification] PRIMARY KEY ([NotificationId])
)
CREATE TABLE [dbo].[NumberGeneratorMonitor] (
    [NumberGeneratorMonitorId] [int] NOT NULL IDENTITY,
    [ServiceCentreCode] [nvarchar](max),
    [NumberGeneratorType] [int] NOT NULL,
    [Number] [nvarchar](max),
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.NumberGeneratorMonitor] PRIMARY KEY ([NumberGeneratorMonitorId])
)
CREATE TABLE [dbo].[OverdueShipment] (
    [Waybill] [nvarchar](100) NOT NULL,
    [OverdueShipmentStatus] [int] NOT NULL,
    [UserId] [nvarchar](max),
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.OverdueShipment] PRIMARY KEY ([Waybill])
)
CREATE TABLE [dbo].[PackingList] (
    [PackingListId] [int] NOT NULL IDENTITY,
    [Waybill] [nvarchar](max),
    [Items] [nvarchar](max),
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.PackingList] PRIMARY KEY ([PackingListId])
)
CREATE TABLE [dbo].[PartnerApplication] (
    [PartnerApplicationId] [int] NOT NULL IDENTITY,
    [FirstName] [nvarchar](max),
    [LastName] [nvarchar](max),
    [CompanyName] [nvarchar](max),
    [Email] [nvarchar](max),
    [PhoneNumber] [nvarchar](max),
    [Address] [nvarchar](max),
    [CompanyRcNumber] [nvarchar](max),
    [IdentificationNumber] [nvarchar](max),
    [PartnerType] [int] NOT NULL,
    [TellAboutYou] [nvarchar](max),
    [IsRegistered] [bit] NOT NULL,
    [PartnerApplicationStatus] [int] NOT NULL,
    [IdentificationTypeId] [int],
    [ApproverId] [int],
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    [Approver_Id] [nvarchar](128),
    CONSTRAINT [PK_dbo.PartnerApplication] PRIMARY KEY ([PartnerApplicationId])
)
CREATE TABLE [dbo].[PaymentPartialTransaction] (
    [PaymentPartialTransactionId] [int] NOT NULL IDENTITY,
    [Waybill] [nvarchar](max),
    [TransactionCode] [nvarchar](max),
    [PaymentStatus] [int] NOT NULL,
    [PaymentType] [int] NOT NULL,
    [Amount] [decimal](18, 2) NOT NULL,
    [UserId] [nvarchar](max),
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.PaymentPartialTransaction] PRIMARY KEY ([PaymentPartialTransactionId])
)
CREATE TABLE [dbo].[PaymentTransaction] (
    [PaymentTransactionId] [int] NOT NULL IDENTITY,
    [Waybill] [nvarchar](max),
    [TransactionCode] [nvarchar](max),
    [PaymentStatus] [int] NOT NULL,
    [PaymentTypes] [int] NOT NULL,
    [UserId] [nvarchar](max),
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.PaymentTransaction] PRIMARY KEY ([PaymentTransactionId])
)
CREATE TABLE [dbo].[PreShipment] (
    [PreShipmentId] [int] NOT NULL IDENTITY,
    [SealNumber] [nvarchar](max),
    [Waybill] [nvarchar](100),
    [Value] [decimal](18, 2) NOT NULL,
    [DeliveryTime] [datetime],
    [PaymentStatus] [int] NOT NULL,
    [CustomerType] [nvarchar](max),
    [CustomerId] [int] NOT NULL,
    [CompanyType] [nvarchar](max),
    [CustomerCode] [nvarchar](max),
    [DepartureServiceCentreId] [int] NOT NULL,
    [DestinationServiceCentreId] [int] NOT NULL,
    [ReceiverName] [nvarchar](max),
    [ReceiverPhoneNumber] [nvarchar](max),
    [ReceiverEmail] [nvarchar](max),
    [ReceiverAddress] [nvarchar](max),
    [ReceiverCity] [nvarchar](max),
    [ReceiverState] [nvarchar](max),
    [ReceiverCountry] [nvarchar](max),
    [DeliveryOptionId] [int] NOT NULL,
    [PickupOptions] [int] NOT NULL,
    [ExpectedDateOfArrival] [datetime],
    [ActualDateOfArrival] [datetime],
    [GrandTotal] [decimal](18, 2) NOT NULL,
    [IsCashOnDelivery] [bit] NOT NULL,
    [CashOnDeliveryAmount] [decimal](18, 2),
    [ExpectedAmountToCollect] [decimal](18, 2),
    [ActualAmountCollected] [decimal](18, 2),
    [UserId] [nvarchar](max),
    [IsdeclaredVal] [bit] NOT NULL,
    [DeclarationOfValueCheck] [decimal](18, 2),
    [AppliedDiscount] [decimal](18, 2),
    [DiscountValue] [decimal](18, 2),
    [Insurance] [decimal](18, 2),
    [Vat] [decimal](18, 2),
    [Total] [decimal](18, 2),
    [ShipmentPackagePrice] [decimal](18, 2) NOT NULL,
    [vatvalue_display] [decimal](18, 2),
    [InvoiceDiscountValue_display] [decimal](18, 2),
    [offInvoiceDiscountvalue_display] [decimal](18, 2),
    [IsCancelled] [bit] NOT NULL,
    [IsInternational] [bit] NOT NULL,
    [Description] [nvarchar](max),
    [RequestStatus] [int] NOT NULL,
    [ProcessingStatus] [int] NOT NULL,
    [DepartureStationId] [int] NOT NULL,
    [DestinationStationId] [int] NOT NULL,
    [IsMapped] [bit] NOT NULL,
    [DeclinedReason] [nvarchar](max),
    [CalculatedTotal] [decimal](18, 2),
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    [DepartureServiceCentre_ServiceCentreId] [int],
    [DepartureStation_StationId] [int],
    [DestinationServiceCentre_ServiceCentreId] [int],
    [DestinationStation_StationId] [int],
    CONSTRAINT [PK_dbo.PreShipment] PRIMARY KEY ([PreShipmentId])
)
CREATE TABLE [dbo].[PreShipmentItem] (
    [PreShipmentItemId] [int] NOT NULL IDENTITY,
    [Description] [nvarchar](max),
    [Description_s] [nvarchar](max),
    [ShipmentType] [int] NOT NULL,
    [Weight] [float] NOT NULL,
    [Nature] [nvarchar](max),
    [Price] [decimal](18, 2) NOT NULL,
    [Quantity] [int] NOT NULL,
    [SerialNumber] [int] NOT NULL,
    [IsVolumetric] [bit] NOT NULL,
    [Length] [float] NOT NULL,
    [Width] [float] NOT NULL,
    [Height] [float] NOT NULL,
    [PreShipmentId] [int] NOT NULL,
    [CalculatedPrice] [decimal](18, 2) NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.PreShipmentItem] PRIMARY KEY ([PreShipmentItemId])
)
CREATE TABLE [dbo].[PreShipmentManifestMapping] (
    [PreShipmentManifestMappingId] [int] NOT NULL IDENTITY,
    [ManifestCode] [nvarchar](max),
    [PreShipmentId] [int] NOT NULL,
    [Waybill] [nvarchar](max),
    [IsActive] [bit] NOT NULL,
    [RegistrationNumber] [nvarchar](max),
    [DriverDetail] [nvarchar](max),
    [DispatchedBy] [nvarchar](max),
    [ReceivedBy] [nvarchar](max),
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.PreShipmentManifestMapping] PRIMARY KEY ([PreShipmentManifestMappingId])
)
CREATE TABLE [dbo].[AspNetRoles] (
    [Id] [nvarchar](128) NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [Name] [nvarchar](256) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY ([Id])
)
CREATE TABLE [dbo].[ScanStatus] (
    [ScanStatusId] [int] NOT NULL IDENTITY,
    [Code] [nvarchar](10),
    [Incident] [nvarchar](max),
    [Reason] [nvarchar](max),
    [Comment] [nvarchar](max),
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.ScanStatus] PRIMARY KEY ([ScanStatusId])
)
CREATE TABLE [dbo].[ShipmentCancel] (
    [Waybill] [nvarchar](100) NOT NULL,
    [CreatedBy] [nvarchar](max),
    [ShipmentCreatedDate] [datetime] NOT NULL,
    [CancelledBy] [nvarchar](max),
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.ShipmentCancel] PRIMARY KEY ([Waybill])
)
CREATE TABLE [dbo].[ShipmentCollection] (
    [Waybill] [nvarchar](100) NOT NULL,
    [Name] [nvarchar](max),
    [PhoneNumber] [nvarchar](max),
    [Email] [nvarchar](max),
    [Address] [nvarchar](max),
    [City] [nvarchar](max),
    [State] [nvarchar](max),
    [IndentificationUrl] [nvarchar](max),
    [ShipmentScanStatus] [int] NOT NULL,
    [UserId] [nvarchar](max),
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.ShipmentCollection] PRIMARY KEY ([Waybill])
)
CREATE TABLE [dbo].[ShipmentDeliveryOptionMapping] (
    [ShipmentDeliveryOptionMappingId] [int] NOT NULL IDENTITY,
    [Waybill] [nvarchar](100),
    [DeliveryOptionId] [int] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.ShipmentDeliveryOptionMapping] PRIMARY KEY ([ShipmentDeliveryOptionMappingId])
)
CREATE TABLE [dbo].[ShipmentPackagePrice] (
    [ShipmentPackagePriceId] [int] NOT NULL IDENTITY,
    [Description] [nvarchar](max),
    [Price] [decimal](18, 2) NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.ShipmentPackagePrice] PRIMARY KEY ([ShipmentPackagePriceId])
)
CREATE TABLE [dbo].[ShipmentReroute] (
    [WaybillNew] [nvarchar](100) NOT NULL,
    [WaybillOld] [nvarchar](100),
    [RerouteBy] [nvarchar](max),
    [ShipmentRerouteInitiator] [int] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.ShipmentReroute] PRIMARY KEY ([WaybillNew])
)
CREATE TABLE [dbo].[ShipmentReturn] (
    [WaybillNew] [nvarchar](100) NOT NULL,
    [WaybillOld] [nvarchar](100),
    [Discount] [decimal](18, 2) NOT NULL,
    [OriginalPayment] [decimal](18, 2) NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.ShipmentReturn] PRIMARY KEY ([WaybillNew])
)
CREATE TABLE [dbo].[ShipmentTracking] (
    [ShipmentTrackingId] [int] NOT NULL IDENTITY,
    [Waybill] [nvarchar](max),
    [Location] [nvarchar](max),
    [Status] [nvarchar](max),
    [DateTime] [datetime] NOT NULL,
    [TrackingType] [int] NOT NULL,
    [UserId] [nvarchar](128),
    [ServiceCentreId] [int] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.ShipmentTracking] PRIMARY KEY ([ShipmentTrackingId])
)
CREATE TABLE [dbo].[SLA] (
    [SLAId] [int] NOT NULL IDENTITY,
    [Content] [nvarchar](max),
    [SLAType] [int] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.SLA] PRIMARY KEY ([SLAId])
)
CREATE TABLE [dbo].[SmsSendLog] (
    [SmsSendLogId] [int] NOT NULL IDENTITY,
    [To] [nvarchar](max),
    [From] [nvarchar](max),
    [Message] [nvarchar](max),
    [Status] [int] NOT NULL,
    [User] [nvarchar](max),
    [ResultStatus] [nvarchar](max),
    [ResultDescription] [nvarchar](max),
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.SmsSendLog] PRIMARY KEY ([SmsSendLogId])
)
CREATE TABLE [dbo].[SpecialDomesticPackage] (
    [SpecialDomesticPackageId] [int] NOT NULL IDENTITY,
    [Name] [nvarchar](max),
    [Status] [bit] NOT NULL,
    [Weight] [decimal](18, 2) NOT NULL,
    [SpecialDomesticPackageType] [int] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.SpecialDomesticPackage] PRIMARY KEY ([SpecialDomesticPackageId])
)
CREATE TABLE [dbo].[SpecialDomesticZonePrice] (
    [SpecialDomesticZonePriceId] [int] NOT NULL IDENTITY,
    [Weight] [decimal](18, 2),
    [Price] [decimal](18, 2) NOT NULL,
    [Description] [nvarchar](max),
    [ZoneId] [int] NOT NULL,
    [SpecialDomesticPackageId] [int] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.SpecialDomesticZonePrice] PRIMARY KEY ([SpecialDomesticZonePriceId])
)
CREATE TABLE [dbo].[StockRequest] (
    [StockRequestId] [int] NOT NULL IDENTITY,
    [IsSupplied] [bit] NOT NULL,
    [SourceId] [int] NOT NULL,
    [StockRequestSourceType] [int] NOT NULL,
    [StockRequestStatus] [int] NOT NULL,
    [Remark] [nvarchar](max),
    [VendorAddress] [nvarchar](max),
    [StockRequestDestinationType] [int] NOT NULL,
    [DateIssued] [datetime] NOT NULL,
    [DateReceived] [datetime] NOT NULL,
    [ConveyingFleetId] [int] NOT NULL,
    [Receiver] [nvarchar](max),
    [Requester] [nvarchar](max),
    [Issuer] [nvarchar](max),
    [StockInApprover] [nvarchar](max),
    [StockOutApprover] [nvarchar](max),
    [DestinationId] [int] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    [ConveyingFleet_FleetId] [int],
    [Destination_ServiceCentreId] [int],
    CONSTRAINT [PK_dbo.StockRequest] PRIMARY KEY ([StockRequestId])
)
CREATE TABLE [dbo].[StockRequestPart] (
    [StockRequestPartId] [int] NOT NULL IDENTITY,
    [Quantity] [int] NOT NULL,
    [QuantitySupplied] [int] NOT NULL,
    [UnitPrice] [decimal](18, 2) NOT NULL,
    [SerialNumber] [nvarchar](max),
    [PartId] [int] NOT NULL,
    [StockRequestId] [int] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.StockRequestPart] PRIMARY KEY ([StockRequestPartId])
)
CREATE TABLE [dbo].[StockSupplyDetails] (
    [StockSupplyDetailsId] [int] NOT NULL IDENTITY,
    [InvoiceNumber] [nvarchar](max),
    [LPONumber] [nvarchar](max),
    [WaybillNumber] [nvarchar](max),
    [ScannedInvoiceURL] [nvarchar](max),
    [StockRequestId] [int] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.StockSupplyDetails] PRIMARY KEY ([StockSupplyDetailsId])
)
CREATE TABLE [dbo].[TransitWaybillNumber] (
    [TransitWaybillNumberId] [int] NOT NULL IDENTITY,
    [WaybillNumber] [nvarchar](100),
    [ServiceCentreId] [int] NOT NULL,
    [UserId] [nvarchar](max),
    [IsGrouped] [bit] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.TransitWaybillNumber] PRIMARY KEY ([TransitWaybillNumberId])
)
CREATE TABLE [dbo].[UserServiceCentreMapping] (
    [UserServiceCentreMappingId] [int] NOT NULL IDENTITY,
    [IsActive] [bit] NOT NULL,
    [UserId] [nvarchar](128),
    [ServiceCentreId] [int] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.UserServiceCentreMapping] PRIMARY KEY ([UserServiceCentreMappingId])
)
CREATE TABLE [dbo].[VAT] (
    [VATId] [int] NOT NULL IDENTITY,
    [Name] [nvarchar](max),
    [Type] [int] NOT NULL,
    [Value] [decimal](18, 2) NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.VAT] PRIMARY KEY ([VATId])
)
CREATE TABLE [dbo].[WalletNumber] (
    [WalletNumberId] [int] NOT NULL IDENTITY,
    [WalletPan] [nvarchar](max),
    [IsActive] [bit] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.WalletNumber] PRIMARY KEY ([WalletNumberId])
)
CREATE TABLE [dbo].[WalletPaymentLog] (
    [WalletPaymentLogId] [int] NOT NULL IDENTITY,
    [WalletId] [int] NOT NULL,
    [Reference] [nvarchar](100),
    [Amount] [decimal](18, 2) NOT NULL,
    [TransactionStatus] [nvarchar](max),
    [UserId] [nvarchar](max),
    [IsWalletCredited] [bit] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.WalletPaymentLog] PRIMARY KEY ([WalletPaymentLogId])
)
CREATE TABLE [dbo].[WalletTransaction] (
    [WalletTransactionId] [int] NOT NULL IDENTITY,
    [DateOfEntry] [datetime] NOT NULL,
    [ServiceCentreId] [int] NOT NULL,
    [WalletId] [int] NOT NULL,
    [UserId] [nvarchar](max),
    [Amount] [decimal](18, 2) NOT NULL,
    [CreditDebitType] [int] NOT NULL,
    [Description] [nvarchar](max),
    [IsDeferred] [bit] NOT NULL,
    [Waybill] [nvarchar](max),
    [ClientNodeId] [nvarchar](max),
    [PaymentType] [int] NOT NULL,
    [PaymentTypeReference] [nvarchar](max),
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.WalletTransaction] PRIMARY KEY ([WalletTransactionId])
)
CREATE TABLE [dbo].[WaybillNumber] (
    [WaybillNumberId] [int] NOT NULL IDENTITY,
    [WaybillCode] [nvarchar](100),
    [IsActive] [bit] NOT NULL,
    [UserId] [nvarchar](max),
    [ServiceCentreId] [int] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.WaybillNumber] PRIMARY KEY ([WaybillNumberId])
)
CREATE TABLE [dbo].[WeightLimitPrice] (
    [WeightLimitPriceId] [int] NOT NULL IDENTITY,
    [ZoneId] [int] NOT NULL,
    [Price] [decimal](18, 2) NOT NULL,
    [Weight] [decimal](18, 2) NOT NULL,
    [RegularEcommerceType] [int] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.WeightLimitPrice] PRIMARY KEY ([WeightLimitPriceId])
)
CREATE TABLE [dbo].[WeightLimit] (
    [WeightLimitId] [int] NOT NULL IDENTITY,
    [Weight] [decimal](18, 2) NOT NULL,
    [Status] [bit] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateModified] [datetime] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.WeightLimit] PRIMARY KEY ([WeightLimitId])
)
CREATE INDEX [IX_WalletId] ON [dbo].[CashOnDeliveryAccount]([WalletId])
CREATE INDEX [IX_WalletId] ON [dbo].[CashOnDeliveryBalance]([WalletId])
CREATE UNIQUE INDEX [IX_Waybill] ON [dbo].[CODSettlementSheet]([Waybill])
CREATE UNIQUE INDEX [IX_PhoneNumber] ON [dbo].[Company]([PhoneNumber])
CREATE INDEX [IX_CompanyId] ON [dbo].[CompanyContactPerson]([CompanyId])
CREATE INDEX [IX_ZoneId] ON [dbo].[CountryRouteZoneMap]([ZoneId])
CREATE INDEX [IX_DepartureId] ON [dbo].[CountryRouteZoneMap]([DepartureId])
CREATE INDEX [IX_DestinationId] ON [dbo].[CountryRouteZoneMap]([DestinationId])
CREATE INDEX [IX_ZoneId] ON [dbo].[DeliveryOptionPrice]([ZoneId])
CREATE INDEX [IX_DeliveryOptionId] ON [dbo].[DeliveryOptionPrice]([DeliveryOptionId])
CREATE INDEX [IX_UserId] ON [dbo].[DeviceManagement]([UserId])
CREATE INDEX [IX_DeviceId] ON [dbo].[DeviceManagement]([DeviceId])
CREATE UNIQUE INDEX [UserNameIndex] ON [dbo].[AspNetUsers]([UserName])
CREATE INDEX [IX_UserId] ON [dbo].[AspNetUserClaims]([UserId])
CREATE INDEX [IX_UserId] ON [dbo].[AspNetUserLogins]([UserId])
CREATE INDEX [IX_UserId] ON [dbo].[AspNetUserRoles]([UserId])
CREATE INDEX [IX_RoleId] ON [dbo].[AspNetUserRoles]([RoleId])
CREATE UNIQUE INDEX [IX_ManifestNumber] ON [dbo].[Dispatch]([ManifestNumber])
CREATE INDEX [IX_ServiceCentreId] ON [dbo].[Dispatch]([ServiceCentreId])
CREATE INDEX [IX_DepartureId] ON [dbo].[Dispatch]([DepartureId])
CREATE INDEX [IX_DestinationId] ON [dbo].[Dispatch]([DestinationId])
CREATE INDEX [IX_StateId] ON [dbo].[Station]([StateId])
CREATE UNIQUE INDEX [IX_Name] ON [dbo].[ServiceCentre]([Name])
CREATE UNIQUE INDEX [IX_Code] ON [dbo].[ServiceCentre]([Code])
CREATE INDEX [IX_StationId] ON [dbo].[ServiceCentre]([StationId])
CREATE INDEX [IX_DispatchId] ON [dbo].[DispatchActivity]([DispatchId])
CREATE INDEX [IX_ZoneId] ON [dbo].[DomesticRouteZoneMap]([ZoneId])
CREATE INDEX [IX_DepartureId] ON [dbo].[DomesticRouteZoneMap]([DepartureId])
CREATE INDEX [IX_DestinationId] ON [dbo].[DomesticRouteZoneMap]([DestinationId])
CREATE INDEX [IX_ZoneId] ON [dbo].[DomesticZonePrice]([ZoneId])
CREATE INDEX [IX_ModelId] ON [dbo].[Fleet]([ModelId])
CREATE INDEX [IX_PartnerId] ON [dbo].[Fleet]([PartnerId])
CREATE INDEX [IX_FleetMake_MakeId] ON [dbo].[Fleet]([FleetMake_MakeId])
CREATE INDEX [IX_MakeId] ON [dbo].[FleetModel]([MakeId])
CREATE INDEX [IX_ModelId] ON [dbo].[FleetPart]([ModelId])
CREATE INDEX [IX_PartId] ON [dbo].[FleetPartInventory]([PartId])
CREATE INDEX [IX_StoreId] ON [dbo].[FleetPartInventory]([StoreId])
CREATE INDEX [IX_PartId] ON [dbo].[FleetPartInventoryHistory]([PartId])
CREATE INDEX [IX_StoreId] ON [dbo].[FleetPartInventoryHistory]([StoreId])
CREATE INDEX [IX_VendorId] ON [dbo].[FleetPartInventoryHistory]([VendorId])
CREATE INDEX [IX_MovedBy_Id] ON [dbo].[FleetPartInventoryHistory]([MovedBy_Id])
CREATE INDEX [IX_FleetId] ON [dbo].[FleetTrip]([FleetId])
CREATE INDEX [IX_Captain_Id] ON [dbo].[FleetTrip]([Captain_Id])
CREATE INDEX [IX_ServiceCentreId] ON [dbo].[GeneralLedger]([ServiceCentreId])
CREATE UNIQUE INDEX [IX_GroupWaybillCode] ON [dbo].[GroupWaybillNumber]([GroupWaybillCode])
CREATE INDEX [IX_ServiceCentreId] ON [dbo].[GroupWaybillNumber]([ServiceCentreId])
CREATE INDEX [IX_DepartureServiceCentre_ServiceCentreId] ON [dbo].[GroupWaybillNumberMapping]([DepartureServiceCentre_ServiceCentreId])
CREATE INDEX [IX_DestinationServiceCentre_ServiceCentreId] ON [dbo].[GroupWaybillNumberMapping]([DestinationServiceCentre_ServiceCentreId])
CREATE INDEX [IX_OriginalDepartureServiceCentre_ServiceCentreId] ON [dbo].[GroupWaybillNumberMapping]([OriginalDepartureServiceCentre_ServiceCentreId])
CREATE INDEX [IX_DepartureId] ON [dbo].[HaulageDistanceMapping]([DepartureId])
CREATE INDEX [IX_DestinationId] ON [dbo].[HaulageDistanceMapping]([DestinationId])
CREATE INDEX [IX_HaulageId] ON [dbo].[HaulageDistanceMappingPrice]([HaulageId])
CREATE UNIQUE INDEX [IX_PhoneNumber] ON [dbo].[IndividualCustomer]([PhoneNumber])
CREATE UNIQUE INDEX [IX_Waybill] ON [dbo].[Shipment]([Waybill])
CREATE INDEX [IX_DeliveryOptionId] ON [dbo].[Shipment]([DeliveryOptionId])
CREATE INDEX [IX_DepartureServiceCentre_ServiceCentreId] ON [dbo].[Shipment]([DepartureServiceCentre_ServiceCentreId])
CREATE INDEX [IX_DestinationServiceCentre_ServiceCentreId] ON [dbo].[Shipment]([DestinationServiceCentre_ServiceCentreId])
CREATE INDEX [IX_IndividualCustomer_IndividualCustomerId] ON [dbo].[Shipment]([IndividualCustomer_IndividualCustomerId])
CREATE INDEX [IX_ShipmentId] ON [dbo].[ShipmentItem]([ShipmentId])
CREATE INDEX [IX_InternationalRequestReceiverId] ON [dbo].[InternationalRequestReceiverItem]([InternationalRequestReceiverId])
CREATE UNIQUE INDEX [IX_InvoiceNo] ON [dbo].[Invoice]([InvoiceNo])
CREATE INDEX [IX_Approver_Id] ON [dbo].[JobCard]([Approver_Id])
CREATE INDEX [IX_Fleet_FleetId] ON [dbo].[JobCard]([Fleet_FleetId])
CREATE INDEX [IX_Requester_Id] ON [dbo].[JobCard]([Requester_Id])
CREATE INDEX [IX_WorkshopId] ON [dbo].[JobCardManagement]([WorkshopId])
CREATE INDEX [IX_JobCardId] ON [dbo].[JobCardManagement]([JobCardId])
CREATE INDEX [IX_MechanicSupervisor_Id] ON [dbo].[JobCardManagement]([MechanicSupervisor_Id])
CREATE INDEX [IX_MechanicUser_Id] ON [dbo].[JobCardManagement]([MechanicUser_Id])
CREATE INDEX [IX_WorkshopSupervisor_Id] ON [dbo].[Workshop]([WorkshopSupervisor_Id])
CREATE INDEX [IX_PartId] ON [dbo].[JobCardManagementPart]([PartId])
CREATE INDEX [IX_JobCardManagementId] ON [dbo].[JobCardManagementPart]([JobCardManagementId])
CREATE INDEX [IX_MainNavId] ON [dbo].[SubNav]([MainNavId])
CREATE INDEX [IX_SubNavId] ON [dbo].[SubSubNav]([SubNavId])
CREATE UNIQUE INDEX [IX_ManifestCode] ON [dbo].[Manifest]([ManifestCode])
CREATE INDEX [IX_ShipmentId] ON [dbo].[Manifest]([ShipmentId])
CREATE INDEX [IX_FleetTripId] ON [dbo].[Manifest]([FleetTripId])
CREATE INDEX [IX_UserId] ON [dbo].[ManifestVisitMonitoring]([UserId])
CREATE INDEX [IX_ServiceCentreId] ON [dbo].[ManifestVisitMonitoring]([ServiceCentreId])
CREATE INDEX [IX_ServiceCentreId] ON [dbo].[ManifestWaybillMapping]([ServiceCentreId])
CREATE UNIQUE INDEX [IX_Waybill] ON [dbo].[OverdueShipment]([Waybill])
CREATE INDEX [IX_IdentificationTypeId] ON [dbo].[PartnerApplication]([IdentificationTypeId])
CREATE INDEX [IX_Approver_Id] ON [dbo].[PartnerApplication]([Approver_Id])
CREATE UNIQUE INDEX [IX_Waybill] ON [dbo].[PreShipment]([Waybill])
CREATE INDEX [IX_DeliveryOptionId] ON [dbo].[PreShipment]([DeliveryOptionId])
CREATE INDEX [IX_DepartureServiceCentre_ServiceCentreId] ON [dbo].[PreShipment]([DepartureServiceCentre_ServiceCentreId])
CREATE INDEX [IX_DepartureStation_StationId] ON [dbo].[PreShipment]([DepartureStation_StationId])
CREATE INDEX [IX_DestinationServiceCentre_ServiceCentreId] ON [dbo].[PreShipment]([DestinationServiceCentre_ServiceCentreId])
CREATE INDEX [IX_DestinationStation_StationId] ON [dbo].[PreShipment]([DestinationStation_StationId])
CREATE INDEX [IX_PreShipmentId] ON [dbo].[PreShipmentItem]([PreShipmentId])
CREATE INDEX [IX_PreShipmentId] ON [dbo].[PreShipmentManifestMapping]([PreShipmentId])
CREATE UNIQUE INDEX [RoleNameIndex] ON [dbo].[AspNetRoles]([Name])
CREATE UNIQUE INDEX [IX_Code] ON [dbo].[ScanStatus]([Code])
CREATE UNIQUE INDEX [IX_Waybill] ON [dbo].[ShipmentCancel]([Waybill])
CREATE UNIQUE INDEX [IX_Waybill] ON [dbo].[ShipmentCollection]([Waybill])
CREATE INDEX [IX_Waybill] ON [dbo].[ShipmentDeliveryOptionMapping]([Waybill])
CREATE INDEX [IX_DeliveryOptionId] ON [dbo].[ShipmentDeliveryOptionMapping]([DeliveryOptionId])
CREATE UNIQUE INDEX [IX_WaybillNew] ON [dbo].[ShipmentReroute]([WaybillNew])
CREATE UNIQUE INDEX [IX_WaybillOld] ON [dbo].[ShipmentReroute]([WaybillOld])
CREATE UNIQUE INDEX [IX_WaybillNew] ON [dbo].[ShipmentReturn]([WaybillNew])
CREATE UNIQUE INDEX [IX_WaybillOld] ON [dbo].[ShipmentReturn]([WaybillOld])
CREATE INDEX [IX_UserId] ON [dbo].[ShipmentTracking]([UserId])
CREATE INDEX [IX_ZoneId] ON [dbo].[SpecialDomesticZonePrice]([ZoneId])
CREATE INDEX [IX_SpecialDomesticPackageId] ON [dbo].[SpecialDomesticZonePrice]([SpecialDomesticPackageId])
CREATE INDEX [IX_ConveyingFleet_FleetId] ON [dbo].[StockRequest]([ConveyingFleet_FleetId])
CREATE INDEX [IX_Destination_ServiceCentreId] ON [dbo].[StockRequest]([Destination_ServiceCentreId])
CREATE INDEX [IX_PartId] ON [dbo].[StockRequestPart]([PartId])
CREATE INDEX [IX_StockRequestId] ON [dbo].[StockRequestPart]([StockRequestId])
CREATE INDEX [IX_StockRequestId] ON [dbo].[StockSupplyDetails]([StockRequestId])
CREATE UNIQUE INDEX [IX_WaybillNumber] ON [dbo].[TransitWaybillNumber]([WaybillNumber])
CREATE INDEX [IX_ServiceCentreId] ON [dbo].[TransitWaybillNumber]([ServiceCentreId])
CREATE INDEX [IX_UserId] ON [dbo].[UserServiceCentreMapping]([UserId])
CREATE INDEX [IX_ServiceCentreId] ON [dbo].[UserServiceCentreMapping]([ServiceCentreId])
CREATE INDEX [IX_WalletId] ON [dbo].[WalletPaymentLog]([WalletId])
CREATE UNIQUE INDEX [IX_Reference] ON [dbo].[WalletPaymentLog]([Reference])
CREATE INDEX [IX_ServiceCentreId] ON [dbo].[WalletTransaction]([ServiceCentreId])
CREATE INDEX [IX_WalletId] ON [dbo].[WalletTransaction]([WalletId])
CREATE UNIQUE INDEX [IX_WaybillCode] ON [dbo].[WaybillNumber]([WaybillCode])
CREATE INDEX [IX_ServiceCentreId] ON [dbo].[WaybillNumber]([ServiceCentreId])
CREATE INDEX [IX_ZoneId] ON [dbo].[WeightLimitPrice]([ZoneId])
ALTER TABLE [dbo].[CashOnDeliveryAccount] ADD CONSTRAINT [FK_dbo.CashOnDeliveryAccount_dbo.Wallet_WalletId] FOREIGN KEY ([WalletId]) REFERENCES [dbo].[Wallet] ([WalletId]) ON DELETE CASCADE
ALTER TABLE [dbo].[CashOnDeliveryBalance] ADD CONSTRAINT [FK_dbo.CashOnDeliveryBalance_dbo.Wallet_WalletId] FOREIGN KEY ([WalletId]) REFERENCES [dbo].[Wallet] ([WalletId]) ON DELETE CASCADE
ALTER TABLE [dbo].[CompanyContactPerson] ADD CONSTRAINT [FK_dbo.CompanyContactPerson_dbo.Company_CompanyId] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Company] ([CompanyId]) ON DELETE CASCADE
ALTER TABLE [dbo].[CountryRouteZoneMap] ADD CONSTRAINT [FK_dbo.CountryRouteZoneMap_dbo.Country_DepartureId] FOREIGN KEY ([DepartureId]) REFERENCES [dbo].[Country] ([CountryId])
ALTER TABLE [dbo].[CountryRouteZoneMap] ADD CONSTRAINT [FK_dbo.CountryRouteZoneMap_dbo.Country_DestinationId] FOREIGN KEY ([DestinationId]) REFERENCES [dbo].[Country] ([CountryId])
ALTER TABLE [dbo].[CountryRouteZoneMap] ADD CONSTRAINT [FK_dbo.CountryRouteZoneMap_dbo.Zone_ZoneId] FOREIGN KEY ([ZoneId]) REFERENCES [dbo].[Zone] ([ZoneId]) ON DELETE CASCADE
ALTER TABLE [dbo].[DeliveryOptionPrice] ADD CONSTRAINT [FK_dbo.DeliveryOptionPrice_dbo.DeliveryOption_DeliveryOptionId] FOREIGN KEY ([DeliveryOptionId]) REFERENCES [dbo].[DeliveryOption] ([DeliveryOptionId]) ON DELETE CASCADE
ALTER TABLE [dbo].[DeliveryOptionPrice] ADD CONSTRAINT [FK_dbo.DeliveryOptionPrice_dbo.Zone_ZoneId] FOREIGN KEY ([ZoneId]) REFERENCES [dbo].[Zone] ([ZoneId]) ON DELETE CASCADE
ALTER TABLE [dbo].[DeviceManagement] ADD CONSTRAINT [FK_dbo.DeviceManagement_dbo.Device_DeviceId] FOREIGN KEY ([DeviceId]) REFERENCES [dbo].[Device] ([DeviceId]) ON DELETE CASCADE
ALTER TABLE [dbo].[DeviceManagement] ADD CONSTRAINT [FK_dbo.DeviceManagement_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id])
ALTER TABLE [dbo].[AspNetUserClaims] ADD CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[AspNetUserLogins] ADD CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[AspNetUserRoles] ADD CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[AspNetUserRoles] ADD CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[Dispatch] ADD CONSTRAINT [FK_dbo.Dispatch_dbo.Station_DepartureId] FOREIGN KEY ([DepartureId]) REFERENCES [dbo].[Station] ([StationId])
ALTER TABLE [dbo].[Dispatch] ADD CONSTRAINT [FK_dbo.Dispatch_dbo.Station_DestinationId] FOREIGN KEY ([DestinationId]) REFERENCES [dbo].[Station] ([StationId])
ALTER TABLE [dbo].[Dispatch] ADD CONSTRAINT [FK_dbo.Dispatch_dbo.ServiceCentre_ServiceCentreId] FOREIGN KEY ([ServiceCentreId]) REFERENCES [dbo].[ServiceCentre] ([ServiceCentreId])
ALTER TABLE [dbo].[Station] ADD CONSTRAINT [FK_dbo.Station_dbo.State_StateId] FOREIGN KEY ([StateId]) REFERENCES [dbo].[State] ([StateId]) ON DELETE CASCADE
ALTER TABLE [dbo].[ServiceCentre] ADD CONSTRAINT [FK_dbo.ServiceCentre_dbo.Station_StationId] FOREIGN KEY ([StationId]) REFERENCES [dbo].[Station] ([StationId]) ON DELETE CASCADE
ALTER TABLE [dbo].[DispatchActivity] ADD CONSTRAINT [FK_dbo.DispatchActivity_dbo.Dispatch_DispatchId] FOREIGN KEY ([DispatchId]) REFERENCES [dbo].[Dispatch] ([DispatchId]) ON DELETE CASCADE
ALTER TABLE [dbo].[DomesticRouteZoneMap] ADD CONSTRAINT [FK_dbo.DomesticRouteZoneMap_dbo.Station_DepartureId] FOREIGN KEY ([DepartureId]) REFERENCES [dbo].[Station] ([StationId])
ALTER TABLE [dbo].[DomesticRouteZoneMap] ADD CONSTRAINT [FK_dbo.DomesticRouteZoneMap_dbo.Station_DestinationId] FOREIGN KEY ([DestinationId]) REFERENCES [dbo].[Station] ([StationId])
ALTER TABLE [dbo].[DomesticRouteZoneMap] ADD CONSTRAINT [FK_dbo.DomesticRouteZoneMap_dbo.Zone_ZoneId] FOREIGN KEY ([ZoneId]) REFERENCES [dbo].[Zone] ([ZoneId]) ON DELETE CASCADE
ALTER TABLE [dbo].[DomesticZonePrice] ADD CONSTRAINT [FK_dbo.DomesticZonePrice_dbo.Zone_ZoneId] FOREIGN KEY ([ZoneId]) REFERENCES [dbo].[Zone] ([ZoneId]) ON DELETE CASCADE
ALTER TABLE [dbo].[Fleet] ADD CONSTRAINT [FK_dbo.Fleet_dbo.FleetMake_FleetMake_MakeId] FOREIGN KEY ([FleetMake_MakeId]) REFERENCES [dbo].[FleetMake] ([MakeId])
ALTER TABLE [dbo].[Fleet] ADD CONSTRAINT [FK_dbo.Fleet_dbo.FleetModel_ModelId] FOREIGN KEY ([ModelId]) REFERENCES [dbo].[FleetModel] ([ModelId]) ON DELETE CASCADE
ALTER TABLE [dbo].[Fleet] ADD CONSTRAINT [FK_dbo.Fleet_dbo.Partner_PartnerId] FOREIGN KEY ([PartnerId]) REFERENCES [dbo].[Partner] ([PartnerId]) ON DELETE CASCADE
ALTER TABLE [dbo].[FleetModel] ADD CONSTRAINT [FK_dbo.FleetModel_dbo.FleetMake_MakeId] FOREIGN KEY ([MakeId]) REFERENCES [dbo].[FleetMake] ([MakeId]) ON DELETE CASCADE
ALTER TABLE [dbo].[FleetPart] ADD CONSTRAINT [FK_dbo.FleetPart_dbo.FleetModel_ModelId] FOREIGN KEY ([ModelId]) REFERENCES [dbo].[FleetModel] ([ModelId]) ON DELETE CASCADE
ALTER TABLE [dbo].[FleetPartInventory] ADD CONSTRAINT [FK_dbo.FleetPartInventory_dbo.FleetPart_PartId] FOREIGN KEY ([PartId]) REFERENCES [dbo].[FleetPart] ([PartId]) ON DELETE CASCADE
ALTER TABLE [dbo].[FleetPartInventory] ADD CONSTRAINT [FK_dbo.FleetPartInventory_dbo.Store_StoreId] FOREIGN KEY ([StoreId]) REFERENCES [dbo].[Store] ([StoreId]) ON DELETE CASCADE
ALTER TABLE [dbo].[FleetPartInventoryHistory] ADD CONSTRAINT [FK_dbo.FleetPartInventoryHistory_dbo.FleetPart_PartId] FOREIGN KEY ([PartId]) REFERENCES [dbo].[FleetPart] ([PartId]) ON DELETE CASCADE
ALTER TABLE [dbo].[FleetPartInventoryHistory] ADD CONSTRAINT [FK_dbo.FleetPartInventoryHistory_dbo.AspNetUsers_MovedBy_Id] FOREIGN KEY ([MovedBy_Id]) REFERENCES [dbo].[AspNetUsers] ([Id])
ALTER TABLE [dbo].[FleetPartInventoryHistory] ADD CONSTRAINT [FK_dbo.FleetPartInventoryHistory_dbo.Store_StoreId] FOREIGN KEY ([StoreId]) REFERENCES [dbo].[Store] ([StoreId]) ON DELETE CASCADE
ALTER TABLE [dbo].[FleetPartInventoryHistory] ADD CONSTRAINT [FK_dbo.FleetPartInventoryHistory_dbo.Vendor_VendorId] FOREIGN KEY ([VendorId]) REFERENCES [dbo].[Vendor] ([VendorId]) ON DELETE CASCADE
ALTER TABLE [dbo].[FleetTrip] ADD CONSTRAINT [FK_dbo.FleetTrip_dbo.AspNetUsers_Captain_Id] FOREIGN KEY ([Captain_Id]) REFERENCES [dbo].[AspNetUsers] ([Id])
ALTER TABLE [dbo].[FleetTrip] ADD CONSTRAINT [FK_dbo.FleetTrip_dbo.Fleet_FleetId] FOREIGN KEY ([FleetId]) REFERENCES [dbo].[Fleet] ([FleetId]) ON DELETE CASCADE
ALTER TABLE [dbo].[GeneralLedger] ADD CONSTRAINT [FK_dbo.GeneralLedger_dbo.ServiceCentre_ServiceCentreId] FOREIGN KEY ([ServiceCentreId]) REFERENCES [dbo].[ServiceCentre] ([ServiceCentreId]) ON DELETE CASCADE
ALTER TABLE [dbo].[GroupWaybillNumber] ADD CONSTRAINT [FK_dbo.GroupWaybillNumber_dbo.ServiceCentre_ServiceCentreId] FOREIGN KEY ([ServiceCentreId]) REFERENCES [dbo].[ServiceCentre] ([ServiceCentreId]) ON DELETE CASCADE
ALTER TABLE [dbo].[GroupWaybillNumberMapping] ADD CONSTRAINT [FK_dbo.GroupWaybillNumberMapping_dbo.ServiceCentre_DepartureServiceCentre_ServiceCentreId] FOREIGN KEY ([DepartureServiceCentre_ServiceCentreId]) REFERENCES [dbo].[ServiceCentre] ([ServiceCentreId])
ALTER TABLE [dbo].[GroupWaybillNumberMapping] ADD CONSTRAINT [FK_dbo.GroupWaybillNumberMapping_dbo.ServiceCentre_DestinationServiceCentre_ServiceCentreId] FOREIGN KEY ([DestinationServiceCentre_ServiceCentreId]) REFERENCES [dbo].[ServiceCentre] ([ServiceCentreId])
ALTER TABLE [dbo].[GroupWaybillNumberMapping] ADD CONSTRAINT [FK_dbo.GroupWaybillNumberMapping_dbo.ServiceCentre_OriginalDepartureServiceCentre_ServiceCentreId] FOREIGN KEY ([OriginalDepartureServiceCentre_ServiceCentreId]) REFERENCES [dbo].[ServiceCentre] ([ServiceCentreId])
ALTER TABLE [dbo].[HaulageDistanceMapping] ADD CONSTRAINT [FK_dbo.HaulageDistanceMapping_dbo.Station_DepartureId] FOREIGN KEY ([DepartureId]) REFERENCES [dbo].[Station] ([StationId])
ALTER TABLE [dbo].[HaulageDistanceMapping] ADD CONSTRAINT [FK_dbo.HaulageDistanceMapping_dbo.Station_DestinationId] FOREIGN KEY ([DestinationId]) REFERENCES [dbo].[Station] ([StationId])
ALTER TABLE [dbo].[HaulageDistanceMappingPrice] ADD CONSTRAINT [FK_dbo.HaulageDistanceMappingPrice_dbo.Haulage_HaulageId] FOREIGN KEY ([HaulageId]) REFERENCES [dbo].[Haulage] ([HaulageId]) ON DELETE CASCADE
ALTER TABLE [dbo].[Shipment] ADD CONSTRAINT [FK_dbo.Shipment_dbo.DeliveryOption_DeliveryOptionId] FOREIGN KEY ([DeliveryOptionId]) REFERENCES [dbo].[DeliveryOption] ([DeliveryOptionId]) ON DELETE CASCADE
ALTER TABLE [dbo].[Shipment] ADD CONSTRAINT [FK_dbo.Shipment_dbo.ServiceCentre_DepartureServiceCentre_ServiceCentreId] FOREIGN KEY ([DepartureServiceCentre_ServiceCentreId]) REFERENCES [dbo].[ServiceCentre] ([ServiceCentreId])
ALTER TABLE [dbo].[Shipment] ADD CONSTRAINT [FK_dbo.Shipment_dbo.ServiceCentre_DestinationServiceCentre_ServiceCentreId] FOREIGN KEY ([DestinationServiceCentre_ServiceCentreId]) REFERENCES [dbo].[ServiceCentre] ([ServiceCentreId])
ALTER TABLE [dbo].[Shipment] ADD CONSTRAINT [FK_dbo.Shipment_dbo.IndividualCustomer_IndividualCustomer_IndividualCustomerId] FOREIGN KEY ([IndividualCustomer_IndividualCustomerId]) REFERENCES [dbo].[IndividualCustomer] ([IndividualCustomerId])
ALTER TABLE [dbo].[ShipmentItem] ADD CONSTRAINT [FK_dbo.ShipmentItem_dbo.Shipment_ShipmentId] FOREIGN KEY ([ShipmentId]) REFERENCES [dbo].[Shipment] ([ShipmentId]) ON DELETE CASCADE
ALTER TABLE [dbo].[InternationalRequestReceiverItem] ADD CONSTRAINT [FK_dbo.InternationalRequestReceiverItem_dbo.InternationalRequestReceiver_InternationalRequestReceiverId] FOREIGN KEY ([InternationalRequestReceiverId]) REFERENCES [dbo].[InternationalRequestReceiver] ([InternationalRequestReceiverId]) ON DELETE CASCADE
ALTER TABLE [dbo].[JobCard] ADD CONSTRAINT [FK_dbo.JobCard_dbo.AspNetUsers_Approver_Id] FOREIGN KEY ([Approver_Id]) REFERENCES [dbo].[AspNetUsers] ([Id])
ALTER TABLE [dbo].[JobCard] ADD CONSTRAINT [FK_dbo.JobCard_dbo.Fleet_Fleet_FleetId] FOREIGN KEY ([Fleet_FleetId]) REFERENCES [dbo].[Fleet] ([FleetId])
ALTER TABLE [dbo].[JobCard] ADD CONSTRAINT [FK_dbo.JobCard_dbo.AspNetUsers_Requester_Id] FOREIGN KEY ([Requester_Id]) REFERENCES [dbo].[AspNetUsers] ([Id])
ALTER TABLE [dbo].[JobCardManagement] ADD CONSTRAINT [FK_dbo.JobCardManagement_dbo.JobCard_JobCardId] FOREIGN KEY ([JobCardId]) REFERENCES [dbo].[JobCard] ([JobCardId]) ON DELETE CASCADE
ALTER TABLE [dbo].[JobCardManagement] ADD CONSTRAINT [FK_dbo.JobCardManagement_dbo.AspNetUsers_MechanicSupervisor_Id] FOREIGN KEY ([MechanicSupervisor_Id]) REFERENCES [dbo].[AspNetUsers] ([Id])
ALTER TABLE [dbo].[JobCardManagement] ADD CONSTRAINT [FK_dbo.JobCardManagement_dbo.AspNetUsers_MechanicUser_Id] FOREIGN KEY ([MechanicUser_Id]) REFERENCES [dbo].[AspNetUsers] ([Id])
ALTER TABLE [dbo].[JobCardManagement] ADD CONSTRAINT [FK_dbo.JobCardManagement_dbo.Workshop_WorkshopId] FOREIGN KEY ([WorkshopId]) REFERENCES [dbo].[Workshop] ([WorkshopId]) ON DELETE CASCADE
ALTER TABLE [dbo].[Workshop] ADD CONSTRAINT [FK_dbo.Workshop_dbo.AspNetUsers_WorkshopSupervisor_Id] FOREIGN KEY ([WorkshopSupervisor_Id]) REFERENCES [dbo].[AspNetUsers] ([Id])
ALTER TABLE [dbo].[JobCardManagementPart] ADD CONSTRAINT [FK_dbo.JobCardManagementPart_dbo.FleetPart_PartId] FOREIGN KEY ([PartId]) REFERENCES [dbo].[FleetPart] ([PartId]) ON DELETE CASCADE
ALTER TABLE [dbo].[JobCardManagementPart] ADD CONSTRAINT [FK_dbo.JobCardManagementPart_dbo.JobCardManagement_JobCardManagementId] FOREIGN KEY ([JobCardManagementId]) REFERENCES [dbo].[JobCardManagement] ([JobCardManagementId]) ON DELETE CASCADE
ALTER TABLE [dbo].[SubNav] ADD CONSTRAINT [FK_dbo.SubNav_dbo.MainNav_MainNavId] FOREIGN KEY ([MainNavId]) REFERENCES [dbo].[MainNav] ([MainNavId]) ON DELETE CASCADE
ALTER TABLE [dbo].[SubSubNav] ADD CONSTRAINT [FK_dbo.SubSubNav_dbo.SubNav_SubNavId] FOREIGN KEY ([SubNavId]) REFERENCES [dbo].[SubNav] ([SubNavId]) ON DELETE CASCADE
ALTER TABLE [dbo].[Manifest] ADD CONSTRAINT [FK_dbo.Manifest_dbo.FleetTrip_FleetTripId] FOREIGN KEY ([FleetTripId]) REFERENCES [dbo].[FleetTrip] ([FleetTripId])
ALTER TABLE [dbo].[Manifest] ADD CONSTRAINT [FK_dbo.Manifest_dbo.Shipment_ShipmentId] FOREIGN KEY ([ShipmentId]) REFERENCES [dbo].[Shipment] ([ShipmentId])
ALTER TABLE [dbo].[ManifestVisitMonitoring] ADD CONSTRAINT [FK_dbo.ManifestVisitMonitoring_dbo.ServiceCentre_ServiceCentreId] FOREIGN KEY ([ServiceCentreId]) REFERENCES [dbo].[ServiceCentre] ([ServiceCentreId]) ON DELETE CASCADE
ALTER TABLE [dbo].[ManifestVisitMonitoring] ADD CONSTRAINT [FK_dbo.ManifestVisitMonitoring_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id])
ALTER TABLE [dbo].[ManifestWaybillMapping] ADD CONSTRAINT [FK_dbo.ManifestWaybillMapping_dbo.ServiceCentre_ServiceCentreId] FOREIGN KEY ([ServiceCentreId]) REFERENCES [dbo].[ServiceCentre] ([ServiceCentreId]) ON DELETE CASCADE
ALTER TABLE [dbo].[PartnerApplication] ADD CONSTRAINT [FK_dbo.PartnerApplication_dbo.AspNetUsers_Approver_Id] FOREIGN KEY ([Approver_Id]) REFERENCES [dbo].[AspNetUsers] ([Id])
ALTER TABLE [dbo].[PartnerApplication] ADD CONSTRAINT [FK_dbo.PartnerApplication_dbo.IdentificationType_IdentificationTypeId] FOREIGN KEY ([IdentificationTypeId]) REFERENCES [dbo].[IdentificationType] ([IdentificationTypeId])
ALTER TABLE [dbo].[PreShipment] ADD CONSTRAINT [FK_dbo.PreShipment_dbo.DeliveryOption_DeliveryOptionId] FOREIGN KEY ([DeliveryOptionId]) REFERENCES [dbo].[DeliveryOption] ([DeliveryOptionId]) ON DELETE CASCADE
ALTER TABLE [dbo].[PreShipment] ADD CONSTRAINT [FK_dbo.PreShipment_dbo.ServiceCentre_DepartureServiceCentre_ServiceCentreId] FOREIGN KEY ([DepartureServiceCentre_ServiceCentreId]) REFERENCES [dbo].[ServiceCentre] ([ServiceCentreId])
ALTER TABLE [dbo].[PreShipment] ADD CONSTRAINT [FK_dbo.PreShipment_dbo.Station_DepartureStation_StationId] FOREIGN KEY ([DepartureStation_StationId]) REFERENCES [dbo].[Station] ([StationId])
ALTER TABLE [dbo].[PreShipment] ADD CONSTRAINT [FK_dbo.PreShipment_dbo.ServiceCentre_DestinationServiceCentre_ServiceCentreId] FOREIGN KEY ([DestinationServiceCentre_ServiceCentreId]) REFERENCES [dbo].[ServiceCentre] ([ServiceCentreId])
ALTER TABLE [dbo].[PreShipment] ADD CONSTRAINT [FK_dbo.PreShipment_dbo.Station_DestinationStation_StationId] FOREIGN KEY ([DestinationStation_StationId]) REFERENCES [dbo].[Station] ([StationId])
ALTER TABLE [dbo].[PreShipmentItem] ADD CONSTRAINT [FK_dbo.PreShipmentItem_dbo.PreShipment_PreShipmentId] FOREIGN KEY ([PreShipmentId]) REFERENCES [dbo].[PreShipment] ([PreShipmentId]) ON DELETE CASCADE
ALTER TABLE [dbo].[PreShipmentManifestMapping] ADD CONSTRAINT [FK_dbo.PreShipmentManifestMapping_dbo.PreShipment_PreShipmentId] FOREIGN KEY ([PreShipmentId]) REFERENCES [dbo].[PreShipment] ([PreShipmentId]) ON DELETE CASCADE
ALTER TABLE [dbo].[ShipmentDeliveryOptionMapping] ADD CONSTRAINT [FK_dbo.ShipmentDeliveryOptionMapping_dbo.DeliveryOption_DeliveryOptionId] FOREIGN KEY ([DeliveryOptionId]) REFERENCES [dbo].[DeliveryOption] ([DeliveryOptionId]) ON DELETE CASCADE
ALTER TABLE [dbo].[ShipmentTracking] ADD CONSTRAINT [FK_dbo.ShipmentTracking_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id])
ALTER TABLE [dbo].[SpecialDomesticZonePrice] ADD CONSTRAINT [FK_dbo.SpecialDomesticZonePrice_dbo.SpecialDomesticPackage_SpecialDomesticPackageId] FOREIGN KEY ([SpecialDomesticPackageId]) REFERENCES [dbo].[SpecialDomesticPackage] ([SpecialDomesticPackageId]) ON DELETE CASCADE
ALTER TABLE [dbo].[SpecialDomesticZonePrice] ADD CONSTRAINT [FK_dbo.SpecialDomesticZonePrice_dbo.Zone_ZoneId] FOREIGN KEY ([ZoneId]) REFERENCES [dbo].[Zone] ([ZoneId]) ON DELETE CASCADE
ALTER TABLE [dbo].[StockRequest] ADD CONSTRAINT [FK_dbo.StockRequest_dbo.Fleet_ConveyingFleet_FleetId] FOREIGN KEY ([ConveyingFleet_FleetId]) REFERENCES [dbo].[Fleet] ([FleetId])
ALTER TABLE [dbo].[StockRequest] ADD CONSTRAINT [FK_dbo.StockRequest_dbo.ServiceCentre_Destination_ServiceCentreId] FOREIGN KEY ([Destination_ServiceCentreId]) REFERENCES [dbo].[ServiceCentre] ([ServiceCentreId])
ALTER TABLE [dbo].[StockRequestPart] ADD CONSTRAINT [FK_dbo.StockRequestPart_dbo.FleetPart_PartId] FOREIGN KEY ([PartId]) REFERENCES [dbo].[FleetPart] ([PartId]) ON DELETE CASCADE
ALTER TABLE [dbo].[StockRequestPart] ADD CONSTRAINT [FK_dbo.StockRequestPart_dbo.StockRequest_StockRequestId] FOREIGN KEY ([StockRequestId]) REFERENCES [dbo].[StockRequest] ([StockRequestId]) ON DELETE CASCADE
ALTER TABLE [dbo].[StockSupplyDetails] ADD CONSTRAINT [FK_dbo.StockSupplyDetails_dbo.StockRequest_StockRequestId] FOREIGN KEY ([StockRequestId]) REFERENCES [dbo].[StockRequest] ([StockRequestId]) ON DELETE CASCADE
ALTER TABLE [dbo].[TransitWaybillNumber] ADD CONSTRAINT [FK_dbo.TransitWaybillNumber_dbo.ServiceCentre_ServiceCentreId] FOREIGN KEY ([ServiceCentreId]) REFERENCES [dbo].[ServiceCentre] ([ServiceCentreId]) ON DELETE CASCADE
ALTER TABLE [dbo].[UserServiceCentreMapping] ADD CONSTRAINT [FK_dbo.UserServiceCentreMapping_dbo.ServiceCentre_ServiceCentreId] FOREIGN KEY ([ServiceCentreId]) REFERENCES [dbo].[ServiceCentre] ([ServiceCentreId]) ON DELETE CASCADE
ALTER TABLE [dbo].[UserServiceCentreMapping] ADD CONSTRAINT [FK_dbo.UserServiceCentreMapping_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id])
ALTER TABLE [dbo].[WalletPaymentLog] ADD CONSTRAINT [FK_dbo.WalletPaymentLog_dbo.Wallet_WalletId] FOREIGN KEY ([WalletId]) REFERENCES [dbo].[Wallet] ([WalletId]) ON DELETE CASCADE
ALTER TABLE [dbo].[WalletTransaction] ADD CONSTRAINT [FK_dbo.WalletTransaction_dbo.ServiceCentre_ServiceCentreId] FOREIGN KEY ([ServiceCentreId]) REFERENCES [dbo].[ServiceCentre] ([ServiceCentreId]) ON DELETE CASCADE
ALTER TABLE [dbo].[WalletTransaction] ADD CONSTRAINT [FK_dbo.WalletTransaction_dbo.Wallet_WalletId] FOREIGN KEY ([WalletId]) REFERENCES [dbo].[Wallet] ([WalletId]) ON DELETE CASCADE
ALTER TABLE [dbo].[WaybillNumber] ADD CONSTRAINT [FK_dbo.WaybillNumber_dbo.ServiceCentre_ServiceCentreId] FOREIGN KEY ([ServiceCentreId]) REFERENCES [dbo].[ServiceCentre] ([ServiceCentreId]) ON DELETE CASCADE
ALTER TABLE [dbo].[WeightLimitPrice] ADD CONSTRAINT [FK_dbo.WeightLimitPrice_dbo.Zone_ZoneId] FOREIGN KEY ([ZoneId]) REFERENCES [dbo].[Zone] ([ZoneId]) ON DELETE CASCADE
CREATE TABLE [dbo].[__MigrationHistory] (
    [MigrationId] [nvarchar](150) NOT NULL,
    [ContextKey] [nvarchar](300) NOT NULL,
    [Model] [varbinary](max) NOT NULL,
    [ProductVersion] [nvarchar](32) NOT NULL,
    CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY ([MigrationId], [ContextKey])
)
INSERT [dbo].[__MigrationHistory]([MigrationId], [ContextKey], [Model], [ProductVersion])
VALUES (N'201812230119545_AutomaticMigration', N'GIGLS.Infrastructure.Migrations.Configuration',  0x1F8B0800000000000400ECBDDB72DC38B628F83E11F30F0E3FCD4CF4B1ABDCBB7BF7EEA89A13B224DBEA2D59DE92EC9A7D5E1430134A718B4966934C55294ECC97CDC37CD2FCC200BC82C4C21D0433D38C8E2EA708AC85DBBA616161E1FFFB7FFEDF5FFEFB1F9BE4D533CE8B384B7F7DFDF39B9F5EBFC26994ADE274FDEBEB5DF9F0DFFEF6FABFFF9FFFEBFFF2CBF96AF3C7AB6F6DBD3FD37A04322D7E7DFD5896DBBFBF7D5B448F78838A379B38CAB3227B28DF44D9E62D5A656FDFFDF4D3BFBDFDF9E7B798A0784D70BD7AF5CBCD2E2DE30DAEFE207F9E666984B7E50E2557D90A2745F39D94DC56585F7D461B5C6C51847F7DFDF1E2E3E5ED9B8BF421474599EFA27297E3375F68C78A92F41CBF7E7592C488F4EB16270FAF5FA134CD4A54925EFFFD6B816FCB3C4BD7B75BF20125772F5B4CEA3DA0A4C0CD68FEDE57D71DD84FEFE8C0DEF6802DAA685794D9C610E1CF7F6E66EAED18DC6ABE5F773349E6F29CCC79F942475DCDE7AFAF4FA228230B71BBDB6C50FEF2FAD5B8C9BF9F2639ADDE4EFA6946A6FA8CD488D337BFA124C1E59B218A3FBDEA2BFEA9A3164254F47F7F7A75BA4BE86AFD9AE25D99A3E44FAFBEECBE2771F4EFF8E52E7BC2E9AFE92E49D81E933E93B2C107F2E94B9E6D715EBEDCE007701C17ABD7AFDE0EB1BC1DA3E9900831D4E3BE48CB3FBF7BFDEA33E918FA9EE08E589839BA2DC9603FE214E7A8C4AB2FA82C714ED6FA6285ABE9E6FA326AF93D4A5045B57583671999120CB428C7D2F49FE2683151F27F33F86E88F38C8CE734C774545DEFC81F77846FAD7011D68E1F620FC82E8A334C68AFC7F43ECB128C52634437D9EF0D8D7698E2B4E2042A9076794EE4C90B9548BFBEFE10FF41DBBB427F5CE2745D3EFEFAFA6FAF5F551FDBBF9B162D48E534DB6C77D570D8EEFEF2B6E7571D2EBECB515AA0A81615F69CCCA099979B998E3870F4084B28AE3ED9D0C61D999A32C8F5C3605D1D39E733CA73C46222FA9028FA015D939F30657F4DE3A8E205A275952D31BD262B8B73CCC8B8C91A5DA4E00F2C0577AB98727B9C9C3F63CA7A2622B0027E33423193F8ABDAB691781D602821779116E42FBCA224CC93B31CF69218EF5FB72B64094EEA21CFE2449BD24E51F1789D12E68BC9B6E9A5912D562A17C43413D9817DB1214321A2506479868B288FB741D4DC48CFE328DEA0E4F5AB2F39F9D5ECA88984BC8D101D29346C397AA27888483AC3DF635EA9716586B86B1A542F8C1C0BD953E73D8EC9E6F9F4FAEC9650C7AE184E41FF75D1EA87A4D549773FA3E7785D818274F9FAD50D4EAAE2E231DEB6EB0D8996FB16E0439E6D6EB24424829A7AF7B7D92EAF6CD14CA3F21DCAD714B795A2683B66A119EA7F665205BD643095FDBA32C59FB0AF5BFCBCDB7CC7F9E45268ECABF12EEE2B426105AA9D506EF1F03A6350602C9A6E5F8A126F5C2553DB895332EFD32B0E227F50FA329889A9DA5A94CA616C158712BF636A6703BEC1B417067CD31777039E4134974BDCB7983D28CB77112A876FA93604ADB45487F51496EAA8B293A57A9AC484353F570AD9440CF6707309BDAE0356926E002D64642A1C34F8D890ECDFA302FFF55F6E7194E352D2F8DF7E9AA271FA5F49A33FFF3449ABC32DBCADBC5824E2819859D767B7B82C13BC214C76FB884DB7C2EF51FAD42378C3A39B4BE6701DB1923D209670DBE697EF3119BEBB085050328E30D55627EBC1D9C4A1FA63A968A8570D9016AA8D2851D3D57968A8B968E67E45286D11B93F88C8ADBD1D2A39FB06B0E46AC8B9446ADDBA951CED4143094F85F1E4897BA3404ECD73B2FE3245E0C90F57CDEBC48D50FB324094C96A95E3A298BC9D2F8F598A9544F0CE879ABE48574460E4D3AF10E48EADF6CBEC77537F720D0B9D0F0E4BF6D94F7D1617D1A4A64B6F6C7EC1799C793A6138250CB7CE18C2614F19FA4253658C899649AFC31CE1D78DDDE2FC398EF029999F1CBBCD4D8D309494584CB3BDF50F36E287F4B2441165BB224B0BD85D08D4BCEF8C39C65D28A9D779003B77A1AC72EB5B7431330798ED6DCE019A790DD041571CAC510E4F28D3F4439C176510FB9406F00569288C7D7A868B789D86898DD633ECBC1A5CAE875D8B96D9772D339D5EE18EA1749490A55E2116B0E945B4066836C551B56EA72B3AD050EAA1693288DC6EDA0AB37B5AC4D381F8272B9AB8C9088EFF4194E015DA5A313B8B605EC6677BE22004C668420904DAA4B36D80B728A773CC21529A5C655C9B5C8690CB39F211080D994DD39194C0AAE178E69E8160CD1A4945C0AE91D5860C1BF9103AEA36180403A31A46575573207D7DD3A1501CDA63A82BCB3B4FFFD5EA7555D1C9A4ACBB63A26228C44C3AA595C6A64A444F8AFBD51A416CC845D01F81A0D7E6D536B2B175FC1B3B1487086662E261276CD899C7106E7F1862B316F07EE64571129564327DDD59E10E2E3FC4099962A77B358B783A44F1F4258F4D2F8D0008F64246553D7117541D9AC3DABC9AC95A8567BF268A09A30F1741B1A71BD6A1E9026C960066B91F83F5DB26756D6E03A501E26DE707B535DEF989EA6875DC7DE777869F2D243485299A7F6713CECFD6F2F839B0080EB30BC4798C924007A61757E717411A7937792B9F50BA223BE87CEB2A96BF16DF4F5195C1CC0DCF97EC779C9FACD0B6EC17D216D76DBC3945F92A10557CC6E5EF59FE44BE3EC7AB00ED05CD61E36587B4182787B28BA13AE20AA5685D4583BA28C81ECBACAAB2EF86BDD21CE208A53E9577C87F7E27201C4369A2671784F1A52C92628FB731B5CD0C5AFC4336B96FEBB2063F5805B0F7E17AA6BB13CA3D7A7DAD6B4A7A4A2BA8FB59D572DA94D41D31766C53B09944AC8D48F522D20C79EE08235F0947B3B626A5ECF693B1972B5C746B7D645E9B165307D2C655F6FFAFF9F4F1C1BE94DF75BE46695C84598AE1F1A97D9A1BEED4A3FFB8E87E60BE4E1F896CC74990D334A6BD2FA828C82E75FA9BE54C9B20690CCA4CF7F5558EB940C995FAC6A8C69F5E5E350B74FEC736CE319CF7D870BA54B723DEFDE5AFBEEE6010DBF521CE37EE2CD24EC327543C06701E12839B186144166EB6D3AF70C0AB1D4C5BDE96E6EEF7EC038AC876E23CA550CEF82EB3E829DB95E76995E5FB6B19F104AF89C04B774EA20817C50742CC7875CA5EB7B5D78D0A4B5097FF542D8BAF7F2428DEC0B70AABED4A5BDE6F7E98CFDC96872D33DD8E5D66EB5870BFB142DB968F7A527F867BD29499F684629074A4291EF5A3FA0A77A32E72DAF89D6CB79566A453ABE172BBBE39EFDE2660200F720F38BD1BAD56E3749502D80CDE7C76A62148940006E6D6643777684BDF50B2F3DD9436B7B40B4FE7BA92013296B9EA1E403B29B69F71F9A6857E53E3FD90139CF4F4E60D87F64FAFB4817B767BA7CB6E7FFEF9FBC39FFFF697BFA2D59FFFFA2FF8CF7FB161BDAA9BFDB1D35B35445B99A0D7AADFD2B329838F7A169A1D06C30CDD78181960C52EF546C637B750ACFBCF2C302983555B65614AF573497FA5720B4B716771B14565F468787AD840CD755AD8346F754AC8C086326B6EF03A2ECAFAC1B6407BCA2B94C60FB8503FA8F0171F69A726CED678838B688757FA4BA7F037E6344AED0C9741F251349DC6ABF7D367E56A33450668AA1D1798388A2F344E72C5647132BE8E1AFA1AEBF13BC30FF8205C7601B5A553F8D6295FCA9F2CF3557CDF2F659A002F9542E5B27EDA5F1F1DA55693F5755415E8EDA086B8BFC36A4E9E9BDBF6F96D134BA7019AC9D0695AB7B17318D060DE9B1A4D9850E2BAAD20277355365257A3E37647FE92ABB5E508F658B4CE609D6107F648FE959C5C072B70B901E15A505240697FEB7CBB503769498B77E06B1F96F01D1B163BA5291C6913E3A8AB01FC5CA27CCCFAC602DD4C7604BB1AE2E77D018520F7D348A854AE417264873CA10E93BAD1576896B6E9A33842AF24DAC4AE8DBA91EB9C71BDDB06759FE1074484D712D57DBC664529DE265ADA13E38D97DCEAB0DE7729B536BFEB9A4D51B7D6BECD8E2BA862AE1A0CB6DBC2819E79D5CCA5B908B1C31662825D91EE7683935BF06EC4E94CACB2462A3EB4391B6BA1673E236BBBE17256C6E20825DCBC1DF604BC807B994581AE662CE26D5FC55B7FA02EF18FB70C75DFD7E65DE45C25A1979CAFE926FFB20D3D25884CB2FB4249DC0034730943A02B560251802794505CB2FC2E326F1F659EFC9015601AC181ABB4262FFEE4D5BD1FC4C2CDC187B28ABABA639920D72FD81297F2495449AFE31E923E35682926AD0C7D1205D4E19859FB74FD70513D0324A1F4CE6F385E3F4EE704F5A1D6A6CDD87783D7BB04E5E751B6D96042FFDC6D49B8C2A2E28E45C529C569C79742593AAC2114A4A36A4E52B43AB7B9C5E9EA325B9B7931AE7051A035D9BD12C8372C9A996428DB051BF139860F2539EFB2C99D0194D2A68F27AEE8218CA779F4F82B4B8A962FC0D6D95A260FBE2D08F18F06306D63219D5B8B723A8CBC701F128C95C9E0006BB9829B49BA576DDB88F50EF088AF729C3EA2A2888B50A11E2911B5A1E24AFC78862A22E02C72E6ABE9055DB4451113C2B3FF070F542825CE3B289497297B296E397A15C8807DD613B24D4CC51215AD805B99BEF8BEFA39387FE50AB9884FBE8669186A4381E2CEDD7735461D6B0BB85DD5B0D46933C5CE9E9D76AD806752B19D883055B19AB2C59F8AAD1A0C12D672859E9628FC631777648D25D28E94DEF7BC0948BC510581D41BD732957CADBC7597CAA00004E5B68310ACE6D4520612D8B94460C3EEC612504B4C781480A4BD20F26F915D7B2EBB1A99E44F7AC1D24124E3BC49AFAE01591795BD73145D9DED68E2076F80E888DADF33492F667B662AC0B47776FE6458D3641031D6B415243EF908B35387B9DC13F2BA52FDFE144A42B619EA6E5943EC9CBB6BF07DD9C31C921D60667CD385B635BE29EC8CEACB567785565C619C0F3E7CB60BE7EEAD05AFF0B3523ABB6FEA8C4CE2BE08B6889972F7BD7CC55FE933618E2A4F93BD5CE990CC798638E888F58122872594F0F98F1D6AEAB908052286CA69C3E1F484B2EAEC2F5B72A01CB1FC638C15990CECF8EC9E01000422504F2C1DA1CAC6D9AEE844E8F6BDA92CED775547A7CF7545C73BD7995DA694329B2F434A2B104C65B6A624F179F19A800731128F2A6349930860716A1FB34A70B03E3FC5853723B4C1B537B668D31F3F2629836CB14CC7F1791B943F4D2E64BAA5E0FC70A312531941262846C97B94A074C2593AAD84443975337BB34DF886D355B644B61DB666F1B9D96824A8EE9E83ABAE61C6F330A63B90ABAC49E56D34A40E4A67404D6583E1B410536EA7DAB634765583AA06C310EFB16483A8E588E1285A209D61D4750DC6D100386D16DB0E1A9B5D35E04C36562FD34D0D2A5D6DE0CF7AAA5B0CB2632482BA445198238C50BBD3E33BA1A61A0EA52F33DC26798FD2A720C4411B3A89229ACF2DD0D06A2EE32C72F6F362F11D92C567E64BB8CB639B34451DEC9CBE02DABEB56FA0050EA5CDBA142B83E42753EB9AA8DCA124648BE77F6C7144E627649BDDD4D652C1514A9CE479FC8C122FB8CE88A2A2CE82BB1C3DE38479C2D7BBD7E0C38EBEB85E94C5A42D7C2DA61C82DEB554E535C49208A8C56B71D03A4CB69D6C5658BC9FA492FDBEAB35DA40B285F08E7150C32AA85BD1B5A60ED4B1FAE29DB85B75B9D3BEB55E82E412AFD6A661DD03D09914FFA00F36CA9F4310CC002050D70FE73451B3B314F0FA248EF2514F4FB6C8B46F021069BD8ACB33FC3DE66FB573657B7C2BBD7A920013F1EB2CE17F432FDFE3647A17C06912133AFC4CEA07A0A22FE865435A0322B999EFC6272E1DEC0D9D7BCC1CEF4C3D90869345E319141B9B0A441E101185EAB87E57725A4CA2BD3589D46F0C0EB49EF8A1414935CE2A91D575B35092EC3B4ADA219A99285FCB38A12F720F71CC65AB0C3A6165AC701842592BF287E53DB91E51B29B5ED08655DE7E1E635A64ED61B8503FE6D96EDBD859ADBFDE6847C5C1CF25AAB88E58892B104B2891C5B61EE4653C5FDC1E6803B6BCA37A3C12CAD11AE4F85462122AEAF276A10AC0CD38E4B05FA1EDB66218E3432C21AEBD11C24D7FFCC8620659489F176DD60B5FFB11B690CA9E58F0866DAD3B0DF32BEFFBF33CAF78AFF3784DF02613F57AD152FBAAA5E005D754578D28BB172191A92F39AC863A5320707841C56D2A4468F4260386369A0E010AD309918B04C3695121D3991C390E8329522072328C3EA15D52E59A37D97F364033D93B4DEB36D60D031ACA96B9CBD2B48F08F17D7C15D24BB53C2D7604BAD55430B4A148F6DB2518D1BCB263D419075102600AB64B6A9A76B5D097270A1739328D8D0E1A5D30E3C08F14AAEA72469412C0F74385C206C1A70AD5B5F547247FAED051CE6B3D03A823E7E77C0B50D2237F123FF8FB80447CE6E50D4AD78E82FF3C5D79C0A26D512BC25A26BDA8BEE88ABDD515DDFE535BB6D6EFF575702AE13AA8AE295D87304EE2B5E6EA87B87EACBE0EC8B2CD89CCE39A49B0F21DB191A730965062946F3DC84D3DBED9E9B6F0FA449AAEE2E778B543C969459AEA7804609FC723998B3AB98E585127882514751E61EA6B32192BE61CAB092FAC3E995A2E41AE281F519AAE60F9B2B56E75BFF31144F3258EA8F0F89A07208486FF83E4BC5F4CD5BD35556F890D4683EBE1C737787D71CF00F456AAAC1E679E4A2BBB25AD6CD0D8E4AD6C40E74A5DD9346F95BD92810DB65FC72870C4C8E4118C8308EC09CEB492F819E72FF0556ABDEB31FCCBD6A312E3246E35FB0D2EDC4CAD709C2F35D7894F82F6398C923CA858A51B1C614AD041ACECB6B1900976DA36C3D8E56D6BA18CDAB6BD20FB81B6B130FB826E68F48A6D3EFDE85AC95EBF19E4EC338EA3A7DDB6C63592F5C312D3ED659B8BA4BA79DDE4F53055444D0A1517141F7394AEEE8815924CA66A2F8A53543C5EA7EDC2B8DAF6436CA657B7F596A5C67A979D6649423EF8425F2F588DBC416D92B4642FAE6A5C142B1C2528C7AB6F1E2EEE56A82A7578FD50597CA78F387AF236DFDB6D42F6A5677111F9A492169FA189AA98D7B4D8E566D97B5506B4B7011B0A08C55EA5D91F7D41D1135AE3694FE59E51F94C57E97E1517DB04BDF85BADE78CF47B4009BEDBC81E1E46CD4C3314229F09D9B129A06CD9D9F7B5FE8081878B4F6C6F7D62438B0E748CB532E57E5CB7F78909AA70EE30513DF3A01EED1071A6495524B8A2AA6C34E16F04308DAB2F00282BCBC6E629BCBF73DD9578037B60D91AF7BD7F931FC6A002F7C83C5C0B7A64DED8D94A513A385C29F8DC4E57D20527C76B031F2C4636A49AEADBBA9FDE2DD1CEE8C0BBC7B2415D609AFC09C7EBC7DE36CD08F59823F98CEA38CEA94F1B27B54EFD3CC242645E3C76FFDB61BA28BE65C96E83C96C46AEE6463DEB8EABFC5BBC72C6F1C907B9E91FEA2C66E6819A99BD36F7A7F745260B6C1D58C655757E039398BF0E6CB600AAA67DBBB8290638949A0F72B432F1A9E7227D0EE25EDCC08B7283FFB9231B9CF61C45C7BE1FB3BA1CDB6C1240DE2D3BC1A0C6192CF857D617F763E4605B8EB1913A5D64C9C0509BAE19D6A69C2EE47560FE4ED6CCA7307316281DE2A29EF6D53896093341A8A218E05E25A647C18C2E9838D79B233A271F9D7C167DE9F47DD4E7BE7579403DCE47CA4D797DA1EA649000B720FB99E37BE9EDA85E700F1694B568F643D4ECE273B0BD51EFFC6D062FD682A57AAFA2354C357905349BD2AE5AB7D3CF1D68B82D75D5E4E76CF2DB0713BF0833D5158206FA0A978F59B0074FCE9830626B411CEA4598B31DF6D25FAFD1FBCB7328C7A46D4DF585EE3539506FCC7C516ED40B073D32C7B5396D15B61C0E2F7CDBF3ED3FB2EFA7285F594459359033316BD3BA0D9332A0A198933419F298A3192117EA34F86E48EE0D2C60E68D4A0CF15E115A2A715A3DDF3BEE2F57B608A1431242B2ADFAC9769B6722877B4351F77DA57E8F3D2EE376CD5C057F8FCBB6A8B9A7650705C21E899F959575A7D9DB2BA68AA9C577AB2B1476ADAFE1E43568D05DA114ADB165BA040EC7BC1AA6EF8783AE192209A5756E775BBAB32BB29CF067BD1A132B9E2B1C3D220210AABD73EAF1F5B20726AC8551E1673F7D5E94F186AE18158B44CC93A5F482970256D92C3DA9B3A67B1EB07564DEE96BB19D005432F5AF64F953F1986D5DF728DA86E062651CA895D1ED6EC49AB317CDF75D6D4E83F295449A14A869AAF05B21DA8B6FCDFE4380B2A1F0F535460500D90E90DEF2361C5A0DA233285AD360385575D381B4824873107D75D900DA5A1A9DEFAA3A196D7DB78C6DB5167426138D5504A69699BE12F16790B56D06095738AAC3FD30F95D164DBEB79ABC651D8552EC242254BF97BA926A9CD895D5F5BB59A6B9AB7D6C98299E7DD934D3BE78D938B78842C96A3FF719F57AADB9B9D2F7212C62EE40C55CE522ACE5808E49496BDE333012BB7258556D5C8EEA9B9AC68027507B4000AC6A60BC6DAC37401ECE49A45F66EBF33A08D1E4F8BB859A4966B7CDDB886916365810334A92DB389039C826559DF082CD339E3E5C88ACD53A4050F1158A1EE334CCEB1074D79E0689FDCEF32CEF444A80B64E131460EF56351528C2AE69AB28D07AFA39BC48897409DAE2F91F110EF6AE68F47497A300A478F748CC44DFA461A24DBFC5455CDE6054D07935D4A90CEC7C9A95E984A57E1D6108A56543F1CDB219398C40317A6C47AC7D0D2EBCBE396FB9B0019A89FD9AD66DF88E010DC570410CA6307E54B2B5429BE95BC988600C92A86C1151FBEA2FB9DD7D276582FC8655D97D27B898044783123EA3E1B0D82D956185CB4C6AD63073652EAC1AB7CA59D8418612997771992C32D36C73AEA9D7168978A012B113772E1291CBF5060B4C0329AD10D44D03ADB41C746C500609EB610557716D29B1E716DA4E727B11DDFB2FBA75D76891DC072AB95BC1E32A2101E10D8B504B3F401A3FE0C22650A1059DCD1D50376FE70FE8618379E09A36151974FC2409189F75D95F26175D2055B41F175B54468F78F59E394C9CFC25AF208D55A7E87779CCC71A2A4562372BAE52F1A26886EC8CC96BB68096C681AB7F6CC1A2E48E45C975BC00EAB976D5EF996ABDA6E34B395D0754B17D3843DE41286F365728EE9E9F7CD92DBA8F79B6DB365947EAD4615768BBAD8499C9999912DDCC8A5BD82F178D2E451A4AD557428634EB45C49C442591F3AE1246D3FAF0942C905B84C5A97FDC3AC258C655C7E057591A9356AC25DB08C9CCF26CD41B172906A00A76A92550B6ABD0EF1707398F9DE3F9E9D1D5DCC99A89D76998878D94AFC5FEFC4E20CECC06E4F5F1F445FDECEB1645FD18A040EADE0B1F04D40210EE16E450A6FB1BE1AD5B5173E33BB7B27ADA6310DEB735360C1AFDE3B4E319E298D92C1876C6C52AE03185320A0E721712CA925914C9A248862277C8A86A3D22AB2F14C152203751DCC60A1BC9DE1A682E615BB76E255D7BD050E2F47DB60A70C77EF7FDBF70347DD2264ACED3DF5C90E5D5F6F81EC4EDA6E0CE2E8605A61AAFA62EFE4084FDBEE88843D211FA72342E0A42AB76499447C073C9D5612FACE42B8FE2D87C59B7B82C93EABEF12839BFDD8BBEA1D2EDB517B18EC341F501E3D577143D4DDE50237ADF4FAFC26F709125CF419A5A36313FA082FA9C95645E23545FBB31D14E2CE44CAA89ED828D5E1AC387524AA1ECF2E5C6E7C2EC0366AF8EA31AB459DE386F0DD91EC4319700003B63250A849882090556FD8679637138666E830A961B32CE12FBB1482046025D3FE37CB533795488B99333029E2B4569BBA3341531EAADA86EE0B721E98CE60D48E42DA861D88EF2D47EE1F51F89D7BF909D38A180CB587DB5646862308033F138D3031B6362047E6CBEAEE695D485CD1736C775425482F264BB4DACFC080D3C3D437DC3E39A4D008C3B622707202CA1C4C187382FCA20318F97285043945451FA12A4ADE57178C755BA89028DAB6688D69117A8D186B7B94DF3E0BBA142B8C34972F23DDB95FF99EDA69FB5E206AF63FA4298BB26E3051DF878B4A892A9021EAC376DC1F01A66FB909CE98DD6C522D9D7C02CE9E3833CE181EF104AAA711158B2BAA6E1BB3C39EB8E0282948E8707D0191900E5145CD6BCCC4E5B8A517297A3B440918DE12840339BCD28E88F9DE92841766C1B4A6678411CDFCDE4823AEAC5C101D640033601F3DD10E7289C0447F1069115F992935F155DBFFE9908DEDB08518CE6E7F48BCFEE47509DA6A2D95526EF8D30F6238517F17B68E217446B257F1701B908485640E6DA879740BE2C067A2EB9D8F7C04A200EC1C3C546A0505924FC9DD2CADBF986921D9ECCAC233C4B2FA1BFC009C0E611CFA715B78C5C6693F9209BC65CE3581B5F66D03E0751C267784BF69644C4F80DFBC54549A47AE5DCF389F7E85338B46D863975685B0B9D8123C8CBB26D636152C57643A3DBF47CFAD1B592FD7AABB71D51C8FA387ADA6D6B5C23593F2C31C47BFEC7164744D3539D73FD7092E7F1334A4C15D14954EE50E284E223D9DAACEE8815924CA66A2F8A53543C5EA7EDC2B8DAC9436CA6EE1FBD65A9B1DE65A759923081E8AEE8EB05AB9137A8991D8823F2401BB18B6285A304E578F5AD271ADBB53CAB5055EAF0FAA1B2F84E1F717F3FC979BEA9939E30595C443EA9A4C56768A22AE6352D768419236F08BF216F033614108ABD4AB33FA24160688DBFE4B1C9980D09EC1995CF7495EE5771B14DD08BBFD57ACE48BF0794E0BB8DECE161D4CC344321F299901D91461E52EE122D4BDFA7A40CED433A14511E877962F006FF73476C736853D76FE947954C6D893C8B707DD157DA0A5FCF78DEDAAD4BA91954A5BF69F183F1A218E6FE74D11F718A5781AEE99EA224DA25D443E355222EBECDBD8D9B18EE26E0B8839E73EFC7D5996003612D3EC2405CD53474027662680C0306130D07AAAD181608623FBC1269AECF184035A4BA9EE6609ACAE6C3809D421AC311018A8605D7570C4F00E4324CEDF5E241D443D35A33AEBAE97058577F1DF02F1F0CAD743F381E014732AEC63DFF24ABEBF412D408B1DBF10DC5B0074738A417AEC7380D8A504739218D5EA6ADFB00E9719B19E5C27F860586A6C56F385E3FBAA693F91C266DEFB47BDDFFD8A186C25C4C7322E3E3F161A2AD91FF2D4B761B4C6633723520EB59775CE5DFE295338E4F3EC8CDE89458774B322D712D7B94BDDDA30C6C8A696C10892925B4575C6D903689A7559A63319EF98D9251871CED13005B28532568AA628F3233D83D604F29A1EB7B4879C8DB5B6739753B9CE132C4693BFB125EA8F3E8104D2D3AF31874E648C06AA84F09844C93CAC09C94EAC9764B1B334B26D300CDA42E6D94A297075116B398DB1A4B23CADEFDE5AF5EE655FA7807EC5B6B28F4BEA9D1B3E2A080E3B861A9136391DD547747D7C43AEDF20811F8373D92B99E52EF3A60F596FA003A94E9A97C21D887F99646F10A1F4F92E15039938F5F26EEB5D9A32FBD1A2954877FD849B01A76C934A7EBAD0B9617BB5BA0BAC53326D4D79A7FBA38A165EFB40891319DD5E1AC7A1762996D060FBF08133F96F9E1E55F0A73B7235896A7107739C2DCE1B84807194EBEE601DE0B61B64840B82454BC5C5C5E1499BB221B46DC591D3E4951CDB5C997F5C96ADFAF4218CA1510EA42B2DF8B658B58D8DBB3018D985F29F14BA2804DE038C7A511B09B5F13BCA2632302590C334B3EB62B2E026F8CE71803039758A223967DC662E006E7D9AEB494000DF0BCBBFACFF877EB8D7D051B7A6FDFB47D9D48CF34BDD835CD0205F449362D5EA47119A3EA81256067C7575AA4CC914B1922012CBD8735EC2263F655C698DFC0371CCB751EAF09ED274D06A6C5745984CAB6C94C593D35637199698C62E6ED4BDB0D97AD0B8B637FDC337E8C8BCBAC7D5C2580CB3DC05BCDBD2470940CEDA25334034B6B58E0DB7BAE1B70A7BC8DB4BCB97C24C25B15E527F5B4B5C47A5FD7E45D6A830A42DFD9B0969B93ECF2C470477C7942FF3F971EB93CB1521D3558B8B8BEB40C11A34686C5C9C3EEDB22560E49ACE833ECA6B8C5E9EA32333CCEAB5F09A7EF1566EB373D92B9F8B8EB80153B0FA04371F55D3639435385307923A1DE8B07021E58227408780810CB5C10C20F6423D78D05CD98B0C8FAC390F55B1CC52821129C267A899A333B437B0DC43197D8073B63A502849842A98320818B431164CB0DA3EC22BEDD89F05AF096A9A4DA62ACFE0802EC7F64A95E1806E4C814A0DA0F49D675C7832C1BE00AE6E0341411B3465E043495E862B87AEC6C75D52203F75806CAFC8022BB0DF20C0A78FF5E8483F1199A81F2DE444378D3D48214A1D9986B088D11D25FFAE3A96ABB7949CB2C7A6AF226DBE82E067C2E7DC574C14A478DE043E9A58BE27657E7BF77952635B938CB72661E6A8CBC9D2BA8E2D2127077042836CEC8B341F9D3E42AF41B4E5759B04768D87961F2A34A5789AB67A13E2F8A62E74913B7C97DDCAFBA66E9337E21F3FC21C1D839E154FB064E008758B52A015AAA166DFA662A5ABB48FBE7C843B477BD2B8335C830D062D61EAD593B1427B069C748D5FB717DC6B01357E3CD3A495D87E4DDEAEE0F2A0BFACED491779CADE8CD14A5EFC63B9AA314C51E98A4B41BAE66698B239469EA2753718B656CE6DA612352BB9CD6F90265569EF00D699D25D5370817E574B4CAA95208B5405408765AE99EA90E8BF6612DA9701F5535554A43E7824EE7871092FE0F5490720883DAEE4AAA92682F75F655656A39919A1A209953510D3A62ADAA382CC1FC28F51B6C81E4F6E597EBB00F79076A8D26EA48F1AA99CCAF379741DD198BF63A5AEDA5A70306D243AE05C455613D20A9EFA409EE7294167139625393001608C34C8A00EA8A8D2A10E1097C9B452934FD5C9BF37AFB21D8FBC01FF36CE7E36DC745E6EEADCC55BE5207F1E9BDF08D3A756D4EF46A8038495FCA2D03749A99A0006B5C846A26512CEA8E8D3896E10A77CEE9E7958DE546DB222383CA4811F388E5A41E04272B35C14CBD2FC23B7BC2F6C677F7A415F587E17E97EFDBC99D99694D006612DFA4651B49DD801D55883717994006691585F00D25BBE9FCFE8B903E8C90EBDF504266C366A35D43BE6111CC968BA6EF828D9818C387DB57D376BFA0E9C3927D998B0B571F12573779818CEFFE0E38BB47322B77F7DDB0E7F0218EB05CEE1E4DF78029EDC90C0C3FFEB793CDA449AB2A1F068ADA47E8035C980DE603AC579AC8C755EC41122DB2766FB7B9F54283DBC0B1A4B96FEBF61B4041156EEB27AAE7B4E9AB71304CE8A01A182CB3EA06A61FF6CA6184249476A05C78FD4096297F71664CAF5E363F6A2B90EC9D5867D522FD0C7F8F4B6EFBCD95EDF1ED442AB9891991BB8BEE5009EE4E939850F367523F001535A2965BE2C177C3896260754CB82561C7919B0E6A0F39A78EC4AE714555814521AE6FEA0C57DA416C53224388AFA3D16F3FA6907590C93E4497388795CC1D4FA27CCED6CB7BB6A10E4A3DC5EF2D07A58B1A6804A856148949F8C8647123753690CB78D3DD263112A623E8B9E4E9A81B562215C0114AAAFA483D32ED5DA089F34ADDE0F52E41F979449FFC866EDAC31516A17A2C4255984164CC965CE610B0022F3FC15ABE24A7B5D09C5F5E3A8ACA7D4E5C657C7BD04702BE45C6ECE931EB6EC3F0EE49543DEF526B918BE24382D645D7B6162F5384C51B068F0F56261B8E15CE9397E6918396A986CB7185A90DD85FF622CC93E52F4D4F5EBFAA62837E7DFD13B78C0338B2F834ADFC18EA6739D415115F254E511AE131E43B7E1DEA1997AC02E77F75588911AE7956A3EE84EE1A549D154DBDDE045E9FB552CB65EA5A2CF34CDAD7749B67112E0ACACB7A33F705A7AB6A13AF47B65F38FC56D45ACD28CEDD49954134139D12D989D217DDE9BE20B3FD1CAF762869BBEE46B575EBEEF3D8E3996B1AF36D46B592EE4476DB175DCAADCF50DCC8B69E251F7282C534CF8CDFEE8A2DE17D7D49D17A103505C548AEB88889534217EB8C9A451E44458B6C9E59FF9CE595A5AD299C73BC89771BDD39FF98256E72F9439C108BD0513AD3371AEB29E7D1CD65D90985AE620178B1A458025E2CD9ACC347CA98B913B9D728E699EF2BB26DD49DE10F78C3D6B65182F450C25903B648E699B1C627A73B69B72F45898562417BD24E1F696286C4CBDC31B8E699C2F3CD36C95EB036E1F9676D03B1F36785DC47799932D5FFC56281CFE2628BCAE8D18BF21C233B082EA94ECA930CAD9C180576973BCC2584F020E6D3D8E4AE9F66FE90E516AC433D53D54C32D692826B06408C63F5CF3F89184917C3BFF618FE6287A13AC66F50FCD500C55916ED36ECBEE55F2D48187A40CA81807974F390EF071427FADB97DB5D443D170FBBC4491C5419CB9C654087659E99BBCB77D193EEC47DA38E6D3D86BF4549969E321245C1E657711A0FFBA260F1ABACCCF2E825622C46054B7FC38F315B5DC1BFEF3354EA32EAFB78D8131BD66C34BD334131780EC481D35B474EFCD879ED9DA7708069269F4C9DF25A5BA43519AB9D26B04E34EF3C7B3D9A9944DA639CAF281718B8623F65BBC26DEB3988897662DF0ECF4CEC8B8A476DAF5456E82A84EB3489536DABAF8D8DD5D3059579FF20DE25419BAA9891360A4DF01BAAD3F6C3BAC0843A9A10335F44C2A09B494A350F6A6BEFB4096D5DA76738A113FAA24B3AED9653DF9628229C2428C5D9AED0A5A133BCD9E5D50B410E3BED765DDC2DEB01A6791677ECB757C902140BB7D70A7B5DB19EA7F4683C6101FE6CB33671F4B4DB5E57178B1CD786C534CFDA7CBABE3A3F3BBFBCF8767EF39FDA36C2F9CDB78BD3F3D3F3CF77E7374EBAAE657B6739C6229A4982D58F5EE9CEE1D84F12E40CE01FD977B2935A39CF36836726CB8C54FB59DB67472ABF7322D366BC1EA4F100D35C1B02DAB6F6ECD5D5DDE68F094C72A6BD11AE1F89FE98A1FB23450EE90F449569FC800B77E5C3229AE9A0EA8FCABFABAD7D1A7FB0D04DC2DBB42343DB46FB9C6F88B176BB299CE79B4534D7C120E98136AD5EDDEACEF34992384D71ED46F720647B3CF3EE07AB58ED2A96596FAEE9B17113FBCA81AA82B96EEE74F7117737D7BABBC113A6AAC29970767BAAEB4838BBEBED6E8547F9E4B61FD6BFAA3AD057FD9B6A585FBAAAFFA6A8FAA5C7FAB32AE6F64B3F053FAB78E5E6435F57B16027677D55C5825DFF3BD35DC58A7DBCEE67E16795EF87199962C9AEFF9DA9AB58B3DB5B6607A658B45B86C07E56ACDAED4D8FF79D4AC0DDF478DFA956ED53BF14EF14ABF625C74239F0CE66E75EDFCF6C2E2B78705003F8E61197A30BF79AA17D34B13E0CA9F28B35068FFE297AF54C8BAE201CF96C158CD50561D25426FC019082D75868FEBC49C17C2C30BF435770E3382047C1905C2CD258966A71C035B1E256BB8EAD3C6C23408C735D55E8DE8AD0B316C6D5ED8E6BAA653CA18F0646884DBEE77EF4CA213D08FFED4D970949D3E8AD1FC7D5F6E2DEE0FFC251E9EAC4ED75CBE8ED6E976513209D292E1CFFAE1F8F91E8FBDCCF70444FE0DCC2C299A96AEEFF9081F95D8231DE79566138B5CA005222E571A9CF3B739C7F74A23E423E641D8F6EA6E36A66FFA7F2E8319B3AE52EE55A7775D8FD9FEA8C91D9FF298C2976FFA7DA55325555BB4A66FFA73092D8FD9FC22462B77FAA5D25BBFB53DD3262767FAAE00276F7A758AFC1EE4FB160ECEE4FB16083DD9F62C506BB3FC5920D767FAA5DE58DC1AEF2467F57F9E5E67CE066519D04DF9CB3BDF6B9B73CB963EAAA7C38673DADBDB339CDEF8D026270966497129731DD307A919B63A4B3F9EF1F1EB4D59C8F7BAA77398A9E9A0E3A4D248B68AECB64EFB35DAA6D255CEFCA617DABA3F7CB13F753F71AC78144875ADC3528A8B524748BE8CD731D177046E8BD28E3E80BA1351F1E7A31DA43B8843A0E96B02260E669D43A9F90FBA482287F8823E6C1D03D18F51CBA1FE658991DFA1965CEFADE8D57E21CE19D676E479DD09EE4119CDB6C776F74B9C499D7387E88641F27459111D14B67A4730AB0A1B4CDE1699B5378D893F374F5AA4ED82603EAB3BBBDF4B9DCA1FAAF5F5D91398DA98F332E5F7E7DFD7F7043D76CB04D14C734D89E210C5BF8793820D242851397F8D54995FEB86A28422B3ECD1699CDD5F04B93759C06809F66694188224ECB31FE2F799C46F1162506E318E178257E9F814F0F477BDAB5392E39C3557A8EB4345845D7CE746D8EA653357BBFBC6548D58482DFA384BAB9CC287808A4A6E0A6BE3D058F1A3C580A86C7311305C3AB7808145C2B0182A64451F985A89E2CBD6F358390A02440200503F58D0858D61E40C09D66DB3B0AD618880ED13420AE24ACB18CCEBD0942C33BFAD6CE0D7546D1F4AD57687B4F9A4539B59A24442C8182A99803302362597B2015EFEA2784866DFCF4E60DEF3DB0A4468D1EE9114085C8991C351644A73B1DD09E5164B7F930A4C90E2E1855F62DEE0F5D727D9A9B32B965D1A3CD0E6CAFA8B3CA9E6D4224C36CDA93D2E3202537D3549DF17BFF74BC7C043A54D2E6FDF74CB3EC9AB975230095B6A6757D49B14ECE3EFC26A457352844B9009409E56A340AD0F068447B47CDFAA3D2937E2CA42385EBAFB2AFAECD44F552D92C020844E187259B552308249B556B7600B299266DB842295A631A09705F7F901029581FA6D161553302859B01E5EE3340FDFB40A1D221E849B267F0A923431295AE986B3F02D028BD90757F9AA0785308E992A903D1222D9E9444E09E00E47AB2DD5679496905354758511E30173AABEC4867C0B8755A6D9FEE9B8FB22EB3759C2A28ABAEB30F94D5F404A0ACF6911D5AADAA3525790D272414790D077F18E445BBAFA0AEAACA3E1057DD11056DD14A5392D660364251D660E47B4F589C4EAF0845DB98AA463CB5C5563502901244D3FE3C8ED26E042026E95CEF3D5D8D5E282DE53E6EB0364457A3F751F5890A6E0120AAB6AB7BB70B908E40871E1A1047B2942E95733742502669BFEBB8589D0EAB81B45856E1BE4174EAA837FA74EB499DC2B3A1BBDCAE1B4F78F48EAD8750AE4D86478D9367BE2AA856BB949106EA94C7AC4F3E1ED5A9B01BE164977892F57C17731F2133FD579F1B439527A029E9B17048AAB23A0BF64E57077B00DC8D616862292940F014BD27FA821FAFD7350827A033B0435A94C602FAA23770F23DF52620CD550FEF9115BBEF684545181C848CF2DACA3614C837041D1C08487CFE4D8372185AE2A981F144B5C2B573EF4B089A6DEEB8194616CAC140EA05208C2858DEE22CDA5AAB4B01F5B6D6A21C886D088F45C34E540006A4CD996D49CD4ECD4E9F876B6342E39147B288208251E58105B3A886A0432A3EA25954EBE6D68F80B44A3BA11372055697516957D3864447CD1C1C7DC2FD0F4C9CF05AED3D65564FEB5DA1277C5FFFCA563811FBB1C1DA105D761503B9B4E18E0194DC174FE4DC964E910E395058479A94CE865B1FC293A436359A112268EA49708B68494146C6E3A66BA53770A6A678E410A54FCA836CAF2CE7CC8DF58069D122790AE785EF801970EC4028A6BB6F13F74A97B8AD25243A131B64881120982E97F0BE591F60CF7516BA01F1416BE3A5706E3E14A5D14EDCD7C2494A197D3D21B5D12AC614C7E0353212F684ECF8EE071571FCAA1C8880A31DEF1EAEBDEFC947492B00909420BBFA569409B526225388FCF7884A2543D11556BE2856B2866E5D99877A6FC97FC52E04114010AAAD5B02DDAE197FB0BA97D43A18829E9336733E8A552D9A633F66A1D34F71612D6C39583DEA6DC0DC88986FFBD025B07044B30962E1FA1EA03C6EC77245DF7B782F4E6DA4849C81C6DB96ADC3E2BDCC9CA9421BC0CD306B87AFE5C091CCA7ECC0053D649DF78D349D29BC2912C01968BA691820EA662C0744D5C3B1E8D0510D31195D0F57D5B543A128FB2E8FB7F7A7685B123C7252666B0A69975632A6D501E6605AAA6AB5F668AA7B57FBE3FC8FBAC6ABEB49DF137EE47BAE43ED15800FEEE317C4B1F900BC563FB5985CE2D59AFA73B5E2A6253010250EAA9B50A3AC1DD330EAF9295463345A4687C7186C8D85F4D4A51094CC3DBEA94BCE0A4090A681973E0D085BD5E20152B7E6908293B8E6E21E329D5FA1ED364ED77DD4AF2DDDCB11E9F14183C38D1D141D71BF43E369BEBB7054F7198751CD32E782AEECC9AC5FE7F19AF42FF146ED728433AC80A24341D7E113DA25688DCFC85E922682E7185338DF2A40685E6118934955B63A4750BE6EA7F47C4E3E82F2751747A747F35F1B118E467D71440D1A984EE7BD3EA2DFAD3DA0D583BD42028FA88EE26ECA0C097600AB4FB1C6A1FB3AAD0374DB0E6AEF762D06E3D121AE066412820797D8B9532172D534AFAAEA26B916D407B3D734558D922809D01F600E6BC550F4C4A1C7C4D58A95F3D59FB0346BB4C950C04D40C3F3EF1098CE186E87959013CCD73EEC6DDBEE5C947873DF8D47D5F9416DD9CCD08A36B3336C009A11C1D4EF8F240487A065AAB608FC484070ADDCFB1140F2F1AF6176E3105F0C920141940A3DB969744D4ADAA001E5DADF9ABA484B9CD7920425CD1BB23738C2547BDDCB0A2969C866D2052D3CD762A04037B41C070525E055004D74D3CBCFEAE8C80129323719E56735A619440001F78FECFB29CA57F727DB6D9E3D4B2E9D8D2B42CCD5D431D1B61CDA1071256DA3F2A892412DCFC3358B27F132D886E23496B8ABE979D03DDE808BCC645B6EBBAEE8270F219907BB7CD8E296809911CCF8FC66A672143A52B1017194E2CAA573EE4A3859CC0CE20A478F288DA3DBDD966EBF0A4918AB0E70003A061A9D87D9DB8E4893DFCBC1024E975B3E7C8389FA2DCB9F8AC76C7BDFFED0202D090C34456D2D93999135310FFD74A3D05FCC6E10D3D34DD714F4AEBB60FEF751618CC7A123A65B18EF2A63BC7EEE9D9943695437B8D4F7FC14705A346C7AE95FD5E661DDEDD31C8D0E1579B8D7A7B99E6EBD998D9E79F16946633CB387A36FBE6DB1912FD60FFB4AEFC2D11958DC3DF0244C205C7C8F5D0C710AB7FBFE193DDF5F110CE45FF101C9A01A783252D5303A1319E204E8B7EDD5DE512DDC759D956F405CCF40C0E5706E3F0CBD359D6F0846421D838A029AB322BB2162E82C03C4BA177407F65DEBD8AB8270273C70555C3B1080F288B08D1F70D1982FD5AD3D1189F05521EA6B6B99101F8059649F42F70AFD45218A3BA2B3941D9023398927DA432F42D29432C280ABE99DA26CA20A26A0A71923038473ECDE8780B4F42D2EE2F22A4BE332CB6910AE5E9C8F16B48CE646803624286FFBF0EEDB198D4B8BC63C5EBA335A704F9D9B910BA41E771950609A9FE70D5A9DDEE8D0C0443479502FD2B68368AE8DB57721CCC4B00C58469143381B8294B67CB8325867587389609DC53E1C09DC24113ED9522AA8EFFE28E378243010B5F3D54D285DD65888633DA0FDFAB1FA87E64FDAB8C954F1D041260D68160A03E48736991A33EAA49E3E1B433BB2BCD1EAF9EC6108C6CFB1E93D22310848C17D6D23D215377280178AD4A3D1211BAF778AD4ABE8AB4BC1A9D8E866911A7432AA9EFF8A11DC9F527EB55B0634F95C95EE77B8ADE7C7F016960EF064F3B50FD7B1443D32A2AF315880199B8DC6AADB4DEC60347A3E8651CC8FE91D36594BD0EB2EE295D82B2D2C1A8B8ECE63D178D3C1A265F4D2A1B01AB8DD21B75B63438296802B687B046949E6B2F60F98E23586351BF16B2CF9A1F001D924D2E155DE4FF16DC4412D88AA9B0A81EE110EBB23F407942FB402D42F4FD700C16909E0C206C7AFD32E859A3D97C2700FD6A7F1314A0AA2834476371E84B7B92CAFD591037401D88C2FB853C0861A0EC84DD00EEF2E47D193F28011AC2DE381B6A20DD90F1B097E9628ED4600092C9DEBBD3F3DBCDD62F221E19F8E1E157C218393E52033C403D2A20085114D1A7603DA28C303DF3FA96C3754AD833D10832B9FD85188EFFECEC953D5DBF5C6A4CB3EE21E8C5FD8E7EBD946AB211C0E2FB0C3D0A1245A7F2A3A67D7D1AD2F2168B8CCA2A72657C13D41F68C5F886E93E78A90C080F4CB5437A25949334192490C3AA09351560430D9ACA872C64E97C68CE985E61D4C31886A7A4C6FA6495A3AAC4B97EA8168F99CDCEF5BAAD7CEAD2381055DDD719601B54969C07E53D3EDA031F8D53FA10CD943EA8586A365F63170BE09195A4F3F7D0A45D3B7BBED367939C3258A93C280AAC57042BA1E801853B6A4C1C3A46DF580E6A06EF5BA1E0C7DDFE5282DE2D2E66127352844E51094099D6BB47A78A1CEFA83D2222C8F81CEFA8BECA96701689E7A09073D310BF4D70387685F046942FF9AAD1F1E0F980D2C341F982DFA11F082F450440A159CF2E7392CD1EA4E804313ADB5D8FBC393DF5092D0EDEE0B3DFFB9CCD6F7F507710E39B83E983F6E54D5288F9CA019284D5ADDE1BD93AB8A21E850460DE148A78A1573ED47301AAD2C2214D54E472D83410127A65906C49C68C5ED1D9E6DA039A2D04681E6C21E8E35C00F484B0CF3000168FA5025B1780C8145B178D50E42169B3B2C0C3D15D62E8AE3F24DECA153E2A8BC11BFE178FD585EC69BB8D4086D006B83D43BAA6844C060230713C220EDBE0E6178085D90AE935B1FBCD3E47915D54D604A0281F3A6031F2F3E5EDED28FF80FB2E851F510104AD3ACBEFCF477B25B3C4D724A0BC5AFAFCB7CC7932C457A8BCB36243E8AB25D5ADEEE361B94C7B878FDAAAEC186BBB3555E00FA0431326A4C82746076A810EF5631058893F3674C2F74F048C735141809C13F565C50C5BF365D02F00AEA29B0D7AA181A7C6B091975EF3D4AE8C398CAEE75F554D89398CCD1E76C05A2640A5578AECFC81F6552654DBD7DAC4251787C402515DE6CB345E90B84AC2DD1C3507150547EC17941A94C846E544D899B90400EF7AE29D1C37093ED4A4CA5CA15DA8AB10D6B2930D7E29F43557F56C08E23F5392CE30A46F81A95A740DAD452627E16217BD6876733310B30B1551438EB8B453CA2DAE5A9926FDB2DAD779AA078038A4CA65C898CBD147499AD63500873958CD0528DAAC25ADF4752AD445C6C51193D422BD015297074977F39145D890AC3D0DEE7F10CCB35FA0362A9BF6BCE08B5CC9E49896466FA2A2A9C4DB8A642E0C0D534713311BA42C44C1D05D6F30DD1E4B7C4A8AABCC31CC261B102571323C92169BEEB405F116D98885034855A78D013343F4C99024B93F206E2BDA648AF23759416DC91BA4C17CB454AEDAD0CD487502525F364300BD6DF8D7BF5292EB43BD7D555B4F28D105E960328DB029D5ED61985E15ED5650A2C1F31596D945CE2D51A439D1995ABB025D97794B49B1E08DDA8820A5F9EEDB6237F0D8F13A8648CB73BB7D440DFD555B4D23C210FE0EC4AF4308CDEB41723E42A5AE1170960696D2DBDCF26C5126BFE6166321562E0515C1E2F50492542BA6BFFBC14E98A3471D469398478EA62E5388B5D2ED8BF31654A2CAA277621ECAA57631D1A356D50A3B1E70C26DFAE440F838400B81A0A8CDDCB8C1CA6AE440F8374B301D4513918BA17C038647D9169CF044681A09E023BB1CCCE05DBE4BE488DA3CA5E7B8311BC7F1F5750E0EB9E30E11075252AB9D0BC45C14B84A6400D2F43A189A54F4D0F0CA42DD2C461A2503560345BE5923D0BDBE26A6AB6304EDE2B6C605C51851F17056C1A74252A0C7151909624528AABA1C0F839EB552F806E58ACC255AD676D3C92296F661EC22AA8A8C07F4D54C16A2793D15C0DE59EA8BA9A7E19830C3128D5DB5D310945251BAD41EE5725E2975670C62851F8C78595759BD1C36F8438972DD8A0541F93C0B6E26AE863E4124DC990739515ED887C5E5D4A2095E88F5095C26E0721610B354DD3536A36424E8971055D7C5992E048E449032A69E21564A0113621A8AFD95A733D5EB41182AB69E2BEC139F58C49D07635B43196BB5C36DF6D054D7C7DA61321C6BE8A0AE7E50948A997276AD04D21F6DDB1852A3C826C153C4E414533FC3217A6B8AADAA1C55CE9E2F10E8A0D70098C75BE8A0ECED11D3718EBA892022F7CAB88C30C57D3386B8163D739FCE2AA2A27DFC91DE4E1A35FB50E7FEBA1888F803587CA874A0BF0B155B4702AAC042036508956BED4666B3C0ED700FBC845D4E82355E0E35031911A8A28823642F31503228B2600233A07D13F32B02ED2A81BAB30B8810B5CD16CA10D33625AE8021CC6F142C369329EC22682417B0A47F575073804534FA120B042BF8569A7108862B8EF2225802994D5970C5002064EA14670857E03C00C8A82416C66900BB5E8739BC3532803900D5102074FA23208C4A0017012E198155F93D82790D19E4641CE198D71F2C967BC4F259F7B26D864D6513C7AB3C887AC2AC736885BF53C6F83705506371898643E5B4010D138D12A306F1A50E251AA81A1B9D48876326D05985545A0969FF91551A3B0AED92845D4E8630627A7C661ECD87D1B96064D165C55361E10029E2A790C9B165E90BCC0283BF389AAEEC3B6416FFCE4B0C5E28E33B5A04968E2F22423671100A31DC5E6F919741B9727187453ACE8735DCB61D00D0260D0500CA19F91376E54C1C0EB5245B7AB4A0EC3AEE115A36EFAE95F18D401A11AA280BFEF2F67D8C14D7F6F626070955F35D716F333BA2B540A6D45B8A27824607D687EA4D19D1A2881C911059E5A4C0F0D1A6D9B02F9665443D2FB41457026A00855198E4987DE86B9CA7762402D09757395418611441EAB50859A0DE9960AACA7350CE9E6C9744EA4DBA4096665C4C4927991DC4C8507035F4BB59E1BF8F6A9AE3C7298A3365CFCBEEFAF789AF8CAEAA17130B2C91205AF6B6286CC52D185018B290322E11562480E2119A214109C418D307D9326A66550B86DB90853C1180F552EDA3CCD67289107B52EDC8C0B2B1B0E4FB81DF73279536FC8E164F592F992A5B5078723C8680FCC94F0E85807EF7473D4DD41B9EF2FB580F6265C513C12B03E3441C25B301AE8807991DCCD719E1D9D893198137FD3219E096F93406754390B6C2555E799BAE27980D651816BEA99B8EF6E5C0966A1ABA0E8755B4F387AF5C03B14C0A0FB3B637EC65DA5906E16443072A68AA2E37D4DE1E8A1301629A220C26070738C7909403625507D8D610160D2C912DEB6D3472F9A4230A4C8CB0C3617FCB466AFAE6B34B40A649A59AB5183361F7467D1C76C3537154DC98E0733192807AD379D825B95C68D0527C9B60B57D9335EBD072329D44036636E60434C6FDBD4840E5E71E3062C3F04B119A98100709BD219C5417BFFD864461B189B91D6A021E6B469099854C1956BCB59A517AFEF4FD196A65D124EE3A09262346C5DE14441D7BD15A8A6E6D7AAB1C6F6944D03F06696A0E7C397B21CA660F816D604C6FDE0E2BCDAB12CAB2E1E8F040A9A24E9657E6DC4019CCDFCD5408D1954C14846AB0005E7529975C0AC895966B57FFBB771E35ACCB20287C994C851E9AD822032DFADE59917A7F3093B2D8F008BDD34C1C8C22C91A0ED5917E93A8FD7A457890F4E52E0B2993439CA10CBA6E84180C583D397C80FE49430E2A950814293AE97C6C5AC99498F9184AD4B8FE634A02C062C3D9EF339B3818EE824E976EEBB0442BA933B04331DF4005A7F7AD507543ACD01932CCA9F641119D55C68D508F4165595C439C11060D094E0EABB16BE10C1DB4CDBBA1A4E05A235505D7D653A81736921A60BFA669D1A486BCCFA469BE96CCE6792B1591EEEFB6E8BE77058513DBA417DD95C4179263430427324CA83613E3D7CE6B3AE65F0E0555A5F3C3419183467EA846CFAE8A79E407132B27B8D746AD0143B6194CD920B627899A499DBA42BE6D41728AC5E3DD7CE8BDD2429BB3FD96EF3EC193E94E7EA8827615C159A62410638399E091DB76D5B42B7EDB082BAD34297ADC9C82777D7B60D3584255FFABE92BAE75D5DD729E8114DBFFACCCD8DAE8BC2E9002A2B47C3C348A647F3D28918353061A2C48B3EA6EC0A478F288DA3DBDD961A3B057C98A60567325A1E7C8A39055A094A8F6DFBA20B500A089BA18AAE44F99ACAA92F47B5B93CEFDB1F72C29455170F4F0205CD9D28BFA836CEA044D7F75687E0BADA26C4D08D750242EB7043094444895E3DCC5B1571238D2E5281188C7208A9358DCA98405523134713C1CD03D4A03BB13CA8E9D879E29A70A2F9C6C49A5C92F6D8C2A95065D0BDEFF2F902DE84610DC9A67F5011F41F80E97A6548805910A51EB61A7BD35CDB3170F4C33AD2AE0FAA0A66406F128698A08D3F9CFAD87C16DABC9EF74C60093F0F402D71FFF9CAD05C88522FAB50894411F80A84C37CC85C6C7C258D21C85C6B669311C8A526C829AD8E50D103540F540A2F9B4645166C8BC6023878451D116D00A4F5CDC72B32FE7DCEE9D41B003849B93EC14AE1D4C39581CBA6569E53DDBCA900C4CA67F596FA1265D5C583954041D309E62397CCA50CFD84440A340B3D07A3358900A0D17879F88926166848985166F4788EFB84F759CC354EBF25B52523160281F329CE00AF8B35C449F8B079DDC3700D28DD11EB1E89DBCDE75C07E3702FC4997CA4F54D072BCEEBE336892192FB0C5BD60F2AD082D31DB07E6881DD84CE176020EA872E657210E643F64D9D1CE650F459453E0CFA2C9D3DAEBAD600C7508A7953866DC8504317A525EF98384DDEE8A911837994416A8D5B824031BB8AF7516C1A9C7CCE9BB758EE1BCB969FDA6105F18006F5A089A2A5729B71882250B641E9232A0691925AF0EA78291D34B2C02CADB760EC5B0E197DD9BEBB22F47EC015D5231DD4974DA6E8E5170DAC53E67F143CAC722F7AD3059839431492D19B6102675BF3A118A77621ADAFF7048EC70512E5509203584C8228A392DFA99E38BF12FB30CFFD69963EE317C25EC2582D5975C9E0C45070BE4FF1E343DA78270CED1A34ABB89922ACAB392AC52D14BBA952DD38F11D513D7AFC497E822FA9AD373CF5B9BDEA352A5DD4139FD6732D0F575B63EA060006431C508BF7091C60873352881F1FB39CC6C12B617A132901510C560C299C4CE92B66462D4C3FA1D0E368EAB31F0D28F1A0D5C0D0CCEABCE266DA4C0079297A1C4E3DC79A90E209D043204A720F414AE75BB3B939E75CB42F9203580C59B44FF23BB19387868E5EFD933C9926AA2A1E9500020C0755BC3EA88577D2E7D1B8970CD5ECAD02518D5008299EC1E1738CCA3914B7108087F92EA8880FA86B324215F9394C5E180A3452E0769ADB5065EBEBEA9995F4F8914FA1DF01AE281917541F9C2AFE2952D9748168DD1D0BBFBCAD61ABE711E314E75DD92F6F6FA347BC41CD875FDE922A11DE963B94D4E991DB824637153D64F3E5D5ED1645A4EFA7FFEDF6F5AB3F36495AFCFAFAB12CB77F7FFBB6A850176F3671946745F650BE89B2CD5BB4CADEBEFBE9A77F7BFBF3CF6F37358EB7D1C0A1FECBA8B75D4B6596A3351E95D257E356F8439C17E5192AD1775490A9395D6DB86A1F2F3E5EDED2BFF11F2311F24B37B56D53CD9940FD98E9ED6EB341790CBE2E44E1686C460B487F37696368736F2ED2871C1565BE8BE8D1F01BFA32655C94984CF29B01762EDD633FC31FC8A0A9D2ABC68F192A502120286E2394A0FC4B9E6D715EBE40C37AB95891E9CA92DD2615958E29568CB97BEC9445287C0155D9C33AF807E85C5DA08F8F10063ECD31F9EF68B08302337C843FE2871842D897E863BC28CE3051196374CC677D5C37D9EFDF289D516F178B8CFDCE63FBE5ED88DEC6C4FE96A3F691FC197391098F0DD4F0146CC63E476DCD6A52242A62EE61052C37AA61C02E9BFA95E601CE0DFC72B3180B25DDEB87C1B3DD63CA1E15EBE3FE8CF21CF13899CFFAB8982EDCE0079C634EE2C03516F123C6F8E38A9FDD2AA68C1727E7CF7064848BE89123D7913B2A0CA245A8AA8FE54CF7D18030D282FC855767D5F36E03DA1894E863BC2493F575BB422052AED0882910C70C688F486DF8CA7B234AFC129C56131A64A78947B41420F8981885950C961C17511E6F015DC51684D6A344A013A63DC3DF6340837085FA786BA7C2781AFBAFFA98A823718CA7FD6630CEEB331A2FB82B4623EC3F2F1AF2083464EBCBF22AA860479E866412014ECF34354CEBF4E2B189DC61628CBE76CB6D7A2C4EC232DFCDB101B26B5062C23AB72F4515703BE49CF6AB79DFA8C707EE5B5D628031DB6C51FA020C962D5804D91108B2A1C1D1F2D984F697A00963FB4B8847CFFE6AC0E5F6175329BC00DB37CB6661E2FD65E224266D7DA652DE2FE70AF1EAB0AB0458C8A31D0CC7988312136E2CF05FFFE51647392EC72CC9969878ED3623E6AEBFE863803608CBEE408EEBB0F891ECF670592655129EDB4738C6D8852F95F875F85303896C973B84E5F815AC61A2FDAA43F2B1F26B3E1AD04D9317F5640DB8FEB8C2394E17EA690258AE2B3031DC897550B9F8C1F102C5C633B9222B0BCE625DB0C8AF63905FF536CFB3D08291EA482A11A462FFCA937FF739A4BABF8920FF48FF551FD3F906C52391D87C321011645647D3527D31335F306FBD981D26AE56392E463650F7511FCF97C72CC5D0F40E0A4C4E76563B42BEA319EABFCEE74369C040E7F2B0684ECFD1595C44BC62ECBF1A5059673D102112672356E64B2D464D68769D8DD79A2F35519144EAA5D7C0D9CBB0C414E3280490472C8D1154E307B97154B428F6E351EC55FC5F4459A7002FF4B96B79690BFA2A5F8146212B07D0026380ABA3BFF25574256F1E309FCDCEFF7954FDD7B036C2192EE2750A84260D0AE6D3D23EEDBB455AEDB1B42236031FC3EB2AA040A45A3249002926D30A8027D3EEB309C95740BC8C181418E3032C3FB66061A3E361A39B6C57627A4782944EC252B206F4D94B8E4541CE2CB080EDC655F4179D028D91B6DF4C746B93116F8C6A5060A4ABDB94083CC641D17240B188048694FDCA0008A306D3C360D3722085E0F568FF75E193854F3AE1AA48D0E6C03172DC1ABCA34220D6182C1CAF34C6A52646256F4D1A9A919E83972F8A93A824E319136EFB75CE08C085EF0F84EFEB246813323FD880B1041060D1130315B05C167455C29BCC5309AC26BDDDC0250567BC93F46E61E3FD65E3E7093817C2A9C5AC30A098EA9F41967C36E642DED8353674711EA304F2E00E4B0C68F2EAFC62448ED517330CEF7814EF4C707C42E98A18ECF9768887F96C1265FBFD147D4F4633DD7F35904AD9EF383F59A16DC9F9CB072506EB176FE8CB70E0020E8B0CA80A97BF67F913F9FA1CAFC658B9C2F94C40C8003437FF1629BFE7525EFA92A3B3BC1763D796FC3214721DD043C2DA60581EFC5E80373DE56FB3B670EBDE72ABE8A50007168512176AB0250C265CB0F14A1D41ACC0479C729ABBFD365FB4407D1052BF60C11F90C00FBC88B17D892B82F99A8F8222D8EF7388A8EB7C8DD2B800A66D5812DA1B4DB982F771F55F17212C9EB7D34794A638E1DDA05CA115DE2FA82888550D686CAE82157E78D9078506D458DD63864C8C61890D469A185084B32E33900FCD9C9DFFB18D73CC275F81CAC346635500A759FA10E79B31A98ECBCCC7FD09158FF088EB12132741B4CBA9722DD166B4A11E15CD1739C680096614AEA1DFC2DDEFD907141153E33CA5FBFE1176BED440EF67D153B62BCFD32A0BD0D7321A19007CB1056EA0CFE33293AD77848BE2032151BC3A05EE4BF1C566728BB782FAAF7B63F09E6CB795184D50BCF19DC58F416D93474B0A3E8D215C0B6A2AA461B5D09684DEC55693009CEDF59F0D717D43C90E42D67CDF1BF2645F4DBBCCD6B1EF4C931C7E0B42D5C021166CA43AECA31C15999C1AD530FF8E47774706055351F01ED049FDBADE64645299706E5402A398568440424D2CCEE67296C6C51695D1A36727A900AB8E7354082ADC0736109CCB91F96E72036A4D4651275C05EF6902E5FAD8DBD73B21CCE33203BBCACBE5F31B5C443BBC124D27506CB057CFE9997CFDCCCF6895062506189B7EE0D5FB1778DDEB1293F1D7F7D6C7F8D8EFE6FD836F15F2A54687AFFDE53ECE6A1A171E5724F28FE5433A2847BEF8116F07452240AAA147849032772940AACC6733D72BD50FDC7E7450608C8F77240E0ACCF0F1A2A3FD688067B71D3DCFC421056B2C0C7F0C0CAF7838C785ED65A875985F0E1F5AB3F272C05400B84752FBCAB0E19E2FC4B70FD7874FDDDF399E4F35523F6A0459F7C312538CD739E7FC18149809BF07B44B4A5EF8359F17517F0CA2BECAE7E3DDB2B312ED30DCE4960E0581ED39AB6B6218B6E5B0796A667F77CA1706DC5B066CFD05953EA2FA7612779D08BB81DB4E8C42E5286921456E3CB6DCDC0DE3C739E83B16F9328B80809FFEEBC2BBC7C0BBD986BABBA2E9122EE8B4A0C3C35A6884140340735C27A8A3BFF24BD285E532B918D741CA054ABE535C2955A1379008121C2A71D0818A64C1A082FE52D7CF000F51B6DF428B131FD7466FF07A47BE9E47D96683F308F3B127708D45141C8128A8FC55B7385D5D666BBF524086594300C8C1A51EB9068A7B5E715466E0BBCA462EABCCE8C6459E8DDE56AABF189CDDE3A240EB1153761FE788CBE723540CCFFB774909F56658628A51B849028A17E97504D2EB43E2FD450B10A586BC12C0098542023C55D17DDC97A0A1D34754909981108F8A0C0E4DD2750C9FC20C4B428BB56AF279DB87F96C306F688B22FEB8AAFB3A9FDF8788179C8CE9AEFB687269232F533E5691F9BC08D86311B015794C206541BCBAA256003C35DD5720F42780A9FE6C1292F9C46D01DB6F0BF71C0DF790159D827900B4DABC03C24E4BA61402E09BEEEB42F04740F08DFAF77C23A3C16A41ED42C810664D03C413FDA0C0181F7F7C3F28303078F73213848F982ADF715E75423F9408F18215C247C83584C06F6206058BA43D02495BE971BAAC139816105A5DD3028695112C246BCD052D2C658D2D724FBB848571F69F712ED267D268E6FB5517357E1356922091FAB106B0A09B91ABA1BFDEFFB14355578758FBAF064709695C024798CC673329E02E4BAA95E0A336B3E5F6880CD74149806A357D8714032835F85C003735715620BCC6643E1FE2ED0E1FAFC12E6CBBB76CCB6BAD4F84FD02E86F4133566A5C884B5F9B3728D44A9DA978E8BAFD066F50FE343ED9ACBF19B04E3B33FC0679546482332E6394BC2745E978A8E3320361B8CB73D21F10EBB8EC502DA46F385D659CA7ADFFBA08ED2310DAF572FA95D0304E0D712C029C9E426B18DEE262BF9BDCBCAA5E0AE6D10D0AC29B70FBE8BA6D5EE25585C948AAE9B7F51EA54FFCA2F45FCD309D44D54BF5506F8162535AE45520FB7D91BD47207BEBC8A53CF67CE5478856D7208661A5063005010DDEB6C0E2020E733D4670136750C3409C92394389103D506C2062FFD8E28810AD103B58C16276EEE2B11C1B1519CC479EC7CF28E1310E0A0CFA4828919ABE77397AC60997241628363813DDD1BCD845395286CC67335C5F8B71F7FAAF86A1891E82454FD1B6443177518CF9BC08FE2310FC1F718A73945CE2D59A8F7D7013FE52D41A0A40012F9AFE01D8987CB94233A2BB7E38A7090B789AEB0A0CF6C613A5F1F19316D44FB246C2DBABB83CC3DF63205A9A2B9C2FD2B9CA0483F31CE2FFF6BB3EB6DFD0CBF73819ED6EBA8F06B397C464FD3F672B8E3C8625265E9D17CA6A50D007536085EF864E14E63C50700DE3161A7E10767C506EB2EE176989F3DAF641C978F147858BC23B06859764DF51D28EC0B3C693E2D651790A04429D3780E3941E57AABFB65CB670C32CE1404279612EF970D27E79B1EDF8F93CCF76DB46E9368E2FBFBCAEC4AFC3EF1A48843CCFC1727C0FD6D05F6F169E8F24E64BE7E0403FD6EE64C9AA17F97038F2A1299F5A4C089AB19216425CFA42A341A1961D4C454382258020B936DFE7101B80E0550CDF62030821B6C6D9B954A5B24A5CCBC8DC6ADDC18AB6C4F50CEE4BE4F19A6049F4C6A7AEBD08E72310CE9FD02EA1F94ABC8A6201520DC12B84144D760330A65EE6B3FEC2DD65693A9278CDA7F976504BD6B71F8301DBF3B1494C23BD36F4D9538948C1AD237801F302B5CC8F23E153482B7360C90DB94889FD92121364893468C85A5E18668E9420D1931C16D92409F3E4E50D4AC7E9DDD8EF06F121E90AC0D57FD5C7E4D3DCF191A57211197B2B32EAE72E1FE23A7738453EC9939C6C03D68F72CA91089796831DF3055CC380783878FA5DD5465DC7A515E12E4251757F882F5DC5CFF16A8792D35D51661BDF67026AFC3AB4A78144B8661C2C477B600D83F0B2BD4C04F211A7DC5B41EDB7B031E9FB7135D15B9210CF11F65FE28A69BEE6A36966BF9BDC7CAA291878766C50B2180E476038DC3EC65BDA9AE7CBD802AC1A725A0C2AE4EC06823B6064BE9B9C5922F08481FD1E3E66CD4FBC47425F5B7E81E2CAD912F32832C021312A32973E4060E3A0C41C2317ED67A5A99B6B4A40F7D88259A5ED911E69354F710377F88625E61885FA18AC608E1FB0804645E638415B842B34C7CB5B5AC312738C80E5352AB2E865FDC49DA0A36DA1B96CACB3C8F1F43B2E35B2CD9E76DB1A722C238745165790AA10F9E60E8FE012D2B08AF1152A710B600593500194AEEEB2728C96FD6E62BF9DA2E2F13A6D576A6CC68D4B4D2EECB09050FC3E5CC37C356BC8BB8CE04EC807783DB94AA62B5A2368C0C7F6AEA04AE8B8B18B628523F219AFBEF1D1E38322130EA760950EBA7EA86CA7D3471C3D8D195D50C96096B7DB846C3ECEE22202AE7A8C0B8DCEB82A18C0121C1519CC735AEC72286946F7D9C4422DC7F6A9D9E3C4BC28309602ADC5FF05454F688D019F375C43BF8567543ED359BE5FC5C536412339C3979AACC573463A33584BB815794DFD16B38787112AC9D09495CD6435212EFEDAE8A0600F6E9B788EEE58BC207BEF05B928F1661A4F0884D9C01B02832B3D22044AE81569CA66E4871EECBE1062BC37B28CDBD1F1DBF361898107C7CB53839F11A58C2196F65BD8435C7FD9C5C8563E16F9CAD81213F9F38D62C12519D45804B125066721385D978FA39390E69B010DC4AB3192E693C1613E40479F8CE9C8AFAF73D1487BAB917A1BDCF351AA00ADD609AA1056B9C1E0CF4B990213313A76FD99BAFCBC38D217B6D95FB6618CFD1BFCCF1D2ECAD62D482D1EDF51318AE6ACB8CC14A598F9E498789E54D737D9CF4AB099B43CAB8DEACF5AF26347FAB044FCD8447EEC99451CFFB0E238A028F62C863D89E0C984A0DF03EF3AB713E124FEAAFAA024A415B78F694EF7270B3F78386A7328BA08D6FD15AC952FDEB70805916A494B01A4E2B4839781DD67E39393CF19888B7E3660622FF9D9A6888B6A00AF70F998AD409C6D9131CE332E3E6350103EDAED6C87F93E751F8D3C9493E41F59F29BFDA802779A705905727D016C1E3C3B021408643BF7B24F21BF38BE7F1046FB47F6FD14E52BBF0C2640AAC1584248D1643700633A653EEB2F1C01123AB4C66546586957F823D24181313EC8DA1915E9E3BC423151A26995449CEB2757B830FEF130FE154AD11AFBD7B14AF4FAC2408643C1243DA840400C2B18E8C7DD96DAB345969F669BAA6B4335C9171B30238E1E511A472066AED0C08B431D12BCA1CF7C36090C4F302A806DC3A0C0A06F45196FA834A0D73A08E3115E017A2AAA642640AA3C0390FCE80A0C055CDD1B50C4F545C6029E11BB12590FD432D84066F953F198710F8DB0DF8DFBED45152F8A636F15474B1C7EF58508AB869A108386A1FB168AF7B30F4B0ED1B7BDBC307BD4BCCC5941F471CC890D41A8091B6310C6A36D10424F830A2BCD11C1E0E7EDD2A90DE185B9F796B92FB37565DBFBE56711560D1616838AE6BB8518132DFBDD401DA224B98DC7FAACFF6A46A5C0BDFEEEAB4974D0331E9D68349F0C7064EBF5F854BDFD66E2038A1EE31448C23428D0C747EF21A61CB2FEABC1E630CFB3FC36DBE5E368FC418121BED3048D2D2CF6BB2136E89C7050608CAF281097D46E5062723090E25C8C162836E8ED1F1106DCA5CC672363337ABACB11778B90F9AE8FEDEE91689FD192B4DFF649467F8B8BB8BCC1A82093E55B524B70EBC96B2902893C62E000D93D2A35718D01F46B41B58BD5B2B7560B75687D46CF7E5941805483078490B2A3140230A67AE673C8C83D1FFB77B2E9401B6E1F423F19E0C808C77304D87F5D58F70858F776F7DD3BE7C23835185704283ED1F90EB06DFFD5C0F088CB64FCD441FDE9F0F8D6A7345B38779F397722E675E15F0B161672F18FCEC8FEC4DBC2C67BCBC657288D1F70E1D9832FC2AA653D8B40C50AA786E0354EFFDD447DD550FC659A61C91C9E3FCF219671B14565F48857EF396FE9B8CC24CCA3BE10C5E31C96E863FC90605CDEE53177F63A2830E2F56E741CBB332526189BB171F8FAEFF3C7C4B7F40B85EBB1258B5C3F22B91EE8B54FE3E60C348187D73F95A844BAE3C85F03F5AFECA67D5F7411387B2F702A47FD5596C6849B2713338A460C848B12938A754608448204A866102EE6E9D69EAF20B1C3CBF3EDE745BEDB789D0269E298CF6667DDCBFBED8BA0B516B40DFF4F6ACEC9DB3010B32A442A293B84170959BED6711855BEE4FF222D7E3C69D1443AF8150F30521D79208254846F701CDF7FD65FB8F7D96A14645B7F317289FF1797F8BFFB68E0D2CAB39177BEFE6270D030CAA571679444A34AC573BB297857D0B0C438CC06F02DB1058B48390691121705F96B9A440B0AE43A2246854148C243404EE4F0C57368F0B24CAA987B280F0E5F6A10E90C5D1DB5B832DA44FE8D366FF5B7D0FBB50F18AFBEA3F11322FD5783D9A925CEFBF1DDA8FEB3C90C1559F2CC2363BF2F369D1AE30F2B803F67FD9BC37EA5AF0CB386E895838BE69E851A13F0B82CBCA9B6C40F1F3F3F55EEC426576696375E5ACF9CA5D5860E8F692212721B08CFF19DB096A566E29D2140B14198F3B07FFCB603AC608A1F42B9F0FC51F0FCF533CE57BB89B2C529906B70B912C3D41B8D5107207B5C5025F411C8C2647BCB64F40D3FF2D765EC3BBE50825883B9A4D0A2896780F82BF683A2F05E81FADD8C2171D59F16363A0A36CA4B62C79CD0A749EBAD90E7CCFC7C03565CA5462266AE312C94C682AF61E00A8AF3A2A43F47BEA0FEB33EAE4B04A1EABF1A39DDE88BF43CB24181A15F1F70E81B5D1DD8D7ACFBF594DC4450DFB84203B1B222D4DC391920E4700DA3CB189476F94DCAA0C0E0EC0727C9C9F76C57FE67B61B9D020D4ACC0298D7541AE45008735F623C66865FE15CEBA25AB6EB47E7924B670CD630A0E0ED36CF80F740D8EF8BA23D0A45FBD2A6708A517297A3B440D124FA56D08E95DAD5C625E655010A5E094B2A86377B990EF0EE25AED044764DF63804A40398020399E4E5198C6507FE6348B4C945991719E622BCD4526B115776E20A46D9942CA26611352DC9E41379D3258875648B0C5A48FEB9F0BD955191C97198E829769B87D87D09252FCF7BE2845ECD78012E390F4AE61569EDDB8ABC09362C31C7E8E715C7C68501748F2D30EF1DAF6B862526EBBC25963F614F69CC91B896494B4519D7EF7329DA12D73BE6EB4C2D38E0EF1B1599E304FD755CA1395E3EB3F8B0C41C239028655464D14BE8654EAED05C365E6F21D3942F35909171F4B4DBD6906319392C32F01FFFB1C5113152A8F173FD7092E7F1F3F8F13C4115839D2BD1F02891B40056D0C7FF9158D2ABBBAC1CA365BF9B9873A7A878BC4EDB951A5B75E35203F93C8084F6F3700DF3D5AC21EF32823BE142E284954C57B446D0808FCD5F4195D0DB878B628523F219AFBEF18F420E8A4C389C82553AE8FAA1B29D4E1FF138E65858C9C80B9D90BDC8595C4480EB675C68D0FF0606B004474506F39C16BB9C3E30339AE3FEB389855A8EED53A3F101A2C0580AB4163F3DE9476BFC258FB984C3600DFD169E51F94C67F97E1517DB048DE40C5F6AB216D5638E83B5845B91D7D46F317B7818A1920C4D59D94C5613E222D285DB7C330526F8267A4456F462A0E57381CD53F4D06E695464605DE45984EB0B2FD0268C2BB5D94594E0493F546EB77310E1876A98500594C9A5FF6AA639E214AFA02B33E33213BB22897609758401928F2B5C5C6FC7E57AA3715F93B9DF20E4662E3818838E1B8E00CA5C714DF17C429801BB2F8418EFCDF28D34A3E37D42C31203B7218ED78F237BAAFD66101D0F2441F96C9C0105B0A28CCD267F6F27DDE23C163968D9121371F48D62C12519D45822B125068167385D978FA3B0B3E69B010DC4AB3192E6933E8E4F001D7D32A6A329DCECBD8603E88B2B5CD4DF71A9BF36C3CA245971F4DB31538A4A641AFC33C2216127A0A63E5DF8CF6D33850CF01657EF2D27501D44990B434CA172035992530FE1192EB9D38061899153A8CBFFCBF984981263073B70EBBDFFBE48E32390C63759823D07199D6CB714AB859415420A978D8B1D5EC852BC0DD07D6B68AE372A22D406947BA547315E0D9294010B37271D0C97E1635062126DC04505182AEE8B348A6960FDD887DC7E35510D3E52C6F84A60F363F1F041A996D61AACBDF99E795A8A5B87AF1508A636567DA628EAC652039F7171166005934D7A731CC3F5962D5878F69878B63EF5F79EB2488DDF84772548A6E65F7D6B4AB895F67C73D3C76D526FB73F3927AF69D4988F67B52ED2C155C2AFF9F85018283797BA8C85080A5DB67C09C05F24ED887C8641859378428D9A3290BF9AF8540C04A2113D9A25AC1CDEDF385DB0E8C2B67BCFB683C8B149B855D6820193CAD1A878938516B1E4B8CE7CA7FA3ECEA817DEDB7BDEBBC179B62B27623B0172038E13625028A4CFF877502755DF8DD5DB75B202B155DF4DBC7DD560F873A0EEB3B9C5DC005FA47119D3748FB058E16B2D4C7C544C4C587022EF028CDB8885610487C8C1F015049BBB07D779BC8E539434973A8708B9C2855B8F885BEFF23A6BE234FC2AC26EC0B162142A6DD4428ACC5BB63CFC26F3326BB2F30DC308BBAF661E35F7471AFC3D8FDDCE2B1F2E3B2C09ED335B9E68F8F1A4DCE589EF9883CB131B61064109E9F4F284A3CEFA93C9297C5A02A7F0CD47039EB93C01A2DEDB8F0B8F1C038F6C8A5B9CAE2E33DF368010AF0EC3488085B4DAC170EC332831D0644EAFBBB9BF2EE7EBD1133F1602D5B8BC0E36F33914BB447041922D31C528743702C58BCC3A0699B5C5518C92B36C436F91468DA3DAB3FCD26A434796692212722F08CFC93861AD90B10F7E248D9FEB71F08C00E68CA4DE222D8E4F5AFC8F2C9DE4784FB31573892141A549FB1D0685D418D40BCDAF5E0EF93C1F3CD20919CF59FBCD55124D21C11729B4BF52A8CCA2A726118867C923C1AC236DA4E0624DDF4371943C2A33A18EDB5D9D43694C1EFD7703BECB763920F3BAAF26764D3FA21A1EB02304752C5B012D29BEDC64AFB441F9D37883547FD3C7F28D6CA433383BE1A8C86EDC4C5E18F9147315CDA4DA4551EC2099D67E37C3D65E9FE4F1F52546DEBB67FC427936C198E32EBED48406EA148A632A68BF9A60AA568147D57D36E17A32E7E3D75E9A6F865474919E34CF810094C3161AE2BDDE9512C48352236BA52561497AA4C5029050E1A15A00F4F18EE9AC0008BBA12500A3D091E014526611B4E5FAABEC2FBD4C0B035B197CA98177348D4B6007C37C363AA9F49C04075A13F37598C6E65B24D67E4BAC8A1B5EEAE415BE4F5395F875A596028994A207B0205D73354CEE2A555935C107ED864506B1235FAE217CCC67E3781608DFA8C8404E44284DF1AA19DFD79BCBD194F2C58B0C52D2D18F2A83AA2779E272488C5EA5904E0B1A72480F8DF0AC19801ED3B7A8CECCDC3E515C95AF5CE31FF36C07248AED3E2F52E208A404258C01AD4D72E152B7150D69A18F4AC61F1006886744F54C28CB5712B6259A73E1712B1EFF7672E7979D01841A9C0B4209DDE5277763E26C3E850CCB0042B10D5DE65E1E285B586B5F59EB37949001D6B69FE73D3E8BDA82DBE4E0623BB7871AF3DFB8CCC47AA6905F500A21AC3ECFA14F17B6DA73B66A6ECE798FB05661D7E62E190A152FB49030970DCB4D390DC6697AE2F98073CCBD81C47CD6C7E5E7B56FE68D61E8901F280EBF67AE679A088F550CB0EDB87491554723ABA67BA29CC36F2DAEAC1E28E74061E162FD3C79FD50E239FF6AE5A060FEFDAD3FC9EA47D6F891A9B52C3AC3DF63E09516AE70BE78522A4688E2C921F1D27E37F6E6827E5CA3D94B6242499FB3154768C31293F3E6EE79FAF1A1335360854FA0D0E11A8B663A0ACD34D9D18FEB998FE5618FF494C7FD7887CF2A3E28589CBE0BF31F0EF357F7482EE34D1347E5DB2A1DA1B791014A1442761D41729200280F7D77C5C7AD1C3FF7836EF07A47BE9E47F4310130021FAEB18881E31203D3490037E6B7E27B09CBCF7107CF5F2EA1858FE6E5A393A2C8A2B87E5F79CC4CD5C5912BF484EFAB5F646D0DF885071EF344F51D60D2D568E2C698EEEB5B53001782F34E21C7ECD3A36C4BC7334FE7ADEB88451FEF50BEE64727EA23788147786F47DAB7D32C5DC574395F5D149F7749F2EBEB079414D86CFCBFBC05E9C29074AAC8F1F499106D96BF7C228441FEB9BFCA9AE7E34C69498E0D242E084497E064CD19AEEE78615B2CBEE84EDA55336611E203895354D13FC18AA7CC0FA9DEE5F1F6FE146D4B14A7E6A4398406499156D1253D169D23A9B5587C91DAA06B16A445E141526A0BFC938E780A9C49A78A0D1DF8619AF2FB33BC25BC418865E06D30A22C73E463C21362D02044D3D60DE954EAA111B4A074DC1812B4F110CDE85D887E3C5C6945FFFC6037BB93F24A7787751A6E11A10FC52F70FB7E3946D046489E1175E148B8C66E8627E49B36E3F6A4BA46D548181E92F7C22727295A0AC74FAA8E1C0557B9CDB6336FB529BE7D986B4A5CA2CCE51A8CA1C07D80B6976A4466E4DD6213A5703F2A438A993A0F769306363F847B4446907A548741BE33593417A473CFF16A8792D35D51661B9CDFB723357331CB113990AD0CB1E1DAF2A8C6AB0C3406033952AD7450866C380BC15A4E9433BDFE23FB7E8AF2D57D9FBAC9804679E0315D363534C8728CCBD137D8A1F1405C5CD7CCB8A4011FF790F9EC9F9C24C3F74632F511970DBD3490EEC45221F2761C756F7728A5EADEFE538B74F4DEE885498B6741330CB43BDD74C81CA54C8FC723D5F448F79F726413E08B70AE508AD6B8B249AF70F488D238BADD6DA91D576456B4A4402820AF1E4A9FD0A42DB91EB4F208FD11A1BCE35674D9A31450E8B0C204C7AC3A133621D1D62F857821D71AD5B4844ADBF044A215AA4989B36AE1B0C9523049CE04F95B963F158FD9F6BEFD61293CA578B890C0A68E06054AF03A921F80D00311CAFA6B46812D0251BFA7A137AD5971A63A1A3D93E2FC842611AD9F73B4DB624AF18CA98EAFAC417F9216F6680F2AEBA519D5F198C6FD866B1CD626F54B8E7D1E80E8A0E3A8B187D12143650B077812A2312843E2ED1172543B2C3A9A2311780ECBE6895C671A6E114D41BD356E53BA2D05C9E04758997A935069DBCC01D2A76A863CD3A487933A3D843E69F4888EECB4067648743CD3D99D681E9D652D8F6A1A4AF62B6F39BC53495CBEA183A4D5C9A52E9BDAFB7EF8028E5958840C8FC9CB5850F88018B78F739A11465F0736B25E1B863B38A466F7701D406B7AFC1222C302F6543840E2930419C413E976FF1138A2FE1F12251A4C909A1CDB7B9EA4657ADBA58BDBECAA7417499B2FDDDF45FB81D2145AD37BB238297AB8DBE8116F503563C51645150BADF087382FCA3354A2EFA8C07595D7AFC8743CC72B9C93D97B2124BC79432BBCB9FD6752E707EA2B5CA1347E20C3BFCB9E70FAEBEB773FFDF4B7D7AF4E92181594A89287D7AFFED82469F1F7A88AEA40699AD51AE3D7D78F65B9FDFBDBB745D562F16613477956640FE59B28DBBC45ABECEDBB9F7EFEF3DB9F7F7E8B579BB763F006AD16969FFEADC55214AB84A528E61E7943472751443351DDEE361B34BE61F8CBBF638E3C5AB2B9C10F20068800C6687E19D1328F8176F0D7D771DA6AAC8F989005BD53FD059525CE53EA8DC3CDD3489448D1F7047784FA56DAD87BC24C55E2A4BA8D872443A53192A6C775F605A6B386680697C56B342BF24719D3DCC716B8FA7BE28EC8985BE235A6EFB1F9E8D8DBE135963CFBFDB9FD2258D7D36CB3DD550D4B5B63EF8EEBD0B730A19E118D4B53E769D3F908CB84B4DE66997321F53AB91F3B7FAEC4F519E5396231A5CF288F1E51FEBF6DD01FFF3B8B8E58164A6C4CCF98AC681E102F3C7E303CBE23160BA18338397FE6B6D87A0C5E01DAF0740738211B5FA405F90BAFE8F2CB965D87AA2F89BDFE754BA1BDA0A3569205B7692FED292A1EAF5342A8F4F1D69786256D16184464B3E042441312C02015A70FD136D00B2B1CC51B94501397FC2A2A5BF56762D4D27D07297E67DC5D2E1BA9BDF4ECB3B63238C6B6F1DF2FD215FEE3D7D7FFB302FAFBAB8BFFEBBE85FBD3ABEB9C18ED7F7FF5D3ABFFDBB8F1367B9F87193FBD3E6B930C2DAA0446B217AAA4A61B1B0123CA2FAC9628208DFB16218347267C50F4681FE55F8830B722EC59A6C5E22A872E8ADA2DE042E06D5FEA14A53E840A217E94BEB06373C2B7C897C9E5CBD07A6879C8D99E6910B9DB330CA2703E18EFB2E358AC868521A767C82EFDBB15174A92C76BB0DE007A482FAF5F5DA13F2E71BA2E1F7F7DFD677316788F0AFCD77FB9C5518E4B29EEBFFD64E1C0D9700A6780F3E79FCC910ECD631B7A5AB8657A6E21DB185C964915307FFBC825C1D4E41A0E8B15F780582635A19B6727C6942FD62C17C5D734FEE78E68973B22EEA9321173898E42B8C111A67AFA643DF03AEDB333807252BD4E5246D2B377897AAE3CBD3EC7DFCCE98AD0D3227DF65BFAD4FB1D2B9153835AC9991E7442E102E9543B728E3C6EF2CF3728E6049EDD5EB59A140F88A89DE0E98067B5CA715178C1F5E5314B313CF1D6FAE19DB17A20784953B99F8906BC0B561E981A8DBBF7D3BBFBE42C2EA249555F6F9E7CC1799C7971649D12EA5F67FD12DBE0BAC1E52E4FAFFD9D27D40881F87097FEF964CF45398752CE5510574409BEB00BB580F038A86D0ECF843ABC0A2CF3A6C8E949AD3764FE14F9192EE275EA2F7E43A2395DD496B9F3AF0374F2FE2D922680A4D9554F8D5A09970AD44E9E74A0138A90A6156F7CDFE0F367332DE41D88BC6F3282943E667685B60EA4CEA27120FB319A0959A07DC0CD4C7AD7506EA2BBBD6169DE3A032AEA82A66E6DC3EA6DFAC000BBF462F1C41F8094A0F46E2316E0E711D57200E0CA2918DF9BE25B88F80088B88D3A685C1116E43CC46043D83C8649CD3B5F7698E788D0FEC163F7F826578FE1C27881190F78D3D786FB040FF89AB26087E6386D4C89B4D132F286F04E9D695E399EF0E07761E389D9F8D99A739FAD99F5797AFEF46706E23C468947F7E2C5D5F9853744EFBC60FA84D2153177F3AD0B3D7F2DBE9FD2EA2E38BE64BFE3FC6485B6653FD936786EE30D4DCDE971D53EE3F2F72C7FFAD2DCE1DE4B13CCDD005B246E20892BCAC16B227B65896775A5F010C784F2188E69D6B2586AD0A19DC206C1BDFB9B39E3011A48CB76AAE19C6C261F1BA5854F27E7D32A85B6056FDAF0A222787B44E1877E924CD68ED1A056FB7BCF27C8B5F3BD92C65E0EA4E32A9BD1D7DCCF79B90F81719DAF511A17FEA6CC3D1A8B32D8E2E851CDD0E923D14838F1E6F663707E4145416C5A3F31D00C5ED745ADEF497ABC04D523BCC9123FD3D8CEDDF91FDB38C7AA3C085AC306036C86E19C7FF9AB55D4CE69963EC4F9C68D18DB117F42C5A3A78D75B4CB690AB7126DB67E16C573201083CFCB14DEFD9E7D4011315FCE530AE584EB328B9EB25D799E564938BE96918CFEB4547C8BD0B96B2751848BE203213CBC3A65C3636DB5046479A8EDF516B2FAFC2745A0F490B39C52C96CB7952C4C50BC096F3F4EB277ABE527959D9E0472B0CDA0DEA9135D2A5669B9C54B516CDF50B2B341A74D65ED5AD2E9B8CCD6B1D5D16705D87BB4DEAA21DACA04BD56FD76A14D097AD433CF7BA3C1283CE39E93B4ADA8A732892C88075E5AB06A2B394CA960BFC4042CFFB47A53838658D9B3B8D8A2327AB4F22A36B056DE440676424D7483D77151D6B9FF3C1A7A6D4656CFD7AFFE627CFD6AE29BB437B888767825582CD3FD724ECF91CF70E9ED5E40D32DBC7AEFE7FE597B2FD713BAB67F3E2E5071298FCD1CD12370B788D12566F5F85D497BE1526F5FB7B0D04D92471354AA89019D728F54B7E22FECA0C6E7CDEF575D7AB610343598D38157F590A04CE02D4EDFBDE3D4C1455C1B7E55A5F45772AD845E42C40A85CCC9023179C8F67DDE93F696A5C1B733D7DFFD551FE771B052D214C8EE719BF54B2713EF36EA46AA5EBA887B2A551FD02E295DE67BD11941AC3B3B5DD19A263696DDD4BAA16AC3AB55873D666F852E142F9CB16F9CD13A0E2AB54189D0C139D7E27071D2B13826641D819B496753DF413A5E95F01B5F7B99451E638E16BE9B9CEFB20DF50E45AE17E1213C56FC27C033210F2E57E197ABF08BA0D0151494EEEDAF328E91B88888019209E5C36F385E3F4EB7059C4DFC4C7B31F106AF7709CACFA36CB3C179849730D6BD67F1CAFD738BD3D565B6B67ADE8B81B77AE56B043F214FDF655E0CD40F79B6F182E80A17055AFBDB40BB879D7B3A5D2E764939EC900784BE372D8B6C995CB6D42FA35B0815E1FBDB2A69D2011E5EACCDE9232A8AB8F0E9D94FD7B1D7A302779BBD5A1F57ABE0146D51C41CA758DE94F22A4DAAF7B1CD2DBA06CCCDA4239BC2947BCD4BA3F10E7049CB2966F609E5A8803DAED013BEA7FF315FD131BCFE06DD4CA657646B23D83B363115EC107FF916EC551BDE8E34EC5650BE6E0B43EE91614397CA8A071AC23066019EA0BC730069C21B032C7438391D361ADC860A19ABC194106183C3372D36AD7823C7069FB743E63D4E24E02FA4C777B0519D5C0D25BEF17A7D70A62694C577B9F7E2AF52C374B96C05A0ADF40B20FAFCD9A1F3ED4C171608C70217E933A1B6CCEEF1061E8BB5F38DC332219FFCC70E35800E6EEF342EA73D14030486A673C6F57E019D6E9B48DA4C1662B070FE7E707ED58C5D58696679F500A027FF61A5049F37DDB79701FCFE9E595CB86C06FDFA292EFCA9D906991F6DCB20FBE195EE0DDEA0FCC9CFB596767E5D77841764D4314ADEA304A5130EFD7497E7A4C35337F3239A35DF70BACA2C4EDA5AB8C5A612336CD083B6ABAC4A7F706F99B7A407B7CFBEABAD826AE2B1D1373DB99A2A1790D07D6B92BA116FC65EF31EE95E1A8FFBEB916DDEE69C28A8E53D4A9FBC2D08457612550F4B7BEC624D858BB7F7304CF1BB3CB6BAAAD2015B9BDA2DF08402B1BBE0C1DCB2F023CAA2728712DF58CFFFD8E2880CDC37DE6E1AEEE25E725833C0499EC7CF28F182EB8CC8486A54DFE5E819274C3A50EFD6F5871DCD6D5C94C5A42D7C2DA61C02108BA91BC3E4682A9FA26D89E2F1F5A645A4CF666F370B626B6FF7E001ECED7AD8C9255EADED422B06086CF40D87604A9D43D05C3F9C57CF7EBB126698E46DDAB7193C25E59D381B081131ABB83CC384899D8D50EF4F579EE1079CE76E92E637F4F23D4EFCEC7E4E939850C7E76CE52BE3F217F4429FB7709D7906CD0D9D32CC78DE7CF4AF610D670F647191527981EA281C97555DB4E3E41B9E8F49F61D256DCFAC34D10083952AE2304CA88B80BCD3765B7ACB2CE32124EAF2C6D521705E9EEDB68DDE6A5C4D36DCC761B1E24010CB845CC8363877063E1FFCE2F3C19CFD31701729308314B842DB6D9C5ADD931722F323131864136F16694B5EC8C49DB30131ED83CBFD63EC3C9A3EB3FDF65E578F58AFF3784DB02693F4781159DE1C6AF0FA8C358CA982D2C3EA29F994E79EEBE175E9BB9C375C4760867D829BBD9FD02EA1E9482C945B036AA3CA18D00915D75D96A6D38542F9DE252EB9D10EC03A6C08B73D1274300D614C0ECC04609AD2286C5A73336696AC880BE71F34E75BE74794A0F3270342E44C24C49B9737285D3B8982F374E58C03362A3418B903DCE3248B0B874FCEE1350B3CC4755AEBEAD4CD82B1792C766FD84258266463BE41FADDCFF10787DACD74D65FD074153FC7AB1D4A4E2B2160E7D0E7B1582D288865C205DDE34C1564948EAFA0F88BACDEC30B7E5ED35A8803C7AD4F74DE191FE87C89236AAB7FCD3DAD59C341DEB2BA2CCA75FA8BCB8FF196C6B458DD5D6E60ADAE2F33B0535AC1184D701831E721EC209C6202E75942DFCB7D5105689B444B3927436EE50A1B70E54550B99DA0349775BCF7CA9FF83C9883AEE60D647FD7CF5A84BE2F68B578FD193A2D469FD6458BD39B11D522F4674C755DAC5F07F344EFB5F0AAF39AD9380E87F06E6E87387ADA6D6B544ED2AFBBD853C5863717685C65737307C927CA8F394A5777647693C9B4D345718A8AC7EBB45D2817EB6B88C93EACDCE46E56DDCA5D769A2509F9305573F5E2D68D354DD9DD2CD269CC6340D745413A97A01CAFBE39864357682A36BF7EA80CA6D3471C3D4D36DFDB6D4276156771114D49452D7E6B0B502FBF46B1CB6D5355E8D9AF934D90B5F0D1F223341B962F287A426B3CAD57F91995CF7495EF5771B14DD0CB74ABFD9C91710C286BEA36B3878751B361864A7407216BF69EA88D68F17969637943654FBC254B44D73E4474F1AEF87BA5775EA3EB9A682788E1EA7C5C25DE38F9D708BC938FAD819F32ECC4B72CEBF1DDFBD913B793C1BA6B6CB663C3B7081F920C99E3F88CA810F0739030A91DE223BD1C9110F1D8096B775DF25B96EC36B82423765124B5EFD56D017F8B57AE283E79A02381135DE7F24B07B9DC7B1122D98B039A7E43667546DE00DB1D8D33C013EA0E6F4EDF898F46165A9F9ED699ADD50DFEE78E589DADA3D8D68852E1B4E30C35CE29638264CD5BD8C7326C8ECFABFBB50AC7E688DB59EE4001BBA162CD01B7789E8171E284EA93BFF179BCC2BF08D13985A87701EA5B784E2B387927826BBC5CD5336F07F6DE2CA1FDCD70BA978F00783D845E24DCF412AE3A3EB0136615A89DDCEA4027B5EDAA563E6773C6984D9C64CD5B605883E80A978F99D70C64674C988B35C3F94CB576B6C35EFAE431666B495F768032D325DC7784C24186060AFE8525B61F1FEB42E1FB46E1FFC8BE9FA27C6543D90DA80D4533A013523269C5B72FA5E9B8EB715883C65D975F210285D32AA1B7639F1666F3170871B2DDE6D9333D33E70C2C2DEF22036F9F2D9AEF569597FCDE25A9B92AB5B95E1073E5BDB09F1D16418064DA0DB35EA114ADB1AD19C02171109B4324130AD0DBDD965ABE4596134EA846EE43865E61029AC6914F9C55F26F2FB6FE0D9135A8F0B36F382F08185D0A2A48880823E4EC052F4552652AF024AA9BEE79C0D65168A796DC75DC6F59FE543C66E3974B3404450FE97420035B2B1AED77804BB88098DFC23E9AD6C89E5EB259BF9F0661F2A9ACDB06E85501D74E36380228CB96E56C7424CBE8A6AA5120247C6BC4B6196F670E7BE9D95F9EF7DD47D9D5D29EBBEC0231CD6149D3A754BD58D3ED4BAECE1635F024EC3EBE3E3CDBCBB5AA2D88BE55C4A258ECA359648C36E35E66EBFA79250B5E6D616DD893859D32C20125C96DEC51E5B189201C239B9EB19FB33032956B4FD10857287A8C537F29B5A8759A7A0BE2C8F32CBFCD76B9A7D78B2A7CA709F2F58A2E45E7F1E8B5C1571468ED67BC1729612BEF58CFFF88B0D7E4C2D1D35D8E3C2DF1DD23D11E36CB61223FBFC5455CDE60549039B093A20C064B593AC230A144F5493B8B7E9F5CBF537FDD67F46C43980DA80D4532A01392A23735E56F574CEC70B4F18329231CBD5C853E1846BBDD7DB7E4B31AD2EA426B07392197DDC56572BC6C064B2A1D076C0BB8EC70F79E319D78D3893D170E75EF1334853A577A1BB8853FF79B3FAF501A3FE0C2CA5BDCC2DAD9A83DEC94FBA5A699B91FAB1C3BAFEC43A627BC64AF99F66B8BCAE811AFDEBFF0A7244EC917BD21AC62A9EEF2D822B28101754A1E53F4F3E422222E8A66729CB0780CB36FF969098A3C18C9EEF5A9522552175DB03C5DAAD459763743A7790B75E1E1603C5C3998AFB23426CD3A72EE08950BBF02A8A60C11F278816D8AF4D6DEFCA153650B1F468CBAA18AD7A9BFAC6170D25E2D7BAD06F5191EB8BCE3FE230AD846B878B08C86985CC42B8F6942E9BA97868B4F91BFF0F50FC5D7CD39B90D23D7A0569CDB834EC8AAEFB395A770E4DDF7FF6212FFBBF95CF2CC8FC3F98ECB35619F73E67653B83A2A9A255DFC1DFBCFF27151103DE992536084C24A04F0280E644F748BCB32A9E286870957EC92C1FABC84D84477EDD91EE803C6ABEFA87FC7C3ED8A492D1BDEFB7A20A9C892676FE87C3ED8B5C8C1A9E5E0E7AC7F46D74608B2F03612700C3FA1F8F369C02C91958745E5955FAC6927CB1B2FA415BD8398AC285F88694A1E6085B3BFDC8FC3A1B89ABFCBA9C321F1D6356970B573CACED519A6A63C34F19BB65A8B301ABF7BA6058FAFD22DE43F39F9D337D5C816EE32B68B8A62C06D94C808FC40368F34CFBC9F6DD542E001083C2F491BD54391F6FB041E8B1DB9435826A4FA0F715E94DECE6A2F914764CD13DEDEF0FD2079B7EB59BB893CF6AFA6AE7617EB117143EEAEF6F41D4E9293EFD9AEFCCF6CE767C0C50D5EC734F79B9B7CE3D9D9DD7A1A2E06C563F1AC0880C325C0B34D2428F4862DAAEE07CC0C69A0815FDAA428314AEE72941628B257C4026476FA5882EC408C51A6D7DE1C22BEB3C4BB6A808993E22F3BD63D9072A6E2C48F1CF1234016C931A5E458BC513F0E6FE74E8E5806DC8A9BF350CF23DCE2F1B3BC53840E86BCF337F103A038A141E22FAA5B85414554FBA4196BDDB86DEEE187D6CD8351A88BC07BAFBCE988B3FFBFBD6BEB8D1B57D27F65308F8BC5647781DDA79C053C7666C60B3B0EEC9C04382F0DA55BB185C82DAD5ADD7BFCEF57142989972A92122F923AFD94B859AC1B3F52BC55312D9B09F7B1E96C1EAF5AA4873AA3EFE178E4BA9A68878EAFBF4DB08E6388E0116F29663B86FE7209F42AFA7C968E0D5E0F253041B458ADCBF59DAE637FCAB63F8E2565E534FA7DF867996E9B8F2199C93C7CBFAAAAEC34BC4435756CBEDAD6C724F7CAF2CF6696BBFBDC78370FF675BA3D5C278797877DD7502ED33091D3F485AF5D3E3DDA8454CAE7E2BAC873EE86916F71B471A930268A9BFC7A16E671DE7F7B6894CB932ADD7D717C6BAD65D376F387EFED84E9FA251DEE547AF737D9976E3A6876D8864451C77FF20CD02EA1E4E1589197204209F8920473D0E4C1C7EABA2A5BB09093F4E439FD5465D37C6405E053529F482B6F76D9A1CC93B770ADDDBE1E27202BB4CCE2FB77496C1C539B6F4703EB663474CC9DE1EF4548CFAF9DB187963C6C0955C536A50107CEBC8635480D9DC33BAC3E7CF0BB3D88191AA67E6CB27DBAF3184F709DE4DB634E7640820E69977D367F2785F05A5B0EF01CBF22B1E1EA947949EA9F1BB85F8DD255E6E4A61FBCDFE0EE593BBEBE7477F72ECECB5EC729FBC3E4129EEB1E71C3C2759F98B108B857ECFB7BCCF1DB784A90C19CE17ACCFB35CD9E5F1C43F13EFACBD411762EEDE301976650C8E4838469F38D2F457E7C4DEBC662976F1B3D3F706BC0AFD9CE95C55F1E70849F05D93C91C35776DAAE1BA65C61D1789973C53CDBEC7287386460C1B9397ED1006E013F6EDEB3A82CA4DF7A0D37F090AD865E78AD7CDFF0BDA9C836F54D5AFB3A76E233AEFA3CD0F1C5EE324E061F27AFCAF2B1C82765B39932F4415768F16BB0978F29F8148A7974254D4AFED7FEFCAFFAEB27FFF19FFFE50B4CCD84A8BB943F014F43ED4939F985DA013FA29EF37D8FDFADDF66BB05A63FF19995E5BCBBF422C6FD6EE645CF437EBA386CBF49717A6752AE37DCFD98C950EB0FAA2E73A9D5F5297ADD62E25DF955F72B6F77F77CDFD9F37757EFCC9F7F27D8E0E219FF5E794A01D765BC18A6889738839F6C5C14AF563A6CC769194E5A3C9818065C4F4C18B6DDDF0C5AD225D94B0F8CD603857B6B0E1D8FE7E3D2DF643E2B3AD8BD9C109D499F784CABA2E1EB3055FF98FEDFE4D97A5B77C6093B53E3211F95DBC0773C1B6B03CFCB71C6F5769FD5194935E874E9F0D221E375C8FA58B92C9D2FFDD1797EE810C5609707B1CA9EB37D92B3F0D0CB7774FDDDF67345D30BBACC2B3B1E2E734A9E47FC65DBB4546F05CB95E76B2BC8531E7A6F4FCB766DE27A677135CF795D86A9C50D5377579346A6BBAB498311AD16F4187A5FFB3A6F6DB475ED9917048747F0EBE129DDEFEE8A699FD8BEF6243C0BB503C2DAD31B4DDEDE8CF2F976839F730F4F173F0EC7BCF63855A00CBD474F5C4695E0A34AD9ACB492FCA67825C1455BB6233B698401394D1A6D504E01471E6F47D762AF9A020C3134C7FBA21876EF6506B0B6BEFA8F62EF709483F0F2D05F055E2197E093BBC9020E783C7F2989CFC79FE3D25A4EA7B75663B5CD2BB8089FCBD1F2C2C7A4BAD8FE605927268D435CFD49638F543FE078737B783AD22C3F2EADF6541C2B65681CCB83339AF273FD7A0B1C9DD7298FE96B52F979BEF24BB3EC2CBC26C6E34DE522EA7DCC7F6E0F87A3A7A1A60BA572BF485CEC4FE95BB67FFE234F53B98B8C6E579AB0CFD392B16D025FEF6A10D7FB61D502E476DFE5EDF7C7F3E1587B65CAA1F7B2FFAC015AD42430627FDB40BDCE623A0473F19488C463DE94E9A95226CD30C8E311AEB30CC2C375A6D1F10838DBF091B2A2E321CF5B26ED3DEEB33AEC9A084AB0E1FC24D284B8F4B696DBA2089F94DA2C8584DA970510CA64390BA0B683BDD1D0FC69C1AF0A97C94394C225E492882671F4D867EF3E3DF8CFD5EF91230960D9A73B66F8DF1FEFBCAF492E03C6790F18ED4B30592D4273C29001F1993268607CC25F9E82BB65CC8B8E8E1361CBC96FEC98B6DBC39F5571744D697A1909428F04A4C9050839C4C461BCA68C083A5E41F757DD532FADE68660E4D1E5D29B83F7E62F579FA774DCA6DA943ECAAAADE14284EBCE7AE0D7AE2E3D2374CFF89AE48D21D367BA7CFD297D45AE1F74664B447D4AFC9CEAFBF8225EF01D09DF2C9669E2FD5C99C7749C8B3C82637DFCBCA6ABE734A1693C975629F746CD1CCBC7C04F25736FC67ABCAAEB75A149DBB2195F7699634FBE8C5391C629C7B79C1526D347AA782F39D347EE3EF0AF0C4EC6C3825675B38EC01EC791C0E3281D9E6ED2A6E33ADFF2F17C6F930C2ECD87AC721B5D7C06A35EE75903CD8FC52EF5D4B8DCE3E22E9EE7D8A0DFFE4B90C9523F3C8E671DCE871C339C6E784E143C3E5370B0EDD469479CCBF96C5E7A7CF81EDFC666DC65AFDDA5A1299D5EE231A9DF033C0276FDD9C230C2DECC0A1C90F6983E1FF3A4FAB02539C43D5C66BFF4EF98FDDBB16B3BF6EAC5C698595E14748D15BD60DD37D6AF0E87629BB56375B724129F89DFB6B9B1D8225A42FF87FDAE7D7DA35B9B776A3FA5F9F7DFBA9FEE8F799D9579B66DA43700FA55EE2FADACC675BF5CB57B23AD02DB64A77AABB16087C9079516D4412844EDFE4511CA565F75963453DC3D79E7A8E9796A97CFF6DBAC4C72D11B12996683491D1688BD3D5BB9E4262DD33673B5D67A57F1BD14A9194C2E79FF8E03D518ACFD9EE4E41D8075618D29ADC15A4F718658EB6C5B03D69A8132D9BF915442C9B6FED40CABC57EC37E44B1D6950BADDBFD16076D80DA903A124110AC81DE425A9BD1BAA20D32CE597C14B41DC9FEFB23C9C94A563CF74939BC17AD81DB91DFB567EDCB7E139BF4DF7EFBEDDF95565538F1F221AE627920D050FDED5AADA575068D6A9B8DF4BE7D16869A3EFEEC829B85E246884C5D0C72C8BF2864DA42BE65E90F91BE694B0199EA22A48DBBBDAE19E0858B8E802BF1F18B76E34D7A1003459844C637B05C14057580291AA5587910D4E91C8880407DC4C4018990A5BE949809938B1DEB9683BA9863DD5484CD3CD69143B7FB649F3CA7E4F47D437FD00C7027B5354F4003068396A82FA0095F186828535D84B42D257506966496ABE408B822E7CA9BEB3CC95EE568E9A12DF964A76DFBD11FA2E0E8AA2C89B456434107B120087E5A336DDAD01137822936F2BACB00F321E6AE78CEF6CB444C77C24444B66A0A8A00A5ABC68E6ACF3A0044B45F3E7EC82F287C68E1D9A0A73567F1E051E641AAABC6E2C8B0EBB484994C2C3C4C9AC1CC8A07E136DAE6A9D6EF4576E57C13F6BF45195D047D453DC492203802FD83342BA37544946895B3DC1888222F387748C23F51C243CF3D90622DAF4600D92374EC1BD075293516A8332ECCB34399D4DB178B13B4118D66FA2631A1E2B7A8FFF14CC68EDE20BB75F4DC07641C12CCA762172C04C5C2FC875E3D1A345F40F779C112B131FE8B2F0724C4C28985E088486983449A96D8C0364C6DD6501BBC92D6A02E4361980DDE51830223F604B0DE3477D93130C65E731879E7C7E3570A5040840C48702E5F2FC8B895CC6A60E4449DE15CB0330D3B0B980541E859EE79F8828016F5447C2AC4E63D12975FB95A3EB08687C2205471A5670329E469B425E2A97DDAE13EF991D2471EEE8B5DAAA4101F9AB4A7169A92FB350AB0064D0135E8CF41A034D869D3AC84D0114F9C456E12E303C91B860CD3A4B69ECAE9AC1060256CF0FF026040206B878311BD38E468121941F65D9A5046C390465A2CE8B4EF9FEC35A7DB5D39DF60FD6F670818D01F48FB31DA6878D1CA8B8518A2C406E8514B1D6ADA47A41435E8AF6735E4A8AF658D97181344B7FB53C3B1A8DE3688FAD35B32349E7AD56175B8E27008B36E6EF805B469F81A2C73933C0FCEDA0C129ABDC4423E15A3BFFC54D8027C84B46E4B390FB034A26741D65FD961E50319B3C080B99EEA5C87B5CEC0158E6E1D06EF8B53BAFB1DCFE8E0E3BAEA32F113EB02AB1B6C58FB6C16089D757D1F9700B9B9BF966380B7D48F267DF31D851D2BE65BB9FBE927061EE434A4DD29E9ACD0D3A9100B7B9FABACDC5C27659DC8F14B213E8E449A0A10FAEBFA3F7EAD1D36C298BF67FED8B54D0F6CB38DDF640C39C64487CCC8C38EA8C0D1488C801A9A6532BF4B77CF6915F6726D2044091608DA482567783D57B4D093F418A823CFB80AA9EBD7093DC50C117F40F13982503573CD48646FC20ED779E78C3840D53300ADA7FA29F0063E2F8C680237ABFC0CC302D1D8DF3EBDE0F1BCF00837ECE211F95065CF8DE2F9659C3C4B5CEA9B7721E8FC2B39E6C9737A931D6A929B5AF9744788A6805510186324671251819837EA63BC3C04C58CC8B960C80943F3C7E5C0EAD3280A5686C2A82B075A3BD23256A3BD050643865880BE4370C0688300D13EE442AB458C742B2F59C9125AAE32736FA7BF38AAF63F9E658EDEDE3C5F92E3E26C01D3FF395033EFE47E2466163C87E790B4882D8E0B960C585AF47E4567CB6D9DBE6E60C3A6B675E02F1ED118D4831684C1DD9896EFF5F183B6D62C77B9113075BBDF65A76C776C38B56F9E92D32AA6131E84A65612339F02C58B1FA600A56D1A50AD1673C0025ACF56A128D8226F3BB6BF24F963FABFC766787D4CB72999E36D7485A4FBE8D087D79470A8238C9321D860A5B5BE01074AAD9BEC50A8D1DB31EBB0C92561F48BD039FEA7F8769D54BBCD55595685E269CF97D99830814DFFDBAA2FB27556D888EA5C3DDF3DB6AED1BDDC625B5C9BC7BD8936A6E56990F4BCB7D1BAB66703D0A5C74768F7DED7B377792E0D3D68C0B4A60B346B51748674099EC97E4C43335A3FD01A99D45E2B7B1684DDA7DB97649F6D9F8E25D9B83868E2323C8E3533C325F2F83312246A932C6948EAB40BFE42C6052AF650691FBF990D245F8BEAC7E1A52837DD7F220D269D3881CFF0E3AAF1D09B61234BF5FB92460CD894A9ED78D6F39849AD3ECBC8A3173E07CEDA34456BC9C7006AAF875CE8044631F330C0C6B9099F0D73860EE46F90F929B0E8B4B01A6ACD06504B4D629C011FBF7D4C4E9BFB8643F32F0ACAAE9C6FF0FEB73847BFADA2E2091EFB2908C4408F20CDC9685D4FEAA839CE02E3A0860107527A4AAB85430DAC4250EC8C68494AEA8E1C4F222360A719F4B2EFE9814DC3D4D0EFE901F086956027581AC4BA1FC34D9AC685B7136A473CF44679101A13117EEF202D100E73DC211A0586D9EF0F75DA7EC90E597D5FECB3BAA84890C91A43DE115B40C0293467787712B3D5931E33A233F83EF7F2A0146B8FD30534B33E0FDD29CEA24ABB68B9350F64A22920F86492331EC62453D7338AB184F157256997B620CEAD31552EF47E8050BCEAA10B30C846EAFCD7CA0080DC1289D977F6276935FC3EAD4A2ADC4A058A570923934B5050C9D5E68199AD1E31F056A56B8FC5E44C10D1C8FF7E961199BC85BE8447C7DC02E232E743D0BCD3A9F1F8597080268C2A28454490E4183362287A1A8CE9B8A1E2375A35A2636511F1BC9731E82C027B31645DC6A1458C4372832C69246A63C15193DC9A38FCD45B098154CAC2BC0E381202BC56DEC0671DBE68941E1771DD765AB741BA5AF04986608A2964E70B49D9D4B5A0F3AA2C892DEDB10E1E37CEA88476EE7F8B130DDE6EE1D46F444F4515B53008D03A8B23ECA72A16D9C82484B3675811F73C86C489ABDCEAD21A05DECC4028CF723B4C6FF38A36C83A433E57C9F64794436E592288A5A170D567438A393632673DCF7E2AD3E687FCA678250B98ED3F8A7D4A93764A059F1A8374393C1172A1AD119238C31B62A74E438E28CCAAD6C2C50866E0AAAEE8C5CCF7ADD19CB826FF4351DC16F288A03FFCB408559D85B43E219C137DB8FC1858AB8BED0F964462D3303BA56FCDD81F237B0B2F597EAD932B38833C2E82413642C5769839AB8B8010ABCCEA61127BCE089799137C8E850FD7480BD9FEE74D5855C0ABAC388ABC730A7355EC72933B07BEF03EE33AA49C31D0460F347C85393067961F0B7B4FC7B2CCDF6ED23AC9F2C34AD127D8A06A24159F270245235783C1CF55B23F64F5FADFE4840C11948209CE70D6071AEA49890888241B8382266B8E36C18C51B69761A23344276AEC192034F8A9C212E114EB94C10938B39E367C4DF29CACC3DEC821C95DF1BCA13FE0E9C168B1901C8CFD1465D092F50534E10BC32406035C84B42D25754D3F279BE52A391AAEDACF7DB255F76C56F28954AC00E026949EE14751B5723D5F431585AB18DEE606DC3C03DC5888CD3EC2AD7D598AAF47CF7F21BAD615E8D7347B7EA9EFB2D7AC5EF8E9B9ACA90830A570F5A7E58A496E72BD63E9437BA7B3A9533735D2AA3F94DDA57F64D5A1BE49EAE45B7250B1446A3DA575776577BB2D8EFBFAE9F8FA9A546FBFFE42CBF99BB912C1D3F6257D4DFEF6EBEE5BD12020F996034C9435252853F8EE617205228D6C81CE24FFB8CB48852CFF704AC98D7755B84C014A96890C629B4EF7D2F6C4F69622D31B108ED0412A20A40645BA199022B92B80447565A38CFC3DC9C90BC746237B3AB3913DA949913C6B1AE563B103A57385A048AEDC24E7E1A6F9A3CEDB74A44F2F29E8588808940BD099E417AF65B2877A6E5F024AEA0AEDD8B7C3CCB6FE945607B0B7C2641AC112A5518B06D7E0F0D497C0B258A11DFBC7E258A76404BF4F4A5C9448A5112B121A54A01F724526FD1912424B0C5CE57BEE0A7F99009224D38C92C9660306C18CCA2C9D111A553861524F1A41277BDE7C1E69440A4F82CBE3A90C92E91EAE228DFE0C7EA00EE5C7B4A6C138C66F625912C2EB3CC95EA10FA250AC17D6129925F2512977C57306C113A0D1CB6E89C6C9A6313E5AD194442F99D09805DF648732A9B72F106AFA22102D7DA941421F78AC08E84B20FE7DA189BDB8AE558588E5A02891C4C21E5010FD1DB3C5A223338F9235D1A929D134C940A26B9A81CA24995DC0357C6F6032500390D2520BEE8633AA0247A393CF9119847F786DA6CD4FCD82A8DDD557E48AC5904891C2208D5DFC55C4B0DF21FEACC886F17D334DCC31EEAC1015C1CAADE4243FA036E2CA70296DB14108CB870588E84B20017DA18D0DF45E186C032D436DA0C5B6426EF7644D5680734688482B96A3338E56053C2CD2DFE1D1AAB01806555DFECA0ED6F6F5B47666F6E406A5BE34FDAF8000D31540E2BA321B8369B672D8405A861A448B0D42FE4C1BE426F95DBA7B06812F9543C2241293C0BCF896E4DDBE1124512200454A34269955712CA51D60552E4004CA06E846CBEF2F0958A8D1D3DA69D3931B94FA2B39E66D6C9CA2425F0209EC0BEDD83753839AEC56E00663841AE10AED245DB04FBE96DA5E2BBB690094D31299898B449022109D49FE7ED74CD976C724BF3E3643DE2BD8352022503E4067FA5CF44937D42F465F047E34FA524B09348B0B2A8516EB24510AA33F0FC70AD9F1E3CA60EFF5C5462186B7E821D9A62AB04AA65A0E9A8ED472BC8616DA9D0A780CE84B6099ACD08EBD06E20A85469C35E0FB179A15717D0924A62FB463AFDDFF0168342247EC000DCF362A32872270F3BE2F1D6B1E326147E8ACCCB49AC837CBBA0FC896EF5004891B4ACD12DADCF88F6902EF69CB04883481C620B37FFA4B11D6974052FA42D360CF1E6752877956000EF0ACCCCC5BC7DE20C152C8F08C0CE0A0AE08F650576A2961CCA4D4A28E4EA7E99354F4650A54458552A798426CA98EFC5601AA8D4CA85346A635E9921E0EF0FCBD2F01A5758526F6D9E1D0A8A1F9782914A03899C820F663314C60019962312450A430496BC14817B00D04181220B90821A801426BD0E5A199B1EC8EBAD98242014957888C7B5E6D1699BB0C1C73845278EF8B23B0DB5E13F2EB633B6D029166D34DA033CA7FEBBEBE5992EB2F5F6868616D50723BA5ACB4B1556394FC4A0739A11494588D809A9C5253270E5F942844F662950C8A3A0D146283320ABDF9C0113979EB4BF00337BBC3B6A76DD2A68A3D1EA0390B57084E5AB872CB35F63559BE42270132816E9DDDD1D8CA2CF23CC53A0E44A495CDD159CA4752D4A1AA20F43AAD902A960AB23449D876174CA65347A4B4D4E231ADC8D99C46819E4227BB27B2165B1F2B1D323A02BD504A6329734834874A1D487472072A93E4BB2B4818F915E44F0A4C2C5F0FF8E9245F080AE0CA4D72908C6DAA4C8410948FD08ED345772E8C935AE8637F4A2CE61850B5108A91B3358E62843464DB43253149B5DAEC803217C0B2252254BA4467900F87AB2B1AC064900E30A5C5FD26389654D1042785B4C1A94DE79B579FA1C34DF22B78B2490AACEEDEA22E168BF17BB8962E55032811893C092E95A7B292AC9FD70334B8EC31B37A13902D103C0EBA6AF8832A532101C52A54F692F5428DF214515CDC81E1127B1785F70B57E56F9ACBEC60D49E186C225C516FC46317D0855ADA1BF62D13BB0BF3EF44D347BB85DD51B7768B441FD62DD29D7CC02DD815FB096E012E7B6FFAABE7805B74F41A03C53BF0D424F8823B540FBCDCCE33D15F569FE216E582F8F0EA15EC175D059D81C2BD79661378291EAA075D50E47968EF1BFA72CB90CAD1DA3148F6C733730D8D00B0F3091438A7C415725640D1050B7003100C20BF060038C4A2166E281C42D1DAA80F8CD07011E608002BF8D3EFC75D186A505A9FA899CB0D62A4C5A60BF580BC0093EA4C3AA956409124402DF51601575F732F60BC0BC81268D30566A866F3C5B8D27C004AAB28FD4163261455D2D6140BFC98D7C57E20E6B1629FE6A1212C6D75A0D48FA16CF71BB19396863293DFB357ACA485FEFB2BD5D0A2B742F9A19C4C8FDD4FA557116B74E20313E286486141ADFE48CC8F580F8AF7A1B5B5613C134C27513B9D2520BC250ABDB1A962AA7E441EE1A02990663142FA693E40E5BF45E510340A652CBCCCC954EDD41DA45BB1B9527FD018ACCD94E2DEF76631BF0B7EDB0CBC710FA8C49E0C006BCA017E020734686F822B80283C437FD7D708D019349187D42D369184DE5CA31F1F4C75CEDC3DE8420D25F6BA529BD915D273421A3FE81E1EF2E404E52854F0007E9E39DEFC3E5A733344878273219810374589206D4D40834381BA7C942B57198C5F7536DCC6E6A0E6AAF5BC19493C66B49227F2D832310DDDF491C188913D01AEA21493DC2A89041CC734AD7DBD833918318E2309DA7EFC4D85A13278EDC0C1D83E34987B1347673944EFCD10B8AE12F12D32C163B8BDB88545715BB984D2EAA62F853CDF8762C417E80616383E16246AB558589182E8115F61F1F03E5D765F9CD2DDEFE0A9A8B992CF2DAF253B69442F13AB84EE6CCB7151975E618C8F581DDC6031D1436B1D9CC661896E22992036D74949F264A27E1188BC77273E8FC5603498A2C2C54836C7D19908BE29397EE214D94421C98679A74D471E76BB0D4C28D2F2D0E70999E01225AAD0C22FA63A819D832622A11E32A716F1E1A6EE01957E9F6D82DB0C3C62BB51BA288B7813BBF5EAD7A9FDFE9C935B112E3FAF631FAAECB97149EE03B5065EE7EE6438618D7EA7DE58C7FF66B43E8550CBC632219047176977EC2D6AFD0C6EA29BD17DA6285B3F89D58C0643164E7094BAAD3D269FD384537E16DC65716D0F23D5DDD6F071574F0E08A650C4C27C9D5C603B969BAA841DB2233AC47EF660AE741E4E2141DE9B812FEE0891D093E2604D3E7C5DA80D06A48F375D4DA4D6DB059E9868E97173F0E46FAD51E6746E51F1A04BF5B531652A43DCE6C451E75873DA33E662FBF465D61214748E4EE336BE71588EABCD555956C5093EF252687CEE324959CFDA4A584AB3E9E6A1FB4B2281DFDDA538A6315CE89B6E205A5DDB7197647BDEA8A100B117F5A17AC8DD5A73723C1FAEB84FB72FC93EDB3E1D4BF2E13FC01BE756F502206211AEC1EE5E1B6AACDB1D5DBEC44DF71F3D4074E43E1D21E77F6C6BA1B91D7DA060606E83809EDA93094BEB14ED9D0EEDD9B6A94A98736D6DD64ED83D9E2E88C0F6028D61EB2AB56A30442CC4853435E6A6CFF109ACF0440ADC0829BD68AB36923B545CBB08893DE9CA054ED839C93CA67FC7123450A4F1A0AA5C0BAEE8CDCC2EFDD9863BF6540D05A8BC1DAB4A481013A932282049521DCCD5ED4CA84421762562992A654B359FB6DA550CBB4B65C8202BF8CA940ED69FEBB039A596DEE7446A196E1153DEDA034A5B2F0E9EE014C082DF0C097DC7BB4DCD79AADD6FD191FBC4129ED7F589BBADACCBD4EAC515D0EB17564E012A6A36DED0073B9E866060DD131CF3BB6E48596A7106A5A10E7D0C05A4A6A55ED1E49C757587ED799445ADB0E3D09CCEC183B2B5F4FE8FBEE77082FD019D55BDF343C960A82D4E941A678094F63052E0AEF58242EED514ACBE725264CAADEDE4122941F608EFE86A46711492385C66674A013EDE7D2C15F886BD61AC7A4924C08D91B28DB79AF7BFE90E1A23A73ED1E6BC1E7167C6AA7EE8198C558E7061A16F97EA7BBA5BBB94D2E80A1426F4B95CC0D2640B7E40535F4F301D49BEBCC1B244034E19C94263BC36DD35758155F26A1D57385ADB3A09B5471763A1EBFA0AB8A1E323D867770D970A7B735DEC4FE95B036BF41E818EDCEFAD0228B5389B4E6952863B3AC0707F16A50D3B4B9EC315E693340D759843342CFDBBE20A5FE73EB281A2BB2D3C2254F0DCBEF3BB4648616FE71C4D95E0EE01D3F70F7CF409F9C7BB08CAB06FDE41B6A81576ACD1BD34D0B2B27A39605ADE44E80900B3CB2C6B86759BE969847EEA69F5CC8147F761F3687D059FF3E9B95D23BF8CA049FA8E916A6ECB4CCAF78EBDFAC0D5D7BCE430D505DC030DE65E65AA12B63BA18F53701ED2BD37E1C345269800B4617012D70DA33E57F37DA7F00F94EF2F93FCE207BA6884097D2E16B1574CA8DDA6974970D3DFBFA37CDA6714B27D5AF565EFDFD14749D80FCD9F755135CB7D9ACEACFDF5FDBBC76353FB35A57F35CBA2EC7960F1BEE1B9A74FD60D4C3B9ADBFDF7E253559469D5DAC06BD49174C5DDC97C333FDB25757255D5D977F2E243556CD3F609D75F7FF992E4C786E443D3EEBBDBFDC3B12E8F756372FAFA2D175255BC7FA797FFFE9DA2F37BBADF75F06142A366D698903EEC7F3F66F9AED7FB8F243F488D86B1B86EBCCF1E706D4F259A7FD3E7B79ED34705471823E6BE9BB44CF764AFF473FA5AE60DB3C3C3FE2939A553746BBEE477E973B27D6B7E3F653BD2053126E68610DDFEFE264B9EABE4F5C0780CF59B3F1B0CEF5EFFF9DFFF0FABA34B67D4C00900 , N'6.1.3-40302')

