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
    public partial class REPReceiptListing : Form
    {
        string cs = globalvar.cos;

        ReportDocument rprt = new ReportDocument();
        DataTable batview = new DataTable();
        DataTable prodview = new DataTable();
        public REPReceiptListing()
        {
            InitializeComponent();
        }

        private void REPReceiptListing_Load(object sender, EventArgs e)
        {
            UserCombo();
            BranchCombo();
          //  getBatchList();
            CurrencyCombo();
            getProduct();
        }

        void getProduct()
        {
            string dsqlb = "select prd_name,prd_id from prodtype order by prd_name  ";
            prodview.Clear();

            using (SqlConnection ndConnHandle2 = new SqlConnection(cs))
            {
                ndConnHandle2.Open();
                SqlDataAdapter dab = new SqlDataAdapter(dsqlb, ndConnHandle2);
                dab.Fill(prodview);
                if (prodview.Rows.Count > 0)
                {
                    comboBox6.DataSource = prodview.DefaultView;
                    comboBox6.DisplayMember = "prd_name";
                    comboBox6.ValueMember = "prd_id";
                    comboBox6.SelectedIndex = -1;
                    ndConnHandle2.Close();
                }
            }
        }//en

        
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

            comboBox3.ValueMember = "oprcode";
            comboBox3.DisplayMember = "oprcode";
            comboBox3.DataSource = dt;
            comboBox3.SelectedIndex = -1;
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

        private void button1_Click(object sender, EventArgs e)
        {
            comboBox1.Visible = false;
            comboBox2.Visible = false;
            comboBox3.Visible = false;
            comboBox4.Visible = false;
            comboBox6.Visible = false;
            label10.Visible = false;
            label1.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            label14.Visible = false;
            label20.Visible = false;
            textBox1.Visible = false;
            textBox2.Visible = false;
            button1.Visible = false;
            dateTimePicker1.Visible = false;
            dateTimePicker2.Visible = false;
            GETDATA();
            WindowState = FormWindowState.Maximized;
        }
        void GETDATA()
        {
            int nccurency = Convert.ToInt32(comboBox1.SelectedValue);
            int nbranch = Convert.ToInt32(comboBox2.SelectedValue);
            int Pproduct = Convert.ToInt32(comboBox6.SelectedValue);
          //  string nUserID = Convert.ToString(comboBox3.SelectedValue);

            SqlConnection ndConnHandle1 = new SqlConnection(cs);
            ndConnHandle1.Open();
            rprt.Load(System.Windows.Forms.Application.StartupPath + "\\Reports\\CRYReceiptReport.rpt");
            SqlCommand cmd = new SqlCommand("SPReceiptListing", ndConnHandle1);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ccurency", nccurency);
            cmd.Parameters.AddWithValue("@branch_id", nbranch);
            cmd.Parameters.AddWithValue("@Product", Pproduct);
            cmd.Parameters.AddWithValue("@UserID", comboBox3.Text);
            cmd.Parameters.AddWithValue("@From", textBox1.Text);
            cmd.Parameters.AddWithValue("@To", textBox2.Text);
            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@TranFromDate", SqlDbType.Date)).Value = dateTimePicker1.Text;
            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@TranToDate", SqlDbType.Date)).Value = dateTimePicker2.Text;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            SPReceiptDataset ds = new SPReceiptDataset();
            sda.Fill(ds, "SPReceiptListing");
            rprt.SetDataSource(ds);
            crystalReportViewer1.ReportSource = rprt;
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
//}
