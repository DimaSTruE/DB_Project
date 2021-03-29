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
    public partial class Table1_Insert : Form
    {
        public Table1_Insert()
        {
            InitializeComponent();
        }

        private void Tabl1InsCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Tabl1InsAdd_Click(object sender, EventArgs e)
        {
            using (MySqlConnection con = new MySqlConnection(h.ConStr))
            {
                //Читаємо дані з форми Table1_Insert
                string tb1 = textBox1.Text;
                string tb2 = textBox2.Text;
                string tb3 = textBox3.Text;
                string tb4 = textBox4.Text;
                string tb5 = textBox5.Text;

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

                //Формуємо команду для додавання запису
                string sql = "INSERT INTO Karcinologia_1 " +
                    "(id, NAME, VID, TYPE, my_date, Fotok)" + 
                    " VALUES (@TK1, @TK2, @TK3, @TK4, @TK5, @File)";

                MySqlCommand cmd = new MySqlCommand(sql, con);
                //Додаємо параметри у колекцію класу Command
                cmd.Parameters.AddWithValue("@TK1", tb1);
                cmd.Parameters.AddWithValue("@TK2", tb2);
                cmd.Parameters.AddWithValue("@TK3", tb3);
                cmd.Parameters.AddWithValue("@TK4", tb4);
                cmd.Parameters.AddWithValue("@TK5", tb5);

                cmd.Parameters.AddWithValue("@File", rawData);

                con.Open();                             //Відкриваємо з'єднання
                cmd.ExecuteNonQuery();                  //Виконуємо команду cmd
                con.Close();                            //Закриваємо з'єднання

                MessageBox.Show("Додавання запису пройшло вдало");
                
            }
            this.Close();
        }

        private void Table1_Insert_Load(object sender, EventArgs e)
        {
            h.pathToPhoto = Application.StartupPath + @"\" + "1.jpg";
            pictureBox1.Image = Image.FromFile(h.pathToPhoto);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Виберіть файл";
            openFileDialog1.Filter = "img files (*.jpg) | *.jpg| bmp file (*.bmp) | *.bmp | All files (*.*) | *.*";
            openFileDialog1.InitialDirectory = Application.StartupPath;

            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;
            {
                h.pathToPhoto = openFileDialog1.FileName;
                pictureBox1.Image = Image.FromFile(h.pathToPhoto);
            }
        }
    }
}
