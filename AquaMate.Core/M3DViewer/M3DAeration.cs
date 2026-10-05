/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System.Collections.Generic;
using BSLib;

namespace AquaMate.M3DViewer
{
    public sealed class M3DAeration
    {
        private readonly M3DBubble[] fBubbles;

        public M3DAeration(int bubblesCount = 150)
        {
            fBubbles = new M3DBubble[bubblesCount];
            for (int i = 0; i < bubblesCount; i++) {
                var bubble = new M3DBubble();
                Init(bubble);
                fBubbles[i] = bubble;
            }
        }

        private void Init(M3DBubble bubble)
        {
            bubble.Size = RandomHelper.GetBoundedRnd(1, 3) / 1000.0f;
            bubble.X = 0.0f;
            bubble.Y = 0.0f;
            bubble.Z = 0.0f;
        }

        private void Update(M3DBubble bubble, float waterHeight, out bool isSurfaced)
        {
            float vel = bubble.Size;
            float dx = (1 - RandomHelper.GetRandom(3)) / 1000.0f;
            float dz = (1 - RandomHelper.GetRandom(3)) / 1000.0f;
            float dy = vel;

            int num = RandomHelper.GetRandom(3);
            switch (num) {
                case 0:
                    dx = 0.0f;
                    break;
                case 1:
                    dy = 0.0f;
                    break;
                case 2:
                    dz = 0.0f;
                    break;
            }

            bubble.X += dx;
            bubble.Y += dy;
            bubble.Z += dz;
            isSurfaced = (bubble.Y >= waterHeight);
        }

        public void DrawBubbles(SceneRenderer renderer, Point3D aeratorPt, float waterHeight, IList<M3DBubble> surfacedBubbles)
        {
            renderer.Color4f(1.0f, 1.0f, 1.0f, 0.45f);

            foreach (M3DBubble bubble in fBubbles) {
                bool isSurfaced;
                Update(bubble, waterHeight, out isSurfaced);
                if (isSurfaced) {
                    if (surfacedBubbles != null) {
                        var surfBubble = bubble.Clone();
                        surfacedBubbles.Add(surfBubble);
                    }
                    Init(bubble);
                }

                var bblPt = aeratorPt.Add(bubble.X, bubble.Y, bubble.Z);
                renderer.DrawSphere(bblPt, bubble.Size, 16, 16);
            }
        }
    }
}
