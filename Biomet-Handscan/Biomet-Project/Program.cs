using Kaliko.ImageLibrary;
using System;
using System.Windows.Forms;

namespace Biomet_Project
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MainWindow window = new MainWindow();
            ImageProcessor ip = new ImageProcessor();

            KalikoImage initialImage = ip.LoadImage();
            window.initialImageBox.Image = initialImage.GetAsBitmap();

            KalikoImage processedImage = ip.GetProcessedImage(initialImage);
            window.processedImageBox.Image = processedImage.GetAsBitmap();

            Application.Run(window);
        }
    }
}
