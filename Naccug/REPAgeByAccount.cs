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
    public partial class REPAgeByAccount : Form
    {
        string cs = globalvar.cos;
        int amale = 2;
        int afemale = 2;
        DataTable prodview = new DataTable();

        ReportDocument rprt = new ReportDocument();
        public REPAgeByAccount()
        {
            InitializeComponent();
        }

        private void CrystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
        public void GETDATA()
        {
            int nbranch = Convert.ToInt32(comboBox2.SelectedValue);
            int Pproduct = Convert.ToInt32(comboBox6.SelectedValue);
            SqlConnection ndConnHandle1 = new SqlConnection(cs);
            rprt.Load(System.Windows.Forms.Application.StartupPath + "\\Reports\\CRPAgeByAccount.rpt");
            SqlCommand cmd = new SqlCommand("SPAgeByAccount", ndConnHandle1);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@branch_id", nbranch);
            cmd.Parameters.AddWithValue("@product", Pproduct);
            cmd.Parameters.AddWithValue("@From", TextBox1.Text);
            cmd.Parameters.AddWithValue("@To", TextBox2.Text);
            cmd.Parameters.AddWithValue("@activeMember", textBox5.Text);
            cmd.Parameters.AddWithValue("@CloseMember", textBox6.Text);
            cmd.Parameters.AddWithValue("@gender", amale);
            cmd.Parameters.AddWithValue("@gender1", afemale);
            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@OpenFromDate", SqlDbType.Date)).Value = dateTimePicker1.Text;
            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@OpenToDate", SqlDbType.Date)).Value = dateTimePicker2.Text;
            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ClosedFromDate", SqlDbType.Date)).Value = dateTimePicker4.Text;
            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ClosedToDate", SqlDbType.Date)).Value = dateTimePicker3.Text;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            SPAgeByAccount ds = new SPAgeByAccount();
            sda.Fill(ds, "SPAgeByAccount");
            rprt.SetDataSource(ds);
            CrystalReportViewer1.ReportSource = rprt;
        }
       void BranchCombo()
        {
          //  int nbranchid = Convert.ToInt32(comboBox2.SelectedValue);
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
        private void Label3_Click(object sender, EventArgs e)
        {

        }

        private void Label2_Click(object sender, EventArgs e)
        {

        }

        private void Label1_Click(object sender, EventArgs e)
        {

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
        private void Button1_Click(object sender, EventArgs e)
        {

 
            textBox3.Visible = false;
            textBox4.Visible = false;
            checkBox4.Visible = false;
            checkBox5.Visible = false;
            checkBox6.Visible = false;
            checkBox7.Visible = false;
            groupBox2.Visible = false;
            comboBox6.Visible = false;
            label13.Visible = false;
            dateTimePicker1.Visible = false;
            dateTimePicker2.Visible = false;
            dateTimePicker3.Visible = false;
            dateTimePicker4.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            label8.Visible = false;
            label9.Visible = false;
            Label1.Visible = false;
            Button1.Visible = false;
            groupBox2.Visible = false;
            Label2.Visible = false;
            Label3.Visible = false;
            Label4.Visible = false;
            TextBox1.Visible = false;
            TextBox2.Visible = false;
            comboBox2.Visible = false;
            GETDATA();
          //  crystalReportViewer1.MaximumSize = MaximumSize;
            WindowState = FormWindowState.Maximized;
        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Label4_Click(object sender, EventArgs e)
        {

        }

        private void REPAgeByAccount_Load(object sender, EventArgs e)
        {
           
            BranchCombo();
            getProduct();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            int che = 1;
            if (checkBox4.Checked)
            {
                amale = che;
                // textBox4.Text = che.ToString().Trim();
                //  MessageBox.Show("saving is done");
                // textBox9.Text = che.ToString();
            }
            else { }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            int che = 0;
            if (checkBox5.Checked)
            {
                afemale = che;
                // textBox7.Text = che.ToString().Trim();
                //  MessageBox.Show("saving is done");
                //  textBox11.Text = che.ToString();
            }

            else { }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            int che3 = 1;
            if (checkBox6.Checked)
            {
                textBox5.Text = che3.ToString();
                dateTimePicker3.Enabled = false;
                dateTimePicker4.Enabled = false;

            }
                   }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            int che4 = 0;
            if (checkBox7.Checked)
            {
                textBox6.Text = che4.ToString();
                dateTimePicker1.Enabled = false;
                dateTimePicker2.Enabled = false;
            }
            else
            {

                dateTimePicker1.Enabled = true;
                dateTimePicker2.Enabled = true;
            }
        }

       
    }
}
