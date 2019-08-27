﻿/*
 *  This file is part of the "AquaLog".
 *  Copyright (C) 2019 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using AquaLog.Core.Model;
using AquaLog.Core.Types;
using AquaLog.TSDB;
using SQLite;

namespace AquaLog.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class ALModel
    {
        private readonly SQLiteConnection fDB;
        private TSDatabase fTSDB;


        public TSDatabase TSDB
        {
            get { return fTSDB; }
        }


        public ALModel()
        {
            fTSDB = new TSDatabase();

            var databasePath = Path.Combine(ALCore.GetAppDataPath(), "ALData.db");
            fDB = new SQLiteConnection(databasePath);

            fDB.CreateTable<Aquarium>();

            fDB.CreateTable<Inhabitant>();
            fDB.CreateTable<Species>();

            fDB.CreateTable<Device>();
            fDB.CreateTable<Note>();
            fDB.CreateTable<History>();
            fDB.CreateTable<Maintenance>();
            fDB.CreateTable<Transfer>();
            fDB.CreateTable<Nutrition>();
            fDB.CreateTable<Measure>();
            fDB.CreateTable<Schedule>();
        }

        /// <summary>
        /// Cleaning waste space
        /// </summary>
        public void CleanSpace()
        {
            fDB.Execute("drop table if exists Expense");
            fDB.Execute("VACUUM;");
        }

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

        public T GetRecord<T>(int objId) where T : new()
        {
            return (objId == 0) ? default(T) : fDB.Get<T>(objId);
        }

        public IEnumerable<T> QueryRecords<T>(string query, params object[] args) where T : new()
        {
            return fDB.Query<T>(query, args);
        }

        public Entity GetRecord(ItemType itemType, int itemId)
        {
            Entity result = null;
            switch (itemType) {
                case ItemType.Aquarium:
                    break;
                case ItemType.Fish:
                    result = GetRecord<Inhabitant>(itemId);
                    break;
                case ItemType.Invertebrate:
                    result = GetRecord<Inhabitant>(itemId);
                    break;
                case ItemType.Plant:
                    result = GetRecord<Inhabitant>(itemId);
                    break;
                case ItemType.Coral:
                    break;
                case ItemType.Nutrition:
                    result = GetRecord<Nutrition>(itemId);
                    break;
                case ItemType.Chemistry:
                    break;
                case ItemType.Additive:
                    break;
                case ItemType.Device:
                    result = GetRecord<Device>(itemId);
                    break;
                case ItemType.Equipment:
                    break;
                case ItemType.Maintenance:
                    break;
                case ItemType.Furniture:
                    break;
                case ItemType.Decoration:
                    break;
            }
            return result;
        }

        public string GetRecordName(ItemType itemType, int itemId)
        {
            Entity itemRec = GetRecord(itemType, itemId);
            return (itemRec == null) ? string.Empty : itemRec.ToString();
        }

        #region Aquarium functions

        public IEnumerable<Aquarium> QueryAquariums()
        {
            return fDB.Query<Aquarium>("select * from Aquarium");
        }

        public int QueryInhabitantsCount(int aquariumId)
        {
            int result = 0;
            var qv = fDB.Query<Transfer>("select ItemType, ItemId, Type, Quantity from Transfer where TargetId = ?", aquariumId);
            foreach (var val in qv) {
                // FIXME: transfer types +/- birth, death and etc
                switch (val.ItemType) {
                    case ItemType.Fish:
                    case ItemType.Invertebrate:
                    case ItemType.Plant:
                        result += val.Quantity;
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
                result += (val.Quantity * factor);
            }
            return result;
        }

        #endregion

        #region Inhabitant functions

        public IEnumerable<Inhabitant> QueryInhabitants()
        {
            return fDB.Query<Inhabitant>("select * from Inhabitant");
        }

        public IEnumerable<Inhabitant> QueryInhabitants(Aquarium aquarium)
        {
            return fDB.Query<Inhabitant>("select inh.Id, inh.SpeciesId, inh.Sex, inh.Name from Inhabitant inh, Transfer tran where (inh.Id = tran.ItemId and tran.ItemType in (2, 3, 5) and TargetId = ?)", aquarium.Id);
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

        #endregion

        #region Device functions

        public IList<Device> QueryDevices()
        {
            return fDB.Query<Device>("select * from Device");
        }

        public IList<Device> QueryDevices(Aquarium aquarium)
        {
            return fDB.Query<Device>("select * from Device where AquariumId = ?", aquarium.Id);
        }

        public IList<QString> QueryDeviceBrands()
        {
            return fDB.Query<QString>("select distinct Brand as element from Device");
        }

        #endregion

        #region History functions

        public IList<History> QueryHistory()
        {
            return fDB.Query<History>("select * from History order by [DateTime]");
        }

        #endregion

        #region Maintenance functions

        public IList<Maintenance> QueryMaintenances()
        {
            return fDB.Query<Maintenance>("select * from Maintenance order by [DateTime]");
        }

        public IList<Maintenance> QueryMaintenances(int aquariumId)
        {
            return fDB.Query<Maintenance>("select * from Maintenance where (AquariumId = ?) order by [DateTime]", aquariumId);
        }

        public IList<Maintenance> QueryWaterChanges(int aquariumId)
        {
            return fDB.Query<Maintenance>("select * from Maintenance where (AquariumId = ? and Type between 0 and 3) order by [DateTime]", aquariumId);
        }

        public double GetWaterVolume(int aquariumId)
        {
            double result = 0.0d;

            var records = QueryWaterChanges(aquariumId);
            foreach (Maintenance rec in records) {
                if (rec.Type == MaintenanceType.Restart) {
                    result = rec.Value;
                } else {
                    int idx = (int)rec.Type;
                    int factor = ALCore.WaterChangeFactors[idx];
                    result += (rec.Value * factor);
                }
            }

            return result;
        }

        public double GetAverageWaterChangeInterval(int aquariumId)
        {
            double result = 0.0d;
            int count = 0;

            DateTime dtPrev = ALCore.ZeroDate;
            var records = QueryWaterChanges(aquariumId);
            foreach (Maintenance rec in records) {
                if (!dtPrev.Equals(ALCore.ZeroDate)) {
                    int days = (rec.DateTime.Date - dtPrev).Days;
                    result += days;
                    count += 1;
                }
                dtPrev = rec.DateTime.Date;
            }

            return result / count;
        }

        public double GetLastWaterChangeInterval(int aquariumId)
        {
            DateTime dtPrev = ALCore.ZeroDate;
            var records = QueryWaterChanges(aquariumId);
            foreach (Maintenance rec in records) {
                dtPrev = rec.DateTime.Date;
            }
            return (DateTime.Now.Date - dtPrev).Days;
        }

        #endregion

        #region Schedule functions

        public IList<Schedule> QuerySchedule()
        {
            return fDB.Query<Schedule>("select * from Schedule order by [DateTime]");
        }

        #endregion

        #region Transfer functions

        public IList<Transfer> QueryTransfers()
        {
            return fDB.Query<Transfer>("select * from Transfer order by [Date]");
        }

        public IList<Transfer> QueryTransfers(int aquariumId)
        {
            return fDB.Query<Transfer>("select * from Transfer where (SourceId = ? or TargetId = ?) order by [Date]", aquariumId, aquariumId);
        }

        public IList<Transfer> QueryTransfers(int itemId, int itemType)
        {
            return fDB.Query<Transfer>("select * from Transfer where (ItemId = ? and ItemType = ?) order by [Date]", itemId, itemType);
        }

        public IList<Transfer> QueryLastTransfers(int itemId, int itemType)
        {
            return fDB.Query<Transfer>("select * from Transfer where (ItemId = ? and ItemType = ?) order by [Date] desc", itemId, itemType);
        }

        #endregion

        #region Note functions

        public IEnumerable<Note> QueryNotes()
        {
            return fDB.Query<Note>("select * from Note");
        }

        public IEnumerable<Note> QueryNotes(Aquarium aquarium)
        {
            return fDB.Query<Note>("select * from Note where AquariumId = ?", aquarium.Id);
        }

        #endregion

        #region Measure functions

        public IEnumerable<Measure> QueryMeasures()
        {
            return fDB.Query<Measure>("select * from Measure order by Timestamp");
        }

        public IEnumerable<Measure> QueryMeasures(Aquarium aquarium)
        {
            return fDB.Query<Measure>("select * from Measure where AquariumId = ? order by Timestamp", aquarium.Id);
        }

        public QDecimal QueryLastMeasure(Aquarium aquarium, string field)
        {
            string query = string.Format("select [{0}] as value from Measure where AquariumId = {1} and [{2}] <> 0.00000 order by Timestamp desc limit 1", field, aquarium.Id, field);
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
            PrepareValue(aquarium, measures, "NO3", "NO3", "mg/l", ALData.NO3Ranges);
            PrepareValue(aquarium, measures, "NO2", "NO2", "mg/l", ALData.NO2Ranges);
            PrepareValue(aquarium, measures, "Cl2", "Cl2", "mg/l", ALData.Cl2Ranges);
            PrepareValue(aquarium, measures, "GH", "GH", "°d", ALData.GHRanges);
            PrepareValue(aquarium, measures, "KH", "KH", "°d", ALData.KHRanges);
            PrepareValue(aquarium, measures, "pH", "pH", "", ALData.pHRanges);
            PrepareValue(aquarium, measures, "CO2", "CO2", "", ALData.CO2Ranges);

            PrepareValue(aquarium, measures, "NH", "NHtot", "", null);
            PrepareValue(aquarium, measures, "NH3", "NH3", "", ALData.NH3Ranges);
            PrepareValue(aquarium, measures, "NH4", "NH4", "", null);

            return measures;
        }

        private void PrepareValue(Aquarium aquarium, List<MeasureValue> measures, string field, string sign, string uom, List<ValueBounds> ranges)
        {
            QDecimal measure = QueryLastMeasure(aquarium, field);
            double mVal = (measure != null) ? measure.value : double.NaN;

            string strVal = !double.IsNaN(mVal) ? ALCore.GetDecimalStr(mVal) : string.Empty;
            string text = string.Format("{0}={1} {2}", sign, strVal, uom);

            Color color = Color.Black;
            if (!double.IsNaN(mVal) && mVal != 0.0d && ranges != null) {
                ValueBounds bounds = CheckValue(mVal, ranges);
                if (bounds != null) {
                    color = bounds.Color;
                }
            }

            var tval = new MeasureValue() {
                Name = sign,
                Value = mVal,
                Unit = uom,
                ValText = strVal,
                Text = text,
                Color = color
            };
            measures.Add(tval);
        }

        private ValueBounds CheckValue(double value, List<ValueBounds> ranges)
        {
            foreach (var bounds in ranges) {
                if (value >= bounds.Min && value <= bounds.Max) {
                    return bounds;
                }
            }
            return null;
        }

        #endregion

        #region Nutrition functions

        public IEnumerable<Nutrition> QueryNutritions()
        {
            return fDB.Query<Nutrition>("select * from Nutrition");
        }

        public IEnumerable<Nutrition> QueryNutritions(Aquarium aquarium)
        {
            return fDB.Query<Nutrition>("select * from Nutrition where AquariumId = ?", aquarium.Id);
        }

        public IList<QString> QueryNutritionBrands()
        {
            return fDB.Query<QString>("select distinct Brand as element from Nutrition");
        }

        #endregion

        #region Expense functions

        public IEnumerable<Transfer> QueryExpenses()
        {
            return fDB.Query<Transfer>("select Date, ItemType, ItemId, Type, Quantity, UnitPrice, Shop from Transfer");
        }

        public IList<QString> QueryShops()
        {
            return fDB.Query<QString>("select distinct Shop as element from Transfer");
        }

        #endregion
    }
}
