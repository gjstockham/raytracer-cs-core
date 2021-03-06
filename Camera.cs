
using System;

namespace Raytracer
{
    public class Camera
    {
        public Camera(Vec3 lookFrom, Vec3 lookAt, Vec3 vUp, double vFov, double aspect)
        {
            Vec3 u,v,w;
            double theta = vFov * Math.PI/180;
            double half_height = Math.Tan(theta / 2);
            double half_width = aspect * half_height;
            Origin = lookFrom;
            w = (lookFrom - lookAt).UnitVector();
            Console.WriteLine($"w: {w.X} {w.Y} {w.Z}");
            u = Vec3.Cross(vUp, w).UnitVector();
            Console.WriteLine($"u: {u.X} {u.Y} {u.Z}");           
            v = Vec3.Cross(w, u);
            Console.WriteLine($"v: {v.X} {v.Y} {v.Z}");
            LowerLeftCorner = Origin - half_width*u - half_height*v - w;
            Horizontal = 2*half_width*u;
            Vertical = 2*half_height*v;
        }

        public Vec3 Origin {get; set;}
        public Vec3 LowerLeftCorner {get; set;}
        public Vec3 Horizontal {get; set;}
        public Vec3 Vertical {get; set;}

        public Ray GetRay(double u, double v)
        {
            return new Ray(Origin, LowerLeftCorner + (u*Horizontal) + (v*Vertical) - Origin);
        }
    }
}