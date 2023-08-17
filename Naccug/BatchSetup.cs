using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using TclassLibrary;

namespace WinTcare
{
    public partial class BatchSetup : Form
    {
        string cs = globalvar.cos;
        int ncompid = globalvar.gnCompid;
        string dloca = globalvar.cLocalCaption;

        DataTable productview = new DataTable();
        DataTable clientview = new DataTable();
        DataTable batview = new DataTable();
        DataTable nbatview = new DataTable();
        DataTable prodview = new DataTable();
        DataTable contview = new DataTable();

//        DataTable memview = new DataTable();

        string gcFname = string.Empty;
        string gcMname = string.Empty;
        string gcLname = string.Empty;
        string gcAcctNumb = string.Empty;

  //      public DataTable reMemView = new DataTable();
        int gnBatchNumb = 0;
 

        bool glnewBatch = true;
        public BatchSetup()
        {
            InitializeComponent();
        }

        private void PayrollSetup_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + "<< Batch Setup >>";
            //            getclientList();
            makeviews();
            getBatchList();
            getDetails();
            getContra();
        }

        private void makeviews()
        {
            prodview.Columns.Add("prd_id");
            prodview.Columns.Add("maincat");
            prodview.Columns.Add("prd_name");
            prodview.Columns.Add("selprd");
            prodview.Columns.Add("product");

            prodGrid.AutoGenerateColumns = false;
            prodGrid.DataSource = prodview.DefaultView;
            prodGrid.Columns[0].DataPropertyName = "prd_id";
            prodGrid.Columns[1].DataPropertyName = "maincat";
            prodGrid.Columns[2].DataPropertyName = "prd_name";
            prodGrid.Columns[3].DataPropertyName = "selprd";
            prodGrid.Columns[5].DataPropertyName = "product";
        }
        private void getBatchList()
        {
            string dsqlb = "select bat_name,bat_id from batch_main order by bat_name  " ;
            batview.Clear();

            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter dab = new SqlDataAdapter(dsqlb, ndConnHandle);
                dab.Fill(batview);
                if (batview.Rows.Count > 0)
                {
                    comboBox3.DataSource = batview.DefaultView;
                    comboBox3.DisplayMember = "bat_name";
                    comboBox3.ValueMember = "bat_id";
                    comboBox3.SelectedIndex = -1;
                }
            }
        }//end of getBatchlist

        private void getContra()
        {
            string dsqlbb = "select subgrp.subgrpcode, cacctnumb, cacctname from  subgrp, glmast where subgrp.lbank= 1 and subgrp.subgrpcode = glmast.acode and glmast.intcode = 1 ";
            batview.Clear();

            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter dabb = new SqlDataAdapter(dsqlbb, ndConnHandle);
                dabb.Fill(contview);
                if (contview.Rows.Count > 0)
                {
                    comboBox4.DataSource = contview.DefaultView;
                    comboBox4.DisplayMember = "cacctname";
                    comboBox4.ValueMember = "cacctnumb";
                    comboBox4.SelectedIndex = -1;
                }
            }
        }//end of getBatchlist

        

        private void getDetails()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                //************Getting product type                
                //                string dsql2 = "select prd_id,prd_name from prodtype ";
                string dsql2 = "select maincat, prd_id, prd_name, adescrip from prodtype, acc_type where prodtype.maincat = acc_type.acode";
                SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle);
