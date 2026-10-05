/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;

namespace AquaMate.M3DViewer
{
    public struct Point3D
    {
        public static readonly Point3D Zero = new Point3D(0, 0, 0);

        public float X;
        public float Y;
        public float Z;


        public Point3D(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public bool IsZero()
        {
            return (X == 0.0f && Y == 0.0f && Z == 0.0f);
        }

        public Point3D Add(Point3D p0)
        {
            Point3D res;
            res.X = X + p0.X;
            res.Y = Y + p0.Y;
            res.Z = Z + p0.Z;
            return res;
        }

        public Point3D Add(float pX, float pY, float pZ)
        {
            Point3D res;
            res.X = X + pX;
            res.Y = Y + pY;
            res.Z = Z + pZ;
            return res;
        }

        public Vector3D Sub(Point3D p0)
        {
            Vector3D res;
            res.X = X - p0.X;
            res.Y = Y - p0.Y;
            res.Z = Z - p0.Z;
            return res;
        }

        public double Distance(Point3D pt2)
        {
            float dx = pt2.X - this.X;
            float dy = pt2.Y - this.Y;
            float dz = pt2.Z - this.Z;
            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        public static Point3D GetLineMidpoint(Point3D p1, Point3D p2)
        {
            float mx = (p1.X + p2.X) / 2.0f;
            float my = (p1.Y + p2.Y) / 2.0f;
            float mz = (p1.Z + p2.Z) / 2.0f;
            return new Point3D(mx, my, mz);
        }

        // https://stackoverflow.com/questions/217578/how-can-i-determine-whether-a-2d-point-is-within-a-polygon
        public static bool IsPointInPolygon(Point3D p, Point3D[] polygon)
        {
            double minX = polygon[0].X;
            double maxX = polygon[0].X;
            double minZ = polygon[0].Z;
            double maxZ = polygon[0].Z;
            for (int i = 1; i < polygon.Length; i++) {
                Point3D q = polygon[i];
                minX = Math.Min(q.X, minX);
                maxX = Math.Max(q.X, maxX);
                minZ = Math.Min(q.Z, minZ);
                maxZ = Math.Max(q.Z, maxZ);
            }

            if (p.X < minX || p.X > maxX || p.Z < minZ || p.Z > maxZ) {
                return false;
            }

            // http://www.ecse.rpi.edu/Homepages/wrf/Research/Short_Notes/pnpoly.html
            bool inside = false;
            for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++) {
                if ((polygon[i].Z > p.Z) != (polygon[j].Z > p.Z) &&
                    p.X < (polygon[j].X - polygon[i].X) * (p.Z - polygon[i].Z) / (polygon[j].Z - polygon[i].Z) + polygon[i].X) {
                    inside = !inside;
                }
            }

            return inside;
        }
    }
}
