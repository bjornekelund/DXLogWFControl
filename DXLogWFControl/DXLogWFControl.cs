﻿using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using ConfigFile;
using IOComm;

namespace DXLog.net
{
    public partial class DXLogWFControl : KForm
    {
        public static string CusWinName
        {
            get { return "Waterfall Controller"; }
        }

        public static int CusFormID
        {
            get { return 1023; }
        }
        
        private ContestData cdata = null;

        private FrmMain mainForm = null;

        private enum RadioType
        {
            None,
            ICOM,
            ElecraftKP3,
            ElecraftK4,
            Kenwood,
            Yaesu
        }

        // Pre-baked CI-V commands
        private byte[] CIVSetFixedModeMain = { 0x27, 0x14, 0x00, 0x01 };
        private byte[] CIVSetFixedModeSub = { 0x27, 0x14, 0x01, 0x01 };

        private byte[] CIVSetEdgeSetMain = { 0x27, 0x16, 0x00, 0xff };
        private byte[] CIVSetEdgeSetSub = { 0x27, 0x16, 0x01, 0xff };

        private byte[] CIVSetRefLevelMain = { 0x27, 0x19, 0x00, 0x00, 0x00, 0x00 };
        private byte[] CIVSetRefLevelSub = { 0x27, 0x19, 0x01, 0x00, 0x00, 0x00 };

        private byte[] CIVSetPwrLevel = { 0x14, 0x0a, 0x00, 0x00};

        private const int MaxMHz = 470;
        private const int TableSize = 74;

        // Maps MHz to internal band index.
        // Bands are 160=0, 80=1, etc. up to 13=70cm
        private int[] bandIndex = new int[MaxMHz];
        private readonly int[] REFbandIndex = new int[TableSize]
            { 0, 0, 0, 1, 1, 2, 3, 3, 3, 4,
            4, 4, 4, 5, 5, 5, 5, 6, 6, 6,
            7, 7, 7, 7, 8, 8, 8, 9, 9, 9,
            9, 9, 9, 9, 9, 9, 9, 9, 9, 9,
            9, 10, 10, 10, 10, 10, 10, 10, 10, 10,
            10, 10, 10, 10, 10, 10, 10, 10, 10, 10,
            10, 11, 11, 11, 11, 11, 11, 11, 11, 11,
            11, 11, 11, 11 };

        // Maps actual MHz to radio's scope edge set.
        private int[] RadioEdgeSet = new int[MaxMHz];
        private readonly int[] REFRadioEdgeSet = new int[TableSize]
            { 1, 2, 3, 3, 3, 3, 4, 4, 5, 5,
            5, 6, 6, 6, 6, 7, 7, 7, 7, 7,
            8, 8, 9, 9, 9, 9, 10, 10, 10, 10,
            11, 11, 11, 11, 11, 11, 11, 11, 11, 11,
            11, 11, 11, 11, 11, 12, 12, 12, 12, 12,
            12, 12, 12, 12, 12, 12, 12, 12, 12, 12,
            13, 13, 13, 13, 13, 13, 13, 13, 13, 13,
            13, 13, 13, 13 };

        private RadioType [] Radiomodel = { RadioType.None, RadioType.None };

        CATCommon[] Radioobject = { null, null };

        int[] CurrentRefLevel = { 0, 0 };
        int[] CurrentPwrLevel = { 0, 0 };
        int[] CurrentMHz = { 0, 0 };
        string[] CurrentMode = { string.Empty, string.Empty };

        RadioSettings Set = new RadioSettings();
        DefaultRadioSettings Def = new DefaultRadioSettings();

        public DXLogWFControl()
        {
            InitializeComponent();
        }

