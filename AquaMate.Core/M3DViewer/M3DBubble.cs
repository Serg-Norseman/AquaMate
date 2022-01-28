/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
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
