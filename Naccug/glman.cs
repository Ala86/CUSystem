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
//using System.Windows.Forms.tr

namespace WinTcare
{
    public partial class glman : Form
    {
        string cs = globalvar.cos;
        int ncompid = globalvar.gnCompid;
        string cloc = globalvar.cLocalCaption;
        DataTable treeview = new DataTable();
        DataTable treeview1 = new DataTable();
        DataTable subtreeview = new DataTable();
        DataTable subtreeview1 = new DataTable();
        DataTable subgrpAcctView = new DataTable();
        string gcSubMain = "";
        public glman()
        {
            InitializeComponent();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void glman_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + "<< General Ledger Management>>";
            fintree.CheckBoxes = true;
            acctGrid.ReadOnly = true;
     //       dtreeview();
            dtreeview1();
            acctGrid.Columns["nbkbal"].SortMode = DataGridViewColumnSortMode.NotSortable;
            acctGrid.Columns["nbkbal"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            finperiod();
        }

        /*       public class myTreeView : TreeView
               {
                   protected override CreateParams CreateParams
                   {
                       get
                       {
                           CreateParams parms = base.CreateParams;
                           parms.Style |= 0x80;
                           return parms;
                           //                    return base.CreateParams;
                       }
                   }
               }*/
               /*
        private void dtreeview()
        {
            fintree.Nodes.Clear();
            string dsql = "select code,categoryname from fincategory order by code ";
           using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(treeview);
                if (treeview.Rows.Count > 0)
                {
                  int nodecount = treeview.Rows.Count;
                    for(int k=0;k<nodecount;k++)
                    {
                        fintree.Nodes.Add(treeview.Rows[k]["categoryname"].ToString());
                        string dnode =treeview.Rows[k]["categoryname"].ToString();
                        subgrptree(Convert.ToInt32(treeview.Rows[k]["code"]), dnode, k);
                    }
                    fintree.ExpandAll();
                    fintree.Nodes[0].EnsureVisible();
                    fintree.ShowNodeToolTips = false; 
                }
            }
        }
        */
        private void dtreeview1()
        {
            treeview1.Clear();
            fintree2.Nodes.Clear();
            string dsql1 = "select code,categoryname from fincategory order by code ";
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da1 = new SqlDataAdapter(dsql1, ndConnHandle);
                da1.Fill(treeview1);
                if (treeview1.Rows.Count > 0)
                {
                    int nodecount = treeview1.Rows.Count;
                    for (int k = 0; k < nodecount; k++)
                    {
                        fintree2.Nodes.Add(treeview1.Rows[k]["categoryname"].ToString());
                        string dnode = treeview1.Rows[k]["categoryname"].ToString();
                        subgrptree1(Convert.ToInt32(treeview1.Rows[k]["code"]), dnode, k);
                    }
                    fintree2.ExpandAll();
                    fintree2.Nodes[0].EnsureVisible();
                    fintree2.ShowNodeToolTips = false;
                }
            }
        }

/*        private void subgrptree(int grpcode,string catname,int dcnt)
        {
            subtreeview.Clear();
            string dsql1 = "select subgrpcode,subgrpname from subgrp where cgrpcode = "+grpcode;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da1 = new SqlDataAdapter(dsql1, ndConnHandle);
                da1.Fill(subtreeview);
                if (subtreeview.Rows.Count > 0)
                {
                    foreach (DataRow dsr in subtreeview.Rows)
                    {
                        string stcode = dsr["subgrpcode"].ToString();
                        string stname = dsr["subgrpname"].ToString();
                        fintree.Nodes[dcnt].Nodes.Add(stcode, stname); 
                    }
                }
            }
        }
        */
        private void subgrptree1(int grpcode, string catname, int dcnt)
        {
            subtreeview1.Clear();
            string dsql13 = "select subgrpcode,subgrpname from subgrp where cgrpcode = " + grpcode;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da13 = new SqlDataAdapter(dsql13, ndConnHandle);
                da13.Fill(subtreeview1);
                if (subtreeview1.Rows.Count > 0)
                {
                    foreach (DataRow dsr in subtreeview1.Rows)
                    {
                        string stcode = dsr["subgrpcode"].ToString();
                        string stname = dsr["subgrpname"].ToString();
                        fintree2.Nodes[dcnt].Nodes.Add(stcode, stname);
                    }
                }
            }
        }

