using AutoMapper;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Core.IServices.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
//using ThirdParty.WebServices;
using ThirdParty.WebServices.Magaya.Business.New;
using ThirdParty.WebServices.Magaya.DTO;
using ThirdParty.WebServices.Magaya.Services;

namespace GIGLS.Services.Business.Magaya.Shipment
{
    public class MagayaService : IMagayaService
    {

        int key = -1;
        int myAccessKey = -1;
        CSSoapServiceSoapClient cs;
        private readonly INumberGeneratorMonitorService _numberGeneratorMonitorService;

        public MagayaService(INumberGeneratorMonitorService numberGeneratorMonitorService)
        {
            string magayaUri = ConfigurationManager.AppSettings["MagayaUrl"];
            var _webServiceUrl = magayaUri;
            var remoteAddress = new System.ServiceModel.EndpointAddress(_webServiceUrl);
            cs = new CSSoapServiceSoapClient(new System.ServiceModel.BasicHttpBinding(), remoteAddress);
            _numberGeneratorMonitorService = numberGeneratorMonitorService;
        }

        //Open Connections
        public bool OpenConnection(out int access_key)
        {
            var magayausername = ConfigurationManager.AppSettings["MagayaUsername"];
            var magayapassword = ConfigurationManager.AppSettings["MagayaPassword"];

            api_session_error result = api_session_error.no_error;
            try
            {
                result = cs.StartSession(magayausername, magayapassword, out key);
                access_key = key;
                return result == api_session_error.no_error;
            }
            catch
            {
                access_key = 0;
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="access_key"></param>
        /// <returns></returns>
        public string CloseConnection(int access_key)
        {
            try
            {
                var result = cs.EndSession(access_key);
                return result.ToString();
            }
            catch
            {
                return null;
            }

        }

        public double CalVolumentricWeight(double volume, double grossWeight)
        {
            var volumetricWeight = volume / 166; // in lb
            return (volumetricWeight > grossWeight) ? volumetricWeight : grossWeight;
        }

        public CurrencyType retCurrencyType()
        {
            return new CurrencyType()
            {
                Code = "USD",
                DecimalPlaces = 2,
                ExchangeRate = 1.00,
                IsHomeCurrency = true,
                Name = "United States Dollar",
                Symbol = "",
                DecimalPlacesSpecified = true,
                ExchangeRateSpecified = true,
                IsHomeCurrencySpecified = true,
            };
        }

        public GUIDItem retNewGuiItem(WarehouseReceipt magayaShipmentDTO)
        {
            return new GUIDItem()
            {
                GUID = Guid.NewGuid().ToString(),
                Number = magayaShipmentDTO.Number,
                Type = TransactionType.WarehouseReceipt
            };
        }

        public MeasurementUnits newMeasurementUnits()
        {
            return new MeasurementUnits()
            {
                LengthUnit = LengthUnitType.@in,
                LengthUnitSpecified = true,
                VolumeUnit = VolumeUnitType.ft3,
                VolumeUnitSpecified = true,
                WeightUnit = WeightUnitType.lb,
                WeightUnitSpecified = true,
                VolumeWeightUnit = VolumeWeightUnitType.vlb,
                VolumeWeightUnitSpecified = true,
                LengthPrecision = 2,
                LengthPrecisionSpecified = true,
                VolumePrecision = 2,
                VolumePrecisionSpecified = true,
                WeightPrecision = 2,
                WeightPrecisionSpecified = true,
                VolumeWeightPrecision = 2,
                VolumeWeightPrecisionSpecified = true,
                AreaPrecision = 2,
                AreaPrecisionSpecified = true,
                VolumeWeightFactor = 166,
                VolumeWeightFactorSpecified = true
            };
        }

        //public Item[] retWarehouseItem(WarehouseReceipt magayaShipmentDTO)
        //{

        //}

        //For creating shipment in Magaya
        public api_session_error SetTransactions(int access_key, WarehouseReceipt magayaShipmentDTO)
        {
            //2. initialize type of shipment and flag
            string type = "WH";
            int flags = 0x00001000;
            Guid guid = Guid.NewGuid();

            //3. replace some important variables
            magayaShipmentDTO.Type = "WH";
            magayaShipmentDTO.GUID = guid.ToString();
            magayaShipmentDTO.Division = null;
            magayaShipmentDTO.MainCarrier = null;
            magayaShipmentDTO.Customs = null;
            magayaShipmentDTO.TotalPieces = null;
            magayaShipmentDTO.BondedEntryNumber = null;
            magayaShipmentDTO.Carrier = null;
            magayaShipmentDTO.CreatedOn = DateTime.Now;

            var cc = retCurrencyType();

            magayaShipmentDTO.Charges.UseSequenceOrder = false;
            var todaysDate = DateTime.Now;
            var totalPiece = 0.00;
            var totalVolume = 0.00;
            var totalWeight = 0.00;

            for (int i = 0; i < magayaShipmentDTO.Items.Length; i++)
            {
                magayaShipmentDTO.Items[i].GUID = Guid.NewGuid().ToString();
                magayaShipmentDTO.Items[i].Status = ItemStatusType.OnHand;
                magayaShipmentDTO.Items[i].IsSummarized = false;
                magayaShipmentDTO.Items[i].WarehouseReceiptGUID = guid.ToString();
                magayaShipmentDTO.Items[i].PackageName = magayaShipmentDTO.Items[i].Package.Name;
                magayaShipmentDTO.Items[i].LocationCode = magayaShipmentDTO.Items[i].Location.Code;
                magayaShipmentDTO.Items[i].Length = new LenghtValue() { Unit = LengthUnitType.@in, Value = magayaShipmentDTO.Items[i].Length.Value };
                magayaShipmentDTO.Items[i].Width = new LenghtValue() { Unit = LengthUnitType.@in, Value = magayaShipmentDTO.Items[i].Width.Value };
                magayaShipmentDTO.Items[i].Height = new LenghtValue() { Unit = LengthUnitType.@in, Value = magayaShipmentDTO.Items[i].Height.Value };
                magayaShipmentDTO.Items[i].Weight = new WeightValue() { Unit = WeightUnitType.lb, Value = magayaShipmentDTO.Items[i].Length.Value };
                magayaShipmentDTO.Items[i].ContainedPiecesWeightIncluded = true;
                var volume = magayaShipmentDTO.Items[i].Length.Value * magayaShipmentDTO.Items[i].Width.Value * magayaShipmentDTO.Items[i].Height.Value;
                magayaShipmentDTO.Items[i].VolumeWeight = new VolumeWeightValue()
                {
                    Unit = VolumeWeightUnitType.vlb,
                    Value = CalVolumentricWeight(volume, magayaShipmentDTO.Items[i].Weight.Value)
                };

                magayaShipmentDTO.Items[i].Package = magayaShipmentDTO.Items[i].Package;
                magayaShipmentDTO.Items[i].OutShipmentGUID = guid.ToString();
                magayaShipmentDTO.Items[i].Location = magayaShipmentDTO.Items[i].Location;
                magayaShipmentDTO.Items[i].IncludeInSED = true;
                magayaShipmentDTO.Items[i].IsContainer = false;
                magayaShipmentDTO.Items[i].OutDate = todaysDate;
                magayaShipmentDTO.Items[i].WarehouseReceiptNumber = magayaShipmentDTO.Number;
                magayaShipmentDTO.Items[i].IsPallet = false;
                magayaShipmentDTO.Items[i].IsOverstock = false;
                magayaShipmentDTO.Items[i].NotLoaded = false;
                magayaShipmentDTO.Items[i].EntryDate = todaysDate;

                totalPiece += Convert.ToDouble(magayaShipmentDTO.Items[i].Pieces);
                totalWeight += magayaShipmentDTO.Items[i].Weight.Value;
                totalVolume += magayaShipmentDTO.Items[i].Volume.Value;
            }

            magayaShipmentDTO.MeasurementUnits = newMeasurementUnits();

            for (int i = 0; i < magayaShipmentDTO.Charges.Charge.Length; i++)
            {
                magayaShipmentDTO.Charges.Charge[i].TaxDefinition = null;
                magayaShipmentDTO.Charges.Charge[i].TaxAmount = new MoneyValue() { Value = 0, Currency = "USD" };
                magayaShipmentDTO.Charges.Charge[i].Price = new MoneyValue()
                {
                    Value = (magayaShipmentDTO.Charges.Charge[i].Price != null) ? magayaShipmentDTO.Charges.Charge[i].Price.Value : 0,
                    Currency = "USD"
                };
                magayaShipmentDTO.Charges.Charge[i].Amount = new MoneyValue()
                {
                    Value = (magayaShipmentDTO.Charges.Charge[i].Amount != null) ? magayaShipmentDTO.Charges.Charge[i].Amount.Value : 0.00,
                    Currency = "USD"
                };
                magayaShipmentDTO.Charges.Charge[i].RetentionAmount = new MoneyValue() { Value = 0, Currency = "USD" };
                magayaShipmentDTO.Charges.Charge[i].Entity = magayaShipmentDTO.BillingClient;
                magayaShipmentDTO.Charges.Charge[i].ExchangeRate = 1.00;
                magayaShipmentDTO.Charges.Charge[i].HomeCurrency = cc;
                magayaShipmentDTO.Charges.Charge[i].Currency = cc;

                magayaShipmentDTO.Charges.Charge[i].PriceInCurrency = new MoneyValue()
                {
                    Value = (magayaShipmentDTO.Charges.Charge[i].Price != null) ? magayaShipmentDTO.Charges.Charge[i].Price.Value : 0,
                    Currency = "USD"
                };

                magayaShipmentDTO.Charges.Charge[i].AmountInCurrency = new MoneyValue()
                {
                    Value = (magayaShipmentDTO.Charges.Charge[i].Amount != null) ? magayaShipmentDTO.Charges.Charge[i].Amount.Value : 0.00,
                    Currency = "USD"
                };

                magayaShipmentDTO.Charges.Charge[i].TaxAmountInCurrency = new MoneyValue() { Value = 0, Currency = "USD" };
                magayaShipmentDTO.Charges.Charge[i].RetentionAmountInCurrency = new MoneyValue() { Value = 0, Currency = "USD" };
                magayaShipmentDTO.Charges.Charge[i].IsThirdPartyCharge = false;
                magayaShipmentDTO.Charges.Charge[i].Status = ChargeStatusType.Open;
                magayaShipmentDTO.Charges.Charge[i].Notes = "";
                magayaShipmentDTO.Charges.Charge[i].Units = "";
                magayaShipmentDTO.Charges.Charge[i].IsCredit = false;
                magayaShipmentDTO.Charges.Charge[i].IsPrepaid = true;
                magayaShipmentDTO.Charges.Charge[i].Quantity = totalWeight;
                magayaShipmentDTO.Charges.Charge[i].Type = ChargeDesc.Freight;
                magayaShipmentDTO.Charges.Charge[i].ShowInDocuments = true;
                magayaShipmentDTO.Charges.Charge[i].ShowInDocumentsSpecified = true;

                magayaShipmentDTO.Charges.Charge[i].CreatedAt = retNewGuiItem(magayaShipmentDTO);

                magayaShipmentDTO.Charges.Charge[i].FreightChargeInfo = new FreightCharge()
                {
                    Pieces = totalPiece,
                    Weight = new WeightValue() { Unit = WeightUnitType.lb, Value = totalWeight },
                    Volume = new VolumeValue() { Unit = VolumeUnitType.ft3, Value = totalVolume },
                    ChargeableWeight = new WeightValue() { Unit = WeightUnitType.lb, Value = ((totalVolume * 1728) < totalWeight) ? totalWeight : totalVolume * 1728 },
                    UseGrossWeight = false,
                    Flags = ChargeFlagsType.Rate,
                    ApplyBy = ApplyByType.Weight,
                    Method = MethodType.Air,
                    MeasurementUnits = magayaShipmentDTO.MeasurementUnits
                };
                magayaShipmentDTO.Charges.Charge[i].Customs = null;
                magayaShipmentDTO.Charges.Charge[i].IsFromSegment = false;
                magayaShipmentDTO.Charges.Charge[i].ModeOfTransportation = magayaShipmentDTO.ModeOfTransportation;
                magayaShipmentDTO.Charges.Charge[i].PointOfOrigin = magayaShipmentDTO.OriginPort;
                magayaShipmentDTO.Charges.Charge[i].PointOfDestination = magayaShipmentDTO.DestinationPort;
            }

            //4. initilize the variables to hold some parameters and return values
            string trans_xml = string.Empty;
            var errval = string.Empty;

            //5. initialize the serializer object
            Serializer sr = new Serializer();

            //6. serialize object to xml from class warehousereceipt
            WarehouseReceipt shipmentdata = new WarehouseReceipt();
            var xmlobject = Mapper.Map<WarehouseReceipt>(magayaShipmentDTO);

            api_session_error result = api_session_error.no_error;

            try
            {
                //serialize to xml for the magaya request
                trans_xml = sr.Serialize<WarehouseReceipt>(xmlobject);

                string error_code = "";
                result = cs.SetTransaction(access_key, type, flags, trans_xml, out error_code);
                errval = error_code;
            }
            catch (Exception ex)
            {
                errval = ex.Message;
            }

            return result;

        }

        public async Task<string> GetMagayaWayBillNumber()
        {
            var mwaybill = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.MagayaWb, "MG");
            return mwaybill;
        }

        //For creating shipment in Magaya
        public string SetEntity(int access_key, EntityDto entitydto)
        {
            //2. initialize type of shipment and flag
            int flags = 0x00000800;

            //3. initilize the variables to hold some parameters and return values
            string entity_xml = string.Empty;
            var errval = string.Empty;

            //4. initialize the serializer object
            Serializer sr = new Serializer();

            //5. serialize object to xml from class warehousereceipt
            var entitydata = new Entity();
            var xmlobject = Mapper.Map<Entity>(entitydto);

            api_session_error result = api_session_error.no_error;

            try
            {
                //serialize to xml for the magaya request
                entity_xml = sr.Serialize<Entity>(xmlobject);

                //trans_xml = sr.ConvertObjectToXMLString(shipmentdata);
                string error_code = "";
                result = cs.SetEntity(access_key, flags, entity_xml, out error_code);
                errval = error_code;
            }
            catch (Exception ex)
            {
                errval = ex.Message;
            }

            return errval;

        }

        public string GetTransactions(int access_key, WarehouseReceipt magayaShipmentDTO)
        {
            //2. initialize type of shipment and flag
            int flags = 0x00000800;

            //3. initilize the variables to hold some parameters and return values
            string entity_xml = string.Empty;
            var errval = string.Empty;

            //4.initialize the serializer object
            Serializer sr = new Serializer();

            //5. serialize object to xml from class warehousereceipt
            var xmlobject = Mapper.Map<WarehouseReceipt>(magayaShipmentDTO);

            api_session_error result = api_session_error.no_error;

            try
            {
                //serialize to xml for the magaya request
                entity_xml = sr.Serialize<WarehouseReceipt>(xmlobject);

                string error_code = "";
                result = cs.SetEntity(access_key, flags, entity_xml, out error_code);
                errval = error_code;
            }
            catch (Exception ex)
            {
                errval = ex.Message;
            }

            return errval;
        }

        public EntityList GetEntityObect()
        {
            Entity[] items = new Entity[1];
            items[0] = new Entity()
            {
                GUID = Guid.NewGuid().ToString()
            };
            return new EntityList()
            {
                Items = items
            };
        }

        //Get customers, forwarding agents, employees etc 
        public EntityList GetEntities(int access_key, string startwithstring)
        {
            //2. initialize type of entity and flag
            int flags = 0x00000800;

            //3. initilize the variables to hold some parameters and return values
            string entity_xml = string.Empty;
            EntityList errval = null;

            //initialize the serializer object
            Serializer sr = new Serializer();
            api_session_error result = api_session_error.no_error;

            try
            {
                //trans_xml = sr.ConvertObjectToXMLString(shipmentdata);
                string error_code = "";
                result = cs.GetEntities(access_key, flags, startwithstring, out error_code);
                var objectOfXml = sr.Deserialize<EntityList>(error_code);
                errval = objectOfXml;

                if (errval.Items != null)
                {
                    var resObj = errval.Items.ToList();
                    resObj.Add(new Entity()
                    {
                        GUID = Guid.NewGuid().ToString(),
                        Name = startwithstring,
                        CreatedOn = DateTime.Now,
                        CreatedOnSpecified = true,
                        Address = new Address()
                        {
                            Street = new string[1]
                        },
                        BillingAddress = new Address()
                        {
                            Street = new string[1]
                        },
                    }); ;
                    errval.Items = resObj.ToArray();
                }
                else
                {
                    Entity[] itemsVals = new Entity[1];
                    itemsVals[0] = new Entity()
                    {
                        GUID = Guid.NewGuid().ToString(),
                        Name = startwithstring,
                        CreatedOn = DateTime.Now,
                        CreatedOnSpecified = true,
                        Address = new Address()
                        {
                            Street = new string[1]
                        },
                        BillingAddress = new Address()
                        {
                            Street = new string[1]
                        },
                    };

                    errval.Items = itemsVals;
                }

            }
            catch (Exception ex)
            {
            }
            return errval;
        }


        //Get Magaya mode of transportation
        public List<ModeOfTransportation> GetModesOfTransportation()
        {
            //2. initialize type of entity and flag
            int flags = 0x00000800;

            //3. Get the objects for mode of transportation
            var listofModes = new List<ModeOfTransportation>();
            listofModes.Add(new ModeOfTransportation()
            {
                Code = "10",
                Description = "Air, Containerize",
                Method = MethodType.Air
            });

            listofModes.Add(new ModeOfTransportation()
            {
                Code = "13",
                Description = "Ground, Containerized",
                Method = MethodType.Ground
            });

            listofModes.Add(new ModeOfTransportation()
            {
                Code = "17",
                Description = "Rail",
                Method = MethodType.Rail
            });

            listofModes.Add(new ModeOfTransportation()
            {
                Code = "15",
                Description = "Occean, Containerized",
                Method = MethodType.Ocean
            });

            return listofModes;
        }

        //Get Magaya ports called routes in Agility
        public PortList GetPorts()
        {
            //2. initialize type of entity and flag
            int flags = 0x00000800;

            //3. initilize the variables to hold some parameters and return values
            string entity_xml = string.Empty;
            var listofPorts = new PortList();

            //initialize the serializer object
            Serializer sr = new Serializer();
            api_session_error result = api_session_error.no_error;

            try
            {
                string error_code = "";
                result = cs.GetWorkingPorts(out error_code);
                var objectOfXml = sr.Deserialize<PortList>(error_code);
                listofPorts = objectOfXml;
            }
            catch (Exception ex)
            {
            }

            return listofPorts;
        }

        //Get Magaya packages list called  special shipment or so in  Agility
        public PackageList GetPackageList()
        {
            //2. initialize type of entity and flag
            int flags = 0x00000800;

            //3. initilize the variables to hold some parameters and return values
            string entity_xml = string.Empty;
            var listOfPackagss = new PackageList();

            //initialize the serializer object
            Serializer sr = new Serializer();
            api_session_error result = api_session_error.no_error;

            try
            {
                string error_code = "";
                result = cs.GetPackageTypes(out error_code);
                var objectOfXml = sr.Deserialize<PackageList>(error_code);
                listOfPackagss = objectOfXml;
            }
            catch (Exception ex)
            {
            }

            return listOfPackagss;
        }

        public LocationList GetLocations()
        {
            //2. initialize type of entity and flag
            //int flags = 0x00000800;

            //3. initilize the variables to hold some parameters and return values
            string entity_xml = string.Empty;
            var listOfLocations = new LocationList();

            //initialize the serializer object
            Serializer sr = new Serializer();
            api_session_error result = api_session_error.no_error;

            try
            {
                string error_code = "";
                //result = cs.GetPackageTypes(out error_code);
                //var path = Directory.GetCurrentDirectory() + @"\ThirdParty.WebServices\XML\Locations.xml";

                var rootDir = System.Web.HttpContext.Current.Server.MapPath("~");
                var path = rootDir + @"\MagayaDirectory\Locations.xml";

                var xmlInputData = File.ReadAllText(path);
                var objectOfXml = sr.Deserialize<LocationList>(xmlInputData);
                listOfLocations = objectOfXml;
            }
            catch (Exception ex)
            {

            }

            return listOfLocations;
        }

        public ChargeDefinitionList GetChargeDefinitionList(int access_key)
        {
            //2. initialize type of entity and flag
            int flags = 0x00000800;

            //3. initilize the variables to hold some parameters and return values
            string entity_xml = string.Empty;
            var listOfChargeDefinitions = new ChargeDefinitionList();

            //initialize the serializer object
            Serializer sr = new Serializer();
            api_session_error result = api_session_error.no_error;

            try
            {
                string error_code = "";
                result = cs.GetChargeDefinitions(access_key, out error_code);
                var objectOfXml = sr.Deserialize<ChargeDefinitionList>(error_code);
                listOfChargeDefinitions = objectOfXml;
            }
            catch (Exception ex)
            {
            }

            return listOfChargeDefinitions;
        }

        //Get List of Statuses for Items in Magaya
        public List<string> GetItemStatus()
        {
            //3.list of strings init
            var listofitemstatus = new List<string>();

            //convert all element of the enum of string list
            foreach (string s in Enum.GetNames(typeof(ItemStatusType)))
            {
                listofitemstatus.Add(s);
            }

            return listofitemstatus;
        }

        public Description CommodityDescription()
        {
            //1. initilize the variables to hold some parameters and return values
            string entity_xml = string.Empty;
            var listOfDescription = new Description();

            //initialize the serializer object
            Serializer sr = new Serializer();
            api_session_error result = api_session_error.no_error;

            try
            {
                string error_code = "";

                var rootDir = System.Web.HttpContext.Current.Server.MapPath("~");
                var path = rootDir + @"\MagayaDirectory\commoditytypes.xml";

                var xmlInputData = File.ReadAllText(path);
                var objectOfXml = sr.Deserialize<Description>(xmlInputData);
                listOfDescription = objectOfXml;
            }
            catch (Exception ex)
            {

            }
            return listOfDescription;
        }


        public TransactionTypes TransactionTypes()
        {
            //1. initilize the variables to hold some parameters and return values
            string entity_xml = string.Empty;
            var listOfTransactionType = new TransactionTypes();

            //initialize the serializer object
            Serializer sr = new Serializer();
            api_session_error result = api_session_error.no_error;

            try
            {
                string error_code = "";

                var rootDir = System.Web.HttpContext.Current.Server.MapPath("~");
                var path = rootDir + @"\MagayaDirectory\transactiontypes.xml";

                var xmlInputData = File.ReadAllText(path);
                var objectOfXml = sr.Deserialize<TransactionTypes>(xmlInputData);
                listOfTransactionType = objectOfXml;
            }
            catch (Exception ex)
            {

            }
            return listOfTransactionType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="access_key"></param>
        /// <param name="querydto"></param>
        /// <returns></returns>
        public GUIDItemList QueryLog(int access_key, QuerylogDt0 querydto)
        {
            //3. initilize the variables to hold some parameters and return values
            var listOfQuidItems = new GUIDItemList();

            //initialize the serializer object
            Serializer sr = new Serializer();
            api_session_error result = api_session_error.no_error;

            try
            {
                var trans_xml_out = string.Empty;
                result = cs.QueryLog(access_key, querydto.start_date, querydto.end_date, querydto.log_entry_type, querydto.trans_type, querydto.flags, out trans_xml_out);
                var objectOfXml = sr.Deserialize<GUIDItemList>(trans_xml_out);
                listOfQuidItems = objectOfXml;
            }
            catch (Exception ex)
            {
            }

            return listOfQuidItems;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="access_key"></param>
        /// <param name="querydto"></param>
        /// <returns></returns>
        public TransactionResults LargeQueryLog(int access_key, QuerylogDt0 querydto)
        {
            //call query log and retrieve the transactions GUIDs
            var listOfResults = QueryLog(access_key, querydto);

            //initialize the serializer object
            Serializer sr = new Serializer();
            api_session_error result = api_session_error.no_error;

            //2. initilize the variables to hold some parameters and return values
            var listOfWarehousereceipt = new List<WarehouseReceipt>();
            var listOfShipment = new List<MagayaShipment>();
            var listOfInvoice = new List<AccountingItem>();
            var listOfPayment = new List<ThirdParty.WebServices.Magaya.Business.New.PaymentType>();

            //test for null;

            foreach (var list in listOfResults.GUIDItem)
            {
                //get the transaction
                var guid = list.GUID;
                string xmlTransList;

                var resultVal = cs.GetTransaction(access_key, querydto.trans_type, querydto.flags, guid, out xmlTransList);

                if (resultVal == api_session_error.no_error)
                {
                    switch (querydto.trans_type)
                    {
                        case "WH":
                            //listOfWarehousereceipt = sr.Deserialize<WarehouseReceipt>(xmlTransList);
                            listOfWarehousereceipt.Add(sr.Deserialize<WarehouseReceipt>(xmlTransList));
                            break;
                        case "SH":
                            //listOfShipment = sr.Deserialize<ShipmentList>(xmlTransList);
                            listOfShipment.Add(sr.Deserialize<MagayaShipment>(xmlTransList));
                            break;
                        case "IN":
                            //listOfInvoice = sr.Deserialize<InvoiceList>(xmlTransList);
                            listOfInvoice.Add(sr.Deserialize<AccountingItem>(xmlTransList));
                            break;
                        case "PM":
                            //listOfPayment = sr.Deserialize<PaymentList>(xmlTransList);
                            listOfPayment.Add(sr.Deserialize<ThirdParty.WebServices.Magaya.Business.New.PaymentType>(xmlTransList));
                            break;
                        default:
                            break;
                    }
                }


            }

            var tran_result = new TransactionResults()
            {
                warehousereceipt = new WarehouseReceiptList() { WarehouseReceipt = listOfWarehousereceipt.Select(s => s).ToArray() },
                shipmentlist = new ShipmentList() { Items = listOfShipment.Select(s => s).ToArray() },
                invoicelist = new InvoiceList() { Items = listOfInvoice.Select(s => s).ToArray() },
                paymentlist = new PaymentList() { Items = listOfPayment.Select(s => s).ToArray() },
            };
            return tran_result;
        }

        public WarehouseReceiptList GetWarehouseReceiptRangeByDate(int access_key, QuerylogDt0 querydto)
        {
            //2. initilize the variables to hold some parameters and return values
            var listOftransactions = new WarehouseReceiptList();

            //initialize the serializer object
            Serializer sr = new Serializer();
            api_session_error result = api_session_error.no_error;

            try
            {
                var trans_xml_out = string.Empty;
                result = cs.GetTransRangeByDate(access_key, querydto.trans_type, querydto.start_date, querydto.end_date, querydto.flags, out trans_xml_out);

                var objectOfXml = sr.Deserialize<WarehouseReceiptList>(trans_xml_out);
                listOftransactions = objectOfXml;
            }
            catch (Exception ex)
            {
            }

            return listOftransactions;
        }

        public Tuple<WarehouseReceiptList, ShipmentList, InvoiceList, PaymentList> GetFirstTransbyDate(int access_key, QuerylogDt0 querydto, out string
            customcookie, out int more_result)
        {
            //2. initilize the variables to hold some parameters and return values
            var listOfWarehousereceipt = new WarehouseReceiptList();
            var listOfShipment = new ShipmentList();
            var listOfInvoice = new InvoiceList();
            var listOfPayment = new PaymentList();

            //initialize the serializer object
            Serializer sr = new Serializer();
            api_session_error result = api_session_error.no_error;
            querydto.end_date = (String.IsNullOrEmpty(querydto.end_date)) ? DateTime.Now.ToString("yyyy-MM-dd") : querydto.end_date;

            try
            {

                result = cs.GetFirstTransbyDate(access_key, querydto.trans_type, querydto.start_date, querydto.end_date, querydto.flags, querydto.record_quatity,
                    querydto.backwards_order, out string cookies, out more_result);
                string xmlTransList;
                customcookie = cookies;
                var res = GetNextTransByDate(access_key, ref customcookie, out xmlTransList, out more_result);
                var objectOfXml = new Object();

                switch (querydto.trans_type)
                {
                    case "WH":
                        listOfWarehousereceipt = sr.Deserialize<WarehouseReceiptList>(xmlTransList);
                        break;
                    case "SH":
                        listOfShipment = sr.Deserialize<ShipmentList>(xmlTransList);
                        break;
                    case "IN":
                        listOfInvoice = sr.Deserialize<InvoiceList>(xmlTransList);
                        break;
                    case "PM":
                        listOfPayment = sr.Deserialize<PaymentList>(xmlTransList);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                customcookie = "";
                more_result = 0;
            }

            var TupleResult = Tuple.Create<WarehouseReceiptList, ShipmentList, InvoiceList, PaymentList>(listOfWarehousereceipt, listOfShipment, listOfInvoice, listOfPayment);
            return TupleResult;
        }

        public bool GetFirstTransByDate(int access_key, QuerylogDt0 querydto, out string cookie, out int more_results)
        {
            try
            {
                api_session_error result = cs.GetFirstTransbyDate(access_key, querydto.trans_type, querydto.start_date, querydto.end_date, querydto.flags, querydto.record_quatity, querydto.backwards_order, out string cookies, out int more_res);
                cookie = cookies;
                more_results = more_res;
                return result == api_session_error.no_error;

            }
            catch
            {
                cookie = string.Empty;
                more_results = 0;
                return false;
            }
        }

        public bool GetNextTransByDate(int access_key, ref string cookie, out string trans_list_xml, out int more_results)
        {

            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.MaxBufferSize = int.MaxValue;

            try
            {
                api_session_error result = cs.GetNextTransbyDate(ref cookie, out trans_list_xml, out more_results);
                return result == api_session_error.no_error;
            }
            catch (Exception ex)
            {
                trans_list_xml = null;
                more_results = 0;
                return false;
            }
        }

        public Tuple<WarehouseReceiptList, ShipmentList, InvoiceList, PaymentList> GetNextTransByDate2(int access_key, out int more_results, ref string cookie, string type)
        {
            var listOfWarehousereceipt = new WarehouseReceiptList();
            var listOfShipment = new ShipmentList();
            var listOfInvoice = new InvoiceList();
            var listOfPayment = new PaymentList();

            string xmlTransList;

            Serializer sr = new Serializer();

            try
            {
                //string xmlTransList;
                api_session_error result = cs.GetNextTransbyDate(ref cookie, out xmlTransList, out more_results);

                switch (type)
                {
                    case "WH":
                        listOfWarehousereceipt = sr.Deserialize<WarehouseReceiptList>(xmlTransList);
                        break;
                    case "SH":
                        listOfShipment = sr.Deserialize<ShipmentList>(xmlTransList);
                        break;
                    case "IN":
                        listOfInvoice = sr.Deserialize<InvoiceList>(xmlTransList);
                        break;
                    case "PM":
                        listOfPayment = sr.Deserialize<PaymentList>(xmlTransList);
                        break;
                    default:
                        break;
                }

                var TupleResult = Tuple.Create<WarehouseReceiptList, ShipmentList, InvoiceList, PaymentList>(listOfWarehousereceipt,
                    listOfShipment, listOfInvoice, listOfPayment);
                return TupleResult;
            }
            catch (Exception ex)
            {
                xmlTransList = null;
                var TupleResult = Tuple.Create<WarehouseReceiptList, ShipmentList, InvoiceList, PaymentList>(listOfWarehousereceipt, listOfShipment, listOfInvoice, listOfPayment);
                more_results = 0;
                return TupleResult;
            }
        }

    }


}
