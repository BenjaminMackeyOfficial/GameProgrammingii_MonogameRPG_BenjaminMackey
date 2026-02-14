using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public static float Clamp(this float num, float min, float max)
        {
            if (num > max) return max;
            if (num < min) return min;
            return num;
        }
        public static double Clamp(this double num, double min, double max)
        {
            if (num > max) return max;
            if (num < min) return min;
            return num;
        }

        public static double Rad(this double num)
        {
            return num*Math.PI/180.0;
        }
        public static float Rad(this float num)
        {
            return (float)(num * Math.PI / 180.0);
        }
        public static double Deg(this double num)
        {
            return num *  180.0 / Math.PI;
        }
        public static float Deg(this float num)
        {
            return (float)(num * 180.0 / Math.PI);
        }

        public static float NormalizeAngle(this float num)
        {
            while (num < 0) num += 360;
            while (num > 360) num -= 360;
            return num;
        }
        public static float Normalize(this float num)
        {
            if (num > 0f) return 1f;
            if (num < 0f) return -1f;
            return 0f;
        }
    }
}