        private void finperiod()
        {
            string dsql1 = "select st_date,ed_date,nperiods,currentperiod from Finperiod where compid = " + ncompid;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da1 = new SqlDataAdapter(dsql1, ndConnHandle);
                DataTable finview = new DataTable();
                da1.Fill(finview);
                if (finview.Rows.Count > 0)
                {
                    textBox1.Text = Convert.ToDateTime(finview.Rows[0]["st_date"]).ToLongDateString();
                    textBox2.Text = Convert.ToDateTime(finview.Rows[0]["ed_date"]).ToLongDateString();
                    textBox3.Text = finview.Rows[0]["currentperiod"].ToString();
                    textBox11.Text = finview.Rows[0]["nperiods"].ToString();
                }//else { MessageBox.Show("nothing is here"); }
            }

        }


        private void subgrpAccounts(int subcode)
        {
            decimal sumtot = 0.00m;
            int rcount = 0;
            subgrpAcctView.Clear();
            //            string dsql12 = "select sum(nbookbal) AS sumtot from subgrp, glmast where subgrp.subgrpcode=glmast.acode and subgrp.subgrpcode= "+subcode;
            string dsql12 = "select cacctnumb,ltrim(rtrim(cacctname)) as cacctname ,nbookbal,nbudamt,ncontamt from subgrp, glmast where subgrp.subgrpcode=glmast.acode and glmast.intcode=1 and subgrp.subgrpcode =" + subcode;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da1 = new SqlDataAdapter(dsql12, ndConnHandle);
                da1.Fill(subgrpAcctView);
                if (subgrpAcctView.Rows.Count > 0)
                {
                    rcount = ((18 - subgrpAcctView.Rows.Count) > 0 ? (18 - subgrpAcctView.Rows.Count) : 0);
                    decimal dbudamt = Convert.ToDecimal(subgrpAcctView.Rows[0]["nbudamt"]);
                    decimal dactamt = Convert.ToDecimal(subgrpAcctView.Rows[0]["ncontamt"]);
                    //                    MessageBox.Show("extra lines " + rcount);
                    acctGrid.AutoGenerateColumns = false;
                    acctGrid.DataSource = subgrpAcctView.DefaultView;
                    acctGrid.Columns[0].DataPropertyName = "cacctnumb";
                    acctGrid.Columns[1].DataPropertyName = "cacctname";
                    acctGrid.Columns[2].DataPropertyName = "nbookbal";
                    for (int i = 0; i < subgrpAcctView.Rows.Count; i++)
                    {
                        sumtot = sumtot + Convert.ToDecimal(subgrpAcctView.Rows[i]["nbookbal"]);
                    }
                    textBox10.Text = sumtot.ToString("N2");
                    textBox6.Text = dbudamt.ToString("N2");     // Convert.ToDecimal(subgrpAcctView.Rows[0]["nbudamt"]).ToString("N2");
                    textBox7.Text = dactamt.ToString("N2");     // Convert.ToDecimal(subgrpAcctView.Rows[0]["ncontamt"]).ToString("N2");
                    textBox8.Text = (dbudamt - dactamt).ToString("N2");
                    textBox9.Text = ((dactamt / dbudamt) * 100).ToString();
                    textBox9.BackColor = ((dactamt / dbudamt) * 100 >= 75.00m ? Color.Yellow : Color.White);
                    textBox9.ForeColor = ((dactamt / dbudamt) * 100 >= 75.00m ? Color.Red : Color.Black);
                    for (int k = 0; k < rcount; k++)
                    {
                        subgrpAcctView.Rows.Add();
                    }
                }
                else
                {
                    for (int k = 0; k < 18; k++)
                    {
                        subgrpAcctView.Rows.Add();
                    }
                }
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            TclassLibrary.acctEnquiry dac = new TclassLibrary.acctEnquiry(cs, ncompid, cloc);
            dac.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            reconcile drec = new reconcile();
            drec.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            acRec ap = new acRec();
            ap.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            acPay ap = new acPay();
            ap.ShowDialog();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            REPTrialBalance trl = new REPTrialBalance();
            trl.ShowDialog();
        }

        private void fintree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            int nodeindex = Convert.ToInt32(e.Node.Name);
            subgrpAccounts(nodeindex);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            TclassLibrary.tranupdate ver = new TclassLibrary.tranupdate(cs, ncompid, cloc, globalvar.gcUserid,globalvar.gnBranchid,globalvar.gnCurrCode,globalvar.gcWorkStation,globalvar.gcWinUser);
            ver.ShowDialog();
        }

        private void fintree2_AfterSelect(object sender, TreeViewEventArgs e)
        {
            int nodeindex = Convert.ToInt32(e.Node.Name);
            gcSubMain = Convert.ToInt32(e.Node.Name).ToString();
            subgrpAccounts(nodeindex);
        }

        private void fintree2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (MessageBox.Show("Do you want to add an account", "Add account information", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
              //  NewAccount addact = new NewAccount(gcSubMain);
              //  addact.ShowDialog();
                subgrpAccounts(Convert.ToInt32(gcSubMain));
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            REPIncomeStatement inc = new REPIncomeStatement();
            inc.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            REPBalanceSheet bal = new REPBalanceSheet();
            bal.ShowDialog();
        }
    }
}
