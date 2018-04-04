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
            this.initialImageBox = new System.Windows.Forms.PictureBox();
            this.processedImageBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.initialImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.processedImageBox)).BeginInit();
            this.SuspendLayout();
            // 
            // initialImageBox
            // 
            this.initialImageBox.Location = new System.Drawing.Point(12, 12);
            this.initialImageBox.Name = "initialImageBox";
            this.initialImageBox.Size = new System.Drawing.Size(437, 534);
            this.initialImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.initialImageBox.TabIndex = 0;
            this.initialImageBox.TabStop = false;
            // 
            // processedImageBox
            // 
            this.processedImageBox.Location = new System.Drawing.Point(472, 12);
            this.processedImageBox.Name = "processedImageBox";
            this.processedImageBox.Size = new System.Drawing.Size(437, 534);
            this.processedImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.processedImageBox.TabIndex = 1;
            this.processedImageBox.TabStop = false;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(921, 558);
            this.Controls.Add(this.initialImageBox);
            this.Controls.Add(this.processedImageBox);
            this.Name = "MainWindow";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.initialImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.processedImageBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.PictureBox initialImageBox;
        public System.Windows.Forms.PictureBox processedImageBox;
    }
}

