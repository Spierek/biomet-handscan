namespace Biomet_Project
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.imageBox = new System.Windows.Forms.PictureBox();
            this.scanButton = new System.Windows.Forms.Button();
            this.scanBox = new System.Windows.Forms.GroupBox();
            this.scanSourceLabel = new System.Windows.Forms.Label();
            this.scanSelectButton = new System.Windows.Forms.Button();
            this.processBox = new System.Windows.Forms.GroupBox();
            this.processButton = new System.Windows.Forms.Button();
            this.verifyBox = new System.Windows.Forms.GroupBox();
            this.verifyAddButton = new System.Windows.Forms.Button();
            this.verifyButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox)).BeginInit();
            this.scanBox.SuspendLayout();
            this.processBox.SuspendLayout();
            this.verifyBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageBox
            // 
            this.imageBox.Location = new System.Drawing.Point(258, 12);
            this.imageBox.Name = "imageBox";
            this.imageBox.Size = new System.Drawing.Size(437, 534);
            this.imageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imageBox.TabIndex = 1;
            this.imageBox.TabStop = false;
            // 
            // scanButton
            // 
            this.scanButton.Location = new System.Drawing.Point(6, 118);
            this.scanButton.Name = "scanButton";
            this.scanButton.Size = new System.Drawing.Size(228, 26);
            this.scanButton.TabIndex = 2;
            this.scanButton.Text = "Scan Image";
            this.scanButton.UseVisualStyleBackColor = true;
            this.scanButton.Click += new System.EventHandler(this.scanButton_Click);
            // 
            // scanBox
            // 
            this.scanBox.Controls.Add(this.scanSelectButton);
            this.scanBox.Controls.Add(this.scanSourceLabel);
            this.scanBox.Controls.Add(this.scanButton);
            this.scanBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scanBox.Location = new System.Drawing.Point(12, 12);
            this.scanBox.Name = "scanBox";
            this.scanBox.Size = new System.Drawing.Size(240, 154);
            this.scanBox.TabIndex = 6;
            this.scanBox.TabStop = false;
            this.scanBox.Text = "SCAN";
            // 
            // scanSourceLabel
            // 
            this.scanSourceLabel.Location = new System.Drawing.Point(6, 18);
            this.scanSourceLabel.Name = "scanSourceLabel";
            this.scanSourceLabel.Size = new System.Drawing.Size(228, 65);
            this.scanSourceLabel.TabIndex = 3;
            this.scanSourceLabel.Text = "Scan source: EPSON X201-42 (Enabled)";
            // 
            // scanSelectButton
            // 
            this.scanSelectButton.Location = new System.Drawing.Point(6, 86);
            this.scanSelectButton.Name = "scanSelectButton";
            this.scanSelectButton.Size = new System.Drawing.Size(228, 26);
            this.scanSelectButton.TabIndex = 4;
            this.scanSelectButton.Text = "Change Active Scanner";
            this.scanSelectButton.UseVisualStyleBackColor = true;
            this.scanSelectButton.Click += new System.EventHandler(this.scanSelectButton_Click);
            // 
            // processBox
            // 
            this.processBox.Controls.Add(this.processButton);
            this.processBox.Location = new System.Drawing.Point(12, 172);
            this.processBox.Name = "processBox";
            this.processBox.Size = new System.Drawing.Size(240, 55);
            this.processBox.TabIndex = 7;
            this.processBox.TabStop = false;
            this.processBox.Text = "PROCESS";
            // 
            // processButton
            // 
            this.processButton.Location = new System.Drawing.Point(6, 21);
            this.processButton.Name = "processButton";
            this.processButton.Size = new System.Drawing.Size(228, 26);
            this.processButton.TabIndex = 2;
            this.processButton.Text = "Process Image";
            this.processButton.UseVisualStyleBackColor = true;
            this.processButton.Click += new System.EventHandler(this.processButton_Click_1);
            // 
            // verifyBox
            // 
            this.verifyBox.Controls.Add(this.verifyButton);
            this.verifyBox.Controls.Add(this.verifyAddButton);
            this.verifyBox.Location = new System.Drawing.Point(12, 233);
            this.verifyBox.Name = "verifyBox";
            this.verifyBox.Size = new System.Drawing.Size(240, 88);
            this.verifyBox.TabIndex = 8;
            this.verifyBox.TabStop = false;
            this.verifyBox.Text = "VERIFY";
            // 
            // verifyAddButton
            // 
            this.verifyAddButton.Location = new System.Drawing.Point(6, 21);
            this.verifyAddButton.Name = "verifyAddButton";
            this.verifyAddButton.Size = new System.Drawing.Size(228, 26);
            this.verifyAddButton.TabIndex = 2;
            this.verifyAddButton.Text = "Add To Database";
            this.verifyAddButton.UseVisualStyleBackColor = true;
            this.verifyAddButton.Click += new System.EventHandler(this.verifyAddButton_Click);
            // 
            // verifyButton
            // 
            this.verifyButton.Location = new System.Drawing.Point(6, 53);
            this.verifyButton.Name = "verifyButton";
            this.verifyButton.Size = new System.Drawing.Size(228, 26);
            this.verifyButton.TabIndex = 3;
            this.verifyButton.Text = "Verify Image";
            this.verifyButton.UseVisualStyleBackColor = true;
            this.verifyButton.Click += new System.EventHandler(this.verifyButton_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(707, 558);
            this.Controls.Add(this.verifyBox);
            this.Controls.Add(this.processBox);
            this.Controls.Add(this.scanBox);
            this.Controls.Add(this.imageBox);
            this.Name = "MainWindow";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.imageBox)).EndInit();
            this.scanBox.ResumeLayout(false);
            this.processBox.ResumeLayout(false);
            this.verifyBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.PictureBox imageBox;
        private System.Windows.Forms.Button scanButton;
        private System.Windows.Forms.GroupBox scanBox;
        private System.Windows.Forms.Button scanSelectButton;
        private System.Windows.Forms.Label scanSourceLabel;
        private System.Windows.Forms.GroupBox processBox;
        private System.Windows.Forms.Button processButton;
        private System.Windows.Forms.GroupBox verifyBox;
        private System.Windows.Forms.Button verifyAddButton;
        private System.Windows.Forms.Button verifyButton;
    }
}

