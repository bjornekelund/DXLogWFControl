using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DXLog.net;
using ConfigFile;
using IOComm;

namespace DXLog.net
{
    public partial class DXLogIcomControl : KForm
    {
        public static string CusWinName
        {
            get { return "ICOM Controller"; }
        }

        public static int CusFormID
        {
            get { return 1022; }
        }
        
        private ContestData _cdata = null;
        //private Font _windowFont = new Font("Courier New", 10, FontStyle.Regular);

        private FrmMain mainForm = null;

        private delegate void newQsoSaved(DXQSO qso);

        private readonly bool NoRadio = false; // For debugging with no radio attached

        // Pre-baked CI-V commands
        private byte[] CIVSetFixedMode = { 0xfe, 0xfe, 0xff, 0xe0, 0x27, 0x14, 0x00, 0x01, 0xfd };
        private byte[] CIVSetEdgeSet = { 0xfe, 0xfe, 0xff, 0xe0, 0x27, 0x16, 0x0, 0xff, 0xfd };
        private byte[] CIVSetRefLevel = { 0xfe, 0xfe, 0xff, 0xe0, 0x27, 0x19, 0x00, 0x00, 0x00, 0x00, 0xfd };
        private byte[] CIVSetPwrLevel = { 0xfe, 0xfe, 0xff, 0xe0, 0x14, 0x0a, 0x00, 0x00, 0xfd };
        private const int HamBands = 14;
        private const int MaxMHz = 470;
        private const int TableSize = 74;

        // Maps MHz to band name.
        private string[] bandName = new string[MaxMHz];
        private readonly string[] REFbandName = new string[TableSize]
            { "??m", "160m", "??m", "80m", "??m", "60m", "40m", "40m", "??m", "30m",
            "30m", "??m", "??m", "20m", "20m", "??m", "??m", "17m", "17m", "??m",
            "15m", "15m", "??m", "??m", "12m", "12m", "??m", "11m", "10m", "10m",
            "??m", "??m", "??m", "??m", "??m", "??m", "??m", "??m", "??m", "??m",
            "??m", "??m", "??m", "??m", "??m", "??m", "??m", "??m", "??m", "6m",
            "6m", "6m", "6m", "6m", "??m", "??m", "??m", "??m", "??m", "??m",
            "??m", "??m", "??m", "??m", "??m", "??m", "??m", "??m", "??m", "4m",
            "4m", "4m", "4m", "4m" };

        // Maps MHz to internal band index.
        // Bands are 160=0, 80=1, etc. up to 11=4m
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

        // Maps actual MHz to radio's scope edge set on ICOM 7xxx. 54 elements.
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

        // Global variables
        private int CurrentLowerEdge, CurrentUpperEdge, CurrentRefLevel, CurrentPwrLevel;
        private int CurrentFrequency = 0, NewMHz, CurrentMHz = 0;
        private string CurrentMode = string.Empty, NewMode = string.Empty;
        //private bool Barefoot;

        CATCommon Radio1 = null;

        RadioSettings set = new RadioSettings();
        DefaultRadioSettings def = new DefaultRadioSettings();

        public DXLogIcomControl()
        {
            InitializeComponent();
        }

        public DXLogIcomControl(ContestData cdata)
        {
            InitializeComponent();
            _cdata = cdata;
            //FormLayoutChangeEvent += new FormLayoutChange(handle_FormLayoutChangeEvent);

            //string message;
            string[] commandLineArguments = Environment.GetCommandLineArgs();

            while (contextMenuStrip1.Items.Count > 0)
                contextMenuStrip2.Items.Add(contextMenuStrip1.Items[0]);
            contextMenuStrip2.Items.RemoveByKey("fixWindowSizeToolStripMenuItem");
            contextMenuStrip2.Items.RemoveByKey("fontSizeToolStripMenuItem");
            contextMenuStrip2.Items.RemoveByKey("colorsToolStripMenuItem");

            // Set the decoding arrays to default
            for (int MHz = 0; MHz < MaxMHz; MHz++)
            {
                bandName[MHz] = "??m";
                bandIndex[MHz] = 1;
                RadioEdgeSet[MHz] = 1;
            }

            // Initialize using tables
            for (int MHz = 0; MHz < TableSize; MHz++)
            {
                bandName[MHz] = REFbandName[MHz];
                bandIndex[MHz] = REFbandIndex[MHz];
                RadioEdgeSet[MHz] = REFRadioEdgeSet[MHz];
            }

            // Add 2m
            for (int MHz = 137; MHz < 200; MHz++)
            {
                bandName[MHz] = "2m";
                bandIndex[MHz] = 12;
                RadioEdgeSet[MHz] = 16;
            }

            // Add 70cm
            for (int MHz = 400; MHz < 470; MHz++)
            {
                bandName[MHz] = "70cm";
                bandIndex[MHz] = 13;
                RadioEdgeSet[MHz] = 17;
            }

            GetConfig();

            if (mainForm == null)
            {
                mainForm = (FrmMain)(ParentForm ?? Owner);

                if (mainForm != null)
                {
                    mainForm.scheduler.Second += UpdateRadio;
                    RadioSettings jj = null;
                    int i = jj.EdgeSet;
                    _cdata.ActiveVFOChanged += new ContestData.ActiveVFOChange(UpdateRadio);
                    _cdata.ActiveRadioBandChanged += new ContestData.ActiveRadioBandChange(UpdateRadio);
                    _cdata.FocusedRadioChanged += new ContestData.FocusedRadioChange(UpdateRadio);
                }
            }
            UpdateRadioReflevel(CurrentRefLevel);
            UpdateRadioPwrlevel(CurrentPwrLevel);
        }

