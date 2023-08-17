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
    public partial class REPTrialBalance : Form
    {
        string cs = globalvar.cos;

        ReportDocument rprt = new ReportDocument();
      //  string cs = globalvar.cos;
        public REPTrialBalance()
        {

            InitializeComponent();

        }
        private void REPTrialBalance_Load(object sender, EventArgs e)
        {
            BranchCombo();
            companyCombo();

        }
        void BranchCombo()
        {
            SqlConnection ndConnHandle1 = new SqlConnection(cs);
            ndConnHandle1.Open();
            SqlCommand cmd = new SqlCommand("[SPbranch]", ndConnHandle1);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[SPbranch]";
            SqlDataReader reader;
            reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();

            dt.Columns.Add("branchid", typeof(string));
            dt.Columns.Add("br_name", typeof(string));
            dt.Load(reader);

            comboBox2.ValueMember = "branchid";
            comboBox2.DisplayMember = "br_name";
            comboBox2.DataSource = dt;
            comboBox2.SelectedIndex = -1;
            ndConnHandle1.Close();
        }
        void companyCombo()
        {
            SqlConnection ndConnHandle1 = new SqlConnection(cs);
            ndConnHandle1.Open();
            SqlCommand cmd = new SqlCommand("[SPcompany]", ndConnHandle1);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[SPcompany]";
            SqlDataReader reader;
            reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();

            dt.Columns.Add("compid", typeof(string));
            dt.Columns.Add("com_name", typeof(string));
            dt.Load(reader);

            comboBox1.ValueMember = "compid";
            comboBox1.DisplayMember = "com_name";
            comboBox1.DataSource = dt;
            comboBox1.SelectedIndex = -1;
            ndConnHandle1.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            label1.Visible = false;
            label20.Visible = false;
            label14.Visible = false;
            label6.Visible = false;
            //textBox1.Visible = false;
            comboBox1.Visible = false;
            comboBox2.Visible = false;
            dateTimePicker1.Visible = false;
            dateTimePicker2.Visible = false;
            button1.Visible = false;

            GETDATA();
            WindowState = FormWindowState.Maximized;
        }
        public void GETDATA()
        {
            int ncompid = Convert.ToInt32(comboBox1.SelectedValue);
            int nbranch = Convert.ToInt32(comboBox2.SelectedValue);
            SqlConnection ndConnHandle1 = new SqlConnection(cs);
            rprt.Load(System.Windows.Forms.Application.StartupPath + "\\Reports\\CRPTrialBalance.rpt");
            SqlCommand cmd = new SqlCommand("tsp_TrialBalance", ndConnHandle1);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@lnCompID", ncompid);
            cmd.Parameters.AddWithValue("@branch", nbranch);
        //    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@FromDate", SqlDbType.Date)).Value = dateTimePicker1.Text;
            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ToDate", SqlDbType.Date)).Value = dateTimePicker2.Text;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            SPTrialBalanceDataset ds = new SPTrialBalanceDataset();
            sda.Fill(ds, "tsp_TrialBalance");
            rprt.SetDataSource(ds);
            crystalReportViewer1.ReportSource = rprt;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void crystalReportViewer1_ReportRefresh(object source, CrystalDecisions.Windows.Forms.ViewerEventArgs e)
        {
            this.Hide();
            REPTrialBalance ala = new REPTrialBalance();
            ala.ShowDialog();
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
