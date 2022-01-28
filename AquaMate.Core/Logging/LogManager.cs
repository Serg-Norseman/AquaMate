/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using BSLib;

namespace AquaMate.Logging
{
    public sealed class LogManager
    {
        private static LogManager fLogManager;

        private LogManager(string logFileName, string logLevel)
        {
            try {
                Log4NetHelper.Init(logFileName, logLevel);
            } catch (Exception e) {
                Log4NetHelper.Init(@".\fatal.log", "ERROR");
                var l = new Log4NetHelper(GetType().ToString());
                l.WriteError("Error while initializing the logger", e);
            }
        }

        public static ILogger GetLogger(string logFileName, string logLevel, string loggerName)
        {
            if (fLogManager == null) {
                fLogManager = new LogManager(logFileName, logLevel);
            }
            return new Log4NetHelper(loggerName);
        }
    }
}
