namespace FS6xEnrollmentKit_CS
{
    partial class FormRecordType
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRecordType));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.radioIso19794 = new System.Windows.Forms.RadioButton();
            this.radioAnsi381 = new System.Windows.Forms.RadioButton();
            this.radioType4 = new System.Windows.Forms.RadioButton();
            this.radioType14 = new System.Windows.Forms.RadioButton();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pictureBox2);
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Controls.Add(this.radioIso19794);
            this.groupBox1.Controls.Add(this.radioAnsi381);
            this.groupBox1.Controls.Add(this.radioType4);
            this.groupBox1.Controls.Add(this.radioType14);
            this.groupBox1.Location = new System.Drawing.Point(17, 17);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(366, 278);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Select the type of fingerprint image record";
            // 
            // pictureBox2
            // 
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox2.Location = new System.Drawing.Point(6, 209);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(350, 1);
            this.pictureBox2.TabIndex = 5;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(7, 143);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(350, 1);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // radioIso19794
            // 
            this.radioIso19794.AutoSize = true;
            this.radioIso19794.Location = new System.Drawing.Point(43, 230);
            this.radioIso19794.Name = "radioIso19794";
            this.radioIso19794.Size = new System.Drawing.Size(134, 21);
            this.radioIso19794.TabIndex = 3;
            this.radioIso19794.Text = "ISO/IEC 19794-4";
            this.radioIso19794.UseVisualStyleBackColor = true;
            this.radioIso19794.CheckedChanged += new System.EventHandler(this.radioIso19794_CheckedChanged);
            // 
            // radioAnsi381
            // 
            this.radioAnsi381.AutoSize = true;
            this.radioAnsi381.Location = new System.Drawing.Point(43, 166);
            this.radioAnsi381.Name = "radioAnsi381";
            this.radioAnsi381.Size = new System.Drawing.Size(172, 21);
            this.radioAnsi381.TabIndex = 2;
            this.radioAnsi381.Text = "ANSI/INCITS 381-2004";
            this.radioAnsi381.UseVisualStyleBackColor = true;
            this.radioAnsi381.CheckedChanged += new System.EventHandler(this.radioAnsi381_CheckedChanged);
            // 
            // radioType4
            // 
            this.radioType4.AutoSize = true;
            this.radioType4.Location = new System.Drawing.Point(43, 102);
            this.radioType4.Name = "radioType4";
            this.radioType4.Size = new System.Drawing.Size(221, 21);
            this.radioType4.TabIndex = 1;
            this.radioType4.Text = "ANSI/NIST-ITL 1-2007  Type 4";
            this.radioType4.UseVisualStyleBackColor = true;
            this.radioType4.CheckedChanged += new System.EventHandler(this.radioType4_CheckedChanged);
            // 
            // radioType14
            // 
            this.radioType14.AutoSize = true;
            this.radioType14.Location = new System.Drawing.Point(43, 58);
            this.radioType14.Name = "radioType14";
            this.radioType14.Size = new System.Drawing.Size(229, 21);
            this.radioType14.TabIndex = 0;
            this.radioType14.Text = "ANSI/NIST-ITL 1-2007  Type 14";
            this.radioType14.UseVisualStyleBackColor = true;
            this.radioType14.CheckedChanged += new System.EventHandler(this.radioType14_CheckedChanged);
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(88, 317);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 33);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(229, 317);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 33);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // FormRecordType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 368);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormRecordType";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select record type";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.RadioButton radioIso19794;
        private System.Windows.Forms.RadioButton radioAnsi381;
        private System.Windows.Forms.RadioButton radioType4;
        private System.Windows.Forms.RadioButton radioType14;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
    }
}