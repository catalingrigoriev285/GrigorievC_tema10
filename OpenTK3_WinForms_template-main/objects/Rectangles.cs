using System;
using System.Collections;
using System.Drawing;

using OpenTK.Graphics.OpenGL;

using OpenTK3_StandardTemplate_WinForms.helpers;

namespace OpenTK3_StandardTemplate_WinForms.objects
{
    class Rectangles
    {
        private ArrayList coordonates;
        private ArrayList colors;
        private PolygonMode currentPolygonState = PolygonMode.Fill;
        private bool visibility;

        public Rectangles()
        {
            coordonates = new ArrayList();
            coordonates.Add(new Coords(5,5,5));
            coordonates.Add(new Coords(12,5,5));
            coordonates.Add(new Coords(12, 2,12));
            coordonates.Add(new Coords(5,2,12));

            colors = new ArrayList();
            for (int i = 0; i < 4; i++)
            {
                colors.Add(Randomizer.GetRandomColor());
            }

            visibility = true;
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

        public void PolygonDrawingStyle(String style)
        {
            if (style == "line")
            {
                currentPolygonState = PolygonMode.Line;
                return;
            }

            if (style == "surface")
            {
                currentPolygonState = PolygonMode.Fill;
                return;
            }

        }

        public void Draw()
        {
            if (!visibility)
            {
                return;
            }

            GL.PolygonMode(MaterialFace.FrontAndBack, currentPolygonState);
            GL.Begin(PrimitiveType.Quads);
            for (int i = 0; i < 4; i++)
            {
                GL.Color3((Color)colors[i]);
                Coords aux = (Coords)coordonates[i];
                GL.Vertex3(aux.X, aux.Y, aux.Z);
            }
            GL.End();
        }
    }
}
