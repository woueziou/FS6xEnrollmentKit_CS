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
    public partial class FormRecordType : Form
    {
        private byte m_nRecordType;

        public FormRecordType()
        {
            InitializeComponent();
            m_nRecordType = 0;
            radioType14.Checked = false;
        }

        public byte GetRecordType()
        {
            return m_nRecordType;
        }

        private void radioType14_CheckedChanged(object sender, EventArgs e)
        {
            m_nRecordType = FPInfo.TYPE_ANSI_NIST_ITL_1_2007_14;
        }

        private void radioType4_CheckedChanged(object sender, EventArgs e)
        {
            m_nRecordType = FPInfo.TYPE_ANSI_NIST_ITL_1_2007_4;
        }

        private void radioAnsi381_CheckedChanged(object sender, EventArgs e)
        {
            m_nRecordType = FPInfo.TYPE_ANSI_381_2004;
        }

        private void radioIso19794_CheckedChanged(object sender, EventArgs e)
        {
            m_nRecordType = FPInfo.TYPE_ISO_IEC_19794_4;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (m_nRecordType != FPInfo.TYPE_ANSI_NIST_ITL_1_2007_4 &&
                m_nRecordType != FPInfo.TYPE_ANSI_NIST_ITL_1_2007_14 &&
                m_nRecordType != FPInfo.TYPE_ANSI_381_2004 &&
                m_nRecordType != FPInfo.TYPE_ISO_IEC_19794_4)
            {
                MessageBox.Show("Please select one type!", "Record Type", MessageBoxButtons.OK);
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            m_nRecordType = 0;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
