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
using TclassLibrary;


namespace WinTcare
{
    public partial class Paymt : Form
    {
        //        public 
        DataTable transview = new DataTable();
        string gcFullName = "";
        decimal gnDepositAmt = 0.00m;
        decimal gnInvBalance = 0.00m;
        bool glClientDeposit = false;
//        bool glAmountPaid = false;
        int gnVisno = 0;
        public Paymt()
        {
            //           DataTable transview = new DataTable();
            InitializeComponent();
        }

        private void Paymt_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + "<< Client Receipt >>";
            textBox2.Text = DateTime.Now.ToShortDateString();
            textBox16.Text = DateTime.Now.ToShortDateString();
            textBox18.Text = DateTime.Now.ToShortDateString();
            maskedTextBox1.Mask = "000,000,000.00";
            getclientList();
            clientgrid.Focus();
            firstclient();
        }

        private void getclientList()
        {
            string cs = globalvar.cos;
            string ncompid = globalvar.gnCompid.ToString().Trim();
            string dsql = "exec tsp_OutstandingReceipts  " + ncompid;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                DataTable ds = new DataTable();
                da.Fill(ds);
                if (ds.Rows.Count > 0)
                {
//                    gnVisno = Convert.ToInt16(ds.Rows["visno"]);
                    clientgrid.AutoGenerateColumns = false;
                    clientgrid.DataSource = ds.DefaultView;
                    clientgrid.Columns[0].DataPropertyName = "fname";
                    clientgrid.Columns[1].DataPropertyName = "mname";
                    clientgrid.Columns[2].DataPropertyName = "lname";
                    clientgrid.Columns[3].DataPropertyName = "visdate";      // "age";
                    clientgrid.Columns[4].DataPropertyName = "vistime";
                    clientgrid.Columns[5].DataPropertyName = "visno";
                    clientgrid.Columns[6].DataPropertyName = "ccustcode";
                    textBox9.Text = ds.Rows.Count.ToString();
      //              for (int i = 0; i < 10; i++)
        //            {
                        //                        DataGridViewRow drow = new  DataGridViewRow();
          //              ds.Rows.Add();
            //        }
                    ndConnHandle.Close();
                    clientgrid.Focus();
                }
            }
        }

        private void gettrans(string tcCustCode)
        {
            string cs1 = globalvar.cos;
            string ncompid = globalvar.gnCompid.ToString().Trim();
            decimal lnInv = 0.00m;
            string dsql1 = "exec tsp_OutstandingReceiptsDetailed " + "'" + tcCustCode.Trim() + "'" + "," + ncompid;
            transview.Clear();
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs1))
            {
                ndConnHandle1.Open();
                SqlDataAdapter da1 = new SqlDataAdapter(dsql1, ndConnHandle1);
                DataTable newd = new DataTable();
                decimal dvalue=0.00m;
                //           DataTable ds1 = new DataTable();
//                SqlDataReader da2 = new SqlDataReader(dsql1,ndConnHandle1);
                da1.Fill(transview);
                if (transview.Rows.Count > 0)
                {
                    TransGrid.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                    TransGrid.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                    TransGrid.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                    TransGrid.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                    TransGrid.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                    TransGrid.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                    TransGrid.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    TransGrid.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    TransGrid.AutoGenerateColumns = false;
                    TransGrid.DataSource = transview.DefaultView;
                    TransGrid.Columns[1].DataPropertyName = "dtrandate";
                    TransGrid.Columns[2].DataPropertyName = "ctrandesc";
                    TransGrid.Columns[3].DataPropertyName = "quantity";
                    TransGrid.Columns[4].DataPropertyName = "unit_price";
                    TransGrid.Columns[5].DataPropertyName = "ntranamnt";
                    TransGrid.Columns[6].DataPropertyName = "rev_acct";
                    TransGrid.Columns[7].DataPropertyName = "exp_acct";
       //             TransGrid.Columns["ucost"].DefaultCellStyle.Format = "#,0.00";
                    ndConnHandle1.Close();

                    int RowCount = transview.Rows.Count;
                    for (int i = 0; i < RowCount; i++)
                    {
                        lnInv = lnInv + Convert.ToDecimal(transview.Rows[i]["ntranamnt"]);
                        //                        transview.Rows[i].cell
                        //            transview.Rows[i][0] = true;
                    }
                    textBox22.Text = lnInv.ToString();
                    textBox19.Text = lnInv.ToString();
                    textBox20.Text = "0.00";
//                    for (int i = 0; i < 10; i++)
  //                  {
                        //                        DataGridViewRow drow = new  DataGridViewRow();
    //                    transview.Rows.Add();
      //              }

                }
                else
                {
                    //                    MessageBox.Show("We have not found any rows here");
                }
            }
        }

        private void getbalance(string tcCustCode)
        {
            string cs1 = globalvar.cos;
            string ncompid = globalvar.gnCompid.ToString().Trim();
            textBox13.Text = "0.00";
            decimal lnInv = 0.00m;
            decimal lnpaid = 0.00m;
            gnInvBalance = 0.00m;
            textBox12.Text = genbill.genvoucher(cs1, DateTime.Now);
            string dsql1 = "exec tsp_PayBalance " + ncompid + "," + "'" + tcCustCode.Trim() + "'";
            string sn1 = "select nbookbal from glmast where nbookbal>0.00 and ccustcode = " + tcCustCode.Trim();
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs1))
            {
                ndConnHandle1.Open();
                SqlDataAdapter da1 = new SqlDataAdapter(dsql1, ndConnHandle1);
                DataTable ds1 = new DataTable();
                da1.Fill(ds1);
                if (ds1.Rows.Count > 0)
                {
                    int RowCount = ds1.Rows.Count;
                    for (int i = 0; i < RowCount; i++)
                    {
                        lnInv = lnInv + (Convert.ToDecimal(ds1.Rows[i]["ntranamnt"]) < 0.00m ? Convert.ToDecimal(ds1.Rows[i]["ntranamnt"]) : 0.00m);
                        lnpaid = lnpaid + (Convert.ToDecimal(ds1.Rows[i]["ntranamnt"]) > 0.00m && Convert.ToBoolean(ds1.Rows[i]["lpaid"]) == true ? Convert.ToDecimal(ds1.Rows[i]["ntranamnt"]) : 0.00m);
                    }
                    //                    MessageBox.Show("invoice, paid " + lnInv + ", " + lnpaid);
                    textBox29.Text = Math.Abs(lnInv - lnpaid).ToString();
                    textBox27.Text = lnpaid.ToString();
                    gnInvBalance = lnInv - lnpaid;
                    textBox5.Text = "0.00";

                    SqlDataAdapter da2 = new SqlDataAdapter(sn1, ndConnHandle1);
                    DataTable bookbal = new DataTable();
                    da2.Fill(bookbal);
                    if (bookbal.Rows.Count > 0)
                    {
                        gnDepositAmt = Convert.ToDecimal(bookbal.Rows[0]["nbookbal"]);
                        textBox13.Text = gnDepositAmt.ToString();
                    }
                    else { gnDepositAmt = 0.00m; }
                    ndConnHandle1.Close();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void firstclient()
        {
            if(clientgrid.Rows.Count>0)
            {
                string firstcode = clientgrid.Rows[0].Cells[6].Value.ToString();
                textBox6.Text = clientgrid.Rows[0].Cells[6].Value.ToString();
                gcFullName = clientgrid.Rows[0].Cells[0].Value.ToString() + " " + clientgrid.Rows[0].Cells[2].Value.ToString();
                gettrans(firstcode);
                getbalance(firstcode);
            }
        }

        private void clientgrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            TransGrid.EndEdit();
            string dcode = clientgrid.Rows[e.RowIndex].Cells[6].Value.ToString();
            textBox6.Text = clientgrid.Rows[e.RowIndex].Cells[6].Value.ToString();
            gcFullName = clientgrid.Rows[e.RowIndex].Cells[0].Value.ToString()+ " "+clientgrid.Rows[e.RowIndex].Cells[2].Value.ToString();
            gnVisno = Convert.ToInt16(clientgrid.Rows[e.RowIndex].Cells[5].Value.ToString()); 
            gettrans(dcode);
            getbalance(dcode);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Down)
            {
                SelectNextControl(ActiveControl, true, true, true, true);
                e.Handled = true;
                //          MessageBox.Show("ABOUT TO to to allclear down");
                AllClear2Go();
            }
            else if (e.KeyCode == Keys.Up)
            {
                SelectNextControl(ActiveControl, false, true, true, true);
                e.Handled = true;
                //        MessageBox.Show("ABOUT TO to to allclear up");
                AllClear2Go();
            }
        }


        #region Checking if all the mandatory conditions are satisfied
        private void AllClear2Go()
        {
            if (maskedTextBox1.Text.Replace(".", "").Replace(",", "").Trim() != "" && textBox4.Text != "" && textBox27.Text != "")
            {
                //MessageBox.Show("All clear to go");
                SaveButton.Enabled = true;
                SaveButton.BackColor = Color.LawnGreen;
                SaveButton.Select();
            }
            else
            {
                //MessageBox.Show("Not yet");
                SaveButton.Enabled = false;
                SaveButton.BackColor = Color.FromArgb(224, 224, 224);        // Color.Red;
            }

        }
        #endregion

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (this.textBox4.Text == "")
            {
                textBox4.Text = "Payment for products/services";
            }
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {
            //      daltext dtext = new daltext();
            //            decimal dpaid = Convert.ToDecimal(textBox15.Text);
            //            MessageBox.Show("We are going for daltext with ");
            //            textBox10.Text = daltext.inwords(Convert.ToDecimal(textBox15.Text));       //  dte "This is the amount in words";
        }

        //       private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        //     {
        //          textBox10.Text = daltext.inwords(Convert.ToDecimal(textBox1.Text));       //  dte "This is the amount in words";
        //   }

        private void itemisedbilling(decimal tnAmt)
        {
            decimal lnOriginalTotalPay = tnAmt;
            decimal lnTotalPay = tnAmt;
            decimal lnPaidTotal = 0.00m;
            decimal lnInvBalance = Convert.ToDecimal(textBox29.Text);
//            MessageBox.Show("We started with an invoice balanceof " + lnInvBalance);
            int RowCount = TransGrid.Rows.Count;
            for (int i = 0; i < RowCount; i++)
            {
                //             MessageBox.Show("working with iteration number " + i);
                decimal lnBilledAmt = Convert.ToDecimal(transview.Rows[i]["ntranamnt"]);
                if (lnTotalPay >= lnBilledAmt)
                {
                    lnTotalPay = lnTotalPay - lnBilledAmt;
                    lnInvBalance = lnInvBalance - lnBilledAmt;
                    lnPaidTotal = lnPaidTotal + lnBilledAmt;
                    maskedTextBox1.Text = lnPaidTotal.ToString();
                    textBox29.Text = lnInvBalance.ToString();
                    string fintext = maskedTextBox1.Text.Replace(",", "").PadLeft(12, ' ');     // .Trim(); 
                                                                                                //            string cwords = daltext.inwords(fintext);
                                                                                                //           textBox10.Text = cwords.Replace("MAIN", globalvar.gcCurrName.Trim()).Replace("UNIT", globalvar.gcCurrUnit.Trim());        // + globalvar.gcCurrName;//  daltext.inwords(fintext);
                                                                                                //              Replace lpaid With.T.
                                                                                                //              glAmountPaid =.T.
                    maskedTextBox1.Text = lnPaidTotal.ToString().Trim().PadLeft(12, ' ');
                }
                else
                {
                    MessageBox.Show("The amount paid cannot cover any item");
                    maskedTextBox1.Text = "0.00";
                    textBox27.Text = "0.00";
                    textBox29.Text = textBox22.Text;
                    break;
                }
            }           //end of for

            if (lnTotalPay > 0.00m)         //Client has tendered something
            {
                if (lnPaidTotal > 0.00m)             //at least a single item has been paid for
                {
           //         MessageBox.Show("Step 3: At least one item has been paid for ");
                    string fintext = maskedTextBox1.Text.Replace(",", "").PadLeft(12, ' ');     // .Trim(); 
                    string cwords = daltext.inwords(fintext);
                    textBox10.Text = cwords.Replace("MAIN", globalvar.gcCurrName.Trim()).Replace("UNIT", globalvar.gcCurrUnit.Trim());        // + globalvar.gcCurrName;//  daltext.inwords(fintext);
                    textBox29.Text = (Convert.ToDecimal(lnInvBalance) - Convert.ToDecimal(maskedTextBox1.Text)).ToString(); // .text29.Value
                }
                else                          //not a single item has been paid for, will check for waiver
                {
                    string dwav = textBox5.Text.ToString().Trim();
                    //          MessageBox.Show("Step 4 going to check for waiver "+dwav);
                    if (Convert.ToDecimal(textBox5.Text) > 0.00m)       // if (textBox5.Text.ToString().Trim() != "")      // && an amount has been waived
                    {
                        //                        MessageBox.Show("Step 5 : an amount has been waived");
//                        glAmountPaid = true;
                        //          textBox27.Text = (Convert.ToDecimal(textBox22.Text) - Convert.ToDecimal(textBox5.Text)).ToString(); //.text3.Value =.text5.Value -.text16.Value
                        //        textBox10.Text = (Convert.ToDecimal(maskedTextBox1.Text) > 0.00m ? daltext.inwords(maskedTextBox1.Text) : ""); //.text33.Value = Iif(.text29.Value > 0.00, daltext(.text29.Value), '')
                        //      textBox29.Text = (gnInvBalance - (Convert.ToDecimal(maskedTextBox1.Text) + Convert.ToDecimal(textBox5.Text))).ToString(); //.text35.Value = gnInvBalance - (.text29.Value +.text16.Value)
                    }
                    else
                    {
   //                     glAmountPaid = false;
                        //           MessageBox.Show("No amount has been waived");
                        maskedTextBox1.Text = "0.00";
                        textBox29.Text = gnInvBalance.ToString();
                    }
                }
            }       //lnTotalPay 
                    //       MessageBox.Show("Step last of itemised billing");

            if (lnOriginalTotalPay > lnPaidTotal)
            {
                string lcOrig = lnOriginalTotalPay.ToString().Trim().PadLeft(20, ' ');                    //lcOrig = Padl(Alltrim(Str(lnOriginalTotalPay, 10, 2)), 20, ' ')
                string lcpaid = lnPaidTotal.ToString().ToString().PadLeft(32, ' ');                       //Padl(Alltrim(Str(lnPaidTotal, 10, 2)), 20, ' ')
                string lcChange = (lnOriginalTotalPay - lnPaidTotal).ToString().Trim().PadLeft(25, ' ');    //Padl(Alltrim(Str(lnOriginalTotalPay - lnPaidTotal, 10, 2)), 20, ' ')
                MessageBox.Show("Total Tendered : " + lcOrig + "\n" + "Total Paid : " + lcpaid + "\n" + "Change Due : " + lcChange, "Payment Details", MessageBoxButtons.OK);
            }
        }       //itemisedbilling



        private void maskedTextBox1_Validated(object sender, EventArgs e)
        {
            string cpayment = maskedTextBox1.Text;
            if (cpayment.Replace(",", "").Replace(".", "").Trim() != "")         //masked text box was not empty, meaning values were keyed in
            {
                if (cpayment.Substring(cpayment.Length - 3, 1) != ".")  //if no decimal are entered, we will conca.. .00 to the endi 
                {
                    cpayment = maskedTextBox1.Text.Replace(",", "").Replace(".", "").Trim() + ".00";
                    maskedTextBox1.Text = cpayment.PadLeft(12, ' ');
                } else
                {
                    cpayment = maskedTextBox1.Text.Replace(",", "").Replace(".", "").Trim();
                }
                string fintext = maskedTextBox1.Text.Replace(",", "").PadLeft(12, ' ');     // .Trim(); 
                string cwords = daltext.inwords(fintext);
                textBox10.Text = cwords.Replace("MAIN", globalvar.gcCurrName.Trim()).Replace("UNIT", globalvar.gcCurrUnit.Trim());        // + globalvar.gcCurrName;//  daltext.inwords(fintext);
            }
            else   //        MessageBox.Show("enter was pressed, text box empty ");
            {
                string cpay = textBox22.Text.Trim().PadLeft(12, ' ').ToString();
                maskedTextBox1.Text = cpay;
                textBox27.Text = cpay;
//                textBox29.Text = "0.00";
                cpay = cpay.Trim().ToString();
                string cwords = daltext.inwords(cpay);
                textBox10.Text = cwords.Replace("MAIN", globalvar.gcCurrName.Trim()).Replace("UNIT", globalvar.gcCurrUnit.Trim());        // + globalvar.gcCurrName;//  daltext.inwords(fintext);
                if (transview.Rows.Count > 0)
                {
                    foreach (DataGridViewRow row in TransGrid.Rows)        //   for (int i = 0; i < RowCount; i++)
                    {
                        row.Cells[0].Value = true;
                    }
                }
            }
        //    MessageBox.Show("before checking for credit amount");
            if (gnDepositAmt > 0.00m && gnDepositAmt >= Convert.ToDecimal(textBox22.Text))
            {
                if (MessageBox.Show("Client has credit balance, do you want to deduct " + "\n" + " this amount from the credit balance", "Credit Balance Check", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    glClientDeposit = true;
                    string cpay = textBox22.Text.Trim().PadLeft(12, ' ').ToString();
                    maskedTextBox1.Text = cpay;
                    textBox27.Text = cpay;
                    textBox29.Text = (Convert.ToDecimal(textBox29.Text) - Convert.ToDecimal(textBox22.Text)).ToString();
                    cpay = cpay.Trim().ToString();
                    string cwords = daltext.inwords(cpay);
                    textBox10.Text = cwords.Replace("MAIN", globalvar.gcCurrName.Trim()).Replace("UNIT", globalvar.gcCurrUnit.Trim());        // + globalvar.gcCurrName;//  daltext.inwords(fintext);
                    if (transview.Rows.Count > 0)
                    {
                        foreach (DataGridViewRow row in TransGrid.Rows)        //   for (int i = 0; i < RowCount; i++)
                        {
                            row.Cells[0].Value = true;
                        }
                    }
                }
                else { glClientDeposit = false; }
            } else
            {
                decimal nAmt = Convert.ToDecimal(maskedTextBox1.Text.Replace(",", ""));
  //              MessageBox.Show("the amount going to itemised is " + nAmt);
//                maskedTextBox1.Text = nAmt.ToString().Trim().PadLeft(12, ' ');
                itemisedbilling(nAmt);
            }
            AllClear2Go();
        }


        private void maskedTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void maskedTextBox1_Enter(object sender, EventArgs e)
        {
            maskedTextBox1.Clear();
            textBox10.Clear();
        }       // end of maskedTextBox1_Enter


        private void textBox4_Leave(object sender, EventArgs e)
        {
            if (this.textBox4.Text == "")
            {
                textBox4.Text = "Payment for products/services";
            }
            AllClear2Go();
        }

        private void maskedTextBox1_Leave(object sender, EventArgs e)
        {
            string cpayment = maskedTextBox1.Text.PadLeft(12, ' ');
        }       //end of maskedTextBox1_Leave

        private void SaveButton_Click(object sender, EventArgs e)
        {

            string cs1 = globalvar.cos;
            string ncompid = globalvar.gnCompid.ToString().Trim();
            string lcServ_name = textBox4.Text.Trim().ToString() + " " + gcFullName;
            string tcCustcode = textBox6.Text;
            string tcAcctNumb = globalvar.ClientAcctPrefix + textBox6.Text.ToString();
            string tcContra = globalvar.gcIntSuspense;
            string tcUserid = globalvar.gcUserid;
            int tncompid = globalvar.gnCompid;
            decimal tnTranAmt = Math.Abs(Convert.ToDecimal(maskedTextBox1.Text.ToString().Replace(",",""))+ Convert.ToDecimal(textBox5.Text.Trim().ToString()));
            decimal tnContAmt = -Math.Abs(tnTranAmt);
            string tcVoucher = genbill.genvoucher(cs1, globalvar.gdSysDate);
            decimal unitprice = tnTranAmt;
            string tcChqno = textBox3.Text;
            string lcChkTransNo = (radioButton4.Checked ? "000001" : textBox3.Text);
            decimal lnWaiveAmt = Convert.ToDecimal(textBox5.Text);
            string tcTranCodeP = "92";          //posting transaction code credit
            string tcTranCodeC = "93";          //contra transaction code debit
            int lnServID = 1;
            bool llPaid = true;
       //     int tnqty = 1;
            string tcReceipt = genreceipt.getreceipt(cs1, globalvar.gdSysDate);
            bool llCashpay = true;      // (checkBox3.Checked ? true : false);
            //int visno = getVisitNumber.visitno(cs1,ncompid,tcCustcode);
            bool lisproduct = false;
            bool lFreeBee = false;
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs1))
            {

                //            *******************update the dashboard******************************
                //                gcDuptype = 'F1' && payment receipt from clients
                //gnDIncome =.text29.Value

                //  gnDnYear = Year(gdSysDate)

                //gnDmonth = Month(gdSysDate)

                //   gnDAcct_rec = -Abs(gnDIncome)
                bool lClDep = glClientDeposit;
                updateGlmast gls = new updateGlmast();
                updateTranhist tls = new updateTranhist();
        //        MessageBox.Show("Before glClientDeposit with "+lClDep);
                if (glClientDeposit == false)     //                   If !glClientDeposit, client pays over the counter.
                {
//                    MessageBox.Show("step 1");
                    gls.updGlmast(cs1, tcAcctNumb, tnTranAmt);                                       //update glmast posting account
                    decimal tnPNewBal = CheckLastBalance.lastbalance(cs1, tcAcctNumb);       //  0.00m;
                    //tls.updTranhist(cs1, tcAcctNumb, tnTranAmt, lcServ_name, tcVoucher, tcChqno, tcUserid, tnPNewBal, tcTranCodeP, lnServID, llPaid, tcContra, lnWaiveAmt, 1, tnTranAmt, tcReceipt, llCashpay, lisproduct,
                    //1, "", "", lFreeBee, tcCustcode, tncompid);                   //update tranhist posting account

                    /* we will revisit this for payment by cheque or by bank for both posting and contra account
                    lcTime = Time()
                    ln = SQLExec(gnConnHandle, "select stackcount from patient_code", "genresult")
                    If ln> 0 And Reccount() > 0
                    lcStack1 = Padl(Alltrim(Str(stackcount)), 12, '0')
                    Endif
                    if (radioButton3.Checked )       //        If.optiongroup3.Value = 2 && bank payment        
                    {
                        updatebankprovider(  .updatebankprovider(1, lcStack1, gnBankID)
                    }
                    if(radioButton5.Checked)
                    {
                        Thisform.updatebankprovider(0, lcStack1, gnProviderID)
                    }*/

  //                  MessageBox.Show("step 2");
                    gls.updGlmast(cs1, tcContra, tnContAmt);                                    //update glmast contra account
                    decimal tnCNewBal = CheckLastBalance.lastbalance(cs1, tcContra);         // 0.00m;
                    //tls.updTranhist(cs1, tcContra, tnContAmt, lcServ_name, tcVoucher, tcChqno, tcUserid, tnCNewBal, tcTranCodeC, lnServID, llPaid, tcAcctNumb, lnWaiveAmt, 1, tnTranAmt, tcReceipt, llCashpay, lisproduct,
                    //1, "", "", lFreeBee, tcCustcode, tncompid);                   //update tranhist account 396 1756
                }
     //           else { MessageBox.Show("We will deduct from the client's credit balance"); }
      //          MessageBox.Show("Step 3");
                updateaccounts();          // breakdown of the amounts are sent to different service centres
                if (maskedTextBox1.Text.ToString().Trim() != "")               //                If.text29.Value <> 0
                {
                    updinvoice(tcCustcode);
                    printreceipt(tcCustcode);
                    updatePatVisit(tcCustcode);
                }

                //****** This must be completed.
                //     Thisform.updatedashboard
                if (textBox5.Text.ToString().Trim() != "")               //   If gnWaiveAmount > 0.00
                {
                    //waivedetails(tcCustcode);
                    //              MessageBox.Show("Finished with waive details");
                    maskedTextBox1.Text = "";
                    textBox4.Text       = "";
                    textBox27.Text      = "";
                    transview.Clear();
                    getclientList();
                    firstclient();
                    clientgrid.Focus();
                    AllClear2Go();
                }
            }
        }       //end of savebutton


        private void updateaccounts()
        {
            string cs1 = globalvar.cos;
            string tcCustcode = textBox6.Text.ToString();
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs1))
            {
                if (transview.Rows.Count > 0)
                {
                    for (int i = 0; i < transview.Rows.Count; i++)
                    {
                        int lsrvid = Convert.ToInt16(transview.Rows[i]["srv_id"]);
                        decimal lnPostAmt = Math.Abs(Convert.ToDecimal(transview.Rows[i]["ntranamnt"]));       // Abs(ntranamnt) && credit is posted to income account
                        string lcServ_name = transview.Rows[i]["ctrandesc"].ToString();        // ctrandesc
                        string lcAcctNumb = transview.Rows[i]["rev_acct"].ToString();          // rev_acct                    
                        string gcVoucherNo = transview.Rows[i]["cvoucherno"].ToString();       // cvoucherno
                        string ProdCode = transview.Rows[i]["prod_code"].ToString();         // prod_code
                        string gcSrvCode = transview.Rows[i]["srv_code"].ToString();           // srv_code
                        string lcStack = transview.Rows[i]["cstack"].ToString();               //cstack
                        bool lisprod = Convert.ToBoolean(transview.Rows[i]["isproduct"]);       // isproduct &&if it is product, inventory = credit(lnPostAmt), cost of goods sold = debit(buy_price)
                        if (lisprod == true)                   // If lisprod
                        {
                            MessageBox.Show(" before prodservacctounts with product code "+ProdCode);
  //                          ProdServAccounts.ProductAccounts(cs1, gcProdCode);
                            postinventory(ProdCode,lnPostAmt, lcServ_name, lsrvid, tcCustcode); //          Thisform.postinventory(gcProdCode, lnPostAmt, lcServ_name, lsrvid)
                        }
                        else
                        {
 //                           MessageBox.Show(" before service accounts");
//                            ProdServAccounts.ServiceAccounts(cs1, gcSrvCode);
                            postservice(gcSrvCode,lnPostAmt,lcServ_name,lsrvid,tcCustcode);  // Thisform.postservice(gcSrvCode, lnPostAmt)
                        }
                    }
                }
            }
        }//end of updateaccounts

        private void postinventory(string tcProd, decimal tnAmt, string tcDesc, int tnsrvid,string tcCode)  //Parameters tcProd,tnAmt,tcdesc,tnsrvid
        {
            string cs1 = globalvar.cos;
            string tcUserid = globalvar.gcUserid;
            bool llpaid = true;
            bool llcashpay = true;
            int visno = gnVisno;
            bool lisproduct = true;
            bool lFreeBee = false;
            string tcCashAcct = globalvar.gcCashAccont;
            int tncompid = globalvar.gnCompid;
            string tcReceipt = genreceipt.getreceipt(cs1, globalvar.gdSysDate);
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs1))
            {
                SqlDataReader cprodread = null;
                SqlCommand cgetserv = new SqlCommand("select inc_acc,cog_acc,acc_pay,acc_rec,inv_acc,buy_price from products where prod_code   = @tcPro", ndConnHandle3);
                cgetserv.Parameters.Add("@tcPro", SqlDbType.Char).Value = tcProd;
                ndConnHandle3.Open();
                cprodread = cgetserv.ExecuteReader();
                cprodread.Read();
                if (cprodread.HasRows == true)
                {
                    string gcIncAcct = cprodread["inc_acc"].ToString();
                    string gcExpAcct = cprodread["cog_acc"].ToString();
                    string gcAccPay = cprodread["acc_pay"].ToString();
                    string gcAccRec = cprodread["acc_rec"].ToString();;
                    string gcInvAcc = cprodread["inv_acc"].ToString();
                    int lsrvid = tnsrvid;
                    decimal lnpostamt = Convert.ToDecimal(cprodread["buy_price"]);     // Abs(buy_price)
                    decimal lnContAmt = -Math.Abs(lnpostamt);
                    string lcServ_name = "Inventory Update " + tcDesc.Trim();           // Alltrim(lcServ_name)
                    string gcVoucherNo = genbill.genvoucher(cs1, globalvar.gdSysDate);

                updateGlmast gls = new updateGlmast();
                updateTranhist tls = new updateTranhist();

                    gls.updGlmast(cs1, gcInvAcc, lnpostamt);                                       //update glmast posting account
                    decimal tnPNewBal = CheckLastBalance.lastbalance(cs1, gcInvAcc);       
                    tls.updTranhist(cs1, gcInvAcc, lnpostamt, lcServ_name,gcVoucherNo,"000001" , tcUserid, tnPNewBal, "93",1, llpaid,
                        gcExpAcct,0.00m, 1, lnpostamt, tcReceipt,llcashpay, visno, lisproduct, 7, "",tcProd, lFreeBee, tcCode, tncompid);                   //update tranhist posting account

                    gls.updGlmast(cs1, gcExpAcct, lnContAmt);                                       //update cost of good sold account - debit
                    decimal tnCNewBal = CheckLastBalance.lastbalance(cs1, gcExpAcct);     
                    tls.updTranhist(cs1, gcExpAcct, lnContAmt, lcServ_name, gcVoucherNo, "000001", tcUserid, tnPNewBal, "92", 1, llpaid,
                        gcInvAcc, 0.00m, 1, lnContAmt, tcReceipt, llcashpay, visno, lisproduct, 7, "", tcProd, lFreeBee, tcCode, tncompid);                   //update tranhist posting account

                    gls.updGlmast(cs1, gcIncAcct, lnpostamt);                                       //&&Update Income account for product - Credit
                    decimal tnPNewBal1 = CheckLastBalance.lastbalance(cs1, gcIncAcct);       
                    tls.updTranhist(cs1, gcIncAcct, lnpostamt, lcServ_name, gcVoucherNo, "000001", tcUserid, tnPNewBal, "93", 1, llpaid,
                        tcCashAcct, 0.00m, 1, lnpostamt, tcReceipt, llcashpay, visno, lisproduct, 7, "", tcProd, lFreeBee, tcCode, tncompid);                   //update tranhist posting account

                    gls.updGlmast(cs1, tcCashAcct, lnContAmt);                                       //update glmast posting account
                    decimal tnCNewBal1 = CheckLastBalance.lastbalance(cs1, gcInvAcc);       //  0.00m;
                    tls.updTranhist(cs1, tcCashAcct, tnAmt, lcServ_name, gcVoucherNo, "000001", tcUserid, tnPNewBal, "92", 1, llpaid,
                        gcIncAcct, 0.00m, 1, tnAmt, tcReceipt, llcashpay, visno, lisproduct, 7, "", tcProd, lFreeBee, tcCode, tncompid);                   //update tranhist posting account

                }
            }
        }       //end of postinventory

        private void postservice(string tcSrvCode, decimal tnAmt, string tcDesc, int lnvid, string tcCode)
        {
            string cs1 = globalvar.cos;
            string tcUserid = globalvar.gcUserid;
            bool llpaid = true;
            bool llcashpay = true;
            int visno = gnVisno;
            bool lisproduct = false;
            bool lFreeBee = false;
            string tcCashAcct = globalvar.gcCashAccont;
            int tncompid = globalvar.gnCompid;
            string tcReceipt = genreceipt.getreceipt(cs1, globalvar.gdSysDate);
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs1))
            {
                SqlDataReader cservread= null;
                SqlCommand cgetserv = new SqlCommand("select inc_acc,exp_acc,acc_pay,acc_rec from servces where srv_code  = @tcSrv", ndConnHandle3);
                cgetserv.Parameters.Add("@tcSrv", SqlDbType.Char).Value = tcSrvCode;
                ndConnHandle3.Open();
                cservread = cgetserv.ExecuteReader();
                cservread.Read();
                if (cservread.HasRows == true)
                {
                    string gcIncAcct = cservread["inc_acc"].ToString();
                    string gcExpAcct = cservread["exp_acc"].ToString();
                    string gcAccPay  = cservread["acc_pay"].ToString();
                    string gcAccRec  = cservread["acc_rec"].ToString();
                    decimal lnpostamt = Math.Abs(tnAmt);
                    decimal lnContAmt = -Math.Abs(lnpostamt);
                    string lcServ_name =tcDesc.Trim();           
                    string gcVoucherNo = genbill.genvoucher(cs1, globalvar.gdSysDate);
                    ndConnHandle3.Close();
                    updateGlmast gls = new updateGlmast();
                    updateTranhist tls = new updateTranhist();

                    gls.updGlmast(cs1, gcIncAcct, lnpostamt);                                       
                    decimal tnPNewBal = CheckLastBalance.lastbalance(cs1, gcIncAcct);
                    tls.updTranhist(cs1, gcIncAcct, lnpostamt, lcServ_name, gcVoucherNo, "000001", tcUserid, tnPNewBal, "93", 1, llpaid,
                        tcCashAcct, 0.00m, 1, lnpostamt, tcReceipt, llcashpay, visno, lisproduct, 7, tcSrvCode, "", lFreeBee, tcCode, tncompid);               

                    gls.updGlmast(cs1, tcCashAcct, lnContAmt);                                     
                    decimal tnCNewBal = CheckLastBalance.lastbalance(cs1, tcCashAcct);
                    tls.updTranhist(cs1, tcCashAcct, lnContAmt, lcServ_name, gcVoucherNo, "000001", tcUserid, tnPNewBal, "92", 1, llpaid,
                        gcIncAcct, 0.00m, 1, lnContAmt, tcReceipt, llcashpay, visno, lisproduct, 7, tcSrvCode, "", lFreeBee, tcCode, tncompid);                  
                }
            }
        }       //end of postservice

        private void updatebankprovider()    //We will come back to this
        {
             /*
                 Parameters tnIsBank, tcStack,tnBnkPrvID
                 With Thisform
                 ln=SQLExec(gnConnHandle,"update tranhist set isbank=?tnIsBank,bnk_prv=?tnBnkPrvID where cstack=?tcStack","bnkresult")
                 If !(ln>0)
                 =sysmsg('Could not update bank/provider details, inform IT DEPT')
                 Else
                 =SQLExec(gnConnHandle,"update todayhist set isbank=?tnIsBank,bnk_prv=?tnBnkPrvID  where cstack=?tcStack","bnkresult")
                 Endif
                .Refresh
            */
            }       //end of updatebankprovider

        private void updinvoice(string tcCode)
        {
            string cs1 = globalvar.cos;
            string tcUserid = globalvar.gcUserid;
            int visno = gnVisno;
            string tcReceipt = genreceipt.getreceipt(cs1, globalvar.gdSysDate);
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs1))
            {
                string cpatquery = "exec tsp_UpdateBillOne @lcStack, @gcReceiptNo,@gcCustCode, @gnVisNo";
                if (transview.Rows.Count > 0)
                {
                    for (int i = 0; i < transview.Rows.Count; i++)
                    {
                        string lcStack = transview.Rows[i]["cstack"].ToString();
                        SqlDataAdapter glinsCommand = new SqlDataAdapter();
                        glinsCommand.UpdateCommand = new SqlCommand(cpatquery, ndConnHandle3);
                        glinsCommand.UpdateCommand.Parameters.Add("@lcStack", SqlDbType.Char).Value = lcStack;
                        glinsCommand.UpdateCommand.Parameters.Add("@gcReceiptNo", SqlDbType.Char).Value = tcReceipt;
                        glinsCommand.UpdateCommand.Parameters.Add("@gcCustCode", SqlDbType.Decimal).Value = tcCode;
                        glinsCommand.UpdateCommand.Parameters.Add("@gnVisNo", SqlDbType.Int).Value = visno;
                        ndConnHandle3.Open();
                        glinsCommand.UpdateCommand.ExecuteNonQuery();
                        ndConnHandle3.Close();
                    }
                }
            }
        }       //end of updinvoice

        private void printreceipt(string tcCode)
        {
            MessageBox.Show("We are in print receipt");
            
            /*
             *If .F.
gcReceiptHash=''
lnAmnt=Thisform.text3.Value
gcReceiptHash=receipthash(gcCustCode,lnAmnt)

sn=SQLExec(gnConnHandle,"Exec tsp_ClientReceipt ?gnCompid,?gcCustCode,?gcReceiptNo","ReceiptPrint")
If sn>0 And Reccount()>0
	gcReceiptTime=Time()
	Report Form "new_receipt.frx" To Print Noconsole
Else
	=Messagebox("No receipts found to print",0,"Seek check")
Endif
*Endif

If .F.
	sn=SQLExec(gnConnHandle,"Exec sp_ClientReceipt ?gnCompid,?gcCustCode,?gcReceiptNo","ReceiptPrint")
	If sn>0 And Reccount()>0
		gcReceiptTime=Time()
		Report Form "new_receipt.frx" Preview
	Else
		=sysmsg("No receipts found to print, inform IT Dept immediately")
	Endif
Endif
             */
        } //end of printreceipt

        private void updatePatVisit(string tcCode)
        {
            string cs1 = globalvar.cos;
            int ncompid = globalvar.gnCompid;
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs1))
            {
                string cpatquery = "EXEC tsp_UpdateVisitFlag @nCompID,@gcCustCode";    
                SqlDataAdapter glinsCommand = new SqlDataAdapter();
                glinsCommand.UpdateCommand = new SqlCommand(cpatquery, ndConnHandle3);
                glinsCommand.UpdateCommand.Parameters.Add("@nCompID", SqlDbType.Int).Value = ncompid;
                glinsCommand.UpdateCommand.Parameters.Add("@gcCustCode", SqlDbType.Char).Value = tcCode;
                ndConnHandle3.Open();
                glinsCommand.UpdateCommand.ExecuteNonQuery();
                ndConnHandle3.Close();
            }
        }           //end of updatePatVisit

        private void waivedetails(string tcCode, int lnVisno)
        {
            string cs1 = globalvar.cos;
            int ncompid = globalvar.gnCompid;
            string lcMacAddr = globalvar.gcMacAddress;
            string lcwkst = globalvar.gcWorkStation;
            string lcuser = globalvar.gcUserid;     
            decimal nwaiveAmt =Convert.ToDecimal(textBox5.Text);
            string lcWaiveRea = "";
            string lcWaiveUsr = "";
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs1))
            {
                string cpatquery = "insert into waiver (waive_Amount,Waive_reason,Waive_date,Waive_time,oprcode,compid,cpid,wkstat,winusr,ccustcode,visno) " ;
                cpatquery +=  "values (@gnWaiveAmount,@gcWaiveReason,convert(date,getdate()),convert(time,getdate()),@gcWaiveUser,@nCompid,@lcMacAddr,@gcWkSta,@lcuser,@gcCustCode,@gnVisNo)";
                SqlDataAdapter waiveCommand = new SqlDataAdapter();
               waiveCommand.InsertCommand = new SqlCommand(cpatquery, ndConnHandle3);
                waiveCommand.InsertCommand.Parameters.Add("@gnWaiveAmount", SqlDbType.Decimal).Value = nwaiveAmt; 
                waiveCommand.InsertCommand.Parameters.Add("@gcWaiveReason", SqlDbType.VarChar).Value = lcWaiveRea;
                waiveCommand.InsertCommand.Parameters.Add("@ncompid", SqlDbType.Int).Value = globalvar.gnCompid;
                waiveCommand.InsertCommand.Parameters.Add("@gcWaiveUser", SqlDbType.VarChar).Value = lcWaiveUsr;
                waiveCommand.InsertCommand.Parameters.Add("@lcMacAddr", SqlDbType.VarChar).Value = lcMacAddr;
                waiveCommand.InsertCommand.Parameters.Add("@gcWkSta", SqlDbType.VarChar).Value = lcwkst;
                waiveCommand.InsertCommand.Parameters.Add("@lcuser", SqlDbType.VarChar).Value = lcuser ;
                waiveCommand.InsertCommand.Parameters.Add("@gcCustCode", SqlDbType.Char).Value = tcCode;
                waiveCommand.InsertCommand.Parameters.Add("@gnVisNo", SqlDbType.Int).Value = lnVisno;
                ndConnHandle3.Open();
                waiveCommand.InsertCommand.ExecuteNonQuery();
                ndConnHandle3.Close();
            }
            /*
             lcMacAddress=CODEC(gcMacAddress)
             */
        }       //end of waivedetails
    }
}
