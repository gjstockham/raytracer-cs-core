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
        public abstract Tuple<bool, HitRecord> Hit(Ray r, double tmin, double tmax);
        
    }

    public class HitableList : Hitable
    {
        private IList<Hitable> _hitables = new List<Hitable>();
        
        public override Tuple<bool, HitRecord> Hit(Ray r, double tmin, double tmax)
        {
            HitRecord toReturn = null;
            bool hitAnything = false;
            double closestSoFar = tmax;
            foreach (var item in _hitables)
            {
                var hit = item.Hit(r, tmin, closestSoFar);
                if(hit.Item1)
                {
                    hitAnything = true;
                    closestSoFar = hit.Item2.T;
                    toReturn = hit.Item2;
                }
            }
            return new Tuple<bool, HitRecord>(hitAnything, toReturn);
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

        public override Tuple<bool, HitRecord> Hit(Ray r, double tMin, double tMax)
        {
            var rec = new HitRecord();
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
                    return new Tuple<bool, HitRecord>(true, rec);
                }
                temp = (-b + Math.Sqrt(b * b - a * c)) / a;
                if (temp < tMax && temp > tMin)
                {
                    rec = new HitRecord();
                    rec.T = temp;
                    rec.P = r.PointAt(rec.T);
                    rec.Normal = (rec.P - Centre) / Radius;
                    rec.Material = Material;
                    return new Tuple<bool, HitRecord>(true, rec);
                }
            }
            rec = null;
            return new Tuple<bool, HitRecord>(false, null);
        }
    }
    
}