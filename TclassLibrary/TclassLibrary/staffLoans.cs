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

namespace TclassLibrary
{
    public partial class staffLoans : Form
    {
        string cs = string.Empty;
        int ncompid = 0;
        string dloca = string.Empty;
        DataTable staffview = new DataTable();
        DataTable filtview = new DataTable();
        DataTable loanview = new DataTable();
        DataTable empLonview = new DataTable();
        double gnTotalAmt = 0.00;

        public staffLoans(string tcCos, int tnCompid, string tcLoca)
        {
            InitializeComponent();
            cs = tcCos;
            ncompid = tnCompid;
            dloca = tcLoca;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void staffLoans_Load(object sender, EventArgs e)
        {
            this.Text = dloca+ "<< Staff Loan Management >>";
            getstaff();
            getLoans();
            loanGrid.Columns["dFrDate"].DefaultCellStyle.Format = "dd/MM/yyyy";
            loanGrid.Columns["dEdDate"].DefaultCellStyle.Format = "dd/MM/yyyy";
        }

        private void getstaff()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                staffview.Clear();
                ndConnHandle.Open();
                string dsql = "exec tsp_getActiveStaff " + ncompid;
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(staffview);
                ClientGrid.AutoGenerateColumns = false;
                ClientGrid.DataSource = staffview.DefaultView;
                ClientGrid.Columns[0].DataPropertyName = "staffno";
                ClientGrid.Columns[1].DataPropertyName = "fullname";
                ClientGrid.Columns[2].DataPropertyName = "depname";
                ClientGrid.Columns[3].DataPropertyName = "desname";
                ClientGrid.Columns[4].DataPropertyName = "dage";
                ClientGrid.Columns[5].DataPropertyName = "dgender";
                ndConnHandle.Close();
                textBox45.Text = staffview.Rows.Count.ToString();
                string tcemp = staffview.Rows[ClientGrid.CurrentCell.RowIndex]["staffno"].ToString();
                string tcempname = staffview.Rows[ClientGrid.CurrentCell.RowIndex]["fullname"].ToString();
                textBox53.Text = tcemp;
                empLonDetails(tcemp);
            }
        }

