/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
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
using CsGL.OpenGL;

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
        private uint fListBase;

        public OGLRenderer(Control viewer)
        {
            fViewer = viewer;
        }

        public override void PushMatrix()
        {
            OpenGL.glPushMatrix();
        }

        public override void PopMatrix()
        {
            OpenGL.glPopMatrix();
        }

        public override void Translatef(float x, float y, float z)
        {
            OpenGL.glTranslatef(x, y, z);
        }

        public override void Rotatef(float angle, float x, float y, float z)
        {
            OpenGL.glRotatef(angle, x, y, z);
        }

        public override void Vertex3f(float x, float y, float z)
        {
            OpenGL.glVertex3f(x, y, z);
        }

        public override void Normal3f(float nx, float ny, float nz)
        {
            OpenGL.glNormal3f(nx, ny, nz);
        }

        public override void Begin(uint mode)
        {
            OpenGL.glBegin(mode);
        }

        public override void End()
        {
            OpenGL.glEnd();
        }

        public override void BeginTriangleStrip()
        {
            OpenGL.glBegin(OpenGL.GL_TRIANGLE_STRIP);
        }

        public override void BeginPolygon()
        {
            OpenGL.glBegin(OpenGL.GL_POLYGON);
        }

        public override void BeginTriangleFan()
        {
            OpenGL.glBegin(OpenGL.GL_TRIANGLE_FAN);
        }

        public override void Color4f(float red, float green, float blue, float alpha)
        {
            OpenGL.glColor4f(red, green, blue, alpha);
        }

        public override void DrawSolidSphere(double radius, int slices, int stacks)
        {
            GLUT.glutSolidSphere(radius, slices, stacks);
        }

        public override void SetMaterial(float[] diffParams, float[] specParams, float[] shin)
        {
            if (diffParams != null) {
                GL.glMaterialfv(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_DIFFUSE, diffParams);
            }
            if (specParams != null) {
                GL.glMaterialfv(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_SPECULAR, specParams);
            }
            if (shin != null) {
                GL.glMaterialfv(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_SHININESS, shin);
            }
            //GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_EMISSION, new float[] { 0.7f, 0.7f, 0.7f, 0.1f });
        }

        public override void SetLight(uint index, float[] ambiParams, float[] diffParams, float[] specParams, float[] pos)
        {
            uint light = OpenGL.GL_LIGHT0 + index;
            OpenGL.glEnable(light);

            if (ambiParams != null) {
                GL.glLightfv(light, OpenGL.GL_AMBIENT, ambiParams);
            }

            if (diffParams != null) {
                GL.glLightfv(light, OpenGL.GL_DIFFUSE, diffParams);
            }

            if (specParams != null) {
                GL.glLightfv(light, OpenGL.GL_SPECULAR, specParams);
            }

            if (pos != null) {
                GL.glLightfv(light, OpenGL.GL_POSITION, pos);
            }
        }

        public override void SetViewport(int width, int height, float fovY, float zNear, float zFar)
        {
            if (width > 0 && height > 0) {
                OpenGL.glViewport(0, 0, width, height);
                OpenGL.glMatrixMode(OpenGL.GL_PROJECTION);
                OpenGL.glLoadIdentity();

                GLU.gluPerspective(fovY, (float)width / height, zNear, zFar);

                OpenGL.glMatrixMode(OpenGL.GL_MODELVIEW);
                OpenGL.glLoadIdentity();
            }
        }

        public override void DrawTriangle(Point3D point1, Point3D point2, Point3D point3, Point3D normal)
        {
            if (normal.IsZero()) {
                normal = CalculateSurfaceNormal(point1, point2, point3);
            }

            OpenGL.glBegin(OpenGL.GL_TRIANGLES);

            OpenGL.glNormal3f(normal.X, normal.Y, normal.Z);
            OpenGL.glVertex3f(point1.X, point1.Y, point1.Z);

            OpenGL.glNormal3f(normal.X, normal.Y, normal.Z);
            OpenGL.glVertex3f(point2.X, point2.Y, point2.Z);

            OpenGL.glNormal3f(normal.X, normal.Y, normal.Z);
            OpenGL.glVertex3f(point3.X, point3.Y, point3.Z);

            OpenGL.glEnd();
        }

        public override void InitScene()
        {
            OpenGL.glClearDepth(1.0f);
            OpenGL.glShadeModel(OpenGL.GL_SMOOTH); // GL_FLAT?
            OpenGL.glEnable(OpenGL.GL_DEPTH_TEST);
            OpenGL.glHint(OpenGL.GL_PERSPECTIVE_CORRECTION_HINT, OpenGL.GL_NICEST);
            OpenGL.glEnable(OpenGL.GL_COLOR_MATERIAL);
            OpenGL.glEnable(OpenGL.GL_CULL_FACE);

            OpenGL.glEnable(OpenGL.GL_POINT_SMOOTH);
            OpenGL.glHint(OpenGL.GL_POINT_SMOOTH_HINT, OpenGL.GL_NICEST);

            OpenGL.glEnable(OpenGL.GL_LINE_SMOOTH);
            OpenGL.glHint(OpenGL.GL_LINE_SMOOTH_HINT, OpenGL.GL_NICEST);

            OpenGL.glEnable(OpenGL.GL_POLYGON_SMOOTH);
            OpenGL.glHint(OpenGL.GL_POLYGON_SMOOTH_HINT, OpenGL.GL_NICEST);

            OpenGL.glDisable(OpenGL.GL_LIGHT0);
            OpenGL.glDisable(OpenGL.GL_LIGHT1);
            OpenGL.glDisable(OpenGL.GL_LIGHT2);
            OpenGL.glDisable(OpenGL.GL_LIGHT3);
            OpenGL.glDisable(OpenGL.GL_LIGHT4);
            OpenGL.glDisable(OpenGL.GL_LIGHT5);
            OpenGL.glDisable(OpenGL.GL_LIGHT6);
            OpenGL.glDisable(OpenGL.GL_LIGHT7);
        }

        public override void BeginDrawing()
        {
            BuildFont();

            OpenGL.glClearColor(0.25f, 0.25f, 0.25f, 0.0f);
            OpenGL.glClear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            OpenGL.glLoadIdentity();

            OpenGL.glEnable(OpenGL.GL_LIGHTING);
            OpenGL.glEnable(OpenGL.GL_NORMALIZE);
            OpenGL.glLightModelf(OpenGL.GL_LIGHT_MODEL_TWO_SIDE, (int)GL.GL_TRUE);
            OpenGL.glEnable(OpenGL.GL_BLEND);
            OpenGL.glBlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);

            OpenGL.glPushMatrix();
        }

        public override void EndDrawing()
        {
            OpenGL.glPopMatrix();
        }

        private void BuildFont()
        {
            //fViewer.Font = new Font("Courier New", 24.0f, FontStyle.Bold);
            fListBase = GL.glGenLists(128);
            using (var gfx = fViewer.CreateGraphics()) {
                wglUseFontBitmaps(gfx.GetHdc(), 0, 128, fListBase);
            }
        }

        public override void DrawText(string text, float x, float y, float z)
        {
            /* if required static position of text?
            OpenGL.glLoadIdentity();
            OpenGL.glTranslatef(0, 0, 0.0f);*/

            OpenGL.glDisable(OpenGL.GL_LIGHTING);
            OpenGL.glColor3f(1.0f, 0.0f, 0.0f);
            OpenGL.glRasterPos3f(x, y, z);

            OpenGL.glPushAttrib(OpenGL.GL_LIST_BIT);
            GL.glListBase((uint)fListBase);
            GL.glCallLists(text.Length, OpenGL.GL_UNSIGNED_SHORT, text);
            OpenGL.glPopAttrib();
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
            OpenGL.glPushMatrix();

            Translatef(-0.25f, 0.4f, 0.0f);
            OpenGL.glEnable(OpenGL.GL_LIGHTING);

            for (uint i = 0; i < morph.LightsNum; i++) {
                var vtx = morph.Lights[i];
                SetLight(2 + i, LightAmbient, LightDiffuse, LightSpecular, new float[] { vtx.x, vtx.y, vtx.z });
            }

            SetMaterial(AlumDiffuse, AlumSpecular, AlumShininess);

            OpenGL.glBegin(OpenGL.GL_QUADS);
            for (int i = 0; i < morph.VertsNum; i++) {
                var vtx = morph.Vertices[i];
                OpenGL.glVertex3f(vtx.x, vtx.y, vtx.z);
            }
            OpenGL.glEnd();

            OpenGL.glPopMatrix();
        }

        #endregion
    }
}

#endif
