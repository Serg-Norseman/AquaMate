/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;
using System.Collections.Generic;

namespace AquaMate.M3DViewer
{
    public sealed class M3DWaterSurface
    {
        private class Cell
        {
            public float Y, V;
        }

        private BoundingBox3D fBoundingBox;
        private float fCellStep;
        private Cell[,] fCells;
        private int fColsCount;
        private float fCurrentTime;
        private bool fIsInitiated;
        private float fLastTime;
        private Vector3D[,] fNormals;
        private Point3D fOffset;
        private readonly Random fRandom;
        private int fRowsCount;
        private long fStartTime;


        public bool IsInitiated
        {
            get { return fIsInitiated; }
        }


        public M3DWaterSurface()
        {
            fIsInitiated = false;
            fRandom = new Random();
        }

        public void Initialize(Point3D[] surfacePolygon, Point3D offset, int size = 200)
        {
            fOffset = offset;

            fBoundingBox = new BoundingBox3D(0);
            foreach (var pt in surfacePolygon) {
                fBoundingBox.CheckPoint(pt);
            }

            // Y ignored
            float xSize = fBoundingBox.GetSizeX();
            float zSize = fBoundingBox.GetSizeZ();
            fCellStep = Math.Max(xSize, zSize) / size;

            fColsCount = (int)Math.Round(xSize / fCellStep);
            fRowsCount = (int)Math.Round(zSize / fCellStep);

            fCells = new Cell[fRowsCount + 2, fColsCount + 2];
            fNormals = new Vector3D[fRowsCount, fColsCount];
            for (int row = 0; row < fRowsCount + 2; row++) {
                for (int col = 0; col < fColsCount + 2; col++) {
                    var cell = new Cell();
                    // Improved initialization with smaller initial values for stability
                    cell.Y = 0.01f * (fRandom.Next(100) - 50); // Smaller range of initial values
                    cell.V = 0.0f; // Initial velocity is zero
                    fCells[row, col] = cell;
                }
            }

            fStartTime = DateTime.Now.Ticks;
            fIsInitiated = true;
        }

        public void Draw(SceneRenderer renderer)
        {
            for (int row = 1; row <= fRowsCount; row++) {
                for (int col = 1; col <= fColsCount; col++) {
                    Vector3D normal;
                    normal.Z = fCells[row - 1, col].Y - fCells[row + 1, col].Y;
                    normal.X = fCells[row, col - 1].Y - fCells[row, col + 1].Y;
                    normal.Y = 4;
                    fNormals[row - 1, col - 1] = normal;
                }
            }

            renderer.SetMaterial(M3DMaterials.Water2Diffuse, M3DMaterials.Water2Specular, M3DMaterials.Water2Shininess);

            for (int row = 1; row <= fRowsCount - 1; row++) {
                renderer.BeginTriangleStrip();

                for (int col = 1; col <= fColsCount; col++) {
                    float xx, yy, zz;
                    xx = fBoundingBox.XMin + (col - 1) * fCellStep;

                    var nrm = fNormals[row - 1, col - 1];
                    renderer.Normal3f(nrm.X, nrm.Y, nrm.Z);

                    yy = fBoundingBox.YMin + fCells[row, col].Y / 1000;
                    zz = fBoundingBox.ZMin + (row - 1) * fCellStep;
                    renderer.Vertex3f(xx, yy, zz);

                    nrm = fNormals[row, col - 1];
                    renderer.Normal3f(nrm.X, nrm.Y, nrm.Z);

                    yy = fBoundingBox.YMin + fCells[row + 1, col].Y / 1000;
                    zz = fBoundingBox.ZMin + (row + 0) * fCellStep;
                    renderer.Vertex3f(xx, yy, zz);
                }

                renderer.End();
            }
        }