        private void getLoans()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string allosql = "select loan_id, loan_name,int_rate,amt_cap,dur_cap,def_dur from loans order by loan_name ";
                SqlDataAdapter alloadap = new SqlDataAdapter(allosql, ndConnHandle);
                alloadap.Fill(loanview);
                if (loanview.Rows.Count > 0)
                {
                    comboBox1.DataSource = loanview.DefaultView;
                    comboBox1.DisplayMember = "loan_name";
                    comboBox1.ValueMember = "loan_id";
                    comboBox1.SelectedIndex = -1;
                }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Down || e.KeyCode == Keys.Tab)
            {
                SelectNextControl(ActiveControl, true, true, true, true);
                e.Handled = true;
                page01ok();
            }
            else if (e.KeyCode == Keys.Up)
            {
                SelectNextControl(ActiveControl, false, true, true, true);
                e.Handled = true;
                page01ok();
            }
        }

        private void page01ok() //staff basic details 
        {

            DateTime dtoday = Convert.ToDateTime(DateTime.Today.ToShortDateString());
            DateTime dStartDate = Convert.ToDateTime(txtStartDate.Value.ToShortDateString());

            bool lAlloOK = dStartDate >= dtoday ? true : false;

            if (comboBox1.SelectedIndex > -1 && lAlloOK && loanAmt.Text != "" && textBox3.Text!="" && textBox18.Text!="")
            {
                lonSaveButton.Enabled = true;
                lonSaveButton.BackColor = Color.LawnGreen;
            }
            else
            {
                lonSaveButton.Enabled = false;
                lonSaveButton.BackColor = Color.Gainsboro;
            }
        }



        private void getfiltview(int tnfiltype)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                switch (tnfiltype)
                {
                    case 1: //branch 
                        filtview.Clear();
                        string basesql130 = "select br_name,branchid from branch where compobnk=1 order by br_name";
                        SqlDataAdapter baseda130 = new SqlDataAdapter(basesql130, ndConnHandle);
                        baseda130.Fill(filtview);
                        if (filtview.Rows.Count > 0)
                        {
                            comboBox17.DataSource = filtview.DefaultView;
                            comboBox17.DisplayMember = "br_name";
                            comboBox17.ValueMember = "branchid";
                            comboBox17.SelectedIndex = -1;
                        }
                        break;
                    case 2: //department 
                        filtview.Clear();
                        string basesql131 = "select dep_name,dep_id from dept order by dep_name";
                        SqlDataAdapter baseda131 = new SqlDataAdapter(basesql131, ndConnHandle);
                        baseda131.Fill(filtview);
                        if (filtview.Rows.Count > 0)
                        {
                            comboBox17.DataSource = filtview.DefaultView;
                            comboBox17.DisplayMember = "dep_name";
                            comboBox17.ValueMember = "dep_id";
                            comboBox17.SelectedIndex = -1;
                        }
                        break;
                    case 3: //designation
                        filtview.Clear();
                        string desigsql = "select des_name,des_id from designation order by des_name";
                        SqlDataAdapter dadesign = new SqlDataAdapter(desigsql, ndConnHandle);
                        dadesign.Fill(filtview);
                        if (filtview.Rows.Count > 0)
                        {
                            comboBox17.DataSource = filtview.DefaultView;
                            comboBox17.DisplayMember = "des_name";
                            comboBox17.ValueMember = "des_id";
                            comboBox17.SelectedIndex = -1;
                        }
                        break;
                    case 4: //band
                        filtview.Clear();
                        string bandsql = "select ban_name,ban_id from band order by ban_name";
                        SqlDataAdapter daband = new SqlDataAdapter(bandsql, ndConnHandle);
                        daband.Fill(filtview);
                        if (filtview.Rows.Count > 0)
                        {
                            comboBox17.DataSource = filtview.DefaultView;
                            comboBox17.DisplayMember = "ban_name";
                            comboBox17.ValueMember = "ban_id";
                            comboBox17.SelectedIndex = -1;
                        }
                        break;
                    case 5: //ethnicity 
                        filtview.Clear();
                        string ethsql = "select eth_name,eth_id from ethnies order by eth_name";
                        SqlDataAdapter daeth = new SqlDataAdapter(ethsql, ndConnHandle);
                        daeth.Fill(filtview);
                        if (filtview.Rows.Count > 0)
                        {
                            comboBox17.DataSource = filtview.DefaultView;
                            comboBox17.DisplayMember = "eth_name";
                            comboBox17.ValueMember = "eth_id";
                            comboBox17.SelectedIndex = -1;
                        }
                        break;
                    case 6: //cost centre 
                        filtview.Clear();
                        string cosql = "select cos_name,cos_id from costcent order by cos_name";
                        SqlDataAdapter dacos = new SqlDataAdapter(cosql, ndConnHandle);
                        dacos.Fill(filtview);
                        if (filtview.Rows.Count > 0)
                        {
                            comboBox17.DataSource = filtview.DefaultView;
                            comboBox17.DisplayMember = "cos_name";
                            comboBox17.ValueMember = "cos_id";
                            comboBox17.SelectedIndex = -1;
                        }
                        break;
                }
            }
        }

        private void getstaffByFilter(int tnFilter)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                staffview.Clear();
                ndConnHandle.Open();
                switch (tnFilter)
                {
                    case 0:     // all staff
                        string dsql0 = "exec tsp_getActiveStaff " + ncompid;
                        SqlDataAdapter da0 = new SqlDataAdapter(dsql0, ndConnHandle);
                        da0.Fill(staffview);
                        break;
                    case 1: //by branch
                        string dsql1 = "exec tsp_getActiveStaffByBranch " + ncompid + "," + comboBox17.SelectedValue;
                        SqlDataAdapter da1 = new SqlDataAdapter(dsql1, ndConnHandle);
                        da1.Fill(staffview);
                        break;
                    case 2: //by department
                        string dsql2 = "exec tsp_getActiveStaffByDept " + ncompid + "," + comboBox17.SelectedValue;
                        SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle);
                        da2.Fill(staffview);
                        break;
                    case 3:     // designation
                        string dsql3 = "exec tsp_getActiveStaffByDesig " + ncompid + "," + comboBox17.SelectedValue;
                        SqlDataAdapter da3 = new SqlDataAdapter(dsql3, ndConnHandle);
                        da3.Fill(staffview);
                        break;
                    case 4: //band
                        string dsql4 = "exec tsp_getActiveStaffByBand " + ncompid + "," + comboBox17.SelectedValue;
                        SqlDataAdapter da4 = new SqlDataAdapter(dsql4, ndConnHandle);
                        da4.Fill(staffview);
                        break;
                    case 5: //by ethnicity
                        string dsql5 = "exec tsp_getActiveStaffByEth " + ncompid + "," + comboBox17.SelectedValue;
                        SqlDataAdapter da5 = new SqlDataAdapter(dsql5, ndConnHandle);
                        da5.Fill(staffview);
                        break;
                    case 6:     // cost centre
                        string dsql6 = "exec tsp_getActiveStaffByCoscen " + ncompid + "," + comboBox17.SelectedValue;
                        SqlDataAdapter da6 = new SqlDataAdapter(dsql6, ndConnHandle);
                        da6.Fill(staffview);
                        break;
                    case 10: //female staff
                        string dsql10 = "exec tsp_getActiveStaffFemale " + ncompid;
                        SqlDataAdapter da10 = new SqlDataAdapter(dsql10, ndConnHandle);
                        da10.Fill(staffview);
                        break;
                    case 11: //by department
                        string dsql11 = "exec tsp_getActiveStaffMale " + ncompid;
                        SqlDataAdapter da11 = new SqlDataAdapter(dsql11, ndConnHandle);
                        da11.Fill(staffview);
                        break;
                }
                if (staffview.Rows.Count > 0)
                {
                    ClientGrid.AutoGenerateColumns = false;
                    ClientGrid.DataSource = staffview.DefaultView;
                    ClientGrid.Columns[0].DataPropertyName = "staffno";
                    ClientGrid.Columns[1].DataPropertyName = "fullname";
                    ClientGrid.Columns[2].DataPropertyName = "depname";
                    ClientGrid.Columns[3].DataPropertyName = "desname";
                    ClientGrid.Columns[4].DataPropertyName = "dage";
                    ClientGrid.Columns[5].DataPropertyName = "dgender";
                    string tcemp1 = staffview.Rows[0]["staffno"].ToString();
                    //                    textBox46.Text = tcemp1;
                    textBox45.Text = staffview.Rows.Count.ToString();
                    string tcemp = staffview.Rows[0]["staffno"].ToString();
                    string tcempname = staffview.Rows[0]["fullname"].ToString();
                    textBox53.Text = tcemp;
                    empLonDetails(tcemp);
                }
                ndConnHandle.Close();
                //string tcemp1 = staffview.Rows[0]["staffno"].ToString();
                //textBox46.Text = tcemp1;
                //textBox45.Text = staffview.Rows.Count.ToString();
            }
        }

        private void empLonDetails(string tcStaffNo)
        {
            empLonview.Clear();
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string empsql = "exec tsp_empLoan " + ncompid + ",'" + tcStaffNo + "'";
                SqlDataAdapter daemp = new SqlDataAdapter(empsql, ndConnHandle);
                daemp.Fill(empLonview);
                if (empLonview.Rows.Count > 0)
                {
                    loanGrid.AutoGenerateColumns = false;
                    loanGrid.DataSource = empLonview.DefaultView;
                    loanGrid.Columns[0].DataPropertyName = "loan_name";
                    loanGrid.Columns[1].DataPropertyName = "loanamt";
                    loanGrid.Columns[2].DataPropertyName = "npayment";
                    loanGrid.Columns[3].DataPropertyName = "bngprd";
                    loanGrid.Columns[4].DataPropertyName = "endprd";
                }
            }
        }
        private void updLonDetails()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
 
                string cquery = "Insert Into loansdet (staffno,loan_id,loanamt,nbperiod,bngprd,endprd,loanrate,npayment,compid) ";
                cquery += "values (@tstaffno,@tloan_id,@tloanamt,@tnbperiod,@tbngprd,@tendprd,@tloanrate,@tnpayment,@tcompid)";

                SqlDataAdapter cuscommand = new SqlDataAdapter();
                cuscommand.InsertCommand = new SqlCommand(cquery, ndConnHandle);

                cuscommand.InsertCommand.Parameters.Add("@tstaffno", SqlDbType.VarChar).Value = textBox53.Text.ToString().Trim();
                cuscommand.InsertCommand.Parameters.Add("@tloan_id", SqlDbType.Int).Value = Convert.ToInt16(comboBox1.SelectedValue);
                cuscommand.InsertCommand.Parameters.Add("@tloanamt", SqlDbType.Decimal).Value = Convert.ToDecimal(loanAmt.Text);
                cuscommand.InsertCommand.Parameters.Add("@tnbperiod", SqlDbType.Int).Value = Convert.ToInt16(textBox3.Text);

                cuscommand.InsertCommand.Parameters.Add("@tbngprd", SqlDbType.DateTime).Value = Convert.ToDateTime(textBox17.Text) ;
                cuscommand.InsertCommand.Parameters.Add("@tendprd", SqlDbType.DateTime).Value = Convert.ToDateTime(txtEndDate.Text);
                cuscommand.InsertCommand.Parameters.Add("@tloanrate", SqlDbType.Decimal).Value = Convert.ToDecimal(textBox1.Text);
                cuscommand.InsertCommand.Parameters.Add("@tnpayment", SqlDbType.Decimal).Value = Convert.ToDecimal(textBox18.Text);


                cuscommand.InsertCommand.Parameters.Add("@tcompid", SqlDbType.Int).Value = ncompid;

                ndConnHandle.Open();
                cuscommand.InsertCommand.ExecuteNonQuery();
                ndConnHandle.Close();
                MessageBox.Show("Staff Loan details added successfully");
            }
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            getstaff();
        }

        private void radioButton14_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton14.Checked)
            {
                getfiltview(1);
            }
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton9.Checked)
            {
                getfiltview(2);
            }
        }

        private void radioButton13_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton13.Checked)
            {
                getfiltview(3);
            }
        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton10.Checked)
            {
                getfiltview(4);
            }
        }

        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton11.Checked)
            {
                getfiltview(5);
            }
        }

        private void radioButton15_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton15.Checked)
            {
                getfiltview(6);
            }
        }

        private void radioButton18_CheckedChanged(object sender, EventArgs e)
        {
            getstaffByFilter(10); //female staff
        }

        private void radioButton22_CheckedChanged(object sender, EventArgs e)
        {
            getstaffByFilter(11); //male staff
        }

        private void comboBox17_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox17.Focused)
            {
                int dfilt = (radioButton8.Checked ? 0 : (radioButton14.Checked ? 1 : (radioButton9.Checked ? 2 : (radioButton13.Checked ? 3 : (radioButton10.Checked ? 4 : (radioButton11.Checked ? 5 :
                    (radioButton15.Checked ? 6 : 7)))))));
                getstaffByFilter(dfilt);
            }
        }

        private void button25_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lonSaveButton_Click(object sender, EventArgs e)
        {
            updLonDetails();
            initvariables();
            getstaff();
        }

        private void initvariables()
        {
            comboBox1.SelectedIndex = -1;
            txtStartDate.Value = DateTime.Today;
            loanAmt.Text = textBox1.Text = textBox2.Text = textBox3.Text = textBox14.Text = textBox21.Text = textBox13.Text = textBox17.Text =
                textBox18.Text = textBox12.Text = textBox8.Text = textBox7.Text = textBox6.Text = textBox5.Text = txtEndDate.Text =  "";
            radioButton1.Checked = radioButton2.Checked = false;
        }

        private void ClientGrid_Click(object sender, EventArgs e)
        {
            string tcemp = staffview.Rows[ClientGrid.CurrentCell.RowIndex]["staffno"].ToString();
            string tcempname = staffview.Rows[ClientGrid.CurrentCell.RowIndex]["fullname"].ToString();
            textBox53.Text = tcemp;
            empLonDetails(tcemp);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.Focused )
            {
                textBox1.Text = Convert.ToDecimal(loanview.Rows[comboBox1.SelectedIndex]["int_rate"]).ToString("N2");
                textBox2.Text = Convert.ToDecimal(loanview.Rows[comboBox1.SelectedIndex]["amt_cap"]).ToString("N2");
                textBox3.Text = Convert.ToInt16(loanview.Rows[comboBox1.SelectedIndex]["def_dur"]).ToString("N0");
                page01ok();
            }
        }

        private void loanAmt_Validated(object sender, EventArgs e)
        {
            if(loanAmt.Text != "")
            {
                loanAmt.Text = Convert.ToDecimal(loanAmt.Text).ToString("N2");
                calcInterest();
            }
            page01ok();
        }

        private void textBox3_Validated(object sender, EventArgs e)
        {
                calcInterest();
                page01ok();
        }

        private void stDate_ValueChanged(object sender, EventArgs e)
        {
            calcInterest();
            page01ok();
        }

        private void calcInterest()
        {
            if (loanAmt.Text != "" && textBox1.Text != "" && textBox3.Text != "" )
            {
//                MessageBox.Show("We are inside calculation of interest method");
                int dmonth = Convert.ToInt32(textBox3.Text);
                DateTime start_date =txtStartDate.Value;                                //start date of loan
                textBox17.Text = start_date.ToLongDateString().ToString();
                DateTime end_date = Convert.ToDateTime(txtStartDate.Text).AddMonths(dmonth);//.ToLongDateString();// .AddDays(ddays).ToLongDateString();
                txtEndDate.Text = end_date.ToLongDateString().ToString();// Convert.ToDateTime(txtEndDate.Text).ToLongDateString();
//                MessageBox.Show("st")
                //                DateTime end_date = Convert.ToDateTime(txtEndDate.Text);                //end date of loan
                double dRate = Convert.ToDouble(textBox1.Text);                         //interest rate per annum
                double pv = Convert.ToDouble(loanAmt.Text);                             // (radioButton1.Checked ? double.Parse(textBox5.Text) : double.Parse(textBox3.Text));              //Principal
                int dDur = Convert.ToInt32(textBox3.Text);                              //duration - currently in months
                double dpaymt = 0.00;
                double intPay = 0.00;
                double totPay = 0.00;

                //double gnLoanInterest = Convert.ToDouble(clientview.Rows[clientgrid.CurrentCell.RowIndex]["loan_interest"]);
                loanCalculation lc = new loanCalculation();
                int ppyr = 12;// Convert.ToInt32(textBox8.Text);           // number of payment per year
                double newrate = dRate / 100 / ppyr;

                if (dRate > 0.00)
                {
                    dpaymt = Math.Abs(loanCalculation.pmt(newrate, dDur, pv, 0.00, 0));  // Fixed Periodic Payment
                    intPay = loanCalculation.intrest(dRate, dDur, ppyr, pv);
                    totPay = dpaymt * dDur;              // Paymt * dDur;
                }
                else
                {
                    dpaymt = pv / dDur;
                    intPay = 0.0;
                    totPay = dpaymt * dDur;
                }

                textBox18.Text = dpaymt.ToString("N2");     // Paymt.ToString("N2");
                textBox13.Text = totPay.ToString("N2");
                gnTotalAmt = totPay;
                textBox14.Text = pv.ToString("N2");
                textBox21.Text = intPay.ToString("N2");         // (totPay - pv).ToString("N2");
                textBox12.Text = textBox3.Text;
                //textBox24.Text = Math.Abs(dpaymt).ToString("N2");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Under construction, pls remind JIBI");
        }
    }
}
