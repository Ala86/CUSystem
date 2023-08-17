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
    public partial class NewAccount : Form
    {
        string cs = string.Empty;
        int ncompid = 0;
        string cloca = string.Empty;
        string lcUserid = string.Empty;

        DataTable currview = new DataTable();
        DataTable brview = new DataTable();
        string gcSubMain = string.Empty;
        bool glSubSelected = false;
        bool glBalSheet = false;
        bool glPandL = false;
        bool glAcctFound = true;
        public NewAccount(string tcCos, int tnCompid, string tcLoca, string submain,string tcUserid)
        {
            InitializeComponent();
            cs = tcCos;
            ncompid = tnCompid;
            cloca = tcLoca;
            gcSubMain = submain;
            lcUserid = tcUserid;
            if(gcSubMain.Trim() != "")
            {
                glSubSelected = true;// (gcSubMain.Trim() != "" ? true : false);
                glBalSheet = (gcSubMain.Substring(0, 1) == "1" || gcSubMain.Substring(0, 1) == "2" || gcSubMain.Substring(0, 1) == "3" ? true : false);
                glPandL = (gcSubMain.Substring(0, 1) == "4" || gcSubMain.Substring(0, 1) == "7" ? true : false);
                getSubMain(4);
                for(int i=0;i<100;i++)
                {
                    getacctItem();
                    if(!glAcctFound)
                    {
                        break;
                    }
                }
            }
            //            MessageBox.Show("The subgrp in new account is " + gcSubMain);
        }

        private void NewAccount_Load(object sender, EventArgs e)
        {
            this.Text = cloca + " << Account Management >>";
            radioButton9.Enabled = !glSubSelected;
            radioButton10.Enabled = !glSubSelected;
            comboBox1.Visible = !glSubSelected;
            textBox5.Visible = glSubSelected;
            radioButton9.Checked = glBalSheet ? true : false; // (gcSubMain.Substring(0,1))
            radioButton10.Checked = glPandL ? true : false;

            //            MessageBox.Show("The main category is "+gnMainCat);
            getcurrency();
            getBranch();
        }


        // getaccount
        private void getmember(string mcode)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                // string tcCode = mcode.PadLeft(6, '0');// Convert.ToString(membcode.Text).Trim().PadLeft(6, '0');
                //   membcode.Text = tcCode;
                ndConnHandle.Open();
                DataTable ds2 = new DataTable();
             //   if (gcActivityType == "C" || gcActivityType == "I" || gcActivityType == "F" || gcActivityType == "D")
             //   {
                    string dsq12 = "select glmast.cacctnumb,glmast.dopedate ,glmast.cacctname,glmast.nbookbal from glmast where  intcode = 1 and glmast.cacctnumb = " + "'" + textBox7.Text.Trim() + "'";
                    SqlDataAdapter da2 = new SqlDataAdapter(dsq12, ndConnHandle);
                    da2.Fill(ds2);
         //       }
                //else
                //{

                //    string dsq12 = "select glmast.cacctnumb, glmast.cacctname, glmast.nbookbal from glmast where (caccstat='C' or caccstat='I' or caccstat='D' or caccstat='F') and  acode in ('250', '251','270','271') and glmast.cacctnumb = " + "'" + membcode.Text.Trim() + "'";
                //    SqlDataAdapter da2 = new SqlDataAdapter(dsq12, ndConnHandle);
                //    da2.Fill(ds2);
                //}

                if (ds2 != null && ds2.Rows.Count > 0)
                {
                    textBox4.Text = ds2.Rows[0]["cacctname"].ToString();
                    textBox9.Text = Convert.ToDecimal(ds2.Rows[0]["nbookbal"]).ToString("N2");
                    textBox8.Text = Convert.ToDateTime(ds2.Rows[0]["dopedate"]).ToString(); 
                }
                else
                {
                    //                    MessageBox.Show("Activity type is "+gcActivityType );
                    MessageBox.Show("Account not found or May have a Balance ");
                    textBox7.Text = "";
                    textBox7.Focus();
                }
                //  AllClear2Go();
            }
        }


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
            if (textBox2.Text != "" && textBox3.Text !="")
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


        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radioButton9_Click(object sender, EventArgs e)
        {
            if(radioButton9.Checked)
            {
                radioButton7.Checked = true;
                radioButton1.Checked = false;
                radioButton2.Checked = false;
                getSubMain(1);
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
                string dsql12 = "exec tsp_getbranch "+ncompid;
                SqlDataAdapter da1 = new SqlDataAdapter(dsql12, dcon);
                da1.Fill(brview);
                if (brview != null)
                {
                    comboBox3.DataSource = brview.DefaultView;
                    comboBox3.DisplayMember = "br_name";
                    comboBox3.ValueMember   = "branchid";
                    comboBox3.SelectedIndex = -1;
                }
            }
        }

        private void getSubMain(int dtype)
        {
            using (SqlConnection dcon = new SqlConnection(cs))
            {
                switch (dtype)
                {
                    case 1:
                        string dsql1 = "select code, cgrpcode,subgrpcode,subgrpname from finCategory,subgrp where finCategory.code = subgrp.cgrpcode and  cgrpcode in (1,2,3) order by code,subgrpcode ";
                        SqlDataAdapter da1 = new SqlDataAdapter(dsql1, dcon);
                        DataTable balview = new DataTable();
                        da1.Fill(balview);
                        if (balview != null)
                        {
                            comboBox1.DataSource = balview.DefaultView;
                            comboBox1.DisplayMember = "subgrpname";
                            comboBox1.ValueMember = "subgrpcode";
                            comboBox1.SelectedIndex = -1;
                        }
                        break;
                    case 2:
                        string dsql2 = "select code, cgrpcode,subgrpcode,subgrpname from finCategory,subgrp where finCategory.code = subgrp.cgrpcode and  cgrpcode not in (1,2,3) order by code,subgrpcode ";
                        SqlDataAdapter da2 = new SqlDataAdapter(dsql2, dcon);
                        DataTable proview = new DataTable();
                        da2.Fill(proview);
                        if (proview != null)
                        {
                            comboBox1.DataSource = proview.DefaultView;
                            comboBox1.DisplayMember = "subgrpname";
                            comboBox1.ValueMember = "subgrpcode";
                            comboBox1.SelectedIndex = -1;
                        }
                        break;
                    case 3:
                        string dsql3 = "select code, cgrpcode,subgrpcode,subgrpname from finCategory,subgrp where finCategory.code = subgrp.cgrpcode and  lbank =1 ";
                        SqlDataAdapter da3 = new SqlDataAdapter(dsql3, dcon);
                        DataTable bnkview = new DataTable();
                        da3.Fill(bnkview);
                        if (bnkview != null)
                        {
                            comboBox1.DataSource = bnkview.DefaultView;
                            comboBox1.DisplayMember = "subgrpname";
                            comboBox1.ValueMember = "subgrpcode";
                            comboBox1.SelectedIndex = -1;
                        }
                        break;
                    case 4:
//                        MessageBox.Show("case 4");
                        string dsql4 = "select code, cgrpcode,subgrpcode,subgrpname from finCategory,subgrp where finCategory.code = subgrp.cgrpcode and subgrpcode= " + gcSubMain; 
                        SqlDataAdapter da4 = new SqlDataAdapter(dsql4, dcon);
                        DataTable sgrview = new DataTable();
                        da4.Fill(sgrview);
                        if (sgrview != null)
                        {
//                            comboBox1.DataSource = sgrview.DefaultView;
  //                          comboBox1.DisplayMember = "subgrpname";
    //                        comboBox1.ValueMember = "subgrpcode";
      //                      comboBox1.SelectedIndex = -1;
                            textBox5.Text = sgrview.Rows[0]["subgrpname"].ToString();
                        }
                        break;
                }
            }
        }//end of getSubMain



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
                    string itemno = acctitemview.Rows[0]["acct_item"].ToString().Trim().PadLeft(6,'0');
                    textBox1.Text = itemno; 
                    textBox2.Text = gcSubMain + itemno+"01";
                    checkAcct(textBox2.Text);
                }
            }
        }

        private void checkAcct(string actnumb)
        {
            using (SqlConnection dcon = new SqlConnection(cs))
            {
                string dsql21 = "select cacctname from glmast where cacctnumb ="+"'"+actnumb+"'";
                SqlDataAdapter da21 = new SqlDataAdapter(dsql21, dcon);
                DataTable actcview = new DataTable();
                da21.Fill(actcview);
                if (actcview != null && actcview.Rows.Count>0)
                {
   //                 MessageBox.Show("This account exists and belong to " + actcview.Rows[0]["cacctname"]);
                    updateClient_Code newcl = new updateClient_Code();
                    newcl.updClient(cs, "acct_item");
                }
     //           else { MessageBox.Show("This account does not exist, you may continue");glAcctFound = false; }
            }
        }


        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void radioButton1_Click(object sender, EventArgs e)
        {
            radioButton9.Checked = true;
            radioButton10.Checked = false;
            getSubMain(3);
        }

        private void radioButton10_Click(object sender, EventArgs e)
        {
            if (radioButton10.Checked)
            {
                radioButton7.Checked = true;
                radioButton1.Checked = false;
                radioButton2.Checked = false;
                getSubMain(2);
                getbankBranch();
            }
        }

        private void getbankBranch()
        {
            using (SqlConnection dcon = new SqlConnection(cs))
            {
                string dsql21q = "select branchid, bnkbr_name,addr,tel,email  from bnk_branch";
                SqlDataAdapter da21q = new SqlDataAdapter(dsql21q, dcon);
                DataTable actcviewq = new DataTable();
                da21q.Fill(actcviewq);
                if (actcviewq != null && actcviewq.Rows.Count > 0)
                {
                    comboBox3.DataSource = actcviewq.DefaultView;
                    comboBox3.DisplayMember = "bnkbr_NAME";
                    comboBox3.ValueMember = "branchid";
                    comboBox3.SelectedIndex = -1;
                }
            }

        }
        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            using (SqlConnection nConnHandle2 = new SqlConnection(cs))
            {

                string cglquery = "insert into glmast (cacctnumb,intcode,cacctname,dopedate,branchid,ccustcode,cuserid,curcode,acode,compid) ";
                cglquery += " values (@lactnumb,1,@lactname,convert(date,getdate()),@lnBranchid,@lAcctItem,@lcUserid,@lnCurrCode,@lcSubCatid,@lnCompID)";

                SqlDataAdapter insCommand = new SqlDataAdapter();
                insCommand.InsertCommand = new SqlCommand(cglquery, nConnHandle2);
                insCommand.InsertCommand.Parameters.Add("@lactnumb", SqlDbType.VarChar).Value = textBox2.Text;
                insCommand.InsertCommand.Parameters.Add("@lactname", SqlDbType.VarChar).Value = textBox3.Text;
                insCommand.InsertCommand.Parameters.Add("@lnBranchid", SqlDbType.Int).Value = Convert.ToInt32(comboBox3.SelectedValue);
                insCommand.InsertCommand.Parameters.Add("@lAcctItem", SqlDbType.VarChar).Value = textBox1.Text; 
                insCommand.InsertCommand.Parameters.Add("@lcUserid", SqlDbType.VarChar).Value = lcUserid;
                insCommand.InsertCommand.Parameters.Add("@lnCurrCode", SqlDbType.Int).Value = Convert.ToInt32(comboBox2.SelectedValue);
                insCommand.InsertCommand.Parameters.Add("@lcSubCatid", SqlDbType.VarChar).Value = gcSubMain;
                insCommand.InsertCommand.Parameters.Add("@lnCompID", SqlDbType.Int).Value = ncompid;

                nConnHandle2.Open();
                insCommand.InsertCommand.ExecuteNonQuery();
                nConnHandle2.Close();
            }
            updateClient_Code updcl = new updateClient_Code();
            updcl.updClient(cs, "acct_item");
            initvariables();
        }

        private void initvariables()
        {
            saveButton.Enabled = false;
            saveButton.BackColor = Color.Gainsboro;
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox5.Text = "";
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton9.Checked = false;
            radioButton10.Checked = false;
            if(glSubSelected)
            {
                this.Close();
            }
        }
        private void textBox3_Validated(object sender, EventArgs e)
        {
            AllClear2Go();
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if(comboBox1.Focused)
            {
                gcSubMain = comboBox1.SelectedValue.ToString();
//                MessageBox.Show("the subgrp is " + gcSubMain);
                getSubMain(4);
                for (int i = 0; i < 100; i++)
                {
                    getacctItem();
                    if (!glAcctFound)
                    {
                        break;
                    }
                }
            }
        }

        private void radioButton7_Click(object sender, EventArgs e)
        {
            getBranch();
        }

        private void radioButton2_Click(object sender, EventArgs e)
        {
            getBranch();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
            {
                string cpatsql =  "update glmast set cacctname = '"+ textBox4.Text + "' where cacctnumb = @membcode";
                SqlDataAdapter glUpdCommand = new SqlDataAdapter();
                glUpdCommand.UpdateCommand = new SqlCommand(cpatsql, ndConnHandle3);
                glUpdCommand.UpdateCommand.Parameters.Add("@membcode", SqlDbType.Char).Value = textBox7.Text;

                ndConnHandle3.Open();
                glUpdCommand.UpdateCommand.ExecuteNonQuery();
                ndConnHandle3.Close();


                //string auditDesc = (gcActivityType == "C" ? "Account Closure  -> " : "Account Activate  -> ") + membcode.Text.Trim();
                //AuditTrail au = new AuditTrail();
                //au.upd_audit_trail(cs, auditDesc, 0.00m, 0.00m, globalvar.gcUserid, "U", "Old Label", "New Label", "Our remarks", "tcID", globalvar.gcWorkStation, globalvar.gcWinUser);
                textBox7.Focus();
            }
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox4.Text = "";
        }

        private void textBox7_Validated(object sender, EventArgs e)
        {
            getmember(textBox7.Text.Trim());
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

