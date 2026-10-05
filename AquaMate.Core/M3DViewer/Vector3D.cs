/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;
using BSLib;

namespace AquaMate.M3DViewer
{
    public struct Vector3D
    {
        public static readonly Vector3D Zero = new Vector3D(0, 0, 0);

        public float X;
        public float Y;
        public float Z;


        public Vector3D(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public bool IsZero()
        {
            return (X == 0.0f && Y == 0.0f && Z == 0.0f);
        }

        public float Length()
        {
            return (float)Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        public Vector3D Scale(float length)
        {
            float scale = length / this.Length();
            return new Vector3D(X * scale, Y * scale, Z * scale);
        }

        public static float GetAngle(Vector3D v1, Vector3D v2)
        {
            float radAngle = (float)Math.Acos((v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z) / (Math.Sqrt(v1.X * v1.X + v1.Y * v1.Y + v1.Z * v1.Z) * Math.Sqrt(v2.X * v2.X + v2.Y * v2.Y + v2.Z * v2.Z)));
            return (float)MathHelper.RadiansToDegrees(radAngle);
        }
    }
}
