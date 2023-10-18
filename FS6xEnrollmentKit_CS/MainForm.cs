using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Futronic.MathAPIHelper;
using System.Runtime.InteropServices;

namespace FS6xEnrollmentKit_CS
{
    public partial class MainForm : Form
    {
        public FPDevice m_devFP;
        private bool m_bRefresh = false;
        private bool m_bAskUnavailabilityReason = false;
        private FingerType m_ftView;

        public MainForm()
        {
            InitializeComponent();
            m_devFP = new FPDevice();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            short nDeviceID = 0;
            DeviceInfo devInfo;
            VersionInfo versionInfo;
            Futronic.MathAPIHelper.Version versionMath;
            while (true)
            {
                if( m_devFP.Open() )
                {
                    versionInfo = m_devFP.m_hDevice.VersionInformation;
                    versionMath = m_devFP.m_hDevice.MathVersion;
                    devInfo = m_devFP.m_hDevice.Information;
                    string messageInfo = "Scanner: " + FPInfo.GetScannerName(devInfo.DeviceCompatibility, ref nDeviceID) + "\r\nScanAPI: " + versionInfo.APIVersion.ToString() + "\r\nFirmware: " + versionInfo.FirmwareVersion.ToString() +
                        "\r\nHardware: " + versionInfo.HardwareVersion.ToString() + "\r\nMathAPI: " + versionMath.ToString();
                    textScannerInfo.Text = messageInfo;
                    m_bAskUnavailabilityReason = false;
                    if (m_devFP.CanSlaps())
                    {
                        checkSlaps.Enabled = true;
                        checkSlaps.Checked = true;
                        checkSound.Enabled = true;
                        checkSound.Checked = true;
                        m_devFP.SetSound(true);
                        m_bAskUnavailabilityReason = true;
                    }
                    if (m_devFP.CanRoll())
                    {
                        checkRoll.Enabled = true;
                        checkRoll.Checked = true;
                        m_bAskUnavailabilityReason = true;
                    }
                    if (m_devFP.CanSegmentation())
                    {
                        checkSegmentation.Enabled = true;
                        checkSegmentation.Checked = true;
                        checkRotateSegment.Enabled = true;
                        checkAutoCapture.Enabled = true;
                        checkAutoCapture.Checked = true;
                        checkAutoAccept.Enabled = true;
                        m_devFP.SetAutoCapture(true);
                        m_bAskUnavailabilityReason = true;
                    }
                    else
                    {
                        checkFlatSingleFingers.Enabled = true;
                    }

                    if (m_bAskUnavailabilityReason)
                    {
                        checkUnavailabilityReason.Enabled = true;
                        checkUnavailabilityReason.Checked = true;
                    }
                    m_devFP.Close();
                    break;
                }
                else
                {
                    string strErrMsg = string.Format("Failed to open device!\nError : {0}\nPlease plug in the scanner and click 'OK'\n\n",m_devFP.GetErrorMessage());
                    DialogResult dialogResult = MessageBox.Show(strErrMsg, "Failed to open device", MessageBoxButtons.OKCancel);
                    if (dialogResult == DialogResult.Cancel)
                    {
                        break;
                    }
                }
            }
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                groupSlaps.Height = this.Height / 2 - 30;
                tabFP.Top = groupSlaps.Height + 15;
                tabFP.Height = groupSlaps.Height;
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            byte nScanType = 0;
            if (checkSlaps.Checked )
                nScanType |= FPInfo.DEVICE_SCAN_TYPE_SLAPS;		//FS60
            if (checkFlatSingleFingers.Checked)
                nScanType |= FPInfo.DEVICE_SCAN_TYPE_FLAT_FINGER;
            if (checkRoll.Checked)
                nScanType |= FPInfo.DEVICE_SCAN_TYPE_ROLLED_FINGER;
            if (checkSegmentation.Checked)
            {
                if (!m_devFP.CanSlaps())
                    nScanType |= FPInfo.DEVICE_SCAN_TYPE_2THUMBS;	//FS50
                m_devFP.SetSegmentation(true);
            }
            else
                m_devFP.SetSegmentation(false);
            if (nScanType == 0)
            {
                MessageBox.Show( "Please select fingers to acquire!", "Select finger", MessageBoxButtons.OK );
                return;
            }
            m_devFP.SetAngle(checkRotateSegment.Checked);
            m_devFP.SetAutoCapture(checkAutoCapture.Checked);
            m_devFP.SetSound(checkSound.Checked);
            m_devFP.FreeBuffer();
            //
            FormAcquisition frmAcquisition = new FormAcquisition(m_devFP);
            frmAcquisition.m_bAutoAccept = checkAutoAccept.Checked;
            frmAcquisition.m_bAskUnavailabilityReason = checkUnavailabilityReason.Checked;
            frmAcquisition.m_nScanType = nScanType;
            m_bRefresh = true;
            frmAcquisition.ShowDialog();
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
            if (m_devFP != null)
            {
                if (m_bRefresh)
                {
                    m_bRefresh = false;
                    ShowAcceptedImage();
                }
                buttonSave.Enabled = m_devFP.m_bIsImageSaved;
                buttonExport.Enabled = m_devFP.m_bIsImageSaved;
            }
        }

