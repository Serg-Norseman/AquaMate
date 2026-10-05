/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using AquaMate.Core.Model;
using AquaMate.Core.Types;
using BSLib;
using SQLite;

namespace AquaMate.TSDB
{
    /// <summary>
    /// 
    /// </summary>
    public class TSPoint : Entity
    {
        [Unique]
        public string Name { get; set; }

        public MeasurementType Type { get; set; }
        public string MeasureUnit { get; set; }
        public string SID { get; set; }

        #region Range

        public double Min { get; set; }
        public double Max { get; set; }
        public double Deviation { get; set; }

        #endregion


        public override EntityType EntityType
        {
            get {
                return EntityType.TSPoint;
            }
        }


        internal string GetDataTableName()
        {
            string tableName = "PD" + ConvertHelper.AdjustNumber(Id, 6, '0');
            return tableName;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
