/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

namespace AquaMate.Core.Calculations
{
    public class CalcParam
    {
        public readonly string PropName;
        public readonly string DispName;

        public CalcParam(string propName, string dispName)
        {
            PropName = propName;
            DispName = dispName;
        }
    }


    public delegate double CalcHandler(double arg);


    public class CalculationProps
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public CalcParam[] Args { get; private set; }
        public CalcParam Result { get; private set; }
        public CalcHandler Handler { get; private set; }

        public CalculationProps(string name, string description, CalcParam[] args, CalcParam result, CalcHandler calcHandler)
        {
            Name = name;
            Description = description;
            Args = args;
            Result = result;
            Handler = calcHandler;
        }
    }
}
