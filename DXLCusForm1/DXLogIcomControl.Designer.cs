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
            this.BandLabel = new System.Windows.Forms.Label();
            this.ModeLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.LowerEdgeTextbox = new System.Windows.Forms.TextBox();
            this.UpperEdgeTextbox = new System.Windows.Forms.TextBox();
            this.BandModeButton = new System.Windows.Forms.Button();
            this.ZoomButton = new System.Windows.Forms.Button();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.RefLevelSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PwrLevelSlider)).BeginInit();
            this.contextMenuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // RefLevelLabel
            // 
            this.RefLevelLabel.AutoSize = true;
            this.RefLevelLabel.Location = new System.Drawing.Point(12, 12);
            this.RefLevelLabel.Name = "RefLevelLabel";
            this.RefLevelLabel.Size = new System.Drawing.Size(52, 13);
            this.RefLevelLabel.TabIndex = 1;
            this.RefLevelLabel.Text = "Ref: ---dB";
            // 
            // RefLevelSlider
            // 
            this.RefLevelSlider.LargeChange = 2;
            this.RefLevelSlider.Location = new System.Drawing.Point(72, 8);
            this.RefLevelSlider.Minimum = -20;
            this.RefLevelSlider.Name = "RefLevelSlider";
            this.RefLevelSlider.Size = new System.Drawing.Size(116, 45);
            this.RefLevelSlider.TabIndex = 1;
            this.RefLevelSlider.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnRefSliderMouseClick);
            // 
            // PwrLevelSlider
            // 
            this.PwrLevelSlider.LargeChange = 2;
            this.PwrLevelSlider.Location = new System.Drawing.Point(248, 8);
            this.PwrLevelSlider.Maximum = 100;
            this.PwrLevelSlider.Name = "PwrLevelSlider";
            this.PwrLevelSlider.Size = new System.Drawing.Size(116, 45);
            this.PwrLevelSlider.TabIndex = 2;
            this.PwrLevelSlider.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnPwrSliderMouseClick);
            // 
            // PwrLevelLabel
            // 
            this.PwrLevelLabel.AutoSize = true;
            this.PwrLevelLabel.Location = new System.Drawing.Point(194, 12);
            this.PwrLevelLabel.Name = "PwrLevelLabel";
            this.PwrLevelLabel.Size = new System.Drawing.Size(48, 13);
            this.PwrLevelLabel.TabIndex = 4;
            this.PwrLevelLabel.Text = "Pwr: ---%";
            this.PwrLevelLabel.Click += new System.EventHandler(this.ToggleBarefoot);
            // 
            // BandLabel
            // 
            this.BandLabel.AutoSize = true;
            this.BandLabel.Location = new System.Drawing.Point(382, 12);
            this.BandLabel.Name = "BandLabel";
            this.BandLabel.Size = new System.Drawing.Size(32, 13);
            this.BandLabel.TabIndex = 5;
            this.BandLabel.Text = "Band";
            // 
            // ModeLabel
            // 
            this.ModeLabel.AutoSize = true;
            this.ModeLabel.Location = new System.Drawing.Point(380, 40);
            this.ModeLabel.Name = "ModeLabel";
            this.ModeLabel.Size = new System.Drawing.Size(34, 13);
            this.ModeLabel.TabIndex = 6;
            this.ModeLabel.Text = "Mode";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Lower:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(178, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "kHz  Upper:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(353, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "kHz";
            // 
            // LowerEdgeTextbox
            // 
            this.LowerEdgeTextbox.Location = new System.Drawing.Point(72, 47);
            this.LowerEdgeTextbox.Name = "LowerEdgeTextbox";
            this.LowerEdgeTextbox.Size = new System.Drawing.Size(100, 20);
            this.LowerEdgeTextbox.TabIndex = 3;
            // 
            // UpperEdgeTextbox
            // 
            this.UpperEdgeTextbox.Location = new System.Drawing.Point(248, 47);
            this.UpperEdgeTextbox.Name = "UpperEdgeTextbox";
            this.UpperEdgeTextbox.Size = new System.Drawing.Size(99, 20);
            this.UpperEdgeTextbox.TabIndex = 4;
            // 
            // BandModeButton
            // 
            this.BandModeButton.Location = new System.Drawing.Point(15, 73);
            this.BandModeButton.Name = "BandModeButton";
            this.BandModeButton.Size = new System.Drawing.Size(75, 23);
            this.BandModeButton.TabIndex = 5;
            this.BandModeButton.Text = "Band+Mode";
            this.BandModeButton.UseVisualStyleBackColor = true;
            this.BandModeButton.Click += new System.EventHandler(this.OnBandModeButton);
            // 
            // ZoomButton
            // 
            this.ZoomButton.Location = new System.Drawing.Point(97, 73);
            this.ZoomButton.Name = "ZoomButton";
            this.ZoomButton.Size = new System.Drawing.Size(75, 23);
            this.ZoomButton.TabIndex = 6;
            this.ZoomButton.Text = "Zoom";
            this.ZoomButton.UseVisualStyleBackColor = true;
            this.ZoomButton.Click += new System.EventHandler(this.OnZoomButton);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.propertiesToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(181, 48);
            // 
            // propertiesToolStripMenuItem
            // 
            this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.propertiesToolStripMenuItem.Text = "Properties";
            this.propertiesToolStripMenuItem.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);
            // 
            // DXLogIcomControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(519, 102);
            this.ContextMenuStrip = this.contextMenuStrip2;
            this.Controls.Add(this.ZoomButton);
            this.Controls.Add(this.BandModeButton);
            this.Controls.Add(this.UpperEdgeTextbox);
            this.Controls.Add(this.LowerEdgeTextbox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ModeLabel);
            this.Controls.Add(this.BandLabel);
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
        private System.Windows.Forms.Label BandLabel;
        private System.Windows.Forms.Label ModeLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox LowerEdgeTextbox;
        private System.Windows.Forms.TextBox UpperEdgeTextbox;
        private System.Windows.Forms.Button BandModeButton;
        private System.Windows.Forms.Button ZoomButton;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
    }
}