using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace OpenTK3_StandardTemplate_WinForms.objects
{
    class Axes
    {
        private bool visibility;
        private const int XYZ_SIZE = 250;
        private Color X_COLOR = Color.Red;
        private Color Y_COLOR = Color.Green;
        private Color Z_COLOR = Color.Blue;

        public Axes(bool _visibility)
        {
            visibility = _visibility;
        }

        public bool GetVisibility()
        {
            return visibility;
        }

        public void SetVisibility(bool _visibility)
        {
            visibility = _visibility;
        }

        public void Show()
        {
            visibility = true;
        }

        public void Hide()
        {
            visibility = false;
        }

        public void ToggleVisibility()
        {
            visibility = !visibility;
        }

        public void Draw()
        {
            if (!visibility)
            {
                return;
            }

            // Set color/coords for Ox, Oy, Oz.
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(X_COLOR);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(XYZ_SIZE, 0, 0);
            GL.Color3(Y_COLOR);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, XYZ_SIZE, 0);
            GL.Color3(Z_COLOR);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0, XYZ_SIZE);
            GL.End();
        }
    }
}
