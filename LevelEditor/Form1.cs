using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        const int IMAGE_SIZE = 64;
        const int IMAGE_GAP = 6;
        const int MAP_FIRST_IMAGE_X = 38;
        const int MAP_FIRST_IMAGE_Y = 38;

        Panel[] panels = new Panel[3];

        List<List<int>>[] mapsValues = new List<List<int>>[3];

        List<List<PictureBox>>[] mapsCells = new List<List<PictureBox>>[3];

        List<Bitmap> bitmaps = new List<Bitmap>();

        List<PictureBox> textureBoxes;
        int selectedTexture = 0;

        public Form1()
        {
            InitializeComponent();

            panels[0] = panel1;
            panels[1] = panel2;
            panels[2] = panel3;

            mapsCells[0] = new List<List<PictureBox>>() { new List<PictureBox>() { pictureBox1 } };
            mapsCells[1] = new List<List<PictureBox>>() { new List<PictureBox>() { pictureBox2 } };
            mapsCells[2] = new List<List<PictureBox>>() { new List<PictureBox>() { pictureBox3 } };

            mapsValues[0] = new List<List<int>>() { new List<int>() { 0 } };
            mapsValues[1] = new List<List<int>>() { new List<int>() { 0 } };
            mapsValues[2] = new List<List<int>>() { new List<int>() { 0 } };

            loadTextures();
        }

        private void loadTextures()
        {
            textureBoxes = new List<PictureBox>() { pictureBox4 };

            for (int i = 0; i < Parser.getSize(16, 16, "../textures/textureMap.txt"); i++)
            {
                Bitmap bitmap = Parser.parsePicture(16, 16, "../textures/textureMap.txt", i);

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

        private void browseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Vyberte soubor";
            openFileDialog.InitialDirectory = Application.StartupPath + @"\..";
            openFileDialog.Filter = "Text File (*.txt)|*.txt";
            openFileDialog.ShowDialog();

            if (openFileDialog.FileName != "")
                fileTextBox.Text = openFileDialog.FileName;
            else
                fileTextBox.Text = "";
        }

        private void buttonAddX_Click(object sender, EventArgs e)
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
                    pictureBox.Click += new EventHandler(Cell_Click);

                    mapsCells[i][j].Add(pictureBox);

                    mapsValues[i][j].Add(0);
                }
            }

            buttonAddX.Location = new Point(buttonAddX.Location.X + (IMAGE_SIZE + IMAGE_GAP), buttonAddX.Location.Y);

            buttonRemoveX.Location = new Point(buttonRemoveX.Location.X + (IMAGE_SIZE + IMAGE_GAP), buttonRemoveX.Location.Y);
            buttonRemoveX.Visible = true;
        }

        private void buttonAddY_Click(object sender, EventArgs e)
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
                    pictureBox.Click += new EventHandler(Cell_Click);

                    mapsCells[i][mapsCells[i].Count - 1].Add(pictureBox);

                    mapsValues[i][mapsCells[i].Count - 1].Add(0);
                }
            }

            buttonAddY.Location = new Point(buttonAddY.Location.X, buttonAddY.Location.Y + (IMAGE_SIZE + IMAGE_GAP));

            buttonRemoveY.Location = new Point(buttonRemoveY.Location.X, buttonRemoveY.Location.Y + (IMAGE_SIZE + IMAGE_GAP));
            buttonRemoveY.Visible = true;
        }

        private void buttonRemoveX_Click(object sender, EventArgs e)
        {
            if (mapsCells[0][0].Count == 1) return;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < mapsCells[i].Count; j++)
                {
                    panels[i].Controls.Remove(mapsCells[i][j][mapsCells[i][j].Count - 1]);
                    mapsCells[i][j].RemoveAt(mapsCells[i][j].Count - 1);

                    mapsValues[i][j].RemoveAt(mapsValues[i][j].Count - 1);
                }
            }
            buttonAddX.Location = new Point(buttonAddX.Location.X - (IMAGE_SIZE + IMAGE_GAP), buttonAddX.Location.Y);

            if (mapsCells[0][0].Count == 1) buttonRemoveX.Visible = false;
            buttonRemoveX.Location = new Point(buttonRemoveX.Location.X - (IMAGE_SIZE + IMAGE_GAP), buttonRemoveX.Location.Y);
        }

        private void buttonRemoveY_Click(object sender, EventArgs e)
        {
            if (mapsCells[0].Count == 1) return;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < mapsCells[i][0].Count; j++)
                {
                    panels[i].Controls.Remove(mapsCells[i][mapsCells[i].Count - 1][j]);
                }
                mapsCells[i].RemoveAt(mapsCells[i].Count - 1);

                mapsValues[i].RemoveAt(mapsValues[i].Count - 1);
            }

            buttonAddY.Location = new Point(buttonAddY.Location.X, buttonAddY.Location.Y - (IMAGE_SIZE + IMAGE_GAP));

            if (mapsCells[0].Count == 1) buttonRemoveY.Visible = false;
            buttonRemoveY.Location = new Point(buttonRemoveY.Location.X, buttonRemoveY.Location.Y - (IMAGE_SIZE + IMAGE_GAP));
        }

        private void fileTextBox_TextChanged(object sender, EventArgs e)
        {
            // TODO: nacist mapu ze souboru
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
            PictureBox pictureBox = (PictureBox)sender;

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
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
