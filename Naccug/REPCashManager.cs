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
    public partial class REPCashManager : Form
    {
        string cs = globalvar.cos;

        ReportDocument rprt = new ReportDocument();
        public REPCashManager()
        {
            InitializeComponent();
        }

        private void REPCashManager_Load(object sender, EventArgs e)
        {
            UserCombo();
            companyCombo();
            BranchCombo();
        }
        void UserCombo()
        {
            SqlConnection ndConnHandle1 = new SqlConnection(cs);
            ndConnHandle1.Open();
            SqlCommand cmd = new SqlCommand("[SPSusers]", ndConnHandle1);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[SPSUsers]";
            SqlDataReader reader;
            reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();

            dt.Columns.Add("oprcode", typeof(string));
            //  dt.Columns.Add("oprcode", typeof(string));
            dt.Load(reader);

            comboBox2.ValueMember = "oprcode";
            comboBox2.DisplayMember = "oprcode";
            comboBox2.DataSource = dt;
            comboBox2.SelectedIndex = -1;
            ndConnHandle1.Close();
        }
        public void GETDATA()
        {
             string UserID = Convert.ToString(comboBox2.SelectedValue);
            int nbranch = Convert.ToInt32(comboBox3.SelectedValue);
            int nCompid = Convert.ToInt32(comboBox1.SelectedValue);

            SqlConnection ndConnHandle1 = new SqlConnection(cs);
            rprt.Load(System.Windows.Forms.Application.StartupPath + "\\Reports\\CRYCashManager.rpt");
            SqlCommand cmd = new SqlCommand("SPCashManager", ndConnHandle1);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserID", UserID);
            cmd.Parameters.AddWithValue("@branch", nbranch);
            cmd.Parameters.AddWithValue("@lnCompID", nCompid);
            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@FromDate", SqlDbType.Date)).Value = dateTimePicker1.Text;
            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ToDate", SqlDbType.Date)).Value = dateTimePicker2.Text;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            SPCashManagerSet ds = new SPCashManagerSet();
            sda.Fill(ds, "SPCashManager");
            rprt.SetDataSource(ds);
            crystalReportViewer1.ReportSource = rprt;
        }
        void companyCombo()
        {
            //int nbranch = Convert.ToInt32(comboBox1.SelectedValue);
            SqlConnection ndConnHandle1 = new SqlConnection(cs);
            ndConnHandle1.Open();
            SqlCommand cmd = new SqlCommand("[SPCompany]", ndConnHandle1);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[SPCompany]";
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

            comboBox3.ValueMember = "branchid";
            comboBox3.DisplayMember = "br_name";
            comboBox3.DataSource = dt;
            comboBox3.SelectedIndex = -1;
            ndConnHandle1.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            comboBox2.Visible = false;
            label5.Visible = false;
            comboBox1.Visible = false;
            comboBox3.Visible = false;
            dateTimePicker1.Visible = false;
            dateTimePicker2.Visible = false;
            button1.Visible = false;
            GETDATA();
            WindowState = FormWindowState.Maximized;
        }
    }
}
