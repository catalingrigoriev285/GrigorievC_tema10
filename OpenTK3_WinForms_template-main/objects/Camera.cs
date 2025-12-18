

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenTK3_StandardTemplate_WinForms.objects
{
    public class Camera
    {
        private int camPosX, camPosY, camPosZ;
        private float camDepth;
        private int viewportWidth, viewportHeight;

        private const float CAM_DEPTH = 1.04f;
        private const int CAM_POS_X = 100;
        private const int CAM_POS_Y = 100;
        private const int CAM_POS_Z = 200;

        public Camera(GLControl viewport)
        {
            Reset();

            viewportWidth = viewport.Width;
            viewportHeight = viewport.Height;
        }

        public void SetView()
        {
            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(camDepth, 4f / 3f, 1f, 1000f);
            Matrix4 lookat = Matrix4.LookAt(camPosX, camPosY, camPosZ, 40, 0, 0, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.LoadMatrix(ref perspective);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.LoadMatrix(ref lookat);
            GL.Viewport(0, 0, viewportWidth, viewportHeight);
        }

        public void Reset()
        {
            camDepth = CAM_DEPTH;
            camPosX = CAM_POS_X;
            camPosY = CAM_POS_Y;
            camPosZ = CAM_POS_Z;
        }

    }
}
