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
    public partial class acRec : Form
    {
        public acRec()
        {
            InitializeComponent();
        }

        private void acRec_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + "<< Accounts Receivables >>";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