        public DXLogWFControl(ContestData cdata)
        {
            InitializeComponent();
            this.cdata = cdata;

            while (contextMenuStrip1.Items.Count > 0)
                contextMenuStrip2.Items.Add(contextMenuStrip1.Items[0]);
            contextMenuStrip2.Items.RemoveByKey("fixWindowSizeToolStripMenuItem");
            contextMenuStrip2.Items.RemoveByKey("fontSizeToolStripMenuItem");
            contextMenuStrip2.Items.RemoveByKey("colorsToolStripMenuItem");

            // Set the decoding arrays to default
            for (int MHz = 0; MHz < MaxMHz; MHz++)
            {
                bandIndex[MHz] = 1;
                RadioEdgeSet[MHz] = 1;
            }

            // Initialize using tables
            for (int MHz = 0; MHz < TableSize; MHz++)
            {
                bandIndex[MHz] = REFbandIndex[MHz];
                RadioEdgeSet[MHz] = REFRadioEdgeSet[MHz];
            }

            // Add 2m
            for (int MHz = 137; MHz < 200; MHz++)
            {
                bandIndex[MHz] = 12;
                RadioEdgeSet[MHz] = 16;
            }

            // Add 70cm
            for (int MHz = 400; MHz < 470; MHz++)
            {
                bandIndex[MHz] = 13;
                RadioEdgeSet[MHz] = 17;
            }

            GetConfig(true);

            for (int r = 0; r < 2; r++)
            {
                Radioobject[r] = mainForm.COMMainProvider.RadioObject(r);
                if (Radioobject[r] != null)
                {
                    string radioname = (string)Radioobject.GetType().GetField("RadioID").GetValue(null);
                    if (Radioobject[r].IsICOM())
                        Radiomodel[r] = RadioType.ICOM;
                    if (radioname.Contains(" K3"))
                        Radiomodel[r] = RadioType.ElecraftKP3;
                    else if (radioname.Contains(" K4"))
                        Radiomodel[r] = RadioType.ElecraftK4;
                    else if (radioname.Contains(" 890") || radioname.Contains(" 990"))
                        Radiomodel[r] = RadioType.Kenwood;
                    else
                        Radiomodel[r] = RadioType.None;
                }
                else
                {
                    Radiomodel[r] = RadioType.None;
                }
            }
        }

        void GetConfig(bool all)
        {
            try
            {
                Set.LowerEdgeCW = Config.Read("WaterfallLowerEdgeCW", Def.LowerEdgeCW).Split(';').Select(s => int.Parse(s)).ToArray();
                Set.UpperEdgeCW = Config.Read("WaterfallUpperEdgeCW", Def.UpperEdgeCW).Split(';').Select(s => int.Parse(s)).ToArray();

                Set.LowerEdgePhone = Config.Read("WaterfallLowerEdgePhone", Def.LowerEdgePhone).Split(';').Select(s => int.Parse(s)).ToArray();
                Set.UpperEdgePhone = Config.Read("WaterfallUpperEdgePhone", Def.UpperEdgePhone).Split(';').Select(s => int.Parse(s)).ToArray();

                Set.LowerEdgeDigital = Config.Read("WaterfallLowerEdgeDigital", Def.LowerEdgeDigital).Split(';').Select(s => int.Parse(s)).ToArray();
                Set.UpperEdgeDigital = Config.Read("WaterfallUpperEdgeDigital", Def.UpperEdgeDigital).Split(';').Select(s => int.Parse(s)).ToArray();

                Set.EdgeSet = Config.Read("WaterfallEdgeSet", Def.EdgeSet);
                Set.Scrolling = Config.Read("WaterfallScrolling", Def.UseScrolling);

                if (all)
                {
                    Set.RefLevelCW = Config.Read("WaterfallRefCW", Def.RefLevelCW).Split(';').Select(s => int.Parse(s)).ToArray();
                    Set.RefLevelPhone = Config.Read("WaterfallRefPhone", Def.RefLevelPhone).Split(';').Select(s => int.Parse(s)).ToArray();
                    Set.RefLevelDigital = Config.Read("WaterfallRefDigital", Def.RefLevelDigital).Split(';').Select(s => int.Parse(s)).ToArray();

                    Set.PwrLevelCW = Config.Read("TransmitPowerCW", Def.PwrLevelCW).Split(';').Select(s => int.Parse(s)).ToArray();
                    Set.PwrLevelPhone = Config.Read("TransmitPowerPhone", Def.PwrLevelPhone).Split(';').Select(s => int.Parse(s)).ToArray();
                    Set.PwrLevelDigital = Config.Read("TransmitPowerDigital", Def.PwrLevelDigital).Split(';').Select(s => int.Parse(s)).ToArray();
                }
            }
            catch
            {
                // Settings are somehow corrupted. Reset everything to default.
                Set.LowerEdgeCW = Def.LowerEdgeCW.Split(';').Select(s => int.Parse(s)).ToArray();
                Set.UpperEdgeCW = Def.UpperEdgeCW.Split(';').Select(s => int.Parse(s)).ToArray();

                Set.LowerEdgePhone = Def.LowerEdgePhone.Split(';').Select(s => int.Parse(s)).ToArray();
                Set.UpperEdgePhone = Def.UpperEdgePhone.Split(';').Select(s => int.Parse(s)).ToArray();

                Set.LowerEdgeDigital = Def.LowerEdgeDigital.Split(';').Select(s => int.Parse(s)).ToArray();
                Set.UpperEdgeDigital = Def.UpperEdgeDigital.Split(';').Select(s => int.Parse(s)).ToArray();

                Set.EdgeSet = Def.EdgeSet;
                Set.Scrolling = Def.UseScrolling;

                Set.RefLevelCW = Def.RefLevelCW.Split(';').Select(s => int.Parse(s)).ToArray();
                Set.RefLevelPhone = Def.RefLevelPhone.Split(';').Select(s => int.Parse(s)).ToArray();
                Set.RefLevelDigital = Def.RefLevelDigital.Split(';').Select(s => int.Parse(s)).ToArray();

                Set.PwrLevelPhone = Def.PwrLevelPhone.Split(';').Select(s => int.Parse(s)).ToArray();
                Set.PwrLevelCW = Def.PwrLevelCW.Split(';').Select(s => int.Parse(s)).ToArray();
                Set.PwrLevelDigital = Def.PwrLevelDigital.Split(';').Select(s => int.Parse(s)).ToArray();
            }
        }

