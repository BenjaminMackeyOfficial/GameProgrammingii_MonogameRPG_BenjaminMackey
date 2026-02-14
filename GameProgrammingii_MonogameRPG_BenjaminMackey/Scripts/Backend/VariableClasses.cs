using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
    public enum Plane { xy, yz, zx }
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

            double temp1;
            double temp2;

            //XY--------------------------------------
            radianAngle = rotation.z * Math.PI / 180.0;
            cosTheta = Math.Cos(radianAngle);
            sinTheta = Math.Sin(radianAngle);
            temp1 = vec.x - worldPoint.x;
            temp2 = vec.y - worldPoint.y;
            vec.x = (cosTheta * temp1 - sinTheta * temp2) + worldPoint.x;
            vec.y = (sinTheta * temp1 + cosTheta * temp2) + worldPoint.y;
            
            //ZX-------------------------------------
            radianAngle = rotation.y * Math.PI / 180.0;
            cosTheta = Math.Cos(radianAngle);
            sinTheta = Math.Sin(radianAngle);
            temp1 = vec.z - worldPoint.z;
            temp2 = vec.x - worldPoint.x;
            vec.z = (cosTheta * temp1 - sinTheta * temp2) + worldPoint.z;
            vec.x = (sinTheta * temp1 + cosTheta * temp2) + worldPoint.x;

            //YZ-------------------------------------
            radianAngle = rotation.x * Math.PI / 180.0;
            cosTheta = Math.Cos(radianAngle);
            temp1 = vec.y - worldPoint.y;
            temp2 = vec.z - worldPoint.z;
            sinTheta = Math.Sin(radianAngle);
            vec.y = (cosTheta * temp1 - sinTheta * temp2) + worldPoint.y;
            vec.z = (sinTheta * temp1 + cosTheta * temp2) + worldPoint.z;


            return vec;
        }
        public static Vector3 AngleBetween(Vector3 left, Vector3 right)
        {
            double dirX = right.x.Rad() - left.x.Rad();
            double dirY = right.y.Rad() - left.y.Rad();
            double dirZ = right.z.Rad() - left.z.Rad();

            return new Vector3(
                Math.Atan2(dirY ,Math.Sqrt(dirX * dirX + dirZ * dirZ)),
                Math.Atan2(dirZ, dirX).Deg(),
                0
                );
        }
        public double Magnitude()
        {
            return Math.Sqrt(x * x + y * y + z * z);
        }
        public static double Dot(Vector3 left, Vector3 right)
        {
            return left.x * right.x + left.y * right.y + left.z * right.z;
        }

        public static Vector3 operator  +(Vector3 left, Vector3 right)
        {
            return new Vector3(left.x + right.x, left.y + right.y, left.z + right.z);
        }
        public static Vector3 operator -(Vector3 left, Vector3 right)
        {
            return new Vector3(left.x - right.x, left.y - right.y, left.z - right.z);
        }
        public static Vector3 operator -(Vector3 left)
        {
            return new Vector3(-left.x, -left.y , -left.z);
        }
        public static Vector3 operator *(Vector3 left, Vector3 right)
        {
            return new Vector3(left.x * right.x, left.y * right.y, left.z * right.z);
        }
        public static Vector3 operator *(Vector3 left, float right)
        {
            return new Vector3(left.x * right, left.y * right, left.z * right);
        }
        public static Vector3 operator /(Vector3 left, Vector3 right)
        {
            return new Vector3(left.x / right.x, left.y / right.y, left.z / right.z);
        }
        public static Vector3 Zero()
        {
            return new Vector3(0, 0, 0);
        }

        
    }
    public struct Vector2
    {
        public double x;
        public double y;
        public Vector2(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }

    //behold, vector one
    public struct Vector1
    {
        public double x;
        public Vector1(double x)
        {
            this.x = x;
        }

    }


}
