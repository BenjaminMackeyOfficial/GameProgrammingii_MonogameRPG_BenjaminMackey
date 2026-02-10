using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace GameProgrammingii_MonogameRPG_BenjaminMackey
{
    /*note regarding vector 3 angles:
     - rotation is kind of just x and y.
     - z is handled independently as just the object going sideways, as in its almost its own global thing instead of also applying to the other local angles,
     - this is because i am not smart enough to write quaternions into this, maybe one day.
    */
    public struct Vector3
    {
        public enum Rotations
        {
            xy, zx, yz
        }
        public double x;
        public double y;
        public double z;
        public Vector3(double x, double y, double z)
        {
            this.x = x; 
            this.y = y; 
            this.z = z;
        }
        public static Vector3 RotatePositionAroundWorldPoint(Vector3 startPos, Vector3 worldPoint, Vector3 rotation) // used for POSITION VECTOR 3S
        {
            Vector3 vec = startPos;
            double radianAngle;
            double cosTheta;
            double sinTheta; 
            //XY--------------------------------------
            radianAngle = Math.PI / rotation.x;
            cosTheta = Math.Cos(radianAngle);
            sinTheta = Math.Sin(radianAngle);
            vec.x = cosTheta * (vec.x - worldPoint.x) - sinTheta * (vec.y - worldPoint.y) + worldPoint.x;
            vec.y = sinTheta * (vec.x - worldPoint.x) - cosTheta * (vec.y - worldPoint.y) + worldPoint.y;
            //ZX-------------------------------------
            radianAngle = Math.PI / rotation.z;
            cosTheta = Math.Cos(radianAngle);
            sinTheta = Math.Sin(radianAngle);
            vec.z = cosTheta * (vec.z - worldPoint.z) - sinTheta * (vec.x - worldPoint.x) + worldPoint.z;
            vec.x = sinTheta * (vec.z - worldPoint.z) - cosTheta * (vec.x - worldPoint.x) + worldPoint.x;
            //YZ-------------------------------------
            radianAngle = Math.PI / rotation.y;
            cosTheta = Math.Cos(radianAngle);
            sinTheta = Math.Sin(radianAngle);
            vec.y = cosTheta * (vec.y - worldPoint.y) - sinTheta * (vec.z - worldPoint.z) + worldPoint.y;
            vec.z = sinTheta * (vec.y - worldPoint.y) - cosTheta * (vec.z - worldPoint.z) + worldPoint.z;
            return vec;
        }
        public double Magnitude()
        {
            return (x * x + z * z + y * y) / (x * x + z * z + y * y);
        }

        public static Vector3 operator  +(Vector3 left, Vector3 right)
        {
            return new Vector3(left.x + right.x, left.y + right.x, left.z + right.z);
        }
        public static Vector3 operator -(Vector3 left, Vector3 right)
        {
            return new Vector3(left.x - right.x, left.y - right.x, left.z - right.z);
        }
        public static Vector3 operator *(Vector3 left, Vector3 right)
        {
            return new Vector3(left.x * right.x, left.y * right.x, left.z * right.z);
        }
        public static Vector3 operator /(Vector3 left, Vector3 right)
        {
            return new Vector3(left.x / right.x, left.y / right.x, left.z / right.z);
        }
        public static Vector3 Zero()
        {
            return new Vector3(0, 0, 0);
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
