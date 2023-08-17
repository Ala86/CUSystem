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
    public partial class ServiceDefine : Form
    {
        public ServiceDefine()
        {
            InitializeComponent();
        }

        private void ServiceDefine_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + "<< Service Definition >>";
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
