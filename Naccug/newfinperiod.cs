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
    public partial class newfinperiod : Form
    {
        public newfinperiod()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void newfinperiod_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + "<< Account Period Setup >>";
        }
    }
}
