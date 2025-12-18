using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenTK3_StandardTemplate_WinForms.objects
{
    public class Bee
    {
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; } = Vector3.Zero;
        public float Size { get; set; } = 15.0f;
        
        // 0: Neutral, 1: Up, 2: Down
        private int wingState = 0;
        private int wingDirection = 1;

        public Bee(Vector3 startPos)
        {
            Position = startPos;
        }

        public void UpdateAnimation()
        {
            // 0 -> 1 -> 2 -> 1 -> 0 ...
            wingState += wingDirection;
            if (wingState > 2)
            {
                wingState = 1;
                wingDirection = -1;
            }
            else if (wingState < 0)
            {
                wingState = 1;
                wingDirection = 1;
            }
        }

        public void Draw()
        {
            GL.PushMatrix();
            GL.Translate(Position);
            GL.Rotate(Rotation.X, 1, 0, 0);
            GL.Rotate(Rotation.Y, 0, 1, 0);
            GL.Rotate(Rotation.Z, 0, 0, 1);
            GL.Scale(Size, Size, Size);

            GL.Disable(EnableCap.Texture2D);

            GL.Color3(Color.Yellow);
            DrawSphere(0.5f, 16, 16);

            DrawWings();

            GL.PopMatrix();
        }

        private void DrawWings()
        {
            GL.Color4(Color.FromArgb(100, 255, 255, 255));
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            float wingAngle = 0;
            if (wingState == 0) wingAngle = 0;       // Neutral
            if (wingState == 1) wingAngle = 30;      // Up
            if (wingState == 2) wingAngle = -30;     // Down

            // aripa stanga
            GL.PushMatrix();
            GL.Translate(-0.4f, 0.3f, 0);
            GL.Rotate(wingAngle, 0, 0, 1);
            GL.Rotate(90, 0, 1, 0);
            DrawOval(0.3f, 0.5f);
            GL.PopMatrix();

            // aripa dreapta
            GL.PushMatrix();
            GL.Translate(0.4f, 0.3f, 0);
            GL.Rotate(-wingAngle, 0, 0, 1);
            GL.Rotate(90, 0, 1, 0);
            DrawOval(0.3f, 0.5f);
            GL.PopMatrix();

            GL.Disable(EnableCap.Blend);
        }

        private void DrawOval(float width, float length)
        {
            GL.PushMatrix();
            GL.Scale(width, 0.05f, length);
            DrawSphere(1.0f, 10, 10);
            GL.PopMatrix();
        }

        private void DrawSphere(float radius, int slices, int stacks)
        {
            for (int i = 0; i < stacks; i++)
            {
                double lat0 = Math.PI * (-0.5 + (double)(i) / stacks);
                double z0 = Math.Sin(lat0);
                double zr0 = Math.Cos(lat0);
                
                double lat1 = Math.PI * (-0.5 + (double)(i + 1) / stacks);
                double z1 = Math.Sin(lat1);
                double zr1 = Math.Cos(lat1);

                GL.Begin(PrimitiveType.QuadStrip);
                for (int j = 0; j <= slices; j++)
                {
                    double lng = 2 * Math.PI * (double)(j) / slices;
                    double x = Math.Cos(lng);
                    double y = Math.Sin(lng);

                    GL.Normal3(x * zr0, y * zr0, z0);
                    GL.Vertex3(radius * x * zr0, radius * y * zr0, radius * z0);
                    
                    GL.Normal3(x * zr1, y * zr1, z1);
                    GL.Vertex3(radius * x * zr1, radius * y * zr1, radius * z1);
                }
                GL.End();
            }
        }
    }
}