        public void Next(IList<M3DBubble> surfacedBubbles, bool simpleWaves)
        {
            GenerateBubbles(surfacedBubbles, simpleWaves);

            fLastTime = fCurrentTime;
            fCurrentTime = (DateTime.Now.Ticks - fStartTime) / 1000.0f;
            float dt = (-fLastTime + fCurrentTime) / 100.0f;

            // Limit time step for stability
            dt = Math.Min(dt, 0.5f);

            // Improved wave algorithm for a more realistic effect
            const float waveFrequency = 0.02f; // Reduced wave frequency for stability
            const float waveAmplitude = 0.02f; // Reduced wave amplitude
            const float damping = 0.05f; // Increased damping for stability

            // Stabilizing values to prevent overflow
            for (int row = 0; row < fRowsCount + 2; row++) {
                for (int col = 0; col < fColsCount + 2; col++) {
                    var cell = fCells[row, col];
                    // Limit values to prevent overflow
                    cell.Y = Math.Max(Math.Min(cell.Y, 10.0f), -10.0f);
                    cell.V = Math.Max(Math.Min(cell.V, 5.0f), -5.0f);
                }
            }

            for (int row = 1; row <= fRowsCount; row++) {
                float y_prv = fCells[row, 0].Y;
                float y_cur = fCells[row, 1].Y;

                for (int col = 1; col <= fColsCount; col++) {
                    float y_nxt = fCells[row, col + 1].Y;
                    // Improved wave force calculation taking into account neighboring cells
                    float dy = 4 * y_cur - fCells[row + 1, col].Y - y_nxt - fCells[row - 1, col].Y - y_prv;
                    float v = fCells[row, col].V;
                    // Improved velocity change calculation with damping
                    float dv = -(dy * waveFrequency + damping * v) * dt;
                    fCells[row, col].V = v + dv;
                    y_prv = y_cur;
                    y_cur = y_nxt;
                }
            }

            // Applying wave effect to cells with additional stabilization
            for (int row = 1; row <= fRowsCount; row++) {
                for (int col = 1; col <= fColsCount; col++) {
                    var cell = fCells[row, col];
                    cell.Y += (cell.V * dt);

                    // Limit height change for stability
                    cell.Y = Math.Max(Math.Min(cell.Y, 5.0f), -5.0f);

                    // Add additional waves for more realism
                    float waveOffset = waveAmplitude * (float)Math.Sin(fCurrentTime * 0.0005f + (row + col) * 0.1f);
                    cell.Y += waveOffset;

                    // Additional damping for stability
                    cell.Y *= 0.999f;
                }
            }
        }

        private void ApplyBubble(int xx, int zz, float ptSize = 10.0f)
        {
            const int wSize = 7; // ???
            if (xx == 0 && zz == 0) {
                zz = (fRandom.Next(fRowsCount - wSize * 2) + 1 + wSize);
                xx = (fRandom.Next(fColsCount - wSize * 2) + 1 + wSize);
            }

            int zMin = Math.Max(zz - wSize, 0);
            int zMax = Math.Min(zz + wSize, fRowsCount);
            int xMin = Math.Max(xx - wSize, 0);
            int xMax = Math.Min(xx + wSize, fColsCount);

            for (int row = zMin; row <= zMax; row++) {
                for (int col = xMin; col <= xMax; col++) {
                    var cell = fCells[row, col];
                    int zzR = row - zz;
                    int xxC = col - xx;
                    cell.Y += (ptSize * (float)Math.Exp(-(zzR * zzR + xxC * xxC) / 5.0d));
                }
            }
        }

        private void GenerateBubbles(IList<M3DBubble> surfacedBubbles, bool simpleWaves)
        {
            if (simpleWaves && (fRandom.Next(20) == 1)) {
                ApplyBubble(0, 0, 3); // for debug set ptSize = 10
            }

            float dX = -fOffset.X - fBoundingBox.XMin;
            float dZ = -fOffset.Z - fBoundingBox.ZMin;

            // bubbles from aeration
            foreach (var bubble in surfacedBubbles) {
                var bSize = (int)(bubble.Size * 1000.0f);
                int xx = (int)((bubble.X + dX) / fCellStep);
                int zz = (int)((bubble.Z + dZ) / fCellStep);
                ApplyBubble(xx, zz, bSize);
            }
        }
    }
}
