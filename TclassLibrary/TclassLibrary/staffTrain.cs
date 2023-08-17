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
    public partial class staffTrain : Form
    {
        string cs = string.Empty;
        int ncompid = 0;
        string dloca = string.Empty;

        public staffTrain(string tcCos, int tnCompid, string tcLoca)
        {
            InitializeComponent();
            cs = tcCos;
            ncompid = tnCompid;
            dloca = tcLoca;
        }
        private void staffTrain_Load(object sender, EventArgs e)
        {
            this.Text = dloca+ "<< Staff Training >>";
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
