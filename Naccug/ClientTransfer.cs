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
    public partial class ClientTransfer : Form
    {
        public ClientTransfer()
        {
            InitializeComponent();
        }

        private void ClientTransfer_Load(object sender, EventArgs e)
        {
            //this.ControlBox = false;
            this.Text = globalvar.cLocalCaption + "<< Client Transfer >>";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
