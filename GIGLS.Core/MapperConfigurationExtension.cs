using AutoMapper;
using GIGLS.Core.DTO.Zone;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.User;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.PaymentTransactions;
using GIGLS.Core.Domain.Partnership;
using GIGLS.Core.DTO.Partnership;
using GIGLS.Core.DTO.Fleets;
using GIGLS.Core.DTO.Customers;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.Client;
using GIGLS.CORE.Domain;
using GIGLS.CORE.DTO.Nav;
using GIGLS.CORE.DTO.Shipments;
using GIGLS.Core.DTO.Haulage;
using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.DTO.MessagingLog;
using GIGLS.Core.Domain.MessagingLog;
using GIGLS.Core.Domain.ShipmentScan;
using GIGLS.Core.DTO.ShipmentScan;
using GIGLS.Core.Domain.Utility;
using GIGLS.Core.DTO.Utility;

namespace GIGLS.Core
{
    public static class MapperConfigurationExtension
    {
        public static void ServiceMapConfigurations(this IRuntimeMapper mapper) { }
    }

    public class MapperConfig
    {
        private static bool isInit = false;
        public static void Initialize()
        {
            if (isInit)
            {
                return;
            }

            Mapper.Initialize(config =>
            {

                config.CreateMap<DomesticRouteZoneMapDTO, DomesticRouteZoneMap>();
                config.CreateMap<DomesticRouteZoneMap, DomesticRouteZoneMapDTO>();

                config.CreateMap<ZoneDTO, GIGLS.Core.Domain.Zone>();
                config.CreateMap<GIGLS.Core.Domain.Zone, ZoneDTO>();

                config.CreateMap<StationDTO, Station>();
                config.CreateMap<Station, StationDTO>();

                config.CreateMap<User, UserDTO>();
                config.CreateMap<UserDTO, User>();

                config.CreateMap<AppRole, RoleDTO>();
                config.CreateMap<RoleDTO, AppRole>();

                config.CreateMap<State, StateDTO>();
                config.CreateMap<StateDTO, State>();

                config.CreateMap<ServiceCentre, ServiceCentreDTO>();
                config.CreateMap<ServiceCentreDTO, ServiceCentre>();

                config.CreateMap<SpecialDomesticPackageDTO, SpecialDomesticPackage>();
                config.CreateMap<SpecialDomesticPackage, SpecialDomesticPackageDTO>();

                config.CreateMap<WeightLimit, WeightLimitDTO>();
                config.CreateMap<WeightLimitDTO, WeightLimit>();

                config.CreateMap<WeightLimitPrice, WeightLimitPriceDTO>();
                config.CreateMap<WeightLimitPriceDTO, WeightLimitPrice>();

                config.CreateMap<PaymentTransaction, PaymentTransactionDTO>();
                config.CreateMap<PaymentTransactionDTO, PaymentTransaction>();

                config.CreateMap<PaymentPartialTransaction, PaymentPartialTransactionDTO>();
                config.CreateMap<PaymentPartialTransactionDTO, PaymentPartialTransaction>();

                config.CreateMap<Partner, PartnerDTO>();
                config.CreateMap<PartnerDTO, Partner>();

                config.CreateMap<PartnerApplication, PartnerApplicationDTO>();
                config.CreateMap<PartnerApplicationDTO, PartnerApplication>();

                config.CreateMap<FleetMake, FleetMakeDTO>();
                config.CreateMap<FleetMakeDTO, FleetMake>();

                config.CreateMap<FleetModel, FleetModelDTO>();
                config.CreateMap<FleetModelDTO, FleetModel>();

                config.CreateMap<Fleet, FleetDTO>();
                config.CreateMap<FleetDTO, Fleet>();

                config.CreateMap<IndividualCustomer, IndividualCustomerDTO>();
                config.CreateMap<IndividualCustomerDTO, IndividualCustomer>();

                config.CreateMap<Shipment, ShipmentDTO>();
                config.CreateMap<ShipmentDTO, Shipment>();

                config.CreateMap<WaybillNumber, WaybillNumberDTO>();
                config.CreateMap<WaybillNumberDTO, WaybillNumber>();

                config.CreateMap<GroupWaybillNumber, GroupWaybillNumberDTO>();
                config.CreateMap<GroupWaybillNumberDTO, GroupWaybillNumber>();

                config.CreateMap<Company, CompanyDTO>();
                config.CreateMap<CompanyDTO, Company>();

                config.CreateMap<CompanyContactPerson, CompanyContactPersonDTO>();
                config.CreateMap<CompanyContactPersonDTO, CompanyContactPerson>();

                config.CreateMap<Manifest, ManifestDTO>();
                config.CreateMap<ManifestDTO, Manifest>();

                config.CreateMap<GeneralLedger, GeneralLedgerDTO>();
                config.CreateMap<GeneralLedgerDTO, GeneralLedger>();

                config.CreateMap<CustomerDTO, CompanyDTO>();
                config.CreateMap<CompanyDTO, CustomerDTO>();

                config.CreateMap<CustomerDTO, IndividualCustomerDTO>();
                config.CreateMap<IndividualCustomerDTO, CustomerDTO>();

                config.CreateMap<ClientNode, ClientNodeDTO>();
                config.CreateMap<ClientNodeDTO, ClientNode>();

                config.CreateMap<Invoice, InvoiceDTO>();
                config.CreateMap<InvoiceDTO, Invoice>();

                config.CreateMap<VAT, VATDTO>();
                config.CreateMap<VATDTO, VAT>();

                config.CreateMap<Insurance, InsuranceDTO>();
                config.CreateMap<InsuranceDTO, Insurance>();

                config.CreateMap<InvoiceShipment, InvoiceShipmentDTO>();
                config.CreateMap<InvoiceShipmentDTO, InvoiceShipment>();

                config.CreateMap<DeliveryOption, DeliveryOptionDTO>();
                config.CreateMap<DeliveryOptionDTO, DeliveryOption>();

                config.CreateMap<ShipmentItem, ShipmentItemDTO>();
                config.CreateMap<ShipmentItemDTO, ShipmentItem>();

                config.CreateMap<ShipmentCollection, ShipmentCollectionDTO>();
                config.CreateMap<ShipmentCollectionDTO, ShipmentCollection>();

                config.CreateMap<ShipmentReturn, ShipmentReturnDTO>();
                config.CreateMap<ShipmentReturnDTO, ShipmentReturn>();

                config.CreateMap<MainNav, MainNavDTO>();
                config.CreateMap<MainNavDTO, MainNav>();

                config.CreateMap<SubNav, SubNavDTO>();
                config.CreateMap<SubNavDTO, SubNav>();

                config.CreateMap<SubSubNav, SubSubNavDTO>();
                config.CreateMap<SubSubNavDTO, SubSubNav>();

                config.CreateMap<Message, MessageDTO>();
                config.CreateMap<MessageDTO, Message>();

                config.CreateMap<Haulage, HaulageDTO>();
                config.CreateMap<HaulageDTO, Haulage>();

                config.CreateMap<HaulageDistanceMapping, HaulageDistanceMappingDTO>();
                config.CreateMap<HaulageDistanceMappingDTO, HaulageDistanceMapping>();

                config.CreateMap<HaulageDistanceMappingPrice, HaulageDistanceMappingPriceDTO>();
                config.CreateMap<HaulageDistanceMappingPriceDTO, HaulageDistanceMappingPrice>();

                config.CreateMap<PackingList, PackingListDTO>();
                config.CreateMap<PackingListDTO, PackingList>();

                config.CreateMap<Wallet, WalletDTO>();
                config.CreateMap<WalletDTO, Wallet>();

                config.CreateMap<WalletTransaction, WalletTransactionDTO>();
                config.CreateMap<WalletTransactionDTO, WalletTransaction>();

                config.CreateMap<CashOnDeliveryBalance, CashOnDeliveryBalanceDTO>();
                config.CreateMap<CashOnDeliveryBalanceDTO, CashOnDeliveryBalance>();

                config.CreateMap<CashOnDeliveryAccount, CashOnDeliveryAccountDTO>();
                config.CreateMap<CashOnDeliveryAccountDTO, CashOnDeliveryAccount>();

                config.CreateMap<Dispatch, DispatchDTO>();
                config.CreateMap<DispatchDTO, DispatchDTO>();

                config.CreateMap<DispatchActivity, DispatchActivityDTO>();
                config.CreateMap<DispatchActivityDTO, DispatchActivity>();

                config.CreateMap<EmailSendLog, EmailSendLogDTO>();
                config.CreateMap<EmailSendLogDTO, EmailSendLog>();

                config.CreateMap<SmsSendLog, SmsSendLogDTO>();
                config.CreateMap<SmsSendLogDTO, SmsSendLog>();

                config.CreateMap<ScanStatus, ScanStatusDTO>();
                config.CreateMap<ScanStatusDTO, ScanStatus>();

                config.CreateMap<GlobalProperty, GlobalPropertyDTO>();
                config.CreateMap<GlobalPropertyDTO, GlobalProperty>();

                config.CreateMap<MissingShipment, MissingShipmentDTO>();
                config.CreateMap<MissingShipmentDTO, MissingShipment>();

                config.CreateMap<ShipmentCancel, ShipmentCancelDTO>();
                config.CreateMap<ShipmentCancelDTO, ShipmentCancel>();
            });

            isInit = true;
        }
    }
}
