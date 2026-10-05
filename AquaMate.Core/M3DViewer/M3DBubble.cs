/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using BSLib;

namespace AquaMate.M3DViewer
{
    public class M3DBubble : ICloneable<M3DBubble>
    {
        public float X;
        public float Y;
        public float Z;
        public float Size;

        public M3DBubble Clone()
        {
            var result = new M3DBubble();
            result.X = X;
            result.Y = Y;
            result.Z = Z;
            result.Size = Size;
            return result;
        }
    }
}