        private void OnClosing(object sender, FormClosingEventArgs e)
        {
            Config.Save("WaterfallRefCW", string.Join(";", Set.RefLevelCW.Select(i => i.ToString()).ToArray()));
            Config.Save("WaterfallRefPhone", string.Join(";", Set.RefLevelPhone.Select(i => i.ToString()).ToArray()));
            Config.Save("WaterfallRefDigital", string.Join(";", Set.RefLevelDigital.Select(i => i.ToString()).ToArray()));

            Config.Save("TransmitPowerCW", string.Join(";", Set.PwrLevelCW.Select(i => i.ToString()).ToArray()));
            Config.Save("TransmitPowerPhone", string.Join(";", Set.PwrLevelPhone.Select(i => i.ToString()).ToArray()));
            Config.Save("TransmitPowerDigital", string.Join(";", Set.PwrLevelDigital.Select(i => i.ToString()).ToArray()));

            //mainForm.scheduler.Second -= UpdateRadio;
            //_cdata.ActiveVFOChanged -= UpdateRadio;
            cdata.ActiveRadioBandChanged -= UpdateRadio;
        }

        public override void InitializeLayout()
        {
            if (mainForm == null)
            {
                mainForm = (FrmMain)(ParentForm ?? Owner);

                if (mainForm != null)
                {
                    //mainForm.scheduler.Second += UpdateRadio;
                    //_cdata.ActiveVFOChanged += new ContestData.ActiveVFOChange(UpdateRadio);
                    cdata.ActiveRadioBandChanged += new ContestData.ActiveRadioBandChange(UpdateRadio);
                }
            }

            UpdateRadio(1);
            UpdateRadio(2);
        }

