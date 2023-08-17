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
    public partial class MemberAcct : Form
    {
        string cs = globalvar.cos;
        int ncompid = globalvar.gnCompid;
        string cloca = globalvar.cLocalCaption;
        bool glClientFound = false;
        DataTable currview = new DataTable();
        DataTable brview = new DataTable();
        DataTable prdview = new DataTable();
        DataTable vtable = new DataTable();
        string gcSubMain = string.Empty;
        string ProductControl;
        bool glSubSelected = false;
        bool glBalSheet = false;
        bool glPandL = false;
        bool glAcctFound = true;
        int ala = 0;
        int ala1 = 0;


        public MemberAcct()
        {
            InitializeComponent();
        }

        private void MemberAcct_Load(object sender, EventArgs e)
        {
            this.Text = cloca + "<< Add Member Account >>";
            getproduct();
            getcurrency();
            getBranch();
          //  SMSAccount();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Down || e.KeyCode == Keys.Tab)
            {
                SelectNextControl(ActiveControl, true, true, true, true);
                e.Handled = true;
              //  AllClear2Go();
            }
            else if (e.KeyCode == Keys.Up)
            {
                SelectNextControl(ActiveControl, false, true, true, true);
                e.Handled = true;
             //   AllClear2Go();
            }
        }

        #region Checking if all the mandatory conditions are satisfied
        private void AllClear2Go()
        {
             if (textBox6.Text != "" && textBox3.Text != "")
            {
                saveButton.Enabled = true;
                saveButton.BackColor = Color.LawnGreen;
            }
            else
            {
                saveButton.Enabled = false;
                saveButton.BackColor = Color.Gainsboro;
            }
        }
        #endregion

        private void checkGlDuplicate(string tcAcct)
        {
            //getproduct();
            ala = Convert.ToInt16(comboBox4.SelectedValue);
            ala1 = Convert.ToInt16(comboBox5.SelectedValue);
            string ncompid = globalvar.gnCompid.ToString().Trim();
            string dddsql = "select cacctnumb ,prd_id ,acode from glmast where ccustcode ="+"'" +tcAcct + "' and acode in ('250','270')";
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                DataTable ddvtable = new DataTable();
                SqlDataAdapter ddda = new SqlDataAdapter(dddsql, ndConnHandle);
                ddda.Fill(ddvtable);
                if (ddvtable.Rows.Count > 0 )
                {
                    string lcProductControl = ddvtable.Rows[0]["cacctnumb"].ToString();
                  //  MessageBox.Show("This is you" + lcProductControl);
                    string  lcAcode = lcProductControl.Substring(0, 3);
                    if (ala != 6)   //(ala1.ToString() == "250" || ala1.ToString() == "251")
                    {
                          
                       // string lcLoanAcct = ala1.ToString() == "250" ? getNewAcct.newAcctNumber(cs, membcode.Text.Trim(), "250") : getNewAcct.newAcctNumber(cs, membcode.Text.Trim(), "270");
                         string lcLoanAcct = ddvtable.Rows[0]["cacctnumb"].ToString() != "" ? getNewAcct.newAcctNumber(cs, membcode.Text.Trim(), "250") : textBox5.Text = "01";
                       // MessageBox.Show("This is savingsAccount" + lcLoanAcct);
                        textBox6.Text = lcLoanAcct;  ///getNewAcct.newAcctNumber(cs, membcode.Text.Trim(), "250");
                       textBox5.Text =ala1.ToString();
                    
                    }
                    else
                    {
                        //   MessageBox.Show("This is you" + ala1.ToString());
                        if (ala == 6)
                        {
                            //    MessageBox.Show("This is savings Shares");
                            string lcLoanAcct = ddvtable.Rows[0]["cacctnumb"].ToString() != "" ? getNewAcct.newAcctNumber(cs, membcode.Text.Trim(), "270") : textBox5.Text = "01";
                           // MessageBox.Show("This is savings Shares"+ lcLoanAcct);
                            textBox6.Text = lcLoanAcct;  ///getNewAcct.newAcctNumber(cs, membcode.Text.Trim(), "250");
                            textBox5.Text = ala1.ToString();
                        }

                    }


                    }
                else
                {
                    if (ala1.ToString() == "250")
                    {
                     //   MessageBox.Show("this is not the cirrect place");
                        textBox6.Text = "250" + tcAcct + "01";
                        textBox5.Text = "250";
                    }

                    if (ala1.ToString() == "270")
                        {
                            textBox6.Text = "270" + tcAcct + "01";
                            textBox5.Text = "270";
                    }
                }
            }
        }

        private void getproduct()
        {
            using (SqlConnection dcon = new SqlConnection(cs))
            {
              
                    string dsql0 = "exec TSp_GetDepositProds " + ncompid;
                    SqlDataAdapter da0 = new SqlDataAdapter(dsql0, dcon);
                    DataSet ds0 = new DataSet();
                    da0.Fill(ds0);
                    if (ds0 != null)
                    {
                       
                    //    comboBox4.SelectedIndex = -1;

                    comboBox4.DataSource = ds0.Tables[0];
                    comboBox4.DisplayMember = "prd_name";
                    comboBox4.ValueMember = "prd_id";
                    comboBox4.SelectedIndex = -1;


                    comboBox5.DataSource = ds0.Tables[0];
                    comboBox5.DisplayMember = "prod_type";
                    comboBox5.ValueMember = "prod_type";

                    checkGlDuplicate(membcode.Text);
                }

            }
        }

        private void getcurrency()
        {
            using (SqlConnection dcon = new SqlConnection(cs))
            {
                string dsql1 = "exec tsp_GetCurrency ";
                SqlDataAdapter da1 = new SqlDataAdapter(dsql1, dcon);
                da1.Fill(currview);
                if (currview != null)
                {
                    comboBox2.DataSource = currview.DefaultView;
                    comboBox2.DisplayMember = "curr_name";
                    comboBox2.ValueMember = "curr_code";
                    comboBox2.SelectedIndex = -1;
                }
            }
        }

        private void getBranch()
        {
            using (SqlConnection dcon = new SqlConnection(cs))
            {
                string dsql12 = "exec tsp_getbranch " + ncompid;
                SqlDataAdapter da1 = new SqlDataAdapter(dsql12, dcon);
                da1.Fill(brview);
                if (brview != null)
                {
                    comboBox3.DataSource = brview.DefaultView;
                    comboBox3.DisplayMember = "br_name";
                    comboBox3.ValueMember = "branchid";
                    comboBox3.SelectedIndex = -1;
                }
            }
        }


        private void getacctItem()
        {
            using (SqlConnection dcon = new SqlConnection(cs))
            {
                string dsql21 = "select acct_item from client_code ";
                SqlDataAdapter da21 = new SqlDataAdapter(dsql21, dcon);
                DataTable acctitemview = new DataTable();
                da21.Fill(acctitemview);
                if (acctitemview != null)
                {
                    string itemno = acctitemview.Rows[0]["acct_item"].ToString().Trim().PadLeft(6, '0');
                    textBox1.Text = itemno;
                    textBox2.Text = gcSubMain + itemno;
                    checkAcct(textBox2.Text);
                }
            }
        }

        private void checkAcct(string actnumb)
        {
            using (SqlConnection dcon = new SqlConnection(cs))
            {
                string dsql21 = "select cacctname from glmast where cacctnumb =" + "'" + actnumb + "'";
                SqlDataAdapter da21 = new SqlDataAdapter(dsql21, dcon);
                DataTable actcview = new DataTable();
                da21.Fill(actcview);
                if (actcview != null && actcview.Rows.Count > 0)
                {
                    //                 MessageBox.Show("This account exists and belong to " + actcview.Rows[0]["cacctname"]);
                    updateClient_Code newcl = new updateClient_Code();
                    newcl.updClient(cs, "acct_item");
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void SMSAccount()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                   string tcCode = textBox1.Text.ToString().Trim().PadLeft(6, '0');
            //    MessageBox.Show("This is the Member Code " + tcCode);
                ndConnHandle.Open();
                string dsql2 = "select ccustfname,ccustmname,ccustlname,glmast.ccustcode,glmast.cacctnumb,nbookbal from cusreg, glmast ";
                dsql2 += "where cusreg.ccustcode = glmast.ccustcode and cusreg.ccustcode = " + "'" + membcode.Text + "' ";
                SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle);
                DataTable ds2 = new DataTable();
                da2.Fill(ds2);
                if (ds2 != null)
                {
                    comboBox6.DataSource = ds2.DefaultView;
                    comboBox6.DisplayMember = "cacctnumb";
                    comboBox6.ValueMember = "cacctnumb";
                    comboBox6.SelectedIndex = -1;
                    // textBox2.Text = ds2.Rows[0]["ccustfname"].ToString().Trim() + ' ' + ds2.Rows[0]["ccustlname"].ToString().Trim();
                    //  textBox3.Text = ds2.Rows[0]["cacctnumb"].ToString().Trim();

                }
            }
        }
        private void saveButton_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
                {
                    string cpatsql = "update glmast set sms = 1 where cacctnumb = @membcode";
                    SqlDataAdapter glUpdCommand = new SqlDataAdapter();
                    glUpdCommand.UpdateCommand = new SqlCommand(cpatsql, ndConnHandle3);
                    glUpdCommand.UpdateCommand.Parameters.Add("@membcode", SqlDbType.Char).Value = comboBox6.SelectedValue;

                    ndConnHandle3.Open();
                    glUpdCommand.UpdateCommand.ExecuteNonQuery();
                    ndConnHandle3.Close();
                    string auditDesc = "SMS -> " + membcode.Text.Trim();
                    AuditTrail au = new AuditTrail();
                    au.upd_audit_trail(cs, auditDesc, 0.00m, 0.00m, globalvar.gcUserid, "U", "Old Label", "New Label", "Our remarks", "tcID", globalvar.gcWorkStation, globalvar.gcWinUser);
                    membcode.Focus();
                }
                saveButton.Enabled = false;
                saveButton.BackColor = Color.Gainsboro;
                comboBox6.SelectedIndex = -1;
            }

            else
            {
                using (SqlConnection nConnHandle2 = new SqlConnection(cs))
                {

                    string cglquery = "insert into glmast (cacctnumb,cacctname,dopedate,branchid,ccustcode,cuserid,curcode,acode,compid,prd_id) ";
                    cglquery += " values (@lactnumb,@lactname,convert(date,getdate()),@lnBranchid,@lAcctItem,@lcUserid,@lnCurrCode,@lcSubCatid,@lnCompID,@prd_id)";

                    SqlDataAdapter insCommand = new SqlDataAdapter();
                    insCommand.InsertCommand = new SqlCommand(cglquery, nConnHandle2);
                    insCommand.InsertCommand.Parameters.Add("@lactnumb", SqlDbType.VarChar).Value = textBox6.Text;
                    insCommand.InsertCommand.Parameters.Add("@lactname", SqlDbType.VarChar).Value = textBox3.Text;
                    insCommand.InsertCommand.Parameters.Add("@lnBranchid", SqlDbType.Int).Value = Convert.ToInt32(comboBox3.SelectedValue);
                    insCommand.InsertCommand.Parameters.Add("@lAcctItem", SqlDbType.VarChar).Value = textBox1.Text;
                    insCommand.InsertCommand.Parameters.Add("@lcUserid", SqlDbType.VarChar).Value = globalvar.gcUserid;
                    insCommand.InsertCommand.Parameters.Add("@lnCurrCode", SqlDbType.Int).Value = Convert.ToInt32(comboBox2.SelectedValue);
                    insCommand.InsertCommand.Parameters.Add("@lcSubCatid", SqlDbType.VarChar).Value = textBox5.Text.Trim();
                    insCommand.InsertCommand.Parameters.Add("@lnCompID", SqlDbType.Int).Value = ncompid;
                    // insCommand.InsertCommand.Parameters.Add("@dsubref", SqlDbType.Char).Value  = textBox5.Text.ToString().Trim().PadLeft(2, '0');
                    insCommand.InsertCommand.Parameters.Add("@prd_id", SqlDbType.VarChar).Value = Convert.ToInt32(comboBox4.SelectedValue);

                    nConnHandle2.Open();
                    insCommand.InsertCommand.ExecuteNonQuery();
                    nConnHandle2.Close();
                }
                updateClient_Code updcl = new updateClient_Code();
                updcl.updClient(cs, "acct_item");
                initvariables();
            }
        }
        private void initvariables()
        {
            saveButton.Enabled = false;
            saveButton.BackColor = Color.Gainsboro;
            membcode.Text = "";
            textBox4.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox6.Text = "";
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            comboBox4.SelectedIndex = -1;
        }

        private void membcode_Validated(object sender, EventArgs e)
        {
            if (membcode.Text.ToString().Trim() != null && membcode.Text.ToString().Trim() != "")
            {

                string custcode = Convert.ToString(membcode.Text).Trim();
                string tcCode = custcode.PadLeft(6, '0');// Convert.ToString(membcode.Text).Trim().PadLeft(6, '0');
                membcode.Text = tcCode;
                ala = Convert.ToInt16(comboBox4.SelectedValue);
                ala1 = Convert.ToInt16(comboBox5.SelectedValue);
                checkClient(tcCode);
            }
            AllClear2Go();
        }

        private void checkClient(string tcCode)
        {
            
            string ncompid = globalvar.gnCompid.ToString().Trim();
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                string dsql = "exec tsp_getMembers_one  " + ncompid + ",'" + tcCode + "'";
                //   ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle1);
                DataTable vtable = new DataTable();
                da.Fill(vtable);
                if (vtable.Rows.Count > 0)
                 //  if (vtable != null) 
                {
                   // MessageBox.Show("You are Inside");
                      textBox4.Text = vtable.Rows[0]["membname"].ToString();
                      textBox3.Text = vtable.Rows[0]["membname"].ToString();
                      textBox1.Text = tcCode;
                }
                else
                {
                    MessageBox.Show("Member has been removed, inform IT DEPT immediately");
                    initvariables();
                }
            }
        }

        private void button29_Click(object sender, EventArgs e)
        {
            using (var findform = new FindClient(cs, ncompid, cloca, 1, "Cusreg"))
            {
                var dresult = findform.ShowDialog();
                if (dresult == DialogResult.OK)
                {
                    string dclientcode = findform.returnValue;
                    MessageBox.Show("THE member is  " + dclientcode);

                    string lcClientCode = dclientcode;
                    membcode.Text = lcClientCode;
                    checkClient(lcClientCode);
                }
            }
            AllClear2Go();
        }

        private void comboBox4_SelectedValueChanged(object sender, EventArgs e)
        {
            if(comboBox4.Focused )
            {
                ala = Convert.ToInt16(comboBox4.SelectedIndex);
                ala1 = Convert.ToInt16(comboBox5.SelectedIndex);
                checkGlDuplicate(membcode.Text);
                 AllClear2Go();

                //string als;
                //als = textBox1.Text.ToString().Trim() + textBox5.Text.ToString().Trim();
                //textBox2.Text = als;
                //   MessageBox.Show(als);
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox2.Focused )
            {
                AllClear2Go();
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox3.Focused)
            {
                AllClear2Go();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            acctEnquiry dac = new acctEnquiry(cs, ncompid,cloca);
            dac.ShowDialog();
        }

        private void membcode_TextChanged(object sender, EventArgs e)
        {
           AllClear2Go();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            AllClear2Go();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            AllClear2Go();
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox4.Focused)
            {
                ala = Convert.ToInt16(comboBox4.SelectedIndex);
                ala1 = Convert.ToInt16(comboBox5.SelectedIndex);
                checkGlDuplicate(membcode.Text);
                AllClear2Go();

                //string als;
                //als = textBox1.Text.ToString().Trim() + textBox5.Text.ToString().Trim();
                //textBox2.Text = als;
                //   MessageBox.Show(als);
            }
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox5.Focused)
            {
                ala = Convert.ToInt16(comboBox4.SelectedIndex);
                ala1 = Convert.ToInt16(comboBox5.SelectedIndex);
                checkGlDuplicate(membcode.Text);
                AllClear2Go();
            }
        }

        private void comboBox5_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox5.Focused)
            {
                ala = Convert.ToInt16(comboBox4.SelectedIndex);
                ala1 = Convert.ToInt16(comboBox5.SelectedIndex);
                checkGlDuplicate(membcode.Text);
                AllClear2Go();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            comboBox1.Enabled = false;
            comboBox2.Enabled = false;
            comboBox3.Enabled = false;
            comboBox4.Enabled = false;
            comboBox5.Enabled = false;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            textBox7.Enabled = false;
            comboBox6.Visible = true;
            SMSAccount();
        }
    }
}
