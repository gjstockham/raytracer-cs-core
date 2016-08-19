using System;

namespace Raytracer
{
    public abstract class Material
    {
       protected Random Random = new Random(DateTime.Now.Millisecond);

        protected Material()
        {
        }

        public virtual Tuple<bool, Vec3, Ray> Scatter(Ray rayIn, HitRecord rec)
        {
            return new Tuple<bool, Vec3, Ray>(false, null, null);
        }


        protected Vec3 RandomInUnitSphere()
        {
            Vec3 p;
            do
            {
                var temp = new Vec3(Random.NextDouble(), Random.NextDouble(), Random.NextDouble());
                p = (2.0 * temp) - new Vec3(1, 1, 1);
            } while (Vec3.Dot(p, p) >= 1.0);
            return p;
        }

        protected Vec3 Reflect(Vec3 v, Vec3 n)
        {
            return v - 2*Vec3.Dot(v, n)*n;
        }
        
    }

    public class Metal : Material
    {
        private double _fuzziness;

        public double Fuzziness
        {
            get { return (_fuzziness > 1) ? 1 : _fuzziness; }
            set { _fuzziness = value; }
        }

        public Vec3 Albedo { get; set; }


        public override Tuple<bool, Vec3, Ray> Scatter(Ray rayIn, HitRecord rec)
        {
            var unitVector = rayIn.Direction/rayIn.Direction.Length;

            Vec3 reflected = Reflect(unitVector, rec.Normal);
            var scattered = new Ray(rec.P, reflected + Fuzziness*RandomInUnitSphere());
            var attenuation = Albedo;
            var hit =  Vec3.Dot(scattered.Direction, rec.Normal) > 0;
            return new Tuple<bool, Vec3, Ray>(hit, attenuation, scattered);
        }

        public Metal(Vec3 albedo, double fuzziness) : base()
        {
            Albedo = albedo;
            Fuzziness = fuzziness;
        }
    }

    public class Dielectric : Material
    {
        public double RefractionIndex { get; set; }

        public Dielectric(double refractionIndex) : base()
        {
            RefractionIndex = refractionIndex;
        }

        public override Tuple<bool, Vec3, Ray> Scatter(Ray rayIn, HitRecord rec)
        {
            Vec3 outwardNormal;
            var reflected = Reflect(rayIn.Direction, rec.Normal);
            double ni_over_nt;
            var attenuation = new Vec3(1.0, 1.0, 0);
            Vec3 refracted = null;
            double reflectProbability;
            double cosine;
            Ray scattered = null;
            if (Vec3.Dot(rayIn.Direction, rec.Normal) > 0)
            {
                outwardNormal = -1 * rec.Normal;
                ni_over_nt = RefractionIndex;
                cosine = RefractionIndex*Vec3.Dot(rayIn.Direction, rec.Normal)/rayIn.Direction.Length;
            }
            else
            {
                outwardNormal = rec.Normal;
                ni_over_nt = 1.0 / RefractionIndex;
                cosine = -1 * Vec3.Dot(rayIn.Direction, rec.Normal) / rayIn.Direction.Length;
            }
            var refract = Refract(rayIn.Direction, outwardNormal, ni_over_nt);
            refracted = refract.Item2;
            if (refract.Item1)
            {
                reflectProbability = Schlick(cosine, RefractionIndex);
            }
            else
            {
               scattered = new Ray(rec.P, reflected);
                reflectProbability = 1.0;
            }
            if (Random.NextDouble() < reflectProbability)
            {
                scattered = new Ray(rec.P, reflected);
            }
            else
            {
                scattered = new Ray(rec.P, refracted);
            }
            return new Tuple<bool, Vec3, Ray>(true, refracted, scattered);
        }

        private Tuple<bool, Vec3> Refract(Vec3 v, Vec3 n, double ni_over_nt)
        {
            var uv = v / v.Length;
            var dt = Vec3.Dot(uv, n);
            var discriminant = 1.0 - ni_over_nt*ni_over_nt*(1 - dt*dt);
            if (discriminant > 0)
            {
                var refracted = ni_over_nt*(v - n*dt) - n*Math.Sqrt(discriminant);
                return new Tuple<bool, Vec3>(true, refracted);
            }
            return new Tuple<bool, Vec3>(false, null);
        }

        private double Schlick(double cosine, double refractiveIndex)
        {
            double r0 = (1 - refractiveIndex)/(1 + refractiveIndex);
            r0 = r0*r0;
            return r0 + (1 - r0)*Math.Pow((1 - cosine), 5);
        }
    }

    public class Lambertian : Material
    {
        public Vec3 Albedo { get; set; }

        public Lambertian(Vec3 albedo) : base()
        {
            Albedo = albedo;
        }

        public override Tuple<bool, Vec3, Ray> Scatter(Ray rayIn, HitRecord rec)
        {
            var target = rec.P + rec.Normal + RandomInUnitSphere();
            var scattered = new Ray(rec.P, target - rec.P);
            var attenuation = Albedo;
            return new Tuple<bool, Vec3, Ray>(true, attenuation, scattered);
        }      
    }
}