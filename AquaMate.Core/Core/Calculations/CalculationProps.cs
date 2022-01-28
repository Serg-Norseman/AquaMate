/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
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