        void GetConfig()
        {
            try
            {
                set.LowerEdgeCW = Config.Read("WaterfallLowerEdgeCW", def.LowerEdgeCW).Split(';').Select(s => int.Parse(s)).ToArray();
                set.UpperEdgeCW = Config.Read("WaterfallUpperEdgeCW", def.UpperEdgeCW).Split(';').Select(s => int.Parse(s)).ToArray();
                set.RefLevelCW = Config.Read("WaterfallRefCW", def.RefLevelCW).Split(';').Select(s => int.Parse(s)).ToArray();
                set.PwrLevelCW = Config.Read("TransmitPowerCW", def.PwrLevelCW).Split(';').Select(s => int.Parse(s)).ToArray();

                set.LowerEdgePhone = Config.Read("WaterfallLowerEdgePhone", def.LowerEdgePhone).Split(';').Select(s => int.Parse(s)).ToArray();
                set.UpperEdgePhone = Config.Read("WaterfallUpperEdgePhone", def.UpperEdgePhone).Split(';').Select(s => int.Parse(s)).ToArray();
                set.RefLevelPhone = Config.Read("WaterfallRefPhone", def.RefLevelPhone).Split(';').Select(s => int.Parse(s)).ToArray();
                set.PwrLevelPhone = Config.Read("TransmitPowerPhone", def.PwrLevelPhone).Split(';').Select(s => int.Parse(s)).ToArray();

                set.LowerEdgeDigital = Config.Read("WaterfallLowerEdgeDigital", def.LowerEdgeDigital).Split(';').Select(s => int.Parse(s)).ToArray();
                set.UpperEdgeDigital = Config.Read("WaterfallUpperEdgeDigital", def.UpperEdgeDigital).Split(';').Select(s => int.Parse(s)).ToArray();
                set.RefLevelDigital = Config.Read("WaterfallRefDigital", def.RefLevelDigital).Split(';').Select(s => int.Parse(s)).ToArray();
                set.PwrLevelDigital = Config.Read("TransmitPowerDigital", def.PwrLevelDigital).Split(';').Select(s => int.Parse(s)).ToArray();

                set.EdgeSet = Config.Read("WaterfallEdgeSet", def.EdgeSet);
                set.Scrolling = Config.Read("WaterfallScrolling", def.UseScrolling);
            }
            catch
            {
                set.LowerEdgeCW = def.LowerEdgeCW.Split(';').Select(s => int.Parse(s)).ToArray();
                set.UpperEdgeCW = def.UpperEdgeCW.Split(';').Select(s => int.Parse(s)).ToArray();
                set.RefLevelCW = def.RefLevelCW.Split(';').Select(s => int.Parse(s)).ToArray();
                set.PwrLevelCW = def.PwrLevelCW.Split(';').Select(s => int.Parse(s)).ToArray();

                set.LowerEdgePhone = def.LowerEdgePhone.Split(';').Select(s => int.Parse(s)).ToArray();
                set.UpperEdgePhone = def.UpperEdgePhone.Split(';').Select(s => int.Parse(s)).ToArray();
                set.RefLevelPhone = def.RefLevelPhone.Split(';').Select(s => int.Parse(s)).ToArray();
                set.PwrLevelPhone = def.PwrLevelPhone.Split(';').Select(s => int.Parse(s)).ToArray();

                set.LowerEdgeDigital = def.LowerEdgeDigital.Split(';').Select(s => int.Parse(s)).ToArray();
                set.UpperEdgeDigital = def.UpperEdgeDigital.Split(';').Select(s => int.Parse(s)).ToArray();
                set.RefLevelDigital = def.RefLevelDigital.Split(';').Select(s => int.Parse(s)).ToArray();
                set.PwrLevelDigital = def.PwrLevelDigital.Split(';').Select(s => int.Parse(s)).ToArray();
            }
        }

