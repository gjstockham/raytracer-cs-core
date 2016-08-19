using System;

namespace Raytracer
{
    public class Vec3
    {
        private double[] _internal = new double[] {0, 0, 0}; 

        public Vec3()
        {
        }

        public Vec3(double a0, double a1, double a2)
        {
            _internal[0] = a0;
            _internal[1] = a1;
            _internal[2] = a2;
        }

        public double X { get { return _internal[0]; } }
        public double Y { get { return _internal[1]; } }
        public double Z { get { return _internal[2]; } }
        public double R { get { return _internal[0]; } }
        public double G { get { return _internal[1]; } }
        public double B { get { return _internal[2]; } }
              
        public double Length
        {
            get
            {
                return Math.Sqrt((X*X) + (Y*Y) + (Z*Z));
            }
        }

        public static Vec3 operator +(Vec3 v1, Vec3 v2)
        {
            return new Vec3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static Vec3 operator -(Vec3 v1, Vec3 v2)
        {
            return new Vec3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);         
        }

        public static Vec3 operator *(Vec3 v1, Vec3 v2)
        {
            return new Vec3(v1.X * v2.X, v1.Y * v2.Y, v1.Z * v2.Z);         
        }

        public static Vec3 operator *(Vec3 v1, double d2)
        {
            return new Vec3(v1.X * d2, v1.Y * d2, v1.Z * d2);         
        }
        public static Vec3 operator *(double d1, Vec3 v2)
        {
            return new Vec3(d1*v2.X, d1*v2.Y, d1*v2.Z);         
        }

        public static Vec3 operator / (Vec3 v1, Vec3 v2)
        {
            return new Vec3(v1.X / v2.X, v1.Y / v2.Y, v1.Z / v2.Z);         
        }

        public static Vec3 operator / (Vec3 v1, double d2)
        {
            return new Vec3(v1.X / d2, v1.Y / d2, v1.Z / d2);         
        }

        public static double Dot(Vec3 v1, Vec3 v2)
        {
            return (v1.X*v2.X) + (v1.Y*v2.Y) + (v1.Z*v2.Z);
        }

        public Vec3 Cross(Vec3 v)
        {
            return new Vec3(
                (Y*v.Z) - (Z*v.Y),
                -(X*v.Z) - (Z*v.X),
                (X*v.Y) - (Y*v.X));
        }

        public Vec3 UnitVector()
        {
            return new Vec3(X, Y, Z) / Length;
        }
    }
}