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

        public IcomProperties()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;

            for (int i = 1; i <= 4; i++)
            {
                edgeSelectionDropDown.Items.Add(i.ToString());
            }

            DefaultValues def = new DefaultValues();


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
