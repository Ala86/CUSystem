using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Data.SqlClient;
using TclassLibrary;

namespace WinTcare
{
    public partial class Triage : Form
    {
        public string tcCustCode = "";
        int gnVisno = 0;
        DataTable clientview = new DataTable();
        DataTable TempAllergy = new DataTable();
        DataTable pharmaitems = new DataTable();
        int currentPharmaRow;
        int currentPharmaCol;
        string cs = globalvar.cos;
        string ncompid = globalvar.gnCompid.ToString().Trim();
        string gcFirstName = "";
        string gcMidName = "";
        string gcLastName = "";

        DateTime gdVisDate = new DateTime(); //     Convert.ToDateTime("  /  /    ");

        public Triage()
        {
            InitializeComponent();
        }

        private void Triage_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + "<< Triage >>" + globalvar.gcFormCaption;
            maskedTextBox1.Mask = @"000\/00";
            maskedTextBox2.Mask = @"00\.00";
            KeyPreview = true;
            getclientList(globalvar.gnQueueID);
            getdruglist();
            getallergy();
            firstclient();
            clientgrid.Focus();
        }

        private void getclientList(int qid)
        {
            string ncompid = globalvar.gnCompid.ToString().Trim();
            string dsql = "exec tsp_Normal_Triage  " + ncompid + "," + qid;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(clientview);
                if (clientview.Rows.Count > 0)
                {
                    clientgrid.AutoGenerateColumns = false;
                    clientgrid.DataSource = clientview.DefaultView;
                    clientgrid.Columns[0].DataPropertyName = "fname";
                    clientgrid.Columns[1].DataPropertyName = "mname";
                    clientgrid.Columns[2].DataPropertyName = "lname";
                    clientgrid.Columns[3].DataPropertyName = "visdate";      // "age";
                    clientgrid.Columns[4].DataPropertyName = "vistime";
                    clientgrid.Columns[5].DataPropertyName = "visno";
                    ndConnHandle.Close();
                    clientgrid.Focus();
                    for (int i = 0; i < 6; i++)
                    {
                        //                        DataGridViewRow drow = new  DataGridViewRow();
                        clientview.Rows.Add();
                    }
                }
            }
        }//end of getclientlist

        private void firstclient()
        {
            if(clientview.Rows.Count>0)
            {
                DataRow drow = clientview.Rows[clientgrid.CurrentCell.RowIndex];
                textBox4.Text = drow["ccustcode"].ToString();
                gnVisno = Convert.ToInt16(drow["visno"]);
                gcFirstName = drow["fname"].ToString();
                gcMidName =  drow["mname"].ToString();
                gcLastName = drow["lname"].ToString();
            }
        }

        private void getCurrentVisitDate(string tcCode)
        {
            string ncompid = globalvar.gnCompid.ToString().Trim();
            string dsql = "exec sp_getCurrentVisit  " + ncompid+",'"+tcCode+"'";
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                DataTable vtable = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(vtable);
                if (vtable.Rows.Count > 0)
                {
                    gdVisDate = Convert.ToDateTime(vtable.Rows[0]["visdate"]);
//                    MessageBox.Show("The gdvisdate is " + gdVisDate);
                }
            }
        }

        private void getdruglist()
        {
            string ncompid = globalvar.gnCompid.ToString().Trim();
            string dsql = "exec tsp_TriageInStock  " + ncompid;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(pharmaitems);
                if (pharmaitems.Rows.Count > 0)
                {
                    //                    gnVisno = Convert.ToInt16(ds.Rows["visno"]);
                    PharmaGrid.AutoGenerateColumns = false;
                    PharmaGrid.DataSource = pharmaitems.DefaultView;
                    PharmaGrid.Columns[0].DataPropertyName = "prod_name";
                    PharmaGrid.Columns[1].DataPropertyName = "quantity";
                    PharmaGrid.Columns[2].DataPropertyName = "qty";      // "age";
                    PharmaGrid.Columns[3].DataPropertyName = "Perday";      // "age";
                    PharmaGrid.Columns[4].DataPropertyName = "Days";
                    PharmaGrid.Columns[5].DataPropertyName = "Issue";
                    PharmaGrid.Columns[6].DataPropertyName = "cashpay";
                    PharmaGrid.Columns[7].DataPropertyName = "Amount";
                    PharmaGrid.Columns[8].DataPropertyName = "dosage";
                    PharmaGrid.Columns[9].DataPropertyName = "notes";
                    PharmaGrid.Columns[10].DataPropertyName = "prod_code";
                    ndConnHandle.Close();
                    clientgrid.Focus();
                    for (int i = 0; i < 3; i++)
                    {
                        pharmaitems.Rows.Add();
                    }
                }
            }
        }

        private void getallergy()
        {
            string dsql = "exec sp_GetAllergy  " + ncompid;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(TempAllergy);
                if (TempAllergy.Rows.Count > 0)
                {
                    AllergyGrid.AutoGenerateColumns = false;
                    AllergyGrid.DataSource = TempAllergy.DefaultView;
                    AllergyGrid.Columns[0].DataPropertyName = "all_name";
                    AllergyGrid.Columns[1].DataPropertyName = "selserv";
                    AllergyGrid.Columns[2].DataPropertyName = "all_id";
                    ndConnHandle.Close();
                    clientgrid.Focus();
                    for (int i = 0; i < 2; i++)
                    {
                        TempAllergy.Rows.Add();
                    }
                }
            }
        }//end of getallergy


        
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Down)
            {
                SelectNextControl(ActiveControl, true, true, true, true);
                e.Handled = true;
                AllClear2Go();
            }
            else if (e.KeyCode == Keys.Up)
            {
                SelectNextControl(ActiveControl, false, true, true, true);
                e.Handled = true;
                AllClear2Go();
            }
        }
        

        private void caclbmi()
        {
            if (textBox7.Text.Trim() != "" && textBox12.Text.Trim() != "")
            {
                label4.Visible = true;
                double lnumer = Convert.ToDouble(textBox12.Text);
                double ldeno = Math.Round(Math.Pow(Convert.ToDouble(textBox7.Text) / 100, 2), 1);
                double bmi = lnumer / ldeno;
                textBox15.Text = (bmi > 29.9 ? "30" : bmi.ToString());
                label4.Text = (bmi < 18.5 ? "Underweight" : (bmi <= 24.9 ? "Normal Weight" : (bmi <= 29.9 ? "Overweight" : "Obese")));

            }
            else { label4.Visible = false; }
        }

        private void AllClear2Go()
        {
            if (textBox12.Text.Trim() != "" && maskedTextBox1.Text.ToString().Trim() != "" && maskedTextBox2.Text.Replace(".", "").Trim() != "")
            {
                SaveButton.Enabled = true;
                SaveButton.BackColor = Color.LawnGreen;
            }
            else
            {
                SaveButton.Enabled = false;
                SaveButton.BackColor = Color.FromArgb(224, 224, 224);        // Color.Red;
            }

        }

  

        private void drugsum()
        {
            /*
                 Select pharmaitems
                Replace seldrug With .F.
                Store 0.00 To gnProdCost,gntotalcost,lnIssue
                gcProdCode=pharmaitems.prod_code
                glTabsCaps=Iif(pharmaitems.t_caps,.T.,.F.)
                gnInStock=available
                lcalias=Alias()
                lnRecNo=Recno()
                llcov=pharmaitems.lcovered
                glDrugCovered=pharmaitems.lcovered
                lncash=pharmaitems.cashpay
                gnProdCost=pharmaitems.unit_price		&& lncash
                lcProdName=pharmaitems.prod_name
                Select(lcalias)
                If glTabsCaps
	                lnIssue=unit_meas*Formula*perday
                Else
	                lnIssue=unit_meas						&&*Formula*perday
                Endif
                lnUnitMeas=unit_meas
                lnFormula=Formula
                lnPerDay=perday
                gcDosage=Alltrim(Dosage)
                If lnIssue>0.00
	                If  lnIssue > gnInStock
		                =sysmsg("Cannot be more than available stock")
		                Replace Formula With 0
		                Replace unit_meas With 0
		                Replace unit_price With 0.00
		                Replace tot_amt With 0.00
		                Replace quantity With 0
		                Replace perday With 0
	                Else
		                Replace quantity With lnIssue
		                Replace tot_amt With quantity*gnProdCost
		                Replace seldrug With .T.
		                Replace unit_price With gnProdCost
                *		Replace lfreebee With llfreebee

		                lcNalias=Alias()
		                Thisform.seldrugupdate
		                Select (lcNalias)
	                Endif
	                Count For seldrug To m
	                glPayDrugSelected=Iif(m>0.00,.T.,.F.)

	                Select 1 From dexclusions Where prod_code=gcProdCode Into Cursor myexcls
	                If _Tally>0
		                llfreebee=.F.
	                *	Wait Window 'This drug must be paid for'
	                Else
		                llfreebee=.T.
	                Endif
	                Update pharmaitems Set lfreebee=llfreebee Where prod_code=gcProdCode
                Else
	                Delete From selDrugItems Where prod_code=gcProdCode 		&&delete from selected files
	                Select selDrugItems
	                Locate
	                sn=SQLExec(gnConnHandle,"delete from drug_dispense_bkp  where ccustcode = ?gcCustCode and visno=?gnVisNo and prod_code=?gcProdCode and sess_id=?gnSessionID","")
	                If !(sn>0)
		                =sysmsg('Could not remove from temp backup file, inform IT DEPT')
	                Endif

	                If unit_meas=0
		                Replace quantity With 0
		                Replace perday With 0
		                Replace Formula With 0
		                Replace tot_amt With 0.00
	                Endif
                Endif
                *SELECT pharmaitems
                *brow
                Select(lcalias)
                Goto lnRecNo
                Thisform.Refresh

             */
        }

        private void seldrugupdate()
        {
            /*
             Select * From selDrugItems Where prod_code=gcProdCode Into Cursor mydrugs
If !_Tally>0
	Insert Into selDrugItems ;
		(unit_price,prod_code,prod_name,unit_meas,quantity,formula,perday,dosage,seldrug,lcov) ;
		VALUES (gnProdCost,gcProdCode,lcProdName,lnUnitMeas,lnIssue,lnFormula,lnPerDay,gcDosage,.T.,glDrugCovered)

*************update temporary file*****************************************
	sn=SQLExec(gnConnHandle,"select 1 from drug_dispense_bkp  where ccustcode = ?gcCustCode and visno=?gnVisNo and prod_code=?gcProdCode and sess_id=?gnSessionID","")
	If !(sn>0 And Reccount()>0)
		fn=SQLExec(gnConnHandle,"Insert Into drug_dispense_bkp (unitprice,prod_code,unitmeas,quantity,cformula,perday,dosage,ccustcode,visno,sess_id,lcovered) "+;
			" values (?gnProdCost,?gcProdCode,?lnUnitMeas,?lnIssue,?lnFormula,?lnPerDay,?gcDosage,?gcCustCode,?gnVisNo,?gnSessionID,?llcov)","lprovw1")
		If !(fn>0)
			=sysmsg('Could not insert into temp file, inform IT DEPT')
		Endif
	Endif
Else
	Select selDrugItems
	Replace prod_name With lcProdName,unit_meas With lnUnitMeas,quantity With lnIssue,formula With lnFormula,;
		perday With lnPerDay,dosage With gcDosage,lcov With llcov For prod_code=gcProdCode
Endif

With Thisform.pageframe1.page4.selPres
	Select selDrugItems
	Sum All quantity*unit_price  For lcov To gnPreCoverPayment
	Sum All quantity*unit_price  For !lcov To gnPreCashPayment
	Locate
	.RecordSource = 'selDrugItems'
	.column1.ControlSource = 'prod_name'
	.column2.ControlSource = 'quantity'
	.column3.ControlSource = 'seldrug'
Endwith
Thisform.Refresh
*/
        }
        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void textBox7_Validated(object sender, EventArgs e)
        {
            caclbmi();
        }

        private void textBox12_Validated(object sender, EventArgs e)
        {
            caclbmi();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            int llEmer = (checkBox1.Checked ? 1 : 0);
            string tcCustCode = textBox4.Text.Trim().ToString();
            //gnVisno = getVisitNumber.visitno(cs,ncompid, tcCustCode);
            getCurrentVisitDate(tcCustCode);

            string sql0 = "update pat_visit set ltriage=1,P_height=@lheight,P_weight=@lweight,P_bmi=@bmi,P_resp=@resp,P_hip=@hip,C_pressure=@press,P_pulse=@pulse,P_temp=@temp,P_waist=@waist,emercase=@emer,";
            sql0 += "triagedate=convert(date,getdate()),triagetime=convert(time,getdate()),clinician=@intdoc where ccustcode=@tcCust and activesession=1";

            string sql1 = "update todayvisit set ltriage=1,P_height=@lheight,P_weight=@lweight,P_bmi=@bmi,P_resp=@resp,P_hip=@hip,C_pressure=@press,P_pulse=@pulse,P_temp=@temp,P_waist=@waist,emercase=@emer,";
            sql1 += "triagedate=convert(date,getdate()),triagetime=convert(time,getdate()),clinician=@intdoc where ccustcode=@tcCust and activesession=1";

            string sql2 = "insert into triage (triagedate,triagetime,emercase,ccustcode,p_hip,c_pressure,p_pulse,p_temp,p_waist,p_height,p_weight,p_bmi,p_resp,compid,visno,dr_id) ";
            sql2 +="values (convert(date,getdate()),convert(time,getdate()),@Emer,@tcCust,@hip,@press,@pulse,@temp,@waist,@lheight,@lweight,@bmi,@resp,@gnCompid,@gnVisno,@intDoc)";

            string sql3 = "delete from drug_dispense_bkp where ccustcode = @gcCustCode and visno=@gnVisNo";

            SqlDataAdapter updvisit = new SqlDataAdapter();
            SqlDataAdapter updvisit1 = new SqlDataAdapter();
            SqlDataAdapter updvisit2 = new SqlDataAdapter();
            SqlDataAdapter updvisit3 = new SqlDataAdapter();

            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
 
                updvisit.UpdateCommand = new SqlCommand(sql0, ndConnHandle);
                updvisit1.UpdateCommand = new SqlCommand(sql1, ndConnHandle);
                updvisit2.InsertCommand = new SqlCommand(sql2, ndConnHandle);
                updvisit3.DeleteCommand = new SqlCommand(sql3, ndConnHandle);
 

                updvisit.UpdateCommand.Parameters.Add("@lheight", SqlDbType.Decimal).Value =Convert.ToDecimal(textBox7.Text);
                updvisit.UpdateCommand.Parameters.Add("@lweight", SqlDbType.Decimal).Value = Convert.ToDecimal(textBox12.Text);
                updvisit.UpdateCommand.Parameters.Add("@bmi", SqlDbType.Decimal).Value = Convert.ToDecimal(textBox15.Text);
                updvisit.UpdateCommand.Parameters.Add("@press", SqlDbType.Char).Value = maskedTextBox1.Text.ToString(); // textBox25.Text.ToString();
                updvisit.UpdateCommand.Parameters.Add("@temp", SqlDbType.Decimal).Value = Convert.ToDecimal(maskedTextBox2.Text.ToString());
                updvisit.UpdateCommand.Parameters.Add("@resp", SqlDbType.Decimal).Value =(textBox16.Text.Trim().ToString()!="" ? Convert.ToDecimal(textBox16.Text) :0.00m);
                updvisit.UpdateCommand.Parameters.Add("@hip", SqlDbType.Decimal).Value = (textBox21.Text.Trim().ToString()!="" ? Convert.ToDecimal(textBox21.Text) :0.00m);
                updvisit.UpdateCommand.Parameters.Add("@pulse", SqlDbType.Decimal).Value = (textBox24.Text.Trim().ToString() != "" ? Convert.ToDecimal(textBox24.Text) : 0.00m); 
                updvisit.UpdateCommand.Parameters.Add("@waist", SqlDbType.Decimal).Value = (textBox22.Text.Trim().ToString() != "" ? Convert.ToDecimal(textBox22.Text) : 0.00m); 
                updvisit.UpdateCommand.Parameters.Add("@emer", SqlDbType.Int).Value = llEmer;
                updvisit.UpdateCommand.Parameters.Add("@intdoc", SqlDbType.Int).Value =1;
                updvisit.UpdateCommand.Parameters.Add("@tcCust", SqlDbType.Char).Value = tcCustCode ;
 

                updvisit1.UpdateCommand.Parameters.Add("@lheight", SqlDbType.Decimal).Value = Convert.ToDecimal(textBox7.Text);
                updvisit1.UpdateCommand.Parameters.Add("@lweight", SqlDbType.Decimal).Value = Convert.ToDecimal(textBox12.Text);
                updvisit1.UpdateCommand.Parameters.Add("@bmi", SqlDbType.Decimal).Value = Convert.ToDecimal(textBox15.Text);
                updvisit1.UpdateCommand.Parameters.Add("@press", SqlDbType.Char).Value = maskedTextBox1.Text.ToString();// textBox25.Text.ToString();
                updvisit1.UpdateCommand.Parameters.Add("@temp", SqlDbType.Decimal).Value = Convert.ToDecimal(maskedTextBox2.Text.ToString());
                updvisit1.UpdateCommand.Parameters.Add("@resp", SqlDbType.Decimal).Value = (textBox16.Text.Trim().ToString() != "" ? Convert.ToDecimal(textBox16.Text) : 0.00m);
                updvisit1.UpdateCommand.Parameters.Add("@hip", SqlDbType.Decimal).Value = (textBox21.Text.Trim().ToString() != "" ? Convert.ToDecimal(textBox21.Text) : 0.00m);
                updvisit1.UpdateCommand.Parameters.Add("@pulse", SqlDbType.Decimal).Value = (textBox24.Text.Trim().ToString() != "" ? Convert.ToDecimal(textBox24.Text) : 0.00m);
                updvisit1.UpdateCommand.Parameters.Add("@waist", SqlDbType.Decimal).Value = (textBox22.Text.Trim().ToString() != "" ? Convert.ToDecimal(textBox22.Text) : 0.00m);
                updvisit1.UpdateCommand.Parameters.Add("@emer", SqlDbType.Int).Value = llEmer;
                updvisit1.UpdateCommand.Parameters.Add("@intdoc", SqlDbType.Int).Value = 1;
                updvisit1.UpdateCommand.Parameters.Add("@tcCust", SqlDbType.Char).Value = tcCustCode;
 

                updvisit2.InsertCommand.Parameters.Add("@lheight", SqlDbType.Decimal).Value = Convert.ToDecimal(textBox7.Text);
                updvisit2.InsertCommand.Parameters.Add("@lweight", SqlDbType.Decimal).Value = Convert.ToDecimal(textBox12.Text);
                updvisit2.InsertCommand.Parameters.Add("@bmi", SqlDbType.Decimal).Value = Convert.ToDecimal(textBox15.Text);
                updvisit2.InsertCommand.Parameters.Add("@press", SqlDbType.Char).Value = maskedTextBox1.Text.ToString();// textBox25.Text.ToString();
                updvisit2.InsertCommand.Parameters.Add("@temp", SqlDbType.Decimal).Value = Convert.ToDecimal(maskedTextBox2.Text.ToString());
                updvisit2.InsertCommand.Parameters.Add("@resp", SqlDbType.Decimal).Value = (textBox16.Text.Trim().ToString() != "" ? Convert.ToDecimal(textBox16.Text) : 0.00m);
                updvisit2.InsertCommand.Parameters.Add("@hip", SqlDbType.Decimal).Value = (textBox21.Text.Trim().ToString() != "" ? Convert.ToDecimal(textBox21.Text) : 0.00m);
                updvisit2.InsertCommand.Parameters.Add("@pulse", SqlDbType.Decimal).Value = (textBox24.Text.Trim().ToString() != "" ? Convert.ToDecimal(textBox24.Text) : 0.00m);
                updvisit2.InsertCommand.Parameters.Add("@waist", SqlDbType.Decimal).Value = (textBox22.Text.Trim().ToString() != "" ? Convert.ToDecimal(textBox22.Text) : 0.00m);
                updvisit2.InsertCommand.Parameters.Add("@emer", SqlDbType.Int).Value = llEmer;
                updvisit2.InsertCommand.Parameters.Add("@intdoc", SqlDbType.Int).Value = 1;
                updvisit2.InsertCommand.Parameters.Add("@tcCust", SqlDbType.Char).Value = tcCustCode;
                updvisit2.InsertCommand.Parameters.Add("@gnVisNo", SqlDbType.Int).Value = gnVisno;
                updvisit2.InsertCommand.Parameters.Add("@gnCompid", SqlDbType.Int).Value = globalvar.gnCompid;

                updvisit3.DeleteCommand.Parameters.Add("@gcCustCode", SqlDbType.Char).Value = tcCustCode ;
                updvisit3.DeleteCommand.Parameters.Add("@gnVisNo", SqlDbType.Char).Value = gnVisno ;


                ndConnHandle.Open();
                updvisit.UpdateCommand.ExecuteNonQuery();
                updvisit1.UpdateCommand.ExecuteNonQuery();
                updvisit2.InsertCommand.ExecuteNonQuery();
                updvisit3.DeleteCommand.ExecuteNonQuery();
                ndConnHandle.Close();
       
                drugdispense();
                updateallergy();

                clientview.Clear();
                pharmaitems.Clear();
                TempAllergy.Clear();
//                AllergyGrid.ClearSelection();
                textBox7.Text = "";
                textBox12.Text = "";
                textBox15.Text = "";
                maskedTextBox1.Text = "";
                maskedTextBox2.Text = "";
                textBox24.Text = "";
                textBox16.Text = "";
                textBox21.Text = "";
                textBox22.Text = "";

                getclientList(globalvar.gnQueueID);
                getdruglist();
                firstclient();
                getallergy();
                clientgrid.Focus();
                SaveButton.Enabled = false;
                SaveButton.BackColor = Color.Gainsboro;
            }
        }

        private void drugdispense()               //             .drugdispense
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string gcVoucherNo = genbill.genvoucher(cs, globalvar.gdSysDate);
                string gcReceiptNo = "";
                string tcuserid = globalvar.gcUserid;
                int ncompid = globalvar.gnCompid;
                string tcCode = textBox4.Text;
                //int nvisno = Convert.ToInt16(getVisitNumber.visitno(cs, ncompid.ToString(), textBox4.Text));
                string tcAcctNumb = globalvar.ClientAcctPrefix + textBox4.Text.ToString();
                string tcContra = globalvar.gcIntSuspense;
                string tcContCode = globalvar.gcIntSuspense.Substring(3, 6);
                bool lisproduct = true;

                string cquery = "insert into drug_dispense (prod_code,visno,visdate,quantity,UnitMeas,UnitPrice,cFormula,compid,ccustcode,dispensed,perday,lemer,notes)";
                cquery += "values (@lcProdCode,@gnVisNo,@gdvisdate,@lnIssue,@lnUnitMeas,@lnUnitPrice,@lcFormula,@gnCompid,@gcCustCode,1,@lnPerday,1,@dnotes)";
                SqlDataAdapter drugupd = new SqlDataAdapter();
                drugupd.InsertCommand = new SqlCommand(cquery, ndConnHandle);
                foreach (DataGridViewRow drow in PharmaGrid.Rows)
                {
                    decimal damt = (drow.Cells["issueAmt"].Value != null ? Convert.ToInt16(drow.Cells["issueAmt"].Value) : 0.00m);

                    if (drow.Cells["issueAmt"].Value != null && Convert.ToInt16(drow.Cells["issueAmt"].Value) > 0)
                    {
                        string lcprodname = drow.Cells["prod_name"].Value.ToString();
                        string lcProdCode = drow.Cells["prod_code"].Value.ToString(); // drow.Cells[10].ToString();
                        decimal lnUnitPrice = (Convert.ToDecimal(drow.Cells["unitPrice"].Value.ToString()) > 0.00m ? Convert.ToDecimal(drow.Cells["unitPrice"].Value) : 0.00m);
                        string ndays = drow.Cells["dDays"].Value.ToString();
                        int lnPerday = Convert.ToInt16(drow.Cells["perday"].Value.ToString());
                        int lnQty = Convert.ToInt16(drow.Cells["qty"].Value.ToString());
                        int nIssue = Convert.ToInt16(drow.Cells["issueAmt"].Value.ToString());
                        // bool llCashPay = false;     //we will revisit this (!gl2Bouquets ? true :()Iif(lcovered, 0, 1))
                        bool llCashPay = true;// (Convert.ToBoolean(drow.Cells["lCovered"]) ? false : true);
                        bool lFreebee = false;      // drow["lfreebee"]
                        bool llpaid = false;
                        string cnotes = drow.Cells["ddosage"].ToString();
                        drugupd.InsertCommand.Parameters.Add("@lcProdCode", SqlDbType.Char).Value = lcProdCode;
                        //drugupd.InsertCommand.Parameters.Add("@gnVisNo", SqlDbType.Int).Value = nvisno;
                        drugupd.InsertCommand.Parameters.Add("@gdvisdate", SqlDbType.DateTime).Value = gdVisDate;
                        drugupd.InsertCommand.Parameters.Add("@lnIssue", SqlDbType.Int).Value = nIssue;
                        drugupd.InsertCommand.Parameters.Add("@lnUnitMeas", SqlDbType.Int).Value = lnQty;
                        drugupd.InsertCommand.Parameters.Add("@lnUnitPrice", SqlDbType.Decimal).Value = lnUnitPrice;
                        drugupd.InsertCommand.Parameters.Add("@lcFormula", SqlDbType.VarChar).Value = ndays;
                        drugupd.InsertCommand.Parameters.Add("@gnCompid", SqlDbType.Int).Value = ncompid;
                        drugupd.InsertCommand.Parameters.Add("@lnPerday", SqlDbType.Int).Value = lnPerday;
                        drugupd.InsertCommand.Parameters.Add("@gcCustCode", SqlDbType.Char).Value = tcCode;
                        drugupd.InsertCommand.Parameters.Add("@dnotes", SqlDbType.Char).Value = cnotes;

                        ndConnHandle.Open();
                        drugupd.InsertCommand.ExecuteNonQuery();
                        drugupd.InsertCommand.Parameters.Clear();

                        int lnServID = 0;                           //  lnProdID;
                        decimal lnPrice = nIssue * lnUnitPrice;     //  total issued X unit price
                        string lcServName = lcprodname.ToUpper();   //  Upper(Alltrim(prod_name)) && Iif(gnRes_type = 1, fee_loc, Iif(gnRes_type = 2, fee_exp, fee_for))
                        decimal lnPostAmt = -Math.Abs(lnPrice);
                        decimal lnContAmt = Math.Abs(lnPrice);

                        updateGlmast gls = new updateGlmast();
                        updateTranhist tls = new updateTranhist();

                        gls.updGlmast(cs, tcAcctNumb, lnPostAmt);                              //   update glmast posting account
                        decimal tnPNewBal = CheckLastBalance.lastbalance(cs, tcAcctNumb);      //   0.00m;
                        tls.updTranhist(cs, tcAcctNumb, lnPostAmt, lcServName, gcVoucherNo, "000001", tcuserid, tnPNewBal, "93", lnServID, llpaid, tcContra, 0.00m, 1, lnPostAmt, gcReceiptNo, llCashPay, gnVisno, lisproduct,
                        7, "", lcProdCode, lFreebee, tcCode, ncompid);                         //   update tranhist posting account

                        gls.updGlmast(cs, tcContra, lnContAmt);                               //    update glmast posting account
                        decimal tnCNewBal = CheckLastBalance.lastbalance(cs, tcContra);       //    0.00m;
                        tls.updTranhist(cs, tcContra, lnContAmt, lcServName, gcVoucherNo, "000001", tcuserid, tnCNewBal, "92", lnServID, llpaid, tcAcctNumb, 0.00m, 1, lnContAmt, gcReceiptNo, llCashPay, gnVisno, lisproduct,
                        7, "", lcProdCode, lFreebee, tcContCode, ncompid);                        //    update tranhist posting account
                        ndConnHandle.Close();
                    }
                }
                ndConnHandle.Close();
            }
        }


        private void updatedrug(string tcProd,int qty)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                SqlCommand upddisp = new SqlCommand("exec tsp_UpdateDrugDispensed " + tcProd + "," + qty + ","+5, ndConnHandle);
                ndConnHandle.Open();
                upddisp.ExecuteNonQuery();
                ndConnHandle.Close();
            }
        }
        private void updateallergy()
        {
            string cquery = "insert into cl_allergy (ccustcode,visno,visdate,all_id,compid) values (@gcCustCode,@gnVisNo,@gdSysDate,@lnAllID,@gnCompid)";
            SqlDataAdapter alladp = new SqlDataAdapter();
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                 alladp.InsertCommand = new SqlCommand(cquery, ndConnHandle);
                foreach(DataGridViewRow darow in AllergyGrid.Rows)
                {
                   string drowcont = (darow.Cells[0].Value.ToString()!=null ? darow.Cells[0].Value.ToString().Trim() : "Nothing");
                    if (drowcont != null && drowcont != string.Empty)
                    {
                        if (darow.Cells["alSelect"].Value != null && Convert.ToBoolean(darow.Cells["alSelect"].Value) == true)
                        {
                            int lnAllID = (darow.Cells["all_id"].Value != null && Convert.ToInt32(darow.Cells["all_id"].Value) > 0 ? Convert.ToInt32(darow.Cells["all_id"].Value) : 0);
                            alladp.InsertCommand.Parameters.Add("@gcCustCode", SqlDbType.Char).Value = textBox4.Text;
                            alladp.InsertCommand.Parameters.Add("@gnVisNo", SqlDbType.Int).Value = gnVisno;
                            alladp.InsertCommand.Parameters.Add("@gdSysDate", SqlDbType.DateTime).Value = gdVisDate;
                            alladp.InsertCommand.Parameters.Add("@lnAllID", SqlDbType.Int).Value = lnAllID;
                            alladp.InsertCommand.Parameters.Add("@gnCompid", SqlDbType.Int).Value = ncompid;
                            ndConnHandle.Open();
                            alladp.InsertCommand.ExecuteNonQuery();
                            alladp.InsertCommand.Parameters.Clear();
                            ndConnHandle.Close();
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                ndConnHandle.Close();
            }
        }//end of update allergy

        private void clientgrid_Click(object sender, EventArgs e)
        {
            DataRow drow = clientview.Rows[clientgrid.CurrentCell.RowIndex];
            textBox4.Text = drow["ccustcode"].ToString();
            gnVisno = Convert.ToInt16(drow["visno"]);
//            gnVisno = getVisitNumber.visitno(globalvar.cos, globalvar.gnCompid.ToString(), textBox4.Text);
            tcCustCode = textBox4.Text;                       //          clientgrid.Rows[e.RowIndex].Cells[3].Value.ToString();
    //       gnVisno = Convert.ToInt16(drow["visno"]);       //          clientgrid.Rows[e.RowIndex].Cells[5].Value.ToString());
    //       int newvisno = Convert.ToInt16(clientgrid.CurrentRow.Cells["visno"].Value);
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void PharmaGrid_KeyDown(object sender, KeyEventArgs e)
        {
   //         MessageBox.Show("In pharmagrid key down at position" + currentRow + "," + currentCell);
           if(e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
            }
            //       DataRow drow = clientview.Rows[clientgrid.CurrentCell.RowIndex];
            //     textBox4.Text = drow["ccustcode"].ToString();

        }//pharmagrid-KEYDOWN

 

        private void PharmaGrid_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            int rowi = PharmaGrid.CurrentCell.RowIndex; ;
            int coli = PharmaGrid.CurrentCell.ColumnIndex;  //.CurrentCell.RowIndex; 


            currentPharmaRow = rowi;
            currentPharmaCol = coli;
            if (e.KeyCode == Keys.Enter)
            {
                MessageBox.Show("We are in the preview key down at old location " + rowi + "," + coli);
                MessageBox.Show("We are in the preview key down at new position "+currentPharmaRow+","+currentPharmaCol);
                if(PharmaGrid.CurrentRow.Index>0)
                {
     //               MessageBox.Show("we will go back one row to ");
                    currentPharmaRow --;
                }
                else
                {
       //             MessageBox.Show("we are in the first row");
                    currentPharmaRow = 0;
                    currentPharmaCol = 0;
                }
            }
            else
            {
             //   MessageBox.Show("You have another key previewkeydown with position " + rowi + "," + coli);
            }
        }

        private void PharmaGrid_Click(object sender, EventArgs e)
        {
      //      MessageBox.Show("You have clicked on the grid");
        }

        private void PharmaGrid_ColumnStateChanged(object sender, DataGridViewColumnStateChangedEventArgs e)
        {
       //     MessageBox.Show("Column state changed event");
        }

        private void PharmaGrid_CurrentCellChanged(object sender, EventArgs e)
        {
       //     MessageBox.Show("Current cell changed event");
        }

        private void PharmaGrid_Enter(object sender, EventArgs e)
        {
          //  MessageBox.Show("pharmagrid enter event");
        }

        private void PharmaGrid_Leave(object sender, EventArgs e)
        {
        //    MessageBox.Show("pharmagrid leave event");
        }

        private void PharmaGrid_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
      //      MessageBox.Show("pharmagrid rowEnter event");
        }

        private void PharmaGrid_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
    //        MessageBox.Show("pharmagrid rowLeave event");
        }

        private void PharmaGrid_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
       //     MessageBox.Show("pharmagrid row state changed event");
        }

        private void PharmaGrid_SelectionChanged(object sender, EventArgs e)
        {
        }

        private void PharmaGrid_Validated(object sender, EventArgs e)
        {
   //         MessageBox.Show("Pharmagrid validated event");
        }

        private void PharmaGrid_KeyUp(object sender, KeyEventArgs e)
        {
            //{
                if (e.KeyCode == Keys.Enter)
            {
                PharmaGrid.CurrentCell = PharmaGrid[currentPharmaCol+1, currentPharmaRow];
                PharmaGrid.CurrentRow.Cells[5].Value =Convert.ToDecimal(PharmaGrid.CurrentRow.Cells[2].Value)*Convert.ToDecimal(PharmaGrid.CurrentRow.Cells[3].Value)
                    *Convert.ToDecimal(PharmaGrid.CurrentRow.Cells[4].Value);
                PharmaGrid.CurrentRow.Cells[7].Value = Convert.ToDecimal(PharmaGrid.CurrentRow.Cells[5].Value) * Convert.ToDecimal(PharmaGrid.CurrentRow.Cells[6].Value);
            }
        }

        private void maskedTextBox2_Leave(object sender, EventArgs e)
        {
           int dlen = maskedTextBox2.Text.Trim().ToString().Length;
            string dpres = "";
            dpres = (dlen == 3 ? maskedTextBox2.Text + "00" : (dlen == 4 ? maskedTextBox2.Text + "0" : maskedTextBox2.Text));
            maskedTextBox2.Text = dpres;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string tcCode = textBox4.Text.Trim().ToString();
            tbscreen tbs = new tbscreen(tcCode,gcFirstName,gcMidName,gcLastName);
            tbs.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MedHistory dmed = new MedHistory(textBox4.Text.Trim().ToString());
            dmed.ShowDialog();
        }
    }
}