        private void UpdateRadio(int radio)
        {
            int lowerEdge, uppderEdge;

            CurrentMHz[radio - 1] = (int)((radio == 1 ? cdata.Radio1_ActiveFreq : cdata.Radio2_ActiveFreq) / 1000);
            CurrentMode[radio - 1] = radio == 1 ? cdata.ActiveR1Mode : cdata.ActiveR2Mode;

            switch (CurrentMode[radio - 1])
            {
                case "CW":
                    lowerEdge = Set.LowerEdgeCW[bandIndex[CurrentMHz[radio - 1]]];
                    uppderEdge = Set.UpperEdgeCW[bandIndex[CurrentMHz[radio - 1]]];
                    CurrentRefLevel[radio - 1] = Set.RefLevelCW[bandIndex[CurrentMHz[radio - 1]]];
                    CurrentPwrLevel[radio - 1] = Set.PwrLevelCW[bandIndex[CurrentMHz[radio - 1]]];
                    break;
                case "SSB":
                case "AM":
                case "FM":
                    lowerEdge = Set.LowerEdgePhone[bandIndex[CurrentMHz[radio - 1]]];
                    uppderEdge = Set.UpperEdgePhone[bandIndex[CurrentMHz[radio - 1]]];
                    CurrentRefLevel[radio - 1] = Set.RefLevelPhone[bandIndex[CurrentMHz[radio - 1]]];
                    CurrentPwrLevel[radio - 1] = Set.PwrLevelPhone[bandIndex[CurrentMHz[radio - 1]]];
                    break;
                default:
                    lowerEdge = Set.LowerEdgeDigital[bandIndex[CurrentMHz[radio - 1]]];
                    uppderEdge = Set.UpperEdgeDigital[bandIndex[CurrentMHz[radio - 1]]];
                    CurrentRefLevel[radio - 1] = Set.RefLevelDigital[bandIndex[CurrentMHz[radio - 1]]];
                    CurrentPwrLevel[radio - 1] = Set.PwrLevelDigital[bandIndex[CurrentMHz[radio - 1]]];
                    break;
            }

            if (radio == 1)
            {
                rangeLabel1.Text = string.Format("WF: {0:N0} - {1:N0}", lowerEdge, uppderEdge); ;
            }
            else
            {
                rangeLabel2.Text = string.Format("{0:N0} - {1:N0}", lowerEdge, uppderEdge);
            }

            switch (Radiomodel[radio - 1])
            {
                case RadioType.ICOM:
                    // Update UI and waterfall edges and ref level in radio 
                    UpdateICOMEdges(radio, lowerEdge, uppderEdge, RadioEdgeSet[CurrentMHz[radio - 1]]);
                    UpdateICOMReflevel(radio, CurrentRefLevel[radio - 1]);
                    UpdateICOMPwrlevel(radio, CurrentPwrLevel[radio - 1]);
                    break;
                case RadioType.ElecraftK4:
                    break;
                case RadioType.ElecraftKP3:
                    break;
                case RadioType.Kenwood:
                    break;
                case RadioType.Yaesu:
                    break;
            }
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form prop = new WFProperties(Set);
  
            if (prop.ShowDialog() == DialogResult.OK)
            {
                GetConfig(false);
            }

            UpdateRadio(1);
            UpdateRadio(2);
        }

        private void RefLevelSlider1_Scroll(object sender, EventArgs e)
        {
            OnRefSlider(0);
        }

        // On mouse modification of slider
        private void RefLevelSlider1_MouseUp(object sender, MouseEventArgs e)
        {
        }

        // Mouse click
        private void RefLevelSlider2_MouseUp(object sender, MouseEventArgs e)
        {
        }

        // Scroll
        private void RefLevelSlider2_Scroll(object sender, EventArgs e)
        {
            OnRefSlider(1);
        }

        private void OnRefSlider(int index)
        {
            CurrentRefLevel[index] = index == 0 ? RefLevelSlider1.Value : RefLevelSlider2.Value;

            UpdateICOMReflevel(index + 1, CurrentRefLevel[index]);

            switch (CurrentMode[index])
            {
                case "CW":
                    Set.RefLevelCW[bandIndex[CurrentMHz[index]]] = CurrentRefLevel[index];
                    break;
                case "SSB":
                case "AM":
                case "FM":
                    Set.RefLevelPhone[bandIndex[CurrentMHz[index]]] = CurrentRefLevel[index];
                    break;
                default:
                    Set.RefLevelDigital[bandIndex[CurrentMHz[index]]] = CurrentRefLevel[index];
                    break;
            }
        }

        // Mouse up
        private void OnPwrLevelSlider1_MouseUp(object sender, MouseEventArgs e)
        {
        }

        // Scroll
        private void OnPwrLevelSlider1_Scroll(object sender, EventArgs e)
        {
            OnPwrSlider(0);
        }

        // Mouse up
        private void OnPwrLevelSlider2_MouseUp(object sender, MouseEventArgs e)
        {
        }

        // Scroll
        private void OnPwrLevelSlider2_Scroll(object sender, EventArgs e)
        {
            OnPwrSlider(1);
        }

