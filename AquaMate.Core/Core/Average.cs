/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

namespace AquaMate.Core
{
    public struct Average
    {
        private double fSum;
        private int fCount;

        public static Average Create()
        {
            Average result = new Average();
            result.fSum = 0.0d;
            result.fCount = 0;
            return result;
        }

        public void AddValue(double value)
        {
            fSum += value;
            fCount += 1;
        }

        public double GetResult()
        {
            return (fCount != 0) ? fSum / fCount : double.NaN;
        }
    }
}
