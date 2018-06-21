using Kaliko.ImageLibrary;
using Kaliko.ImageLibrary.BitFilters;
using LSTools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TwainDotNet;

namespace Biomet_Project
{
    public partial class MainWindow : Form
    {
        private ScanManager m_ScanManager;
        private ImageProcessor m_ImageProcessor;
        private HandAnalyzer m_HandAnalyzer;

        // scanner input
        private Bitmap m_ScannedImage;
        private Bitmap m_ScannedMarkers;

        // processed images
        private KalikoImage m_ProcessedImage;
        private KalikoImage m_ProcessedMarkers;

        // final composite (image - markers)
        private KalikoImage m_FinalComposite;

        public MainWindow()
        {
            InitializeComponent();

            InitializeScanning();
            InitializeProcessor();
            InitializeDetector();

            InitializeInput();
        }

        private void InitializeScanning()
        {
            m_ScanManager = new ScanManager();
            m_ScanManager.Initialize(this);
            scanSourceLabel.Text = m_ScanManager.GetActiveSourceLabel();
        }

        private void InitializeProcessor()
        {
            m_ImageProcessor = new ImageProcessor();
        }

        private void InitializeDetector()
        {
            m_HandAnalyzer = new HandAnalyzer();
        }

        private void InitializeInput()
        {
            PreviewKeyDown += HandleEscPressed;

            // disable all buttons (except debug) by default
            scanMarkersButton.Enabled = false;
            scanImageButton.Enabled = false;
            scanSelectButton.Enabled = false;

            previewMarkersScanButton.Enabled = false;
            previewMarkersProcessedButton.Enabled = false;
            previewImageScanButton.Enabled = false;
            previewImageProcessedButton.Enabled = false;
            previewOutlineButton.Enabled = false;

            verifyAddButton.Enabled = false;
            verifyButton.Enabled = false;

            // enable initial buttons
            if (m_ScanManager.IsInitialized)
            {
                scanSelectButton.Enabled = true;
                scanMarkersButton.Enabled = true;
            }
        }

        private void DisplayBitmap(Bitmap bitmap)
        {
            imageBox.Image = bitmap;
        }

        private void HandleEscPressed(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        private void scanSelectButton_Click(object sender, EventArgs e)
        {
            m_ScanManager.SelectScanSource();
            scanSourceLabel.Text = m_ScanManager.GetActiveSourceLabel();
        }

        private void markerScanButton_Click(object sender, EventArgs e)
        {
            if (m_ScanManager.IsInitialized)
            {
                scanMarkersButton.Enabled = false;
                m_ScanManager.Twain.TransferImage += HandleMarkerScanFinished;
                m_ScanManager.StartScan();
            }
        }

        private void imageScanButton_Click(object sender, EventArgs e)
        {
            if (m_ScanManager.IsInitialized)
            {
                scanMarkersButton.Enabled = true;
                m_ScanManager.Twain.TransferImage += HandleImageScanFinished;
                m_ScanManager.StartScan();
            }
        }

        private void HandleMarkerScanFinished(Object sender, TransferImageEventArgs e)
        {
            m_ScanManager.Twain.TransferImage -= HandleMarkerScanFinished;
            scanMarkersButton.Enabled = true;
            HandleMarkerScanFinished(e.Image, true);
        }

        private void HandleMarkerScanFinished(Bitmap bitmap, bool preview)
        {
            if (bitmap != null)
            {
                KalikoImage img = new KalikoImage(bitmap);
                img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                bitmap = img.GetAsBitmap();

                m_ScannedMarkers = bitmap;
                m_ProcessedMarkers = m_ImageProcessor.GetProcessedMarkers(m_ScannedMarkers);
                if (preview)
                {
                    DisplayBitmap(m_ScannedMarkers);
                }

                previewMarkersScanButton.Enabled = true;
                previewMarkersProcessedButton.Enabled = true;
            }
        }

        private void HandleImageScanFinished(Object sender, TransferImageEventArgs e)
        {
            m_ScanManager.Twain.TransferImage -= HandleImageScanFinished;
            scanImageButton.Enabled = true;
            HandleImageScanFinished(e.Image, true);
        }

        private void HandleImageScanFinished(Bitmap bitmap, bool preview)
        {
            if (bitmap != null)
            {
                KalikoImage img = new KalikoImage(bitmap);
                img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                bitmap = img.GetAsBitmap();

                m_ScannedImage = bitmap;
                m_ProcessedImage = m_ImageProcessor.GetProcessedImage(m_ScannedImage, m_ProcessedMarkers);
                if (preview)
                {
                    DisplayBitmap(m_ScannedImage);
                }

                previewImageScanButton.Enabled = true;
                previewImageProcessedButton.Enabled = true;
                previewOutlineButton.Enabled = true;
            }
        }

        private void debugScanButton_Click(object sender, EventArgs e)
        {
            KalikoImage markers = new KalikoImage(@"C:\Projects\Biomet-Handscan\markers_empty.jpg");
            if (markers != null)
            {
                HandleMarkerScanFinished(markers.GetAsBitmap(), false);
            }

            KalikoImage image = new KalikoImage(@"C:\Projects\Biomet-Handscan\handA2_color.jpg");
            if (image != null)
            {
                HandleImageScanFinished(image.GetAsBitmap(), true);
            }
        }

        private void previewMarkersScanButton_Click(object sender, EventArgs e)
        {
            DisplayBitmap(m_ScannedMarkers);
        }

        private void previewMarkersProcessedButton_Click(object sender, EventArgs e)
        {
            DisplayBitmap(m_ProcessedMarkers.GetAsBitmap());
        }

        private void previewImageScanButton_Click(object sender, EventArgs e)
        {
            DisplayBitmap(m_ScannedImage);
        }

        private void previewImageProcessedButton_Click(object sender, EventArgs e)
        {
            DisplayBitmap(m_ProcessedImage.GetAsBitmap());
        }

        private void verifyAddButton_Click(object sender, EventArgs e)
        {

        }

        private void verifyButton_Click(object sender, EventArgs e)
        {

        }

        private void previewFinalButton_Click(object sender, EventArgs e)
        {

        }

        private void previewOutlineButton_Click(object sender, EventArgs e)
        {
            BitMatrix matrix = new BitMatrix(m_ProcessedImage);
            List<Point> path = m_HandAnalyzer.FindLongestPath(matrix);

            // find centroid
            Point centroid = m_HandAnalyzer.FindCentroid(matrix, path);

            // find min/max points
            List<APair<int, double>> minimums, maximums;
            m_HandAnalyzer.FindFingerPoints(path, centroid, out maximums, out minimums);

            // find finger lengths and surface areas
            List<double> distances = m_HandAnalyzer.FindFingerFeatures(matrix, path, centroid, maximums, minimums);

            // path preview
            BitMatrix pathMatrix = new BitMatrix(matrix.Width, matrix.Height);
            pathMatrix.SetPoints(path, true);

            // centroid preview
            KalikoImage pathImage = pathMatrix.ToImage();
            pathImage.DrawMarker(centroid, Color.Magenta, 4);

            // finger points preview
            for (int i = 0; i < 5; ++i)
            {
                Point p = path[maximums[i].First];
                pathImage.DrawMarker(p, Color.Green, 4);
            }
            for (int i = 0; i < 4; ++i)
            {
                Point p = path[minimums[i].First];
                pathImage.DrawMarker(p, Color.Yellow, 4);
            }

            DisplayBitmap(pathImage.GetAsBitmap());
        }
    }
}