        // Save all settings when closing program
        private void OnClosing(object sender, EventArgs e)
        {
            Config.Save("WaterfallLowerEdgeCW", string.Join(";", set.LowerEdgeCW.Select(i => i.ToString()).ToArray()));
            Config.Save("WaterfallUpperEdgeCW", string.Join(";", set.UpperEdgeCW.Select(i => i.ToString()).ToArray()));
            Config.Save("WaterfallRefCW", string.Join(";", set.RefLevelCW.Select(i => i.ToString()).ToArray()));
            Config.Save("TransmitPowerCW", string.Join(";", set.PwrLevelCW.Select(i => i.ToString()).ToArray()));

            Config.Save("WaterfallLowerEdgePhone", string.Join(";", set.LowerEdgePhone.Select(i => i.ToString()).ToArray()));
            Config.Save("WaterfallUpperEdgePhone", string.Join(";", set.UpperEdgePhone.Select(i => i.ToString()).ToArray()));
            Config.Save("WaterfallRefPhone", string.Join(";", set.RefLevelPhone.Select(i => i.ToString()).ToArray()));
            Config.Save("TransmitPowerPhone", string.Join(";", set.PwrLevelPhone.Select(i => i.ToString()).ToArray()));

            Config.Save("WaterfallLowerEdgeDigital", string.Join(";", set.LowerEdgeDigital.Select(i => i.ToString()).ToArray()));
            Config.Save("WaterfallUpperEdgeDigital", string.Join(";", set.UpperEdgeDigital.Select(i => i.ToString()).ToArray()));
            Config.Save("WaterfallRefDigital", string.Join(";", set.RefLevelDigital.Select(i => i.ToString()).ToArray()));
            Config.Save("TransmitPowerDigital", string.Join(";", set.PwrLevelDigital.Select(i => i.ToString()).ToArray()));

            mainForm.scheduler.Second -= UpdateRadio;
        }

        public override void InitializeLayout()
        {
            //InitializeLayout(_windowFont);
            //if (FormLayout.FontName.Contains("Courier"))
            //    _windowFont = new Font(FormLayout.FontName, FormLayout.FontSize, FontStyle.Regular);
            //else
            //    _windowFont = Helper.GetSpecialFont(FontStyle.Regular, FormLayout.FontSize);

        }

        private void UpdateRadio(int radionumber)
        {
            UpdateRadio();
        }


        delegate void UpdateRadioCB();

        private void UpdateRadio()
        {
            if (RefLevelLabel.InvokeRequired)
            {
                UpdateRadioCB d = new UpdateRadioCB(UpdateRadio);
                RefLevelLabel.BeginInvoke(d);
            }
            else
            {
                Radio1 = mainForm.COMMainProvider.RadioObject(1);

                NewMode = _cdata.ActiveR1Mode;
                NewMHz = (int)(_cdata.Radio1_ActiveFreq / 1000.0 + 0.5);

                bandlabel.Text = NewMHz.ToString();
                modelabel.Text = NewMode;

                CurrentMHz = NewMHz;
                CurrentMode = NewMode;

                switch (CurrentMode)
                {
                    case "CW":
                        CurrentLowerEdge = set.LowerEdgeCW[bandIndex[CurrentMHz]];
                        CurrentUpperEdge = set.UpperEdgeCW[bandIndex[CurrentMHz]];
                        CurrentRefLevel = set.RefLevelCW[bandIndex[CurrentMHz]];
                        CurrentPwrLevel = set.PwrLevelCW[bandIndex[CurrentMHz]];
                        break;
                    case "SSB":
                    case "AM":
                    case "FM":
                        CurrentLowerEdge = set.LowerEdgePhone[bandIndex[CurrentMHz]];
                        CurrentUpperEdge = set.UpperEdgePhone[bandIndex[CurrentMHz]];
                        CurrentRefLevel = set.RefLevelPhone[bandIndex[CurrentMHz]];
                        CurrentPwrLevel = set.PwrLevelPhone[bandIndex[CurrentMHz]];
                        break;
                    default:
                        CurrentLowerEdge = set.LowerEdgeDigital[bandIndex[CurrentMHz]];
                        CurrentUpperEdge = set.UpperEdgeDigital[bandIndex[CurrentMHz]];
                        CurrentRefLevel = set.RefLevelDigital[bandIndex[CurrentMHz]];
                        CurrentPwrLevel = set.PwrLevelDigital[bandIndex[CurrentMHz]];
                        break;
                }

                // Update UI and waterfall edges and ref level in radio 
                UpdateRadioEdges(CurrentLowerEdge, CurrentUpperEdge, RadioEdgeSet[CurrentMHz]);
                UpdateRadioReflevel(CurrentRefLevel);
                UpdateRadioPwrlevel(CurrentPwrLevel);
            }
        }

