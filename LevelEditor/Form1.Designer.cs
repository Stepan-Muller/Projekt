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
            this.fileTextBox = new System.Windows.Forms.TextBox();
            this.browseButton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.buttonAddX = new System.Windows.Forms.Button();
            this.buttonAddY = new System.Windows.Forms.Button();
            this.buttonRemoveX = new System.Windows.Forms.Button();
            this.buttonRemoveY = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // fileTextBox
            // 
            this.fileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileTextBox.Location = new System.Drawing.Point(12, 12);
            this.fileTextBox.Name = "fileTextBox";
            this.fileTextBox.Size = new System.Drawing.Size(1159, 20);
            this.fileTextBox.TabIndex = 0;
            this.fileTextBox.TextChanged += new System.EventHandler(this.fileTextBox_TextChanged);
            // 
            // browseButton
            // 
            this.browseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.browseButton.Location = new System.Drawing.Point(1177, 9);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(75, 23);
            this.browseButton.TabIndex = 1;
            this.browseButton.Text = "Procházet ...";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(38, 38);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(64, 64);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // buttonAddX
            // 
            this.buttonAddX.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonAddX.ForeColor = System.Drawing.Color.Green;
            this.buttonAddX.Location = new System.Drawing.Point(70, 0);
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
            this.buttonAddY.Location = new System.Drawing.Point(0, 70);
            this.buttonAddY.Name = "buttonAddY";
            this.buttonAddY.Size = new System.Drawing.Size(32, 32);
            this.buttonAddY.TabIndex = 4;
            this.buttonAddY.Text = "+";
            this.buttonAddY.UseVisualStyleBackColor = true;
            this.buttonAddY.Click += new System.EventHandler(this.buttonAddY_Click);
            // 
            // buttonRemoveX
            // 
            this.buttonRemoveX.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonRemoveX.ForeColor = System.Drawing.Color.Red;
            this.buttonRemoveX.Location = new System.Drawing.Point(38, 0);
            this.buttonRemoveX.Name = "buttonRemoveX";
            this.buttonRemoveX.Size = new System.Drawing.Size(32, 32);
            this.buttonRemoveX.TabIndex = 5;
            this.buttonRemoveX.Text = "-";
            this.buttonRemoveX.UseVisualStyleBackColor = true;
            this.buttonRemoveX.Visible = false;
            this.buttonRemoveX.Click += new System.EventHandler(this.buttonRemoveX_Click);
            // 
            // buttonRemoveY
            // 
            this.buttonRemoveY.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonRemoveY.ForeColor = System.Drawing.Color.Red;
            this.buttonRemoveY.Location = new System.Drawing.Point(0, 38);
            this.buttonRemoveY.Name = "buttonRemoveY";
            this.buttonRemoveY.Size = new System.Drawing.Size(32, 32);
            this.buttonRemoveY.TabIndex = 6;
            this.buttonRemoveY.Text = "-";
            this.buttonRemoveY.UseVisualStyleBackColor = true;
            this.buttonRemoveY.Visible = false;
            this.buttonRemoveY.Click += new System.EventHandler(this.buttonRemoveY_Click);
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.Location = new System.Drawing.Point(1177, 646);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 7;
            this.saveButton.Text = "Uložit";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Controls.Add(this.buttonRemoveX);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.buttonRemoveY);
            this.panel1.Controls.Add(this.buttonAddX);
            this.panel1.Controls.Add(this.buttonAddY);
            this.panel1.Location = new System.Drawing.Point(12, 38);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(105, 105);
            this.panel1.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.fileTextBox);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox fileTextBox;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonAddX;
        private System.Windows.Forms.Button buttonAddY;
        private System.Windows.Forms.Button buttonRemoveX;
        private System.Windows.Forms.Button buttonRemoveY;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Panel panel1;
    }
}

