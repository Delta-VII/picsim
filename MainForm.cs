﻿using System;
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
        private BitArray PortAbits = new BitArray(5);
        private BitArray PortBbits = new BitArray(8);
        
        

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
            
        }
        
        private void btnStart_Click(object sender, System.EventArgs e)
        {
            Stop.Enabled = true;
            Start.Enabled = false;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            Stop.Enabled = false;
            Start.Enabled = true;
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

        private void DgProgram_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void btnStep_Click(object sender, EventArgs e)
        {
            Run();
        }

        private void Run()
        {
            DgProgram.ClearSelection();
            DgProgram.Rows[ProgramLines[_pU.PicObject.ProgCntr].LineNumber].Selected = true;
            RefreshDisplay();
            _pU.Execute();
            RefreshDisplay();
        }

        private void RefreshDisplay()
        {
            SetStatus();
            SetOption();
            SetIntcon();
            SetTrisA();
            SetTrisB();
            labelWregister.Text = "0x" + (_pU.PicObject.Wreg & 0b_1111_1111).ToString("X");
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

    }
}