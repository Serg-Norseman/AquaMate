/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;
using AquaMate.Core.Types;
using SQLite;

namespace AquaMate.Core.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class Aquarium : Entity, IBrandedItem
    {
        [MaxLength(140)]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Brand { get; set; }

        public AquariumWaterType WaterType { get; set; }

        public TankShape TankShape { get; set; }

        public string TankProperties { get; set; }

        private ITank fTank;

        [Ignore]
        public ITank Tank
        {
            get {
                if (fTank == null) {
                    fTank = GetTank(TankShape, TankProperties);
                }
                return fTank;
            }
            set {
                fTank = value;
                TankProperties = StringSerializer.Serialize(fTank);
            }
        }

        /// <summary>
        /// The volume of an aquarium (litres).
        /// </summary>
        public double TankVolume { get; set; }

        /// <summary>
        /// The volume of water (litres).
        /// </summary>
        public double WaterVolume { get; set; }


        /// <summary>
        /// The height (offset) of an underfill from top of aquarium to water level (cm).
        /// </summary>
        public double UnderfillHeight { get; set; }

        /// <summary>
        /// The height (thickness) of an soil from bottom of aquarium (cm).
        /// </summary>
        public double SoilHeight { get; set; }


        public override EntityType EntityType
        {
            get {
                return EntityType.Aquarium;
            }
        }


        public Aquarium()
        {
        }

        public bool IsSalt()
        {
            return (WaterType != AquariumWaterType.FreshWater);
        }

        /// <summary>
        /// The base area of an aquarium (cm2).
        /// </summary>
        public double CalcBaseArea()
        {
            double result = Tank.CalcBaseArea();
            return result;
        }

        public double CalcTankVolume()
        {
            return CalcTankVolume(TankShape);
        }

        public double CalcTankVolume(TankShape tankShape)
        {
            ITank tank = GetTank(tankShape, TankProperties);
            double result = tank.CalcTankVolume();
            return result;
        }

        public double CalcWaterVolume()
        {
            return CalcWaterVolume(TankShape);
        }

        public double CalcWaterVolume(TankShape tankShape)
        {
            return CalcWaterVolume(tankShape, UnderfillHeight, SoilHeight);
        }

        public double CalcWaterVolume(TankShape tankShape, double underfillHeight, double soilHeight)
        {
            ITank tank = GetTank(tankShape, TankProperties);
            double result = tank.CalcWaterVolume(underfillHeight, soilHeight);
            return result;
        }

        public double CalcSoilVolume()
        {
            return CalcSoilVolume(TankShape, SoilHeight);
        }

        public double CalcSoilVolume(TankShape tankShape, double soilHeight)
        {
            ITank tank = GetTank(tankShape, TankProperties);
            double result = tank.CalcSoilVolume(soilHeight);
            return result;
        }

        public override string ToString()
        {
            return Name;
        }

        public ITank GetTank(TankShape tankShape, string str)
        {
            Type tankType = ALData.TankTypes[(int)tankShape];
            ITank result = (ITank)StringSerializer.Deserialize(tankType, str);
            return result;
        }
    }
}
