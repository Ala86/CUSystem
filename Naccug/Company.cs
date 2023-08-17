using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using TclassLibrary;

namespace TclassLibrary
{
    public partial class Company : Form
    {
        int gnCompanyCode = 0;
        string gcInputStat = "N";
        DataTable compview = new DataTable();

        string cs = string.Empty;
        int ncompid = 0;
        string dloca = string.Empty;

        public Company(string tcCos, int tnCompid, string tcLoca)
        {
            InitializeComponent();
            cs = tcCos;
            ncompid = tnCompid;
            dloca = tcLoca;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Company_Load(object sender, EventArgs e)
        {
            this.Text = dloca + "<< Credit Union Setup >>";
            getcompany();
            shaFrequency();
            shaproFrequency();
            feeFrequency();
            feeproFrequency();
        }


        private void getcompany()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                ///get company details 
                string dsql0 = "exec tsp_getCompanies";
                SqlDataAdapter da0 = new SqlDataAdapter(dsql0, ndConnHandle);
                //                DataTable ds0 = new DataTable();
                da0.Fill(compview);
                if (compview != null)
                {
                    comboBox1.DataSource = compview.DefaultView;
                    comboBox1.DisplayMember = "com_name";
                    comboBox1.ValueMember = "compid";
                    comboBox1.SelectedIndex = -1;
                }

                //************Getting product type                
                string dsqlp2 = "select prd_id,prd_name from prodtype ";
                SqlDataAdapter dap2 = new SqlDataAdapter(dsqlp2, ndConnHandle);
                DataTable dsp2 = new DataTable();
                dap2.Fill(dsp2);
                if (dsp2 != null)
                {
                    comboBox3.DataSource = dsp2.DefaultView;
                    comboBox3.DisplayMember = "prd_name";
                    comboBox3.ValueMember = "prd_id";
                    comboBox3.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not find product types, inform IT Dept immediately"); }


                //************Getting country
                string dsql1 = "exec tsp_GetCountry  ";
                SqlDataAdapter da1 = new SqlDataAdapter(dsql1, ndConnHandle);
                DataTable ds1 = new DataTable();
                da1.Fill(ds1);
                if (ds1 != null)
                {
                    comboBox7.DataSource = ds1.DefaultView;
                    comboBox7.DisplayMember = "cou_name";
                    comboBox7.ValueMember = "cou_id";
                    comboBox7.SelectedIndex = -1;
                }


                string secsql = "select sec_name,sec_id from sector ";
                SqlDataAdapter das = new SqlDataAdapter(secsql, ndConnHandle);
                DataTable sectab = new DataTable();
                das.Fill(sectab);
                if (sectab != null)
                {
                    comboBox4.DataSource = sectab.DefaultView;
                    comboBox4.DisplayMember = "sec_name";
                    comboBox4.ValueMember = "sec_id";
                    comboBox4.SelectedIndex = -1;
                }


                string lansql = "select lan_name,lan_id from lang_tp ";
                SqlDataAdapter dalan = new SqlDataAdapter(lansql, ndConnHandle);
                DataTable lantab = new DataTable();
                dalan.Fill(lantab);
                if (lantab != null)
                {
                    comboBox5.DataSource = lantab.DefaultView;
                    comboBox5.DisplayMember = "lan_name";
                    comboBox5.ValueMember = "lan_id";
                    comboBox5.SelectedIndex = -1;
                }


                string cursql = "select curr_name, curr_code  from ccurrency ";
                SqlDataAdapter dacur = new SqlDataAdapter(cursql, ndConnHandle);
                DataTable curtab = new DataTable();
                dacur.Fill(curtab);
                if (curtab != null)
                {
                    comboBox6.DataSource = curtab.DefaultView;
                    comboBox6.DisplayMember = "curr_name";
                    comboBox6.ValueMember = "curr_code";
                    comboBox6.SelectedIndex = -1;
                }


                string savesql = "exec tsp_SaveControls " + ncompid;
                SqlDataAdapter dasave = new SqlDataAdapter(savesql, ndConnHandle);
                DataTable savetab = new DataTable();
                DataTable savetab1 = new DataTable();
                DataTable savetab2 = new DataTable();
                DataTable savetab3 = new DataTable();

                dasave.Fill(savetab);
                dasave.Fill(savetab1);
                dasave.Fill(savetab2);
                dasave.Fill(savetab3);

                if (savetab != null)
                {
                    comboBox20.DataSource = savetab.DefaultView;
                    comboBox20.DisplayMember = "cacctname";
                    comboBox20.ValueMember = "cacctnumb";
                    comboBox20.SelectedIndex = -1;
                }


                if (savetab1 != null)
                {
                    comboBox22.DataSource = savetab1.DefaultView;
                    comboBox22.DisplayMember = "cacctname";
                    comboBox22.ValueMember = "cacctnumb";
                    comboBox22.SelectedIndex = -1;
                }

                if (savetab2 != null)
                {
                    comboBox24.DataSource = savetab2.DefaultView;
                    comboBox24.DisplayMember = "cacctname";
                    comboBox24.ValueMember = "cacctnumb";
                    comboBox24.SelectedIndex = -1;
                }

                if (savetab3 != null)
                {
                    comboBox26.DataSource = savetab3.DefaultView;
                    comboBox26.DisplayMember = "cacctname";
                    comboBox26.ValueMember = "cacctnumb";
                    comboBox26.SelectedIndex = -1;
                }



                string loansql = "exec tsp_ShareControls " + ncompid;
                SqlDataAdapter daloan = new SqlDataAdapter(loansql, ndConnHandle);
                DataTable loantab = new DataTable();
                DataTable loantab1 = new DataTable();
                DataTable loantab2 = new DataTable();
                DataTable loantab3 = new DataTable();

                daloan.Fill(loantab);
                daloan.Fill(loantab1);
                daloan.Fill(loantab2);
                daloan.Fill(loantab3);

                if (loantab != null)
                {
                    comboBox21.DataSource = loantab.DefaultView;
                    comboBox21.DisplayMember = "cacctname";
                    comboBox21.ValueMember = "cacctnumb";
                    comboBox21.SelectedIndex = -1;
                }


                if (loantab1 != null)
                {
                    comboBox23.DataSource = loantab1.DefaultView;
                    comboBox23.DisplayMember = "cacctname";
                    comboBox23.ValueMember = "cacctnumb";
                    comboBox23.SelectedIndex = -1;
                }

                if (loantab2 != null)
                {
                    comboBox25.DataSource = loantab2.DefaultView;
                    comboBox25.DisplayMember = "cacctname";
                    comboBox25.ValueMember = "cacctnumb";
                    comboBox25.SelectedIndex = -1;
                }

                if (loantab3 != null)
                {
                    comboBox27.DataSource = loantab3.DefaultView;
                    comboBox27.DisplayMember = "cacctname";
                    comboBox27.ValueMember = "cacctnumb";
                    comboBox27.SelectedIndex = -1;
                }


            }
        }

