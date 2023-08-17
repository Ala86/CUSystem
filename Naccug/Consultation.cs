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
    public partial class Consultation : Form
    {
        int gnVisno = 0;
        DataTable clientview = new DataTable();
        DataTable icdview = new DataTable();
        DataTable labview = new DataTable();
        DataTable radview = new DataTable();
        DataTable optview = new DataTable();
        DataTable pharmaitems = new DataTable();
        DataTable sympview = new DataTable();
        DataTable treeview = new DataTable();
        DataTable selTestView = new DataTable();
        DataTable selExamView = new DataTable();
        DataTable selProcView = new DataTable();
        DataTable tempLabView = new DataTable();
        DataTable selDiagView = new DataTable();

        DataTable selSympView = new DataTable();

        temporary_files tempfiles = new temporary_files();

        bool glLabUpdated = false;
        bool glRadUpdated = false;
        bool glOptUpdated = false;
        bool glFreeBee = false;
        bool glCliUpdated = false;
        bool glSpeUpdated = false;
        bool glSent2Adm = false;
        bool glPayDrugSelected = false;
        bool glSent2Pha = false;
        bool glSent2Lab = false;
        bool glSent2Rad = false;
        bool glSent2Opt = false;
        bool glDiagSelected = false;
        int gnTempLabTestID = 0;
        int gnTempRadTestID = 0;
        int gnTempOptTestID = 0;
        string gcReq_Numb = "";
        DateTime gdVisDate = new DateTime();





        int currentPharmaRow;
        int currentPharmaCol;

        public Consultation()
        {
            InitializeComponent();

/*            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            int w = Width >= screen.Width ? screen.Width : (screen.Width + Width) / 4;
            int h = Height >= screen.Height ? screen.Height : (screen.Height + Height) / 4;
            this.Location = new Point((screen.Width - w) / 4, (screen.Height - h) / 4);
            this.Size = new Size(w, h);*/
        }


        public class myTreeView :TreeView
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
        }
        private void dtreeview()
        {
            string cs = globalvar.cos;
            string ncompid = globalvar.gnCompid.ToString().Trim();
            string dsql = "select * from body_system order by bod_name ";
            //signTree = new TreeView();

            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
//                DataTable treeview = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(treeview);
                if (treeview.Rows.Count > 0)
                {
                    foreach (DataRow dr in treeview.Rows)
                    {
                        signTree.Nodes["bodynode"].Nodes.Add(dr["bod_name"].ToString());
                    }
                    signTree.ExpandAll();
                    signTree.Nodes[0].EnsureVisible();
                    signTree.ShowNodeToolTips = false;
                }
                else { MessageBox.Show("No records found in Body Systems, inform IT DEPT immediately"); }
            }
        }

        private void getCurrentVisitDate(string tcCode)
        {
            string cs = globalvar.cos;
            //            string tcCode =textBox4.Text;
            string ncompid = globalvar.gnCompid.ToString().Trim();
            string dsql = "exec sp_getCurrentVisit  " + ncompid + ",'" + tcCode + "'";
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
        private void Consultation_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + " << Consultation >>";
            label36.Text = globalvar.gcCopyRight;
            this.label38.MaximumSize = new Size(50, 0);
            this.label38.AutoSize = true;

            DataColumn dc = new DataColumn("selDiagName");
            selDiagView.Columns.Add(dc);
            dc = new DataColumn("selDiagCode");
            selDiagView.Columns.Add(dc);

            //            this.Location = new Point(0, 0);
            //          this.Size = Screen.PrimaryScreen.WorkingArea.Size;
            //            this.Size = Screen.PrimaryScreen.WorkingArea.
            selDiagGrid.DataSource = selDiagView.DefaultView;
            getclientList(globalvar.gnQueueID);
            admcombos();
            firstclient();
//            icd10(); we will call this one from inside the search textbox in Diagnosis screen
            labtest();
            radtest();
            opttest();
            druglist();
            dtreeview();
            symptomtype();
            textBox29.Text = DateTime.Now.ToLongDateString();
            tabPage7.Focus();
        }


        
        private void admcombos()
        {
            string cs = globalvar.cos;
            int ncompid = globalvar.gnCompid;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();

                //************Getting Client status
                string dsql = "exec sp_ClientStatus "+ncompid;
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                DataSet ds = new DataSet();
                da.Fill(ds);
                if (ds != null)
                {
                    comboBox2.DataSource = ds.Tables[0];
                    comboBox2.DisplayMember = "cls_name";
                    comboBox2.ValueMember = "cls_id";
                }
                else { MessageBox.Show("Could not get Client Status Types, inform IT Dept immediately"); }


                //************Admission Type
                string dsql1 = "exec tsp_AdmissionType " + ncompid;
                SqlDataAdapter da1 = new SqlDataAdapter(dsql1, ndConnHandle);
                DataSet ds1 = new DataSet();
                da1.Fill(ds1);
                if (ds1 != null)
                {
                    comboBox3.DataSource = ds1.Tables[0];
                    comboBox3.DisplayMember = "adm_name";
                    comboBox3.ValueMember = "adm_code";
                }
                else { MessageBox.Show("Could not get Admission Types, inform IT Dept immediately"); }


                //************Package Type
                string dsql2 = "exec tsp_GetPackage " + ncompid;
                SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle);
                DataSet ds2 = new DataSet();
                da2.Fill(ds2);
                if (ds2 != null)
                {
                    comboBox4.DataSource = ds2.Tables[0];
                    comboBox4.DisplayMember = "pack_name";
                    comboBox4.ValueMember = "pack_code";
                }
                else { MessageBox.Show("Could not get package Types, inform IT Dept immediately"); }
                
            }
        }
        private void symptomtype()
        {
       //     string[] dsymp = { "Chronic", "Relapsing", "Remitting" };
            string[] symps = new string[3];
            symps[0] = "Chronic";
            symps[1] = "Relapsing";
            symps[2] = "Remitting";
            symtype.DataSource = symps;
        }
        private void getclientList(int qid)
        {
            string cs = globalvar.cos;
            string ncompid = globalvar.gnCompid.ToString().Trim();
            string dsql = "exec tsp_Normal_Clients  " + ncompid + "," + qid;
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
                    clientgrid.Columns[0].DataPropertyName = "fname";
                    clientgrid.Columns[1].DataPropertyName = "mname";
                    clientgrid.Columns[2].DataPropertyName = "lname";
                    clientgrid.Columns[3].DataPropertyName = "visdate";      // "age";
                    clientgrid.Columns[4].DataPropertyName = "vistime";
                    clientgrid.Columns[5].DataPropertyName = "visno";
                    clientgrid.Columns[6].DataPropertyName = "ccustcode";
                    ndConnHandle.Close();
                    clientgrid.Focus();
                    for (int i = 0; i < 10; i++)
                    {
                        //                        DataGridViewRow drow = new  DataGridViewRow();
                        clientview.Rows.Add();
                    }
                }
            }
        }//end of getclientlist

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Down || e.KeyCode == Keys.Tab)
            {
                SelectNextControl(ActiveControl, true, true, true, true);
                e.Handled = true;
//                AllClear2Go();
            }
            else if (e.KeyCode == Keys.Up)
            {
                SelectNextControl(ActiveControl, false, true, true, true);
                e.Handled = true;
  //              AllClear2Go();
            }
        }

        private void firstclient()
        {
            if (clientview.Rows.Count > 0)
            {
                DataRow drow = clientview.Rows[clientgrid.CurrentCell.RowIndex];
                textBox5.Text = drow["ccustcode"].ToString();
                gnVisno = Convert.ToInt16(drow["visno"]);
               string dcust = drow["ccustcode"].ToString();
            }
        }//firstclient

        private void icd10(bool licd,string searchkey)
        {
            string cs = globalvar.cos;
            string ncompid = globalvar.gnCompid.ToString().Trim();
//            string dsql = "exec tsp_ICD10NewList  ";
            string dsql = (licd ? "exec tsp_ICD10Search  " : "exec tsp_LocalDiagSearch  ") +"'"+searchkey+"'";
            icdview.Clear();

            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(icdview);
                if (icdview.Rows.Count > 0)
                {
                    icdGrid.AutoGenerateColumns = false;
                    icdGrid.DataSource = icdview.DefaultView;
                    icdGrid.Columns[0].DataPropertyName = "icd_name";
                    icdGrid.Columns[1].DataPropertyName = "icd_sel";
                    icdGrid.Columns[2].DataPropertyName = "icd_code";
                    ndConnHandle.Close();
  //                  clientgrid.Focus();
                    for (int i = 0; i < 10; i++)
                    {
                        //                        DataGridViewRow drow = new  DataGridViewRow();
                        icdview.Rows.Add();
                    }
                    textBox27.Text = icdview.Rows.Count.ToString();
                }
            }

        }//end of icd10

 
        private void labtest()
        {
            string cs = globalvar.cos;
            string ncompid = globalvar.gnCompid.ToString().Trim();
            string dsql = "exec tsp_LFees  " + ncompid;
            labview.Clear();
    //        bool tsel = false;

            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(labview);
                if (labview.Rows.Count > 0)
                {
                    labGrid.AutoGenerateColumns = false;
                    labGrid.DataSource = labview.DefaultView;
                    labGrid.Columns[0].DataPropertyName = "servce_name";
                    labGrid.Columns[1].DataPropertyName = "servce_fee";
                    labGrid.Columns[2].DataPropertyName = "tsel";
                    labGrid.Columns[3].DataPropertyName = "indic";
                    labGrid.Columns[4].DataPropertyName = "srv_code";
                    labGrid.Columns[5].DataPropertyName = "service_id";
                    labGrid.Columns[6].DataPropertyName = "spe_id";
                    labGrid.Columns["servFee"].SortMode = DataGridViewColumnSortMode.NotSortable;
                    labGrid.Columns["servFee"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                    ndConnHandle.Close();
    //                clientgrid.Focus();
                    for (int i = 0; i < 6; i++)
                    {
                        //                        DataGridViewRow drow = new  DataGridViewRow();
                        labview.Rows.Add();
                    }
                }
            }

        }// end of labtest


        private void radtest()
        {
            string cs = globalvar.cos;
            string ncompid = globalvar.gnCompid.ToString().Trim();
            string dsql = "exec tsp_RFees  " + ncompid;
            radview.Clear();

            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(radview);
                if (radview.Rows.Count > 0)
                {
                    radGrid.AutoGenerateColumns = false;
                    radGrid.DataSource = radview.DefaultView;
                    radGrid.Columns[0].DataPropertyName = "servce_name";
                    radGrid.Columns[1].DataPropertyName = "servce_fee";
                    radGrid.Columns[2].DataPropertyName = "tsel";
                    radGrid.Columns[3].DataPropertyName = "indic";
                    radGrid.Columns[4].DataPropertyName = "srv_code";
                    radGrid.Columns[5].DataPropertyName = "service_id";
                    radGrid.Columns["examFee"].SortMode = DataGridViewColumnSortMode.NotSortable;
                    radGrid.Columns["examFee"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                    ndConnHandle.Close();
//                    clientgrid.Focus();
                    for (int i = 0; i < 6; i++)
                    {
                        //                        DataGridViewRow drow = new  DataGridViewRow();
                        radview.Rows.Add();
                    }
                }
            }

        }


        private void opttest()
        {
            string cs = globalvar.cos;
            string ncompid = globalvar.gnCompid.ToString().Trim();
            string dsql = "exec tsp_otFees  " + ncompid;
            optview.Clear();

            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(optview);
                if (optview.Rows.Count > 0)
                {
                    optGrid.AutoGenerateColumns = false;
                    optGrid.DataSource = optview.DefaultView;
                    optGrid.Columns[0].DataPropertyName = "servce_name";
                    optGrid.Columns[1].DataPropertyName = "servce_fee";
                    optGrid.Columns[2].DataPropertyName = "tsel";
                    optGrid.Columns[3].DataPropertyName = "srv_code";
                    optGrid.Columns[4].DataPropertyName = "service_id";
                    optGrid.Columns["ProcFee"].SortMode = DataGridViewColumnSortMode.NotSortable;
                    optGrid.Columns["ProcFee"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                    ndConnHandle.Close();
                    clientgrid.Focus();
                    for (int i = 0; i < 6; i++)
                    {
                        //                        DataGridViewRow drow = new  DataGridViewRow();
                        optview.Rows.Add();
                    }
                }
            }

        }

        private void druglist()
        {
            string cs = globalvar.cos;
            string ncompid = globalvar.gnCompid.ToString().Trim();
            string dsql = "exec tsp_PhaInStock  " + ncompid;
            pharmaitems.Clear();
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(pharmaitems);
                if (pharmaitems.Rows.Count > 0)
                {
                    PharmaGrid.AutoGenerateColumns = false;
                    PharmaGrid.DataSource = pharmaitems.DefaultView;
                    PharmaGrid.Columns[0].DataPropertyName = "prod_name";
                    PharmaGrid.Columns[1].DataPropertyName = "quantity";
                    PharmaGrid.Columns[2].DataPropertyName = "qty";
                    PharmaGrid.Columns[3].DataPropertyName = "Perday";
                    PharmaGrid.Columns[4].DataPropertyName = "Days";
                    PharmaGrid.Columns[5].DataPropertyName = "Issue";
                    PharmaGrid.Columns[6].DataPropertyName = "cashpay";
                    PharmaGrid.Columns[7].DataPropertyName = "Amount";
                    PharmaGrid.Columns[8].DataPropertyName = "dosage";
                    PharmaGrid.Columns[9].DataPropertyName = "prod_code";
//                    PharmaGrid.Columns[10].DataPropertyName = false.ToString();
                    PharmaGrid.Columns["unitPrice"].SortMode = DataGridViewColumnSortMode.NotSortable;
                    PharmaGrid.Columns["costAmt"].SortMode = DataGridViewColumnSortMode.NotSortable;
                    PharmaGrid.Columns["unitPrice"].HeaderCell.Style.Alignment =  DataGridViewContentAlignment.MiddleRight;
                    PharmaGrid.Columns["costAmt"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                    
                    ndConnHandle.Close();
                    clientgrid.Focus();
                    //foreach()
                    //                    pharmaitems.Columns["Amount"].he
                    for (int i = 0; i < 6; i++)
                    {
                        pharmaitems.Rows.Add();
                    }
                }
            }
        }


        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
        // /   this.tabControl1.Visible = false;
        //    this.dataGridView6.Visible = true;
          //  this.listView2.Visible = true;
          //  this.groupBox1.Visible = true;
           // this.richTextBox1.Visible = true;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
