namespace WzDumper {
	partial class MainForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
            if (CancelSource != null)
                CancelSource.Dispose();
            base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.SelectWzFileButton = new System.Windows.Forms.Button();
			this.DumpWzButton = new System.Windows.Forms.Button();
			this.Info = new System.Windows.Forms.TextBox();
			this.includePngMp3Box = new System.Windows.Forms.CheckBox();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new WzDumper.SafeToolStripLabel();
			this.CancelOpButton = new System.Windows.Forms.Button();
			this.versionBox = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.MapleVersionComboBox = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.WZFileTB = new System.Windows.Forms.TextBox();
			this.outputFolderTB = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.openFolderButton = new System.Windows.Forms.Button();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.clearInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.includeVersionInFolderBox = new System.Windows.Forms.CheckBox();
			this.multiThreadCheckBox = new System.Windows.Forms.CheckBox();
			this.extractorThreadsNum = new System.Windows.Forms.NumericUpDown();
			this.extractorThreadsLabel = new System.Windows.Forms.Label();
			this.SelectWzFolder = new System.Windows.Forms.Button();
			this.SelectExtractDestination = new System.Windows.Forms.Button();
			this.LinkTypeLabel = new System.Windows.Forms.Label();
			this.LinkTypeComboBox = new System.Windows.Forms.ComboBox();
			this.indentCheckBox = new System.Windows.Forms.CheckBox();
			this.statusStrip1.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.extractorThreadsNum)).BeginInit();
			this.SuspendLayout();
			// 
			// SelectWzFileButton
			// 
			this.SelectWzFileButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.SelectWzFileButton.Location = new System.Drawing.Point(558, 271);
			this.SelectWzFileButton.Name = "SelectWzFileButton";
			this.SelectWzFileButton.Size = new System.Drawing.Size(82, 21);
			this.SelectWzFileButton.TabIndex = 0;
			this.SelectWzFileButton.Text = "選擇檔案";
			this.SelectWzFileButton.UseVisualStyleBackColor = true;
			this.SelectWzFileButton.Click += new System.EventHandler(this.SelectWzFile);
			// 
			// DumpWzButton
			// 
			this.DumpWzButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.DumpWzButton.Enabled = false;
			this.DumpWzButton.Location = new System.Drawing.Point(558, 324);
			this.DumpWzButton.Name = "DumpWzButton";
			this.DumpWzButton.Size = new System.Drawing.Size(82, 21);
			this.DumpWzButton.TabIndex = 5;
			this.DumpWzButton.Text = "提取";
			this.DumpWzButton.UseVisualStyleBackColor = true;
			this.DumpWzButton.Click += new System.EventHandler(this.DumpFile);
			// 
			// Info
			// 
			this.Info.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			this.Info.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.Info.Location = new System.Drawing.Point(12, 27);
			this.Info.Multiline = true;
			this.Info.Name = "Info";
			this.Info.ReadOnly = true;
			this.Info.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.Info.Size = new System.Drawing.Size(723, 239);
			this.Info.TabIndex = 9;
			this.Info.TabStop = false;
			// 
			// includePngMp3Box
			// 
			this.includePngMp3Box.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.includePngMp3Box.AutoSize = true;
			this.includePngMp3Box.Location = new System.Drawing.Point(15, 357);
			this.includePngMp3Box.Name = "includePngMp3Box";
			this.includePngMp3Box.Size = new System.Drawing.Size(108, 16);
			this.includePngMp3Box.TabIndex = 3;
			this.includePngMp3Box.Text = "包含圖像及音樂";
			this.includePngMp3Box.UseVisualStyleBackColor = true;
			this.includePngMp3Box.CheckedChanged += new System.EventHandler(this.IncludePngMp3Box_CheckedChanged);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.toolStripStatusLabel1 });
			this.statusStrip1.Location = new System.Drawing.Point(0, 404);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(747, 22);
			this.statusStrip1.TabIndex = 17;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
			// 
			// CancelOpButton
			// 
			this.CancelOpButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.CancelOpButton.Enabled = false;
			this.CancelOpButton.Location = new System.Drawing.Point(653, 324);
			this.CancelOpButton.Name = "CancelOpButton";
			this.CancelOpButton.Size = new System.Drawing.Size(82, 21);
			this.CancelOpButton.TabIndex = 7;
			this.CancelOpButton.Text = "取消";
			this.CancelOpButton.UseVisualStyleBackColor = true;
			this.CancelOpButton.Click += new System.EventHandler(this.CancelOperation);
			// 
			// versionBox
			// 
			this.versionBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.versionBox.Enabled = false;
			this.versionBox.Location = new System.Drawing.Point(381, 328);
			this.versionBox.MaxLength = 5;
			this.versionBox.Name = "versionBox";
			this.versionBox.ReadOnly = true;
			this.versionBox.Size = new System.Drawing.Size(42, 22);
			this.versionBox.TabIndex = 2;
			// 
			// label2
			// 
			this.label2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.label2.Location = new System.Drawing.Point(12, 332);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(58, 13);
			this.label2.TabIndex = 15;
			this.label2.Text = "加密方式:";
			// 
			// MapleVersionComboBox
			// 
			this.MapleVersionComboBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.MapleVersionComboBox.FormattingEnabled = true;
			this.MapleVersionComboBox.Items.AddRange(new object[] { "None(GMS-v117+/TMS-Lastest)", "GMS (v0.56-0.116)", "CMS/EMS/MSEA/TMS" });
			this.MapleVersionComboBox.Location = new System.Drawing.Point(97, 329);
			this.MapleVersionComboBox.Name = "MapleVersionComboBox";
			this.MapleVersionComboBox.Size = new System.Drawing.Size(190, 20);
			this.MapleVersionComboBox.TabIndex = 1;
			this.MapleVersionComboBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MapleVersionComboBoxKeyPress);
			// 
			// label3
			// 
			this.label3.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.label3.Location = new System.Drawing.Point(293, 332);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(82, 13);
			this.label3.TabIndex = 16;
			this.label3.Text = "偵測檔案版本:";
			// 
			// WZFileTB
			// 
			this.WZFileTB.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.WZFileTB.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.WZFileTB.Location = new System.Drawing.Point(97, 272);
			this.WZFileTB.Name = "WZFileTB";
			this.WZFileTB.ReadOnly = true;
			this.WZFileTB.Size = new System.Drawing.Size(455, 22);
			this.WZFileTB.TabIndex = 10;
			// 
			// outputFolderTB
			// 
			this.outputFolderTB.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.outputFolderTB.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.outputFolderTB.Location = new System.Drawing.Point(97, 300);
			this.outputFolderTB.Name = "outputFolderTB";
			this.outputFolderTB.ReadOnly = true;
			this.outputFolderTB.Size = new System.Drawing.Size(455, 22);
			this.outputFolderTB.TabIndex = 11;
			// 
			// label4
			// 
			this.label4.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(12, 275);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(71, 12);
			this.label4.TabIndex = 13;
			this.label4.Text = "檔案/資料夾:";
			// 
			// label5
			// 
			this.label5.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(13, 303);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(32, 12);
			this.label5.TabIndex = 14;
			this.label5.Text = "輸出:";
			// 
			// openFolderButton
			// 
			this.openFolderButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.openFolderButton.Enabled = false;
			this.openFolderButton.Location = new System.Drawing.Point(653, 299);
			this.openFolderButton.Name = "openFolderButton";
			this.openFolderButton.Size = new System.Drawing.Size(82, 21);
			this.openFolderButton.TabIndex = 12;
			this.openFolderButton.Text = "開啟";
			this.openFolderButton.UseVisualStyleBackColor = true;
			this.openFolderButton.Click += new System.EventHandler(this.OpenFolder);
			// 
			// menuStrip1
			// 
			this.menuStrip1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.fileToolStripMenuItem, this.aboutToolStripMenuItem });
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.menuStrip1.Size = new System.Drawing.Size(747, 24);
			this.menuStrip1.TabIndex = 8;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { this.clearInfoToolStripMenuItem, this.exitToolStripMenuItem });
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
			this.fileToolStripMenuItem.Text = "檔案";
			// 
			// clearInfoToolStripMenuItem
			// 
			this.clearInfoToolStripMenuItem.Name = "clearInfoToolStripMenuItem";
			this.clearInfoToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
			this.clearInfoToolStripMenuItem.Text = "清除信息";
			this.clearInfoToolStripMenuItem.Click += new System.EventHandler(this.ClearInfoToolStripMenuItemClick);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
			this.exitToolStripMenuItem.Text = "離開";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItemClick);
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { this.aboutToolStripMenuItem1 });
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
			this.aboutToolStripMenuItem.Text = "關於";
			// 
			// aboutToolStripMenuItem1
			// 
			this.aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
			this.aboutToolStripMenuItem1.Size = new System.Drawing.Size(98, 22);
			this.aboutToolStripMenuItem1.Text = "關於";
			this.aboutToolStripMenuItem1.Click += new System.EventHandler(this.AboutToolStripMenuItem1Click);
			// 
			// includeVersionInFolderBox
			// 
			this.includeVersionInFolderBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.includeVersionInFolderBox.AutoSize = true;
			this.includeVersionInFolderBox.Location = new System.Drawing.Point(357, 357);
			this.includeVersionInFolderBox.Name = "includeVersionInFolderBox";
			this.includeVersionInFolderBox.Size = new System.Drawing.Size(180, 16);
			this.includeVersionInFolderBox.TabIndex = 4;
			this.includeVersionInFolderBox.Text = "資料夾名稱後添加檔案版本號";
			this.includeVersionInFolderBox.UseVisualStyleBackColor = true;
			// 
			// multiThreadCheckBox
			// 
			this.multiThreadCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.multiThreadCheckBox.AutoSize = true;
			this.multiThreadCheckBox.Location = new System.Drawing.Point(15, 384);
			this.multiThreadCheckBox.Name = "multiThreadCheckBox";
			this.multiThreadCheckBox.Size = new System.Drawing.Size(132, 16);
			this.multiThreadCheckBox.TabIndex = 19;
			this.multiThreadCheckBox.Text = "使用多線程導出檔案";
			this.multiThreadCheckBox.UseVisualStyleBackColor = true;
			this.multiThreadCheckBox.CheckedChanged += new System.EventHandler(this.MultiThreadCheckBox_CheckedChanged);
			// 
			// extractorThreadsNum
			// 
			this.extractorThreadsNum.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.extractorThreadsNum.Enabled = false;
			this.extractorThreadsNum.Location = new System.Drawing.Point(267, 381);
			this.extractorThreadsNum.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
			this.extractorThreadsNum.Name = "extractorThreadsNum";
			this.extractorThreadsNum.Size = new System.Drawing.Size(39, 22);
			this.extractorThreadsNum.TabIndex = 25;
			this.extractorThreadsNum.Value = new decimal(new int[] { 1, 0, 0, 0 });
			// 
			// extractorThreadsLabel
			// 
			this.extractorThreadsLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.extractorThreadsLabel.AutoSize = true;
			this.extractorThreadsLabel.Enabled = false;
			this.extractorThreadsLabel.Location = new System.Drawing.Point(189, 385);
			this.extractorThreadsLabel.Name = "extractorThreadsLabel";
			this.extractorThreadsLabel.Size = new System.Drawing.Size(56, 12);
			this.extractorThreadsLabel.TabIndex = 20;
			this.extractorThreadsLabel.Text = "最高線程:";
			// 
			// SelectWzFolder
			// 
			this.SelectWzFolder.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.SelectWzFolder.Location = new System.Drawing.Point(653, 271);
			this.SelectWzFolder.Name = "SelectWzFolder";
			this.SelectWzFolder.Size = new System.Drawing.Size(82, 21);
			this.SelectWzFolder.TabIndex = 21;
			this.SelectWzFolder.Text = "選擇資料夾";
			this.SelectWzFolder.UseVisualStyleBackColor = true;
			this.SelectWzFolder.Click += new System.EventHandler(this.SelectWzFolder_Click);
			// 
			// SelectExtractDestination
			// 
			this.SelectExtractDestination.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.SelectExtractDestination.Location = new System.Drawing.Point(558, 299);
			this.SelectExtractDestination.Name = "SelectExtractDestination";
			this.SelectExtractDestination.Size = new System.Drawing.Size(82, 21);
			this.SelectExtractDestination.TabIndex = 22;
			this.SelectExtractDestination.Text = "選擇資料夾";
			this.SelectExtractDestination.UseVisualStyleBackColor = true;
			this.SelectExtractDestination.Click += new System.EventHandler(this.SelectExtractDestination_Click);
			// 
			// LinkTypeLabel
			// 
			this.LinkTypeLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.LinkTypeLabel.AutoSize = true;
			this.LinkTypeLabel.Location = new System.Drawing.Point(189, 358);
			this.LinkTypeLabel.Name = "LinkTypeLabel";
			this.LinkTypeLabel.Size = new System.Drawing.Size(56, 12);
			this.LinkTypeLabel.TabIndex = 23;
			this.LinkTypeLabel.Text = "連結類型:";
			// 
			// LinkTypeComboBox
			// 
			this.LinkTypeComboBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.LinkTypeComboBox.Enabled = false;
			this.LinkTypeComboBox.FormattingEnabled = true;
			this.LinkTypeComboBox.Location = new System.Drawing.Point(252, 354);
			this.LinkTypeComboBox.Name = "LinkTypeComboBox";
			this.LinkTypeComboBox.Size = new System.Drawing.Size(82, 20);
			this.LinkTypeComboBox.TabIndex = 24;
			this.LinkTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.LinkTypeComboBox_SelectedIndexChanged);
			this.LinkTypeComboBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.LinkTypeComboBox_KeyPress);
			// 
			// indentCheckBox
			// 
			this.indentCheckBox.Location = new System.Drawing.Point(357, 378);
			this.indentCheckBox.Name = "indentCheckBox";
			this.indentCheckBox.Size = new System.Drawing.Size(104, 24);
			this.indentCheckBox.TabIndex = 26;
			this.indentCheckBox.Text = "啟用縮排";
			this.indentCheckBox.UseVisualStyleBackColor = true;
			this.indentCheckBox.CheckedChanged += new System.EventHandler(this.indentCheckBox_CheckedChanged);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(747, 426);
			this.Controls.Add(this.indentCheckBox);
			this.Controls.Add(this.extractorThreadsNum);
			this.Controls.Add(this.extractorThreadsLabel);
			this.Controls.Add(this.LinkTypeComboBox);
			this.Controls.Add(this.multiThreadCheckBox);
			this.Controls.Add(this.LinkTypeLabel);
			this.Controls.Add(this.SelectExtractDestination);
			this.Controls.Add(this.SelectWzFolder);
			this.Controls.Add(this.includeVersionInFolderBox);
			this.Controls.Add(this.openFolderButton);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.outputFolderTB);
			this.Controls.Add(this.WZFileTB);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.MapleVersionComboBox);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.versionBox);
			this.Controls.Add(this.CancelOpButton);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.menuStrip1);
			this.Controls.Add(this.includePngMp3Box);
			this.Controls.Add(this.Info);
			this.Controls.Add(this.DumpWzButton);
			this.Controls.Add(this.SelectWzFileButton);
			this.MainMenuStrip = this.menuStrip1;
			this.MinimumSize = new System.Drawing.Size(720, 464);
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "WZ 提取器";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1FormClosing);
			this.Load += new System.EventHandler(this.Form1Load);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.extractorThreadsNum)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		private System.Windows.Forms.CheckBox indentCheckBox;

		#endregion

		private System.Windows.Forms.Button SelectWzFileButton;
		private System.Windows.Forms.Button DumpWzButton;
		private System.Windows.Forms.TextBox Info;
		private System.Windows.Forms.CheckBox includePngMp3Box;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.Button CancelOpButton;
		private SafeToolStripLabel toolStripStatusLabel1;
		private System.Windows.Forms.TextBox versionBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox MapleVersionComboBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox WZFileTB;
		private System.Windows.Forms.TextBox outputFolderTB;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button openFolderButton;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.CheckBox includeVersionInFolderBox;
		private System.Windows.Forms.ToolStripMenuItem clearInfoToolStripMenuItem;
        private System.Windows.Forms.CheckBox multiThreadCheckBox;
        private System.Windows.Forms.Label extractorThreadsLabel;
        private System.Windows.Forms.NumericUpDown extractorThreadsNum;
        private System.Windows.Forms.Button SelectWzFolder;
        private System.Windows.Forms.Button SelectExtractDestination;
        private System.Windows.Forms.Label LinkTypeLabel;
        private System.Windows.Forms.ComboBox LinkTypeComboBox;
    }
}