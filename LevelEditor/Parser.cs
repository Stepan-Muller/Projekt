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
        
        /// <summary>
        /// Ziska poradove cislo hodnoty
        /// </summary>
        /// <param name="words">Prohledavany soubor rozdelen do jednotlivych slov</param>
        /// <param name="valueName">Jmeno vyhledavane hodnoty</param>
        static int getIndexOf(string[] words, string valueName)
        {
            // Projde vsechny slova v souboru
            for (int i = 0; true; i++)
                // Pokud nasleduje hodnota
                if (words[i] == "#")
                {
                    i++;
                    // Pokud je jmeno hodnoty spravne => vratit poradove cislo hodnoty
                    if (words[i] == valueName)
                        return i + 1;
                }
        }
        
        /// <summary>
        /// Ziska ciselnou hodnotu ze souboru
        /// </summary>
        /// <param name="path">Cesta k souboru</param>
        /// <param name="valueName">Jmeno hodnoty</param>
        public static int parseInt(string path, string valueName)
        {
            // Nacist soubor do retezce
            string text = File.ReadAllText(path);
            // Rodelit retezec na jednotliva slova
            string[] words = text.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

            // Precist hodnotu na spravne pozici
            return int.Parse(words[getIndexOf(words, valueName)]);
        }

        /// <summary>
        /// Ziska slovni hodnotu ze souboru
        /// </summary>
        /// <param name="path">Cesta k souboru</param>
        /// <param name="valueName">Jmeno hodnoty</param>
        public static string parseString(string path, string valueName)
        {
            // Nacist soubor do retezce
            string text = File.ReadAllText(path);
            // Rodelit retezec na jednotliva slova
            string[] words = text.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

            // Precist hodnotu na spravne pozici
            return words[getIndexOf(words, valueName)];
        }

        /// <summary>
        /// Ziska pole ciselnych hodnot ze souboru
        /// </summary>
        /// <param name="path">Cesta k souboru</param>
        /// <param name="valueName">Jmeno pole hodnot</param>
        /// <param name="size">Velikost pole hodnot</param>
        public static int[] parseIntArray(string path, string valueName, int size)
        {
            // Nacist soubor do retezce
            string text = File.ReadAllText(path);
            // Rodelit retezec na jednotliva slova
            string[] words = text.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

            // Zjistit pozici prvniho prvku v poli 
            int starterIndex = getIndexOf(words, valueName);
            // Vytvorit pole o spravne velikosti
            int[] array = new int[size];

            // Precist vsechny hodnoty v poli
            for (int i = 0; i < size; i++)
                array[i] = int.Parse(words[starterIndex + i]);

            return array;
        }

        /// <summary>
        /// Ziska obrazovou hodnotu ze souboru
        /// </summary>
        /// <param name="path">Cesta k souboru</param>
        /// <param name="valueName">Jmeno hodnoty</param>
        /// <param name="index">Poradove cislo bitmapy</param>
        /// <param name="width">Sirka bitmapy</param>
        /// <param name="height">Vyska bitmapy</param>
        public static Bitmap parseBitmap(string path, string valueName, int index, int width, int height, )
        {
            // Nacist soubor do retezce
            string text = File.ReadAllText(path);
            // Rodelit retezec na jednotliva slova
            string[] words = text.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

            // Zjistit pozici prvniho subpixelu v obrazku 
            int starterIndex = getIndexOf(words, valueName) + index * width * height * 3;
            // Vytvorit bitmapu o spravne velikosti
            Bitmap bitmap = new Bitmap(width, height);

            // Projit vsechny pixely v obrazku
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    // Precist RGB hodnoty pixelu
                    int red = int.Parse(words[starterIndex + x * 3 + y * 3 * width]);
                    int green = int.Parse(words[starterIndex + x * 3 + y * 3 * width + 1]);
                    int blue = int.Parse(words[starterIndex + x * 3 + y * 3 * width + 2]);

                    // Nakreslit pixel do bitmapy
                    bitmap.SetPixel(x, y, Color.FromArgb(red, green, blue));
                }

            return bitmap;
        }

        /* Metody pro psani do souboru */

        /// <summary>
        /// Zapise hodnotu do souboru
        /// </summary>
        /// <param name="path">Cesta k souboru</param>
        /// <param name="valueName">Jmeno hodnoty</param>
        /// <param name="value">Hodnota k zapsani</param>
        public static void writeValue(string path, string valueName, string value)
        {
            // Zapsat hodnotu do souboru v formátu "# (jmeno hodnoty) (hodnota)"
            File.AppendAllText(path, "# " + valueName + " " +  value + "\n");
        }

        /// <summary>
        /// Zapise dvojrozmerne pole hodnot do souboru
        /// </summary>
        /// <param name="path">Cesta k souboru</param>
        /// <param name="valueName">Jmeno pole hodnot</param>
        /// <param name="value">Pole hodnot k zapisu</param>
        public static void write2DArray(string path, string valueName, int[][] array)
        {
            // Zapsat jmeno hodnoty do souboru
            File.AppendAllText(path, "# " + valueName + "\n");
            
            // Zapsat kazdou hodnotu pole do souboru
            for (int i = 0; i < array.Length; i++) 
            {
                for (int j = 0; j < array[i].Length; j++)
                    File.AppendAllText(path, array[i][j] + " ");

                // Po kazdem radku pole zapsat novy radek
                File.AppendAllText(path, "\n");
            }
        }
    }
}
