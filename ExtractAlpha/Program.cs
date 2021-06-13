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

        static void GetAlphaFromDir(string dir, bool doModelDir = false)
        {

            var files = Directory.GetFiles(dir, "*.png", SearchOption.AllDirectories);

            foreach (var file in files)
            {

                if (!doModelDir && (file.Contains("\\models\\") || file.Contains("/models/")))
                {
                    continue;
                }

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

                switch (args.Length)
                {
                    case 1:
                        GetAlphaFromDir(args[0]);
                        break;
                    case 2:
                        if (bool.TryParse(args[1], out var res))
                        {
                            GetAlphaFromDir(args[0], res);
                        }
                        else
                        {
                            throw new ArgumentException("Second argument to command line was invalid! Please use \"True\" or \"False\"");
                        }
                        break;
                    case 0:
                    default:
                        GetAlphaFromDir(AppDomain.CurrentDomain.BaseDirectory);
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.ReadLine();

        }
    }
}
