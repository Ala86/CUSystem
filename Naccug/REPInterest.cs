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
using TclassLibrary;

namespace WinTcare
{
    public partial class REPInterest : Form
    {
        string cs = globalvar.cos;
        ReportDocument rprt = new ReportDocument();
        public REPInterest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Visible = false;
            comboBox1.Visible = false;
            label1.Visible = false;
            // int nbranch = Convert.ToInt32(comboBox2.SelectedValue);
            SqlConnection ndConnHandle1 = new SqlConnection(cs);
            rprt.Load(System.Windows.Forms.Application.StartupPath + "\\Reports\\CRPInterestAccuredBal.rpt");
            SqlCommand cmd = new SqlCommand("SPnminimumBalRep", ndConnHandle1);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@prd_id", comboBox1.SelectedValue);
            // cmd.Parameters.AddWithValue("@gender", textBox6.Text);
            // cmd.Parameters.AddWithValue("@gender1", textBox5.Text);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            MinmumBalDATA ds = new MinmumBalDATA();
            sda.Fill(ds, "SPnminimumBalRep");
            rprt.SetDataSource(ds);
            crystalReportViewer1.ReportSource = rprt;
            WindowState = FormWindowState.Maximized;
        }
        void ProductCombo()
        {
            SqlConnection ndConnHandle1 = new SqlConnection(cs);
            ndConnHandle1.Open();
            SqlCommand cmd = new SqlCommand("[SPProductType]", ndConnHandle1);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[SPProductType]";
            SqlDataReader reader;
            reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            //if (reader != null)
            //{
            dt.Columns.Add("prd_id", typeof(string));
            dt.Columns.Add("prd_name", typeof(string));
            dt.Load(reader);

            comboBox1.ValueMember = "prd_id";
            comboBox1.DisplayMember = "prd_name";
            comboBox1.DataSource = dt;
            ndConnHandle1.Close();
            // }
        }

        private void REPInterest_Load(object sender, EventArgs e)
        {
            ProductCombo();
        }
    }
}
