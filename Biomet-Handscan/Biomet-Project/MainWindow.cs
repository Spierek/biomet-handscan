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
        private Verifier m_Verifier;

        // scanner input
        private Bitmap m_ScannedImage;
        private Bitmap m_ScannedMarkers;

        // processed images
        private KalikoImage m_ProcessedImage;
        private KalikoImage m_ProcessedMarkers;

        // feature images
        private KalikoImage m_FeaturesOutline;
        private KalikoImage m_FeaturesPoints;

        // verification data
        private List<double> m_VerificationScanFeatures;

        private Color m_VerifyColorBase;
        private Color m_VerifyColorCorrect = Color.LightGreen;
        private Color m_VerifyColorIncorrect = Color.Pink;

        private const string MARKERS_EMPTY_PATH = @"C:\Projects\Biomet-Handscan\markers_empty.jpg";
        private const string A1_PATH = @"C:\Projects\Biomet-Handscan\handA1_color.jpg";
        private const string A2_PATH = @"C:\Projects\Biomet-Handscan\handA2_color.jpg";
        private const string B1_PATH = @"C:\Projects\Biomet-Handscan\handB1_color.jpg";

        public MainWindow()
        {
            InitializeComponent();

            InitializeScanning();
            m_ImageProcessor = new ImageProcessor();
            m_HandAnalyzer = new HandAnalyzer();
            m_Verifier = new Verifier();

            InitializeInput();
        }

        private void InitializeScanning()
        {
            m_ScanManager = new ScanManager();
            m_ScanManager.Initialize(this);
            scanSourceLabel.Text = m_ScanManager.GetActiveSourceLabel();
        }

        private void InitializeInput()
        {
            m_VerifyColorBase = verifyButton.BackColor;

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
            previewPointsButton.Enabled = false;

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
                m_VerificationScanFeatures = PrepareFeatures();
                if (preview)
                {
                    DisplayBitmap(m_ScannedImage);
                }

                previewImageScanButton.Enabled = true;
                previewImageProcessedButton.Enabled = true;
                previewOutlineButton.Enabled = true;
                previewPointsButton.Enabled = true;
                verifyAddButton.Enabled = true;

                if (m_Verifier.HasIdentity)
                {
                    verifyButton.Enabled = true;
                    verifyButton.BackColor = m_VerifyColorBase;
                }
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

        private void previewOutlineButton_Click(object sender, EventArgs e)
        {
            DisplayBitmap(m_FeaturesOutline.GetAsBitmap());
        }

        private void previewPointsButton_Click(object sender, EventArgs e)
        {
            DisplayBitmap(m_FeaturesPoints.GetAsBitmap());
        }

        private void verifyAddButton_Click(object sender, EventArgs e)
        {
            m_Verifier.SetIdentity(m_VerificationScanFeatures);
            verifyButton.Enabled = true;
            verifyButton.BackColor = m_VerifyColorBase;
        }

        private void verifyButton_Click(object sender, EventArgs e)
        {
            bool result = m_Verifier.Verify(m_VerificationScanFeatures);
            verifyButton.BackColor = result ? m_VerifyColorCorrect : m_VerifyColorIncorrect;
        }

        private List<double> PrepareFeatures()
        {
            BitMatrix matrix = new BitMatrix(m_ProcessedImage);
            List<Point> path = m_HandAnalyzer.FindLongestPath(matrix);

            // find centroid
            Point centroid = m_HandAnalyzer.FindCentroid(matrix, path);

            // find min/max points
            List<APair<int, double>> minimums, maximums;
            m_HandAnalyzer.FindFingerPoints(path, centroid, out maximums, out minimums);

            // find finger lengths and surface areas
            List<double> features = m_HandAnalyzer.FindFingerFeatures(matrix, path, centroid, maximums, minimums);

            // path preview
            BitMatrix pathMatrix = new BitMatrix(matrix.Width, matrix.Height);
            pathMatrix.SetPoints(path, true);
            m_FeaturesOutline = pathMatrix.ToImage();

            // centroid preview
            m_FeaturesPoints = pathMatrix.ToImage();
            m_FeaturesPoints.DrawMarker(centroid, Color.Magenta, 4);

            // finger points preview
            for (int i = 0; i < 5; ++i)
            {
                Point p = path[maximums[i].First];
                m_FeaturesPoints.DrawMarker(p, Color.Green, 4);
            }
            for (int i = 0; i < 4; ++i)
            {
                Point p = path[minimums[i].First];
                m_FeaturesPoints.DrawMarker(p, Color.Yellow, 4);
            }

            return features;
        }

        // DEBUG
        private void debugA1Button_Click(object sender, EventArgs e)
        {
            LoadDebug(A1_PATH);
        }

        private void debugA2Button_Click(object sender, EventArgs e)
        {
            LoadDebug(A2_PATH);
        }

        private void debugB1Button_Click(object sender, EventArgs e)
        {
            LoadDebug(B1_PATH);
        }

        private void LoadDebug(string path)
        {
            KalikoImage markers = new KalikoImage(MARKERS_EMPTY_PATH);
            if (markers != null)
            {
                HandleMarkerScanFinished(markers.GetAsBitmap(), false);
            }

            KalikoImage image = new KalikoImage(path);
            if (image != null)
            {
                HandleImageScanFinished(image.GetAsBitmap(), true);
            }
        }
    }
}
