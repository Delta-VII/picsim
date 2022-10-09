using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using picsim.Instructions;
using picsim.Instructions.ByteOrientedInstructions;
using Timer = System.Threading.Timer;

namespace picsim
{
    public partial class MainForm : Form
    {
        private PicUtil _pU;
        public BindingList<CodeLine> ProgramLines = new BindingList<CodeLine>();

        private List<Label> Status = new List<Label>();
        private List<Label> Option = new List<Label>();
        private List<Label> Intcon = new List<Label>();
        private List<Label> TrisA = new List<Label>();
        private List<Label> PortALabels = new List<Label>();
        private List<Button> PortAButtons = new List<Button>();
        private List<Label> TrisB = new List<Label>();
        private List<Label> PortBLabels = new List<Label>();
        private List<Button> PortBButtons = new List<Button>();
        private BitArray PortAbits = new BitArray(32);
        private BitArray PortBbits = new BitArray(32);
        private bool _breakPointReached;
        private System.Timers.Timer _timer = new System.Timers.Timer();
        private bool _watchdog;
        
        

        public MainForm()
        {
            InitializeComponent();
            _pU = new PicUtil();
            DgProgram.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            DgRamBank0.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            DgRamBank0.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            DgRamBank1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            DgRamBank1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            DgStack.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            
            Status.Add(labelC);
            Status.Add(labelDc);
            Status.Add(labelZ);
            Status.Add(labelPd);
            Status.Add(labelTo);
            Status.Add(labelRp0);
            Status.Add(labelRp1);
            Status.Add(labelIrp);
            
            Option.Add(labelPs0);
            Option.Add(labelPs1);
            Option.Add(labelPs2);
            Option.Add(labelPsa);
            Option.Add(labelT0se);
            Option.Add(labelT0cs);
            Option.Add(labelIntedg);
            Option.Add(labelRbpu);
            
            Intcon.Add(labelRbif);
            Intcon.Add(labelIntf);
            Intcon.Add(labelT0if);
            Intcon.Add(labelRbie);
            Intcon.Add(labelInte);
            Intcon.Add(labelT0ie);
            Intcon.Add(labelEeie);
            Intcon.Add(labelGie);
            
            TrisA.Add(labelTrisA0);
            TrisA.Add(labelTrisA1);
            TrisA.Add(labelTrisA2);
            TrisA.Add(labelTrisA3);
            TrisA.Add(labelTrisA4);

            TrisB.Add(labelTrisB0);
            TrisB.Add(labelTrisB1);
            TrisB.Add(labelTrisB2);
            TrisB.Add(labelTrisB3);
            TrisB.Add(labelTrisB4);
            TrisB.Add(labelTrisB5);
            TrisB.Add(labelTrisB6);
            TrisB.Add(labelTrisB7);
            
            PortALabels.Add(labelRa0);
            PortALabels.Add(labelRa1);
            PortALabels.Add(labelRa2);
            PortALabels.Add(labelRa3);
            PortALabels.Add(labelRa4);
            
            PortAButtons.Add(buttonRa0);
            PortAButtons.Add(buttonRa1);
            PortAButtons.Add(buttonRa2);
            PortAButtons.Add(buttonRa3);
            PortAButtons.Add(buttonRa4);
            
            PortBLabels.Add(labelRb0);
            PortBLabels.Add(labelRb1);
            PortBLabels.Add(labelRb2);
            PortBLabels.Add(labelRb3);
            PortBLabels.Add(labelRb4);
            PortBLabels.Add(labelRb5);
            PortBLabels.Add(labelRb6);
            PortBLabels.Add(labelRb7);
            
            PortBButtons.Add(buttonRb0);
            PortBButtons.Add(buttonRb1);
            PortBButtons.Add(buttonRb2);
            PortBButtons.Add(buttonRb3);
            PortBButtons.Add(buttonRb4);
            PortBButtons.Add(buttonRb5);
            PortBButtons.Add(buttonRb6);
            PortBButtons.Add(buttonRb7);

            _timer.Interval = 1000;
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = true;

            labelWdg.Text = "OFF";
        }
        
