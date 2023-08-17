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
    public partial class REPDetailedAging : Form
    {
        string cs = globalvar.cos;

        ReportDocument rprt = new ReportDocument();
        DataTable prodview = new DataTable();
        public REPDetailedAging()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            CMBTRANSACTION_TYPE.Visible = false;
            ComboBox2.Visible = false;
            TextBox2.Visible = false;
           comboBox1.Visible = false;
            dateTimePicker1.Visible = false;
            label13.Visible = false;
            label12.Visible = false;
            TextBox1.Visible = false;
            textBox10.Visible = false;
            TextBox3.Visible = false;
            TextBox4.Visible = false;
            TextBox5.Visible = false;
            TextBox6.Visible = false;
            TextBox7.Visible = false;
            TextBox8.Visible = false;
            textBox9.Visible = false;
            label21.Visible = false;
            Label1.Visible = false;
            Label2.Visible = false;
            Label3.Visible = false;
            Label4.Visible = false;
            Label5.Visible = false;
            Label6.Visible = false;
            Label7.Visible = false;
            Label8.Visible = false;
            Label9.Visible = false;
            Label10.Visible = false;
            label12.Visible = false;
            label11.Visible = false;
          
            Button1.Visible = false;
            GETDATA();
            crystalReportViewer1.MaximumSize = MaximumSize;
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

            ComboBox2.ValueMember = "branchid";
            ComboBox2.DisplayMember = "br_name";
            ComboBox2.DataSource = dt;
            ComboBox2.SelectedIndex = -1;
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

            CMBTRANSACTION_TYPE.ValueMember = "curr_code";
            CMBTRANSACTION_TYPE.DisplayMember = "curr_name";
            CMBTRANSACTION_TYPE.DataSource = dt;
            CMBTRANSACTION_TYPE.SelectedIndex = -1;
            ndConnHandle1.Close();
        }

        void getProduct()
        {
            string dsqlb = "select prd_name,prd_id from prodtype  where maincat = '130' order by prd_name  ";
            prodview.Clear();

            using (SqlConnection ndConnHandle2 = new SqlConnection(cs))
            {
                ndConnHandle2.Open();
                SqlDataAdapter dab = new SqlDataAdapter(dsqlb, ndConnHandle2);
                dab.Fill(prodview);
                if (prodview.Rows.Count > 0)
                {
                    comboBox1.DataSource = prodview.DefaultView;
                    comboBox1.DisplayMember = "prd_name";
                    comboBox1.ValueMember = "prd_id";
                    comboBox1.SelectedIndex = -1;
                    ndConnHandle2.Close();
                }
            }
        }//en
        private void REPDetailedAging_Load(object sender, EventArgs e)
        {
            BranchCombo();
           // ProductCombo();
            CurrencyCombo();
            getProduct();
        }
        public void GETDATA()
        {
            int Pproduct = Convert.ToInt32(comboBox1.SelectedValue);
            SqlConnection ndConnHandle1 = new SqlConnection(cs);
            ndConnHandle1.Open();
            rprt.Load(System.Windows.Forms.Application.StartupPath + "\\Reports\\CRPDetailedAgingReport.rpt");
            SqlCommand cmd = new SqlCommand("[SPLoanAgingReport]", ndConnHandle1);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@agingcurrentfrom", textBox10.Text.Trim());
            cmd.Parameters.AddWithValue("@agingcurrentTo", textBox9.Text.Trim());
            cmd.Parameters.AddWithValue("@aging31to90from", TextBox1.Text.Trim());
            cmd.Parameters.AddWithValue("@aging31to90to", TextBox2.Text.Trim());
            cmd.Parameters.AddWithValue("@aging91to180from", TextBox4.Text.Trim());
            cmd.Parameters.AddWithValue("@aging91to180to", TextBox3.Text.Trim());
            cmd.Parameters.AddWithValue("@aging181to365from", TextBox6.Text.Trim());
            cmd.Parameters.AddWithValue("@aging181to365to", TextBox5.Text.Trim());
            cmd.Parameters.AddWithValue("@aging366toAbovefrom", TextBox8.Text.Trim());
            cmd.Parameters.AddWithValue("@aging366toAboveto", TextBox7.Text.Trim());
           // cmd.Parameters.AddWithValue("@ToDate", dateTimePicker1.Text);
            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ToDate", SqlDbType.Date)).Value = dateTimePicker1.Text;
            cmd.Parameters.AddWithValue("@Product", Pproduct);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            SPDetailedAgingRepData ds = new SPDetailedAgingRepData();
            sda.Fill(ds, "SPLoanAgingReport");
            rprt.SetDataSource(ds);
            crystalReportViewer1.ReportSource = rprt;
            WindowState = FormWindowState.Maximized;
        }

        private void crystalReportViewer1_ReportRefresh(object source, CrystalDecisions.Windows.Forms.ViewerEventArgs e)
        {
            this.Hide();
            REPDetailedAging ala = new REPDetailedAging();
            ala.ShowDialog();
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
