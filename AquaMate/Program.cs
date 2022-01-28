/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using AquaMate.Core;
using AquaMate.UI;

namespace AquaMate
{
    internal static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            AppHost.Start<WFAppHost, MainForm>(args);
        }
    }
}
