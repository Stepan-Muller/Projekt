using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LevelEditor
{
    public partial class Form1 : Form
    {
        /* Pravidla pro vykreslovani tabulky a textur
         * zadavat podle tlacitek a prvniho pole v navrhu */
        const int IMAGE_SIZE = 32;
        const int IMAGE_GAP = 6;
        const int MAP_FIRST_IMAGE_X = 38;
        const int MAP_FIRST_IMAGE_Y = 38;
        const int SPAWN_POINT_BORDER_SIZE = 2;

        // Panely pro mapy
        Panel[] panels = new Panel[3];

        // Mapy jako poradove cisla textur
        List<List<int>>[] mapsValues = new List<List<int>>[3];

        // Mapy jako obrazky
        List<List<PictureBox>>[] mapsCells = new List<List<PictureBox>>[3];

        // Seznam textur z mapy textur
        List<Bitmap> bitmaps = new List<Bitmap>();

        // Obrazky pro textury z mapy textur
        List<PictureBox> textureBoxes;
        
        // Aktualne vybrana textura
        int selectedTexture = 0;

        // Pozice apwnpointu
        int spawnPointX = 0, spawnPointY = 0;
        // Meni se aktulne pozice spawnpointu nebo textury?
        bool settingSpawnPoint = false;

        public Form1()
        {
            InitializeComponent();

            /* Inicializovat promenne */

            panels[0] = panel1;
            panels[1] = panel2;
            panels[2] = panel3;

            mapsValues[0] = new List<List<int>>() { new List<int>() { 0 } };
            mapsValues[1] = new List<List<int>>() { new List<int>() { 0 } };
            mapsValues[2] = new List<List<int>>() { new List<int>() { 0 } };

            mapsCells[0] = new List<List<PictureBox>>() { new List<PictureBox>() { pictureBox1 } };
            mapsCells[1] = new List<List<PictureBox>>() { new List<PictureBox>() { pictureBox2 } };
            mapsCells[2] = new List<List<PictureBox>>() { new List<PictureBox>() { pictureBox3 } };

            pictureBox1.Tag = new CellPosition(0, 0, 0);
            pictureBox2.Tag = new CellPosition(1, 0, 0);
            pictureBox3.Tag = new CellPosition(2, 0, 0);

            textureBoxes = new List<PictureBox>() { pictureBox4 };

            // Nacist textury z mapy textur
            loadTextures();
        }

        /// <summary> 
        /// Nacte textury ze souboru "../textures/textureMap.txt" 
        /// </summary>
        private void loadTextures()
        {
            // Precte pocet a rozmery textur v mape textur
            int count = Parser.parseInt("../textures/textureMap.txt", "count");
            int width = Parser.parseInt("../textures/textureMap.txt", "width");
            int height = Parser.parseInt("../textures/textureMap.txt", "height");

            // Nacte vsechny textury z mapy
            for (int i = 0; i < count; i++)
            {
                // Precte bitmapu ze souboru
                Bitmap bitmap = Parser.parseBitmap("../textures/textureMap.txt", "textureMap", i, width, height);

                // Zvetsi bitmapu na velikost obrazku
                Bitmap resizedBitmap = new Bitmap(IMAGE_SIZE, IMAGE_SIZE);
                Graphics graphics = Graphics.FromImage(resizedBitmap);
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                graphics.DrawImage(bitmap, 0, 0, IMAGE_SIZE, IMAGE_SIZE);

                // Prida bitmapu do seznamu
                bitmaps.Add(resizedBitmap);

                // Prida obrazek na paletu textur
                PictureBox pictureBox = new PictureBox();
                flowLayoutPanel1.Controls.Add(pictureBox);
                pictureBox.Image = resizedBitmap;
                pictureBox.Size = new Size(IMAGE_SIZE, IMAGE_SIZE);
                pictureBox.Click += new EventHandler(Texture_Click);

                // Prida obrazek do seznamu
                textureBoxes.Add(pictureBox);
            }
        }

        string fileName = "";
        /// <value> 
        /// Nazev aktualniho souboru
        /// Pri zmene se vypise na obrazovku
        /// </value>
        string FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;
                drawFileName();
            }
        }

        /// <value> 
        /// Urcuje zda je prace na souboru ulozena
        /// Pri zmene se zobrazi jako hvezdicka u nazvu aktualniho souboru
        /// </value>
        bool isSaved = true;
        bool IsSaved
        {
            get { return isSaved; }
            set
            {
                isSaved = value;
                drawFileName();
            }
        }

        /// <summary>
        /// Napise jmeno souboru a pripadnou hvezdicku pri neulozeni souboru na vrsek obrazovky
        /// </summary>
        private void drawFileName()
        {
            string text = fileName;

            // Pokud neni vybran zadny soubor napsat "Nový Soubor"
            if (text == "") text = "Nový Soubor";

            // Pokud neni prace na souboru ulozena pridat nakonec hvezdicku
            if (!isSaved) text += "*";

            // Napsat vysledny text na obrazovku
            fileNameLabel.Text = text;
        }

        /// <summary>
        /// Prida zadany pocet sloupcu bunek do mapy napravo
        /// </summary>
        /// <param name="num">Pocet sloupcu k pridani</param>
        private void addX(int num)
        {
            // Pro pocet sloupcu k pridani
            for (int n = 0; n < num; n++)
            {
                // Pro kazdou mapu
                for (int i = 0; i < 3; i++)
                {
                    // Pro kazdou bunku v sloupci
                    for (int j = 0; j < mapsCells[i].Count; j++)
                    {
                        // Vytvorit obrazek se vsemi vlastnostmi
                        PictureBox pictureBox = new PictureBox();
                        panels[i].Controls.Add(pictureBox);
                        pictureBox.Size = new Size(IMAGE_SIZE, IMAGE_SIZE);
                        pictureBox.Location = new Point(MAP_FIRST_IMAGE_X + (IMAGE_SIZE + IMAGE_GAP) * (mapsCells[i][j].Count), MAP_FIRST_IMAGE_Y + (IMAGE_SIZE + IMAGE_GAP) * j);
                        pictureBox.BorderStyle = BorderStyle.FixedSingle;
                        pictureBox.Tag = new CellPosition(i, mapsCells[i][j].Count, j);
                        pictureBox.MouseMove += new MouseEventHandler(Cell_MouseMove);
                        pictureBox.MouseClick += new MouseEventHandler(Cell_MouseClick);
                        pictureBox.BringToFront();

                        // Pridat obrazek na mapu
                        mapsCells[i][j].Add(pictureBox);

                        // Pridat 0 na mapu hodnot
                        mapsValues[i][j].Add(0);
                    }
                }
            }

            // Posunout tlacitko na pridavani sloupcu
            buttonAddX.Location = new Point(buttonAddX.Location.X + (IMAGE_SIZE + IMAGE_GAP) * num, buttonAddX.Location.Y);

            // Posunout tlacitko na odebirani sloupcu
            buttonRemoveX.Location = new Point(buttonRemoveX.Location.X + (IMAGE_SIZE + IMAGE_GAP) * num, buttonRemoveX.Location.Y);
            // Pokud bylo tlacitko na odebirani sloupcu skryto, zobrazit
            buttonRemoveX.Visible = true;
        }

        /// <summary>
        /// Prida zadany pocet radku bunek do mapy dolu
        /// </summary>
        /// <param name="num">Pocet radku k pridani</param>
        private void addY(int num)
        {
            // Pro pocet radku k pridani
            for (int n = 0; n < num; n++)
            {
                // Pro kazdou mapu
                for (int i = 0; i < 3; i++)
                {
                    // Pridat prazdny radek
                    mapsCells[i].Add(new List<PictureBox>());

                    mapsValues[i].Add(new List<int>());

                    // Pro kazdou bunku v radku
                    for (int j = 0; j < mapsCells[i][0].Count; j++)
                    {
                        // Vytvorit obrazek se vsemi vlastnostmi
                        PictureBox pictureBox = new PictureBox();
                        panels[i].Controls.Add(pictureBox);
                        pictureBox.Size = new System.Drawing.Size(IMAGE_SIZE, IMAGE_SIZE);
                        pictureBox.Location = new Point(MAP_FIRST_IMAGE_X + (IMAGE_SIZE + IMAGE_GAP) * j, MAP_FIRST_IMAGE_Y + (IMAGE_SIZE + IMAGE_GAP) * (mapsCells[i].Count - 1));
                        pictureBox.BorderStyle = BorderStyle.FixedSingle;
                        pictureBox.Tag = new CellPosition(i, j, mapsCells[i].Count - 1);
                        pictureBox.MouseMove += new MouseEventHandler(Cell_MouseMove);
                        pictureBox.MouseClick += new MouseEventHandler(Cell_MouseClick);
                        pictureBox.BringToFront();

                        // Pridat obrazek na mapu
                        mapsCells[i][mapsCells[i].Count - 1].Add(pictureBox);

                        // Pridat 0 na mapu hodnot
                        mapsValues[i][mapsCells[i].Count - 1].Add(0);
                    }
                }
            }

            // Posunout tlacitko na pridavani radku
            buttonAddY.Location = new Point(buttonAddY.Location.X, buttonAddY.Location.Y + (IMAGE_SIZE + IMAGE_GAP) * num);

            // Posunout tlacitko na odebirani radku
            buttonRemoveY.Location = new Point(buttonRemoveY.Location.X, buttonRemoveY.Location.Y + (IMAGE_SIZE + IMAGE_GAP) * num);
            // Pokud bylo tlacitko na odebirani radku skryto, zobrazit
            buttonRemoveY.Visible = true;
        }

        /// <summary>
        /// Odebere zadany pocet sloupcu bunek do mapy zprava
        /// </summary>
        /// <param name="num">Pocet sloupcu k odebrani</param>
        private void removeX(int num)
        {
            // Pokud zbyva mene sloupcu nez se ma odebirat
            if (mapsCells[0][0].Count <= num) return;

            // Pro pocet sloupcu k odebrani
            for (int n = 0; n < num; n++)
            {
                // Pro kazdou mapu
                for (int i = 0; i < 3; i++)
                {
                    // Pro kazdou bunku v sloupci
                    for (int j = 0; j < mapsCells[i].Count; j++)
                    {
                        // Znicit obrazek
                        panels[i].Controls.Remove(mapsCells[i][j][mapsCells[i][j].Count - 1]);
                        // Odebrat obrazek ze seznamu
                        mapsCells[i][j].RemoveAt(mapsCells[i][j].Count - 1);

                        // Odebrat hodnotu ze seznamu
                        mapsValues[i][j].RemoveAt(mapsValues[i][j].Count - 1);
                    }
                }
            }

            // Posunout tlacitko na pridavani sloupcu
            buttonAddX.Location = new Point(buttonAddX.Location.X - (IMAGE_SIZE + IMAGE_GAP) * num, buttonAddX.Location.Y);

            // Pokud uz nejde odebirat sloupce skryt tlacitko pro odebirani sloupcu
            if (mapsCells[0][0].Count == 1) buttonRemoveX.Visible = false;
            // Posunout tlacitko na odebirani sloupcu
            buttonRemoveX.Location = new Point(buttonRemoveX.Location.X - (IMAGE_SIZE + IMAGE_GAP) * num, buttonRemoveX.Location.Y);
        }

        /// <summary>
        /// Odebere zadany pocet radku bunek do mapy zdola
        /// </summary>
        /// <param name="num">Pocet radku k odebrani</param>
        private void removeY(int num)
        {
            // Pokud zbyva mene radku nez se ma odebirat
            if (mapsCells[0].Count <= num) return;

            // Pro pocet radku k odebrani
            for (int n = 0; n < num; n++)
            {
                // Pro kazdou mapu
                for (int i = 0; i < 3; i++)
                {
                    // Pro kazdou bunku v radku
                    for (int j = 0; j < mapsCells[i][0].Count; j++)
                    {
                        // Znicit obrazek
                        panels[i].Controls.Remove(mapsCells[i][mapsCells[i].Count - 1][j]);
                    }
                    // Odebrat radku ze seznamu obrazku
                    mapsCells[i].RemoveAt(mapsCells[i].Count - 1);

                    // Odebrat radku ze seznamu hodnot
                    mapsValues[i].RemoveAt(mapsValues[i].Count - 1);
                }
            }

            // Posunout tlacitko na pridavani radku
            buttonAddY.Location = new Point(buttonAddY.Location.X, buttonAddY.Location.Y - (IMAGE_SIZE + IMAGE_GAP) * num);

            // Pokud uz nejde odebirat radky skryt tlacitko pro odebirani radku
            if (mapsCells[0].Count == 1) buttonRemoveY.Visible = false;
            // Posunout tlacitko na odebirani radku
            buttonRemoveY.Location = new Point(buttonRemoveY.Location.X, buttonRemoveY.Location.Y - (IMAGE_SIZE + IMAGE_GAP) * num);
        }

        /// <summary>
        /// Prida jeden sloupec bunek doprava
        /// Zobrazi hvezdicku u jmena aktualniho souboru
        /// </summary>
        private void buttonAddX_Click(object sender, EventArgs e)
        {
            // Pridat jeden sloupec
            addX(1);

            // Zobrazit hvezdicku u jmena aktualniho souboru
            IsSaved = false;
        }

        /// <summary>
        /// Prida jeden radek bunek dolu
        /// Zobrazi hvezdicku u jmena aktualniho souboru
        /// </summary>
        private void buttonAddY_Click(object sender, EventArgs e)
        {
            // Pridat jeden radek
            addY(1);

            // Zobrazit hvezdicku u jmena aktualniho souboru
            IsSaved = false;
        }

        /// <summary>
        /// Odebere jeden sloupec bunek zprava
        /// Zobrazi hvezdicku u jmena aktualniho souboru
        /// </summary>
        private void buttonRemoveX_Click(object sender, EventArgs e)
        {
            // Odebrat jeden sloupec
            removeX(1);

            // Zobrazit hvezdicku u jmena aktualniho souboru
            IsSaved = false;
        }

        /// <summary>
        /// Odebere jeden radek bunek zdola
        /// Zobrazi hvezdicku u jmena aktualniho souboru
        /// </summary>
        private void buttonRemoveY_Click(object sender, EventArgs e)
        {
            // Odebrat jeden radek
            removeY(1);

            // Zobrazit hvezdicku u jmena aktualniho souboru
            IsSaved = false;
        }

        /// <summary>
        /// Urci ze chce uzivatel zmenit spawnpoint mapy, posune zvyrazneni na prislusne tlacitko
        /// </summary>
        private void spawnPointButton_Click(object sender, EventArgs e)
        {
            // Zvyrazni tlacitko
            spawnPointButton.BorderStyle = BorderStyle.Fixed3D;

            // Odstrani posledni zvyrazneni tlacitka
            textureBoxes[selectedTexture].BorderStyle = BorderStyle.FixedSingle;

            settingSpawnPoint = true;
        }

        /// <summary>
        /// Urci jakou chce uzivatel kreslit texturu, posune zvyrazneni na prislusne tlacitko
        /// </summary>
        private void Texture_Click(object sender, EventArgs e)
        {
            // Odstrani posledni zvyrazneni tlacitka, pokud to byla textura
            textureBoxes[selectedTexture].BorderStyle = BorderStyle.FixedSingle;

            // Najde jaky obrazek byl zakliknut
            PictureBox pictureBox = (PictureBox)sender;

            // Odstrani posledni zvyrazneni tlacitka, pokud se menil spawnpoint
            spawnPointButton.BorderStyle = BorderStyle.FixedSingle;
            // Zvyrazni tlacitko
            pictureBox.BorderStyle = BorderStyle.Fixed3D;

            // Najde poradove cislo zakliknute textury
            selectedTexture = textureBoxes.IndexOf(pictureBox);
        }

        /// <summary>
        /// Zmeni spawnpoint mapy na urcenou pozici
        /// </summary>
        /// <param name="x">Nova pozice x spawnpointu</param>
        /// <param name="y">Nova pozice y spawnpointu</param>
        private void changeSpawnPoint(int x, int y)
        {
            spawnPointX = x;
            spawnPointY = y;
            // Vypocita lokaci ramecku vyznacujiciho spawnpoint
            spawnPoint.Location = new Point(MAP_FIRST_IMAGE_X + x * (IMAGE_SIZE + IMAGE_GAP) - SPAWN_POINT_BORDER_SIZE, MAP_FIRST_IMAGE_Y + y * (IMAGE_SIZE + IMAGE_GAP) - SPAWN_POINT_BORDER_SIZE);
        }

        /// <summary>
        /// Zmeni texturu zakliknute bunky nebo spawnpoint mapy
        /// Zobrazi hvezdicku u jmena aktualniho souboru
        /// </summary> 
        private void Cell_MouseClick(object sender, MouseEventArgs e)
        {
            // Najde jaka bunka byla zakliknuta
            PictureBox pictureBox = (PictureBox)sender;

            // Najde pozici zakliknute bunky
            CellPosition cellPosition = (CellPosition)pictureBox.Tag;

            // Pokud se ma menit textura
            if (!settingSpawnPoint)
            {
                Control control = (Control)sender;
                control.Capture = false;

                int texture = 0;
                if (e.Button == MouseButtons.Left) texture = selectedTexture;

                // Pokud se ma textura nakreslit
                if (texture > 0)
                {
                    // Vytvorit novou bitmapu o spravne velikosti
                    Bitmap bitmap = new Bitmap(IMAGE_SIZE, IMAGE_SIZE);

                    // Zvetsit obrazek
                    Graphics graphics = Graphics.FromImage(bitmap);
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                    // Od cisla textury se musi odecist 1, protoze textura cislo 0 je prazdna
                    graphics.DrawImage(bitmaps[texture - 1], 0, 0, IMAGE_SIZE, IMAGE_SIZE);

                    // Vymenit starou bitmapu za novou
                    pictureBox.Image = bitmap;
                }
                // Pokud se ma textura vymazat
                else pictureBox.Image = new Bitmap(1, 1);

                // Upraveni pole hodnot na pozadi
                mapsValues[cellPosition.mapIndex][cellPosition.y][cellPosition.x] = texture;
            }
            // Pokud se ma menit spawnpoint
            else changeSpawnPoint(cellPosition.x, cellPosition.y);

            // Zobrazit hvezdicku u jmena aktualniho souboru
            IsSaved = false;
        }

        /// <summary>
        /// Pokud kurzor mysi prejizdi pres bunku a je zmacknute tlacitko mysi, dela to same jako kliknuti na bunku
        /// </summary>
        private void Cell_MouseMove(object sender, MouseEventArgs e)
        {
            // Pokud je zmacknute jedno ze dvou tlacitek mysi
            if (MouseButtons == MouseButtons.Left || MouseButtons == MouseButtons.Right) Cell_MouseClick(sender, e);
        }

        /// <summary>
        /// Nastavi velikost mapy a mapu vycisti
        /// <summary>
        /// <param name="width">Nova sirka mapy</param>
        /// <param name="height">Nova vyska mapy</param>
        private void setSize(int width, int height)
        {
            // Pokud ma byt nova velikost vetsi nez stara => odstrani prebytecne bunky
            if (width < mapsCells[0][0].Count)
                removeX(mapsCells[0][0].Count - width);
            if (height < mapsCells[0].Count)
                removeY(mapsCells[0].Count - height);

            // Pro kazdou mapu
            for (int i = 0; i < 3; i++)
            {
                // Vycistit kazdou bunku v mape
                for (int x = 0; x < mapsCells[0].Count; x++)
                {
                    for (int y = 0; y < mapsCells[0][0].Count; y++)
                    {
                        mapsCells[i][x][y].Image = new Bitmap(1, 1);
                        mapsValues[i][x][y] = 0;
                    }
                }
            }

            // Pokud ma byt nova velikost vetsi nez stara => pridat potrebne bunky
            if (width > mapsCells[0][0].Count)
                addX(width - mapsCells[0][0].Count);
            if (height > mapsCells[0].Count)
                addY(height - mapsCells[0].Count);
        }

        /// <summary>
        /// Nacte mapu z promenne
        /// </summary>
        /// <param name="map">Promenna s mapu k nacteni</param>
        /// <param name="mapIndex">Poradove cislo mapy v promenne</param>
        /// <param name="width">Sirka mapy</param>
        /// <param name="height">Vyska mapy</param>
        private void loadMap(int[] map, int mapIndex, int width, int height)
        {
            // Pro kazdou bunku v promenne
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    // Pokud neni prazdna => nacist texturu
                    if (map[y * width + x] > 0)
                    {
                        // Vytvorit novou bitmapu o spravne velikosti
                        Bitmap bitmap = new Bitmap(IMAGE_SIZE, IMAGE_SIZE);

                        // Zvetsit obrazek
                        Graphics graphics = Graphics.FromImage(bitmap);
                        graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                        graphics.DrawImage(bitmaps[map[y * width + x] - 1], 0, 0, IMAGE_SIZE, IMAGE_SIZE);

                        // Ulozit bitmapu
                        mapsCells[mapIndex][x][y].Image = bitmap;
                    }
                    // Pokud je prazda => nacist prazdnou texturu
                    else mapsCells[mapIndex][x][y].Image = new Bitmap(1, 1);

                    // Ulozit hodnotu bunky
                    mapsValues[mapIndex][x][y] = map[y * width + x];
                }
            }
        }

        /// <summary>
        /// Otevre okno pro vyber souboru
        /// </summary>
        /// <param name="exists">Musi vybrany soubor jiz existovat?</param>
        private void getFile(bool exists = true)
        {
            // Otevrit okno
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Vyberte soubor";
            openFileDialog.InitialDirectory = Application.StartupPath + @"\..";
            openFileDialog.Filter = "Text File (*.txt)|*.txt";
            // Pokud ma bude kontrolovat ze soubor existuje
            if (!exists) openFileDialog.CheckFileExists = false;
            openFileDialog.ShowDialog();

            // Ulozit vybranou cestu
            FileName = openFileDialog.FileName;
        }

        /// <summary>
        /// Nacte soubor
        /// Odstrani hvezdicku u jmena aktualniho souboru
        /// </summary>
        private void loadButton_Click(object sender, EventArgs e)
        {
            // Pokud neni vybran existujici soubor => otevrit okno pro vyber souboru
            do getFile(); while (!File.Exists(FileName));
            
            // Precist velikost mapy ze souboru
            int width = Parser.parseInt(FileName, "width");
            int height = Parser.parseInt(FileName, "height");
            setSize(width, height);

            // Precist spawnpoint ze souboru
            changeSpawnPoint(Parser.parseInt(fileName, "spawnX"), Parser.parseInt(fileName, "spawnY"));

            // Precist dalsi level ze souboru
            nextLevel.Text = Parser.parseString(FileName, "nextLevel");

            // Nacist vsechny mapy
            loadMap(Parser.parseIntArray(FileName, "mapWalls", width * height), 0, width, height);
            loadMap(Parser.parseIntArray(FileName, "mapFloors", width * height), 1, width, height);
            loadMap(Parser.parseIntArray(FileName, "mapCeilings", width * height), 2, width, height);

            // Skryje hvezdicku u jmena aktualniho souboru
            IsSaved = true;
        }

        /// <summary>
        /// Ulozi mapu do vybraneho souboru, pokud neni vybran soubor zobrazi okno pro vyber souboru
        /// Odstrani hvezdicku u jmena aktualniho souboru
        /// </summary>
        private void saveButton_Click(object sender, EventArgs e)
        {
            // Pokud neni vybran zadny soubor => otevrit okno pro vyber souboru
            while (FileName == "") getFile(false);

            // Zapsat velikost mapy do souboru
            Parser.writeValue(FileName, "width", mapsValues[0].Count.ToString());
            Parser.writeValue(FileName, "height", mapsValues[0][0].Count.ToString());

            // Pridat prazdny radek pro citelnost
            File.AppendAllText(FileName, "\n");

            // Zapsat spawnpoint do souboru
            Parser.writeValue(FileName, "spawnX", spawnPointX.ToString());
            Parser.writeValue(FileName, "spawnY", spawnPointY.ToString());

            // Pridat prazdny radek pro citelnost
            File.AppendAllText(FileName, "\n");

            // Zapsat dalsi level do souboru
            Parser.writeValue(FileName, "nextLevel", nextLevel.Text);

            // Pridat prazdny radek pro citelnost
            File.AppendAllText(FileName, "\n");

            // Zapsat mapy do souboru
            Parser.write2DArray(FileName, "mapWalls", mapsValues[0].Select(list => list.ToArray()).ToArray());
            File.AppendAllText(FileName, "\n");
            Parser.write2DArray(FileName, "mapFloors", mapsValues[1].Select(list => list.ToArray()).ToArray());
            File.AppendAllText(FileName, "\n");
            Parser.write2DArray(FileName, "mapCeilings", mapsValues[2].Select(list => list.ToArray()).ToArray());

            // Skryje hvezdicku u jmena aktualniho souboru
            IsSaved = true;
        }
        
        /// <summary>
        /// Zobrazi okno pro vyber souboru a do vybraneho souboru ulozi mapu
        /// </summary>
        private void saveAsButton_Click(object sender, EventArgs e)
        {
            // Vymazat aktualni soubor pro ukladani
            FileName = "";

            // Spustit funkci jako kdyby bylo zakliknuto tlacitko pro ukladani
            saveButton_Click(sender, e);
        }

        /// <summary>
        /// Zobrazi hvezdicku u jmena aktualniho souboru
        /// </summary>
        private void nextLevel_TextChanged(object sender, EventArgs e)
        {
            IsSaved = false;
        }

        /// <summary>
        /// Zepta se zda chce uzivatel ulozit neulozenou praci
        /// </summary>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Pokud je prace ulozena => nechat zavrit okno
            if (isSaved) return;

            // Pokud pracew neni ulozena => zeptat se na ulozeni
            DialogResult result = MessageBox.Show("Chcete uložit provedené změny?", "Potvrzeni uzavření", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            
            // Pokud se ma prace ulozit
            if (result == DialogResult.Yes) saveButton_Click(sender, e);
            // Pokud se ma zavreni okna zrusit
            else if (result == DialogResult.Cancel) e.Cancel = true;
            // Pokud se prace nema ulozit => neudelat nic a zavrit
        }
    }
}
