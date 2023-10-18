using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace FS6xEnrollmentKit_CS
{
    public partial class FormAcquisition : Form
    {
        private delegate void SetControlTextCallback(Object obj, string text);
        private delegate void EnableControlsCallback(bool bEnable);
        private delegate void EnableOneButtonCallback(Button button, bool bEnable);

        private FPDevice m_devFP = null;
        private bool m_bExit = false;
        private bool m_bCaptureOrAccept = true;
        private int m_nDiagnosticCode = 0;
        public FingerType m_nSequence = 0;
        public bool m_bIsReCapture = false;
        public byte m_nScanType = 0;	//bit0. Slaps 1.Single 2.Rolled
        public bool m_bAutoAccept;
        public bool m_bAskUnavailabilityReason;
        private Thread m_WorkerThread = null;

        public FormAcquisition( FPDevice devFP )
        {
            InitializeComponent();
            m_nSequence = 0;
            m_devFP = devFP;
            comboACTL.SelectedIndex = m_devFP.GetACTL() - 1;
            textMatchScoreThreshold.Text = FPVerify.m_nThreshold.ToString();
        }

        private void SetControlText(Object obj, String strText)
        {
            // Do not change the state control during application closing.
            if (m_bExit)
                return;

            if (this.textDiagnostic.InvokeRequired)
            {
                SetControlTextCallback d = new SetControlTextCallback(this.SetControlText);
                this.Invoke(d, new object[] {obj, strText });
            }
            else
            {
                if (obj is TextBox)
                    ((TextBox)obj).Text = strText;
                else if (obj is Button)
                    ((Button)obj).Text = strText;
                this.Update();
            }
        }

        private void SetButtonState(bool bEnable)
        {
            // Do not change the state control during application closing.
            if (m_bExit)
                return;
            if (this.InvokeRequired)
            {
                EnableControlsCallback d = new EnableControlsCallback(this.SetButtonState);
                this.Invoke(d, new object[] { bEnable });
            }
            else
            {
                buttonAccept.Enabled = bEnable;
                buttonRepeat.Enabled = bEnable;
                buttonBreak.Enabled = bEnable;
                if (m_bIsReCapture)
                    buttonSkip.Enabled = false;
                else
                    buttonSkip.Enabled = bEnable;
                buttonStop.Enabled = false;
            }
        }

        private void SetOneButtonState(Button button, bool bEnable)
        {
            // Do not change the state control during application closing.
            if (m_bExit)
                return;
            if (this.InvokeRequired)
            {
                EnableOneButtonCallback d = new EnableOneButtonCallback(this.SetOneButtonState);
                this.Invoke(d, new object[] { button, bEnable });
            }
            else
            {
                button.Enabled = bEnable;
            }
        }

        private void FormAcquisition_Load(object sender, EventArgs e)
        {
            m_devFP.SetShowImageHandler(pictureAcq);
            m_bExit = false;
            m_devFP.OnAction += new FPDevice.OnActiveHandler(this.OnAction);
            //
            StartOperation();
        }

        private void StartOperation()
        {
            if (!m_bIsReCapture)
            {
                if ((m_nScanType & FPInfo.DEVICE_SCAN_TYPE_SLAPS) != 0)	//Slaps
                    m_nSequence = FingerType.FT_LEFT_4_FINGERS;
                else if ((m_nScanType & FPInfo.DEVICE_SCAN_TYPE_2THUMBS) != 0)
                    m_nSequence = FingerType.FT_2_THUMBS;
                else if ((m_nScanType & FPInfo.DEVICE_SCAN_TYPE_FLAT_FINGER) != 0)
                    m_nSequence = FingerType.FT_LEFT_LITTLE;
                else if ((m_nScanType & FPInfo.DEVICE_SCAN_TYPE_ROLLED_FINGER) != 0)
                    m_nSequence = FingerType.FT_ROLLED_LEFT_LITTLE;
            }
            StartScanning();
        }

        private void StartScanning()
        {
            SetControlText(textMessage, "");
            SetControlText(textDiagnostic,"");
            m_devFP.SetCurrentFinger(m_nSequence);
            ReloadBitmapAndShowMessage(m_nSequence);
            SetButtonState(false);

            if (m_nSequence < FingerType.FT_LEFT_LITTLE)
            {
                SetOneButtonState(buttonStop, true);
                if (!m_bIsReCapture)
                    SetOneButtonState(buttonSkip, true);
                SetOneButtonState(buttonBreak, true);
                if (m_nSequence == FingerType.FT_2_THUMBS)
                    m_devFP.SetImageFormat(ImageFormat.FORMAT_800_750);
                else
                    m_devFP.SetImageFormat(ImageFormat.FORMAT_1600_1500);
                SetControlText(buttonAccept, "Capture");
                m_bCaptureOrAccept = true;
                if (!m_devFP.Scan())
                    SetControlText(textMessage, m_devFP.GetErrorMessage());
                SetOneButtonState(buttonAccept, true);
            }
            else if (m_nSequence < FingerType.FT_ROLLED_LEFT_LITTLE)
            {
                SetOneButtonState(buttonStop, true);
                if (!m_bIsReCapture)
                    SetOneButtonState(buttonSkip, true);
                SetOneButtonState(buttonBreak, true);
                m_devFP.SetImageFormat(ImageFormat.FORMAT_800_750);
                SetControlText(buttonAccept, "Capture");
                m_bCaptureOrAccept = true;
                if (!m_devFP.Scan())
                    SetControlText(textMessage, m_devFP.GetErrorMessage());
                SetOneButtonState(buttonAccept, true);
            }
            else if (m_nSequence < FingerType.FT_ROLLED_RIGHT_LITTLE + 1)
            {
                SetControlText(buttonAccept, "Accept");
                m_bCaptureOrAccept = false;
                m_devFP.SetImageFormat(ImageFormat.FORMAT_800_750);
                if (!m_devFP.RollScan())
                    SetControlText(textMessage, m_devFP.GetErrorMessage());
            }
            else if (m_nSequence < FingerType.FT_PLAIN_RIGHT_THUMB + 1)
            {
                SetOneButtonState(buttonStop, true);
                if (!m_bIsReCapture)
                    SetOneButtonState(buttonSkip, true);
                SetOneButtonState(buttonBreak, true);
                m_devFP.SetImageFormat(ImageFormat.FORMAT_800_750);
                SetControlText(buttonAccept, "Capture");
                m_bCaptureOrAccept = true;
                if (!m_devFP.Scan())
                    SetControlText(textMessage, m_devFP.GetErrorMessage());
                SetOneButtonState(buttonAccept, true);
            }
            else
            {
                SetOneButtonState(buttonStop, false);
                SetOneButtonState(buttonSkip, false);
                SetOneButtonState(buttonAccept, false);
                SetOneButtonState(buttonRepeat, false);
                SetOneButtonState(buttonBreak, true);
                SetControlText(buttonBreak, "Exit");
                SetControlText(textMessage, "-- Finished -- Please click the Exit button to exit.");
                m_devFP.TurnOffLed();
                m_bExit = true;
            }
        }

        private void FormAcquisition_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (m_devFP != null)
            {
                m_devFP.OnAction -= new FPDevice.OnActiveHandler(this.OnAction);
                m_devFP.Stop();
                m_devFP.Close();
                m_devFP.TurnOffLed();
                m_devFP = null;
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            m_devFP.Stop();
            m_devFP.TurnOffLed();
            buttonStop.Enabled = false;
            buttonAccept.Enabled = false;
            buttonRepeat.Enabled = true; 
        }

        public void OnAction(int nAction)
        {
	        if( nAction == 0 )
	        {
                SetControlText(textMessage, "Verifying......" );           
                if( m_devFP.VerifyImage( m_nSequence ) )
                {
                    SetControlText(textMessage, "Acquisition Successful!" );
                    String strDiag = m_devFP.GetDiagnostic(ref m_nDiagnosticCode);
                    if (m_nDiagnosticCode == 10)
                    {
                        SetButtonState(true);
                        SetOneButtonState(buttonAccept, false);
                    }
                    else
                    {
                        SetButtonState(true);
                        SetControlText(buttonAccept, "Accept");
                        m_bCaptureOrAccept = false;	//Accept
                    }
                    if(m_nDiagnosticCode != 0)
                        textDiagnostic.ForeColor = Color.Red;
                    else
                        textDiagnostic.ForeColor = Color.Black;
                    SetControlText(textDiagnostic,strDiag);
                    if (m_bAutoAccept && m_nDiagnosticCode == 0)
                    {
                        m_devFP.ResetAmpNumber(m_nSequence);
                        m_WorkerThread = new Thread(new ThreadStart(AcceptImageThread));
                        m_WorkerThread.Start();
                    }
                }
                else
                {
                    m_nDiagnosticCode = 1;
                    SetControlText(textMessage, "Failed, please repeat!" );
                    textDiagnostic.ForeColor = Color.Red;
                    SetControlText(textDiagnostic, m_devFP.GetErrorMessage() );
                    SetOneButtonState(buttonAccept, false);
                    SetOneButtonState(buttonStop, false);
                    SetOneButtonState(buttonRepeat, true);
                }
            }
	        else if( nAction == 1)
	        {
                SetControlText(textMessage, "Verifying......" );
                if(m_devFP.VerifyImage(m_nSequence))
                {
                    SetButtonState(true);
                    SetControlText(textMessage,  "Rolling Successful!" );
                    String strDiag = m_devFP.GetDiagnostic(ref m_nDiagnosticCode);
                    if (m_nDiagnosticCode != 0)
                        textDiagnostic.ForeColor = Color.Red;
                    else
                        textDiagnostic.ForeColor = Color.Black;
                    SetControlText(textDiagnostic,strDiag);
                    if (m_bAutoAccept && m_nDiagnosticCode == 0)
                    {
                        m_devFP.ResetAmpNumber(m_nSequence);
                        m_WorkerThread = new Thread(new ThreadStart(AcceptImageThread));
                        m_WorkerThread.Start();
                    }
                }
                else
                {
                    m_nDiagnosticCode = 1;
                    SetControlText(textMessage, "Failed, please repeat!" );
                    textDiagnostic.ForeColor = Color.Red;
                    SetControlText(textDiagnostic, m_devFP.GetErrorMessage() );
                    SetOneButtonState(buttonAccept, false);
                    SetOneButtonState(buttonStop, false);
                    SetOneButtonState(buttonRepeat, true);
                }
	        }
	        else if( nAction == 2)
	        {
                SetOneButtonState(buttonAccept, false);
	        }
	        else if( nAction == 3 )
	        {
                SetOneButtonState(buttonStop, true);
                if( !m_bIsReCapture )
                    SetOneButtonState(buttonSkip, true);
                SetOneButtonState(buttonBreak, true);
	        }
            else if( nAction == 4 )
            {
                SetControlText(textMessage, m_devFP.GetErrorMessage() );
                SetOneButtonState(buttonAccept, false);
                SetOneButtonState(buttonStop, false);
                SetOneButtonState(buttonRepeat, true);
            }
        }

        private void AcceptImageThread()
        {
            while (m_devFP.IsScanning())
            {
                Thread.Sleep(50);
            }
            AcceptImage();
        }

        private void ReloadBitmapAndShowMessage(FingerType ftFinger)
        {
            switch (ftFinger)
            {
                case FingerType.FT_LEFT_4_FINGERS:
                    SetControlText(textScanningFinger, "Left Four Fingers");
                    pictureScanningFinger.Image = new Bitmap(FS6xEnrollmentKit_CS.Properties.Resources.Left_4_Fingers);
                    break;
                case FingerType.FT_2_THUMBS:
                    SetControlText(textScanningFinger, "Two Thumbs");
                    pictureScanningFinger.Image = new Bitmap(FS6xEnrollmentKit_CS.Properties.Resources._2_Thumbs);
                    break;
                case FingerType.FT_RIGHT_4_FINGERS:
                    SetControlText(textScanningFinger, "Right Four Fingers");
                    pictureScanningFinger.Image = new Bitmap(FS6xEnrollmentKit_CS.Properties.Resources.Right_4_Fingers);
                    break;
                case FingerType.FT_LEFT_LITTLE:
                    SetControlText(textScanningFinger, "Flat Left Little Finger");
                    pictureScanningFinger.Image = new Bitmap(FS6xEnrollmentKit_CS.Properties.Resources.Left_Little);
                    break;
                case FingerType.FT_ROLLED_LEFT_LITTLE:
                    SetControlText(textScanningFinger, "Rolled Left Little Finger");
                    pictureScanningFinger.Image = new Bitmap(FS6xEnrollmentKit_CS.Properties.Resources.Left_Little);
                    break;
                case FingerType.FT_LEFT_RING:
                    SetControlText(textScanningFinger, "Flat Left Ring Finger");
                    pictureScanningFinger.Image = new Bitmap(FS6xEnrollmentKit_CS.Properties.Resources.Left_Ring);
                    break;
                case FingerType.FT_ROLLED_LEFT_RING:
                    SetControlText(textScanningFinger, "Rolled Left Ring Finger");
                    pictureScanningFinger.Image = new Bitmap(FS6xEnrollmentKit_CS.Properties.Resources.Left_Ring);
                    break;
                case FingerType.FT_LEFT_MIDDLE:
                    SetControlText(textScanningFinger, "Flat Left Middle Finger");
                    pictureScanningFinger.Image = new Bitmap(FS6xEnrollmentKit_CS.Properties.Resources.Left_Middle);
                    break;
                case FingerType.FT_ROLLED_LEFT_MIDDLE:
                    SetControlText(textScanningFinger, "Rolled Left Middle Finger");
                    pictureScanningFinger.Image = new Bitmap(FS6xEnrollmentKit_CS.Properties.Resources.Left_Middle);
                    break;
                case FingerType.FT_LEFT_INDEX:
                    SetControlText(textScanningFinger, "Flat Left Index Finger");
                    pictureScanningFinger.Image = new Bitmap(FS6xEnrollmentKit_CS.Properties.Resources.Left_Index);
                    break;
                case FingerType.FT_ROLLED_LEFT_INDEX:
                    SetControlText(textScanningFinger, "Rolled Left Index Finger");
                    pictureScanningFinger.Image = new Bitmap(FS6xEnrollmentKit_CS.Properties.Resources.Left_Index);
                    break;
                case FingerType.FT_LEFT_THUMB:
                    SetControlText(textScanningFinger, "Flat Left Thumb Finger");
                    pictureScanningFinger.Image = new Bitmap(FS6xEnrollmentKit_CS.Properties.Resources.Left_Thumb);
                    break;
                case FingerType.FT_ROLLED_LEFT_THUMB:
                    SetControlText(textScanningFinger, "Rolled Left Thumb Finger");
                    pictureScanningFinger.Image = new Bitmap(FS6xEnrollmentKit_CS.Properties.Resources.Left_Thumb);
                    break;
                case FingerType.FT_RIGHT_LITTLE:
                    SetControlText(textScanningFinger, "Flat Right Little Finger");
                    pictureScanningFinger.Image = new Bitmap(FS6xEnrollmentKit_CS.Properties.Resources.Right_Little);
                    break;
                case FingerType.FT_ROLLED_RIGHT_LITTLE:
                    SetControlText(textScanningFinger, "Rolled Right Little Finger");
                    pictureScanningFinger.Image = new Bitmap(FS6xEnrollmentKit_CS.Properties.Resources.Right_Little);
                    break;
                case FingerType.FT_RIGHT_RING:
                    SetControlText(textScanningFinger, "Flat Right Ring Finger");
                    pictureScanningFinger.Image = new Bitmap(FS6xEnrollmentKit_CS.Properties.Resources.Right_Ring);
                    break;
                case FingerType.FT_ROLLED_RIGHT_RING:
                    SetControlText(textScanningFinger, "Rolled Right Ring Finger");
                    pictureScanningFinger.Image = new Bitmap(FS6xEnrollmentKit_CS.Properties.Resources.Right_Ring);
                    break;
                case FingerType.FT_RIGHT_MIDDLE:
                    SetControlText(textScanningFinger, "Flat Right Middle Finger");
                    pictureScanningFinger.Image = new Bitmap(FS6xEnrollmentKit_CS.Properties.Resources.Right_Middle);
                    break;
                case FingerType.FT_ROLLED_RIGHT_MIDDLE:
                    SetControlText(textScanningFinger, "Rolled Right Middle Finger");
                    pictureScanningFinger.Image = new Bitmap(FS6xEnrollmentKit_CS.Properties.Resources.Right_Middle);
                    break;
                case FingerType.FT_RIGHT_INDEX:
                    SetControlText(textScanningFinger, "Flat Right Index Finger");
                    pictureScanningFinger.Image = new Bitmap(FS6xEnrollmentKit_CS.Properties.Resources.Right_Index);
                    break;
                case FingerType.FT_ROLLED_RIGHT_INDEX:
                    SetControlText(textScanningFinger, "Rolled Right Index Finger");
                    pictureScanningFinger.Image = new Bitmap(FS6xEnrollmentKit_CS.Properties.Resources.Right_Index);
                    break;
                case FingerType.FT_RIGHT_THUMB:
                    SetControlText(textScanningFinger, "Flat Right Thumb Finger");
                    pictureScanningFinger.Image = new Bitmap(FS6xEnrollmentKit_CS.Properties.Resources.Right_Thumb);
                    break;
                case FingerType.FT_ROLLED_RIGHT_THUMB:
                    SetControlText(textScanningFinger, "Rolled Right Thumb Finger");
                    pictureScanningFinger.Image = new Bitmap(FS6xEnrollmentKit_CS.Properties.Resources.Right_Thumb);
                    break;
                case FingerType.FT_PLAIN_LEFT_THUMB:
                    SetControlText(textScanningFinger, "Plain Left Thumb Finger");
                    pictureScanningFinger.Image = new Bitmap(FS6xEnrollmentKit_CS.Properties.Resources.Left_Thumb);
                    break;
                case FingerType.FT_PLAIN_RIGHT_THUMB:
                    SetControlText(textScanningFinger, "Plain Right Thumb Finger");
                    pictureScanningFinger.Image = new Bitmap(FS6xEnrollmentKit_CS.Properties.Resources.Right_Thumb);
                    break;
                default:
                    SetControlText(textScanningFinger, "");
                    pictureScanningFinger.Image = new Bitmap(FS6xEnrollmentKit_CS.Properties.Resources.Hands_empty);
                    break;
            }
        }

        private void buttonAccept_Click(object sender, EventArgs e)
        {
            buttonAccept.Enabled = false;
            buttonStop.Enabled = false;
            if (m_bCaptureOrAccept)	// TRUE - Capture, FALSE - Accept
            {
                m_devFP.SetPreviewMode(false);  //change to capture mode
            }
            else
            {
                buttonAccept.Enabled = false;
                textDiagnostic.Text = "";
                if (m_bAskUnavailabilityReason)
                {
                    int nUnavailableFingers = 0;
                    bool bUnavailable = m_devFP.IsUnavailable(ref nUnavailableFingers);
                    m_devFP.ResetAmpNumber(m_nSequence);
                    if (bUnavailable)
                    {
                        FormUnavailabilityReason frmReason = new FormUnavailabilityReason();
                        frmReason.Initialize(m_devFP, m_nSequence, nUnavailableFingers);
                        frmReason.ShowDialog();
                    }
                }
                AcceptImage();
            }
        }

        private void AcceptImage()
        {
            m_devFP.SaveAcceptedImage(m_nSequence);
            m_nSequence++;
            if (m_nSequence == FingerType.FT_RIGHT_4_FINGERS && ((m_nScanType & FPInfo.DEVICE_SCAN_TYPE_2THUMBS) != 0))
                m_nSequence++;	//Skip the RIGHT_4_FINGERS
            if (m_nSequence == FingerType.FT_LEFT_LITTLE && ((m_nScanType & FPInfo.DEVICE_SCAN_TYPE_FLAT_FINGER) == 0))
                m_nSequence = FingerType.FT_ROLLED_LEFT_LITTLE;
            if (m_nSequence == FingerType.FT_ROLLED_LEFT_LITTLE && ((m_nScanType & FPInfo.DEVICE_SCAN_TYPE_ROLLED_FINGER) == 0))
                m_nSequence = FingerType.FT_ROLLED_RIGHT_LITTLE + 1;
            if (m_nSequence == FingerType.FT_PLAIN_LEFT_THUMB)
                if (((m_nScanType & FPInfo.DEVICE_SCAN_TYPE_FLAT_FINGER) != 0) || (((m_nScanType & FPInfo.DEVICE_SCAN_TYPE_2THUMBS) == 0)
                        && ((m_nScanType & FPInfo.DEVICE_SCAN_TYPE_SLAPS) == 0)))
                    m_nSequence = FingerType.FT_PLAIN_RIGHT_THUMB + 1;		//out of sequence range

            if (m_nSequence >= FingerType.FT_ROLLED_LEFT_LITTLE)
            {
                FingerType nSeq = m_nSequence;
                for (FingerType i = nSeq; i <= FingerType.FT_PLAIN_RIGHT_THUMB; i++)
                {
                    if (!m_devFP.IsFingerUnavailable(i))
                        break;
                    else
                        m_nSequence++;
                }
            }

            if (m_bIsReCapture)
                m_nSequence = FingerType.FT_PLAIN_RIGHT_THUMB + 1;		//out of sequence range

            StartScanning();
        }

        private void buttonRepeat_Click(object sender, EventArgs e)
        {
            StartScanning();
        }

        private void buttonSkip_Click(object sender, EventArgs e)
        {
            buttonSkip.Enabled = false;
            m_devFP.Stop();
            if (m_bAskUnavailabilityReason)
            {
                int nUnavailableFingers = 0;
                if (m_nSequence == 0 || m_nSequence == FingerType.FT_RIGHT_4_FINGERS )
                    nUnavailableFingers = 4;
                else if (m_nSequence == FingerType.FT_2_THUMBS)
                    nUnavailableFingers = 2;
                else if (m_nSequence >= FingerType.FT_ROLLED_LEFT_LITTLE && m_nSequence < FingerType.FT_PLAIN_RIGHT_THUMB)
                    nUnavailableFingers = 1;
                if (nUnavailableFingers > 0)
                {
                    FormUnavailabilityReason frmReason = new FormUnavailabilityReason();
                    frmReason.Initialize(m_devFP, m_nSequence, nUnavailableFingers);
                    frmReason.ShowDialog();
                }
            }
            m_nSequence++;
            if (m_nSequence == FingerType.FT_RIGHT_4_FINGERS && ((m_nScanType & FPInfo.DEVICE_SCAN_TYPE_2THUMBS) != 0))
                m_nSequence++;	//Skip the RIGHT_4_FINGERS
            if (m_nSequence == FingerType.FT_LEFT_LITTLE && ((m_nScanType & FPInfo.DEVICE_SCAN_TYPE_FLAT_FINGER) == 0))
                m_nSequence = FingerType.FT_ROLLED_LEFT_LITTLE;
            if (m_nSequence == FingerType.FT_ROLLED_LEFT_LITTLE && ((m_nScanType & FPInfo.DEVICE_SCAN_TYPE_ROLLED_FINGER) == 0))
                m_nSequence = FingerType.FT_ROLLED_RIGHT_LITTLE + 1;
            if (m_nSequence == FingerType.FT_PLAIN_LEFT_THUMB)
                if (((m_nScanType & FPInfo.DEVICE_SCAN_TYPE_FLAT_FINGER) != 0) || (((m_nScanType & FPInfo.DEVICE_SCAN_TYPE_2THUMBS) == 0) && ((m_nScanType & FPInfo.DEVICE_SCAN_TYPE_SLAPS) == 0)))
                    m_nSequence = FingerType.FT_PLAIN_RIGHT_THUMB + 1;		//out of sequence range
            buttonSkip.Enabled = true;
            StartScanning();                
        }

        private void buttonBreak_Click(object sender, EventArgs e)
        {
            if (!m_bExit)
            {
                DialogResult nResponse = MessageBox.Show( "Are you sure to Break the processing?", 
                    "Acquision", MessageBoxButtons.YesNo );
                if (nResponse != DialogResult.Yes )
                    return;
            }
            if (m_devFP != null)
            {
                m_devFP.Stop();
                m_devFP.TurnOffLed();
                m_devFP.Close();
            }
            this.Close();
        }

        private void comboACTL_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_devFP.SetACTL((byte)(comboACTL.SelectedIndex + 1));
        }

        private void textMatchScoreThreshold_TextChanged(object sender, EventArgs e)
        {
            int nThreshold = int.Parse(textMatchScoreThreshold.Text);
            if( nThreshold >= 1 && nThreshold <= 100 )
                FPVerify.m_nThreshold = nThreshold;
        }
    }
}
