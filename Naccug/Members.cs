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
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Web;
using AForge.Video;
using AForge.Video.DirectShow;
using System.IO;
using System.Net.NetworkInformation;
using RestSharp;
using System.Net;
using System.Transactions;

namespace WinTcare
{
    public partial class Members : Form
    {
        string cs = globalvar.cos;
        int ncompid = globalvar.gnCompid;
        int nwebcams = 0;
        int lnPrd_ID = 0;
        string dloca = globalvar.cLocalCaption;
        string gcInputStat = "N";
        string gcNonInterestAcct = string.Empty;
        string gcJoinFeeAcct = string.Empty;
        string gcMemberSavingsAcct = string.Empty;
        string gcMemberSharesAcct = string.Empty;

        string gcSavingsControlAcct = string.Empty;
        string gcSharesControlAcct = string.Empty;

        decimal gnRegistrationFee = 0.00m;
        decimal gnSharePrice = 0.00m;
        //string gcMemberPicture=

        string gcMembCode = "";
        string gcMembName = "";
        DataTable emplview = new DataTable();
        DataTable batview = new DataTable();
        DataTable memberview = new DataTable();
        DataTable mandview = new DataTable();
        DataTable mandview1 = new DataTable();

        public EventHandler RefreshNeeded;
        bool glPage01Page01 = false;
        bool glPage01Page02 = false;
        bool glPage01Page03 = false;
        bool glPage01Page04 = false;

        bool glPage02Page01 = false;
        bool glPage02Page02 = false;
        bool glPage02Page03 = false;
        bool glPage02Page04 = false;

        public Members()
        {
            InitializeComponent();
        }
        private FilterInfoCollection webcam;
        private VideoCaptureDevice cam;

        private void button10_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void cam_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bit = (Bitmap)eventArgs.Frame.Clone();
            pictureBox1.Image = bit;
        }

    
        private void Members_Load(object sender, EventArgs e)
        {
           
            pictureBox1.Image = Properties.Resources.CULogo;// Image.FromStream.this.ic "";
            webcam = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo VideoCaptureDevice in webcam)
            {
                webcams.Items.Add(VideoCaptureDevice.Name);
            }
            nwebcams = webcams.Items.Count;
            webcams.SelectedIndex = nwebcams >0 ? 0 : -1;

            comtrib();
            comblvelPoverty();
            combleveledu();
            this.Text = globalvar.cLocalCaption + "<< Member Registration >>";
            getBatchList();
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();

                //************The code block below is deprecated and replace by GetClient_Code.clientCode_int,  a class in TClassLibrary
                textBox42.Text = GetClient_Code.clientCode_int(cs, "clientid").ToString().Trim().PadLeft(6, '0');
                textBox46.Text = GetClient_Code.clientCode_int(cs, "clientid").ToString().Trim().PadLeft(6, '0');

                //************Getting nationality
                string dsql0 = "exec tsp_GetCountry  ";
                SqlDataAdapter da0 = new SqlDataAdapter(dsql0, ndConnHandle);
                DataSet ds0 = new DataSet();
                da0.Fill(ds0);
                if (ds0 != null)
                {
                    comboBox3.DataSource = ds0.Tables[0];
                    comboBox3.DisplayMember = "cou_name";
                    comboBox3.ValueMember = "cou_id";
                    comboBox3.SelectedIndex = -1;
                }

                //************Getting country
                string dsqcl0 = "exec tsp_GetCountry  ";
                SqlDataAdapter dac0 = new SqlDataAdapter(dsqcl0, ndConnHandle);
                DataSet dsc0 = new DataSet();
                DataTable corCoun = new DataTable();

                dac0.Fill(dsc0);
                dac0.Fill(corCoun);

                if (dsc0 != null)
                {
                    comboBox9.DataSource = dsc0.Tables[0];
                    comboBox9.DisplayMember = "cou_name";
                    comboBox9.ValueMember = "cou_id";
                    comboBox9.SelectedIndex = -1;

                    comboBox112.DataSource = corCoun.DefaultView; ;
                    comboBox112.DisplayMember = "cou_name";
                    comboBox112.ValueMember = "cou_id";
                    comboBox112.SelectedIndex = -1;
                }


                //************Getting Titles
                string dsql1 = "select tit_name,tit_id from titres order by tit_name ";
                SqlDataAdapter da1 = new SqlDataAdapter(dsql1, ndConnHandle);
                DataTable ds1 = new DataTable();
                da1.Fill(ds1);
                if (ds1 != null)
                {
                    comboBox2.DataSource = ds1.DefaultView;
                    comboBox2.DisplayMember = "tit_name";
                    comboBox2.ValueMember = "tit_id";
                    comboBox2.SelectedIndex = -1;
                }

                //************Getting branch id
                string dsqlb = "select br_name,branchid from branch order by br_name ";
                SqlDataAdapter dab = new SqlDataAdapter(dsqlb, ndConnHandle);
                DataTable dsb = new DataTable();
                DataTable corbr = new DataTable();
                dab.Fill(dsb);
                dab.Fill(corbr);

                if (dsb != null)
                {
                    comboBox1.DataSource = dsb.DefaultView;
                    comboBox1.DisplayMember = "br_name";
                    comboBox1.ValueMember = "branchid";
                    comboBox1.SelectedIndex = -1;

                    comboBox16.DataSource = corbr.DefaultView;
                    comboBox16.DisplayMember = "br_name";
                    comboBox16.ValueMember = "branchid";
                    comboBox16.SelectedIndex = -1;
                }

