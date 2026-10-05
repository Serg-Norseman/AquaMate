/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;
using System.Collections.Generic;
using AquaMate.Core.Model;
using AquaMate.Core.Types;
using AquaMate.DataCollection;
using AquaMate.TSDB;
using AquaMate.UI;
using BSLib.Design.MVP.Controls;

namespace AquaMate.Core
{
    /// <summary>
    /// 
    /// </summary>
    public interface IModel
    {
        IBrowser Browser { get; }
        EntitiesCache Cache { get; }
        TSDatabase TSDB { get; }

        event DataReceivedEventHandler ReceivedData;

        void CleanSpace();
        void Execute(string query, params object[] args);
        int AddRecord(Entity obj);
        void UpdateRecord(Entity obj);
        void DeleteRecord(Entity obj);
        void DeleteRecord<T>(int objId);
        T GetRecord<T>(int objId) where T : new();
        Entity GetRecord(ItemType itemType, int itemId);
        string GetRecordName(ItemType itemType, int itemId);
        IList<T> QueryRecords<T>(string query, params object[] args) where T : new();
        string GetEntityName(Entity entity);

        IList<Aquarium> QueryAquariums();
        IList<ComboItem<int>> QueryAquariumsList(bool showInactive);
        IList<string> QueryAquariumBrands();
        int QueryInhabitantsCount(int aquariumId);
        int QueryInhabitantsCount(int itemId, ItemType itemType);
        void GetInhabitantDates(int recId, ItemType itemType, out DateTime inclusionDate, out DateTime exclusionDate, out int currAqmId);
        void GetItemState(int recId, ItemType itemType, out ItemState itemState, out DateTime exclusionDate);
        string GetItemStateStr(int recId, ItemType itemType, out ItemState itemState);

        IList<Inhabitant> QueryInhabitants();
        IList<Inhabitant> QueryInhabitants(int aquariumId);
        List<InhabitantDispItem> PrepareInhabitants(int aquariumId);

        IList<Species> QuerySpecies();
        IList<Species> QuerySpecies(int type);
        SpeciesType GetSpeciesType(int speciesId);
        IList<string> QuerySpeciesFamilies();
        IList<string> QuerySpeciesDistributions();
        IList<string> QuerySpeciesHabitats();

        IList<Device> QueryDevices();
        IList<Device> QueryDevices(int aquariumId);
        IList<Device> QueryDevices(Aquarium aquarium);
        IList<string> QueryDeviceBrands();

        IList<Inventory> QueryInventory();
        IList<string> QueryInventoryBrands();

        List<Maintenance> QueryMaintenances();
        List<Maintenance> QueryMaintenances(int aquariumId);
        IList<Maintenance> QueryWaterChanges(int aquariumId);
        double GetWaterVolume(int aquariumId);
        void GetWaterChangeIntervals(int aquariumId, WorkTime workTime, out double avgChangeDays, out double lastChangeDays);
        WorkTime GetWorkTime(Aquarium aquarium);
        TankState GetTankState(WorkTime workTime);

        IList<Schedule> QuerySchedule();
        IList<Schedule> QuerySchedule(int aquariumId);

        List<Transfer> QueryTransfers();
        IList<Transfer> QueryTransfers(int aquariumId);
        IList<Transfer> QueryTransfers(int itemId, int itemType);
        IList<Transfer> QueryLastTransfers(int itemId, int itemType);

        List<Note> QueryNotes();
        List<Note> QueryNotes(int aquariumId);

        List<Measure> QueryMeasures();
        List<Measure> QueryMeasures(int aquariumId);
        QDecimal QueryLastMeasure(Aquarium aquarium, string field);
        double GetCurrentMeasureValue(Aquarium aquarium, string field);
        List<MeasureValue> CollectData(Aquarium aquarium);

        IList<Nutrition> QueryNutritions();
        IList<Nutrition> QueryNutritions(Aquarium aquarium);
        IList<string> QueryNutritionBrands();

        IList<Transfer> QueryTransferExpenses();
        IList<string> QueryTransferShops();

        IList<Brand> QueryBrands();
        IList<string> QueryBrandCountries();

        IList<Shop> QueryShops();

        List<Snapshot> QuerySnapshots();
        IList<Snapshot> QuerySnapshots(int itemId, int itemType);

        void ApplySettings(ALSettings settings);
        double GetCurrentValue(int pointId);

        bool CheckConstraints(Entity entity);
    }
}
