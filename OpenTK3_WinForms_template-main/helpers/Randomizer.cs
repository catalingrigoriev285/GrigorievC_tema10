using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK3_StandardTemplate_WinForms.helpers
{
    public static class Randomizer
    {
        private static Random rnd;

        public static void Init()
        {
            rnd = new Random();
        }

        public static Color GetRandomColor()
        {
            Color newColor = Color.FromArgb(rnd.Next(255), rnd.Next(255), rnd.Next(255));

            return newColor;
        }

        public static int GetFirstN(int n)
        {
            int val = rnd.Next(n);

            return val;
        }

        public static int GetNInterval(int n)
        {
            int val = rnd.Next(2 * n);
            val = val - n;

            return val;
        }

        public static int GetNtoM(int n, int m)
        {
            int lim1, lim2;

            if (n > m)
            {
                lim1 = m;
                lim2 = n;
            } else
            {
                lim1 = n;
                lim2 = m;
            }

            int val = rnd.Next(lim2 - lim1);
            val = val + lim1;

            return val;
        }
    }
}
