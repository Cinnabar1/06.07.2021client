using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace PracticaTest2
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            tbPost.Text = Properties.Settings.Default.email;
            tbLogin.Text = Properties.Settings.Default.login;
            tbPassword.Text = Properties.Settings.Default.password;
            tbConnect.Text = Properties.Settings.Default.serverAddress;
            textBox1.Text = Properties.Settings.Default.dbname;
        }


        //void socket()
        //{
        //    try
        //    {
        //        SendMessageFromSocket(11000);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.ToString());
        //    }
        //    finally
        //    {
        //        Console.ReadLine();
        //    }
        //}

        void SendMessageFromSocket(int port, int number)
        {
            // Буфер для входящих данных
            byte[] bytes = new byte[1024];

            // Соединяемся с удаленным устройством

            // Устанавливаем удаленную точку для сокета
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);

            Socket sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Соединяем сокет с удаленной точкой
            sender.Connect(ipEndPoint);

            string message = "";
            //Console.Write("Введите сообщение: ");
            if (number == 1)
            {
                message = "1 " + Properties.Settings.Default.email;
            } else if (number == 2)
            {
                //message = "2 " + "Server=" + Properties.Settings.Default.serverAddress + ";Database=" + Properties.Settings.Default.dbname + ";User Id=" + Properties.Settings.Default.login + ";Password=" + Properties.Settings.Default.password;
                message = "2 " + "Server=" + Properties.Settings.Default.serverAddress + ";Database=" + Properties.Settings.Default.dbname + ";User Id=" + Properties.Settings.Default.login + ";Password=" + Properties.Settings.Default.password + ";Trusted_Connection=True;";
            }
            else if (number == 3)
            {
                message = "3";
            }
            
            Console.WriteLine("Сокет соединяется с {0} ", sender.RemoteEndPoint.ToString());
            byte[] msg = Encoding.UTF8.GetBytes(message);

            // Отправляем данные через сокет
            int bytesSent = sender.Send(msg);

            // Получаем ответ от сервера
            int bytesRec = sender.Receive(bytes);
            if (Encoding.UTF8.GetString(bytes, 0, bytesRec) == "good")
            {
                Properties.Settings.Default.working = "Работает";
                Properties.Settings.Default.Save();
            }
            Console.WriteLine("\nОтвет от сервера: {0}\n\n", Encoding.UTF8.GetString(bytes, 0, bytesRec));

            // Используем рекурсию для неоднократного вызова SendMessageFromSocket()
            //if (message.IndexOf("<TheEnd>") == -1)
            //    SendMessageFromSocket(port);

            // Освобождаем сокет
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }

        private void TextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (tbLogin.Text != "") { 
                Properties.Settings.Default.login = tbLogin.Text;
            }
            if (tbPassword.Text != "")
            {
                Properties.Settings.Default.password = tbPassword.Text;
            }
            if (tbConnect.Text != "")
            {
                Properties.Settings.Default.serverAddress = tbConnect.Text;
            }
            if (textBox1.Text != "")
            {
                Properties.Settings.Default.dbname = textBox1.Text;
            }
            Properties.Settings.Default.Save();
            try
            {
                SendMessageFromSocket(11000, 2);
            }
            catch
            {
                MessageBox.Show("Данные не удалось передать серверу.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.email = tbPost.Text;
            Properties.Settings.Default.Save();
            SendMessageFromSocket(11000, 1);
        }
    }
}
