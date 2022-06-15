using GIGLS.Core.IServices.Utility;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using QRCoder;
using BarcodeLib;
using System.Web.Hosting;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Services.Implementation.Shipments;
using GIGLS.Core.Domain;
using GIGLS.Core.Enums;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core;
using GIGLS.Core.IServices.User;
using GIGLS.Infrastructure;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace GIGLS.Services.Implementation.Utility
{
    public class AutoManifestAndGroupingService : IAutoManifestAndGroupingService
    {

        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        private readonly INumberGeneratorMonitorService _numberGeneratorMonitorService;

        public AutoManifestAndGroupingService(IUnitOfWork uow, IUserService userService,INumberGeneratorMonitorService numberGeneratorMonitorService)
        {
            _uow = uow;
            _userService = userService;
            _numberGeneratorMonitorService = numberGeneratorMonitorService;
            MapperConfig.Initialize();
        }

        #region Automatic Manifesting and Grouping for Manifests Management
        public async Task MappingWaybillNumberToGroup(string waybill)
        {
            try
            {
                // get the service centres of login user
                var serviceCenters = await _userService.GetPriviledgeServiceCenters();
                if (serviceCenters.Length == 0)
                {
                    throw new GenericException("Error Grouping waybill. An error occured while trying to group waybill automatically,please manually group waybill.");
                }

                //validate waybill 
                var shipment = await _uow.Shipment.GetAsync(x => x.Waybill == waybill);
                if (shipment is null)
                {
                    throw new GenericException("Error Grouping waybill. An error occured while trying to group waybill automatically,please manually group waybill.");
                }

                int departureServiceCenterId = serviceCenters[0];
                var currentUserId = await _userService.GetCurrentUserId();
                var destServiceCentre = await _uow.ServiceCentre.GetAsync(shipment.DestinationServiceCentreId);
                var deptServiceCentre = await _uow.ServiceCentre.GetAsync(departureServiceCenterId);
                var groupWaybillNumber = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.GroupWaybillNumber, deptServiceCentre.Code);

                //validate the ids are in the system
                var serviceCenterId = int.Parse(groupWaybillNumber.Substring(1, 3));
                var groupWaybill = new GroupWaybillNumber();


                //check if the group already exist for centre
                var groupwaybillExist = _uow.GroupWaybillNumber.GetAllAsQueryable().OrderByDescending(x => x.DateCreated).Where(x => x.ServiceCentreId == shipment.DestinationServiceCentreId && x.DepartureServiceCentreId == shipment.DepartureServiceCentreId && x.ExpressDelivery == shipment.ExpressDelivery && x.IsBulky == false).FirstOrDefault();
                if (groupwaybillExist == null)
                {
                    await NewGroupWaybillProcess(shipment, deptServiceCentre, destServiceCentre, currentUserId);
                }
                else
                {
                    groupWaybill = groupwaybillExist;
                    var today = DateTime.Now;
                    int groupHours = Convert.ToInt32((today - groupwaybillExist.DateCreated).TotalHours);
                    if (groupHours >= 24)
                    {
                        var newGroup = await NewGroupWaybill(shipment, deptServiceCentre, destServiceCentre, currentUserId);
                        groupWaybill = newGroup;
                    }

                    //check if it has a manifest mapping
                    var isManifestGroupWaybillMapped = _uow.ManifestGroupWaybillNumberMapping.GetAllAsQueryable().OrderByDescending(x => x.DateCreated).Where(x => x.GroupWaybillNumber == groupWaybill.GroupWaybillCode).FirstOrDefault();
                    if (isManifestGroupWaybillMapped is null)
                    {
                        //map new waybill to existing groupwaybill 
                        await CreateNewManifestGroupWaybill(shipment, deptServiceCentre, destServiceCentre, currentUserId, groupWaybill);
                    }

                    else
                    {
                        //confirm if the manifest has been dispatched
                        var manifestDispatched = await _uow.Manifest.ExistAsync(x => x.ManifestCode == isManifestGroupWaybillMapped.ManifestCode && x.IsDispatched);
                        if (manifestDispatched)
                        {
                            await NewGroupWaybillProcess(shipment, deptServiceCentre, destServiceCentre, currentUserId);
                        }
                        else
                        {
                            var manifest = await _uow.Manifest.GetAsync(x => x.ManifestCode == isManifestGroupWaybillMapped.ManifestCode);
                            if (manifest is null)
                            {
                                await CreateNewManifest(shipment, deptServiceCentre, destServiceCentre, currentUserId, groupWaybill);
                            }
                            else if (manifest != null)
                            {
                                //get date for the manifest
                                int manifestHours = Convert.ToInt32((today - manifest.DateCreated).TotalHours);
                                if (manifestHours >= 24)
                                {
                                    await NewGroupWaybillProcess(shipment, deptServiceCentre, destServiceCentre, currentUserId);
                                }
                                else
                                {
                                    //map new waybill to existing groupwaybill 
                                    await MapExistingGroupWaybill(shipment, deptServiceCentre, destServiceCentre, currentUserId, manifest, groupWaybill);
                                }
                            }
                        }
                    }
                }
                shipment.IsGrouped = true;
                var updateTransitWaybill = await _uow.TransitWaybillNumber.GetAsync(x => x.WaybillNumber == shipment.Waybill);
                if (updateTransitWaybill != null)
                {
                    updateTransitWaybill.IsGrouped = true;
                }
                _uow.Complete();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task MappingWaybillNumberToGroupForBulk(string waybill)
        {
            try
            {
                // get the service centres of login user
                var serviceCenters = await _userService.GetPriviledgeServiceCenters();
                if (serviceCenters.Length == 0)
                {
                    throw new GenericException("Error Grouping waybill. An error occured while trying to group waybill automatically,please manually group waybill.");
                }

                //validate waybill 
                var shipment = await _uow.Shipment.GetAsync(x => x.Waybill == waybill);
                if (shipment is null)
                {
                    throw new GenericException("Error Grouping waybill. An error occured while trying to group waybill automatically,please manually group waybill.");
                }

                int departureServiceCenterId = serviceCenters[0];
                var currentUserId = await _userService.GetCurrentUserId();
                var destServiceCentre = await _uow.ServiceCentre.GetAsync(shipment.DestinationServiceCentreId);
                var deptServiceCentre = await _uow.ServiceCentre.GetAsync(departureServiceCenterId);
                var groupWaybillNumber = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.GroupWaybillNumber, deptServiceCentre.Code);

                //validate the ids are in the system
                var serviceCenterId = int.Parse(groupWaybillNumber.Substring(1, 3));
                var groupWaybill = new GroupWaybillNumber();



                //check if the bulk manifest exist
                var bulkManifest = _uow.Manifest.GetAllAsQueryable().OrderByDescending(x => x.DateCreated).Where(x => x.IsBulky && !x.IsDispatched && x.ExpressDelivery == shipment.ExpressDelivery && x.DepartureServiceCentreId == shipment.DepartureServiceCentreId).FirstOrDefault();
                if (bulkManifest is null)
                {
                    await NewGroupWaybillProcess(shipment, deptServiceCentre, destServiceCentre, currentUserId);
                }
                else
                {
                    var today = DateTime.Now;
                    int hours = Convert.ToInt32((today - bulkManifest.DateCreated).TotalHours);
                    if (hours >= 24)
                    {
                        await NewGroupWaybillProcess(shipment, deptServiceCentre, destServiceCentre, currentUserId);
                    }
                    else
                    {
                        var groupwaybillExist = _uow.GroupWaybillNumber.GetAllAsQueryable().OrderByDescending(x => x.DateCreated).Where(x => x.ServiceCentreId == shipment.DestinationServiceCentreId && x.DepartureServiceCentreId == shipment.DepartureServiceCentreId && x.ExpressDelivery == shipment.ExpressDelivery && x.IsBulky == shipment.IsBulky).FirstOrDefault();
                        if (groupwaybillExist == null)
                        {
                            bulkManifest = _uow.Manifest.GetAllAsQueryable().OrderByDescending(x => x.DateCreated).Where(x => x.IsBulky && !x.IsDispatched && x.ExpressDelivery == shipment.ExpressDelivery && x.DepartureServiceCentreId == shipment.DepartureServiceCentreId).FirstOrDefault();
                            await MapNewGroupWaybillToExistingManifest(shipment, deptServiceCentre, destServiceCentre, currentUserId, bulkManifest);
                        }
                        else
                        {

                            groupWaybill = groupwaybillExist;
                            int groupHours = Convert.ToInt32((today - groupWaybill.DateCreated).TotalHours);
                            if (groupHours >= 24)
                            {
                                var newGroup = await NewGroupWaybill(shipment, deptServiceCentre, destServiceCentre, currentUserId);
                                groupWaybill = newGroup;
                            }
                            //map new waybill to existing groupwaybill
                            var isManifestGroupWaybillMapped = _uow.ManifestGroupWaybillNumberMapping.GetAllAsQueryable().OrderByDescending(x => x.DateCreated).Where(x => x.GroupWaybillNumber == groupWaybill.GroupWaybillCode).FirstOrDefault();
                            if (isManifestGroupWaybillMapped is null)
                            {
                                //map new waybill to existing groupwaybill 
                                //await CreateNewManifestGroupWaybill(shipment, deptServiceCentre, destServiceCentre, currentUserId, groupWaybill);
                                await MapExistingGroupWaybill(shipment, deptServiceCentre, destServiceCentre, currentUserId, bulkManifest, groupWaybill);
                            }
                            else
                            {
                                var manifestDispatched = await _uow.Manifest.ExistAsync(x => x.ManifestCode == isManifestGroupWaybillMapped.ManifestCode && x.IsDispatched);
                                if (manifestDispatched)
                                {
                                    await MapNewGroupWaybillToExistingManifest(shipment, deptServiceCentre, destServiceCentre, currentUserId, bulkManifest);
                                }
                                else
                                {
                                    await MapExistingGroupWaybill(shipment, deptServiceCentre, destServiceCentre, currentUserId, bulkManifest, groupWaybill);
                                } 
                            }
                        }
                    }
                }
                shipment.IsGrouped = true;
                var updateTransitWaybill = await _uow.TransitWaybillNumber.GetAsync(x => x.WaybillNumber == shipment.Waybill);
                if (updateTransitWaybill != null)
                {
                    updateTransitWaybill.IsGrouped = true;
                }
                _uow.Complete();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task NewGroupWaybillProcess(Shipment shipment, ServiceCentre deptServiceCentre, ServiceCentre destServiceCentre, string userId)
        {
            // generate new manifest code
            var manifestCode = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.Manifest, destServiceCentre.Code);
            var groupWaybillNumber = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.GroupWaybillNumber, destServiceCentre.Code);

            // create a group waybillnumbermapping
            var newGroupWaybillNoMapping = new GroupWaybillNumberMapping
            {
                GroupWaybillNumber = groupWaybillNumber,
                UserId = userId,
                DestinationServiceCentreId = destServiceCentre.ServiceCentreId,
                IsActive = true,
                DepartureServiceCentreId = deptServiceCentre.ServiceCentreId,
                ExpressDelivery = shipment.ExpressDelivery,
                OriginalDepartureServiceCentreId = deptServiceCentre.ServiceCentreId,
                WaybillNumber = shipment.Waybill,
                DateMapped = DateTime.Now,
                IsBulky = shipment.IsBulky
            };
            _uow.GroupWaybillNumberMapping.Add(newGroupWaybillNoMapping);

            //create a groupwaybill for centre
            var newGroupWaybill = new GroupWaybillNumber
            {
                GroupWaybillCode = groupWaybillNumber,
                UserId = userId,
                ServiceCentreId = destServiceCentre.ServiceCentreId,
                IsActive = true,
                DepartureServiceCentreId = deptServiceCentre.ServiceCentreId,
                ExpressDelivery = shipment.ExpressDelivery,
                HasManifest = true,
                IsBulky = shipment.IsBulky
            };
            _uow.GroupWaybillNumber.Add(newGroupWaybill);

            //also create a minifest group manifest and add group waybill to it
            //Add new Mapping
            var newMapping = new ManifestGroupWaybillNumberMapping
            {
                ManifestCode = manifestCode,
                GroupWaybillNumber = newGroupWaybill.GroupWaybillCode,
                IsActive = true,
                DateMapped = DateTime.Now
            };
            _uow.ManifestGroupWaybillNumberMapping.Add(newMapping);

            //create new manifest
            var newManifest = new Manifest
            {
                DateTime = DateTime.Now,
                ManifestCode = manifestCode,
                ExpressDelivery = shipment.ExpressDelivery,
                IsBulky = shipment.IsBulky,
                DepartureServiceCentreId = deptServiceCentre.ServiceCentreId,
            };
            _uow.Manifest.Add(newManifest);
        }

        private async Task CreateNewManifestGroupWaybill(Shipment shipment, ServiceCentre deptServiceCentre, ServiceCentre destServiceCentre, string userId, GroupWaybillNumber groupWaybill)
        {
            var manifestCode = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.Manifest, destServiceCentre.Code);
            //create new manifest
            var newManifest = new Manifest
            {
                DateTime = DateTime.Now,
                ManifestCode = manifestCode,
                ExpressDelivery = shipment.ExpressDelivery,
                IsBulky = shipment.IsBulky,
                DepartureServiceCentreId = deptServiceCentre.ServiceCentreId,
            };
            _uow.Manifest.Add(newManifest);

            var exist = await _uow.ManifestGroupWaybillNumberMapping.ExistAsync(x => x.GroupWaybillNumber == groupWaybill.GroupWaybillCode);
            if (!exist)
            {
                //also  map group waybill to existing manifest
                var newMapping = new ManifestGroupWaybillNumberMapping
                {
                    ManifestCode = manifestCode,
                    GroupWaybillNumber = groupWaybill.GroupWaybillCode,
                    IsActive = true,
                    DateMapped = DateTime.Now,
                };
                _uow.ManifestGroupWaybillNumberMapping.Add(newMapping);
            }
            if (exist)
            {
                var gmw = await _uow.ManifestGroupWaybillNumberMapping.GetAsync(x => x.GroupWaybillNumber == groupWaybill.GroupWaybillCode);
                gmw.DateMapped = DateTime.Now;
            }

            var waybillExist = await _uow.GroupWaybillNumberMapping.ExistAsync(x => x.WaybillNumber == shipment.Waybill);
            if (!waybillExist)
            {
                //map new waybill to existing groupwaybill 
                var newGroupWaybillNoMapping = new GroupWaybillNumberMapping
                {
                    GroupWaybillNumber = groupWaybill.GroupWaybillCode,
                    UserId = userId,
                    DestinationServiceCentreId = destServiceCentre.ServiceCentreId,
                    IsActive = true,
                    DepartureServiceCentreId = deptServiceCentre.ServiceCentreId,
                    ExpressDelivery = shipment.ExpressDelivery,
                    OriginalDepartureServiceCentreId = deptServiceCentre.ServiceCentreId,
                    WaybillNumber = shipment.Waybill,
                    DateMapped = DateTime.Now,
                    IsBulky = shipment.IsBulky
                };
                _uow.GroupWaybillNumberMapping.Add(newGroupWaybillNoMapping); 
            }

        }

        private async Task MapExistingGroupWaybill(Shipment shipment, ServiceCentre deptServiceCentre, ServiceCentre destServiceCentre, string userId, Manifest manifest, GroupWaybillNumber groupWaybill)
        {
            //also  map group waybill to existing manifest
            var exist = await _uow.ManifestGroupWaybillNumberMapping.ExistAsync(x => x.GroupWaybillNumber == groupWaybill.GroupWaybillCode);
            if (!exist)
            {
                var newMapping = new ManifestGroupWaybillNumberMapping
                {
                    ManifestCode = manifest.ManifestCode,
                    GroupWaybillNumber = groupWaybill.GroupWaybillCode,
                    IsActive = true,
                    DateMapped = DateTime.Now,
                };
                _uow.ManifestGroupWaybillNumberMapping.Add(newMapping);
            }
            if (exist)
            {
                var gmw = await _uow.ManifestGroupWaybillNumberMapping.GetAsync(x => x.GroupWaybillNumber == groupWaybill.GroupWaybillCode);
                gmw.DateMapped = DateTime.Now;
            }

            var waybillExist = await _uow.GroupWaybillNumberMapping.ExistAsync(x => x.WaybillNumber == shipment.Waybill);
            if (!waybillExist)
            {
                //map new waybill to existing groupwaybill 
                var newGroupWaybillNoMapping = new GroupWaybillNumberMapping
                {
                    GroupWaybillNumber = groupWaybill.GroupWaybillCode,
                    UserId = userId,
                    DestinationServiceCentreId = destServiceCentre.ServiceCentreId,
                    IsActive = true,
                    DepartureServiceCentreId = deptServiceCentre.ServiceCentreId,
                    ExpressDelivery = shipment.ExpressDelivery,
                    OriginalDepartureServiceCentreId = deptServiceCentre.ServiceCentreId,
                    WaybillNumber = shipment.Waybill,
                    DateMapped = DateTime.Now,
                    IsBulky = shipment.IsBulky
                };
                _uow.GroupWaybillNumberMapping.Add(newGroupWaybillNoMapping); 
            }

        }

        private async Task CreateNewManifest(Shipment shipment, ServiceCentre deptServiceCentre, ServiceCentre destServiceCentre, string userId, GroupWaybillNumber groupWaybill)
        {
            var manifestCode = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.Manifest, destServiceCentre.Code);
            //create new manifest
            var newManifest = new Manifest
            {
                DateTime = DateTime.Now,
                ManifestCode = manifestCode,
                ExpressDelivery = shipment.ExpressDelivery,
                IsBulky = shipment.IsBulky,
                DepartureServiceCentreId = deptServiceCentre.ServiceCentreId,
            };
            _uow.Manifest.Add(newManifest);

            //also  map group waybill to existing manifest
            var exist = await _uow.ManifestGroupWaybillNumberMapping.ExistAsync(x => x.GroupWaybillNumber == groupWaybill.GroupWaybillCode);
            if (!exist)
            {
                var newMapping = new ManifestGroupWaybillNumberMapping
                {
                    ManifestCode = manifestCode,
                    GroupWaybillNumber = groupWaybill.GroupWaybillCode,
                    IsActive = true,
                    DateMapped = DateTime.Now,
                };
                _uow.ManifestGroupWaybillNumberMapping.Add(newMapping);
            }
            if (exist)
            {
                var gmw = await _uow.ManifestGroupWaybillNumberMapping.GetAsync(x => x.GroupWaybillNumber == groupWaybill.GroupWaybillCode);
                gmw.DateMapped = DateTime.Now;
            }

            //map new waybill to existing groupwaybill 
            var waybillExist = await _uow.GroupWaybillNumberMapping.ExistAsync(x => x.WaybillNumber == shipment.Waybill);
            if (!waybillExist)
            {
                var newGroupWaybillNoMapping = new GroupWaybillNumberMapping
                {
                    GroupWaybillNumber = groupWaybill.GroupWaybillCode,
                    UserId = userId,
                    DestinationServiceCentreId = destServiceCentre.ServiceCentreId,
                    IsActive = true,
                    DepartureServiceCentreId = deptServiceCentre.ServiceCentreId,
                    ExpressDelivery = shipment.ExpressDelivery,
                    OriginalDepartureServiceCentreId = deptServiceCentre.ServiceCentreId,
                    WaybillNumber = shipment.Waybill,
                    DateMapped = DateTime.Now,
                    IsBulky = shipment.IsBulky
                };
                _uow.GroupWaybillNumberMapping.Add(newGroupWaybillNoMapping); 
            }
        }

        private async Task MapNewGroupWaybillToExistingManifest(Shipment shipment, ServiceCentre deptServiceCentre, ServiceCentre destServiceCentre, string userId, Manifest manifest)
        {
            var groupWaybillNumber = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.GroupWaybillNumber, destServiceCentre.Code);

            //create a groupwaybill for centre
            var newGroupWaybill = new GroupWaybillNumber
            {
                GroupWaybillCode = groupWaybillNumber,
                UserId = userId,
                ServiceCentreId = destServiceCentre.ServiceCentreId,
                IsActive = true,
                DepartureServiceCentreId = deptServiceCentre.ServiceCentreId,
                ExpressDelivery = shipment.ExpressDelivery,
                HasManifest = true,
                IsBulky = shipment.IsBulky
            };
            _uow.GroupWaybillNumber.Add(newGroupWaybill);

            var exist = await _uow.ManifestGroupWaybillNumberMapping.ExistAsync(x => x.GroupWaybillNumber == groupWaybillNumber);
            if (!exist)
            {
                //also  map group waybill to existing manifest
                var newMapping = new ManifestGroupWaybillNumberMapping
                {
                    ManifestCode = manifest.ManifestCode,
                    GroupWaybillNumber = groupWaybillNumber,
                    IsActive = true,
                    DateMapped = DateTime.Now,
                };
                _uow.ManifestGroupWaybillNumberMapping.Add(newMapping);
            }
            if (exist)
            {
                var gmw = await _uow.ManifestGroupWaybillNumberMapping.GetAsync(x => x.GroupWaybillNumber == groupWaybillNumber);
                gmw.DateMapped = DateTime.Now;
            }

            var waybillExist = await _uow.GroupWaybillNumberMapping.ExistAsync(x => x.WaybillNumber == shipment.Waybill);
            if (!waybillExist)
            {
                //map new waybill to existing groupwaybill 
                var newGroupWaybillNoMapping = new GroupWaybillNumberMapping
                {
                    GroupWaybillNumber = groupWaybillNumber,
                    UserId = userId,
                    DestinationServiceCentreId = destServiceCentre.ServiceCentreId,
                    IsActive = true,
                    DepartureServiceCentreId = deptServiceCentre.ServiceCentreId,
                    ExpressDelivery = shipment.ExpressDelivery,
                    OriginalDepartureServiceCentreId = deptServiceCentre.ServiceCentreId,
                    WaybillNumber = shipment.Waybill,
                    DateMapped = DateTime.Now,
                    IsBulky = shipment.IsBulky
                };
                _uow.GroupWaybillNumberMapping.Add(newGroupWaybillNoMapping); 
            }
        }

        private async Task<GroupWaybillNumber> NewGroupWaybill(Shipment shipment, ServiceCentre deptServiceCentre, ServiceCentre destServiceCentre, string userId)
        {
            // generate new manifest code
            var manifestCode = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.Manifest, destServiceCentre.Code);
            var groupWaybillNumber = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.GroupWaybillNumber, destServiceCentre.Code);
            //create a groupwaybill for centre
            var newGroupWaybill = new GroupWaybillNumber
            {
                GroupWaybillCode = groupWaybillNumber,
                UserId = userId,
                ServiceCentreId = destServiceCentre.ServiceCentreId,
                IsActive = true,
                DepartureServiceCentreId = deptServiceCentre.ServiceCentreId,
                ExpressDelivery = shipment.ExpressDelivery,
                HasManifest = true,
                IsBulky = shipment.IsBulky
            };
            _uow.GroupWaybillNumber.Add(newGroupWaybill);
            return newGroupWaybill;
        }

        #endregion


        #region Automatic Manifesting and Grouping for Move Manifests Management
        public async Task MapMoveManifest(string movemanifestNo)
        {
            //get all the manifest in movemanifest if movemanifest exist and group by destination centreid
            var userId = await _userService.GetCurrentUserId();
            var movemanifest = _uow.MovementManifestNumber.GetAsync(x => x.MovementManifestCode == movemanifestNo);
            if (movemanifest != null)
            {
                var manifestNoList = _uow.MovementManifestNumberMapping.GetAllAsQueryable().Where(x => x.MovementManifestCode == movemanifestNo).Select(x => x.ManifestNumber).ToList();
                if (manifestNoList.Any())
                {
                    var manifests = _uow.Manifest.GetAllAsQueryable().Where(x => manifestNoList.Contains(x.ManifestCode)).GroupBy(x => x.DestinationServiceCentreId).ToList();
                    var destIDs = manifests.Select(x => x.Key);
                    var alreadyExistingMoveManifests = _uow.MovementManifestNumber.GetAllAsQueryable().Where(x => destIDs.Contains(x.DestinationServiceCentreId) && x.IsAutomated && x.MovementStatus == MovementStatus.InProgress ).ToList();

                    var destinationCentres = _uow.ServiceCentre.GetAllAsQueryable().Where(x => destIDs.Contains(x.ServiceCentreId)).ToList();
                    if (manifests.Any())
                    {
                        foreach (var item in manifests)
                        {
                            var manifestExist = await _uow.MovementManifestNumberMapping.GetAsync(x => x.MovementManifestCode == movemanifestNo && x.ManifestNumber == item.FirstOrDefault().ManifestCode);
                            if (manifestExist != null)
                            {
                                continue;
                            }

                            //check if an automated movemanifest has already created if so
                            //add current manifest to already create; else create a new one
                            var existingManifest = alreadyExistingMoveManifests.Where(x => x.DestinationServiceCentreId == item.Key).FirstOrDefault();
                            var centre = destinationCentres.Where(x => x.ServiceCentreId == item.Key).FirstOrDefault();
                            var manifest = await _uow.Manifest.GetAsync(x => x.ManifestCode == item.FirstOrDefault().ManifestCode);
                            if (existingManifest is null)
                            {
                                await NewMovementManifest(manifest, centre,userId);
                            }
                            else
                            {
                                await ExistingMovementManifest(existingManifest,manifest,centre,userId);
                            }
                        }
                    }
                    await _uow.CompleteAsync();
                }

            }
        }


        private async Task NewMovementManifest(Manifest manifest, ServiceCentre destServiceCentre, string userId)
        {
            var moveManifestCode = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.MovementManifestNumber, destServiceCentre.Code);
            var driverCode = await GenerateDeliveryCode();
            var destServiceCentreCode = await GenerateDeliveryCode();


            var movemanifestmapping = new MovementManifestNumberMapping()
            {
                MovementManifestCode = moveManifestCode,
                ManifestNumber = manifest.ManifestCode,
                UserId = userId
            };
            _uow.MovementManifestNumberMapping.Add(movemanifestmapping);

            var movemanifest = new MovementManifestNumber()
            {
                MovementManifestCode = moveManifestCode,
                MovementStatus = MovementStatus.InProgress,
                UserId = userId,
                DepartureServiceCentreId = manifest.DepartureServiceCentreId,
                DestinationServiceCentreId = manifest.DestinationServiceCentreId,
                DriverCode = driverCode,
                DestinationServiceCentreCode = destServiceCentreCode
            };
            _uow.MovementManifestNumber.Add(movemanifest);
        }

        private async Task ExistingMovementManifest(MovementManifestNumber movementManifestNumber, Manifest manifest, ServiceCentre destServiceCentre, string userId)
        {
            var moveManifest = await _uow.MovementManifestNumber.GetAsync(x => x.MovementManifestCode == movementManifestNumber.MovementManifestCode);
            var driverCode = await GenerateDeliveryCode();
            var destServiceCentreCode = await GenerateDeliveryCode();


            var movemanifestmapping = new MovementManifestNumberMapping()
            {
                MovementManifestCode = moveManifest.MovementManifestCode,
                ManifestNumber = manifest.ManifestCode,
                UserId = userId
            };
            _uow.MovementManifestNumberMapping.Add(movemanifestmapping);
        }

        private async Task<string> GenerateDeliveryCode()
        {
            try
            {
                int maxSize = 6;
                char[] chars = new char[54];
                string a;
                a = "abcdefghjkmnpqrstuvwxyzABCDEFGHJKMNPQRSTUVWXYZ23456789";
                chars = a.ToCharArray();
                int size = maxSize;
                byte[] data = new byte[1];
                RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
                crypto.GetNonZeroBytes(data);
                size = maxSize;
                data = new byte[size];
                crypto.GetNonZeroBytes(data);
                StringBuilder result = new StringBuilder(size);
                foreach (byte b in data)
                { result.Append(chars[b % (chars.Length - 1)]); }
                var strippedText = result.ToString();
                var number = "DN" + strippedText.ToUpper();
                return number;
            }
            catch
            {
                throw;
            }
        }

        #endregion





    }
}
