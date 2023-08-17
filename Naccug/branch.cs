using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using TclassLibrary;

namespace WinTcare
{
    public partial class branch : Form
    {
        string dtime = "";
        public branch(string cTime)
        {
            InitializeComponent();
            dtime = cTime;
        }


        private void branch_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + "<< Branch Setup >>" + globalvar.gcFormCaption;
            if(dtime == "C")
            {
                MessageBox.Show("We will be doing company branch ");
            } else { MessageBox.Show("We will be doing bank branch"); }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
