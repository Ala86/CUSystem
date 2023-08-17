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
    public partial class CoverManagement : Form
    {
        DataTable clientview = new DataTable();
        DataTable bouqStatView = new DataTable();
        DataTable boudetview = new DataTable();
        bool glhasins = false;
        bool glhascor = false;
        bool glhasnhi = false;
        int gnCoverStatus = 0;
        string gcBouName = ""; 
        public CoverManagement()
        {
            InitializeComponent();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CoverManagement_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + "<< Cover Management >>";
      //      bouquetStatus();
            getclientList();
            firstclient();
            clientgrid.Focus();
        }

        private void getclientList()
        {
            string cs = globalvar.cos;
            string ncompid = globalvar.gnCompid.ToString().Trim();
            string dsql = "exec tsp_Ready4CoverConfirm_All   " + ncompid;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
       //         DataTable clientview = new DataTable();
                da.Fill(clientview);
                int colcount = clientview.Columns.Count;
//                MessageBox.Show("Number of columns " + colcount);
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
                    clientgrid.Columns[6].DataPropertyName = "clientcode";
                    ndConnHandle.Close();
                    clientgrid.Focus();
                    for (int i = 0; i < 10; i++)
                    {
                        clientview.Rows.Add();
                    }
                }
            }
        }

        #region Checking if all the mandatory conditions are satisfied
        private void AllClear2Go()
        {
            if (textBox1.Text != "" && gnCoverStatus>0 )
            {
                SaveButton.Enabled = true;
                SaveButton.BackColor = Color.LawnGreen;
          //      SaveButton.Focus();
            //    SaveButton.Select();
            }
            else
            {
                SaveButton.Enabled = false;
                SaveButton.BackColor = Color.FromArgb(224, 224, 224);        // Color.Red;
            }
            
        }
        #endregion



        private void firstclient()
        {
            textBox1.Text = clientgrid.Rows[0].Cells[6].Value.ToString();
        }

        private void getdrest(string dcode)
        {
      //      MessageBox.Show("inside getrest ");
            string cs = globalvar.cos;
            string ncompid = globalvar.gnCompid.ToString().Trim();
            int lnvisno =Convert.ToInt32(clientgrid.CurrentRow.Cells[5].Value.ToString());
            string dsql = "exec tsp_GetClientCover_One " + ncompid + ",'" + dcode.Trim() + "'," + lnvisno;        //?gcCoverCustCode,?gnVisno;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                DataTable instview = new DataTable();
                da.Fill(instview);
                if (instview.Rows.Count > 0)
                {
                    comboBox1.DataSource = instview.DefaultView;
                    comboBox1.DisplayMember = "insu_name";
                    comboBox1.ValueMember = "insu_id";
                } else { MessageBox.Show("Cover Institution cannot be found, inform IT Dept immediately"); }
            }
        }

        private void bouquets(int insid)
        {
            string cs = globalvar.cos;
            string ncompid = globalvar.gnCompid.ToString().Trim();
            string dsql = "exec tsp_Bouquets " + ncompid + "," +insid;  
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                DataTable bouview = new DataTable();
                da.Fill(bouview);
                if (bouview.Rows.Count > 0)
                {
                    comboBox2.DataSource = bouview.DefaultView;
                    comboBox2.DisplayMember = "bou_name";
                    comboBox2.ValueMember = "bou_id";
                    comboBox2.Enabled = true;
                }
                else { MessageBox.Show("Bouquets not defined for selected Institution, inform IT Dept immediately"); }
            }

 

            /*

                lfound=Thisform.member_update(1)
                If !lfound
                    Do Form memberupdate.scx With gcCoverCustCode,gnInsuID
                    .text2.Value = gcMemberID
                Endif
                 */
        }

        private void getbouquetsDetails(int bouid)
        {
            string cs = globalvar.cos;
            string ncompid = globalvar.gnCompid.ToString().Trim();
            int insid =Convert.ToInt32(comboBox1.SelectedValue.ToString());
            string dsql = "exec tsp_Bouquetsdetails " + ncompid + "," + insid + "," + bouid; ;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(boudetview);
                if (boudetview.Rows.Count > 0)
                {
                    bougrid.DataSource = boudetview.DefaultView;
                    bougrid.Columns[0].DataPropertyName = "bou_name";
                    bougrid.Columns[1].DataPropertyName = "coveramt";
                    bougrid.Columns[2].DataPropertyName = "corepay";
                    bougrid.Columns[3].DataPropertyName = "corepercent";
                    bougrid.Columns[4].DataPropertyName = "conspay";
                    bougrid.Columns[5].DataPropertyName = "vislim";
                    bougrid.Columns[6].DataPropertyName = "visrbate";
                    bougrid.Columns[7].DataPropertyName = "startdate";
                    bougrid.Columns[8].DataPropertyName = "bouqstat";
                }
                else { MessageBox.Show("Bouquets not defined for selected Institution, inform IT Dept immediately"); }
            }

            /*

                lfound=Thisform.member_update(1)
                If !lfound
                    Do Form memberupdate.scx With gcCoverCustCode,gnInsuID
                    .text2.Value = gcMemberID
                Endif
                 */
        }

        private void bouquetStatus()
        {
            string cs = globalvar.cos;
            string ncompid = globalvar.gnCompid.ToString().Trim();
            string dsql = "exec sp_GetCoverStatus " + ncompid;         
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                DataTable boustatview = new DataTable();
                da.Fill(boustatview);
                if (boustatview.Rows.Count > 0)
                {
                    comboBox3.DataSource = boustatview.DefaultView;
                    comboBox3.DisplayMember = "sta_name";
                    comboBox3.ValueMember = "sta_id";
                }
                else { MessageBox.Show("Bouquets status not defined, inform IT Dept immediately"); }
            }
        }

        /*                         
=FreeServiceCheck(lnSrvCentre)     &&Checking for free services

         */



        private void clientgrid_Click(object sender, EventArgs e)
        {
          textBox1.Text = clientgrid.CurrentRow.Cells[6].Value.ToString();
            DataRow drow = clientview.Rows[clientgrid.CurrentCell.RowIndex];
            bool glhasins =Convert.ToBoolean(drow["wit_ins"].ToString());
           bool glhascor = Convert.ToBoolean(drow["wit_cor"].ToString());
            bool glhasnhi = Convert.ToBoolean(drow["wit_nhi"].ToString());
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ins_id =comboBox1.SelectedValue.ToString();
            /*
             With Thisform
	If !Empty(This.Value)
		gnInsuID  = Medinsurance.insu_id
		gcCoverName=Medinsurance.insu_name
		gnInsType=Medinsurance.ins_type
		gcCoverContra=gcIntSuspense
		.text9.Value=gcCoverContra
		.text5.Value=Medinsurance.corepay
		.text8.Value=Medinsurance.co_payPercent
		.text7.Value=Medinsurance.conspay
		fn=SQLExec(gnConnHandle,"exec tsp_Bouquets ?gnCompid,?gnInsuID","ViewIns")
		If fn>0 And Reccount()>0
			.combo5.RowSource='ViewIns.bou_name,bou_id'
			.combo5.Enabled=.T.
			lfound=Thisform.member_update(1)
			If !lfound
				Do Form memberupdate.scx With gcCoverCustCode,gnInsuID
				.text2.Value = gcMemberID
			Endif
		Else
			=sysmsg("No bouquets defined")
			.combo5.Enabled=.F.
		Endif
	Endif
	.Refresh
Endwith
             */
            comboBox2.Enabled = false;
            if(ins_id!=null && ins_id!= "System.Data.DataRowView")
            {
                comboBox2.Enabled = true;
            }
            AllClear2Go();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string bou_id = comboBox2.SelectedValue.ToString();
            if (bou_id.ToString().Trim() != "System.Data.DataRowView")
            {
                gcBouName = comboBox2.Text.ToString().Trim();
                comboBox3.Enabled = true;
                if(boudetview.Rows.Count>0)
                {
                    foreach(DataRow drow in boudetview.Rows)
                    {
                        if(drow["bou_name"].ToString().Trim()!=gcBouName)
                        {
                            getbouquetsDetails(Convert.ToInt32(bou_id));
                            break;
                        }
                    }
                }else
                {
                    getbouquetsDetails(Convert.ToInt32(bou_id));
                }
            } else { comboBox3.Enabled = false; }
            AllClear2Go();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            string disp = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(comboBox3.Text.ToString());
            string dvalue = comboBox3.SelectedValue.ToString();
            int rowno = boudetview.Rows.Count;
            if (dvalue != null && dvalue != "System.Data.DataRowView" )
            {
                string disp1 = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(comboBox3.Text.ToString());
                gnCoverStatus = Convert.ToInt32(dvalue);
                boudetview.AsEnumerable().Where(s => Convert.ToString(s["bouqstat"]) != disp && Convert.ToString(s["bou_name"]).Trim()== gcBouName).ToList().ForEach(d => d.SetField("bouqstat", disp));
            }
            AllClear2Go();
        }

        private void comboBox1_Enter(object sender, EventArgs e)
        {
            getdrest(textBox1.Text);
        }

        private void comboBox2_Enter(object sender, EventArgs e)
        {
            string ins_id = comboBox1.SelectedValue.ToString();
            if (ins_id != null && ins_id != "System.Data.DataRowView")
            {
                bouquets(Convert.ToInt32(ins_id));
            }
        }

        private void comboBox3_Enter(object sender, EventArgs e)
        {
            bouquetStatus();
        }


        //save detaisl
        private void SaveButton_Click(object sender, EventArgs e)
        {
            int lnConfirmed = 0;
            int lnPending = 0;
            int lnSuspended = 0;
            int lnCancelled = 0;
            string gcCoverCustCode = textBox1.Text;
            string lcCustCode = textBox1.Text;
            decimal gnTotalCorePay = 0.00m;
            string tcContra = globalvar.gcIntSuspense;
            string tcUserid = globalvar.gcUserid;
       //     string cs1 = globalvar.cos;
            string dswitch = "";

            if (boudetview.Rows.Count > 0)
            {
                foreach (DataRow drow in boudetview.Rows)
                {
                    gnTotalCorePay = gnTotalCorePay + Convert.ToDecimal(drow["corepay"]);
          //          string dswitch = drow["bouqstat"].ToString().Trim().ToUpper();
                    switch (dswitch)
                    {
                        case "CONFIRMED":
                            lnConfirmed++;
                            break;
                        case "PENDING":
                            lnPending++;
                            break;
                        case "SUSPENDED":
                            lnSuspended++;
                            break;
                        case "CANCELLED":
                            lnCancelled++;
                            break;
                    }
                }
            }

            //            boudetview.Rows[0].
            //        DataRow srow = boudetview.Rows;
            //      boudetview.Rows[0];
            updatedetails(lcCustCode, dswitch);
            getclientList();
            firstclient();
            clientgrid.Focus();
        } //end of savebutton


        private void updatedetails(string lcCode, string lcstatus)
        {
            string cs1 = globalvar.cos;

            using (SqlConnection ndConnHandle = new SqlConnection(cs1))
            {
                int dr = boudetview.Rows.Count;
                string drowcont = boudetview.Rows[0]["boustat"].ToString();
                MessageBox.Show("The number of rows is " + dr+" and content of "+drowcont);
                if (boudetview.Rows.Count > 0)
                {
                    foreach (DataRow nrow in boudetview.Rows)
                    {
                        int gnInsuID = Convert.ToInt32(comboBox1.SelectedValue.ToString());
                        int lbou_ID = Convert.ToInt32(nrow["bou_id"].ToString().Trim());
                        int gnBouID = Convert.ToInt16(lbou_ID);
                        decimal lnAmt = Convert.ToDecimal(nrow["coveramt"]);
                        int ncompid = globalvar.gnCompid;
                        string gcAcctNumb = globalvar.ClientAcctPrefix + textBox1.Text.ToString();
                        string lcBouName = nrow["bou_name"].ToString();
                        DateTime ldStDate = Convert.ToDateTime(nrow["startdate"].ToString());
                        decimal gnCorePay = Convert.ToDecimal(nrow["corepay"]);
                        decimal gnConsPay = Convert.ToDecimal(nrow["conspay"]);
                        int lnvisno = Convert.ToInt32(clientgrid.CurrentRow.Cells[5].Value.ToString());
                        DateTime ldEdDate = DateTime.Now;
                        string lcmemno = textBox7.Text;
                        string tcUserid = globalvar.gcUserid;
                        string gcContNumb = globalvar.gcIntSuspense;
                        //                bool llhasIns = nrow[]
                        //        using (SqlConnection ndConnHandle1 = new SqlConnection(cs1))
                        //      {
             //           string lcstatus = nrow["bouqstat"].ToString().ToUpper().Trim();
                        switch (lcstatus)
                        {
                            case "CONFIRMED":
                                medcoveritems(cs1, lcCode, gnInsuID, lbou_ID, lnAmt, lcmemno, lnvisno, ncompid, ldStDate, ldEdDate);  //update medcoveritems

                                if (gnCorePay > 0.00m)
                                {
                                    billcopay(cs1, gnCorePay, lcCode, gcAcctNumb, tcUserid, lnvisno);
                                }

                                if (gnConsPay > 0.00m)
                                {
                                    billconspay(cs1, gnConsPay, lcCode, gcAcctNumb, tcUserid, lnvisno);
                                }
                                medcover(cs1, lbou_ID, lcCode, lnvisno);              //update medcover.dbf
                                Patcover(cs1, 1, lcCode, gnInsuID, lbou_ID, glhascor, glhasins, glhasnhi);             //updates the inscover,corcover,nhifcover as covered of the client in pat_visit,todayvisit
                                                                                                                           //                                private void Patcover(string cs, int i, string tcCode, int lninsuid, int lnbouid, bool lhascor, bool lhasins, bool lnhif)
                                break;

                            case "PENDING":
                                medcoveritems(cs1, lcCode, gnInsuID, lbou_ID, lnAmt, lcmemno, lnvisno, ncompid, ldStDate, ldEdDate);
                                if (MessageBox.Show("Has Client signed consent form ", "Consent form Check", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    medcoveritems(cs1, lcCode, gnInsuID, lbou_ID, lnAmt, lcmemno, lnvisno, ncompid, ldStDate, ldEdDate);
                                    if (gnCorePay > 0.00m)
                                    {
                                        billcopay(cs1, gnCorePay, lcCode, gcAcctNumb, tcUserid, lnvisno);
                                    }
                                    if (gnConsPay > 0.00m)
                                    {
                                        billcopay(cs1, gnConsPay, lcCode, gcAcctNumb, tcUserid, lnvisno);
                                    }
                                    medcover(cs1, lbou_ID, lcCode, lnvisno);              //update medcover.dbf
                                    Patcover(cs1, 1, lcCode, gnInsuID, lbou_ID, glhascor, glhasins, glhasnhi);             //updates the inscover,corcover,nhifcover as covered of the client in pat_visit,todayvisit
                                }
                                else
                                {
                                    if (MessageBox.Show("Send Client to pay cash?", "Client status changek", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                    {
                                        send2cash();            //client is sent to pay cash for the system unit booked for (only once)
                                        Patcover(cs1, 2, lcCode, gnInsuID, lbou_ID, glhascor, glhasins, glhasnhi);             //updates the inscover,corcover,nhifcover as covered of the client in pat_visit,todayvisit
                                    }
                                    else
                                    {
                                        dischargeclient(lcCode);
                                    }
                                }
                                break;

                            default:
                                if (MessageBox.Show("Send Client to pay cash?", "Client status changek", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    send2cash();            //client is sent to pay cash for the system unit booked for (only once)
                                    Patcover(cs1, 2, lcCode, gnInsuID, lbou_ID, glhascor, glhasins, glhasnhi);             //updates the inscover,corcover,nhifcover as covered of the client in pat_visit,todayvisit
                                }
                                else
                                {
                                    dischargeclient(lcCode);
                                }
                                break;
                        }
                    }
                }
            }

        }

        private void medcover(string cs,int lnbouid,string tcCode,int dvisno)
        {
            MessageBox.Show("doing medcover");
            //"update medcover set bou_id=?gnBouID where ccustcode=?gcCoverCustCode and visno=?gnVisno", "bouupd")
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                string updMedCover = "update medcover set bou_id=@gnBouID where ccustcode=@gcCoverCustCode and visno=@gnVisno";
                SqlDataAdapter updMed = new SqlDataAdapter();
                updMed.UpdateCommand = new SqlCommand(updMedCover, ndConnHandle1);
                updMed.UpdateCommand.Parameters.Add("@gnBouID", SqlDbType.Int).Value = lnbouid;
                updMed.UpdateCommand.Parameters.Add("@gcCoverCustCode", SqlDbType.Char).Value = tcCode;
                updMed.UpdateCommand.Parameters.Add("@gnVisno", SqlDbType.Int).Value = dvisno;
                ndConnHandle1.Open();
                updMed.UpdateCommand.ExecuteNonQuery();
                ndConnHandle1.Close();
            }

        }
        private void dischargeclient(string tcCode)
        {
            MessageBox.Show("This option will discharge client ");
            string cs = globalvar.cos;
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                string updpatv = "update pat_visit set activesession=0 where ccustcode=@gcCoverCustCode";
                string updtodayv = "update TodayVisit set activesession=0 where ccustcode=@gcCoverCustCode";

                SqlDataAdapter updpav = new SqlDataAdapter();
                SqlDataAdapter updtov = new SqlDataAdapter();

                updpav.UpdateCommand = new SqlCommand(updpatv, ndConnHandle1);
                updtov.UpdateCommand = new SqlCommand(updtodayv, ndConnHandle1);


                //update pat_visit
                updpav.UpdateCommand.Parameters.Add("@gcCoverCustCode", SqlDbType.Char).Value = tcCode;

                //update todayvisit
                updtov.UpdateCommand.Parameters.Add("@gcCoverCustCode", SqlDbType.Char).Value = tcCode;

                ndConnHandle1.Open();
                updpav.UpdateCommand.ExecuteNonQuery();
                updtov.UpdateCommand.ExecuteNonQuery();
                ndConnHandle1.Close();
            }
        }

        private void Patcover(string cs, int i,string tcCode,int lninsuid, int lnbouid,bool lhascor,bool lhasins, bool lnhif)
        {
            MessageBox.Show("will update pat_visit and todayvisit");
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                if(i==1)        //update patient record as covered
                {
                    string updMedCover = "update medcoveritems set lupdated=1 where ccustcode=@gcCoverCustCode and ins_id=@gnInsuID and bou_id=@gnBouID";
                    string updpatv = "update pat_visit set ready4triage=@lcReady4Triage where ccustcode=@gcCoverCustCode";
                    string updtodayv = "update TodayVisit set ready4triage=@lcReady4Triage where ccustcode=@gcCoverCustCode";
                    string updmeditem = "update medcover set lupdated=1 where ccustcode=@gcCoverCustCode and lupdated=0";

                    SqlDataAdapter updMed = new SqlDataAdapter();
                    SqlDataAdapter updpav = new SqlDataAdapter();
                    SqlDataAdapter updtov = new SqlDataAdapter();
                    SqlDataAdapter updcov = new SqlDataAdapter();

                    updMed.UpdateCommand = new SqlCommand(updMedCover, ndConnHandle1);
                    updpav.UpdateCommand = new SqlCommand(updpatv, ndConnHandle1);
                    updtov.UpdateCommand = new SqlCommand(updtodayv, ndConnHandle1);
                    updcov.UpdateCommand = new SqlCommand(updmeditem, ndConnHandle1);

                    //update medcoveritems
                    updMed.UpdateCommand.Parameters.Add("@gcCoverCustCode", SqlDbType.Int).Value = tcCode;
                    updMed.UpdateCommand.Parameters.Add("@gnInsuID", SqlDbType.Int).Value = lninsuid;
                    updMed.UpdateCommand.Parameters.Add("@gnBouID", SqlDbType.Int).Value = lnbouid;

                    //update pat_visit
                    updpav.UpdateCommand.Parameters.Add("@lcReady4Triage", SqlDbType.Bit).Value = true;
                    updpav.UpdateCommand.Parameters.Add("@gcCoverCustCode", SqlDbType.Char).Value = tcCode;

                    //update todayvisit
                    updtov.UpdateCommand.Parameters.Add("@lcReady4Triage", SqlDbType.Bit).Value = true;
                    updtov.UpdateCommand.Parameters.Add("@gcCoverCustCode", SqlDbType.Char).Value = tcCode;

                    //update medcover
                    updcov.UpdateCommand.Parameters.Add("@gcCoverCustCode", SqlDbType.Char).Value = tcCode;


                    ndConnHandle1.Open();
                    updMed.UpdateCommand.ExecuteNonQuery();
                    updpav.UpdateCommand.ExecuteNonQuery();
                    updtov.UpdateCommand.ExecuteNonQuery();
                    updcov.UpdateCommand.ExecuteNonQuery();
                    ndConnHandle1.Close();
                }
                else         //update patient record as !covered (cash)
                {                   
                    string updpatv = "update pat_visit set wit_cas=1,wit_ins=0,ins_conf=0,wit_cor=0,cor_conf=0,wit_nhi=0,nhi_conf=0 where ccustcode=@gcCoverCustCode and activesession=1";
                    string updtodayv = "update TodayVisit set wit_cas=1,wit_ins=0,ins_conf=0,wit_cor=0,cor_conf=0,wit_nhi=0,nhi_conf=0 where ccustcode=@gcCoverCustCode and activesession=1";
                    string updmeditem = "update medcover set lupdated=1 where ccustcode=@gcCoverCustCode and lupdated=0";

                    SqlDataAdapter updpav = new SqlDataAdapter();
                    SqlDataAdapter updtov = new SqlDataAdapter();
                    SqlDataAdapter updcov = new SqlDataAdapter();

                    updpav.UpdateCommand = new SqlCommand(updpatv, ndConnHandle1);
                    updtov.UpdateCommand = new SqlCommand(updtodayv, ndConnHandle1);
                    updcov.UpdateCommand = new SqlCommand(updmeditem, ndConnHandle1);

                    //update pat_visit
                    updpav.UpdateCommand.Parameters.Add("@gcCoverCustCode", SqlDbType.Char).Value = tcCode;

                    //update todayvisit
                    updtov.UpdateCommand.Parameters.Add("@gcCoverCustCode", SqlDbType.Char).Value = tcCode;

                    //update medcover
                    updcov.UpdateCommand.Parameters.Add("@gcCoverCustCode", SqlDbType.Char).Value = tcCode;

                    ndConnHandle1.Open();
                    updpav.UpdateCommand.ExecuteNonQuery();
                    updtov.UpdateCommand.ExecuteNonQuery();
                    updcov.UpdateCommand.ExecuteNonQuery();
                    ndConnHandle1.Close();
                }
            }
        }

        private void send2cash()
        {
            MessageBox.Show("Client will be sent to cash");
            /*
             Parameters tnInsuID
sn=SQLExec(gnConnHandle,"update medcover set lupdated=1 where ccustcode=?gcCoverCustCode and ins_id=?tnInsuID","coverupd")
If !(sn>0 )
	=sysmsg("Could not update medical cover flag, inform IT DEPT ")
Else
	gcbillNo=genbill('2')
	lnPrice=Thisform.text6.Value
	gcServName=	'Consultation fee'										&&Iif(gnPatType=1,fee_loc,Iif(gnPatType=2,fee_exp,fee_for))
	lnServID=99
	lncash=1
*****************************************update for patient account - posting account
	lnPrice=serv_fee
	lnPostAmt=-Abs(lnPrice)
	lnPatID=0
	glFreeBee=thisform.checksrvfreebee ('CONSP001')

**********update client  account
	=UpdateGlmast(gcCoverAcctNumb,lnPostAmt)			&&client Account - Posting Account
	gnPNewBal=CheckLastBalance(gcCoverAcctNumb)
	=UpdateTransactionHistory(gcCoverAcctNumb,lnPostAmt,gcServName,gcbillNo,'000001',gcUserID,gnPNewBal,'01',lnServID,gnPatID,0,gcIntSuspense,0.00,1,lnPostAmt,'',lncash,gnVisitNo,.F.,1,'CONSP001','')

*****************************************update for Internal contra account
	lnContAmt=Abs(lnPrice)
	gnDIncome=lnContAmt
	gnDAcct_rec=lnContAmt
	lnPatID=0

	=UpdateGlmast(gcIntSuspense,lnContAmt)			&&update internal Suspense Account - Contra account
	gncNewBal=CheckLastBalance(gcIntSuspense)
	=UpdateTransactionHistory(gcIntSuspense,lnContAmt,gcServName,gcbillNo,'000001',gcUserID,gncNewBal,'92',lnServID,gnPatID,0,gcCoverAcctNumb,0.00,1,lnContAmt,'',lncash,gnVisitNo,.F.,1,'CONSP001','')

Endif

             */
        }


        private void medcoveritems(string cs,string tcCode,int lninsuid,int lbou_id,decimal lnAmt,string lcmemno,int lnvisno,int ncompid,DateTime ldStDate,DateTime ldEdDate)
        {
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                string cupdatequery = "insert into medcoveritems (cov_id,ins_id,bou_id,ccustcode,compid,amount,st_date,ed_date,memberno,coverstatus,dtrandate,ctrantime) ";
                cupdatequery += " values (@lnInsid,@gnInsuID,@lnbou_id,@gcCoverCustCode,@gnCompid,@lnAmt,@ldStDate,@ldEdDate,@lcMemno,@lnstatus,convert(date,getdate()),convert(time,getdate()))";
                SqlDataAdapter updCommand = new SqlDataAdapter();
                updCommand.InsertCommand = new SqlCommand(cupdatequery, ndConnHandle1);
                updCommand.InsertCommand.Parameters.Add("@lnInsid", SqlDbType.Int).Value = 1;
                updCommand.InsertCommand.Parameters.Add("@gnInsuID", SqlDbType.Int).Value = lninsuid;
                updCommand.InsertCommand.Parameters.Add("@lnbou_id", SqlDbType.Int).Value = lbou_id;
                updCommand.InsertCommand.Parameters.Add("@gcCoverCustCode", SqlDbType.Char).Value =tcCode;
                updCommand.InsertCommand.Parameters.Add("@gnCompid", SqlDbType.Int).Value = ncompid;
                updCommand.InsertCommand.Parameters.Add("@lnAmt", SqlDbType.Decimal).Value = lnAmt;
                updCommand.InsertCommand.Parameters.Add("@ldStDate", SqlDbType.DateTime).Value = ldStDate;
                updCommand.InsertCommand.Parameters.Add("@ldEdDate", SqlDbType.DateTime).Value = ldEdDate;
                updCommand.InsertCommand.Parameters.Add("@lcMemno", SqlDbType.VarChar).Value = lcmemno;
                updCommand.InsertCommand.Parameters.Add("@lnstatus", SqlDbType.Int).Value = 1;
                ndConnHandle1.Open();
                updCommand.InsertCommand.ExecuteNonQuery();
            }
        }       //end of medcoveritems

        private void billcopay(string cs, decimal lnprice, string tcCode, string tcAcctNumb, string tcUserid, int dvisno)
        {
            MessageBox.Show("update bill copay");
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                //      llCash=CashPay
                decimal lnPrice = lnprice;// gnCorePay  &&check if service centre offers free service 
                string gcServName = "Co-Payment Fee";  //&&	Iif(glHasIns,'Co-Payment ','Consultation Fee')					&&The client is billed for the agreed co-pay which must be paid at pay point
                int lnServID = 99;
                int ncompid = globalvar.gnCompid;
                string tcChqno = "000001";
                string gcbillNo = genbill.genvoucher(cs, globalvar.gdSysDate);   //gcbillNo =genbill('2')
                bool lncash = true;
                bool llpaid = false;
                bool isproduct = false;
                string tcContra = globalvar.gcIntSuspense;
                //      *****************************************update for patient account - posting account
                decimal lnPostAmt = -Math.Abs(lnPrice);
           //     int lnPatID = 0;
                bool lFreeBee = false;     //this will be visited later thisform.checksrvfreebee ('CP001')

                updateGlmast gls = new updateGlmast();
                updateTranhist tls = new updateTranhist();

                //                      **********update client  account
                gls.updGlmast(cs, tcAcctNumb, lnPostAmt);                                       //update glmast posting account
                decimal tnPNewBal = CheckLastBalance.lastbalance(cs, tcAcctNumb);
                tls.updTranhist(cs, tcAcctNumb, lnPostAmt, gcServName, gcbillNo, tcChqno, tcUserid, tnPNewBal, "79", lnServID, llpaid, tcContra, 0.00m, 1, lnPostAmt, " ", lncash, dvisno, isproduct,
                      1, "CP001", "", lFreeBee, tcCode, ncompid);                   //update tranhist posting account


                //*****************************************update for patient control account - contra account
                decimal lnContAmt = Math.Abs(lnPrice);
                decimal gnDIncome = lnContAmt;
                decimal gnDAcct_rec = lnContAmt;
                gls.updGlmast(cs, tcContra, lnContAmt);                                       //update glmast posting account
                decimal tnCNewBal = CheckLastBalance.lastbalance(cs, tcContra);
                tls.updTranhist(cs, tcContra, lnContAmt, gcServName, gcbillNo, tcChqno, tcUserid, tnPNewBal, "92", lnServID, llpaid, tcAcctNumb, 0.00m, 1, lnContAmt, " ", lncash, dvisno, isproduct,
                      1, "CP001", "", lFreeBee, tcCode, ncompid);                   //update tranhist contra account
                                                                                    //         MessageBox.Show("Bill co payment done");
            }
        }  //end of billcopay


        private void billconspay(string cs, decimal lnprice, string tcCode, string tcAcctNumb, string tcUserid, int dvisno)
        {
           MessageBox.Show("We will update consultation fee");
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                decimal lnPrice = lnprice;// gnCorePay  &&check if service centre offers free service 
                string gcServName = "Consultation Fee";  //&&	Iif(glHasIns,'Co-Payment ','Consultation Fee')					&&The client is billed for the agreed co-pay which must be paid at pay point
                int lnServID = 99;
                int ncompid = globalvar.gnCompid;
                string tcChqno = "000001";
                string gcbillNo = genbill.genvoucher(cs, globalvar.gdSysDate);   //gcbillNo =genbill('2')
                bool lncash = false; 
                bool llpaid = false;
                bool isproduct = false;
                string tcContra = globalvar.gcIntSuspense;
                //      *****************************************update for patient account - posting account
                decimal lnPostAmt = -Math.Abs(lnPrice);
           //     int lnPatID = 0;
                bool lFreeBee = false;     //this will be visited later thisform.checksrvfreebee ('CP001')

                updateGlmast gls = new updateGlmast();
                updateTranhist tls = new updateTranhist();

                //                      **********update client  account
                gls.updGlmast(cs, tcAcctNumb, lnPostAmt);                                       //update glmast posting account
                decimal tnPNewBal = CheckLastBalance.lastbalance(cs, tcAcctNumb);
                tls.updTranhist(cs, tcAcctNumb, lnPostAmt, gcServName, gcbillNo, tcChqno, tcUserid, tnPNewBal, "01", lnServID, llpaid, tcContra, 0.00m, 1, lnPostAmt, " ", lncash, dvisno, isproduct,
                      1, "CONSP001", "", lFreeBee, tcCode, ncompid);                   //update tranhist posting account


                //*****************************************update for patient control account - contra account
                decimal lnContAmt = Math.Abs(lnPrice);
                decimal gnDIncome = lnContAmt;
                decimal gnDAcct_rec = lnContAmt;
                gls.updGlmast(cs, tcContra, lnContAmt);                                       //update glmast posting account
                decimal tnCNewBal = CheckLastBalance.lastbalance(cs, tcContra);
                tls.updTranhist(cs, tcContra, lnContAmt, gcServName, gcbillNo, tcChqno, tcUserid, tnPNewBal, "92", lnServID, llpaid, tcAcctNumb, 0.00m, 1, lnContAmt, " ", lncash, dvisno, isproduct,
                      1, "CONSP001", "", lFreeBee, tcCode, ncompid);                   //update tranhist contra account
                                                                                    //         MessageBox.Show("Bill co payment done");
            }
        }       //END of billconspay        
    }
}
