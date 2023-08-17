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
    public partial class Orders : Form
    {
        string cs = globalvar.cos;
        int ncompid = globalvar.gnCompid;
        DataTable orderview = new DataTable();
        DataTable userview = new DataTable();
        int gnClinicID = 0;
        int gnWardID = 0;
        string gcDestName = "";
        int selcount = 0;
        int gnDestype;
        int gnStaffDept = 0;
        int gnStaffDes = 0;
        string tcuserid = globalvar.gcUserid;

        public Orders(int nqueue,int destype)
        {
            InitializeComponent();

            gnClinicID =(destype == 1 ? nqueue : 0);    //service centre if destype =1
            gnWardID = (destype == 2 ? nqueue : 0);     //ward if destype = 2
            gnDestype = destype; //1=wards, 2=service centres
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Orders_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + " << Orders >>";
            productlist(1);
            textBox2.Text = globalvar.gcUserName;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                MessageBox.Show("The clinic is " + gnClinicID);
                string dsql = "select srv_name from servcent where srv_id=" + gnClinicID;//?gnServiceCentre  ";
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                DataTable ds = new DataTable();
                da.Fill(ds);
                if (ds != null)
                {
                    textBox1.Text = ds.Rows[0]["srv_name"].ToString();//   comboBox2.DataSource = ds.Tables[0];
                    gcDestName = ds.Rows[0]["srv_name"].ToString();//   comboBox2.DataSource = ds.Tables[0];
                }
            }
        }


        private void gethrdetails()
        {
            /*
             	sn=SQLExec(gnConnHandle,"select dep_id,des_ID,susers.oprcode from susers,staff where staff.staffno=susers.staffno and susers.oprcode=?gcUserid ", "theuser")
	If !(sn>0 And Reccount()>0)
		=sysmsg('The User is not authorised to make requisitions')
		.nav_butt1.Visible = .f.
	*	Thisform.Release
	Else
		.text3.Value = dep_id
		.text4.Value = des_id
		.nav_butt1.Visible = .t.
	Endif
*		WAIT WINDOW 'service centre before refresh '+STR(gnServiceCentre)
	.Refresh
*		WAIT WINDOW 'service centre after refresh '+STR(gnServiceCentre)+' '+TYPE('gnServiceCentre')
Endwith
             */
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string ordersql23 = "select dep_id,des_id,susers.oprcode from susers,staff where staff.staffno=susers.staffno and susers.oprcode="+"'"+tcuserid+"'";
                SqlDataAdapter orderda23 = new SqlDataAdapter(ordersql23, ndConnHandle);
                orderda23.Fill(userview);
            if (userview.Rows.Count > 0)
                {
                    gnStaffDept = Convert.ToInt32(userview.Rows[0]["dep_id"]);
                    gnStaffDes = Convert.ToInt32(userview.Rows[0]["des_id"]);
                }
            }
    }
        private void productlist(int tntype)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                orderview.Clear();
                ndConnHandle.Open();
                switch (tntype)
                {
                    case 1:
                        string ordersql = "exec tsp_ClientOrderfromPha  " + ncompid;
                        SqlDataAdapter orderda = new SqlDataAdapter(ordersql, ndConnHandle);
                        orderda.Fill(orderview);
                        break;
                    case 2:
                        string ordersql1 = "exec tsp_ClientOrderfromSto  " + ncompid;
                        SqlDataAdapter orderda1 = new SqlDataAdapter(ordersql1, ndConnHandle);
                        orderda1.Fill(orderview);
                        break;
                    case 3:
                        string ordersql2 = "exec tsp_ClientOrderfromSug  " + ncompid;
                        SqlDataAdapter orderda2= new SqlDataAdapter(ordersql2, ndConnHandle);
                        orderda2.Fill(orderview);
                        break;
                }
                if (orderview.Rows.Count > 0)
                {
                    orderGrid.AutoGenerateColumns = false;
                    orderGrid.DataSource = orderview.DefaultView;
                    orderGrid.Columns[0].DataPropertyName = "selitem";
                    orderGrid.Columns[1].DataPropertyName = "prod_name";
                    orderGrid.Columns[2].DataPropertyName = "available";
                    orderGrid.Columns[3].DataPropertyName = "toissue";      // "age";
                    orderGrid.Columns[4].DataPropertyName = "prod_code";      // "age";

                    orderGrid.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                    orderGrid.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                    orderGrid.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                    orderGrid.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                    ndConnHandle.Close();
                    orderGrid.Focus();
                }
            }
        }//end of productlist

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
//                MessageBox.Show("From pharmacy");
                productlist(1);
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
  //              MessageBox.Show("From mainstore");
                productlist(2);
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
     //           MessageBox.Show("From surgical store");
                productlist(3);
            }
        }

        private void orderGrid_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void orderGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            orderGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void orderGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (orderGrid.Columns[e.ColumnIndex].Name == "requ")
            {
                if (Convert.ToInt32(orderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) > 0)
                {
                    selcount++;
                }
                else
                {
                    selcount--;
                }

                if (selcount > 0)
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
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                MessageBox.Show("inside save button");
                string tcCode = "";
                string cquery0 = "insert into orders (toissue,srv_id,oprcode,compid,ord_date,ord_time,ordsource,orddest,hwa_id,ccustcode,prod_code,dep_ID,des_id,nopenbal) values " ;
                cquery0 += "(@ltoissue,@lnServiceCentre,@gcUserid,@gnCompid,convert(date,getdate()),convert(time,getdate()),@Ordsource,@lorddest,@lnWardID,@lcCustCode,@lcProdCode,@lnDept,@lnDes,@lnOpenBal)";

                int lnordsource = (radioButton1.Checked ? 1 : (radioButton2.Checked ? 0 : 9)); // Iif(.optiongroup1.Value = 1, 1, IIF(.optiongroup1.Value = 2, 0, 2)) && Source of order (main store = 0, pharmacy = 1, surgical = 2)
                 SqlDataAdapter patupd0 = new SqlDataAdapter();
                patupd0.InsertCommand = new SqlCommand(cquery0, ndConnHandle);
                ndConnHandle.Open();
                foreach(DataRow drow in orderview.Rows)
                {
                    if(Convert.ToInt32(drow["toissue"])>0)
                    {
                        patupd0.InsertCommand.Parameters.Add("@ltoissue", SqlDbType.Int).Value = Convert.ToInt32(drow["toissue"]);
                        patupd0.InsertCommand.Parameters.Add("@lnServiceCentre", SqlDbType.Int).Value = gnClinicID;             //service centre to deliver to
                        patupd0.InsertCommand.Parameters.Add("@gcUserid", SqlDbType.Char).Value = globalvar.gcUserid;
                        patupd0.InsertCommand.Parameters.Add("@gnCompid", SqlDbType.Int).Value = globalvar.gnCompid;
                        patupd0.InsertCommand.Parameters.Add("@Ordsource", SqlDbType.Int).Value = lnordsource;                  //source type: 0=main store, 1 = pharmacy, 2=surgical
                        patupd0.InsertCommand.Parameters.Add("@lorddest", SqlDbType.Int).Value = gnDestype;                     //destination type : 1= wards, 2 = service centres
                        patupd0.InsertCommand.Parameters.Add("@lnWardID", SqlDbType.Int).Value = gnWardID;                      //globalvar.gnIntDocID;
                        patupd0.InsertCommand.Parameters.Add("@lcCustCode", SqlDbType.Char).Value = tcCode;
                        patupd0.InsertCommand.Parameters.Add("@lcProdCode", SqlDbType.Char).Value = drow["prod_code"];          // nvisno;
                        patupd0.InsertCommand.Parameters.Add("@lnDept", SqlDbType.Int).Value = gnStaffDept;
                        patupd0.InsertCommand.Parameters.Add("@lnDes", SqlDbType.Int).Value = gnStaffDes;
                        patupd0.InsertCommand.Parameters.Add("@lnOpenBal", SqlDbType.Decimal).Value = drow["available"];
                        MessageBox.Show("we will now insert into orders");
                        patupd0.InsertCommand.ExecuteNonQuery();
                        patupd0.InsertCommand.Parameters.Clear();
                    }
                }
                ndConnHandle.Close();
                SaveButton.Enabled = false;
                SaveButton.BackColor = Color.Gainsboro;
                orderview.Clear();
                productlist(1);
            }
            /*
             With Thisform
    This.Enabled=.F.
    lnDept=.text3.value
    lnDes=.text4.value
    lnServiceCentre=Iif(Type('gnServiceCentre')='U',gnTempServiceCentre,gnServiceCentre)
    lnWardID=Iif(Type('gnWardID')<>'U',gnWardID,0)
    lordsource=Iif(.optiongroup1.Value=1,1,IIF(.optiongroup1.Value=2,0,2))	&&Source of order (main store=0,pharmacy=1,surgical=2)
    lorddest=1						&&Destination of order is (1=Wards,2=other Service centres,3=client)
    lcCustCode=gcClientCode
    Select tempProduct
    Set Filter To selitem
    Locate
    Do While !Eof()
        lnprodid=product_id
        ltoissue=toissue
        lcProdCode=prod_code
        lnOpenBal=available
        sn=SQLExec(gnConnHandle,"insert into orders (product_id,toissue,srv_id,oprcode,compid,ord_date,ord_time,ordsource,orddest,hwa_id,ccustcode,prod_code,dep_ID,des_id,nopenbal) values "+;
            "(?lnProdid,?ltoissue,?lnServiceCentre,?gcUserid,?gnCompid,convert(date,getdate()),convert(time,getdate()),?lOrdsource,?lorddest,?lnWardID,?lcCustCode,?lcProdCode,?lnDept,?lnDes,?lnOpenBal)","orderin")
        If !(sn>0 )
            =sysmsg("Could not update orders, inform IT DEPT")
        Endif
        Select tempProduct
        Skip
    Enddo
    gcInputStat='N'
    .combo2.Value=0
    Select tempProduct
    Set Filter To
    Replace All selitem With .F.
    Replace All toissue With 0
    Locate
    If gcOrderType='C'
        .combo2.Value=0
    Endif
    glProdSelected=.F.
    .Refresh
Endwith

             */
        }
        }
}
