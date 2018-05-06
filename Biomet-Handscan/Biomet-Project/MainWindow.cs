using Kaliko.ImageLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TwainDotNet;

namespace Biomet_Project
{
    public partial class MainWindow : Form
    {
        private ScanManager m_ScanManager;
        private ImageProcessor m_ImageProcessor;

        private bool m_ScanningEnabled = false;

        public MainWindow()
        {
            InitializeComponent();

            InitializeScanning();
            InitializeProcessor();

            InitializeInput();
        }

        private void InitializeScanning()
        {
            m_ScanManager = new ScanManager();
            m_ScanManager.Initialize(this);

            if (m_ScanManager.IsInitialized)
            {
                m_ScanManager.Twain.TransferImage += HandleScanFinished;
                infoLabel.Text = "Scanner ready.";
            }
            else
            {
                infoLabel.Text = "ERROR: No scanners detected! Please make sure that a scanner is connected to the computer.";
            }
        }

        private void InitializeProcessor()
        {
            m_ImageProcessor = new ImageProcessor();
            KalikoImage initialImage = m_ImageProcessor.LoadImage();
            KalikoImage processedImage = m_ImageProcessor.GetProcessedImage(initialImage);
            imageBox.Image = processedImage.GetAsBitmap();
        }

        private void InitializeInput()
        {
            PreviewKeyDown += HandleEscPressed;

            // disable all buttons by default
            scanButton.Enabled = false;
            processButton.Enabled = false;
            verifyButton.Enabled = false;

            if (m_ScanManager.IsInitialized)
            {
                scanButton.Enabled = true;
            }
        }

        private void HandleEscPressed(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        private void scanButton_Click(object sender, EventArgs e)
        {
            processButton.Enabled = false;
            verifyButton.Enabled = false;

            m_ScanManager.StartScan();
        }

        private void HandleScanFinished(Object sender, TransferImageEventArgs e)
        {
            scanButton.Enabled = true;

            if (e.Image != null)
            {
                imageBox.Image = e.Image;
                processButton.Enabled = true;
            }
        }

        private void processButton_Click(object sender, EventArgs e)
        {

        }

        private void verifyButton_Click(object sender, EventArgs e)
        {

        }
    }
}
