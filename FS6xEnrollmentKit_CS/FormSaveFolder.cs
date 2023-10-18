using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace FS6xEnrollmentKit_CS
{
    public partial class FormSaveFolder : Form
    {
        private String m_strFullQualifiedFolderName;

        public FormSaveFolder()
        {
            InitializeComponent();
            m_strFullQualifiedFolderName = null;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            String strFolderName = textFolderName.Text;
            if (String.IsNullOrEmpty(strFolderName))
            {
                MessageBox.Show("Please input the folder name.", "Folder name", MessageBoxButtons.OK);
                return;
            }
            if (strFolderName.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) != -1)
            {
                MessageBox.Show("Invalid folder name!", "Folder name", MessageBoxButtons.OK);
                return;
            }
            if (!checkBmp.Checked && !checkWsq.Checked)
            {
                MessageBox.Show("Please select the file format!", "Folder name", MessageBoxButtons.OK);
                return;
            }

            String path = Environment.CurrentDirectory + "\\" + strFolderName + "\\";
            if (File.Exists(path))
            {
                MessageBox.Show("File name exists! Please enter another name!", "Folder name", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (Directory.Exists(path))
            {
                DialogResult nRet = MessageBox.Show("The folder existed!\r\n\r\nDo you want to delete the files inside the folder?\r\n",
                    "Folder name", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (nRet == DialogResult.No)
                    return;
                if (checkBmp.Checked)
                {
                    String[] bitmapFiles = Directory.GetFiles(path, "*.bmp");
                    foreach (String bmpFile in bitmapFiles)
                        File.Delete(bmpFile);
                }
                if (checkWsq.Checked)
                {
                    String[] wsqFiles = Directory.GetFiles(path, "*.bmp");
                    foreach (String wsqFile in wsqFiles)
                        File.Delete(wsqFile);
                }
            }
            else
                Directory.CreateDirectory(path);
            m_strFullQualifiedFolderName = path;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            m_strFullQualifiedFolderName = null;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        public String GetFullQualifiedFolderName(bool[] bImageFormat)
        {
            bImageFormat[0] = checkBmp.Checked;
            bImageFormat[1] = checkWsq.Checked;
            return m_strFullQualifiedFolderName;
        }

        public String GetFolderName()
        {
            return textFolderName.Text;
        }
    }
}
