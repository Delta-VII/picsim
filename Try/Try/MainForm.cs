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
        private List<string> Filelines;
        private DateTime referenceTime;
        private TimeSpan ts = default;
        private int codePointer;
        public MainForm()

        // erste Tabelle

        {

            InitializeComponent();

            dataGrid1.Columns[0].Name = "Product ID";
            dataGrid1.Columns[1].Name = "Product Name";
            dataGrid1.Columns[2].Name = "Product Price";



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
        }

        //Zweite Tabelle

        private void FullfillRows(String name)
        {
            string[] row = new string[] { name };
            dataGrid1.Rows.Add(row);
        }
        private void timer1_Tick(object sender, System.EventArgs e)
        {            
            timerValueLabel.Text = Convert.ToString(DateTime.Now - referenceTime);
            ts =  referenceTime - DateTime.Now;
        }     
        
        // Button handling
        private void Start_Click(object sender, System.EventArgs e)
        {
            if( this.Filelines != default)
            {
                timer1.Enabled = true;
                Start.Enabled = false;
                Stop.Enabled = true;
                referenceTime = DateTime.Now.Add(ts);
            }
                
        }
        private void Stop_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            Stop.Enabled = false;
            Start.Enabled = true;
            timerValueLabel.Text = Convert.ToString(DateTime.Now - referenceTime);
        }

        private void Reset_Click(object sender, EventArgs e)
        {
            this.Dispose(false);
            MainForm NewForm = new MainForm();
            NewForm.Show();
        }

        private void Step_Click_1(object sender, EventArgs e)
        {
            codeGrid.ClearSelection();
            codePointer++;            
            codeGrid.Rows[codePointer].Selected = true;
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
                    this.Filelines = System.IO.File.ReadLines(filePath).ToList();

                    foreach (String item in this.Filelines )
                    {
                        codeGrid.Rows.Add(item);
                    }
                    codePointer = 0;
                    codeGrid.ClearSelection();
                    codeGrid.Rows[codePointer].Selected = true;                    
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
    }

      

    
}




 
