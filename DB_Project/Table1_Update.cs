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

namespace DB_Project
{
    public partial class Table1_Update : Form
    {
        public Table1_Update()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            string sqlStr;
            if ((checkBox1.Checked == true) && (checkBox2.Checked == false))
            {
                //Формуємо запит на видалення таблиці
                sqlStr = "UPDATE Karcinologia_1 SET " + tbSetToUpdate.Text + " WHERE " + tbWhereToUpdate.Text;

                if (MessageBox.Show("Ви впевнені, що хочете замінити дані?", "Заміна",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (MySqlConnection con = new MySqlConnection(h.ConStr))
                    {
                        MySqlCommand cmd = new MySqlCommand(sqlStr, con);

                        con.Open();                                 //Відкриваємо з'єднання
                        cmd.ExecuteNonQuery();                      //Виконуємо команду cmd
                        con.Close();                                //Закриваємо з'єднання
                    }
                }
            }
            if ((checkBox1.Checked == false)&&(checkBox2.Checked == true))
            {
                //Формуємо запит тільки на зміну зображення
                int FileSize;
                byte[] rawData;
                FileStream fs;
                string strFileName;

                strFileName = h.pathToPhoto;
                fs = new FileStream(strFileName, FileMode.Open, FileAccess.Read);
                FileSize = (Int32)fs.Length;
                rawData = new byte[FileSize];
                fs.Read(rawData, 0, FileSize);
                fs.Close();

                sqlStr = "UPDATE Karcinologia_1 SET " + "Fotok = @File" +
                    " WHERE " + tbWhereToUpdate.Text;

                if (MessageBox.Show("Ви впевнені, що хочете замінити дані?", "Заміна",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (MySqlConnection con = new MySqlConnection(h.ConStr))
                    {
                        MySqlCommand cmd = new MySqlCommand(sqlStr, con);
                        //Додаємо параметри у колекцію класу Command
                        cmd.Parameters.AddWithValue("@FileName", strFileName);
                        cmd.Parameters.AddWithValue("@File", rawData);

                        con.Open();                                 //Відкриваємо з'єднання
                        cmd.ExecuteNonQuery();                      //Виконуємо команду cmd
                        con.Close();                                //Закриваємо з'єднання
                        MessageBox.Show("Редагування запису пройшло вдало");
                    }

                }
            }
            if ((checkBox1.Checked == true) && (checkBox2.Checked == true))
            {
                //Формуємо запит тільки на зміну зображення
                int FileSize;
                byte[] rawData;
                FileStream fs;
                string strFileName;

                strFileName = h.pathToPhoto;
                fs = new FileStream(strFileName, FileMode.Open, FileAccess.Read);
                FileSize = (Int32)fs.Length;
                rawData = new byte[FileSize];
                fs.Read(rawData, 0, FileSize);
                fs.Close();

                sqlStr = "UPDATE Karcinologia_1 SET " + tbSetToUpdate.Text + " Fotok = @File" +
                    " WHERE " + tbWhereToUpdate.Text;

                if (MessageBox.Show("Ви впевнені, що хочете замінити дані?", "Заміна",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (MySqlConnection con = new MySqlConnection(h.ConStr))
                    {
                        MySqlCommand cmd = new MySqlCommand(sqlStr, con);
                        //Додаємо параметри у колекцію класу Command
                        cmd.Parameters.AddWithValue("@FileName", strFileName);
                        cmd.Parameters.AddWithValue("@File", rawData);

                        con.Open();                                 //Відкриваємо з'єднання
                        cmd.ExecuteNonQuery();                      //Виконуємо команду cmd
                        con.Close();                                //Закриваємо з'єднання
                        MessageBox.Show("Редагування запису пройшло вдало");
                    }

                }
            }
            //this.Close();
        }

        private void Table1_Update_Load(object sender, EventArgs e)
        {
            h.pathToPhoto = Application.StartupPath + @"\" + "1.jpg";
            pictureBox1.Image = Image.FromFile(h.pathToPhoto);
            tbWhereToUpdate.Text = h.keyName + " = " + h.curVal0; 
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                label1.Visible = true;
                tbSetToUpdate.Visible = true;
                btnChange.Visible = true;
            }
            else if (checkBox1.Checked == false)
            {
                label1.Visible = false;
                tbSetToUpdate.Visible = false;
                if (checkBox2.Checked == false)
                {
                    btnChange.Visible = false;
                }
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                panel2.Visible = true;
                label5.Visible = true;
                button1.Visible = true;
                pictureBox1.Visible = true;
                btnChange.Visible = true;
            }
            else if (checkBox2.Checked == false)
            {
                panel2.Visible = false;
                label5.Visible = false;
                button1.Visible = false;
                pictureBox1.Visible = false;
                if (checkBox1.Checked == false)
                {
                    btnChange.Visible = false;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Виберіть файл";
            openFileDialog.Filter = "img files (*.jpg) | *.jpg| bmp file (*.bmp) | *.bmp | All files (*.*) | *.*";
            openFileDialog.InitialDirectory = Application.StartupPath;

            if (openFileDialog.ShowDialog() != DialogResult.OK) return;
            {
                h.pathToPhoto = openFileDialog.FileName;
                pictureBox1.Image = Image.FromFile(h.pathToPhoto);
            }
        }
    }
}
