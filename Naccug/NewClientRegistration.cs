using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using TclassLibrary;




namespace WinTcare
{
    public partial class NewClientRegistration : Form
    {
        //public 
            DataTable ClientBills = new DataTable() ;
            DataTable insurance = new DataTable();
            DataTable tempcover = new DataTable();
            DataView tempc = new DataView();
            string cs = globalvar.cos;
            string gcIpID = "";
            string gcAcctNumber = string.Empty;
            int gnVisno = 0;
            int ncompid = globalvar.gnCompid;
            bool glRebook = false;
            bool glAdmitted = false;
            bool glClientFound = false;
            int gnTempServiceID = -1;
      //  string cs = globalvar.cos;
       // int ncompid = globalvar.gnCompid;
        string cloc = globalvar.cLocalCaption;
        public NewClientRegistration()
        {
//            string csd = globalvar.cos;
            SqlConnection ndNewConnHandle = new SqlConnection(cs);
            InitializeComponent();
        }

        private void tempTableCreation(DataTable clientTable)
        {
            //          create clientTable
            if (clientTable == null)
            {
                MessageBox.Show("For some reason this is null");
            }
            else
            {
                clientTable.Columns.Add("Servce_code");
                clientTable.Columns.Add("Servce_fee");
                clientTable.Columns.Add("Servce_name");
                //      MessageBox.Show("ok here it is");
            }

            //          create tempcover
            if (tempcover == null)
            {
                MessageBox.Show("For some reason tempcover  is null");
            }
            else
            {
                tempcover.Columns.Add("insu_name");
                tempcover.Columns.Add("insu_id");
                tempcovergrid.AutoGenerateColumns = false;
                tempcovergrid.DataSource = tempcover.DefaultView;
                tempcovergrid.Columns[0].DataPropertyName = "insu_name";
                tempcovergrid.Columns[1].DataPropertyName = "insu_id";
//                comboBox2.ValueMember = "idtype";
            }
        }


        private void NewClientRegistration_Load(object sender, EventArgs e)
        {
            tempTableCreation(ClientBills);
            this.Size = new Size(850, 552);
            this.Text = globalvar.cLocalCaption + "<< New Client Registration >>";
            label22.Text = globalvar.gcCopyRight;
            string ncompid = globalvar.gnCompid.ToString().Trim();
            this.textBox7.Text = globalvar.gcIDDCODE.ToString();
    //        string cs = globalvar.cos;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataReader cUserDetails = null;
                SqlCommand cGetUser = new SqlCommand("select ccustcode from patient_code", ndConnHandle);
                cUserDetails = cGetUser.ExecuteReader();
                cUserDetails.Read();
                if (cUserDetails.HasRows == true)
                {
                    this.textBox1.Text = cUserDetails.GetString(0).Trim();
                    textBox2.Focus();
                }
                else { MessageBox.Show("Could not get next client code, inform IT Dept immediately"); }
                cUserDetails.Close();


                //************Getting ID TYPE
                string dsql = "exec tsp_GetIDTYPE ";
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                DataSet ds = new DataSet();
                da.Fill(ds);
                if (ds != null)
                {
                    comboBox2.DataSource = ds.Tables[0];
                    comboBox2.DisplayMember = "id_name";
                    comboBox2.ValueMember = "idtype";
                    comboBox2.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not get ID Types, inform IT Dept immediately"); }

                //************Getting location combobox1                
                string dsql1 = "exec Tsp_getLocation ";
                SqlDataAdapter da1 = new SqlDataAdapter(dsql1, ndConnHandle);
                DataSet ds1 = new DataSet();
                da1.Fill(ds1);
                if (ds1 != null)
                {
                    comboBox1.DataSource = ds1.Tables[0];
                    comboBox1.DisplayMember = "loc_name";
                    comboBox1.ValueMember = "loc_id";
                    comboBox1.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not get locations, inform IT Dept immediately"); }


                //************Getting hospitals for referrals in  combobox4
                string dsql3 = "exec tsp_GetHospitals " + ncompid;
                SqlDataAdapter da3 = new SqlDataAdapter(dsql3, ndConnHandle);
                DataSet ds3 = new DataSet();
                da3.Fill(ds3);
                if (ds3 != null)
                {
                    comboBox4.DataSource = ds3.Tables[0];
                    comboBox4.DisplayMember = "hos_name";
                    comboBox4.ValueMember = "hos_id";
                    comboBox4.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not get referral hospitals, inform IT Dept immediately"); }

                //************Getting Service centres for: normal or walkin service centres combobox5
                string dsql5 = "exec tsp_getServCent " + ncompid;       //normal service centre
                SqlDataAdapter da5 = new SqlDataAdapter(dsql5, ndConnHandle);
                DataSet ds5 = new DataSet();
                da5.Fill(ds5);
                if (ds5 != null)
                {
                    //                    MessageBox.Show("company is " + ncompid);
                    comboBox5.DataSource = ds5.Tables[0];
                    comboBox5.DisplayMember = "srv_name";
                    comboBox5.ValueMember = "srv_id";
                    comboBox5.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not find service centres, inform IT Dept immediately"); }
                radioButton13.Enabled = false;
                radioButton14.Enabled = false;
                radioButton15.Enabled = false;
                radioButton16.Enabled = false;
                radioButton17.Enabled = false;
                radioButton18.Enabled = false;
            }
        }




        public override string ToString()
        {
            return Name;
        }
/*
        protected override void OnKeyUp(KeyEventArgs e)
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
        */
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
            // string newpass = UserPassWord.Text.ToUpper();
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != ""
                && textBox6.Text != "" && textBox13.Text != "" && textBox9.Text != "" && textBox15.Text != ""
                && textBox16.Text != "" && gnTempServiceID > -1)
            {
                SaveButton.Enabled = true;
                SaveButton.BackColor = Color.LawnGreen;
                SaveButton.Select();
            }
            else
            {
                SaveButton.Enabled = false;
                //*            MessageBox.Show("Invalid User or Password");
            }

        }
        #endregion 

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            textBox8.Text = dob.Value.Year.ToString();
            textBox5.Text = ((DateTime.Now.Year - dob.Value.Year)).ToString();
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox9_KeyPress(object sender, KeyPressEventArgs e)
        {
            //      this.ActiveControl=MonthCalendar();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
//            string cs = globalvar.cos;
            string tcCustCode = textBox1.Text.ToString();
            string tcAcctNumb = globalvar.ClientAcctPrefix + textBox1.Text.ToString();
            globalvar.gcAcctNumb = globalvar.ClientAcctPrefix + textBox1.Text.ToString();
            decimal srvFee =(textBox17.Text.Trim()!="" ? Convert.ToDecimal(this.textBox17.Text) : 0.00m);
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                if (radioButton9.Checked == true)      //A new client will be inserted into the tables
                {
                    insertpatients();
                    insertTodayPatients();
                    if(radioButton9.Checked == true)        //this is a new client
                    {
                        createAccount();
                    }
                    updateHistory();
                    updateTodayQueue();
                    medcover();
                    ndConnHandle.Open();
                    string dnewcode = (Int32.Parse(textBox1.Text) + 1).ToString().Trim();
                    if (dnewcode.Length < 6)
                    {
                        dnewcode = "0" + dnewcode;
                    }
                    string cupdatequery = "update patient_code set ccustcode = @cdnewcode ";
                    SqlDataAdapter updCommand = new SqlDataAdapter();
                    updCommand.UpdateCommand = new SqlCommand(cupdatequery, ndConnHandle);
                    updCommand.UpdateCommand.Parameters.Add("@cdnewcode", SqlDbType.VarChar).Value = dnewcode;
                    updCommand.UpdateCommand.ExecuteNonQuery();

                    SqlDataReader cNewUserID = null;
                    SqlCommand cGetDnewUser = new SqlCommand("select ccustcode from patient_code", ndConnHandle);
                    cNewUserID = cGetDnewUser.ExecuteReader();
                    cNewUserID.Read();
                    if (cNewUserID.HasRows == true)
                    {
                        this.textBox1.Text = cNewUserID.GetString(0).Trim();
                        var newt = cNewUserID.GetString(0).Trim();
                        textBox2.Focus();
                    }
                    cNewUserID.Close();
                }
                else                                   //This is a revisit client
                {
                    //                    MessageBox.Show("This is a revisit client");
                    ndConnHandle.Open();
                    string cupdatequery = "update pat_visit set activesession=0,larchived=1 where ccustcode=@cCustCode and activesession=1 ";
                    SqlDataAdapter updCommand = new SqlDataAdapter();
                    updCommand.UpdateCommand = new SqlCommand(cupdatequery, ndConnHandle);
                    updCommand.UpdateCommand.Parameters.Add("@cCustCode", SqlDbType.VarChar).Value = tcCustCode;
                    updCommand.UpdateCommand.ExecuteNonQuery();
                    ndConnHandle.Close();

                    deletetables dtabledel = new deletetables();
                    dtabledel.zaptempfiles(cs, "pat_visit_bkp", tcCustCode);
                    dtabledel.zaptempfiles(cs, "labtestitems_bkp", tcCustCode);
                    dtabledel.zaptempfiles(cs, "radtestitems_bkp", tcCustCode);
                    dtabledel.zaptempfiles(cs, "opttestitems_bkp", tcCustCode);
                    dtabledel.zaptempfiles(cs, "diag_rept_bkp", tcCustCode);
                    dtabledel.zaptempfiles(cs, "drug_dispense_bkp", tcCustCode);
                    dtabledel.zaptempfiles(cs, "TodayVisit", tcCustCode);
                    dtabledel.zaptempfiles(cs, "todayhist", tcCustCode);
                    dtabledel.zaptempfiles(cs, "todaypats", tcCustCode);

                    insertTodayPatients();
                    updateHistory();
                    updateTodayQueue();
                    medcover();
                }

                ndConnHandle.Close();
                this.SaveButton.Enabled = false;
                SaveButton.BackColor = Color.Gainsboro;
                this.textBox2.Text = "";
                this.textBox3.Text = "";
                this.textBox4.Text = "";
                this.textBox5.Text = "";
                this.dob.Text = "";
                this.textBox6.Text = "";
                this.textBox7.Text = "";
                this.textBox8.Text = "";
                this.textBox13.Text = "";
                this.textBox9.Text = "";
                this.textBox16.Text = "";
                this.textBox15.Text = "";
                textBox17.Text = "";
                gnTempServiceID = -1;
                textBox17.Visible = true;
                this.comboBox1.Refresh();
                this.radioButton4.Checked = true;
                this.radioButton3.Checked = false;
                this.radioButton1.Checked = true;
                this.radioButton2.Checked = false;
                this.radioButton15.Checked = false;
                this.radioButton16.Checked = true;
                this.radioButton13.Checked = true;
                this.radioButton14.Checked = false;
                this.radioButton17.Checked = true;
                this.radioButton18.Checked = false;
                this.checkBox1.Checked = false;
                this.checkBox2.Checked = false;
                this.checkBox9.Checked = false;
                this.checkBox3.Checked = false;
                this.checkBox4.Checked = false;
                tempcover.Clear();
                this.textBox2.Focus();
            }
        }

