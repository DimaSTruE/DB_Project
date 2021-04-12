using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data.OleDb;
using Excel = Microsoft.Office.Interop.Excel;

namespace DB_Project
{
    public partial class t1Karcinologia_1 : Form
    {
        DataTable dt;
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

            //h.bs1.Sort = dataGridView1.Columns[1].Name;
            dt = (DataTable)h.bs1.DataSource;
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

        private void AddNew_Click(object sender, EventArgs e)
        {
            Table1_Insert f1add = new Table1_Insert();
            f1add.ShowDialog();
            h.bs1.DataSource = h.MyfunDt("SELECT * FROM Karcinologia_1");
            dataGridView1.DataSource = h.bs1;
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            //Знаходимо значення ключового поля поточного запису
            h.curVal0 = dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString();
            h.keyName = dataGridView1.Columns[0].Name;

            Table1_Delete f3 = new Table1_Delete();
            f3.ShowDialog();

            //Оновлюємо джерело даних застосунку клієнта
            h.bs1.DataSource = h.MyfunDt("SELECT * FROM Karcinologia_1");
            dataGridView1.DataSource = h.bs1; //Оновлюємо DataGridView
        }

        private void dataGridView1_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            int curColidx = dataGridView1.CurrentCellAddress.X; // Індекс стовпця поточної клітинки
            int curRowidx = dataGridView1.CurrentCellAddress.Y; // Індекс рядка поточної клітинки
            string curColName0 = dataGridView1.Columns[0].Name; // Назва стовпця ключового поля
            string curColName = dataGridView1.Columns[curColidx].Name; // Назва поточного стовпця
            h.curVal0 = dataGridView1[0, curRowidx].Value.ToString(); // Значення клітинки ключового поля поточного рядка

            string newCurCellVal = e.Value.ToString(); // Нове значення поточної клітинки

            if (curColName == "NAME" || curColName == "VID" || curColName == "TYPE")
            {
                newCurCellVal = "'" + newCurCellVal + "'"; // Якщо поле текстове, беремо в лапки
            }
            string sqlStr = "UPDATE Karcinologia_1 SET " + curColName + " = " + newCurCellVal +
                " WHERE " + curColName0 + " = " + h.curVal0;