        private void btnStart_Click(object sender, EventArgs e)
        {
            Stop.Enabled = true;
            Start.Enabled = false;
            _timer.Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            Stop.Enabled = false;
            Start.Enabled = true;
            _timer.Stop();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            this.Dispose(false);
            MainForm newForm = new MainForm();
            newForm.Show();
        }

        private void BtnFile_Click(object sender, EventArgs e)
        {
            var filePath = string.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
                    var fileLines = System.IO.File.ReadLines(filePath);
                    InitProgram(fileLines);
                    _pU.InitPic();
                    DgRamBank0.DataSource = _pU.PicObject.RamBank0;
                    DgRamBank1.DataSource = _pU.PicObject.RamBank1;
                    DgStack.DataSource = _pU.PicObject.StackDGV;
                    RefreshDisplay();
                }
            }
        }

        private void InitProgram(IEnumerable<String> CodeLines)
        {
            var index = 0;
            foreach (var line in CodeLines)
            {
                if (line.StartsWith("0") || line.StartsWith("1"))
                {
                    ProgramLines.Add(new CodeLine(line, index));
                    _pU.DecodeInstructions(int.Parse(line.Substring(5, 5), System.Globalization.NumberStyles.HexNumber));
                    index++;
                }
            }
            
            DgProgram.DataSource = ProgramLines;
            DgProgram.ClearSelection();
            foreach (var line in ProgramLines)
            {
                if (line.ContainsCode)
                {
                    DgProgram.Rows[line.LineNumber].Selected = true;
                    break;
                }
            }
        }

        private void btnStep_Click(object sender, EventArgs e)
        {
            Run();
        }

        private void Run()
        {
            
            if (ProgramLines[_pU.PicObject.ProgCntr].BreakPoint && _breakPointReached)
            {
                _timer.Stop();
               _breakPointReached = false;
            }
            RefreshDisplay();
            DgProgram.ClearSelection();
            DgProgram.Rows[ProgramLines[_pU.PicObject.ProgCntr].LineNumber].Selected = true;
            _pU.Execute();
            RefreshDisplay();
            _breakPointReached = true;
            MatchTris();
            MatchPorts();
        }

        private void RefreshDisplay()
        {
            SetStatus();
            SetOption();
            SetIntcon();
            SetTrisA();
            SetTrisB();
            labelWregister.Text = "0x" + (_pU.PicObject.Wreg & 0b_1111_1111).ToString("X");
            labelRuntime.Text = _pU.PicObject.Runtime.ToString() + " us";
            DgStack.Refresh();
            DgRamBank0.Refresh();
            DgRamBank1.Refresh();
        }

        private void SetStatus()
        {
            var value = _pU.PicObject.RamBank0[0x03].Value;
            BitArray temp = new BitArray(new int[] { value });
            
            for (int i = 0; i < 8; i++)
            {
                Status[i].Text = Convert.ToString(Convert.ToInt16(temp[i]));
            }
        }
        
        private void SetOption()
        {
            var value = _pU.PicObject.RamBank1[0x01].Value;
            BitArray temp = new BitArray(new int[] { value });
            
            for (int i = 0; i < 8; i++)
            {
                Option[i].Text = Convert.ToString(Convert.ToInt16(temp[i]));
            }
        }
        
        private void SetIntcon()
        {
            var value = _pU.PicObject.RamBank0[0x0B].Value;
            BitArray temp = new BitArray(new int[] { value });
            
            for (int i = 0; i < 8; i++)
            {
                Intcon[i].Text = Convert.ToString(Convert.ToInt16(temp[i]));
            }
        }
        
        private void SetTrisA()
        {
            var value = _pU.PicObject.RamBank1[0x05].Value;
            BitArray temp = new BitArray(new int[] { value });
            
            for (int i = 0; i < 5; i++)
            {
                TrisA[i].Text = Convert.ToString(Convert.ToInt16(temp[i]));
            }
        }
        
        private void SetTrisB()
        {
            var value = _pU.PicObject.RamBank1[0x06].Value;
            BitArray temp = new BitArray(new int[] { value });
            
            for (int i = 0; i < 8; i++)
            {
                TrisB[i].Text = Convert.ToString(Convert.ToInt16(temp[i]));
            }
        }
        
        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            Run();
        }

        private void DgProgram_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DgProgram.Rows[e.RowIndex].DefaultCellStyle.BackColor == Color.Crimson)
            {
                DgProgram.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Empty;
                ProgramLines[e.RowIndex].BreakPoint = false;
                _breakPointReached = false;
            }
            else if (DgProgram.Rows[e.RowIndex].DefaultCellStyle.BackColor == Color.Empty)
            {
                DgProgram.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Crimson;
                ProgramLines[e.RowIndex].BreakPoint = true;
                _breakPointReached = true;
            }

        }

        private void buttonRa0_Click(object sender, EventArgs e)
        {
            setButtonA(0);
        }

        private void setButtonA(int index)
        {
            if (PortAbits[index])
            {
                PortALabels[index].BackColor = Color.Empty;
                PortAbits[index] = false;
                switch (index)
                {
                    case 0:
                        _pU.PicObject.RamBank0[5].Value &= ~(1 << 0);
                        break;
                    case 1:
                        _pU.PicObject.RamBank0[5].Value &= ~(1 << 1);
                        break;
                    case 2:
                        _pU.PicObject.RamBank0[5].Value &= ~(1 << 2);
                        break;
                    case 3:
                        _pU.PicObject.RamBank0[5].Value &= ~(1 << 3);
                        break;
                    case 4:
                        _pU.PicObject.RamBank0[5].Value &= ~(1 << 4);
                        break;
                    case 5:
                        _pU.PicObject.RamBank0[5].Value &= ~(1 << 5);
                        break;
                    case 6:
                        _pU.PicObject.RamBank0[5].Value &= ~(1 << 6);
                        break;
                    case 7:
                        _pU.PicObject.RamBank0[5].Value &= ~(1 << 7);
                        break;
                    default:
                        break;
                    
                }
            }
            else
            {
                PortALabels[index].BackColor = Color.Crimson;
                PortAbits[index] = true;
                switch (index)
                {
                    case 0:
                        _pU.PicObject.RamBank0[5].Value |= 1 << 0;
                        break;
                    case 1:
                        _pU.PicObject.RamBank0[5].Value |= 1 << 1;
                        break;
                    case 2:
                        _pU.PicObject.RamBank0[5].Value |= 1 << 2;
                        break;
                    case 3:
                        _pU.PicObject.RamBank0[5].Value |= 1 << 3;
                        break;
                    case 4:
                        _pU.PicObject.RamBank0[5].Value |= 1 << 4;
                        break;
                    case 5:
                        _pU.PicObject.RamBank0[5].Value |= 1 << 5;
                        break;
                    case 6:
                        _pU.PicObject.RamBank0[5].Value |= 1 << 6;
                        break;
                    case 7:
                        _pU.PicObject.RamBank0[5].Value |= 1 << 7;
                        break;
                    default:
                        break;
                }
            }
        }

        private void setButtonB(int index)
        {
            if (PortBbits[index])
            {
                PortBLabels[index].BackColor = Color.Empty;
                PortBbits[index] = false;
                int[] array = new int[1];
                PortBbits.CopyTo(array, 0);
                _pU.PicObject.RamBank0[6].Value = array[0];
            }
            else
            {
                PortBLabels[index].BackColor = Color.Crimson;
                PortBbits[index] = true;
                int[] array = new int[1];
                PortBbits.CopyTo(array, 0);
                _pU.PicObject.RamBank0[6].Value = array[0];
            }
        }
        
        private void buttonRb0_Click(object sender, EventArgs e)
        {
            setButtonB(0);
        }

        private void buttonRb1_Click(object sender, EventArgs e)
        {
            setButtonB(1);
        }

        private void buttonRb2_Click(object sender, EventArgs e)
        {
            setButtonB(2);
        }
        
        private void buttonRb3_Click(object sender, EventArgs e)
        {
            setButtonB(3);
        }

        private void buttonRb4_Click(object sender, EventArgs e)
        {
            setButtonB(4);
        }

        private void buttonRb5_Click(object sender, EventArgs e)
        {
            setButtonB(5);
        }

        private void buttonRb6_Click(object sender, EventArgs e)
        {
            setButtonB(6);
        }

        private void buttonRb7_Click(object sender, EventArgs e)
        {
            setButtonB(7);
        }

        private void MatchTris()
        {
            var valueA = _pU.PicObject.RamBank1[0x05].Value;
            var valueB = _pU.PicObject.RamBank1[0x06].Value;

            BitArray bA = new BitArray(new int[] { valueA });
            BitArray bB = new BitArray(new int[] { valueB });

            for (int i = 0; i <= 4; i++)
            {
                if (bA[i])
                {
                    PortAButtons[i].Enabled = true;
                }
                else
                {
                    PortAButtons[i].Enabled = false;
                }
            }
            
            for (int i = 0; i <= 7; i++)
            {
                if (bB[i])
                {
                    PortBButtons[i].Enabled = true;
                }
                else
                {
                    PortBButtons[i].Enabled = false;
                }
            }

        }

        private void MatchPorts()
        {
            var valueA = _pU.PicObject.RamBank0[0x05].Value;
            var valueB = _pU.PicObject.RamBank0[0x06].Value;
            var valueTA = _pU.PicObject.RamBank1[0x05].Value;
            var valueTB = _pU.PicObject.RamBank1[0x06].Value;

            BitArray bA = new BitArray(new int[] { valueA });
            BitArray bB = new BitArray(new int[] { valueB });
            BitArray bTA = new BitArray(new int[] { valueTA });
            BitArray bTB = new BitArray(new int[] { valueTB });
            
            for (int i = 0; i <= 4; i++)
            {
                if (bA[i])
                {
                    PortALabels[i].BackColor = Color.Crimson;
                    PortAbits[i] = false;
                }
                else if (bA[i] == false)
                {
                    PortALabels[i].BackColor = Color.Empty;
                    PortAbits[i] = true;
                }
            }
            
            for (int i = 0; i <= 7; i++)
            {
                if (bB[i])
                {
                    PortBLabels[i].BackColor = Color.Crimson;
                    PortBbits[i] = false;
                }
                else if (bB[i] == false)
                {
                    PortBLabels[i].BackColor = Color.Empty;
                    PortBbits[i] = true;
                }
            }
        }

        private void buttonRa1_Click(object sender, EventArgs e)
        {
            setButtonA(1);
        }

        private void buttonRa2_Click(object sender, EventArgs e)
        {
            setButtonA(2);
        }

        private void buttonRa3_Click(object sender, EventArgs e)
        {
            setButtonA(3);
        }

        private void buttonRa4_Click(object sender, EventArgs e)
        {
            setButtonA(4);
            _pU.PicObject.TimersetIO();
        }

        private void btnResetMcu_Click(object sender, EventArgs e)
        {
            _pU.PicObject.Reset();
            DgProgram.ClearSelection();
            DgProgram.Rows[ProgramLines[_pU.PicObject.ProgCntr].LineNumber].Selected = true;
            DgRamBank0.Refresh();
            DgRamBank1.Refresh();
        }

        private void buttonWdg_Click(object sender, EventArgs e)
        {
            if (_watchdog == false)
            {
                _watchdog = true;
                _pU.PicObject.Watchdogtrigger = true;
                labelWdg.Text = "ON";
            }
            else
            {
                _watchdog = false;
                _pU.PicObject.Watchdogtrigger = false;
                labelWdg.Text = "OFF";
            }
        }
    }
}