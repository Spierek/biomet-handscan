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
            this.scanImageButton = new System.Windows.Forms.Button();
            this.scanBox = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.debugB1Button = new System.Windows.Forms.Button();
            this.debugA2Button = new System.Windows.Forms.Button();
            this.debugA1Button = new System.Windows.Forms.Button();
            this.scanMarkersButton = new System.Windows.Forms.Button();
            this.scanSelectButton = new System.Windows.Forms.Button();
            this.scanSourceLabel = new System.Windows.Forms.Label();
            this.processBox = new System.Windows.Forms.GroupBox();
            this.previewOutlineButton = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.previewImageProcessedButton = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.previewImageScanButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.previewMarkersProcessedButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.previewMarkersScanButton = new System.Windows.Forms.Button();
            this.verifyBox = new System.Windows.Forms.GroupBox();
            this.verifyButton = new System.Windows.Forms.Button();
            this.verifyAddButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox)).BeginInit();
            this.scanBox.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.processBox.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
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
            // scanImageButton
            // 
            this.scanImageButton.Location = new System.Drawing.Point(6, 159);
            this.scanImageButton.Name = "scanImageButton";
            this.scanImageButton.Size = new System.Drawing.Size(228, 26);
            this.scanImageButton.TabIndex = 2;
            this.scanImageButton.Text = "Scan Image";
            this.scanImageButton.UseVisualStyleBackColor = true;
            this.scanImageButton.Click += new System.EventHandler(this.imageScanButton_Click);
            // 
            // scanBox
            // 
            this.scanBox.Controls.Add(this.groupBox2);
            this.scanBox.Controls.Add(this.scanMarkersButton);
            this.scanBox.Controls.Add(this.scanSelectButton);
            this.scanBox.Controls.Add(this.scanSourceLabel);
            this.scanBox.Controls.Add(this.scanImageButton);
            this.scanBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scanBox.Location = new System.Drawing.Point(12, 12);
            this.scanBox.Name = "scanBox";
            this.scanBox.Size = new System.Drawing.Size(240, 256);
            this.scanBox.TabIndex = 6;
            this.scanBox.TabStop = false;
            this.scanBox.Text = "SCAN";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.debugB1Button);
            this.groupBox2.Controls.Add(this.debugA2Button);
            this.groupBox2.Controls.Add(this.debugA1Button);
            this.groupBox2.Location = new System.Drawing.Point(6, 191);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(228, 57);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "DEBUG";
            // 
            // debugB1Button
            // 
            this.debugB1Button.Location = new System.Drawing.Point(158, 21);
            this.debugB1Button.Name = "debugB1Button";
            this.debugB1Button.Size = new System.Drawing.Size(64, 26);
            this.debugB1Button.TabIndex = 7;
            this.debugB1Button.Text = "B1";
            this.debugB1Button.UseVisualStyleBackColor = true;
            this.debugB1Button.Click += new System.EventHandler(this.debugB1Button_Click);
            // 
            // debugA2Button
            // 
            this.debugA2Button.Location = new System.Drawing.Point(82, 21);
            this.debugA2Button.Name = "debugA2Button";
            this.debugA2Button.Size = new System.Drawing.Size(64, 26);
            this.debugA2Button.TabIndex = 6;
            this.debugA2Button.Text = "A2";
            this.debugA2Button.UseVisualStyleBackColor = true;
            this.debugA2Button.Click += new System.EventHandler(this.debugA2Button_Click);
            // 
            // debugA1Button
            // 
            this.debugA1Button.Location = new System.Drawing.Point(6, 21);
            this.debugA1Button.Name = "debugA1Button";
            this.debugA1Button.Size = new System.Drawing.Size(64, 26);
            this.debugA1Button.TabIndex = 5;
            this.debugA1Button.Text = "A1";
            this.debugA1Button.UseVisualStyleBackColor = true;
            this.debugA1Button.Click += new System.EventHandler(this.debugA1Button_Click);
            // 
            // scanMarkersButton
            // 
            this.scanMarkersButton.Location = new System.Drawing.Point(6, 127);
            this.scanMarkersButton.Name = "scanMarkersButton";
            this.scanMarkersButton.Size = new System.Drawing.Size(228, 26);
            this.scanMarkersButton.TabIndex = 6;
            this.scanMarkersButton.Text = "Scan Markers";
            this.scanMarkersButton.UseVisualStyleBackColor = true;
            this.scanMarkersButton.Click += new System.EventHandler(this.markerScanButton_Click);
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
            // scanSourceLabel
            // 
            this.scanSourceLabel.Location = new System.Drawing.Point(6, 18);
            this.scanSourceLabel.Name = "scanSourceLabel";
            this.scanSourceLabel.Size = new System.Drawing.Size(228, 65);
            this.scanSourceLabel.TabIndex = 3;
            this.scanSourceLabel.Text = "Scan source: EPSON X201-42 (Enabled)";
            // 
            // processBox
            // 
            this.processBox.Controls.Add(this.previewOutlineButton);
            this.processBox.Controls.Add(this.groupBox3);
            this.processBox.Controls.Add(this.groupBox1);
            this.processBox.Location = new System.Drawing.Point(12, 274);
            this.processBox.Name = "processBox";
            this.processBox.Size = new System.Drawing.Size(240, 178);
            this.processBox.TabIndex = 7;
            this.processBox.TabStop = false;
            this.processBox.Text = "PREVIEW";
            // 
            // previewOutlineButton
            // 
            this.previewOutlineButton.Location = new System.Drawing.Point(9, 143);
            this.previewOutlineButton.Name = "previewOutlineButton";
            this.previewOutlineButton.Size = new System.Drawing.Size(225, 26);
            this.previewOutlineButton.TabIndex = 4;
            this.previewOutlineButton.Text = "Create Outline";
            this.previewOutlineButton.UseVisualStyleBackColor = true;
            this.previewOutlineButton.Click += new System.EventHandler(this.previewOutlineButton_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.previewImageProcessedButton);
            this.groupBox3.Controls.Add(this.button6);
            this.groupBox3.Controls.Add(this.previewImageScanButton);
            this.groupBox3.Location = new System.Drawing.Point(9, 82);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(225, 55);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Image";
            // 
            // previewImageProcessedButton
            // 
            this.previewImageProcessedButton.Location = new System.Drawing.Point(114, 21);
            this.previewImageProcessedButton.Name = "previewImageProcessedButton";
            this.previewImageProcessedButton.Size = new System.Drawing.Size(105, 26);
            this.previewImageProcessedButton.TabIndex = 3;
            this.previewImageProcessedButton.Text = "Processed";
            this.previewImageProcessedButton.UseVisualStyleBackColor = true;
            this.previewImageProcessedButton.Click += new System.EventHandler(this.previewImageProcessedButton_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(6, 115);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(228, 26);
            this.button6.TabIndex = 2;
            this.button6.Text = "Preview Markers";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // previewImageScanButton
            // 
            this.previewImageScanButton.Location = new System.Drawing.Point(6, 21);
            this.previewImageScanButton.Name = "previewImageScanButton";
            this.previewImageScanButton.Size = new System.Drawing.Size(105, 26);
            this.previewImageScanButton.TabIndex = 2;
            this.previewImageScanButton.Text = "Scan";
            this.previewImageScanButton.UseVisualStyleBackColor = true;
            this.previewImageScanButton.Click += new System.EventHandler(this.previewImageScanButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.previewMarkersProcessedButton);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.previewMarkersScanButton);
            this.groupBox1.Location = new System.Drawing.Point(9, 21);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(225, 55);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Markers";
            // 
            // previewMarkersProcessedButton
            // 
            this.previewMarkersProcessedButton.Location = new System.Drawing.Point(114, 21);
            this.previewMarkersProcessedButton.Name = "previewMarkersProcessedButton";
            this.previewMarkersProcessedButton.Size = new System.Drawing.Size(105, 26);
            this.previewMarkersProcessedButton.TabIndex = 3;
            this.previewMarkersProcessedButton.Text = "Processed";
            this.previewMarkersProcessedButton.UseVisualStyleBackColor = true;
            this.previewMarkersProcessedButton.Click += new System.EventHandler(this.previewMarkersProcessedButton_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 115);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(228, 26);
            this.button1.TabIndex = 2;
            this.button1.Text = "Preview Markers";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // previewMarkersScanButton
            // 
            this.previewMarkersScanButton.Location = new System.Drawing.Point(6, 21);
            this.previewMarkersScanButton.Name = "previewMarkersScanButton";
            this.previewMarkersScanButton.Size = new System.Drawing.Size(105, 26);
            this.previewMarkersScanButton.TabIndex = 2;
            this.previewMarkersScanButton.Text = "Scan";
            this.previewMarkersScanButton.UseVisualStyleBackColor = true;
            this.previewMarkersScanButton.Click += new System.EventHandler(this.previewMarkersScanButton_Click);
            // 
            // verifyBox
            // 
            this.verifyBox.Controls.Add(this.verifyButton);
            this.verifyBox.Controls.Add(this.verifyAddButton);
            this.verifyBox.Location = new System.Drawing.Point(12, 458);
            this.verifyBox.Name = "verifyBox";
            this.verifyBox.Size = new System.Drawing.Size(240, 88);
            this.verifyBox.TabIndex = 8;
            this.verifyBox.TabStop = false;
            this.verifyBox.Text = "VERIFY";
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
            // verifyAddButton
            // 
            this.verifyAddButton.Location = new System.Drawing.Point(6, 21);
            this.verifyAddButton.Name = "verifyAddButton";
            this.verifyAddButton.Size = new System.Drawing.Size(228, 26);
            this.verifyAddButton.TabIndex = 2;
            this.verifyAddButton.Text = "Set As Owner";
            this.verifyAddButton.UseVisualStyleBackColor = true;
            this.verifyAddButton.Click += new System.EventHandler(this.verifyAddButton_Click);
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
            this.groupBox2.ResumeLayout(false);
            this.processBox.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.verifyBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.PictureBox imageBox;
        private System.Windows.Forms.Button scanImageButton;
        private System.Windows.Forms.GroupBox scanBox;
        private System.Windows.Forms.Button scanSelectButton;
        private System.Windows.Forms.Label scanSourceLabel;
        private System.Windows.Forms.GroupBox processBox;
        private System.Windows.Forms.Button previewMarkersScanButton;
        private System.Windows.Forms.GroupBox verifyBox;
        private System.Windows.Forms.Button verifyAddButton;
        private System.Windows.Forms.Button verifyButton;
        private System.Windows.Forms.Button debugA1Button;
        private System.Windows.Forms.Button scanMarkersButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button previewMarkersProcessedButton;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button previewImageProcessedButton;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button previewImageScanButton;
        private System.Windows.Forms.Button previewOutlineButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button debugB1Button;
        private System.Windows.Forms.Button debugA2Button;
    }
}

