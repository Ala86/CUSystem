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
using CrystalDecisions.CrystalReports.Engine;

namespace WinTcare
{
    public partial class REPAccounts : Form
    {
        string cs = globalvar.cos;

        ReportDocument rprt = new ReportDocument();
        public REPAccounts()
        {
            InitializeComponent();
        }

        private void REPAccounts_Load(object sender, EventArgs e)
        {
           
        }
        public void GETDATA()
        {
         
            SqlConnection ndConnHandle1 = new SqlConnection(cs);
            rprt.Load(System.Windows.Forms.Application.StartupPath + "\\Reports\\CRPAccounts.rpt");
            SqlCommand cmd = new SqlCommand("SPAccounts", ndConnHandle1);
            cmd.CommandType = CommandType.StoredProcedure;
           cmd.Parameters.AddWithValue("@intcode_id", textBox1.Text);
            cmd.Parameters.AddWithValue("@intcode_id1", textBox2.Text);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            SPAccounts ds = new SPAccounts();
            sda.Fill(ds, "SPAccounts");
            rprt.SetDataSource(ds);
            crystalReportViewer1.ReportSource = rprt;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
            button1.Visible = false;
            textBox1.Visible = false;
            textBox2.Visible = false;
            GETDATA();
            crystalReportViewer1.MaximumSize = MaximumSize;
            WindowState = FormWindowState.Maximized;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

           
            int che4 = 1;
            if (checkBox1.Checked)
            {
               // che4 = Convert.ToInt32(textBox1.Text).ToString();
               // che4 = Convert.ToInt32(textBox1.Text);
                //  MessageBox.Show("saving is done");
               textBox1.Text = che4.ToString();
            }
            else

            {
                textBox1.Text = "";
                    
            };
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            int che4 = 2;
            if (checkBox2.Checked)
            {
               // che4 = Convert.ToInt32(textBox2.Text).ToString(); 
                //  MessageBox.Show("saving is done");
                textBox2.Text = che4.ToString();
            }
            else

            {
                textBox2.Text = "";

            };

        }
    }
}
