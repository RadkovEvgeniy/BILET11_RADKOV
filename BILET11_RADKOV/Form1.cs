using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;

namespace BILET11_RADKOV
{
    public partial class Form1 : Form
    {
        DataBase_Connection DataBase = new DataBase_Connection();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var login = textBox1.Text;
            var password = textBox2.Text;
            var surname = textBox3.Text;
            var name = textBox4.Text;
            var email = textBox5.Text;

            String query = $"insert into Users(login, password, Surname, Name, Email) values ('{login}', '{password}', '{surname}', '{name}', '{email}')";

            SqlCommand command = new SqlCommand(query, DataBase.GetConnection());

            DataBase.OpenConnection();

            if (CheckReg())
            {
                return;
            }
            if (command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Регистрация выполнена успешно");
                MailMessage mes = new MailMessage();
                mes.From = new MailAddress("boss.radkov2000@list.ru");
                mes.To.Add(new MailAddress(textBox5.Text));
                mes.Subject = "Регистрация выполнена успешно!";
                mes.Body = textBox4.Text + ", Вы успешно зарегистрировались!";

                SmtpClient client = new SmtpClient();
                client.Host = "smtp.mail.ru";
                client.Port = 587;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("boss.radkov2000@list.ru", "z4tLFWH3hcRhydMeArm0");

                client.Send(mes);

                MessageBox.Show("Письмо успешно отправлено " + textBox5.Text + "!");
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";

            }
            else
            {
                MessageBox.Show("Произошла ошибка");
            }
        }

        private Boolean CheckReg()
        {
            var login = textBox1.Text;
            var password = textBox2.Text;
            var surname = textBox3.Text;
            var name = textBox4.Text;
            var email = textBox5.Text;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            String query = $"select login, password, Surname, Name, Email from Users where login = '{login}' and password = '{password}' and Surname = '{surname}' and Name = '{name}' and Email = '{email}'";

            SqlCommand command = new SqlCommand(query, DataBase.GetConnection());

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                MessageBox.Show("Такой аккаунт уже существует");
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
