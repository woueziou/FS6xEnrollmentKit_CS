namespace FS6xEnrollmentKit_CS
{
    partial class FormConfiguration
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConfiguration));
            this.label1 = new System.Windows.Forms.Label();
            this.comboStandard = new System.Windows.Forms.ComboBox();
            this.labelFourFingers = new System.Windows.Forms.Label();
            this.labelTwoThumbs = new System.Windows.Forms.Label();
            this.labelRolledFinger = new System.Windows.Forms.Label();
            this.labelPlainThumb = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.textPTWidth = new System.Windows.Forms.TextBox();
            this.textPTHeight = new System.Windows.Forms.TextBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Standard";
            // 
            // comboStandard
            // 
            this.comboStandard.FormattingEnabled = true;
            this.comboStandard.Items.AddRange(new object[] {
            "ANSI/NIST-ITL 1-2007",
            "ANSI/INCITS 381-2004",
            "ISO/IEC 19794-4"});
            this.comboStandard.Location = new System.Drawing.Point(110, 31);
            this.comboStandard.Name = "comboStandard";
            this.comboStandard.Size = new System.Drawing.Size(264, 24);
            this.comboStandard.TabIndex = 1;
            this.comboStandard.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // labelFourFingers
            // 
            this.labelFourFingers.Location = new System.Drawing.Point(28, 133);
            this.labelFourFingers.Name = "labelFourFingers";
            this.labelFourFingers.Size = new System.Drawing.Size(103, 23);
            this.labelFourFingers.TabIndex = 2;
            this.labelFourFingers.Text = "Four Fingers";
            this.labelFourFingers.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTwoThumbs
            // 
            this.labelTwoThumbs.Location = new System.Drawing.Point(28, 176);
            this.labelTwoThumbs.Name = "labelTwoThumbs";
            this.labelTwoThumbs.Size = new System.Drawing.Size(103, 23);
            this.labelTwoThumbs.TabIndex = 3;
            this.labelTwoThumbs.Text = "Two Thumbs";
            this.labelTwoThumbs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelRolledFinger
            // 
            this.labelRolledFinger.Location = new System.Drawing.Point(28, 218);
            this.labelRolledFinger.Name = "labelRolledFinger";
            this.labelRolledFinger.Size = new System.Drawing.Size(103, 23);
            this.labelRolledFinger.TabIndex = 4;
            this.labelRolledFinger.Text = "Rolled Finger";
            this.labelRolledFinger.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelPlainThumb
            // 
            this.labelPlainThumb.Location = new System.Drawing.Point(28, 258);
            this.labelPlainThumb.Name = "labelPlainThumb";
            this.labelPlainThumb.Size = new System.Drawing.Size(103, 23);
            this.labelPlainThumb.TabIndex = 5;
            this.labelPlainThumb.Text = "Plain Thumb";
            this.labelPlainThumb.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(154, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "Width";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(255, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 17);
            this.label3.TabIndex = 7;
            this.label3.Text = "Height";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(324, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 17);
            this.label4.TabIndex = 8;
            this.label4.Text = "(Inches)";
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Location = new System.Drawing.Point(147, 133);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(59, 22);
            this.textBox1.TabIndex = 9;
            this.textBox1.Text = "3.2";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox2
            // 
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox2.Location = new System.Drawing.Point(249, 133);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(59, 22);
            this.textBox2.TabIndex = 10;
            this.textBox2.Text = "3.0";
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox3
            // 
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox3.Location = new System.Drawing.Point(147, 176);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(59, 22);
            this.textBox3.TabIndex = 11;
            this.textBox3.Text = "1.6";
            this.textBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox4
            // 
            this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox4.Location = new System.Drawing.Point(249, 176);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(59, 22);
            this.textBox4.TabIndex = 12;
            this.textBox4.Text = "1.5";
            this.textBox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox5
            // 
            this.textBox5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox5.Location = new System.Drawing.Point(147, 218);
            this.textBox5.Name = "textBox5";
            this.textBox5.ReadOnly = true;
            this.textBox5.Size = new System.Drawing.Size(59, 22);
            this.textBox5.TabIndex = 13;
            this.textBox5.Text = "1.6";
            this.textBox5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox6
            // 
            this.textBox6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox6.Location = new System.Drawing.Point(249, 218);
            this.textBox6.Name = "textBox6";
            this.textBox6.ReadOnly = true;
            this.textBox6.Size = new System.Drawing.Size(59, 22);
            this.textBox6.TabIndex = 14;
            this.textBox6.Text = "1.5";
            this.textBox6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textPTWidth
            // 
            this.textPTWidth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textPTWidth.Location = new System.Drawing.Point(147, 258);
            this.textPTWidth.Name = "textPTWidth";
            this.textPTWidth.ReadOnly = true;
            this.textPTWidth.Size = new System.Drawing.Size(59, 22);
            this.textPTWidth.TabIndex = 15;
            this.textPTWidth.Text = "1.0";
            this.textPTWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textPTHeight
            // 
            this.textPTHeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textPTHeight.Location = new System.Drawing.Point(249, 258);
            this.textPTHeight.Name = "textPTHeight";
            this.textPTHeight.ReadOnly = true;
            this.textPTHeight.Size = new System.Drawing.Size(59, 22);
            this.textPTHeight.TabIndex = 16;
            this.textPTHeight.Text = "1.2";
            this.textPTHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(147, 327);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(107, 31);
            this.buttonOK.TabIndex = 17;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(22, 306);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(360, 1);
            this.pictureBox1.TabIndex = 18;
            this.pictureBox1.TabStop = false;
            // 
            // FormConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 373);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.textPTHeight);
            this.Controls.Add(this.textPTWidth);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelPlainThumb);
            this.Controls.Add(this.labelRolledFinger);
            this.Controls.Add(this.labelTwoThumbs);
            this.Controls.Add(this.labelFourFingers);
            this.Controls.Add(this.comboStandard);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormConfiguration";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configuration";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboStandard;
        private System.Windows.Forms.Label labelFourFingers;
        private System.Windows.Forms.Label labelTwoThumbs;
        private System.Windows.Forms.Label labelRolledFinger;
        private System.Windows.Forms.Label labelPlainThumb;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox textPTWidth;
        private System.Windows.Forms.TextBox textPTHeight;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}