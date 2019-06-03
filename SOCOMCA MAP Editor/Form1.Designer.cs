namespace SOCOMCA_MAP_Editor
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.button1 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button2 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button3 = new System.Windows.Forms.Button();
            this.txtAddr = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.lblPreview = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            this.button15 = new System.Windows.Forms.Button();
            this.button16 = new System.Windows.Forms.Button();
            this.dxTimer = new System.Windows.Forms.Timer(this.components);
            this.button17 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(88, 32);
            this.button1.TabIndex = 0;
            this.button1.Text = "Open Map File";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 50);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(88, 50);
            this.button2.TabIndex = 1;
            this.button2.Text = "RAW Pic";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Fuchsia;
            this.pictureBox1.Location = new System.Drawing.Point(200, 72);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 84);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(12, 106);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(88, 50);
            this.button3.TabIndex = 3;
            this.button3.Text = "TIF Load";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // txtAddr
            // 
            this.txtAddr.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAddr.Location = new System.Drawing.Point(106, 122);
            this.txtAddr.Name = "txtAddr";
            this.txtAddr.Size = new System.Drawing.Size(88, 20);
            this.txtAddr.TabIndex = 4;
            this.txtAddr.Text = "01348b40";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(106, 12);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(88, 32);
            this.button4.TabIndex = 5;
            this.button4.Text = "Preview >>";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // lblPreview
            // 
            this.lblPreview.AutoSize = true;
            this.lblPreview.Location = new System.Drawing.Point(211, 12);
            this.lblPreview.Name = "lblPreview";
            this.lblPreview.Size = new System.Drawing.Size(71, 13);
            this.lblPreview.TabIndex = 6;
            this.lblPreview.Text = "Preview (0/0)";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(106, 50);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(88, 50);
            this.button5.TabIndex = 7;
            this.button5.Text = "Load RDR";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(12, 162);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(87, 50);
            this.button6.TabIndex = 8;
            this.button6.Text = "Extract From DUMP";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(106, 162);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(87, 49);
            this.button7.TabIndex = 9;
            this.button7.Text = "Test Render";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(13, 248);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(87, 25);
            this.button8.TabIndex = 10;
            this.button8.Text = "Scale Down .1";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(13, 217);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(87, 25);
            this.button9.TabIndex = 11;
            this.button9.Text = "Scale Up .1";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(106, 217);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(87, 25);
            this.button10.TabIndex = 13;
            this.button10.Text = "Rotate X";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(106, 248);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(87, 25);
            this.button11.TabIndex = 12;
            this.button11.Text = "Rotate Y";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(105, 317);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(87, 25);
            this.button12.TabIndex = 15;
            this.button12.Text = "Move Right";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(12, 317);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(87, 25);
            this.button13.TabIndex = 14;
            this.button13.Text = "Move Left";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.button13_Click);
            // 
            // button14
            // 
            this.button14.Location = new System.Drawing.Point(60, 286);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(87, 25);
            this.button14.TabIndex = 16;
            this.button14.Text = "Move Up";
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Click += new System.EventHandler(this.button14_Click);
            // 
            // button15
            // 
            this.button15.Location = new System.Drawing.Point(60, 348);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(87, 25);
            this.button15.TabIndex = 17;
            this.button15.Text = "Move Down";
            this.button15.UseVisualStyleBackColor = true;
            this.button15.Click += new System.EventHandler(this.button15_Click);
            // 
            // button16
            // 
            this.button16.Location = new System.Drawing.Point(12, 379);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(88, 33);
            this.button16.TabIndex = 18;
            this.button16.Text = "DirectX Test";
            this.button16.UseVisualStyleBackColor = true;
            this.button16.Click += new System.EventHandler(this.button16_Click);
            // 
            // dxTimer
            // 
            this.dxTimer.Interval = 1;
            this.dxTimer.Tick += new System.EventHandler(this.dxTimer_Tick);
            // 
            // button17
            // 
            this.button17.Location = new System.Drawing.Point(106, 379);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(86, 33);
            this.button17.TabIndex = 19;
            this.button17.Text = "Models";
            this.button17.UseVisualStyleBackColor = true;
            this.button17.Click += new System.EventHandler(this.button17_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1025, 665);
            this.Controls.Add(this.button17);
            this.Controls.Add(this.button16);
            this.Controls.Add(this.button15);
            this.Controls.Add(this.button14);
            this.Controls.Add(this.button12);
            this.Controls.Add(this.button13);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.lblPreview);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.txtAddr);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "[TEST] SCA Map Editor";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox txtAddr;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label lblPreview;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.Button button16;
        private System.Windows.Forms.Timer dxTimer;
        private System.Windows.Forms.Button button17;
    }
}

