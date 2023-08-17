using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TclassLibrary
{
    public partial class PayPara : Form
    {
        string cs = string.Empty;
        int ncompid = 0;
        string dloca = string.Empty;
        public PayPara(string tcCos, int tnCompid, string tcLoca)
        {
            InitializeComponent();
            cs = tcCos;
            ncompid = tnCompid;
            dloca = tcLoca;
        }

        private void PayPara_Load(object sender, EventArgs e)
        {
            this.Text = dloca + "<< Payroll Parameters Setup >>";

        }

        private void button25_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