        private void medcover()
        {
    //        string cs = globalvar.cos;
            int ncompid = globalvar.gnCompid;
            int lnVisno = (this.radioButton9.Checked == true ? 1 : Int32.Parse(this.textBox18.Text));
            string tcCode = textBox1.Text.ToString().Trim();
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                ndConnHandle1.Open();
                string dsql1 = "insert into medcover(ins_id, ccustcode, compid, dtrandate, ctrantime, visno)";
                dsql1 += "values (@lnin,@gcCustCode,@gnCompID,convert(date,getdate()),convert(time,getdate()),@gnVisitNo)";

                SqlDataAdapter medcommand = new SqlDataAdapter();
                medcommand.InsertCommand = new SqlCommand(dsql1, ndConnHandle1);
                if(tempcover.Rows.Count>0)
                {
                    for(int j=0;j<tempcover.Rows.Count;j++)
                    {
                        int lnin = Convert.ToInt32(tempcover.Rows[j]["insu_id"]);
//                        MessageBox.Show("lnin is " + lnin+" tcCode is "+tcCode+" compid is "+ncompid+" visno is "+lnVisno);
                        medcommand.InsertCommand.Parameters.Add("@lnin", SqlDbType.Int).Value = lnin;
                        medcommand.InsertCommand.Parameters.Add("@gcCustCode", SqlDbType.VarChar).Value = tcCode;
                        medcommand.InsertCommand.Parameters.Add("@gnCompID", SqlDbType.Int).Value = ncompid;
                        medcommand.InsertCommand.Parameters.Add("@gnVisitNo", SqlDbType.Int).Value =lnVisno ;
                        medcommand.InsertCommand.ExecuteNonQuery();
                    }
                }
            }
        }

