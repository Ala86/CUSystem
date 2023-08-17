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
    public partial class employees : Form
    {
        public employees()
        {
            InitializeComponent();
        }

        private void employees_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + "<< Staff Management >>";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