        private void ShowEnlargeImage()
        {
            FormEnlarge frmEnlarge = new FormEnlarge();
            frmEnlarge.Initialize(m_devFP, m_ftView);
            frmEnlarge.ShowDialog();
        }

        private void ReCaptureImage()
        {
            byte nScanType = 0;
            if (checkSlaps.Checked)
                nScanType |= FPInfo.DEVICE_SCAN_TYPE_SLAPS;		//FS60
            if (checkFlatSingleFingers.Checked)
                nScanType |= FPInfo.DEVICE_SCAN_TYPE_FLAT_FINGER;
            if (checkRoll.Checked)
                nScanType |= FPInfo.DEVICE_SCAN_TYPE_ROLLED_FINGER;
            if (checkSegmentation.Checked)
            {
                if (!m_devFP.CanSlaps())
                    nScanType |= FPInfo.DEVICE_SCAN_TYPE_2THUMBS;	//FS50
                m_devFP.SetSegmentation(true);
            }
            else
                m_devFP.SetSegmentation(false);
            if (nScanType == 0)
            {
                MessageBox.Show("Please select fingers to acquire!", "Select finger", MessageBoxButtons.OK);
                return;
            }
            m_devFP.SetAngle(checkRotateSegment.Checked);
            m_devFP.SetAutoCapture(checkAutoCapture.Checked);
            m_devFP.SetSound(checkSound.Checked);
            FormAcquisition frmAcquisition = new FormAcquisition(m_devFP);
            frmAcquisition.m_bAutoAccept = checkAutoAccept.Checked;
            frmAcquisition.m_bIsReCapture = true;
            frmAcquisition.m_bAskUnavailabilityReason = checkUnavailabilityReason.Checked;
            frmAcquisition.m_nScanType = nScanType;
            frmAcquisition.m_nSequence = m_ftView;
            m_bRefresh = true;
            frmAcquisition.ShowDialog();
        }

