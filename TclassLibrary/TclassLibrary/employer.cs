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
    public partial class employer : Form
    {
        string cs = string.Empty;
        int ncompid = 0;
        string cLocalCaption = string.Empty;
        public employer()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void employer_Load(object sender, EventArgs e)
        {
            this.Text = cLocalCaption + "<< Employer Setup >>";
        }
    }
}
