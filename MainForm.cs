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

namespace Try
{
    public partial class MainForm : Form
    {
        private List<string> _filelines;
        private DateTime _referenceTime;
        private TimeSpan _ts = default;
        private int _codePointer;
        private PicUtil _pU;
        public List<string> Program = new List<string>();

        public MainForm()

        // erste Tabelle

        {

            InitializeComponent();
            _pU = new PicUtil();
            _pU.EvtUpdateRegisters += OnEvtUpdateRegisters;

            dataGrid1.Columns[0].Name = "Product ID";
            dataGrid1.Columns[1].Name = "Product Name";
            dataGrid1.Columns[2].Name = "Product Price";

            //243

            for (int i = 1; i < dataGridView5.ColumnCount; i++)
            {
                dataGridView5.Rows[0].Cells[i].Value = "i";
            }

            for (int i = 1; i < dataGridView6.ColumnCount; i++)
            {
                dataGridView6.Rows[0].Cells[i].Value = "o";
            }

            for (int i = 1; i < RAGrid.ColumnCount; i++)
            {
                RAGrid.Rows[0].Cells[i].Value = 1;
            }

            for (int i = 1; i < RBGrid.ColumnCount; i++)
            {
                RBGrid.Rows[0].Cells[i].Value = 1;
            }

            for (int i = 1; i < dataGridView2.ColumnCount; i++)
            {
                dataGridView2.Rows[0].Cells[i].Value = 0;
            }

            for (int i = 1; i < dataGridView4.ColumnCount; i++)
            {
                dataGridView4.Rows[0].Cells[i].Value = 0;
            }

            for (int i = 1; i < dataGridView3.ColumnCount; i++)
            {
                dataGridView3.Rows[0].Cells[i].Value = 0;
            }

            FullfillRows("00");
            FullfillRows("08");
            FullfillRows("10");
            FullfillRows("18");
            FullfillRows("20");
            FullfillRows("28");
            FullfillRows("30");
            FullfillRows("38");
            FullfillRows("40");
            FullfillRows("48");
            FullfillRows("50");
            FullfillRows("58");
            FullfillRows("60");
            FullfillRows("68");
            FullfillRows("70");
            FullfillRows("78");
            FullfillRows("80");
            FullfillRows("88");
            FullfillRows("90");
            FullfillRows("98");
            FullfillRows("A0");
            FullfillRows("A8");
            FullfillRows("B0");
            FullfillRows("B8");
            FullfillRows("C0");
            FullfillRows("C8");
            FullfillRows("D0");
            FullfillRows("D8");
            FullfillRows("E0");
            FullfillRows("E8");
            FullfillRows("F0");
            FullfillRows("F8");
            timer1.Interval = 10;
            timer1.Tick += timer1_Tick;
        }

        //Zweite Tabelle

        private void FullfillRows(String name)
        {
            string[] row = new string[] { name };
            dataGrid1.Rows.Add(row);
        }
        private void timer1_Tick(object sender, System.EventArgs e)
        {
            timerValueLabel.Text = Convert.ToString(DateTime.Now - _referenceTime);
            _ts = _referenceTime - DateTime.Now;
        }

        // Button handling
        private void Start_Click(object sender, System.EventArgs e)
        {
            if (this._filelines != default)
            {
                timer1.Enabled = true;
                Start.Enabled = false;
                Stop.Enabled = true;
                _referenceTime = DateTime.Now.Add(_ts);
                _pU.InitSimulator(Program);
            }

        }
        private void Stop_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            Stop.Enabled = false;
            Start.Enabled = true;
            timerValueLabel.Text = Convert.ToString(DateTime.Now - _referenceTime);
        }

        private void Reset_Click(object sender, EventArgs e)
        {
            this.Dispose(false);
            MainForm newForm = new MainForm();
            newForm.Show();
        }

        private void Step_Click_1(object sender, EventArgs e)
        {
            codeGrid.ClearSelection();
            _pU.Run();
            _codePointer++;
            codeGrid.Rows[_codePointer].Selected = true;
        }

        private void Go_Click(object sender, EventArgs e)
        {

        }

        private void File_Click(object sender, EventArgs e)
        {
            var filePath = string.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
                    this._filelines = System.IO.File.ReadLines(filePath).ToList();
                    bool FirstCharEqu0 = false;
                    int i = 0;
                    foreach (String item in this._filelines)
                    {
                        codeGrid.Rows.Add(item);
                        i++;
                        if (item[0] == '0' && FirstCharEqu0 == false)
                        {
                            FirstCharEqu0 = true;
                            _codePointer = i - 1;
                        }
                    }
                    loadProgram();
                    codeGrid.ClearSelection();
                    codeGrid.Rows[_codePointer].Selected = true;
                }
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void RAGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            bool[] raValues = new bool[8];
            if (sender is DataGridView dGv)
            {
                if (dGv.CurrentCell.Value is int)
                {
                    dGv.CurrentCell.Value = (int)dGv.CurrentCell.Value == 1 ? 0 : 1;
                }
                for (int i = 1; i < RAGrid.ColumnCount; i++)
                {
                    raValues[i - 1] = (int)RAGrid.Rows[0].Cells[i].Value == 1 ? true : false;
                }
                _pU.Tgui2Sim.IoToggleRa = raValues;
            }
        }

        private void RBGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            bool[] rbValues = new bool[8];
            if (sender is DataGridView dGv)
            {
                if (dGv.CurrentCell.Value is int)
                {
                    dGv.CurrentCell.Value = (int)dGv.CurrentCell.Value == 1 ? 0 : 1;
                }
                for (int i = 1; i < RBGrid.ColumnCount; i++)
                {
                    rbValues[i - 1] = (int)RBGrid.Rows[0].Cells[i].Value == 1 ? true : false;
                }
                _pU.Tgui2Sim.IoToggleRb = rbValues;
            }
        }

        private void dataGridView5_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //do nothing
        }

        private void dataGridView6_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //do nothing
        }

        private void OnEvtUpdateRegisters(object sender, UpdateRegistersEventArgs e)
        {
            for (int i = 0; i < dataGridView2.ColumnCount; i++)
            {
                dataGridView2.Rows[0].Cells[i + 1].Value = _pU.Tsim2Gui1.Sfr[i];
            }
            for (int i = 0; i < dataGridView4.ColumnCount; i++)
            {
                dataGridView4.Rows[0].Cells[i + 1].Value = _pU.Tsim2Gui1.Sfr[i + 8];
            }
            for (int i = 0; i < dataGridView3.ColumnCount; i++)
            {
                dataGridView3.Rows[0].Cells[i + 1].Value = _pU.Tsim2Gui1.Sfr[i + 16];
            }
        }
        
        public void loadProgram()
        {
            foreach (String item in this._filelines)
            {
                if (item[0] == '0')
                {
                    Program.Add(item.Substring(5,5));
                }
                
            }
        }

    }
}