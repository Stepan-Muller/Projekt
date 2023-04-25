using System;
using System.Windows.Forms;

namespace LevelEditor
{
    partial class Form1
    {
        /// <summary>
        /// Vyžaduje se proměnná návrháře.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Uvolněte všechny používané prostředky.
        /// </summary>
        /// <param name="disposing">hodnota true, když by se měl spravovaný prostředek odstranit; jinak false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kód generovaný Návrhářem Windows Form

        /// <summary>
        /// Metoda vyžadovaná pro podporu Návrháře - neupravovat
        /// obsah této metody v editoru kódu.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonRemoveX = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.spawnPoint = new System.Windows.Forms.Panel();
            this.buttonRemoveY = new System.Windows.Forms.Button();
            this.buttonAddX = new System.Windows.Forms.Button();
            this.buttonAddY = new System.Windows.Forms.Button();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.topDock = new System.Windows.Forms.Panel();
            this.fileNameLabel = new System.Windows.Forms.Label();
            this.saveAsButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.loadButton = new System.Windows.Forms.Button();
            this.fillDock = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.spawnPointButton = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.nextLevel = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.flowLayoutPanel2.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.topDock.SuspendLayout();
            this.fillDock.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.buttonRemoveX);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.spawnPoint);
            this.panel1.Controls.Add(this.buttonRemoveY);
            this.panel1.Controls.Add(this.buttonAddX);
            this.panel1.Controls.Add(this.buttonAddY);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(75, 75);
            this.panel1.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Walls";
            // 
            // buttonRemoveX
            // 
            this.buttonRemoveX.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonRemoveX.ForeColor = System.Drawing.Color.Red;
            this.buttonRemoveX.Location = new System.Drawing.Point(0, 0);
            this.buttonRemoveX.Name = "buttonRemoveX";
            this.buttonRemoveX.Size = new System.Drawing.Size(32, 32);
            this.buttonRemoveX.TabIndex = 5;
            this.buttonRemoveX.Text = "-";
            this.buttonRemoveX.UseVisualStyleBackColor = true;
            this.buttonRemoveX.Visible = false;
            this.buttonRemoveX.Click += new System.EventHandler(this.buttonRemoveX_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(38, 38);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Cell_MouseClick);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Cell_MouseMove);
            // 
            // spawnPoint
            // 
            this.spawnPoint.BackColor = System.Drawing.Color.Gold;
            this.spawnPoint.Location = new System.Drawing.Point(36, 36);
            this.spawnPoint.Name = "spawnPoint";
            this.spawnPoint.Size = new System.Drawing.Size(36, 36);
            this.spawnPoint.TabIndex = 11;
            // 
            // buttonRemoveY
            // 
            this.buttonRemoveY.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonRemoveY.ForeColor = System.Drawing.Color.Red;
            this.buttonRemoveY.Location = new System.Drawing.Point(0, 0);
            this.buttonRemoveY.Name = "buttonRemoveY";
            this.buttonRemoveY.Size = new System.Drawing.Size(32, 32);
            this.buttonRemoveY.TabIndex = 6;
            this.buttonRemoveY.Text = "-";
            this.buttonRemoveY.UseVisualStyleBackColor = true;
            this.buttonRemoveY.Visible = false;
            this.buttonRemoveY.Click += new System.EventHandler(this.buttonRemoveY_Click);
            // 
            // buttonAddX
            // 
            this.buttonAddX.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonAddX.ForeColor = System.Drawing.Color.Green;
            this.buttonAddX.Location = new System.Drawing.Point(38, 0);
            this.buttonAddX.Name = "buttonAddX";
            this.buttonAddX.Size = new System.Drawing.Size(32, 32);
            this.buttonAddX.TabIndex = 3;
            this.buttonAddX.Text = "+";
            this.buttonAddX.UseVisualStyleBackColor = true;
            this.buttonAddX.Click += new System.EventHandler(this.buttonAddX_Click);
            // 
            // buttonAddY
            // 
            this.buttonAddY.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonAddY.ForeColor = System.Drawing.Color.Green;
            this.buttonAddY.Location = new System.Drawing.Point(0, 38);
            this.buttonAddY.Name = "buttonAddY";
            this.buttonAddY.Size = new System.Drawing.Size(32, 32);
            this.buttonAddY.TabIndex = 4;
            this.buttonAddY.Text = "+";
            this.buttonAddY.UseVisualStyleBackColor = true;
            this.buttonAddY.Click += new System.EventHandler(this.buttonAddY_Click);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel2.AutoScroll = true;
            this.flowLayoutPanel2.Controls.Add(this.panel1);
            this.flowLayoutPanel2.Controls.Add(this.panel2);
            this.flowLayoutPanel2.Controls.Add(this.panel3);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(12, 94);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(1240, 537);
            this.flowLayoutPanel2.TabIndex = 9;
            // 
            // panel2
            // 
            this.panel2.AutoSize = true;
            this.panel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.pictureBox2);
            this.panel2.Location = new System.Drawing.Point(84, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(73, 73);
            this.panel2.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Floors";
            // 
            // pictureBox2
            // 
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox2.Location = new System.Drawing.Point(38, 38);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(32, 32);
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Cell_MouseClick);
            this.pictureBox2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Cell_MouseMove);
            // 
            // panel3
            // 
            this.panel3.AutoSize = true;
            this.panel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.pictureBox3);
            this.panel3.Location = new System.Drawing.Point(163, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(73, 73);
            this.panel3.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Ceilings";
            // 
            // pictureBox3
            // 
            this.pictureBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox3.Location = new System.Drawing.Point(38, 38);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(32, 32);
            this.pictureBox3.TabIndex = 2;
            this.pictureBox3.TabStop = false;
            this.pictureBox3.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Cell_MouseClick);
            this.pictureBox3.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Cell_MouseMove);
            // 
            // topDock
            // 
            this.topDock.Controls.Add(this.fileNameLabel);
            this.topDock.Controls.Add(this.saveAsButton);
            this.topDock.Controls.Add(this.saveButton);
            this.topDock.Controls.Add(this.loadButton);
            this.topDock.Dock = System.Windows.Forms.DockStyle.Top;
            this.topDock.Location = new System.Drawing.Point(0, 0);
            this.topDock.Name = "topDock";
            this.topDock.Size = new System.Drawing.Size(1264, 38);
            this.topDock.TabIndex = 10;
            // 
            // fileNameLabel
            // 
            this.fileNameLabel.AutoSize = true;
            this.fileNameLabel.Location = new System.Drawing.Point(12, 13);
            this.fileNameLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.fileNameLabel.Name = "fileNameLabel";
            this.fileNameLabel.Size = new System.Drawing.Size(69, 13);
            this.fileNameLabel.TabIndex = 5;
            this.fileNameLabel.Text = "Nový Soubor";
            // 
            // saveAsButton
            // 
            this.saveAsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.saveAsButton.Location = new System.Drawing.Point(1181, 8);
            this.saveAsButton.Name = "saveAsButton";
            this.saveAsButton.Size = new System.Drawing.Size(75, 23);
            this.saveAsButton.TabIndex = 4;
            this.saveAsButton.Text = "Uložit jako...";
            this.saveAsButton.UseVisualStyleBackColor = true;
            this.saveAsButton.Click += new System.EventHandler(this.saveAsButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.Location = new System.Drawing.Point(1101, 8);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 3;
            this.saveButton.Text = "Uložit";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // loadButton
            // 
            this.loadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.loadButton.Location = new System.Drawing.Point(1021, 8);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(75, 23);
            this.loadButton.TabIndex = 2;
            this.loadButton.Text = "Načíst";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // fillDock
            // 
            this.fillDock.Controls.Add(this.label5);
            this.fillDock.Controls.Add(this.nextLevel);
            this.fillDock.Controls.Add(this.label4);
            this.fillDock.Controls.Add(this.spawnPointButton);
            this.fillDock.Controls.Add(this.flowLayoutPanel1);
            this.fillDock.Controls.Add(this.flowLayoutPanel2);
            this.fillDock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fillDock.Location = new System.Drawing.Point(0, 38);
            this.fillDock.Name = "fillDock";
            this.fillDock.Size = new System.Drawing.Size(1264, 643);
            this.fillDock.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(53, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(112, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "- Nastavit  spawnpoint";
            // 
            // spawnPointButton
            // 
            this.spawnPointButton.BackColor = System.Drawing.Color.Gold;
            this.spawnPointButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.spawnPointButton.Location = new System.Drawing.Point(15, 9);
            this.spawnPointButton.Name = "spawnPointButton";
            this.spawnPointButton.Size = new System.Drawing.Size(32, 32);
            this.spawnPointButton.TabIndex = 12;
            this.spawnPointButton.Click += new System.EventHandler(this.spawnPointButton_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Controls.Add(this.pictureBox4);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 50);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1240, 38);
            this.flowLayoutPanel1.TabIndex = 11;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // pictureBox4
            // 
            this.pictureBox4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox4.Location = new System.Drawing.Point(3, 3);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(32, 32);
            this.pictureBox4.TabIndex = 7;
            this.pictureBox4.TabStop = false;
            this.pictureBox4.Click += new System.EventHandler(this.Texture_Click);
            // 
            // nextLevel
            // 
            this.nextLevel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nextLevel.Location = new System.Drawing.Point(241, 15);
            this.nextLevel.Name = "nextLevel";
            this.nextLevel.Size = new System.Drawing.Size(1011, 20);
            this.nextLevel.TabIndex = 14;
            this.nextLevel.TextChanged += new System.EventHandler(this.nextLevel_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(171, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Další mapa:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.fillDock);
            this.Controls.Add(this.topDock);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Raycaster Level Editor";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.topDock.ResumeLayout(false);
            this.topDock.PerformLayout();
            this.fillDock.ResumeLayout(false);
            this.fillDock.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button buttonRemoveX;
        private System.Windows.Forms.Button buttonRemoveY;
        private System.Windows.Forms.Button buttonAddX;
        private System.Windows.Forms.Button buttonAddY;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Panel topDock;
        private System.Windows.Forms.Panel fillDock;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.Label fileNameLabel;
        private System.Windows.Forms.Button saveAsButton;
        private Panel spawnPoint;
        private Label label4;
        private Panel spawnPointButton;
        private Label label5;
        private TextBox nextLevel;
    }
}

