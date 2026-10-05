/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using AquaMate.Core.Model;
using AquaMate.Core.Types;
using AquaMate.DataCollection;
using AquaMate.Logging;
using AquaMate.TSDB;
using AquaMate.UI;
using BSLib;
using BSLib.Design.MVP.Controls;
using SQLite;

namespace AquaMate.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class ALModel : IModel
    {
        private readonly ILogger fLogger = LogManager.GetLogger(ALCore.LOG_FILE, ALCore.LOG_LEVEL, "ALModel");

        private readonly IBrowser fBrowser;
        private readonly EntitiesCache fCache;
        private readonly SQLiteConnection fDB;
        private readonly TSDatabase fTSDB;
        private IChannel fChannel;


        public IBrowser Browser
        {
            get { return fBrowser; }
        }

        public EntitiesCache Cache
        {
            get { return fCache; }
        }

        public TSDatabase TSDB
        {
            get { return fTSDB; }
        }


        public event DataReceivedEventHandler ReceivedData;


        static ALModel()
        {
            //SQLiteLoader.Load();
        }

        public ALModel(IBrowser browser)
        {
            fBrowser = browser;

            fTSDB = new TSDatabase();

            var databasePath = Path.Combine(AppHost.GetAppDataPath(), "ALData.db");
            fDB = new SQLiteConnection(databasePath, storeDateTimeAsTicks: false);

            fDB.CreateTable<Aquarium>();

            fDB.CreateTable<Inhabitant>();
            fDB.CreateTable<Species>();

            fDB.CreateTable<Device>();
            fDB.CreateTable<Note>();
            fDB.CreateTable<Maintenance>();
            fDB.CreateTable<Transfer>();
            fDB.CreateTable<Nutrition>();
            fDB.CreateTable<Measure>();
            fDB.CreateTable<Schedule>();
            fDB.CreateTable<Inventory>();

            fDB.CreateTable<Snapshot>();
            fDB.CreateTable<Brand>();
            fDB.CreateTable<Shop>();

            fCache = new EntitiesCache(this);
        }

        /// <summary>
        /// Cleaning waste space
        /// </summary>
        public void CleanSpace()
        {
            var transfers = QueryTransfers();
            foreach (Transfer rec in transfers) {
                var itemRec = GetRecord(rec.ItemType, rec.ItemId);
                if (itemRec == null) continue;

                bool valid = true;
                ItemType sourItemType = ItemType.None;
                switch (itemRec.EntityType) {
                    case EntityType.Aquarium:
                    case EntityType.Species:
                    case EntityType.Nutrition:
                    case EntityType.Device:
                    case EntityType.Maintenance:
                    case EntityType.Measure:
                    case EntityType.Note:
                    case EntityType.Schedule:
                    case EntityType.Transfer:
                    case EntityType.Snapshot:
                    case EntityType.Brand:
                    case EntityType.Shop:
                    case EntityType.TSPoint:
                        break;

                    case EntityType.Inhabitant:
                        if (itemRec is Inhabitant inhab) {
                            var species = GetRecord<Species>(inhab.SpeciesId);
                            sourItemType = ALCore.GetItemType(species.Type);
                            valid = rec.ItemType == sourItemType;
                        }
                        break;

                    case EntityType.Inventory:
                        if (itemRec is Inventory invent) {
                            sourItemType = ALCore.GetItemType(invent.Type);
                            valid = rec.ItemType == sourItemType;
                        }
                        break;
                }

                if (!valid) {
                    string str = string.Format("itemId={0}, itemType={1}, sourItemType={2}", rec.ItemId, rec.ItemType, sourItemType);
                    System.Diagnostics.Debug.WriteLine(str);
                }
            }

            fDB.Execute("drop table if exists Expense");
            fDB.Execute("VACUUM;");
        }

        #region Records

        public void Execute(string query, params object[] args)
        {
            fDB.Execute(query, args);
        }

        public int AddRecord(Entity obj)
        {
            return fDB.Insert(obj);
        }

        public void UpdateRecord(Entity obj)
        {
            fDB.Update(obj);
        }

        public void DeleteRecord(Entity obj)
        {
            fDB.Delete(obj);
        }

        public void DeleteRecord<T>(int objId)
        {
            fDB.Delete<T>(objId);
        }

        // TODO: need to add control of keys of deleted records
        public T GetRecord<T>(int objId) where T : new()
        {
            T result;
            if (objId <= 0) {
                result = default(T);
            } else {
                try {
                    result = fDB.Get<T>(objId);
                } catch (InvalidOperationException) {
                    // record not exists
                    result = default(T);
                }
            }
            return result;
        }

        public Entity GetRecord(ItemType itemType, int itemId)
        {
            Entity result = null;
            switch (itemType) {
                case ItemType.Aquarium:
                    result = GetRecord<Aquarium>(itemId);
                    break;

                case ItemType.Fish:
                case ItemType.Invertebrate:
                case ItemType.Plant:
                case ItemType.Coral:
                    result = GetRecord<Inhabitant>(itemId);
                    break;

                case ItemType.Nutrition:
                    result = GetRecord<Nutrition>(itemId);
                    break;

                case ItemType.Device:
                    result = GetRecord<Device>(itemId);
                    break;

                case ItemType.Additive:
                case ItemType.Chemistry:
                case ItemType.Equipment:
                case ItemType.Maintenance:
                case ItemType.Furniture:
                case ItemType.Decoration:
                case ItemType.Soil:
                    result = GetRecord<Inventory>(itemId);
                    break;
            }
            return result;
        }

        public string GetRecordName(ItemType itemType, int itemId)
        {
            Entity itemRec = GetRecord(itemType, itemId);
            return (itemRec == null) ? string.Empty : itemRec.ToString();
        }

        public IList<T> QueryRecords<T>(string query, params object[] args) where T : new()
        {
            return fDB.Query<T>(query, args);
        }

        public string GetEntityName(Entity entity)
        {
            string result = string.Empty;

            switch (entity.EntityType) {
                case EntityType.Aquarium:
                case EntityType.Inhabitant:
                case EntityType.Species:
                case EntityType.Nutrition:
                case EntityType.Device:
                case EntityType.Inventory:
                case EntityType.Note:
                case EntityType.Schedule:
                case EntityType.Snapshot:
                case EntityType.Brand:
                case EntityType.Shop:
                case EntityType.TSPoint:
                    result = entity.ToString();
                    break;

                case EntityType.Maintenance: {
                        var mntRec = entity as Maintenance;
                        string strType = Localizer.LS(ALData.MaintenanceTypes[(int)mntRec.Type].Name);
                        string timestamp = ALCore.GetDateStr(mntRec.Timestamp);
                        result = strType + " [" + timestamp + "]";
                    }
                    break;

                case EntityType.Measure: {
                        var msrRec = entity as Measure;
                        string strType = Localizer.LS(LSID.Measure);
                        string timestamp = ALCore.GetDateStr(msrRec.Timestamp);
                        result = strType + " [" + timestamp + "]";
                    }
                    break;

                case EntityType.Transfer: {
                        var trnRec = entity as Transfer;
                        var itemRec = GetRecord(trnRec.ItemType, trnRec.ItemId);
                        string itName = (itemRec == null) ? string.Empty : itemRec.ToString();
                        string strType = Localizer.LS(ALData.TransferTypes[(int)trnRec.Type]);
                        string timestamp = ALCore.GetDateStr(trnRec.Timestamp);
                        result = strType + " [" + timestamp + ", " + itName + "]";
                    }
                    break;

                default:
                    result = entity.ToString();
                    break;
            }

            return result;
        }

        #endregion

        #region Aquarium functions

        public IList<Aquarium> QueryAquariums()
        {
            return fDB.Query<Aquarium>("select * from Aquarium");
        }

        public IList<ComboItem<int>> QueryAquariumsList(bool showInactive)
        {
            var result = new List<ComboItem<int>>();
            var queryResult = QueryAquariums();
            foreach (var aqm in queryResult) {
                var workTime = GetWorkTime(aqm);
                if (!workTime.IsActive() && !showInactive)
                    continue;

                result.Add(new ComboItem<int>(aqm.Name, aqm.Id));
            }
            return result;
        }

        public IList<string> QueryAquariumBrands()
        {
            var queryResult = fDB.Query<QString>("select distinct Brand as element from Aquarium");
            return ALData.GetStringList(queryResult);
        }

        public int QueryInhabitantsCount(int aquariumId)
        {
            int result = 0;
            var qv = fDB.Query<Transfer>("select * from Transfer where SourceId = ? or TargetId = ?", aquariumId, aquariumId);
            foreach (var val in qv) {
                int factor = 0;
                switch (val.Type) {
                    case TransferType.Relocation:
                        if (val.SourceId == aquariumId) {
                            factor = -1;
                        } else if (val.TargetId == aquariumId) {
                            factor = +1;
                        }
                        break;

                    case TransferType.Purchase:
                    case TransferType.Birth:
                        factor = +1;
                        break;

                    case TransferType.Sale:
                    case TransferType.Death:
                        factor = -1;
                        break;
                }

                switch (val.ItemType) {
                    case ItemType.Fish:
                    case ItemType.Invertebrate:
                    case ItemType.Plant:
                        result += ((int)val.Quantity * factor);
                        break;
                }
            }
            return result;
        }

        public int QueryInhabitantsCount(int itemId, ItemType itemType)
        {
            int result = 0;
            var qv = fDB.Query<Transfer>("select Type, Quantity from Transfer where ItemType = ? and ItemId = ?", (int)itemType, itemId);
            foreach (var val in qv) {
                int factor = 0;
                switch (val.Type) {
                    case TransferType.Relocation:
                        break;
                    case TransferType.Purchase:
                    case TransferType.Birth:
                        factor = +1;
                        break;
                    case TransferType.Sale:
                    case TransferType.Death:
                        factor = -1;
                        break;
                }
                result += ((int)val.Quantity * factor);
            }
            return result;
        }

        public void GetInhabitantDates(int recId, ItemType itemType, out DateTime inclusionDate, out DateTime exclusionDate, out int currAqmId)
        {
            currAqmId = -1;
            inclusionDate = ALCore.ZeroDate;
            exclusionDate = ALCore.ZeroDate;

            IList<Transfer> transfers = QueryTransfers(recId, (int)itemType);
            int count = 0;
            foreach (var trf in transfers) {
                switch (trf.Type) {
                    case TransferType.Relocation:
                        break;

                    case TransferType.Purchase:
                    case TransferType.Birth:
                        count += (int)trf.Quantity;
                        if (ALCore.IsZeroDate(inclusionDate)) {
                            inclusionDate = trf.Timestamp;
                        }
                        break;

                    case TransferType.Sale:
                    case TransferType.Death:
                        count -= (int)trf.Quantity;
                        if (count == 0) {
                            exclusionDate = trf.Timestamp;
                        }
                        break;
                }

                if (trf.TargetId > 0) {
                    currAqmId = trf.TargetId;
                }
            }
        }

        public void GetItemState(int recId, ItemType itemType, out ItemState itemState, out DateTime exclusionDate)
        {
            itemState = ItemState.Unknown;
            exclusionDate = ALCore.ZeroDate;

            IList<Transfer> transfers = QueryTransfers(recId, (int)itemType);
            foreach (var trf in transfers) {
                switch (trf.Type) {
                    case TransferType.Death:
                        if (ALCore.IsZeroDate(exclusionDate)) {
                            itemState = ItemState.Dead;
                            exclusionDate = trf.Timestamp;
                        }
                        break;

                    case TransferType.Sale:
                        if (ALCore.IsZeroDate(exclusionDate)) {
                            itemState = ItemState.Sold;
                            exclusionDate = trf.Timestamp;
                        }
                        break;

                    case TransferType.Exclusion:
                        if (ALCore.IsZeroDate(exclusionDate)) {
                            itemState = ALData.ItemTypes[(int)itemType].ExclusionState;
                            exclusionDate = trf.Timestamp;
                        }
                        break;
                }
            }
        }

        public string GetItemStateStr(int recId, ItemType itemType, out ItemState itemState)
        {
            DateTime exclusionDate;
            GetItemState(recId, itemType, out itemState, out exclusionDate);
            string strState;
            if (itemState == ItemState.Unknown) {
                strState = string.Empty;
            } else {
                string strExclusDate = ALCore.IsZeroDate(exclusionDate) ? "-" : ALCore.GetDateStr(exclusionDate);
                strState = string.Format("{0} [{1}]", Localizer.LS(ALData.ItemStates[(int)itemState]), strExclusDate);
            }
            return strState;
        }

        #endregion

        #region Inhabitant functions

        public IList<Inhabitant> QueryInhabitants()
        {
            return fDB.Query<Inhabitant>("select * from Inhabitant");
        }

        public IList<Inhabitant> QueryInhabitants(int aquariumId)
        {
            return fDB.Query<Inhabitant>("select inh.Id, inh.SpeciesId, inh.Sex, inh.Name from Inhabitant inh, Transfer tran where (inh.Id = tran.ItemId and tran.ItemType in (2, 3, 4, 5) and TargetId = ?)", aquariumId);
        }

        public List<InhabitantDispItem> PrepareInhabitants(int aquariumId)
        {
            var result = new List<InhabitantDispItem>();

            IList<Inhabitant> records = QueryInhabitants();
            foreach (Inhabitant rec in records) {
                Species spc = GetRecord<Species>(rec.SpeciesId);

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

                SpeciesType speciesType = GetSpeciesType(rec.SpeciesId);
                ItemType itemType = ALCore.GetItemType(speciesType);

                rec.Quantity = QueryInhabitantsCount(rec.Id, itemType);
                bool fin = (rec.Quantity == 0);

                if (fin && ALSettings.Instance.HideLosses) continue;

                int currAqmId = 0;
                DateTime inclusionDate, exclusionDate;
                GetInhabitantDates(rec.Id, itemType, out inclusionDate, out exclusionDate, out currAqmId);

                if (aquariumId > 0 && currAqmId != aquariumId) continue;

                string aqmName = GetRecordName(ItemType.Aquarium, currAqmId);
                string strInclusDate = ALCore.IsZeroDate(inclusionDate) ? string.Empty : ALCore.GetDateStr(inclusionDate);
                string strExclusDate = ALCore.IsZeroDate(exclusionDate) || !fin ? string.Empty : ALCore.GetDateStr(exclusionDate);

                DateTime endDate = ALCore.IsZeroDate(exclusionDate) || !fin ? DateTime.Now.Date : exclusionDate;
                string strLifespan = ALCore.IsZeroDate(inclusionDate) ? string.Empty : ALCore.GetTimespanText(inclusionDate, endDate);

                int iDays = (!ALCore.IsZeroDate(exclusionDate)) ? (exclusionDate - inclusionDate).Days : 0;

                ItemState itemState;
                string strState = GetItemStateStr(rec.Id, itemType, out itemState);
                if (itemState == ItemState.Unknown || !fin) {
                    strState = Localizer.LS(ALData.ItemStates[(int)rec.State]);
                }
                string sx = ALCore.IsAnimal(spType) ? Localizer.LS(ALData.SexNames[(int)rec.Sex]) : "–";

                var inhDisp = new InhabitantDispItem() {
                    Id = rec.Id,
                    Name = rec.Name,
                    Sex = rec.Sex,
                    SexName = sx,
                    Quantity = rec.Quantity,
                    SpeciesId = rec.SpeciesId,
                    SpeciesName = spName,
                    State = rec.State,
                    StateStr = strState,
                    AquariumId = rec.AquariumId,
                    AquariumName = aqmName,
                    InclusionDate = strInclusDate,
                    ExclusionDate = strExclusDate,
                    LifeSpan = strLifespan,
                    Temp = spTemp,
                    PH = spPH,
                    GH = spGH,
                    iDays = iDays,
                    Fin = fin,
                    Note = rec.Note,
                    ItemType = itemType
                };

                result.Add(inhDisp);
            }

            return result;
        }

        #endregion

        #region Species functions

        public IList<Species> QuerySpecies()
        {
            return fDB.Query<Species>("select * from Species");
        }

        public IList<Species> QuerySpecies(int type)
        {
            return fDB.Query<Species>("select * from Species where [Type] = ?", type);
        }

        public SpeciesType GetSpeciesType(int speciesId)
        {
            var species = fDB.Query<Species>("select * from Species where [Id] = ?", speciesId);
            return (species != null && species.Count > 0) ? species[0].Type : SpeciesType.Fish;
        }

        public IList<string> QuerySpeciesFamilies()
        {
            var queryResult = fDB.Query<QString>("select distinct BioFamily as element from Species");
            return ALData.GetStringList(queryResult);
        }

        public IList<string> QuerySpeciesDistributions()
        {
            var queryResult = fDB.Query<QString>("select distinct Distribution as element from Species");
            return ALData.GetStringList(queryResult);
        }

        public IList<string> QuerySpeciesHabitats()
        {
            var queryResult = fDB.Query<QString>("select distinct Habitat as element from Species");
            return ALData.GetStringList(queryResult);
        }

        #endregion

        #region Device functions

        public IList<Device> QueryDevices()
        {
            return fDB.Query<Device>("select * from Device");
        }

        public IList<Device> QueryDevices(int aquariumId)
        {
            return fDB.Query<Device>("select * from Device where AquariumId = ?", aquariumId);
        }

        public IList<Device> QueryDevices(Aquarium aquarium)
        {
            return fDB.Query<Device>("select * from Device where AquariumId = ?", aquarium.Id);
        }

        public IList<string> QueryDeviceBrands()
        {
            var queryResult = fDB.Query<QString>("select distinct Brand as element from Device");
            return ALData.GetStringList(queryResult);
        }

        #endregion

        #region Inventory functions

        public IList<Inventory> QueryInventory()
        {
            return fDB.Query<Inventory>("select * from Inventory");
        }

        public IList<string> QueryInventoryBrands()
        {
            var queryResult = fDB.Query<QString>("select distinct Brand as element from Inventory");
            return ALData.GetStringList(queryResult);
        }

        #endregion

        #region Maintenance functions

        public List<Maintenance> QueryMaintenances()
        {
            return fDB.Query<Maintenance>("select * from Maintenance order by [Timestamp]");
        }

        public List<Maintenance> QueryMaintenances(int aquariumId)
        {
            return fDB.Query<Maintenance>("select * from Maintenance where (AquariumId = ?) order by [Timestamp]", aquariumId);
        }

        public IList<Maintenance> QueryWaterChanges(int aquariumId)
        {
            return fDB.Query<Maintenance>("select * from Maintenance where (AquariumId = ? and Type between 0 and 3) order by [Timestamp]", aquariumId);
        }

        public IList<Maintenance> QueryWaterChangesEx(int aquariumId)
        {
            return fDB.Query<Maintenance>("select * from Maintenance where (AquariumId = ? and Type in (0, 1, 2, 3, 8, 9)) order by [Timestamp]", aquariumId);
        }

        public double GetWaterVolume(int aquariumId)
        {
            double result = 0.0d;

            var records = QueryWaterChangesEx(aquariumId);
            foreach (Maintenance rec in records) {
                switch (rec.Type) {
                    case MaintenanceType.Restart:
                        result = rec.Value;
                        break;

                    case MaintenanceType.AquariumStarted:
                    case MaintenanceType.AquariumStopped:
                        result = 0.0d;
                        break;

                    default:
                        int idx = (int)rec.Type;
                        int factor = ALData.MaintenanceTypes[idx].WaterChangeFactor;
                        result += (rec.Value * factor);
                        break;
                }
            }

            return result;
        }

        public void GetWaterChangeIntervals(int aquariumId, WorkTime workTime, out double avgChangeDays, out double lastChangeDays)
        {
            double changesDays = 0.0d;
            int changesCount = 0;

            DateTime dtPrev = ALCore.ZeroDate;
            var records = fDB.Query<Maintenance>("select * from Maintenance where ((AquariumId = ?) and (Type between 0 and 3) and (Timestamp >= ?)) order by [Timestamp]", aquariumId, workTime.Start);
            foreach (Maintenance rec in records) {
                if (!ALCore.IsZeroDate(dtPrev)) {
                    int days = (rec.Timestamp.Date - dtPrev).Days;
                    changesDays += days;
                    changesCount += 1;
                }
                dtPrev = rec.Timestamp.Date;
            }

            avgChangeDays = changesDays / changesCount;
            lastChangeDays = (DateTime.Now.Date - dtPrev).Days;
        }

        public WorkTime GetWorkTime(Aquarium aquarium)
        {
            int aquariumId = aquarium.Id;
            IList<Maintenance> maintenances = fDB.Query<Maintenance>("select * from Maintenance where (AquariumId = ? and Type between 8 and 9) order by [Timestamp] desc", aquariumId);

            DateTime startDate = ALCore.ZeroDate;
            DateTime stopDate = ALCore.ZeroDate;

            foreach (var mtn in maintenances) {
                if (mtn.Type == MaintenanceType.AquariumStopped && ALCore.IsZeroDate(stopDate)) {
                    stopDate = mtn.Timestamp;
                }

                if (mtn.Type == MaintenanceType.AquariumStarted && ALCore.IsZeroDate(startDate)) {
                    startDate = mtn.Timestamp;
                    break;
                }
            }

            return new WorkTime(startDate, stopDate);
        }

        public TankState GetTankState(WorkTime workTime)
        {
            return (!workTime.WasStarted() || workTime.IsInactive()) ? TankState.Inactive : TankState.Normal;
        }

        #endregion

        #region Schedule functions

        public IList<Schedule> QuerySchedule()
        {
            return fDB.Query<Schedule>("select * from Schedule order by [Timestamp]");
        }

        public IList<Schedule> QuerySchedule(int aquariumId)
        {
            return fDB.Query<Schedule>("select * from Schedule where (AquariumId = ?) order by [Timestamp]", aquariumId);
        }

        #endregion

        #region Transfer functions

        public List<Transfer> QueryTransfers()
        {
            return fDB.Query<Transfer>("select * from Transfer order by [Timestamp]");
        }

        public IList<Transfer> QueryTransfers(int aquariumId)
        {
            return fDB.Query<Transfer>("select * from Transfer where (SourceId = ? or TargetId = ?) order by [Timestamp]", aquariumId, aquariumId);
        }

        public IList<Transfer> QueryTransfers(int itemId, int itemType)
        {
            return fDB.Query<Transfer>("select * from Transfer where (ItemId = ? and ItemType = ?) order by [Timestamp]", itemId, itemType);
        }

        public IList<Transfer> QueryLastTransfers(int itemId, int itemType)
        {
            return fDB.Query<Transfer>("select * from Transfer where (ItemId = ? and ItemType = ?) order by [Timestamp] desc", itemId, itemType);
        }

        #endregion

        #region Note functions

        public List<Note> QueryNotes()
        {
            return fDB.Query<Note>("select * from Note order by [Timestamp]");
        }

        public List<Note> QueryNotes(int aquariumId)
        {
            return fDB.Query<Note>("select * from Note where (AquariumId = ?) order by [Timestamp]", aquariumId);
        }

        #endregion

        #region Measure functions

        public List<Measure> QueryMeasures()
        {
            return fDB.Query<Measure>("select * from Measure order by [Timestamp]");
        }

        public List<Measure> QueryMeasures(int aquariumId)
        {
            return fDB.Query<Measure>("select * from Measure where AquariumId = ? order by [Timestamp]", aquariumId);
        }

        public QDecimal QueryLastMeasure(Aquarium aquarium, string field)
        {
            string query = string.Format("select [{0}] as value from Measure where AquariumId = {1} and [{2}] <> 0.00000 order by [Timestamp] desc limit 1", field, aquarium.Id, field);
            List<QDecimal> list = fDB.Query<QDecimal>(query);
            return (list != null && list.Count > 0) ? list[0] : null;
        }

        public double GetCurrentMeasureValue(Aquarium aquarium, string field)
        {
            QDecimal measure = QueryLastMeasure(aquarium, field);
            double mVal = (measure != null) ? measure.value : double.NaN;
            return mVal;
        }

        public List<MeasureValue> CollectData(Aquarium aquarium)
        {
            List<MeasureValue> measures = new List<MeasureValue>();

            PrepareValue(aquarium, measures, "Temperature", "T", "°C", null);
            double NO3 = PrepareValue(aquarium, measures, "NO3", "NO3", "mg/l", ALData.NO3Ranges);
            PrepareValue(aquarium, measures, "NO2", "NO2", "mg/l", ALData.NO2Ranges);
            PrepareValue(aquarium, measures, "Cl2", "Cl2", "mg/l", ALData.Cl2Ranges);
            PrepareValue(aquarium, measures, "GH", "GH", "°d", ALData.GHRanges);
            PrepareValue(aquarium, measures, "KH", "KH", "°d", ALData.KHRanges);
            PrepareValue(aquarium, measures, "pH", "pH", "", ALData.pHRanges);
            PrepareValue(aquarium, measures, "CO2", "CO2", "", ALData.CO2Ranges);

            PrepareValue(aquarium, measures, "NH", "NHtot", "", null);
            PrepareValue(aquarium, measures, "NH3", "NH3", "", ALData.NH3Ranges);
            PrepareValue(aquarium, measures, "NH4", "NH4", "", null);

            double PO4 = PrepareValue(aquarium, measures, "PO4", "PO4", "", ALData.PO4Ranges);

            double redfield = (!double.IsNaN(PO4) && !DoubleHelper.Equals(PO4, 0.0001, 0.0001)) ? ALData.CalcRedfield(NO3, PO4) : double.NaN;
            PrepareValue(measures, redfield, "Redfield", "", ALData.RedfieldRanges);

            return measures;
        }

        private double PrepareValue(Aquarium aquarium, IList<MeasureValue> measures, string field, string sign, string uom, ValueRange[] ranges)
        {
            if (aquarium == null) return double.NaN;

            QDecimal measure = QueryLastMeasure(aquarium, field);
            double mVal = (measure != null) ? measure.value : double.NaN;

            PrepareValue(measures, mVal, sign, uom, ranges);

            return mVal;
        }

        private void PrepareValue(IList<MeasureValue> measures, double mVal, string sign, string uom, ValueRange[] ranges)
        {
            string strVal = !double.IsNaN(mVal) ? ALCore.GetDecimalStr(mVal) : string.Empty;
            string text = string.Format("{0}={1} {2}", sign, strVal, uom);

            Color color = Color.Black;
            ValueRange bounds = CheckValue(mVal, ranges);
            if (bounds != null) {
                color = bounds.Color;
            }

            var tval = new MeasureValue(sign, mVal, uom, strVal, text, color, ranges);
            measures.Add(tval);
        }

        private double FindMeasure(IList<MeasureValue> measures, string sign)
        {
            foreach (MeasureValue tVal in measures) {
                if (tVal.Name == sign) {
                    return tVal.Value;
                }
            }
            return 0.0001;
        }

        public static ValueRange CheckValue(double value, ValueRange[] ranges)
        {
            if (double.IsNaN(value) || DoubleHelper.Equals(value, 0.0d, 0.0000000001d) || ranges == null) {
                return null;
            }

            foreach (var bounds in ranges) {
                if (value >= bounds.Min && value <= bounds.Max) {
                    return bounds;
                }
            }

            return null;
        }

        #endregion

        #region Nutrition functions

        public IList<Nutrition> QueryNutritions()
        {
            return fDB.Query<Nutrition>("select * from Nutrition");
        }

        public IList<Nutrition> QueryNutritions(Aquarium aquarium)
        {
            return fDB.Query<Nutrition>("select * from Nutrition where AquariumId = ?", aquarium.Id);
        }

        public IList<string> QueryNutritionBrands()
        {
            var queryResult = fDB.Query<QString>("select distinct Brand as element from Nutrition");
            return ALData.GetStringList(queryResult);
        }

        #endregion

        #region Expense functions

        public IList<Transfer> QueryTransferExpenses()
        {
            return fDB.Query<Transfer>("select Timestamp, ItemType, ItemId, Type, Quantity, UnitPrice, Shop from Transfer order by [Timestamp]");
        }

        public IList<string> QueryTransferShops()
        {
            var queryResult = fDB.Query<QString>("select distinct Shop as element from Transfer");
            return ALData.GetStringList(queryResult);
        }

        #endregion

        #region Brand functions

        public IList<Brand> QueryBrands()
        {
            return fDB.Query<Brand>("select * from Brand");
        }

        public IList<string> QueryBrandCountries()
        {
            var queryResult = fDB.Query<QString>("select distinct Country as element from Brand");
            return ALData.GetStringList(queryResult);
        }

        #endregion

        #region Shop functions

        public IList<Shop> QueryShops()
        {
            return fDB.Query<Shop>("select * from Shop");
        }

        #endregion

        #region Snapshot functions

        public List<Snapshot> QuerySnapshots()
        {
            return fDB.Query<Snapshot>("select * from Snapshot");
        }

        public IList<Snapshot> QuerySnapshots(int itemId, int itemType)
        {
            return fDB.Query<Snapshot>("select * from Snapshot where (ItemId = ? and ItemType = ?) order by [Timestamp]", itemId, itemType);
        }

        #endregion

        #region Data Acquisition

        public void ApplySettings(ALSettings settings)
        {
            if (fChannel != null)
                fChannel.Dispose();

            if (settings.ChannelEnabled) {
                fChannel = BaseChannel.CreateChannel(settings.ChannelName, settings.ChannelParameters, OnReceivedData);
            }
        }

        private void OnReceivedData(object sender, DataReceivedEventArgs e)
        {
            try {
                fTSDB.ReceiveData(e.SensorId, e.Value);

                DataReceivedEventHandler handler = ReceivedData;
                if (handler != null) handler(sender, e);
            } catch (Exception ex) {
                fLogger.WriteError("OnReceivedData()", ex);
            }
        }

        public double GetCurrentValue(int pointId)
        {
            return (pointId != 0) ? fTSDB.GetCurrentValue(pointId) : double.NaN;
        }

        #endregion

        #region Constraints, record links

        public bool CheckConstraints(Entity entity)
        {
            // any links?
            bool result = false;

            switch (entity.EntityType) {
                case EntityType.Aquarium: {
                        var aqmRec = entity as Aquarium;
                        ItemType itemType = ItemType.Aquarium;
                        result = HasTransfers(fDB, entity.Id, itemType) || HasAquariumLinks(fDB, entity.Id) || HasAquariumSTT(fDB, entity.Id);
                    }
                    break;

                case EntityType.Inhabitant: {
                        var inhRec = entity as Inhabitant;
                        SpeciesType speciesType = GetSpeciesType(inhRec.SpeciesId);
                        ItemType itemType = ALCore.GetItemType(speciesType);
                        result = HasTransfers(fDB, entity.Id, itemType) || HasSnapshots(fDB, entity.Id, itemType);
                    }
                    break;

                case EntityType.Species: {
                        var spcRec = entity as Species;
                        string query = string.Format("select count(*) as value from Inhabitant where SpeciesId = {0}", entity.Id);
                        result = HasCount(fDB, query);
                    }
                    break;

                case EntityType.Nutrition: {
                        var nutrRec = entity as Nutrition;
                        ItemType itemType = ItemType.Nutrition;
                        result = HasTransfers(fDB, entity.Id, itemType);
                    }
                    break;

                case EntityType.Device: {
                        var devRec = entity as Device;
                        ItemType itemType = ItemType.Device;
                        result = HasTransfers(fDB, entity.Id, itemType);
                    }
                    break;

                case EntityType.Inventory: {
                        // for Soils in the future - links from aquarium records
                        var invRec = entity as Inventory;
                        ItemType itemType = ALCore.GetItemType(invRec.Type);
                        result = HasTransfers(fDB, entity.Id, itemType);
                    }
                    break;

                case EntityType.Maintenance:
                case EntityType.Measure:
                case EntityType.Note:
                case EntityType.Schedule:
                case EntityType.Transfer:
                case EntityType.Snapshot:
                    // default, no external links
                    break;

                case EntityType.Brand:
                case EntityType.Shop:
                    // default, because it is not used as a reference table
                    break;

                case EntityType.TSPoint: {
                        var tspRec = entity as TSPoint;
                        string query = string.Format("select count(*) as value from Device where PointId = {0}", entity.Id);
                        result = HasCount(fDB, query);
                    }
                    break;
            }

            return result;
        }

        private static bool HasAquariumLinks(SQLiteConnection db, int aquariumId)
        {
            return HasAquariumDetails(db, "Schedule", aquariumId) || HasAquariumDetails(db, "Note", aquariumId)
                || HasAquariumDetails(db, "Measure", aquariumId) || HasAquariumDetails(db, "Maintenance", aquariumId)
                || HasAquariumDetails(db, "Device", aquariumId);
        }

        private static bool HasAquariumSTT(SQLiteConnection db, int aquariumId)
        {
            string query = string.Format("select count(*) as value from Transfer where (SourceId = {0} or TargetId = {0})", aquariumId);
            return HasCount(db, query);
        }

        private static bool HasAquariumDetails(SQLiteConnection db, string table, int aquariumId)
        {
            string query = string.Format("select count(*) as value from {0} where (AquariumId = {1})", table, aquariumId);
            return HasCount(db, query);
        }

        private static bool HasSnapshots(SQLiteConnection db, int itemId, ItemType itemType)
        {
            string query = string.Format("select count(*) as value from Snapshot where (ItemId = {0} and ItemType = {1})", itemId, (int)itemType);
            return HasCount(db, query);
        }

        private static bool HasTransfers(SQLiteConnection db, int itemId, ItemType itemType)
        {
            string query = string.Format("select count(*) as value from Transfer where (ItemId = {0} and ItemType = {1})", itemId, (int)itemType);
            return HasCount(db, query);
        }

        private static bool HasCount(SQLiteConnection db, string query)
        {
            List<QDecimal> list = db.Query<QDecimal>(query);
            return (list != null && list.Count != 0 && list[0].value > 0.0d);
        }

        #endregion
    }
}