        // On arrow key modification of slider
        //private void OnRefSliderKey(object sender, KeyEventArgs e)
        //{
        //    UpdateRefSlider();
        //}

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form prop = new IcomProperties(set);
  
            if (prop.ShowDialog() == DialogResult.OK)
            {
                GetConfig();
            }
        }

        //private void ToggleBarefoot(object sender, EventArgs e)
        //{
        //    Barefoot = !Barefoot;

        //    UpdateRadioPwrlevel(CurrentPwrLevel);
        //}

        // On mouse modification of slider
        private void OnRefSliderMouseClick(object sender, MouseEventArgs e)
        {
            CurrentRefLevel = (int)(RefLevelSlider.Value + 0.0f);

            UpdateRadioReflevel(CurrentRefLevel);

            switch (CurrentMode)
            {
                case "CW":
                    set.RefLevelCW[bandIndex[CurrentMHz]] = CurrentRefLevel;
                    break;
                case "SSB":
                case "AM":
                case "FM":
                    set.RefLevelPhone[bandIndex[CurrentMHz]] = CurrentRefLevel;
                    break;
                default:
                    set.RefLevelDigital[bandIndex[CurrentMHz]] = CurrentRefLevel;
                    break;
            }
        }

        //private void OnRefSliderMouseClick(object sender, EventArgs e)
        //{
        //}

        // Update ref level on slider action
        //private void UpdateRefSlider()
        //{
        //}

        // on mouse movement of power slider
        private void OnPwrSliderMouseClick(object sender, MouseEventArgs e)
        {
            CurrentPwrLevel = (int)(PwrLevelSlider.Value + 0.0f);
            UpdateRadioPwrlevel(CurrentPwrLevel);

            if (CurrentMHz != 0)
            {
                switch (CurrentMode)
                {
                    case "CW":
                        set.PwrLevelCW[bandIndex[CurrentMHz]] = CurrentPwrLevel;
                        break;
                    case "SSB":
                    case "AM":
                    case "FM":
                        set.PwrLevelPhone[bandIndex[CurrentMHz]] = CurrentPwrLevel;
                        break;
                    default:
                        set.PwrLevelDigital[bandIndex[CurrentMHz]] = CurrentPwrLevel;
                        break;
                }
            }
        }

        // Update radio with new waterfall edges
        private void UpdateRadioEdges(int lower_edge, int upper_edge, int ICOMedgeSegment)
        {
            // Compose CI-V command to set waterfall edges
            byte[] CIVSetEdges = new byte[19]
            {
                0xfe, 0xfe, Radio1.GetCIVAddress(), 0xe0,
                0x27, 0x1e,
                (byte)((ICOMedgeSegment / 10) * 16 + (ICOMedgeSegment % 10)),
                (byte)set.EdgeSet,
                0x00, // Lower 10Hz & 1Hz
                (byte)((lower_edge % 10) * 16 + 0), // 1kHz & 100Hz
                (byte)(((lower_edge / 100) % 10) * 16 + ((lower_edge / 10) % 10)), // 100kHz & 10kHz
                (byte)(((lower_edge / 10000) % 10) * 16 + (lower_edge / 1000) % 10), // 10MHz & 1MHz
                (byte)(((lower_edge / 1000000) % 10) * 16 + (lower_edge / 100000) % 10), // 1GHz & 100MHz
                0x00, // // Upper 10Hz & 1Hz 
                (byte)((upper_edge % 10) * 16 + 0), // 1kHz & 100Hz
                (byte)(((upper_edge / 100) % 10) * 16 + (upper_edge / 10) % 10), // 100kHz & 10kHz
                (byte)(((upper_edge / 10000) % 10) * 16 + (upper_edge / 1000) % 10), // 10MHz & 1MHz
                (byte)(((upper_edge / 1000000) % 10) * 16 + (upper_edge / 100000) % 10), // 1GHz & 100MHz
                0xfd
            };

            // Update radio if we are not in debug mode
            if (!NoRadio && Radio1 != null && Radio1.IsICOM())
            {
                CIVSetFixedMode[2] = Radio1.GetCIVAddress();
                CIVSetFixedMode[7] = (byte)(set.Scrolling ? 0x03 : 0x01);
                CIVSetEdgeSet[2] = Radio1.GetCIVAddress();
                CIVSetEdgeSet[7] = (byte)set.EdgeSet;

                Radio1.SendCustomCommand(CIVSetFixedMode);
                Radio1.SendCustomCommand(CIVSetEdgeSet);
                Radio1.SendCustomCommand(CIVSetEdges);
            }
        }

