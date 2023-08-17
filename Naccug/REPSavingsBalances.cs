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
    public partial class REPSavingsBalances : Form
    {
        string cs = globalvar.cos;

        ReportDocument rprt = new ReportDocument();
        public REPSavingsBalances()
        {
            InitializeComponent();
        }

        private void REPSavingsBalances_Load(object sender, EventArgs e)
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
            //if (reader != null)
            //{
                dt.Columns.Add("prd_id", typeof(string));
                dt.Columns.Add("prd_name", typeof(string));
                dt.Load(reader);

                comboBox3.ValueMember = "prd_id";
                comboBox3.DisplayMember = "prd_name";
                comboBox3.DataSource = dt;
                ndConnHandle1.Close();
           // }
        }
        public void GETDATA()
        {
            //session.clear();
            int nbranch = Convert.ToInt32(comboBox2.SelectedValue);
           // if (comboBox3.SelectedIndex > -1) {
            int nProductType = Convert.ToInt32(comboBox3.SelectedValue);
            SqlConnection ndConnHandle1 = new SqlConnection(cs);
            ndConnHandle1.Open();
            rprt.Load(System.Windows.Forms.Application.StartupPath + "\\Reports\\CRPSavingsBalances.rpt");
            SqlCommand cmd = new SqlCommand("SPSavingsBalances", ndConnHandle1);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@branch_id", nbranch);
            cmd.Parameters.AddWithValue("@ProductType", nProductType);
            cmd.Parameters.AddWithValue("@CustType0", textBox1.Text);
            cmd.Parameters.AddWithValue("@CustType1", textBox2.Text);
            cmd.Parameters.AddWithValue("@CustType2", textBox3.Text);
            cmd.Parameters.AddWithValue("@gender", textBox4.Text);
            cmd.Parameters.AddWithValue("@gender1", textBox5.Text);
             cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@TRANDate", SqlDbType.Date)).Value = dateTimePicker2.Text;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            SPSavingsBalances ds = new SPSavingsBalances();
            sda.Fill(ds, "SPSavingsBalances");
            rprt.SetDataSource(ds);
            crystalReportViewer1.ReportSource = rprt;
            WindowState = FormWindowState.Maximized;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            string che4 = "1";
            if (checkBox4.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox4.Text = che4;
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            string che5 = "2";
            if (checkBox5.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox5.Text = che5;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            int che = 1;
            if (checkBox1.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox1.Text = che.ToString();
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            int che1 = 2;
            if (checkBox2.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox2.Text = che1.ToString();
                textBox5.Enabled = false;
               // textBox6.Enabled = false;
                //checkBox4.Enabled = false;
                //checkBox5.Enabled = false;
            }
            else
            {

                textBox5.Enabled = true;
              //  textBox6.Enabled = true;
                //checkBox4.Enabled = true;
                //checkBox5.Enabled = true;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            int che2 = 3;
            if (checkBox3.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox3.Text = che2.ToString();
                textBox5.Enabled = false;
               // textBox6.Enabled = false;
                //checkBox4.Enabled = false;
                //checkBox5.Enabled = false;
            }
            else
            {

                textBox5.Enabled = true;
                //textBox6.Enabled = true;
                //checkBox4.Enabled = true;
                //checkBox5.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            comboBox1.Visible = false;
            comboBox2.Visible = false;
            comboBox3.Visible = false;
            groupBox1.Visible = false;
            groupBox2.Visible = false;
            label5.Visible = false;
            label7.Visible = false;
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
           // label4.Visible = false;
            label5.Visible = false;
            label20.Visible = false;
            button1.Visible = false;
            GETDATA();
            crystalReportViewer1.MaximumSize = MaximumSize;
        }

        private void checkBox4_CheckedChanged_1(object sender, EventArgs e)
        {
            int che = 1;
            if (checkBox1.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox4.Text = che.ToString();
            }
        }

        private void checkBox5_CheckedChanged_1(object sender, EventArgs e)
        {
            int che = 0;
            if (checkBox1.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox5.Text = che.ToString();
            }
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
