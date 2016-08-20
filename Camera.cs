
using System;

namespace Raytracer
{
    public class Camera
    {
        static Random random = new Random(DateTime.Now.Millisecond);

        public Camera(Vec3 lookFrom, Vec3 lookAt, Vec3 vUp, double vFov, double aspect, double aperture, double focusDist)
        {
            LensRadius = aperture / 2;
            double theta = vFov * Math.PI/180;
            double half_height = Math.Tan(theta / 2);
            double half_width = aspect * half_height;
            Origin = lookFrom;
            W = (lookFrom - lookAt).UnitVector();
            U = Vec3.Cross(vUp, W).UnitVector();
            V = Vec3.Cross(W, U);
            LowerLeftCorner = Origin - half_width*focusDist*U - half_height*focusDist*V - focusDist*W;
            Horizontal = 2*half_width*focusDist*U;
            Vertical = 2*half_height*focusDist*V;
        }

        public Vec3 Origin {get; protected set;}
        public Vec3 LowerLeftCorner {get; protected set;}
        public Vec3 Horizontal {get; protected set;}
        public Vec3 Vertical {get; protected set;}
        public double LensRadius {get; protected set;}
        public Vec3 U {get; protected set;}
        public Vec3 V {get; protected set;}
        public Vec3 W {get; protected set;}

        public Ray GetRay(double s, double t)
        {
            Vec3 rd = LensRadius*RandomInUnitDisc();
            Vec3 offset = (rd.X * U) + (rd.Y * V);
            return new Ray(Origin + offset, LowerLeftCorner + (s*Horizontal) + (t*Vertical) - Origin - offset);
        }

        private Vec3 RandomInUnitDisc()
        {
            Vec3 p;
            do {
                p = (2.0 * new Vec3(random.NextDouble(), random.NextDouble(), 0)) - new Vec3(1, 1, 0); 
            } while (Vec3.Dot(p, p) >= 1.0);
            return p;
        }
    }
}