//            this.tabControl1.Visible = true;
  //          this.dataGridView6.Visible = false;
    //        this.listView2.Visible = false;
      //      this.groupBox1.Visible = false;
        //    this.richTextBox1.Visible = false;
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
         //   this.tabControl1.Visible = true;
  
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
  //          this.richTextBox1.Visible = false;
//**            this.listView2.Visible = true;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
    //        this.richTextBox1.Visible = true;
      //      this.listView2.Visible = false;
        }

        private void label38_Click(object sender, EventArgs e)
        {
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabPage18_Click(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void PharmaGrid_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            int rowi = PharmaGrid.CurrentCell.RowIndex; ;
            int coli = PharmaGrid.CurrentCell.ColumnIndex;  //.CurrentCell.RowIndex; 


            currentPharmaRow = rowi;
            currentPharmaCol = coli;
            if (e.KeyCode == Keys.Enter)
            {
                if (PharmaGrid.CurrentRow.Index > 0)
                {
                    currentPharmaRow--;
                }
                else
                {
                    currentPharmaRow = 0;
                    currentPharmaCol = 0;
                }
            }
            else
            {
                //   MessageBox.Show("You have another key previewkeydown with position " + rowi + "," + coli);
            }

        }

        private void PharmaGrid_KeyUp(object sender, KeyEventArgs e)
        {
//            PharmaGrid.EndEdit();
            //{
            if (e.KeyCode == Keys.Enter)
            {
               PharmaGrid.CurrentCell = PharmaGrid[currentPharmaCol + 1, currentPharmaRow];
                PharmaGrid.CurrentRow.Cells[5].Value = Convert.ToDecimal(PharmaGrid.CurrentRow.Cells[2].Value) * Convert.ToDecimal(PharmaGrid.CurrentRow.Cells[3].Value)
                    * Convert.ToDecimal(PharmaGrid.CurrentRow.Cells[4].Value);
                PharmaGrid.CurrentRow.Cells[7].Value = Convert.ToDecimal(PharmaGrid.CurrentRow.Cells[5].Value) * Convert.ToDecimal(PharmaGrid.CurrentRow.Cells[6].Value);
                if (PharmaGrid.CurrentRow.Cells["issueAmt"]!=null && Convert.ToInt16(PharmaGrid.CurrentRow.Cells["issueAmt"].Value)>0)
                {
                    string cs = globalvar.cos;
                    string tcode = textBox5.Text.ToString();
                    string lcprodname = PharmaGrid.CurrentRow.Cells["prod_name"].Value.ToString();
                    string lcProdCode = PharmaGrid.CurrentRow.Cells["prod_code"].Value.ToString(); // drow.Cells[10].ToString();
                    decimal lnUnitPrice = (Convert.ToDecimal(PharmaGrid.CurrentRow.Cells["unitPrice"].Value.ToString()) > 0.00m ? Convert.ToDecimal(PharmaGrid.CurrentRow.Cells["unitPrice"].Value) : 0.00m);
                    int ndays = Convert.ToInt16(PharmaGrid.CurrentRow.Cells["dDays"].Value);
                    int lnPerday = Convert.ToInt16(PharmaGrid.CurrentRow.Cells["perday"].Value);
                    int lnQty = Convert.ToInt16(PharmaGrid.CurrentRow.Cells["qty"].Value.ToString());
                    int nIssue = Convert.ToInt16(PharmaGrid.CurrentRow.Cells["issueAmt"].Value.ToString());
                    string ddosage = (PharmaGrid.CurrentRow.Cells["ddosage"].Value != null ? PharmaGrid.CurrentRow.Cells["ddosage"].Value.ToString().Trim() : "");

                    if (Convert.ToInt16(PharmaGrid.CurrentRow.Cells["issueAmt"].Value) > 0)
                    {
                      //  tempfiles.temporary_files_update_pharmacy(cs, lnUnitPrice, lcProdCode, lnQty, nIssue, ndays, lnPerday, ddosage, tcode, gnVisno, false, true);
                    }
                }
            }
        }



        private void signTree_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Parent != null)
            {
                if (e.Node.Parent.Checked == true)
                {
                    if (e.Node.Checked) // || e.Node.IsSelected)
                    {
                        signTree.SelectedNode = e.Node;
                        e.Node.Parent.BackColor = Color.Blue;
                        e.Node.Parent.BackColor = Color.White;
                        string dind = e.Node.Index.ToString();
                        string dnode = treeview.Rows[e.Node.Index]["bod_id"].ToString();
                        int bod_id = Convert.ToInt32(treeview.Rows[e.Node.Index]["bod_id"]);
                        e.Node.BackColor = Color.Green;
                        e.Node.ForeColor = Color.White;
                        sympview.Clear();
                        string cs = globalvar.cos;
                        string ncompid = globalvar.gnCompid.ToString().Trim();
                        string dsql = "exec tsp_GetSymptoms " + bod_id;      // ncompid;
                        using (SqlConnection ndConnHandle = new SqlConnection(cs))
                        {
                            ndConnHandle.Open();
                            SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                            da.Fill(sympview);
                            if (sympview.Rows.Count > 0)
                            {
                                symptomGrid.AutoGenerateColumns = false;
                                symptomGrid.DataSource = sympview.DefaultView;
                                symptomGrid.Columns[0].DataPropertyName = "symptomname".Trim();
                                ndConnHandle.Close();
                            }
                        }
                    }
                    else
                    {
                        if(e.Node.Checked==false)
                        {
                            e.Node.BackColor = Color.White;
                            e.Node.ForeColor = Color.Black;
                            sympview.Clear();
                            signTree.SelectedNode = e.Node;
              //              e.Node.BackColor = Color.White;
                //            e.Node.ForeColor = Color.Red;
                        }
                    }
                }
                else //parent not checked
                {
                    if (e.Node.Checked == true)
                    {
                        e.Node.Checked = false;
                    }
                }
            }
            else //parent is null, this is the root node
            {
                signTree.SelectedNode = e.Node;
            }
        }//end of aftercheck


        private static string getreqnumb(string cs, DateTime dsysdate)
        {
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
            {
                string sreq = "select req_numb from patient_code";
                string crequest = "";
                int rcounter;
                SqlDataAdapter requestview = new SqlDataAdapter(sreq, ndConnHandle3);
                ndConnHandle3.Open();
                DataTable reqView = new DataTable();
                requestview.Fill(reqView);
                if (reqView != null)
                {
                    rcounter = Convert.ToInt32(reqView.Rows[0]["req_numb"]);
                    if (Convert.ToUInt32(reqView.Rows[0]["req_numb"]) >= 9999)
                    {
                        crequest = dsysdate.Year.ToString().Substring(2, 2) + dsysdate.Month.ToString().PadLeft(2, '0') + dsysdate.Day.ToString().PadLeft(2, '0') + "0001";
                        return crequest;
                    }
                    else
                    {
                        crequest = dsysdate.Year.ToString().Substring(2, 2) + dsysdate.Month.ToString().PadLeft(2, '0') + dsysdate.Day.ToString().PadLeft(2, '0') + rcounter.ToString().PadLeft(4, '0');
                        return crequest;
                    }
                }
                else { return ""; }
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            string cs = globalvar.cos;
            ServiceRequestNumber dserv = new ServiceRequestNumber();
            gcReq_Numb = dserv.reqnumb(cs, globalvar.gdSysDate);
            getCurrentVisitDate(textBox5.Text.ToString());
            string tcCode = textBox5.Text.ToString();
            //int nvisno = Convert.ToInt16(getVisitNumber.visitno(cs, globalvar.gnCompid.ToString(), textBox5.Text));

            updateDashBoard dDashBoard = new updateDashBoard();
            temporary_files dtemp = new temporary_files();


 
            //Laboratory Tests to be updated
            glLabUpdated = false;
            if (labview.Rows.Count > 0)
            {
                foreach (DataGridViewRow dr in labGrid.Rows)
                {
                    if (dr.Cells["testSelect"].Value != null)
                        if (Convert.ToBoolean(dr.Cells["testSelect"].Value))
                        {
                            glLabUpdated = true;
                            break;
                        }
                }
            }

            //Radiology examinations to be updated
            glRadUpdated = false;
            if (radview.Rows.Count > 0)
            {
                foreach (DataGridViewRow dr in radGrid.Rows)
                {
                    if (dr.Cells["examSelect"].Value != null)
                        if (Convert.ToBoolean(dr.Cells["examSelect"].Value))
                        {
                            glRadUpdated = true;
                            break;
                        }
                }
            }

            //Operating theatre procedures  to be updated
            glOptUpdated = false;
            if (optview.Rows.Count > 0)
            {
                foreach (DataGridViewRow dr in optGrid.Rows)
                {
                    if (dr.Cells["procSelect"].Value != null)
                        if (Convert.ToBoolean(dr.Cells["procSelect"].Value))
                        {
                            glOptUpdated = true;
                            break;
                        }
                }
            }

            //Pharmacy drugs to be updated
            glPayDrugSelected = false;
            if (pharmaitems.Rows.Count > 0)
            {
                foreach (DataGridViewRow dr in PharmaGrid.Rows)
                {
                    if (dr.Cells["issueAmt"].Value != null)
                        if (Convert.ToInt16(dr.Cells["issueAmt"].Value)>0)
                        {
                            glPayDrugSelected = true;
                            break;
                        }
                }
            }

            //diag_rept to update diagnosis report

            if (glLabUpdated == true)           //		If glLabUpdated						&&glSent2Lab - Transaction Code is 02
            {
                updateLabTest();                            
                updateLabTestItems();                      
                updateaccounts(1);  
                updatecodes(1); 
                glFreeBee = false;
                dDashBoard.updDashBoard();
            }

            if (glRadUpdated == true)      //If glRadUpdated						&&glSent2Rad - Transaction Code is 02
            {
                updateRadExam();       
                updateRadExamItems(); 
                updateaccounts(2);  
                updatecodes(2);     
                glFreeBee = false;
                dDashBoard.updDashBoard();
            }

            if (glOptUpdated == true)      //	If glOptUpdated						&&glSent2Opt - Transaction Code is 02
            {
                updateOptProc();     
                updateOptProcItems();       
                updateaccounts(3);   
                updatecodes(3);      
                glFreeBee = false;
                dDashBoard.updDashBoard();
            }

            if (glPayDrugSelected == true)     //	  Drugs have been prescribed
            {
                // glFreeBee = Iif(glGloNoPre,.T.,.F.)
                if (glSent2Adm == true)          //    && Drugs on admission
                {
                    glSent2Pha = true;     // glSent2Pha =.T.
                    admissiondrugs();     //  .admissiondrugs
                }
                else                            //   && Out patient drugs
                {
                    glSent2Pha = true;      //  glSent2Pha =.T.
                    drugdispense();         //  .drugdispense
                    upddispense();          //  .upddispense
                }
                glFreeBee = false;            //            glFreeBee =.F.
                dDashBoard.updDashBoard();
            }
            else
            {
                glSent2Pha = false;                //        glSent2Pha =.F.
            }



            if (glCliUpdated == true)      //		If glCliUpdated						&&sent to another clinic
            {
                //                *******************parameters to update the dashboard
                //            gcDuptype = 'O1'
                //            gnCliRequest = Iif(glSent2Cli, 1, 0)
                updateaccounts(4);      //          .updateaccounts(4) && update accounts(glmast and tranhist)
            }


            if (glSpeUpdated == true)      //		                If glSpeUpdated						&& sent to specialist
            {
                //                *******************parameters to update the dashboard
                //                            gcDuptype = 'O1'
                //                            gnSpeRequest = Iif(glSent2Spe, 1, 0)
                updateaccounts(5);  //                            .updateaccounts(5) && update accounts(glmast and tranhist)
            }


            if (glSent2Adm == true)        //                If glSent2Adm						&&Client sent to admission
            {
                //*******************parameters to update the dashboard
                //          gcDuptype='A1'
                send2admit();       //			                .send2admit
            }



            bool llDiagOnly = (!glLabUpdated && !glRadUpdated && !glOptUpdated && !glCliUpdated && !glSpeUpdated && !glPayDrugSelected && !glSent2Adm ? true : false);

            if (llDiagOnly)                   // If llDiagOnly                   && Diagnosis only
            {
                if (MessageBox.Show("Only Diagnosis has been done for this client." + "\n" + " Do you want to discharge client ? " + "\n" + "[Yes] to discharge, [No] to return to Consultation Screen", "Diagnosis only alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    using (SqlConnection ndConnHandle = new SqlConnection(cs))
                    {
                        diag_rept();    //save the diagnosis
                                        //   ps = SQLExec(gnConnHandle, 'update pat_visit set activesession=0,complain=?lcComplain,findings=?lcFindings,clinician=?gnIntDocID where ccustcode=?gcCustCode and activesession=1  and visno=?gnVisNo', 'actview')
                                        //sp =  SQLExec(gnConnHandle, 'update TodayVisit set activesession=0,complain=?lcComplain,findings=?lcFindings,clinician=?gnIntDocID where ccustcode=?gcCustCode and activesession=1 and visno=?gnVisNo', 'actview')
                        string cquery0 = "update pat_visit set activesession=0,clinician=@gnIntDocID where ccustcode=@gcCustCode and activesession=1  and visno=@gnVisNo";
                        string cquery01 = "update todayvisit set activesession=0,clinician=@gnIntDocID where ccustcode=@gcCustCode and activesession=1  and visno=@gnVisNo";


                        SqlDataAdapter patupd0 = new SqlDataAdapter();
                        patupd0.UpdateCommand = new SqlCommand(cquery0, ndConnHandle);

                        SqlDataAdapter todupd0 = new SqlDataAdapter();
                        todupd0.UpdateCommand = new SqlCommand(cquery01, ndConnHandle);

                        ndConnHandle.Open();

                        patupd0.UpdateCommand.Parameters.Add("@gnIntDocID", SqlDbType.Int).Value = globalvar.gnIntDocID;
                        patupd0.UpdateCommand.Parameters.Add("@gcCustCode", SqlDbType.Char).Value = tcCode;
                        //patupd0.UpdateCommand.Parameters.Add("@gnVisNo", SqlDbType.Int).Value = nvisno;

                        todupd0.UpdateCommand.Parameters.Add("@gnIntDocID", SqlDbType.Int).Value = globalvar.gnIntDocID;
                        todupd0.UpdateCommand.Parameters.Add("@gcCustCode", SqlDbType.Char).Value = tcCode;
                        //todupd0.UpdateCommand.Parameters.Add("@gnVisNo", SqlDbType.Int).Value = nvisno;

                        patupd0.UpdateCommand.ExecuteNonQuery();
                        todupd0.UpdateCommand.ExecuteNonQuery();
                        ndConnHandle.Close();
                    }
                }else { MessageBox.Show("We will stay in the consultation screen"); }
            }
            else
            {
//                string cs = globalvar.cos;
                using (SqlConnection ndConnHandle = new SqlConnection(cs))
                {
//                    string tcCode = textBox5.Text;
//                    int nvisno = Convert.ToInt16(getVisitNumber.visitno(cs, globalvar.gnCompid.ToString(), textBox5.Text));
                    string tcAcctNumb = globalvar.ClientAcctPrefix + textBox5.Text.ToString();

                    string cquery = "update pat_visit set clinician=@gnIntDocID,sent2lab=@glLabUpdated,sent2Rad=@glRadUpdated,sent2Opt=@glOptUpdated,sent2cli=@glCliUpdated,sent2Admit=@glSent2Adm," ;
                    cquery+="consdate=convert(date,getdate()),constime=convert(time,getdate()) where ccustcode=@gcCustCode and activesession=1 and visno=@gnVisNo";

                    string cquery1 = "update pat_visit set clinician=@gnIntDocID,sent2lab=@glLabUpdated,sent2Rad=@glRadUpdated,sent2Opt=@glOptUpdated,sent2cli=@glCliUpdated,sent2Admit=@glSent2Adm,";
                    cquery1 += "consdate=convert(date,getdate()),constime=convert(time,getdate()) where ccustcode=@gcCustCode and activesession=1 and visno=@gnVisNo";

                    SqlDataAdapter patupd = new SqlDataAdapter();
                    patupd.UpdateCommand = new SqlCommand(cquery, ndConnHandle);

                    SqlDataAdapter todupd = new SqlDataAdapter();
                    todupd.UpdateCommand = new SqlCommand(cquery1, ndConnHandle);

                    ndConnHandle.Open();

                    patupd.UpdateCommand.Parameters.Add("@gnIntDocID", SqlDbType.Int).Value = globalvar.gnIntDocID;
                    patupd.UpdateCommand.Parameters.Add("@glLabUpdated", SqlDbType.Bit).Value = glLabUpdated;
                    patupd.UpdateCommand.Parameters.Add("@glRadUpdated", SqlDbType.Bit).Value = glRadUpdated;
                    patupd.UpdateCommand.Parameters.Add("@glOptUpdated", SqlDbType.Bit).Value = glOptUpdated;
                    patupd.UpdateCommand.Parameters.Add("@glCliUpdated", SqlDbType.Bit).Value = glCliUpdated;
                    patupd.UpdateCommand.Parameters.Add("@glSent2Adm", SqlDbType.Bit).Value = glSent2Adm;
                    patupd.UpdateCommand.Parameters.Add("@gcCustCode", SqlDbType.Char).Value = tcCode;
                    //patupd.UpdateCommand.Parameters.Add("@gnVisNo", SqlDbType.Int).Value = nvisno;

                    todupd.UpdateCommand.Parameters.Add("@gnIntDocID", SqlDbType.Int).Value = globalvar.gnIntDocID;
                    todupd.UpdateCommand.Parameters.Add("@glLabUpdated", SqlDbType.Bit).Value = glLabUpdated;
                    todupd.UpdateCommand.Parameters.Add("@glRadUpdated", SqlDbType.Bit).Value = glRadUpdated;
                    todupd.UpdateCommand.Parameters.Add("@glOptUpdated", SqlDbType.Bit).Value = glOptUpdated;
                    todupd.UpdateCommand.Parameters.Add("@glCliUpdated", SqlDbType.Bit).Value = glCliUpdated;
                    todupd.UpdateCommand.Parameters.Add("@glSent2Adm", SqlDbType.Bit).Value = glSent2Adm;
                    todupd.UpdateCommand.Parameters.Add("@gcCustCode", SqlDbType.Char).Value = tcCode;
                    //todupd.UpdateCommand.Parameters.Add("@gnVisNo", SqlDbType.Int).Value = nvisno;

                    patupd.UpdateCommand.ExecuteNonQuery();
                    todupd.UpdateCommand.ExecuteNonQuery();
                    ndConnHandle.Close();
                    diag_rept(); //save diagnosis details
                }

            }



            //            if (glCliUpdated || glSent2Adm || glSent2Lab || glSent2Rad || glSent2Pha || glSent2Opt || glDiagSelected)
            if (glDiagSelected || glLabUpdated || glRadUpdated || glOptUpdated || glCliUpdated || glSpeUpdated || glPayDrugSelected || glSent2Adm)
            {
                updatetodayq(); //                                  .updatetodayq && update today's queue since the session for this consultant is complete
                                //.updatedashboard
            }
            temporary_files_clear();        //                                         Thisform.temporary_files_clear 			&&clear temporary files
            icdview.Clear();
            //dtemp.temporary_files_clear(cs, tcCode, nvisno);
            getclientList(globalvar.gnQueueID);
//            icd10();
            labtest();
            radtest();
            opttest();
            druglist();
//            symptomtype();

        }//END of saveButton

        private void diag_rept()
        {
            //            MessageBox.Show("We will save the diagnosis");
            /*
             *Select ICDView
            *Set Filter To icd_sel
            With .pageframe1.page10		&&complaints & findings
                lcComplain=Alltrim(.edit1.Value)
                lcFindings=Alltrim(.edit5.Value)
            Endwith
            Select selDiagitems
            Set Filter To
            Locate
            Do While !Eof()
                lcCode=icdno		&&icd_code
            ***************We will revisit this since there could be multiple prov/final diagnoses
                If !glFinalDiag				&&Provisional Diagnosis
                    sn=SQLExec(gnConnHandle,"insert into diag_rept (dr_id,ccustcode,icd_code,provdiag,diag_date,diag_time,visno,compid,complain,findings) "+;
                        "values (?gnIntDocID,?gcCustCode,?lcCode,?lcCode,convert(date,getdate()),convert(time,getdate()),?gnVisNo,?gnCompid,?lcComplain,?lcFindings)","UPDDIS")
                    If !(sn>0)
                        =sysmsg("Could not insert Provisional Diagnosis, inform IT Dept")
                    Endif
                Else						&&Final Diagnosis
                    sn=SQLExec(gnConnHandle,"insert into diag_rept (dr_id,ccustcode,icd_code,finadiag,diag_date,diag_time,visno,compid,finaldiag,complain,findings) "+;
                        "values (?gnIntDocID,?gcCustCode,?lcCode,?lcCode,convert(date,getdate()),convert(time,getdate()),?gnVisNo,?gnCompid,1,?lcComplain,?lcFindings)","UPDDIS")
                    If !(sn>0)
                        =sysmsg("Could not insert Provisional Diagnosis, inform IT Dept")
                    Endif
                Endif
                Select selDiagitems 			&&ICDView
                Skip
            Enddo
            Select ICDView
            Set Filter To
            Replace All icd_sel With .F.
            Locate
            Select selDiagitems
            Set Filter To
            Zap
                         */
            string cs1 = globalvar.cos;
            int nfcompid = globalvar.gnCompid;
            //string ncompid = globalvar.gnCompid.ToString();
            //int nvisno = Convert.ToInt16(getVisitNumber.visitno(cs1, ncompid, textBox5.Text));
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs1))
            {
                string cDiaquery = "insert into diag_rept (dr_id,ccustcode,icd_code,provdiag,diag_date,diag_time,visno,compid) " ;
                cDiaquery += "values (@gnIntDocID,@gcCustCode,@lcCode,@lcCode,convert(date,getdate()),convert(time,getdate()),@gnVisNo,@gnCompid)";
                SqlDataAdapter diaCommand = new SqlDataAdapter();
                diaCommand.InsertCommand = new SqlCommand(cDiaquery, ndConnHandle3);
                diaCommand.InsertCommand.Parameters.Add("@gnIntDocID", SqlDbType.Int).Value = globalvar.gnIntDocID;
                diaCommand.InsertCommand.Parameters.Add("@gcCustCode", SqlDbType.VarChar).Value = textBox5.Text.ToString();

                diaCommand.InsertCommand.Parameters.Add("@gnTempLabTestID", SqlDbType.Int).Value = gnTempLabTestID;
                diaCommand.InsertCommand.Parameters.Add("@gcReq_Numb", SqlDbType.VarChar).Value = gcReq_Numb;   
                diaCommand.InsertCommand.Parameters.Add("@gcUserid", SqlDbType.VarChar).Value = globalvar.gcUserid;
                diaCommand.InsertCommand.Parameters.Add("@gnCompid", SqlDbType.Int).Value = globalvar.gnCompid;
                diaCommand.InsertCommand.Parameters.Add("@gnDocQueueID", SqlDbType.Int).Value = globalvar.gnQueueID;
                //diaCommand.InsertCommand.Parameters.Add("@gnVisNo", SqlDbType.Int).Value = nvisno;
                diaCommand.InsertCommand.Parameters.Add("@gcLabReason", SqlDbType.Char).Value = "This is the lab reason";
                ndConnHandle3.Open();
                diaCommand.InsertCommand.ExecuteNonQuery();
                ndConnHandle3.Close();
            }

        }

        private void UpdateSymtoms()
        {

        }
        private void getnewidnumb(int tntime)
        {
            string cs1 = globalvar.cos;
            int ncompid = globalvar.gnCompid;
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs1))
            {

                switch (tntime)
                {
                    case 1:        //Case tntime = 1 && labtest            ;
                        ndConnHandle3.Open();
                        string dsql = "select tes_id from patient_code";
                        SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle3);
                        DataTable ds = new DataTable();
                        da.Fill(ds);
                        if (ds.Rows.Count > 0)
                        {
                           gnTempLabTestID = Convert.ToInt16(ds.Rows[0]["tes_id"]);
                        }
                            break;
                    case 2:         //Case tntime = 2 && radexam
                        ndConnHandle3.Open();
                        string dsql1 = "select exa_id  from patient_code";
                        SqlDataAdapter da1 = new SqlDataAdapter(dsql1, ndConnHandle3);
                        DataTable ds1 = new DataTable();
                        da1.Fill(ds1);
                        if (ds1.Rows.Count > 0)
                        {
                            gnTempRadTestID = Convert.ToInt16(ds1.Rows[0]["exa_id"]);
                        }
                        break;
                    case 3:         // Case tntime = 3 && opt proc 
                        ndConnHandle3.Open();
                        string dsql2 = "select pro_id  from patient_code";
                        SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle3);
                        DataTable ds2 = new DataTable();
                        da2.Fill(ds2);
                        if (ds2.Rows.Count > 0)
                        {
                            gnTempOptTestID = Convert.ToInt16(ds2.Rows[0]["pro_id"]);
                        }
                        break;
                }
            }
        }

   
        private void updateLabTest ()   //.updateLabTest					&&update labtest from temporary table=templabtest on the backend
        {
            getnewidnumb(1);                                //.getnewidnumb (1)
            string cs1 = globalvar.cos;
            int nfcompid = globalvar.gnCompid;
            string ncompid = globalvar.gnCompid.ToString();
            //int nvisno = Convert.ToInt16(getVisitNumber.visitno(cs1, ncompid, textBox5.Text));
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs1))
            {
               string cpatquery = "insert into labtest (tes_id,ccustcode,dr_id,req_numb,req_type,req_by,req_date,req_time,sent_by,sent_date,sent_time,cuserid,compid,srv_id,visno,labreason)" ;
                cpatquery += "values (@gnTempLabTestID,@gcCustCode,@gnIntDocID,@gcReq_Numb,2,@gnIntDocID,convert(date,getdate()),convert(time,getdate()),@gnIntDocID,convert(date,getdate()),convert(time,getdate()),@gcUserid,@gnCompid,@gnDocQueueID,@gnVisNo,@gcLabReason)";
                SqlDataAdapter labCommand = new SqlDataAdapter();
                labCommand.InsertCommand = new SqlCommand(cpatquery, ndConnHandle3);
                labCommand.InsertCommand.Parameters.Add("@gnTempLabTestID", SqlDbType.Int).Value = gnTempLabTestID;
                labCommand.InsertCommand.Parameters.Add("@gcCustCode", SqlDbType.VarChar).Value = textBox5.Text.ToString();
                labCommand.InsertCommand.Parameters.Add("@gnIntDocID", SqlDbType.Int).Value =globalvar.gnIntDocID ;
                labCommand.InsertCommand.Parameters.Add("@gcReq_Numb", SqlDbType.VarChar).Value = gcReq_Numb;  // tcReqNumb;    //  getreqnumb(cs1, globalvar.gdSysDate);     //  TclassLibrary. "2019_06_12";   //  TclassLibrary.  TclassLibrary.re genrequest.getreqnumb(cs1,globalvar.gdSysDate);
                labCommand.InsertCommand.Parameters.Add("@gcUserid", SqlDbType.VarChar).Value = globalvar.gcUserid;      
                labCommand.InsertCommand.Parameters.Add("@gnCompid", SqlDbType.Int).Value = globalvar.gnCompid;
                labCommand.InsertCommand.Parameters.Add("@gnDocQueueID", SqlDbType.Int).Value = globalvar.gnQueueID;
                //labCommand.InsertCommand.Parameters.Add("@gnVisNo", SqlDbType.Int).Value = nvisno;
               labCommand.InsertCommand.Parameters.Add("@gcLabReason", SqlDbType.Char).Value ="This is the lab reason";
                ndConnHandle3.Open();
                labCommand.InsertCommand.ExecuteNonQuery();
                ndConnHandle3.Close();
            }

        }

        private void updateLabTestItems()  //.updateLabTestItems             &&update labtestitems from temporaray table= templabtestitems on the backend
        {
            if (labview.Rows.Count > 0)
            {
                string cs1 = globalvar.cos;
                int nfcompid = globalvar.gnCompid;
                string ncompid = globalvar.gnCompid.ToString();
                using (SqlConnection ndConnHandle3 = new SqlConnection(cs1))
                {
                    string cpatquery = "insert into labtestitems(tes_id, item_name, itemNo, total_cost, compid, vatamt, req_type, req_by, req_date, req_time, cuserid, lcovered, labreason, srv_code, spectype)" ;
                    cpatquery += "values (@gnTestItemID,@lcItem_name,@lnItemNo,@lnPrice,@gnCompid,@lnVatAmt,@lnreq_type,@gnIntDocID,convert(date,getdate()),convert(time,getdate()),@gcUserid,@lcov,@lcindic,@lcsrv,@lcSpecType)";
                    ndConnHandle3.Open();
                    foreach (DataGridViewRow dr in labGrid.Rows)
                    {
                        if (dr.Cells["testSelect"].Value != null)
                            if (Convert.ToBoolean(dr.Cells["testSelect"].Value)==true)
                            {
                                SqlDataAdapter labCommandt = new SqlDataAdapter();
                                labCommandt.InsertCommand = new SqlCommand(cpatquery, ndConnHandle3);
                                labCommandt.InsertCommand.Parameters.Add("@gnTestItemID", SqlDbType.Int).Value = gnTempLabTestID;
                                labCommandt.InsertCommand.Parameters.Add("@lcItem_name", SqlDbType.VarChar).Value = dr.Cells["itemName"].Value.ToString().Trim();
                                labCommandt.InsertCommand.Parameters.Add("@lnItemNo", SqlDbType.Int).Value = dr.Cells["service_id"].Value.ToString().Trim();
                                labCommandt.InsertCommand.Parameters.Add("@lnPrice", SqlDbType.Decimal).Value = dr.Cells["servFee"].Value.ToString().Trim();
                                labCommandt.InsertCommand.Parameters.Add("@gnCompid", SqlDbType.Int).Value = globalvar.gnCompid;
                                labCommandt.InsertCommand.Parameters.Add("@lnVatAmt", SqlDbType.Decimal).Value = 0.00m;
                                labCommandt.InsertCommand.Parameters.Add("@lnreq_type", SqlDbType.Int).Value = 2;
                                labCommandt.InsertCommand.Parameters.Add("@gnIntDocID", SqlDbType.Int).Value = globalvar.gnIntDocID;
                                labCommandt.InsertCommand.Parameters.Add("@gcUserid", SqlDbType.VarChar).Value = globalvar.gcUserid;
                                labCommandt.InsertCommand.Parameters.Add("@lcov", SqlDbType.Bit).Value = false; ;
                                labCommandt.InsertCommand.Parameters.Add("@lcindic", SqlDbType.Char).Value = "This is the indication";
                                labCommandt.InsertCommand.Parameters.Add("@lcsrv", SqlDbType.Char).Value = dr.Cells["srv_code"].Value.ToString().Trim();
                                labCommandt.InsertCommand.Parameters.Add("@lcSpecType", SqlDbType.Int).Value = dr.Cells["spe_id"].Value.ToString().Trim();

                                labCommandt.InsertCommand.ExecuteNonQuery();
                                labCommandt.InsertCommand.Parameters.Clear();
                            }
                    }
                    ndConnHandle3.Close();
                }
            }
        }

        private void updateRadExam()        // .updateRadExam 					&&update radtest from temporary table=tempradtest on the backend
        {
            getnewidnumb(2);                                //.getnewidnumb (2)
            string cs1 = globalvar.cos;
            int nfcompid = globalvar.gnCompid;
            string ncompid = globalvar.gnCompid.ToString();
            //int nvisno = Convert.ToInt16(getVisitNumber.visitno(cs1, ncompid, textBox5.Text));
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs1))
            {
                string cpatquery = "insert into radtest (tes_id,ccustcode,dr_id,req_numb,req_type,req_by,req_date,req_time,sent_by,sent_date,sent_time,cuserid,compid,srv_id,visno,radreason)";
                cpatquery += "values (@gnTempRadTestID,@gcCustCode,@gnIntDocID,@gcReq_Numb,2,@gnIntDocID,convert(date,getdate()),convert(time,getdate()),@gnIntDocID,convert(date,getdate()),convert(time,getdate()),@gcUserid,@gnCompid,@gnDocQueueID,@gnVisNo,@gcRadReason)";
                SqlDataAdapter labCommand = new SqlDataAdapter();
                labCommand.InsertCommand = new SqlCommand(cpatquery, ndConnHandle3);
                labCommand.InsertCommand.Parameters.Add("@gnTempRadTestID", SqlDbType.Int).Value = gnTempRadTestID;
                labCommand.InsertCommand.Parameters.Add("@gcCustCode", SqlDbType.VarChar).Value = textBox5.Text.ToString();
                labCommand.InsertCommand.Parameters.Add("@gnIntDocID", SqlDbType.Int).Value = globalvar.gnIntDocID;
                labCommand.InsertCommand.Parameters.Add("@gcReq_Numb", SqlDbType.VarChar).Value = gcReq_Numb;//  getreqnumb(cs1, globalvar.gdSysDate);     //  TclassLibrary. "2019_06_12";   //  TclassLibrary.  TclassLibrary.re genrequest.getreqnumb(cs1,globalvar.gdSysDate);
                labCommand.InsertCommand.Parameters.Add("@gcUserid", SqlDbType.VarChar).Value = globalvar.gcUserid;
                labCommand.InsertCommand.Parameters.Add("@gnCompid", SqlDbType.Int).Value = globalvar.gnCompid;
                labCommand.InsertCommand.Parameters.Add("@gnDocQueueID", SqlDbType.Int).Value = globalvar.gnQueueID;
                //labCommand.InsertCommand.Parameters.Add("@gnVisNo", SqlDbType.Int).Value = nvisno;
                labCommand.InsertCommand.Parameters.Add("@gcRadReason", SqlDbType.Char).Value = "This is the rad reason";
                ndConnHandle3.Open();
                labCommand.InsertCommand.ExecuteNonQuery();
                ndConnHandle3.Close();
            }

        }

        private void updateRadExamItems()       //            .updateRadExamItems             &&update radtestitems from temporaray table= tempradtestitems on the backend
        {
            if (radview.Rows.Count > 0)
            {
                string cs1 = globalvar.cos;
                int nfcompid = globalvar.gnCompid;
                string ncompid = globalvar.gnCompid.ToString();
                using (SqlConnection ndConnHandle3 = new SqlConnection(cs1))
                {

                    string cpatquery = "insert into radtestitems(tes_id, item_name, itemNo, total_cost, compid, vatamt, req_type, req_by, req_date, req_time, cuserid, lcovered, radreason, srv_code)";
                    cpatquery += "values (@gnExamItemID,@lcItem_name,@lnItemNo,@lnPrice,@gnCompid,@lnVatAmt,@lnreq_type,@gnIntDocID,convert(date,getdate()),convert(time,getdate()),@gcUserid,@lcov,@lcindic,@lcsrv)";
                    ndConnHandle3.Open();
                    foreach (DataGridViewRow dr in radGrid.Rows)
                    {
                        if (dr.Cells["examSelect"].Value != null)
                            if (Convert.ToBoolean(dr.Cells["examSelect"].Value) == true)
                            {
                                SqlDataAdapter labCommandt = new SqlDataAdapter();
                                labCommandt.InsertCommand = new SqlCommand(cpatquery, ndConnHandle3);
                                labCommandt.InsertCommand.Parameters.Add("@gnExamItemID", SqlDbType.Int).Value = gnTempRadTestID;
                                labCommandt.InsertCommand.Parameters.Add("@lcItem_name", SqlDbType.VarChar).Value = dr.Cells["examName"].Value.ToString().Trim();
                                labCommandt.InsertCommand.Parameters.Add("@lnItemNo", SqlDbType.Int).Value = dr.Cells["servce_id"].Value.ToString().Trim();
                                labCommandt.InsertCommand.Parameters.Add("@lnPrice", SqlDbType.Decimal).Value = dr.Cells["examFee"].Value.ToString().Trim();
                                labCommandt.InsertCommand.Parameters.Add("@gnCompid", SqlDbType.Int).Value = globalvar.gnCompid;
                                labCommandt.InsertCommand.Parameters.Add("@lnVatAmt", SqlDbType.Decimal).Value = 0.00m;
                                labCommandt.InsertCommand.Parameters.Add("@lnreq_type", SqlDbType.Int).Value = 2;
                                labCommandt.InsertCommand.Parameters.Add("@gnIntDocID", SqlDbType.Int).Value = globalvar.gnIntDocID;
                                labCommandt.InsertCommand.Parameters.Add("@gcUserid", SqlDbType.VarChar).Value = globalvar.gcUserid;
                                labCommandt.InsertCommand.Parameters.Add("@lcov", SqlDbType.Bit).Value = false; ;
                                labCommandt.InsertCommand.Parameters.Add("@lcindic", SqlDbType.Char).Value = "This is the rad indication";
                                labCommandt.InsertCommand.Parameters.Add("@lcsrv", SqlDbType.Char).Value = dr.Cells["srvcode"].Value.ToString().Trim();
                                labCommandt.InsertCommand.ExecuteNonQuery();
                                labCommandt.InsertCommand.Parameters.Clear();
                            }
                    }
                    ndConnHandle3.Close();
                }
            }
        }

        private void updateOptProc()       //.updateOptProc 					&&update optproc from temporary table=tempopttest on the backend
        {
            getnewidnumb(3);                                //.getnewidnumb (2)
            string cs1 = globalvar.cos;
            int nfcompid = globalvar.gnCompid;
            string ncompid = globalvar.gnCompid.ToString();
            //int nvisno = Convert.ToInt16(getVisitNumber.visitno(cs1, ncompid, textBox5.Text));
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs1))
            {

                string cpatquery = "insert into opttest (tes_id,ccustcode,dr_id,req_numb,req_type,req_by,req_date,req_time,sent_by,sent_date,sent_time,cuserid,compid,srv_id,visno,optreason)";
                cpatquery += "values (@gnTempOptTestID,@gcCustCode,@gnIntDocID,@gcReq_Numb,2,@gnIntDocID,convert(date,getdate()),convert(time,getdate()),@gnIntDocID,convert(date,getdate()),convert(time,getdate()),@gcUserid,@gnCompid,@gnDocQueueID,@gnVisNo,@gcOptReason)";
                SqlDataAdapter labCommand = new SqlDataAdapter();
                labCommand.InsertCommand = new SqlCommand(cpatquery, ndConnHandle3);
                labCommand.InsertCommand.Parameters.Add("@gnTempOptTestID", SqlDbType.Int).Value = gnTempOptTestID;
                labCommand.InsertCommand.Parameters.Add("@gcCustCode", SqlDbType.VarChar).Value = textBox5.Text.ToString();
                labCommand.InsertCommand.Parameters.Add("@gnIntDocID", SqlDbType.Int).Value = globalvar.gnIntDocID;
                labCommand.InsertCommand.Parameters.Add("@gcReq_Numb", SqlDbType.VarChar).Value = gcReq_Numb;// getreqnumb(cs1, globalvar.gdSysDate);     //  TclassLibrary. "2019_06_12";   //  TclassLibrary.  TclassLibrary.re genrequest.getreqnumb(cs1,globalvar.gdSysDate);
                labCommand.InsertCommand.Parameters.Add("@gcUserid", SqlDbType.VarChar).Value = globalvar.gcUserid;
                labCommand.InsertCommand.Parameters.Add("@gnCompid", SqlDbType.Int).Value = globalvar.gnCompid;
                labCommand.InsertCommand.Parameters.Add("@gnDocQueueID", SqlDbType.Int).Value = globalvar.gnQueueID;
                ////labCommand.InsertCommand.Parameters.Add("@gnVisNo", SqlDbType.Int).Value = nvisno;
                labCommand.InsertCommand.Parameters.Add("@gcOptReason", SqlDbType.Char).Value = "This is the opt reason";
                ndConnHandle3.Open();
                labCommand.InsertCommand.ExecuteNonQuery();
                ndConnHandle3.Close();
            }
        }

        private void updateOptProcItems()       //.updateOptProcItems             &&update opttestitems from temporaray table= tempopttestitems on the backend
        {
            if (optview.Rows.Count > 0)
            {
                string cs1 = globalvar.cos;
                int nfcompid = globalvar.gnCompid;
                string ncompid = globalvar.gnCompid.ToString();
                using (SqlConnection ndConnHandle3 = new SqlConnection(cs1))
                {

                    string cpatquery = "insert into opttestitems(tes_id, item_name, itemNo, total_cost, compid, vatamt, req_type, req_by, req_date, req_time, cuserid, lcovered,lreceived,tested,test_by,test_date,test_time,srv_code)";
                    cpatquery += "values (@gnProcItemID,@lcItem_name,@lnItemNo,@lnPrice,@gnCompid,@lnVatAmt,@lnreq_type,@gnIntDocID,convert(date,getdate()),convert(time,getdate()),@gcUserid,@lcov,1,1,@gnIntDocID,convert(date,getdate()),convert(time,getdate()),@lcsrv)";
                    ndConnHandle3.Open();
                    foreach (DataGridViewRow dr in optGrid.Rows)
                    {
                        if (dr.Cells["procSelect"].Value != null)
                        if (Convert.ToBoolean(dr.Cells["procSelect"].Value) == true)
                        {
                            SqlDataAdapter labCommandt = new SqlDataAdapter();
                            labCommandt.InsertCommand = new SqlCommand(cpatquery, ndConnHandle3);
                            labCommandt.InsertCommand.Parameters.Add("@gnProcItemID", SqlDbType.Int).Value = gnTempOptTestID;
                            labCommandt.InsertCommand.Parameters.Add("@lcItem_name", SqlDbType.VarChar).Value = dr.Cells["procName"].Value.ToString().Trim();
                            labCommandt.InsertCommand.Parameters.Add("@lnItemNo", SqlDbType.Int).Value = dr.Cells["servicid"].Value.ToString().Trim();
                            labCommandt.InsertCommand.Parameters.Add("@lnPrice", SqlDbType.Decimal).Value = dr.Cells["ProcFee"].Value.ToString().Trim();
                            labCommandt.InsertCommand.Parameters.Add("@gnCompid", SqlDbType.Int).Value = globalvar.gnCompid;
                            labCommandt.InsertCommand.Parameters.Add("@lnVatAmt", SqlDbType.Decimal).Value = 0.00m;
                            labCommandt.InsertCommand.Parameters.Add("@lnreq_type", SqlDbType.Int).Value = 2;
                            labCommandt.InsertCommand.Parameters.Add("@gnIntDocID", SqlDbType.Int).Value = globalvar.gnIntDocID;
                            labCommandt.InsertCommand.Parameters.Add("@gcUserid", SqlDbType.VarChar).Value = globalvar.gcUserid;
                            labCommandt.InsertCommand.Parameters.Add("@lcov", SqlDbType.Bit).Value = false; ;
                            labCommandt.InsertCommand.Parameters.Add("@lcsrv", SqlDbType.Char).Value = dr.Cells["srvcode1"].Value.ToString().Trim();
                            labCommandt.InsertCommand.ExecuteNonQuery();
                            labCommandt.InsertCommand.Parameters.Clear();
                        }
                    }
                    ndConnHandle3.Close();
                }
            }
        }

        private void updateaccounts(int updtime)        // .updateaccounts(1)              &&update accounts(glmast and tranhist)
        {
            string cs1 = globalvar.cos;
            int nfcompid = globalvar.gnCompid;
            string gcVoucherNo = genbill.genvoucher(cs1, globalvar.gdSysDate); // genbill('2')
            string tcReceipt = "";
            bool glVatable;
            int lnServID;
            bool lnCashPay;          // = Iif(lCovered, 0, 1)
            decimal lnPrice;        // = Iif(glVatable, total_cost * (1 + gnVat), total_cost)
            string lcServ_name;     //= item_name
            string lcSrvCode;       //= srv_code
            decimal gnPostAmt;      // = -Abs(lnPrice)
            decimal gnContAmt;      // = Abs(lnPrice)
            string tcCustcode = textBox5.Text.ToString();
            string tcAcctNumb = globalvar.ClientAcctPrefix + textBox5.Text.ToString();
            string tcVoucher = genbill.genvoucher(cs1, globalvar.gdSysDate);
            string tcContra = globalvar.gcIntSuspense;
            string tcUserid = globalvar.gcUserid;
            int tncompid = globalvar.gnCompid;
            bool lisproduct = false;
            //gnVisno = getVisitNumber.visitno(cs1, tncompid.ToString(), textBox5.Text.ToString());
            string tcChqno = "000001";

            updateGlmast gls = new updateGlmast();
            updateTranhist tls = new updateTranhist();

            using (SqlConnection ndConnHandle3 = new SqlConnection(cs1))
            {
                switch (updtime)
                {
                    case 1:  //    Case tn = 1 && laboratory test
                        string dsql = "select * from labtestitems where tes_id=" + gnTempLabTestID;
                            ndConnHandle3.Open();
                            SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle3);
                            DataTable ds = new DataTable();
                            da.Fill(ds);
                        if (ds.Rows.Count > 0)
                        {
                            foreach(DataRow drow in ds.Rows)  //12430-8800
                            {
                                glVatable = Convert.ToBoolean(drow["vatable"]);
                                lnServID = Convert.ToInt16(drow["itemno"]);
                                lnCashPay = (Convert.ToBoolean(drow["lCovered"]) ? false : true);
                                lnPrice = (glVatable ? Convert.ToDecimal(drow["total_cost"]) * (1 + globalvar.gnVat) : Convert.ToDecimal(drow["total_cost"]));
                                lcServ_name = drow["item_name"].ToString();
                                lcSrvCode = drow["srv_code"].ToString();
                                gnPostAmt = -Math.Abs(lnPrice);
                                gnContAmt = Math.Abs(lnPrice);
                                bool  lFreeBee = false;  //will be coming back to do this for MIC

                                gls.updGlmast(cs1, tcAcctNumb, gnPostAmt);                               //update glmast posting account
                                decimal tnPNewBal = CheckLastBalance.lastbalance(cs1, tcAcctNumb);       //  0.00m;
                                tls.updTranhist(cs1, tcAcctNumb, gnPostAmt, lcServ_name, tcVoucher, tcChqno, tcUserid, tnPNewBal, "02", lnServID,false, tcContra,0.00m, 1, gnPostAmt, tcReceipt, lnCashPay, gnVisno, lisproduct,5,lcSrvCode, "", lFreeBee, tcCustcode, tncompid);                   //update tranhist posting account

                                gls.updGlmast(cs1, tcContra, gnContAmt);                                    //update glmast contra account
                                decimal tnCNewBal = CheckLastBalance.lastbalance(cs1, tcContra);         // 0.00m;
                                tls.updTranhist(cs1, tcContra, gnContAmt, lcServ_name, tcVoucher, tcChqno, tcUserid, tnCNewBal, "92", lnServID,false, tcAcctNumb,0.00m, 1, gnContAmt, tcReceipt, lnCashPay, gnVisno, lisproduct,5,lcSrvCode, "", lFreeBee, tcCustcode, tncompid);                   //update tranhist account 396 1756
                            }
                        }
                                break;
                    case 2:  //    Case tn = 2 && Radiology exams
                        string dsql1 = "select * from radtestitems where tes_id=" + gnTempRadTestID;
                        ndConnHandle3.Open();
                        SqlDataAdapter da1 = new SqlDataAdapter(dsql1, ndConnHandle3);
                        DataTable ds1 = new DataTable();
                        da1.Fill(ds1);
                        if (ds1.Rows.Count > 0)
                        {
                            foreach (DataRow drow in ds1.Rows)
                            {
                                glVatable = Convert.ToBoolean(drow["vatable"]);
                                lnServID = Convert.ToInt16(drow["itemno"]);
                                lnCashPay = (Convert.ToBoolean(drow["lCovered"]) ? false : true);
                                lnPrice = (glVatable ? Convert.ToDecimal(drow["total_cost"]) * (1 + globalvar.gnVat) : Convert.ToDecimal(drow["total_cost"]));
                                lcServ_name = drow["item_name"].ToString();
                                lcSrvCode = drow["srv_code"].ToString();
                                gnPostAmt = -Math.Abs(lnPrice);
                                gnContAmt = Math.Abs(lnPrice);
                                bool lFreeBee = false;  //will be coming back to do this for MIC

                                gls.updGlmast(cs1, tcAcctNumb, gnPostAmt);                               //update glmast posting account
                                decimal tnPNewBal = CheckLastBalance.lastbalance(cs1, tcAcctNumb);       //  0.00m;
                                tls.updTranhist(cs1, tcAcctNumb, gnPostAmt, lcServ_name, tcVoucher, tcChqno, tcUserid, tnPNewBal, "02", lnServID, false, tcContra, 0.00m, 1, gnPostAmt, tcReceipt, lnCashPay, gnVisno, lisproduct, 6, lcSrvCode, "", lFreeBee, tcCustcode, tncompid);                   //update tranhist posting account

                                gls.updGlmast(cs1, tcContra, gnContAmt);                                    //update glmast contra account
                                decimal tnCNewBal = CheckLastBalance.lastbalance(cs1, tcContra);         // 0.00m;
                                tls.updTranhist(cs1, tcContra, gnContAmt, lcServ_name, tcVoucher, tcChqno, tcUserid, tnCNewBal, "92", lnServID, false, tcAcctNumb, 0.00m, 1, gnContAmt, tcReceipt, lnCashPay, gnVisno, lisproduct, 6, lcSrvCode, "", lFreeBee, tcCustcode, tncompid);                   //update tranhist account 396 1756
                            }
                        }
                        break;
                    case 3:  //    Case tn = 3 && Operating Theatre Procedures
                        string dsql2 = "select * from opttestitems where tes_id=" + gnTempRadTestID;
                        ndConnHandle3.Open();
                        SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle3);
                        DataTable ds2 = new DataTable();
                        da2.Fill(ds2);
                        if (ds2.Rows.Count > 0)
                        {
                            foreach (DataRow drow in ds2.Rows)
                            {
                                glVatable = Convert.ToBoolean(drow["vatable"]);
                                lnServID = Convert.ToInt16(drow["itemno"]);
                                lnCashPay = (Convert.ToBoolean(drow["lCovered"]) ? false : true);
                                lnPrice = (glVatable ? Convert.ToDecimal(drow["total_cost"]) * (1 + globalvar.gnVat) : Convert.ToDecimal(drow["total_cost"]));
                                lcServ_name = drow["item_name"].ToString();
                                lcSrvCode = drow["srv_code"].ToString();
                                gnPostAmt = -Math.Abs(lnPrice);
                                gnContAmt = Math.Abs(lnPrice);
                                bool lFreeBee = false;  //will be coming back to do this for MIC

                                gls.updGlmast(cs1, tcAcctNumb, gnPostAmt);                               //update glmast posting account
                                decimal tnPNewBal = CheckLastBalance.lastbalance(cs1, tcAcctNumb);       //  0.00m;
                                tls.updTranhist(cs1, tcAcctNumb, gnPostAmt, lcServ_name, tcVoucher, tcChqno, tcUserid, tnPNewBal, "02", lnServID, false, tcContra, 0.00m, 1, gnPostAmt, tcReceipt, lnCashPay, gnVisno, lisproduct, 8, lcSrvCode, "", lFreeBee, tcCustcode, tncompid);                   //update tranhist posting account

                                gls.updGlmast(cs1, tcContra, gnContAmt);                                    //update glmast contra account
                                decimal tnCNewBal = CheckLastBalance.lastbalance(cs1, tcContra);         // 0.00m;
                                tls.updTranhist(cs1, tcContra, gnContAmt, lcServ_name, tcVoucher, tcChqno, tcUserid, tnCNewBal, "92", lnServID, false, tcAcctNumb, 0.00m, 1, gnContAmt, tcReceipt, lnCashPay, gnVisno, lisproduct, 8, lcSrvCode, "", lFreeBee, tcCustcode, tncompid);                   //update tranhist account 396 1756
                            }
                        }
                        break;
                }//end of switch
            }//end of sql connection 
            /*
             Parameters tn
With Thisform
	fn=SQLExec(gnConnHandle,"select 1 from patients where ccustcode=?gcCustCode","getidview")
	If fn>0 And Reccount()>0
		gnPatientID=0		&&pat_id
		Do Case
	

		Case tn = 4						&&send to internal clinic. The mandatory fee is the consultation fee of the destination clinic
			gcVoucherNo=genbill('2')
			gcReceiptNo=''
			Select billspeItems
			Locate
			Do While !Eof()
				glVatable=.F.
				lnServID=serv_id
				lnCashPay=Iif(lCovered,0,1)
				lnPrice=serv_fee
				lcServ_name=serv_name

				lcSrv=srv_code
				gnPostAmt=-Abs(lnPrice)
				gnContAmt=Abs(lnPrice)

				glFreeBee=Thisform.checkSrvfreebee (lcSrv,1)
				=UpdateGlmast(gcAcctNumb,gnPostAmt)			&&Client Account  - Posting Account
				gnPNewBal=CheckLastBalance(gcAcctNumb)
				=UpdateTransactionHistory(gcAcctNumb,gnPostAmt,lcServ_name,gcVoucherNo,'000001',gcUserID,gnPNewBal,'02',lnServID,0,0,gcIntSuspense,0.00,1,gnPostAmt,gcReceiptNo,lnCashPay,gnVisno,.F.,1,lcSrv,'')

				=UpdateGlmast(gcIntSuspense,gnContAmt)			&&update internal Suspense Account - Contra account
				gncNewBal=CheckLastBalance(gcIntSuspense)
				=UpdateTransactionHistory(gcIntSuspense,gnContAmt,lcServ_name,gcVoucherNo,'000001',gcUserID,gnPNewBal,'92',lnServID,0,0,gcAcctNumb,0.00,1,gnContAmt,gcReceiptNo,lnCashPay,gnVisno,.F.,1,lcSrv,'')

				gnDIncome=0.00
				gnDExpense=0.00
				gnDAcct_Rec=Abs(gnPostAmt)
				gnDCliRequest=1
				.updatedashboard
				Select billspeItems
				Skip
			Enddo
		Case tn = 5						&&send to specialist
			gcVoucherNo=genbill('2')
			gcReceiptNo=''
			Select billCliitems
			Locate
			Do While !Eof()
				glVatable=.F.
				lnServID=serv_id
				lnCashPay=Iif(!gl2Bouquets,1,0)
				lnPrice=serv_fee
				lcServ_name=serv_name
				lcSrv=srv_code
				gnPostAmt=-Abs(lnPrice)
				gnContAmt=Abs(lnPrice)

				glFreeBee=Thisform.checkSrvfreebee (lcSrv,1)
				=UpdateGlmast(gcAcctNumb,gnPostAmt)			&&Client Account  - Posting Account
				gnPNewBal=CheckLastBalance(gcAcctNumb)
				=UpdateTransactionHistory(gcAcctNumb,gnPostAmt,lcServ_name,gcVoucherNo,'000001',gcUserID,gnPNewBal,'02',lnServID,0,0,gcIntSuspense,0.00,1,gnPostAmt,gcReceiptNo,lnCashPay,gnVisno,.F.,1,lcSrv,'')

				=UpdateGlmast(gcIntSuspense,gnContAmt)			&&update internal Suspense Account - Contra account
				gncNewBal=CheckLastBalance(gcIntSuspense)
				=UpdateTransactionHistory(gcIntSuspense,gnContAmt,lcServ_name,gcVoucherNo,'000001',gcUserID,gnPNewBal,'92',lnServID,0,0,gcAcctNumb,0.00,1,gnContAmt,gcReceiptNo,lnCashPay,gnVisno,.F.,1,lcSrv,'')

				gnDIncome=0.00
				gnDExpense=0.00
				gnDAcct_Rec=Abs(gnPostAmt)
				gnDCliRequest=1
				.updatedashboard
				Select billCliitems
				Skip
			Enddo
		Endcase
	Else
		=sysmsg("Client not found, inform IT DEPT")
	Endif
Endwith

             */
        }


        private void updatecodes(int dtime)     //.updatecodes(1)
        {
            string cs1 = globalvar.cos;
            int nfcompid = globalvar.gnCompid;
            string ncompid = globalvar.gnCompid.ToString();
            //int nvisno = Convert.ToInt16(getVisitNumber.visitno(cs1, ncompid, textBox5.Text));
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs1))
            {
                ndConnHandle3.Open();
                switch (dtime)
                {
                    case 1: //&& update test code
                        string cupdatequery = "update patient_code set tes_id=tes_id+1 ";
                        SqlDataAdapter updCommand = new SqlDataAdapter();
                        updCommand.UpdateCommand = new SqlCommand(cupdatequery, ndConnHandle3);
                        updCommand.UpdateCommand.ExecuteNonQuery();
                        break;
                    case 2: //&& update exam code
                        string cupdatequery1 = "update patient_code set exa_id=exa_id+1";
                        SqlDataAdapter updCommand1 = new SqlDataAdapter();
                        updCommand1.UpdateCommand = new SqlCommand(cupdatequery1, ndConnHandle3);
                        updCommand1.UpdateCommand.ExecuteNonQuery();
                        break;
                    case 3: //&& update proc code
                        string cupdatequery2 = "update patient_code set pro_id=pro_id+1";
                        SqlDataAdapter updCommand2 = new SqlDataAdapter();
                        updCommand2.UpdateCommand = new SqlCommand(cupdatequery2, ndConnHandle3);
                        updCommand2.UpdateCommand.ExecuteNonQuery();
                        break;
                }
                ndConnHandle3.Close();
            }
        }

        private void send2admit()               //client is sent to admission
        {
//            MessageBox.Show("Client is sent to admission and we are confirming that");
            string cs = globalvar.cos;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string tcCode = textBox5.Text;
                int ncompid = globalvar.gnCompid;
                //int nvisno = Convert.ToInt16(getVisitNumber.visitno(cs, globalvar.gnCompid.ToString(), textBox5.Text));
                string tcAcctNumb = globalvar.ClientAcctPrefix + textBox5.Text.ToString();

                string admquery = "update pat_visit set sent2admit = 1,sent2admdate=CONVERT(date,getdate()),sent2admtime=CONVERT(time,getdate()) where compid = @gnCompid and ccustcode = @gcCustCode and admitted = 0 and activesession=1  ";
                string admquery1 = "update todayvisit set sent2admit = 1,sent2admdate=CONVERT(date,getdate()),sent2admtime=CONVERT(time,getdate()) where compid = @gnCompid and ccustcode = @gcCustCode and admitted = 0 and activesession=1  ";

                SqlDataAdapter patcl = new SqlDataAdapter();
                patcl.UpdateCommand = new SqlCommand(admquery, ndConnHandle);

                SqlDataAdapter todcl = new SqlDataAdapter();
                todcl.UpdateCommand = new SqlCommand(admquery1, ndConnHandle);

                ndConnHandle.Open();

                patcl.UpdateCommand.Parameters.Add("@gcCustCode", SqlDbType.Char).Value = tcCode;
                patcl.UpdateCommand.Parameters.Add("@gnCompid", SqlDbType.Int).Value = ncompid;

                todcl.UpdateCommand.Parameters.Add("@gcCustCode", SqlDbType.Char).Value = tcCode;
                todcl.UpdateCommand.Parameters.Add("@gnCompid", SqlDbType.Int).Value = ncompid;

                patcl.UpdateCommand.ExecuteNonQuery();
                todcl.UpdateCommand.ExecuteNonQuery();
                ndConnHandle.Close();
            }
        }

        private void admissiondrugs()
        {
            MessageBox.Show("Client is prescribed admission drugs");
            /*
                         With Thisform
	            lordsource=1														&&Source of order (pharmacy=1,main store=0)
	            lorddest=3															&&Destination of order is in-patient client
	            Select pharmaitems
	            Set Filter To seldrug
	            Locate
	            Do While !Eof()
		            lnprodid=product_id
		            ltoissue=quantity
		            sn=SQLExec(gnConnHandle,"insert into orders (product_id,toissue,srv_id,hwa_id,oprcode,compid,ord_date,ord_time,ccustcode,ordsource,orddest) values "+;
			            "(?lnProdid,?ltoissue,998,0,?gcUserid,?gnCompid,convert(date,getdate()),convert(time,getdate()),?gcCustCode,?lordsource,?lorddest)","orderin")
		            If !(sn>0 )
			            =sysmsg("Could not update orders, inform IT DEPT")
		            Endif
		            Select pharmaitems
		            Skip
	            Enddo
	            .Refresh
            Endwith
             */
        }

        private void drugdispense()               //             .drugdispense
        {
            string cs = globalvar.cos;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string gcVoucherNo = genbill.genvoucher(cs, globalvar.gdSysDate);
                string gcReceiptNo = "";
                string tcuserid = globalvar.gcUserid;
                int ncompid = globalvar.gnCompid;
                string tcCode = textBox5.Text;
                //int nvisno = Convert.ToInt16(getVisitNumber.visitno(cs, ncompid.ToString(), textBox5.Text));
                //                MessageBox.Show("Drug dispense step 0");
                string tcAcctNumb = globalvar.ClientAcctPrefix + textBox5.Text.ToString();
                string tcContra = globalvar.gcIntSuspense;
                bool lisproduct = true;

                string cquery = "insert into drug_dispense (prod_code,visno,visdate,quantity,UnitMeas,UnitPrice,cFormula,compid,ccustcode,dispensed,perday,lemer,notes)";
                cquery += "values (@lcProdCode,@gnVisNo,@gdvisdate,@lnIssue,@lnUnitMeas,@lnUnitPrice,@lcFormula,@gnCompid,@gcCustCode,1,@lnPerday,1,@dnotes)";
                SqlDataAdapter drugupd = new SqlDataAdapter();
                drugupd.InsertCommand = new SqlCommand(cquery, ndConnHandle);
                ndConnHandle.Open();
                foreach (DataGridViewRow drow in PharmaGrid.Rows)
                {
                    if (drow.Cells["issueAmt"].Value!=null && Convert.ToInt16(drow.Cells["issueAmt"].Value) > 0)
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

                        drugupd.InsertCommand.ExecuteNonQuery();
                        drugupd.InsertCommand.Parameters.Clear();

                        int lnServID = 0;   // lnProdID;
                        decimal lnPrice = nIssue * lnUnitPrice; //total issued X unit price
                        string lcServName = lcprodname.ToUpper();   // Upper(Alltrim(prod_name)) && Iif(gnRes_type = 1, fee_loc, Iif(gnRes_type = 2, fee_exp, fee_for))
                        decimal lnPostAmt = -Math.Abs(lnPrice);
                        decimal lnContAmt = Math.Abs(lnPrice);

                        updateGlmast gls = new updateGlmast();
                        updateTranhist tls = new updateTranhist();

                        gls.updGlmast(cs, tcAcctNumb, lnPostAmt);                                       //update glmast posting account
                        decimal tnPNewBal = CheckLastBalance.lastbalance(cs, tcAcctNumb);       //  0.00m;
                        tls.updTranhist(cs, tcAcctNumb, lnPostAmt, lcServName, gcVoucherNo, "000001", tcuserid, tnPNewBal, "93", lnServID, llpaid, tcContra, 0.00m, 1, lnPostAmt, gcReceiptNo, llCashPay, gnVisno, lisproduct,
                        7, "", lcProdCode, lFreebee, tcCode, ncompid);                   //update tranhist posting account

                        gls.updGlmast(cs, tcContra, lnContAmt);                                       //update glmast posting account
                        decimal tnCNewBal = CheckLastBalance.lastbalance(cs, tcContra);       //  0.00m;
                        tls.updTranhist(cs, tcContra, lnContAmt, lcServName, gcVoucherNo, "000001", tcuserid, tnCNewBal, "92", lnServID, llpaid, tcAcctNumb, 0.00m, 1, lnContAmt, gcReceiptNo, llCashPay, gnVisno, lisproduct,
                        7, "", lcProdCode, lFreebee, tcCode, ncompid);                   //update tranhist posting account
                    }
                }
                ndConnHandle.Close();
            }
    }

        private void upddispense()                //           .upddispense
        {
            string cs = globalvar.cos;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string tcCode = textBox5.Text;
                //int nvisno = Convert.ToInt16(getVisitNumber.visitno(cs,globalvar.gnCompid.ToString(), textBox5.Text));
                string tcAcctNumb = globalvar.ClientAcctPrefix + textBox5.Text.ToString();

                string cquery = "update pat_visit set prescribed=1 where ccustcode=@gcCustCode and activesession=1 and visno=@gnVisNo";
                string cquery1 = "update todayvisit set prescribed=1 where ccustcode=@gcCustCode and activesession=1 and visno=@gnVisNo";

                SqlDataAdapter drugdisp = new SqlDataAdapter();
                drugdisp.UpdateCommand = new SqlCommand(cquery, ndConnHandle);

                SqlDataAdapter drugdisp1 = new SqlDataAdapter();
                drugdisp1.UpdateCommand = new SqlCommand(cquery1, ndConnHandle);

                ndConnHandle.Open();

                drugdisp.UpdateCommand.Parameters.Add("@gcCustCode", SqlDbType.Char).Value = tcCode;
                //drugdisp.UpdateCommand.Parameters.Add("@gnVisNo", SqlDbType.Int).Value = nvisno;

                drugdisp1.UpdateCommand.Parameters.Add("@gcCustCode", SqlDbType.Char).Value = tcCode;
                //drugdisp1.UpdateCommand.Parameters.Add("@gnVisNo", SqlDbType.Int).Value = nvisno;

                drugdisp.UpdateCommand.ExecuteNonQuery();
                drugdisp1.UpdateCommand.ExecuteNonQuery();
                ndConnHandle.Close();
            }
        }

        private void updatetodayq()
        {
      //      MessageBox.Show("we will update today queue now");

            string cs = globalvar.cos;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                if(glCliUpdated==true)
                {
                    //internalreferral() will come back to this
                }
                else
                {
                    string tcCode = textBox5.Text;
                    //int nvisno = Convert.ToInt16(getVisitNumber.visitno(cs, globalvar.gnCompid.ToString(), textBox5.Text));
                    string cquery = "update pat_visit set lconsult=1,consdate=convert(date,getdate()),constime=convert(time,getdate()) where ccustcode=@gcCustCode and activesession=1  and visno=@gnVisNo";
                    string cquery1 = "update todayvisit set lconsult=1,consdate=convert(date,getdate()),constime=convert(time,getdate()) where ccustcode=@gcCustCode and activesession=1  and visno=@gnVisNo";

                    SqlDataAdapter qdisp = new SqlDataAdapter();
                    qdisp.UpdateCommand = new SqlCommand(cquery, ndConnHandle);

                    SqlDataAdapter qdisp1 = new SqlDataAdapter();
                    qdisp1.UpdateCommand = new SqlCommand(cquery1, ndConnHandle);

                    ndConnHandle.Open();

                    qdisp.UpdateCommand.Parameters.Add("@gcCustCode", SqlDbType.Char).Value = tcCode;
                    //qdisp.UpdateCommand.Parameters.Add("@gnVisNo", SqlDbType.Int).Value = nvisno;

                    qdisp1.UpdateCommand.Parameters.Add("@gcCustCode", SqlDbType.Char).Value = tcCode;
                    //qdisp1.UpdateCommand.Parameters.Add("@gnVisNo", SqlDbType.Int).Value = nvisno;

                    qdisp.UpdateCommand.ExecuteNonQuery();
                    qdisp1.UpdateCommand.ExecuteNonQuery();
                    ndConnHandle.Close();
                }
            }
        }


        private void temporary_files_upload(int tntime)
        {

        }

        private void temporary_files_update(int tnSource)
        {
        }
        private void temporary_files_clear()
        {
//            MessageBox.Show("we are in Clear temporary files");
        }

        private void labGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //            bool dtest = labGrid.SelectedCells.  Convert.ToBoolean(labGrid.Rows[e.RowIndex].Cells["testSelect"].Value);
            //          MessageBox.Show("The selection is " + dtest);
         //  if (Convert.ToBoolean(labGrid.Rows[e.RowIndex].Cells["testSelect"].Value)==true)
           // {
             //   MessageBox.Show("we will be doing temporary file update");
    //            tempfiles.temporary_files_update(1);  //we will update temporary lab files
      //      }else { MessageBox.Show("Test is not selected"); }
            /*
             With Thisform.pageframe1.page1
	lcTestName=''
	lcaTemp=Alias()
	lnRecNo=Recno()
	lcTestName=Alltrim(templabtestitems.item_name)
	lnTestCost=templabtestitems.total_cost
	gcTempSrvCode=templabtestitems.srv_code
	llCov=templabtestitems.lcovered
	lnItemNo=itemno
	glLabUpdated=Iif(Reccount()>0,.T.,.F.)

	If This.Value			&&add new test
		Select * From seltestItems Where itemno=lnItemNo Into Cursor mysel
		If !(_Tally>0)
			Insert Into seltestItems (itemno,test_name,total_cost,testsel,lcov,srv_code)	Values (lnItemNo,lcTestName,lnTestCost,.T.,llcov,gcTempSrvCode)
		Endif
		Thisform.temporary_file_update (2,lnItemNo,lcTestName,lnTestCost,llCov)
	Else					&&delete an item
		Delete From seltestItems Where itemno=lnItemNo
		=SQLExec(gnConnHandle,'delete from labtestitems_bkp where ccustcode=?gcCustCode and visno=?gnVisNo and srv_code=?gcTempSrvCode','itemview')
	Endif
	Select (lcaTemp)
	Goto lnRecNo
	With Thisform.pageframe1.page1.seltest
		Select seltestItems
		Sum All total_cost For testsel And lcov To gnLabCoverPayment
		Sum All total_cost For testsel And !lcov To gnLabCashPayment
		Locate
		.RecordSource = 'selTestItems'
		.column1.ControlSource = 'test_name'
		.column2.ControlSource = 'total_cost'
		.column3.ControlSource = 'testsel'
	Endwith
	Thisform.pageframe1.page1.seltest.Refresh
	.Refresh
Endwith
Thisform.Refresh
             */
        }

        private void radGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            /*
             With Thisform.pageframe1.page2
	lcTestName=''
	lca=Alias()
	lnRecNo=Recno()
	lcTestName=Alltrim(tempradtestitems.item_name)
	lnTestCost=tempradtestitems.total_cost
	gcTempSrvCode=tempradtestitems.srv_code
	lnItemNo=itemno
	llcov=tempradtestitems.lcovered
	glradUpdated=Iif(Reccount()>0,.T.,.F.)

	If This.Value			&&add new test
		If gl2Bouquets
			Select * From tempCoverService Where serv_id=lnItemNo Into Cursor mycov
			If _Tally>0
				glCoveredPay=Iif(!lrestrict,.T.,.F.)
				gnExamCost=Iif(!lrestrict,coverpay,lnTestCost)
			Endif
		Else
			gnExamCost=lnTestCost
		Endif

**********************************************************************
		gnExamCost=Iif(gl2Bouquets,Iif(gnExamCost>0.00,gnExamCost,lnTestCost),lnTestCost)
		Select * From selExamItems Where itemno=lnItemNo Into Cursor mysel
		If !(_Tally>0)
			Insert Into selExamItems (itemno,exam_name,total_cost,examsel,lcov)	Values (lnItemNo,lcTestName,lnTestCost,.T.,llcov)
		Endif
		Thisform.temporary_file_update (3,lnItemNo,lcTestName,lnTestCost,llcov)
	Else					&&delete an item
		Delete From selExamItems Where itemno=lnItemNo
		=SQLExec(gnConnHandle,'delete from radtestitems_bkp where ccustcode=?gcCustCode and visno=?gnVisNo and srv_code=?gcTempSrvCode','itemview')
	Endif
	Select (lca)
	Goto lnRecNo

	With Thisform.pageframe1.page2.selExam
		Select selExamItems
		Select selExamItems
		Sum All total_cost For examsel And lcov To gnradCoverPayment
		Sum All total_cost For examsel And !lcov To gnradCashPayment
		Locate
		.RecordSource = 'selExamItems'
		.column1.ControlSource = 'exam_name'
		.column2.ControlSource = 'total_cost'
		.column3.ControlSource = 'examsel'
	Endwith
*	.Refresh
	Thisform.pageframe1.page2.selExam.Refresh
Endwith
Thisform.Refresh
             */
        }

        private void optGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            /*
             With Thisform.pageframe1.page3
	lcTestName=''
	lca=Alias()
	lnRecNo=Recno()
	lcTestName=Alltrim(tempopttestitems.item_name)
	lnProcCost=tempopttestitems.total_cost
	llcov=tempopttestitems.lcovered
	gcTempSrvCode=tempopttestitems.srv_code
	lnItemNo=itemno
	gloptUpdated=Iif(Reccount()>0,.T.,.F.)
	If This.Value			&&add new procedure
		If gl2Bouquets
			Select * From tempCoverService Where serv_id=lnItemNo Into Cursor mycov
			If _Tally>0
				glCoveredPay=Iif(!lrestrict,.T.,.F.)
				gnProcCost=Iif(!lrestrict,coverpay,lnProcCost)
			Endif
		Else
			gnProcCost=lnProcCost
		Endif

		gnProcCost=Iif(gl2Bouquets,Iif(gnProcCost>0.00,gnProcCost,lnProcCost),lnProcCost)
 		Select * From selProcItems Where itemno=lnItemNo Into Cursor mysel
		If !(_Tally>0)
			Insert Into selProcItems (itemno,proc_name,total_cost,procsel,lcov,srv_code)	Values (lnItemNo,lcTestName,lnProcCost,.T.,llcov,gcTempSrvCode)
		Endif
		Thisform.temporary_file_update (4,lnItemNo,lcTestName,lnProcCost,llcov)
	Else					&&delete an item
		Delete From selProcItems Where itemno=lnItemNo
		=SQLExec(gnConnHandle,'delete from opttestitems_bkp where ccustcode=?gcCustCode and visno=?gnVisNo and srv_code=?gcTempSrvCode','itemview')
	Endif
	Select (lca)
*	Sum All total_cost For tsel And lcovered To gnoptCoverPayment
*	Sum All total_cost For tsel And !lcovered To gnoptCashPayment
	Goto lnRecNo

	With Thisform.pageframe1.page3.selproc
		Select selProcItems
		Sum All total_cost For procsel And lcov To gnoptCoverPayment
		Sum All total_cost For procsel And !lcov To gnoptCashPayment
		Locate
		.RecordSource = 'selProcItems'
		.column1.ControlSource = 'proc_name'
		.column2.ControlSource = 'total_cost'
		.column3.ControlSource = 'procsel'
	Endwith
*	.Refresh
	.selproc.Refresh
Endwith
thisform.Refresh 

             */
        }

        private void clientgrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            clientgrid.EndEdit();
            string cs = globalvar.cos;
            string  ncompid = globalvar.gnCompid.ToString();
            textBox5.Text = Convert.ToString(clientview.Rows[clientgrid.CurrentCell.RowIndex]["ccustcode"]);
           //            textBox5.Text = clientgrid.Rows[e.RowIndex].Cells["ccustcode"].Value.ToString();
           //gnVisno = getVisitNumber.visitno(cs, ncompid, textBox5.Text.ToString());
            temporary_files dtempfile = new temporary_files();
            /*
            dtempfile.temporary_files_upload(1);
            dtempfile.temporary_files_upload(2);
            dtempfile.temporary_files_upload(3);
            dtempfile.temporary_files_upload(4);
            dtempfile.temporary_files_upload(5);
            dtempfile.temporary_files_upload(6);
            dtempfile.temporary_files_upload(7);
            */

            //            dtempfile.temporary_files_update(1); 

            //            textBox5.Text = drow["ccustcode"].ToString();
            /*
                            Lparameters nColIndex
                            gcCustCode=Right(SpeCliView.ccustcode,6)
                            gnDocQueueID=SpeCliView.q_id
                            glClientGender=SpeCliView.gender
                            If gcCustCode<>gcCurrentClient
                            gcCurrentClient=gcCustCode
                            Thisform.resetvalues
                            Endif
                            =FreeServiceCheck(gnDocQueueID)

                            If .F.
                            *glFreeBee=Iif(glGloNoPre,.T.,.F.)
                                If glGloNoCon
                                    =sysmsg('No consultation for this queue')
                                Endif
                                If glGloNoLab
                                    =sysmsg('Free lab services for this queue')
                                Endif

                                If glGloNoRad
                                    =sysmsg('Free rad services for this queue')
                                Endif

                                If glGloNoPre
                                    =sysmsg('Free Prescriptions for this queue')
                                Endif

                                If glGloNoOpt
                                    =sysmsg('Free procedures for this queue')
                                Endif
                            Endif

                                gcAcctNumb=SpeCliView.cacctnumb
                                Thisform.text1.Value=Year(gdSysDate)-Year(Ttod(SpeCliView.ddatebirth))+1
                                Thisform.text2.Value=SpeCliView.pc_tel
                                gl2Bouquets=Iif(SpeCliView.wit_cas,.F.,.T.)		&&Iif(wit_ins Or wit_cor Or wit_nhi,.T.,.F.))
                                *glMedCare=SpeCliView.casemed
                                Thisform.text15.Value = SpeCliView.client_place
                                Thisform.label4.Caption = 'Client ID : '+gcCustCode
                                Thisform.getvisit
            */

            /*
                            *WAIT WINDOW gnDocQueueID
                            =FreeServiceCheck(gnDocQueueID)    &&checking for free drugs offered by thiis service centre
                            *IF glGloNopre
                            *=sysmsg('free prescription')
                            *endif

                            If 	gl2Bouquets
                                Thisform.getinsurance
                            Endif

                            Thisform.getlastconsult

                            With Thisform.pageframe1.page8
                                .text2.Value=SpeCliView.fname
                                .text3.Value=SpeCliView.lname
                                .text8.Value = Dmy(Ttod(gdSysDate))
                            Endwith

                            Thisform.triagevitals
                            Thisform.triagedrugs
                            Thisform.triageallergy
                            Thisform.triagetb
                            Thisform.pageframe1.page11.SetFocus
                            Thisform.Refresh
                         */
        }

        private void tabPage10_Enter(object sender, EventArgs e)
        {
            //getrequestnumber("1");
        }

        private void labGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if(labGrid.Columns[e.ColumnIndex].Name == "testSelect")
            {
                string cs = globalvar.cos;
                string tcCode = textBox5.Text.ToString();
                string srvcode = labGrid.CurrentRow.Cells["srv_code"].Value.ToString();// labGrid.Rows[e.RowIndex].Cells["srv_code"].ToString();
                string srvname = labGrid.Rows[e.RowIndex].Cells["itemName"].Value.ToString();
                decimal srvFee = Convert.ToDecimal(labGrid.Rows[e.RowIndex].Cells["servFee"].Value);
                if (Convert.ToBoolean(labGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value))
                {
                    tempfiles.temporary_files_update(cs,2,tcCode,gnVisno,srvcode,srvname,srvFee,false,true);  //we will update temporary lab files
                }else
                {
                    tempfiles.temporary_files_update(cs, 2, tcCode, gnVisno, srvcode, srvname, srvFee, false,false);  //we will update temporary lab files
                }
            }
        }

        private void labGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
           labGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void radGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (radGrid.Columns[e.ColumnIndex].Name == "ExamSelect")
            {
                string cs = globalvar.cos;
                string tcCode = textBox5.Text.ToString();
                string procode = radGrid.CurrentRow.Cells["SrvCode"].Value.ToString();// labGrid.Rows[e.RowIndex].Cells["srv_code"].ToString();
                string examname = radGrid.Rows[e.RowIndex].Cells["examName"].Value.ToString();
                decimal examFee = Convert.ToDecimal(radGrid.Rows[e.RowIndex].Cells["examFee"].Value);
                if (Convert.ToBoolean(radGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value))
                {
                    tempfiles.temporary_files_update(cs, 3, tcCode, gnVisno, procode, examname,examFee, false, true);  //we will update temporary lab files
                }
                else
                {
                    tempfiles.temporary_files_update(cs, 3, tcCode, gnVisno, procode, examname, examFee , false, false);  //we will update temporary lab files
                }
            }
        }

        private void radGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            radGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void optGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (optGrid.Columns[e.ColumnIndex].Name == "procSelect")
            {
                string cs = globalvar.cos;
                string tcCode = textBox5.Text.ToString();
                string optcode = optGrid.CurrentRow.Cells["srvcode1"].Value.ToString();// labGrid.Rows[e.RowIndex].Cells["srv_code"].ToString();
                string optname = optGrid.Rows[e.RowIndex].Cells["procName"].Value.ToString();
                decimal optFee = Convert.ToDecimal(optGrid.Rows[e.RowIndex].Cells["ProcFee"].Value);
                if (Convert.ToBoolean(optGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value))
                {
                    tempfiles.temporary_files_update(cs, 4, tcCode, gnVisno, optcode, optname, optFee, false, true);  //we will update temporary lab files
                }
                else
                {
                    tempfiles.temporary_files_update(cs, 4, tcCode, gnVisno, optcode, optname, optFee, false, false);  //we will update temporary lab files
                }
            }
        }

        private void optGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            optGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Do you want to admit Client","Admission Check", MessageBoxButtons.YesNo)==DialogResult.Yes)
            {
                glSent2Adm = true;  //.T.
                MessageBox.Show("Client will be sent to Admission when you Save Details");
            }
            else
            {
                glSent2Adm = false;     // .F.
//                MessageBox.Show("Client will not be sent to Admission");
            }
        }

        private void PharmaGrid_KeyPress(object sender, KeyPressEventArgs e)
        {
//            PharmaGrid.EndEdit();
        }

        private void PharmaGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
      //      PharmaGrid.CurrentCell = PharmaGrid.Rows[(e.RowIndex)].Cells[(e.ColumnIndex + 1)];
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MedHistory dmed = new MedHistory(textBox5.Text.Trim().ToString());
            dmed.ShowDialog();
        }

        private void textBox6_Validated(object sender, EventArgs e)
        {
            //            MessageBox.Show("text validated");
            bool llicd = radioButton2.Checked;
//            MessageBox.Show("Your selection is " + (llicd ? "ICD" : "National"));
            icd10(llicd,textBox6.Text.ToString().Trim());
        }

        private void icdGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
         icdGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void icdGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (icdGrid.Columns[e.ColumnIndex].Name == "diagSelect")
            {
                //                string cs = globalvar.cos;
                string icdcode = icdGrid.CurrentRow.Cells["icdcode"].Value.ToString();    
                string icdname = icdGrid.CurrentRow.Cells["icdname"].Value.ToString();   
                if (Convert.ToBoolean(icdGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value))  //item has been selected
                {
                    //update  diagview table    
                    //                    selDiagView.Rows.Add()
                    MessageBox.Show("We will update the diagselect");
                    selDiagView.Rows.Add();
                    selDiagView.Rows[e.RowIndex]["selDiagName"] = icdname;
//                    selDiagView.
                    //selDiagView.Rows[e.RowIndex]["seldiagname"] = icdname; //DBTable.Rows[iter]["First Name"] = firstName;
                    //selDiagView.Rows.Add()
                }
                else
                {
                    MessageBox.Show("We will not update the diagselect");
                    //                  tempfiles.temporary_files_update(cs, 2, tcCode, gnVisno, srvcode, srvname, srvFee, false, false);  //we will update temporary lab files
                }
            }
        }
    }
}
