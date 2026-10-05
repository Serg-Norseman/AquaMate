/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

#if !NET8_0_OR_GREATER

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using AquaMate.Core.Model;
using AquaMate.M3DViewer;
using AquaMate.M3DViewer.Tanks;
using BSLib;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace AquaMate.UI.Components
{
    public struct Vertex
    {
        public float x, y, z;
    }

    public class DeviceModel
    {
        public int VertsNum;
        public Vertex[] Vertices;

        public int LightsNum;
        public Vertex[] Lights;
    }

    /// <summary>
    /// 
    /// </summary>
    public class OGLRenderer : SceneRenderer
    {
        private readonly Control fViewer;
        private int fListBase;

        public OGLRenderer(Control viewer)
        {
            fViewer = viewer;
        }

        public override void PushMatrix()
        {
            GL.PushMatrix();
        }

        public override void PopMatrix()
        {
            GL.PopMatrix();
        }

        public override void Translatef(float x, float y, float z)
        {
            GL.Translate(x, y, z);
        }

        public override void Rotatef(float angle, float x, float y, float z)
        {
            GL.Rotate(angle, x, y, z);
        }

        public override void Vertex3f(float x, float y, float z)
        {
            GL.Vertex3(x, y, z);
        }

        public override void Normal3f(float nx, float ny, float nz)
        {
            GL.Normal3(nx, ny, nz);
        }

        public override void Begin(uint mode)
        {
            //GL.Begin(mode);
        }

        public override void End()
        {
            GL.End();
        }

        public override void BeginTriangleStrip()
        {
            GL.Begin(BeginMode.TriangleStrip);
        }

        public override void BeginPolygon()
        {
            GL.Begin(BeginMode.Polygon);
        }

        public override void BeginTriangleFan()
        {
            GL.Begin(BeginMode.TriangleFan);
        }

        public override void Color4f(float red, float green, float blue, float alpha)
        {
            GL.Color4(red, green, blue, alpha);
        }

        public override void DrawSolidSphere(double radius, int slices, int stacks)
        {
            //GLUT.glutSolidSphere(radius, slices, stacks);
        }

        public override void SetMaterial(float[] diffParams, float[] specParams, float[] shin)
        {
            if (diffParams != null) {
                GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Diffuse, diffParams);
            }
            if (specParams != null) {
                GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Specular, specParams);
            }
            if (shin != null) {
                GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Shininess, shin);
            }
            //GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_EMISSION, new float[] { 0.7f, 0.7f, 0.7f, 0.1f });
        }

        public override void SetLight(uint index, float[] ambiParams, float[] diffParams, float[] specParams, float[] pos)
        {
            LightName light = (LightName)((int)LightName.Light0 + index);

            EnableCap lightCap = (EnableCap)((int)EnableCap.Light0 + index);
            GL.Enable(lightCap);

            if (ambiParams != null) {
                GL.Light(light, LightParameter.Ambient, ambiParams);
            }

            if (diffParams != null) {
                GL.Light(light, LightParameter.Diffuse, diffParams);
            }

            if (specParams != null) {
                GL.Light(light, LightParameter.Specular, specParams);
            }

            if (pos != null) {
                GL.Light(light, LightParameter.Position, pos);
            }
        }

        public override void SetViewport(int width, int height, float fovY, float zNear, float zFar)
        {
            if (width > 0 && height > 0) {
                GL.Viewport(0, 0, width, height);

                GL.MatrixMode(MatrixMode.Projection);
                GL.LoadIdentity();

                //GLU.gluPerspective(fovY, (float)width / height, zNear, zFar);
                Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(
                    OpenTK.Mathematics.MathHelper.DegreesToRadians(fovY),
                    (float)width / (float)height,
                    (float)Math.Max(0.0001, zNear),
                    zFar
                );
                GL.LoadMatrix(ref perspective);

                GL.MatrixMode(MatrixMode.Modelview);
                GL.LoadIdentity();
            }
        }

        public override void DrawTriangle(Point3D point1, Point3D point2, Point3D point3, Point3D normal)
        {
            if (normal.IsZero()) {
                normal = CalculateSurfaceNormal(point1, point2, point3);
            }

            GL.Begin(BeginMode.Triangles);

            GL.Normal3(normal.X, normal.Y, normal.Z);
            GL.Vertex3(point1.X, point1.Y, point1.Z);

            GL.Normal3(normal.X, normal.Y, normal.Z);
            GL.Vertex3(point2.X, point2.Y, point2.Z);

            GL.Normal3(normal.X, normal.Y, normal.Z);
            GL.Vertex3(point3.X, point3.Y, point3.Z);

            GL.End();
        }

        public override void InitScene()
        {
            GL.ClearDepth(1.0f);
            GL.ShadeModel(ShadingModel.Smooth); // GL_FLAT?
            GL.Enable(EnableCap.DepthTest);
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
            GL.Enable(EnableCap.ColorMaterial);
            GL.Enable(EnableCap.CullFace);

            GL.Enable(EnableCap.PointSmooth);
            GL.Hint(HintTarget.PointSmoothHint, HintMode.Nicest);

            GL.Enable(EnableCap.LineSmooth);
            GL.Hint(HintTarget.LineSmoothHint, HintMode.Nicest);

            GL.Enable(EnableCap.PolygonSmooth);
            GL.Hint(HintTarget.PolygonSmoothHint, HintMode.Nicest);

            GL.Disable(EnableCap.Light0);
            GL.Disable(EnableCap.Light1);
            GL.Disable(EnableCap.Light2);
            GL.Disable(EnableCap.Light3);
            GL.Disable(EnableCap.Light4);
            GL.Disable(EnableCap.Light5);
            GL.Disable(EnableCap.Light6);
            GL.Disable(EnableCap.Light7);
        }

        public override void BeginDrawing()
        {
            BuildFont();

            GL.ClearColor(0.25f, 0.25f, 0.25f, 0.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadIdentity();

            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Normalize);
            GL.LightModel(LightModelParameter.LightModelTwoSide, (int)OpenTK.Graphics.OpenGL.Boolean.True);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            GL.PushMatrix();
        }

        public override void EndDrawing()
        {
            GL.PopMatrix();
        }

        private void BuildFont()
        {
            //fViewer.Font = new Font("Courier New", 24.0f, FontStyle.Bold);
            /*fListBase = GL.GenLists(128);
            using (var gfx = fViewer.CreateGraphics()) {
                wglUseFontBitmaps(gfx.GetHdc(), 0, 128, fListBase);
            }*/
        }

        public override void DrawText(string text, float x, float y, float z)
        {
            /* if required static position of text?
            OpenGL.glLoadIdentity();
            OpenGL.glTranslatef(0, 0, 0.0f);*/

            /*GL.Disable(EnableCap.Lighting);
            GL.Color3(1.0f, 0.0f, 0.0f);
            GL.RasterPos3(x, y, z);

            GL.PushAttrib(AttribMask.ListBit);
            GL.ListBase((uint)fListBase);
            GL.CallLists(text.Length, ListNameType.UnsignedShort, text);
            GL.PopAttrib();*/
        }

        [DllImport("opengl32.dll")]
        private static extern void wglUseFontBitmaps(IntPtr hdc, uint first, uint count, uint listBase);

        public override void DrawSphere(Point3D pt, double radius, int slices, int stacks)
        {
            PushMatrix();
            Translatef(pt.X, pt.Y, pt.Z);
            DrawSolidSphere(radius, slices, stacks);
            PopMatrix();
        }

        #region Models

        private enum LineMode { None, Vert, Light }

        public DeviceModel ObjLoad(string fileName)
        {
            if (!File.Exists(fileName)) {
                return null;
            }

            DeviceModel result = new DeviceModel();
            StreamReader reader = null;
            try {
                reader = new StreamReader(fileName, new ASCIIEncoding());

                string line;
                string[] splitter;
                LineMode mode = LineMode.None;
                int vi = -1;
                int li = -1;
                while ((line = reader.ReadLine()) != null) {
                    if (!string.IsNullOrEmpty(line)) {
                        if (line.StartsWith("Vertices")) {
                            splitter = line.Split();
                            int num = Convert.ToInt32(splitter[1]);
                            result.VertsNum = num;
                            result.Vertices = new Vertex[num];
                            mode = LineMode.Vert;
                            continue;
                        }

                        if (line.StartsWith("Lights")) {
                            splitter = line.Split();
                            int num = Convert.ToInt32(splitter[1]);
                            result.LightsNum = num;
                            result.Lights = new Vertex[num];
                            mode = LineMode.Light;
                            continue;
                        }

                        if (mode == LineMode.Vert) {
                            vi += 1;
                            splitter = line.Split();

                            float rx = (float)ConvertHelper.ParseFloat(splitter[0], 0.0f, true);
                            float ry = (float)ConvertHelper.ParseFloat(splitter[1], 0.0f, true);
                            float rz = (float)ConvertHelper.ParseFloat(splitter[2], 0.0f, true);

                            result.Vertices[vi].x = rx * TankRenderer<ITank>.ScaleFactor;
                            result.Vertices[vi].y = ry * TankRenderer<ITank>.ScaleFactor;
                            result.Vertices[vi].z = rz * TankRenderer<ITank>.ScaleFactor;
                        }

                        if (mode == LineMode.Light) {
                            li += 1;
                            splitter = line.Split();

                            float rx = (float)ConvertHelper.ParseFloat(splitter[0], 0.0f, true);
                            float ry = (float)ConvertHelper.ParseFloat(splitter[1], 0.0f, true);
                            float rz = (float)ConvertHelper.ParseFloat(splitter[2], 0.0f, true);

                            result.Lights[li].x = rx * TankRenderer<ITank>.ScaleFactor;
                            result.Lights[li].y = ry * TankRenderer<ITank>.ScaleFactor;
                            result.Lights[li].z = rz * TankRenderer<ITank>.ScaleFactor;
                        }
                    }
                }
            } catch (Exception e) {
                string errorMsg = "An Error Occurred While Loading And Parsing Object Data:\n\t" + fileName + "\n" + "\n\nStack Trace:\n\t" + e.StackTrace + "\n";
                MessageBox.Show(errorMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            } finally {
                if (reader != null) {
                    reader.Close();
                }
            }
            return result;
        }

        public static readonly float[] AlumDiffuse = new float[] { 0.5f, 0.5f, 0.5f, 1.0f };
        public static readonly float[] AlumSpecular = new float[] { 0.95f, 0.95f, 0.95f, 1.0f };
        public static readonly float[] AlumShininess = new float[] { 128.0f };

        public void ObjDraw(DeviceModel morph)
        {
            GL.PushMatrix();

            Translatef(-0.25f, 0.4f, 0.0f);
            GL.Enable(EnableCap.Lighting);

            for (uint i = 0; i < morph.LightsNum; i++) {
                var vtx = morph.Lights[i];
                SetLight(2 + i, LightAmbient, LightDiffuse, LightSpecular, new float[] { vtx.x, vtx.y, vtx.z });
            }

            SetMaterial(AlumDiffuse, AlumSpecular, AlumShininess);

            GL.Begin(BeginMode.Quads);
            for (int i = 0; i < morph.VertsNum; i++) {
                var vtx = morph.Vertices[i];
                GL.Vertex3(vtx.x, vtx.y, vtx.z);
            }
            GL.End();

            GL.PopMatrix();
        }

        #endregion
    }
}

#endif
