using System;
using System.IO;

namespace Raytracer
{
    public class Program
    {
        static Random random = new Random(DateTime.Now.Millisecond);

        public static void Main(string[] args)
        {
            int nx = 200;
            int ny = 100;
            int ns = 100;

            using(var file = File.CreateText("out.ppm"))
            {
                file.WriteLine($"P3\n{nx} {ny}\n255");

                var world = new HitableList();
                world.Add(new Sphere(new Vec3(0, 0, -1), 0.5, new Lambertian(new Vec3(0.1, 0.2, 0.5))));
                world.Add(new Sphere(new Vec3(0, -100.5, -1), 100, new Lambertian(new Vec3(0.8, 0.8, 0))));
                world.Add(new Sphere(new Vec3(1, 0, -1), 0.5, new Metal(new Vec3(0.8, 0.6, 0.2), 0.3)));
                world.Add(new Sphere(new Vec3(-1, 0, -1), 0.5, new Dielectric(1.5)));
                var camera = new Camera(new Vec3(-2, 2, 1), new Vec3(0, 0, -1), new Vec3(0, 1, 0), 20, (double)nx/(double)ny);
                Console.WriteLine($"Camera.Origin: {camera.Origin.X} {camera.Origin.Y} {camera.Origin.Z}");
                Console.WriteLine($"Camera.Horizontal: {camera.Horizontal.X} {camera.Horizontal.Y} {camera.Horizontal.Z}");
                Console.WriteLine($"Camera.Vertical: {camera.Vertical.X} {camera.Vertical.Y} {camera.Vertical.Z}");
                Console.WriteLine($"Camera.LowerLeftCorner: {camera.LowerLeftCorner.X} {camera.LowerLeftCorner.Y} {camera.LowerLeftCorner.Z}");
                for (int j = ny-1; j >= 0; j--)
                {
                    for (int i = 0; i < nx; i++)
                    {

                        var colour = new Vec3(0, 0, 0);

                        for (int s = 0; s < ns; s++)
                        {
                            double u = (i + random.NextDouble()) / nx;
                            double v = (j + random.NextDouble()) / ny;
                            var r = camera.GetRay(u, v);
                            Vec3 p = r.PointAt(2.0);
                            colour = colour + Colour(r, world, 0);
                        }
                        colour = colour / (double)ns;
                        colour = new Vec3(Math.Sqrt(colour.R), Math.Sqrt(colour.G), Math.Sqrt(colour.B));
                        int ir = (int)(255.99 * colour.R);
                        int ig = (int)(255.99 * colour.G);
                        int ib = (int)(255.99 * colour.B);
                        file.WriteLine($"{ir} {ig} {ib}");
                    }
                    //Console.WriteLine($"Row: {j}");
               }
            }
        }

         private static Vec3 Colour(Ray r, Hitable world, int depth)
        {
            HitRecord rec;
            if(world.Hit(r, 0.001, Double.MaxValue, out rec))
            {
                Ray scattered;
                Vec3 attenuation;

                if (depth < 50 && rec.Material.Scatter(r, rec, out attenuation, out scattered))
                {
                    return attenuation * Colour(scattered, world, depth+1);
                }
                else
                {
                    return new Vec3(0, 0, 0);
                }
            }
            else
            {
                Vec3 unitDirection = r.Direction.UnitVector();
                double t = 0.5 * (unitDirection.Y + 1.0);
                Vec3 c1 = new Vec3(1.0, 1.0, 1.0);
                Vec3 c2 = new Vec3(0.5, 0.7, 1.0);
                return ((1.0 - t) * c1) + (t * c2);
            }
        }
    }
}
