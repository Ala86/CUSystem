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
    public partial class REPSavingsByValue : Form
    {
        string cs = globalvar.cos;

        ReportDocument rprt = new ReportDocument();
        public REPSavingsByValue()
        {
            InitializeComponent();
        }

        private void REPSavingsByValue_Load(object sender, EventArgs e)
        {
            BranchCombo();
            ProductCombo();
            CurrencyCombo();
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
        void CurrencyCombo()
        {
            SqlConnection ndConnHandle1 = new SqlConnection(cs);
            ndConnHandle1.Open();
            SqlCommand cmd = new SqlCommand("[SPcurrency]", ndConnHandle1);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[SPcurrency]";
            SqlDataReader reader;
            reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();

            dt.Columns.Add("curr_code", typeof(string));
            dt.Columns.Add("curr_name", typeof(string));
            dt.Load(reader);

            comboBox1.ValueMember = "curr_code";
            comboBox1.DisplayMember = "curr_name";
            comboBox1.DataSource = dt;
            comboBox1.SelectedIndex = -1;
            ndConnHandle1.Close();
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

            dt.Columns.Add("prd_id", typeof(string));
            dt.Columns.Add("prd_name", typeof(string));
            dt.Load(reader);

            comboBox3.ValueMember = "prd_id";
            comboBox3.DisplayMember = "prd_name";
            comboBox3.DataSource = dt;
            comboBox3.SelectedIndex = -1;
            ndConnHandle1.Close();
        }
        public void GETDATA()
        {
            int nbranch = Convert.ToInt32(comboBox2.SelectedValue);
            int nProductType = Convert.ToInt32(comboBox3.SelectedValue);
            double tcCode = Convert.ToDouble(textBox1.Text);
            //MessageBox.Show(Convert.ToInt32(textBox1.Text));
            double tcCode1= Convert.ToDouble(textBox2.Text);
            SqlConnection ndConnHandle1 = new SqlConnection(cs);
            ndConnHandle1.Open();
            rprt.Load(System.Windows.Forms.Application.StartupPath + "\\Reports\\CRPSavingsByValue.rpt");
            SqlCommand cmd = new SqlCommand("SPSavingsByValue", ndConnHandle1);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@branch_id", nbranch);
            cmd.Parameters.AddWithValue("@ProductType", nProductType);
            cmd.Parameters.AddWithValue("@From", tcCode);
            cmd.Parameters.AddWithValue("@To", tcCode1);
            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@TRANDate", SqlDbType.Date)).Value = dateTimePicker2.Text;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            SPSavingsByValue ds = new SPSavingsByValue();
            sda.Fill(ds, "SPSavingsByValue");
            rprt.SetDataSource(ds);
            crystalReportViewer1.ReportSource = rprt;
            WindowState = FormWindowState.Maximized;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            comboBox1.Visible = false;
            comboBox2.Visible = false;
            comboBox3.Visible = false;
            //comboBox4.Visible = false;
            //  comboBox5.Visible = false;
            textBox1.Visible = false;
            textBox2.Visible = false;
            //textBox3.Visible = false;
            checkBox6.Visible = false;
            checkBox7.Visible = false;
            //checkBox3.Visible = false;
            //groupBox1.Visible = false;
            //dateTimePicker1.Visible = false;
            dateTimePicker2.Visible = false;
            label6.Visible = false;
            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            label20.Visible = false;
            button1.Visible = false;
            GETDATA();
            crystalReportViewer1.MaximumSize = MaximumSize;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
