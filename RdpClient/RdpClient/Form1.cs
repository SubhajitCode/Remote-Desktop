using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RdpClient
{
    public partial class Form1 : Form
    {
        bool _isloggedin = false;
        string invitation = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread ConnectThread = new Thread(new ThreadStart(ConnectAsClient));
            ConnectThread.Start();
            if (_isloggedin == true)
            {
                Form2 frm2 = new Form2(invitation);
                frm2.Show();
            }

        }
        void ConnectAsClient()
        {
            string ip = textBox1.Text;
            int port = 8888;

            TcpClient client = new TcpClient();
            client.Connect(IPAddress.Parse(ip), port);
            string Credential = "User Name : " + textBox2.Text + " Password : " + textBox3.Text;
            string returndata = "";
            if (client.Connected)
            {
                NetworkStream stream = client.GetStream();
                if (stream.CanWrite)
                {
                    byte[] buffer = Encoding.UTF8.GetBytes("Credential");
                    stream.Write(buffer, 0, buffer.Length);
                   // MessageBox.Show("credential req sent");
                }

                if(stream.CanRead)
                {
                    byte[] buffer = new byte[client.ReceiveBufferSize];
                    stream.Read(buffer,0,(int)client.ReceiveBufferSize);
                    returndata = Encoding.UTF8.GetString(buffer);
                    
                }
                
            }//End of if
            if(string.Compare(returndata,Credential)==0)
            {
                NetworkStream stream = client.GetStream();
               // MessageBox.Show("Logged in");
                if (stream.CanWrite)
                {
                    byte[] buffer = Encoding.UTF8.GetBytes("Loggedin");
                    stream.Write(buffer, 0, buffer.Length);
                   // MessageBox.Show("Loggedin req sent");
                }
                if (stream.CanRead)
                {
                    byte[] buffer3 = new byte[client.ReceiveBufferSize];
                    stream.Read(buffer3, 0, (int)client.ReceiveBufferSize);
                   string returndata2 = Encoding.UTF8.GetString(buffer3);
                   _isloggedin = true;
                   invitation = returndata2;
                   
                   MessageBox.Show("client :: connection string " + returndata2);

                }


            }
            else
            {
                MessageBox.Show("Logged in Faliure");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (_isloggedin == true)
            {
                Form2 frm2 = new Form2(invitation);
                frm2.Show();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }//EndPoint of connect Client
    }
}
