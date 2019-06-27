namespace ReadFile_RegisterVN
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtpathfile = new System.Windows.Forms.TextBox();
            this.btnReadFile = new System.Windows.Forms.Button();
            this.btnBrowser = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.DBView = new System.Windows.Forms.DataGridView();
            this.btnInsert = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtLan = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMaNV = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtMaCa = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtMaTram = new System.Windows.Forms.TextBox();
            this.btnReadLog = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.DBView)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Path:";
            // 
            // txtpathfile
            // 
            this.txtpathfile.Location = new System.Drawing.Point(51, 10);
            this.txtpathfile.Name = "txtpathfile";
            this.txtpathfile.Size = new System.Drawing.Size(271, 20);
            this.txtpathfile.TabIndex = 1;
            // 
            // btnReadFile
            // 
            this.btnReadFile.Location = new System.Drawing.Point(410, 10);
            this.btnReadFile.Name = "btnReadFile";
            this.btnReadFile.Size = new System.Drawing.Size(75, 23);
            this.btnReadFile.TabIndex = 2;
            this.btnReadFile.Text = "ReadFile";
            this.btnReadFile.UseVisualStyleBackColor = true;
            this.btnReadFile.Click += new System.EventHandler(this.btnReadFile_Click);
            // 
            // btnBrowser
            // 
            this.btnBrowser.Location = new System.Drawing.Point(329, 10);
            this.btnBrowser.Name = "btnBrowser";
            this.btnBrowser.Size = new System.Drawing.Size(75, 23);
            this.btnBrowser.TabIndex = 3;
            this.btnBrowser.Text = "Browser";
            this.btnBrowser.UseVisualStyleBackColor = true;
            this.btnBrowser.Click += new System.EventHandler(this.btnBrowser_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(572, 10);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // DBView
            // 
            this.DBView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DBView.Location = new System.Drawing.Point(12, 213);
            this.DBView.Name = "DBView";
            this.DBView.Size = new System.Drawing.Size(816, 294);
            this.DBView.TabIndex = 5;
            // 
            // btnInsert
            // 
            this.btnInsert.Location = new System.Drawing.Point(572, 45);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(75, 23);
            this.btnInsert.TabIndex = 6;
            this.btnInsert.Text = "Insert";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(12, 74);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(816, 133);
            this.txtLog.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Làn";
            // 
            // txtLan
            // 
            this.txtLan.Location = new System.Drawing.Point(47, 52);
            this.txtLan.Name = "txtLan";
            this.txtLan.Size = new System.Drawing.Size(38, 20);
            this.txtLan.TabIndex = 9;
            this.txtLan.Text = "txtLan";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(96, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "MaNV:";
            // 
            // txtMaNV
            // 
            this.txtMaNV.Location = new System.Drawing.Point(142, 52);
            this.txtMaNV.Name = "txtMaNV";
            this.txtMaNV.Size = new System.Drawing.Size(44, 20);
            this.txtMaNV.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(203, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Ca:";
            this.label4.Click += new System.EventHandler(this.Label4_Click);
            // 
            // txtMaCa
            // 
            this.txtMaCa.Location = new System.Drawing.Point(232, 52);
            this.txtMaCa.Name = "txtMaCa";
            this.txtMaCa.Size = new System.Drawing.Size(38, 20);
            this.txtMaCa.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(297, 55);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(31, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Trạm";
            // 
            // txtMaTram
            // 
            this.txtMaTram.Location = new System.Drawing.Point(334, 52);
            this.txtMaTram.Name = "txtMaTram";
            this.txtMaTram.Size = new System.Drawing.Size(47, 20);
            this.txtMaTram.TabIndex = 15;
            // 
            // btnReadLog
            // 
            this.btnReadLog.Location = new System.Drawing.Point(491, 45);
            this.btnReadLog.Name = "btnReadLog";
            this.btnReadLog.Size = new System.Drawing.Size(75, 23);
            this.btnReadLog.TabIndex = 16;
            this.btnReadLog.Text = "Read";
            this.btnReadLog.UseVisualStyleBackColor = true;
            this.btnReadLog.Click += new System.EventHandler(this.BtnReadLog_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(846, 519);
            this.Controls.Add(this.btnReadLog);
            this.Controls.Add(this.txtMaTram);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtMaCa);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtMaNV);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtLan);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.btnInsert);
            this.Controls.Add(this.DBView);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnBrowser);
            this.Controls.Add(this.btnReadFile);
            this.Controls.Add(this.txtpathfile);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.DBView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtpathfile;
        private System.Windows.Forms.Button btnReadFile;
        private System.Windows.Forms.Button btnBrowser;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.DataGridView DBView;
        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtLan;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMaNV;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtMaCa;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtMaTram;
        private System.Windows.Forms.Button btnReadLog;
    }
}

