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
    public partial class StockTransfer : Form
    {
        string cs = string.Empty;
        int ncompid = 0;
        string dloca = string.Empty;

        public StockTransfer(string tcCos, int tnCompid, string tcLoca)
        {
            InitializeComponent();
            cs = tcCos;
            ncompid = tnCompid;
            dloca = tcLoca;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void StockTransfer_Load(object sender, EventArgs e)
        {
            this.Text = dloca + "<< Stock Transfer >>";
        }
    }
}
