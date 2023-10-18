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
    public partial class FormUnavailabilityReason : Form
    {
        private FingerType m_ftFinger;
        private int m_nNumberOfUnavailability;
        private FS6xEnrollmentKit_CS.AcceptedImage.FINGER_AMP[] m_faFinger = null;
        private FPDevice m_devFP = null;

        public FormUnavailabilityReason()
        {
            InitializeComponent();
        }

        public void Initialize(FPDevice devFP, FingerType ftFinger, int nUnavailableFingers)
        {
            m_devFP = devFP;
            m_ftFinger = ftFinger;
            m_nNumberOfUnavailability = nUnavailableFingers;
            InitControls();
        }

        private void InitControls()
        {
            m_faFinger = new FS6xEnrollmentKit_CS.AcceptedImage.FINGER_AMP[4];
            for (int i = 0; i < 4; i++)
                m_faFinger[i] = new FS6xEnrollmentKit_CS.AcceptedImage.FINGER_AMP();
            switch (m_ftFinger)
            {
                case FingerType.FT_LEFT_4_FINGERS:
                    SetControls("Left Little", "Left Ring", "Left Middle", "Left Index");
                    break;
                case FingerType.FT_2_THUMBS:
                    SetControls("Left Thumb", "Right Thumb", "", "");
                    break;
                case FingerType.FT_RIGHT_4_FINGERS:
                    SetControls("Right Index", "Right Middle", "Right Ring", "Right Little");
                    break;
                case FingerType.FT_ROLLED_LEFT_LITTLE:
                    SetControls("Left Little", "", "", "");
                    break;
                case FingerType.FT_ROLLED_LEFT_RING:
                    SetControls("Left Ring", "", "", "");
                    break;
                case FingerType.FT_ROLLED_LEFT_MIDDLE:
                    SetControls("Left Middle", "", "", "");
                    break;
                case FingerType.FT_ROLLED_LEFT_INDEX:
                    SetControls("Left Index", "", "", "");
                    break;
                case FingerType.FT_ROLLED_LEFT_THUMB:
                    SetControls("Left Thumb", "", "", "");
                    break;
                case FingerType.FT_ROLLED_RIGHT_THUMB:
                    SetControls("Right Thumb", "", "", "");
                    break;
                case FingerType.FT_ROLLED_RIGHT_INDEX:
                    SetControls("Right Index", "", "", "");
                    break;
                case FingerType.FT_ROLLED_RIGHT_MIDDLE:
                    SetControls("Right Middle", "", "", "");
                    break;
                case FingerType.FT_ROLLED_RIGHT_RING:
                    SetControls("Right Ring", "", "", "");
                    break;
                case FingerType.FT_ROLLED_RIGHT_LITTLE:
                    SetControls("Right Little", "", "", "");
                    break;
                default:
                    break;
            }
        }

        private void SetControls(String strF1, String strF2, String strF3, String strF4)
        {
            if (!String.IsNullOrEmpty(strF1))
            {
                labelF1.Text = strF1;
                labelF1.Visible = true;
                comboF1.Visible = true;
            }
            if (!String.IsNullOrEmpty(strF2))
            {
                labelF2.Text = strF2;
                labelF2.Visible = true;
                comboF2.Visible = true;
            }
            if (!String.IsNullOrEmpty(strF3))
            {
                labelF3.Text = strF3;
                labelF3.Visible = true;
                comboF3.Visible = true;
            }
            if (!String.IsNullOrEmpty(strF4))
            {
                labelF4.Text = strF4;
                labelF4.Visible = true;
                comboF4.Visible = true;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            int nTotal = 0;
            int nIndex = comboF1.SelectedIndex;
            if (nIndex > 0)
            {
                m_faFinger[nTotal].FingerId = 0;
                m_faFinger[nTotal].AMPCode = (byte)nIndex;
                nTotal++;
            }
            nIndex = comboF2.SelectedIndex;
            if (nIndex > 0)
            {
                m_faFinger[nTotal].FingerId = 1;
                m_faFinger[nTotal].AMPCode = (byte)nIndex;
                nTotal++;
            }
            nIndex = comboF3.SelectedIndex;
            if (nIndex > 0)
            {
                m_faFinger[nTotal].FingerId = 2;
                m_faFinger[nTotal].AMPCode = (byte)nIndex;
                nTotal++;
            }
            nIndex = comboF4.SelectedIndex;
            if (nIndex > 0)
            {
                m_faFinger[nTotal].FingerId = 3;
                m_faFinger[nTotal].AMPCode = (byte)nIndex;
                nTotal++;
            }
            if (nTotal != m_nNumberOfUnavailability)
            {
                String strMsg;
                if (nTotal > m_nNumberOfUnavailability)
                {
                    strMsg = String.Format("Too Many Reasons!\r\nThe Unavailability Reason is {0}\r\nYou choose {1}\r\n", m_nNumberOfUnavailability, nTotal);
                    MessageBox.Show(strMsg, "Unavailability Reason", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                else
                {
                    strMsg = String.Format("Reasons is not enough!\r\nThe Unavailability Reason is {0}\r\nYou choose {1}\r\n", m_nNumberOfUnavailability, nTotal);
                    MessageBox.Show(strMsg, "Unavailability Reason", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }
            for (int i = 0; i < nTotal; i++)
                m_devFP.SetFingerAmpCode(m_ftFinger, m_faFinger[i].FingerId, m_faFinger[i].AMPCode);
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult nRet = MessageBox.Show( "Are you sure to cancel!", "Unavailability Reason", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (nRet == DialogResult.No)
                return;
            this.Close();
        }
    }
}
