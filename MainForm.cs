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

namespace picsim
{
    public partial class MainForm : Form
    {
        private PicUtil _pU;
        public BindingList<CodeLine> ProgramLines = new BindingList<CodeLine>();
        private int CodeOffset;

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
        }

        // Button handling
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
                    DgRamBank0.Refresh();
                    DgRamBank1.Refresh();
                    DgStack.DataSource = _pU.PicObject.StackDGV;
                }
            }
        }

        private void InitProgram(IEnumerable<String> CodeLines)
        {
            foreach (var line in CodeLines)
            {
                ProgramLines.Add(new CodeLine(line));
                if (line.StartsWith("0") || line.StartsWith("1"))
                {
                    _pU.DecodeInstructions(int.Parse(line.Substring(5, 5),
                        System.Globalization.NumberStyles.HexNumber));
                }
            }

            var index = 0;
            foreach (var line in CodeLines)
            {
                index++;
                if (line.StartsWith("0"))
                {
                    CodeOffset = index;

                    break;
                }
            }

            DgProgram.DataSource = ProgramLines;
            DgProgram.ClearSelection();
            DgProgram.Rows[CodeOffset - 1].Selected = true;
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
            DgProgram.Rows[_pU.PicObject.ProgCntr + CodeOffset - 1].Selected = true;
            labelWregister.Text = "0x" + (_pU.PicObject.Wreg & 0b_1111_1111).ToString("X");
            _pU.Execute();
            labelWregister.Text = "0x" + (_pU.PicObject.Wreg & 0b_1111_1111).ToString("X");
        }
    }
}