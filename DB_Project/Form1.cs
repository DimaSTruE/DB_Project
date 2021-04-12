using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DB_Project
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void проПрограмуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About tmp = new About();
            tmp.ShowDialog();
        }

        private void калькулятор1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Calc tmp = new Calc();
            tmp.ShowDialog();
        }

        private void калькулятор2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Calculator tmp = new Calculator();
            tmp.ShowDialog();
        }

        private void вихідToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void karcinologiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            t1Karcinologia_1 t1 = new t1Karcinologia_1();
            t1.ShowDialog();
        }

        private void karcinologia2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            t1Karcinologia_2 t2 = new t1Karcinologia_2();
            t2.ShowDialog();
        }
 
    }
}
