namespace DXLog.net
{
    partial class DXLogIcomControl
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
            this.RefLevelLabel = new System.Windows.Forms.Label();
            this.RefLevelSlider = new System.Windows.Forms.TrackBar();
            this.PwrLevelSlider = new System.Windows.Forms.TrackBar();
            this.PwrLevelLabel = new System.Windows.Forms.Label();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rangeLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.RefLevelSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PwrLevelSlider)).BeginInit();
            this.contextMenuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // RefLevelLabel
            // 
            this.RefLevelLabel.AutoSize = true;
            this.RefLevelLabel.Location = new System.Drawing.Point(4, 15);
            this.RefLevelLabel.Name = "RefLevelLabel";
            this.RefLevelLabel.Size = new System.Drawing.Size(65, 13);
            this.RefLevelLabel.TabIndex = 1;
            this.RefLevelLabel.Text = "REF: +20dB";
            // 
            // RefLevelSlider
            // 
            this.RefLevelSlider.LargeChange = 2;
            this.RefLevelSlider.Location = new System.Drawing.Point(68, 12);
            this.RefLevelSlider.Maximum = 20;
            this.RefLevelSlider.Minimum = -20;
            this.RefLevelSlider.Name = "RefLevelSlider";
            this.RefLevelSlider.Size = new System.Drawing.Size(116, 45);
            this.RefLevelSlider.TabIndex = 1;
            this.RefLevelSlider.Scroll += new System.EventHandler(this.OnRefSliderMouseClick);
            this.RefLevelSlider.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnRefSliderMouseClick);
            // 
            // PwrLevelSlider
            // 
            this.PwrLevelSlider.LargeChange = 2;
            this.PwrLevelSlider.Location = new System.Drawing.Point(68, 43);
            this.PwrLevelSlider.Maximum = 100;
            this.PwrLevelSlider.Name = "PwrLevelSlider";
            this.PwrLevelSlider.Size = new System.Drawing.Size(116, 45);
            this.PwrLevelSlider.TabIndex = 2;
            this.PwrLevelSlider.Scroll += new System.EventHandler(this.OnPwrSliderMouseClick);
            this.PwrLevelSlider.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnPwrSliderMouseClick);
            // 
            // PwrLevelLabel
            // 
            this.PwrLevelLabel.AutoSize = true;
            this.PwrLevelLabel.Location = new System.Drawing.Point(4, 46);
            this.PwrLevelLabel.Name = "PwrLevelLabel";
            this.PwrLevelLabel.Size = new System.Drawing.Size(65, 13);
            this.PwrLevelLabel.TabIndex = 4;
            this.PwrLevelLabel.Text = "PWR: 100%";
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.propertiesToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(128, 26);
            // 
            // propertiesToolStripMenuItem
            // 
            this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.propertiesToolStripMenuItem.Text = "Properties";
            this.propertiesToolStripMenuItem.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);
            // 
            // rangeLabel
            // 
            this.rangeLabel.AutoSize = true;
            this.rangeLabel.Location = new System.Drawing.Point(32, 75);
            this.rangeLabel.Name = "rangeLabel";
            this.rangeLabel.Size = new System.Drawing.Size(117, 13);
            this.rangeLabel.TabIndex = 5;
            this.rangeLabel.Text = "WF: 432,400 - 432,600";
            // 
            // DXLogIcomControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(197, 91);
            this.ContextMenuStrip = this.contextMenuStrip2;
            this.Controls.Add(this.rangeLabel);
            this.Controls.Add(this.PwrLevelLabel);
            this.Controls.Add(this.PwrLevelSlider);
            this.Controls.Add(this.RefLevelSlider);
            this.Controls.Add(this.RefLevelLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.FormID = 1000;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DXLogIcomControl";
            this.Text = "ICOM controller";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnClosing);
            ((System.ComponentModel.ISupportInitialize)(this.RefLevelSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PwrLevelSlider)).EndInit();
            this.contextMenuStrip2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label RefLevelLabel;
        private System.Windows.Forms.TrackBar RefLevelSlider;
        private System.Windows.Forms.TrackBar PwrLevelSlider;
        private System.Windows.Forms.Label PwrLevelLabel;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
        private System.Windows.Forms.Label rangeLabel;
    }
}