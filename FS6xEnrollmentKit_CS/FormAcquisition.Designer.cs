namespace FS6xEnrollmentKit_CS
{
    partial class FormAcquisition
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAcquisition));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureAcq = new System.Windows.Forms.PictureBox();
            this.comboACTL = new System.Windows.Forms.ComboBox();
            this.pictureScanningFinger = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textMessage = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textMatchScoreThreshold = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonStop = new System.Windows.Forms.Button();
            this.textDiagnostic = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textScanningFinger = new System.Windows.Forms.TextBox();
            this.buttonBreak = new System.Windows.Forms.Button();
            this.buttonSkip = new System.Windows.Forms.Button();
            this.buttonRepeat = new System.Windows.Forms.Button();
            this.buttonAccept = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureAcq)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureScanningFinger)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::FS6xEnrollmentKit_CS.Properties.Resources.nfiq;
            this.pictureBox1.Location = new System.Drawing.Point(81, 43);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(296, 36);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // pictureAcq
            // 
            this.pictureAcq.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureAcq.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureAcq.Location = new System.Drawing.Point(421, 64);
            this.pictureAcq.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureAcq.Name = "pictureAcq";
            this.pictureAcq.Size = new System.Drawing.Size(603, 452);
            this.pictureAcq.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureAcq.TabIndex = 20;
            this.pictureAcq.TabStop = false;
            // 
            // comboACTL
            // 
            this.comboACTL.FormattingEnabled = true;
            this.comboACTL.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7"});
            this.comboACTL.Location = new System.Drawing.Point(261, 231);
            this.comboACTL.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboACTL.Name = "comboACTL";
            this.comboACTL.Size = new System.Drawing.Size(43, 24);
            this.comboACTL.TabIndex = 2;
            this.comboACTL.TabStop = false;
            this.comboACTL.SelectedIndexChanged += new System.EventHandler(this.comboACTL_SelectedIndexChanged);
            // 
            // pictureScanningFinger
            // 
            this.pictureScanningFinger.Image = global::FS6xEnrollmentKit_CS.Properties.Resources.Left_4_Fingers;
            this.pictureScanningFinger.Location = new System.Drawing.Point(72, 76);
            this.pictureScanningFinger.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureScanningFinger.Name = "pictureScanningFinger";
            this.pictureScanningFinger.Size = new System.Drawing.Size(260, 150);
            this.pictureScanningFinger.TabIndex = 1;
            this.pictureScanningFinger.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(61, 236);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(183, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Auto Capture Trigger Level:";
            // 
            // textMessage
            // 
            this.textMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textMessage.Location = new System.Drawing.Point(421, 14);
            this.textMessage.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textMessage.Name = "textMessage";
            this.textMessage.ReadOnly = true;
            this.textMessage.Size = new System.Drawing.Size(605, 30);
            this.textMessage.TabIndex = 19;
            this.textMessage.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textMatchScoreThreshold);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Location = new System.Drawing.Point(12, 459);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox3.Size = new System.Drawing.Size(399, 80);
            this.groupBox3.TabIndex = 18;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Sequence check setting";
            // 
            // textMatchScoreThreshold
            // 
            this.textMatchScoreThreshold.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textMatchScoreThreshold.Location = new System.Drawing.Point(249, 39);
            this.textMatchScoreThreshold.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textMatchScoreThreshold.Name = "textMatchScoreThreshold";
            this.textMatchScoreThreshold.Size = new System.Drawing.Size(38, 27);
            this.textMatchScoreThreshold.TabIndex = 1;
            this.textMatchScoreThreshold.TabStop = false;
            this.textMatchScoreThreshold.TextChanged += new System.EventHandler(this.textMatchScoreThreshold_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(196, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "Finger match score threshold:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "NFIQ:";
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(12, 14);
            this.buttonStop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(77, 36);
            this.buttonStop.TabIndex = 11;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // textDiagnostic
            // 
            this.textDiagnostic.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textDiagnostic.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textDiagnostic.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textDiagnostic.Location = new System.Drawing.Point(421, 532);
            this.textDiagnostic.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textDiagnostic.Multiline = true;
            this.textDiagnostic.Name = "textDiagnostic";
            this.textDiagnostic.Size = new System.Drawing.Size(605, 60);
            this.textDiagnostic.TabIndex = 21;
            this.textDiagnostic.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.pictureBox1);
            this.groupBox2.Location = new System.Drawing.Point(12, 356);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Size = new System.Drawing.Size(399, 84);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Image quality";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.comboACTL);
            this.groupBox1.Controls.Add(this.pictureScanningFinger);
            this.groupBox1.Controls.Add(this.textScanningFinger);
            this.groupBox1.Location = new System.Drawing.Point(12, 64);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(399, 273);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Fingers";
            // 
            // textScanningFinger
            // 
            this.textScanningFinger.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textScanningFinger.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textScanningFinger.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.textScanningFinger.Location = new System.Drawing.Point(21, 30);
            this.textScanningFinger.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textScanningFinger.Name = "textScanningFinger";
            this.textScanningFinger.ReadOnly = true;
            this.textScanningFinger.Size = new System.Drawing.Size(355, 30);
            this.textScanningFinger.TabIndex = 0;
            this.textScanningFinger.TabStop = false;
            this.textScanningFinger.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonBreak
            // 
            this.buttonBreak.Location = new System.Drawing.Point(333, 14);
            this.buttonBreak.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonBreak.Name = "buttonBreak";
            this.buttonBreak.Size = new System.Drawing.Size(77, 36);
            this.buttonBreak.TabIndex = 15;
            this.buttonBreak.Text = "Break";
            this.buttonBreak.UseVisualStyleBackColor = true;
            this.buttonBreak.Click += new System.EventHandler(this.buttonBreak_Click);
            // 
            // buttonSkip
            // 
            this.buttonSkip.Location = new System.Drawing.Point(253, 14);
            this.buttonSkip.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonSkip.Name = "buttonSkip";
            this.buttonSkip.Size = new System.Drawing.Size(77, 36);
            this.buttonSkip.TabIndex = 14;
            this.buttonSkip.Text = "Skip";
            this.buttonSkip.UseVisualStyleBackColor = true;
            this.buttonSkip.Click += new System.EventHandler(this.buttonSkip_Click);
            // 
            // buttonRepeat
            // 
            this.buttonRepeat.Location = new System.Drawing.Point(173, 14);
            this.buttonRepeat.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonRepeat.Name = "buttonRepeat";
            this.buttonRepeat.Size = new System.Drawing.Size(77, 36);
            this.buttonRepeat.TabIndex = 13;
            this.buttonRepeat.Text = "Repeat";
            this.buttonRepeat.UseVisualStyleBackColor = true;
            this.buttonRepeat.Click += new System.EventHandler(this.buttonRepeat_Click);
            // 
            // buttonAccept
            // 
            this.buttonAccept.Location = new System.Drawing.Point(92, 14);
            this.buttonAccept.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAccept.Name = "buttonAccept";
            this.buttonAccept.Size = new System.Drawing.Size(77, 36);
            this.buttonAccept.TabIndex = 12;
            this.buttonAccept.Text = "Accept";
            this.buttonAccept.UseVisualStyleBackColor = true;
            this.buttonAccept.Click += new System.EventHandler(this.buttonAccept_Click);
            // 
            // FormAcquisition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1039, 606);
            this.Controls.Add(this.pictureAcq);
            this.Controls.Add(this.textMessage);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.textDiagnostic);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonBreak);
            this.Controls.Add(this.buttonSkip);
            this.Controls.Add(this.buttonRepeat);
            this.Controls.Add(this.buttonAccept);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FormAcquisition";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Acquisition";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormAcquisition_FormClosed);
            this.Load += new System.EventHandler(this.FormAcquisition_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureAcq)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureScanningFinger)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureAcq;
        private System.Windows.Forms.ComboBox comboACTL;
        private System.Windows.Forms.PictureBox pictureScanningFinger;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textMessage;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textMatchScoreThreshold;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.TextBox textDiagnostic;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textScanningFinger;
        private System.Windows.Forms.Button buttonBreak;
        private System.Windows.Forms.Button buttonSkip;
        private System.Windows.Forms.Button buttonRepeat;
        private System.Windows.Forms.Button buttonAccept;
    }
}