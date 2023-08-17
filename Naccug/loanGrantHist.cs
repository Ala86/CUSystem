using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TclassLibrary;
using System.Data.SqlClient;

namespace WinTcare
{
    public partial class LoanGrantHist : Form
    {
        string cs = globalvar.cos;
        int ncompid = globalvar.gnCompid;
        string dloca = globalvar.cLocalCaption; 

        DataTable clientview = new DataTable();
        DataTable indClientView = new DataTable();
        DataTable guarview = new DataTable();
        string gcMembCode = string.Empty;
        int gnLoanID = 0;
        decimal gnLoanAmt = 0.00m;
        decimal gnGuarAmt = 0.00m;// 0.00m;
        public LoanGrantHist()
        {
            InitializeComponent();
        }


        private void LoanGrantHist_Load(object sender, EventArgs e)
        {
            this.Text = dloca + " << Loan Guarantee History >>";
            clientgrid.Columns["loanAmt"].SortMode = DataGridViewColumnSortMode.NotSortable;
            clientgrid.Columns["loanAmt"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            clientgrid.Columns["appldate"].SortMode = DataGridViewColumnSortMode.NotSortable;
            clientgrid.Columns["appldate"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter ;

            guarGrid.Columns["dloanAmt"].SortMode = DataGridViewColumnSortMode.NotSortable;
            guarGrid.Columns["dloanAmt"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            guarGrid.Columns["dguaramt"].SortMode = DataGridViewColumnSortMode.NotSortable;
            guarGrid.Columns["dguaramt"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            getclientList();
            clientgrid.Focus();
            if (clientview.Rows.Count > 0)
            {
                gcMembCode = Convert.ToString(clientgrid.CurrentRow.Cells["dcuscode"].Value);// clientview.Rows[clientgrid.CurrentCell.RowIndex]["ccustcode"]);
                gnLoanID = Convert.ToInt16(clientgrid.CurrentRow.Cells["loanid"].Value);// Convert.ToInt32(clientview.Rows[clientgrid.CurrentCell.RowIndex]["loan_id"]);
                gnLoanAmt = Convert.ToDecimal(clientgrid.CurrentRow.Cells["loanAmt"].Value);
                getGurantorHist(gnLoanID);
            }
        }

        private void getclientList()
        {
            string dsql = "exec tsp_getLoans4Guarantors  " + ncompid;
            clientview.Clear();

            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(clientview);
                if (clientview.Rows.Count > 0)
                {
                    clientgrid.AutoGenerateColumns = false;
                    clientgrid.DataSource = clientview.DefaultView;
                    clientgrid.Columns[0].DataPropertyName = "ccustcode";
                    clientgrid.Columns[1].DataPropertyName = "membername";
                    clientgrid.Columns[2].DataPropertyName = "principal_amt";
                    clientgrid.Columns[3].DataPropertyName = "loan_appl_date";
                    clientgrid.Columns[4].DataPropertyName = "loan_id";
                    ndConnHandle.Close();
                    clientgrid.Focus();
                    for (int i = 0; i < 5; i++)
                    {
                        clientview.Rows.Add();
                    }
                }
            }
        }//end of getclientlist


        private void getIndclientList(string tccustcode)
        {
            string dsql0 = "exec tsp_getLoans4Guarantors_One  " + ncompid + ",'" + tccustcode + "'";
            indClientView.Clear();
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql0, ndConnHandle);
                da.Fill(indClientView);
                if (indClientView.Rows.Count > 0)
                {
//                    MessageBox.Show("we have found something for the member");
                    clientgrid.AutoGenerateColumns = false;
                    clientgrid.DataSource = indClientView.DefaultView;
                    clientgrid.Columns[0].DataPropertyName = "ccustcode";
                    clientgrid.Columns[1].DataPropertyName = "membername";
                    clientgrid.Columns[2].DataPropertyName = "principal_amt";
                    clientgrid.Columns[3].DataPropertyName = "loan_appl_date";
                    clientgrid.Columns[4].DataPropertyName = "loan_id";
                    ndConnHandle.Close();
                    clientgrid.Focus();
                    for (int i = 0; i < 1; i++)
                    {
                        clientview.Rows.Add();
                    }
                    int dnloanid = Convert.ToInt16(indClientView.Rows[0]["loan_id"]);
                    gnLoanAmt = Convert.ToDecimal(indClientView.Rows[0]["principal_amt"]);
                    getGurantorHist(dnloanid);
                }
            }
        }//end of getclientlist



        //        exec[tsp_getGuarantorHistory] 30, '000012'
        private void getGurantorHist(int dloanid)
        {
            string cs = globalvar.cos;
            string ncompid = globalvar.gnCompid.ToString().Trim();
            string dsqlg = "exec tsp_getGuaranteedSoFar   " + ncompid + "," + dloanid;
            guarview.Clear();
            gnGuarAmt = 0.00m;
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;

            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter dag = new SqlDataAdapter(dsqlg, ndConnHandle);
                dag.Fill(guarview);
                if (guarview.Rows.Count > 0)
                {
                    textBox2.Text = Convert.ToDecimal(guarview.Rows[0]["totgran"]).ToString("N2");
                    gnGuarAmt = Convert.ToDecimal(guarview.Rows[0]["totgran"]);
                    textBox3.Text =(gnGuarAmt>0.00m ? (gnLoanAmt - gnGuarAmt).ToString("N2") :"0.00");
                    guarGrid.AutoGenerateColumns = false;
                    guarGrid.DataSource = guarview.DefaultView;
                    guarGrid.Columns[0].DataPropertyName = "grantorcode";
                    guarGrid.Columns[1].DataPropertyName = "grantor";
                    guarGrid.Columns[2].DataPropertyName = "loanamt";
                    guarGrid.Columns[3].DataPropertyName = "guaramt";
                    guarGrid.Columns[4].DataPropertyName = "guardate";
                    ndConnHandle.Close();
//                    clientgrid.Focus();
                    for (int i = 0; i < 5; i++)
                    {
                        guarview.Rows.Add();
                    }
                }
                else
                {
                    textBox2.Text = "0.00";// gnLoanAmt.ToString("N2");
                    textBox3.Text = gnLoanAmt.ToString("N2");
                }
            }
        }//end of getclientlist

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Down || e.KeyCode == Keys.Tab)
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