        private void ShowAcceptedImage()
        {
            m_devFP.ShowAcceptedImage(FingerType.FT_LEFT_4_FINGERS, pictureLeft4Fingers);
            m_devFP.ShowAcceptedImage(FingerType.FT_2_THUMBS, picture2Thumbs);
            m_devFP.ShowAcceptedImage(FingerType.FT_RIGHT_4_FINGERS, pictureRight4Fingers);
            m_devFP.ShowAcceptedImage(FingerType.FT_LEFT_LITTLE, pictureFlatLeftLittle);
            m_devFP.ShowAcceptedImage(FingerType.FT_LEFT_RING, pictureFlatLeftRing);
            m_devFP.ShowAcceptedImage(FingerType.FT_LEFT_MIDDLE, pictureFlatLeftMiddle);
            m_devFP.ShowAcceptedImage(FingerType.FT_LEFT_INDEX, pictureFlatLeftIndex);
            m_devFP.ShowAcceptedImage(FingerType.FT_LEFT_THUMB, pictureFlatLeftThumb);
            m_devFP.ShowAcceptedImage(FingerType.FT_RIGHT_THUMB, pictureFlatRightThumb);
            m_devFP.ShowAcceptedImage(FingerType.FT_RIGHT_INDEX, pictureFlatRightIndex);
            m_devFP.ShowAcceptedImage(FingerType.FT_RIGHT_MIDDLE, pictureFlatRightMiddle);
            m_devFP.ShowAcceptedImage(FingerType.FT_RIGHT_RING, pictureFlatRightRing);
            m_devFP.ShowAcceptedImage(FingerType.FT_RIGHT_LITTLE, pictureFlatRightLittle);
            m_devFP.ShowAcceptedImage(FingerType.FT_ROLLED_LEFT_LITTLE, pictureRolledLeftLittle);
            m_devFP.ShowAcceptedImage(FingerType.FT_ROLLED_LEFT_RING, pictureRolledLeftRing);
            m_devFP.ShowAcceptedImage(FingerType.FT_ROLLED_LEFT_MIDDLE, pictureRolledLeftMiddle);
            m_devFP.ShowAcceptedImage(FingerType.FT_ROLLED_LEFT_INDEX, pictureRolledLeftIndex);
            m_devFP.ShowAcceptedImage(FingerType.FT_ROLLED_LEFT_THUMB, pictureRolledLeftThumb);
            m_devFP.ShowAcceptedImage(FingerType.FT_ROLLED_RIGHT_THUMB, pictureRolledRightThumb);
            m_devFP.ShowAcceptedImage(FingerType.FT_ROLLED_RIGHT_INDEX, pictureRolledRightIndex);
            m_devFP.ShowAcceptedImage(FingerType.FT_ROLLED_RIGHT_MIDDLE, pictureRolledRightMiddle);
            m_devFP.ShowAcceptedImage(FingerType.FT_ROLLED_RIGHT_RING, pictureRolledRightRing);
            m_devFP.ShowAcceptedImage(FingerType.FT_ROLLED_RIGHT_LITTLE, pictureRolledRightLittle);
            m_devFP.ShowAcceptedImage(FingerType.FT_PLAIN_LEFT_THUMB, picturePlainLeftThumb);
            m_devFP.ShowAcceptedImage(FingerType.FT_PLAIN_RIGHT_THUMB, picturePlainRightThumb);
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {            
            this.Close();
        }

        private void checkFlatSingleFingers_Click(object sender, EventArgs e)
        {
            if (checkSegmentation.Checked)
            {
                String strMsg = "The Segmentation option is chosen.\n\nIf you choose Flat Fingers, the Segmentation option will be disabled.\n\n";
                DialogResult nRet = MessageBox.Show(strMsg, "Warning", MessageBoxButtons.YesNo);
                if (nRet == DialogResult.Yes)
                {
                    checkSegmentation.Checked = false;
                    checkRotateSegment.Checked = false;
                    checkAutoCapture.Checked = false;
                    checkAutoCapture.Enabled = false;
                    checkAutoAccept.Checked = false;
                    checkAutoAccept.Enabled = false;
                }
                else
                {
                    checkFlatSingleFingers.Checked = false;
                    checkAutoCapture.Enabled = true;
                    checkAutoAccept.Enabled = true;
                }
            }    
        }

        private void checkSegmentation_Click(object sender, EventArgs e)
        {
            if (checkFlatSingleFingers.Checked)
            {
                String strMsg = "The Flat Fingers option is chosen.\n\nIf you choose Segmentation, the Flat Fingers will be disabled.\n\n";
                DialogResult nRet = MessageBox.Show(strMsg, "Warning", MessageBoxButtons.YesNo);
                if (nRet == DialogResult.Yes)
                {
                    checkFlatSingleFingers.Checked = false;
                    checkAutoCapture.Enabled = true;
                    checkAutoAccept.Enabled = true;
                }
                else
                    checkSegmentation.Checked = false;
            }    
        }

        private void checkAutoCapture_Click(object sender, EventArgs e)
        {
            checkAutoAccept.Enabled = checkAutoCapture.Checked;
            if (!checkAutoCapture.Checked)
                checkAutoAccept.Checked = false;
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip ctxMenu = (ContextMenuStrip)sender;
            string strControlName = ctxMenu.SourceControl.Name;
            if (strControlName.Contains("pictureFlat"))
                reCaptureToolStripMenuItem.Enabled = false;
            else
                reCaptureToolStripMenuItem.Enabled = true;
            CheckFingerType(strControlName);
        }

        private void CheckFingerType(string strControlName)
        {
            switch (strControlName)
            {
                case "pictureLeft4Fingers":
                    m_ftView = FingerType.FT_LEFT_4_FINGERS;
                    break;
                case "picture2Thumbs":
                    m_ftView = FingerType.FT_2_THUMBS;
                    break;
                case "pictureRight4Fingers":
                    m_ftView = FingerType.FT_RIGHT_4_FINGERS;
                    break;
                case "pictureFlatLeftThumb":
                    m_ftView = FingerType.FT_LEFT_THUMB;
                    break;
                case "pictureRolledLeftThumb":
                    m_ftView = FingerType.FT_ROLLED_LEFT_THUMB;
                    break;
                case "pictureFlatLeftIndex":
                    m_ftView = FingerType.FT_LEFT_INDEX;
                    break;
                case "pictureRolledLeftIndex":
                    m_ftView = FingerType.FT_ROLLED_LEFT_INDEX;
                    break;
                case "pictureFlatLeftMiddle":
                    m_ftView = FingerType.FT_LEFT_MIDDLE;
                    break;
                case "pictureRolledLeftMiddle":
                    m_ftView = FingerType.FT_ROLLED_LEFT_MIDDLE;
                    break;
                case "pictureFlatLeftRing":
                    m_ftView = FingerType.FT_LEFT_RING;
                    break;
                case "pictureRolledLeftRing":
                    m_ftView = FingerType.FT_ROLLED_LEFT_RING;
                    break;
                case "pictureFlatLeftLittle":
                    m_ftView = FingerType.FT_LEFT_LITTLE;
                    break;
                case "pictureRolledLeftLittle":
                    m_ftView = FingerType.FT_ROLLED_LEFT_LITTLE;
                    break;
                case "pictureFlatRightThumb":
                    m_ftView = FingerType.FT_RIGHT_THUMB;
                    break;
                case "pictureRolledRightThumb":
                    m_ftView = FingerType.FT_ROLLED_RIGHT_THUMB;
                    break;
                case "pictureFlatRightIndex":
                    m_ftView = FingerType.FT_RIGHT_INDEX;
                    break;
                case "pictureRolledRightIndex":
                    m_ftView = FingerType.FT_ROLLED_RIGHT_INDEX;
                    break;
                case "pictureFlatRightMiddle":
                    m_ftView = FingerType.FT_RIGHT_MIDDLE;
                    break;
                case "pictureRolledRightMiddle":
                    m_ftView = FingerType.FT_ROLLED_RIGHT_MIDDLE;
                    break;
                case "pictureFlatRightRing":
                    m_ftView = FingerType.FT_RIGHT_RING;
                    break;
                case "pictureRolledRightRing":
                    m_ftView = FingerType.FT_ROLLED_RIGHT_RING;
                    break;
                case "pictureFlatRightLittle":
                    m_ftView = FingerType.FT_RIGHT_LITTLE;
                    break;
                case "pictureRolledRightLittle":
                    m_ftView = FingerType.FT_ROLLED_RIGHT_LITTLE;
                    break;
                case "picturePlainLeftThumb":
                    m_ftView = FingerType.FT_PLAIN_LEFT_THUMB;
                    break;
                case "picturePlainRightThumb":
                    m_ftView = FingerType.FT_PLAIN_RIGHT_THUMB;
                    break;
            }
        }

        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowEnlargeImage();
        }

