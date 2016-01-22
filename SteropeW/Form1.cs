using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Ktos.Sterope.SteropeW
{
    public partial class Form1 : Form
    {
        private GraWZycie life;

        public Form1()
        {
            InitializeComponent();
            // new int[] { 3 },  new int[] { 2, 3 } => klasyczne reguły Conveya 23/3
            life = new GraWZycie(30, new int[] { 3 },  new int[] { 2, 3 });
            //life = new GraWZycie(30, new int[] { 3,4 },  new int[] { 3,4 });

            dataGridView1.ColumnCount = 30;
            dataGridView1.RowCount = 30;

            for (int i = 0; i < 30; i++)
            {
                dataGridView1.Columns[i].Width = 10;
                dataGridView1.Rows[i].Height = 10;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PokazPlanszeGry(life.GetPlansza());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            life.NastepnaRunda();
            PokazPlanszeGry(life.GetPlansza());

            label1.Text = life.GetRunda().ToString();
        }

        private void PokazPlanszeGry(int[,] p)
        {
            for (int j = 0; j < p.GetLength(1); j++)
            {
                for (int i = 0; i < p.GetLength(0); i++)
                {
                    if (p[i, j] != 0)
                        dataGridView1.Rows[j].Cells[i].Style.BackColor = Color.Red;
                    else
                        dataGridView1.Rows[j].Cells[i].Style.BackColor = Color.White;
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (life.GetStan(e.ColumnIndex, e.RowIndex) == 0)
            {                
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Red;
                life.SetStan(e.ColumnIndex, e.RowIndex, 1);
            }
            else
            {
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;
                life.SetStan(e.ColumnIndex, e.RowIndex, 0);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Enabled = !timer1.Enabled;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            button1_Click(this, null);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            life.Clear();
            PokazPlanszeGry(life.GetPlansza());
        }
    }
}
