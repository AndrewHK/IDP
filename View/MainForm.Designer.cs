/*
 * Created by SharpDevelop.
 * User: Andrew
 * Date: 23-Jun-14
 * Time: 2:58 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace IDPParser.View
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.rumorMillTB = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.crawlBtn = new System.Windows.Forms.ToolStripButton();
            this.excelSheetGB = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.rumorTypeGB = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.interClubGB = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.rumorSrcGB = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.navGB = new System.Windows.Forms.GroupBox();
            this.timeSpent = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.countLB = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.maxLB = new System.Windows.Forms.Label();
            this.logTB = new System.Windows.Forms.TextBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.groupBox1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.excelSheetGB.SuspendLayout();
            this.rumorTypeGB.SuspendLayout();
            this.interClubGB.SuspendLayout();
            this.rumorSrcGB.SuspendLayout();
            this.navGB.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.toolStrip1);
            this.groupBox1.Controls.Add(this.excelSheetGB);
            this.groupBox1.Controls.Add(this.rumorTypeGB);
            this.groupBox1.Controls.Add(this.interClubGB);
            this.groupBox1.Controls.Add(this.rumorSrcGB);
            this.groupBox1.Controls.Add(this.navGB);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.logTB);
            this.groupBox1.Controls.Add(this.progressBar);
            this.groupBox1.Location = new System.Drawing.Point(2, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(882, 477);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rumorMillTB,
            this.toolStripSeparator1,
            this.crawlBtn});
            this.toolStrip1.Location = new System.Drawing.Point(3, 16);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(876, 25);
            this.toolStrip1.TabIndex = 12;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // rumorMillTB
            // 
            this.rumorMillTB.Name = "rumorMillTB";
            this.rumorMillTB.Size = new System.Drawing.Size(730, 25);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // crawlBtn
            // 
            this.crawlBtn.AutoSize = false;
            this.crawlBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.crawlBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.crawlBtn.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.crawlBtn.Image = ((System.Drawing.Image)(resources.GetObject("crawlBtn.Image")));
            this.crawlBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.crawlBtn.Name = "crawlBtn";
            this.crawlBtn.Size = new System.Drawing.Size(125, 22);
            this.crawlBtn.Text = "Crawl";
            this.crawlBtn.Click += new System.EventHandler(this.CrawlBtn_Click);
            // 
            // excelSheetGB
            // 
            this.excelSheetGB.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.excelSheetGB.Controls.Add(this.label5);
            this.excelSheetGB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.excelSheetGB.Location = new System.Drawing.Point(752, 394);
            this.excelSheetGB.Name = "excelSheetGB";
            this.excelSheetGB.Size = new System.Drawing.Size(121, 44);
            this.excelSheetGB.TabIndex = 11;
            this.excelSheetGB.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(14, 14);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 16);
            this.label5.TabIndex = 4;
            this.label5.Text = "Excel Sheet";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rumorTypeGB
            // 
            this.rumorTypeGB.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.rumorTypeGB.Controls.Add(this.label4);
            this.rumorTypeGB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rumorTypeGB.Location = new System.Drawing.Point(752, 333);
            this.rumorTypeGB.Name = "rumorTypeGB";
            this.rumorTypeGB.Size = new System.Drawing.Size(121, 44);
            this.rumorTypeGB.TabIndex = 10;
            this.rumorTypeGB.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(11, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 16);
            this.label4.TabIndex = 4;
            this.label4.Text = "Rumor Types";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // interClubGB
            // 
            this.interClubGB.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.interClubGB.Controls.Add(this.label2);
            this.interClubGB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.interClubGB.Location = new System.Drawing.Point(752, 271);
            this.interClubGB.Name = "interClubGB";
            this.interClubGB.Size = new System.Drawing.Size(121, 44);
            this.interClubGB.TabIndex = 9;
            this.interClubGB.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(4, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "Interested Clubs";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rumorSrcGB
            // 
            this.rumorSrcGB.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.rumorSrcGB.Controls.Add(this.label1);
            this.rumorSrcGB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rumorSrcGB.Location = new System.Drawing.Point(752, 209);
            this.rumorSrcGB.Name = "rumorSrcGB";
            this.rumorSrcGB.Size = new System.Drawing.Size(122, 44);
            this.rumorSrcGB.TabIndex = 8;
            this.rumorSrcGB.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "Rumor Sources";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // navGB
            // 
            this.navGB.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.navGB.Controls.Add(this.timeSpent);
            this.navGB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.navGB.Location = new System.Drawing.Point(752, 147);
            this.navGB.Name = "navGB";
            this.navGB.Size = new System.Drawing.Size(121, 44);
            this.navGB.TabIndex = 7;
            this.navGB.TabStop = false;
            // 
            // timeSpent
            // 
            this.timeSpent.AutoSize = true;
            this.timeSpent.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timeSpent.Location = new System.Drawing.Point(23, 16);
            this.timeSpent.Name = "timeSpent";
            this.timeSpent.Size = new System.Drawing.Size(73, 16);
            this.timeSpent.TabIndex = 4;
            this.timeSpent.Text = "Navigation";
            this.timeSpent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.countLB);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.maxLB);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(752, 46);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(121, 78);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            // 
            // countLB
            // 
            this.countLB.AutoSize = true;
            this.countLB.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.countLB.Location = new System.Drawing.Point(47, 12);
            this.countLB.Name = "countLB";
            this.countLB.Size = new System.Drawing.Size(19, 20);
            this.countLB.TabIndex = 3;
            this.countLB.Text = "0";
            this.countLB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "---------------------------";
            // 
            // maxLB
            // 
            this.maxLB.AutoSize = true;
            this.maxLB.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.maxLB.Location = new System.Drawing.Point(47, 53);
            this.maxLB.Name = "maxLB";
            this.maxLB.Size = new System.Drawing.Size(19, 20);
            this.maxLB.TabIndex = 4;
            this.maxLB.Text = "0";
            this.maxLB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // logTB
            // 
            this.logTB.Location = new System.Drawing.Point(6, 52);
            this.logTB.Multiline = true;
            this.logTB.Name = "logTB";
            this.logTB.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.logTB.Size = new System.Drawing.Size(740, 387);
            this.logTB.TabIndex = 1;
            // 
            // progressBar
            // 
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar.Location = new System.Drawing.Point(3, 452);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(876, 22);
            this.progressBar.TabIndex = 2;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(889, 482);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TransferMarkt Parser";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.excelSheetGB.ResumeLayout(false);
            this.excelSheetGB.PerformLayout();
            this.rumorTypeGB.ResumeLayout(false);
            this.rumorTypeGB.PerformLayout();
            this.interClubGB.ResumeLayout(false);
            this.interClubGB.PerformLayout();
            this.rumorSrcGB.ResumeLayout(false);
            this.rumorSrcGB.PerformLayout();
            this.navGB.ResumeLayout(false);
            this.navGB.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

		}
        private System.Windows.Forms.TextBox logTB;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label countLB;
        private System.Windows.Forms.Label maxLB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox navGB;
        private System.Windows.Forms.Label timeSpent;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox interClubGB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox rumorSrcGB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox rumorTypeGB;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox excelSheetGB;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripTextBox rumorMillTB;
        private System.Windows.Forms.ToolStripButton crawlBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
	}
}