                //************Getting counties/regions
                string dsql2 = "select coun_name,coun_id from county order by coun_name ";
                SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle);
                DataTable ds2 = new DataTable();
                DataTable ds2c = new DataTable();
                da2.Fill(ds2);
                da2.Fill(ds2c);
                if (ds2 != null)
                {
                    comboBox6.DataSource = ds2.DefaultView;
                    comboBox6.DisplayMember = "coun_name";
                    comboBox6.ValueMember = "coun_id";
                    comboBox6.SelectedIndex = -1;

                    comboBox101.DataSource = ds2c.DefaultView;
                    comboBox101.DisplayMember = "coun_name";
                    comboBox101.ValueMember = "coun_id";
                    comboBox101.SelectedIndex = -1;
                }


                //= opendbf('area', 'area_id')
                string dsql3 = "select area_name,area_id from area order by area_name ";
                SqlDataAdapter da3 = new SqlDataAdapter(dsql3, ndConnHandle);
                DataTable ds3 = new DataTable();
                DataTable ds3c = new DataTable();
                da3.Fill(ds3);
                da3.Fill(ds3c);
                if (ds3 != null)
                {
                    comboBox7.DataSource = ds3.DefaultView;
                    comboBox7.DisplayMember = "area_name";
                    comboBox7.ValueMember = "area_id";
                    comboBox7.SelectedIndex = -1;

                    comboBox92.DataSource = ds3c.DefaultView;
                    comboBox92.DisplayMember = "area_name";
                    comboBox92.ValueMember = "area_id";
                    comboBox92.SelectedIndex = -1;
                }

                //get identification number
                string dsql4 = "exec tsp_GetIDTYPE ";
                SqlDataAdapter da4 = new SqlDataAdapter(dsql4, ndConnHandle);
                DataTable ds4 = new DataTable();
                da4.Fill(ds4);
                if (ds4 != null)
                {
                    comboBox5.DataSource = ds4.DefaultView;
                    comboBox5.DisplayMember = "id_name";
                    comboBox5.ValueMember = "idtype";
                    comboBox5.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not get ID Types, inform IT Dept immediately"); }

                //get ward5 number which is the smallest administrative unit
                string dsql5 = "select ward_name,ward_id from ward order by ward_name ";
                SqlDataAdapter da5 = new SqlDataAdapter(dsql5, ndConnHandle);
                DataTable ds5 = new DataTable();
                DataTable ds5c = new DataTable();
                da5.Fill(ds5);
                da5.Fill(ds5c);
                if (ds5 != null)
                {
                    comboBox8.DataSource = ds5.DefaultView;
                    comboBox8.DisplayMember = "ward_name";
                    comboBox8.ValueMember = "ward_id";
                    comboBox8.SelectedIndex = -1;

                    comboBox81.DataSource = ds5c.DefaultView;
                    comboBox81.DisplayMember = "ward_name";
                    comboBox81.ValueMember = "ward_id";
                    comboBox81.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not find ward Types, inform IT Dept immediately"); }

                //get marital status
                string dsql6 = "select mar_name,mar_id from marystat order by mar_name ";
                SqlDataAdapter da6 = new SqlDataAdapter(dsql6, ndConnHandle);
                DataTable ds6 = new DataTable();
                da6.Fill(ds6);
                if (ds6 != null)
                {
                    comboBox4.DataSource = ds6.DefaultView;
                    comboBox4.DisplayMember = "mar_name";
                    comboBox4.ValueMember = "mar_id";
                    comboBox4.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not find marital status, inform IT Dept immediately"); }

                //tsp_GetRelation
                string dsql7 = "exec tsp_GetFamilyRelation ";
                SqlDataAdapter da7 = new SqlDataAdapter(dsql7, ndConnHandle);
                DataTable ds7 = new DataTable();
                da7.Fill(ds7);
                if (ds7 != null)
                {
                    comboBox11.DataSource = ds7.DefaultView;
                    comboBox11.DisplayMember = "fam_relation";
                    comboBox11.ValueMember = "fam_id";
                    comboBox11.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not find relations, inform IT Dept immediately"); }


                //Get designation tsp_GetDesig
                string dsql8 = "exec tsp_GetDesig "+ncompid;
                SqlDataAdapter da8 = new SqlDataAdapter(dsql8, ndConnHandle);
                DataTable ds8 = new DataTable();
                da8.Fill(ds8);
                if (ds8 != null)
                {
                    comboBox13.DataSource = ds8.DefaultView;
                    comboBox13.DisplayMember = "des_name";
                    comboBox13.ValueMember = "des_id";
                    comboBox13.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not find designation, inform IT Dept immediately"); }


                //get department tsp_GetDept
                string dsql9 = "exec tsp_GetDept "+ncompid;
                SqlDataAdapter da9 = new SqlDataAdapter(dsql9, ndConnHandle);
                DataTable ds9 = new DataTable();
                da9.Fill(ds9);
                if (ds9 != null)
                {
                    comboBox14.DataSource = ds9.DefaultView;
                    comboBox14.DisplayMember = "dep_name";
                    comboBox14.ValueMember = "dep_id";
                    comboBox14.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not find departments, inform IT Dept immediately"); }

                //get the Credit Union Details
                string dsql10 = "exec tsp_getcomp " + ncompid;
                SqlDataAdapter da10 = new SqlDataAdapter(dsql10, ndConnHandle);
                DataTable ds10 = new DataTable();
                da10.Fill(ds10);
                if (ds10 != null)
                {
                    textBox28.Text = ds10.Rows[0]["com_name"].ToString();
                    textBox19.Text = ds10.Rows[0]["com_name"].ToString();
                    maskedTextBox1.Text = Convert.ToDecimal(ds10.Rows[0]["regfee"]).ToString("N2");
                    maskedTextBox2.Text =Convert.ToDecimal(ds10.Rows[0]["shareprice"]).ToString("N2");
                    gnRegistrationFee = Convert.ToDecimal(ds10.Rows[0]["regfee"]);
                    gnSharePrice = Convert.ToDecimal(ds10.Rows[0]["shareprice"]);

                    maskedTextBox8.Text = Convert.ToDecimal(ds10.Rows[0]["regfee"]).ToString("N2");
                    maskedTextBox9.Text = Convert.ToDecimal(ds10.Rows[0]["shareprice"]).ToString("N2");
                    textBox22.Text = ds10.Rows[0]["regAcct"].ToString().Trim();
                    gcJoinFeeAcct = ds10.Rows[0]["regAcct"].ToString().Trim();
                    textBox24.Text = ds10.Rows[0]["regAcctName"].ToString().Trim();
                }
                else { MessageBox.Show("Could not find Credit Union details, inform IT Dept immediately"); }

                //tsp_getEmployer
                string dsql11 = "exec tsp_getEmployer " ;
                SqlDataAdapter da11 = new SqlDataAdapter(dsql11, ndConnHandle);
                da11.Fill(emplview);
                if (emplview != null)
                {
                    comboBox12.DataSource = emplview.DefaultView;
                    comboBox12.DisplayMember = "emp_name";
                    comboBox12.ValueMember = "emp_id";
                    comboBox12.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not find employer details, inform IT Dept immediately"); }

                //************Getting business nature
                string dsql1b = "select cat_name,cat_id from bizcat order by cat_name ";
                SqlDataAdapter da1b = new SqlDataAdapter(dsql1b, ndConnHandle);
                DataTable ds1b = new DataTable();
                da1b.Fill(ds1b);
                if (ds1b != null)
                {
                    comboBox18.DataSource = ds1b.DefaultView;
                    comboBox18.DisplayMember = "cat_name";
                    comboBox18.ValueMember = "cat_id";
                    comboBox18.SelectedIndex = -1;
                }

                //************Getting Mandatory products
                string sqlmand = "select ltrim(rtrim(prd_name)) as prd_name,prod_control,prd_id,nint_inc from prodtype where man_product =1 order by prd_name ";
                SqlDataAdapter da1bm = new SqlDataAdapter(sqlmand, ndConnHandle);
                da1bm.Fill(mandview);
                if (mandview.Rows.Count> 0)
                {
                    prodGrid.AutoGenerateColumns = false;
                    prodGrid.DataSource = mandview.DefaultView;
                    prodGrid.Columns[0].DataPropertyName = "prd_name";
                    prodGrid.Columns[1].DataPropertyName = "prod_control";
                }else { MessageBox.Show("No mandatory fields found"); }


                //************Getting Mandatory products
               // mandview.Clear();
                string sqlmand1 = "select ltrim(rtrim(prd_name)) as prd_name,prod_control,prd_id,nint_inc from prodtype where man_product =1 order by prd_name ";
                SqlDataAdapter da1bm1 = new SqlDataAdapter(sqlmand1, ndConnHandle);
                da1bm1.Fill(mandview1);
                if (mandview1.Rows.Count > 0)
                {
                    prodGrid1.AutoGenerateColumns = false;
                    prodGrid1.DataSource = mandview1.DefaultView;
                    prodGrid1.Columns[0].DataPropertyName = "prd_name";
                    prodGrid1.Columns[1].DataPropertyName = "prod_control";
                }
                else { MessageBox.Show("No mandatory fields found"); }
                textBox1.Focus();
                textBox1.Focus();
            }
        }

        private void getBatchList()
        {
            string dsqlb = "select bat_name,bat_id from batch_main order by bat_name  ";
            batview.Clear();

            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter dab = new SqlDataAdapter(dsqlb, ndConnHandle);
                dab.Fill(batview);
                if (batview.Rows.Count > 0)
                {
                    comboBox15.DataSource = batview.DefaultView;
                    comboBox15.DisplayMember = "bat_name";
                    comboBox15.ValueMember = "bat_id";
                    comboBox15.SelectedIndex = -1;

                    comboBox19.DataSource = batview.DefaultView;
                    comboBox19.DisplayMember = "bat_name";
                    comboBox19.ValueMember = "bat_id";
                    comboBox19.SelectedIndex = -1;
                }
            }
        }//end of getBatchlist
        private void getcity(int couid)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string dsql7 = "exec tsp_GetCity "+couid;
                SqlDataAdapter da7 = new SqlDataAdapter(dsql7, ndConnHandle);
                DataTable ds7 = new DataTable();
                da7.Fill(ds7);
                if (ds7 != null)
                {
                    comboBox10.DataSource = ds7.DefaultView;
                    comboBox10.DisplayMember = "city_name";
                    comboBox10.ValueMember = "city_id";
                    comboBox10.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not find city name, inform IT Dept immediately"); }
            }
        }

        private void getcity1(int couid)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string dsql7c = "exec tsp_GetCity " + couid;
                SqlDataAdapter da7c = new SqlDataAdapter(dsql7c, ndConnHandle);
                DataTable ds7c = new DataTable();
                da7c.Fill(ds7c);
                if (ds7c != null)
                {
                    comboBox123.DataSource = ds7c.DefaultView;
                    comboBox123.DisplayMember = "city_name";
                    comboBox123.ValueMember = "city_id";
                    comboBox123.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not find city name, inform IT Dept immediately"); }
            }
        }
        private void comtrib()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string dsql2 = "select tname,code from tribe_setup order by tname ";
                SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle);
                DataTable ds2 = new DataTable();
                DataTable ds2c = new DataTable();
                da2.Fill(ds2);
                da2.Fill(ds2c);
                if (ds2 != null)
                {
                    comboBox21.DataSource = ds2.DefaultView;
                    comboBox21.DisplayMember = "tname";
                    comboBox21.ValueMember = "code";
                    comboBox21.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not find Tribes, inform IT Dept immediately"); }

            }
        }

        private void combleveledu()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string dsql2 = "select tname,code from levelOfedu order by tname ";
                SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle);
                DataTable ds2 = new DataTable();
                DataTable ds2c = new DataTable();
                da2.Fill(ds2);
                da2.Fill(ds2c);
                if (ds2 != null)
                {
                    comboBox20.DataSource = ds2.DefaultView;
                    comboBox20.DisplayMember = "tname";
                    comboBox20.ValueMember = "code";
                    comboBox20.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not find Tribes, inform IT Dept immediately"); }

            }
        }
        private void comblvelPoverty()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string dsql2 = "select pname,code from povertylevel order by code ";
                SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle);
                DataTable ds2 = new DataTable();
                DataTable ds2c = new DataTable();
                da2.Fill(ds2);
                da2.Fill(ds2c);
                if (ds2 != null)
                {
                    comboBox22.DataSource = ds2.DefaultView;
                    comboBox22.DisplayMember = "pname";
                    comboBox22.ValueMember = "code";
                    comboBox22.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not find Tribes, inform IT Dept immediately"); }

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

        
        private void page01Ok()
        {
            int ncity = Convert.ToInt32(comboBox3.SelectedValue);
            //************************************pageframe 1, page 1 validation
            glPage01Page01 = (textBox1.Text != string.Empty && textBox3.Text != string.Empty && Convert.ToInt32(comboBox1.SelectedValue) > 0 && Convert.ToInt32(comboBox2.SelectedValue) > 0 && Convert.ToInt32(comboBox3.SelectedValue) > 0 &&
             Convert.ToInt32(comboBox4.SelectedValue) > 0 && Convert.ToInt32(comboBox5.SelectedValue) > 0 && Convert.ToInt32(comboBox6.SelectedValue) > 0 && Convert.ToInt32(comboBox7.SelectedValue) > 0 && Convert.ToInt32(comboBox8.SelectedValue) > 0 &&
             textBox4.Text != string.Empty && textBox5.Text != string.Empty && (dmale.Checked || dfemale.Checked) ? true : false);


            //************************************pageframe 1, page 2 validation
            glPage01Page02 = (Convert.ToInt32(comboBox9.SelectedValue) > 0 && Convert.ToInt32(comboBox10.SelectedValue) > 0 && textBox6.Text != string.Empty && textBox12.Text != string.Empty &&
               Convert.ToInt32(comboBox11.SelectedValue) > 0 && textBox13.Text != string.Empty ? true : false);

//            if (glPage01Page02)
  //          {
    //            MessageBox.Show("page 2 is ok");
      //      }


            //************************************pageframe 1, page 3 validation
           // glPage01Page03 = (Convert.ToInt32(comboBox12.SelectedValue) > 0 && Convert.ToInt32(comboBox13.SelectedValue) > 0 && textBox18.Text != string.Empty &&
             //  Convert.ToInt32(comboBox14.SelectedValue) > 0 && maskedTextBox6.Text != string.Empty ? true : false);

    //        if (glPage01Page03)
      //      {
        //        MessageBox.Show("page 3 is ok");
          //  }


            //***********************************pageframe 1, page 4 validation
            if (maskedTextBox1.Text !=null && maskedTextBox2.Text !=null)
            {
                glPage01Page04 =(textBox25.Text!="" && textBox23.Text != string.Empty && maskedTextBox3.Text!= "" ? true : false) ;
            }

//            if (glPage01Page04)
  //          {
    //            MessageBox.Show("page 4 is ok");
      //      }

            AllClear2Go();
        }//end of page 01


        #region Checking if all the mandatory conditions are satisfied
        private void AllClear2Go()
        {
            if (glPage01Page01 && glPage01Page02  && glPage01Page04)// && glPage01Page05)
            {
                IndSaveButton.Enabled = true;
                IndSaveButton.BackColor = Color.LawnGreen;
                IndSaveButton.Select();
            }
            else
            {
                IndSaveButton.Enabled = false;
                IndSaveButton.BackColor = Color.Gainsboro;
            }
        }
        #endregion 

        private void page02Ok()
        {
            int lnmemtype = radioButton18.Checked ? 2 : 3;
            //************************************pageframe 2, page 1 validation
            glPage02Page01 = (textBox45.Text != string.Empty && Convert.ToInt32(comboBox16.SelectedValue) > 0 && Convert.ToInt32(comboBox18.SelectedValue) > 0 &&
             Convert.ToInt32(comboBox112.SelectedValue) > 0 && Convert.ToInt32(comboBox123.SelectedValue) > 0 && Convert.ToInt32(comboBox101.SelectedValue) > 0 &&
             Convert.ToInt32(comboBox92.SelectedValue) > 0 && Convert.ToInt32(comboBox81.SelectedValue) > 0 && textBox39.Text != string.Empty &&
            textBox120.Text != string.Empty && textBox436.Text != string.Empty && textBox407.Text != string.Empty && lnmemtype>1 ? true : false) ;
            if (glPage02Page01)
            {
          //      MessageBox.Show("frame 2, page 1 is ok");
            }
            //************************************pageframe 2, page 2 validation
            glPage02Page02 = (textBox534.Text.ToString().Trim() != "" && textBox554.Text.ToString().Trim() != "" && textBox224.Text.ToString().Trim() != "" && textBox574.Text.ToString().Trim() != "" && 
            textBox184.Text.ToString().Trim() != "" && textBox564.Text.ToString().Trim() != ""   ? true : false);

            //************************************pageframe 2, page 3 validation
            glPage02Page03 = true;// (textBox664.Text != string.Empty && textBox684.Text != string.Empty && textBox674.Text != string.Empty  ? true : false);
            
            //***********************************pageframe 2, page 4 validation
            if (maskedTextBox7.Text != null)
            {
                glPage02Page04 = (textBox294.Text != "" && textBox504.Text != string.Empty && textBox514.Text != string.Empty && maskedTextBox8.Text != "" && maskedTextBox9.Text != "" ? true : false);
            }
            Page2Clear2Go();
        }//end of page 02

        private void Page2Clear2Go()
        {
            if (glPage02Page01 && glPage02Page02 && glPage02Page03 && glPage02Page04)// && glPage02Page05)
            {
                corSaveButton.Enabled = true;
                corSaveButton.BackColor = Color.LawnGreen;
                corSaveButton.Select();
            }
            else
            {
                corSaveButton.Enabled = false;
                corSaveButton.BackColor = Color.Gainsboro;
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox9_SelectedValueChanged(object sender, EventArgs e)
        {
        }

        private void comboBox9_Leave(object sender, EventArgs e)
        {
//            MessageBox.Show("selected value changed");
//            int coun = Convert.ToInt32(comboBox9.SelectedValue);
  //          getcity(coun);
    //        page01Ok();
        }

        private void button29_Click(object sender, EventArgs e)
        {
            FindClient fc = new FindClient(cs,ncompid,dloca, 1,"cusreg");
            fc.ShowDialog();                
        }

        private void button30_Click(object sender, EventArgs e)
        {
            FindClient fc = new FindClient(cs, ncompid, dloca, 1, "cusreg");
            fc.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FindClient fc = new FindClient(cs, ncompid, dloca, 1, "cusreg");
            fc.ShowDialog();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            FindClient fc = new FindClient(cs, ncompid, dloca, 1, "cusreg");
            fc.ShowDialog();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            FindClient fc = new FindClient(cs, ncompid, dloca, 1, "cusreg");
            fc.ShowDialog();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            FindClient fc = new FindClient(cs, ncompid, dloca, 1, "cusreg");
            fc.ShowDialog();
        }

        private void button27_Click(object sender, EventArgs e)
        {
            FindClient fc = new FindClient(cs, ncompid, dloca, 1, "cusreg");
            fc.ShowDialog();

        }

        private void button25_Click(object sender, EventArgs e)
        {
            FindClient fc = new FindClient(cs, ncompid, dloca, 1, "cusreg");
            fc.ShowDialog();
        }

        private void button28_Click(object sender, EventArgs e)
        {
            FindClient fc = new FindClient(cs, ncompid, dloca, 1, "cusreg");
            fc.ShowDialog();
        }

        private void button26_Click(object sender, EventArgs e)
        {
            FindClient fc = new FindClient(cs, ncompid, dloca, 1, "cusreg");
            fc.ShowDialog();
        }

        private void tabPage5_Click(object sender, EventArgs e)
        {

        }

 
        private void textBox9_Leave(object sender, EventArgs e)
        {
            page01Ok();
        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            page01Ok();
        }

        private void comboBox3_SelectedValueChanged(object sender, EventArgs e)
        {
            page01Ok();
        }

        private void TXTIDNUMBER_Leave(object sender, EventArgs e)
        {
            page01Ok();
        }

        private void TXTPLACEISSUED_TextChanged(object sender, EventArgs e)
        {
            page01Ok();
        }

        private void comboBox7_Leave(object sender, EventArgs e)
        {
            page01Ok();
        }

        private void comboBox8_Leave(object sender, EventArgs e)
        {
            page01Ok();
        }


        private void comboBox2_Leave_1(object sender, EventArgs e)
        {
            page01Ok();
        }

        private void comboBox3_Leave_1(object sender, EventArgs e)
        {
            page01Ok();
        }

        private void comboBox10_Leave(object sender, EventArgs e)
        {
            page01Ok();
        }

        private void textBox6_Leave(object sender, EventArgs e)
        {
            page01Ok();
        }

        private void textBox12_Leave(object sender, EventArgs e)
        {
            page01Ok();
        }

        private void comboBox11_Leave(object sender, EventArgs e)
        {
            page01Ok();
        }

        private void textBox13_Leave(object sender, EventArgs e)
        {
            page01Ok();
        }

        private void comboBox12_Leave(object sender, EventArgs e)
        {
//            int rec = emplview.Rows.Count;
  //          MessageBox.Show("The number of rows");

/*            if (emplview.Rows.Count > 0)
            {
                textBox14.Text    = emplview.Rows[0]["cou_name"].ToString();
                textBox21.Text    = emplview.Rows[0]["city_name"].ToString();
                richTextBox8.Text = emplview.Rows[0]["str_no"].ToString();
                textBox15.Text    = emplview.Rows[0]["tel"].ToString();
                textBox17.Text    = emplview.Rows[0]["email"].ToString();
            }
            else { MessageBox.Show("Nothing is in the employer table"); }*/
            page01Ok();
        }

        private void textBox18_Leave(object sender, EventArgs e)
        {
            page01Ok();
        }

        private void comboBox13_Leave(object sender, EventArgs e)
        {
            page01Ok();
        }

        private void comboBox14_Leave(object sender, EventArgs e)
        {
            page01Ok();
        }

        private void textBox20_Leave(object sender, EventArgs e)
        {
            page01Ok();
        }

        private void textBox25_Leave(object sender, EventArgs e)
        {
//            decimal shareprice = Convert.ToDecimal(maskedTextBox2.Text);
  //          int sharenumb = Convert.ToInt32(textBox25.Text);
    //        MessageBox.Show(" figures are " + shareprice + " and " + sharenumb);

            maskedTextBox3.Text = Convert.ToString(Convert.ToDecimal(maskedTextBox2.Text) * Convert.ToInt32(textBox25.Text)); 
            page01Ok();
        }

        private void textBox22_Leave(object sender, EventArgs e)
        {
            page01Ok();
        }

        private void textBox23_Leave(object sender, EventArgs e)
        {
            page01Ok();
        }

        private void IndSaveButton_Click(object sender, EventArgs e)
        {
         //   This is insert !!!
         //   {
                if (gcInputStat == "N")  //new member
                {
                    insertmember();
                Members nmem = new WinTcare.Members();
                nmem.Show();
                // insertnewmember();

            }
                else                   //edit member
                {
                    updateMember();
                    updateglmasts();
                    IndSaveButton.Enabled = false;
                    IndSaveButton.BackColor = Color.Gainsboro;
                    //initvariables();
                    //                MessageBox.Show("Individual Member code " + gcMembCode + " updated Successfully");
                    //       gcMembCode = "";
                    if (checkBox11.Checked)
                    {

                        sms();
                    }
                    else
                    {
                        MessageBox.Show("SMS or the fee is not selected ");
                    }
                  //  scope.Complete();
                }
                initvariables();
            //   }
        }

        private void updateglmasts()
        {

            using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
            {
                //**********update glmast account
                string cpatquery = "update glmast set cacctname = '" + gcMembName + "' where ccustcode = '" + gcMembCode + "'";

                SqlDataAdapter glinsCommand = new SqlDataAdapter();

                glinsCommand.UpdateCommand = new SqlCommand(cpatquery, ndConnHandle3);
                ndConnHandle3.Open();
                glinsCommand.UpdateCommand.ExecuteNonQuery();
                ndConnHandle3.Close();
            }
        }

        // SMS
        private void sms()
        {
            //  MessageBox.Show("You are inside Ok 1");
           // decimal tnPNewBal = CheckLastBalance.lastbalance(cs, tcAcctNumb);      //  0.00m;
                                                                                   // string tcDesc = "Loan Disbursement";
          //  decimal balance = tnPNewBal;
            // double balance = Convert.ToDouble(textBox101.Text) + Convert.ToDouble(maskedTextBox1.Text);
            // MessageBox.Show("This is the Balabce 1" + balance);
            string messages = "Welcome to Sofora Cooperative Credit Union! Your registration has been completed";
            string PhoneNumber = textBox6.Text.Trim();
            var client = new RestClient("https://alchemytelco.com:9443/api?action=sendmessage&originator=NACCUG&username=naccug&password=n@ccu9&recipient=PhoneNumber&messagetype=SMS:TEXT&messagedata=messages");
            client.Timeout = -1;
            var requestPost = new RestRequest(Method.POST);
            requestPost.AddParameter("messagedata", messages);
            requestPost.AddParameter("recipient", PhoneNumber);
            IRestResponse response = client.Execute(requestPost);
            string rawResponse = response.Content.Trim();
            int nStatusCode = (int)response.StatusCode;
            //  MessageBox.Show("This is STatus Code 2 ALA " + nStatusCode);
            string lcStatusCode = response.StatusDescription.ToString().Trim();
            //  MessageBox.Show("This is STatus Code 3 " + nStatusCode);
            //   string lcStatusDesc = errorMessages(nStatusCode.ToString().Trim().PadLeft(2, '0'));
            if (response.StatusCode == HttpStatusCode.OK)
            {
                //  MessageBox.Show("This is STatus Code 4 " + nStatusCode);
              //  postFee();
            }

        }
        private void insertmember()
        {
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                string lcMemCode = GetClient_Code.clientCode_int(cs, "clientid").ToString().Trim().PadLeft(6, '0');
                gcMembCode = lcMemCode;              // textBox46.Text.Trim().PadLeft(6, '0');
                                                     // MessageBox.Show("This code is" + gcMembCode);
                textBox46.Text = (Convert.ToInt32(gcMembCode) + 1).ToString().PadLeft(6, '0');                               //                textBox46.Text = lcMemCode;
                string lcFname = textBox1.Text;
                string lcMname = textBox2.Text;
                string lcLname = textBox3.Text;
                gcMembName = textBox1.Text.Trim() + ' ' + textBox3.Text.Trim();
                bool lnempl = (checkBox2.Checked ? true : false);
                string lcEmployee = textBox18.Text;

                //pageframe 1, page 1 
                int lnTitle = Convert.ToInt32(comboBox2.SelectedValue);
                int lnNatCode = Convert.ToInt32(comboBox3.SelectedValue);
                DateTime ldDOB = Convert.ToDateTime(dob.Value);
                DateTime ldJoin = Convert.ToDateTime(doj.Value);
                bool llMale = (Convert.ToBoolean(dmale.Checked) ? true : false);
                int lnMarital = Convert.ToInt32(comboBox4.SelectedValue);
                int lnIDtype = Convert.ToInt32(comboBox5.SelectedValue);
                string lcIDNumb = textBox4.Text;
                string lcPlaceIssued = textBox5.Text;
                DateTime ldDateIssue = Convert.ToDateTime(dateIssued.Value);
                DateTime ldDateExpire = Convert.ToDateTime(expDate.Value);
                int lnLevelOfedu = Convert.ToInt32(comboBox20.SelectedValue);
                int lnPovertyLevel = Convert.ToInt32(comboBox22.SelectedValue);
                int lnTribe = Convert.ToInt32(comboBox21.SelectedValue);
                int lnRegion = Convert.ToInt32(comboBox6.SelectedValue);
                int lnDistrict = Convert.ToInt32(comboBox7.SelectedValue);
                int lnWard = Convert.ToInt32(comboBox8.SelectedValue);
                bool llRes = (Convert.ToBoolean(radioButton4.Checked) ? true : false);
                char lcMType = (radioButton5.Checked ? 'C' : (radioButton6.Checked ? 'S' : 'B'));

                //pageframe 1, page 2
                int lnCou = Convert.ToInt32(comboBox9.SelectedValue);
                int lnCity = Convert.ToInt32(comboBox10.SelectedValue);
                string lcStreet = contaddr.Text.ToString();
                string lcTel = textBox6.Text;
                string lcTel1 = textBox7.Text;
                string lcEmail = textBox8.Text;

                string lcName1 = textBox9.Text;
                string lcAddr1 = refAddr.Text;
                string lcMobile1 = textBox10.Text;
                string lcEmail1 = textBox11.Text;

                string lcNokName = textBox12.Text;
                string lcNokAddr = nokAddr.Text;
                int lnNokRel = Convert.ToInt32(comboBox11.SelectedValue);
                string lcNokTel = textBox13.Text;

                //pageframe 1, page 3
                int lnEmployer = Convert.ToInt32(comboBox12.SelectedValue);
                string lcStaffNo = textBox18.Text;
                int lnDesig = Convert.ToInt32(comboBox13.SelectedValue);
                int lnDept = Convert.ToInt32(comboBox14.SelectedValue);
                int lnYears = maskedTextBox4.Text != "" ? Convert.ToInt32(maskedTextBox4.Text) : 0;
                decimal lnSal = maskedTextBox6.Text != "" ? Convert.ToDecimal(maskedTextBox6.Text) : 0.00m;

                //pageframe 1, page 4
                int lnShares = textBox25.Text.ToString().Trim() != "" ? Convert.ToInt16(textBox25.Text) : 0;
                decimal lnSaveAmt = maskedTextBox5.Text != "" ? Convert.ToDecimal(maskedTextBox5.Text) : 0.00m;
                bool llSaveType = (radioButton8.Checked ? true : false);
                string lcSignatory = textBox23.Text;
                bool llmodepay = checkBox1.Checked;
                int lbatid = Convert.ToInt32(comboBox15.SelectedValue);
                bool llMobileWallet = checkBox10.Checked ? true : false;
                string cquery = "insert into cusreg (ccustcode,ccustfname,ccustmname,ccustlname,employed,ccusttitle,natcode,ddatebirth,datejoin,gender,marital,idtype,cpassno,cplacissue,ddateissue,ddateexpire,nregion,ndist,nward,residents, cust_type,cou_id,";
                cquery += "ncity,cstreet,ctel,ctel1,cemail,cname1,caddr1,cMobile1,cEmail1,cName2,cAddr2,nRel,cMobile2,nEmployer,payroll_id,nDesig,nDept,nYears,nSal,nRegFee,nSharePrice,nShares,";
                cquery += "nSaveAmt,nSaveType,cSignatory,modepay,compid,branch_id,bat_id,memPict,memsign,levelofedu,povertylevel,tribe,mobileWallet)";
                cquery += "values ";
                cquery += "(@MemCode,@lcFname,@lcMname,@lcLname,@lnEmpl,@lnTitle,@lnNatCode,@ldDOB,@ldJoin,@lnmale,@marital,@idtype,@lcIdNumb,@lcPlace,@dDateIssue,@dDateExpire,@lnReg,@lnDis,@nWard,@lnRes,@lcMType,@lnCou,";
                cquery += "@lnCity,@cStreet,@cTel,@cTel1,@cEmail,@cName1,@cAddr1,@cMobile1,@cEmail1,@lcName2,@lcAddr2,@lnRel,@lcMobile2,@nEmployer,@cStaffNo,@nDesig,@nDept,@nYears,@nSal,@lnRegFee,@lnSharePrice,@lnShares,";
                cquery += "@lnSaveAmt,@lnSaveType,@lcSignatory,@lmodepay,@lcommpid,@branid,@batid,@tbmemPict,@tbmemsign,@levelofedu,@povertylevel,@tribe,@tlWalleto)";

          
                SqlDataAdapter cuscommand = new SqlDataAdapter();
                cuscommand.InsertCommand = new SqlCommand(cquery, ndConnHandle1);

                //page 1 top details
                cuscommand.InsertCommand.Parameters.Add("@MemCode", SqlDbType.VarChar).Value = gcMembCode;
                cuscommand.InsertCommand.Parameters.Add("@lcFname", SqlDbType.VarChar).Value = lcFname;
                cuscommand.InsertCommand.Parameters.Add("@lcMname", SqlDbType.VarChar).Value = lcMname;
                cuscommand.InsertCommand.Parameters.Add("@lcLname", SqlDbType.VarChar).Value = lcLname;
                cuscommand.InsertCommand.Parameters.Add("@lnEmpl", SqlDbType.Bit).Value = lnempl;


                //pageframe1, page 1 details
                cuscommand.InsertCommand.Parameters.Add("@lnTitle", SqlDbType.Int).Value = lnTitle;
                cuscommand.InsertCommand.Parameters.Add("@lnNatCode", SqlDbType.Int).Value = lnNatCode;
                cuscommand.InsertCommand.Parameters.Add("@ldDOB", SqlDbType.DateTime).Value = ldDOB;
                cuscommand.InsertCommand.Parameters.Add("@ldJoin", SqlDbType.DateTime).Value = ldJoin;
                cuscommand.InsertCommand.Parameters.Add("@lnmale", SqlDbType.Bit).Value = llMale;
                cuscommand.InsertCommand.Parameters.Add("@marital", SqlDbType.Int).Value = lnMarital;
                cuscommand.InsertCommand.Parameters.Add("@idtype", SqlDbType.Int).Value = lnIDtype;
                cuscommand.InsertCommand.Parameters.Add("@lcIdNumb", SqlDbType.VarChar).Value = lcIDNumb;
                cuscommand.InsertCommand.Parameters.Add("@lcPlace", SqlDbType.VarChar).Value = lcPlaceIssued;
                cuscommand.InsertCommand.Parameters.Add("@dDateIssue", SqlDbType.DateTime).Value = ldDateIssue;
                cuscommand.InsertCommand.Parameters.Add("@dDateExpire", SqlDbType.DateTime).Value = ldDateExpire;
                cuscommand.InsertCommand.Parameters.Add("@lnReg", SqlDbType.Int).Value = lnRegion;
                cuscommand.InsertCommand.Parameters.Add("@lnDis", SqlDbType.Int).Value = lnDistrict;
                cuscommand.InsertCommand.Parameters.Add("@nWard", SqlDbType.Int).Value = lnWard;
                cuscommand.InsertCommand.Parameters.Add("@lnRes", SqlDbType.Bit).Value = llRes;
                cuscommand.InsertCommand.Parameters.Add("@lcMType", SqlDbType.VarChar).Value = lcMType;
                cuscommand.InsertCommand.Parameters.Add("@tlWalleto", SqlDbType.Bit).Value = llMobileWallet;

                //pageframe1, page 2 details
                cuscommand.InsertCommand.Parameters.Add("@lnCou", SqlDbType.Int).Value = lnCou;
                cuscommand.InsertCommand.Parameters.Add("@lnCity", SqlDbType.Int).Value = lnCity;
                cuscommand.InsertCommand.Parameters.Add("@cStreet", SqlDbType.VarChar).Value = lcStreet;
                cuscommand.InsertCommand.Parameters.Add("@cTel", SqlDbType.VarChar).Value = lcTel;
                cuscommand.InsertCommand.Parameters.Add("@cTel1", SqlDbType.VarChar).Value = lcTel1;
                cuscommand.InsertCommand.Parameters.Add("@cEmail", SqlDbType.VarChar).Value = lcEmail;
                cuscommand.InsertCommand.Parameters.Add("@cName1", SqlDbType.VarChar).Value = lcName1;
                cuscommand.InsertCommand.Parameters.Add("@cAddr1", SqlDbType.VarChar).Value = lcAddr1;
                cuscommand.InsertCommand.Parameters.Add("@cMobile1", SqlDbType.VarChar).Value = lcMobile1;
                cuscommand.InsertCommand.Parameters.Add("@cEmail1", SqlDbType.VarChar).Value = lcEmail1;
                cuscommand.InsertCommand.Parameters.Add("@lcName2", SqlDbType.VarChar).Value = lcNokName;
                cuscommand.InsertCommand.Parameters.Add("@lcAddr2", SqlDbType.VarChar).Value = lcNokAddr;
                cuscommand.InsertCommand.Parameters.Add("@lnRel", SqlDbType.Int).Value = lnNokRel;
                cuscommand.InsertCommand.Parameters.Add("@lcMobile2", SqlDbType.VarChar).Value = lcNokTel;


                //pageframe1, page 3 details
                cuscommand.InsertCommand.Parameters.Add("@nEmployer", SqlDbType.Int).Value = lnEmployer;
                cuscommand.InsertCommand.Parameters.Add("@cStaffNo", SqlDbType.VarChar).Value = lcStaffNo;
                cuscommand.InsertCommand.Parameters.Add("@nDesig", SqlDbType.Int).Value = lnDesig;
                cuscommand.InsertCommand.Parameters.Add("@nDept", SqlDbType.Int).Value = lnDept;
                cuscommand.InsertCommand.Parameters.Add("@nYears", SqlDbType.Int).Value = lnYears;
                cuscommand.InsertCommand.Parameters.Add("@nSal", SqlDbType.Decimal).Value = lnSal;


                //pageframe 1, page 4 details
                cuscommand.InsertCommand.Parameters.Add("@lnRegFee", SqlDbType.Decimal).Value = gnRegistrationFee;
                cuscommand.InsertCommand.Parameters.Add("@lnSharePrice", SqlDbType.Decimal).Value = gnSharePrice; 
                cuscommand.InsertCommand.Parameters.Add("@lnShares", SqlDbType.Int).Value = lnShares;
                cuscommand.InsertCommand.Parameters.Add("@lnSaveAmt", SqlDbType.Decimal).Value = lnSaveAmt;
                cuscommand.InsertCommand.Parameters.Add("@lnSaveType", SqlDbType.Bit).Value = llSaveType;
                cuscommand.InsertCommand.Parameters.Add("@lcSignatory", SqlDbType.VarChar).Value = lcSignatory;
                cuscommand.InsertCommand.Parameters.Add("@lmodepay", SqlDbType.Bit).Value = llmodepay;
                cuscommand.InsertCommand.Parameters.Add("@lcommpid", SqlDbType.Int).Value = globalvar.gnCompid;
                cuscommand.InsertCommand.Parameters.Add("@branid", SqlDbType.Int).Value = Convert.ToInt32(comboBox1.SelectedValue);
                cuscommand.InsertCommand.Parameters.Add("@batid", SqlDbType.Int).Value = Convert.ToInt32(comboBox15.SelectedValue);

                //pageframe 1, page biometric details
                cuscommand.InsertCommand.Parameters.Add("@tbmemPict", SqlDbType.Image).Value = ConvertImageToBinary(pictureBox1.Image);
                cuscommand.InsertCommand.Parameters.Add("@tbmemsign", SqlDbType.Image).Value = ConvertImageToBinary(pictureBox4.Image);

                cuscommand.InsertCommand.Parameters.Add("@levelofedu", SqlDbType.Int).Value = lnLevelOfedu;
                cuscommand.InsertCommand.Parameters.Add("@povertylevel", SqlDbType.Int).Value = lnPovertyLevel;
                cuscommand.InsertCommand.Parameters.Add("@tribe", SqlDbType.Int).Value = lnTribe;

                ndConnHandle1.Open();
               int nn= cuscommand.InsertCommand.ExecuteNonQuery();  //Insert new record
                if(nn> 0) //query is successful
                {
                    if (mandview.Rows.Count > 0)
                    {
                        for (int j = 0; j < mandview.Rows.Count; j++)
                        {
                            string lcMemCode1 = GetClient_Code.clientCode_int(cs, "clientid").ToString().Trim().PadLeft(6, '0');
                            string lcProductControl = mandview.Rows[j]["prod_control"].ToString().Trim();
                            string lcAcode = lcProductControl.Substring(0, 3);
                            lnPrd_ID = Convert.ToInt16(mandview.Rows[j]["prd_id"]);
                            string lcAcctNumb = lcAcode + lcMemCode1 + "01";
                            gcNonInterestAcct = mandview.Rows[j]["nint_inc"].ToString().Trim();
                            string auditDesc = "Individual Member Creation " + lcMemCode1; 
                            AuditTrail au = new AuditTrail();
                            au.upd_audit_trail(cs, auditDesc, 0.00m, 0.00m, globalvar.gcUserid, "C", "", lcMemCode1, "", "", globalvar.gcWorkStation, globalvar.gcWinUser);

                            if (lcAcode == "250" || lcAcode == "251")
                            {
                                 createAccount(lcAcctNumb);
                                decimal tnPostAmt = gnRegistrationFee;
                                gcMemberSavingsAcct = lcAcctNumb;
                                gcSavingsControlAcct = lcProductControl;
                            }

                            if (lcAcode == "270" || lcAcode == "271")
                            {
                              //  MessageBox.Show("This is the Third catch");
                                createAccount(lcAcctNumb);
                              //  MessageBox.Show("This is the Fouth catch");
                                decimal tnPostAmt = gnSharePrice;
                                gcMemberSharesAcct = lcAcctNumb;
                                gcSharesControlAcct = lcProductControl;
                            }
                            initvariables();
                            textBox1.Focus();
                            IndSaveButton.Enabled = false;
                            IndSaveButton.BackColor = Color.Gainsboro;
                            lcMemCode1 = "";
                        }
                      //  MessageBox.Show("This is the Fifth catch");
                        updateCustomerCode();
                    }
                    postJoiningFee();
                }

                ndConnHandle1.Close();

//                if (llMobileWallet)                           //update record in mobile database 
//                {
////                    string WalletSource = "SQL5081.site4now.net";
//                    string cos = "Data Source=SQL5081.site4now.net;Initial Catalog=DB_A45989_Walleto;User Id=DB_A45989_Walleto_admin;Password=Kuyateh@k13";
//                    using (SqlConnection ndWalcon = new SqlConnection(cos))
//                    {
       
//                        if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
//                        {
//                                         ndWalcon.Open();
//                            //Mobile Wallet information 
//                            string lcPino = "1234567";
//                            string lcLoginCode = ncompid.ToString().Trim().PadLeft(3, '0') + gcMembCode;
//                            string cquery1 = "insert into cusreg (ccustcode,ccustfname,ccustmname,ccustlname,ccusttitle,natcode,ddatebirth,datejoin,gender,marital,idtype,cpassno,nregion,ndist,nward,residents,cou_id,";
//                            cquery1 += "ncity,cstreet,ctel,cemail,pinnumber,compid,branch_id,logincode )";
//                            cquery1 += "values ";
//                            cquery1 += "(@MemCode,@lcFname,@lcMname,@lcLname,@lnTitle,@lnNatCode,@ldDOB,@ldJoin,@lnmale,@marital,@idtype,@lcIdNumb,@lnReg,@lnDis,@nWard,@lnRes,@lnCou,";
//                            cquery1 += "@lnCity,@cStreet,@cTel,@cEmail,@lcPin,@tnCompid,@tnBranch,@lcLoginCode)";


//                            SqlDataAdapter mobcommand = new SqlDataAdapter();
//                            mobcommand.InsertCommand = new SqlCommand(cquery1, ndWalcon);

//                            mobcommand.InsertCommand.Parameters.Add("@MemCode", SqlDbType.VarChar).Value = gcMembCode;
//                            mobcommand.InsertCommand.Parameters.Add("@lcFname", SqlDbType.VarChar).Value = lcFname;
//                            mobcommand.InsertCommand.Parameters.Add("@lcMname", SqlDbType.VarChar).Value = lcMname;
//                            mobcommand.InsertCommand.Parameters.Add("@lcLname", SqlDbType.VarChar).Value = lcLname;

//                            mobcommand.InsertCommand.Parameters.Add("@lnTitle", SqlDbType.Int).Value = lnTitle;
//                            mobcommand.InsertCommand.Parameters.Add("@lnNatCode", SqlDbType.Int).Value = lnNatCode;
//                            mobcommand.InsertCommand.Parameters.Add("@ldDOB", SqlDbType.DateTime).Value = ldDOB;
//                            mobcommand.InsertCommand.Parameters.Add("@ldJoin", SqlDbType.DateTime).Value = ldJoin;
//                            mobcommand.InsertCommand.Parameters.Add("@lnmale", SqlDbType.Bit).Value = llMale;
//                            mobcommand.InsertCommand.Parameters.Add("@marital", SqlDbType.Int).Value = lnMarital;
//                            mobcommand.InsertCommand.Parameters.Add("@idtype", SqlDbType.Int).Value = lnIDtype;
//                            mobcommand.InsertCommand.Parameters.Add("@lcIdNumb", SqlDbType.VarChar).Value = lcIDNumb;
//                            mobcommand.InsertCommand.Parameters.Add("@lnReg", SqlDbType.Int).Value = lnRegion;
//                            mobcommand.InsertCommand.Parameters.Add("@lnDis", SqlDbType.Int).Value = lnDistrict;

//                            mobcommand.InsertCommand.Parameters.Add("@nWard", SqlDbType.Int).Value = lnWard;
//                            mobcommand.InsertCommand.Parameters.Add("@lnRes", SqlDbType.Bit).Value = llRes;
//                            mobcommand.InsertCommand.Parameters.Add("@lnCou", SqlDbType.Int).Value = lnCou;
//                            mobcommand.InsertCommand.Parameters.Add("@lnCity", SqlDbType.Int).Value = lnCity;
//                            mobcommand.InsertCommand.Parameters.Add("@cStreet", SqlDbType.VarChar).Value = lcStreet;
//                            mobcommand.InsertCommand.Parameters.Add("@cTel", SqlDbType.VarChar).Value = lcTel;
//                            mobcommand.InsertCommand.Parameters.Add("@cEmail", SqlDbType.VarChar).Value = lcEmail;
//                            mobcommand.InsertCommand.Parameters.Add("@lcPin", SqlDbType.VarChar).Value = lcPino; ;
//                            mobcommand.InsertCommand.Parameters.Add("@tnCompid", SqlDbType.Int).Value = globalvar.gnCompid;
//                            mobcommand.InsertCommand.Parameters.Add("@tnBranch", SqlDbType.Int).Value = Convert.ToInt32(comboBox1.SelectedValue);
//                            mobcommand.InsertCommand.Parameters.Add("@lcLoginCode", SqlDbType.VarChar).Value = lcLoginCode;

//                            try
//                            {
//                                mobcommand.InsertCommand.ExecuteNonQuery();  //Insert new record
//                            }
//                            catch (Exception ex)
//                            {
//                                MessageBox.Show(ex.Message);
//                            }
//                            ndWalcon.Close();
//                        }
//                        else
//                        {
//                            MessageBox.Show("The network is not available for now, Please use Mobile Wallet interface to add member");
//                            //Do stuffs when network not available
//                        }
//                    }
//                }

              
            }
        }

        private void updateMember()
        {
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {

                string lcMemCode = textBox46.Text.ToString().Trim(); //GetClient_Code.clientCode_str(cs, "ccustcode");// textBox46.Text;
                gcMembCode = lcMemCode;
                                                //                textBox46.Text = lcMemCode;
                string lcFname = textBox1.Text;
                string lcMname = textBox2.Text;
                string lcLname = textBox3.Text;
                gcMembName = textBox1.Text.Trim() + ' ' + textBox3.Text.Trim();
                bool lnempl = (checkBox2.Checked ? true : false);
                string lcEmployee = textBox18.Text;

                //pageframe 1, page 1 
                int lnTitle = Convert.ToInt32(comboBox2.SelectedValue);
                int lnNatCode = Convert.ToInt32(comboBox3.SelectedValue);
                DateTime ldDOB = Convert.ToDateTime(dob.Value);
                DateTime ldJoin = Convert.ToDateTime(doj.Value);
                bool llMale = (Convert.ToBoolean(dmale.Checked) ? true : false);
                int lnMarital = Convert.ToInt32(comboBox4.SelectedValue);
                int lnIDtype = Convert.ToInt32(comboBox5.SelectedValue);
                string lcIDNumb = textBox4.Text;
                string lcPlaceIssued = textBox5.Text;
                DateTime ldDateIssue = Convert.ToDateTime(dateIssued.Value);
                DateTime ldDateExpire = Convert.ToDateTime(expDate.Value);
                int lnLevelOfedu = Convert.ToInt32(comboBox20.SelectedValue);
                int lnPovertyLevel = Convert.ToInt32(comboBox22.SelectedValue);
                int lnTribe = Convert.ToInt32(comboBox21.SelectedValue);
                int lnRegion = Convert.ToInt32(comboBox6.SelectedValue);
                int lnDistrict = Convert.ToInt32(comboBox7.SelectedValue);
                int lnWard = Convert.ToInt32(comboBox8.SelectedValue);
                bool llRes = (Convert.ToBoolean(radioButton4.Checked) ? true : false);
                char lcMType = (radioButton5.Checked ? 'C' : (radioButton6.Checked ? 'S' : 'B'));

                //pageframe 1, page 2
                int lnCou = Convert.ToInt32(comboBox9.SelectedValue);
                int lnCity = Convert.ToInt32(comboBox10.SelectedValue);
                string lcStreet = contaddr.Text.ToString();
                string lcTel = textBox6.Text;
                string lcTel1 = textBox7.Text;
                string lcEmail = textBox8.Text;

                string lcName1 = textBox9.Text;
                string lcAddr1 = refAddr.Text;
                string lcMobile1 = textBox10.Text;
                string lcEmail1 = textBox11.Text;

                string lcNokName = textBox12.Text;
                string lcNokAddr = nokAddr.Text;
                int lnNokRel = Convert.ToInt32(comboBox11.SelectedValue);
                string lcNokTel = textBox13.Text;

                //pageframe 1, page 3
                int lnEmployer = Convert.ToInt32(comboBox12.SelectedValue);
                string lcStaffNo = textBox18.Text;
                int lnDesig = Convert.ToInt32(comboBox13.SelectedValue);
                int lnDept = Convert.ToInt32(comboBox14.SelectedValue);
                int lnYears = Convert.ToInt32(maskedTextBox4.Text)>0 ? Convert.ToInt32(maskedTextBox4.Text) : 0;
                decimal lcSal =Convert.ToDecimal(maskedTextBox6.Text.Trim());
                decimal lnSal = lcSal>0.00m ? Convert.ToDecimal(maskedTextBox6.Text) : 0.00m;
                //pageframe 1, page 4
                decimal lnRegFee = Convert.ToDecimal(maskedTextBox1.Text);
                decimal lnSharesp = Convert.ToDecimal(maskedTextBox3.Text);
                int lnShares;
                bool success = Int32.TryParse(textBox25.Text, out lnShares);
                decimal lnSaveAmt = maskedTextBox5.Text != "" ? Convert.ToDecimal(maskedTextBox5.Text) : 0.00m;

                bool llSaveType = (radioButton8.Checked ? true : false);
                string lcSignatory = textBox23.Text;
                bool llmodepay = checkBox1.Checked;
                int lbatid = Convert.ToInt32(comboBox15.SelectedValue);

                string cquery = "update cusreg set ccustfname=@tlcFname, ccustmname=@tlcMname, ccustlname=@tlcLname,employed=@tlnEmpl,ccusttitle=@tlnTitle,natcode=@tlnNatCode,ddatebirth=@tldDOB,datejoin=@tldJoin,gender=@tlnmale,marital=@tmarital,idtype=@tidtype,";
                cquery += "cpassno =@tlcIdNumb,cplacissue=@tlcPlace,ddateissue=@tdDateIssue,ddateexpire=@tdDateExpire,nregion=@tlnReg,ndist=@tlnDis,nward=@tnWard,residents=@tlnRes, cust_type=@tlcMType,cou_id=@tlnCou,";
                cquery += "ncity=@tlnCity,cstreet=@tcStreet,ctel=@tcTel,ctel1=@tcTel1,cemail=@tcEmail,cname1=@tcName1,caddr1=@tcAddr1,cMobile1=@tcMobile1,cEmail1=@tcEmail1,cName2=@tlcName2,cAddr2=@tlcAddr2,nRel=@tlnRel,cMobile2=@tlcMobile2,";
                cquery += "nEmployer =@tnEmployer,cstaffno=@tcStaffNo,payroll_id=@tcStaffNo,nDesig=@tnDesig,nDept=@tnDept,nYears=@tnYears,nSal=@tnSal,nRegFee=@tlnRegFee,nSharePrice=@tlnSharePrice,nShares=@tlnShares,";
                cquery += "nSaveAmt=@tlnSaveAmt,nSaveType=@tlnSaveType,cSignatory=@tlcSignatory,modepay=@tlmodepay,branch_id=@tbranid,bat_id=@tbatid,memPict=@tbmemPict,memsign=@tbmemsign,levelofedu = @levelofedu,tribe = @tribe,povertylevel = @povertylevel  where ccustcode = @MemCode";

                //string cquery = "update cusreg set memPict=@tbmemPict where ccustcode = @MemCode";

                SqlDataAdapter cusAmdcommand = new SqlDataAdapter();
                cusAmdcommand.UpdateCommand = new SqlCommand(cquery, ndConnHandle1);

                ////page 1 top details
                cusAmdcommand.UpdateCommand.Parameters.Add("@MemCode", SqlDbType.VarChar).Value = gcMembCode;
                cusAmdcommand.UpdateCommand.Parameters.Add("@tlcFname", SqlDbType.VarChar).Value = lcFname;
                cusAmdcommand.UpdateCommand.Parameters.Add("@tlcMname", SqlDbType.VarChar).Value = lcMname;
                cusAmdcommand.UpdateCommand.Parameters.Add("@tlcLname", SqlDbType.VarChar).Value = lcLname;
                cusAmdcommand.UpdateCommand.Parameters.Add("@tlnEmpl", SqlDbType.Bit).Value = lnempl;


                //pageframe1, page 1 details
                cusAmdcommand.UpdateCommand.Parameters.Add("@tlnTitle", SqlDbType.Int).Value = lnTitle;
                cusAmdcommand.UpdateCommand.Parameters.Add("@tlnNatCode", SqlDbType.Int).Value = lnNatCode;
                cusAmdcommand.UpdateCommand.Parameters.Add("@tldDOB", SqlDbType.DateTime).Value = ldDOB;
                cusAmdcommand.UpdateCommand.Parameters.Add("@tldJoin", SqlDbType.DateTime).Value = ldJoin;
                cusAmdcommand.UpdateCommand.Parameters.Add("@tlnmale", SqlDbType.Bit).Value = llMale;
                cusAmdcommand.UpdateCommand.Parameters.Add("@tmarital", SqlDbType.Int).Value = lnMarital;
                cusAmdcommand.UpdateCommand.Parameters.Add("@tidtype", SqlDbType.Int).Value = lnIDtype;
                cusAmdcommand.UpdateCommand.Parameters.Add("@tlcIdNumb", SqlDbType.VarChar).Value = lcIDNumb;
                cusAmdcommand.UpdateCommand.Parameters.Add("@tlcPlace", SqlDbType.VarChar).Value = lcPlaceIssued;
                cusAmdcommand.UpdateCommand.Parameters.Add("@tdDateIssue", SqlDbType.DateTime).Value = ldDateIssue;
                cusAmdcommand.UpdateCommand.Parameters.Add("@tdDateExpire", SqlDbType.DateTime).Value = ldDateExpire;
                cusAmdcommand.UpdateCommand.Parameters.Add("@tlnReg", SqlDbType.Int).Value = lnRegion;
                cusAmdcommand.UpdateCommand.Parameters.Add("@tlnDis", SqlDbType.Int).Value = lnDistrict;
                cusAmdcommand.UpdateCommand.Parameters.Add("@tnWard", SqlDbType.Int).Value = lnWard;
                cusAmdcommand.UpdateCommand.Parameters.Add("@tlnRes", SqlDbType.Bit).Value = llRes;
                cusAmdcommand.UpdateCommand.Parameters.Add("@tlcMType", SqlDbType.VarChar).Value = lcMType;

                //pageframe1, page 2 details
                cusAmdcommand.UpdateCommand.Parameters.Add("@tlnCou", SqlDbType.Int).Value = lnCou;
                cusAmdcommand.UpdateCommand.Parameters.Add("@tlnCity", SqlDbType.Int).Value = lnCity;
                cusAmdcommand.UpdateCommand.Parameters.Add("@tcStreet", SqlDbType.VarChar).Value = lcStreet;
                cusAmdcommand.UpdateCommand.Parameters.Add("@tcTel", SqlDbType.VarChar).Value = lcTel;
                cusAmdcommand.UpdateCommand.Parameters.Add("@tcTel1", SqlDbType.VarChar).Value = lcTel1;
                cusAmdcommand.UpdateCommand.Parameters.Add("@tcEmail", SqlDbType.VarChar).Value = lcEmail;
                cusAmdcommand.UpdateCommand.Parameters.Add("@tcName1", SqlDbType.VarChar).Value = lcName1;
                cusAmdcommand.UpdateCommand.Parameters.Add("@tcAddr1", SqlDbType.VarChar).Value = lcAddr1;
                cusAmdcommand.UpdateCommand.Parameters.Add("@tcMobile1", SqlDbType.VarChar).Value = lcMobile1;
                cusAmdcommand.UpdateCommand.Parameters.Add("@tcEmail1", SqlDbType.VarChar).Value = lcEmail1;
                cusAmdcommand.UpdateCommand.Parameters.Add("@tlcName2", SqlDbType.VarChar).Value = lcNokName;
                cusAmdcommand.UpdateCommand.Parameters.Add("@tlcAddr2", SqlDbType.VarChar).Value = lcNokAddr;
                cusAmdcommand.UpdateCommand.Parameters.Add("@tlnRel", SqlDbType.Int).Value = lnNokRel;
                cusAmdcommand.UpdateCommand.Parameters.Add("@tlcMobile2", SqlDbType.VarChar).Value = lcNokTel;


                //pageframe1, page 3 details
                cusAmdcommand.UpdateCommand.Parameters.Add("@tnEmployer", SqlDbType.Int).Value = lnEmployer;
                cusAmdcommand.UpdateCommand.Parameters.Add("@tcStaffNo", SqlDbType.VarChar).Value = lcStaffNo;
                cusAmdcommand.UpdateCommand.Parameters.Add("@tnDesig", SqlDbType.Int).Value = lnDesig;
                cusAmdcommand.UpdateCommand.Parameters.Add("@tnDept", SqlDbType.Int).Value = lnDept;
                cusAmdcommand.UpdateCommand.Parameters.Add("@tnYears", SqlDbType.Int).Value = lnYears;
                cusAmdcommand.UpdateCommand.Parameters.Add("@tnSal", SqlDbType.Decimal).Value = lnSal;


                //pageframe 1, page 4 details
                cusAmdcommand.UpdateCommand.Parameters.Add("@tlnRegFee", SqlDbType.Decimal).Value = gnRegistrationFee;
                cusAmdcommand.UpdateCommand.Parameters.Add("@tlnSharePrice", SqlDbType.Decimal).Value = lnSharesp;
                cusAmdcommand.UpdateCommand.Parameters.Add("@tlnShares", SqlDbType.Int).Value = lnShares;
                cusAmdcommand.UpdateCommand.Parameters.Add("@tlnSaveAmt", SqlDbType.Decimal).Value = lnSaveAmt;
                cusAmdcommand.UpdateCommand.Parameters.Add("@tlnSaveType", SqlDbType.Bit).Value = llSaveType;
                cusAmdcommand.UpdateCommand.Parameters.Add("@tlcSignatory", SqlDbType.VarChar).Value = lcSignatory;
                cusAmdcommand.UpdateCommand.Parameters.Add("@tlmodepay", SqlDbType.Bit).Value = llmodepay;
                cusAmdcommand.UpdateCommand.Parameters.Add("@tlcommpid", SqlDbType.Int).Value = globalvar.gnCompid;
                cusAmdcommand.UpdateCommand.Parameters.Add("@tbranid", SqlDbType.Int).Value = Convert.ToInt32(comboBox1.SelectedValue);
                cusAmdcommand.UpdateCommand.Parameters.Add("@tbatid", SqlDbType.Int).Value = Convert.ToInt32(comboBox15.SelectedValue);

                //pageframe 1, page biometric details
                cusAmdcommand.UpdateCommand.Parameters.Add("@tbmemPict", SqlDbType.Image).Value = ConvertImageToBinary(pictureBox1.Image);
                cusAmdcommand.UpdateCommand.Parameters.Add("@tbmemsign", SqlDbType.Image).Value = ConvertImageToBinary(pictureBox4.Image);

                cusAmdcommand.UpdateCommand.Parameters.Add("@levelofedu", SqlDbType.Int).Value = lnLevelOfedu;
                cusAmdcommand.UpdateCommand.Parameters.Add("@tribe", SqlDbType.Int).Value = lnTribe;
                cusAmdcommand.UpdateCommand.Parameters.Add("@povertylevel", SqlDbType.Int).Value = lnPovertyLevel;
                //           insCommand.UpdateCommand.Parameters.Add("@tcCustPict", SqlDbType.Image).Value = ConvertImageToBinary(pictureBox1.Image); tbmemsign
                ndConnHandle1.Open();
                cusAmdcommand.UpdateCommand.ExecuteNonQuery();  //Insert new record
                ndConnHandle1.Close();
            
               // textBox1.Focus();
            }
        }

        // Institutional Edit
       

        private void initvariables()
        {
            textBox1.Text = textBox46.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            comboBox4.SelectedIndex = -1;
            comboBox5.SelectedIndex = -1;
            comboBox6.SelectedIndex = -1;
            comboBox7.SelectedIndex = -1;
            comboBox8.SelectedIndex = -1;
            comboBox20.SelectedIndex = -1;
            comboBox21.SelectedIndex = -1;
            comboBox22.SelectedIndex = -1;
            textBox4.Text = "";
            textBox5.Text = "";
            comboBox9.SelectedIndex = -1;
            comboBox10.SelectedIndex = -1;
            comboBox11.SelectedIndex = -1;
            comboBox12.SelectedIndex = -1;
            comboBox13.SelectedIndex = -1;
            comboBox14.SelectedIndex = -1;
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
            textBox11.Text = "";
            textBox12.Text = "";
            textBox13.Text = "";
            textBox14.Text = "";
            textBox15.Text = "";
            textBox16.Text = "";
            textBox17.Text = "";
            textBox18.Text = "";
            textBox21.Text = "";

            textBox23.Text = "";
            textBox25.Text = "";
            contaddr.Text = "";
            refAddr.Text = "";
            nokAddr.Text = "";
            maskedTextBox3.Text = "";
            maskedTextBox4.Text = "";
            maskedTextBox5.Text = "";
            maskedTextBox6.Text = "";
            richTextBox8.Text = "";

            radioButton4.Checked = false;
            radioButton3.Checked = false;
            radioButton5.Checked = false;
            radioButton6.Checked = false;
            radioButton7.Checked = false;
            radioButton8.Checked = false;
            radioButton9.Checked = false;
            pictureBox4.Image = null;
            pictureBox1.Image = null; 
        }

        private void member_payroll(string lcMemCode)
        {
     
        }

        private void updateCustomerCode()       
        {
            //******************Code block deprecated and replace with a line from TclassLibrary

            updateClient_Code upd = new updateClient_Code();
            upd.updClient(cs, "clientid");
            textBox42.Text = GetClient_Code.clientCode_int(cs, "clientid").ToString().Trim().PadLeft(6, '0');
            textBox46.Text = GetClient_Code.clientCode_int(cs, "clientid").ToString().Trim().PadLeft(6, '0');
        }

        private void createAccount(string acctnumb)
        {
           // MessageBox.Show("This is the Eighth catch");
            string cs = globalvar.cos;
            string acode = acctnumb.Substring(0, 3);
            using (SqlConnection nConnHandle2 = new SqlConnection(cs))
            {
                string cglquery = "Insert Into glmast (cacctnumb,cacctname,acode,dopedate,compid,ccustcode,cuserid,prd_id)";
                cglquery += " values (@tcAcctNumb,@lcacctname,@lcAcode,convert(date,getdate()),@ncompid,@lcCustCode,@lcuserid,@lPrdid)";

                SqlDataAdapter insCommand = new SqlDataAdapter();

                insCommand.InsertCommand = new SqlCommand(cglquery, nConnHandle2);
                insCommand.InsertCommand.Parameters.Add("@tcAcctNumb", SqlDbType.VarChar).Value = acctnumb;
                insCommand.InsertCommand.Parameters.Add("@lcAcode", SqlDbType.VarChar).Value = acode;
                insCommand.InsertCommand.Parameters.Add("@ncompid", SqlDbType.Int).Value = ncompid;
                insCommand.InsertCommand.Parameters.Add("@lcCustCode", SqlDbType.VarChar).Value = gcMembCode;
                insCommand.InsertCommand.Parameters.Add("@lcUserid", SqlDbType.VarChar).Value = globalvar.gcUserid;
                insCommand.InsertCommand.Parameters.Add("@lcacctname", SqlDbType.VarChar).Value = gcMembName;
                insCommand.InsertCommand.Parameters.Add("@lPrdid", SqlDbType.Int).Value = lnPrd_ID;
                nConnHandle2.Open();
                insCommand.InsertCommand.ExecuteNonQuery();
                nConnHandle2.Close();
            }
        }

        //                        postAccounts(lcAcctNumb, tnPostAmt,lcAcode,lcProductControl); //registration fee

        private void postAccounts(decimal nTranAmt,string tcAcode)
        {
            string tcContra = globalvar.gcIntSuspense;
            string tcUserid = globalvar.gcUserid;
            int tncompid = globalvar.gnCompid;
            string tcDesc = string.Empty;
            string tcCustcode = textBox46.Text.ToString();
            decimal tnTranAmt = -Math.Abs(nTranAmt);
            decimal tnContAmt = Math.Abs(nTranAmt);
            string tcVoucher = CuVoucher.genCuVoucher(cs, globalvar.gdSysDate);
            decimal unitprice = Math.Abs(nTranAmt);
            string tcChqno = "000001";
            decimal lnWaiveAmt = 0.00m;
            string tcTranCode = "15";
            int lnServID = 0;
            bool llPaid = false;
            int tnqty = 1;
            string tcReceipt = "";
            bool llCashpay = true;
            int visno = 1;
            bool isproduct = false;
            int srvid = globalvar.gnBranchid; 
            bool lFreeBee = false;
            updateGlmast gls = new updateGlmast();
            updateDailyBalance dbal = new updateDailyBalance();
            updateCuTranhist tls1 = new updateCuTranhist();
            AuditTrail au = new AuditTrail();
            updateJournal upj = new updateJournal();
            updateClient_Code updcl = new updateClient_Code();


            //************************************The posting should follow the processes detailed below
            if (tcAcode == "")// ************* for joining fees which will be deducted from the member's savings account and credited to an income account
            {
                tcDesc = "Joining Fee";
                string auditDesc = string.Empty; 
                gls.updGlmast(cs, gcMemberSavingsAcct, tnTranAmt);                              
               // dbal.updDayBal(cs, globalvar.gdSysDate, gcMemberSavingsAcct, tnTranAmt, globalvar.gnBranchid, globalvar.gnCompid);
                decimal tnPNewBal = CheckLastBalance.lastbalance(cs, gcMemberSavingsAcct);      
                tls1.updCuTranhist(cs, gcMemberSavingsAcct, tnTranAmt, tcDesc, tcVoucher, tcChqno, tcUserid, tnPNewBal, tcTranCode, lnServID, llPaid, tcContra, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, visno, isproduct,
                srvid, "", "", lFreeBee, tcCustcode, tncompid, globalvar.gnBranchid, globalvar.gnCurrCode, globalvar.gdSysDate, globalvar.gdSysDate);                          //update tranhist posting account

                //savings product control account and send for verification - debit 
                auditDesc = "Joining Fee -> " + gcSavingsControlAcct;
                au.upd_audit_trail(cs, auditDesc, 0.00m, tnTranAmt, globalvar.gcUserid, "U", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);
                upj.updJournal(cs, gcSavingsControlAcct, tcDesc, tnTranAmt, tcVoucher, tcTranCode, globalvar.gdSysDate, globalvar.gcUserid, globalvar.gnCompid);
                updcl.updClient(cs, "nvoucherno");

                // joining fee defined in company details  - credit
//                gcNonInterestAcct = textBox22.Text;
                MessageBox.Show("The joining fee account = " + gcJoinFeeAcct);
                auditDesc = "Joining Fee -> " + gcJoinFeeAcct;
                au.upd_audit_trail(cs, auditDesc, 0.00m, Math.Abs(tnTranAmt), globalvar.gcUserid, "U", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);
                upj.updJournal(cs, gcJoinFeeAcct, tcDesc, Math.Abs(tnTranAmt), tcVoucher, tcTranCode, globalvar.gdSysDate, globalvar.gcUserid, globalvar.gnCompid);
                updcl.updClient(cs, "nvoucherno");

                if (gcSavingsControlAcct != null && gcSavingsControlAcct != string.Empty)
                {
                    updateTemporaryCashBalances(globalvar.gcCashAccont, Math.Abs(tnTranAmt));
                    cashAccount(globalvar.gcCashAccont);
                }
            }
            else
            {
                //**********for share values

                tcDesc = "Shares Purchase";
                // debit member savings account
                string auditDesc = string.Empty;
                gls.updGlmast(cs, gcMemberSavingsAcct, tnTranAmt);
             //   dbal.updDayBal(cs, globalvar.gdSysDate, gcMemberSavingsAcct, tnTranAmt, globalvar.gnBranchid, globalvar.gnCompid);
                decimal tnPNewBal = CheckLastBalance.lastbalance(cs, gcMemberSavingsAcct);
                tls1.updCuTranhist(cs, gcMemberSavingsAcct, tnTranAmt, tcDesc, tcVoucher, tcChqno, tcUserid, tnPNewBal, tcTranCode, lnServID, llPaid, gcMemberSharesAcct, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, visno, isproduct,
                srvid, "", "", lFreeBee, tcCustcode, tncompid, globalvar.gnBranchid, globalvar.gnCurrCode, globalvar.gdSysDate, globalvar.gdSysDate);                          //update tranhist posting account
                
                // debit savings product control account and send for verification
                auditDesc = "Shares values -> " + gcSavingsControlAcct;
                au.upd_audit_trail(cs, auditDesc, 0.00m, tnTranAmt, globalvar.gcUserid, "U", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);
                upj.updJournal(cs, gcSavingsControlAcct, tcDesc, tnTranAmt, tcVoucher, tcTranCode, globalvar.gdSysDate, globalvar.gcUserid, globalvar.gnCompid);
                updcl.updClient(cs, "nvoucherno");

                // credit member shares account
                gls.updGlmast(cs, gcMemberSharesAcct, tnContAmt);
             //   dbal.updDayBal(cs, globalvar.gdSysDate, gcMemberSharesAcct, Math.Abs(tnTranAmt), globalvar.gnBranchid, globalvar.gnCompid);
                tnPNewBal = CheckLastBalance.lastbalance(cs, gcMemberSharesAcct);
                tls1.updCuTranhist(cs, gcMemberSharesAcct, Math.Abs(tnTranAmt), tcDesc, tcVoucher, tcChqno, tcUserid, tnPNewBal, tcTranCode, lnServID, llPaid, gcMemberSavingsAcct, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, visno, isproduct,
                srvid, "", "", lFreeBee, tcCustcode, tncompid, globalvar.gnBranchid, globalvar.gnCurrCode, globalvar.gdSysDate, globalvar.gdSysDate);                          //update tranhist posting account
                
                // credit shares product control account and send for verification
                auditDesc = "Shares values -> " + gcSharesControlAcct;
                au.upd_audit_trail(cs, auditDesc, 0.00m, Math.Abs(tnTranAmt), globalvar.gcUserid, "U", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);
                upj.updJournal(cs, gcSharesControlAcct, tcDesc, Math.Abs(tnTranAmt), tcVoucher, tcTranCode, globalvar.gdSysDate, globalvar.gcUserid, globalvar.gnCompid);
                updcl.updClient(cs, "nvoucherno");

                if (gcMemberSharesAcct != null && gcMemberSharesAcct != string.Empty)
                {
                    updateTemporaryCashBalances(globalvar.gcCashAccont, Math.Abs(tnTranAmt));
                    cashAccount(globalvar.gcCashAccont);
                }
            }

          }

        private void cashAccount(string actnumb)
        {
            using (SqlConnection nConnHandle = new SqlConnection(cs))
            {
                string cashap = "select cacctname,nbookbal from Daytrans where cacctnumb =" + "'" + actnumb + "'";
                SqlDataAdapter da21 = new SqlDataAdapter(cashap, nConnHandle);
                nConnHandle.Open();
                DataTable ds1 = new DataTable();
                da21.Fill(ds1);
                if (ds1 != null & ds1.Rows.Count > 0)
                {
                    globalvar.gcCashAccontName = ds1.Rows[0]["cacctname"].ToString();
                    globalvar.gcCashAccontBal = Convert.ToDecimal(ds1.Rows[0]["nbookbal"]);
                    globalvar.gnCashierBalance = Convert.ToDecimal(ds1.Rows[0]["nbookbal"]);
                    globalvar.gcToolbutton = ds1.Rows[0]["nbookbal"].ToString();
                    RefreshNeeded?.Invoke(this, new EventArgs());
                }
            }
        }

        private void updateTemporaryCashBalances(string actnumb, decimal tnTransAmount)
        {
            using (SqlConnection ncon = new SqlConnection(cs))
            {

                string cashap = "update Daytrans set nbookbal = @nAmount where cacctnumb = @cAcct";
                SqlDataAdapter cashUpd = new SqlDataAdapter();
                cashUpd.UpdateCommand = new SqlCommand(cashap, ncon);
                cashUpd.UpdateCommand.Parameters.Add("@nAmount", SqlDbType.Decimal).Value = tnTransAmount + globalvar.gcCashAccontBal;
                cashUpd.UpdateCommand.Parameters.Add("@cAcct", SqlDbType.VarChar).Value = actnumb;
                ncon.Open();
                cashUpd.UpdateCommand.ExecuteNonQuery();
            }
        }
        private void comboBox12_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox12.Focused == true)
            {
                          if (emplview.Rows.Count > 0)
                          {
                              textBox14.Text = emplview.Rows[0]["cou_name"].ToString();
                              textBox21.Text = emplview.Rows[0]["city_name"].ToString();
                              richTextBox8.Text = emplview.Rows[0]["str_no"].ToString();
                              textBox15.Text = emplview.Rows[0]["tel"].ToString();
                              textBox17.Text = emplview.Rows[0]["email"].ToString();
                          }
                page01Ok();
            }
        }

        private void comboBox9_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox9.Focused ==true)
            {
                //            MessageBox.Show("selected value changed");
                int coun = Convert.ToInt32(comboBox9.SelectedValue);
                getcity(coun);
                page01Ok();
            }
        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void maskedTextBox5_Leave(object sender, EventArgs e)
        {
            page01Ok();
        }

        private void maskedTextBox4_Leave(object sender, EventArgs e)
        {
            page01Ok();

        }

        private void maskedTextBox6_Leave(object sender, EventArgs e)
        {
            page01Ok();
        }

        private void maskedTextBox6_Validated(object sender, EventArgs e)
        {
            string cpayment = maskedTextBox6.Text;
            maskedTextBox6.Text = Convert.ToDouble(cpayment).ToString("N2");
            page01Ok();
        }

        private void textBox19_Validated(object sender, EventArgs e)
        {
//            textBox19.Text = textBox19.ToString("N3");
        }

        private void maskedTextBox5_Validated(object sender, EventArgs e)
        {
            string cpayment = maskedTextBox5.Text;
            maskedTextBox5.Text = Convert.ToDouble(cpayment).ToString("N2");
            page01Ok();
        }

        private void comboBox2_SelectedValueChanged_1(object sender, EventArgs e)
        {
            if(comboBox2.Focused )
            {
                //MessageBox.Show("combo 2 selected value");
                page01Ok();
            }
        }

        private void comboBox3_SelectedValueChanged_1(object sender, EventArgs e)
        {
            if(comboBox3.Focused)
            {
                //MessageBox.Show("combo 3 selected value");
                page01Ok();
            }
        }

        private void dmale_Click(object sender, EventArgs e)
        {
            if(dmale.Checked )
            {
                //MessageBox.Show("dmale checked");
                page01Ok();
            }
        }

        private void dfemale_Click(object sender, EventArgs e)
        {
            if(dfemale.Checked )
            {
                //MessageBox.Show("dfemale checked");
                page01Ok();
            }
        }

        private void comboBox4_SelectedValueChanged(object sender, EventArgs e)
        {
            if(comboBox4.Focused)
            {
                //MessageBox.Show("combo 4 selected value");
                page01Ok();
            }
        }

        private void comboBox5_SelectedValueChanged(object sender, EventArgs e)
        {
            if(comboBox5.Focused )
            {
                //MessageBox.Show("combo 5 selected value");
                page01Ok();
            }
        }

        private void textBox4_Validated(object sender, EventArgs e)
        {
            //MessageBox.Show("textbox 4");
            page01Ok();
        }

        private void textBox5_Validated(object sender, EventArgs e)
        {
            //MessageBox.Show("textbox 5");
            page01Ok();
        }

        private void comboBox6_SelectedValueChanged(object sender, EventArgs e)
        {
            if(comboBox6.Focused)
            {
                //MessageBox.Show("combo 6 selected value");
                page01Ok();
            }
        }

        private void comboBox7_SelectedValueChanged(object sender, EventArgs e)
        {
            if(comboBox7.Focused)
            {
                //MessageBox.Show("combo 7 selected value");
                page01Ok();
            }
        }

        private void comboBox8_SelectedValueChanged(object sender, EventArgs e)
        {
            if(comboBox8.Focused )
            {
                //MessageBox.Show("combo 8 selected value");
                page01Ok();
            }
        }

        private void textBox1_Validated(object sender, EventArgs e)
        {
     //       MessageBox.Show("textbox 1 validated");
            page01Ok();
       //     MessageBox.Show("after page10");
        }

        private void textBox3_Validated(object sender, EventArgs e)
        {
            //MessageBox.Show("textbox 3 validated");
            page01Ok();
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if(comboBox1.Focused)
            {
                //MessageBox.Show("combo 1 selected value");
                page01Ok();
            }
        }

        private void comboBox9_SelectedValueChanged_1(object sender, EventArgs e)
        {
            if (comboBox9.Focused == true)
            {
                int coun = Convert.ToInt32(comboBox9.SelectedValue);
                getcity(coun);
                page01Ok();
            }
        }

        private void comboBox10_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox10.Focused == true)
            {
                //            MessageBox.Show("selected value changed");
      //          int coun = Convert.ToInt32(comboBox9.SelectedValue);
        //        getcity(coun);
                page01Ok();
            }
        }

        private void comboBox11_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox11.Focused == true)
            {
                //            MessageBox.Show("selected value changed");
      //          int coun = Convert.ToInt32(comboBox9.SelectedValue);
        //        getcity(coun);
                page01Ok();
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            page01Ok();
        }

        private void textBox12_Validated(object sender, EventArgs e)
        {
            page01Ok();
        }

        private void textBox13_Validated(object sender, EventArgs e)
        {
            page01Ok();
        }

        private void textBox6_Validated(object sender, EventArgs e)
        {
            page01Ok();
        }

        private void textBox18_Validated(object sender, EventArgs e)
        {
            using (SqlConnection nConnHandle = new SqlConnection(cs))
            {
                if (gcInputStat == "N")  //new member
                {

                nConnHandle.Open();
                string dpayid = textBox18.Text.Trim();

                SqlDataReader cUserDetails = null;
                SqlCommand cGetUser = new SqlCommand("select payroll_id from cusreg where payroll_id = '" + dpayid + "'", nConnHandle);
                cUserDetails = cGetUser.ExecuteReader();
                cUserDetails.Read();
                if (cUserDetails.HasRows == true)
                {
                    MessageBox.Show("Payroll ID Exit" + dpayid);
                    textBox18.Text = "";
                }

                }
                //else
                //{
                //    textBox3.Enabled = textBox4.Enabled = false;
                //    textBox2.Text = "";
                //    textBox2.Focus();
                //}
            }
            page01Ok();
        }

        private void comboBox13_Validated(object sender, EventArgs e)
        {

        }

        private void comboBox14_SelectedValueChanged(object sender, EventArgs e)
        {
           
            if (comboBox14.Focused)
            {
                page01Ok();
            }
        }

        private void maskedTextBox4_Validated(object sender, EventArgs e)
        {
            page01Ok();
        }

        private void textBox25_Validated(object sender, EventArgs e)
        {
   
            maskedTextBox3.Text = (Convert.ToDecimal(maskedTextBox2.Text) * Convert.ToDecimal(textBox25.Text)).ToString("N2");
            page01Ok();
        }

        private void comboBox15_SelectedValueChanged(object sender, EventArgs e)
        {
            if(comboBox15.Focused)
            {
                page01Ok();
            }
        }

        private void textBox23_Validated(object sender, EventArgs e)
        {
            page01Ok();
        }

        private void comboBox12_SelectedValueChanged(object sender, EventArgs e)
        {
            if(comboBox12.Focused)
            {
                            int rec = emplview.Rows.Count;
                //          MessageBox.Show("The number of rows");

            if (emplview.Rows.Count > 0)
            {
                textBox14.Text    = emplview.Rows[0]["cou_name"].ToString();
                textBox21.Text    = emplview.Rows[0]["city_name"].ToString();
                richTextBox8.Text = emplview.Rows[0]["str_no"].ToString();
                textBox15.Text    = emplview.Rows[0]["tel"].ToString();
                textBox17.Text    = emplview.Rows[0]["email"].ToString();
            }
                page01Ok();
            }
        }

        private void comboBox13_SelectedValueChanged(object sender, EventArgs e)
        {
            if(comboBox13.Focused)
            {
                page01Ok();
            }
        }

        private void textBox45_Validated(object sender, EventArgs e)
        {
            page02Ok();
        }

        private void comboBox18_SelectedValueChanged(object sender, EventArgs e)
        {
            if(comboBox18.Focused )
            {
                page02Ok();
            }
        }

        private void comboBox16_SelectedValueChanged(object sender, EventArgs e)
        {
            if(comboBox16.Focused)
            {
                page02Ok();
            }
        }

        private void comboBox112_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox112.Focused == true)
            {
                int coun1 = Convert.ToInt32(comboBox112.SelectedValue);
                getcity1(coun1);
                page02Ok();
            }


        }

        private void comboBox123_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox123.Focused)
            {
                page02Ok();
            }
        }

        private void comboBox101_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox101.Focused)
            {
                page02Ok();
            }
        }

        private void comboBox92_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox92.Focused)
            {
                page02Ok();
            }
        }

        private void comboBox81_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox81.Focused)
            {
                page02Ok();
            }
        }

        private void textBox39_Validated(object sender, EventArgs e)
        {
            page02Ok();
        }

        private void textBox120_Validated(object sender, EventArgs e)
        {
            page02Ok();
        }

        private void textBox436_Validated(object sender, EventArgs e)
        {
            page02Ok();
        }

        private void textBox407_Validated(object sender, EventArgs e)
        {
            page02Ok();
        }

        private void textBox534_Validated(object sender, EventArgs e)
        {
            page02Ok();
        }

        private void textBox224_Validated(object sender, EventArgs e)
        {
            page02Ok();
        }

        private void textBox184_Validated(object sender, EventArgs e)
        {
            page02Ok();
        }

        private void textBox554_Validated(object sender, EventArgs e)
        {
            page02Ok();
        }

        private void textBox574_Validated(object sender, EventArgs e)
        {
            page02Ok();
        }

        private void textBox564_Validated(object sender, EventArgs e)
        {
            page02Ok();
        }

        private void textBox324_Validated(object sender, EventArgs e)
        {
            page02Ok();
        }

        private void textBox344_Validated(object sender, EventArgs e)
        {
            page02Ok();
        }

        private void textBox334_Validated(object sender, EventArgs e)
        {
            page02Ok();
        }

        private void textBox364_Validated(object sender, EventArgs e)
        {
            page02Ok();
        }

        private void textBox384_Validated(object sender, EventArgs e)
        {
            page02Ok();
        }

        private void textBox374_Validated(object sender, EventArgs e)
        {
            page02Ok();
        }

        private void textBox664_Validated(object sender, EventArgs e)
        {
            page02Ok();
        }

        private void textBox684_Validated(object sender, EventArgs e)
        {
            page02Ok();
        }

        private void textBox674_Validated(object sender, EventArgs e)
        {
            page02Ok();
        }

        private void textBox294_Validated(object sender, EventArgs e)
        {
            textBox284.Text= (Convert.ToDecimal(maskedTextBox9.Text) * Convert.ToDecimal(textBox294.Text)).ToString("N2");
            page02Ok();
        }

        private void textBox254_Validated(object sender, EventArgs e)
        {
            page02Ok();
        }

        private void textBox504_Validated(object sender, EventArgs e)
        {
            page02Ok();
        }

        private void textBox514_Validated(object sender, EventArgs e)
        {
            page02Ok();
        }

        private void comboBox19_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox19.Focused)
            {
                page02Ok();
            }
        }

        private void maskedTextBox7_Validated(object sender, EventArgs e)
        {
            maskedTextBox7.Text = Convert.ToDecimal(maskedTextBox7.Text).ToString("N2");
            page02Ok();
        }


        private void button6_Click(object sender, EventArgs e)
        {
            LoanApplication lp = new WinTcare.LoanApplication();
            lp.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            LoanApplication lp = new WinTcare.LoanApplication();
            lp.ShowDialog();
        }

        private void corSaveButton_Click_1(object sender, EventArgs e)
        {

        }

        private void corSaveButton_Click(object sender, EventArgs e)
        {
             if (gcInputStat == "N")  //new member
            {
                insertCorGrpMember();

                if (checkBox2.Checked)
                {
                    member_payroll(gcMembCode);
                }
               //if (mandview.Rows.Count > 0)
               // {
               //     for (int j = 0; j < mandview.Rows.Count; j++)
               //     {
               //          string lcMemCode1 = GetClient_Code.clientCode_int(cs, "clientid").ToString().Trim().PadLeft(6, '0');
               //         string lcProductControl = mandview.Rows[j]["prod_control"].ToString().Trim();
               //         string lcAcode = lcProductControl.Substring(0, 3);
               //         lnPrd_ID = Convert.ToInt16(mandview.Rows[j]["prd_id"]);
               //         string lcAcctNumb = lcAcode + lcMemCode1 + "01";
               //         gcNonInterestAcct = mandview.Rows[j]["nint_inc"].ToString().Trim();
               //         string auditDesc = (radioButton18.Checked ? "Corporate Member Creation -> " : "Group Member Creation -> ") + gcMembCode;
               //         AuditTrail au = new AuditTrail();
               //         au.upd_audit_trail(cs, auditDesc, 0.00m, 0.00m, globalvar.gcUserid, "C", "", lcMemCode1, "", "", globalvar.gcWorkStation, globalvar.gcWinUser);


               //         gcNonInterestAcct = mandview.Rows[j]["nint_inc"].ToString().Trim();
               //        // createAccount(lcAcctNumb);


               //         if (lcAcode == "250" || lcAcode == "251")
               //         {
               //             createAccount(lcAcctNumb);
               //             decimal tnPostAmt = gnRegistrationFee;
               //             gcMemberSavingsAcct = lcAcctNumb;
               //             gcSavingsControlAcct = lcProductControl;
               //             //createAccount(lcAcctNumb);
               //             //decimal tnPostAmt = gnRegistrationFee;
               //             //gcMemberSavingsAcct = lcAcctNumb;
               //             //gcSavingsControlAcct = lcProductControl;
               //             //postAccounts(tnPostAmt, lcAcode); //registration fee
               //         }
               //         if (lcAcode == "270" || lcAcode == "271")
               //         {
               //             //  MessageBox.Show("This is the Third catch");
               //             createAccount(lcAcctNumb);
               //             //  MessageBox.Show("This is the Fouth catch");
               //             decimal tnPostAmt = gnSharePrice;
               //             gcMemberSharesAcct = lcAcctNumb;
               //             gcSharesControlAcct = lcProductControl;
               //             //createAccount(lcAcctNumb);
               //             //decimal tnPostAmt = gnSharePrice;
               //             //gcMemberSharesAcct = lcAcctNumb;
               //             //gcSharesControlAcct = lcProductControl;
               //             //postAccounts(tnPostAmt, lcAcode); //registration fee
               //         }
               //     }
               //    // updateCustomerCode();
               //     updateCustomerCode();
               //    initCorVariables();
               // }
               // postJoiningFee();

            }

            ////ndConnHandle1.Open();
            ////int nn = cuscommand.InsertCommand.ExecuteNonQuery();  //Insert new record
            ////if (nn > 0) //query is successful
            ////{
            ////    if (mandview.Rows.Count > 0)
            ////    {
            ////        for (int j = 0; j < mandview.Rows.Count; j++)
            ////        {
            ////            string lcMemCode1 = GetClient_Code.clientCode_int(cs, "clientid").ToString().Trim().PadLeft(6, '0');
            ////            string lcProductControl = mandview.Rows[j]["prod_control"].ToString().Trim();
            ////            string lcAcode = lcProductControl.Substring(0, 3);
            ////            lnPrd_ID = Convert.ToInt16(mandview.Rows[j]["prd_id"]);
            ////            string lcAcctNumb = lcAcode + lcMemCode1 + "01";
            ////            gcNonInterestAcct = mandview.Rows[j]["nint_inc"].ToString().Trim();
            ////            string auditDesc = "Individual Member Creation " + lcMemCode1;
            ////            AuditTrail au = new AuditTrail();
            ////            au.upd_audit_trail(cs, auditDesc, 0.00m, 0.00m, globalvar.gcUserid, "C", "", lcMemCode1, "", "", globalvar.gcWorkStation, globalvar.gcWinUser);

            ////            if (lcAcode == "250" || lcAcode == "251")
            ////            {
            ////                createAccount(lcAcctNumb);
            ////                decimal tnPostAmt = gnRegistrationFee;
            ////                gcMemberSavingsAcct = lcAcctNumb;
            ////                gcSavingsControlAcct = lcProductControl;
            ////            }

            ////            if (lcAcode == "270" || lcAcode == "271")
            ////            {
            ////                //  MessageBox.Show("This is the Third catch");
            ////                createAccount(lcAcctNumb);
            ////                //  MessageBox.Show("This is the Fouth catch");
            ////                decimal tnPostAmt = gnSharePrice;
            ////                gcMemberSharesAcct = lcAcctNumb;
            ////                gcSharesControlAcct = lcProductControl;
            ////            }
            ////            initvariables();
            ////            textBox1.Focus();
            ////            IndSaveButton.Enabled = false;
            ////            IndSaveButton.BackColor = Color.Gainsboro;
            ////            lcMemCode1 = "";
            ////        }
            ////        //  MessageBox.Show("This is the Fifth catch");
            ////        updateCustomerCode();
            ////    }
            ////    postJoiningFee();
            ////}

            ////ndConnHandle1.Close();

            else                   //edit member
            {
                updateMemberInt();
                updateglmasts();
                IndSaveButton.Enabled = false;
                IndSaveButton.BackColor = Color.Gainsboro;
                //initvariables();
                //                MessageBox.Show("Individual Member code " + gcMembCode + " updated Successfully");
                //       gcMembCode = "";
                initvariables();
            }

          
        }

        // we are editing the Institution

        private void updateMemberInt()
        {
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {

                string lcMemCode = textBox42.Text.ToString().Trim(); //GetClient_Code.clientCode_str(cs, "ccustcode");// textBox46.Text;
                gcMembCode = lcMemCode;

                decimal gnregfee = Convert.ToDecimal(maskedTextBox8.Text);
                decimal NoShares = Convert.ToDecimal(textBox294.Text);
                  decimal lnSharesp = Convert.ToDecimal(textBox284.Text);
                //int lnShares;
                //bool success = Int32.TryParse(textBox25.Text, out lnShares);
                //decimal lnSaveAmt = maskedTextBox5.Text != "" ? Convert.ToDecimal(maskedTextBox5.Text) : 0.00m;

                //  bool llSaveType = (radioButton8.Checked ? true : false);
                //  string lcSignatory = textBox23.Text;
                //  bool llmodepay = checkBox1.Checked;
                //  int lbatid = Convert.ToInt32(comboBox15.SelectedValue);
                int lnmem_type = radioButton18.Checked ? 2 : 3;

                string cquery1 = "update cusreg set ccustcode = @MemCode, ccustname =  @lcFname, bizcat = @lbizcat, cou_id =  @lnCou, ncity = @lnCity, cstreet = @cStreet, ctel =  @cTel, ctel1 =  @cTel1, cemail =  @cEmail,";
                    cquery1 += "INCORPC =  @incno, tin = @tin, INCORPD =@incdate, datejoin =  @ldJoin, nregion =  @lnReg, ndist = @lnDis, nward =  @nWard, residents = @lnRes, cust_type =  @lcusType, chairname = @chairname, chairtin = @chairtin, chairtel =  @chairtel,";
                cquery1 += "chairmail = @chairmail, chairsig =  @chairsig, vcname =  @vcname, vctin =  @vctin, vctel =  @vctel, vcmail = @vcmail, vcsign = @vcsign, treaname =  @treaname, treatin =  @treatin, treatel = @treatel, treamail =  @treamail, treasign = @treasign, secname =  @secname, sectin =  @sectin, sectel = @sectel, secmail = @secmail, secsign=@secsign, ref1name = @ref1name, ref1addr = @ref1addr,";
                cquery1 += "ref1tel = @ref1tel, ref1mail = @ref1mail, ref2name =  @ref2name, ref2addr = @ref2addr, ref2tel = @ref2tel, ref2mail =  @ref2mail, ref3name= @ref3name, ref3addr =  @ref3addr, ref3tel =  @ref3tel, ref3mail =  @ref3mail, ref4name = @ref4name, ref4addr =  @ref4addr, ref4tel =  @ref4tel, ref4mail =  @ref4mail, nRegFee =  @lnRegFee, nSharePrice = @lnSharePrice, nShares =  @lnShares,";
                cquery1 += "nSaveAmt = @lnSaveAmt, nSaveType =  @lnSaveType, sign1 = @sign1, sign2 = @sign2, sign3 = @sign3, sign4 = @sign4, compid =  @ncompid, branch_id = @branid, bat_id = @batid,mem_type = @memtype, memPict = @tbmempict, memSign = @tbmemsign  where ccustcode = @MemCode";  
                SqlDataAdapter cuscommand = new SqlDataAdapter();
                cuscommand.UpdateCommand = new SqlCommand(cquery1, ndConnHandle1);

                // top details
                cuscommand.UpdateCommand.Parameters.Add("@MemCode", SqlDbType.VarChar).Value = gcMembCode;
                cuscommand.UpdateCommand.Parameters.Add("@lcFname", SqlDbType.VarChar).Value = textBox45.Text.Trim();
                cuscommand.UpdateCommand.Parameters.Add("@lbizcat", SqlDbType.Int).Value = Convert.ToInt32(comboBox18.SelectedValue);

                //pageframe2, page 1 details
                cuscommand.UpdateCommand.Parameters.Add("@lnCou", SqlDbType.Int).Value = Convert.ToInt32(comboBox112.SelectedValue);
                cuscommand.UpdateCommand.Parameters.Add("@lnCity", SqlDbType.Int).Value = Convert.ToInt32(comboBox123.SelectedValue);
                cuscommand.UpdateCommand.Parameters.Add("@cStreet", SqlDbType.VarChar).Value = lscortreet.Text.Trim();
                cuscommand.UpdateCommand.Parameters.Add("@cTel", SqlDbType.VarChar).Value = textBox39.Text.Trim();
                cuscommand.UpdateCommand.Parameters.Add("@cTel1", SqlDbType.VarChar).Value = textBox533.Text.Trim();
                cuscommand.UpdateCommand.Parameters.Add("@cEmail", SqlDbType.VarChar).Value = textBox120.Text.Trim();
                cuscommand.UpdateCommand.Parameters.Add("@incno", SqlDbType.VarChar).Value = textBox436.Text.Trim();
                cuscommand.UpdateCommand.Parameters.Add("@tin", SqlDbType.VarChar).Value = textBox407.Text.Trim();
                cuscommand.UpdateCommand.Parameters.Add("@incdate", SqlDbType.DateTime).Value = Convert.ToDateTime(incDate.Text);
                cuscommand.UpdateCommand.Parameters.Add("@ldJoin", SqlDbType.DateTime).Value = Convert.ToDateTime(dateJoin.Text);
                cuscommand.UpdateCommand.Parameters.Add("@lnReg", SqlDbType.Int).Value = Convert.ToInt32(comboBox101.SelectedValue);
                cuscommand.UpdateCommand.Parameters.Add("@lnDis", SqlDbType.Int).Value = Convert.ToInt32(comboBox92.SelectedValue);
                cuscommand.UpdateCommand.Parameters.Add("@nWard", SqlDbType.Int).Value = Convert.ToInt32(comboBox81.SelectedValue);
                cuscommand.UpdateCommand.Parameters.Add("@lnRes", SqlDbType.Bit).Value = radioButton12.Checked ? true : false;
                cuscommand.UpdateCommand.Parameters.Add("@lcusType", SqlDbType.Char).Value = (radioButton10.Checked ? 'C' : (radioButton2.Checked ? 'S' : 'B'));

                ///********************************
                //pageframe2, page 2 details
                cuscommand.UpdateCommand.Parameters.Add("@chairname", SqlDbType.VarChar).Value = textBox534.Text.Trim();
                cuscommand.UpdateCommand.Parameters.Add("@chairtin", SqlDbType.VarChar).Value = textBox494.Text.Trim();
                cuscommand.UpdateCommand.Parameters.Add("@chairtel", SqlDbType.VarChar).Value = textBox224.Text.Trim();
                cuscommand.UpdateCommand.Parameters.Add("@chairmail", SqlDbType.VarChar).Value = textBox184.Text.Trim();
                cuscommand.UpdateCommand.Parameters.Add("@chairsig", SqlDbType.Bit).Value = checkBox3.Checked ? true : false;

                cuscommand.UpdateCommand.Parameters.Add("@vcname", SqlDbType.VarChar).Value = textBox324.Text.Trim();
                cuscommand.UpdateCommand.Parameters.Add("@vctin", SqlDbType.VarChar).Value = textBox234.Text.Trim();
                cuscommand.UpdateCommand.Parameters.Add("@vctel", SqlDbType.VarChar).Value = textBox344.Text.Trim();
                cuscommand.UpdateCommand.Parameters.Add("@vcmail", SqlDbType.VarChar).Value = textBox334.Text.Trim();
                cuscommand.UpdateCommand.Parameters.Add("@vcsign", SqlDbType.Bit).Value = checkBox4.Checked ? true : false;

                cuscommand.UpdateCommand.Parameters.Add("@treaname", SqlDbType.VarChar).Value = textBox554.Text.Trim();
                cuscommand.UpdateCommand.Parameters.Add("@treatin", SqlDbType.VarChar).Value = textBox544.Text.Trim();
                cuscommand.UpdateCommand.Parameters.Add("@treatel", SqlDbType.VarChar).Value = textBox574.Text.Trim();
                cuscommand.UpdateCommand.Parameters.Add("@treamail", SqlDbType.VarChar).Value = textBox564.Text.Trim();
                cuscommand.UpdateCommand.Parameters.Add("@treasign", SqlDbType.Bit).Value = checkBox6.Checked ? true : false;

                cuscommand.UpdateCommand.Parameters.Add("@secname", SqlDbType.VarChar).Value = textBox364.Text.Trim();
                cuscommand.UpdateCommand.Parameters.Add("@sectin", SqlDbType.VarChar).Value = textBox354.Text.Trim();
                cuscommand.UpdateCommand.Parameters.Add("@sectel", SqlDbType.VarChar).Value = textBox384.Text.Trim();
                cuscommand.UpdateCommand.Parameters.Add("@secmail", SqlDbType.VarChar).Value = textBox374.Text.Trim();
                cuscommand.UpdateCommand.Parameters.Add("@secsign", SqlDbType.Bit).Value = checkBox5.Checked ? true : false;


                //pageframe2, page 3 details
                cuscommand.UpdateCommand.Parameters.Add("@ref1name", SqlDbType.VarChar).Value = textBox664.Text.Trim();
                cuscommand.UpdateCommand.Parameters.Add("@ref1addr", SqlDbType.VarChar).Value = ref1Addr.Text.Trim();
                cuscommand.UpdateCommand.Parameters.Add("@ref1tel", SqlDbType.VarChar).Value = textBox684.Text.Trim();
                cuscommand.UpdateCommand.Parameters.Add("@ref1mail", SqlDbType.VarChar).Value = textBox674.Text.Trim();

                cuscommand.UpdateCommand.Parameters.Add("@ref2name", SqlDbType.VarChar).Value = textBox114.Text.Trim();
                cuscommand.UpdateCommand.Parameters.Add("@ref2addr", SqlDbType.VarChar).Value = ref2Addr.Text.Trim();
                cuscommand.UpdateCommand.Parameters.Add("@ref2tel", SqlDbType.VarChar).Value = textBox134.Text.Trim();
                cuscommand.UpdateCommand.Parameters.Add("@ref2mail", SqlDbType.VarChar).Value = textBox124.Text.Trim();

                cuscommand.UpdateCommand.Parameters.Add("@ref3name", SqlDbType.VarChar).Value = textBox594.Text.Trim();
                cuscommand.UpdateCommand.Parameters.Add("@ref3addr", SqlDbType.VarChar).Value = ref3Addr.Text.Trim();
                cuscommand.UpdateCommand.Parameters.Add("@ref3tel", SqlDbType.VarChar).Value = textBox614.Text.Trim();
                cuscommand.UpdateCommand.Parameters.Add("@ref3mail", SqlDbType.VarChar).Value = textBox604.Text.Trim();

                cuscommand.UpdateCommand.Parameters.Add("@ref4name", SqlDbType.VarChar).Value = textBox164.Text.Trim();
                cuscommand.UpdateCommand.Parameters.Add("@ref4addr", SqlDbType.VarChar).Value = ref4Addr.Text.Trim();
                cuscommand.UpdateCommand.Parameters.Add("@ref4tel", SqlDbType.VarChar).Value = textBox584.Text.Trim();
                cuscommand.UpdateCommand.Parameters.Add("@ref4mail", SqlDbType.VarChar).Value = textBox174.Text.Trim();

                             //pageframe2, page 4 details

                cuscommand.UpdateCommand.Parameters.Add("@lnRegFee", SqlDbType.Decimal).Value = gnregfee;
                cuscommand.UpdateCommand.Parameters.Add("@lnSharePrice", SqlDbType.Decimal).Value = lnSharesp;
                cuscommand.UpdateCommand.Parameters.Add("@lnShares", SqlDbType.Int).Value = NoShares;
                cuscommand.UpdateCommand.Parameters.Add("@lnSaveAmt", SqlDbType.Decimal).Value = Convert.ToDecimal(maskedTextBox7.Text);
                cuscommand.UpdateCommand.Parameters.Add("@lnSaveType", SqlDbType.Bit).Value = radioButton16.Checked ? true : false;

                cuscommand.UpdateCommand.Parameters.Add("@sign1", SqlDbType.VarChar).Value = textBox504.Text;
                cuscommand.UpdateCommand.Parameters.Add("@sign2", SqlDbType.VarChar).Value = textBox514.Text;
                cuscommand.UpdateCommand.Parameters.Add("@sign3", SqlDbType.VarChar).Value = textBox244.Text;
                cuscommand.UpdateCommand.Parameters.Add("@sign4", SqlDbType.VarChar).Value = textBox524.Text;

                cuscommand.UpdateCommand.Parameters.Add("@ncompid", SqlDbType.Int).Value = ncompid;
                cuscommand.UpdateCommand.Parameters.Add("@branid", SqlDbType.Int).Value = Convert.ToInt32(comboBox16.SelectedValue);
                cuscommand.UpdateCommand.Parameters.Add("@batid", SqlDbType.Int).Value = Convert.ToInt32(comboBox19.SelectedValue);
                cuscommand.UpdateCommand.Parameters.Add("@memtype", SqlDbType.Int).Value = lnmem_type;

                cuscommand.UpdateCommand.Parameters.Add("@tbmemPict", SqlDbType.Image).Value = ConvertImageToBinary(pictureBox6.Image);
                cuscommand.UpdateCommand.Parameters.Add("@tbmemsign", SqlDbType.Image).Value = ConvertImageToBinary(pictureBox2.Image);


                ndConnHandle1.Open();
                cuscommand.UpdateCommand.ExecuteNonQuery();  //Insert new record
                ndConnHandle1.Close();
                //    initvariables();
                initCorVariables();
            }
        }
        private void initCorVariables()
        {

            //            textBox42.Text = "";
            textBox45.Text = "";
            comboBox18.SelectedIndex = -1;

            //pageframe2, page 1 details
            comboBox112.SelectedIndex = -1;
            comboBox123.SelectedIndex = -1;
            lscortreet.Text = "";
            textBox39.Text = "";
            textBox533.Text = "";
            textBox120.Text = "";
            textBox436.Text = "";
            textBox407.Text = "";
            comboBox101.SelectedIndex = -1;
            comboBox92.SelectedIndex = -1;
            comboBox81.SelectedIndex = -1;
            radioButton12.Checked = false;
            radioButton10.Checked = false;
            radioButton2.Checked = false;

            ///********************************
            //pageframe2, page 2 details
            textBox534.Text = "";
            textBox494.Text = "";
            textBox224.Text = "";
            textBox184.Text = "";
            checkBox3.Checked = false;
            textBox324.Text = "";
            textBox234.Text = "";
            textBox344.Text = "";
            textBox334.Text = "";
            checkBox4.Checked = false;
            textBox554.Text = "";
            textBox544.Text = "";
            textBox574.Text = "";
            textBox564.Text = "";
            checkBox6.Checked = false;
            textBox364.Text = "";
            textBox354.Text = "";
            textBox384.Text = "";
            textBox374.Text = "";
            checkBox5.Checked = false;


            //pageframe2, page 3 details
            textBox664.Text = "";
            ref1Addr.Text = "";
            textBox684.Text = "";
            textBox674.Text = "";
            textBox114.Text = "";
            ref2Addr.Text = "";
            textBox134.Text = "";
            textBox124.Text = "";
            textBox594.Text = "";
            ref3Addr.Text = "";
            textBox614.Text = "";
            textBox604.Text = "";
            textBox164.Text = "";
            ref4Addr.Text = "";
            textBox584.Text = "";
            textBox174.Text = "";

            //MessageBox.Show("Stopped here");
            //pageframe2, page 4 details

            maskedTextBox8.Text = "";
            maskedTextBox9.Text = "";
            textBox294.Text = "";
            maskedTextBox7.Text = "";
            radioButton16.Checked = false;
            textBox504.Text = "";
            textBox514.Text = "";
            textBox244.Text = "";
            textBox524.Text = "";
            comboBox16.SelectedIndex = -1;
            comboBox19.SelectedIndex = -1;
            textBox45.Focus();
            corSaveButton.Enabled = false;
            corSaveButton.BackColor = Color.Gainsboro;
            int lnmem_type = radioButton18.Checked ? 2 : 3;
            string lcmemtpe = (lnmem_type == 2 ? "Corporate " : "Group");
            MessageBox.Show(lcmemtpe + " Member code " + gcMembCode + " added successfully");
            gcMembCode = "";
        }

        private void insertCorGrpMember()
        {
            int lnmem_type = radioButton18.Checked ? 2 : 3;
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                string lcMemCode = GetClient_Code.clientCode_int(cs, "clientid").ToString().Trim().PadLeft(6, '0');
                gcMembCode = lcMemCode;
                int lnBizCat = Convert.ToInt32(comboBox18.SelectedValue);
                gcMembName = textBox45.Text.Trim();

                string cquery1 = "insert into cusreg (ccustcode, ccustname, bizcat, cou_id, ncity, cstreet, ctel, ctel1, cemail, INCORPC, tin, INCORPD, datejoin, nregion, ndist, nward, residents, cust_type, chairname, chairtin, chairtel,";
                cquery1 += "chairmail, chairsig, vcname, vctin, vctel, vcmail, vcsign, treaname, treatin, treatel, treamail, treasign, secname, sectin, sectel, secmail, secsign, ref1name, ref1addr,";
                cquery1 += "ref1tel, ref1mail, ref2name, ref2addr, ref2tel, ref2mail, ref3name, ref3addr, ref3tel, ref3mail, ref4name, ref4addr, ref4tel, ref4mail, nRegFee, nSharePrice, nShares,";
                cquery1 += "nSaveAmt, nSaveType, sign1, sign2, sign3, sign4, compid, branch_id, bat_id,mem_type,memPict,memSign) ";
                cquery1 += "values  ";
                cquery1 += "(@MemCode, @lcFname, @lbizcat, @lnCou, @lnCity, @cStreet, @cTel, @cTel1, @cEmail, @incno, @tin, @incdate, @ldJoin, @lnReg, @lnDis, @nWard, @lnRes, @lcusType, @chairname, @chairtin, @chairtel,";
                cquery1 += "@chairmail, @chairsig, @vcname, @vctin, @vctel, @vcmail, @vcsign, @treaname, @treatin, @treatel, @treamail, @treasign, @secname, @sectin, @sectel, @secmail, @secsign, @ref1name, @ref1addr,";
                cquery1 += "@ref1tel, @ref1mail, @ref2name, @ref2addr, @ref2tel, @ref2mail, @ref3name, @ref3addr, @ref3tel, @ref3mail, @ref4name, @ref4addr, @ref4tel, @ref4mail, @lnRegFee, @lnSharePrice, @lnShares,";
                cquery1 += "@lnSaveAmt, @lnSaveType, @sign1, @sign2, @sign3, @sign4, @ncompid, @branid, @batid,@memtype,@tbmemPict,@tbmemSign)";

                SqlDataAdapter cuscommand = new SqlDataAdapter();
                cuscommand.InsertCommand = new SqlCommand(cquery1, ndConnHandle1);

                // top details
                cuscommand.InsertCommand.Parameters.Add("@MemCode", SqlDbType.VarChar).Value = gcMembCode;
                cuscommand.InsertCommand.Parameters.Add("@lcFname", SqlDbType.VarChar).Value = textBox45.Text.Trim();
                cuscommand.InsertCommand.Parameters.Add("@lbizcat", SqlDbType.Int).Value = Convert.ToInt32(comboBox18.SelectedValue);

                //pageframe2, page 1 details
                cuscommand.InsertCommand.Parameters.Add("@lnCou", SqlDbType.Int).Value = Convert.ToInt32(comboBox112.SelectedValue);
                cuscommand.InsertCommand.Parameters.Add("@lnCity", SqlDbType.Int).Value = Convert.ToInt32(comboBox123.SelectedValue);
                cuscommand.InsertCommand.Parameters.Add("@cStreet", SqlDbType.VarChar).Value = lscortreet.Text.Trim();
                cuscommand.InsertCommand.Parameters.Add("@cTel", SqlDbType.VarChar).Value = textBox39.Text.Trim();
                cuscommand.InsertCommand.Parameters.Add("@cTel1", SqlDbType.VarChar).Value = textBox533.Text.Trim();
                cuscommand.InsertCommand.Parameters.Add("@cEmail", SqlDbType.VarChar).Value = textBox120.Text.Trim();
                cuscommand.InsertCommand.Parameters.Add("@incno", SqlDbType.VarChar).Value = textBox436.Text.Trim();
                cuscommand.InsertCommand.Parameters.Add("@tin", SqlDbType.VarChar).Value = textBox407.Text.Trim();
                cuscommand.InsertCommand.Parameters.Add("@incdate", SqlDbType.DateTime).Value = Convert.ToDateTime(incDate.Text);
                cuscommand.InsertCommand.Parameters.Add("@ldJoin", SqlDbType.DateTime).Value = Convert.ToDateTime(dateJoin.Text);
                cuscommand.InsertCommand.Parameters.Add("@lnReg", SqlDbType.Int).Value = Convert.ToInt32(comboBox101.SelectedValue);
                cuscommand.InsertCommand.Parameters.Add("@lnDis", SqlDbType.Int).Value = Convert.ToInt32(comboBox92.SelectedValue);
                cuscommand.InsertCommand.Parameters.Add("@nWard", SqlDbType.Int).Value = Convert.ToInt32(comboBox81.SelectedValue);
                cuscommand.InsertCommand.Parameters.Add("@lnRes", SqlDbType.Bit).Value = radioButton12.Checked ? true : false;
                cuscommand.InsertCommand.Parameters.Add("@lcusType", SqlDbType.Char).Value = (radioButton10.Checked ? 'C' : (radioButton2.Checked ? 'S' : 'B'));

                ///********************************
                //pageframe2, page 2 details
                cuscommand.InsertCommand.Parameters.Add("@chairname", SqlDbType.VarChar).Value = textBox534.Text.Trim();
                cuscommand.InsertCommand.Parameters.Add("@chairtin", SqlDbType.VarChar).Value = textBox494.Text.Trim();
                cuscommand.InsertCommand.Parameters.Add("@chairtel", SqlDbType.VarChar).Value = textBox224.Text.Trim();
                cuscommand.InsertCommand.Parameters.Add("@chairmail", SqlDbType.VarChar).Value = textBox184.Text.Trim();
                cuscommand.InsertCommand.Parameters.Add("@chairsig", SqlDbType.Bit).Value = checkBox3.Checked ? true : false;

                cuscommand.InsertCommand.Parameters.Add("@vcname", SqlDbType.VarChar).Value = textBox324.Text.Trim();
                cuscommand.InsertCommand.Parameters.Add("@vctin", SqlDbType.VarChar).Value = textBox234.Text.Trim();
                cuscommand.InsertCommand.Parameters.Add("@vctel", SqlDbType.VarChar).Value = textBox344.Text.Trim();
                cuscommand.InsertCommand.Parameters.Add("@vcmail", SqlDbType.VarChar).Value = textBox334.Text.Trim();
                cuscommand.InsertCommand.Parameters.Add("@vcsign", SqlDbType.Bit).Value = checkBox4.Checked ? true : false;

                cuscommand.InsertCommand.Parameters.Add("@treaname", SqlDbType.VarChar).Value = textBox554.Text.Trim();
                cuscommand.InsertCommand.Parameters.Add("@treatin", SqlDbType.VarChar).Value = textBox544.Text.Trim();
                cuscommand.InsertCommand.Parameters.Add("@treatel", SqlDbType.VarChar).Value = textBox574.Text.Trim();
                cuscommand.InsertCommand.Parameters.Add("@treamail", SqlDbType.VarChar).Value = textBox564.Text.Trim();
                cuscommand.InsertCommand.Parameters.Add("@treasign", SqlDbType.Bit).Value = checkBox6.Checked ? true : false;

                cuscommand.InsertCommand.Parameters.Add("@secname", SqlDbType.VarChar).Value = textBox364.Text.Trim();
                cuscommand.InsertCommand.Parameters.Add("@sectin", SqlDbType.VarChar).Value = textBox354.Text.Trim();
                cuscommand.InsertCommand.Parameters.Add("@sectel", SqlDbType.VarChar).Value = textBox384.Text.Trim();
                cuscommand.InsertCommand.Parameters.Add("@secmail", SqlDbType.VarChar).Value = textBox374.Text.Trim();
                cuscommand.InsertCommand.Parameters.Add("@secsign", SqlDbType.Bit).Value = checkBox5.Checked ? true : false;


                //pageframe2, page 3 details
                cuscommand.InsertCommand.Parameters.Add("@ref1name", SqlDbType.VarChar).Value = textBox664.Text.Trim();
                cuscommand.InsertCommand.Parameters.Add("@ref1addr", SqlDbType.VarChar).Value = ref1Addr.Text.Trim();
                cuscommand.InsertCommand.Parameters.Add("@ref1tel", SqlDbType.VarChar).Value = textBox684.Text.Trim();
                cuscommand.InsertCommand.Parameters.Add("@ref1mail", SqlDbType.VarChar).Value = textBox674.Text.Trim();

                cuscommand.InsertCommand.Parameters.Add("@ref2name", SqlDbType.VarChar).Value = textBox114.Text.Trim();
                cuscommand.InsertCommand.Parameters.Add("@ref2addr", SqlDbType.VarChar).Value = ref2Addr.Text.Trim();
                cuscommand.InsertCommand.Parameters.Add("@ref2tel", SqlDbType.VarChar).Value = textBox134.Text.Trim();
                cuscommand.InsertCommand.Parameters.Add("@ref2mail", SqlDbType.VarChar).Value = textBox124.Text.Trim();

                cuscommand.InsertCommand.Parameters.Add("@ref3name", SqlDbType.VarChar).Value = textBox594.Text.Trim();
                cuscommand.InsertCommand.Parameters.Add("@ref3addr", SqlDbType.VarChar).Value = ref3Addr.Text.Trim();
                cuscommand.InsertCommand.Parameters.Add("@ref3tel", SqlDbType.VarChar).Value = textBox614.Text.Trim();
                cuscommand.InsertCommand.Parameters.Add("@ref3mail", SqlDbType.VarChar).Value = textBox604.Text.Trim();

                cuscommand.InsertCommand.Parameters.Add("@ref4name", SqlDbType.VarChar).Value = textBox164.Text.Trim();
                cuscommand.InsertCommand.Parameters.Add("@ref4addr", SqlDbType.VarChar).Value = ref4Addr.Text.Trim();
                cuscommand.InsertCommand.Parameters.Add("@ref4tel", SqlDbType.VarChar).Value = textBox584.Text.Trim();
                cuscommand.InsertCommand.Parameters.Add("@ref4mail", SqlDbType.VarChar).Value = textBox174.Text.Trim();

                //pageframe2, page 4 details

                cuscommand.InsertCommand.Parameters.Add("@lnRegFee", SqlDbType.Decimal).Value = Convert.ToDecimal(maskedTextBox8.Text);
                cuscommand.InsertCommand.Parameters.Add("@lnSharePrice", SqlDbType.Decimal).Value = Convert.ToDecimal(maskedTextBox9.Text);
                cuscommand.InsertCommand.Parameters.Add("@lnShares", SqlDbType.Int).Value = Convert.ToInt32(textBox294.Text);
                cuscommand.InsertCommand.Parameters.Add("@lnSaveAmt", SqlDbType.Decimal).Value = Convert.ToDecimal(maskedTextBox7.Text);
                cuscommand.InsertCommand.Parameters.Add("@lnSaveType", SqlDbType.Bit).Value = radioButton16.Checked ? true : false;

                cuscommand.InsertCommand.Parameters.Add("@sign1", SqlDbType.VarChar).Value = textBox504.Text;
                cuscommand.InsertCommand.Parameters.Add("@sign2", SqlDbType.VarChar).Value = textBox514.Text;
                cuscommand.InsertCommand.Parameters.Add("@sign3", SqlDbType.VarChar).Value = textBox244.Text;
                cuscommand.InsertCommand.Parameters.Add("@sign4", SqlDbType.VarChar).Value = textBox524.Text;

                cuscommand.InsertCommand.Parameters.Add("@ncompid", SqlDbType.Int).Value = ncompid;
                cuscommand.InsertCommand.Parameters.Add("@branid", SqlDbType.Int).Value = Convert.ToInt32(comboBox16.SelectedValue);
                cuscommand.InsertCommand.Parameters.Add("@batid", SqlDbType.Int).Value = Convert.ToInt32(comboBox19.SelectedValue);
                cuscommand.InsertCommand.Parameters.Add("@memtype", SqlDbType.Int).Value = lnmem_type;

                cuscommand.InsertCommand.Parameters.Add("@tbmemPict", SqlDbType.Image).Value = ConvertImageToBinary(pictureBox6.Image);
                cuscommand.InsertCommand.Parameters.Add("@tbmemsign", SqlDbType.Image).Value = ConvertImageToBinary(pictureBox2.Image);
               // ndConnHandle1.Open();
            //    cuscommand.InsertCommand.ExecuteNonQuery();  //Insert new record
                ndConnHandle1.Open();
                int nn = cuscommand.InsertCommand.ExecuteNonQuery();  //Insert new record
                if (nn > 0) //query is successful
                {
                    if (mandview.Rows.Count > 0)
                    {
                       // MessageBox.Show("This is the Fifth catch");
                        for (int j = 0; j < mandview.Rows.Count; j++)
                        {
                         //   MessageBox.Show("This is the Sixth catch");
                            string lcMemCode1 = GetClient_Code.clientCode_int(cs, "clientid").ToString().Trim().PadLeft(6, '0');
                            gcMembCode = lcMemCode1;
                            string lcProductControl = mandview.Rows[j]["prod_control"].ToString().Trim();
                            string lcAcode = lcProductControl.Substring(0, 3);
                            lnPrd_ID = Convert.ToInt16(mandview.Rows[j]["prd_id"]);
                            string lcAcctNumb = lcAcode + lcMemCode1 + "01";
                            gcNonInterestAcct = mandview.Rows[j]["nint_inc"].ToString().Trim();
                            string auditDesc = "Coopearate Member Creation " + lcMemCode1;
                            AuditTrail au = new AuditTrail();
                            au.upd_audit_trail(cs, auditDesc, 0.00m, 0.00m, globalvar.gcUserid, "C", "", lcMemCode1, "", "", globalvar.gcWorkStation, globalvar.gcWinUser);

                            if (lcAcode == "250" || lcAcode == "251")
                            {
                             //   MessageBox.Show("This is the Seventh catch");
                                createAccount(lcAcctNumb);
                                decimal tnPostAmt = gnRegistrationFee;
                                gcMemberSavingsAcct = lcAcctNumb;
                                gcSavingsControlAcct = lcProductControl;
                            }

                            if (lcAcode == "270" || lcAcode == "271")
                            {
                                //  MessageBox.Show("This is the Third catch");
                                createAccount(lcAcctNumb);
                                //  MessageBox.Show("This is the Fouth catch");
                                decimal tnPostAmt = gnSharePrice;
                                gcMemberSharesAcct = lcAcctNumb;
                                gcSharesControlAcct = lcProductControl;
                            }
                            initCorVariables();
                            textBox45.Focus();
                           corSaveButton.Enabled = false;
                           corSaveButton.BackColor = Color.Gainsboro;
                           lcMemCode1 = "";
                        }
                        //  MessageBox.Show("This is the Fifth catch");
                        updateCustomerCode();
                    }
                    postJoiningFee();             
                }
                ndConnHandle1.Close();
            }
        }
        private void tabPage66_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            cam = new VideoCaptureDevice(webcam[webcams.SelectedIndex].MonikerString);
            cam.NewFrame += new NewFrameEventHandler(cam_NewFrame);
            cam.Start();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            using (var findform = new FindClient(cs, ncompid, dloca, 1,"cusreg"))
            {
                var dresult = findform.ShowDialog();
                if (dresult == DialogResult.OK)
                {
                    gcInputStat = "A";
                    string dclientcode = findform.returnValue;
                    getbasicDetails(dclientcode);
                }
            }
        }

        private void getbasicDetails(string tcMemCode)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                memberview.Clear();
                string basicsql = "select * from cusreg where ccustcode = " + "'" + tcMemCode + "'"; 
                SqlDataAdapter dabas = new SqlDataAdapter(basicsql, ndConnHandle);
                dabas.Fill(memberview);
                if (memberview != null && memberview.Rows.Count>0)
                {
                    if (!Convert.IsDBNull(memberview.Rows[0]["memPict"]))
                    {
                        byte[] myimg = (byte[])memberview.Rows[0]["memPict"];
                        if (myimg != null)
                        {
                            MemoryStream ms = new MemoryStream(myimg);
                            ms.Position = 0;
                            Bitmap bm = new Bitmap(ms);
                            pictureBox1.Image = bm;
                        }
                    }
                    if (!Convert.IsDBNull(memberview.Rows[0]["memsign"]))
                    {
                        byte[] mysig = (byte[])memberview.Rows[0]["memsign"];
                        if (mysig != null)
                        {
                            MemoryStream al = new MemoryStream(mysig);
                            al.Position = 0;
                            Bitmap bms = new Bitmap(al);
                            pictureBox4.Image = bms;
                        }
                     }  

                    textBox1.Text = memberview.Rows[0]["ccustfname"].ToString().Trim();
                    textBox2.Text = memberview.Rows[0]["ccustmname"].ToString().Trim();
                    textBox3.Text = memberview.Rows[0]["ccustlname"].ToString().Trim();
                    textBox46.Text = memberview.Rows[0]["ccustcode"].ToString().Trim();
                    comboBox1.SelectedValue  = Convert.ToInt16(memberview.Rows[0]["branch_id"]);
                    textBox3.Text = memberview.Rows[0]["ccustlname"].ToString().Trim();
                    comboBox2.SelectedValue = Convert.ToInt16(memberview.Rows[0]["ccusttitle"]);
                    comboBox3.SelectedValue = Convert.ToInt16(memberview.Rows[0]["natcode"]);
                    dob.Value = Convert.ToDateTime(memberview.Rows[0]["ddatebirth"]);
                    doj.Value = Convert.ToDateTime(memberview.Rows[0]["datejoin"]);
                    dmale.Checked = Convert.ToBoolean(memberview.Rows[0]["gender"]);
                    dfemale.Checked = !dmale.Checked;
                    comboBox4.SelectedValue = Convert.ToInt16(memberview.Rows[0]["marital"]);
                    comboBox5.SelectedValue = Convert.ToInt16(memberview.Rows[0]["idtype"]);
                    textBox4.Text = memberview.Rows[0]["cpassno"].ToString().Trim();
                    textBox5.Text = memberview.Rows[0]["cplacissue"].ToString().Trim();
                    dateIssued.Value = Convert.ToDateTime(memberview.Rows[0]["ddateissue"]);
                    expDate.Value = Convert.ToDateTime(memberview.Rows[0]["ddateexpire"]);

                    comboBox20.SelectedValue = Convert.ToInt16(memberview.Rows[0]["levelofedu"]);
                    comboBox21.SelectedValue = Convert.ToInt16(memberview.Rows[0]["tribe"]);
                    comboBox22.SelectedValue = Convert.ToInt16(memberview.Rows[0]["povertylevel"]);
                    comboBox6.SelectedValue = Convert.ToInt16(memberview.Rows[0]["nregion"]);
                    comboBox7.SelectedValue = Convert.ToInt16(memberview.Rows[0]["ndist"]);
                    comboBox8.SelectedValue = Convert.ToInt16(memberview.Rows[0]["nward"]);
                    radioButton4.Checked = Convert.ToBoolean(memberview.Rows[0]["residents"]);
                    radioButton3.Checked = !radioButton4.Checked;
                    radioButton5.Checked = Convert.ToBoolean(memberview.Rows[0]["cust_type"].ToString().Trim()=="C");
                    radioButton6.Checked = Convert.ToBoolean(memberview.Rows[0]["cust_type"].ToString().Trim() == "S");
                    radioButton7.Checked = Convert.ToBoolean(memberview.Rows[0]["cust_type"].ToString().Trim() == "B");

                    comboBox9.SelectedValue = Convert.ToInt16(memberview.Rows[0]["cou_id"]);
                    comboBox10.SelectedValue = Convert.ToInt16(memberview.Rows[0]["ncity"]);
                    comboBox11.SelectedValue = Convert.ToInt16(memberview.Rows[0]["nrel"]);
                    contaddr.Text = memberview.Rows[0]["cstreet"].ToString().Trim();
                    textBox6.Text = memberview.Rows[0]["ctel"].ToString().Trim();
                    textBox7.Text = memberview.Rows[0]["ctel1"].ToString().Trim();
                    textBox8.Text = memberview.Rows[0]["cemail"].ToString().Trim();
                    textBox9.Text = memberview.Rows[0]["cname1"].ToString().Trim();
                    refAddr.Text = memberview.Rows[0]["caddr1"].ToString().Trim();
                    textBox10.Text = memberview.Rows[0]["cmobile1"].ToString().Trim();
                    textBox11.Text = memberview.Rows[0]["cemail"].ToString().Trim();

                    textBox12.Text = memberview.Rows[0]["cname2"].ToString().Trim();
                    nokAddr.Text = memberview.Rows[0]["caddr2"].ToString().Trim();
                    textBox13.Text = memberview.Rows[0]["cmobile2"].ToString().Trim();

                    comboBox12.SelectedValue = Convert.ToInt16(memberview.Rows[0]["nEmployer"]);

                    textBox18.Text = memberview.Rows[0]["payroll_id"].ToString().Trim();
                    comboBox13.SelectedValue = Convert.ToInt16(memberview.Rows[0]["nDesig"]);
                    comboBox14.SelectedValue = Convert.ToInt16(memberview.Rows[0]["ndept"]);
                    maskedTextBox6.Text = memberview.Rows[0]["nSal"].ToString().Trim();

                    maskedTextBox4.Text = memberview.Rows[0]["nyears"].ToString().Trim();
                    //  lnShares = Convert.ToInt16(memberview.Rows[0]["nsal"]);

                    maskedTextBox3.Text = memberview.Rows[0]["nSharePrice"].ToString().Trim();

                    textBox25.Text = memberview.Rows[0]["nShares"].ToString().Trim();
                    comboBox12.SelectedValue = Convert.ToInt16(memberview.Rows[0]["bat_id"]);
                    maskedTextBox5.Text = memberview.Rows[0]["nSaveAmt"].ToString().Trim();
                    textBox23.Text = memberview.Rows[0]["cSignatory"].ToString().Trim();
                    radioButton8.Checked = Convert.ToBoolean(memberview.Rows[0]["nSaveType"]);
                    radioButton9.Checked = !radioButton8.Checked;
                    page02Ok();
                }
                else { MessageBox.Show("Basic details removed, inform IT Dept immediately"); }
            }
        }

        private void getbasicDetailsInt(string tcMemCode)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                memberview.Clear();
                string basicsql = "select * from cusreg where ccustcode = " + "'" + tcMemCode + "'";
                SqlDataAdapter dabas = new SqlDataAdapter(basicsql, ndConnHandle);
                dabas.Fill(memberview);
                if (memberview != null && memberview.Rows.Count > 0)
                {
                    if (!Convert.IsDBNull(memberview.Rows[0]["memPict"]))
                    {
                        byte[] myimg = (byte[])memberview.Rows[0]["memPict"];
                        if (myimg != null)
                        {
                            MemoryStream ms = new MemoryStream(myimg);
                            ms.Position = 0;
                            Bitmap bm = new Bitmap(ms);
                            pictureBox6.Image = bm;
                        }
                    }
                    if (!Convert.IsDBNull(memberview.Rows[0]["memsign"]))
                    {
                        byte[] mysig = (byte[])memberview.Rows[0]["memsign"];
                        if (mysig != null)
                        {
                            MemoryStream al = new MemoryStream(mysig);
                            al.Position = 0;
                            Bitmap bms = new Bitmap(al);
                            pictureBox2.Image = bms;
                        }
                    }

                    //textBox1.Text = memberview.Rows[0]["ccustfname"].ToString().Trim();
                    //textBox2.Text = memberview.Rows[0]["ccustmname"].ToString().Trim();
                    textBox45.Text = memberview.Rows[0]["ccustname"].ToString().Trim();
                    textBox42.Text = memberview.Rows[0]["ccustcode"].ToString().Trim();
                    comboBox16.SelectedValue = Convert.ToInt16(memberview.Rows[0]["branch_id"]);
                    comboBox18.SelectedValue = Convert.ToInt16(memberview.Rows[0]["bizcat"]);

                    comboBox112.SelectedValue = Convert.ToInt16(memberview.Rows[0]["cou_id"]);
                    comboBox123.SelectedValue = Convert.ToInt16(memberview.Rows[0]["ncity"]);
                    comboBox101.SelectedValue = Convert.ToInt16(memberview.Rows[0]["nrel"]);
                    lscortreet.Text = memberview.Rows[0]["cstreet"].ToString().Trim();
                    textBox39.Text = memberview.Rows[0]["ctel"].ToString().Trim();
                    textBox533.Text = memberview.Rows[0]["ctel1"].ToString().Trim();
                    textBox120.Text = memberview.Rows[0]["cemail"].ToString().Trim();

                    textBox436.Text = memberview.Rows[0]["INCORPC"].ToString().Trim();
                    textBox407.Text = memberview.Rows[0]["tin"].ToString().Trim();
                   incDate.Value = Convert.ToDateTime(memberview.Rows[0]["INCORPD"]);
                    dateJoin.Value = Convert.ToDateTime(memberview.Rows[0]["datejoin"]);

                    comboBox101.SelectedValue = memberview.Rows[0]["nregion"].ToString().Trim();
                    comboBox92.SelectedValue = memberview.Rows[0]["ndist"].ToString().Trim();
                    comboBox81.SelectedValue = memberview.Rows[0]["nward"].ToString().Trim();

                    radioButton10.Checked = Convert.ToBoolean(memberview.Rows[0]["cust_type"].ToString().Trim() == "C");
                    radioButton2.Checked = Convert.ToBoolean(memberview.Rows[0]["cust_type"].ToString().Trim() == "S");
                    radioButton1.Checked = Convert.ToBoolean(memberview.Rows[0]["cust_type"].ToString().Trim() == "B");

                    textBox534.Text = memberview.Rows[0]["chairname"].ToString();
                   textBox494.Text = memberview.Rows[0]["chairtin"].ToString();
                    textBox224.Text = memberview.Rows[0]["chairtel"].ToString().Trim();
                    textBox184.Text = memberview.Rows[0]["chairmail"].ToString().Trim();

                    textBox324.Text = memberview.Rows[0]["vcname"].ToString().Trim();
                    textBox234.Text = memberview.Rows[0]["vctin"].ToString().Trim();
                    textBox344.Text = memberview.Rows[0]["vctel"].ToString().Trim();
                    textBox334.Text = memberview.Rows[0]["vcsign"].ToString().Trim();

                    textBox554.Text = memberview.Rows[0]["treaname"].ToString().Trim();
                    textBox544.Text = memberview.Rows[0]["treatin"].ToString().Trim();
                    textBox574.Text = memberview.Rows[0]["treatel"].ToString().Trim();
                    textBox564.Text = memberview.Rows[0]["treamail"].ToString().Trim();

                    textBox364.Text = memberview.Rows[0]["secname"].ToString().Trim();
                    textBox354.Text = memberview.Rows[0]["sectin"].ToString().Trim();
                    textBox384.Text = memberview.Rows[0]["sectel"].ToString().Trim();
                    textBox374.Text = memberview.Rows[0]["secmail"].ToString().Trim();

                    textBox664.Text = memberview.Rows[0]["ref1name"].ToString().Trim();
                    ref1Addr.Text = memberview.Rows[0]["ref1addr"].ToString().Trim();
                    textBox684.Text = memberview.Rows[0]["ref1tel"].ToString().Trim();
                    textBox674.Text = memberview.Rows[0]["ref1mail"].ToString().Trim();

                    textBox114.Text = memberview.Rows[0]["ref2name"].ToString().Trim();
                    ref2Addr.Text = memberview.Rows[0]["ref2addr"].ToString().Trim();
                    textBox134.Text = memberview.Rows[0]["ref2tel"].ToString().Trim();
                    textBox124.Text = memberview.Rows[0]["ref2mail"].ToString().Trim();

                    textBox594.Text = memberview.Rows[0]["ref3name"].ToString().Trim();
                    ref3Addr.Text = memberview.Rows[0]["ref3addr"].ToString().Trim();
                    textBox614.Text = memberview.Rows[0]["ref3tel"].ToString().Trim();
                    textBox604.Text = memberview.Rows[0]["ref3mail"].ToString().Trim();

                    textBox164.Text = memberview.Rows[0]["ref4name"].ToString().Trim();
                    ref4Addr.Text = memberview.Rows[0]["ref4addr"].ToString().Trim();
                    textBox584.Text = memberview.Rows[0]["ref4tel"].ToString().Trim();
                    textBox174.Text = memberview.Rows[0]["ref4mail"].ToString().Trim();

                    maskedTextBox8.Text = memberview.Rows[0]["nRegFee"].ToString().Trim();
                    //  lnShares = Convert.ToInt16(memberview.Rows[0]["nsal"]);

                    textBox284.Text = memberview.Rows[0]["nSharePrice"].ToString().Trim();

                    textBox294.Text = memberview.Rows[0]["nShares"].ToString().Trim();
                    comboBox19.SelectedValue = Convert.ToInt16(memberview.Rows[0]["bat_id"]);
                    maskedTextBox7.Text = memberview.Rows[0]["nSaveAmt"].ToString().Trim();
                    textBox504.Text = memberview.Rows[0]["sign1"].ToString().Trim();
                    textBox514.Text = memberview.Rows[0]["sign2"].ToString().Trim();
                    textBox244.Text = memberview.Rows[0]["sign3"].ToString().Trim();
                    textBox524.Text = memberview.Rows[0]["sign4"].ToString().Trim();
                    radioButton16.Checked = Convert.ToBoolean(memberview.Rows[0]["nSaveType"]);
                    radioButton15.Checked = !radioButton8.Checked;
                    radioButton18.Checked = Convert.ToBoolean(memberview.Rows[0]["mem_type"]);
                    radioButton17.Checked = !radioButton8.Checked;
                    maskedTextBox9.Text = gnSharePrice.ToString();
                    page02Ok();
                }
                else { MessageBox.Show("Basic details removed, inform IT Dept immediately"); }
            }
        }
        private void button11_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            WebCam wc = new WebCam();
            wc.ShowDialog();
        }

        private void button11_Click_1(object sender, EventArgs e)
        {
            if (cam.IsRunning)
            {
                cam.Stop();
            }
        }

        byte[] ConvertImageToBinary(Image img)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }

        }
        private void button17_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Picture taken");
            //using (SqlConnection nConnHandle2 = new SqlConnection(cs))
            //{
            //    string cglquery = "Insert Into memberpictures (ccustcode,custpicture)";
            //    cglquery += " values (@tcCustCode,@tcCustPict)";
            //    SqlDataAdapter insCommand = new SqlDataAdapter();
            //    insCommand.InsertCommand = new SqlCommand(cglquery, nConnHandle2);
            //    insCommand.InsertCommand.Parameters.Add("@tcCustCode", SqlDbType.VarChar).Value = "000001";
            //    insCommand.InsertCommand.Parameters.Add("@tcCustPict", SqlDbType.Image).Value = ConvertImageToBinary(pictureBox1.Image); 
            //    nConnHandle2.Open();
            //    insCommand.InsertCommand.ExecuteNonQuery();
            //    nConnHandle2.Close();
            //}
        }

        private class WebCam
        {
            internal void ShowDialog()
            {
                throw new NotImplementedException();
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {

        }

        private void button21_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "JPG Files(*.jpg)|*.jpg|PNG Files(*.png)|*.png|All Files(*.*)|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string picloc = dlg.FileName.ToString();
                textBox20.Text = picloc;
                pictureBox4.ImageLocation = picloc;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "JPG Files(*.jpg)|*.jpg|PNG Files(*.png)|*.png|All Files(*.*)|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string picloc = dlg.FileName.ToString();
               // textBox20.Text = picloc;
                pictureBox1.ImageLocation = picloc;
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
          //  pictureBox4. = '';
        }

        private void maskedTextBox6_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void maskedTextBox4_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox2.Checked )
            {
                panel16.Enabled = true;
            }
            else
            {
                panel16.Enabled = false;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (var findform = new FindClient(cs, ncompid, dloca, 1, "cusreg"))
            {
                var dresult = findform.ShowDialog();
                if (dresult == DialogResult.OK)
                {
                    gcInputStat = "A";
                    string dclientcode = findform.returnValue;
                    getbasicDetailsInt(dclientcode);
                }
            }
        }

        private void textBox594_TextChanged(object sender, EventArgs e)
        {

        }

        private void button36_Click(object sender, EventArgs e)
        {

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "JPG Files(*.jpg)|*.jpg|PNG Files(*.png)|*.png|All Files(*.*)|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string picloc = dlg.FileName.ToString();
                // textBox20.Text = picloc;
                pictureBox6.ImageLocation = picloc;
            }
        }

        private void button35_Click(object sender, EventArgs e)
        {

        }

        private void button32_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "JPG Files(*.jpg)|*.jpg|PNG Files(*.png)|*.png|All Files(*.*)|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string picloc = dlg.FileName.ToString();
                textBox20.Text = picloc;
                pictureBox2.ImageLocation = picloc;
            }
        }

        private void comboBox19_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label124_Click(object sender, EventArgs e)
        {

        }

        //private void insertnewmember()
        //{
        //    using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
        //    {
        //        string lcMemCode = GetClient_Code.clientCode_int(cs, "clientid").ToString().Trim().PadLeft(6, '0');
        //        gcMembCode = lcMemCode;              // textBox46.Text.Trim().PadLeft(6, '0');
        //                                             // MessageBox.Show("This code is" + gcMembCode);
        //        textBox46.Text = (Convert.ToInt32(gcMembCode) + 1).ToString().PadLeft(6, '0');                               //                textBox46.Text = lcMemCode;
        //        string lcFname = textBox1.Text;
        //        string lcMname = textBox2.Text;
        //        string lcLname = textBox3.Text;
        //        gcMembName = textBox1.Text.Trim() + ' ' + textBox3.Text.Trim();
        //        bool lnempl = (checkBox2.Checked ? true : false);
        //        string lcEmployee = textBox18.Text;

        //        //pageframe 1, page 1 
        //        int lnTitle = Convert.ToInt32(comboBox2.SelectedValue);
        //        int lnNatCode = Convert.ToInt32(comboBox3.SelectedValue);
        //        DateTime ldDOB = Convert.ToDateTime(dob.Value);
        //        DateTime ldJoin = Convert.ToDateTime(doj.Value);
        //        bool llMale = (Convert.ToBoolean(dmale.Checked) ? true : false);
        //        int lnMarital = Convert.ToInt32(comboBox4.SelectedValue);
        //        int lnIDtype = Convert.ToInt32(comboBox5.SelectedValue);
        //        string lcIDNumb = textBox4.Text;
        //        string lcPlaceIssued = textBox5.Text;
        //        DateTime ldDateIssue = Convert.ToDateTime(dateIssued.Value);
        //        DateTime ldDateExpire = Convert.ToDateTime(expDate.Value);
        //        int lnLevelOfedu = Convert.ToInt32(comboBox20.SelectedValue);
        //        int lnPovertyLevel = Convert.ToInt32(comboBox22.SelectedValue);
        //        int lnTribe = Convert.ToInt32(comboBox21.SelectedValue);
        //        int lnRegion = Convert.ToInt32(comboBox6.SelectedValue);
        //        int lnDistrict = Convert.ToInt32(comboBox7.SelectedValue);
        //        int lnWard = Convert.ToInt32(comboBox8.SelectedValue);
        //        bool llRes = (Convert.ToBoolean(radioButton4.Checked) ? true : false);
        //        char lcMType = (radioButton5.Checked ? 'C' : (radioButton6.Checked ? 'S' : 'B'));

        //        //pageframe 1, page 2
        //        int lnCou = Convert.ToInt32(comboBox9.SelectedValue);
        //        int lnCity = Convert.ToInt32(comboBox10.SelectedValue);
        //        string lcStreet = contaddr.Text.ToString();
        //        string lcTel = textBox6.Text;
        //        string lcTel1 = textBox7.Text;
        //        string lcEmail = textBox8.Text;

        //        string lcName1 = textBox9.Text;
        //        string lcAddr1 = refAddr.Text;
        //        string lcMobile1 = textBox10.Text;
        //        string lcEmail1 = textBox11.Text;

        //        string lcNokName = textBox12.Text;
        //        string lcNokAddr = nokAddr.Text;
        //        int lnNokRel = Convert.ToInt32(comboBox11.SelectedValue);
        //        string lcNokTel = textBox13.Text;

        //        //pageframe 1, page 3
        //        int lnEmployer = Convert.ToInt32(comboBox12.SelectedValue);
        //        string lcStaffNo = textBox18.Text;
        //        int lnDesig = Convert.ToInt32(comboBox13.SelectedValue);
        //        int lnDept = Convert.ToInt32(comboBox14.SelectedValue);
        //        int lnYears = maskedTextBox4.Text != "" ? Convert.ToInt32(maskedTextBox4.Text) : 0;
        //        decimal lnSal = maskedTextBox6.Text != "" ? Convert.ToDecimal(maskedTextBox6.Text) : 0.00m;

        //        //pageframe 1, page 4
        //        //decimal lnRegFee = Convert.ToDecimal(maskedTextBox1.Text);
        //        //decimal lnSharePrice = Convert.ToDecimal(maskedTextBox2.Text);
        //        int lnShares = textBox25.Text.ToString().Trim() != "" ? Convert.ToInt16(textBox25.Text) : 0;
        //        decimal lnSaveAmt = maskedTextBox5.Text != "" ? Convert.ToDecimal(maskedTextBox5.Text) : 0.00m;
        //        bool llSaveType = (radioButton8.Checked ? true : false);
        //        string lcSignatory = textBox23.Text;
        //        bool llmodepay = checkBox1.Checked;
        //        int lbatid = Convert.ToInt32(comboBox15.SelectedValue);
        //        bool llMobileWallet = checkBox10.Checked ? true : false;
        //        string cquery = "exec tsp_Insert_NewMember @MemCode,@lcFname,@lcMname,@lcLname,@lnEmpl,@lnTitle,@lnNatCode,@ldDOB,@ldJoin,@lnmale,@marital,@idtype,@lcIdNumb,@lcPlace,@dDateIssue,@dDateExpire,@lnReg,@lnDis,@nWard,@lnRes,@lcMType,@lnCou,";
        //        cquery += "@lnCity,@cStreet,@cTel,@cTel1,@cEmail,@cName1,@cAddr1,@cMobile1,@cEmail1,@lcName2,@lcAddr2,@lnRel,@lcMobile2,@nEmployer,@cStaffNo,@nDesig,@nDept,@nYears,@nSal,@lnRegFee,@lnSharePrice,@lnShares,";
        //        cquery += "@lnSaveAmt,@lnSaveType,@lcSignatory,@lmodepay,@lcommpid,@branid,@batid,@tbmemPict,@tbmemsign,@levelofedu,@povertylevel,@tribe,@tlWalleto";


        //        SqlDataAdapter cuscommand = new SqlDataAdapter();
        //        cuscommand.InsertCommand = new SqlCommand(cquery, ndConnHandle1);

        //        //page 1 top details
        //        cuscommand.InsertCommand.Parameters.Add("@MemCode", SqlDbType.VarChar).Value = gcMembCode;
        //        cuscommand.InsertCommand.Parameters.Add("@lcFname", SqlDbType.VarChar).Value = lcFname;
        //        cuscommand.InsertCommand.Parameters.Add("@lcMname", SqlDbType.VarChar).Value = lcMname;
        //        cuscommand.InsertCommand.Parameters.Add("@lcLname", SqlDbType.VarChar).Value = lcLname;
        //        cuscommand.InsertCommand.Parameters.Add("@lnEmpl", SqlDbType.Bit).Value = lnempl;


        //        //pageframe1, page 1 details
        //        cuscommand.InsertCommand.Parameters.Add("@lnTitle", SqlDbType.Int).Value = lnTitle;
        //        cuscommand.InsertCommand.Parameters.Add("@lnNatCode", SqlDbType.Int).Value = lnNatCode;
        //        cuscommand.InsertCommand.Parameters.Add("@ldDOB", SqlDbType.DateTime).Value = ldDOB;
        //        cuscommand.InsertCommand.Parameters.Add("@ldJoin", SqlDbType.DateTime).Value = ldJoin;
        //        cuscommand.InsertCommand.Parameters.Add("@lnmale", SqlDbType.Bit).Value = llMale;
        //        cuscommand.InsertCommand.Parameters.Add("@marital", SqlDbType.Int).Value = lnMarital;
        //        cuscommand.InsertCommand.Parameters.Add("@idtype", SqlDbType.Int).Value = lnIDtype;
        //        cuscommand.InsertCommand.Parameters.Add("@lcIdNumb", SqlDbType.VarChar).Value = lcIDNumb;
        //        cuscommand.InsertCommand.Parameters.Add("@lcPlace", SqlDbType.VarChar).Value = lcPlaceIssued;
        //        cuscommand.InsertCommand.Parameters.Add("@dDateIssue", SqlDbType.DateTime).Value = ldDateIssue;
        //        cuscommand.InsertCommand.Parameters.Add("@dDateExpire", SqlDbType.DateTime).Value = ldDateExpire;
        //        cuscommand.InsertCommand.Parameters.Add("@lnReg", SqlDbType.Int).Value = lnRegion;
        //        cuscommand.InsertCommand.Parameters.Add("@lnDis", SqlDbType.Int).Value = lnDistrict;
        //        cuscommand.InsertCommand.Parameters.Add("@nWard", SqlDbType.Int).Value = lnWard;
        //        cuscommand.InsertCommand.Parameters.Add("@lnRes", SqlDbType.Bit).Value = llRes;
        //        cuscommand.InsertCommand.Parameters.Add("@lcMType", SqlDbType.VarChar).Value = lcMType;
        //        cuscommand.InsertCommand.Parameters.Add("@tlWalleto", SqlDbType.Bit).Value = llMobileWallet;

        //        //pageframe1, page 2 details
        //        cuscommand.InsertCommand.Parameters.Add("@lnCou", SqlDbType.Int).Value = lnCou;
        //        cuscommand.InsertCommand.Parameters.Add("@lnCity", SqlDbType.Int).Value = lnCity;
        //        cuscommand.InsertCommand.Parameters.Add("@cStreet", SqlDbType.VarChar).Value = lcStreet;
        //        cuscommand.InsertCommand.Parameters.Add("@cTel", SqlDbType.VarChar).Value = lcTel;
        //        cuscommand.InsertCommand.Parameters.Add("@cTel1", SqlDbType.VarChar).Value = lcTel1;
        //        cuscommand.InsertCommand.Parameters.Add("@cEmail", SqlDbType.VarChar).Value = lcEmail;
        //        cuscommand.InsertCommand.Parameters.Add("@cName1", SqlDbType.VarChar).Value = lcName1;
        //        cuscommand.InsertCommand.Parameters.Add("@cAddr1", SqlDbType.VarChar).Value = lcAddr1;
        //        cuscommand.InsertCommand.Parameters.Add("@cMobile1", SqlDbType.VarChar).Value = lcMobile1;
        //        cuscommand.InsertCommand.Parameters.Add("@cEmail1", SqlDbType.VarChar).Value = lcEmail1;
        //        cuscommand.InsertCommand.Parameters.Add("@lcName2", SqlDbType.VarChar).Value = lcNokName;
        //        cuscommand.InsertCommand.Parameters.Add("@lcAddr2", SqlDbType.VarChar).Value = lcNokAddr;
        //        cuscommand.InsertCommand.Parameters.Add("@lnRel", SqlDbType.Int).Value = lnNokRel;
        //        cuscommand.InsertCommand.Parameters.Add("@lcMobile2", SqlDbType.VarChar).Value = lcNokTel;


        //        //pageframe1, page 3 details
        //        cuscommand.InsertCommand.Parameters.Add("@nEmployer", SqlDbType.Int).Value = lnEmployer;
        //        cuscommand.InsertCommand.Parameters.Add("@cStaffNo", SqlDbType.VarChar).Value = lcStaffNo;
        //        cuscommand.InsertCommand.Parameters.Add("@nDesig", SqlDbType.Int).Value = lnDesig;
        //        cuscommand.InsertCommand.Parameters.Add("@nDept", SqlDbType.Int).Value = lnDept;
        //        cuscommand.InsertCommand.Parameters.Add("@nYears", SqlDbType.Int).Value = lnYears;
        //        cuscommand.InsertCommand.Parameters.Add("@nSal", SqlDbType.Decimal).Value = lnSal;


        //        //pageframe 1, page 4 details
        //        cuscommand.InsertCommand.Parameters.Add("@lnRegFee", SqlDbType.Decimal).Value = gnRegistrationFee;
        //        cuscommand.InsertCommand.Parameters.Add("@lnSharePrice", SqlDbType.Decimal).Value = gnSharePrice;
        //        cuscommand.InsertCommand.Parameters.Add("@lnShares", SqlDbType.Int).Value = lnShares;
        //        cuscommand.InsertCommand.Parameters.Add("@lnSaveAmt", SqlDbType.Decimal).Value = lnSaveAmt;
        //        cuscommand.InsertCommand.Parameters.Add("@lnSaveType", SqlDbType.Bit).Value = llSaveType;
        //        cuscommand.InsertCommand.Parameters.Add("@lcSignatory", SqlDbType.VarChar).Value = lcSignatory;
        //        cuscommand.InsertCommand.Parameters.Add("@lmodepay", SqlDbType.Bit).Value = llmodepay;
        //        cuscommand.InsertCommand.Parameters.Add("@lcommpid", SqlDbType.Int).Value = globalvar.gnCompid;
        //        cuscommand.InsertCommand.Parameters.Add("@branid", SqlDbType.Int).Value = Convert.ToInt32(comboBox1.SelectedValue);
        //        cuscommand.InsertCommand.Parameters.Add("@batid", SqlDbType.Int).Value = Convert.ToInt32(comboBox15.SelectedValue);

        //        //pageframe 1, page biometric details
        //        cuscommand.InsertCommand.Parameters.Add("@tbmemPict", SqlDbType.Image).Value = ConvertImageToBinary(pictureBox1.Image);
        //        cuscommand.InsertCommand.Parameters.Add("@tbmemsign", SqlDbType.Image).Value = ConvertImageToBinary(pictureBox4.Image);

        //        cuscommand.InsertCommand.Parameters.Add("@levelofedu", SqlDbType.Int).Value = lnLevelOfedu;
        //        cuscommand.InsertCommand.Parameters.Add("@povertylevel", SqlDbType.Int).Value = lnPovertyLevel;
        //        cuscommand.InsertCommand.Parameters.Add("@tribe", SqlDbType.Int).Value = lnTribe;

        //        string cquery = "update cusreg set ccustfname=@tlcFname, ccustmname=@tlcMname, ccustlname=@tlcLname,employed=@tlnEmpl,ccusttitle=@tlnTitle,natcode=@tlnNatCode,ddatebirth=@tldDOB,datejoin=@tldJoin,gender=@tlnmale,marital=@tmarital,idtype=@tidtype,";
        //        cquery += "cpassno =@tlcIdNumb,cplacissue=@tlcPlace,ddateissue=@tdDateIssue,ddateexpire=@tdDateExpire,nregion=@tlnReg,ndist=@tlnDis,nward=@tnWard,residents=@tlnRes, cust_type=@tlcMType,cou_id=@tlnCou,";
        //        cquery += "ncity=@tlnCity,cstreet=@tcStreet,ctel=@tcTel,ctel1=@tcTel1,cemail=@tcEmail,cname1=@tcName1,caddr1=@tcAddr1,cMobile1=@tcMobile1,cEmail1=@tcEmail1,cName2=@tlcName2,cAddr2=@tlcAddr2,nRel=@tlnRel,cMobile2=@tlcMobile2,";
        //        cquery += "nEmployer =@tnEmployer,cstaffno=@tcStaffNo,payroll_id=@tcStaffNo,nDesig=@tnDesig,nDept=@tnDept,nYears=@tnYears,nSal=@tnSal,nRegFee=@tlnRegFee,nSharePrice=@tlnSharePrice,nShares=@tlnShares,";
        //        cquery += "nSaveAmt=@tlnSaveAmt,nSaveType=@tlnSaveType,cSignatory=@tlcSignatory,modepay=@tlmodepay,branch_id=@tbranid,bat_id=@tbatid,memPict=@tbmemPict,memsign=@tbmemsign,levelofedu = @levelofedu,tribe = @tribe,povertylevel = @povertylevel  where ccustcode = @MemCode";

        //        //string cquery = "update cusreg set memPict=@tbmemPict where ccustcode = @MemCode";

        //        SqlDataAdapter cusAmdcommand = new SqlDataAdapter();
        //        cusAmdcommand.UpdateCommand = new SqlCommand(cquery, ndConnHandle1);

        //        ////page 1 top details
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@MemCode", SqlDbType.VarChar).Value = gcMembCode;
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tlcFname", SqlDbType.VarChar).Value = lcFname;
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tlcMname", SqlDbType.VarChar).Value = lcMname;
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tlcLname", SqlDbType.VarChar).Value = lcLname;
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tlnEmpl", SqlDbType.Bit).Value = lnempl;


        //        //pageframe1, page 1 details
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tlnTitle", SqlDbType.Int).Value = lnTitle;
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tlnNatCode", SqlDbType.Int).Value = lnNatCode;
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tldDOB", SqlDbType.DateTime).Value = ldDOB;
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tldJoin", SqlDbType.DateTime).Value = ldJoin;
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tlnmale", SqlDbType.Bit).Value = llMale;
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tmarital", SqlDbType.Int).Value = lnMarital;
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tidtype", SqlDbType.Int).Value = lnIDtype;
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tlcIdNumb", SqlDbType.VarChar).Value = lcIDNumb;
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tlcPlace", SqlDbType.VarChar).Value = lcPlaceIssued;
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tdDateIssue", SqlDbType.DateTime).Value = ldDateIssue;
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tdDateExpire", SqlDbType.DateTime).Value = ldDateExpire;
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tlnReg", SqlDbType.Int).Value = lnRegion;
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tlnDis", SqlDbType.Int).Value = lnDistrict;
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tnWard", SqlDbType.Int).Value = lnWard;
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tlnRes", SqlDbType.Bit).Value = llRes;
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tlcMType", SqlDbType.VarChar).Value = lcMType;

        //        //pageframe1, page 2 details
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tlnCou", SqlDbType.Int).Value = lnCou;
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tlnCity", SqlDbType.Int).Value = lnCity;
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tcStreet", SqlDbType.VarChar).Value = lcStreet;
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tcTel", SqlDbType.VarChar).Value = lcTel;
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tcTel1", SqlDbType.VarChar).Value = lcTel1;
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tcEmail", SqlDbType.VarChar).Value = lcEmail;
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tcName1", SqlDbType.VarChar).Value = lcName1;
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tcAddr1", SqlDbType.VarChar).Value = lcAddr1;
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tcMobile1", SqlDbType.VarChar).Value = lcMobile1;
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tcEmail1", SqlDbType.VarChar).Value = lcEmail1;
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tlcName2", SqlDbType.VarChar).Value = lcNokName;
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tlcAddr2", SqlDbType.VarChar).Value = lcNokAddr;
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tlnRel", SqlDbType.Int).Value = lnNokRel;
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tlcMobile2", SqlDbType.VarChar).Value = lcNokTel;


        //        //pageframe1, page 3 details
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tnEmployer", SqlDbType.Int).Value = lnEmployer;
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tcStaffNo", SqlDbType.VarChar).Value = lcStaffNo;
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tnDesig", SqlDbType.Int).Value = lnDesig;
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tnDept", SqlDbType.Int).Value = lnDept;
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tnYears", SqlDbType.Int).Value = lnYears;
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tnSal", SqlDbType.Decimal).Value = lnSal;


        //        //pageframe 1, page 4 details
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tlnRegFee", SqlDbType.Decimal).Value = gnRegistrationFee;
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tlnSharePrice", SqlDbType.Decimal).Value = lnSharesp;
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tlnShares", SqlDbType.Int).Value = lnShares;
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tlnSaveAmt", SqlDbType.Decimal).Value = lnSaveAmt;
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tlnSaveType", SqlDbType.Bit).Value = llSaveType;
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tlcSignatory", SqlDbType.VarChar).Value = lcSignatory;
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tlmodepay", SqlDbType.Bit).Value = llmodepay;
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tlcommpid", SqlDbType.Int).Value = globalvar.gnCompid;
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tbranid", SqlDbType.Int).Value = Convert.ToInt32(comboBox1.SelectedValue);
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tbatid", SqlDbType.Int).Value = Convert.ToInt32(comboBox15.SelectedValue);

        //        //pageframe 1, page biometric details
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tbmemPict", SqlDbType.Image).Value = ConvertImageToBinary(pictureBox1.Image);
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tbmemsign", SqlDbType.Image).Value = ConvertImageToBinary(pictureBox4.Image);

        //        cusAmdcommand.UpdateCommand.Parameters.Add("@levelofedu", SqlDbType.Int).Value = lnLevelOfedu;
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@tribe", SqlDbType.Int).Value = lnTribe;
        //        cusAmdcommand.UpdateCommand.Parameters.Add("@povertylevel", SqlDbType.Int).Value = lnPovertyLevel;


        //        ndConnHandle1.Open();
        //        cuscommand.InsertCommand.ExecuteNonQuery();  //Insert new record
        //        ndConnHandle1.Close();

        //        //                if (llMobileWallet)                           //update record in mobile database 
        //        //                {
        //        ////                    string WalletSource = "SQL5081.site4now.net";
        //        //                    string cos = "Data Source=SQL5081.site4now.net;Initial Catalog=DB_A45989_Walleto;User Id=DB_A45989_Walleto_admin;Password=Kuyateh@k13";
        //        //                    using (SqlConnection ndWalcon = new SqlConnection(cos))
        //        //                    {

        //        //                        if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
        //        //                        {
        //        //                                         ndWalcon.Open();
        //        //                            //Mobile Wallet information 
        //        //                            string lcPino = "1234567";
        //        //                            string lcLoginCode = ncompid.ToString().Trim().PadLeft(3, '0') + gcMembCode;
        //        //                            string cquery1 = "insert into cusreg (ccustcode,ccustfname,ccustmname,ccustlname,ccusttitle,natcode,ddatebirth,datejoin,gender,marital,idtype,cpassno,nregion,ndist,nward,residents,cou_id,";
        //        //                            cquery1 += "ncity,cstreet,ctel,cemail,pinnumber,compid,branch_id,logincode )";
        //        //                            cquery1 += "values ";
        //        //                            cquery1 += "(@MemCode,@lcFname,@lcMname,@lcLname,@lnTitle,@lnNatCode,@ldDOB,@ldJoin,@lnmale,@marital,@idtype,@lcIdNumb,@lnReg,@lnDis,@nWard,@lnRes,@lnCou,";
        //        //                            cquery1 += "@lnCity,@cStreet,@cTel,@cEmail,@lcPin,@tnCompid,@tnBranch,@lcLoginCode)";


        //        //                            SqlDataAdapter mobcommand = new SqlDataAdapter();
        //        //                            mobcommand.InsertCommand = new SqlCommand(cquery1, ndWalcon);

        //        //                            mobcommand.InsertCommand.Parameters.Add("@MemCode", SqlDbType.VarChar).Value = gcMembCode;
        //        //                            mobcommand.InsertCommand.Parameters.Add("@lcFname", SqlDbType.VarChar).Value = lcFname;
        //        //                            mobcommand.InsertCommand.Parameters.Add("@lcMname", SqlDbType.VarChar).Value = lcMname;
        //        //                            mobcommand.InsertCommand.Parameters.Add("@lcLname", SqlDbType.VarChar).Value = lcLname;

        //        //                            mobcommand.InsertCommand.Parameters.Add("@lnTitle", SqlDbType.Int).Value = lnTitle;
        //        //                            mobcommand.InsertCommand.Parameters.Add("@lnNatCode", SqlDbType.Int).Value = lnNatCode;
        //        //                            mobcommand.InsertCommand.Parameters.Add("@ldDOB", SqlDbType.DateTime).Value = ldDOB;
        //        //                            mobcommand.InsertCommand.Parameters.Add("@ldJoin", SqlDbType.DateTime).Value = ldJoin;
        //        //                            mobcommand.InsertCommand.Parameters.Add("@lnmale", SqlDbType.Bit).Value = llMale;
        //        //                            mobcommand.InsertCommand.Parameters.Add("@marital", SqlDbType.Int).Value = lnMarital;
        //        //                            mobcommand.InsertCommand.Parameters.Add("@idtype", SqlDbType.Int).Value = lnIDtype;
        //        //                            mobcommand.InsertCommand.Parameters.Add("@lcIdNumb", SqlDbType.VarChar).Value = lcIDNumb;
        //        //                            mobcommand.InsertCommand.Parameters.Add("@lnReg", SqlDbType.Int).Value = lnRegion;
        //        //                            mobcommand.InsertCommand.Parameters.Add("@lnDis", SqlDbType.Int).Value = lnDistrict;

        //        //                            mobcommand.InsertCommand.Parameters.Add("@nWard", SqlDbType.Int).Value = lnWard;
        //        //                            mobcommand.InsertCommand.Parameters.Add("@lnRes", SqlDbType.Bit).Value = llRes;
        //        //                            mobcommand.InsertCommand.Parameters.Add("@lnCou", SqlDbType.Int).Value = lnCou;
        //        //                            mobcommand.InsertCommand.Parameters.Add("@lnCity", SqlDbType.Int).Value = lnCity;
        //        //                            mobcommand.InsertCommand.Parameters.Add("@cStreet", SqlDbType.VarChar).Value = lcStreet;
        //        //                            mobcommand.InsertCommand.Parameters.Add("@cTel", SqlDbType.VarChar).Value = lcTel;
        //        //                            mobcommand.InsertCommand.Parameters.Add("@cEmail", SqlDbType.VarChar).Value = lcEmail;
        //        //                            mobcommand.InsertCommand.Parameters.Add("@lcPin", SqlDbType.VarChar).Value = lcPino; ;
        //        //                            mobcommand.InsertCommand.Parameters.Add("@tnCompid", SqlDbType.Int).Value = globalvar.gnCompid;
        //        //                            mobcommand.InsertCommand.Parameters.Add("@tnBranch", SqlDbType.Int).Value = Convert.ToInt32(comboBox1.SelectedValue);
        //        //                            mobcommand.InsertCommand.Parameters.Add("@lcLoginCode", SqlDbType.VarChar).Value = lcLoginCode;

        //        //                            try
        //        //                            {
        //        //                                mobcommand.InsertCommand.ExecuteNonQuery();  //Insert new record
        //        //                            }
        //        //                            catch (Exception ex)
        //        //                            {
        //        //                                MessageBox.Show(ex.Message);
        //        //                            }
        //        //                            ndWalcon.Close();
        //        //                        }
        //        //                        else
        //        //                        {
        //        //                            MessageBox.Show("The network is not available for now, Please use Mobile Wallet interface to add member");
        //        //                            //Do stuffs when network not available
        //        //                        }
        //        //                    }
        //        //                }

        //        if (mandview.Rows.Count > 0)
        //        {
        //            for (int j = 0; j < mandview.Rows.Count; j++)
        //            {
        //                string lcMemCode1 = GetClient_Code.clientCode_int(cs, "clientid").ToString().Trim().PadLeft(6, '0');
        //                string lcProductControl = mandview.Rows[j]["prod_control"].ToString().Trim();
        //                string lcAcode = lcProductControl.Substring(0, 3);
        //                lnPrd_ID = Convert.ToInt16(mandview.Rows[j]["prd_id"]);
        //                string lcAcctNumb = lcAcode + lcMemCode1 + "01";

        //                gcNonInterestAcct = mandview.Rows[j]["nint_inc"].ToString().Trim();
        //                // createAccount(lcAcctNumb);

        //                string auditDesc = "Individual Member Creation " + lcMemCode1;// textBox1.Text.Trim() + " " + tcAcctNumb;// "Loan Disbursement Completed";
        //                AuditTrail au = new AuditTrail();
        //                au.upd_audit_trail(cs, auditDesc, 0.00m, 0.00m, globalvar.gcUserid, "C", "", lcMemCode1, "", "", globalvar.gcWorkStation, globalvar.gcWinUser);

        //                if (lcAcode == "250" || lcAcode == "251")
        //                {
        //                    createAccount(lcAcctNumb);
        //                    decimal tnPostAmt = gnRegistrationFee;
        //                    gcMemberSavingsAcct = lcAcctNumb;
        //                    gcSavingsControlAcct = lcProductControl;
        //                    postAccounts(tnPostAmt, lcAcode); //registration fee
        //                }
        //                if (lcAcode == "270" || lcAcode == "271")
        //                {
        //                    createAccount(lcAcctNumb);
        //                    decimal tnPostAmt = gnSharePrice;
        //                    gcMemberSharesAcct = lcAcctNumb;
        //                    gcSharesControlAcct = lcProductControl;
        //                    postAccounts(tnPostAmt, lcAcode); //registration fee
        //                }
        //                initvariables();
        //                textBox1.Focus();
        //                IndSaveButton.Enabled = false;
        //                IndSaveButton.BackColor = Color.Gainsboro;
        //                lcMemCode1 = "";
        //            }
        //            updateCustomerCode();
        //        }

        //}
        //}

        private void postJoiningFee()
        {
            string tcContra = globalvar.gcIntSuspense;
            string tcUserid = globalvar.gcUserid;
            int tncompid = globalvar.gnCompid;
            string tcDesc = string.Empty;
            string tcCustcode = textBox46.Text.ToString();
            decimal tnTranAmt = -Math.Abs(gnRegistrationFee);
            decimal tnContAmt = Math.Abs(gnRegistrationFee);
            string tcVoucher = CuVoucher.genCuVoucher(cs, globalvar.gdSysDate);
            //decimal unitprice = Math.Abs(nTranAmt);
            string tcChqno = "000001";
            decimal lnWaiveAmt = 0.00m;
            string tcTranCode = "15";
            int lnServID = 0;
            bool llPaid = false;
            int tnqty = 1;
            string tcReceipt = "";
            bool llCashpay = true;
            int visno = 1;
            bool isproduct = false;
            int srvid = globalvar.gnBranchid;
            bool lFreeBee = false;
            updateGlmast gls = new updateGlmast();
            updateDailyBalance dbal = new updateDailyBalance();
            updateCuTranhist tls1 = new updateCuTranhist();
            AuditTrail au = new AuditTrail();
            updateJournal upj = new updateJournal();
            updateClient_Code updcl = new updateClient_Code();


            //************************************The posting should follow the processes detailed below
            //if (tcAcode == "")// ************* for joining fees which will be deducted from the member's savings account and credited to an income account
            //{
            //Member's savings account - debit 
            tcDesc = "Joining Fee";
                string auditDesc = string.Empty;
                gls.updGlmast(cs, gcMemberSavingsAcct, tnTranAmt);                                                                       //update member savings account glmast 
                decimal tnPNewBal = CheckLastBalance.lastbalance(cs, gcMemberSavingsAcct);
                tls1.updCuTranhist(cs, gcMemberSavingsAcct, tnTranAmt, tcDesc, tcVoucher, tcChqno, tcUserid, tnPNewBal, tcTranCode, lnServID, llPaid, tcContra, lnWaiveAmt, tnqty, tnTranAmt, tcReceipt, llCashpay, visno, isproduct,
                srvid, "", "", lFreeBee, tcCustcode, tncompid, globalvar.gnBranchid, globalvar.gnCurrCode, globalvar.gdSysDate, globalvar.gdSysDate);                          //update tranhist posting account

            //savings product control account and send for verification - debit 
                auditDesc = "Joining Fee -> " + gcSavingsControlAcct;
                au.upd_audit_trail(cs, auditDesc, 0.00m, tnTranAmt, globalvar.gcUserid, "U", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);
  //              dbal.updDayBal(cs, globalvar.gdSysDate, gcSavingsControlAcct,tnTranAmt, globalvar.gnBranchid, globalvar.gnCompid);      //update daybal for savings control account  
                upj.updJournal(cs, gcSavingsControlAcct, tcDesc, tnTranAmt, tcVoucher, tcTranCode, globalvar.gdSysDate, globalvar.gcUserid, globalvar.gnCompid);
                updcl.updClient(cs, "nvoucherno");

                // joining fee defined in company details  - credit
//                MessageBox.Show("The joining fee account = " + gcJoinFeeAcct);
                auditDesc = "Joining Fee -> " + gcJoinFeeAcct;
                au.upd_audit_trail(cs, auditDesc, 0.00m, Math.Abs(tnTranAmt), globalvar.gcUserid, "U", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);
                upj.updJournal(cs, gcJoinFeeAcct, tcDesc, Math.Abs(tnTranAmt), tcVoucher, tcTranCode, globalvar.gdSysDate, globalvar.gcUserid, globalvar.gnCompid);
                updcl.updClient(cs, "nvoucherno");
            //}
            //else
            //{
                ////**********for share values

                //tcDesc = "Shares Purchase";
                //// debit member savings account
                //string auditDesc = string.Empty;
                //gls.updGlmast(cs, gcMemberSavingsAcct, tnTranAmt);
                //dbal.updDayBal(cs, globalvar.gdSysDate, gcMemberSavingsAcct, tnTranAmt, globalvar.gnBranchid, globalvar.gnCompid);
                //decimal tnPNewBal = CheckLastBalance.lastbalance(cs, gcMemberSavingsAcct);
                //tls1.updCuTranhist(cs, gcMemberSavingsAcct, tnTranAmt, tcDesc, tcVoucher, tcChqno, tcUserid, tnPNewBal, tcTranCode, lnServID, llPaid, gcMemberSharesAcct, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, visno, isproduct,
                //srvid, "", "", lFreeBee, tcCustcode, tncompid, globalvar.gnBranchid, globalvar.gnCurrCode, globalvar.gdSysDate, globalvar.gdSysDate);                          //update tranhist posting account

                //// debit savings product control account and send for verification
                //auditDesc = "Shares values -> " + gcSavingsControlAcct;
                //au.upd_audit_trail(cs, auditDesc, 0.00m, tnTranAmt, globalvar.gcUserid, "U", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);
                //upj.updJournal(cs, gcSavingsControlAcct, tcDesc, tnTranAmt, tcVoucher, tcTranCode, globalvar.gdSysDate, globalvar.gcUserid, globalvar.gnCompid);
                //updcl.updClient(cs, "nvoucherno");

                //// credit member shares account
                //gls.updGlmast(cs, gcMemberSharesAcct, tnContAmt);
                //dbal.updDayBal(cs, globalvar.gdSysDate, gcMemberSharesAcct, Math.Abs(tnTranAmt), globalvar.gnBranchid, globalvar.gnCompid);
                //tnPNewBal = CheckLastBalance.lastbalance(cs, gcMemberSharesAcct);
                //tls1.updCuTranhist(cs, gcMemberSharesAcct, Math.Abs(tnTranAmt), tcDesc, tcVoucher, tcChqno, tcUserid, tnPNewBal, tcTranCode, lnServID, llPaid, gcMemberSavingsAcct, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, visno, isproduct,
                //srvid, "", "", lFreeBee, tcCustcode, tncompid, globalvar.gnBranchid, globalvar.gnCurrCode, globalvar.gdSysDate, globalvar.gdSysDate);                          //update tranhist posting account

                //// credit shares product control account and send for verification
                //auditDesc = "Shares values -> " + gcSharesControlAcct;
                //au.upd_audit_trail(cs, auditDesc, 0.00m, Math.Abs(tnTranAmt), globalvar.gcUserid, "U", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);
                //upj.updJournal(cs, gcSharesControlAcct, tcDesc, Math.Abs(tnTranAmt), tcVoucher, tcTranCode, globalvar.gdSysDate, globalvar.gcUserid, globalvar.gnCompid);
                //updcl.updClient(cs, "nvoucherno");
            //}
        }


    }
}

