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
            this.Height = 410;
            DataTable minmaxValue = h.MyfunDt("SELECT MIN(my_date), MAX(my_date) , " + "MIN(id), MAX(id) FROM Karcinologia_1");
            //
            dtpIn.Value = Convert.ToDateTime(minmaxValue.Rows[0][0].ToString());
            dtpOut.Value = Convert.ToDateTime(minmaxValue.Rows[0][1].ToString());
            //
            txtBEGIN.Text = minmaxValue.Rows[0][2].ToString();
            txtEND.Text = minmaxValue.Rows[0][3].ToString();
            //
            minmaxValue = h.MyfunDt("SELECT DISTINCT VID FROM Karcinologia_1");
            cmbVID.Items.Add("");
            for (int i = 0; i < minmaxValue.Rows.Count; i++)
            {
                cmbVID.Items.Add(minmaxValue.Rows[i][0].ToString());
            }
            cmbVID.DropDownStyle = ComboBoxStyle.DropDownList;
            /////////////////////////////////////////////////////////////
            h.bs1 = new BindingSource();
            h.bs1.DataSource = h.MyfunDt("SELECT * FROM Karcinologia_1");
            dataGridView1.DataSource = h.bs1;
            t1Karcinologia_1_FormatDGV();
            bindingNavigator1.BindingSource = h.bs1;

            h.bs1.Sort = dataGridView1.Columns[1].Name;
        }

        private void t1Karcinologia_1_FormatDGV()
        {
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Black;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.Pink;
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
            dataGridView1.Columns[4].Width = 75;
            dataGridView1.Columns[4].HeaderText = "Дата";
            dataGridView1.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

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

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (btnFind.Checked)
            {
                txtBNV.Visible = true;
                label1.Visible = true;
                label1.Text = "Пошук:";
                txtBNV.Focus();
            }
            else
            {
                txtBNV.Text = "";
                txtBNV.Visible = false;
                label1.Visible = false;
                dataGridView1.ClearSelection();
            }
        }

        private void txtBNV_TextChanged(object sender, EventArgs e)
        {
            if (btnFind.Checked)
            {
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    dataGridView1.Rows[i].Selected = false; 
                    for (int j = 0; j < dataGridView1.ColumnCount; j++)
                    {
                        if (dataGridView1.Rows[i].Cells[j].Value != null)
                        {
                            if (dataGridView1.Rows[i].Cells[j].Value.ToString().Contains(txtBNV.Text))
                            {
                                dataGridView1.Rows[i].Selected = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void txtBNV_Leave(object sender, EventArgs e)
        {
            txtBNV.Visible = false;
            txtBNV.Text = "";
            label1.Visible = false;
            dataGridView1.ClearSelection();
            btnFind.Checked = false;
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            if (btnFilter.Checked)
            {
                this.Height = 560;
                panel1.Visible = true;
            }
            else
            {
                panel1.Visible = false;
                h.bs1.Filter = "";
                this.Height = 410;
            }
        }

        private void btnOkFilter_Click(object sender, EventArgs e)
        {
            string strFilter = "id > 0";

            strFilter += " AND (my_date >= '" + dtpIn.Value.ToString("yyyy-MM-dd") + "'" +
                " AND my_date <= '" + dtpOut.Value.ToString("yyyy-MM-dd") + "') ";
            //
            if ((txtBEGIN.Text != "") && (txtEND.Text != ""))
            {
                strFilter += " AND (id >= '" + int.Parse(txtBEGIN.Text) + "' AND id <= '" + int.Parse(txtEND.Text) + "') ";
            }
            else if ((txtBEGIN.Text == "") && (txtEND.Text != ""))
            {
                strFilter += " AND (id <= '" + int.Parse(txtEND.Text) + "') ";
            }
            else if ((txtBEGIN.Text != "") && (txtEND.Text == ""))
            {
                strFilter += " AND (id >= '" + int.Parse(txtBEGIN.Text) + "') ";
            }

            if (cmbVID.Text != "")
            {
                strFilter += " AND (VID LIKE '%" + cmbVID.Text + "%') ";
            }
            h.bs1.Filter = strFilter;
        }

        private void btnCancelFilter_Click(object sender, EventArgs e)
        {
            h.bs1.Filter = "";
        }
    }
}
