using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace PracticaTest2
{


    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            //trayIcon = new NotifyIcon();
            //trayIcon.Text = "My application";
            //trayIcon.Icon = TheIcon
        }
        List<string[]> data1 = new List<string[]>();
        List<string[]> data2 = new List<string[]>();

        // меняем текст лейбла
        async void updateStatus()
        {
            while (true) { 
            labelStatus.Text = Properties.Settings.Default.working;
            if (labelStatus.Text == "Не работает")
                {
                    labelStatus.ForeColor = System.Drawing.Color.Red;
                }
            else
                {
                    labelStatus.ForeColor = System.Drawing.Color.Green;
                }
            await Task.Delay(30000);
            }
        }

        private void ButtonSettings_Click(object sender, EventArgs e)
        {
            Form2 fr2 = new Form2();
            fr2.Show();
            //this.Enabled = false;
        }

        private void ButtonDownErrors_Click(object sender, EventArgs e)
        {
            //Console.WriteLine(String.Format("SELECT * FROM PMServiceLogs WHERE ErrStatus = 1 AND DateTimeS BETWEEN '{0}' AND '{1}' ORDER BY ID", dtpBegin.Value.ToString("yyyy-MM-ddThh:mm:ss"), dtpEnd.Value.ToString("yyyy-MM-ddThh:mm:ss")));
            if (dtpBegin.Value < dtpEnd.Value)
            {

                dataGridView1.Rows.Clear();
                data1.Clear();
                data2.Clear();
                comboBox1.Items.Clear();
                comboBox1.Items.Add("Все");


                //string connectionString1 = "Server=" + Properties.Settings.Default.serverAddress + ";Database=" + Properties.Settings.Default.dbname + ";UserId=" + Properties.Settings.Default.login + ";Password=" + Properties.Settings.Default.password + ";";
                //Console.WriteLine(connectionString1);
                //string connectionString = "Server=.\\SQLEXPRESS;Database=Test;Trusted_Connection=True;";
                SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder();
                connectionStringBuilder.DataSource = Properties.Settings.Default.serverAddress;       // используйте конструктор строк подключения для 
                connectionStringBuilder.InitialCatalog = Properties.Settings.Default.dbname;          // предотвращения изменения пользователем структуры строки подключения
                //connectionStringBuilder.IntegratedSecurity = true;
                connectionStringBuilder.UserID = Properties.Settings.Default.login;
                connectionStringBuilder.Password = Properties.Settings.Default.password;

                string sqlExpression = String.Format("SELECT * FROM PMServiceLogs WHERE ErrStatus = 1 AND DateTimeS BETWEEN '{0}' AND '{1}' ORDER BY ID", dtpBegin.Value.ToString("yyyy-MM-ddThh:mm:ss"), dtpEnd.Value.ToString("yyyy-MM-ddThh:mm:ss"));
                string sqlExpression1 = String.Format("SELECT * FROM PMServiceLogs_H WHERE ErrStatus = 1 AND DateTimeS BETWEEN '{0}' AND '{1}' ORDER BY ID", dtpBegin.Value.ToString("yyyy-MM-ddThh:mm:ss"), dtpEnd.Value.ToString("yyyy-MM-ddThh:mm:ss"));
                //string sqlExpression1 = String.Format("SELECT * FROM PMServiceLogs_H WHERE ErrStatus = 1 AND DateTimeS BETWEEN CAST('{0}', AS datetime) AND CAST('{1}', AS datetime) ORDER BY ID",  dtpEnd.Value.ToString(), dtpBegin.Value.ToString());
                string sqlExpression2 = String.Format("SELECT * FROM PointMasterLogs WHERE (Msg LIKE '%Error%' OR Msg LIKE '%error%' OR Msg LIKE '%ERROR%') AND DateTimeS BETWEEN '{0}' AND '{1}' ORDER BY ID", dtpBegin.Value.ToString("yyyy-MM-ddThh:mm:ss"), dtpEnd.Value.ToString("yyyy-MM-ddThh:mm:ss"));
                // автоматически закрывает соединение 
                try {
                    //using (SqlConnection connection = new SqlConnection(connectionString1))
                    using (SqlConnection connection = new SqlConnection(connectionStringBuilder.ConnectionString))
                    {
                        
                            connection.Open();
                            MessageBox.Show("Подключение открыто. Если вы видите это сообщение, сообщите о нем по обратной связи.", "Успех!", MessageBoxButtons.OK,);
                            SqlCommand command = new SqlCommand(sqlExpression, connection);
                            SqlDataReader reader = command.ExecuteReader();
                       

                        //List<string[]> data1 = new List<string[]>();
                        //List<string[]> data2 = new List<string[]>();
                        List<string> list = new List<string>();
                        try
                        {
                            if (reader.HasRows) // если есть данные
                        {
                            // выводим названия столбцов
                            //Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}", reader.GetName(0), reader.GetName(1), reader.GetName(2), reader.GetName(3), reader.GetName(4), reader.GetName(5), reader.GetName(6), reader.GetName(7), reader.GetName(8));
                            
                                while (reader.Read()) // построчно считываем данные
                            {
                                data1.Add(new string[10]);
                                data1[data1.Count - 1][0] = reader[0].ToString();
                                data1[data1.Count - 1][1] = " ";
                                data1[data1.Count - 1][2] = reader[1].ToString();
                                data1[data1.Count - 1][3] = reader[2].ToString();
                                data1[data1.Count - 1][4] = reader[3].ToString();
                                data1[data1.Count - 1][5] = reader[4].ToString();
                                data1[data1.Count - 1][6] = reader[5].ToString();
                                data1[data1.Count - 1][7] = reader[6].ToString();
                                data1[data1.Count - 1][8] = reader[7].ToString();
                                data1[data1.Count - 1][9] = reader[8].ToString();
                                if (!list.Contains(reader[4].ToString()))
                                {
                                    list.Add(reader[4].ToString());
                                }

                                //object ID = reader.GetValue(0);
                                //object DateTimeS = reader.GetValue(1);
                                //object FileName = reader.GetValue(2);
                                //object FileNameDest = reader.GetValue(3);
                                //object Operation = reader.GetValue(4);
                                //object MngrName = reader.GetValue(5);
                                //object MngrID = reader.GetValue(6);
                                //object ErrStatus = reader.GetValue(7);
                                //object Comment = reader.GetValue(8);
                                //Console.WriteLine("{0} \t{1} \t{2} \t{3} \t{4} \t{5} \t{6} \t{7} \t{8}", ID, DateTimeS, FileName, FileNameDest, Operation, MngrName, MngrID, ErrStatus, Comment);
                            }
                        }
                        }
                        catch (Exception x)
                        {
                            MessageBox.Show("Ошибка при считывании данных с первой таблицы. Если вы видите это сообщение, сообщите о нем по обратной связи.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        reader.Close();
                        SqlCommand command1 = new SqlCommand(sqlExpression1, connection);
                        SqlDataReader reader1 = command1.ExecuteReader();
                        try
                        {
                            if (reader1.HasRows) // если есть данные
                        {


                            while (reader1.Read()) // построчно считываем данные
                            {
                                data1.Add(new string[10]);
                                data1[data1.Count - 1][0] = reader1[0].ToString();
                                data1[data1.Count - 1][1] = " ";
                                data1[data1.Count - 1][2] = reader1[1].ToString();
                                data1[data1.Count - 1][3] = reader1[2].ToString();
                                data1[data1.Count - 1][4] = reader1[3].ToString();
                                data1[data1.Count - 1][5] = reader1[4].ToString();
                                data1[data1.Count - 1][6] = reader1[5].ToString();
                                data1[data1.Count - 1][7] = reader1[6].ToString();
                                data1[data1.Count - 1][8] = reader1[7].ToString();
                                data1[data1.Count - 1][9] = reader1[8].ToString();
                                if (!list.Contains(reader1[4].ToString()))
                                {
                                    list.Add(reader1[4].ToString());
                                }
                            }
                        }
                            catch (Exception x)
                        {
                            MessageBox.Show("Ошибка при считывании данных из второй таблицы. Если вы видите это сообщение, сообщите о нем по обратной связи.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        reader1.Close();

                        SqlCommand command2 = new SqlCommand(sqlExpression2, connection);
                        SqlDataReader reader2 = command2.ExecuteReader();
                            try
                            {
                                if (reader2.HasRows) // если есть данные
                        {


                            while (reader2.Read()) // построчно считываем данные
                            {
                                data2.Add(new string[10]);
                                data2[data2.Count - 1][0] = reader2[0].ToString();
                                data2[data2.Count - 1][1] = reader2[1].ToString();
                                data2[data2.Count - 1][2] = reader2[5].ToString();
                                data2[data2.Count - 1][3] = reader2[2].ToString();
                                data2[data2.Count - 1][4] = " ";
                                data2[data2.Count - 1][5] = " ";
                                data2[data2.Count - 1][6] = reader2[3].ToString();
                                data2[data2.Count - 1][7] = reader2[4].ToString();
                                data2[data2.Count - 1][8] = " ";
                                data2[data2.Count - 1][9] = reader2[6].ToString();
                                //if (!list.Contains(reader2[4].ToString()))
                                //{
                                //    list.Add(reader2[4].ToString());
                                //}
                            }
                        }
                                catch (Exception x)
                        {
                            MessageBox.Show("Ошибка при считывании данных из третьей таблицы. Если вы видите это сообщение, сообщите о нем по обратной связи.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }


                        reader2.Close();

                        foreach (string[] s in data1)
                            dataGridView1.Rows.Add(s);

                        foreach (string[] s in data2)
                            dataGridView1.Rows.Add(s);

                        foreach (string s in list)
                            comboBox1.Items.Add(s);

                    }
                    Console.WriteLine("Подключение закрыто...");
                }
                catch
                {
                    MessageBox.Show("Проверьте настройки подключения к базе данных, либо наличие необходимых таблиц","Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            else
            {
                MessageBox.Show("Дата начала периода должна быть меньше даты окончаия периода.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            updateStatus();
            Thread myThread = new Thread(new ThreadStart(EmailScheduler.Start)); //EmailScheduler.Start();
            myThread.Start();

            //labelStatus.Text = Properties.Settings.Default.working;
            dataGridView1.Rows.Clear();
            data1.Clear();
            data2.Clear();
            comboBox1.Items.Clear();
            comboBox1.Items.Add("Все");

            //string connectionString = "Server=.\\SQLEXPRESS;Database=Test;Trusted_Connection=True;";
            //string connectionString1 = "Server=" + Properties.Settings.Default.serverAddress + ";Database=" + Properties.Settings.Default.dbname + ";UserId=" + Properties.Settings.Default.login + ";Password=" + Properties.Settings.Default.password + ";";
            SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder();
            connectionStringBuilder.DataSource = Properties.Settings.Default.serverAddress;       // используйте конструктор строк подключения для 
            connectionStringBuilder.InitialCatalog = Properties.Settings.Default.dbname;          // предотвращения изменения пользователем структуры строки подключения
            //connectionStringBuilder.IntegratedSecurity = true;
            connectionStringBuilder.UserID = Properties.Settings.Default.login;
            connectionStringBuilder.Password = Properties.Settings.Default.password;

            string sqlExpression = "SELECT * FROM PMServiceLogs WHERE ErrStatus = 1 AND DateTimeS >= DATEADD(week, -1, GETDATE()) ORDER BY ID";
            string sqlExpression1 = "SELECT * FROM PMServiceLogs_H WHERE ErrStatus = 1 AND DateTimeS >= DATEADD(week, -1, GETDATE()) ORDER BY ID";
            string sqlExpression2 = "SELECT * FROM PointMasterLogs WHERE (Msg LIKE '%Error%' OR Msg LIKE '%error%' OR Msg LIKE '%ERROR%') AND DateTimeS >= DATEADD(week, -1, GETDATE()) ORDER BY ID";
            // автоматически закрывает соединение
            try {
                //using (SqlConnection connection = new SqlConnection(connectionString1))
                using (SqlConnection connection = new SqlConnection(connectionStringBuilder.ConnectionString))
                {
                    connection.Open();
                    Console.WriteLine("Подключение открыто");
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    //List<string[]> data1 = new List<string[]>();
                    //List<string[]> data2 = new List<string[]>();
                    List<string> list = new List<string>();
                    if (reader.HasRows) // если есть данные
                    {
                        // выводим названия столбцов
                        //Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}", reader.GetName(0), reader.GetName(1), reader.GetName(2), reader.GetName(3), reader.GetName(4), reader.GetName(5), reader.GetName(6), reader.GetName(7), reader.GetName(8));

                        while (reader.Read()) // построчно считываем данные
                        {
                            data1.Add(new string[10]);
                            data1[data1.Count - 1][0] = reader[0].ToString();
                            data1[data1.Count - 1][1] = " ";
                            data1[data1.Count - 1][2] = reader[1].ToString();
                            data1[data1.Count - 1][3] = reader[2].ToString();
                            data1[data1.Count - 1][4] = reader[3].ToString();
                            data1[data1.Count - 1][5] = reader[4].ToString();
                            data1[data1.Count - 1][6] = reader[5].ToString();
                            data1[data1.Count - 1][7] = reader[6].ToString();
                            data1[data1.Count - 1][8] = reader[7].ToString();
                            data1[data1.Count - 1][9] = reader[8].ToString();
                            if (!list.Contains(reader[4].ToString()))
                            {
                                list.Add(reader[4].ToString());
                            }

                            //object ID = reader.GetValue(0);
                            //object DateTimeS = reader.GetValue(1);
                            //object FileName = reader.GetValue(2);
                            //object FileNameDest = reader.GetValue(3);
                            //object Operation = reader.GetValue(4);
                            //object MngrName = reader.GetValue(5);
                            //object MngrID = reader.GetValue(6);
                            //object ErrStatus = reader.GetValue(7);
                            //object Comment = reader.GetValue(8);
                            //Console.WriteLine("{0} \t{1} \t{2} \t{3} \t{4} \t{5} \t{6} \t{7} \t{8}", ID, DateTimeS, FileName, FileNameDest, Operation, MngrName, MngrID, ErrStatus, Comment);
                        }
                    }

                    reader.Close();
                    SqlCommand command1 = new SqlCommand(sqlExpression1, connection);
                    SqlDataReader reader1 = command1.ExecuteReader();

                    if (reader1.HasRows) // если есть данные
                    {


                        while (reader1.Read()) // построчно считываем данные
                        {
                            data1.Add(new string[10]);
                            data1[data1.Count - 1][0] = reader1[0].ToString();
                            data1[data1.Count - 1][1] = " ";
                            data1[data1.Count - 1][2] = reader1[1].ToString();
                            data1[data1.Count - 1][3] = reader1[2].ToString();
                            data1[data1.Count - 1][4] = reader1[3].ToString();
                            data1[data1.Count - 1][5] = reader1[4].ToString();
                            data1[data1.Count - 1][6] = reader1[5].ToString();
                            data1[data1.Count - 1][7] = reader1[6].ToString();
                            data1[data1.Count - 1][8] = reader1[7].ToString();
                            data1[data1.Count - 1][9] = reader1[8].ToString();
                            if (!list.Contains(reader1[4].ToString()))
                            {
                                list.Add(reader1[4].ToString());
                            }
                        }
                    }

                    reader1.Close();

                    SqlCommand command2 = new SqlCommand(sqlExpression2, connection);
                    SqlDataReader reader2 = command2.ExecuteReader();

                    if (reader2.HasRows) // если есть данные
                    {


                        while (reader2.Read()) // построчно считываем данные
                        {
                            data2.Add(new string[10]);
                            data2[data2.Count - 1][0] = reader2[0].ToString();
                            data2[data2.Count - 1][1] = reader2[1].ToString();
                            data2[data2.Count - 1][2] = reader2[5].ToString();
                            data2[data2.Count - 1][3] = reader2[2].ToString();
                            data2[data2.Count - 1][4] = " ";
                            data2[data2.Count - 1][5] = " ";
                            data2[data2.Count - 1][6] = reader2[3].ToString();
                            data2[data2.Count - 1][7] = reader2[4].ToString();
                            data2[data2.Count - 1][8] = " ";
                            data2[data2.Count - 1][9] = reader2[6].ToString();
                            //if (!list.Contains(reader2[4].ToString()))
                            //{
                            //    list.Add(reader2[4].ToString());
                            //}
                        }
                    }

                    reader2.Close();


                    foreach (string[] s in data1)
                        dataGridView1.Rows.Add(s);


                    foreach (string[] s in data2)
                        dataGridView1.Rows.Add(s);

                    foreach (string s in list)
                        comboBox1.Items.Add(s);

                }
                Console.WriteLine("Подключение закрыто...");
            }
            catch
            {
                MessageBox.Show("Проверьте настройки подключения к базе данных, либо наличие необходимых таблиц", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            if (comboBox1.SelectedIndex == 0)
            {
                foreach (string[] s in data1)
                    dataGridView1.Rows.Add(s);

                foreach (string[] s in data2)
                    dataGridView1.Rows.Add(s);

            }
            else { 
                foreach (string[] s in data1)
                {
                    if (s[5] == comboBox1.Text)
                        dataGridView1.Rows.Add(s);
                }
                foreach (string[] s in data2)
                {
                    if (s[5] == comboBox1.Text)
                        dataGridView1.Rows.Add(s);
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //this.Hide();
            //e.Cancel = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }
    }
}
