namespace SOCOMCA_MAP_Editor
{
    partial class frmModels
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
            this.txtFile = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.txtAddr = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.nodeTree = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // txtFile
            // 
            this.txtFile.Location = new System.Drawing.Point(12, 12);
            this.txtFile.Name = "txtFile";
            this.txtFile.Size = new System.Drawing.Size(672, 20);
            this.txtFile.TabIndex = 0;
            this.txtFile.Text = "C:\\Users\\Dom\\Desktop\\PS2 Stuff\\CA\\MAP Mods\\E7763CEA (Citadel)\\RUN\\MOROCCO_FORT\\VC" +
    "OMMON_MP315SUP\\clib";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(690, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(30, 20);
            this.button1.TabIndex = 1;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtAddr
            // 
            this.txtAddr.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAddr.Location = new System.Drawing.Point(12, 38);
            this.txtAddr.Name = "txtAddr";
            this.txtAddr.Size = new System.Drawing.Size(77, 20);
            this.txtAddr.TabIndex = 2;
            this.txtAddr.Text = "0014cff0";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(95, 38);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(89, 20);
            this.button2.TabIndex = 3;
            this.button2.Text = "Load Model";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(402, 64);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(426, 448);
            this.textBox1.TabIndex = 5;
            // 
            // nodeTree
            // 
            this.nodeTree.Location = new System.Drawing.Point(12, 64);
            this.nodeTree.Name = "nodeTree";
            this.nodeTree.Size = new System.Drawing.Size(378, 448);
            this.nodeTree.TabIndex = 6;
            this.nodeTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.nodeTree_AfterSelect_1);
            // 
            // frmModels
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(840, 524);
            this.Controls.Add(this.nodeTree);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.txtAddr);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtFile);
            this.Name = "frmModels";
            this.Text = "frmModels";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtAddr;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TreeView nodeTree;
    }
}