            using (MySqlConnection con = new MySqlConnection(h.ConStr))
            {
                MySqlCommand cmd = new MySqlCommand(sqlStr, con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        private void Edit_Click(object sender, EventArgs e)
        {
            Table1_Update f4 = new Table1_Update();
            f4.ShowDialog();
            h.bs1.DataSource = h.MyfunDt("SELECT * FROM Karcinologia_1");
            dataGridView1.DataSource = h.bs1; //Оновлюємо DataGridView
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            int rIndx = dataGridView1.CurrentCell.RowIndex;
            if (rIndx < dataGridView1.RowCount - 1)
            {
                byte[] a = (byte[])dataGridView1.Rows[rIndx].Cells[5].Value;
                MemoryStream memImage = new MemoryStream(a);
                pictureBox1.Image = Image.FromStream(memImage);
                memImage.Close();
            }
            else
            {
                pictureBox1.Image = null; //при виході за межі записів
            }
        }

        private void btnOLE_DB_Click(object sender, EventArgs e)
        {
            //Задаємо шлях збереження файлу
            string fileName = Application.StartupPath + @"\Report\File_xls.xls";
            //Якщо файл інсує,тоді його витираємо
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            //Записуємо рядок з'єднання
            string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                " Data Source=" + fileName + "; " +
                "Extended Properties = \"Excel 8.0; CharacterSet=1251; HDR=NO\"";
            //Записуємо команду створення таблиці Excel
            string commandCreateoldb = "CREATE TABLE [MySheet]" +
                " ([" + dt.Columns[0].ColumnName + "] int";
            for (int i = 1; i < dt.Columns.Count; i++)
            {
                commandCreateoldb += ", [" + dt.Columns[i].ColumnName + "] string";
            }
            commandCreateoldb += ")";

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                using (OleDbCommand cmd = new OleDbCommand(commandCreateoldb, conn))
                {
                    try
                    {
                        cmd.ExecuteNonQuery(); // Створення таблиці Excel

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            cmd.CommandText = "insert into [MySheet$] values (" + Convert.ToString(dt.Rows[i][0]);
                            for (int j = i; j < (dt.Columns.Count); j++)
                            {
                                if (dt.Columns[j].DataType.ToString() == "System.String")
                                {
                                    cmd.CommandText += ", '" + Convert.ToString(dt.Rows[i][j]) + "'";
                                }
                                else if (dt.Columns[j].DataType.ToString() == "System.Int32")
                                {
                                    cmd.CommandText += ", '" + Convert.ToInt32(dt.Rows[i][j]) + "'";
                                }
                                else if (dt.Columns[j].DataType.ToString() == "System.DateTime")
                                {
                                    cmd.CommandText += ", '" + Convert.ToDateTime(dt.Rows[i][j]) + "'";
                                }
                                else
                                {
                                    cmd.CommandText += ", 'NULL'";
                                }
                            }
                            cmd.CommandText += ")";
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Таблиця MySheet уже існує!");
                    }
                }
                conn.Close();
            }
            MessageBox.Show("File 'File_xls.xls' is created!");
        }

        private void btnStream_Click(object sender, EventArgs e)
        {
            //Вивід у файл потоками
            var ec1251 = Encoding.GetEncoding(1251);
            //Визначаємо вибране у radioButton розширення файлу
            string extend;
            if (radioButton1.Checked)
            {
                extend = "tsv";
            }
            else if (radioButton2.Checked)
            {
                extend = "doc";
            }
            else if (radioButton3.Checked)
            {
                extend = "xls";
            }
            else
            {
                extend = "txt";
            }
            //Задаємо шлях збереження файлу
            string path = Application.StartupPath + @"\Report\";
            string filePath = path + "File_2" + extend + "." + extend;
            //Оголошуємо потік
            StreamWriter wr = new StreamWriter(filePath, false, encoding: ec1251);

            //Виводимо dt у файл
            try
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    wr.Write(dt.Columns[i].ToString().ToUpper() + "\t");
                }
                wr.WriteLine();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (dt.Rows[i][j] != null)
                        {
                            wr.Write(Convert.ToString(dt.Rows[i][j]) + "\t");
                        }
                        else
                        {
                            wr.Write("\t");
                        }
                    }
                    wr.WriteLine();
                }
                wr.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            MessageBox.Show("File_" + extend + "." + extend + " is created");
        }

        private void btnCom_Click(object sender, EventArgs e)
        {
            //Задаємо шлях збереження файлу
            string fileName = Application.StartupPath + @"\Report\File3_xls.xls";

            Excel.Application excel = new Excel.Application();//Створюємо COM-об'єкт Excel
            excel.SheetsInNewWorkbook = 2;//Кількість аркушів в книзі Excel;
            excel.Workbooks.Add(Type.Missing);//Додаємо книгу
            Excel.Workbook workbook = excel.Workbooks[1];//Отримуємо посилання на першу відкриту книгу
            Excel.Worksheet sheet = workbook.Worksheets.get_Item(1);//Отримуємо посилання на перший аркуш
            sheet.Name = "Карцинологія";//Змінюємо назву аркуша

            //Виводимо назви полів
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                sheet.Cells[1, j + 1].Value = dt.Columns[j].ColumnName;
            }
            //Виводимо записи
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dt.Columns[j].DataType.ToString() == "System.Byte[]")
                    {
                        sheet.Cells[i + 2, j + 1].Value = "NULL";
                    }
                    else
                    {
                        sheet.Cells[i + 2, j + 1].Value = dt.Rows[i][j];
                    }
                }
            }
            //Форматуємо аркуш Excel
            format_File3(sheet);

            excel.DisplayAlerts = false;
            excel.Application.ActiveWorkbook.SaveAs(fileName, Excel.XlSaveAsAccessMode.xlNoChange);
            excel.Quit();
            MessageBox.Show("Файл 'File3_xls.xls' створено");
        }
        private void format_File3(Excel.Worksheet sheet)
        {
            int r1 = 1;
            int c1 = 1;
            int r2 = dt.Rows.Count + 1;
            int c2 = dt.Columns.Count;

            Excel.Range range0 = (Excel.Range)sheet.Range[sheet.Cells[9, 2], sheet.Cells[9, 2]];
            Excel.Range range1 = (Excel.Range)sheet.Range[sheet.Cells[r1, c1], sheet.Cells[r2, c2]];
            Excel.Range range2 = (Excel.Range)sheet.Range[sheet.Cells[10, 1], sheet.Cells[10, 5]];

            range1.Font.Background = true; //Жирний шрифт
            range1.Font.Size = 12; //Розмір 20
            range1.Font.Color = ColorTranslator.ToOle(Color.Black);//Колір-чорний
            range1.Font.Name = "Times New Roman";//Шрифт

            range1.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;//Рамка лінія
            range1.Borders.Weight = Excel.XlBorderWeight.xlThin;//Тонка
            range1.Borders.Color = ColorTranslator.ToOle(Color.Pink);

            //Вирівнювання в діапазоні
            range1.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
            range1.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;

            //Ширина і висота клітинки Height i Width
            range1.ColumnWidth = 20;
            range0.RowHeight = 35;

            range1.EntireColumn.AutoFit();//Авто ширина і висота
            range1.EntireRow.AutoFit();

            //Колір заливки
            range1.Interior.Color = ColorTranslator.ToOle(Color.Red);
            range2.Merge(Type.Missing);//Об'єднання клітин діапазону
        }
    }
}
