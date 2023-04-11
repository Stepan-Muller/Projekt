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
        /* Pravidla pro vykreslovani tabulky
         * zadavat podle tlacitek a prvniho pole v navrhu */
        const int IMAGE_SIZE = 64;
        const int IMAGE_GAP = 6;
        const int FIRST_IMAGE_X = 38;
        const int FIRST_IMAGE_Y = 38;

        Panel[] panels = new Panel[3];

        List<List<PictureBox>>[] maps = new List<List<PictureBox>>[3];

        List<Bitmap> bitmaps;

        List<PictureBox> textureBoxes;
        int selectedTexture = 0;

        public Form1()
        {
            InitializeComponent();

            panels[0] = panel1;
            panels[1] = panel2;
            panels[2] = panel3;

            maps[0] = new List<List<PictureBox>>() { new List<PictureBox>() { pictureBox1 } };
            maps[1] = new List<List<PictureBox>>() { new List<PictureBox>() { pictureBox2 } };
            maps[2] = new List<List<PictureBox>>() { new List<PictureBox>() { pictureBox3 } };

            Bitmap bitmap1 = new Bitmap(32, 32);
            bitmap1.SetPixel(10, 10, Color.Red);

            Bitmap bitmap2 = new Bitmap(32, 32);
            bitmap2.SetPixel(10, 10, Color.Green);

            Bitmap bitmap3 = new Bitmap(32, 32);
            bitmap3.SetPixel(10, 10, Color.Blue);

            bitmaps = new List<Bitmap>() { bitmap1, bitmap2, bitmap3 };

            textureBoxes = new List<PictureBox>() { pictureBox4, pictureBox5, pictureBox6 };
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
                for (int j = 0; j < maps[i].Count; j++)
                {
                    PictureBox pictureBox = new PictureBox();
                    panels[i].Controls.Add(pictureBox);
                    pictureBox.Size = new Size(IMAGE_SIZE, IMAGE_SIZE);
                    pictureBox.Location = new Point(FIRST_IMAGE_X + (IMAGE_SIZE + IMAGE_GAP) * (maps[i][j].Count), FIRST_IMAGE_Y + (IMAGE_SIZE + IMAGE_GAP) * j);
                    pictureBox.BorderStyle = BorderStyle.FixedSingle;
                    pictureBox.Click += new System.EventHandler(Cell_Click);

                    maps[i][j].Add(pictureBox);
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
                maps[i].Add(new List<PictureBox>());

                for (int j = 0; j < maps[i][0].Count; j++)
                {
                    PictureBox pictureBox = new PictureBox();
                    panels[i].Controls.Add(pictureBox);
                    pictureBox.Size = new System.Drawing.Size(IMAGE_SIZE, IMAGE_SIZE);
                    pictureBox.Location = new Point(FIRST_IMAGE_X + (IMAGE_SIZE + IMAGE_GAP) * j, FIRST_IMAGE_Y + (IMAGE_SIZE + IMAGE_GAP) * (maps[i].Count - 1));
                    pictureBox.BorderStyle = BorderStyle.FixedSingle;
                    pictureBox.Click += new System.EventHandler(Cell_Click);

                    maps[i][maps[i].Count - 1].Add(pictureBox);
                }
            }

            buttonAddY.Location = new Point(buttonAddY.Location.X, buttonAddY.Location.Y + (IMAGE_SIZE + IMAGE_GAP));

            buttonRemoveY.Location = new Point(buttonRemoveY.Location.X, buttonRemoveY.Location.Y + (IMAGE_SIZE + IMAGE_GAP));
            buttonRemoveY.Visible = true;
        }

        private void buttonRemoveX_Click(object sender, EventArgs e)
        {
            if (maps[0][0].Count == 1) return;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < maps[i].Count; j++)
                {
                    panels[i].Controls.Remove(maps[i][j][maps[i][j].Count - 1]);
                    maps[i][j].RemoveAt(maps[i][j].Count - 1);
                }
            }
            buttonAddX.Location = new Point(buttonAddX.Location.X - (IMAGE_SIZE + IMAGE_GAP), buttonAddX.Location.Y);

            if (maps[0][0].Count == 1) buttonRemoveX.Visible = false;
            buttonRemoveX.Location = new Point(buttonRemoveX.Location.X - (IMAGE_SIZE + IMAGE_GAP), buttonRemoveX.Location.Y);
        }

        private void buttonRemoveY_Click(object sender, EventArgs e)
        {
            if (maps[0].Count == 1) return;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < maps[i][0].Count; j++)
                {
                    panels[i].Controls.Remove(maps[i][maps[i].Count - 1][j]);
                }
                maps[i].RemoveAt(maps[i].Count - 1);
            }

            buttonAddY.Location = new Point(buttonAddY.Location.X, buttonAddY.Location.Y - (IMAGE_SIZE + IMAGE_GAP));

            if (maps[0].Count == 1) buttonRemoveY.Visible = false;
            buttonRemoveY.Location = new Point(buttonRemoveY.Location.X, buttonRemoveY.Location.Y - (IMAGE_SIZE + IMAGE_GAP));
        }

        private void fileTextBox_TextChanged(object sender, EventArgs e)
        {
            // TODO: nacist mapu ze souboru
        }

        private void Texture_Click(object sender, EventArgs e)
        {
            textureBoxes[selectedTexture].BorderStyle = BorderStyle.None;

            PictureBox pictureBox = (PictureBox)sender;

            pictureBox.BorderStyle = BorderStyle.Fixed3D;

            selectedTexture = textureBoxes.IndexOf(pictureBox);
        }

        private void Cell_Click(object sender, EventArgs e)
        {
            PictureBox pictureBox = (PictureBox)sender;
            pictureBox.Image = new Bitmap(bitmaps[selectedTexture], new Size(64, 64));
        }
    }
}