        #region Checking if all the mandatory conditions are satisfied
        private void AllClear2Go()
        {
            //if (guarcode.Text.ToString().Trim() != "" && textBox6.Text.ToString().Trim() != "")  

            //{
            //    SaveButton.Enabled = true;
            //    SaveButton.BackColor = Color.LawnGreen;
            //}
            //else
            //{
            //    SaveButton.Enabled = false;
            //    SaveButton.BackColor = Color.Gainsboro;
            //}
        }
        #endregion 

        private void textBox3_Validated(object sender, EventArgs e)
        {

        }

        private void guarcode_Validated(object sender, EventArgs e)
        {
            if (guarcode.Text != "")
            {
                using (SqlConnection ndConnHandle = new SqlConnection(cs))
                {
//                    MessageBox.Show("inside guard code ");
                    string tcCode = guarcode.Text.ToString().Trim().PadLeft(6, '0');
                    guarcode.Text = tcCode;
                    ndConnHandle.Open();
                    string dsql2 = "select ccustfname,ccustmname,ccustlname,glmast.ccustcode,glmast.cacctnumb,nbookbal from cusreg, glmast ";
                    dsql2 += "where cusreg.ccustcode = glmast.ccustcode and left(glmast.cacctnumb, 3) ='250' and cusreg.ccustcode = " + "'" + tcCode + "'";
                    SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle);
                    DataTable ds2 = new DataTable();
                    da2.Fill(ds2);
                    if (ds2 != null && ds2.Rows.Count>0)
                    {
                        string grantor = ds2.Rows[0]["ccustfname"].ToString().Trim() + ' ' + ds2.Rows[0]["ccustmname"].ToString().Trim() + ' ' + ds2.Rows[0]["ccustlname"].ToString().Trim();
                        //textBox1.Text = grantor; 
                        //textBox5.Text = Convert.ToDecimal(ds2.Rows[0]["nbookbal"]).ToString("N2");
                        //textBox6.Enabled = Convert.ToDecimal(ds2.Rows[0]["nbookbal"]) > 0.00m ? true : false;
                        //textBox6.Text = "";
                    } else { MessageBox.Show("No savings account found for this member"); }
                }
         //       getGurantorHist(guarcode.Text);
            }
        }

 

        private void SaveButton_Click(object sender, EventArgs e)
        {
          //  insertgrantor();
          //  gnGuarAmt = gnGuarAmt + Convert.ToDecimal(textBox6.Text);
          //  if(gnGuarAmt>=gnLoanAmt)
          //  {
          ////      MessageBox.Show("we will update lgranteed flag because loan amt =" + gnLoanAmt + " and the guar amt = " + gnGuarAmt);
          //     updateloan();
          //  }
          //  guarcode.Text = "";
          //  textBox1.Text = "";
          //  textBox5.Text = "";
          //  textBox6.Text = "";
          //  textBox7.Text = "";
          //  textBox8.Text = "";
          //  SaveButton.Enabled = false;
          //  SaveButton.BackColor = Color.Gainsboro;
          //  getclientList();
          //  clientgrid.Focus();
          //  if (clientview.Rows.Count > 0)
          //  {
          //      gcMembCode = Convert.ToString(clientview.Rows[clientgrid.CurrentCell.RowIndex]["ccustcode"]);
          //      gnLoanID = Convert.ToInt32(clientview.Rows[clientgrid.CurrentCell.RowIndex]["loan_id"]);
          //      //      MessageBox.Show("we are in");
          //      getGurantorHist(gnLoanID);
          //  }

        }

        private void updateloan()
        {
            using (SqlConnection nConnHandle2 = new SqlConnection(cs))
            {
                    string cglquery = "update loan_det set lgranteed = 1  where loan_id=@loanid";
                    SqlDataAdapter insCommand = new SqlDataAdapter();
                    insCommand.InsertCommand = new SqlCommand(cglquery, nConnHandle2);
                    insCommand.InsertCommand.Parameters.Add("@loanid", SqlDbType.Int).Value = gnLoanID;
                    nConnHandle2.Open();
                    insCommand.InsertCommand.ExecuteNonQuery();
                    nConnHandle2.Close();
            }
        }
        private void insertgrantor()
        {
            //using (SqlConnection nConnHandle2 = new SqlConnection(cs))
            //{

            //    string cglquery = "insert into grantee (ccustcode,loan_id,grantorcode,guaramt,guardate,compid,lgranteed) " ;
            //    cglquery += " values (@lccustcode,@lloan_id,@lgrantorcode,@lguaramt,convert(date,getdate()),@lncompid,@lgranteed)";

            //    SqlDataAdapter insCommand = new SqlDataAdapter();
            //    insCommand.InsertCommand = new SqlCommand(cglquery, nConnHandle2);
            //    insCommand.InsertCommand.Parameters.Add("@lccustcode", SqlDbType.VarChar).Value = gcMembCode;
            //    insCommand.InsertCommand.Parameters.Add("@lloan_id", SqlDbType.Int).Value = gnLoanID;
            //    insCommand.InsertCommand.Parameters.Add("@lgrantorcode", SqlDbType.Char).Value =guarcode.Text ;
            //    insCommand.InsertCommand.Parameters.Add("@lguaramt", SqlDbType.Decimal).Value = Convert.ToDecimal(textBox6.Text) ;
            //    insCommand.InsertCommand.Parameters.Add("@lncompid", SqlDbType.Int).Value =ncompid ;
            //    insCommand.InsertCommand.Parameters.Add("@lgranteed", SqlDbType.Bit).Value = true;

            //    nConnHandle2.Open();
            //    insCommand.InsertCommand.ExecuteNonQuery();
            //    nConnHandle2.Close();

            //    string auditDesc = "Loan Guarantor for  -> " + gcMembCode;
            //    AuditTrail au = new AuditTrail();
            //    au.upd_audit_trail(cs, auditDesc, 0.00m, Convert.ToDecimal(textBox6.Text), globalvar.gcUserid, "U", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);

            //    //                MessageBox.Show("Insert Successful");
            //}
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button29_Click(object sender, EventArgs e)
        {
            FindClient FC = new FindClient(cs,ncompid,dloca,1, "Cusreg");
            FC.ShowDialog();
        }

        private void clientgrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            gcMembCode = Convert.ToString(clientview.Rows[clientgrid.CurrentCell.RowIndex]["ccustcode"]);
            gnLoanID = Convert.ToInt32(clientview.Rows[clientgrid.CurrentCell.RowIndex]["loan_id"]);
            gnLoanAmt = Convert.ToInt32(clientview.Rows[clientgrid.CurrentCell.RowIndex]["principal_amt"]);
            int drowindex = Convert.ToInt16(clientgrid.CurrentCell.RowIndex);
            MessageBox.Show("memcode = " + gcMembCode + " and rowindex is " + drowindex);
            getGurantorHist(gnLoanID);
            clientgrid.EndEdit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("under construction");
            LoanGrantHist lgh = new LoanGrantHist();
            lgh.ShowDialog();
        }

        private void clientgrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            gcMembCode = Convert.ToString(clientgrid.CurrentRow.Cells["dcuscode"].Value); 
            int drowindex = Convert.ToInt16(clientgrid.CurrentCell.RowIndex);
            gnLoanID = Convert.ToInt32(clientgrid.CurrentRow.Cells["loanid"].Value); 
            gnLoanAmt = Convert.ToDecimal(clientgrid.CurrentRow.Cells["loanAmt"].Value);
            getGurantorHist(gnLoanID);
            clientgrid.EndEdit();
        }

        private void clientgrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            clientgrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void textBox10_Validated(object sender, EventArgs e)
        {
            if(textBox10.Text != "")
            {
                textBox10.Text = textBox10.Text.ToString().PadLeft(6, '0');
                getIndclientList(textBox10.Text);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                textBox10.Enabled = false;
                getclientList();
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton2.Checked)
            {
                textBox10.Enabled = true;
                textBox10.Text = ""; 

            }
        }

    }
}