        private void getcity(int couid)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string dsql7 = "exec tsp_GetCity " + couid;
                SqlDataAdapter da7 = new SqlDataAdapter(dsql7, ndConnHandle);
                DataTable ds7 = new DataTable();
                da7.Fill(ds7);
                if (ds7 != null)
                {
                    comboBox2.DataSource = ds7.DefaultView;
                    comboBox2.DisplayMember = "city_name";
                    comboBox2.ValueMember = "city_id";
                    comboBox2.SelectedIndex = -1;
                }
            }
        }

        private void shaFrequency()
        {
            string[] mones0 = new string[7];
            mones0[0] = "Daily";
            mones0[1] = "Weekly";
            mones0[2] = "Fortnight";
            mones0[3] = "Monthly";
            mones0[4] = "Quarterly";
            mones0[5] = "Half-Yearly";
            mones0[6] = "Yearly";

            comboBox12.DataSource = mones0;
            comboBox12.SelectedIndex = -1;
        }

        private void shaproFrequency()
        {
            string[] mones1 = new string[7];
            mones1[0] = "Daily";
            mones1[1] = "Weekly";
            mones1[2] = "Fortnight";
            mones1[3] = "Monthly";
            mones1[4] = "Quarterly";
            mones1[5] = "Half-Yearly";
            mones1[6] = "Yearly";
            comboBox11.DataSource = mones1;
            comboBox11.SelectedIndex = -1;
        }




        private void feeFrequency()
        {
            string[] mones2 = new string[8];
            mones2[0] = "Daily";
            mones2[1] = "Weekly";
            mones2[2] = "Fortnight";
            mones2[3] = "Monthly";
            mones2[4] = "Quarterly";
            mones2[5] = "Half-Yearly";
            mones2[6] = "Yearly";

            comboBox8.DataSource = mones2;
            comboBox8.SelectedIndex = -1;
        }

        private void feeproFrequency()
        {
            string[] mones3 = new string[8];
            mones3[0] = "Daily";
            mones3[1] = "Weekly";
            mones3[2] = "Fortnight";
            mones3[3] = "Monthly";
            mones3[4] = "Quarterly";
            mones3[5] = "Half-Yearly";
            mones3[6] = "Yearly";

            comboBox10.DataSource = mones3;
            comboBox10.SelectedIndex = -1;

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
            //          bool lallc = textBox1.Text.Trim() != "" && richTextBox1.Text.Trim() != "" ? true : false;
            //        MessageBox.Show("status is " + lallc);
            bool lnewComp = gcInputStat == "N" && textBox1.Text != "" ? true : false;
            bool loldComp = gcInputStat != "N" && Convert.ToInt32(comboBox5.SelectedValue) > 0 ? true : false;

            if (lnewComp || loldComp && richTextBox1.Text != "" && Convert.ToInt32(comboBox7.SelectedValue) > 0 && Convert.ToInt32(comboBox2.SelectedValue) > 0 &&
               Convert.ToInt32(comboBox4.SelectedValue) > 0 && Convert.ToInt32(comboBox5.SelectedValue) > 0 && Convert.ToInt32(comboBox6.SelectedValue) > 0 &&
                textBox3.Text != "" && textBox4.Text != "")
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

        private void companies()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                compview.Clear();
                ndConnHandle.Open();
                string dsql0 = "exec tsp_getCompanies";
                SqlDataAdapter da0 = new SqlDataAdapter(dsql0, ndConnHandle);
                da0.Fill(compview);
                if (compview != null)
                {
                    comboBox1.DataSource = compview.DefaultView;
                    comboBox1.DisplayMember = "com_name";
                    comboBox1.ValueMember = "compid";
                    comboBox1.SelectedIndex = -1;
                }
            }
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void comboBox14_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox13_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox12_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void initvariables()
        {
            textBox1.Text = "";
            richTextBox1.Text = "";
            textBox1.Text = "";
            comboBox7.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            textBox3.Text = textBox4.Text = textBox6.Text = textBox15.Text = textBox16.Text = textBox5.Text = textBox10.Text = textBox11.Text = textBox9.Text = textBox12.Text = 
            textBox8.Text = "";

            checkBox1.Checked = checkBox2.Checked = checkBox3.Checked = checkBox4.Checked = checkBox5.Checked = checkBox6.Checked = checkBox7.Checked = 
            checkBox8.Checked = radioButton1.Checked = radioButton2.Checked = radioButton3.Checked = saveButton.Enabled = updateButton.Enabled = false;

            saveButton.BackColor = Color.Gainsboro;
            updateButton.BackColor = Color.Gainsboro;
            comboBox20.SelectedIndex = comboBox21.SelectedIndex = comboBox22.SelectedIndex = comboBox23.SelectedIndex = comboBox24.SelectedIndex = 
            comboBox25.SelectedIndex = comboBox26.SelectedIndex = comboBox27.SelectedIndex = comboBox4.SelectedIndex = comboBox5.SelectedIndex = comboBox6.SelectedIndex = -1;
            comboBox1.Enabled = false;
            companies();
            textBox1.Focus();
        }

        private void insertcompany()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string tcAcctNumb = globalvar.ClientAcctPrefix + textBox5.Text.ToString();

                int lcutype = (radioButton1.Checked ? 1 : (radioButton2.Checked ? 2 : 3));
                decimal lnCorpTax = textBox8.Text != "" ? Convert.ToDecimal(textBox8.Text) : 0.00m;
                bool lbio = checkBox8.Checked;
                int lRetAge = textBox9.Text != "" ? Convert.ToInt32(textBox9.Text) : 0;
                //                MessageBox.Show("retirement age is " + lRetAge );
                string cquery = "Insert Into company (com_name,caddress,cou_id,city_id,tel,email,com_logo,curr_id,lang,pin,sec_type,";
                cquery += "wkd1,wkd2,wkd3,wkd4,wkd5,wkd6,wkd7,corp_Tax,cutype,biolink,retireage,guarPercent,prd_ID,chargeoffloor,annual_dues,feeFrequency,runFrequency,shfreq,shrunfreq)";
                cquery += " values ";
                cquery += "(@lcompname,@laddr,@lcouid,@lcityid,@ltel,@lemail,@lclogo,@lcurrid,@llangid,@llcpin,@lsectype,";
                cquery += "@llnwkd1,@llnwkd2,@llnwkd3,@llnwkd4,@llnwkd5,@llnwkd6,@llnwkd7,@llnCorpTax,@lcutype,@llnBioLink,@llnRetireAge,@lguarPercent,@lprd_id,@chargeoff,@tannual_dues,";
                cquery += "@tfeeFrequency,@trunFrequency,@tshaFrequency,@tsharunFrequency)";

                SqlDataAdapter comins = new SqlDataAdapter();
                comins.InsertCommand = new SqlCommand(cquery, ndConnHandle);

                ndConnHandle.Open();

                comins.InsertCommand.Parameters.Add("@lcompname", SqlDbType.VarChar).Value = textBox1.Text.ToString();
                comins.InsertCommand.Parameters.Add("@laddr", SqlDbType.VarChar).Value = richTextBox1.Text.ToString();
                comins.InsertCommand.Parameters.Add("@lcouid", SqlDbType.Int).Value = Convert.ToInt32(comboBox7.SelectedValue);
                comins.InsertCommand.Parameters.Add("@lcityid", SqlDbType.Int).Value = Convert.ToInt32(comboBox2.SelectedValue);
                comins.InsertCommand.Parameters.Add("@ltel", SqlDbType.VarChar).Value = textBox3.Text;
                comins.InsertCommand.Parameters.Add("@lemail", SqlDbType.VarChar).Value = textBox4.Text;
                comins.InsertCommand.Parameters.Add("@lclogo", SqlDbType.VarChar).Value = textBox12.Text;
                comins.InsertCommand.Parameters.Add("@lcurrid", SqlDbType.Int).Value = Convert.ToInt32(comboBox6.SelectedValue);
                comins.InsertCommand.Parameters.Add("@llangid", SqlDbType.Int).Value = Convert.ToInt32(comboBox5.SelectedValue);
                comins.InsertCommand.Parameters.Add("@llcpin", SqlDbType.VarChar).Value = textBox6.Text;
                comins.InsertCommand.Parameters.Add("@lsectype", SqlDbType.Int).Value = Convert.ToInt32(comboBox4.SelectedValue);

                comins.InsertCommand.Parameters.Add("@llnwkd1", SqlDbType.Bit).Value = Convert.ToBoolean(checkBox1.Checked);
                comins.InsertCommand.Parameters.Add("@llnwkd2", SqlDbType.Bit).Value = Convert.ToBoolean(checkBox2.Checked);
                comins.InsertCommand.Parameters.Add("@llnwkd3", SqlDbType.Bit).Value = Convert.ToBoolean(checkBox3.Checked);
                comins.InsertCommand.Parameters.Add("@llnwkd4", SqlDbType.Bit).Value = Convert.ToBoolean(checkBox4.Checked);
                comins.InsertCommand.Parameters.Add("@llnwkd5", SqlDbType.Bit).Value = Convert.ToBoolean(checkBox5.Checked);
                comins.InsertCommand.Parameters.Add("@llnwkd6", SqlDbType.Bit).Value = Convert.ToBoolean(checkBox6.Checked);
                comins.InsertCommand.Parameters.Add("@llnwkd7", SqlDbType.Bit).Value = Convert.ToBoolean(checkBox7.Checked);
                comins.InsertCommand.Parameters.Add("@llnCorpTax", SqlDbType.Decimal).Value = lnCorpTax;
                comins.InsertCommand.Parameters.Add("@lcutype", SqlDbType.Int).Value = lcutype;
                comins.InsertCommand.Parameters.Add("@llnBioLink", SqlDbType.Bit).Value = lbio;
                comins.InsertCommand.Parameters.Add("@llnRetireAge", SqlDbType.Int).Value = lRetAge;
                comins.InsertCommand.Parameters.Add("@lguarPercent", SqlDbType.Int).Value = globalvar.gnGuarPercent;
                comins.InsertCommand.Parameters.Add("@lprd_id", SqlDbType.Int).Value = Convert.ToInt16(comboBox3.SelectedValue);
                comins.InsertCommand.Parameters.Add("@chargeoff", SqlDbType.Int).Value = Convert.ToInt16(textBox7.Text);
                comins.InsertCommand.Parameters.Add("@tannual_dues", SqlDbType.Decimal).Value = Convert.ToDecimal(textBox14.Text);
                comins.InsertCommand.Parameters.Add("@tfeeFrequency", SqlDbType.Int).Value = Convert.ToInt16(comboBox8.SelectedIndex);
                comins.InsertCommand.Parameters.Add("@trunFrequency", SqlDbType.Int).Value = Convert.ToInt16(comboBox10.SelectedIndex);
                comins.InsertCommand.Parameters.Add("@tshaFrequency", SqlDbType.Int).Value = Convert.ToInt16(comboBox12.SelectedIndex);
                comins.InsertCommand.Parameters.Add("@tsharunFrequency", SqlDbType.Int).Value = Convert.ToInt16(comboBox11.SelectedIndex);


                comins.InsertCommand.ExecuteNonQuery();
                ndConnHandle.Close();
            }
        }

        private void updatecompany()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                MessageBox.Show("Going to update company details");

                int lcutype = (radioButton1.Checked ? 1 : (radioButton2.Checked ? 2 : 3));
                decimal lnCorpTax = textBox8.Text != "" ? Convert.ToDecimal(textBox8.Text) : 0.00m;
                bool lbio = checkBox8.Checked;
                int lRetAge = textBox9.Text != "" ? Convert.ToInt32(textBox9.Text) : 0;
                string lcCompName = textBox1.Text.ToString().Trim();
                //                MessageBox.Show(" company name is " + lcCompName);


                string cquery1 = "update company set  com_name=@lcompname,caddress=@laddr,cou_id=@lcouid,city_id=@lcityid,tel=@ltel,email=@lemail,com_logo=@lclogo,curr_id=@lcurrid,lang=@llangid,pin=@llcpin,sec_type=@lsectype,";
                cquery1 += "wkd1=@llnwkd1,wkd2=@llnwkd2,wkd3=@llnwkd3,wkd4=@llnwkd4,wkd5=@llnwkd5,wkd6=@llnwkd6,wkd7=@llnwkd7,biolink=@llnBioLink,retireage=@llnRetireAge,corp_Tax=@llnCorpTax,cutype=@lcutype,";
                cquery1 += "savectrl_ind=@lsavectrl_ind,savectrl_cor=@lsavectrl_cor,savectrl_grp=@lsavectrl_grp,savectrl_stf=@lsavectrl_stf, sharectrl_ind=@lsharectrl_ind, sharectrl_cor=@lsharectrl_cor, sharectrl_grp=@lsharectrl_grp,";
                cquery1 += "sharectrl_stf =@lsharectrl_stf,guarPercent=@lguarPercent,prd_id=@lprd_id,chargeoffloor = @chargeoff, writeoffloor=@writeoffloor,annual_dues=@tannual_dues,feeFrequency=@tfeeFrequency,runFrequency=@trunFrequency,";
                cquery1 += "regAcct = @tcregAcct,shFreq=@tshaFrequency,shrunFreq=@tsharunFrequency where compid =@lgnCompanyCode";

                SqlDataAdapter comupd = new SqlDataAdapter();
                comupd.UpdateCommand = new SqlCommand(cquery1, ndConnHandle);

                ndConnHandle.Open();

                comupd.UpdateCommand.Parameters.Add("@lcompname", SqlDbType.VarChar).Value = lcCompName;
                comupd.UpdateCommand.Parameters.Add("@laddr", SqlDbType.VarChar).Value = richTextBox1.Text.ToString();
                comupd.UpdateCommand.Parameters.Add("@lcouid", SqlDbType.Int).Value = Convert.ToInt32(comboBox7.SelectedValue);
                comupd.UpdateCommand.Parameters.Add("@lcityid", SqlDbType.Int).Value = Convert.ToInt32(comboBox2.SelectedValue);
                comupd.UpdateCommand.Parameters.Add("@ltel", SqlDbType.VarChar).Value = textBox3.Text;
                comupd.UpdateCommand.Parameters.Add("@lemail", SqlDbType.VarChar).Value = textBox4.Text;
                comupd.UpdateCommand.Parameters.Add("@lclogo", SqlDbType.VarChar).Value = textBox12.Text;
                comupd.UpdateCommand.Parameters.Add("@lcurrid", SqlDbType.Int).Value = Convert.ToInt32(comboBox6.SelectedValue);
                comupd.UpdateCommand.Parameters.Add("@llangid", SqlDbType.Int).Value = Convert.ToInt32(comboBox5.SelectedValue);
                comupd.UpdateCommand.Parameters.Add("@llcpin", SqlDbType.VarChar).Value = textBox6.Text;
                comupd.UpdateCommand.Parameters.Add("@lsectype", SqlDbType.Int).Value = Convert.ToInt32(comboBox4.SelectedValue);

                comupd.UpdateCommand.Parameters.Add("@llnwkd1", SqlDbType.Bit).Value = Convert.ToBoolean(checkBox1.Checked);
                comupd.UpdateCommand.Parameters.Add("@llnwkd2", SqlDbType.Bit).Value = Convert.ToBoolean(checkBox2.Checked);
                comupd.UpdateCommand.Parameters.Add("@llnwkd3", SqlDbType.Bit).Value = Convert.ToBoolean(checkBox3.Checked);
                comupd.UpdateCommand.Parameters.Add("@llnwkd4", SqlDbType.Bit).Value = Convert.ToBoolean(checkBox4.Checked);
                comupd.UpdateCommand.Parameters.Add("@llnwkd5", SqlDbType.Bit).Value = Convert.ToBoolean(checkBox5.Checked);
                comupd.UpdateCommand.Parameters.Add("@llnwkd6", SqlDbType.Bit).Value = Convert.ToBoolean(checkBox6.Checked);
                comupd.UpdateCommand.Parameters.Add("@llnwkd7", SqlDbType.Bit).Value = Convert.ToBoolean(checkBox7.Checked);
                comupd.UpdateCommand.Parameters.Add("@llnCorpTax", SqlDbType.Decimal).Value = lnCorpTax;
                comupd.UpdateCommand.Parameters.Add("@lcutype", SqlDbType.Int).Value = lcutype;
                comupd.UpdateCommand.Parameters.Add("@llnBioLink", SqlDbType.Bit).Value = lbio;
                comupd.UpdateCommand.Parameters.Add("@llnRetireAge", SqlDbType.Int).Value = lRetAge;
                comupd.UpdateCommand.Parameters.Add("@lgnCompanyCode", SqlDbType.Int).Value = gnCompanyCode;
                comupd.UpdateCommand.Parameters.Add("@lguarPercent", SqlDbType.Decimal).Value = globalvar.gnGuarPercent;


                comupd.UpdateCommand.Parameters.Add("@lsavectrl_ind", SqlDbType.VarChar).Value = comboBox20.Text != "" ? comboBox20.SelectedValue.ToString() : "";
                comupd.UpdateCommand.Parameters.Add("@lsavectrl_cor", SqlDbType.VarChar).Value = comboBox22.Text != "" ? comboBox22.SelectedValue.ToString() : "";
                comupd.UpdateCommand.Parameters.Add("@lsavectrl_grp", SqlDbType.VarChar).Value = comboBox24.Text != "" ? comboBox24.SelectedValue.ToString() : "";
                comupd.UpdateCommand.Parameters.Add("@lsavectrl_stf", SqlDbType.VarChar).Value = comboBox26.Text != "" ? comboBox26.SelectedValue.ToString() : "";
                comupd.UpdateCommand.Parameters.Add("@lsharectrl_ind", SqlDbType.VarChar).Value = comboBox21.Text != "" ? comboBox21.SelectedValue.ToString() : "";
                comupd.UpdateCommand.Parameters.Add("@lsharectrl_cor", SqlDbType.VarChar).Value = comboBox23.Text != "" ? comboBox23.SelectedValue.ToString() : "";
                comupd.UpdateCommand.Parameters.Add("@lsharectrl_grp", SqlDbType.VarChar).Value = comboBox25.Text != "" ? comboBox25.SelectedValue.ToString() : "";
                comupd.UpdateCommand.Parameters.Add("@lsharectrl_stf", SqlDbType.VarChar).Value = comboBox27.Text != "" ? comboBox27.SelectedValue.ToString() : "";
                comupd.UpdateCommand.Parameters.Add("@lprd_id", SqlDbType.Int).Value = Convert.ToInt16(comboBox3.SelectedValue);
                comupd.UpdateCommand.Parameters.Add("@chargeoff", SqlDbType.Int).Value = textBox7.Text != "" ? Convert.ToInt16(textBox7.Text) : 0;
                comupd.UpdateCommand.Parameters.Add("@writeoffloor", SqlDbType.Int).Value = textBox13.Text != "" ? Convert.ToInt16(textBox13.Text) : 0;
                comupd.UpdateCommand.Parameters.Add("@tannual_dues", SqlDbType.Decimal).Value = textBox14.Text != "" ? Convert.ToDecimal(textBox14.Text) : 0.00m;
                comupd.UpdateCommand.Parameters.Add("@tfeeFrequency", SqlDbType.Int).Value = Convert.ToInt16(comboBox8.SelectedIndex);
                comupd.UpdateCommand.Parameters.Add("@trunFrequency", SqlDbType.Int).Value = Convert.ToInt16(comboBox10.SelectedIndex);
                comupd.UpdateCommand.Parameters.Add("@tshaFrequency", SqlDbType.Int).Value = Convert.ToInt16(comboBox12.SelectedIndex);
                comupd.UpdateCommand.Parameters.Add("@tsharunFrequency", SqlDbType.Int).Value = Convert.ToInt16(comboBox11.SelectedIndex);

                comupd.UpdateCommand.Parameters.Add("@tcregAcct", SqlDbType.VarChar).Value = textBox16.Text != "" ? textBox16.Text.ToString().Trim() : "";

                
                comupd.UpdateCommand.ExecuteNonQuery();
                ndConnHandle.Close();
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            //        MessageBox.Show("we will save the detatils");
            if (gcInputStat == "N")
            {
                insertcompany();
                initvariables();
            }
            else
            {
                updatecompany();
                initvariables();
            }
        }


        private void button3_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox1.Focused)
            {
                gnCompanyCode = Convert.ToInt32(comboBox1.SelectedValue);
                int rowind = Convert.ToInt16(comboBox1.SelectedIndex);
                textBox1.Text = compview.Rows[rowind]["com_name"].ToString();
                updateButton.Visible = gnCompanyCode > 0 ? true : false;
                richTextBox1.Text = compview.Rows[rowind]["caddress"].ToString();
                comboBox7.SelectedValue = Convert.ToInt32(compview.Rows[rowind]["cou_id"]);
                getcity(Convert.ToInt32(compview.Rows[rowind]["cou_id"]));
                comboBox2.SelectedValue = Convert.ToInt32(compview.Rows[rowind]["city_id"]);
                textBox3.Text = compview.Rows[rowind]["tel"].ToString();
                textBox4.Text = compview.Rows[rowind]["email"].ToString();
                textBox5.Text = compview.Rows[rowind]["minshares"].ToString();
                textBox6.Text = compview.Rows[rowind]["pin"].ToString();
                textBox8.Text = compview.Rows[rowind]["corp_tax"].ToString();
                textBox10.Text = Convert.ToDecimal(compview.Rows[rowind]["shareprice"]).ToString("N2");
                textBox11.Text = Convert.ToDecimal(compview.Rows[rowind]["regfee"]).ToString("N2");
                textBox14.Text = Convert.ToDecimal(compview.Rows[rowind]["annual_dues"]).ToString("N2");
                textBox15.Text = compview.Rows[rowind]["regAcctName"].ToString();
                textBox16.Text = compview.Rows[rowind]["regacct"].ToString();

                comboBox4.SelectedValue = Convert.ToInt32(compview.Rows[rowind]["sec_type"]);
                comboBox5.SelectedValue = Convert.ToInt32(compview.Rows[rowind]["lang"]);
                comboBox6.SelectedValue = Convert.ToInt32(compview.Rows[rowind]["curr_id"]);

                comboBox20.SelectedValue = compview.Rows[rowind]["savectrl_ind"].ToString();
                comboBox22.SelectedValue = compview.Rows[rowind]["savectrl_cor"].ToString();
                comboBox24.SelectedValue = compview.Rows[rowind]["savectrl_grp"].ToString();
                comboBox26.SelectedValue = compview.Rows[rowind]["savectrl_stf"].ToString();

                comboBox21.SelectedValue = compview.Rows[rowind]["sharectrl_ind"].ToString();
                comboBox23.SelectedValue = compview.Rows[rowind]["sharectrl_cor"].ToString();
                comboBox25.SelectedValue = compview.Rows[rowind]["sharectrl_grp"].ToString();
                comboBox27.SelectedValue = compview.Rows[rowind]["sharectrl_stf"].ToString();

                comboBox8.SelectedIndex = Convert.ToInt32(compview.Rows[rowind]["feefrequency"]);
                comboBox10.SelectedIndex = Convert.ToInt32(compview.Rows[rowind]["runfrequency"]);
                comboBox12.SelectedIndex = Convert.ToInt32(compview.Rows[rowind]["shfreq"]);
                comboBox11.SelectedIndex = Convert.ToInt32(compview.Rows[rowind]["shrunfreq"]);


                AllClear2Go();
            }
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            gcInputStat = "A";
            //          textBox1.Enabled = false;
            comboBox1.Enabled = true;
            AllClear2Go();
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            updatecompany();
            initvariables();
            tabPage1.Focus();
        }

        private void comboBox7_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox7.Focused)
            {
                getcity(Convert.ToInt16(comboBox7.SelectedValue));
                AllClear2Go();
            }
        }

        private void textBox2_Validated(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            {
                decimal dper = Convert.ToDecimal(textBox2.Text);
                if (dper > 100.00m)
                {
                    textBox2.Text = "100.00";
                }
                else
                {
                    textBox2.Text = Convert.ToDecimal(textBox2.Text).ToString("N2");
                }
            }
            AllClear2Go();
        }

        private void richTextBox1_Validated(object sender, EventArgs e)
        {
            AllClear2Go();
        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox2.Focused)
            {
                AllClear2Go();
            }
        }

        private void textBox3_Validated(object sender, EventArgs e)
        {
            AllClear2Go();
        }

        private void textBox4_Validated(object sender, EventArgs e)
        {
            AllClear2Go();
        }

        private void textBox6_Validated(object sender, EventArgs e)
        {
            AllClear2Go();
        }

        private void comboBox4_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox4.Focused)
            {
                AllClear2Go();
            }
        }

        private void comboBox5_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox5.Focused)
            {
                AllClear2Go();
            }
        }

        private void comboBox6_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox6.Focused)
            {
                AllClear2Go();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"D:\",
                Title = "Browse Picture Files",

                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "png",
                Filter = "Picture Files (*.png;*.jpg)|*.png;*.bmp;*.jpg",
                FilterIndex = 1,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox12.Text = openFileDialog1.SafeFileName;
                //     string dlogo = openFileDialog1.FileName;
                pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
            }
            else
            {
                textBox12.Text = "";
            }

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox20_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox20.Focused)
            {
                page2ok();
            }
        }

        private void page2ok()
        {
            if (comboBox20.Text != "" || comboBox21.Text != "" || comboBox22.Text != "" || comboBox23.Text != "" || comboBox24.Text != "" || comboBox25.Text != "" ||
                comboBox26.Text != "" || comboBox27.Text != "") // && textBox5.Text!="" && textBox10.Text != "" && textBox11.Text != "" && )
            {
                updateButton.Enabled = true;
                updateButton.BackColor = Color.LawnGreen;
            }
            else
            {
                updateButton.Enabled = false;
                updateButton.BackColor = Color.Gainsboro;
            }
        }

        private void comboBox22_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox22.Focused)
            {
                page2ok();
            }
        }

        private void comboBox24_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox24.Focused)
            {
                page2ok();
            }
        }

        private void comboBox26_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox26.Focused)
            {
                page2ok();
            }
        }

        private void comboBox21_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox21.Focused)
            {
                page2ok();
            }
        }

        private void comboBox23_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox23.Focused)
            {
                page2ok();
            }
        }

        private void comboBox25_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox25.Focused)
            {
                page2ok();
            }
        }

        private void comboBox27_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox27.Focused)
            {
                page2ok();
            }
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            comboBox8.Enabled = false;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            comboBox8.Enabled = true;
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            comboBox10.Enabled = false;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            comboBox10.Enabled = true;
        }

        private void textBox16_Validated(object sender, EventArgs e)
        {

            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                string lcAccount = textBox16.Text.ToString().Trim();
                string dsql0 = "select ltrim(rtrim(cacctname)) from glmast where cacctnumb = '" + lcAccount + "'";
                SqlDataAdapter da0 = new SqlDataAdapter(dsql0, ndConnHandle);
                DataTable regview = new DataTable();
                da0.Fill(regview);
                if (regview.Rows.Count > 0)
                {
                    textBox15.Text = regview.Rows[0]["cacctname"].ToString().Trim();
                }
                else
                {
                    MessageBox.Show("Account not found ");
                }
            }
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            comboBox12.Enabled = false;
        }

        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {
            comboBox11.Enabled = false;
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            comboBox12.Enabled = true;
        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            comboBox11.Enabled = true;
        }


    }
}

