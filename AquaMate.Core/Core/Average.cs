/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
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
