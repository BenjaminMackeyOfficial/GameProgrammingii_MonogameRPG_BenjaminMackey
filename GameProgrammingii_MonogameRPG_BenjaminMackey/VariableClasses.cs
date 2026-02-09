using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProgrammingii_MonogameRPG_BenjaminMackey
{

    public struct Vector3
    {
        public float x;
        public float y;
        public float z;
        public Vector3(int x, int y, int z)
        {
            this.x = x; 
            this.y = y; 
            this.z = z;
        }
    }
    public struct Vector2
    {
        public float x;
        public float y;
        public Vector2(int x, int y)
        {
            this.x = x;
            this.y = y;

        }

    }

    //behold, vector one
    public struct Vector1
    {
        public float x;
        public Vector1(int x)
        {
            this.x = x;
        }

    }


}