        private void OnPwrSlider(int index)
        {
            CurrentPwrLevel[index] = index == 0 ? PwrLevelSlider1.Value : PwrLevelSlider2.Value;
            UpdateICOMPwrlevel(index + 1, CurrentPwrLevel[index]);

            if (CurrentMHz[index] != 0)
            {
                switch (CurrentMode[index])
                {
                    case "CW":
                        Set.PwrLevelCW[bandIndex[CurrentMHz[index]]] = CurrentPwrLevel[index];
                        break;
                    case "SSB":
                    case "AM":
                    case "FM":
                        Set.PwrLevelPhone[bandIndex[CurrentMHz[index]]] = CurrentPwrLevel[index];
                        break;
                    default:
                        Set.PwrLevelDigital[bandIndex[index]] = CurrentPwrLevel[index];
                        break;
                }
            }
        }

        // Update ICOM radio with new waterfall edges
        private void UpdateICOMEdges(int radio, int lower_edge, int upper_edge, int ICOMedgeSegment)
        {
            // Compose CI-V command to set waterfall edges
            byte[] CIVSetEdges = new byte[]
            {
                0x27, 0x1e,
                (byte)((ICOMedgeSegment / 10) * 16 + (ICOMedgeSegment % 10)),
                (byte)Set.EdgeSet,
                0x00, // Lower 10Hz & 1Hz
                (byte)((lower_edge % 10) * 16 + 0), // 1kHz & 100Hz
                (byte)(((lower_edge / 100) % 10) * 16 + ((lower_edge / 10) % 10)), // 100kHz & 10kHz
                (byte)(((lower_edge / 10000) % 10) * 16 + (lower_edge / 1000) % 10), // 10MHz & 1MHz
                (byte)(((lower_edge / 1000000) % 10) * 16 + (lower_edge / 100000) % 10), // 1GHz & 100MHz
                0x00, // // Upper 10Hz & 1Hz 
                (byte)((upper_edge % 10) * 16 + 0), // 1kHz & 100Hz
                (byte)(((upper_edge / 100) % 10) * 16 + (upper_edge / 10) % 10), // 100kHz & 10kHz
                (byte)(((upper_edge / 10000) % 10) * 16 + (upper_edge / 1000) % 10), // 10MHz & 1MHz
                (byte)(((upper_edge / 1000000) % 10) * 16 + (upper_edge / 100000) % 10) // 1GHz & 100MHz
            };

            CIVSetFixedModeMain[3] = (byte)(Set.Scrolling ? 0x03 : 0x01);
            CIVSetEdgeSetMain[3] = (byte)Set.EdgeSet;

            CIVSetFixedModeSub[3] = (byte)(Set.Scrolling ? 0x03 : 0x01);
            CIVSetEdgeSetSub[3] = (byte)Set.EdgeSet;

            //debuglabel1.Text = BitConverter.ToString(CIVSetFixedModeMain).Replace("-", " ");
            //debuglabel2.Text = BitConverter.ToString(CIVSetEdgeSetMain).Replace("-", " ");
            //debuglabel3.Text = BitConverter.ToString(CIVSetEdges).Replace("-", " ");

            if (Radioobject[radio - 1] != null && Radioobject[radio - 1].IsICOM())
            {
                Radioobject[radio - 1].SendCustomCommand(CIVSetFixedModeMain);
                Radioobject[radio - 1].SendCustomCommand(CIVSetFixedModeSub);
                Radioobject[radio - 1].SendCustomCommand(CIVSetEdgeSetMain);
                Radioobject[radio - 1].SendCustomCommand(CIVSetEdgeSetSub);
                Radioobject[radio - 1].SendCustomCommand(CIVSetEdges);
            }
        }

        // Update ICOM radio with new REF level
        private void UpdateICOMReflevel(int radionumber, int ref_level)
        {
            if (RefLevelLabel1 != null)
            {
                RefLevelSlider1.Value = ref_level;
                RefLevelLabel1.Text = string.Format("REF: {0,3:+#;-#;0}dB", ref_level);
            }

            int absRefLevel = (ref_level >= 0) ? ref_level : -ref_level;

            CIVSetRefLevelMain[3] = (byte)((absRefLevel / 10) * 16 + absRefLevel % 10);
            CIVSetRefLevelMain[5] = (ref_level >= 0) ? (byte)0 : (byte)1;

            CIVSetRefLevelSub[3] = (byte)((absRefLevel / 10) * 16 + absRefLevel % 10);
            CIVSetRefLevelSub[5] = (ref_level >= 0) ? (byte)0 : (byte)1;

            //debuglabel1.Text = BitConverter.ToString(CIVSetRefLevel).Replace("-", " ");
            //debuglabel2.Text = "";
            //debuglabel3.Text = "";

            if (Radioobject[radionumber - 1] != null && Radioobject[radionumber - 1].IsICOM())
            {
                Radioobject[radionumber - 1].SendCustomCommand(CIVSetRefLevelMain);
                Radioobject[radionumber - 1].SendCustomCommand(CIVSetRefLevelSub);
            }
        }

