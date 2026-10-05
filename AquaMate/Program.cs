/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;
using AquaMate.Core;
using AquaMate.UI;

namespace AquaMate
{
    public static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            AppHost.Start<WFAppHost, MainForm>(args);
        }
    }
}
