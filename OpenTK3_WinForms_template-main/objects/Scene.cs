using System.Drawing;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenTK3_StandardTemplate_WinForms.objects
{
    class Scene
    {
        private Color DEFAULT_BKG_COLOR = Color.LightGray;

        private GLControl viewport;

        public Scene()
        {
            viewport = new GLControl(new OpenTK.Graphics.GraphicsMode(32, 24, 0, 8));
            viewport.TabIndex = 0;
            viewport.Name = "mainViewport";
            viewport.BackColor = DEFAULT_BKG_COLOR;
            viewport.Location = new Point(12, 12);
            viewport.Size = new Size(800, 600);
            viewport.VSync = false;
        }

        public void Load()
        {
        }

        public Color GetBackgroundColor()
        {
            return viewport.BackColor;
        }

        public void SetBackgroundColor(Color color)
        {
            viewport.BackColor = color;
        }

        public GLControl GetViewport()
        {
            return viewport;
        }

        public void Reset()
        {
            GL.ClearColor(DEFAULT_BKG_COLOR);
            
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Multisample);
            GL.Hint(HintTarget.MultisampleFilterHintNv, HintMode.Nicest);
            GL.Hint(HintTarget.PolygonSmoothHint, HintMode.Nicest);
            GL.Hint(HintTarget.LineSmoothHint, HintMode.Nicest);
        }

        public void Invalidate()
        {
            viewport.Invalidate();
        }

        public void Draw()
        {
        }

        public void Update()
        {
        }

    }
}
