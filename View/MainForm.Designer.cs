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
			this.crawlBtn = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.rumorMillTB = new System.Windows.Forms.TextBox();
			this.logTB = new System.Windows.Forms.TextBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// crawlBtn
			// 
			this.crawlBtn.Location = new System.Drawing.Point(6, 413);
			this.crawlBtn.Name = "crawlBtn";
			this.crawlBtn.Size = new System.Drawing.Size(487, 23);
			this.crawlBtn.TabIndex = 0;
			this.crawlBtn.Text = "Crawl";
			this.crawlBtn.UseVisualStyleBackColor = true;
			this.crawlBtn.Click += new System.EventHandler(this.CrawlBtnClick);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.logTB);
			this.groupBox1.Controls.Add(this.rumorMillTB);
			this.groupBox1.Controls.Add(this.crawlBtn);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(493, 442);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "groupBox1";
			// 
			// rumorMillTB
			// 
			this.rumorMillTB.Location = new System.Drawing.Point(6, 19);
			this.rumorMillTB.Name = "rumorMillTB";
			this.rumorMillTB.Size = new System.Drawing.Size(481, 20);
			this.rumorMillTB.TabIndex = 0;
			// 
			// logTB
			// 
			this.logTB.Location = new System.Drawing.Point(6, 45);
			this.logTB.Multiline = true;
			this.logTB.Name = "logTB";
			this.logTB.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.logTB.Size = new System.Drawing.Size(481, 362);
			this.logTB.TabIndex = 1;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(517, 466);
			this.Controls.Add(this.groupBox1);
			this.Name = "MainForm";
			this.Text = "IDP Parser";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.TextBox logTB;
		private System.Windows.Forms.Button crawlBtn;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox rumorMillTB;
	}
}
