using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DB_Project
{
    public partial class t1Karcinologia_2 : Form
    {
        int rowEdit = -1;
        int colEdit = -1;

        public t1Karcinologia_2()
        {
            InitializeComponent();
        }

        private void t1Karcinologia_2_Load(object sender, EventArgs e)
        {
            h.bs1 = new BindingSource();
            h.bs1.DataSource = h.MyfunDt("SELECT * FROM Karcinologia_2");
            dataGridView2.DataSource = h.bs1;
            t1Karcinologia_2_FormatDGV();
            bindingNavigator2.BindingSource = h.bs1;
        }

        private void t1Karcinologia_2_FormatDGV()
        {
            dataGridView2.DefaultCellStyle.SelectionForeColor = Color.Black;
            dataGridView2.DefaultCellStyle.SelectionBackColor = Color.White;
            dataGridView2.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dataGridView2.GridColor = Color.Black;
            dataGridView2.Columns[0].Width = 50;
            dataGridView2.Columns[0].HeaderText = "Популяція";
            dataGridView2.Columns[1].Width = 95;
            dataGridView2.Columns[1].HeaderText = "Живлення";
            dataGridView2.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView2.Columns[2].Width = 95;
            dataGridView2.Columns[2].HeaderText = "Домен";
            dataGridView2.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            if (int.Parse(h.typeUser) > 2)
            {
                dataGridView2.ReadOnly = true;
            }
        }

        private void dataGridView2_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dataGridView2.CurrentCell.EditType != typeof(DataGridViewTextBoxEditingControl))
            {
                return;
            }
            rowEdit = dataGridView2.CurrentCell.RowIndex;
            colEdit = dataGridView2.CurrentCell.ColumnIndex;
        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            if (colEdit == -1)
            {
                return;
            }

            var r = rowEdit;
            var c = colEdit;

            dataGridView2.CurrentCell = dataGridView2[c, r];
        }

        private void dataGridView2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            else
            {
                int ri, ci;
                if (e.KeyCode == Keys.Enter)
                {
                    ri = dataGridView2.CurrentCell.RowIndex;
                    ci = dataGridView2.CurrentCell.ColumnIndex;
                    e.SuppressKeyPress = true;

                    if (dataGridView2.Columns.Count > ci + 1)
                    {
                        dataGridView2.CurrentCell = dataGridView2.Rows[ri].Cells[ci + 1];
                        return;
                    }
                    else
                    {
                        if (dataGridView2.Rows.Count > ri + 1)
                        {
                            dataGridView2.CurrentCell = dataGridView2.Rows[ri + 1].Cells[0];
                        }
                    }
                }
            }
        }
    }
}
