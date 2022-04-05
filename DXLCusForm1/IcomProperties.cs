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
        Settings _set;

        public IcomProperties(Settings sett)
        {
            InitializeComponent();

            DialogResult = DialogResult.Cancel;

            _set = sett;

            for (int i = 1; i <= 4; i++)
            {
                edgeSelectionDropDown.Items.Add(i.ToString());
            }

            DefaultSettings def = new DefaultSettings();


            edgeSelectionDropDown.SelectedIndex = Config.Read("ICOMedgeSet", def.EdgeSet) - 1;
            useScrollModeCheckBox.Checked = Config.Read("ICOMuseScroll", def.UseScrolling);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Config.Save("ICOMedgeSet", edgeSelectionDropDown.SelectedIndex + 1);
            Config.Save("ICOMuseScroll", useScrollModeCheckBox.Checked);

            //int count = 11;
            //for (int i = 0; i <= count; i++)
            //{
            //    TextBox textbox = (TextBox)Controls.Find(string.Format("tbox{0}", i), false).FirstOrDefault();
            //    string s = textbox.Text;
            //}

            Config.Save("WaterfallLowerEdgeCW", string.Join(";", _set.lowerEdgeCW.Select(i => i.ToString()).ToArray()));
            Config.Save("WaterfallUpperEdgeCW", string.Join(";", _set.upperEdgeCW.Select(i => i.ToString()).ToArray()));
            Config.Read("WaterfallRefCW", string.Join(";", _set.refLevelCW.Select(i => i.ToString()).ToArray()));
            Config.Read("TransmitPowerCW", string.Join(";", _set.pwrLevelCW.Select(i => i.ToString()).ToArray()));

            Config.Read("WaterfallLowerEdgePhone", string.Join(";", _set.lowerEdgePhone.Select(i => i.ToString()).ToArray()));
            Config.Read("WaterfallUpperEdgePhone", string.Join(";", _set.upperEdgePhone.Select(i => i.ToString()).ToArray()));
            Config.Read("WaterfallRefPhone", string.Join(";", _set.refLevelPhone.Select(i => i.ToString()).ToArray()));
            Config.Read("TransmitPowerPhone", string.Join(";", _set.pwrLevelPhone.Select(i => i.ToString()).ToArray()));

            Config.Read("WaterfallLowerEdgeDigital", string.Join(";", _set.lowerEdgeDigital.Select(i => i.ToString()).ToArray()));
            Config.Read("WaterfallUpperEdgeDigital", string.Join(";", _set.upperEdgeDigital.Select(i => i.ToString()).ToArray()));
            Config.Read("WaterfallRefDigital", string.Join(";", _set.refLevelDigital.Select(i => i.ToString()).ToArray()));
            Config.Read("TransmitPowerDigital", string.Join(";", _set.pwrLevelDigital.Select(i => i.ToString()).ToArray()));

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
