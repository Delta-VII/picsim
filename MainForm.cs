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
        

        public MainForm()

        // erste Tabelle

        {

            InitializeComponent();
            _pU = new PicUtil();

            DgRamBank0.Columns[0].Name = "Product ID";
            DgRamBank0.Columns[1].Name = "Product Name";
            DgRamBank0.Columns[2].Name = "Product Price";
            
        }

        // Button handling
        private void Start_Click(object sender, System.EventArgs e)
        {

        }
        private void Stop_Click(object sender, EventArgs e)
        {
            Stop.Enabled = false;
            Start.Enabled = true;
        }

        private void Reset_Click(object sender, EventArgs e)
        {
            this.Dispose(false);
            MainForm newForm = new MainForm();
            newForm.Show();
        }

        private void File_Click(object sender, EventArgs e)
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
                    //_pU.DecodeInstructions(int.Parse(line.Substring(5, 5), System.Globalization.NumberStyles.HexNumber));
                }
                
            }
            DgProgram.DataSource = null;
            DgProgram.DataSource = ProgramLines;
            DgProgram.Refresh();
        }

        private void DgProgram_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }
    }
}