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
    public partial class REPTransactionListing : Form
    {
        string cs = globalvar.cos;

        ReportDocument rprt = new ReportDocument();
        DataTable batview = new DataTable();
        DataTable prodview = new DataTable();
        public REPTransactionListing()
        {
            InitializeComponent();
        }

        private void REPTransactionListing_Load(object sender, EventArgs e)
        {
            UserCombo();
            BranchCombo();
            getBatchList();
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

        void getBatchList()
        {
            string dsqlb = "select bat_name,bat_id from batch_main order by bat_name  ";
            batview.Clear();

            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter dab = new SqlDataAdapter(dsqlb, ndConnHandle);
                dab.Fill(batview);
                if (batview.Rows.Count > 0)
                {
                    comboBox5.DataSource = batview.DefaultView;
                    comboBox5.DisplayMember = "bat_name";
                    comboBox5.ValueMember = "bat_id";
                    comboBox5.SelectedIndex = -1;
                    ndConnHandle.Close();
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
        public void GETDATA()
        {
            int nccurency = Convert.ToInt32(comboBox1.SelectedValue);
            int nbranch = Convert.ToInt32(comboBox2.SelectedValue);
            int batchid = Convert.ToInt32(comboBox5.SelectedValue);
            int Pproduct = Convert.ToInt32(comboBox6.SelectedValue);
            // int nProductType = Convert.ToInt32(comboBox7.SelectedValue);
            // string nUserID = Convert.ToInt32(comboBox3.SelectedValue);
           // MessageBox.Show("This is the product ID" + Pproduct);

            SqlConnection ndConnHandle1 = new SqlConnection(cs);
            ndConnHandle1.Open();
            rprt.Load(System.Windows.Forms.Application.StartupPath + "\\Reports\\CRPTransactionListing.rpt");
           // rprt.Load(@"C:\\Software Solutions\\NACCUG\\CUsystem\\naccug\\CRPTransactionListing.rpt");
            SqlCommand cmd = new SqlCommand("SPTranhistListing", ndConnHandle1);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ccurency", nccurency);
            cmd.Parameters.AddWithValue("@branch_id", nbranch);
            cmd.Parameters.AddWithValue("@batchid", batchid);
            cmd.Parameters.AddWithValue("@Product", Pproduct);
            cmd.Parameters.AddWithValue("@UserID", comboBox3.Text);
            cmd.Parameters.AddWithValue("@deposit", textBox3.Text);
            cmd.Parameters.AddWithValue("@withdrawal", textBox4.Text);
            cmd.Parameters.AddWithValue("@loanIssued", textBox5.Text);
            cmd.Parameters.AddWithValue("@LoanRepayment", textBox6.Text);
            cmd.Parameters.AddWithValue("@InterestCharged", textBox7.Text);
            cmd.Parameters.AddWithValue("@InterestPaid", textBox8.Text);
            cmd.Parameters.AddWithValue("@SavingsInterest", textBox9.Text);
            cmd.Parameters.AddWithValue("@FeeCharged  ", textBox10.Text);
            cmd.Parameters.AddWithValue("@FeePaid", textBox11.Text);
            cmd.Parameters.AddWithValue("@Ajustment  ", textBox12.Text);
            cmd.Parameters.AddWithValue("@Reversal  ", textBox13.Text);
            cmd.Parameters.AddWithValue("@Transfer", textBox14.Text);
            cmd.Parameters.AddWithValue("@ChargeOff  ", textBox15.Text);
            cmd.Parameters.AddWithValue("@WriteOff  ", textBox16.Text);
            cmd.Parameters.AddWithValue("@StandingOrder", textBox17.Text);
            cmd.Parameters.AddWithValue("@DepoditInterest  ", textBox18.Text);
            cmd.Parameters.AddWithValue("@MobileMoneyTran  ", textBox19.Text);
            cmd.Parameters.AddWithValue("@ATM", textBox20.Text);
            cmd.Parameters.AddWithValue("@dividend", textBox21.Text);
            cmd.Parameters.AddWithValue("@InternetBanking", textBox22.Text);
            cmd.Parameters.AddWithValue("@BadDebitTransfer", textBox23.Text);
            cmd.Parameters.AddWithValue("@BadDebitRecovered", textBox24.Text);
            cmd.Parameters.AddWithValue("@BatchInterestPaid", textBox25.Text);
            cmd.Parameters.AddWithValue("@BatchLoanRepayment", textBox26.Text);
            cmd.Parameters.AddWithValue("@AnnualFee", textBox27.Text);
            cmd.Parameters.AddWithValue("@AnnualShares", textBox28.Text);
            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@TranFromDate", SqlDbType.Date)).Value = dateTimePicker1.Text;
            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@TranToDate", SqlDbType.Date)).Value = dateTimePicker2.Text;
            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PostFromDate", SqlDbType.Date)).Value = dateTimePicker4.Text;
            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PostToDate", SqlDbType.Date)).Value = dateTimePicker3.Text;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            SPTranhistListing ds = new SPTranhistListing();
            sda.Fill(ds, "SPTranhistListing");
            rprt.SetDataSource(ds);
            crystalReportViewer1.ReportSource = rprt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            groupBox2.Visible = false;
            groupBox3.Visible = false;
            comboBox1.Visible = false;
            comboBox2.Visible = false;
            comboBox3.Visible = false;
            comboBox4.Visible = false;
            comboBox5.Visible = false;
            comboBox6.Visible = false;
            label10.Visible = false;
            label9.Visible = false;
            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            label8.Visible = false;
            label14.Visible = false;
            label20.Visible = false;
            textBox1.Visible = false;
            textBox2.Visible = false;
            button1.Visible = false;
            dateTimePicker1.Visible = false;
            dateTimePicker2.Visible = false;
            dateTimePicker3.Visible = false;
            dateTimePicker4.Visible = false;

            GETDATA();
            WindowState = FormWindowState.Maximized;
        }
        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            string che = "01";
            if (checkBox12.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox3.Text = che;
            }
            else
            {
                textBox3.Text = "";

            };
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            string che = "02";
            if (checkBox13.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox4.Text = che;
            }
            else
            {
                textBox4.Text = "";

            };
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            string che = "03";
            if (checkBox11.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox17.Text = che;
            }
            else
            {
                textBox17.Text = "";

            };
        }

        private void checkBox28_CheckedChanged(object sender, EventArgs e)
        {
            string che = "04";
            if (checkBox28.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox9.Text = che;
            }
            else
            {
                textBox9.Text = "";

            };
        }

        private void checkBox17_CheckedChanged(object sender, EventArgs e)
        {
            string che = "05";
            if (checkBox17.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox8.Text = che;
            }
            else
            {
                textBox8.Text = "";

            };
        }

        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {
            string che = "06";
            if (checkBox15.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox5.Text = che;
            }
            else
            {
                textBox5.Text = "";

            };
        }

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            string che = "07";
            if (checkBox14.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox6.Text = che;
            }
            else
            {
                textBox6.Text = "";

            };
        }

        private void checkBox34_CheckedChanged(object sender, EventArgs e)
        {
            string che = "08";
            if (checkBox34.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox26.Text = che;
            }
            else
            {
                textBox26.Text = "";

            };
        }

        private void checkBox37_CheckedChanged(object sender, EventArgs e)
        {
            string che = "10";
            if (checkBox37.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox24.Text = che;
            }
            else
            {
                textBox24.Text = "";

            };
        }

        private void checkBox36_CheckedChanged(object sender, EventArgs e)
        {
            string che = "11";
            if (checkBox36.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox23.Text = che;
            }
            else
            {
                textBox23.Text = "";

            };
        }

        private void checkBox35_CheckedChanged(object sender, EventArgs e)
        {
            string che = "13";
            if (checkBox35.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox25.Text = che;
            }
            else
            {
                textBox25.Text = "";

            };
        }

        private void checkBox38_CheckedChanged(object sender, EventArgs e)
        {
            string che = "14";
            if (checkBox38.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox27.Text = che;
            }
            else
            {
                textBox27.Text = "";

            };
        }

        private void checkBox19_CheckedChanged(object sender, EventArgs e)
        {
            string che = "15";
            if (checkBox19.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox11.Text = che;
            }
            else
            {
                textBox11.Text = "";

            };
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            string che = "16";
            if (checkBox10.Checked)
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
                textBox7.Text = che;
            }
            else
            {
                textBox7.Text = "";

            };
        }

        private void checkBox18_CheckedChanged(object sender, EventArgs e)
        {
            string che = "19";
            if (checkBox18.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox10.Text = che;
            }
            else
            {
                textBox10.Text = "";

            };
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            string che = "20";
            if (checkBox9.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox21.Text = che;
            }
            else
            {
                textBox21.Text = "";

            };
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            string che = "21";
            if (checkBox7.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox12.Text = che;
            }
            else
            {
                textBox12.Text = "";

            };
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            string che = "22";
            if (checkBox8.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox15.Text = che;
            }
            else
            {
                textBox15.Text = "";

            };
        }

        private void checkBox29_CheckedChanged(object sender, EventArgs e)
        {
            string che = "22";
            if (checkBox29.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox16.Text = che;
            }
            else
            {
                textBox16.Text = "";

            };
        }

        private void checkBox30_CheckedChanged(object sender, EventArgs e)
        {
            string che = "24";
            if (checkBox30.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox18.Text = che;
            }
            else
            {
                textBox18.Text = "";

            };
        }

        private void checkBox31_CheckedChanged(object sender, EventArgs e)
        {
            string che = "25";
            if (checkBox31.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox19.Text = che;
            }
            else
            {
                textBox19.Text = "";

            };
        }

        private void checkBox32_CheckedChanged(object sender, EventArgs e)
        {
            string che = "26";
            if (checkBox32.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox20.Text = che;
            }
            else
            {
                textBox20.Text = "";

            };
        }

        private void checkBox33_CheckedChanged(object sender, EventArgs e)
        {
            string che = "27";
            if (checkBox33.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox22.Text = che;
            }
            else
            {
                textBox22.Text = "";

            };
        }

        private void checkBox12_CheckedChanged_1(object sender, EventArgs e)
        {
            string che = "01";
            if (checkBox12.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox3.Text = che;
            }
            else
            {
                textBox3.Text = "";

            };
        }

        private void checkBox13_CheckedChanged_1(object sender, EventArgs e)
        {
            string che = "02";
            if (checkBox13.Checked)
            {
                textBox4.Text = che;
            }
            else
            {
                textBox4.Text = "";

            };
        }

        private void checkBox14_CheckedChanged_1(object sender, EventArgs e)
        {
            string che = "07";
            if (checkBox14.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox6.Text = che;
            }
            else
            {
                textBox6.Text = "";

            };
        }

        private void checkBox15_CheckedChanged_1(object sender, EventArgs e)
        {
            string che = "06";
            if (checkBox15.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox5.Text = che;
            }
            else
            {
                textBox5.Text = "";

            };
        }

        private void checkBox16_CheckedChanged_1(object sender, EventArgs e)
        {
            string che = "17";
            if (checkBox16.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox7.Text = che;
            }
            else
            {
                textBox7.Text = "";

            };
        }

        private void checkBox17_CheckedChanged_1(object sender, EventArgs e)
        {
            string che = "05";
            if (checkBox17.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox8.Text = che;
            }
            else
            {
                textBox8.Text = "";

            };
        }

        private void checkBox18_CheckedChanged_1(object sender, EventArgs e)
        {
            string che = "19";
            if (checkBox18.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox10.Text = che;
            }
            else
            {
                textBox10.Text = "";

            };
        }

        private void checkBox19_CheckedChanged_1(object sender, EventArgs e)
        {
            string che = "15";
            if (checkBox19.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox11.Text = che;
            }
            else
            {
                textBox11.Text = "";

            };
        }

        private void checkBox36_CheckedChanged_1(object sender, EventArgs e)
        {
            string che = "11";
            if (checkBox36.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox23.Text = che;
            }
            else
            {
                textBox23.Text = "";

            };
        }

        private void checkBox37_CheckedChanged_1(object sender, EventArgs e)
        {
            string che = "10";
            if (checkBox37.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox24.Text = che;
            }
            else
            {
                textBox24.Text = "";

            };
        }

        private void checkBox7_CheckedChanged_1(object sender, EventArgs e)
        {
            string che = "21";
            if (checkBox7.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox12.Text = che;
            }
            else
            {
                textBox12.Text = "";

            };
        }

        private void checkBox8_CheckedChanged_1(object sender, EventArgs e)
        {
            string che = "22";
            if (checkBox8.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox15.Text = che;
            }
            else
            {
                textBox15.Text = "";

            };
        }

        private void checkBox9_CheckedChanged_1(object sender, EventArgs e)
        {
            string che = "20";
            if (checkBox9.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox21.Text = che;
            }
            else
            {
                textBox21.Text = "";

            };
        }

        private void checkBox10_CheckedChanged_1(object sender, EventArgs e)
        {
            string che = "16";
            if (checkBox10.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox13.Text = che;
            }
            else
            {
                textBox13.Text = "";

            };
        }

        private void checkBox28_CheckedChanged_1(object sender, EventArgs e)
        {
            string che = "04";
            if (checkBox28.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox9.Text = che;
            }
            else
            {
                textBox9.Text = "";

            };
        }

        private void checkBox29_CheckedChanged_1(object sender, EventArgs e)
        {
            string che = "22";
            if (checkBox29.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox16.Text = che;
            }
            else
            {
                textBox16.Text = "";

            };
        }

        private void checkBox11_CheckedChanged_1(object sender, EventArgs e)
        {
            string che = "03";
            if (checkBox11.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox17.Text = che;
            }
            else
            {
                textBox17.Text = "";

            };
        }

        private void checkBox34_CheckedChanged_1(object sender, EventArgs e)
        {
            string che = "08";
            if (checkBox34.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox26.Text = che;
            }
            else
            {
                textBox26.Text = "";

            };
        }

        private void checkBox35_CheckedChanged_1(object sender, EventArgs e)
        {
            string che = "13";
            if (checkBox35.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox25.Text = che;
            }
            else
            {
                textBox25.Text = "";

            };
        }

        private void checkBox32_CheckedChanged_1(object sender, EventArgs e)
        {
            string che = "26";
            if (checkBox32.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox20.Text = che;
            }
            else
            {
                textBox20.Text = "";

            };
        }

        private void checkBox33_CheckedChanged_1(object sender, EventArgs e)
        {
            string che = "27";
            if (checkBox33.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox22.Text = che;
            }
            else
            {
                textBox22.Text = "";

            };
        }

        private void checkBox30_CheckedChanged_1(object sender, EventArgs e)
        {
            string che = "24";
            if (checkBox30.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox18.Text = che;
            }
            else
            {
                textBox18.Text = "";

            };
        }

        private void checkBox38_CheckedChanged_1(object sender, EventArgs e)
        {
            string che = "14";
            if (checkBox38.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox27.Text = che;
            }
            else
            {
                textBox27.Text = "";

            };
        }

        private void checkBox31_CheckedChanged_1(object sender, EventArgs e)
        {
            string che = "25";
            if (checkBox31.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox19.Text = che;
            }
            else
            {
                textBox19.Text = "";

            };
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            string che = "26";
            if (checkBox31.Checked)
            {
                textBox19.Text = che;
            }
            else
            {
                textBox19.Text = "";

            };
        }

        private void crystalReportViewer1_ReportRefresh(object source, CrystalDecisions.Windows.Forms.ViewerEventArgs e)
        {
            this.Hide();
            REPTransactionListing ala = new REPTransactionListing();
            ala.ShowDialog();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            string che = "18";
            if (checkBox1.Checked)
            {
                textBox28.Text = che;
            }
            else
            {
                textBox28.Text = "";

            };
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            string che = "12";
            if (checkBox30.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox29.Text = che;
            }
            else
            {
                textBox29.Text = "";

            };
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
