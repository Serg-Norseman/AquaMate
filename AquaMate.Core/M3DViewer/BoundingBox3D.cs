/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;

namespace AquaMate.M3DViewer
{
    public struct BoundingBox3D
    {
        public float XMin;
        public float XMax;
        public float YMin;
        public float YMax;
        public float ZMin;
        public float ZMax;

        public BoundingBox3D(int dummy)
        {
            XMin = float.MaxValue;
            XMax = float.MinValue;

            YMin = float.MaxValue;
            YMax = float.MinValue;

            ZMin = float.MaxValue;
            ZMax = float.MinValue;
        }

        public BoundingBox3D(float xmin, float xmax, float ymin, float ymax, float zmin, float zmax)
        {
            XMin = xmin;
            XMax = xmax;

            YMin = ymin;
            YMax = ymax;

            ZMin = zmin;
            ZMax = zmax;
        }

        public void CheckPoint(Point3D point)
        {
            XMin = Math.Min(XMin, point.X);
            XMax = Math.Max(XMax, point.X);

            YMin = Math.Min(YMin, point.Y);
            YMax = Math.Max(YMax, point.Y);

            ZMin = Math.Min(ZMin, point.Z);
            ZMax = Math.Max(ZMax, point.Z);
        }

        public float GetSizeX()
        {
            return XMax - XMin;
        }

        public float GetSizeY()
        {
            return YMax - YMin;
        }

        public float GetSizeZ()
        {
            return ZMax - ZMin;
        }
    }
}
