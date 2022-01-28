/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AquaMate.Core;
using AquaMate.Logging;
using BSLib;
using SQLite;

namespace AquaMate.TSDB
{
    /// <summary>
    /// 
    /// </summary>
    public class TSDatabase
    {
        private readonly ILogger fLogger = LogManager.GetLogger(ALCore.LOG_FILE, ALCore.LOG_LEVEL, "TSDatabase");

        private readonly Dictionary<int, SDCompression> fCompressionCache;
        private readonly SQLiteConnection fDB;

        public TSDatabase()
        {
            fCompressionCache = new Dictionary<int, SDCompression>();

            var databasePath = Path.Combine(AppHost.GetAppDataPath(), "ALTSDB.db");
            fDB = new SQLiteConnection(databasePath);
            fDB.CreateTable<TSPoint>();
        }

        public void AddPoint(TSPoint point)
        {
            fDB.Insert(point);

            string tableName = point.GetDataTableName();

            string tblExpr = string.Format("create table {0}(\"Timestamp\" datetime, \"Value\" float)", tableName);
            fDB.Execute(tblExpr);

            string idxExpr = string.Format("create unique index \"{0}_Timestamp\" on \"{1}\"(\"Timestamp\")", tableName, tableName);
            fDB.Execute(idxExpr);
        }

        public void UpdatePoint(TSPoint point)
        {
            fDB.Update(point);
        }

        public void DeletePoint(TSPoint point)
        {
            string tableName = point.GetDataTableName();

            string idxExpr = string.Format("drop index \"{0}_Timestamp\"", tableName);
            fDB.Execute(idxExpr);

            string tblExpr = string.Format("drop table {0}", tableName);
            fDB.Execute(tblExpr);

            fDB.Delete(point);
        }

        public IList<TSPoint> GetPoints(string ptFilter = "*")
        {
            return fDB.Query<TSPoint>("select * from TSPoint");
        }

        public TSPoint GetPoint(int pointId)
        {
            return fDB.Get<TSPoint>(pointId);
        }

        public void InsertValue(int pointId, DateTime timestamp, double value)
        {
            TSPoint point = fDB.Get<TSPoint>(pointId);
            string tableName = point.GetDataTableName();
            InsertValue(tableName, timestamp, value);
        }

        private void InsertValue(string tableName, DateTime timestamp, double value)
        {
            string tsx = ALCore.FmtSQLiteDate(timestamp);
            string vx = ALCore.FmtSQLiteFloat(value);
            fDB.Execute(string.Format("insert into {0}(Timestamp, Value) values('{1}', '{2}')", tableName, tsx, vx));
        }

        public void UpdateValue(int pointId, DateTime timestamp, double value)
        {
            TSPoint point = fDB.Get<TSPoint>(pointId);
            string tableName = point.GetDataTableName();

            string tsx = ALCore.FmtSQLiteDate(timestamp);
            string vx = ALCore.FmtSQLiteFloat(value);
            string query = string.Format("update {0} set Value = '{1}' where Timestamp = '{2}'", tableName, vx, tsx);
            fDB.Execute(query);
        }

        public void DeleteValue(int pointId, DateTime timestamp)
        {
            TSPoint point = fDB.Get<TSPoint>(pointId);
            string tableName = point.GetDataTableName();

            string tsx = ALCore.FmtSQLiteDate(timestamp);
            fDB.Execute(string.Format("delete from {0} where Timestamp = '{1}'", tableName, tsx));
        }

        public IList<TSValue> QueryValues(int pointId, DateTime begTime, DateTime endTime)
        {
            TSPoint point = fDB.Get<TSPoint>(pointId);
            string tableName = point.GetDataTableName();

            string tsBeg = ALCore.FmtSQLiteDate(begTime);
            string tsEnd = ALCore.FmtSQLiteDate(endTime);
            string query = string.Format("select * from {0} where Timestamp between '{1}' and '{2}'", tableName, tsBeg, tsEnd);
            return fDB.Query<TSValue>(query);
        }

        public void ReceivePointValue(int pointId, DateTime timestamp, double value)
        {
            TSPoint point = fDB.Get<TSPoint>(pointId);

            SDCompression compression;
            if (!fCompressionCache.TryGetValue(pointId, out compression)) {
                compression = new SDCompression(point.Deviation, 60 * 10); // default: 60sec * 10min
                fCompressionCache.Add(pointId, compression);
            }

            if (compression.ReceivePoint(ref timestamp, ref value)) {
                string tableName = point.GetDataTableName();
                InsertValue(tableName, timestamp, value);
            }
        }

        public double GetCurrentValue(int pointId)
        {
            TSPoint point = fDB.Get<TSPoint>(pointId);

            SDCompression compression;
            if (!fCompressionCache.TryGetValue(pointId, out compression)) {
                compression = new SDCompression(point.Deviation, 60 * 10); // default: 60sec * 10min
                fCompressionCache.Add(pointId, compression);
            }

            if (compression != null) {
                return compression.CurrentPoint.Value;
            }

            return double.NaN;
        }

        private TSPoint FindPointBySID(string sid)
        {
            var sidPoints = fDB.Query<TSPoint>("select * from TSPoint where SID = ?", sid);
            return sidPoints.FirstOrDefault();
        }

        public void ReceiveData(string sid, float value)
        {
            try {
                var point = FindPointBySID(sid);
                if (point != null) {
                    ReceivePointValue(point.Id, DateTime.Now, value);
                }
            } catch (Exception ex) {
                fLogger.WriteError("ReceiveData()", ex);
            }
        }
    }
}
