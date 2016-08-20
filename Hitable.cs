using System;
using System.Collections.Generic;

namespace Raytracer
{
    public class HitRecord
    {
        public double T {get; set;}
        public Vec3 P {get; set;}
        public Vec3 Normal {get; set;}
        public Material Material {get; set;}
    }
    
    public abstract class Hitable
    {
        public abstract bool Hit(Ray r, double tmin, double tmax, out HitRecord rec);
        
    }

    public class HitableList : Hitable
    {
        private IList<Hitable> _hitables = new List<Hitable>();
        
        public override bool Hit(Ray r, double tMin, double tMax, out HitRecord rec)
        {
            HitRecord tempRecord = null;
            rec = null;
            bool hitAnything = false;
            double closestSoFar = tMax;
            foreach (var hitable in _hitables)
            {
                if(hitable.Hit(r, tMin, closestSoFar, out tempRecord))
                {
                    hitAnything = true;
                    closestSoFar = tempRecord.T;
                    rec = tempRecord;
                }
            }
            return hitAnything;
        }

        public void Add(Hitable hitable)
        {
            _hitables.Add(hitable);
        }
    }

    public class Sphere : Hitable
    {
        public Vec3 Centre { get; set; }
        public double Radius { get; set; }
        public Material Material { get; set; }

        public Sphere(Vec3 centre, double radius, Material material)
        {
            Centre = centre;
            Radius = radius;
            Material = material;
        }

        public override bool Hit(Ray r, double tMin, double tMax, out HitRecord rec)
        {
            Vec3 oc = r.Origin - Centre;
            double a = Vec3.Dot(r.Direction, r.Direction);
            double b = Vec3.Dot(oc, r.Direction);
            double c = Vec3.Dot(oc, oc) - (Radius * Radius);
            double discriminant = b * b - a * c;
            if (discriminant > 0)
            {
                double temp = (-b - Math.Sqrt(b*b - a*c))/a;
                if (temp < tMax && temp > tMin)
                {
                    rec = new HitRecord();
                    rec.T = temp;
                    rec.P = r.PointAt(rec.T);
                    rec.Normal = (rec.P - Centre)/Radius;
                    rec.Material = Material;
                    return true;
                }
                temp = (-b + Math.Sqrt(b * b - a * c)) / a;
                if (temp < tMax && temp > tMin)
                {
                    rec = new HitRecord();
                    rec.T = temp;
                    rec.P = r.PointAt(rec.T);
                    rec.Normal = (rec.P - Centre) / Radius;
                    rec.Material = Material;
                    return true;
                }
            }
            rec = null;
            return false;
        }
    }
    
}