//                DataTable ds2 = new DataTable();
                da2.Fill(productview);
                if (productview != null)
                {
                    comboBox1.DataSource = productview.DefaultView;
                    comboBox1.DisplayMember = "prd_name";
                    comboBox1.ValueMember = "prd_id";
                    comboBox1.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not find product types, inform IT Dept immediately"); }

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
            if (TextBox1.Text!="") //&& Convert.ToInt16(comboBox2.SelectedValue)>0)// && glPage01Page05)
            {
                saveButton.Enabled = true;
                saveButton.BackColor = Color.LawnGreen;
                saveButton.Select();
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

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (glnewBatch)
            {
                insertBatch();          //insert a  new batch
               // getbatchnumber();
               // insertProduct();        //insert products to the batch
               // insertMembers();        //insert members to the batch
            }
            else
            {
                updateBatch();          //update name of batch
               // insertProduct();        //insert products to the batch
               // insertMembers();        //insert members to the batch
            }
            initvariables();
            saveButton.Enabled = false;
            saveButton.BackColor = Color.Gainsboro;

        }

        private void getbatchnumber()
        {
            string dsqlb = "select bat_id from batch_main order by bat_id desc  ";
            nbatview.Clear();
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter nbatadp = new SqlDataAdapter(dsqlb, ndConnHandle);
                nbatadp.Fill(nbatview);
                if (nbatview.Rows.Count > 0)
                {

                    gnBatchNumb = Convert.ToInt16(nbatview.Rows[0]["bat_id"]);
                }

            }
        }
        private void insertBatch()
        {
             string lcBat_name = TextBox1.Text.ToString();


            using (SqlConnection nConnHandle2 = new SqlConnection(cs))
            {
                string cglquery = "insert into batch_main (bat_name,compid,bat_cont) values (@lbat_name,@lcompid,@contra)";
                SqlDataAdapter insCommand = new SqlDataAdapter();
                insCommand.InsertCommand = new SqlCommand(cglquery, nConnHandle2);
                insCommand.InsertCommand.Parameters.Add("@lbat_name", SqlDbType.VarChar).Value = lcBat_name ;
                insCommand.InsertCommand.Parameters.Add("@lcompid", SqlDbType.Int).Value = ncompid ;
                insCommand.InsertCommand.Parameters.Add("@contra", SqlDbType.VarChar).Value = comboBox4.SelectedValue.ToString();
                nConnHandle2.Open();
                insCommand.InsertCommand.ExecuteNonQuery();
                nConnHandle2.Close();
            }
    }

        private void insertProduct()
        {
            int lnProdid = Convert.ToInt32(comboBox1.SelectedValue);
            using (SqlConnection nConnHandle2 = new SqlConnection(cs))
            {
                string cglquery = "insert into Batch_products (bat_id,prd_id,compid) values (@lbatid,@lprodid,@ncompid)";
                SqlDataAdapter insCommand = new SqlDataAdapter();
                insCommand.InsertCommand = new SqlCommand(cglquery, nConnHandle2);
                insCommand.InsertCommand.Parameters.Add("@lbatid", SqlDbType.Int).Value = gnBatchNumb;
                insCommand.InsertCommand.Parameters.Add("@lprodid", SqlDbType.Int).Value = lnProdid;
                insCommand.InsertCommand.Parameters.Add("@ncompid", SqlDbType.Int).Value = ncompid;
                nConnHandle2.Open();
                insCommand.InsertCommand.ExecuteNonQuery();
                nConnHandle2.Close();
            }
        }

        private void insertMembers()
        {
 //           string lcmemcode = comboBox2.SelectedValue.ToString();
            using (SqlConnection nConnHandle2 = new SqlConnection(cs))
            {
                nConnHandle2.Open();
                string cglquery = "insert into Batch_members (bat_id,prd_id,ccustcode,compid) values (@lbatid,@lprodid,@cmemcode,@ncompid)";
                SqlDataAdapter insCommand = new SqlDataAdapter();
                insCommand.InsertCommand = new SqlCommand(cglquery, nConnHandle2);
                for (int i=0;i< clientview.Rows.Count;i++)
                {
                    insCommand.InsertCommand.Parameters.Add("@lbatid", SqlDbType.Int).Value = gnBatchNumb;
                    insCommand.InsertCommand.Parameters.Add("@lprodid", SqlDbType.Int).Value = Convert.ToInt32(comboBox1.SelectedValue);
                    insCommand.InsertCommand.Parameters.Add("@cmemcode", SqlDbType.Char).Value = clientview.Rows[i]["ccustcode"].ToString();
                    insCommand.InsertCommand.Parameters.Add("@ncompid", SqlDbType.Int).Value = ncompid;

                    insCommand.InsertCommand.ExecuteNonQuery();
                    insCommand.InsertCommand.Parameters.Clear();
                }
                nConnHandle2.Close();
            }
        }


        private void updateBatch()
        {

        }


        private void updateBatchSums(int updtype)
        {
            if(updtype==1)
            {
                //update batch_main set pro_numb = pro_numb+1
            }
            else
            {
                //update batch_main set mem_numb = mem_numb+1
            }

        }
        private void initvariables()
        {
            TextBox1.Text = "";
            comboBox1.SelectedIndex = -1;
//            comboBox2.SelectedIndex = - 1;
            clientview.Clear();
            prodview.Clear();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            AllClear2Go();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("under construction");
            glnewBatch = false;
            TextBox1.Visible = false;
            comboBox3.Visible = true;
            comboBox3.Focus();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Focused)
            {
                int dtex =Convert.ToInt16(comboBox1.SelectedValue);
                DataRow drow = prodview.NewRow();
                drow["prd_id"] = TextBox1.Text.Trim(); 
                drow["maincat"] = Convert.ToString(productview.Rows[comboBox1.SelectedIndex]["adescrip"]).Trim();
                drow["prd_name"] = Convert.ToString(comboBox1.Text).Trim();
                drow["selprd"] = true;
                drow["product"] = dtex.ToString();
                prodview.Rows.Add(drow);
        //        getmembers();
                AllClear2Go();
            }
        }
        /*
        private void getmembers()
        { 
            string dsql = "exec tsp_getMembers  " + ncompid;
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
                    clientgrid.Columns[1].DataPropertyName = "fname";
                    clientgrid.Columns[2].DataPropertyName = "mname";
                    clientgrid.Columns[3].DataPropertyName = "lname";
                    ndConnHandle.Close();
                    for (int i = 0; i < 10; i++)
                    {
                        clientview.Rows.Add();
                    }
                    clientgrid.Focus();
                }
            }
        }//end of getclientlist
        */

        private void prodGrid_Enter(object sender, EventArgs e)
        {
        //    MessageBox.Show("The grid has the focus, we will enable members");
        }

        private void prodGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!(prodGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex != -1)) return;
            {
                   using (var findform = new FindClient(cs,ncompid,dloca,2, "Cusreg"))
                   {
                       var dresult = findform.ShowDialog();
                       if (dresult == DialogResult.OK)
                       {
                        string dclientcode = findform.returnValue;
                        checkClient(dclientcode);
                        DataRow mrow = clientview.NewRow();
                        mrow["prdname"]= Convert.ToString(comboBox1.Text).Trim();
                        mrow["cacctnumb"] = gcAcctNumb;
                        mrow["fname"] = gcFname;
                        mrow["mname"] = gcMname;
                        mrow["lname"] = gcLname;
                        mrow["selprd"] = true;
                        mrow["prdid"] = Convert.ToInt16(comboBox1.SelectedValue);
                        mrow["ccustcode"] = dclientcode;
                        clientview.Rows.Add(mrow);
                        AllClear2Go();
                    }
                }
            }
        }

        private void checkClient(string tcCode)
        {
            string ncompid = globalvar.gnCompid.ToString().Trim();
            string dsql = "exec tsp_New_Client_One  " + ncompid + ",'" + tcCode + "'";
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                DataTable clv = new DataTable();
                da.Fill(clv);
                if (clv.Rows.Count > 0)
                {
                    gcFname = clv.Rows[0]["fname"] .ToString();
                    gcMname = clv.Rows[0]["mname"].ToString();
                    gcLname = clv.Rows[0]["lname"].ToString();
                    gcAcctNumb = clv.Rows[0]["cacctnumb"].ToString();
                }
            }
        }
        private void prodGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
