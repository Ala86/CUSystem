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
    public partial class REPTransactionSummary : Form
    {
        string cs = globalvar.cos;

        ReportDocument rprt = new ReportDocument();
        DataTable batview = new DataTable();
        public REPTransactionSummary()
        {
            InitializeComponent();
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void REPTransactionSummary_Load(object sender, EventArgs e)
        {
            UserCombo();
            BranchCombo();
         //   getBatchList();
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

            comboBox7.ValueMember = "branchid";
            comboBox7.DisplayMember = "br_name";
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

            comboBox8.ValueMember = "oprcode";
            comboBox8.DisplayMember = "oprcode";
            comboBox8.DataSource = dt;
            comboBox8.SelectedIndex = -1;
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

            comboBox6.ValueMember = "curr_code";
            comboBox6.DisplayMember = "curr_name";
            comboBox6.DataSource = dt;
            comboBox6.SelectedIndex = -1;
            ndConnHandle1.Close();
        }
        public void GETDATA()
        {
            int nccurency = Convert.ToInt32(comboBox6.SelectedValue);
            int nbranch = Convert.ToInt32(comboBox7.SelectedValue);
            SqlConnection ndConnHandle1 = new SqlConnection(cs);
            ndConnHandle1.Open();
            rprt.Load(System.Windows.Forms.Application.StartupPath + "\\Reports\\CRPTransactionSummary.rpt");
            SqlCommand cmd = new SqlCommand("SPTranhistSummary", ndConnHandle1);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ccurency", nccurency);
            cmd.Parameters.AddWithValue("@branch_id", nbranch);
            cmd.Parameters.AddWithValue("@UserID", comboBox8.SelectedValue.ToString());
            cmd.Parameters.AddWithValue("@deposit", textBox1.Text);
            cmd.Parameters.AddWithValue("@withdrawal", textBox4.Text);
            cmd.Parameters.AddWithValue("@loanIssued", textBox5.Text);
            cmd.Parameters.AddWithValue("@LoanRepayment", textBox6.Text);
            cmd.Parameters.AddWithValue("@InterestCharged", textBox7.Text);
            cmd.Parameters.AddWithValue("@InterestPaid", textBox8.Text);
            cmd.Parameters.AddWithValue("@SavingsInterest", textBox9.Text);
            cmd.Parameters.AddWithValue("@FeeCharged  ", textBox10.Text);
            cmd.Parameters.AddWithValue("@FeePaid", textBox11.Text);
            cmd.Parameters.AddWithValue("@Ajustment  ", textBox12.Text);
            cmd.Parameters.AddWithValue("@Reversal  ", textBox20.Text);
            cmd.Parameters.AddWithValue("@Transfer", textBox21.Text);
            cmd.Parameters.AddWithValue("@ChargeOff  ", textBox22.Text);
            cmd.Parameters.AddWithValue("@WriteOff  ", textBox23.Text);
            cmd.Parameters.AddWithValue("@StandingOrder", textBox24.Text);
            cmd.Parameters.AddWithValue("@DepoditInterest  ", textBox25.Text);
            cmd.Parameters.AddWithValue("@MobileMoneyTran  ", textBox26.Text);
            cmd.Parameters.AddWithValue("@ATM", textBox27.Text);
            cmd.Parameters.AddWithValue("@dividend", textBox28.Text);
            cmd.Parameters.AddWithValue("@InternetBanking", textBox29.Text);
            //   cmd.Parameters.AddWithValue("@custinduced", textBox36.Text);
            cmd.Parameters.AddWithValue("@BadDebitTransfer", textBox30.Text);
            cmd.Parameters.AddWithValue("@BadDebitRecovered", textBox31.Text);
            cmd.Parameters.AddWithValue("@BatchInterestPaid", textBox32.Text);
            cmd.Parameters.AddWithValue("@BatchLoanRepayment", textBox33.Text);
            cmd.Parameters.AddWithValue("@AnnualFee", textBox34.Text);
            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@TranFromDate", SqlDbType.Date)).Value = dateTimePicker5.Text;
            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@TranToDate", SqlDbType.Date)).Value = dateTimePicker6.Text;
            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PostFromDate", SqlDbType.Date)).Value = dateTimePicker7.Text;
            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PostToDate", SqlDbType.Date)).Value = dateTimePicker8.Text;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            SPTransactionSummary ds = new SPTransactionSummary();
            sda.Fill(ds, "SPTranhistSummary");
            rprt.SetDataSource(ds);
            crystalReportViewer1.ReportSource = rprt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
            groupBox4.Visible = false;
            comboBox6.Visible = false;
            comboBox7.Visible = false;
            comboBox8.Visible = false;
            comboBox9.Visible = false;
            comboBox5.Visible = false;
            label10.Visible = false;
            label11.Visible = false;
            label12.Visible = false;
            label13.Visible = false;
            label15.Visible = false;
            label16.Visible = false;
            label17.Visible = false;
            label18.Visible = false;
            label19.Visible = false;
          //  label20.Visible = false;
           // label2.Visible = false;
            textBox1.Visible = false;
           // textBox2.Visible = false;
            button1.Visible = false;
            dateTimePicker5.Visible = false;
            dateTimePicker6.Visible = false;
            dateTimePicker7.Visible = false;
            dateTimePicker8.Visible = false;

            GETDATA();
            WindowState = FormWindowState.Maximized;
        }

        private void checkBox52_CheckedChanged(object sender, EventArgs e)
        {
            string che = "01";
            if (checkBox52.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox1.Text = che;
            }
            else
            {
                textBox1.Text = "";

            };
        }

        private void checkBox51_CheckedChanged(object sender, EventArgs e)
        {
            string che = "02";
            if (checkBox51.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox4.Text = che;
            }
            else
            {
                textBox4.Text = "";

            };
        }

        private void checkBox50_CheckedChanged(object sender, EventArgs e)
        {
            string che = "07";
            if (checkBox50.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox6.Text = che;
            }
            else
            {
                textBox6.Text = "";

            };
        }

        private void checkBox49_CheckedChanged(object sender, EventArgs e)
        {
            string che = "06";
            if (checkBox49.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox5.Text = che;
            }
            else
            {
                textBox5.Text = "";

            };
        }

        private void checkBox48_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void checkBox47_CheckedChanged(object sender, EventArgs e)
        {
            string che = "05";
            if (checkBox47.Checked)
            {
                textBox8.Text = che;
            }
            else
            {
                textBox8.Text = "";

            };
        }

        private void checkBox25_CheckedChanged(object sender, EventArgs e)
        {
            string che = "03";
            if (checkBox25.Checked)
            {
                textBox24.Text = che;
            }
            else
            {
                textBox24.Text = "";

            };
        }

        private void checkBox42_CheckedChanged(object sender, EventArgs e)
        {
            string che = "04";
            if (checkBox42.Checked)
            {
                textBox9.Text = che;
            }
            else
            {
                textBox9.Text = "";

            };
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            string che = "08";
            if (checkBox3.Checked)
            {
                textBox33.Text = che;
            }
            else
            {
                textBox33.Text = "";

            };
        }

        private void checkBox43_CheckedChanged(object sender, EventArgs e)
        {
            string che = "10";
            if (checkBox43.Checked)
            {
                textBox31.Text = che;
            }
            else
            {
                textBox31.Text = "";

            };
        }

        private void checkBox45_CheckedChanged(object sender, EventArgs e)
        {
            string che = "11";
            if (checkBox45.Checked)
            {
                textBox30.Text = che;
            }
            else
            {
                textBox30.Text = "";

            };
        }

        private void checkBox23_CheckedChanged(object sender, EventArgs e)
        {
            string che = "12";
            if (checkBox23.Checked)
            {
                textBox21.Text = che;
            }
            else
            {
                textBox21.Text = "";

            };
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            string che = "13";
            if (checkBox2.Checked)
            {
                textBox32.Text = che;
            }
            else
            {
                textBox32.Text = "";

            };
        }

        private void checkBox44_CheckedChanged(object sender, EventArgs e)
        {
            string che = "14";
            if (checkBox44.Checked)
            {
                textBox34.Text = che;
            }
            else
            {
                textBox34.Text = "";

            };
        }

        private void checkBox46_CheckedChanged(object sender, EventArgs e)
        {
            string che = "18";
            if (checkBox46.Checked)
            {
                textBox11.Text = che;
            }
            else
            {
                textBox11.Text = "";

            };
        }

        private void checkBox26_CheckedChanged(object sender, EventArgs e)
        {
            string che = "16";
            if (checkBox26.Checked)
            {
                textBox20.Text = che;
            }
            else
            {
                textBox20.Text = "";

            };
        }

        private void checkBox27_CheckedChanged(object sender, EventArgs e)
        {
            string che = "20";
            if (checkBox27.Checked)
            {
                textBox28.Text = che;
            }
            else
            {
                textBox28.Text = "";

            };
        }

        private void checkBox40_CheckedChanged(object sender, EventArgs e)
        {
            string che = "21";
            if (checkBox40.Checked)
            {
                textBox12.Text = che;
            }
            else
            {
                textBox12.Text = "";

            };
        }

        private void checkBox39_CheckedChanged(object sender, EventArgs e)
        {
            string che = "22";
            if (checkBox39.Checked)
            {
                textBox22.Text = che;
            }
            else
            {
                textBox22.Text = "";

            };
        }

        private void checkBox24_CheckedChanged(object sender, EventArgs e)
        {
            string che = "23";
            if (checkBox24.Checked)
            {
                textBox23.Text = che;
            }
            else
            {
                textBox23.Text = "";

            };
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
