/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;
using System.ComponentModel;
using AquaMate.Core.Types;
using SQLite;

namespace AquaMate.Core.Model
{
    public interface IDeviceProperties : IEntityProperties
    {
    }


    /// <summary>
    /// Electrical and/or measurement equipments.
    /// </summary>
    public class Device : AquariumDetails, IStateItem, IBrandedItem
    {
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Note { get; set; }

        public DeviceType Type { get; set; }

        public bool Enabled { get; set; }
        public bool Digital { get; set; }
        public double Power { get; set; }
        public double WorkTime { get; set; }

        // not used
        public ItemState State { get; set; }

        public int PointId { get; set; }

        private IDeviceProperties fProperties;

        [Ignore]
        public IDeviceProperties Properties
        {
            get {
                if (fProperties == null) {
                    fProperties = GetProperties(Type, RawProperties);
                }
                return fProperties;
            }
            set {
                fProperties = value;
                RawProperties = StringSerializer.Serialize(fProperties);
            }
        }

        public string RawProperties { get; set; }


        public override EntityType EntityType
        {
            get {
                return EntityType.Device;
            }
        }


        public Device()
        {
        }

        public override string ToString()
        {
            return Name;
        }

        public IDeviceProperties GetProperties(DeviceType type, string str)
        {
            Type propsType = ALData.DeviceProps[(int)type].PropsType;
            return (propsType == null) ? null : (IDeviceProperties)StringSerializer.Deserialize(propsType, str);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public sealed class Light : EntityProperties, IDeviceProperties
    {
        [Browsable(true), DisplayName("LightTemperature")]
        public float LightTemperature { get; set; } // UoM: K

        [Browsable(true), DisplayName("LuminousFlux")]
        public float LuminousFlux { get; set; } // UoM: lm

        [Browsable(true), DisplayName("PAR")]
        public float PAR { get; set; } // Photosynthetically Active Radiation, UoM: W/m2


        public override void SetPropNames()
        {
            ALCore.SetDisplayNameValue(this, "LightTemperature", ALData.GetLSuom(LSID.LightTemperature, MeasurementType.LightTemperature));
            ALCore.SetDisplayNameValue(this, "LuminousFlux", ALData.GetLSuom(LSID.LuminousFlux, MeasurementType.LuminousFlux));
            ALCore.SetDisplayNameValue(this, "PAR", ALData.GetLSuom(LSID.PhotosyntheticallyActiveRadiation, MeasurementType.PhotosyntheticallyActiveRadiation));
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public class Pump : EntityProperties, IDeviceProperties
    {
        [Browsable(true), DisplayName("MinFlow")]
        public float MinFlow { get; set; } // UoM: l/h

        [Browsable(true), DisplayName("MaxFlow")]
        public float MaxFlow { get; set; } // UoM: l/h


        public override void SetPropNames()
        {
            //ALCore.SetDisplayNameValue(this, "MinFlow", ALData.GetLSuom(LSID.MinFlow, MeasurementType.Flow));
            //ALCore.SetDisplayNameValue(this, "MaxFlow", ALData.GetLSuom(LSID.MaxFlow, MeasurementType.Flow));
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public sealed class Filter : Pump
    {
        [Browsable(true), DisplayName("Volume")]
        public float Volume { get; set; } // UoM: l


        public override void SetPropNames()
        {
            base.SetPropNames();
            ALCore.SetDisplayNameValue(this, "Volume", ALData.GetLSuom(LSID.Volume, MeasurementType.Volume));
        }
    }
}
