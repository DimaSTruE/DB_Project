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
    public partial class t1Karcinologia_1 : Form
    {
        int rowEdit = -1;
        int colEdit = -1;

        public t1Karcinologia_1()
        {
            InitializeComponent();
        }

        private void t1Karcinologia_1_Load(object sender, EventArgs e)
        {
            h.bs1 = new BindingSource();
            h.bs1.DataSource = h.MyfunDt("SELECT * FROM Karcinologia_1");
            dataGridView1.DataSource = h.bs1;
            t1Karcinologia_1_FormatDGV();
            bindingNavigator1.BindingSource = h.bs1;

        }

        private void t1Karcinologia_1_FormatDGV()
        {
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Black;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.White;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dataGridView1.GridColor = Color.Black;
            dataGridView1.Columns[0].Width = 50;
            dataGridView1.Columns[0].HeaderText = "Номер";
            dataGridView1.Columns[1].Width = 95;
            dataGridView1.Columns[1].HeaderText = "Ім'я";
            dataGridView1.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[2].Width = 95;
            dataGridView1.Columns[2].HeaderText = "Вид";
            dataGridView1.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[3].Width = 95;
            dataGridView1.Columns[3].HeaderText = "Тип";
            dataGridView1.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            
            if (int.Parse(h.typeUser) > 2)
            {
                dataGridView1.ReadOnly = true;
            }
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dataGridView1.CurrentCell.EditType != typeof(DataGridViewTextBoxEditingControl))
            {
                return;
            }
            rowEdit = dataGridView1.CurrentCell.RowIndex;
            colEdit = dataGridView1.CurrentCell.ColumnIndex;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (colEdit == -1)
            {
                return;
            }

            var r = rowEdit;
            var c = colEdit;

            dataGridView1.CurrentCell = dataGridView1[c, r];
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
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
                    ri = dataGridView1.CurrentCell.RowIndex;
                    ci = dataGridView1.CurrentCell.ColumnIndex;
                    e.SuppressKeyPress = true;

                    if (dataGridView1.Columns.Count > ci + 1)
                    {
                        dataGridView1.CurrentCell = dataGridView1.Rows[ri].Cells[ci + 1];
                        return;
                    }
                    else
                    {
                        if (dataGridView1.Rows.Count > ri +1)
                        {
                            dataGridView1.CurrentCell = dataGridView1.Rows[ri + 1].Cells[0];
                        }
                    }
                }
            }
        }
    }
}
