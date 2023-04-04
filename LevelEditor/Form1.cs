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
        
        List<List<PictureBox>> table;

        public Form1()
        {
            InitializeComponent();

            table = new List<List<PictureBox>>() { new List<PictureBox>() { pictureBox1 } };
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
            for (int i = 0; i < table.Count; i++)
            {
                PictureBox pictureBox = new PictureBox();
                panel1.Controls.Add(pictureBox);
                pictureBox.Size = new Size(IMAGE_SIZE, IMAGE_SIZE);
                pictureBox.Location = new Point(FIRST_IMAGE_X + (IMAGE_SIZE + IMAGE_GAP) * (table[i].Count), FIRST_IMAGE_Y + (IMAGE_SIZE + IMAGE_GAP) * i);
                pictureBox.BorderStyle = BorderStyle.FixedSingle;

                table[i].Add(pictureBox);
            }

            buttonAddX.Location = new Point(buttonAddX.Location.X + (IMAGE_SIZE + IMAGE_GAP), buttonAddX.Location.Y);

            buttonRemoveX.Location = new Point(buttonRemoveX.Location.X + (IMAGE_SIZE + IMAGE_GAP), buttonRemoveX.Location.Y);
            buttonRemoveX.Visible = true;
        }

        private void buttonAddY_Click(object sender, EventArgs e)
        {
            table.Add(new List<PictureBox>());

            for (int i = 0; i < table[0].Count; i++)
            {
                PictureBox pictureBox = new PictureBox();
                panel1.Controls.Add(pictureBox);
                pictureBox.Size = new System.Drawing.Size(IMAGE_SIZE, IMAGE_SIZE);
                pictureBox.Location = new Point(FIRST_IMAGE_X + (IMAGE_SIZE + IMAGE_GAP) * i, FIRST_IMAGE_Y + (IMAGE_SIZE + IMAGE_GAP) * (table.Count - 1));
                pictureBox.BorderStyle = BorderStyle.FixedSingle;

                table[table.Count - 1].Add(pictureBox);
            }

            buttonAddY.Location = new Point(buttonAddY.Location.X, buttonAddY.Location.Y + (IMAGE_SIZE + IMAGE_GAP));

            buttonRemoveY.Location = new Point(buttonRemoveY.Location.X, buttonRemoveY.Location.Y + (IMAGE_SIZE + IMAGE_GAP));
            buttonRemoveY.Visible = true;
        }

        private void buttonRemoveX_Click(object sender, EventArgs e)
        {
            if (table[0].Count == 1) return;

            for (int i = 0; i < table.Count; i++)
            {
                panel1.Controls.Remove(table[i][table[i].Count - 1]);
                table[i].RemoveAt(table[i].Count - 1);
            }

            buttonAddX.Location = new Point(buttonAddX.Location.X - (IMAGE_SIZE + IMAGE_GAP), buttonAddX.Location.Y);

            if (table[0].Count == 1) buttonRemoveX.Visible = false;
            buttonRemoveX.Location = new Point(buttonRemoveX.Location.X - (IMAGE_SIZE + IMAGE_GAP), buttonRemoveX.Location.Y);
        }

        private void buttonRemoveY_Click(object sender, EventArgs e)
        {
            if (table.Count == 1) return;

            for (int i = 0; i < table[0].Count; i++)
            {
                panel1.Controls.Remove(table[table.Count - 1][i]);
            }
            table.RemoveAt(table.Count - 1);

            buttonAddY.Location = new Point(buttonAddY.Location.X, buttonAddY.Location.Y - (IMAGE_SIZE + IMAGE_GAP));

            if (table.Count == 1) buttonRemoveY.Visible = false;
            buttonRemoveY.Location = new Point(buttonRemoveY.Location.X, buttonRemoveY.Location.Y - (IMAGE_SIZE + IMAGE_GAP));
        }

        private void fileTextBox_TextChanged(object sender, EventArgs e)
        {
            // TODO: nacist mapu ze souboru
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            // TODO: ukladani
        }
    }
}
