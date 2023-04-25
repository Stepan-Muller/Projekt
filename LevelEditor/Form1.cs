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

        Panel[] panels = new Panel[3];

        List<List<int>>[] mapsValues = new List<List<int>>[3];

        List<List<PictureBox>>[] mapsCells = new List<List<PictureBox>>[3];

        List<Bitmap> bitmaps;

        List<PictureBox> textureBoxes;
        int selectedTexture = 0;

        public Form1()
        {
            InitializeComponent();

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

            loadTextures();
        }

        private void loadTextures()
        {
            textureBoxes = new List<PictureBox>() { pictureBox4 };

            int count = Parser.parseInt("../textures/textureMap.txt", "count");
            int width = Parser.parseInt("../textures/textureMap.txt", "width");
            int height = Parser.parseInt("../textures/textureMap.txt", "height");

            bitmaps = new List<Bitmap>();

            for (int i = 0; i < count; i++)
            {
                Bitmap bitmap = Parser.parseBitmap(width, height, "../textures/textureMap.txt", "textureMap", i);

                Bitmap resizedBitmap = new Bitmap(IMAGE_SIZE, IMAGE_SIZE);
                Graphics graphics = Graphics.FromImage(resizedBitmap);
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                graphics.DrawImage(bitmap, 0, 0, IMAGE_SIZE, IMAGE_SIZE);

                bitmaps.Add(resizedBitmap);

                PictureBox pictureBox = new PictureBox();
                flowLayoutPanel1.Controls.Add(pictureBox);
                pictureBox.Image = resizedBitmap;
                pictureBox.Size = new Size(IMAGE_SIZE, IMAGE_SIZE);
                pictureBox.Click += new EventHandler(Texture_Click);

                textureBoxes.Add(pictureBox);
            }
        }

        string fileName = "";
        string FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;
                drawFileName();
            }
        }

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

        private void drawFileName()
        {
            string text = fileName;

            if (text == "") text = "Nový Soubor";

            if (!isSaved) text += "*";

            fileNameLabel.Text = text;
        }

        private void addX(int num)
        {
            for (int n = 0; n < num; n++)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < mapsCells[i].Count; j++)
                    {
                        PictureBox pictureBox = new PictureBox();
                        panels[i].Controls.Add(pictureBox);
                        pictureBox.Size = new Size(IMAGE_SIZE, IMAGE_SIZE);
                        pictureBox.Location = new Point(MAP_FIRST_IMAGE_X + (IMAGE_SIZE + IMAGE_GAP) * (mapsCells[i][j].Count), MAP_FIRST_IMAGE_Y + (IMAGE_SIZE + IMAGE_GAP) * j);
                        pictureBox.BorderStyle = BorderStyle.FixedSingle;
                        pictureBox.Tag = new CellPosition(i, mapsCells[i][j].Count, j);
                        pictureBox.MouseMove += new MouseEventHandler(Cell_MouseMove);
                        pictureBox.Click += new EventHandler(Cell_Click);

                        mapsCells[i][j].Add(pictureBox);

                        mapsValues[i][j].Add(0);
                    }
                }
            }

            buttonAddX.Location = new Point(buttonAddX.Location.X + (IMAGE_SIZE + IMAGE_GAP) * num, buttonAddX.Location.Y);

            buttonRemoveX.Location = new Point(buttonRemoveX.Location.X + (IMAGE_SIZE + IMAGE_GAP) * num, buttonRemoveX.Location.Y);
            buttonRemoveX.Visible = true;
        }

        private void addY(int num)
        {
            for (int n = 0; n < num; n++)
            {
                for (int i = 0; i < 3; i++)
                {
                    mapsCells[i].Add(new List<PictureBox>());

                    mapsValues[i].Add(new List<int>());

                    for (int j = 0; j < mapsCells[i][0].Count; j++)
                    {
                        PictureBox pictureBox = new PictureBox();
                        panels[i].Controls.Add(pictureBox);
                        pictureBox.Size = new System.Drawing.Size(IMAGE_SIZE, IMAGE_SIZE);
                        pictureBox.Location = new Point(MAP_FIRST_IMAGE_X + (IMAGE_SIZE + IMAGE_GAP) * j, MAP_FIRST_IMAGE_Y + (IMAGE_SIZE + IMAGE_GAP) * (mapsCells[i].Count - 1));
                        pictureBox.BorderStyle = BorderStyle.FixedSingle;
                        pictureBox.Tag = new CellPosition(i, j, mapsCells[i].Count - 1);
                        pictureBox.MouseMove += new MouseEventHandler(Cell_MouseMove);
                        pictureBox.Click += new EventHandler(Cell_Click);

                        mapsCells[i][mapsCells[i].Count - 1].Add(pictureBox);

                        mapsValues[i][mapsCells[i].Count - 1].Add(0);
                    }
                }
            }

            buttonAddY.Location = new Point(buttonAddY.Location.X, buttonAddY.Location.Y + (IMAGE_SIZE + IMAGE_GAP) * num);

            buttonRemoveY.Location = new Point(buttonRemoveY.Location.X, buttonRemoveY.Location.Y + (IMAGE_SIZE + IMAGE_GAP) * num);
            buttonRemoveY.Visible = true;
        }

        private void removeX(int num)
        {
            if (mapsCells[0][0].Count == 1) return;

            for (int n = 0; n < num; n++)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < mapsCells[i].Count; j++)
                    {
                        panels[i].Controls.Remove(mapsCells[i][j][mapsCells[i][j].Count - 1]);
                        mapsCells[i][j].RemoveAt(mapsCells[i][j].Count - 1);

                        mapsValues[i][j].RemoveAt(mapsValues[i][j].Count - 1);
                    }
                }
            }

            buttonAddX.Location = new Point(buttonAddX.Location.X - (IMAGE_SIZE + IMAGE_GAP) * num, buttonAddX.Location.Y);

            if (mapsCells[0][0].Count == 1) buttonRemoveX.Visible = false;
            buttonRemoveX.Location = new Point(buttonRemoveX.Location.X - (IMAGE_SIZE + IMAGE_GAP) * num, buttonRemoveX.Location.Y);
        }

        private void removeY(int num)
        {
            if (mapsCells[0].Count == 1) return;

            for (int n = 0; n < num; n++)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < mapsCells[i][0].Count; j++)
                    {
                        panels[i].Controls.Remove(mapsCells[i][mapsCells[i].Count - 1][j]);
                    }
                    mapsCells[i].RemoveAt(mapsCells[i].Count - 1);

                    mapsValues[i].RemoveAt(mapsValues[i].Count - 1);
                }
            }

            buttonAddY.Location = new Point(buttonAddY.Location.X, buttonAddY.Location.Y - (IMAGE_SIZE + IMAGE_GAP) * num);

            if (mapsCells[0].Count == 1) buttonRemoveY.Visible = false;
            buttonRemoveY.Location = new Point(buttonRemoveY.Location.X, buttonRemoveY.Location.Y - (IMAGE_SIZE + IMAGE_GAP) * num);
        }

        private void buttonAddX_Click(object sender, EventArgs e)
        {
            addX(1);

            IsSaved = false;
        }

        private void buttonAddY_Click(object sender, EventArgs e)
        {
            addY(1);

            IsSaved = false;
        }

        private void buttonRemoveX_Click(object sender, EventArgs e)
        {
            removeX(1);

            IsSaved = false;
        }

        private void buttonRemoveY_Click(object sender, EventArgs e)
        {
            removeY(1);

            IsSaved = false;
        }

        private void Texture_Click(object sender, EventArgs e)
        {
            textureBoxes[selectedTexture].BorderStyle = BorderStyle.FixedSingle;

            PictureBox pictureBox = (PictureBox)sender;

            pictureBox.BorderStyle = BorderStyle.Fixed3D;

            selectedTexture = textureBoxes.IndexOf(pictureBox);
        }

        private void Cell_Click(object sender, EventArgs e)
        {
            Control control = (Control)sender;
            control.Capture = false;

            PictureBox pictureBox = (PictureBox)sender;

            // Renderovani
            if (selectedTexture > 0)
            {
                Bitmap bitmap = new Bitmap(IMAGE_SIZE, IMAGE_SIZE);

                // Zvetsit obrazek
                Graphics graphics = Graphics.FromImage(bitmap);
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                graphics.DrawImage(bitmaps[selectedTexture - 1], 0, 0, IMAGE_SIZE, IMAGE_SIZE);

                pictureBox.Image = bitmap;
            }
            else pictureBox.Image = new Bitmap(1, 1);

            // Upraveni pole hodnot na pozadi
            CellPosition cellPosition = (CellPosition)pictureBox.Tag;

            mapsValues[cellPosition.mapIndex][cellPosition.y][cellPosition.x] = selectedTexture;

            IsSaved = false;
        }

        private void Cell_MouseMove(object sender, MouseEventArgs e)
        {
            if (MouseButtons != MouseButtons.Left) return;

            Cell_Click(sender, e);
        }

        private void setSize(int width, int height)
        {
            if (width < mapsCells[0][0].Count)
                removeX(mapsCells[0][0].Count - width);
            if (height < mapsCells[0].Count)
                removeY(mapsCells[0].Count - height);

            for (int i = 0; i < 3; i++)
            {
                for (int x = 0; x < mapsCells[0].Count; x++)
                {
                    for (int y = 0; y < mapsCells[0][0].Count; y++)
                    {
                        mapsCells[i][x][y].Image = new Bitmap(1, 1);
                        mapsValues[i][x][y] = 0;
                    }
                }
            }

            if (width > mapsCells[0][0].Count)
                addX(width - mapsCells[0][0].Count);
            if (height > mapsCells[0].Count)
                addY(height - mapsCells[0].Count);
        }

        private void loadMap(int[] map, int mapIndex, int width, int height)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (map[y * width + x] > 0)
                    {
                        Bitmap bitmap = new Bitmap(IMAGE_SIZE, IMAGE_SIZE);

                        // Zvetsit obrazek
                        Graphics graphics = Graphics.FromImage(bitmap);
                        graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                        graphics.DrawImage(bitmaps[map[y * width + x] - 1], 0, 0, IMAGE_SIZE, IMAGE_SIZE);

                        mapsCells[mapIndex][x][y].Image = bitmap;
                    }
                    else mapsCells[mapIndex][x][y].Image = new Bitmap(1, 1);

                    mapsValues[mapIndex][x][y] = map[y * width + x];
                }
            }
        }

        private void getFile(bool exists = true)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Vyberte soubor";
            openFileDialog.InitialDirectory = Application.StartupPath + @"\..";
            openFileDialog.Filter = "Text File (*.txt)|*.txt";
            if (!exists) openFileDialog.CheckFileExists = false;
            openFileDialog.ShowDialog();

            FileName = openFileDialog.FileName;
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            while (!File.Exists(FileName)) getFile();
            
            fillDock.Visible = false;
            
            int width = Parser.parseInt(FileName, "width");
            int height = Parser.parseInt(FileName, "height");

            setSize(width, height);

            loadMap(Parser.parseIntArray(FileName, "mapWalls", width * height), 0, width, height);
            loadMap(Parser.parseIntArray(FileName, "mapFloors", width * height), 1, width, height);
            loadMap(Parser.parseIntArray(FileName, "mapCeilings", width * height), 2, width, height);

            fillDock.Visible = true;

            IsSaved = true;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            while (FileName == "") getFile(false);
            
            File.WriteAllText(FileName, "");

            Parser.writeValue(FileName, "width", mapsValues[0].Count.ToString());
            Parser.writeValue(FileName, "height", mapsValues[0][0].Count.ToString());

            File.AppendAllText(FileName, "\n");

            /* TODO: dalsi hodnoty levelu */ 

            Parser.write2DArray(FileName, "mapWalls", mapsValues[0].Select(list => list.ToArray()).ToArray());
            File.AppendAllText(FileName, "\n");
            Parser.write2DArray(FileName, "mapFloors", mapsValues[1].Select(list => list.ToArray()).ToArray());
            File.AppendAllText(FileName, "\n");
            Parser.write2DArray(FileName, "mapCeilings", mapsValues[2].Select(list => list.ToArray()).ToArray());

            IsSaved = true;
        }

        private void saveAsButton_Click(object sender, EventArgs e)
        {
            FileName = "";

            saveButton_Click(sender, e);
        }
    }
}
