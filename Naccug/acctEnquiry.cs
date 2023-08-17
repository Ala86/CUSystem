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
    public partial class acctEnquiry : Form
    {
        int ncompid = globalvar.gnCompid;
        DataTable acctview = new DataTable();
        DataTable transview = new DataTable();
        DataTable entview = new DataTable();
        DataTable entAcctView = new DataTable();
        DataTable entTranView = new DataTable();
        DataTable intview = new DataTable();
        string cs = globalvar.cos;
        string cloca = globalvar.cLocalCaption;
       // private string cloca;

        public acctEnquiry()
        {
            InitializeComponent();
        }

        public acctEnquiry(string cs, int ncompid, string cloca)
        {
            this.cs = cs;
            this.ncompid = ncompid;
            this.cloca = cloca;

          //  MessageBox.Show("This is the connection string " + cs);
        }

        private void acctEnquiry_Load(object sender, EventArgs e)
        {
         this.Text = globalvar.cLocalCaption + "<< Account Enquiry >>";
            getAccounts();
            getEntityCode(1);
            acctGrid.Columns["dbookbal"].SortMode = DataGridViewColumnSortMode.NotSortable;
            acctGrid.Columns["dbookbal"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            acctGrid.Columns["dcleabal"].SortMode = DataGridViewColumnSortMode.NotSortable;
            acctGrid.Columns["dcleabal"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            acctGrid.Columns["dunclbal"].SortMode = DataGridViewColumnSortMode.NotSortable;
            acctGrid.Columns["dunclbal"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;


            tranGrid.Columns["ddebit"].SortMode = DataGridViewColumnSortMode.NotSortable;
            tranGrid.Columns["ddebit"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            tranGrid.Columns["dcredit"].SortMode = DataGridViewColumnSortMode.NotSortable;
            tranGrid.Columns["dcredit"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            tranGrid.Columns["dbalance"].SortMode = DataGridViewColumnSortMode.NotSortable;
            tranGrid.Columns["dbalance"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Down || e.KeyCode == Keys.Tab)
            {
                SelectNextControl(ActiveControl, true, true, true, true);
                e.Handled = true;
        //        AllClear2Go();
            }
            else if (e.KeyCode == Keys.Up)
            {
                SelectNextControl(ActiveControl, false, true, true, true);
                e.Handled = true;
       //         AllClear2Go();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void getClientAccount(string actnumb)
        {
            acctview.Clear();
            string dsql1 = "exec tsp_AcctEnqOne  " + ncompid + ",'" + actnumb + "'";
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da1 = new SqlDataAdapter(dsql1, ndConnHandle);
                da1.Fill(acctview);
                if (acctview!=null && acctview.Rows.Count > 0)
                {
                    textBox1.Text = acctview.Rows[0]["cacctname"].ToString();
                    textBox3.Text = Convert.ToDecimal(acctview.Rows[0]["nbookbal"]).ToString("N2");
                    textBox2.Text = acctview.Rows[0]["nunclbal"].ToString();
                    textBox5.Text = Convert.ToDecimal(acctview.Rows[0]["ncleabal"]).ToString("N2");
                    textBox7.Text = acctview.Rows[0]["cacctnumb"].ToString();
                } else { MessageBox.Show("Account is not found"); }
            }
        }

        private void getEntityCode(int entitycode)
        {
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                entview.Clear();
                ndConnHandle1.Open();
                switch (entitycode)
                {
                    case 1: //Individual
                        string dsql1 = "exec tsp_getIndMembers  " + ncompid;
                        SqlDataAdapter da1 = new SqlDataAdapter(dsql1, ndConnHandle1);
                        da1.Fill(entview);
                        if (entview != null && entview.Rows.Count > 0)
                        {
                            comboBox3.DataSource = entview.DefaultView;
                            comboBox3.DisplayMember ="membname";
                            comboBox3.ValueMember = "ccustcode";
                            comboBox3.SelectedIndex = -1;
                        }
                        break;
                    case 2: //Corporate
                        string dsql2 = "exec tsp_getCorMembers  " + ncompid;
                        SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle1);
                        da2.Fill(entview);
                        if (entview != null && entview.Rows.Count > 0)
                        {
                            comboBox3.DataSource = entview.DefaultView;
                            comboBox3.DisplayMember = "membname";
                            comboBox3.ValueMember = "ccustcode";
                            comboBox3.SelectedIndex = -1;
                        }
                        break;
                    case 3://Group
                        string dsql3 = "exec tsp_getGrpMembers  " + ncompid;
                        SqlDataAdapter da3 = new SqlDataAdapter(dsql3, ndConnHandle1);
                        da3.Fill(entview);
                        if (entview != null && entview.Rows.Count > 0)
                        {
                            comboBox3.DataSource = entview.DefaultView;
                            comboBox3.DisplayMember = "membname";
                            comboBox3.ValueMember = "ccustcode";
                            comboBox3.SelectedIndex = -1;
                        }
                        break;
                    case 4://supplier
                        string dsql4 = "exec tsp_getSuppliers  " + ncompid;
                        SqlDataAdapter da4 = new SqlDataAdapter(dsql4, ndConnHandle1);
                        da4.Fill(entview);
                        if (entview != null && entview.Rows.Count > 0)
                        {
                            comboBox3.DataSource = entview.DefaultView;
                            comboBox3.DisplayMember = "sup_name";
                            comboBox3.ValueMember = "supp_no";
                            comboBox3.SelectedIndex = -1;
                        }
                        break;
                    case 5://staff
                        string dsql5 = "exec tsp_getCuStaff  " + ncompid;
                        SqlDataAdapter da5 = new SqlDataAdapter(dsql5, ndConnHandle1);
                        da5.Fill(entview);
                        if (entview != null && entview.Rows.Count > 0)
                        {
                            comboBox3.DataSource = entview.DefaultView;
                            comboBox3.DisplayMember = "staffname";
                            comboBox3.ValueMember = "staffno";
                            comboBox3.SelectedIndex = -1;
                        }
                        break;
                }
            }
        }

        private void getEntityAccounts(string dsqlstring,string entitycode)
        {
            entAcctView.Clear();
            //            MessageBox.Show("The string is " + dsqlstring+" and entity code is "+entitycode);
            textBox14.Text = entitycode;
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                //************Getting accounts                
                string dsqlstr = dsqlstring;   
                SqlDataAdapter dasqlAdp = new SqlDataAdapter();
                dasqlAdp.SelectCommand = new SqlCommand(dsqlstr, ndConnHandle1);
                dasqlAdp.SelectCommand.Parameters.Add("@ncompid", SqlDbType.Int).Value = ncompid;
                dasqlAdp.SelectCommand.Parameters.Add("@entcode", SqlDbType.VarChar).Value = entitycode;
                dasqlAdp.Fill(entAcctView);
                if (entAcctView != null)
                {
                    acctGrid.AutoGenerateColumns = false;
                    acctGrid.DataSource = entAcctView.DefaultView;
                    acctGrid.Columns[0].DataPropertyName = "cacctnumb";
                    acctGrid.Columns[1].DataPropertyName = "cacctname";
                    acctGrid.Columns[2].DataPropertyName = "nbookbal";
                    acctGrid.Columns[3].DataPropertyName = "ncleabal";
                    acctGrid.Columns[4].DataPropertyName = "nunclbal";
                    ndConnHandle1.Close();
                    for (int i = 0; i < 10; i++)
                    {
                        entAcctView.Rows.Add();
                    }
                    acctGrid.Focus();
                }
            }
        }


        private void getAccounts()
        {
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                //************Getting accounts                
                string dsql1 = "select  cacctname,cacctnumb,nbookbal,nunclbal,ncleabal from glmast where ccustcode not in (select ccustcode from cusreg) ";
                SqlDataAdapter da1 = new SqlDataAdapter(dsql1, ndConnHandle1);
                da1.Fill(intview);
                if (intview != null)
                {
                    comboBox4.DataSource = intview.DefaultView;
                    comboBox4.DisplayMember = "cacctname";
                    comboBox4.ValueMember = "cacctnumb";
                    comboBox4.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not find main accounts, inform IT Dept immediately"); }
            }
        }

        private void getLoanDetails(string memcode)
        {
/*            Parameters tnPageNumb
Do Case
Case tnPageNumb = 1

 Case tnPageNumb = 2
 
     With Thisform.pageframe1.page2
 
         gn = SQLExec(gnConnHandle, "EXECUTE sp_AcctByCustCode ?gnCompID,?.text11.value", "custview")
 
         If gn > 0 And Reccount() > 0

             .text13.Value = Alltrim(pc_tel)
 

             gnCustInd = 1
             .text14.Value = Iif(gnCustInd = 1 And !Empty(ccustfname), Alltrim(ccustfname), '')
             .text9.Value = Iif(gnCustInd = 1 And !Empty(ccustmname), Alltrim(ccustmname), '')
             .text10.Value = Iif(gnCustInd = 1 And !Empty(ccustlname), Alltrim(ccustlname), '')

             .p2grid1.Visible =.T.
 
             .p2grid4.Visible =.F.
 
             .p2grid1.RecordSource = 'custview'
             .p2grid1.column1.ControlSource = 'custview.ccustcode'
             .p2grid1.column2.ControlSource = 'custview.ccustfname'
             .p2grid1.column3.ControlSource = 'custview.ccustmname'
             .p2grid1.column4.ControlSource = 'custview.ccustlname'
 

             sn = SQLExec(gnConnHandle, "select cacctnumb,cacctname,nbookbal,ncleabal from glmast where ccustcode=?gcCustCode", "glmastview")
 
             If sn > 0 And Reccount() > 0
                 .p2grid2.RecordSource = 'glmastview'
 
                 gcAcctName = cacctname
                 .p2grid2.column1.ControlSource = 'glmastview.cacctnumb'
                 .p2grid2.column2.ControlSource = 'glmastview.cacctname'
                 .p2grid2.column3.ControlSource = 'glmastview.nbookbal'
                 .p2grid2.column4.ControlSource = 'glmastview.ncleabal'
 

             Endif
 
         Else
             = Messagebox('Searched record not found', 0, 'Search failure')
 
         Endif
         .Refresh
 
     Endwith
 Case tnPageNumb = 3
 
     With Thisform.pageframe1.page3.invogrid
 
         hn = SQLExec(gnConnHandle, "exec tsp_AcctByInvoice ?gnCompID,?gcInvoiceNo", "invrecs")
 
         If hn > 0 And Reccount() > 0
             .RecordSource = 'jourrecs'
             .column1.ControlSource = 'ttod(jourrecs.dpostdate)'
             .column2.ControlSource = 'ttod(jourrecs.dvaluedate)'
             .column3.ControlSource = 'iif(jourrecs.ntranamnt<0,ABS(jourrecs.ntranamnt),0000000.00)'
             .column4.ControlSource = 'iif(jourrecs.ntranamnt>0,ABS(jourrecs.ntranamnt),0000000.00)'
             .column5.ControlSource = 'iif(!ISNULL(jourrecs.nnewbal),ABS(jourrecs.nnewbal),0.00)'
             .column6.ControlSource = 'jourrecs.ctrandesc'
             .column7.ControlSource = 'iif(!ISNULL(jourrecs.cchqno),jourrecs.cchqno,"")'
             .column8.ControlSource = 'jourrecs.cacctnumb'
             .column9.ControlSource = 'jourrecs.ccontra'
 *           .command2.Enabled =.T.
         Else
             = sysmsg('Voucher number not found')
 *           .command2.Enabled =.F.
         Endif
         .Refresh
 
     Endwith
 Case tnPageNumb = 5
 
     With Thisform.pageframe1.page5
 * *******************************************
 *lcConf = Iif(.optiongroup1.Value = 1,[sverified = 1],[sverified = 0])
 
         lcDate = Iif(!Empty(gdStDate) And !Empty(gdEdDate),[dpostdate between ? gdStDate and ? gdEdDate],;
            IIF(Empty(gdStDate) And !Empty(gdEdDate),[dPosDate <=? gdEdDate],;
            IIF(!Empty(gdStDate) And Empty(gdEdDate),[dPosDate >=? gdStDate],;

            [dpostdate<>''])))
		lcUser=[cuserid=?gcUserid]
        lcFinCond=lcDate+' and '+lcUser
        gn = SQLExec(gnConnHandle, "Select cacctnumb,newacct,ccontra,dpostdate,dvaluedate,cchqno,ntranamnt,nnewbal,ctrandesc," +;
			"cuserid,cstack,cvoucherno from tranhist where  &lcFinCond order by cstack","cashrecs")
		If gn>0  And Reccount()>0
			.cashgrid.RecordSource = 'cashrecs'
			.cashgrid.column1.ControlSource = 'cashrecs.dpostdate'
			.cashgrid.column2.ControlSource = 'cashrecs.dvaluedate'
			.cashgrid.column3.ControlSource = 'iif(cashrecs.ntranamnt<0,ABS(cashrecs.ntranamnt),0000000.00)'
			.cashgrid.column4.ControlSource = 'iif(cashrecs.ntranamnt>0,ABS(cashrecs.ntranamnt),0000000.00)'
			.cashgrid.column5.ControlSource = 'cashrecs.nnewbal'
			.cashgrid.column6.ControlSource = 'cashrecs.ctrandesc'
			.cashgrid.column7.ControlSource = 'cashrecs.cchqno'
			.cashgrid.column8.ControlSource = 'cashrecs.newacct'
			.cashgrid.column9.ControlSource = 'cashrecs.ccontra'
			.cashgrid.column10.ControlSource = 'cashrecs.cUSERID'
			.command2.Enabled=.T.
* ELSE
*		WAIT WINDOW 'no transactions for selected teller'
			.command2.Enabled=.F.
        Endif
        .Refresh
    Endwith
Case tnPageNumb = 6

    With Thisform.pageframe1.page6

        hn= SQLExec(gnConnHandle, "exec sp_AcctByVoucher ?gnCompID,?gcVoucherNo", "jourrecs")

        If hn>0 And Reccount()>0
			.jourgrid.RecordSource = 'jourrecs'
			.jourgrid.column1.ControlSource = 'ttod(jourrecs.dpostdate)'
			.jourgrid.column2.ControlSource = 'ttod(jourrecs.dvaluedate)'
			.jourgrid.column3.ControlSource = 'iif(jourrecs.ntranamnt<0,ABS(jourrecs.ntranamnt),0000000.00)'
			.jourgrid.column4.ControlSource = 'iif(jourrecs.ntranamnt>0,ABS(jourrecs.ntranamnt),0000000.00)'
			.jourgrid.column5.ControlSource = 'iif(!ISNULL(jourrecs.nnewbal),ABS(jourrecs.nnewbal),0.00)'
			.jourgrid.column6.ControlSource = 'jourrecs.ctrandesc'
			.jourgrid.column7.ControlSource = 'iif(!ISNULL(jourrecs.cchqno),jourrecs.cchqno,"")'
			.jourgrid.column8.ControlSource = 'jourrecs.cacctnumb'
			.jourgrid.column9.ControlSource = 'jourrecs.ccontra'
*			.jourgrid.column10.ControlSource = 'jourrecs.cvoucherno'
			.command2.Enabled=.T.
        Else
			=sysmsg('Voucher number not found')
			.command2.Enabled=.F.
        Endif
        .Refresh
    Endwith
Endcase
*/
    }

    private void textBox7_Validated(object sender, EventArgs e)
        {
            getClientAccount(textBox7.Text);
            comboBox4.SelectedValue = textBox7.Text;
        }

        private void dateTimePicker1_Validated(object sender, EventArgs e)
        {
            /*
             With Thisform.pageframe1.page1
	If !Empty(This.Value)
		If  !Empty(.text11.Value)
			If .text10.Value>.text11.Value
				lld=.text10.Value
				.text10.Value=.text11.Value
				.text11.Value=lld
			Endif
		Endif
		Thisform.definegrid(1)
		Thisform.getrdata(1)
	Endif
	.Refresh
Endwith

             */
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            /*
             With Thisform.pageframe1.page1
	If !Empty(.text10.Value) And !Empty(.text11.Value)
		If .text10.Value>.text11.Value
			lld=.text10.Value
			.text10.Value=.text11.Value
			.text11.Value=lld
		Endif
	Endif
	Thisform.definegrid(1)
	Thisform.getrdata(1)
	.Refresh
Endwith

             */
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string dsql = "EXEC tsp_AcctTransAll @nCompID,@cAcct,@dFromDate,@dToDate";
            transview.Clear();

            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter glSelCommand = new SqlDataAdapter();
                glSelCommand.SelectCommand = new SqlCommand(dsql, ndConnHandle);
                glSelCommand.SelectCommand.Parameters.Add("@nCompID", SqlDbType.Int).Value = ncompid;
                glSelCommand.SelectCommand.Parameters.Add("@cAcct", SqlDbType.VarChar).Value = textBox7.Text;
                glSelCommand.SelectCommand.Parameters.Add("@dFromDate", SqlDbType.DateTime).Value = Convert.ToDateTime(dfromdate.Text);
                glSelCommand.SelectCommand.Parameters.Add("@dToDate", SqlDbType.DateTime).Value = Convert.ToDateTime(dtodate.Text);
                glSelCommand.SelectCommand.ExecuteNonQuery();

                glSelCommand.Fill(transview);
                if (transview.Rows.Count > 0)
                {
      //              MessageBox.Show("we have found rows ");
                    transGrid.AutoGenerateColumns = false;
                    transGrid.DataSource = transview.DefaultView;
                    transGrid.Columns[0].DataPropertyName = "dpostdate";
                    transGrid.Columns[1].DataPropertyName = "dvaluedate";
                    transGrid.Columns[2].DataPropertyName = "ndebit";
                    transGrid.Columns[3].DataPropertyName = "ncredit";
                    transGrid.Columns[4].DataPropertyName = "nnewbal";
                    transGrid.Columns[5].DataPropertyName = "ctrandesc";

                    for (int i = 0; i < 10; i++)
                    {
                        transview.Rows.Add();
                    }
                    transGrid.Focus();
                }
                ndConnHandle.Close();
            }
        } //

        private void comboBox4_Validated(object sender, EventArgs e)
        {
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox4.Focused)
            {
//                MessageBox.Show("Inside the selected index");
                getClientAccount(Convert.ToString(comboBox4.SelectedValue));
            }
        }

        private void button29_Click(object sender, EventArgs e)
        {
            FindClient fc = new FindClient(cs, ncompid, cloca, 1, "Cusreg");  // string cloc = globalvar.cLocalCaption;
            fc.ShowDialog();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            entAcctView.Clear();
            entTranView.Clear();
            if (radioButton1.Checked) { getEntityCode(1); }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            entAcctView.Clear();
            entTranView.Clear();
            if (radioButton2.Checked) { getEntityCode(2); }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            entAcctView.Clear();
            entTranView.Clear();
            if (radioButton3.Checked) { getEntityCode(3); }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            entAcctView.Clear();
            entTranView.Clear();
            if (radioButton4.Checked) { getEntityCode(4); }
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            entAcctView.Clear();
            entTranView.Clear();
            if (radioButton5.Checked) { getEntityCode(5); }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox3.Focused)
            {
                string entcode = comboBox3.SelectedValue.ToString();
                string sqlcode = 
                    (radioButton1.Checked ? "exec tsp_getIndMembersAccounts_one @ncompid,@entcode" : 
                    (radioButton2.Checked ? "exec tsp_getCorMembersAccounts_one @ncompid,@entcode" : 
                    (radioButton3.Checked ? "exec tsp_getGrpMembersAccounts_one @ncompid,@entcode" :
                    (radioButton4.Checked ? "exec tsp_getSupplierAccounts_one @ncompid,@entcode" :
                    "exec tsp_getStaffAccounts_one @ncompid,@entcode"))));
                getEntityAccounts(sqlcode,entcode);
            }  
        }

        private void acctGrid_Click(object sender, EventArgs e)
        {
            string dacct = entAcctView.Rows[acctGrid.CurrentCell.RowIndex]["cacctnumb"].ToString().Trim();
            getEntityTransaction(dacct);
        }

        private void getEntityTransaction(string acctnumb )
        {
            entTranView.Clear();
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                //************Getting accounts                
                string dsqlstr1 = "exec tsp_Transactions_one @ncompid,@actnumb";
                SqlDataAdapter dasqlAdp1 = new SqlDataAdapter();
                dasqlAdp1.SelectCommand = new SqlCommand(dsqlstr1, ndConnHandle1);
                dasqlAdp1.SelectCommand.Parameters.Add("@ncompid", SqlDbType.Int).Value = ncompid;
                dasqlAdp1.SelectCommand.Parameters.Add("@actnumb", SqlDbType.VarChar).Value = acctnumb;
                dasqlAdp1.Fill(entTranView);
                if (entTranView != null)
                {
                    tranGrid.AutoGenerateColumns = false;
                    tranGrid.DataSource = entTranView.DefaultView;
                    tranGrid.Columns[0].DataPropertyName = "dpostdate";
                    tranGrid.Columns[1].DataPropertyName = "dvaluedate";
                    tranGrid.Columns[2].DataPropertyName = "ndebit";
                    tranGrid.Columns[3].DataPropertyName = "ncredit";
                    tranGrid.Columns[4].DataPropertyName = "nnewbal";
                    tranGrid.Columns[5].DataPropertyName = "ctrandesc";
                    tranGrid.Columns[6].DataPropertyName = "cchqno";
                    tranGrid.Columns[7].DataPropertyName = "cuserid";
                    tranGrid.Columns[8].DataPropertyName = "cvoucherno";
                    ndConnHandle1.Close();
                    for (int i = 0; i < 10; i++)
                    {
                        entTranView.Rows.Add();
                    }
                    tranGrid.Focus();
                }
            }
        }

        private void acctGrid_Enter(object sender, EventArgs e)
        {
            string dacct = entAcctView.Rows[acctGrid.CurrentCell.RowIndex]["cacctnumb"].ToString().Trim();
            getEntityTransaction(dacct);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            entAcctView.Clear();
            entTranView.Clear();
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;
            radioButton5.Checked = false;
            comboBox3.SelectedIndex = -1;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            FindClient fc = new FindClient(cs, ncompid, cloca, 1, "Cusreg"); //string cloc = globalvar.cLocalCaption;
            fc.ShowDialog();
        }

        private void textBox14_Validated(object sender, EventArgs e)
        {
//            MessageBox.Show("inside validation");
            string entcode = textBox14.Text.Trim().PadLeft(6,'0') ;
            comboBox3.SelectedValue = entcode;
            string sqlcode =
                (radioButton1.Checked ? "exec tsp_getIndMembersAccounts_one @ncompid,@entcode" :
                (radioButton2.Checked ? "exec tsp_getCorMembersAccounts_one @ncompid,@entcode" :
                (radioButton3.Checked ? "exec tsp_getGrpMembersAccounts_one @ncompid,@entcode" :
                (radioButton4.Checked ? "exec tsp_getSupplierAccounts_one @ncompid,@entcode" :
                "exec tsp_getStaffAccounts_one @ncompid,@entcode"))));

                getEntityAccounts(sqlcode, entcode);
        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