        private void insertpatients()
        {
//            string cs = globalvar.cos;
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                string ncompid = globalvar.gnCompid.ToString().Trim();
                string cquery = "insert into patients (ccustcode,ccustfname,ccustmname,ccustlname,pc_tel,ddatebirth,datejoin,date_creat,cpassno,nokid,nokname,noktel,locid,idtype,gender,resident,married,compid) ";
                cquery += "values (@lcCardNumber,@lcFirstName,@lcMidName,@lcLastName,@tel,@dob,convert(date,getdate()),convert(date,getdate()),@cpassno,@nokid,@nokname,@noktel,@locid,@idtype,@gender,@rstatus,@married,@ncompid)";
                SqlDataAdapter myCommand = new SqlDataAdapter();
                myCommand.InsertCommand = new SqlCommand(cquery, ndConnHandle1);
                myCommand.InsertCommand.Parameters.Add("@lcCardNumber", SqlDbType.VarChar).Value = this.textBox1.Text.Trim().ToUpper();
                myCommand.InsertCommand.Parameters.Add("@lcFirstName", SqlDbType.VarChar).Value = this.textBox2.Text.Trim().ToUpper(); ;
                myCommand.InsertCommand.Parameters.Add("@lcMidName", SqlDbType.VarChar).Value = this.textBox3.Text.Trim().ToUpper(); ;
                myCommand.InsertCommand.Parameters.Add("@tel", SqlDbType.VarChar).Value = this.textBox6.Text.Trim().ToUpper(); ;
                myCommand.InsertCommand.Parameters.Add("@lcLastName", SqlDbType.VarChar).Value = this.textBox4.Text.Trim().ToUpper(); ;
                myCommand.InsertCommand.Parameters.Add("@dob", SqlDbType.DateTime).Value = this.dob.Text;
                myCommand.InsertCommand.Parameters.Add("@cpassno", SqlDbType.VarChar).Value = this.textBox13.Text;
                myCommand.InsertCommand.Parameters.Add("@nokid", SqlDbType.VarChar).Value = this.textBox9.Text;
                myCommand.InsertCommand.Parameters.Add("@nokname", SqlDbType.VarChar).Value = this.textBox16.Text;
                myCommand.InsertCommand.Parameters.Add("@noktel", SqlDbType.VarChar).Value = this.textBox15.Text;
                myCommand.InsertCommand.Parameters.Add("@locid", SqlDbType.Int).Value = comboBox1.SelectedValue;
                myCommand.InsertCommand.Parameters.Add("@idtype", SqlDbType.Int).Value = comboBox2.SelectedValue;
                myCommand.InsertCommand.Parameters.Add("@gender", SqlDbType.Bit).Value = radioButton1.Checked;
                myCommand.InsertCommand.Parameters.Add("@rstatus", SqlDbType.Bit).Value = radioButton4.Checked;
                myCommand.InsertCommand.Parameters.Add("@married", SqlDbType.Bit).Value = checkBox1.Checked;
                myCommand.InsertCommand.Parameters.Add("@ncompid", SqlDbType.Int).Value = globalvar.gnCompid;

                /*
                string cquery1 = "insert into todaypats (ccustcode,ccustfname,ccustmname,ccustlname,pc_tel,ddatebirth,datejoin,date_creat,cpassno,nokid,nokname,noktel,locid,idtype,gender,resident,married,compid) ";
                cquery1 += "values (@lcCardNumber,@lcFirstName,@lcMidName,@lcLastName,@tel,@dob,convert(date,getdate()),convert(date,getdate()),@cpassno,@nokid,@nokname,@noktel,@locid,@idtype,@gender,@rstatus,@married,@ncompid)";
                SqlDataAdapter myCommand1 = new SqlDataAdapter();
                myCommand1.InsertCommand = new SqlCommand(cquery1, ndConnHandle1);
                myCommand1.InsertCommand.Parameters.Add("@lcCardNumber", SqlDbType.VarChar).Value = this.textBox1.Text.Trim().ToUpper();
                myCommand1.InsertCommand.Parameters.Add("@lcFirstName", SqlDbType.VarChar).Value = this.textBox2.Text.Trim().ToUpper(); ;
                myCommand1.InsertCommand.Parameters.Add("@lcMidName", SqlDbType.VarChar).Value = this.textBox3.Text.Trim().ToUpper(); ;
                myCommand1.InsertCommand.Parameters.Add("@tel", SqlDbType.VarChar).Value = this.textBox6.Text.Trim().ToUpper(); ;
                myCommand1.InsertCommand.Parameters.Add("@lcLastName", SqlDbType.VarChar).Value = this.textBox4.Text.Trim().ToUpper(); ;
                myCommand1.InsertCommand.Parameters.Add("@dob", SqlDbType.DateTime).Value = this.dob.Text;
                myCommand1.InsertCommand.Parameters.Add("@cpassno", SqlDbType.VarChar).Value = this.textBox13.Text;
                myCommand1.InsertCommand.Parameters.Add("@nokid", SqlDbType.VarChar).Value = this.textBox9.Text;
                myCommand1.InsertCommand.Parameters.Add("@nokname", SqlDbType.VarChar).Value = this.textBox16.Text;
                myCommand1.InsertCommand.Parameters.Add("@noktel", SqlDbType.VarChar).Value = this.textBox15.Text;
                myCommand1.InsertCommand.Parameters.Add("@locid", SqlDbType.Int).Value = comboBox1.SelectedValue;
                myCommand1.InsertCommand.Parameters.Add("@idtype", SqlDbType.Int).Value = comboBox2.SelectedValue;
                myCommand1.InsertCommand.Parameters.Add("@gender", SqlDbType.Bit).Value = radioButton1.Checked;
                myCommand1.InsertCommand.Parameters.Add("@rstatus", SqlDbType.Bit).Value = radioButton4.Checked;
                myCommand1.InsertCommand.Parameters.Add("@married", SqlDbType.Bit).Value = checkBox1.Checked;
                myCommand1.InsertCommand.Parameters.Add("@ncompid", SqlDbType.Int).Value = globalvar.gnCompid;
                */
                ndConnHandle1.Open();
                myCommand.InsertCommand.ExecuteNonQuery();  //Insert new record
//                myCommand1.InsertCommand.ExecuteNonQuery();  //Insert new record
                ndConnHandle1.Close();
            }
        }

        private void insertTodayPatients()
        {
            //            string cs = globalvar.cos;
            gcAcctNumber = (radioButton9.Checked ? string.Empty : globalvar.ClientAcctPrefix + textBox1.Text.ToString().Trim());
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                string cquery1 = "insert into todaypats (ccustcode,ccustfname,ccustmname,ccustlname,pc_tel,ddatebirth,datejoin,date_creat,cpassno,nokid,nokname,noktel,locid,idtype,gender,resident,married,compid,cacctnumb) ";
                cquery1 += "values (@lcCardNumber,@lcFirstName,@lcMidName,@lcLastName,@tel,@dob,convert(date,getdate()),convert(date,getdate()),@cpassno,@nokid,@nokname,@noktel,@locid,@idtype,@gender,@rstatus,@married,@ncompid,@actnumb)";
                SqlDataAdapter insTodayPats = new SqlDataAdapter();
                insTodayPats.InsertCommand = new SqlCommand(cquery1, ndConnHandle1);
                insTodayPats.InsertCommand.Parameters.Add("@lcCardNumber", SqlDbType.VarChar).Value = this.textBox1.Text.Trim().ToUpper();
                insTodayPats.InsertCommand.Parameters.Add("@lcFirstName", SqlDbType.VarChar).Value = this.textBox2.Text.Trim().ToUpper(); ;
                insTodayPats.InsertCommand.Parameters.Add("@lcMidName", SqlDbType.VarChar).Value = this.textBox3.Text.Trim().ToUpper(); ;
                insTodayPats.InsertCommand.Parameters.Add("@tel", SqlDbType.VarChar).Value = this.textBox6.Text.Trim().ToUpper(); ;
                insTodayPats.InsertCommand.Parameters.Add("@lcLastName", SqlDbType.VarChar).Value = this.textBox4.Text.Trim().ToUpper(); ;
                insTodayPats.InsertCommand.Parameters.Add("@dob", SqlDbType.DateTime).Value = this.dob.Text;
                insTodayPats.InsertCommand.Parameters.Add("@cpassno", SqlDbType.VarChar).Value = this.textBox13.Text;
                insTodayPats.InsertCommand.Parameters.Add("@nokid", SqlDbType.VarChar).Value = this.textBox9.Text;
                insTodayPats.InsertCommand.Parameters.Add("@nokname", SqlDbType.VarChar).Value = this.textBox16.Text;
                insTodayPats.InsertCommand.Parameters.Add("@noktel", SqlDbType.VarChar).Value = this.textBox15.Text;
                insTodayPats.InsertCommand.Parameters.Add("@locid", SqlDbType.Int).Value = comboBox1.SelectedValue;
                insTodayPats.InsertCommand.Parameters.Add("@idtype", SqlDbType.Int).Value = comboBox2.SelectedValue;
                insTodayPats.InsertCommand.Parameters.Add("@gender", SqlDbType.Bit).Value = radioButton1.Checked;
                insTodayPats.InsertCommand.Parameters.Add("@rstatus", SqlDbType.Bit).Value = radioButton4.Checked;
                insTodayPats.InsertCommand.Parameters.Add("@married", SqlDbType.Bit).Value = checkBox1.Checked;
                insTodayPats.InsertCommand.Parameters.Add("@ncompid", SqlDbType.Int).Value = globalvar.gnCompid;
                insTodayPats.InsertCommand.Parameters.Add("@actnumb", SqlDbType.Char).Value = gcAcctNumber;
                

                ndConnHandle1.Open();
                insTodayPats.InsertCommand.ExecuteNonQuery();  //Insert new record
                ndConnHandle1.Close();
            }
        }

        private void createAccount()
        {
            string cs = globalvar.cos;
            using (SqlConnection nConnHandle2 = new SqlConnection(cs))
            {

                string cglquery = "Insert Into glmast (cacctnumb,cacctname,acode,dopedate,compid,ccustcode,cuserid)";
                cglquery += " values (@tcAcctNumb,@lcAcctName,@lcAcode,convert(date,getdate()),@ncompid,@lcCustCode,@lcuserid)";

                string cglquery1 = "update patients set cacctnumb=@tcAcctNumb where ccustcode = @lcCustCode";
                string cglquery2 = "update todaypats set cacctnumb=@tcAcctNumb where ccustcode = @lcCustCode";

                SqlDataAdapter insCommand = new SqlDataAdapter();
                SqlDataAdapter updCommand = new SqlDataAdapter();
                SqlDataAdapter updCommand1 = new SqlDataAdapter();

                insCommand.InsertCommand = new SqlCommand(cglquery, nConnHandle2);
                insCommand.InsertCommand.Parameters.Add("@tcAcctNumb", SqlDbType.VarChar).Value = globalvar.ClientAcctPrefix+ this.textBox1.Text.Trim().ToUpper();
                insCommand.InsertCommand.Parameters.Add("@lcAcctName", SqlDbType.VarChar).Value = this.textBox2.Text.ToUpper().Trim().ToString() + this.textBox4.Text.ToUpper().Trim().ToString();
                insCommand.InsertCommand.Parameters.Add("@lcAcode", SqlDbType.VarChar).Value = globalvar.ClientAcctPrefix;
                insCommand.InsertCommand.Parameters.Add("@ncompid", SqlDbType.VarChar).Value = globalvar.gnCompid;
                insCommand.InsertCommand.Parameters.Add("@lcCustCode", SqlDbType.VarChar).Value = this.textBox1.Text.Trim().ToUpper();
                insCommand.InsertCommand.Parameters.Add("@lcUserid", SqlDbType.VarChar).Value = globalvar.gcUserid;

                updCommand.UpdateCommand = new SqlCommand(cglquery1, nConnHandle2);
                updCommand.UpdateCommand.Parameters.Add("@tcAcctNumb", SqlDbType.VarChar).Value = "104" + this.textBox1.Text.Trim(); //.ToUpper();
                updCommand.UpdateCommand.Parameters.Add("@lcCustCode", SqlDbType.VarChar).Value = this.textBox1.Text.Trim().ToUpper();

                updCommand1.UpdateCommand = new SqlCommand(cglquery2, nConnHandle2);
                updCommand1.UpdateCommand.Parameters.Add("@tcAcctNumb", SqlDbType.VarChar).Value = "104" + this.textBox1.Text.Trim(); //.ToUpper();
                updCommand1.UpdateCommand.Parameters.Add("@lcCustCode", SqlDbType.VarChar).Value = this.textBox1.Text.Trim().ToUpper();

                nConnHandle2.Open();
                insCommand.InsertCommand.ExecuteNonQuery();
                updCommand.UpdateCommand.ExecuteNonQuery();
                updCommand1.UpdateCommand.ExecuteNonQuery();
                nConnHandle2.Close();
            }
        }

        private void updateHistory()
        {
           int billcount = ClientBills.Rows.Count;
            for (int i = 0; i < billcount; i++)
            {
                string dcode = ClientBills.Rows[i]["servce_code"].ToString();
                decimal sfee = Convert.ToDecimal(ClientBills.Rows[i]["servce_fee"]);
                string sname = ClientBills.Rows[i]["servce_name"].ToString();
//                MessageBox.Show("The code =" + dcode + ", Service name = " + sname + ", service fee is " + sfee.ToString());
                mandatoryBill(globalvar.gcAcctNumb, dcode, sfee, sname);
                //       mandatoryBill(string tcA)
            }

        }

        private void updateTodayQueue()
        {
   //         MessageBox.Show("we are here to update the queue");
//            string cs = globalvar.cos;
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
            {
                if (this.checkBox5.Checked == true)     //This is a walk-in client
                {
                    //       lpaid = Iif(glMandatoryFee, Iif(gnTotal2Pay = 0, 1, 0), 1)
                    //     gnVisitNo = Iif(gcInputStat = 'R', gnVisitNo, 1)
                    //   Select billitems

                    //         Count For Inlist(srvcent, 3, 5) To m      && Laboratory
                    //        If m> 0 && send to lab
                    //           Set Filter To Inlist(srvcent,3, 5)
                    if (this.comboBox5.SelectedValue.Equals(5))
                    {
                        this.send2Lab();
                    }
                    //          Locate
                    //                Select billitems
                    //              gn2Lab = 1
                    //        Endif

                    //      Count For srvcent = 6 To N        && Radiology
                    //    If N> 0 && send to Rad
                    //      Set Filter To srvcent = 6
                    //    Locate
                    if (this.comboBox5.SelectedValue.Equals(6))
                    {
                        this.send2Rad();
                    }
                    //  Select billitems
                    //  gn2Rad = 1
                    //Endif

                    //                Count For srvcent = 8 To s        && Operating Theatre
                    //                 If s > 0 && send to Opt
                    //                  Set Filter To srvcent = 8
                    ///                   Locate
                    if (this.comboBox5.SelectedValue.Equals(8))
                    {
                        this.send2Opt();
                    }
                    //                    Select billitems
                    //                    gn2Opt = 1
                    //              Endif

                    //                If gl2Pha && send to Pha
                    //                  gn2Pha = 1
                    //            Endif


                    //            fn = SQLExec(gnConnHandle, "select visno from pat_visit where ccustcode=?gcCustCode order by visno", "VisitView")

                    int lnVisno = (this.radioButton9.Checked == true ? 1 : Int32.Parse(this.textBox18.Text));
                    bool ln2Lab = (this.checkBox5.Checked == false ? false : (this.comboBox5.SelectedValue.Equals(5) ? true : false));
                    bool ln2Rad = (this.checkBox5.Checked == false ? false : (this.comboBox5.SelectedValue.Equals(6) ? true : false));
                    bool ln2Opt = (this.checkBox5.Checked == false ? false : (this.comboBox5.SelectedValue.Equals(8) ? true : false));
                    bool ln2Pha = (this.checkBox5.Checked == false ? false : (this.comboBox5.SelectedValue.Equals(7) ? true : false));

                    string cpatquery = "insert into pat_visit (ccustcode,visno,visdate,vistime,compid,q_id,lpaid,refer_in,refer_from,ltriage,lconsult,sent2lab,sent2rad,sent2opt,prescribed,emercase,walkin,wit_cas,wit_cor,wit_nhi,wit_ins,unknown,lspecserv,dr_id,loc_id)";
                    cpatquery += "values (@lcCustCode,@lnVisitNo,convert(date,getdate()),convert(time,getdate()),@nCompID,@srvcen,0,@lrefin,@lrefrom,1,1,@ln2Lab,@ln2Rad,@ln2Opt,@ln2Pha,@lnemer,1,@lnCash,@lnCor,@lnNhi,@lnIns,@lnUnk,@lnSpec,@lnSpecID,@lnLocId)";

                    string cpatquery1 = "insert into todayvisit (ccustcode,visno,visdate,vistime,compid,q_id,lpaid,refer_in,refer_from,ltriage,lconsult,sent2lab,sent2rad,sent2opt,prescribed,emercase,walkin,wit_cas,wit_cor,wit_nhi,wit_ins,unknown,lspecserv,dr_id,loc_id)";
                    cpatquery1 += "values (@lcCustCode,@lnVisitNo,convert(date,getdate()),convert(time,getdate()),@nCompID,@srvcen,0,@glrefin,@lrefrom,1,1,@ln2Lab,@ln2Rad,@ln2Opt,@ln2Pha,@lnemer,1,@lnCash,@lnCor,@lnNhi,@lnIns,@lnUnk,@lnSpec,@lnSpecID,@lnLocId)";


                    SqlDataAdapter patinsCommand = new SqlDataAdapter();
                    SqlDataAdapter patinsCommand1 = new SqlDataAdapter();

                    patinsCommand.InsertCommand = new SqlCommand(cpatquery, ndConnHandle3);
                    patinsCommand.InsertCommand.Parameters.Add("@lcCustCode", SqlDbType.VarChar).Value = this.textBox1.Text.Trim().ToUpper();
                    patinsCommand.InsertCommand.Parameters.Add("@lnVisitNo", SqlDbType.Int).Value = lnVisno;
                    patinsCommand.InsertCommand.Parameters.Add("@ncompid", SqlDbType.VarChar).Value = globalvar.gnCompid;
                    patinsCommand.InsertCommand.Parameters.Add("@srvcen", SqlDbType.Int).Value = this.comboBox5.SelectedValue;
                    patinsCommand.InsertCommand.Parameters.Add("@lrefin", SqlDbType.Bit).Value = this.checkBox7.Checked;
                    patinsCommand.InsertCommand.Parameters.Add("@lrefrom", SqlDbType.Bit).Value = this.comboBox4.SelectedValue;
                    patinsCommand.InsertCommand.Parameters.Add("@ln2Lab", SqlDbType.Bit).Value = ln2Lab;
                    patinsCommand.InsertCommand.Parameters.Add("@ln2Rad", SqlDbType.Bit).Value = ln2Rad;
                    patinsCommand.InsertCommand.Parameters.Add("@ln2Opt", SqlDbType.Bit).Value = ln2Opt;
                    patinsCommand.InsertCommand.Parameters.Add("@ln2Pha", SqlDbType.Bit).Value = ln2Pha;
                    patinsCommand.InsertCommand.Parameters.Add("@lnemer", SqlDbType.Bit).Value = this.checkBox2.Checked;
                    patinsCommand.InsertCommand.Parameters.Add("@@lnCash", SqlDbType.Bit).Value = this.checkBox3.Checked;
                    patinsCommand.InsertCommand.Parameters.Add("@lncor", SqlDbType.Bit).Value = this.radioButton14.Checked;
                    patinsCommand.InsertCommand.Parameters.Add("@lnNhi", SqlDbType.Bit).Value = this.radioButton18.Checked;
                    patinsCommand.InsertCommand.Parameters.Add("@lnIns", SqlDbType.Bit).Value = this.radioButton15.Checked;
                    patinsCommand.InsertCommand.Parameters.Add("@lnUnk", SqlDbType.Bit).Value = this.checkBox9.Checked;
                    patinsCommand.InsertCommand.Parameters.Add("@lnSpec", SqlDbType.Bit).Value = this.checkBox6.Checked;
                    patinsCommand.InsertCommand.Parameters.Add("@lnLocId", SqlDbType.Int).Value = this.comboBox1.SelectedValue;
                    patinsCommand.InsertCommand.Parameters.Add("@lnSpecID", SqlDbType.Int).Value = 0;

                    patinsCommand1.InsertCommand = new SqlCommand(cpatquery1, ndConnHandle3);
                    patinsCommand1.InsertCommand.Parameters.Add("@lcCustCode", SqlDbType.VarChar).Value = this.textBox1.Text.Trim().ToUpper();
                    patinsCommand1.InsertCommand.Parameters.Add("@lnVisitNo", SqlDbType.Int).Value = lnVisno;
                    patinsCommand1.InsertCommand.Parameters.Add("@ncompid", SqlDbType.VarChar).Value = globalvar.gnCompid;
                    patinsCommand1.InsertCommand.Parameters.Add("@srvcen", SqlDbType.Int).Value = this.comboBox5.SelectedValue;
                    patinsCommand1.InsertCommand.Parameters.Add("@lrefin", SqlDbType.Bit).Value = this.checkBox7.Checked;
                    patinsCommand1.InsertCommand.Parameters.Add("@lrefrom", SqlDbType.Bit).Value = this.comboBox4.SelectedValue;
                    patinsCommand1.InsertCommand.Parameters.Add("@ln2Lab", SqlDbType.Bit).Value = ln2Lab;
                    patinsCommand1.InsertCommand.Parameters.Add("@ln2Rad", SqlDbType.Bit).Value = ln2Rad;
                    patinsCommand1.InsertCommand.Parameters.Add("@ln2Opt", SqlDbType.Bit).Value = ln2Opt;
                    patinsCommand1.InsertCommand.Parameters.Add("@ln2Pha", SqlDbType.Bit).Value = ln2Pha;
                    patinsCommand1.InsertCommand.Parameters.Add("@lnemer", SqlDbType.Bit).Value = this.checkBox2.Checked;
                    patinsCommand1.InsertCommand.Parameters.Add("@@lnCash", SqlDbType.Bit).Value = this.checkBox3.Checked;
                    patinsCommand1.InsertCommand.Parameters.Add("@lncor", SqlDbType.Bit).Value = this.radioButton14.Checked;
                    patinsCommand1.InsertCommand.Parameters.Add("@lnNhi", SqlDbType.Bit).Value = this.radioButton18.Checked;
                    patinsCommand1.InsertCommand.Parameters.Add("@lnIns", SqlDbType.Bit).Value = this.radioButton15.Checked;
                    patinsCommand1.InsertCommand.Parameters.Add("@lnUnk", SqlDbType.Bit).Value = this.checkBox9.Checked;
                    patinsCommand1.InsertCommand.Parameters.Add("@lnSpec", SqlDbType.Bit).Value = this.checkBox6.Checked;
                    patinsCommand1.InsertCommand.Parameters.Add("@lnLocId", SqlDbType.Int).Value = this.comboBox1.SelectedValue;
                    patinsCommand1.InsertCommand.Parameters.Add("@lnSpecID", SqlDbType.Int).Value = 0;

                    ndConnHandle3.Open();
                    patinsCommand.InsertCommand.ExecuteNonQuery();
                    patinsCommand1.InsertCommand.ExecuteNonQuery();
                    ndConnHandle3.Close();
                }
                else                                   //This is a normal client
                {
                    bool lpaid = false;     // (glRegCashPay==true || glInsPay==true ? false:true); // cash must go to paypoint and insurance clients must go to cover mgt and paypoint if co - payment
                    int lnVisno = (this.radioButton9.Checked == true ? 1 : Int32.Parse(this.textBox18.Text));
                    bool ln2Tria = false;    // (this.checkBox3.Checked ? (glgl Iif(glGloNoCon, IIF(!glMandatoryFee, 1, 0), Iif(gnTotalPayment > 0.00, 0, 1)) : false);

                    bool ln2Lab = (this.checkBox5.Checked == false ? false : (this.comboBox5.SelectedValue.Equals(5) ? true : false));
                    bool ln2Rad = (this.checkBox5.Checked == false ? false : (this.comboBox5.SelectedValue.Equals(6) ? true : false));
                    bool ln2Opt = (this.checkBox5.Checked == false ? false : (this.comboBox5.SelectedValue.Equals(8) ? true : false));
                    bool ln2Pha = (this.checkBox5.Checked == false ? false : (this.comboBox5.SelectedValue.Equals(7) ? true : false));


                    string cpatquery = "insert into pat_visit (ccustcode,visno,visdate,vistime,compid,q_id,lpaid,refer_in,refer_from,wit_cas,wit_ins,wit_cor,wit_nhi,ready4Triage,emercase,unknown,lspecserv,dr_id,loc_id)";
                    cpatquery += "values (@lcCustCode,@lnVisitNo,convert(date,getdate()),convert(time,getdate()),@nCompID,@srvcen,@dlpaid,@lrefin,@lrefrom,@lnCash,@lnIns,@lnCor,@lnNhi,@ln2Tri,@lnemer,@lnUnk,@lnSpec,@lnSpecID,@lnLocId)";

                    string cpatquery1 = "insert into TodayVisit (ccustcode,visno,visdate,vistime,compid,q_id,lpaid,refer_in,refer_from,wit_cas,wit_ins,wit_cor,wit_nhi,ready4Triage,emercase,unknown,lspecserv,dr_id,loc_id)";
                    cpatquery1 += "values (@lcCustCode,@lnVisitNo,convert(date,getdate()),convert(time,getdate()),@nCompID,@srvcen,@dlpaid,@lrefin,@lrefrom,@lnCash,@lnIns,@lnCor,@lnNhi,@ln2Tri,@lnemer,@lnUnk,@lnSpec,@lnSpecID,@lnLocId)";

                    SqlDataAdapter patinsCommand = new SqlDataAdapter();
                    SqlDataAdapter patinsCommand1 = new SqlDataAdapter();

                    patinsCommand.InsertCommand = new SqlCommand(cpatquery, ndConnHandle3);
                    patinsCommand.InsertCommand.Parameters.Add("@lcCustCode", SqlDbType.VarChar).Value = this.textBox1.Text.Trim().ToUpper();
                    patinsCommand.InsertCommand.Parameters.Add("@lnVisitNo", SqlDbType.Int).Value = lnVisno;
                    patinsCommand.InsertCommand.Parameters.Add("@ncompid", SqlDbType.VarChar).Value = globalvar.gnCompid;
                    patinsCommand.InsertCommand.Parameters.Add("@srvcen", SqlDbType.Int).Value = this.comboBox5.SelectedValue;
                    patinsCommand.InsertCommand.Parameters.Add("@lrefin", SqlDbType.Bit).Value = this.checkBox7.Checked;
                    patinsCommand.InsertCommand.Parameters.Add("@lrefrom", SqlDbType.Bit).Value = (checkBox7.Checked ? this.comboBox4.SelectedValue : false);
                    patinsCommand.InsertCommand.Parameters.Add("@ln2Lab", SqlDbType.Bit).Value = ln2Lab;
                    patinsCommand.InsertCommand.Parameters.Add("@ln2Rad", SqlDbType.Bit).Value = ln2Rad;
                    patinsCommand.InsertCommand.Parameters.Add("@ln2Opt", SqlDbType.Bit).Value = ln2Opt;
                    patinsCommand.InsertCommand.Parameters.Add("@ln2Pha", SqlDbType.Bit).Value = ln2Pha;
                    patinsCommand.InsertCommand.Parameters.Add("@lnemer", SqlDbType.Bit).Value = this.checkBox2.Checked;
                    patinsCommand.InsertCommand.Parameters.Add("@lnCash", SqlDbType.Bit).Value = this.checkBox3.Checked;
                    patinsCommand.InsertCommand.Parameters.Add("@lnIns", SqlDbType.Bit).Value = this.radioButton15.Checked;
                    patinsCommand.InsertCommand.Parameters.Add("@lncor", SqlDbType.Bit).Value = this.radioButton14.Checked;
                    patinsCommand.InsertCommand.Parameters.Add("@lnNhi", SqlDbType.Bit).Value = this.radioButton18.Checked;
                    patinsCommand.InsertCommand.Parameters.Add("@lnUnk", SqlDbType.Bit).Value = this.checkBox9.Checked;
                    patinsCommand.InsertCommand.Parameters.Add("@lnSpec", SqlDbType.Bit).Value = this.checkBox6.Checked;
                    patinsCommand.InsertCommand.Parameters.Add("@lnLocId", SqlDbType.Int).Value = this.comboBox1.SelectedValue;
                    patinsCommand.InsertCommand.Parameters.Add("@lnSpecID", SqlDbType.Int).Value = 0;
                    patinsCommand.InsertCommand.Parameters.Add("@ln2Tri", SqlDbType.Bit).Value = ln2Tria;
                    patinsCommand.InsertCommand.Parameters.Add("@dlpaid", SqlDbType.Bit).Value = lpaid;

                    patinsCommand1.InsertCommand = new SqlCommand(cpatquery1, ndConnHandle3);
                    patinsCommand1.InsertCommand.Parameters.Add("@lcCustCode", SqlDbType.VarChar).Value = this.textBox1.Text.Trim().ToUpper();
                    patinsCommand1.InsertCommand.Parameters.Add("@lnVisitNo", SqlDbType.Int).Value = lnVisno;
                    patinsCommand1.InsertCommand.Parameters.Add("@ncompid", SqlDbType.VarChar).Value = globalvar.gnCompid;
                    patinsCommand1.InsertCommand.Parameters.Add("@srvcen", SqlDbType.Int).Value = this.comboBox5.SelectedValue;
                    patinsCommand1.InsertCommand.Parameters.Add("@lrefin", SqlDbType.Bit).Value = this.checkBox7.Checked;
                    patinsCommand1.InsertCommand.Parameters.Add("@lrefrom", SqlDbType.Bit).Value = (checkBox7.Checked ? this.comboBox4.SelectedValue : false);// this.comboBox4.SelectedValue;
                    patinsCommand1.InsertCommand.Parameters.Add("@ln2Lab", SqlDbType.Bit).Value = ln2Lab;
                    patinsCommand1.InsertCommand.Parameters.Add("@ln2Rad", SqlDbType.Bit).Value = ln2Rad;
                    patinsCommand1.InsertCommand.Parameters.Add("@ln2Opt", SqlDbType.Bit).Value = ln2Opt;
                    patinsCommand1.InsertCommand.Parameters.Add("@ln2Pha", SqlDbType.Bit).Value = ln2Pha;
                    patinsCommand1.InsertCommand.Parameters.Add("@lnemer", SqlDbType.Bit).Value = this.checkBox2.Checked;
                    patinsCommand1.InsertCommand.Parameters.Add("@lnCash", SqlDbType.Bit).Value = this.checkBox3.Checked;
                    patinsCommand1.InsertCommand.Parameters.Add("@lncor", SqlDbType.Bit).Value = this.radioButton14.Checked;
                    patinsCommand1.InsertCommand.Parameters.Add("@lnNhi", SqlDbType.Bit).Value = this.radioButton18.Checked;
                    patinsCommand1.InsertCommand.Parameters.Add("@lnIns", SqlDbType.Bit).Value = this.radioButton15.Checked;
                    patinsCommand1.InsertCommand.Parameters.Add("@lnUnk", SqlDbType.Bit).Value = this.checkBox9.Checked;
                    patinsCommand1.InsertCommand.Parameters.Add("@lnSpec", SqlDbType.Bit).Value = this.checkBox6.Checked;
                    patinsCommand1.InsertCommand.Parameters.Add("@lnLocId", SqlDbType.Int).Value = this.comboBox1.SelectedValue;
                    patinsCommand1.InsertCommand.Parameters.Add("@lnSpecID", SqlDbType.Int).Value = 0;
                    patinsCommand1.InsertCommand.Parameters.Add("@ln2Tri", SqlDbType.Bit).Value = ln2Tria;
                    patinsCommand1.InsertCommand.Parameters.Add("@dlpaid", SqlDbType.Bit).Value = lpaid;
                    ndConnHandle3.Open();
                    patinsCommand.InsertCommand.ExecuteNonQuery();
                    patinsCommand1.InsertCommand.ExecuteNonQuery();
                    ndConnHandle3.Close();
                }
            }
        }


        private void send2Lab()
        {

        }


        private void send2Rad()
        {

        }


        private void send2Opt()
        {

        }


        private void updateDashBoard()
        {
            /*gnDIncome=0.00		&&Iif(glVatable,gnDIncome*(1+gnVat),gnDIncome)
gnDAcct_rec=Iif(glVatable,gnDAcct_rec*(1+gnVat),gnDAcct_rec)
sn=SQLExec(gnConnHandle,"exec sp_UpdateDashBoard ?gnCompid, ?gnDnYear,?gnDmonth,?gcDuptype,?gnDPatients,?gnDdoctors,?gnDNurses,?gnDStaff,?gnDIncome,"+;
	"?gnDExpense,?gnDLabRequest,?gnDLabReceipt,?gnDLabTests,?gnDLabInvalid,?gnDLabValid,?gnDLabTurn,?gnDRadRequest,?gnDRadReceipt,?gnDRadTest,?gnDRadInvalid,?gnDRadValid,?gnDRadTurn,"+;
	"?gnDOtRequest,?gnDOtReceipt,?gnDOtSurgeries,?gnDOtDeaths,?gnDOtTurn,?gnDDeliveries,?gnDTotalDeaths,?gnDfemale,?gnDmale,?gnDchild,?gnDpLocal,?gnDforeigner,?gnDteenager,?gnDAdult,?gnDprepaid,"+;
	"?gnDhas_ins,?gnDhas_cor,?gnDAcct_rec,?gnDAcct_pay,?gnDDrugStockValue,0","Patupd")
If !(sn>0)
	=sysmsg("Dashboard not updated")
Endif
            */
        }


        private void textBox1_Validated(object sender, EventArgs e)
        {
            if (this.textBox1.Text.Trim() != "")
            {
                string lcClientCode = this.textBox1.Text.Trim().PadLeft(6, '0');
                textBox1.Text = lcClientCode;
                checkClient(lcClientCode);
                if(glClientFound == true )
                {
                    getipid(textBox1.Text.ToString());
                    if (glAdmitted == false)
                    {
                        getrevisit(textBox1.Text.ToString());
                        lastvisit(textBox1.Text.ToString());
                        if (glRebook == true)
                        {
                                       getClientDetails(lcClientCode);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("client is not found");
                    textBox1.Text = "";
                    textBox14.Text = "";
                    textBox1.Focus();
                }

            }
        }

        private void checkClient(string tcCode)
        {
            /*exec tsp_New_Client_One ?gnCompid,?lcCardNumber*/
            string ncompid = globalvar.gnCompid.ToString().Trim();
            string dsql = "exec tsp_New_Client_One  " + ncompid + ",'" + tcCode + "'";
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                DataTable vtable = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(vtable);
                if (vtable.Rows.Count > 0)
                {
                    glClientFound = true;
                }
                else { glClientFound = false; }
            }
        }
        private void getipid(string tcCode)
        {
            /*        tm=SQLExec(gnConnHandle,"exec tsp_GetAdminID ?gnCompid,?gcCustCode","admView")   */
            string ncompid = globalvar.gnCompid.ToString().Trim();
            string dsql = "exec tsp_GetAdminID  " + ncompid + ",'" + tcCode + "'";
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                DataTable vtable = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(vtable);
                if (vtable.Rows.Count > 0)
                {
                    gcIpID = Convert.ToString(vtable.Rows[0]["ipid"]);
                    textBox20.Text = gcIpID;
                    glAdmitted = true;
                    glRebook = false;
                    MessageBox.Show("Admitted Client has not been discharged, cannot re-book");
                }
                else { gcIpID = ""; glAdmitted = false; }
            }
        }

        private void getrevisit(string tcCode)
        {
            string fn2 = "exec tsp_Ready4DisClearance_One  " + ncompid + ",'" + tcCode + "'";
            string fn3 = "exec tsp_Active_Client_List_One  " + ncompid + ",'" + tcCode + "'";
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                DataTable clstable = new DataTable();
                DataTable acttable = new DataTable();

                SqlDataAdapter dcls = new SqlDataAdapter(fn2, ndConnHandle);
                SqlDataAdapter dact = new SqlDataAdapter(fn3, ndConnHandle);

                dcls.Fill(clstable);
                dact.Fill(acttable);
                if (glAdmitted == true)
                {
                    if (clstable.Rows.Count > 0)
                    {
                        MessageBox.Show("Client is at Clearance Desk, cannot re-book");
                    }
                    glRebook = false;
                }
                else
                {
                    if (acttable.Rows.Count > 0)
                    {
                        DateTime ldVisDate = Convert.ToDateTime(acttable.Rows[0]["visdate"]); 
                        MessageBox.Show("Client has Active Registration for "+ldVisDate+" Please discharge and then re-book");
                        textBox1.Text = "";
                        textBox14.Text = "";
                        glRebook = false;
                        textBox1.Focus();
                    }
                    else
                    {
                        glRebook = true;
                   //     MessageBox.Show("Client does not have an Active Registration Please re-book");
                    }
                }
            }
        }

        private void lastvisit(string tcCode)
        {
            string ncompid = globalvar.gnCompid.ToString().Trim();
            string lvs = "select visdate as lastvisit,visno from pat_visit where ccustcode = " + "'" + tcCode + "'" + " and compid=" + ncompid+" order by lastvisit desc ";
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                DataTable vistable = new DataTable();
                SqlDataAdapter davis = new SqlDataAdapter(lvs, ndConnHandle);
                davis.Fill(vistable);
                if (vistable.Rows.Count > 0)
                {
                    textBox14.Text = Convert.ToDateTime(vistable.Rows[0]["lastvisit"]).ToString();
                    gnVisno = Convert.ToInt32(vistable.Rows[0]["visno"])+1;
                    textBox18.Text = gnVisno.ToString();
                }
            }
        }

        private void getClientDetails(string tcCode)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                string dsql1 = "exec tsp_New_Client_One  " + ncompid + "," + "'"+tcCode+"'";
                SqlDataAdapter dad = new SqlDataAdapter(dsql1,ndConnHandle);
                DataTable restview = new DataTable();
                dad.Fill(restview);

                if (restview.Rows.Count > 0)
                {
                    int didtype = Convert.ToInt32(restview.Rows[0]["idtype"]);  // cUserDetails.GetName("[ cUserDetails.GetInt32(8).ToString();
                    comboBox2.Enabled = true;
                    textBox2.Text = restview.Rows[0]["fname"].ToString();                       //first name          // cUserDetails.GetString(0).Trim().ToUpper();
                    textBox3.Text = restview.Rows[0]["mname"].ToString();                       //mid name           // cUserDetails.GetString(0).Trim().ToUpper();
                    textBox4.Text = restview.Rows[0]["lname"].ToString();                       //last name           // cUserDetails.GetString(0).Trim().ToUpper();
                    dob.Text = Convert.ToDateTime(restview.Rows[0]["ddatebirth"]).ToString();   // cUserDetails.GetDateTime(4).ToShortDateString();        // .GetString(4).Trim();       //Date of birth
                    textBox6.Text = restview.Rows[0]["pc_tel"].ToString();                  //Client telephone
                    textBox13.Text = restview.Rows[0]["cpassno"].ToString();                // cUserDetails.GetString(7).Trim();                 //ID Number
                    textBox16.Text = restview.Rows[0]["nokname"].ToString();                 //nok name
                    textBox15.Text = restview.Rows[0]["noktel"].ToString();                //nok tel
                    textBox9.Text = restview.Rows[0]["nokid"].ToString();                 //nok ID
                    comboBox1.SelectedValue = restview.Rows[0]["locid"];                    //Location
                    comboBox2.SelectedValue = restview.Rows[0]["idtype"];                   // cUserDetails.GetInt32(8);                     //Identity type
                    textBox2.Enabled = false;
                    textBox3.Enabled = false;
                    textBox4.Enabled = false;
                    dob.Enabled = false;
                    textBox6.Enabled = false;
                    textBox13.Enabled = false;
                    textBox16.Enabled = false;
                    textBox15.Enabled = false;
                    textBox9.Enabled = false;
                    comboBox1.Enabled = false;
                    comboBox2.Enabled = false;
                }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
        }

        private void clphoto_Click(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            if (textBox8.Text.ToString().Length == 4) //&& Int16.Parse(textBox8.Text) > 1900 && Int16.Parse(textBox8.Text) <= DateTime.Now.Year)
            {
                textBox5.Text = ((DateTime.Now.Year - Int16.Parse(textBox8.Text))).ToString();
            }
            else
            {
                textBox5.Text = "0";
            }
        }


        private void textBox8_Validated(object sender, EventArgs e)
        {
            //    MessageBox.Show("We are in the validation of textbox8");
            if (textBox8.Text.ToString().Length == 4)      //&& Int16.Parse(textBox8.Text) > 1900 && Int16.Parse(textBox8.Text) <= DateTime.Now.Year)
            {
                textBox5.Text = ((DateTime.Now.Year - Int16.Parse(textBox8.Text))).ToString();
            }
            else
            {
                textBox5.Text = "0";
            }
        }


        private void textBox5_Validated(object sender, EventArgs e)
        {
            if (textBox5.Text.ToString().Trim()!="" && Int32.Parse(textBox5.Text) <= 110)
            {
                textBox8.Text = ((Int16.Parse(textBox5.Text) - DateTime.Now.Year) * (-1)).ToString();    // dageyear.ToString();
                string dyear = textBox8.Text.Trim();                                                           //year of birth
                string dmonth = DateTime.Today.Month.ToString().Trim();                                        //month of birth
                string dday = DateTime.Today.Day.ToString().Trim();                                            //day of birth
                string newdob = dyear + "/" + dmonth + "/" + dday;                                      //date string
                DateTime dDob = Convert.ToDateTime(newdob).Date;
                dob.Text = dDob.ToShortDateString();
            }
            else
            {
                MessageBox.Show("Invalid age entered");
                textBox8.Text = " ";
                textBox5.Text = "";
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
        }


        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
        private void getIDtype()       // (object sender, EventArgs e)
        {
        }

     

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            //************Getting Service centres walkin service centres combobox5
            string cs = globalvar.cos;
            string ncompid = globalvar.gnCompid.ToString().Trim();
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string dsql4 = "exec tsp_getWalkinCent " + ncompid;      //walkin service centre
                if (checkBox5.Checked == true)          //This is a walkin client
                {
                    SqlDataAdapter da4 = new SqlDataAdapter(dsql4, ndConnHandle);
                    DataSet ds4 = new DataSet();
                    da4.Fill(ds4);
                    if (ds4 != null)
                    {
                        comboBox5.DataSource = ds4.Tables[0];
                        comboBox5.DisplayMember = "srv_name";
                        comboBox5.ValueMember = "srv_id";
                    }
                }
                else
                {
                    string dsql5 = "exec tsp_getServCent " + ncompid;       //normal service centre
                    SqlDataAdapter da5 = new SqlDataAdapter(dsql5, ndConnHandle);
                    DataSet ds5 = new DataSet();
                    da5.Fill(ds5);
                    if (ds5 != null)
                    {
                        comboBox5.DataSource = ds5.Tables[0];
                        comboBox5.DisplayMember = "srv_name";
                        comboBox5.ValueMember = "srv_id";
                    }
                }
            }
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox7.Checked == true)
            {
                comboBox4.Enabled = true;
            }
            else
            {
                comboBox4.Enabled = false;
            }
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {
            //group
        }

        private void radioButton15_CheckedChanged(object sender, EventArgs e)       //insurance yes
        {
            if (radioButton15.Checked == true)
            {
                insurance.Clear();
                string cs = globalvar.cos;
                string ncompid = globalvar.gnCompid.ToString().Trim();
                using (SqlConnection ndConnHandle = new SqlConnection(cs))
                {
                    //************Getting institution for cover clients combobox3                
                    string dsql2 = "exec tsp_Cover_Comps_type " + ncompid+","+1;
                    SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle);
                    da2.Fill(insurance);
                    if (insurance != null)
                    {
                        comboBox3.DataSource = insurance.DefaultView;    
                        comboBox3.DisplayMember = "insu_name";
                        comboBox3.ValueMember = "insu_id";
                     //   comboBox3.SelectedIndex = -1;
                    }
                    //                    comboBox3.Text = "first call";
                    //                  comboBox3.SelectedValue = 0;
                    comboBox3.Enabled = true;
                    radioButton13.Enabled = false;
                    radioButton14.Enabled = false;
                    radioButton17.Enabled = false;
                    radioButton18.Enabled = false;
                }
            }
            else
            {
                radioButton13.Enabled = true;
                radioButton14.Enabled = true;
                radioButton17.Enabled = true;
                radioButton18.Enabled = true;
//                comboBox3.Enabled = (radioButton16.Checked == true ? false : true);    
            }
        }

        private void radioButton14_CheckedChanged(object sender, EventArgs e)       //corporate yes
        {
            if (radioButton14.Checked == true)
            {
                insurance.Clear();
                string cs = globalvar.cos;
                string ncompid = globalvar.gnCompid.ToString().Trim();
                using (SqlConnection ndConnHandle = new SqlConnection(cs))
                {
                    //************Getting institution for corporate                
                    string dsql2 = "exec tsp_Cover_Comps_type " + ncompid + "," + 2;
                    SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle);
                    da2.Fill(insurance);
                    if (insurance != null)
                    {
                        comboBox3.DataSource = insurance.DefaultView;      
                        comboBox3.DisplayMember = "insu_name";
                        comboBox3.ValueMember = "insu_id";
                    }
                    comboBox3.Enabled = true;
                    radioButton15.Enabled = false;
                    radioButton16.Enabled = false;
                    radioButton17.Enabled = false;
                    radioButton18.Enabled = false;
                }
            } else
            {
                radioButton15.Enabled = true;
                radioButton16.Enabled = true;
                radioButton17.Enabled = true;
                radioButton18.Enabled = true;
            }
        }

        private void radioButton18_CheckedChanged(object sender, EventArgs e)       //NHIF yes
        {
            if (radioButton18.Checked == true)
            {
                insurance.Clear();
                string cs = globalvar.cos;
                string ncompid = globalvar.gnCompid.ToString().Trim();
                using (SqlConnection ndConnHandle = new SqlConnection(cs))
                {
                    //************Getting institution for corporate                
                    string dsql2 = "exec tsp_Cover_Comps_type " + ncompid + "," + 3;
                    SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle);
                    da2.Fill(insurance);
                    if (insurance != null)
                    {
                        comboBox3.DataSource = insurance.DefaultView;       
                        comboBox3.DisplayMember = "insu_name";
                        comboBox3.ValueMember = "insu_id";
                    }
                    comboBox3.Enabled = true;
                    radioButton15.Enabled = false;
                    radioButton16.Enabled = false;
                    radioButton13.Enabled = false;
                    radioButton14.Enabled = false;
                }
            }
            else
            {
                radioButton15.Enabled = true;
                radioButton16.Enabled = true;
                radioButton13.Enabled = true;
                radioButton14.Enabled = true;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked == false)
            {
                radioButton13.Enabled = false;
                radioButton14.Enabled = false;
                radioButton15.Enabled = false;
                radioButton16.Enabled = false;
                radioButton17.Enabled = false;
                radioButton18.Enabled = false;
            }
            else
            {
                radioButton13.Enabled = true;
                radioButton14.Enabled = true;
                radioButton15.Enabled = true;
                radioButton16.Enabled = true;
                radioButton17.Enabled = true;
                radioButton18.Enabled = true;
                comboBox5.Enabled = true;
                checkBox3.Checked = false;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true)
            {
                checkBox4.Checked = false;
                radioButton13.Enabled = false;
                radioButton14.Enabled = false;
                radioButton15.Enabled = false;
                radioButton16.Enabled = false;
                radioButton17.Enabled = false;
                radioButton18.Enabled = false;
                comboBox5.Enabled = true;
                checkBox4.Checked = false;
            }
            else
            {
                this.Size = new Size(850, 552);
                this.CenterToScreen();
            }
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox8.Checked == true)
            {
                this.Width = 1200;
                this.CenterToScreen();      // . StartPosition = CenterToScreen(); 
            }
            else
            {
                this.Width = 850;
                CenterToScreen();
                //                this.StartPosition = CenterToScreen();
            }
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton9.Checked == true)
            {
                using (SqlConnection ndConnHandle = new SqlConnection(cs))
                {
                    ndConnHandle.Open();
                    SqlDataReader cUserDetails = null;
                    SqlCommand cGetUser = new SqlCommand("select ccustcode from patient_code", ndConnHandle);
                    cUserDetails = cGetUser.ExecuteReader();
                    cUserDetails.Read();
                    if (cUserDetails.HasRows == true)
                    {
                        this.textBox1.Text = cUserDetails.GetString(0).Trim();
                        textBox1.Enabled = false;
                        textBox2.Focus();
                    }
                    else { MessageBox.Show("Could not get next client code, inform IT Dept immediately"); }
                    cUserDetails.Close();
                }
            }
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton8.Checked == true)
            {
                textBox1.Text = "";
//                int didtype = Convert.ToInt32(restview.Rows[0]["idtype"]);  // cUserDetails.GetName("[ cUserDetails.GetInt32(8).ToString();
                comboBox2.Enabled = true;
                textBox2.Text = ""; 
                textBox3.Text = ""; 
                textBox4.Text = ""; 
                dob.Text = ""; 
                textBox6.Text = ""; 
                textBox13.Text = ""; 
                textBox16.Text = ""; 
                textBox15.Text = ""; 
                textBox9.Text = "";
                comboBox1.SelectedValue = -1;
                comboBox2.SelectedValue = -1;
                textBox1.Enabled = true;
                textBox1.Focus();
            }
        }


        private void updateTempTables(DataTable tablename, string srvcode, decimal srvfee,string srvname)
        {
            DataRow drowcl = tablename.NewRow();
            drowcl["Servce_code"] = srvcode;
            drowcl["Servce_fee"] = srvfee;
            drowcl["Servce_name"] = srvname;
            tablename.Rows.Add(drowcl);
        //    MessageBox.Show("We are here and have added stuff. Now we will update tranhist");
        }

        private void getmandatoryfee(string nsrvcentre)
        {
            //**********we need to insert into the temporary table created  for billed items
            string cs = globalvar.cos;
            string tcAcctNumb = globalvar.ClientAcctPrefix + textBox1.Text.ToString();
            SqlConnection ndConnHandle = new SqlConnection(cs);
            string ncompid = globalvar.gnCompid.ToString();
            DataSet mds = new DataSet();
            ndConnHandle.Open();
            string dsql = "exec tsp_MandatoryServices " + nsrvcentre + "," + ncompid;
            SqlDataAdapter mda = new SqlDataAdapter(dsql, ndConnHandle);

            mda.Fill(mds, "mysr");
            if (mds != null)
            {
                decimal Servce_Fee = 0.00m;
                string servce_code = "";
                string srvc_name = "";
                int rcount = mds.Tables[0].Rows.Count;
                for (int i = 1; i <= rcount; i++)
                {
                    servce_code = mds.Tables[0].Rows[i - 1]["Srv_code"].ToString();
                    Servce_Fee = Servce_Fee + Convert.ToInt32(mds.Tables[0].Rows[i - 1]["Servce_fee"]);
                    srvc_name = mds.Tables[0].Rows[i - 1]["Servce_name"].ToString().Trim();
                    updateTempTables(ClientBills, servce_code,Servce_Fee,srvc_name);
                }
                label26.Visible = true;
                textBox17.Visible = true;
                textBox17.Text = Servce_Fee.ToString();
                //               return Servee;
                //               if (Servce_Fee == 0.00M && checkBox5.Checked == false && checkBox3.Checked == true && checkBox6.Checked == false) //no mandatory service fee for this unit, request confirmation
                if (checkBox5.Checked == false && checkBox3.Checked == true && checkBox6.Checked == false) //no mandatory service fee for this unit, request confirmation
                {
                    getNonMandatoryfee(nsrvcentre);
                }
            }
            else
            {
                MessageBox.Show("Something is not right");
            }
        }

        private void getNonMandatoryfee(string srvCentre)
        {
            string cs = globalvar.cos;
            string ncompid = globalvar.gnCompid.ToString().Trim();
            string dsql = "exec tsp_NonMandatoryServices " + srvCentre + "," + ncompid;

            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                DataTable ds = new DataTable();
                da.Fill(ds);
                if (ds != null)
                {
                    ServiceGrid.AutoGenerateColumns = false;
                    //                    ServiceGrid.DataSource = ds.Tables["servces"].DefaultView;
                    ServiceGrid.DataSource = ds.DefaultView;
                    ServiceGrid.Columns[0].DataPropertyName = "vatable";
                    ServiceGrid.Columns[1].DataPropertyName = "servce_name";
                    ServiceGrid.Columns[2].DataPropertyName = "servce_fee";
                    ServiceGrid.Columns[3].DataPropertyName = "srv_code";
                    ndConnHandle.Close();
                }
            }
        }

        private void getdrest()
        {
            /*
             Parameters tcCardNumber,N
With Thisform
	If N=2
		sn=SQLExec(gnConnHandle,"exec tsp_New_Client_One ?gnCompid,?tcCardNumber","cardview")
		If sn>0 And Reccount()>0
			Select cardView
			Locate
			.text2.Value=Alltrim(ccustfname)
			.text3.Value=Alltrim(ccustmname)
			.text4.Value=Alltrim(ccustlname)
			.text12.Value=nokname
			.text13.Value=noktel
			.text14.Value=opid
			.text16.Value=nokid
			.text6.Value=Year(Ttod(gdsysdate))-Year(Ttod(ddatebirth))
			.text18.Value=Year(Ttod(ddatebirth))
			.combo2.Enabled=Iif(gcInputStat='A',.F.,.T.)
			gnPatSearch=pat_id
			gnPatientID=pat_id
			gcAcctNumb=cacctnumb
			gcCustCode=ccustcode
			.check2.Value=Iif(prepaid,1,0)
			.text5.Value=ccustcode
			.text5.Enabled=.F.
			gnPatientID=pat_id
			gnPatType=res_type
			.text7.Value=Ttod(ddatebirth)
			.text8.Value=Ttod(date_creat)
			.text9.Value=pc_tel
			.text14.Value=opid
			.text17.Value=cpassno
			.combo1.Value=locid
			.optiongroup1.Value=res_type
			.optiongroup2.Value=Iif(!gender,1,2)
			.combo1.Value=locid
			.combo7.Value=idtype
			glClientCover=Iif(has_ins Or has_cor,.T.,.F.)
			If !Empty(.text7.Value)
				ndays=Ttod(gdsysdate)-.text7.Value
				ny1=Int(ndays/365)
				ny2=Int(ndays/364)
				nyears=Iif(ny2>ny1,ny2,ny1)
				nmonths=(Mod(ndays,364))/(30)
				ndays=Mod(Mod(ndays,364),30)
				.text1.Value = nyears
				.text10.Value = nmonths
				.text11.Value = ndays
				.label16.Caption=Iif(Int(nyears)<>1,'Years','Year')
				.label17.Caption=Iif(Int(nmonths)<>1,'Months','Month')
				.label18.Caption=Iif(Int(ndays)<>1,'Days','Day')
			Endif
			gnClientAge=.text1.Value
			.image1.Picture=cphoto
			lcalias=Alias()
			.visitnumber
			.getipid
			.getrevisit
			.lastvisit
			Select (lcalias)
		Else
			.combo2.Enabled=.F.
			=sysmsg('Client not found, please register')
			This.text5.Value = ''
			Return This
		Endif
	Endif
	If .check2.Value=1 And !Empty(.combo2.Value)
		.getmandatoryfee
	Endif
	.Refresh
Endwith

             */
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }


        private void button5_Click(object sender, EventArgs e)
        {
            sched dapp = new sched();
            dapp.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            CoverManagement dcover = new CoverManagement();
            dcover.ShowDialog();
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ServiceGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            decimal dvalue = 0.00m;
            string tcAcctNumb = "104" + textBox1.Text.ToString();
            /*
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
             */

//            if(ServiceGrid.Columns[e.ColumnIndex].Name == "itemSelect")
  //          {
    //            MessageBox.Show("The clicked is item select");
      //      }
            if (e.RowIndex >= 0)
            {
                if (ServiceGrid.Rows[e.RowIndex].Cells[0].Value != null && (bool)ServiceGrid.Rows[e.RowIndex].Cells[0].Value)
                {
                    dvalue = Convert.ToDecimal(ServiceGrid.Rows[e.RowIndex].Cells[2].Value);
                    decimal dtot = decimal.Parse(textBox17.Text) + dvalue;
                    textBox17.Text = dtot.ToString();
                    string srvcode = ServiceGrid.Rows[e.RowIndex].Cells[3].Value.ToString();
                    string cdesc = ServiceGrid.Rows[e.RowIndex].Cells[1].Value.ToString().Trim();
                    updateTempTables(ClientBills, srvcode, dvalue,cdesc);     //we are updating the bill to include the selective service
                }
                else
                {
                    dvalue = Convert.ToDecimal(ServiceGrid.Rows[e.RowIndex].Cells[2].Value);
                    decimal dtot = decimal.Parse(textBox17.Text) - dvalue;
                    textBox17.Text = dtot.ToString();
                }
            }
        }


        private void ServiceGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ServiceGrid.EndEdit();
        }


        private void mandatoryBill(string tcAcctNumb, string srvcode, decimal nsrvFee, string tcDesc)
        {
            string cs = globalvar.cos;
            string ncompid = globalvar.gnCompid.ToString().Trim();
            string tcContra = globalvar.gcIntSuspense;
            string tcUserid = globalvar.gcUserid;
            int tncompid = globalvar.gnCompid;
            string tcCustcode = textBox1.Text.ToString();
            decimal tnTranAmt = -Math.Abs(nsrvFee);
            decimal tnContAmt = Math.Abs(nsrvFee);
            string tcVoucher = genbill.genvoucher(cs, globalvar.gdSysDate);
            decimal unitprice = nsrvFee;
            string tcChqno = "000001";
            decimal lnWaiveAmt = 0.00m;
            string tcTranCode = "01";
            int lnServID = 0;
            bool llPaid = false;
            int tnqty = 1;
            string tcReceipt = "";
            bool llCashpay = (checkBox3.Checked ? true : false);
            //int visno = (radioButton9.Checked ? 1 : Convert.ToInt16(getVisitNumber.visitno(cs, ncompid, tcCustcode)));          //visitno(string cs,int ncompid, string tcCustCode)
            bool isproduct = false;
            int srvid = Convert.ToInt16(comboBox5.SelectedValue);
            bool lFreeBee = globalvar.glFreeBee; // false;

            updateGlmast gls = new updateGlmast();
            updateTranhist tls = new updateTranhist();
//            MessageBox.Show("we will update glmast with " + tnTranAmt);
            gls.updGlmast(cs, tcAcctNumb, tnTranAmt);                                       //update glmast posting account
            decimal tnPNewBal = CheckLastBalance.lastbalance(cs, tcAcctNumb);       //  0.00m;
            //tls.updTranhist(cs, tcAcctNumb, tnTranAmt, tcDesc, tcVoucher, tcChqno, tcUserid, tnPNewBal, tcTranCode, lnServID, llPaid, tcContra, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, visno, isproduct,
                  //srvid, srvcode, "", lFreeBee, tcCustcode, tncompid);                   //update tranhist posting account


            gls.updGlmast(cs, tcContra, tnContAmt);                                     //update glmast contra account
            decimal tnCNewBal = CheckLastBalance.lastbalance(cs, tcContra);         // 0.00m;
            //tls.updTranhist(cs, tcContra, tnContAmt, tcDesc, tcVoucher, tcChqno, tcUserid, tnCNewBal, tcTranCode, lnServID, llPaid, tcAcctNumb, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, visno, isproduct,
                  //srvid, srvcode, "", lFreeBee, tcCustcode, tncompid);                   //update tranhist account 396 1756
        } //
  

        class timeNow          //we plan to use this for or current time
        {
            // Static variable that must be initialized at run time.
            static readonly long baseline;
            static timeNow()
            {
                baseline = DateTime.Now.Ticks;
            }
        }                   //class timeNow  


        private void comboBox5_SelectedValueChanged(object sender, EventArgs e)
        {
            /*string gnTempServiceID = comboBox5.SelectedValue.ToString();
            ClientBills.Clear();

            if (checkBox3.Checked == true)                            //Cash client
            {
                if (checkBox6.Checked == false)                       //normal registration
                {
                    this.Size = new Size(850, 552);
                    getmandatoryfee(gnTempServiceID);                 //getmandatoryfee();
                }
                this.Size = new Size(1202, 552);
                this.CenterToScreen();
            } */
        }

        private void radioButton16_CheckedChanged(object sender, EventArgs e)
        {
            comboBox3.Enabled =(radioButton16.Checked == true ? false : true);
        }

        private void radioButton13_CheckedChanged(object sender, EventArgs e)
        {
            comboBox3.Enabled = (radioButton13.Checked == true ? false : true);
        }

        private void radioButton17_CheckedChanged(object sender, EventArgs e)
        {
            comboBox3.Enabled = (radioButton17.Checked == true ? false : true);
        }


        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            string dselval = Convert.ToString(comboBox3.SelectedValue).Trim();
            if(dselval!="System.Data.DataRowView" && dselval!="")
            {
       //         MessageBox.Show("We are inside selected index changed with value ="+dselval );
                tempcovergrid.AllowUserToAddRows = false;
                string cname = comboBox3.Text;
                DataRow row = tempcover.NewRow();
//                DataGridViewRow row =tempcover.NewRow();
                row["insu_id"] = dselval;
                row["insu_name"] = cname;
                tempcover.Rows.Add(row);
            //    medcover();
                if (radioButton15.Checked == true)
                {
//                    MessageBox.Show("button 15 is checked comboBox3");
                    radioButton13.Enabled = true;
                    radioButton14.Enabled = true;
                    radioButton17.Enabled = true;
                    radioButton18.Enabled = true;
                    radioButton15.Enabled = true;
                    radioButton16.Enabled = true;
//                    radioButton15.Checked = false;
                }

                if (radioButton14.Checked == true)
                {
  //                  MessageBox.Show("button 14 is checked comboBox3");
                    radioButton13.Enabled = true;
                    radioButton14.Enabled = true;
                    radioButton17.Enabled = true;
                    radioButton18.Enabled = true;
                    radioButton15.Enabled = true;
                    radioButton16.Enabled = true;
  //                  radioButton14.Checked = false;
                }

                if (radioButton18.Checked == true)
                {
    //                MessageBox.Show("button 18 is checked comboBox3");
                    radioButton13.Enabled = true;
                    radioButton14.Enabled = true;
                    radioButton17.Enabled = true;
                    radioButton18.Enabled = true;
                    radioButton15.Enabled = true;
                    radioButton16.Enabled = true;
    //                radioButton18.Checked = false;
                }

                //                comboBox3.DisplayMember = "insu_name";
                //              comboBox3.ValueMember = "insu_id";

                //                tempcover.Rows.Add( .Add("this is the row");
                //MessageBox.Show("We will be doing something");
                /*


*/
                insurance.Clear();
            }
        }

        private void NewClientRegistration_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Tab) {AllClear2Go(); }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            AllClear2Go();
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            AllClear2Go();
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            AllClear2Go();
        }

        private void textBox5_Leave(object sender, EventArgs e)
        {
            AllClear2Go();
        }

        private void textBox8_Leave(object sender, EventArgs e)
        {
            AllClear2Go();
        }

        private void dob_Leave(object sender, EventArgs e)
        {
            AllClear2Go();
        }

        private void textBox6_Leave(object sender, EventArgs e)
        {
            AllClear2Go();
        }

        private void comboBox2_Leave(object sender, EventArgs e)
        {
            AllClear2Go();
        }

        private void textBox13_Leave(object sender, EventArgs e)
        {
            AllClear2Go();
        }

        private void comboBox1_Leave(object sender, EventArgs e)
        {
            AllClear2Go();
        }

        private void comboBox4_Leave(object sender, EventArgs e)
        {
            AllClear2Go();
        }

        private void comboBox5_Leave(object sender, EventArgs e)
        {
            AllClear2Go();
        }

        private void textBox16_Leave(object sender, EventArgs e)
        {
            AllClear2Go();
        }

        private void textBox15_Leave(object sender, EventArgs e)
        {
            AllClear2Go();
        }

        private void textBox9_Leave(object sender, EventArgs e)
        {
            AllClear2Go();
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
             gnTempServiceID =(Convert.ToInt16(comboBox1.SelectedIndex) > - 1 ? Convert.ToUInt16(comboBox5.SelectedValue) : -1 );
            ClientBills.Clear();

            if (checkBox3.Checked == true)                            //Cash client
            {
                if (checkBox6.Checked == false)                       //normal registration
                {
                    this.Size = new Size(850, 552);
                    getmandatoryfee(gnTempServiceID.ToString());                 //getmandatoryfee();
                    ServiceGrid.Focus();
                }
                this.Size = new Size(1202, 552);
                this.CenterToScreen();
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            radioButton8.Checked = true;
            using (var findform = new FindClient(cs, ncompid, cloc, 1, "Cusreg"))
            {
                var dresult = findform.ShowDialog();
                if(dresult == DialogResult.OK)
                {
                    string dclientcode = findform.returnValue;
                    MessageBox.Show("tHE CLIENT IS " + dclientcode);

                    string lcClientCode = dclientcode;
                    textBox1.Text = lcClientCode;
                    checkClient(lcClientCode);
                    if (glClientFound == true)
                    {
                        getipid(textBox1.Text.ToString());
                        if (glAdmitted == false)
                        {
                            getrevisit(textBox1.Text.ToString());
                            lastvisit(textBox1.Text.ToString());
                            if (glRebook == true)
                            {
                                getClientDetails(lcClientCode);
                            }
                        }
                    }

                }
            }
        }

        private void ServiceGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            ServiceGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }
    }                       //public partial class NewClientRegistration : Form
}                           //namespace WinTcare
