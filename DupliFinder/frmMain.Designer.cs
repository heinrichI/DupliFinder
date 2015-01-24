namespace DupliFinder
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.lblStart = new System.Windows.Forms.Label();
            this.tbPath = new System.Windows.Forms.TextBox();
            this.btBrowse = new System.Windows.Forms.Button();
            this.btSearch = new System.Windows.Forms.Button();
            this.cbIncSubfolders = new System.Windows.Forms.CheckBox();
            this.fd = new System.Windows.Forms.FolderBrowserDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutDeadRingerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.pbWorkingAll = new System.Windows.Forms.ToolStripProgressBar();
            this.lblWorking = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelSimilarityThreshold = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.trackBarSimilarityThreshold = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.cbAlgorithms = new System.Windows.Forms.ComboBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lvResults = new System.Windows.Forms.ListView();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.btRemDup = new System.Windows.Forms.Button();
            this.btRemOrig = new System.Windows.Forms.Button();
            this.lvDuplicates = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.pbDup = new System.Windows.Forms.PictureBox();
            this.pbOrig = new System.Windows.Forms.PictureBox();
            this.chName = new System.Windows.Forms.ColumnHeader();
            this.chSize = new System.Windows.Forms.ColumnHeader();
            this.chSimilarity = new System.Windows.Forms.ColumnHeader();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSimilarityThreshold)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbDup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbOrig)).BeginInit();
            this.SuspendLayout();
            // 
            // lblStart
            // 
            this.lblStart.AutoSize = true;
            this.lblStart.Location = new System.Drawing.Point(1, 19);
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size(79, 13);
            this.lblStart.TabIndex = 0;
            this.lblStart.Text = "S&tart search at:";
            // 
            // tbPath
            // 
            this.tbPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPath.Location = new System.Drawing.Point(89, 16);
            this.tbPath.Name = "tbPath";
            this.tbPath.ReadOnly = true;
            this.tbPath.Size = new System.Drawing.Size(433, 20);
            this.tbPath.TabIndex = 1;
            this.tbPath.TabStop = false;
            // 
            // btBrowse
            // 
            this.btBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btBrowse.Location = new System.Drawing.Point(523, 14);
            this.btBrowse.Name = "btBrowse";
            this.btBrowse.Size = new System.Drawing.Size(24, 23);
            this.btBrowse.TabIndex = 2;
            this.btBrowse.Text = "...";
            this.btBrowse.UseVisualStyleBackColor = true;
            this.btBrowse.Click += new System.EventHandler(this.btBrowse_Click);
            // 
            // btSearch
            // 
            this.btSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btSearch.Enabled = false;
            this.btSearch.Location = new System.Drawing.Point(478, 68);
            this.btSearch.Name = "btSearch";
            this.btSearch.Size = new System.Drawing.Size(75, 23);
            this.btSearch.TabIndex = 4;
            this.btSearch.Text = "&Search";
            this.btSearch.UseVisualStyleBackColor = true;
            this.btSearch.Click += new System.EventHandler(this.btSearch_Click);
            // 
            // cbIncSubfolders
            // 
            this.cbIncSubfolders.AutoSize = true;
            this.cbIncSubfolders.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbIncSubfolders.Location = new System.Drawing.Point(4, 70);
            this.cbIncSubfolders.Name = "cbIncSubfolders";
            this.cbIncSubfolders.Size = new System.Drawing.Size(112, 17);
            this.cbIncSubfolders.TabIndex = 3;
            this.cbIncSubfolders.Text = "&Include subfolders";
            this.cbIncSubfolders.UseVisualStyleBackColor = true;
            // 
            // fd
            // 
            this.fd.RootFolder = System.Environment.SpecialFolder.MyComputer;
            this.fd.ShowNewFolderButton = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(559, 24);
            this.menuStrip1.TabIndex = 9;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutDeadRingerToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutDeadRingerToolStripMenuItem
            // 
            this.aboutDeadRingerToolStripMenuItem.Name = "aboutDeadRingerToolStripMenuItem";
            this.aboutDeadRingerToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.aboutDeadRingerToolStripMenuItem.Text = "About DeadRinger";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pbWorkingAll,
            this.lblWorking});
            this.statusStrip1.Location = new System.Drawing.Point(0, 510);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(559, 22);
            this.statusStrip1.TabIndex = 10;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // pbWorkingAll
            // 
            this.pbWorkingAll.Name = "pbWorkingAll";
            this.pbWorkingAll.Size = new System.Drawing.Size(100, 16);
            // 
            // lblWorking
            // 
            this.lblWorking.Name = "lblWorking";
            this.lblWorking.Size = new System.Drawing.Size(0, 17);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelSimilarityThreshold);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btSearch);
            this.groupBox1.Controls.Add(this.trackBarSimilarityThreshold);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cbAlgorithms);
            this.groupBox1.Controls.Add(this.tbPath);
            this.groupBox1.Controls.Add(this.lblStart);
            this.groupBox1.Controls.Add(this.btBrowse);
            this.groupBox1.Controls.Add(this.cbIncSubfolders);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 24);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(559, 100);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search Parameters";
            // 
            // labelSimilarityThreshold
            // 
            this.labelSimilarityThreshold.AutoSize = true;
            this.labelSimilarityThreshold.Location = new System.Drawing.Point(502, 46);
            this.labelSimilarityThreshold.Name = "labelSimilarityThreshold";
            this.labelSimilarityThreshold.Size = new System.Drawing.Size(0, 13);
            this.labelSimilarityThreshold.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(254, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Threshold:";
            // 
            // trackBarSimilarityThreshold
            // 
            this.trackBarSimilarityThreshold.Location = new System.Drawing.Point(317, 42);
            this.trackBarSimilarityThreshold.Maximum = 10000;
            this.trackBarSimilarityThreshold.Name = "trackBarSimilarityThreshold";
            this.trackBarSimilarityThreshold.Size = new System.Drawing.Size(191, 45);
            this.trackBarSimilarityThreshold.TabIndex = 10;
            this.trackBarSimilarityThreshold.TickFrequency = 0;
            this.trackBarSimilarityThreshold.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarSimilarityThreshold.ValueChanged += new System.EventHandler(this.trackBarSimilarityThreshold_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Algorithm to use:";
            // 
            // cbAlgorithms
            // 
            this.cbAlgorithms.FormattingEnabled = true;
            this.cbAlgorithms.Location = new System.Drawing.Point(89, 43);
            this.cbAlgorithms.MaxDropDownItems = 15;
            this.cbAlgorithms.Name = "cbAlgorithms";
            this.cbAlgorithms.Size = new System.Drawing.Size(147, 21);
            this.cbAlgorithms.TabIndex = 8;
            this.cbAlgorithms.SelectionChangeCommitted += new System.EventHandler(this.cbAlgorithms_SelectionChangeCommitted);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 124);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btRemDup);
            this.splitContainer1.Panel1.Controls.Add(this.btRemOrig);
            this.splitContainer1.Panel1.Controls.Add(this.pbOrig);
            this.splitContainer1.Panel1.Controls.Add(this.pbDup);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lvDuplicates);
            this.splitContainer1.Panel2.Controls.Add(this.lvResults);
            this.splitContainer1.Size = new System.Drawing.Size(559, 386);
            this.splitContainer1.SplitterDistance = 190;
            this.splitContainer1.TabIndex = 12;
            // 
            // lvResults
            // 
            this.lvResults.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader5});
            this.lvResults.FullRowSelect = true;
            this.lvResults.GridLines = true;
            this.lvResults.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvResults.HideSelection = false;
            this.lvResults.Location = new System.Drawing.Point(0, 6);
            this.lvResults.Name = "lvResults";
            this.lvResults.Size = new System.Drawing.Size(367, 177);
            this.lvResults.TabIndex = 11;
            this.lvResults.UseCompatibleStateImageBehavior = false;
            this.lvResults.View = System.Windows.Forms.View.Details;
            this.lvResults.SelectedIndexChanged += new System.EventHandler(this.lvResults_SelectedIndexChanged);
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Name";
            this.columnHeader4.Width = 100;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Directory";
            this.columnHeader5.Width = 200;
            // 
            // btRemDup
            // 
            this.btRemDup.Image = global::DupliFinder.Properties.Resources.del;
            this.btRemDup.Location = new System.Drawing.Point(141, 330);
            this.btRemDup.Name = "btRemDup";
            this.btRemDup.Size = new System.Drawing.Size(36, 36);
            this.btRemDup.TabIndex = 14;
            this.btRemDup.UseVisualStyleBackColor = true;
            this.btRemDup.Click += new System.EventHandler(this.btRemDup_Click);
            // 
            // btRemOrig
            // 
            this.btRemOrig.Image = global::DupliFinder.Properties.Resources.del;
            this.btRemOrig.Location = new System.Drawing.Point(141, 147);
            this.btRemOrig.Name = "btRemOrig";
            this.btRemOrig.Size = new System.Drawing.Size(36, 36);
            this.btRemOrig.TabIndex = 13;
            this.btRemOrig.UseVisualStyleBackColor = true;
            this.btRemOrig.Click += new System.EventHandler(this.btRemOrig_Click);
            // 
            // lvDuplicates
            // 
            this.lvDuplicates.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvDuplicates.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader6});
            this.lvDuplicates.FullRowSelect = true;
            this.lvDuplicates.GridLines = true;
            this.lvDuplicates.HideSelection = false;
            this.lvDuplicates.Location = new System.Drawing.Point(0, 189);
            this.lvDuplicates.Name = "lvDuplicates";
            this.lvDuplicates.Size = new System.Drawing.Size(365, 197);
            this.lvDuplicates.TabIndex = 12;
            this.lvDuplicates.UseCompatibleStateImageBehavior = false;
            this.lvDuplicates.View = System.Windows.Forms.View.Details;
            this.lvDuplicates.SelectedIndexChanged += new System.EventHandler(this.lvDuplicates_SelectedIndexChanged);
            this.lvDuplicates.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvDuplicates_ColumnClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 170;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Size";
            this.columnHeader2.Width = 90;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Similarity (%)";
            this.columnHeader3.Width = 80;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Path";
            this.columnHeader6.Width = 200;
            // 
            // pbDup
            // 
            this.pbDup.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbDup.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbDup.Location = new System.Drawing.Point(0, 189);
            this.pbDup.Name = "pbDup";
            this.pbDup.Size = new System.Drawing.Size(177, 177);
            this.pbDup.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbDup.TabIndex = 9;
            this.pbDup.TabStop = false;
            this.pbDup.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pbOrig_MouseDoubleClick);
            // 
            // pbOrig
            // 
            this.pbOrig.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbOrig.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbOrig.Location = new System.Drawing.Point(0, 6);
            this.pbOrig.Name = "pbOrig";
            this.pbOrig.Size = new System.Drawing.Size(177, 177);
            this.pbOrig.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbOrig.TabIndex = 10;
            this.pbOrig.TabStop = false;
            this.pbOrig.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pbOrig_MouseDoubleClick);
            this.pbOrig.Resize += new System.EventHandler(this.pbOrig_Resize);
            // 
            // chName
            // 
            this.chName.Text = "Name";
            this.chName.Width = 170;
            // 
            // chSize
            // 
            this.chSize.Text = "Size";
            this.chSize.Width = 90;
            // 
            // chSimilarity
            // 
            this.chSimilarity.Text = "Similarity";
            this.chSimilarity.Width = 80;
            // 
            // frmMain
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(559, 532);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(538, 487);
            this.Name = "frmMain";
            this.Text = "DeadRinger";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSimilarityThreshold)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbDup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbOrig)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblStart;
        private System.Windows.Forms.TextBox tbPath;
        private System.Windows.Forms.Button btBrowse;
        private System.Windows.Forms.Button btSearch;
        private System.Windows.Forms.CheckBox cbIncSubfolders;
        private System.Windows.Forms.FolderBrowserDialog fd;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutDeadRingerToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar pbWorkingAll;
        private System.Windows.Forms.ToolStripStatusLabel lblWorking;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView lvResults;
        private System.Windows.Forms.Button btRemDup;
        private System.Windows.Forms.Button btRemOrig;
        private System.Windows.Forms.ListView lvDuplicates;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.PictureBox pbDup;
        private System.Windows.Forms.PictureBox pbOrig;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.ColumnHeader chSize;
        private System.Windows.Forms.ColumnHeader chSimilarity;
        private System.Windows.Forms.ComboBox cbAlgorithms;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar trackBarSimilarityThreshold;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelSimilarityThreshold;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
    }
}

