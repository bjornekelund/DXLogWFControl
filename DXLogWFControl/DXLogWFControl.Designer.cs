namespace DXLog.net
{
    partial class DXLogWFControl
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
            this.RefLevelLabel1 = new System.Windows.Forms.Label();
            this.RefLevelSlider1 = new System.Windows.Forms.TrackBar();
            this.PwrLevelSlider1 = new System.Windows.Forms.TrackBar();
            this.PwrLevelLabel1 = new System.Windows.Forms.Label();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rangeLabel1 = new System.Windows.Forms.Label();
            this.rangeLabel2 = new System.Windows.Forms.Label();
            this.PwrLevelLabel2 = new System.Windows.Forms.Label();
            this.PwrLevelSlider2 = new System.Windows.Forms.TrackBar();
            this.RefLevelSlider2 = new System.Windows.Forms.TrackBar();
            this.RefLevelLabel2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.RefLevelSlider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PwrLevelSlider1)).BeginInit();
            this.contextMenuStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PwrLevelSlider2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RefLevelSlider2)).BeginInit();
            this.SuspendLayout();
            // 
            // RefLevelLabel1
            // 
            this.RefLevelLabel1.AutoSize = true;
            this.RefLevelLabel1.Location = new System.Drawing.Point(6, 23);
            this.RefLevelLabel1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.RefLevelLabel1.Name = "RefLevelLabel1";
            this.RefLevelLabel1.Size = new System.Drawing.Size(97, 20);
            this.RefLevelLabel1.TabIndex = 1;
            this.RefLevelLabel1.Text = "REF: +20dB";
            // 
            // RefLevelSlider1
            // 
            this.RefLevelSlider1.LargeChange = 1;
            this.RefLevelSlider1.Location = new System.Drawing.Point(102, 18);
            this.RefLevelSlider1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.RefLevelSlider1.Maximum = 20;
            this.RefLevelSlider1.Minimum = -20;
            this.RefLevelSlider1.Name = "RefLevelSlider1";
            this.RefLevelSlider1.Size = new System.Drawing.Size(174, 69);
            this.RefLevelSlider1.TabIndex = 1;
            this.RefLevelSlider1.Scroll += new System.EventHandler(this.RefLevelSlider1_Scroll);
            this.RefLevelSlider1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.RefLevelSlider1_MouseUp);
            // 
            // PwrLevelSlider1
            // 
            this.PwrLevelSlider1.LargeChange = 1;
            this.PwrLevelSlider1.Location = new System.Drawing.Point(102, 66);
            this.PwrLevelSlider1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.PwrLevelSlider1.Maximum = 100;
            this.PwrLevelSlider1.Name = "PwrLevelSlider1";
            this.PwrLevelSlider1.Size = new System.Drawing.Size(174, 69);
            this.PwrLevelSlider1.TabIndex = 2;
            this.PwrLevelSlider1.Scroll += new System.EventHandler(this.OnPwrLevelSlider1_Scroll);
            this.PwrLevelSlider1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnPwrLevelSlider1_MouseUp);
            // 
            // PwrLevelLabel1
            // 
            this.PwrLevelLabel1.AutoSize = true;
            this.PwrLevelLabel1.Location = new System.Drawing.Point(6, 71);
            this.PwrLevelLabel1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.PwrLevelLabel1.Name = "PwrLevelLabel1";
            this.PwrLevelLabel1.Size = new System.Drawing.Size(95, 20);
            this.PwrLevelLabel1.TabIndex = 4;
            this.PwrLevelLabel1.Text = "PWR: 100%";
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.propertiesToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(165, 36);
            // 
            // propertiesToolStripMenuItem
            // 
            this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(164, 32);
            this.propertiesToolStripMenuItem.Text = "Properties";
            this.propertiesToolStripMenuItem.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);
            // 
            // rangeLabel1
            // 
            this.rangeLabel1.AutoSize = true;
            this.rangeLabel1.Location = new System.Drawing.Point(48, 115);
            this.rangeLabel1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.rangeLabel1.Name = "rangeLabel1";
            this.rangeLabel1.Size = new System.Drawing.Size(171, 20);
            this.rangeLabel1.TabIndex = 5;
            this.rangeLabel1.Text = "WF: 432,400 - 432,600";
            // 
            // rangeLabel2
            // 
            this.rangeLabel2.AutoSize = true;
            this.rangeLabel2.Location = new System.Drawing.Point(294, 115);
            this.rangeLabel2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.rangeLabel2.Name = "rangeLabel2";
            this.rangeLabel2.Size = new System.Drawing.Size(171, 20);
            this.rangeLabel2.TabIndex = 10;
            this.rangeLabel2.Text = "WF: 432,400 - 432,600";
            // 
            // PwrLevelLabel2
            // 
            this.PwrLevelLabel2.AutoSize = true;
            this.PwrLevelLabel2.Location = new System.Drawing.Point(273, 71);
            this.PwrLevelLabel2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.PwrLevelLabel2.Name = "PwrLevelLabel2";
            this.PwrLevelLabel2.Size = new System.Drawing.Size(50, 20);
            this.PwrLevelLabel2.TabIndex = 9;
            this.PwrLevelLabel2.Text = "100%";
            // 
            // PwrLevelSlider2
            // 
            this.PwrLevelSlider2.LargeChange = 1;
            this.PwrLevelSlider2.Location = new System.Drawing.Point(325, 66);
            this.PwrLevelSlider2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.PwrLevelSlider2.Maximum = 100;
            this.PwrLevelSlider2.Name = "PwrLevelSlider2";
            this.PwrLevelSlider2.Size = new System.Drawing.Size(174, 69);
            this.PwrLevelSlider2.TabIndex = 8;
            this.PwrLevelSlider2.Scroll += new System.EventHandler(this.OnPwrLevelSlider2_Scroll);
            this.PwrLevelSlider2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnPwrLevelSlider2_MouseUp);
            // 
            // RefLevelSlider2
            // 
            this.RefLevelSlider2.LargeChange = 1;
            this.RefLevelSlider2.Location = new System.Drawing.Point(325, 18);
            this.RefLevelSlider2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.RefLevelSlider2.Maximum = 20;
            this.RefLevelSlider2.Minimum = -20;
            this.RefLevelSlider2.Name = "RefLevelSlider2";
            this.RefLevelSlider2.Size = new System.Drawing.Size(174, 69);
            this.RefLevelSlider2.TabIndex = 6;
            this.RefLevelSlider2.Scroll += new System.EventHandler(this.RefLevelSlider2_Scroll);
            this.RefLevelSlider2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.RefLevelSlider2_MouseUp);
            // 
            // RefLevelLabel2
            // 
            this.RefLevelLabel2.AutoSize = true;
            this.RefLevelLabel2.Location = new System.Drawing.Point(273, 23);
            this.RefLevelLabel2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.RefLevelLabel2.Name = "RefLevelLabel2";
            this.RefLevelLabel2.Size = new System.Drawing.Size(56, 20);
            this.RefLevelLabel2.TabIndex = 7;
            this.RefLevelLabel2.Text = "+20dB";
            // 
            // DXLogWFControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 140);
            this.ContextMenuStrip = this.contextMenuStrip2;
            this.Controls.Add(this.rangeLabel2);
            this.Controls.Add(this.PwrLevelLabel2);
            this.Controls.Add(this.PwrLevelSlider2);
            this.Controls.Add(this.RefLevelSlider2);
            this.Controls.Add(this.RefLevelLabel2);
            this.Controls.Add(this.rangeLabel1);
            this.Controls.Add(this.PwrLevelLabel1);
            this.Controls.Add(this.PwrLevelSlider1);
            this.Controls.Add(this.RefLevelSlider1);
            this.Controls.Add(this.RefLevelLabel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.FormID = 1000;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DXLogWFControl";
            this.Text = "Waterfall controller";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnClosing);
            ((System.ComponentModel.ISupportInitialize)(this.RefLevelSlider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PwrLevelSlider1)).EndInit();
            this.contextMenuStrip2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PwrLevelSlider2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RefLevelSlider2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label RefLevelLabel1;
        private System.Windows.Forms.TrackBar RefLevelSlider1;
        private System.Windows.Forms.TrackBar PwrLevelSlider1;
        private System.Windows.Forms.Label PwrLevelLabel1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
        private System.Windows.Forms.Label rangeLabel1;
        private System.Windows.Forms.Label rangeLabel2;
        private System.Windows.Forms.Label PwrLevelLabel2;
        private System.Windows.Forms.TrackBar PwrLevelSlider2;
        private System.Windows.Forms.TrackBar RefLevelSlider2;
        private System.Windows.Forms.Label RefLevelLabel2;
    }
}