/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

namespace AquaMate.M3DViewer
{
    public static class M3DMaterials
    {
        public static readonly float[] LightAmbient = { 0.5f, 0.5f, 0.5f, 0.95f };
        public static readonly float[] LightDiffuse = { 1.0f, 1.0f, 1.0f, 1.0f };
        public static readonly float[] LightSpecular = { 1.0f, 1.0f, 1.0f, 1.0f };
        public static readonly float[] LightPosition = { 0.0f, 5.0f, -5.0f, 1.0f };


        public static readonly float[] GlassDiffuse = new float[] { 0.878f, 1.0f, 1.0f, 0.3f };
        public static readonly float[] GlassSpecular = new float[] { 0.95f, 0.95f, 0.95f, 1.0f };
        public static readonly float[] GlassShininess = new float[] { 128.0f };

        /*public static readonly float[] GlassDiffuse = new float[] { 0.588235f, 0.670588f, 0.729412f, 1.0f };
        public static readonly float[] GlassSpecular = new float[] { 0.9f, 0.9f, 0.9f, 1.0f };
        public static readonly float[] GlassShininess = new float[] { 96.0f };*/


        public static readonly float[] AlumDiffuse = new float[] { 0.5f, 0.5f, 0.5f, 1.0f };
        public static readonly float[] AlumSpecular = new float[] { 0.95f, 0.95f, 0.95f, 1.0f };
        public static readonly float[] AlumShininess = new float[] { 128.0f };


        public static readonly float[] Water2Diffuse = new float[] { 0.1f, 0.3f, 0.8f, 0.7f };
        public static readonly float[] Water2Specular = new float[] { 1.0f, 1.0f, 1.0f, 0.8f };
        public static readonly float[] Water2Shininess = new float[] { 64.0f };
    }
}
