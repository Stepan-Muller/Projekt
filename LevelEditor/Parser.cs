using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelEditor
{
    class Parser
    {
        public static int getSize(int width, int height, string path)
        {
            string text = File.ReadAllText(path);
            string[] bits = text.Split(' ', '\n');

            return bits.Length / width / height / 3;
        }

        public static Bitmap parsePicture(int width, int height, string path, int index)
        {
            Bitmap bitmap = new Bitmap(width, height);

            int starterIndex = index * width * height * 3;

            string text = File.ReadAllText(path);
            string[] bits = text.Split(' ', '\n');

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    int red = int.Parse(bits[starterIndex + x * 3 + y * 3 * width]);
                    int green = int.Parse(bits[starterIndex + x * 3 + y * 3 * width + 1]);
                    int blue = int.Parse(bits[starterIndex + x * 3 + y * 3 * width + 2]);

                    bitmap.SetPixel(x, y, Color.FromArgb(red, green, blue));
                }        

            return bitmap;
        }
    }
}
