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
    public partial class REPTransactionEnquiry : Form
    {
        string cs = globalvar.cos;

        ReportDocument rprt = new ReportDocument();
        public REPTransactionEnquiry()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tabControl1.Visible = false;
            button1.Visible = false;
            GETDATA();
            WindowState = FormWindowState.Maximized;
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

            comboBox7.ValueMember = "prd_id";
            comboBox7.DisplayMember = "prd_name";
            comboBox7.DataSource = dt;
            comboBox7.SelectedIndex = -1;
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
        public void GETDATA()
        {
            int nccurency = Convert.ToInt32(comboBox1.SelectedValue);
            int nbranch = Convert.ToInt32(comboBox2.SelectedValue);
            int nProductType = Convert.ToInt32(comboBox7.SelectedValue);
          //  int nUserID = Convert.ToInt32(comboBox3.SelectedValue);

            SqlConnection ndConnHandle1 = new SqlConnection(cs);
            ndConnHandle1.Open();
            rprt.Load(System.Windows.Forms.Application.StartupPath + "\\Reports\\CRPTranshistReport.rpt");
            //  rprt.Load(@"C:\\Software Solutions\\NACCUG\\CUsystem\\naccug\\CRPTranshistReport.rpt");
            SqlCommand cmd = new SqlCommand("SPTranhist", ndConnHandle1);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ccurency", nccurency);
            cmd.Parameters.AddWithValue("@branch_id", nbranch);
            cmd.Parameters.AddWithValue("@ProductType", nProductType);
            cmd.Parameters.AddWithValue("@UserID", comboBox3.Text);
            cmd.Parameters.AddWithValue("@deposit", textBox11.Text);
            cmd.Parameters.AddWithValue("@withdrawal", textBox12.Text);
            cmd.Parameters.AddWithValue("@loanIssued", textBox13.Text);
            cmd.Parameters.AddWithValue("@LoanRepayment", textBox14.Text);
            cmd.Parameters.AddWithValue("@InterestCharged", textBox15.Text);
            cmd.Parameters.AddWithValue("@InterestPaid", textBox16.Text);
            cmd.Parameters.AddWithValue("@SavingsInterest", textBox17.Text);
            cmd.Parameters.AddWithValue("@FeeCharged  ", textBox18.Text);
            cmd.Parameters.AddWithValue("@FeePaid", textBox19.Text);
            cmd.Parameters.AddWithValue("@Ajustment  ", textBox20.Text);
            cmd.Parameters.AddWithValue("@Reversal  ", textBox21.Text);
            cmd.Parameters.AddWithValue("@Transfer", textBox22.Text);
            cmd.Parameters.AddWithValue("@ChargeOff  ", textBox23.Text);
            cmd.Parameters.AddWithValue("@WriteOff  ", textBox24.Text);
            cmd.Parameters.AddWithValue("@StandingOrder", textBox25.Text);
            cmd.Parameters.AddWithValue("@DepoditInterest  ", textBox26.Text);
            cmd.Parameters.AddWithValue("@MobileMoneyTran  ", textBox27.Text);
            cmd.Parameters.AddWithValue("@ATM", textBox28.Text);
            cmd.Parameters.AddWithValue("@dividend", textBox29.Text);
            cmd.Parameters.AddWithValue("@InternetBanking", textBox30.Text);
            //   cmd.Parameters.AddWithValue("@custinduced", textBox36.Text);
            cmd.Parameters.AddWithValue("@BadDebitTransfer", textBox33.Text);
            cmd.Parameters.AddWithValue("@BadDebitRecovered", textBox34.Text);
            cmd.Parameters.AddWithValue("@BatchInterestPaid", textBox32.Text);
            cmd.Parameters.AddWithValue("@BatchLoanRepayment", textBox31.Text);
            cmd.Parameters.AddWithValue("@AnnualFee", textBox35.Text);
            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@TranFromDate", SqlDbType.Date)).Value = dateTimePicker1.Text;
            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@TranToDate", SqlDbType.Date)).Value = dateTimePicker2.Text;
            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PostFromDate", SqlDbType.Date)).Value = dateTimePicker8.Text;
            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PostToDate", SqlDbType.Date)).Value = dateTimePicker7.Text;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            SPTranshistDATASet ds = new SPTranshistDATASet();
            sda.Fill(ds, "SPTranhist");
            rprt.SetDataSource(ds);
            crystalReportViewer1.ReportSource = rprt;
        }

        private void REPTransactionEnquiry_Load(object sender, EventArgs e)
        {
            UserCombo();
            BranchCombo();
            ProductCombo();
            CurrencyCombo();
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
       
        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void checkBox7_CheckedChanged_1(object sender, EventArgs e)
        {
            string che = "21";
            if (checkBox7.Checked)
            {
                textBox20.Text = che;
            }
            else
            {
                textBox20.Text = "";

            };
        }
        private void checkBox14_CheckedChanged_1(object sender, EventArgs e)
        {
            string che = "07";
            if (checkBox14.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox14.Text = che;
            }
            else
            {
                textBox14.Text = "";

            };
        }
        private void checkBox30_CheckedChanged_1(object sender, EventArgs e)
        {
            string che = "24";
            if (checkBox30.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox26.Text = che;
            }
            else
            {
                textBox26.Text = "";

            };
        }
        private void checkBox12_CheckedChanged_1(object sender, EventArgs e)
        {
            string che = "01";
            if (checkBox12.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox11.Text = che;
            }
            else
            {
                textBox11.Text = "";

            };
        }

        private void checkBox13_CheckedChanged_1(object sender, EventArgs e)
        {
            string che = "02";
            if (checkBox13.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox12.Text = che;
            }
            else
            {
                textBox12.Text = "";

            };
        }

        private void checkBox11_CheckedChanged_1(object sender, EventArgs e)
        {
            string che = "03";
            if (checkBox11.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox25.Text = che;
            }
            else
            {
                textBox25.Text = "";

            };
        }

        private void checkBox15_CheckedChanged_1(object sender, EventArgs e)
        {
            string che = "06";
            if (checkBox15.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox13.Text = che;
            }
            else
            {
                textBox13.Text = "";

            };
        }

        private void checkBox16_CheckedChanged_1(object sender, EventArgs e)
        {
            string che = "17";
            if (checkBox16.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox15.Text = che;
            }
            else
            {
                textBox15.Text = "";

            };
        }

        private void checkBox17_CheckedChanged_1(object sender, EventArgs e)
        {
            string che = "05";
            if (checkBox17.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox16.Text = che;
            }
            else
            {
                textBox16.Text = "";

            };
        }

        private void checkBox28_CheckedChanged_1(object sender, EventArgs e)
        {
            string che = "04";
            if (checkBox28.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox17.Text = che;
            }
            else
            {
                textBox17.Text = "";

            };
        }

        private void checkBox18_CheckedChanged_1(object sender, EventArgs e)
        {
            string che = "19";
            if (checkBox18.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox18.Text = che;
            }
            else
            {
                textBox18.Text = "";

            };
        }

        private void checkBox19_CheckedChanged_1(object sender, EventArgs e)
        {
            string che = "18";
            if (checkBox19.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox19.Text = che;
            }
            else
            {
                textBox19.Text = "";

            };
        }

        private void checkBox10_CheckedChanged_(object sender, EventArgs e)
        {
            string che = "16";
            if (checkBox10.Checked)
            {
                textBox21.Text = che;
            }
            else
            {
                textBox21.Text = "";

            };
        }

        private void checkBox8_CheckedChanged_1(object sender, EventArgs e)
        {
            string che = "22";
            if (checkBox8.Checked)
            {
                textBox22.Text = che;
            }
            else
            {
                textBox22.Text = "";

            };
        }

        private void checkBox29_CheckedChanged_1(object sender, EventArgs e)
        {
            string che = "23";
            if (checkBox29.Checked)
            {
                textBox23.Text = che;
            }
            else
            {
                textBox23.Text = "";

            };
        }

        private void checkBox31_CheckedChanged_1(object sender, EventArgs e)
        {
            string che = "25";
            if (checkBox31.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox27.Text = che;
            }
            else
            {
                textBox27.Text = "";

            };
        }

        private void checkBox32_CheckedChanged_1(object sender, EventArgs e)
        {
            string che = "26";
            if (checkBox32.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox28.Text = che;
            }
            else
            {
                textBox28.Text = "";

            };
        }

        private void checkBox9_CheckedChanged_1(object sender, EventArgs e)
        {
            string che = "20";
            if (checkBox9.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox29.Text = che;
            }
            else
            {
                textBox29.Text = "";

            };
        }

        private void checkBox33_CheckedChanged_1(object sender, EventArgs e)
        {
            string che = "27";
            if (checkBox33.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox30.Text = che;
            }
            else
            {
                textBox30.Text = "";

            };
        }

        private void checkBox34_CheckedChanged_1(object sender, EventArgs e)
        {
            string che = "08";
            if (checkBox34.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox31.Text = che;
            }
            else
            {
                textBox31.Text = "";

            };
        }

        private void checkBox35_CheckedChanged_1(object sender, EventArgs e)
        {
            string che = "13";
            if (checkBox35.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox32.Text = che;
            }
            else
            {
                textBox32.Text = "";

            };
        }

        private void checkBox36_CheckedChanged_1(object sender, EventArgs e)
        {
            string che = "11";
            if (checkBox36.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox33.Text = che;
            }
            else
            {
                textBox33.Text = "";

            };
        }

        private void checkBox37_CheckedChanged_1(object sender, EventArgs e)
        {
            string che = "10";
            if (checkBox37.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox34.Text = che;
            }
            else
            {
                textBox34.Text = "";

            };
        }

        private void checkBox38_CheckedChanged_1(object sender, EventArgs e)
        {
            string che = "14";
            if (checkBox38.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox35.Text = che;
            }
            else
            {
                textBox35.Text = "";

            };
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {

            if (checkBox1.Checked)
            {
                textBox36.Text = true.ToString();
            }
            else
            {
                textBox36.Text = false.ToString();

            };
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            string che = "21";
            if (checkBox7.Checked)
            {
                textBox20.Text = che;
            }
            else
            {
                textBox20.Text = "";

            };
        }

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            string che = "07";
            if (checkBox14.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox14.Text = che;
            }
            else
            {
                textBox14.Text = "";

            };
        }

        private void checkBox30_CheckedChanged(object sender, EventArgs e)
        {
            string che = "24";
            if (checkBox30.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox26.Text = che;
            }
            else
            {
                textBox26.Text = "";

            };
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            string che = "02";
            if (checkBox13.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox12.Text = che;
            }
            else
            {
                textBox12.Text = "";

            };
        }

        private void checkBox17_CheckedChanged(object sender, EventArgs e)
        {
            string che = "05";
            if (checkBox17.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox16.Text = che;
            }
            else
            {
                textBox16.Text = "";

            };
        }

        private void checkBox34_CheckedChanged(object sender, EventArgs e)
        {
            string che = "08";
            if (checkBox34.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox31.Text = che;
            }
            else
            {
                textBox31.Text = "";

            };
        }

        private void checkBox28_CheckedChanged(object sender, EventArgs e)
        {
            string che = "04";
            if (checkBox28.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox17.Text = che;
            }
            else
            {
                textBox17.Text = "";

            };
        }

        private void checkBox18_CheckedChanged(object sender, EventArgs e)
        {

            string che = "19";
            if (checkBox18.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox18.Text = che;
            }
            else
            {
                textBox18.Text = "";

            };
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            string che = "03";
            if (checkBox11.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox25.Text = che;
            }
            else
            {
                textBox25.Text = "";

            };
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            string che = "22";
            if (checkBox8.Checked)
            {
                textBox22.Text = che;
            }
            else
            {
                textBox22.Text = "";

            };
        }

        private void checkBox33_CheckedChanged(object sender, EventArgs e)
        {
            string che = "27";
            if (checkBox33.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox30.Text = che;
            }
            else
            {
                textBox30.Text = "";

            };
        }

        private void checkBox38_CheckedChanged(object sender, EventArgs e)
        {
            string che = "14";
            if (checkBox38.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox35.Text = che;
            }
            else
            {
                textBox35.Text = "";

            };
        }

        private void checkBox31_CheckedChanged(object sender, EventArgs e)
        {
            string che = "25";
            if (checkBox31.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox27.Text = che;
            }
            else
            {
                textBox27.Text = "";

            };
        }

        private void checkBox29_CheckedChanged(object sender, EventArgs e)
        {
            string che = "23";
            if (checkBox29.Checked)
            {
                textBox23.Text = che;
            }
            else
            {
                textBox23.Text = "";

            };
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            string che = "16";
            if (checkBox10.Checked)
            {
                textBox21.Text = che;
            }
            else
            {
                textBox21.Text = "";

            };
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            string che = "20";
            if (checkBox9.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox29.Text = che;
            }
            else
            {
                textBox29.Text = "";

            };
        }

        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {
            string che = "06";
            if (checkBox15.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox13.Text = che;
            }
            else
            {
                textBox13.Text = "";

            };
        }

        private void checkBox16_CheckedChanged(object sender, EventArgs e)
        {
            string che = "17";
            if (checkBox16.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox15.Text = che;
            }
            else
            {
                textBox15.Text = "";

            };
        }

        private void checkBox19_CheckedChanged(object sender, EventArgs e)
        {
            string che = "15";
            if (checkBox19.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox19.Text = che;
            }
            else
            {
                textBox19.Text = "";

            };
        }

        private void checkBox36_CheckedChanged(object sender, EventArgs e)
        {
            string che = "11";
            if (checkBox36.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox33.Text = che;
            }
            else
            {
                textBox33.Text = "";

            };
        }

        private void checkBox37_CheckedChanged(object sender, EventArgs e)
        {
            string che = "10";
            if (checkBox37.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox34.Text = che;
            }
            else
            {
                textBox34.Text = "";

            };
        }

        private void checkBox32_CheckedChanged(object sender, EventArgs e)
        {
            string che = "26";
            if (checkBox32.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox28.Text = che;
            }
            else
            {
                textBox28.Text = "";

            };
        }

        private void checkBox35_CheckedChanged(object sender, EventArgs e)
        {
            string che = "13";
            if (checkBox35.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox32.Text = che;
            }
            else
            {
                textBox32.Text = "";

            };
        }

        private void checkBox27_CheckedChanged(object sender, EventArgs e)
        {

        }
    }


}
