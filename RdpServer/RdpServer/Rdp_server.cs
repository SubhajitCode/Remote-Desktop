using RDPCOMAPILib;
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

namespace RdpServer
{
    public partial class Rdp_Server : Form
    {
        RDPSession x = new RDPSession();
        string connection = "";
        string Ip = "";
        int port = 8888;
        void serverstart()
           {
               Ip = GetIP();
            TcpListener Tcplsn = new TcpListener(IPAddress.Parse(Ip), port);
            try
            {
                Tcplsn.Start();
                while (true)
                {
                    TcpClient Client = Tcplsn.AcceptTcpClient();
                    if (Client.Connected)
                    {
                        Thread TcpHandlerThread = new Thread(new ParameterizedThreadStart(TcpHandler));
                        TcpHandlerThread.Start(Client);
                    }
                }//end of while
            }//end of try
            catch
            {

            }




        }//end of server start
        void TcpHandler(object Client)
        {
            TcpClient mClient = (TcpClient)Client;
            while (mClient.Connected)
            {
                try
                {
                    string Credential = "User Name : " + textBox1.Text + " Password : " + textBox2.Text;
                    NetworkStream strem = mClient.GetStream();

                    if (strem.CanRead)
                    {
                        byte[] buffer = new byte[mClient.ReceiveBufferSize];
                        strem.Read(buffer, 0, (int)mClient.ReceiveBufferSize);
                        string returndata = Encoding.UTF8.GetString(buffer);
                        if (string.Compare(returndata, "Credential") == 0)
                        {
                            if (strem.CanWrite)
                            {
                                byte[] buffer1 = Encoding.UTF8.GetBytes(Credential);
                                strem.Write(buffer1, 0, buffer1.Length);
                               // MessageBox.Show("Server : " + Credential);
                            }
                        }
                        if (string.Compare(returndata, "Loggedin") == 0)
                        {
                            
                            if (strem.CanWrite)
                            {
                                byte[] buffer1 = Encoding.UTF8.GetBytes(connection);
                                strem.Write(buffer1, 0, buffer1.Length);
                                //MessageBox.Show("Server : " + connection);
                            }
                        }

                    }
                    

                 }
                catch
                {

                }
            }

        }
        public Rdp_Server()
        {
            InitializeComponent();
        }
        private void Incoming(object Guest)
        {
            IRDPSRAPIAttendee MyGuest = (IRDPSRAPIAttendee)Guest;//???
            MyGuest.ControlLevel = CTRL_LEVEL.CTRL_LEVEL_INTERACTIVE;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox3.Text = GetIP();
            x.OnAttendeeConnected += Incoming;
            x.Open();
            IRDPSRAPIInvitation Invitation = x.Invitations.CreateInvitation("Trial", "MyGroup", "", 10);
            connection = Invitation.ConnectionString;
             Thread serverthread = new Thread(new ThreadStart(serverstart));
             serverthread.Start();

        }

        private void button2_Click(object sender, EventArgs e)
        {
                x.Close();
                x = null;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
        private string GetIP()
        {
            string strHostName = "";
            strHostName = System.Net.Dns.GetHostName();

            IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

            IPAddress[] addr = ipEntry.AddressList;

            return addr[addr.Length - 1].ToString();

        }

    }
}
