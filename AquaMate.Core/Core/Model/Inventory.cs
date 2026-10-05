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
    public interface IInventoryProperties : IEntityProperties
    {
    }


    /// <summary>
    /// Inventory: additive, chemistry, equipment, maintenance, furniture, decoration.
    /// </summary>
    public class Inventory : Entity, IStateItem, IBrandedItem
    {
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Note { get; set; }

        public InventoryType Type { get; set; }

        // not used
        public ItemState State { get; set; }

        private IInventoryProperties fProperties;

        [Ignore]
        public IInventoryProperties Properties
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
                return EntityType.Inventory;
            }
        }


        public Inventory()
        {
        }

        public override string ToString()
        {
            return Name;
        }

        public IInventoryProperties GetProperties(InventoryType type, string str)
        {
            Type propsType = ALData.InventoryTypes[(int)type].PropsType;
            return (propsType == null) ? null : (IInventoryProperties)StringSerializer.Deserialize(propsType, str);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public sealed class Decoration : EntityProperties, IInventoryProperties
    {
        [Browsable(true), DisplayName("Size")]
        public float Size { get; set; }

        [Browsable(true), DisplayName("Weight")]
        public float Weight { get; set; }


        public override void SetPropNames()
        {
            ALCore.SetDisplayNameValue(this, "Size", ALData.GetLSuom(LSID.Size, MeasurementType.Length));
            ALCore.SetDisplayNameValue(this, "Weight", ALData.GetLSuom(LSID.Weight, MeasurementType.Mass));
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public sealed class Soil : EntityProperties, IInventoryProperties
    {
        [Browsable(true), DisplayName("Density")]
        public float Density { get; set; } // kg/l


        public override void SetPropNames()
        {
            ALCore.SetDisplayNameValue(this, "Density", ALData.GetLSuom(LSID.Density, MeasurementType.Density));
        }
    }
}
