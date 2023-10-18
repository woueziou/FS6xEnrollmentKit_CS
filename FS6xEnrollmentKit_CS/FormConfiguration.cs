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
    public partial class FormConfiguration : Form
    {
        public FormConfiguration()
        {
            InitializeComponent();
            comboStandard.SelectedIndex = 0;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int nSel = comboStandard.SelectedIndex;
            if (nSel == 1 || nSel == 2)
            {
                labelPlainThumb.Visible = false;
                textPTHeight.Visible = false;
                textPTWidth.Visible = false;
            }
            else
            {
                labelPlainThumb.Visible = true;
                textPTHeight.Visible = true;
                textPTWidth.Visible = true;
            }
        }
    }
}
