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
    public partial class REPLoanProvision : Form
    {
        string cs = globalvar.cos;
        DataTable prodview = new DataTable();
        ReportDocument rprt = new ReportDocument();
        public REPLoanProvision()
        {
            InitializeComponent();
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
        public void GETDATA()
        {
            int Pproduct = Convert.ToInt32(comboBox1.SelectedValue);
            decimal lncure = 0.00m;
            decimal ln9cure = 0.00m;
            decimal ln180cure = 0.00m;
            decimal ln365cure = 0.00m;
            decimal lnAbovecure = 0.00m;
            decimal lnsav = 0.00m;
            decimal Sn9cure = 0.00m;
            decimal Sn180cure = 0.00m;
            decimal Sn365cure = 0.00m;
            decimal SnAbovecure = 0.00m;
            string lcsaveact = string.Empty;
            string lcloanact = string.Empty;
            decimal lbalance = 0.00m;
            decimal lbalance90 = 0.00m;
            decimal lbalance180 = 0.00m;
            decimal lbalance365 = 0.00m;
            decimal lbalanceA365 = 0.00m;
            decimal sbalance = 0.00m;
            decimal sbalance90 = 0.00m;
            decimal sbalance180 = 0.00m;
            decimal sbalance365 = 0.00m;
            decimal sbalanceA365 = 0.00m;
            decimal lnineBal = 0.00m;

            string cacctnumb = string.Empty;
            SqlConnection ndConnHandle1 = new SqlConnection(cs);
           // ndConnHandle1.CommandTimeout = 150;
            ndConnHandle1.Open();
            rprt.Load(System.Windows.Forms.Application.StartupPath + "\\Reports\\CRPProvision.rpt");
            SqlCommand cmd = new SqlCommand("[SPLoanProvisioningReport]", ndConnHandle1);
            cmd.CommandTimeout = 0;
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
            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ToDate", SqlDbType.Date)).Value = dateTimePicker1.Text;
            cmd.Parameters.AddWithValue("@Product", Pproduct);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            SPLoanProvisioningReport ds = new SPLoanProvisioningReport();
          //  DataTable dd1c = new DataTable();
          //  DataTable savd = new DataTable();
           // sda.Fill(dd1c);
           // sda.Fill(savd);
           // if (dd1c.Rows.Count > 0)
           // {
           ////     MessageBox.Show("Total number of rows = "+dd1c.Rows.Count);
           //     foreach (DataRow dr in dd1c.Rows)            //loan accounts
           //     {
           //         if (Convert.ToString(dr["cacctnumb"]).Trim().Substring(0, 3) == "130")
           //         {
           //             lcloanact = Convert.ToString(dr["ccustcode"]).Trim();

           //             lncure =Math.Abs(Convert.ToDecimal(dr["currentday"]));                     
           //             ln9cure= Math.Abs(Convert.ToDecimal(dr["nintyday"]));
           //             ln180cure = Math.Abs(Convert.ToDecimal(dr["oneightyday"]));
           //             ln365cure = Math.Abs(Convert.ToDecimal(dr["onesixtyfiveyday"]));
           //             lnAbovecure = Math.Abs(Convert.ToDecimal(dr["above365"]));
           //             //  MessageBox.Show("loan account is " + lcloanact+"\nLoan Amount is "+lncure);

                      
           //             foreach (DataRow dr2 in savd.Rows)              //savings
           //             {
           //                 if (Convert.ToString(dr2["cacctnumb"]).Substring(0, 3) == "250")
           //                 {
           //              //       MessageBox.Show("have found a savings account " + Convert.ToString(dr["cacctnumb"]).Trim().Substring(3,6));
           //                     if (Convert.ToString(dr2["ccustcode"]).Trim() == lcloanact )
           //                     {
           //                         lnsav = Math.Abs(Convert.ToDecimal(dr2["scurrentday"]));
           //                         Sn9cure = Math.Abs(Convert.ToDecimal(dr2["Snintyday"]));
           //                         Sn180cure = Math.Abs(Convert.ToDecimal(dr2["Soneightyday"]));
           //                         Sn365cure = Math.Abs(Convert.ToDecimal(dr2["Sonesixtyfiveyday"]));
           //                         SnAbovecure = Math.Abs(Convert.ToDecimal(dr2["Sabovethreesixsixday"]));
                                      
           //                         sbalance = sbalance + lnsav;
           //                      //   MessageBox.Show("savings account is " + lcloanact + "\nSavings Amount is 30days " + sbalance);
           //                         sbalance90 = sbalance90 + Sn9cure;
           //                         sbalance180 = sbalance180 + Sn180cure;
           //                         sbalance365 = sbalance365 + Sn365cure;
           //                         sbalanceA365 = sbalanceA365 + SnAbovecure;

           //                         lnsav = 0.00m;
           //                         Sn9cure = 0.00m;
           //                         Sn180cure = 0.00m;
           //                         Sn365cure = 0.00m;
           //                         SnAbovecure = 0.00m;
           //                           break;
           //                     }
           //                 }
                    //    }
               //   }

                    //both savings and loan details found 
                    //insert into datatable
              sda.Fill(ds, "SPLoanProvisioningReport");
            rprt.SetDataSource(ds);
            crystalReportViewer1.ReportSource = rprt;
            WindowState = FormWindowState.Maximized;

            //lbalance = lbalance + lncure;
            //lbalance90 = lbalance90 + ln9cure;
            //lbalance180 = lbalance180 + ln180cure;
            //lbalance365 = lbalance365 + ln365cure;
            //lbalanceA365 = lbalanceA365 + lnAbovecure;

            //lncure = 0.00m;
            //ln9cure = 0.00m;
            //ln180cure = 0.00m;
            //ln365cure = 0.00m;
            //lnAbovecure = 0.00m;

            //       }
            //     //  MessageBox.Show("Total loan balance = "+lbalance+"\nTotal Savings Balance = "+sbalance);

            //       using (SqlConnection nConnHandle2 = new SqlConnection(cs))
            //       {
            //           string cglquery = "update ProvisioningRep set s30days = @s30days, s90days = @s90days, s180days = @s180days,s365days = @s365days,sAbove365days = @sAbove365days,l30days = @l30days,l90days = @l90days,l180days = @l180days,l365days = @l365days,lAbove365days = @lAbove365days where ID = 1";
            //           SqlDataAdapter updCommand = new SqlDataAdapter();
            //           updCommand.UpdateCommand = new SqlCommand(cglquery, nConnHandle2);
            //           updCommand.UpdateCommand.Parameters.Add("@s30days", SqlDbType.Decimal).Value = sbalance;
            //           updCommand.UpdateCommand.Parameters.Add("@s90days", SqlDbType.Decimal).Value = sbalance90;
            //           updCommand.UpdateCommand.Parameters.Add("@s180days", SqlDbType.Decimal).Value = sbalance180;
            //           updCommand.UpdateCommand.Parameters.Add("@s365days", SqlDbType.Decimal).Value = sbalance365;
            //           updCommand.UpdateCommand.Parameters.Add("@sAbove365days", SqlDbType.Decimal).Value = sbalanceA365;
            //           updCommand.UpdateCommand.Parameters.Add("@l30days", SqlDbType.Decimal).Value = lbalance;
            //           updCommand.UpdateCommand.Parameters.Add("@l90days", SqlDbType.Decimal).Value = lbalance90;
            //           updCommand.UpdateCommand.Parameters.Add("@l180days", SqlDbType.Decimal).Value = lbalance180;
            //           updCommand.UpdateCommand.Parameters.Add("@l365days", SqlDbType.Decimal).Value = lbalance365;
            //           updCommand.UpdateCommand.Parameters.Add("@lAbove365days", SqlDbType.Decimal).Value = lbalanceA365;
            //           nConnHandle2.Open();
            //           updCommand.UpdateCommand.ExecuteNonQuery();
            //           nConnHandle2.Close();
            //       }

            //ds.spLoanTable.Rows.Add(new object[] { lbalance, lbalance90, lbalance180, lbalance365, lbalanceA365, sbalance, sbalance90, sbalance180, sbalance365, sbalanceA365 });

            //  decimal lnfirstloan = Convert.ToDecimal(ds.spLoanTable.Rows[0]["lbalance"]);
            //     MessageBox.Show("The First Row " + lbalance + lbalance90 + lbalance180+ lbalance365+ lbalanceA365 + sbalance+ sbalance90+ sbalance180+ sbalance365+ sbalanceA365);
            //   decimal lnfirstloan1 = Convert.ToDecimal(ds.spLoanTable.Rows[0]["sbalance"]);
            //  MessageBox.Show("Total Savings balance 2= " + lnfirstloan1);
            // decimal lnfirstloan2 = Convert.ToDecimal(ds.spLoanTable.Rows[0]["sbalance180"]);
            // MessageBox.Show("Total Savings balance 3= " + lnfirstloan2);

            //  }
            //    sda.Fill(ds, "SPLoanAgingReport");
            //    DataRow dr = new DataRow();// ds.SPLoanAgingReport.Rows);


        }
        private void Button1_Click(object sender, EventArgs e)
        {
            CMBTRANSACTION_TYPE.Visible = false;
            ComboBox2.Visible = false;
            TextBox2.Visible = false;
            comboBox1.Visible = false;
            label22.Visible = false;
            TextBox1.Visible = false;
            textBox10.Visible = false;
            TextBox3.Visible = false;
            TextBox4.Visible = false;
            TextBox5.Visible = false;
            TextBox6.Visible = false;
            TextBox7.Visible = false;
            TextBox8.Visible = false;
            textBox9.Visible = false;
            textBox11.Visible = false;
            textBox12.Visible = false;
            textBox13.Visible = false;
            textBox14.Visible = false;
            textBox15.Visible = false;
            textBox18.Visible = false;
            textBox17.Visible = false;
            textBox16.Visible = false;
            label18.Visible = false;
            label19.Visible = false;
            label13.Visible = false;
            label14.Visible = false;
            label20.Visible = false;
            dateTimePicker1.Visible = false;
            label21.Visible = false;

            label15.Visible = false;
            label16.Visible = false;
            label17.Visible = false;
            label14.Visible = false;

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

        private void crystalReportViewer1_ReportRefresh(object source, CrystalDecisions.Windows.Forms.ViewerEventArgs e)
        {
            this.Hide();
            REPLoanProvision ala = new REPLoanProvision();
            ala.ShowDialog();
        }

        private void REPLoanProvision_Load(object sender, EventArgs e)
        {
            getProduct();
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
