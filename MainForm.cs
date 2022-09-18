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

namespace picsim
{
    public partial class MainForm : Form
    {
        private PicUtil _pU;

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
                    var fileLines = System.IO.File.ReadAllText(filePath);
                    LoadProgram(fileLines);
                    TbProgram.Text = fileLines;
                }
            }
        }

        public void LoadProgram(string fileLines)
        {
            var lines = fileLines.Split(Environment.NewLine);
            var instructionsCodes = new List<int>();
            
            foreach (var line in lines.Where(x => x.StartsWith('0') | x.StartsWith('1')))
            {
                    instructionsCodes.Add(int.Parse(line.Substring(5,5), System.Globalization.NumberStyles.HexNumber));                
            }
            _pU.DecodeInstructions(instructionsCodes);
            
        }

    }
}