        // Update ICOM radio with new PWR level
        private void UpdateICOMPwrlevel(int radionumber, int pwr_level)
        {
            if (radionumber == 1)
            {
                if (PwrLevelSlider1 != null)
                {
                    PwrLevelSlider1.Value = pwr_level;
                    PwrLevelLabel1.Text = string.Format("PWR:{0,3}%", pwr_level);
                }
            }
            else
            {
                if (PwrLevelSlider2 != null)
                {
                    PwrLevelSlider2.Value = pwr_level;
                    PwrLevelLabel2.Text = string.Format("{0,3}%", pwr_level);
                }
            }

            int icomPower = (int)(255.0f * pwr_level / 100.0f + 0.99f); // Weird ICOM mapping of percent to binary

            CIVSetPwrLevel[2] = (byte)((icomPower / 100) % 10);
            CIVSetPwrLevel[3] = (byte)((((icomPower / 10) % 10) << 4) + (icomPower % 10));

            //debuglabel1.Text = BitConverter.ToString(CIVSetPwrLevel).Replace("-", " ");
            //debuglabel2.Text = "";
            //debuglabel3.Text = "";
            if (Radioobject[radionumber - 1] != null && Radioobject[radionumber - 1].IsICOM())
            {
                Radioobject[radionumber - 1].SendCustomCommand(CIVSetPwrLevel);
            }
        }
    }

    public class DefaultRadioSettings
    {
        public string LowerEdgeCW = "1810;3500;5352;7000;10100;14000;18068;21000;24890;28000;50000;70000;144000;432000";
        public string UpperEdgeCW = "1840;3570;5366;7040;10130;14070;18109;21070;24920;28070;50150;71000;144100;432100";
        public string RefLevelCW = "0;0;0;0;0;0;0;0;0;0;0;0;0;0";
        public string PwrLevelCW = "10;10;10;10;10;10;10;10;10;10;10;10;10;10";

        public string LowerEdgePhone = "1840;3600;5352;7040;10100;14100;18111;21150;24931;28300;50100;70000;144200;432200";
        public string UpperEdgePhone = "2000;3800;5366;7200;10150;14350;18168;21450;24990;28600;50500;71000;144400;432300";
        public string RefLevelPhone = "0;0;0;0;0;0;0;0;0;0;0;0;0;0";
        public string PwrLevelPhone = "10;10;10;10;10;10;10;10;10;10;10;10;10;10";

        public string LowerEdgeDigital = "1840;3570;5352;7040;10130;14070;18089;21070;24910;28070;50300;70000;144000;432000";
        public string UpperEdgeDigital = "1860;3600;5366;7080;10150;14100;18109;21150;24932;28110;50350;71000;144400;432400";
        public string RefLevelDigital = "0;0;0;0;0;0;0;0;0;0;0;0;0;0";
        public string PwrLevelDigital = "10;10;10;10;10;10;10;10;10;10;10;10;10;10";

        public int EdgeSet = 4;
        public bool UseScrolling = false;
    }

    public class RadioSettings
    {
        public const int HamBands = 14;
        public int Bands = HamBands;

        public int[] LowerEdgeCW = new int[HamBands];
        public int[] UpperEdgeCW = new int[HamBands];
        public int[] RefLevelCW = new int[HamBands];
        public int[] LowerEdgePhone = new int[HamBands];
        public int[] UpperEdgePhone = new int[HamBands];
        public int[] RefLevelPhone = new int[HamBands];
        public int[] LowerEdgeDigital = new int[HamBands];
        public int[] UpperEdgeDigital = new int[HamBands];
        public int[] RefLevelDigital = new int[HamBands];
        public int[] PwrLevelCW = new int[HamBands];
        public int[] PwrLevelPhone = new int[HamBands];
        public int[] PwrLevelDigital = new int[HamBands];
        public int EdgeSet;
        public bool Scrolling;
    }
}
