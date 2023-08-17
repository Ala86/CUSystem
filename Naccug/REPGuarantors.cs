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
    public partial class REPGuarantors : Form
    {
        string cs = globalvar.cos;

        ReportDocument rprt = new ReportDocument();
     

        public REPGuarantors()
        {
            InitializeComponent();
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            comboBox1.Visible = false;
            comboBox3.Visible = false;
            dateTimePicker1.Visible = false;
            dateTimePicker2.Visible = false;
            button1.Visible = false;
            GETDATA();
            WindowState = FormWindowState.Maximized;
        }
        public void GETDATA()
        {
            int nbranch = Convert.ToInt32(comboBox3.SelectedValue);
            SqlConnection ndConnHandle1 = new SqlConnection(cs);
            rprt.Load(System.Windows.Forms.Application.StartupPath + "\\Reports\\CRPGuarantors.rpt");
            SqlCommand cmd = new SqlCommand("SPGuarantors", ndConnHandle1);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProductType", nbranch);
            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@TranFromDate", SqlDbType.Date)).Value = dateTimePicker1.Text;
            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@TranToDate", SqlDbType.Date)).Value = dateTimePicker2.Text;
            //cmd.Parameters.AddWithValue("@gender", textBox6.Text);
            //cmd.Parameters.AddWithValue("@gender1", textBox5.Text);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            SPGuarantors ds = new SPGuarantors();
            sda.Fill(ds, "SPGuarantors");
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

        private void REPGuarantors_Load(object sender, EventArgs e)
        {
            companyCombo();
            ProductCombo();
        }
        void ProductCombo()
        {
            SqlConnection ndConnHandle1 = new SqlConnection(cs);
            ndConnHandle1.Open();
            SqlCommand cmd = new SqlCommand("[SPProductTypel]", ndConnHandle1);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[SPProductTypel]";
            SqlDataReader reader;
            reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();

            dt.Columns.Add("prd_id", typeof(string));
            dt.Columns.Add("prd_name", typeof(string));
            dt.Load(reader);

            comboBox3.ValueMember = "prd_id";
            comboBox3.DisplayMember = "prd_name";
            comboBox3.DataSource = dt;
            comboBox3.SelectedIndex = -1;
            ndConnHandle1.Close();
        }
    }
}
