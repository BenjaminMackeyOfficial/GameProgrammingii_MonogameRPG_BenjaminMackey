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


        public static Vector2 DigThrouh2D(this List<int[]> stuff, Vector2 searchFocus, Vector2 blacklist, int target, Vector2 bounds) //hyper focused booooo, low on time
        {
            Vector2[] directions = new Vector2[] {
                                    new Vector2(0,1),   // up
                                    new Vector2(1,0),   // right
                                    new Vector2(0,-1),  // down
                                    new Vector2(-1,0)   // left
                                };

            foreach (var dir in directions)
            {
                int x = (int)searchFocus.x + (int)dir.x;
                int y = (int)searchFocus.y + (int)dir.y;

                if (x < 0 || x >= stuff.Count) continue;
                if (y < 0 || y >= stuff[x].Length) continue;
                if (x == (int)blacklist.x && y == (int)blacklist.y) continue;

                if (stuff[x][y] == target)
                    return new Vector2(x, y);
            }
            return new Vector2(-1, -1);
        }
        
    }
}
