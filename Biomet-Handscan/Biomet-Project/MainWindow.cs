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
        private KalikoImage m_FeaturesImage;

        // verification data
        private KalikoImage m_VerificationOwnerImage;
        private List<double> m_VerificationScanFeatures;

        private Color m_ButtonColorBase;
        private Color m_ButtonColorCorrect = Color.LightGreen;
        private Color m_ButtonColorIncorrect = Color.Pink;

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
            m_ButtonColorBase = verifyButton.BackColor;

            PreviewKeyDown += HandleEscPressed;

            // disable all buttons (except debug) by default
            scanMarkersButton.Enabled = false;
            scanImageButton.Enabled = false;
            scanSelectButton.Enabled = false;

            previewMarkersScanButton.Enabled = false;
            previewMarkersProcessedButton.Enabled = false;
            previewImageScanButton.Enabled = false;
            previewImageProcessedButton.Enabled = false;
            analyzeGenerateButton.Enabled = false;
            analyzePreviewButton.Enabled = false;

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

        private void loadMarkersButton_Click(object sender, EventArgs e)
        {
            KalikoImage markers = new KalikoImage(MARKERS_EMPTY_PATH);
            if (markers != null)
            {
                HandleMarkerScanFinished(markers.GetAsBitmap(), false, true);
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
            HandleMarkerScanFinished(e.Image, true, false);
        }

        private void HandleMarkerScanFinished(Bitmap bitmap, bool preview, bool rotate)
        {
            if (bitmap != null)
            {
                if (rotate)
                {
                    KalikoImage img = new KalikoImage(bitmap);
                    img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    bitmap = img.GetAsBitmap();
                }

                m_ScannedMarkers = bitmap;
                m_ProcessedMarkers = m_ImageProcessor.GetProcessedMarkers(m_ScannedMarkers);
                if (preview)
                {
                    DisplayBitmap(m_ScannedMarkers);
                }

                scanImageButton.Enabled = true;
                previewMarkersScanButton.Enabled = true;
                previewMarkersProcessedButton.Enabled = true;

                previewImageScanButton.Enabled = false;
                previewImageProcessedButton.Enabled = false;

                analyzeGenerateButton.Enabled = false;
                analyzeGenerateButton.BackColor = m_ButtonColorBase;
                analyzePreviewButton.Enabled = false;
                verifyAddButton.Enabled = false;
                verifyPreviewOwnerButton.Enabled = false;
                verifyButton.Enabled = false;
                verifyButton.BackColor = m_ButtonColorBase;
            }
        }

        private void HandleImageScanFinished(Object sender, TransferImageEventArgs e)
        {
            m_ScanManager.Twain.TransferImage -= HandleImageScanFinished;
            scanImageButton.Enabled = true;
            HandleImageScanFinished(e.Image, true, false);
        }

        private void HandleImageScanFinished(Bitmap bitmap, bool preview, bool rotate)
        {
            if (bitmap != null)
            {
                if (rotate)
                {
                    KalikoImage img = new KalikoImage(bitmap);
                    img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    bitmap = img.GetAsBitmap();
                }

                m_ScannedImage = bitmap;
                m_ProcessedImage = m_ImageProcessor.GetProcessedImage(m_ScannedImage, m_ProcessedMarkers);
                if (preview)
                {
                    DisplayBitmap(m_ScannedImage);
                }

                previewImageScanButton.Enabled = true;
                previewImageProcessedButton.Enabled = true;

                analyzeGenerateButton.Enabled = true;
                analyzeGenerateButton.BackColor = m_ButtonColorBase;
                analyzePreviewButton.Enabled = false;

                verifyAddButton.Enabled = false;
                verifyPreviewOwnerButton.Enabled = false;
                verifyButton.Enabled = false;
                verifyButton.BackColor = m_ButtonColorBase;
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

        private void analyzeGenerateButton_Click(object sender, EventArgs e)
        {
            m_VerificationScanFeatures = PrepareFeatures();
            DisplayBitmap(m_FeaturesImage.GetAsBitmap());
            analyzePreviewButton.Enabled = true;

            if (m_VerificationScanFeatures == null)
            {
                analyzeGenerateButton.BackColor = m_ButtonColorIncorrect;
            }
            else
            {
                analyzeGenerateButton.BackColor = m_ButtonColorCorrect;
                verifyAddButton.Enabled = true;
                if (m_Verifier.HasIdentity)
                {
                    verifyPreviewOwnerButton.Enabled = true;
                    verifyButton.Enabled = true;
                    verifyButton.BackColor = m_ButtonColorBase;
                }
            }
        }

        private void analyzePreviewButton_Click(object sender, EventArgs e)
        {
            DisplayBitmap(m_FeaturesImage.GetAsBitmap());
        }

        private void verifyAddButton_Click(object sender, EventArgs e)
        {
            m_Verifier.SetIdentity(m_VerificationScanFeatures);
            m_VerificationOwnerImage = m_FeaturesImage;
            verifyPreviewOwnerButton.Enabled = true;
            verifyButton.Enabled = true;
            verifyButton.BackColor = m_ButtonColorBase;
        }

        private void verifyPreviewOwnerButton_Click(object sender, EventArgs e)
        {
            DisplayBitmap(m_VerificationOwnerImage.GetAsBitmap());
        }

        private void verifyButton_Click(object sender, EventArgs e)
        {
            bool result = m_Verifier.Verify(m_VerificationScanFeatures);
            verifyButton.BackColor = result ? m_ButtonColorCorrect : m_ButtonColorIncorrect;
        }

        private List<double> PrepareFeatures()
        {
            List<double> features = null;

            BitMatrix matrix = new BitMatrix(m_ProcessedImage);
            List<Point> path = m_HandAnalyzer.FindLongestPath(matrix);

            // find centroid
            Point centroid = m_HandAnalyzer.FindCentroid(matrix, path);

            // find min/max points
            List<APair<int, double>> minimums, maximums;
            bool result = m_HandAnalyzer.FindFingerPoints(path, centroid, out maximums, out minimums);

            if (result)
            {
                // find finger features
                features = m_HandAnalyzer.FindFingerFeatures(matrix, path, centroid, maximums, minimums);
            }

            // path preview
            BitMatrix pathMatrix = new BitMatrix(matrix.Width, matrix.Height);
            pathMatrix.SetPoints(path, true);

            // centroid preview
            m_FeaturesImage = pathMatrix.ToImage();
            m_FeaturesImage.DrawMarker(centroid, Color.Magenta, 4);

            // finger points preview
            for (int i = 0; i < maximums.Count; ++i)
            {
                Point p = path[maximums[i].First];
                m_FeaturesImage.DrawMarker(p, Color.Green, 4);
            }
            for (int i = 0; i < minimums.Count; ++i)
            {
                Point p = path[minimums[i].First];
                m_FeaturesImage.DrawMarker(p, Color.Yellow, 4);
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
            loadMarkersButton_Click(null, null);

            KalikoImage image = new KalikoImage(path);
            if (image != null)
            {
                HandleImageScanFinished(image.GetAsBitmap(), true, true);
            }
        }
    }
}
