using System;

namespace OpenTK3_StandardTemplate_WinForms.helpers
{
    public class Coords
    {
        public int X;
        public int Y;
        public int Z;

        public Coords(int _x, int _y, int _z)
        {
            X = _x;
            Y = _y;
            Z = _z;
        }

        public void DisplayCoords()
        {
            Console.Write("(" + X + ","+ Y + ","+ Z + ")");
        }
    }
}
