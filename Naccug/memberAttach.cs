using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TclassLibrary;
using System.Data.SqlClient;

namespace WinTcare
{
    public partial class memberAttach : Form
    {
        string cs = globalvar.cos;
        int ncompid = globalvar.gnCompid;
        string cloca = globalvar.cLocalCaption;
        public memberAttach()
        {
            InitializeComponent();
        }

        private void memberAttach_Load(object sender, EventArgs e)
        {
            this.Text = cloca + " << Member Attachment Setup >>";
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button29_Click(object sender, EventArgs e)
        {
            FindClient fc = new FindClient(cs, ncompid, cloca,1,"Cusreg");
            fc.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FindClient fc = new FindClient(cs,ncompid,cloca, 1, "Cusreg");
            fc.ShowDialog();
        }
    }
}