        private void pictureBox_DoubleClick(object sender, EventArgs e)
        {
            PictureBox picControl = (PictureBox)sender;
            string strControlName = picControl.Name;
            CheckFingerType(strControlName);
            ShowEnlargeImage();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            FormSaveFolder frmFolder = new FormSaveFolder();
            DialogResult nRet = frmFolder.ShowDialog();
            if (nRet == DialogResult.OK)
            {
                buttonStart.Enabled = false;
                buttonConfiguration.Enabled = false;
                buttonSave.Enabled = false;
                buttonExport.Enabled = false;
                bool[] bFormats = new bool[2];
                String strFullQualifiedFolderName = frmFolder.GetFullQualifiedFolderName(bFormats);
                String strFolderName = frmFolder.GetFolderName();

                if (m_devFP.SaveAcceptedImageToFile(strFullQualifiedFolderName, strFolderName, bFormats[0], bFormats[1]))
                {
                    String strMsg = "The finger images are saved to the folder:\r\n";
                    strMsg += strFullQualifiedFolderName;
                    MessageBox.Show(strMsg);
                }
                buttonStart.Enabled = true;
                buttonConfiguration.Enabled = true;
                buttonSave.Enabled = true;
                buttonExport.Enabled = true;
            }
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            FormRecordType frmRec = new FormRecordType();
            DialogResult nRet = frmRec.ShowDialog();
            if (nRet == DialogResult.OK)
            {
                byte recordType = frmRec.GetRecordType();
                if (recordType == 0)
                    return;
                SaveFileDialog dlgSave = new SaveFileDialog();
                dlgSave.Filter = "binary files (*.bin)|*.bin";
                if (dlgSave.ShowDialog() == DialogResult.OK)
                {
                    String strFileName = dlgSave.FileName;
                    if (!strFileName.EndsWith(".bin"))
                        strFileName += ".bin";
                    buttonStart.Enabled = false;
                    buttonConfiguration.Enabled = false;
                    buttonSave.Enabled = false;
                    buttonExport.Enabled = false;
                    if (m_devFP.ExportToEFTS(strFileName, recordType))
                        MessageBox.Show( "Exoprt to EFTS successfully! \r\n\t" + strFileName, "Export", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    else
                        MessageBox.Show("Failed to export to ETFS!", "Export", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    buttonStart.Enabled = true;
                    buttonConfiguration.Enabled = true;
                    buttonSave.Enabled = true;
                    buttonExport.Enabled = true;
                }
            }
        }

        private void reCaptureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReCaptureImage();
        }

        private void buttonConfiguration_Click(object sender, EventArgs e)
        {
            FormConfiguration frmConf = new FormConfiguration();
            frmConf.ShowDialog();
        }
    }
}
