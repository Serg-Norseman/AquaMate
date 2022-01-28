/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
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
