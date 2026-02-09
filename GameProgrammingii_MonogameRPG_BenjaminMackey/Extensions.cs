using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProgrammingii_MonogameRPG_BenjaminMackey
{
    public static class Extensions
    {
        public static int Clamp(this int num, int min, int max)
        {
            if (num > max) return max;
            if (num < min) return min;
            return num;
        }
    }
}