        // Update radio with new REF level
        private void UpdateRadioReflevel(int ref_level)
        {
            if (RefLevelLabel != null)
            {
                RefLevelSlider.Value = ref_level;
                RefLevelLabel.Text = string.Format("Ref: {0:+#;-#;0}dB", ref_level);
            }

            if (Radio1 != null && Radio1.IsICOM())
            {
                int absRefLevel = (ref_level >= 0) ? ref_level : -ref_level;

                CIVSetRefLevel[2] = Radio1.GetCIVAddress();
                CIVSetRefLevel[7] = (byte)((absRefLevel / 10) * 16 + absRefLevel % 10);
                CIVSetRefLevel[9] = (ref_level >= 0) ? (byte)0 : (byte)1;

                Radio1.SendCustomCommand(CIVSetRefLevel);
            }
        }

        // Update radio with new PWR level
        private void UpdateRadioPwrlevel(int pwr_level)
        {
            if (PwrLevelSlider != null)
            {
                PwrLevelSlider.Value = pwr_level;
                PwrLevelLabel.Text = string.Format("Pwr:{0,3}%", pwr_level);
            }

            if (Radio1 != null && Radio1.IsICOM())
            {
                int icomPower = (int)(255.0f * pwr_level / 100.0f + 0.99f); // Weird ICOM mapping of percent to binary

                CIVSetPwrLevel[2] = Radio1.GetCIVAddress();
                CIVSetPwrLevel[6] = (byte)((icomPower / 100) % 10);
                CIVSetPwrLevel[7] = (byte)((((icomPower / 10) % 10) << 4) + (icomPower % 10));
                Radio1.SendCustomCommand(CIVSetPwrLevel);
            }
        }
    }

    public class DefaultRadioSettings
    {
        public string LowerEdgeCW = "1810;3500;5352;7000;10100;14000;18068;21000;24890;28000;50000;70000;144000;432000";
        public string UpperEdgeCW = "1840;3570;5366;7040;10130;14070;18109;21070;24920;28070;50150;71000;144100;432100";
        public string RefLevelCW = "0;0;0;0;0;0;0;0;0;0;0;0;0;0";
        public string PwrLevelCW = "18;18;18;18;18;18;18;18;18;18;18;18;18;18";

        public string LowerEdgePhone = "1840;3600;5353;7040;10100;14100;18111;21150;24931;28300;50100;70000;144200;432200";
        public string UpperEdgePhone = "2000;3800;5366;7200;10150;14350;18168;21450;24990;28600;50500;71000;144400;432300";
        public string RefLevelPhone = "0;0;0;0;0;0;0;0;0;0;0;0;0;0";
        public string PwrLevelPhone = "18;18;18;18;18;18;18;18;18;18;18;18;18;18";

        public string LowerEdgeDigital = "1840;3570;5352;7040;10130;14070;18089;21070;24910;28070;50300;70000;144000;432000";
        public string UpperEdgeDigital = "1860;3600;5366;7080;10150;14100;18109;21150;24932;28110;50350;71000;144400;432400";
        public string RefLevelDigital = "0;0;0;0;0;0;0;0;0;0;0;0;0;0";
        public string PwrLevelDigital = "18;18;18;18;18;18;18;18;18;18;18;18;18;18";

        public int EdgeSet = 4;
        public bool UseScrolling = true;
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
