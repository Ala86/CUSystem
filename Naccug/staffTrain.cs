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
    public partial class staffTrain : Form
    {
        public staffTrain()
        {
            InitializeComponent();
        }

        private void staffTrain_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + "<< Staff Training >>";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
