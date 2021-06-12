using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Extensions.Transforms;
using System.Drawing;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;

namespace ExtractAlpha
{
    class Program
    {

        static void GetAlphaFromDir(string dir)
        {

            var files = Directory.GetFiles(dir, "*.png", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                
                Image<Rgba32> img = Image.Load<Rgba32>(file);

                var imgOut = new Image<Rgba32>(img.GetConfiguration(), img.Width, img.Height);

                bool alpha = false;

                for (int x = 0; x < img.Width; x++)
                {
                    for (int y = 0; y < img.Height; y++)
                    {

                        if (!alpha && img[x, y].A != byte.MaxValue)
                        {
                            alpha = true;
                        }

                        imgOut[x, y] = new Rgba32(
                            img[x, y].A,
                            img[x, y].A,
                            img[x, y].A,
                            byte.MaxValue
                        );

                    }
                }

                if (alpha)
                {
                    var path = Path.GetDirectoryName(file) + "\\" + Path.GetFileNameWithoutExtension(file) + "_alphaMask.png";
                    imgOut.SaveAsPng(path);
                    Console.WriteLine($"Saving: {path}");
                }

            }

        }

        static void Main(string[] args)
        {

            try
            {
                GetAlphaFromDir(args.Length == 0 ? AppDomain.CurrentDomain.BaseDirectory : args[0]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.ReadLine();

        }
    }
}
