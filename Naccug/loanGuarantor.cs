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
    public partial class loanGuarantor : Form
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
        decimal ala = 0.00m;
        public loanGuarantor()
        {
            InitializeComponent();
        }


        private void loanGuarantor_Load(object sender, EventArgs e)
        {
            this.Text = dloca + " << Loan Guarantor Setup >>";
            textBox8.Text = globalvar.gdSysDate.ToLongDateString();
            clientgrid.Columns["loanAmt"].SortMode = DataGridViewColumnSortMode.NotSortable;
            clientgrid.Columns["loanAmt"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            clientgrid.Columns["appldate"].SortMode = DataGridViewColumnSortMode.NotSortable;
            clientgrid.Columns["appldate"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter ;

            guarGrid.Columns["dloanAmt"].SortMode = DataGridViewColumnSortMode.NotSortable;
            guarGrid.Columns["dloanAmt"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            guarGrid.Columns["dguaramt"].SortMode = DataGridViewColumnSortMode.NotSortable;
            guarGrid.Columns["dguaramt"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            textBox4.Text = globalvar.gnGuarPercent.ToString("N2");
            getclientList();
            clientgrid.Focus();
            if (clientview.Rows.Count > 0)
            {
                gcMembCode = Convert.ToString(clientgrid.CurrentRow.Cells["dcuscode"].Value);// clientview.Rows[clientgrid.CurrentCell.RowIndex]["ccustcode"]);
                gnLoanID = Convert.ToInt16(clientgrid.CurrentRow.Cells["loanid"].Value);// Convert.ToInt32(clientview.Rows[clientgrid.CurrentCell.RowIndex]["loan_id"]);
                gnLoanAmt = Convert.ToDecimal(clientgrid.CurrentRow.Cells["loanAmt"].Value);
                getGuranteedHist(gnLoanID);
            }
        }

        private void getclientList()
        {
            //string cs = globalvar.cos;
            //string ncompid = globalvar.gnCompid.ToString().Trim();
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
                    getGuranteedHist(dnloanid);
                }
            }
        }//end of getclientlist



        //        exec[tsp_getGuarantorHistory] 30, '000012'
        private void getGuranteedHist(int dloanid)
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


//        exec tsp_getGuarantorHistory 30,'000078'

       private void getGurantorHistory(string tcGuarCode)
        {
            string dsqlg = "exec tsp_getGuarantorHistory   " + ncompid + ",'" + tcGuarCode+"'";
            guarview.Clear();
            gnGuarAmt = 0.00m;
            textBox2.Text = textBox3.Text = string.Empty;

            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter dag = new SqlDataAdapter(dsqlg, ndConnHandle);
                dag.Fill(guarview);
                if (guarview.Rows.Count > 0)
                {
                   textBox3.Text = (gnGuarAmt > 0.00m ? (gnLoanAmt - gnGuarAmt).ToString("N2") : "0.00");
                    guarGrid.AutoGenerateColumns = false;
                    guarGrid.DataSource = guarview.DefaultView;
                    guarGrid.Columns[0].DataPropertyName = "membercode";
                    guarGrid.Columns[1].DataPropertyName = "member";
                    guarGrid.Columns[2].DataPropertyName = "loanamt";
                    guarGrid.Columns[3].DataPropertyName = "guaramt";
                    guarGrid.Columns[4].DataPropertyName = "guardate";
                    ndConnHandle.Close();
                    for (int j = 0; j<guarview.Rows.Count; j++)
                    {
                        gnGuarAmt = gnGuarAmt + Convert.ToDecimal(guarview.Rows[j]["guaramt"]);
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        guarview.Rows.Add();
                    }
                    textBox2.Text = gnGuarAmt.ToString("N2");
                    textBox3.Text = "0.00";
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
            if (guarcode.Text.ToString().Trim() != "" && textBox6.Text.ToString().Trim() != "" && textBox1.Text.ToString().Trim() != "")  

            {
                SaveButton.Enabled = true;
                SaveButton.BackColor = Color.LawnGreen;
            }
            else
            {
                SaveButton.Enabled = false;
                SaveButton.BackColor = Color.Gainsboro;
            }
        }
        #endregion 

        private void textBox3_Validated(object sender, EventArgs e)
        {

        }

        private void guarcode_Validated(object sender, EventArgs e)
        {
            if (guarcode.Text != "" && radioButton4.Checked )  //||
            {
                using (SqlConnection ndConnHandle = new SqlConnection(cs))
                {
                    string tcCode = guarcode.Text.ToString().Trim().PadLeft(6, '0');
                    guarcode.Text = tcCode;
                    ndConnHandle.Open();
                    string dsql2 = "select ccustfname,ccustmname,ccustlname,ccustname,glmast.ccustcode,glmast.cacctnumb,nbookbal from cusreg, glmast ";
                    dsql2 += "where cusreg.ccustcode = glmast.ccustcode and left(glmast.cacctnumb, 3) ='250' and cusreg.ccustcode = " + "'" + tcCode + "'";
                    SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle);
                    DataTable ds2 = new DataTable();
                    da2.Fill(ds2);
                    if (ds2 != null && ds2.Rows.Count > 0)
                    {
                        string grantor = ds2.Rows[0]["ccustfname"].ToString().Trim() + ' ' + ds2.Rows[0]["ccustmname"].ToString().Trim() + ' ' + ds2.Rows[0]["ccustlname"].ToString().Trim() + ' ' + ds2.Rows[0]["ccustname"].ToString().Trim();
                      //  MessageBox.Show("This is the name " + grantor);
                        textBox1.Text = grantor;
                        textBox5.Text = Convert.ToDecimal(ds2.Rows[0]["nbookbal"]).ToString("N2");
                        textBox6.Enabled = Convert.ToDecimal(ds2.Rows[0]["nbookbal"]) > 0.00m ? true : false;
                        textBox6.Text = "";
                        label11.Visible = true;
                    }
                    else { MessageBox.Show("No savings account found for this member"); }
                }
            }
            else if (guarcode.Text != "" && radioButton3.Checked)
            {
                using (SqlConnection ndConnHandle = new SqlConnection(cs))
                {
                    string tcCode = guarcode.Text.ToString().Trim().PadLeft(6, '0');
                    guarcode.Text = tcCode;
                    ndConnHandle.Open();
                    string dsql2 = "select ccustfname,ccustmname,ccustlname,glmast.ccustcode,glmast.cacctnumb,nbookbal from cusreg, glmast ";
                    dsql2 += "where cusreg.ccustcode = glmast.ccustcode and left(glmast.cacctnumb, 3) ='250' and cusreg.ccustcode = " + "'" + tcCode + "'";
                    SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle);
                    DataTable ds2 = new DataTable();
                    da2.Fill(ds2);
                    if (ds2 != null && ds2.Rows.Count > 0)
                    {
                       // string grantor = ds2.Rows[0]["ccustfname"].ToString().Trim() + ' ' + ds2.Rows[0]["ccustmname"].ToString().Trim() + ' ' + ds2.Rows[0]["ccustlname"].ToString().Trim();
                       // textBox1.Text = grantor;
                        textBox5.Text = Convert.ToDecimal(ds2.Rows[0]["nbookbal"]).ToString("N2");
                       // textBox6.Enabled = Convert.ToDecimal(ds2.Rows[0]["nbookbal"]) > 0.00m ? true : false;
                        //textBox6.Text = "";
                        textBox6.Text = gnLoanAmt.ToString("N2");
                    }
                    else { MessageBox.Show("No savings account found for this member"); }
                }
            }
            //if (guarcode.Text != "")
            //{
            //    using (SqlConnection ndConnHandle = new SqlConnection(cs))
            //    {
            //        string tcCode = guarcode.Text.ToString().Trim().PadLeft(6, '0');
            //        guarcode.Text = tcCode;
            //        ndConnHandle.Open();
            //        string dsql2 = "select ccustfname,ccustmname,ccustlname,glmast.ccustcode,glmast.cacctnumb,nbookbal from cusreg, glmast ";
            //        dsql2 += "where cusreg.ccustcode = glmast.ccustcode and left(glmast.cacctnumb, 3) ='250' and cusreg.ccustcode = " + "'" + tcCode + "'";
            //        SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle);
            //        DataTable ds2 = new DataTable();
            //        da2.Fill(ds2);
            //        if (ds2 != null && ds2.Rows.Count>0)
            //        {
            //            string grantor = ds2.Rows[0]["ccustfname"].ToString().Trim() + ' ' + ds2.Rows[0]["ccustmname"].ToString().Trim() + ' ' + ds2.Rows[0]["ccustlname"].ToString().Trim();
            //            textBox1.Text = grantor; 
            //            textBox5.Text = Convert.ToDecimal(ds2.Rows[0]["nbookbal"]).ToString("N2");
            //            textBox6.Enabled = clientview.Rows.Count == 0 ? false : Convert.ToDecimal(ds2.Rows[0]["nbookbal"]) > 0.00m ? true : false;
            //            loanGrant.Enabled = clientview.Rows.Count == 0 ? false : true;
            //            textBox6.Text = "";
            //        } else { MessageBox.Show("No savings account found for this member"); }
            //    }
            //    guarGrid.Columns[0].HeaderText = "Member ID";
            //    guarGrid.Columns[1].HeaderText = "Member Name";
            // //   getGurantorHistory(guarcode.Text.ToString().Trim());
            //    grantHist.Checked = true;
            //}
        }

        private void textBox6_Validated(object sender, EventArgs e)
        {
            if (Convert.ToString(textBox6.Text).Trim() != "")
            {
                if (Convert.ToDecimal(textBox5.Text) > 0.00m)
                {
                    decimal savebal = textBox5.Text.Trim() != "" ? Convert.ToDecimal(textBox5.Text) : 0.00m;
                    decimal guaramt = textBox6.Text.Trim() != "" ? Convert.ToDecimal(textBox6.Text) : 0.00m;
                    decimal guarneed = textBox3.Text.Trim() != "" ? Convert.ToDecimal(textBox3.Text) : 0.00m;

                    if (guaramt > guarneed)
                    {
                        MessageBox.Show("Amount exceeds required amount");
                        textBox6.Text = "";// ''' textBox3.Text;
                        guaramt = 0.00m;// Convert.ToDecimal(textBox3.Text);
                    }


                    if (globalvar.glGrantee2Sav)         //Amount guaranteed is tied to savings balance of grantor
                    {
                        if (guaramt > savebal)
                        {
                            MessageBox.Show("Guarantee Amount " + guaramt + " cannot be more than Savings balance of " + savebal);
                            textBox6.Text = "";
                        }
                        else
                        {
                            textBox6.Text = textBox6.Text.Trim() != "" ? Convert.ToDecimal(textBox6.Text).ToString("N2") : "";
                        }
                    }
                    else
                    {
                        textBox6.Text = Convert.ToDecimal(textBox6.Text).ToString("N2");
                    }
                }
                AllClear2Go();
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            ala = Convert.ToDecimal(textBox11.Text);
            ala = Convert.ToDecimal(textBox6.Text);
            //  textBox11.Text = 0.00m;
            insertgrantor();
            gnGuarAmt = gnGuarAmt + Convert.ToDecimal(textBox6.Text);
            if(gnGuarAmt>=gnLoanAmt)
            {
               updateloan();
            } else
            {
                MessageBox.Show("guaranteed amount "+gnGuarAmt +" is less than loan amount "+gnLoanAmt);
            }

            guarcode.Text = "";
            textBox1.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            radioButton1.Checked = true;
            radioButton2.Checked = false;
            textBox10.Text = "";
            SaveButton.Enabled = false;
            SaveButton.BackColor = Color.Gainsboro;
            getclientList();
            clientgrid.Focus();
            if (clientview.Rows.Count > 0)
            {
                gcMembCode = Convert.ToString(clientview.Rows[clientgrid.CurrentCell.RowIndex]["ccustcode"]);
                gnLoanID = Convert.ToInt32(clientview.Rows[clientgrid.CurrentCell.RowIndex]["loan_id"]);
                getGuranteedHist(gnLoanID);
            }

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
            using (SqlConnection nConnHandle2 = new SqlConnection(cs))
            {

                string cglquery = "insert into grantee (ccustcode,loan_id,grantorcode,guaramt,guardate,compid,lgranteed,collaValue,collaDesc) " ;
                cglquery += " values (@lccustcode,@lloan_id,@lgrantorcode,@lguaramt,convert(date,getdate()),@lncompid,@lgranteed,@collaValue,@collaDesc)";

                SqlDataAdapter insCommand = new SqlDataAdapter();
                insCommand.InsertCommand = new SqlCommand(cglquery, nConnHandle2);
                insCommand.InsertCommand.Parameters.Add("@lccustcode", SqlDbType.VarChar).Value = gcMembCode;
                insCommand.InsertCommand.Parameters.Add("@lloan_id", SqlDbType.Int).Value = gnLoanID;
                insCommand.InsertCommand.Parameters.Add("@lgrantorcode", SqlDbType.Char).Value =guarcode.Text ;
                insCommand.InsertCommand.Parameters.Add("@lguaramt", SqlDbType.Decimal).Value = Convert.ToDecimal(textBox6.Text) ;
                insCommand.InsertCommand.Parameters.Add("@lncompid", SqlDbType.Int).Value =ncompid ;
                insCommand.InsertCommand.Parameters.Add("@lgranteed", SqlDbType.Bit).Value = true;
                insCommand.InsertCommand.Parameters.Add("@collaValue", SqlDbType.Decimal).Value = Convert.ToDecimal(textBox11.Text);
                insCommand.InsertCommand.Parameters.Add("@collaDesc", SqlDbType.Char).Value = textBox1.Text;

                nConnHandle2.Open();
                insCommand.InsertCommand.ExecuteNonQuery();
                nConnHandle2.Close();

                string auditDesc = "Loan Guarantor for  -> " + gcMembCode;
                AuditTrail au = new AuditTrail();
                au.upd_audit_trail(cs, auditDesc, 0.00m, Convert.ToDecimal(textBox6.Text), globalvar.gcUserid, "U", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);
            }
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
            getGuranteedHist(gnLoanID);
            clientgrid.EndEdit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //LoanGrantHist lgh = new LoanGrantHist();
            //lgh.ShowDialog();
        }

        private void clientgrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(Convert.ToString(clientgrid.CurrentRow.Cells[0].Value).ToString().Trim() != "")
            {
                loanGrant.Checked = true;
                grantHist.Checked = false;
                gcMembCode = Convert.ToString(clientgrid.CurrentRow.Cells["dcuscode"].Value);
                int drowindex = Convert.ToInt16(clientgrid.CurrentCell.RowIndex);
                gnLoanID = Convert.ToInt32(clientgrid.CurrentRow.Cells["loanid"].Value);
                gnLoanAmt = Convert.ToDecimal(clientgrid.CurrentRow.Cells["loanAmt"].Value);
                getGuranteedHist(gnLoanID);
                clientgrid.EndEdit();
            }
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
                gcMembCode = textBox10.Text;
                getIndclientList(textBox10.Text);
                gnLoanID = Convert.ToInt16(clientgrid.CurrentRow.Cells["loanid"].Value);
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

        private void loanGrant_CheckedChanged(object sender, EventArgs e)
        {
            if(clientgrid.Rows.Count > 0)
            {
                guarGrid.Columns[0].HeaderText = "Guarantor ID";
                guarGrid.Columns[1].HeaderText = "Guarantor Name";
                gcMembCode = Convert.ToString(clientgrid.CurrentRow.Cells["dcuscode"].Value);
                int drowindex = Convert.ToInt16(clientgrid.CurrentCell.RowIndex);
                gnLoanID = Convert.ToInt32(clientgrid.CurrentRow.Cells["loanid"].Value);
                gnLoanAmt = Convert.ToDecimal(clientgrid.CurrentRow.Cells["loanAmt"].Value);
                getGuranteedHist(gnLoanID);
            }
        }

        private void grantHist_CheckedChanged(object sender, EventArgs e)
        {
            if (guarcode.Text.ToString().Trim() != "")
            {
                guarGrid.Columns[0].HeaderText = "Member ID";
                guarGrid.Columns[1].HeaderText = "Member Name";
                getGurantorHistory(guarcode.Text.ToString().Trim());
            }
        }

        private void guarcode_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            label10.Visible = false;
            textBox1.Enabled = false;
            label12.Visible = false;
            textBox11.Visible = false;
            label11.Visible = true;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
               // string tcCode = guarcode.Text.ToString().Trim().PadLeft(6, '0');
                guarcode.Text = gcMembCode;
                ndConnHandle.Open();
                string dsql2 = "select ccustfname,ccustmname,ccustlname,glmast.ccustcode,glmast.cacctnumb,nbookbal from cusreg, glmast ";
                dsql2 += "where cusreg.ccustcode = glmast.ccustcode and left(glmast.cacctnumb, 3) ='250' and cusreg.ccustcode = " + "'" + gcMembCode + "'";
                SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle);
                DataTable ds2 = new DataTable();
                da2.Fill(ds2);
                if (ds2 != null && ds2.Rows.Count > 0)
                {
                    // string grantor = ds2.Rows[0]["ccustfname"].ToString().Trim() + ' ' + ds2.Rows[0]["ccustmname"].ToString().Trim() + ' ' + ds2.Rows[0]["ccustlname"].ToString().Trim();
                    // textBox1.Text = grantor;
                    textBox5.Text = Convert.ToDecimal(ds2.Rows[0]["nbookbal"]).ToString("N2");
                    // textBox6.Enabled = Convert.ToDecimal(ds2.Rows[0]["nbookbal"]) > 0.00m ? true : false;
                    textBox1.Text = "";
                    textBox6.Text = gnLoanAmt.ToString();
                }
                else { MessageBox.Show("No savings account found for this member"); }
            }
            label10.Visible = true;
            textBox1.Enabled = true;
            guarcode.Enabled = false;
            label12.Visible = true;
            textBox11.Visible = true;
            label13.Visible = true;
            label11.Visible = false;
        }
    }
}
