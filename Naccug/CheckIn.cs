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
    public partial class CheckIn : Form
    {
        public CheckIn()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CheckIn_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + "<< Admission >>";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
