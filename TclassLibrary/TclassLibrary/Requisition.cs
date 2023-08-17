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
    public partial class Requisition : Form
    {
        string cs = string.Empty;
        int ncompid = 0;
        string dloca = string.Empty;
        DataTable itemview = new DataTable();
        bool glValidStaff = false;
        int gnDeptID = 0;
        int gnDesID = 0;
        string gcTempStaffNo = "";
        string tcUserID = "";
        int gnSelectedItems = 0;
        public Requisition(string tcCos, int tnCompid, string tcLoca,string tcUser)
        {
            InitializeComponent();
            cs = tcCos;
            ncompid = tnCompid;
            dloca = tcLoca;
            tcUserID = tcUser;
        }

        private void Requisition_Load(object sender, EventArgs e)
        {
            this.Text =dloca+ "<< Item Requisition >>";
            getitems();
            getstaff(tcUserID);

            prodGrid.Columns["avlAmt"].SortMode = DataGridViewColumnSortMode.NotSortable;
            prodGrid.Columns["avlAmt"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            prodGrid.Columns["reqAmt"].SortMode = DataGridViewColumnSortMode.NotSortable;
            prodGrid.Columns["reqAmt"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            prodGrid.Columns["purAmt"].SortMode = DataGridViewColumnSortMode.NotSortable;
            prodGrid.Columns["purAmt"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;

            int reqCounter = GetClient_Code.clientCode_int(cs, "req");
            if(reqCounter >9999)
            {
                resetClient_Code rc = new resetClient_Code();
                rc.setClient(cs, "req");
                textBox1.Text = "RQ" + DateTime.Today.Year.ToString().Trim().PadLeft(2, '0') +
                    DateTime.Today.Month.ToString().Trim().PadLeft(2, '0') + "0001";
            }
            else
            {
                textBox1.Text = "RQ" + DateTime.Today.Year.ToString().Trim().PadLeft(2, '0') +
                    DateTime.Today.Month.ToString().Trim().PadLeft(2, '0') + reqCounter.ToString().Trim().PadLeft(4, '0'); 
            }
        }

        /*
              Function GENREQ					&&This function generates REQUISITION NUMBERS
Parameters tcwhen
sn=SQLExec(gnConnHandle,"select req from patient_code","updateVouView")
If !(sn>0)
	=sysmsg("something is not right")
Else
	If req>=9999
		fn=SQLExec(gnConnHandle,"update patient_code set req=1","updcounter")
		gcVoucher='RQ'+Right(Alltrim(Str(Year(gdSysDate))),2)+;
			PADL(Alltrim(Str(Month(gdSysDate))),2,'0')+;
			PADL(Alltrim(Str(Day(gdSysDate))),2,'0')+'0001'
	Else
		gcVoucher='RQ'+Right(Alltrim(Str(Year(gdSysDate))),2)+;
			PADL(Alltrim(Str(Month(gdSysDate))),2,'0')+;
			PADL(Alltrim(Str(Day(gdSysDate))),2,'0')+;
			PADL(Alltrim(Str(req)),4,'0')
		If tcwhen='2'
			sn=SQLExec(gnConnHandle,"update patient_code set req=req+1","tupdateview")
			If !(sn>0)
				=Messagebox("something is wrong","Information Centre")
			Endif
		Endif
	Endif
Endif
Return gcVoucher
             */


        private void getitems()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                itemview.Clear();
                ndConnHandle.Open();
                string dsql = "exec tsp_RequisitionOrderList " + ncompid;
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(itemview);
                if(itemview.Rows.Count >0)
                {
                    prodGrid.AutoGenerateColumns = false;
                    prodGrid.DataSource = itemview.DefaultView;
                    prodGrid.Columns[0].DataPropertyName = "dsel";
                    prodGrid.Columns[1].DataPropertyName = "prod_name";
                    prodGrid.Columns[2].DataPropertyName = "available";
                    prodGrid.Columns[3].DataPropertyName = "toissue";
                    prodGrid.Columns[7].DataPropertyName = "prod_code";
                }
                ndConnHandle.Close();
            }
        }

        private void getstaff(string tcUserid)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                string dsql = "exec tsp_StaffDeptDes " + ncompid + ",'" + tcUserid  + "'"; ;
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                DataTable stfview = new DataTable();
                da.Fill(stfview);
                if(stfview.Rows.Count>0)
                {
                    glValidStaff = true;
                    gnDeptID = Convert.ToInt16(stfview.Rows[0]["dep_id"]);
                    gnDesID = Convert.ToInt16(stfview.Rows[0]["des_id"]);
                    gcTempStaffNo = stfview.Rows[0]["staffno"].ToString().Trim();
                    textBox10.Text = stfview.Rows[0]["dep_name"].ToString().Trim();
                    textBox11.Text = stfview.Rows[0]["username"].ToString().Trim();
                    textBox12.Text = stfview.Rows[0]["des_name"].ToString().Trim();
                }
                ndConnHandle.Close();
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Down || e.KeyCode == Keys.Tab)
            {
                SelectNextControl(ActiveControl, true, true, true, true);
                e.Handled = true;
                page01ok();
            }
            else if (e.KeyCode == Keys.Up)
            {
                SelectNextControl(ActiveControl, false, true, true, true);
                e.Handled = true;
                page01ok();
            }
        }

        private void page01ok() //staff basic details 
        {
            //bool llAuth = radioButton1.Checked || radioButton2.Checked ? true : false;
            //bool lAbsOK = absTo.Value > absFrom.Value ? true : false;
//            MessageBox.Show("The number of selected items is " + gnSelectedItems);

            if (gnSelectedItems>0)   //comboBox2.SelectedIndex > -1 && absDetails.Text.ToString().Trim() != "" && llAuth && lAbsOK)
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


        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            updreqDetails();
            initvariables();
            updateClient_Code updc = new updateClient_Code();
            updc.updClient(cs, "req");

            int reqCounter = GetClient_Code.clientCode_int(cs, "req");
            if (reqCounter > 9999)
            {
                resetClient_Code rc = new resetClient_Code();
                rc.setClient(cs, "req");
                textBox1.Text = "RQ" + DateTime.Today.Year.ToString().Trim().PadLeft(2, '0') +
                    DateTime.Today.Month.ToString().Trim().PadLeft(2, '0') + "0001";
            }
            else
            {
                textBox1.Text = "RQ" + DateTime.Today.Year.ToString().Trim().PadLeft(2, '0') +
                    DateTime.Today.Month.ToString().Trim().PadLeft(2, '0') + reqCounter.ToString().Trim().PadLeft(4, '0');
            }
        }



        private void prodGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (prodGrid.Columns[e.ColumnIndex].Name == "reqAmt")
            {
                if (prodGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "")
                {
                    int lnavlAmt = Convert.ToInt32(prodGrid.Rows[e.RowIndex].Cells["avlAmt"].Value);
                    int lnreqAmt = Convert.ToInt32(prodGrid.Rows[e.RowIndex].Cells["reqAmt"].Value);
                    if(lnreqAmt>lnavlAmt)
                    {
                     if (MessageBox.Show("More than available quantity, Request to Purchase","Request to purchase",MessageBoxButtons.YesNo,MessageBoxIcon.Question,MessageBoxDefaultButton.Button2)==DialogResult.Yes)
                        {
                            prodGrid.Rows[e.RowIndex].Cells["purAmt"].Value = lnreqAmt-lnavlAmt;
                            prodGrid.Rows[e.RowIndex].Cells["iselect"].Value = true;
                            gnSelectedItems++;
//                            page01ok();
                        }
                        else
                        {
                            prodGrid.Rows[e.RowIndex].Cells["reqAmt"].Value = 0;
                            prodGrid.Rows[e.RowIndex].Cells["purAmt"].Value = 0;
                            prodGrid.Rows[e.RowIndex].Cells["iselect"].Value = false;
                            gnSelectedItems--;
                        }
                    }
                    else
                    {
                        prodGrid.Rows[e.RowIndex].Cells["iselect"].Value = true;
                        gnSelectedItems++;
                    }
                }
            }
            page01ok();
        }

        private void updreqDetails()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                bool lallowed = radioButton1.Checked ? true : false;
                int lnOrdSource = 0; //1 = pharmacy: 0 = main store. orders to pharmacy will be done from service centres and wards.
                int lnOrdDest = 2; //1=Wards, 2= Service Centres/departments, 3 = clients/patients
                 
                ndConnHandle.Open();
                string cquery = "insert into orders (prod_code,toissue,srv_id,oprcode,ord_date,ord_time,ordsource," ;
                cquery += "orddest,req,dep_id,des_id,preqno,topurchase,lpetty,lasset,nopenbal,compid)  ";
                cquery += "values (@tprod_code,@ttoissue,@tsrv_id,@toprcode,convert(date,getdate()),convert(time,getdate()),@tordsource,";
                cquery += "@torddest,1,@tdep_id,@tdes_id,@tpreqno,@ttopurchase,@tlpetty,@tlasset,@tnopenbal,@tcompid)";

                SqlDataAdapter cuscommand = new SqlDataAdapter();
                cuscommand.InsertCommand = new SqlCommand(cquery, ndConnHandle);

                for(int j =0; j<prodGrid.Rows.Count;j++)
                {
                    if(Convert.ToBoolean(prodGrid.Rows[j].Cells["iselect"].Value))
                    {
                        cuscommand.InsertCommand.Parameters.Add("@tprod_code", SqlDbType.VarChar).Value = prodGrid.Rows[j].Cells["prdCode"].Value.ToString().Trim();
                        cuscommand.InsertCommand.Parameters.Add("@ttoissue", SqlDbType.Int).Value = Convert.ToInt16(prodGrid.Rows[j].Cells["reqAmt"].Value);
                        cuscommand.InsertCommand.Parameters.Add("@tsrv_id", SqlDbType.Int).Value = 1;
                        cuscommand.InsertCommand.Parameters.Add("@toprcode", SqlDbType.VarChar).Value = tcUserID;
                        cuscommand.InsertCommand.Parameters.Add("@tordsource", SqlDbType.Int).Value = lnOrdSource;
                        cuscommand.InsertCommand.Parameters.Add("@torddest", SqlDbType.Int).Value = lnOrdDest;
                        cuscommand.InsertCommand.Parameters.Add("@tdep_id", SqlDbType.Int).Value = gnDeptID;
                        cuscommand.InsertCommand.Parameters.Add("@tdes_id", SqlDbType.Int).Value = gnDesID;

                        cuscommand.InsertCommand.Parameters.Add("@tpreqno", SqlDbType.VarChar).Value = textBox1.Text.ToString().Trim();
                        cuscommand.InsertCommand.Parameters.Add("@ttopurchase", SqlDbType.Int).Value = Convert.ToInt16(prodGrid.Rows[j].Cells["purAmt"].Value);
                        cuscommand.InsertCommand.Parameters.Add("@tlpetty", SqlDbType.Bit).Value = Convert.ToBoolean(prodGrid.Rows[j].Cells["lpetty"].Value);
                        cuscommand.InsertCommand.Parameters.Add("@tlasset", SqlDbType.Bit).Value = Convert.ToBoolean(prodGrid.Rows[j].Cells["lasset"].Value);
                        cuscommand.InsertCommand.Parameters.Add("@tnopenbal", SqlDbType.Int).Value = Convert.ToInt16(prodGrid.Rows[j].Cells["avlAmt"].Value);
                        cuscommand.InsertCommand.Parameters.Add("@tcompid", SqlDbType.Int).Value = ncompid;
                        cuscommand.InsertCommand.ExecuteNonQuery();
                        cuscommand.InsertCommand.Parameters.Clear();
                    }
                }

                //ndConnHandle.Close();
                MessageBox.Show("New Requisition details added successfully");
            }
        }

        private void initvariables()
        {
            for(int i=0;i<prodGrid.Rows.Count;i++)
            {
                prodGrid.Rows[i].Cells["reqAmt"].Value = 0;
                prodGrid.Rows[i].Cells["purAmt"].Value = 0;
                prodGrid.Rows[i].Cells["iselect"].Value = false;
                SaveButton.Enabled = false;
                SaveButton.BackColor = Color.Gainsboro;
            }
        }
    }
}
