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
    public partial class Bonding : Form
    {
        string cs = string.Empty;
        int ncompid = 0;
        string cLoca = string.Empty;

        public Bonding(string cos, int dcompid, string dloca)
        {
            InitializeComponent();
            cs = cos;
            ncompid = dcompid;
            cLoca = dloca;
        }



        private void Bonding_Load(object sender, EventArgs e)
        {
            this.Text = cLoca + "<< Training Bond Details >>";
        }
    }
}
