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
        /* Metody pro cteni ze souboru */
        
        static int getIndexOf(string[] words, string valueName)
        {
            int i = 0;
            while (true)
            {
                if (words[i] == "#")
                {
                    i++;
                    Console.WriteLine(words[i]);
                    if (words[i] == valueName)
                        return i + 1;
                }

                i++;
            }
        }
        
        public static int parseInt(string path, string valueName)
        {
            string text = File.ReadAllText(path);
            string[] words = text.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

            return int.Parse(words[getIndexOf(words, valueName)]);
        }

        public static string parseString(string path, string valueName)
        {
            string text = File.ReadAllText(path);
            string[] words = text.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

            return words[getIndexOf(words, valueName)];
        }

        public static int[] parseIntArray(string path, string valueName, int size)
        {
            string text = File.ReadAllText(path);
            string[] words = text.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

            int starterIndex = getIndexOf(words, valueName);
            int[] array = new int[size];

            for (int j = 0; j < size; j++)
                array[j] = int.Parse(words[starterIndex + j]);

            return array;
        }

        public static Bitmap parseBitmap(int width, int height, string path, string valueName, int index)
        {
            string text = File.ReadAllText(path);
            string[] words = text.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

            int starterIndex = getIndexOf(words, valueName) + index * width * height * 3;
            Bitmap bitmap = new Bitmap(width, height);

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    int red = int.Parse(words[starterIndex + x * 3 + y * 3 * width]);
                    int green = int.Parse(words[starterIndex + x * 3 + y * 3 * width + 1]);
                    int blue = int.Parse(words[starterIndex + x * 3 + y * 3 * width + 2]);

                    bitmap.SetPixel(x, y, Color.FromArgb(red, green, blue));
                }

            return bitmap;
        }

        /* Metody pro psani do souboru */

        public static void writeValue(string path, string valueName, string value)
        {
            File.AppendAllText(path, "# " + valueName + " " +  value + "\n");
        }

        public static void write2DArray(string path, string valueName, int[][] array)
        {
            File.AppendAllText(path, "# " + valueName + "\n");
            
            for (int i = 0; i < array.Length; i++) 
            {
                for (int j = 0; j < array[i].Length; j++)
                    File.AppendAllText(path, array[i][j] + " ");

                File.AppendAllText(path, "\n");
            }
        }
    }
}
