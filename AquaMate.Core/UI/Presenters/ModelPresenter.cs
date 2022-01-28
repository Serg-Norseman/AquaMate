/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Collections.Generic;
using AquaMate.Core;
using AquaMate.Core.Model;
using AquaMate.Core.Types;
using AquaMate.Logging;
using AquaMate.TSDB;
using BSLib;
using BSLib.Design;
using BSLib.Design.Graphics;
using BSLib.Design.MVP.Controls;

namespace AquaMate.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class ModelPresenter
    {
        private static readonly ILogger fLogger = LogManager.GetLogger(ALCore.LOG_FILE, ALCore.LOG_LEVEL, "ModelPresenter");


        private ModelPresenter()
        {
        }

        #region Aquariums
        #endregion

        #region Inhabitants

        public static void FillInhabitantLV(IListView listView, ILabel footer, IModel model)
        {
            try {
                listView.Clear();
                listView.AddColumn(Localizer.LS(LSID.Name), 200, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Sex), 50, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Quantity), 50, true, BSDTypes.HorizontalAlignment.Right);
                listView.AddColumn(Localizer.LS(LSID.SpeciesS), 150, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.State), 80, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Aquarium), 150, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.InclusionDate), 150, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.ExclusionDate), 150, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.LifeSpan), 150, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn("Temp", 100, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn("PH", 100, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn("GH", 100, true, BSDTypes.HorizontalAlignment.Left);

                Average avgLifespan = new Average();
                IList<Inhabitant> records = model.QueryInhabitants();
                foreach (Inhabitant rec in records) {
                    Species spc = model.GetRecord<Species>(rec.SpeciesId);

                    SpeciesType spType;
                    string spName, spTemp, spGH, spPH;
                    if (spc == null) {
                        spType = SpeciesType.Fish;
                        spName = string.Empty;
                        spTemp = string.Empty;
                        spGH = string.Empty;
                        spPH = string.Empty;
                    } else {
                        spType = spc.Type;
                        spName = spc.Name;
                        spTemp = spc.GetTempRange();
                        spGH = spc.GetGHRange();
                        spPH = spc.GetPHRange();
                    }

                    SpeciesType speciesType = model.GetSpeciesType(rec.SpeciesId);
                    ItemType itemType = ALCore.GetItemType(speciesType);

                    rec.Quantity = model.QueryInhabitantsCount(rec.Id, itemType);
                    bool fin = (rec.Quantity == 0);

                    if (fin && ALSettings.Instance.HideLosses) continue;

                    int currAqmId = 0;
                    DateTime inclusionDate, exclusionDate;
                    model.GetInhabitantDates(rec.Id, itemType, out inclusionDate, out exclusionDate, out currAqmId);

                    string aqmName = model.GetRecordName(ItemType.Aquarium, currAqmId);
                    string strInclusDate = ALCore.IsZeroDate(inclusionDate) ? string.Empty : ALCore.GetDateStr(inclusionDate);
                    string strExclusDate = ALCore.IsZeroDate(exclusionDate) || !fin ? string.Empty : ALCore.GetDateStr(exclusionDate);

                    DateTime endDate = ALCore.IsZeroDate(exclusionDate) || !fin ? DateTime.Now.Date : exclusionDate;
                    string strLifespan = ALCore.IsZeroDate(inclusionDate) ? string.Empty : ALCore.GetTimespanText(inclusionDate, endDate);

                    if (!ALCore.IsZeroDate(exclusionDate)) {
                        int iDays = (exclusionDate - inclusionDate).Days;
                        avgLifespan.AddValue(iDays);
                    }

                    ItemState itemState;
                    string strState = model.GetItemStateStr(rec.Id, itemType, out itemState);
                    if (itemState == ItemState.Unknown || !fin) {
                        strState = Localizer.LS(ALData.ItemStates[(int)rec.State]);
                    }
                    string sx = ALCore.IsAnimal(spType) ? Localizer.LS(ALData.SexNames[(int)rec.Sex]) : "–";

                    var item = listView.AddItem(rec,
                               rec.Name,
                               sx,
                               rec.Quantity.ToString(),
                               spName,
                               strState,
                               aqmName,
                               strInclusDate,
                               strExclusDate,
                               strLifespan,
                               spTemp,
                               spPH,
                               spGH
                           );

                    if (fin) {
                        item.SetForeColor(BSDConsts.Colors.Gray); // death, sale or gift?
                    }
                }

                listView.Sort(6, BSDTypes.SortOrder.Ascending);

                footer.Text = string.Format(Localizer.LS(LSID.LifeExpectancy) + ": {0}", ALCore.GetTimespanText((int)avgLifespan.GetResult()));
            } catch (Exception ex) {
                fLogger.WriteError("FillInhabitantLV()", ex);
            }
        }

        #endregion

        #region Species

        public static void FillSpeciesLV(IListView listView, IModel model)
        {
            try {
                listView.Clear();
                listView.AddColumn(Localizer.LS(LSID.Name), 200, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.ScientificName), 200, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.BioFamily), 200, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Type), 100, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn("Temp", 100, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn("PH", 100, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn("GH", 100, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.AdultSize), 100, true, BSDTypes.HorizontalAlignment.Right);
                listView.AddColumn(Localizer.LS(LSID.LifeSpan), 100, true, BSDTypes.HorizontalAlignment.Right);
                listView.AddColumn(Localizer.LS(LSID.SwimLevel), 100, true, BSDTypes.HorizontalAlignment.Right);
                listView.AddColumn(Localizer.LS(LSID.Distribution), 100, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Habitat), 100, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.CareLevel), 100, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Temperament), 100, true, BSDTypes.HorizontalAlignment.Left);

                var records = model.QuerySpecies();
                foreach (Species rec in records) {
                    string strType = Localizer.LS(ALData.SpeciesTypes[(int)rec.Type].Name);
                    string strLevel = Localizer.LS(ALData.SwimLevels[(int)rec.SwimLevel]);
                    string strCareLevel = Localizer.LS(ALData.CareLevels[(int)rec.CareLevel]);
                    string strTemperament = Localizer.LS(ALData.Temperaments[(int)rec.Temperament]);

                    var item = listView.AddItem(rec,
                               rec.Name,
                               rec.ScientificName,
                               rec.BioFamily,
                               strType,
                               rec.GetTempRange(),
                               rec.GetPHRange(),
                               rec.GetGHRange(),
                               ALCore.GetDecimalStr(rec.AdultSize),
                               ALCore.GetDecimalStr(rec.LifeSpan),
                               strLevel,
                               rec.Distribution,
                               rec.Habitat,
                               strCareLevel,
                               strTemperament
                           );
                }
            } catch (Exception ex) {
                fLogger.WriteError("FillSpeciesLV()", ex);
            }
        }

        #endregion

        #region Nutritions

        public static void FillNutritionsLV(IListView listView, IModel model)
        {
            try {
                listView.Clear();
                listView.AddColumn(Localizer.LS(LSID.Name), 100, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Brand), 50, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Amount), 100, true, BSDTypes.HorizontalAlignment.Right);
                listView.AddColumn(Localizer.LS(LSID.Note), 80, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.State), 80, true, BSDTypes.HorizontalAlignment.Left);

                var records = model.QueryNutritions();
                foreach (Nutrition rec in records) {
                    ItemState itemState;
                    string strState = model.GetItemStateStr(rec.Id, ItemType.Nutrition, out itemState);

                    bool fin = (itemState == ItemState.Finished);
                    if (fin && ALSettings.Instance.HideLosses) continue;

                    var item = listView.AddItem(rec,
                               rec.Name,
                               rec.Brand,
                               ALCore.GetDecimalStr(rec.Amount),
                               rec.Note,
                               strState
                           );

                    if (fin) {
                        item.SetForeColor(BSDConsts.Colors.Gray);
                    }
                }
            } catch (Exception ex) {
                fLogger.WriteError("FillNutritionsLV()", ex);
            }
        }

        #endregion

        #region Devices

        public static void FillDevicesLV(IListView listView, ILabel footer, IModel model)
        {
            try {
                listView.Clear();
                listView.AddColumn(Localizer.LS(LSID.Aquarium), 200, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Name), 100, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Brand), 50, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Type), 100, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Enabled), 60, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Digital), 60, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Power), 100, true, BSDTypes.HorizontalAlignment.Right);
                listView.AddColumn(Localizer.LS(LSID.WorkTime), 100, true, BSDTypes.HorizontalAlignment.Right);
                listView.AddColumn(Localizer.LS(LSID.State), 80, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Value), 80, true, BSDTypes.HorizontalAlignment.Right);

                double totalPow = 0.0d;
                var records = model.QueryDevices();
                foreach (Device rec in records) {
                    Aquarium aqm = model.GetRecord<Aquarium>(rec.AquariumId);
                    string aqmName = (aqm == null) ? "" : aqm.Name;
                    string strType = Localizer.LS(ALData.DeviceProps[(int)rec.Type].Name);

                    ItemState itemState;
                    string strState = model.GetItemStateStr(rec.Id, ItemType.Device, out itemState);

                    bool fin = (itemState == ItemState.Broken);
                    if (fin && ALSettings.Instance.HideLosses) continue;

                    var item = listView.AddItem(rec,
                               aqmName,
                               rec.Name,
                               rec.Brand,
                               strType,
                               rec.Enabled.ToString(),
                               rec.Digital.ToString(),
                               ALCore.GetDecimalStr(rec.Power),
                               ALCore.GetDecimalStr(rec.WorkTime),
                               strState,
                               string.Empty
                           );

                    if (rec.Enabled) {
                        totalPow += (rec.Power /* W/h */ * rec.WorkTime /* h/day */);
                    }

                    if (fin) {
                        item.SetForeColor(BSDConsts.Colors.Gray);
                    }
                }

                totalPow /= 1000.0d;
                double electricCost = totalPow * ALData.kWhCost;
                footer.Text = string.Format(Localizer.LS(LSID.PowerFooter), totalPow, electricCost);
            } catch (Exception ex) {
                fLogger.WriteError("FillDevicesLV()", ex);
            }
        }

        #endregion

        #region Inventory

        public static void FillInventoryLV(IListView listView, IModel model)
        {
            try {
                listView.Clear();
                listView.AddColumn(Localizer.LS(LSID.Name), 100, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Brand), 50, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Type), 75, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Note), 50, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.State), 80, true, BSDTypes.HorizontalAlignment.Left);

                var records = model.QueryInventory();
                foreach (Inventory rec in records) {
                    string strType = Localizer.LS(ALData.InventoryTypes[(int)rec.Type].Name);

                    ItemType itemType = ALCore.GetItemType(rec.Type);
                    ItemState itemState;
                    string strState = model.GetItemStateStr(rec.Id, itemType, out itemState);

                    bool fin = (itemState == ItemState.Finished || itemState == ItemState.Broken);
                    if (fin && ALSettings.Instance.HideLosses) continue;

                    var item = listView.AddItem(rec,
                               rec.Name,
                               rec.Brand,
                               strType,
                               rec.Note,
                               strState
                           );

                    if (fin) {
                        item.SetForeColor(BSDConsts.Colors.Gray);
                    }
                }
            } catch (Exception ex) {
                fLogger.WriteError("FillInventoryLV()", ex);
            }
        }

        #endregion

        #region Maintenances

        public static void FillMaintenancesLV(IListView listView, IModel model, string selectedAquarium)
        {
            try {
                listView.Clear();
                listView.AddColumn(Localizer.LS(LSID.Aquarium), 120, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Date), 120, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Type), 100, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Value), 100, true, BSDTypes.HorizontalAlignment.Right);
                listView.AddColumn(Localizer.LS(LSID.Note), 250, true, BSDTypes.HorizontalAlignment.Left);

                var records = model.QueryMaintenances();
                foreach (Maintenance rec in records) {
                    Aquarium aqm = model.Cache.Get<Aquarium>(ItemType.Aquarium, rec.AquariumId);
                    string aqmName = (aqm == null) ? "" : aqm.Name;
                    if (selectedAquarium != "*" && selectedAquarium != aqmName) continue;

                    string strType = Localizer.LS(ALData.MaintenanceTypes[(int)rec.Type].Name);

                    var item = listView.AddItem(rec,
                               aqmName,
                               ALCore.GetTimeStr(rec.Timestamp),
                               strType,
                               ALCore.GetDecimalStr(rec.Value),
                               rec.Note
                           );
                }
            } catch (Exception ex) {
                fLogger.WriteError("FillMaintenancesLV()", ex);
            }
        }

        #endregion

        #region Measures

        public static void FillMeasuresLV(IListView listView, IModel model, string selectedAquarium)
        {
            try {
                listView.Clear();
                listView.AddColumn(Localizer.LS(LSID.Aquarium), 120, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Timestamp), 120, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn("Temp (°C)", 60, true, BSDTypes.HorizontalAlignment.Right);
                listView.AddColumn("NO3 (mg/l)", 60, true, BSDTypes.HorizontalAlignment.Right);
                listView.AddColumn("NO2 (mg/l)", 60, true, BSDTypes.HorizontalAlignment.Right);
                listView.AddColumn("GH (°d)", 60, true, BSDTypes.HorizontalAlignment.Right);
                listView.AddColumn("KH (°d)", 60, true, BSDTypes.HorizontalAlignment.Right);
                listView.AddColumn("pH", 60, true, BSDTypes.HorizontalAlignment.Right);
                listView.AddColumn("Cl2 (mg/l)", 60, true, BSDTypes.HorizontalAlignment.Right);
                listView.AddColumn("CO2", 60, true, BSDTypes.HorizontalAlignment.Right);
                listView.AddColumn("NHtot", 60, true, BSDTypes.HorizontalAlignment.Right);
                listView.AddColumn("NH3", 60, true, BSDTypes.HorizontalAlignment.Right);
                listView.AddColumn("NH4", 60, true, BSDTypes.HorizontalAlignment.Right);
                listView.AddColumn("PO4", 60, true, BSDTypes.HorizontalAlignment.Right);

                var records = model.QueryMeasures();
                foreach (Measure rec in records) {
                    Aquarium aqm = model.Cache.Get<Aquarium>(ItemType.Aquarium, rec.AquariumId);
                    string aqmName = (aqm == null) ? "" : aqm.Name;
                    if (selectedAquarium != "*" && selectedAquarium != aqmName) continue;

                    var item = listView.AddItem(rec,
                               aqmName,
                               ALCore.GetTimeStr(rec.Timestamp),
                               ALCore.GetDecimalStr(rec.Temperature, 2, true),
                               ALCore.GetDecimalStr(rec.NO3, 2, true),
                               ALCore.GetDecimalStr(rec.NO2, 2, true),
                               ALCore.GetDecimalStr(rec.GH, 2, true),
                               ALCore.GetDecimalStr(rec.KH, 2, true),
                               ALCore.GetDecimalStr(rec.pH, 2, true),
                               ALCore.GetDecimalStr(rec.Cl2, 2, true),
                               ALCore.GetDecimalStr(rec.CO2, 2, true),
                               ALCore.GetDecimalStr(rec.NH, 2, true),
                               ALCore.GetDecimalStr(rec.NH3, 2, true),
                               ALCore.GetDecimalStr(rec.NH4, 2, true),
                               ALCore.GetDecimalStr(rec.PO4, 2, true)
                           );
                }
            } catch (Exception ex) {
                fLogger.WriteError("FillMeasuresLV()", ex);
            }
        }

        #endregion

        #region Notes

        public static void FillNotesLV(IListView listView, IModel model)
        {
            try {
                listView.Clear();
                listView.AddColumn(Localizer.LS(LSID.Aquarium), 120, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Date), 120, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Event), 100, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Text), 250, true, BSDTypes.HorizontalAlignment.Left);

                var records = model.QueryNotes();
                foreach (Note rec in records) {
                    Aquarium aqm = model.GetRecord<Aquarium>(rec.AquariumId);
                    string aqmName = (aqm == null) ? "" : aqm.Name;

                    var item = listView.AddItem(rec,
                               aqmName,
                               ALCore.GetTimeStr(rec.Timestamp),
                               rec.Event,
                               rec.Content
                           );
                }
            } catch (Exception ex) {
                fLogger.WriteError("FillNotesLV()", ex);
            }
        }

        #endregion

        #region Schedule

        public static void FillScheduleLV(IListView listView, IModel model)
        {
            try {
                listView.Clear();
                listView.AddColumn(Localizer.LS(LSID.Aquarium), 120, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Date), 120, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Event), 100, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Reminder), 80, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Type), 80, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Status), 80, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Note), 250, true, BSDTypes.HorizontalAlignment.Left);

                var records = model.QuerySchedule();
                foreach (Schedule rec in records) {
                    Aquarium aqm = model.GetRecord<Aquarium>(rec.AquariumId);
                    string aqmName = (aqm == null) ? "" : aqm.Name;
                    string strType = Localizer.LS(ALData.ScheduleTypes[(int)rec.Type]);
                    string strStatus = Localizer.LS(ALData.TaskStatuses[(int)rec.Status]);

                    var item = listView.AddItem(rec,
                               aqmName,
                               ALCore.GetTimeStr(rec.Timestamp),
                               rec.Event,
                               rec.Reminder.ToString(),
                               strType,
                               strStatus,
                               rec.Note
                           );
                }
            } catch (Exception ex) {
                fLogger.WriteError("FillScheduleLV()", ex);
            }
        }

        #endregion

        #region Transfers

        public static void FillTransfersLV(IListView listView, IModel model, IFont boldFont)
        {
            listView.Clear();
            listView.AddColumn(Localizer.LS(LSID.Date), 80, true, BSDTypes.HorizontalAlignment.Left);
            listView.AddColumn(Localizer.LS(LSID.Brand), 50, true, BSDTypes.HorizontalAlignment.Left);
            listView.AddColumn(Localizer.LS(LSID.Item), 140, true, BSDTypes.HorizontalAlignment.Left);
            listView.AddColumn(Localizer.LS(LSID.Type), 80, true, BSDTypes.HorizontalAlignment.Left);
            listView.AddColumn(Localizer.LS(LSID.SourceTank), 80, true, BSDTypes.HorizontalAlignment.Left);
            listView.AddColumn(Localizer.LS(LSID.TargetTank), 80, true, BSDTypes.HorizontalAlignment.Left);
            listView.AddColumn(Localizer.LS(LSID.Quantity), 80, true, BSDTypes.HorizontalAlignment.Right);
            listView.AddColumn(Localizer.LS(LSID.UnitPrice), 80, true, BSDTypes.HorizontalAlignment.Right);
            listView.AddColumn(Localizer.LS(LSID.Shop), 180, true, BSDTypes.HorizontalAlignment.Left);
            listView.AddColumn(Localizer.LS(LSID.Cause), 80, true, BSDTypes.HorizontalAlignment.Left);

            var records = model.QueryTransfers();
            foreach (Transfer rec in records) {
                ItemType itemType = rec.ItemType;

                Aquarium aqmSour = model.Cache.Get<Aquarium>(ItemType.Aquarium, rec.SourceId);
                Aquarium aqmTarg = model.Cache.Get<Aquarium>(ItemType.Aquarium, rec.TargetId);

                var itemRec = model.GetRecord(rec.ItemType, rec.ItemId);
                string itName = (itemRec == null) ? string.Empty : itemRec.ToString();
                string strType = Localizer.LS(ALData.TransferTypes[(int)rec.Type]);
                var brandedItem = itemRec as IBrandedItem;
                string brand = (brandedItem == null) ? "-" : brandedItem.Brand;

                var item = listView.AddItem(rec,
                               ALCore.GetDateStr(rec.Timestamp),
                               brand,
                               itName,
                               strType,
                               (aqmSour == null) ? string.Empty : aqmSour.Name,
                               (aqmTarg == null) ? string.Empty : aqmTarg.Name,
                               rec.Quantity.ToString(),
                               ALCore.GetDecimalStr(rec.UnitPrice),
                               rec.Shop,
                               rec.Cause
                           );

                if (itemType == ItemType.Aquarium) {
                    item.SetFont(boldFont);
                }

                switch (rec.Type) {
                    case TransferType.Sale:
                        item.SetForeColor(BSDConsts.Colors.DimGray);
                        break;

                    case TransferType.Death:
                    case TransferType.Exclusion:
                        item.SetForeColor(BSDConsts.Colors.Gray);
                        break;
                }

                // validation after format changes
                /*if (itemRec is Inventory) {
                    var inv = itemRec as Inventory;
                    var invType = ALCore.GetItemType(inv.Type);
                    if (invType != itemType) {
                        item.ForeColor = Color.Red;
                    }
                }*/
            }
        }

        public static void FillTransfersLVPreview(IListView listView, IModel model, Entity item, bool ftAquariums = true)
        {
            try {
                ItemType itemType;
                switch (item.EntityType) {
                    case EntityType.Aquarium:
                        itemType = ItemType.Aquarium;
                        break;
                    case EntityType.Inhabitant:
                        var hItem = item as Inhabitant;
                        SpeciesType speciesType = model.GetSpeciesType(hItem.SpeciesId);
                        itemType = ALCore.GetItemType(speciesType);
                        break;
                    case EntityType.Nutrition:
                        itemType = ItemType.Nutrition;
                        break;
                    case EntityType.Device:
                        itemType = ItemType.Device;
                        break;
                    case EntityType.Inventory:
                        var invItem = item as Inventory;
                        itemType = ALCore.GetItemType(invItem.Type);
                        break;
                    default:
                        itemType = ItemType.None;
                        break;
                }

                int itemId = item.Id;

                listView.BeginUpdate();

                listView.Clear();
                listView.AddColumn(Localizer.LS(LSID.Date), 80, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Type), 80, true, BSDTypes.HorizontalAlignment.Left);
                if (ftAquariums) {
                    listView.AddColumn(Localizer.LS(LSID.SourceTank), 80, true, BSDTypes.HorizontalAlignment.Left);
                    listView.AddColumn(Localizer.LS(LSID.TargetTank), 80, true, BSDTypes.HorizontalAlignment.Left);
                }
                listView.AddColumn(Localizer.LS(LSID.Quantity), 80, true, BSDTypes.HorizontalAlignment.Right);
                listView.AddColumn(Localizer.LS(LSID.UnitPrice), 80, true, BSDTypes.HorizontalAlignment.Right);
                listView.AddColumn(Localizer.LS(LSID.Shop), 180, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Cause), 80, true, BSDTypes.HorizontalAlignment.Left);

                //Font defFont = listView.Font;
                //Font boldFont = new Font(defFont, FontStyle.Bold);

                var records = model.QueryTransfers(itemId, (int)itemType);
                foreach (Transfer rec in records) {
                    var itemRec = model.GetRecord(rec.ItemType, rec.ItemId);
                    string itName = (itemRec == null) ? string.Empty : itemRec.ToString();
                    string strType = Localizer.LS(ALData.TransferTypes[(int)rec.Type]);

                    IListItem listItem;
                    if (ftAquariums) {
                        Aquarium aqmSour = model.Cache.Get<Aquarium>(ItemType.Aquarium, rec.SourceId);
                        Aquarium aqmTarg = model.Cache.Get<Aquarium>(ItemType.Aquarium, rec.TargetId);

                        listItem = listView.AddItem(rec,
                                       ALCore.GetDateStr(rec.Timestamp),
                                       strType,
                                       (aqmSour == null) ? string.Empty : aqmSour.Name,
                                       (aqmTarg == null) ? string.Empty : aqmTarg.Name,
                                       rec.Quantity.ToString(),
                                       ALCore.GetDecimalStr(rec.UnitPrice),
                                       rec.Shop,
                                       rec.Cause
                                   );
                    } else {
                        listItem = listView.AddItem(rec,
                                       ALCore.GetDateStr(rec.Timestamp),
                                       strType,
                                       rec.Quantity.ToString(),
                                       ALCore.GetDecimalStr(rec.UnitPrice),
                                       rec.Shop,
                                       rec.Cause
                                   );
                    }

                    if (itemType == ItemType.Aquarium) {
                        //listItem.Font = boldFont;
                    }

                    switch (rec.Type) {
                        case TransferType.Sale:
                            listItem.SetForeColor(BSDConsts.Colors.DimGray);
                            break;

                        case TransferType.Death:
                        case TransferType.Exclusion:
                            listItem.SetForeColor(BSDConsts.Colors.Gray);
                            break;
                    }
                }

                listView.EndUpdate();
            } catch (Exception ex) {
                fLogger.WriteError("FillTransfersLV()", ex);
            }
        }

        #endregion

        #region Budget

        public static void FillBudgetLV(IListView listView, IModel model, IList<Transfer> records, IFont boldFont)
        {
            try {
                listView.Clear();
                listView.AddColumn(Localizer.LS(LSID.Date), 80, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Type), 80, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Brand), 50, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Item), 140, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Quantity), 80, true, BSDTypes.HorizontalAlignment.Right);
                listView.AddColumn(Localizer.LS(LSID.UnitPrice), 80, true, BSDTypes.HorizontalAlignment.Right);
                listView.AddColumn(Localizer.LS(LSID.Sum), 80, true, BSDTypes.HorizontalAlignment.Right);
                listView.AddColumn(Localizer.LS(LSID.Shop), 180, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.State), 80, true, BSDTypes.HorizontalAlignment.Left);

                foreach (Transfer rec in records) {
                    int factor = 0;
                    switch (rec.Type) {
                        case TransferType.Purchase:
                            factor = -1;
                            break;
                        case TransferType.Sale:
                            factor = +1;
                            break;
                    }

                    if (factor != 0) {
                        double sum = (rec.Quantity * rec.UnitPrice) * factor;

                        if (listView != null) {
                            ItemType itemType = rec.ItemType;
                            var itemRec = model.GetRecord(itemType, rec.ItemId);
                            string itName = (itemRec == null) ? string.Empty : itemRec.ToString();

                            ItemState itemState;
                            string strState = model.GetItemStateStr(rec.ItemId, itemType, out itemState);

                            var brandedItem = itemRec as IBrandedItem;
                            string brand = (brandedItem == null) ? "-" : brandedItem.Brand;

                            var item = listView.AddItem(rec,
                                       ALCore.GetDateStr(rec.Timestamp),
                                       Localizer.LS(ALData.ItemTypes[(int)rec.ItemType].Name),
                                       brand,
                                       itName,
                                       rec.Quantity.ToString(),
                                       ALCore.GetDecimalStr(rec.UnitPrice),
                                       ALCore.GetDecimalStr(sum),
                                       rec.Shop,
                                       strState
                                   );

                            if (itemType == ItemType.Aquarium) {
                                item.SetFont(boldFont);
                            }
                        }
                    }
                }
            } catch (Exception ex) {
                fLogger.WriteError("FillBudgetLV()", ex);
            }
        }

        public static void FillPricelistLV(IListView listView, IModel model, IList<Transfer> records)
        {
            if (listView == null) return;

            try {
                listView.Clear();
                listView.AddColumn(Localizer.LS(LSID.Type), 80, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Brand), 50, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Item), 140, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Shop), 180, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Date), 80, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.UnitPrice), 80, true, BSDTypes.HorizontalAlignment.Right);

                foreach (Transfer rec in records) {
                    if (rec.Type == TransferType.Purchase && rec.UnitPrice != 0.0f) {
                        ItemType itemType = rec.ItemType;
                        var itemRec = model.GetRecord(itemType, rec.ItemId);
                        string itName = (itemRec == null) ? string.Empty : itemRec.ToString();

                        var brandedItem = itemRec as IBrandedItem;
                        string brand = (brandedItem == null) ? "-" : brandedItem.Brand;

                        listView.AddItem(rec,
                            Localizer.LS(ALData.ItemTypes[(int)rec.ItemType].Name),
                            brand,
                            itName,
                            rec.Shop,
                            ALCore.GetDateStr(rec.Timestamp),
                            ALCore.GetDecimalStr(rec.UnitPrice)
                        );
                    }
                }

                listView.Sort(2, BSDTypes.SortOrder.Ascending);
            } catch (Exception ex) {
                fLogger.WriteError("FillPricelistLV()", ex);
            }
        }

        #endregion

        #region Brands

        public static void FillBrandsLV(IListView listView, IModel model)
        {
            try {
                listView.Clear();
                listView.AddColumn(Localizer.LS(LSID.Name), 120, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Country), 120, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.WebSite), 120, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Email), 120, true, BSDTypes.HorizontalAlignment.Left);

                var records = model.QueryBrands();
                foreach (Brand rec in records) {
                    var item = listView.AddItem(rec,
                               rec.Name,
                               rec.Country,
                               rec.WebSite,
                               rec.Email
                           );
                }

                listView.Sort(0, BSDTypes.SortOrder.Ascending);
            } catch (Exception ex) {
                fLogger.WriteError("FillBrandsLV()", ex);
            }
        }

        #endregion

        #region Shops

        public static void FillShopsLV(IListView listView, IModel model)
        {
            try {
                listView.Clear();
                listView.AddColumn(Localizer.LS(LSID.Name), 120, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Address), 120, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Telephone), 120, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.WebSite), 120, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Email), 120, true, BSDTypes.HorizontalAlignment.Left);

                var records = model.QueryShops();
                foreach (Shop rec in records) {
                    var item = listView.AddItem(rec,
                               rec.Name,
                               rec.Address,
                               rec.Telephone,
                               rec.WebSite,
                               rec.Email
                           );
                }

                listView.Sort(0, BSDTypes.SortOrder.Ascending);
            } catch (Exception ex) {
                fLogger.WriteError("FillShopsLV()", ex);
            }
        }

        #endregion

        #region Snapshots

        public static void FillSnapshotsLV(IListView listView, IModel model)
        {
            try {
                listView.Clear();
                listView.AddColumn(Localizer.LS(LSID.Name), 120, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Date), 120, true, BSDTypes.HorizontalAlignment.Left);

                var records = model.QuerySnapshots();
                foreach (Snapshot rec in records) {
                    var item = listView.AddItem(rec,
                               rec.Name,
                               ALCore.GetTimeStr(rec.Timestamp)
                           );
                }
            } catch (Exception ex) {
                fLogger.WriteError("FillSnapshotsLV()", ex);
            }
        }

        #endregion

        #region TSPoints

        public static void FillTSPointsLV(IListView listView, IModel model)
        {
            try {
                listView.Clear();
                listView.AddColumn(Localizer.LS(LSID.Name), 140, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Unit), 80, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Min), 80, true, BSDTypes.HorizontalAlignment.Right);
                listView.AddColumn(Localizer.LS(LSID.Max), 80, true, BSDTypes.HorizontalAlignment.Right);
                listView.AddColumn(Localizer.LS(LSID.Deviation), 80, true, BSDTypes.HorizontalAlignment.Right);
                listView.AddColumn("SID", 140, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Value), 80, true, BSDTypes.HorizontalAlignment.Right);

                TSDatabase tsdb = model.TSDB;
                var records = tsdb.GetPoints();
                foreach (TSPoint rec in records) {
                    var item = listView.AddItem(rec,
                               rec.Name,
                               rec.MeasureUnit,
                               ALCore.GetDecimalStr(rec.Min),
                               ALCore.GetDecimalStr(rec.Max),
                               ALCore.GetDecimalStr(rec.Deviation),
                               rec.SID,
                               string.Empty
                           );
                }
            } catch (Exception ex) {
                fLogger.WriteError("FillTSPointsLV()", ex);
            }
        }

        #endregion

        #region TSValues

        public static void FillTSValuesLV(IListView listView, IModel model, int pointId)
        {
            try {
                listView.Clear();
                listView.AddColumn(Localizer.LS(LSID.Timestamp), 140, true, BSDTypes.HorizontalAlignment.Left);
                listView.AddColumn(Localizer.LS(LSID.Value), 120, true, BSDTypes.HorizontalAlignment.Right);

                TSDatabase tsdb = model.TSDB;
                var records = tsdb.QueryValues(pointId, DateTime.Now.AddDays(-60), DateTime.Now);
                foreach (TSValue rec in records) {
                    var item = listView.AddItem(rec,
                               ALCore.GetTimeStr(rec.Timestamp),
                               ALCore.GetDecimalStr(rec.Value)
                           );
                }
            } catch (Exception ex) {
                fLogger.WriteError("FillTSValuesLV()", ex);
            }
        }

        #endregion
    }
}
