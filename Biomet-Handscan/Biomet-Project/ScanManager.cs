using Kaliko.ImageLibrary;
using System;
using System.Windows.Forms;
using TwainDotNet;
using TwainDotNet.TwainNative;
using TwainDotNet.WinFroms;

namespace Biomet_Project
{
    public class ScanManager
    {
        private Twain m_Twain;
        public Twain Twain { get { return m_Twain; } }

        private ScanSettings m_CurrentScanSettings = new ScanSettings();

        public bool IsInitialized { get; private set; }
        public bool IsScanning { get; private set; }

        public void Initialize(Form form)
        {
            try
            {
                m_Twain = new Twain(new WinFormsWindowMessageHook(form));
                m_CurrentScanSettings = new ScanSettings
                {
                    ShowProgressIndicatorUI = true,
                    Resolution = ResolutionSettings.ColourPhotocopier,
                    Rotation = new RotationSettings()
                    {
                        AutomaticRotate = true,
                        AutomaticBorderDetection = true,
                    }
                };

                m_Twain.ScanningComplete += delegate
                {
                    IsScanning = false;
                };

                IsInitialized = true;
            }
            catch (Exception e)
            {
                IsInitialized = false;
            }
        }

        public void StartScan()
        {
            if (IsInitialized)
            {
                IsScanning = true;
                m_Twain.StartScanning(m_CurrentScanSettings);
            }
        }

        public void SelectScanSource()
        {
            m_Twain.SelectSource();
        }

        public string GetActiveSourceLabel()
        {
            if (IsInitialized)
            {
                return string.Format("Scan source: {0}", m_Twain.DefaultSourceName);
            }
            else
            {
                return "ERROR: No scanners detected! Please make sure that a scanner is connected to the computer.";
            }
        }
    }
}