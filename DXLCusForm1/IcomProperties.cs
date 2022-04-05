using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ConfigFile;

namespace DXLog.net
{
    public partial class IcomProperties : Form
    {
        public RadioSettings Settings;
        public int zz;

        public IcomProperties()
        {
            InitializeComponent();
        }

        public IcomProperties(RadioSettings sett)
        {
            InitializeComponent();

            DialogResult = DialogResult.Cancel;

            Settings = sett;

            for (int i = 1; i <= 4; i++)
            {
                edgeSelectionDropDown.Items.Add(i.ToString());
            }

            edgeSelectionDropDown.SelectedIndex = Settings.EdgeSet - 1;
            useScrollModeCheckBox.Checked = Settings.Scrolling;

            for (int i = 0; i < Settings.Bands; i++)
            {
                //Button mbtn = (Button)(Controls.Find("btnMsg" + btn.LabelName, true)[0]);

                TextBox tbcwl = (TextBox)Controls.Find(string.Format("tbcwl{0}", i), true)[0];
                TextBox tbcwu = (TextBox)Controls.Find(string.Format("tbcwu{0}", i), true)[0];
                TextBox tbphl = (TextBox)Controls.Find(string.Format("tbphl{0}", i), true)[0];
                TextBox tbphu = (TextBox)Controls.Find(string.Format("tbphu{0}", i), true)[0];
                TextBox tbdgl = (TextBox)Controls.Find(string.Format("tbdgl{0}", i), true)[0];
                TextBox tbdgu = (TextBox)Controls.Find(string.Format("tbdgu{0}", i), true)[0];

                tbcwl.Text = Settings.LowerEdgeCW[i].ToString();
                tbcwu.Text = Settings.UpperEdgeCW[i].ToString();
                tbphl.Text = Settings.LowerEdgePhone[i].ToString();
                tbphu.Text = Settings.UpperEdgePhone[i].ToString();
                tbdgl.Text = Settings.LowerEdgeDigital[i].ToString();
                tbdgu.Text = Settings.UpperEdgeDigital[i].ToString();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Config.Save("WaterfallEdgeSet", edgeSelectionDropDown.SelectedIndex + 1);
            Config.Save("WaterfallScrolling", useScrollModeCheckBox.Checked);

            try
            {
                for (int i = 0; i < Settings.Bands; i++)
                {
                    //Button mbtn = (Button)(Controls.Find("btnMsg" + btn.LabelName, true)[0]);

                    TextBox tbcwl = (TextBox)Controls.Find(string.Format("tbcwl{0}", i), true)[0];
                    TextBox tbcwu = (TextBox)Controls.Find(string.Format("tbcwu{0}", i), true)[0];
                    TextBox tbphl = (TextBox)Controls.Find(string.Format("tbphl{0}", i), true)[0];
                    TextBox tbphu = (TextBox)Controls.Find(string.Format("tbphu{0}", i), true)[0];
                    TextBox tbdgl = (TextBox)Controls.Find(string.Format("tbdgl{0}", i), true)[0];
                    TextBox tbdgu = (TextBox)Controls.Find(string.Format("tbdgu{0}", i), true)[0];

                    Settings.LowerEdgeCW[i] = int.Parse(tbcwl.Text);
                    Settings.UpperEdgeCW[i] = int.Parse(tbcwu.Text);
                    Settings.LowerEdgePhone[i] = int.Parse(tbphl.Text);
                    Settings.UpperEdgePhone[i] = int.Parse(tbphu.Text);
                    Settings.LowerEdgeDigital[i] = int.Parse(tbdgl.Text);
                    Settings.UpperEdgeDigital[i] = int.Parse(tbdgu.Text);
                }
            }
            catch
            {
                MessageBox.Show("Invalid entry", "ICOM control properties", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //int count = 11;
            //for (int i = 0; i <= count; i++)
            //{
            //    TextBox textbox = (TextBox)Controls.Find(string.Format("tbox{0}", i), false).FirstOrDefault();
            //    string s = textbox.Text;
            //}

            Config.Save("WaterfallLowerEdgeCW", string.Join(";", Settings.LowerEdgeCW.Select(i => i.ToString()).ToArray()));
            Config.Save("WaterfallUpperEdgeCW", string.Join(";", Settings.UpperEdgeCW.Select(i => i.ToString()).ToArray()));
            Config.Save("WaterfallRefCW", string.Join(";", Settings.RefLevelCW.Select(i => i.ToString()).ToArray()));
            Config.Save("TransmitPowerCW", string.Join(";", Settings.PwrLevelCW.Select(i => i.ToString()).ToArray()));

            Config.Save("WaterfallLowerEdgePhone", string.Join(";", Settings.LowerEdgePhone.Select(i => i.ToString()).ToArray()));
            Config.Save("WaterfallUpperEdgePhone", string.Join(";", Settings.UpperEdgePhone.Select(i => i.ToString()).ToArray()));
            Config.Save("WaterfallRefPhone", string.Join(";", Settings.RefLevelPhone.Select(i => i.ToString()).ToArray()));
            Config.Save("TransmitPowerPhone", string.Join(";", Settings.PwrLevelPhone.Select(i => i.ToString()).ToArray()));

            Config.Save("WaterfallLowerEdgeDigital", string.Join(";", Settings.LowerEdgeDigital.Select(i => i.ToString()).ToArray()));
            Config.Save("WaterfallUpperEdgeDigital", string.Join(";", Settings.UpperEdgeDigital.Select(i => i.ToString()).ToArray()));
            Config.Save("WaterfallRefDigital", string.Join(";", Settings.RefLevelDigital.Select(i => i.ToString()).ToArray()));
            Config.Save("TransmitPowerDigital", string.Join(";", Settings.PwrLevelDigital.Select(i => i.ToString()).ToArray()));

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void edgeSelectionDropDown_SelectedValueChanged(object sender, EventArgs e)
        {

        }
    }
}
