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
    public partial class sched : Form
    {
        public sched()
        {
            InitializeComponent();
        }

        private void sched_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + " << Appointment >>";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
