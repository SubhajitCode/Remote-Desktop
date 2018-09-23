using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RdpClient
{
    public partial class Form2 : Form
    {
        
        public Form2(string connection)
        {
            InitializeComponent();
            axRDPViewer1.Connect(connection, "User1", "");
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            
        }
    }
}
