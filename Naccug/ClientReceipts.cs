using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinTcare
{
    public partial class ClientReceipts : Form
    {
        public ClientReceipts()
        {
            InitializeComponent();
        }

        private void ClientReceipts_Load(object sender, EventArgs e)
        {
            //this.ControlBox = false;
            this.Text = globalvar.cLocalCaption + "<< Client Receipt >>";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
