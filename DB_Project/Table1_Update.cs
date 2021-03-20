using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            //Формуємо запит на видалення таблиці
            string sqlStr = "UPDATE Karcinologia_1 SET " + textBox1.Text + " WHERE " + textBox2.Text;

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
            this.Close();
        }
    }
}
