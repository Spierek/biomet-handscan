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
        private Bitmap m_CurrentBitmap;

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
            scanSourceLabel.Text = m_ScanManager.GetActiveSourceLabel();

            if (m_ScanManager.IsInitialized)
            {
                m_ScanManager.Twain.TransferImage += HandleScanFinished;
            }
        }

        private void InitializeProcessor()
        {
            m_ImageProcessor = new ImageProcessor();
        }

        private void InitializeInput()
        {
            PreviewKeyDown += HandleEscPressed;

            // disable all buttons by default
            scanButton.Enabled = false;
            scanSelectButton.Enabled = false;
            processButton.Enabled = false;
            verifyAddButton.Enabled = false;
            verifyButton.Enabled = false;

            if (m_ScanManager.IsInitialized)
            {
                scanButton.Enabled = true;
                scanSelectButton.Enabled = true;
            }
        }

        private void SetCurrentBitmap(Bitmap bitmap)
        {
            m_CurrentBitmap = bitmap;
            imageBox.Image = m_CurrentBitmap;
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
            verifyAddButton.Enabled = false;
            verifyButton.Enabled = false;

            m_ScanManager.StartScan();
        }

        private void HandleScanFinished(Object sender, TransferImageEventArgs e)
        {
            scanButton.Enabled = true;

            if (e.Image != null)
            {
                SetCurrentBitmap(e.Image);
                processButton.Enabled = true;
            }
        }

        private void scanSelectButton_Click(object sender, EventArgs e)
        {
            m_ScanManager.SelectScanSource();
            scanSourceLabel.Text = m_ScanManager.GetActiveSourceLabel();
        }

        private void processButton_Click_1(object sender, EventArgs e)
        {
            if (m_CurrentBitmap != null)
            {
                KalikoImage scanImage = new KalikoImage(m_CurrentBitmap);
                KalikoImage processedImage = m_ImageProcessor.GetProcessedImage(scanImage);
                if (processedImage != null)
                {
                    SetCurrentBitmap(processedImage.GetAsBitmap());

                    processButton.Enabled = false;
                    verifyAddButton.Enabled = true;
                    verifyButton.Enabled = true;
                }
            }
        }

        private void verifyAddButton_Click(object sender, EventArgs e)
        {

        }

        private void verifyButton_Click(object sender, EventArgs e)
        {

        }
    }
}
