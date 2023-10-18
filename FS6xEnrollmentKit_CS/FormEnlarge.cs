using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FS6xEnrollmentKit_CS
{
    public partial class FormEnlarge : Form
    {
        private FingerType m_ftShow;
        private FPDevice m_devFP = null;

        public FormEnlarge()
        {
            InitializeComponent();
            m_ftShow = FingerType.FT_LEFT_4_FINGERS;	//0
            EnableControlButton();
        }

        public void Initialize(FPDevice devFP, FingerType ftView)
        {
            m_devFP = devFP;
            m_ftShow = ftView;
            ShowImage();
            EnableControlButton();
        }

        private void ShowImage()
        {
            if (m_devFP == null)
                return;
            ShowFingerMsg();
            m_devFP.ShowAcceptedImage(m_ftShow, pictureImage, true);
        }

        private void ShowFingerMsg()
        {
            switch (m_ftShow)
            {
                case FingerType.FT_LEFT_4_FINGERS:
                    labelMessage.Text = "Left Four Fingers";
                    break;
                case FingerType.FT_2_THUMBS:
                    labelMessage.Text = "Two Thumbs";
                    break;
                case FingerType.FT_RIGHT_4_FINGERS:
                    labelMessage.Text = "Right Four Fingers";
                    break;
                case FingerType.FT_LEFT_LITTLE:
                    labelMessage.Text = "Flat Left Little Finger";
                    break;
                case FingerType.FT_LEFT_RING:
                    labelMessage.Text = "Flat Left Ring Finger";
                    break;
                case FingerType.FT_LEFT_MIDDLE:
                    labelMessage.Text = "Flat Left Middle Finger";
                    break;
                case FingerType.FT_LEFT_INDEX:
                    labelMessage.Text = "Flat Left Index Finger";
                    break;
                case FingerType.FT_LEFT_THUMB:
                    labelMessage.Text = "Flat Left Thumb Finger";
                    break;
                case FingerType.FT_RIGHT_THUMB:
                    labelMessage.Text = "Flat Right Thumb Finger";
                    break;
                case FingerType.FT_RIGHT_INDEX:
                    labelMessage.Text = "Flat Right Index Finger";
                    break;
                case FingerType.FT_RIGHT_MIDDLE:
                    labelMessage.Text = "Flat Right Middle Finger";
                    break;
                case FingerType.FT_RIGHT_RING:
                    labelMessage.Text = "Flat Right Ring Finger";
                    break;
                case FingerType.FT_RIGHT_LITTLE:
                    labelMessage.Text = "Flat Right Little Finger";
                    break;
                case FingerType.FT_ROLLED_LEFT_LITTLE:
                    labelMessage.Text = "Rolled Left Little Finger";
                    break;
                case FingerType.FT_ROLLED_LEFT_RING:
                    labelMessage.Text = "Rolled Left Ring Finger";
                    break;
                case FingerType.FT_ROLLED_LEFT_MIDDLE:
                    labelMessage.Text = "Rolled Left Middle Finger";
                    break;
                case FingerType.FT_ROLLED_LEFT_INDEX:
                    labelMessage.Text = "Rolled Left Index Finger";
                    break;
                case FingerType.FT_ROLLED_LEFT_THUMB:
                    labelMessage.Text = "Rolled Left Thumb Finger";
                    break;
                case FingerType.FT_ROLLED_RIGHT_THUMB:
                    labelMessage.Text = "Rolled Right Thumb Finger";
                    break;
                case FingerType.FT_ROLLED_RIGHT_INDEX:
                    labelMessage.Text = "Rolled Right Index Finger";
                    break;
                case FingerType.FT_ROLLED_RIGHT_MIDDLE:
                    labelMessage.Text = "Rolled Right Middle Finger";
                    break;
                case FingerType.FT_ROLLED_RIGHT_RING:
                    labelMessage.Text = "Rolled Right Ring Finger";
                    break;
                case FingerType.FT_ROLLED_RIGHT_LITTLE:
                    labelMessage.Text = "Rolled Right Little Finger";
                    break;
                case FingerType.FT_PLAIN_LEFT_THUMB:
                    labelMessage.Text = "Plain Left Thumb Finger";
                    break;
                case FingerType.FT_PLAIN_RIGHT_THUMB:
                    labelMessage.Text = "Plain Right Thumb Finger";
                    break;
                default:
                    break;
            }
        }

        private void EnableControlButton()
        {
            if (m_ftShow == FingerType.FT_LEFT_4_FINGERS)
            {
                buttonBegin.Enabled = false;
                buttonPrevious.Enabled = false;
                buttonNext.Enabled = true;
                buttonEnd.Enabled = true;
            }
            else if (m_ftShow == FingerType.FT_PLAIN_RIGHT_THUMB)
            {
                buttonBegin.Enabled = true;
                buttonPrevious.Enabled = true;
                buttonNext.Enabled = false;
                buttonEnd.Enabled = false;
            }
            else
            {
                buttonBegin.Enabled = true;
                buttonPrevious.Enabled = true;
                buttonNext.Enabled = true;
                buttonEnd.Enabled = true;
            }
        }

        private void buttonBegin_Click(object sender, EventArgs e)
        {
            m_ftShow = FingerType.FT_LEFT_4_FINGERS;
            ShowImage();
            EnableControlButton();
        }

        private void buttonPrevious_Click(object sender, EventArgs e)
        {
            if (m_ftShow > FingerType.FT_LEFT_4_FINGERS)
            {
                m_ftShow = m_ftShow - 1;
                ShowImage();
            }
            EnableControlButton();
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (m_ftShow < FingerType.FT_PLAIN_RIGHT_THUMB)
            {
                m_ftShow = m_ftShow + 1;
                ShowImage();
            }
            EnableControlButton();
        }

        private void buttonEnd_Click(object sender, EventArgs e)
        {
            m_ftShow = FingerType.FT_PLAIN_RIGHT_THUMB;
	        ShowImage();
            EnableControlButton();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
