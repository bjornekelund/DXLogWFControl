using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using ConfigFile;

namespace DXLog.net
{
    public partial class IcomProperties : Form
    {
        public RadioSettings Settings; // Why is this not accessible from DXLogWFControl??

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

            refreshTable();
        }

        private void refreshTable()
        {
            edgeSelectionDropDown.SelectedIndex = Settings.EdgeSet - 1;
            useScrollModeCheckBox.Checked = Settings.Scrolling;

            for (int i = 0; i < Settings.Bands; i++)
            {
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
            try
            {
                for (int i = 0; i < Settings.Bands; i++)
                {
                    TextBox tbcwl = (TextBox)Controls.Find(string.Format("tbcwl{0}", i), true)[0];
                    TextBox tbcwu = (TextBox)Controls.Find(string.Format("tbcwu{0}", i), true)[0];
                    Settings.LowerEdgeCW[i] = int.Parse(tbcwl.Text);
                    Settings.UpperEdgeCW[i] = int.Parse(tbcwu.Text);

                    TextBox tbphl = (TextBox)Controls.Find(string.Format("tbphl{0}", i), true)[0];
                    TextBox tbphu = (TextBox)Controls.Find(string.Format("tbphu{0}", i), true)[0];
                    Settings.LowerEdgePhone[i] = int.Parse(tbphl.Text);
                    Settings.UpperEdgePhone[i] = int.Parse(tbphu.Text);

                    TextBox tbdgl = (TextBox)Controls.Find(string.Format("tbdgl{0}", i), true)[0];
                    TextBox tbdgu = (TextBox)Controls.Find(string.Format("tbdgu{0}", i), true)[0];
                    Settings.LowerEdgeDigital[i] = int.Parse(tbdgl.Text);
                    Settings.UpperEdgeDigital[i] = int.Parse(tbdgu.Text);
                }
            }
            catch
            {
                MessageBox.Show("Invalid entry", "ICOM control properties", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Config.Save("WaterfallEdgeSet", edgeSelectionDropDown.SelectedIndex + 1);
            Config.Save("WaterfallScrolling", useScrollModeCheckBox.Checked);

            Config.Save("WaterfallLowerEdgeCW", string.Join(";", Settings.LowerEdgeCW.Select(i => i.ToString()).ToArray()));
            Config.Save("WaterfallUpperEdgeCW", string.Join(";", Settings.UpperEdgeCW.Select(i => i.ToString()).ToArray()));
            Config.Save("WaterfallLowerEdgePhone", string.Join(";", Settings.LowerEdgePhone.Select(i => i.ToString()).ToArray()));
            Config.Save("WaterfallUpperEdgePhone", string.Join(";", Settings.UpperEdgePhone.Select(i => i.ToString()).ToArray()));
            Config.Save("WaterfallLowerEdgeDigital", string.Join(";", Settings.LowerEdgeDigital.Select(i => i.ToString()).ToArray()));
            Config.Save("WaterfallUpperEdgeDigital", string.Join(";", Settings.UpperEdgeDigital.Select(i => i.ToString()).ToArray()));

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnDefaults_Click(object sender, EventArgs e)
        {
            DefaultRadioSettings def = new DefaultRadioSettings();

            Settings.LowerEdgeCW = def.LowerEdgeCW.Split(';').Select(s => int.Parse(s)).ToArray();
            Settings.UpperEdgeCW = def.UpperEdgeCW.Split(';').Select(s => int.Parse(s)).ToArray();
            Settings.RefLevelCW = def.RefLevelCW.Split(';').Select(s => int.Parse(s)).ToArray();
            Settings.PwrLevelCW = def.PwrLevelCW.Split(';').Select(s => int.Parse(s)).ToArray();

            Settings.LowerEdgePhone = def.LowerEdgePhone.Split(';').Select(s => int.Parse(s)).ToArray();
            Settings.UpperEdgePhone = def.UpperEdgePhone.Split(';').Select(s => int.Parse(s)).ToArray();
            Settings.RefLevelPhone = def.RefLevelPhone.Split(';').Select(s => int.Parse(s)).ToArray();
            Settings.PwrLevelPhone = def.PwrLevelPhone.Split(';').Select(s => int.Parse(s)).ToArray();

            Settings.LowerEdgeDigital = def.LowerEdgeDigital.Split(';').Select(s => int.Parse(s)).ToArray();
            Settings.UpperEdgeDigital = def.UpperEdgeDigital.Split(';').Select(s => int.Parse(s)).ToArray();
            Settings.RefLevelDigital = def.RefLevelDigital.Split(';').Select(s => int.Parse(s)).ToArray();
            Settings.PwrLevelDigital = def.PwrLevelDigital.Split(';').Select(s => int.Parse(s)).ToArray();

            refreshTable();
        }
    }
}
