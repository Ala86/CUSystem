using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Data.SqlClient;
using TclassLibrary;

namespace WinTcare
{
    public partial class userManagement : Form
    {
        string cs = string.Empty;
        int ncompid = 0;
        string cLocalCaption = string.Empty;

        private const int CP_NOCLOSE_BUTTON = 0x200;
        int gnGroupID = 0;
        int gnAreaID = 0;
        int gnUserNumb = 0;
        int gnRoleID = 0;
        int gnUserBaseRole = 0;
        bool glUserOK = false;
        string dNewpassw = string.Empty;
        string dNewpasswConfirmed = string.Empty;
        bool glPassWordMatch = false;
        bool glNewUser = true;
        DataTable userView = new DataTable();
        DataTable grpview = new DataTable();
        DataTable grpuview = new DataTable();
        DataTable grpMoview = new DataTable();
        DataTable usrMoview = new DataTable();
        DataTable usersinview = new DataTable();
        DataTable usersninview = new DataTable();
        DataTable grproleview = new DataTable();
        DataTable useroleview = new DataTable();
        DataTable newroleview = new DataTable();
        DataTable userroleview = new DataTable();
        DataTable newuserroleview = new DataTable();
        DataTable userlistview = new DataTable();
        DataTable grpbelongview = new DataTable();
        DataTable accessview = new DataTable();
        DataTable useraccessview = new DataTable();
        DataTable grpAccessView = new DataTable();
        DataTable usrAccessView = new DataTable();
        DataTable funview = new DataTable();
        DataTable newfunview = new DataTable();
        DataTable baseview = new DataTable();
        DataTable effRightsview = new DataTable();
        DataTable userview = new DataTable();

        public userManagement(string cos, int dcompid, string dloca)
        {
            InitializeComponent();
            cs = cos;
            ncompid = dcompid;
            cLocalCaption = dloca;
        }
        private void userManagement_Load(object sender, EventArgs e)
        {
            this.Text = cLocalCaption + "<< User Management >>";
            getbaseRoles();
            getBasicDetails();
//            maskedTextBox1.Mask = "000,000,000.00";
        }

        /* we will use the code below to disable the close(x) button
    //    private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }
        */

        private void getbaseRoles()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                baseview.Clear();
                string baseuserstring = "exec tsp_GetSysRoles  ";
                SqlDataAdapter baseuseradapt = new SqlDataAdapter(baseuserstring, ndConnHandle);
                baseuseradapt.Fill(baseview);
                if (baseview.Rows.Count > 0)
                {
                    comboBox12.DataSource = baseview.DefaultView;
                    comboBox12.DisplayMember = "rolename";
                    comboBox12.ValueMember = "roleid";
                    comboBox12.SelectedIndex = -1;
                }
            }
        }
        private void getuserRoles()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                userroleview.Clear();
                string userstring = "exec tsp_GetSysRoles  ";
                SqlDataAdapter useradapt = new SqlDataAdapter(userstring, ndConnHandle);
                useradapt.Fill(userroleview);
                if (userroleview.Rows.Count > 0)
                {
                    userRolesGrid.AutoGenerateColumns = false;
                    userRolesGrid.DataSource = userroleview.DefaultView;
                    userRolesGrid.Columns[0].DataPropertyName = "roleselect";
                    userRolesGrid.Columns[1].DataPropertyName = "rolename";
                    userRolesGrid.Columns[2].DataPropertyName = "roleid";
                }
            }
        }

        private void areaFunctions(int tngrp)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                funview.Clear();
                string funtring = "exec tsp_AreaFunctions_One  " + ncompid + "," + tngrp;
                SqlDataAdapter grpadapt = new SqlDataAdapter(funtring, ndConnHandle);
                grpadapt.Fill(funview);
                if (funview.Rows.Count > 0)
                {
                    grpFunGrid.AutoGenerateColumns = false;
                    grpFunGrid.DataSource = funview.DefaultView;
                    grpFunGrid.Columns[0].DataPropertyName = "rolesel";
                    grpFunGrid.Columns[1].DataPropertyName = "functioname";
                    grpFunGrid.Columns[2].DataPropertyName = "funid";
                }
            }
        }


        //private void getSystemRoles()
        //{
        //    using (SqlConnection ndConnHandle = new SqlConnection(cs))
        //    {
        //        rolesview.Clear();
        //        string rolestring = "exec tsp_GetSysRoles  ";
        //        SqlDataAdapter acceadapt = new SqlDataAdapter(rolestring, ndConnHandle);
        //        acceadapt.Fill(rolesview);
        //        if (rolesview.Rows.Count > 0)
        //        {
        //            groupRolesGrid.AutoGenerateColumns = false;
        //            rolesGrid.DataSource = rolesview.DefaultView;
        //            rolesGrid.Columns[0].DataPropertyName = "roleselect";
        //            rolesGrid.Columns[1].DataPropertyName = "rolename";
        //            rolesGrid.Columns[2].DataPropertyName = "roleid";
        //        }
        //    }
        //  }
        private void getgroupRoles()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                grproleview.Clear();
                string grpstring = "exec tsp_GetSysRoles  ";
                SqlDataAdapter grpadapt = new SqlDataAdapter(grpstring, ndConnHandle);
                grpadapt.Fill(grproleview);
                if (grproleview.Rows.Count > 0)
                {
                    groupRolesGrid.AutoGenerateColumns = false;
                    groupRolesGrid.DataSource = grproleview.DefaultView;
                    groupRolesGrid.Columns[0].DataPropertyName = "roleselect";
                    groupRolesGrid.Columns[1].DataPropertyName = "rolename";
                    groupRolesGrid.Columns[2].DataPropertyName = "roleid";
                }

            }
        }


        private void getGroupAccessAreas()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                accessview.Clear();
                string accstring = "exec tsp_SolutionModules_One  " + ncompid;
                SqlDataAdapter acceadapt = new SqlDataAdapter(accstring, ndConnHandle);
                acceadapt.Fill(accessview);
                if (accessview.Rows.Count > 0)
                {
                    groupAccessGrid.AutoGenerateColumns = false;
                    groupAccessGrid.DataSource = accessview.DefaultView;
                    groupAccessGrid.Columns[0].DataPropertyName = "accSelect";
                    groupAccessGrid.Columns[1].DataPropertyName = "acessname";
                    groupAccessGrid.Columns[2].DataPropertyName = "acode";
                }

            }
        }


        private void getUserAccessAreas()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                useraccessview.Clear();
                string accstring = "exec tsp_SolutionModules_One  " + ncompid;
                SqlDataAdapter acceadapt = new SqlDataAdapter(accstring, ndConnHandle);
                acceadapt.Fill(useraccessview);
                if (useraccessview.Rows.Count > 0)
                {
                    userAccessGrid.AutoGenerateColumns = false;
                    userAccessGrid.DataSource = useraccessview.DefaultView;
                    userAccessGrid.Columns[0].DataPropertyName = "accSelect";
                    userAccessGrid.Columns[1].DataPropertyName = "acessname";
                    userAccessGrid.Columns[2].DataPropertyName = "acode";
                }

            }
        }


        private void getactiveUsers()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                
                string dsqlcat = "exec tsp_StaffDepartment " + ncompid;
                SqlDataAdapter dabat = new SqlDataAdapter(dsqlcat, ndConnHandle);

                DataTable hodview = new DataTable();
                DataTable prox1view = new DataTable();
                DataTable prox2view = new DataTable();

                dabat.Fill(hodview);
                dabat.Fill(prox1view);
                dabat.Fill(prox2view);

                if (hodview.Rows.Count > 0)
                {
                    comboBox9.DataSource = hodview.DefaultView;
                    comboBox9.DisplayMember = "staffname";
                    comboBox9.ValueMember = "staffno";
                    comboBox9.SelectedIndex = -1;

                    comboBox10.DataSource = prox1view.DefaultView;
                    comboBox10.DisplayMember = "staffname";
                    comboBox10.ValueMember = "staffno";
                    comboBox10.SelectedIndex = -1;

                    comboBox11.DataSource = prox2view.DefaultView;
                    comboBox11.DisplayMember = "staffname";
                    comboBox11.ValueMember = "staffno";
                    comboBox11.SelectedIndex = -1;
                }
            }
        }


        private void getBasicDetails()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                userView.Clear();
                ndConnHandle.Open();
                string dsql = "exec tsp_getActiveUserList  " + ncompid;
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(userView);
                da.Fill(usrMoview);
                if (userView.Rows.Count > 0)
                {
                    userGrid.AutoGenerateColumns = false;
                    userGrid.DataSource = userView.DefaultView;
                    userGrid.Columns[0].DataPropertyName = "oprcode";
                    userGrid.Columns[1].DataPropertyName = "username";

                    for (int i = 0; i < 5; i++)
                    {
                        userView.Rows.Add();
                    }
                    userGrid.Focus();
                }


                string dsqlc = "exec tsp_getComp " + ncompid;
                SqlDataAdapter dab = new SqlDataAdapter(dsqlc, ndConnHandle);
                DataTable compview = new DataTable();
                dab.Fill(compview);
                if (compview.Rows.Count > 0)
                {
                    comboBox1.DataSource = compview.DefaultView;
                    comboBox1.DisplayMember = "com_name";
                    comboBox1.ValueMember = "compid";
                    comboBox1.SelectedIndex = -1;
                }

                string dsqlcp = "exec tsp_GetNewDept " + ncompid;
                SqlDataAdapter dabp = new SqlDataAdapter(dsqlcp, ndConnHandle);
                DataTable deptview = new DataTable();
                dabp.Fill(deptview);
                if (deptview.Rows.Count > 0)
                {
                    comboBox8.DataSource = deptview.DefaultView;
                    comboBox8.DisplayMember = "dep_name";
                    comboBox8.ValueMember = "dep_id";
                    comboBox8.SelectedIndex = -1;
                }

                string dsqlb = "select br_name,branchid from branch order by br_name ";
                SqlDataAdapter dac = new SqlDataAdapter(dsqlb, ndConnHandle);
                DataTable branchview = new DataTable();
                dac.Fill(branchview);
                if (branchview.Rows.Count > 0)
                {
                    comboBox2.DataSource = branchview.DefaultView;
                    comboBox2.DisplayMember = "br_name";
                    comboBox2.ValueMember = "branchid";
                    comboBox2.SelectedIndex = -1;
                }


                string dsql1 = "exec tsp_Cash_Control " + ncompid;
                SqlDataAdapter dac1 = new SqlDataAdapter(dsql1, ndConnHandle);
                DataTable cashview = new DataTable();
                dac1.Fill(cashview);
                if (cashview.Rows.Count > 0)
                {
                    comboBox3.DataSource = cashview.DefaultView;
                    comboBox3.DisplayMember = "cacctname";
                    comboBox3.ValueMember = "cacctnumb";
                    comboBox3.SelectedIndex = -1;
                }


                string dsql2 = "exec tsp_Cash_Control " + ncompid;
                SqlDataAdapter dac2 = new SqlDataAdapter(dsql2, ndConnHandle);
                DataTable suspview = new DataTable();
                dac2.Fill(suspview);
                if (suspview.Rows.Count > 0)
                {
                    comboBox4.DataSource = suspview.DefaultView;
                    comboBox4.DisplayMember = "cacctname";
                    comboBox4.ValueMember = "cacctnumb";
                    comboBox4.SelectedIndex = -1;
                }

                string dsql3 = "exec tsp_GetGroups " + ncompid;
                SqlDataAdapter dac3 = new SqlDataAdapter(dsql3, ndConnHandle);
                dac3.Fill(grpview);
                dac3.Fill(grpMoview);
                if (grpview.Rows.Count > 0)
                {
                    groupGrid.AutoGenerateColumns = false;
                    groupGrid.DataSource = grpview.DefaultView;
                    groupGrid.Columns[0].DataPropertyName = "groupname";
                    groupGrid.Columns[1].DataPropertyName = "groupid";

                    comboBox6.DataSource = grpMoview.DefaultView;
                    comboBox6.DisplayMember = "groupname";
                    comboBox6.ValueMember = "groupid";
                    comboBox6.SelectedIndex = -1;

                    for (int i = 0; i < 2; i++)
                    {
                        grpview.Rows.Add();
                    }
                }


                string dsql33 = "select * from secpara ";
                SqlDataAdapter dac33 = new SqlDataAdapter(dsql33, ndConnHandle);
                DataTable secview = new DataTable();
                dac33.Fill(secview);
                if (secview.Rows.Count > 0)
                {
                    textBox11.Text = secview.Rows[0]["minpass"].ToString();
                    textBox12.Text = secview.Rows[0]["passvalid"].ToString();
                    textBox13.Text = secview.Rows[0]["loctrials"].ToString();
                    textBox14.Text = secview.Rows[0]["locduration"].ToString();
                    forceChange.Value = DateTime.Today.AddDays(Convert.ToDouble(textBox12.Text));
                    checkBox2.Checked = Convert.ToBoolean(secview.Rows[0]["conclog"]);
                    checkBox6.Checked = Convert.ToBoolean(secview.Rows[0]["fprint"]);
                    checkBox7.Checked = Convert.ToBoolean(secview.Rows[0]["faceprint"]);
                    checkBox8.Checked = Convert.ToBoolean(secview.Rows[0]["mobilecheck"]);
                    radioButton5.Checked = Convert.ToInt16(secview.Rows[0]["passcomp"]) == 1 ? true : false;
                    radioButton6.Checked = Convert.ToInt16(secview.Rows[0]["passcomp"]) == 2 ? true : false;
                    radioButton7.Checked = Convert.ToInt16(secview.Rows[0]["passcomp"]) == 3 ? true : false;
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
            bool glbasic = Convert.ToInt16(comboBox1.SelectedValue) > 0 && Convert.ToInt16(comboBox2.SelectedValue) > 0 && textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "";
            glUserOK = radioButton1.Checked || radioButton2.Checked || radioButton3.Checked || radioButton4.Checked;
            bool glCashier = (!radioButton2.Checked ? (Convert.ToInt16(comboBox3.SelectedIndex) > -1 || Convert.ToInt16(comboBox4.SelectedIndex) > -1 ? true : false) : true);

            if (glbasic && glUserOK && glPassWordMatch && glCashier && gnUserBaseRole>0)
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

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button28_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button20_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button41_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button40_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button48_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button49_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button31_Click(object sender, EventArgs e)
        {
            twocode d2code = new twocode(cs, "Allergy type setup", "allergy", "all_name", ncompid);
            d2code.ShowDialog();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            //bnk bk = new bnk();
            //bk.ShowDialog();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            /*  glFormOpened=.t. =getAccessLevel(gnCompid,gnaccesslvl) reprnt=Createobject('twocodeonly','County Setup','country','cou_name',gnCompID) reprnt.Show glFormOpened=.f.  */
            onecode d1code = new onecode(cs, "Country Setup", "country", "cou_name");
            d1code.ShowDialog();
        }

        private void button26_Click(object sender, EventArgs e)
        {
            /*   glFormOpened=.t. getAccessLevel(gnCompid,gnaccesslvl) reprnt=Createobject('twocode','Location Setup','location','loc_name',gnCompID) reprnt.Show glFormOpened=.f.  */
            twocode d2code = new twocode(cs, "Location Setup", "location", "loc_name", ncompid);
            d2code.ShowDialog();
        }

        private void button27_Click(object sender, EventArgs e)
        {
            twocode d2code = new twocode(cs, "Department Setup", "dept", "dep_name", ncompid);
            d2code.ShowDialog();

        }

        private void button12_Click(object sender, EventArgs e)
        {
            /* glFormOpened=.t. =getAccessLevel(gnCompid,gnaccesslvl) reprnt=Createobject('twocode','Sector Setup','sector','sec_name',gnCompID) reprnt.Show glFormOpened=.f.           */
            twocode d2code = new twocode(cs, "Sector Setup", "sector", "sec_name", ncompid);
            d2code.ShowDialog();
        }

        private void button25_Click(object sender, EventArgs e)
        {
            onecode d1code = new onecode(cs, "City Setup", "biz_nature", "biz_name");
            d1code.ShowDialog();

        }

        private void button21_Click(object sender, EventArgs e)
        {
            onecode d1code = new onecode(cs, "Bank Branch Setup", "biz_nature", "biz_name");
            d1code.ShowDialog();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            onecode d1code = new onecode(cs, "Branch Setup", "biz_nature", "biz_name");
            d1code.ShowDialog();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            /*   glFormOpened=.t. =getAccessLevel(gnCompid,gnaccesslvl) reprnt=Createobject('twocode','Institution Type Setup','inst_type','ins_name',gnCompID) reprnt.Show glFormOpened=.f.   */
            twocode d2code = new twocode(cs, "Institution Type Setup", "inst_type", "ins_name", ncompid);
            d2code.ShowDialog();
        }

        private void button24_Click(object sender, EventArgs e)
        {
            /* glFormOpened=.t. =getAccessLevel(gnCompid,gnaccesslvl) reprnt=Createobject('twocode','Telecom Provider Setup','provider','pro_name',gnCompID) reprnt.Show glFormOpened=.f.   */
            twocode d2code = new twocode(cs, "Telecom Provider Setup", "provider", "pro_name", ncompid);
            d2code.ShowDialog();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            /*         glFormOpened=.t.=getAccessLevel(gnCompid,gnaccesslvl)reprnt=Createobject('twocodeonly','Institution Setup','institution','ins_name',gnCompID)reprnt.ShowglFormOpened=.f.             */
            //        onecode d1code = new onecode(cos, "Institution Setup", "institution", "ins_name");
            //      d1code.ShowDialog();
            //employer emp = new employer();
            //emp.ShowDialog();
        }

        private void button22_Click(object sender, EventArgs e)
        {
            /*  reprnt=Createobject('twocodeonly','County Setup','county','coun_name',gnCompID) reprnt.Show              */
            onecode d1code = new onecode(cs, "County Setup", "county", "coun_name");
            d1code.ShowDialog();
        }

        private void button23_Click(object sender, EventArgs e)
        {
            onecode d1code = new onecode(cs, "Financier Setup", "financier", "fin_name");
            d1code.ShowDialog();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            ProductDef dpr = new ProductDef(cs,ncompid,cLocalCaption);
            dpr.ShowDialog();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Company dcomp = new Company(cs, ncompid, cLocalCaption);
            dcomp.ShowDialog();
        }

        private void button39_Click(object sender, EventArgs e)
        {
            //doctors ddoc = new doctors();
            //ddoc.ShowDialog();
        }

        private void button36_Click(object sender, EventArgs e)
        {
            /*              glFormOpened=.t.=getAccessLevel(gnCompid,gnaccesslvl)reprnt=Createobject('twocodeonly','Doctor Specialisation Setup','docspecial','spe_name',gnCompID)reprnt.ShowglFormOpened=.f.            */
            onecode d1code = new onecode(cs, "Doctor Specialisation Setup", "docspecial", "spe_name");
            d1code.ShowDialog();
        }

        private void button33_Click(object sender, EventArgs e)
        {
            /*             glFormOpened=.t.reprnt=Createobject('twocodeonly','TB SCREENING Setup','tbscreen','tb_name',gnCompID)reprnt.ShowglFormOpened=.f.             */
            onecode d1code = new onecode(cs, "TB Screening Setup", "tbscreen", "tb_name");
            d1code.ShowDialog();
        }

        private void button30_Click(object sender, EventArgs e)
        {
            /*  glFormOpened=.t. =getAccessLevel(gnCompid,gnaccesslvl) reprnt=Createobject('twocode','KMPDB Register Status','dr_Status','sta_name',gnCompID) reprnt.Show glFormOpened=.f.       */
            twocode d2code = new twocode(cs, "KMPDB Register Status", "dr_Status", "sta_name", ncompid);
            d2code.ShowDialog();

        }

        private void button38_Click(object sender, EventArgs e)
        {
            /* glFormOpened=.t. =getAccessLevel(gnCompid,gnaccesslvl) reprnt=Createobject('twocode','Hospitals','hospitals','hos_name',gnCompID) reprnt.Show glFormOpened=.f.   */
            twocode d2code = new twocode(cs, "Hospitals ", "hospitals", "hos_name", ncompid);
            d2code.ShowDialog();
        }

        private void button35_Click(object sender, EventArgs e)
        {
            /* glFormOpened=.t. =getAccessLevel(gnCompid,gnaccesslvl) reprnt=Createobject('twocode','Medical Qualification','qual_typ','qua_name',gnCompID) reprnt.Show glFormOpened=.f.   */
            twocode d2code = new twocode(cs, "Medical Qualification", "qual_typ", "qua_name", ncompid);
            d2code.ShowDialog();
        }

        private void button32_Click(object sender, EventArgs e)
        {
            /* glFormOpened=.t. =getAccessLevel(gnCompid,gnaccesslvl) reprnt=Createobject('twocode','KMPDB Rating','dr_rating','rat_name',gnCompID) reprnt.Show glFormOpened=.f. */
            twocode d2code = new twocode(cs, "KMPDB Rating", "dr_rating", "rat_name", ncompid);
            d2code.ShowDialog();
        }

        private void button37_Click(object sender, EventArgs e)
        {
            /* glFormOpened=.t. =getAccessLevel(gnCompid,gnaccesslvl) reprnt=Createobject('twocode','Appointment Reason Setup','apreason','apr_name',gnCompID) reprnt.Show glFormOpened=.f. */
            twocode d2code = new twocode(cs, "Appointment Reason Setup", "apreason", "apr_name", ncompid);
            d2code.ShowDialog();
        }

        private void button34_Click(object sender, EventArgs e)
        {
            /* glFormOpened=.t. =getAccessLevel(gnCompid,gnaccesslvl) reprnt=Createobject('twocode','Cause of Death','deathcause','ded_name',gnCompID) reprnt.Show glFormOpened=.f. */
            twocode d2code = new twocode(cs, "Cause of Death", "deathcause", "ded_name", ncompid);
            d2code.ShowDialog();
        }

        private void button86_Click(object sender, EventArgs e)
        {
            /*
             glFormOpened=.t.
*=getAccessLevel(gnCompid,gnaccesslvl)
* DO FORM symptomps.SCX
glFormOpened=.f.
             */
        }

        private void button29_Click(object sender, EventArgs e)
        {
            /*  glFormOpened=.t. =getAccessLevel(gnCompid,gnaccesslvl) reprnt=Createobject('twocode','KMPDB Register Authorisation','dr_auto','ath_name',gnCompID) reprnt.Show glFormOpened=.f. */
            twocode d2code = new twocode(cs, "KMPDB Register Authorisation", "dr_auto", "ath_name", ncompid);
            d2code.ShowDialog();
        }

        private void button85_Click(object sender, EventArgs e)
        {
            /*   glFormOpened=.t. reprnt=Createobject('twocode','Maternity Delivery Setup','baby_deli_method','del_name',gnCompID) reprnt.Show glFormOpened=.f. */
            twocode d2code = new twocode(cs, "Maternity Delivery Setup", "baby_deli_method", "del_name", ncompid);
            d2code.ShowDialog();
        }

        private void button47_Click(object sender, EventArgs e)
        {
            /*
             glFormOpened=.t.
=getAccessLevel(gnCompid,gnaccesslvl)
DO FORM procitem.scx
glFormOpened=.f.
             */
        }

        private void button45_Click(object sender, EventArgs e)
        {
            /*        glFormOpened=.t.=getAccessLevel(gnCompid,gnaccesslvl)reprnt=Createobject('twocodeonly','Delegated Authority','dele_auth','aut_name',gnCompid)reprnt.ShowglFormOpened=.f.             */
            onecode d1code = new onecode(cs, "Delegated Authority", "dele_auth", "aut_name");
            d1code.ShowDialog();
        }

        private void button43_Click(object sender, EventArgs e)
        {
            /*
             =getAccessLevel(gnCompid,gnaccesslvl)
            DO FORM chartacc
             */
            //ChartAcc chat = new ChartAcc();
            //chat.ShowDialog();
        }

        private void button44_Click(object sender, EventArgs e)
        {
            /* glFormOpened=.t. =getAccessLevel(gnCompid,gnaccesslvl) reprnt=Createobject('twocode','Supplier Category Setup','suppl_cat','supcat_name',gnCompID) reprnt.Show glFormOpened=.f. */
            twocode d2code = new twocode(cs, "Supplier Category Setup", "suppl_cat", "supcat_name", ncompid);
            d2code.ShowDialog();
        }

        private void button42_Click(object sender, EventArgs e)
        {
            /*
             =getAccessLevel(gnCompid,gnaccesslvl)
glFormOpened=.t.
DO FORM newFinPeriod
glFormOpened=.f.

             */
            //newfinperiod dper = new newfinperiod();
            //dper.ShowDialog();
        }

        private void button46_Click(object sender, EventArgs e)
        {
            /*
             glFormOpened=.t.
=getAccessLevel(gnCompid,gnaccesslvl)
DO FORM sponsors.scx
glFormOpened=.f.

             */
        }

        private void button87_Click(object sender, EventArgs e)
        {
            onecode d1code = new onecode(cs, "ID Type Setup", "id_type", "id_name");
            d1code.ShowDialog();
        }

        private void button54_Click(object sender, EventArgs e)
        {
            /*
             glFormOpened=.t.
=getAccessLevel(gnCompid,gnaccesslvl)
DO FORM band.scx
glFormOpened=.f.
             */
            //band db = new band();
            //db.ShowDialog();
        }

        private void button51_Click(object sender, EventArgs e)
        {
            /*
             glFormOpened=.t.
=getAccessLevel(gnCompid,gnaccesslvl)
DO FORM alowance.scx
glFormOpened=.f.

             */
            //alowance dall = new alowance(cs,ncompid,cLocalCaption);
            //dall.ShowDialog();
        }

        private void button50_Click(object sender, EventArgs e)
        {
            /*
             glFormOpened=.t.
=getAccessLevel(gnCompid,gnaccesslvl)
DO FORM loans.scx
glFormOpened=.f.

             */
            //loans dlon = new loans(cs,ncompid,cLocalCaption);
            //dlon.ShowDialog();
        }

        private void button53_Click(object sender, EventArgs e)
        {
            twocode d2code = new twocode(cs, "Cost Centre Setup", "costcent", "cos_name", ncompid);
            d2code.ShowDialog();
        }

        private void button56_Click(object sender, EventArgs e)
        {
            /*
             glFormOpened=.t.
=getAccessLevel(gnCompid,gnaccesslvl)
DO FORM nssfrates.scx
glFormOpened=.f.
             */
        }

        private void button55_Click(object sender, EventArgs e)
        {
            /*
             glFormOpened=.t.
=getAccessLevel(gnCompid,gnaccesslvl)
DO FORM nhifrates.scx
glFormOpened=.f.
             */
        }

        private void button52_Click(object sender, EventArgs e)
        {
            /*
             glFormOpened=.t.
=getAccessLevel(gnCompid,gnaccesslvl)
DO FORM othcontr.scx
glFormOpened=.f.

             */
            //othcontr dcont = new othcontr();
            //dcont.ShowDialog();
        }

        private void button71_Click(object sender, EventArgs e)
        {
            /* glFormOpened=.t. =getAccessLevel(gnCompid,gnaccesslvl) reprnt=Createobject('twocode','Training Types Setup','train_type','trn_name') reprnt.Show glFormOpened=.f. */
            twocode d2code = new twocode(cs, "Training Types Setup", "train_type", "trn_name", ncompid);
            d2code.ShowDialog();
        }

        private void button67_Click(object sender, EventArgs e)
        {
            onecode d1code = new onecode(cs, "Language Setup", "lang_tp", "lan_name");
            d1code.ShowDialog();
        }

        private void button63_Click(object sender, EventArgs e)
        {
            trn_institute trn = new trn_institute(cs, ncompid, cLocalCaption);
            trn.ShowDialog();
        }

        private void button59_Click(object sender, EventArgs e)
        {
            ///*              glFormOpened=.t. =getAccessLevel(gnCompid,gnaccesslvl) DO form apraisaln.scx glFormOpened=.f. */
            //apraisaln dapr = new apraisaln();
            //dapr.ShowDialog();
        }

        private void button74_Click(object sender, EventArgs e)
        {
            /* glFormOpened=.t. =getAccessLevel(gnCompid,gnaccesslvl) reprnt=Createobject('twocode','Disciplinary action Types','actions','act_name') reprnt.Show glFormOpened=.f. */
            twocode d2code = new twocode(cs, "Disciplinary Action Type", "actions", "act_name", ncompid);
            d2code.ShowDialog();
        }

        private void button70_Click(object sender, EventArgs e)
        {
            /*
             glFormOpened=.t.
=getAccessLevel(gnCompid,gnaccesslvl)
DO FORM stanlett.scx
glFormOpened=.f.
             */
        }

        private void button66_Click(object sender, EventArgs e)
        {
            /* glFormOpened=.t. =getAccessLevel(gnCompid,gnaccesslvl) reprnt=Createobject('twocode','Grievance Types','griv_tp','gri_name') reprnt.Show glFormOpened=.f. */
            twocode d2code = new twocode(cs, "Grievance Types", "griv_tp", "gri_name", ncompid);
            d2code.ShowDialog();
        }

        private void button62_Click(object sender, EventArgs e)
        {
            /*  glFormOpened=.t. =getAccessLevel(gnCompid,gnaccesslvl) reprnt=Createobject('twocode','Dependent Types','depen_tp','dep_name') reprnt.Show glFormOpened=.f. */
            twocode d2code = new twocode(cs, "Dependent Types", "depen_tp", "dep_name", ncompid);
            d2code.ShowDialog();
        }

        private void button58_Click(object sender, EventArgs e)
        {
            /* glFormOpened=.t. =getAccessLevel(gnCompid,gnaccesslvl) reprnt=Createobject('twocode','Disciplining Authority Setup','disp_auth','aut_name') reprnt.Show glFormOpened=.f. */
            twocode d2code = new twocode(cs, "Disciplinary Authority Setup", "disp_auth", "aut_name", ncompid);
            d2code.ShowDialog();
        }

        private void button73_Click(object sender, EventArgs e)
        {
            /*              glFormOpened=.t.=getAccessLevel(gnCompid,gnaccesslvl)reprnt=Createobject('twocodeonly','Ethnicity types setup ','ethnies','eth_name')reprnt.ShowglFormOpened=.f.             */
            onecode d1code = new onecode(cs, "Ethicity types Setup", "ethnies", "eth_name");
            d1code.ShowDialog();
        }

        private void button69_Click(object sender, EventArgs e)
        {
            onecode d1code = new onecode(cs, "Disability types Setup", "disa_type", "dis_name");
            d1code.ShowDialog();
        }

        private void button65_Click(object sender, EventArgs e)
        {
            /*  glFormOpened=.t. =getAccessLevel(gnCompid,gnaccesslvl) reprnt=Createobject('twocode','Absence Types','absence','abs_name') reprnt.Show glFormOpened=.f.    */
            twocode d2code = new twocode(cs, "Absence Types", "absence", "abs_name", ncompid);
            d2code.ShowDialog();
        }

        private void button61_Click(object sender, EventArgs e)
        {
            /* glFormOpened=.t. =getAccessLevel(gnCompid,gnaccesslvl) reprnt=Createobject('twocode','Accident Types','accd_tp','acct_name') reprnt.Show glFormOpened=.f.  */
            twocode d2code = new twocode(cs, "Accident Types", "accd_tp", "acct_name", ncompid);
            d2code.ShowDialog();
        }

        private void button57_Click(object sender, EventArgs e)
        {
            /*              glFormOpened=.t.=getAccessLevel(gnCompid,gnaccesslvl)reprnt=Createobject('twocodeonly','Career Reasons ','creason','cre_name')reprnt.ShowglFormOpened=.f.             */
            onecode d1code = new onecode(cs, "Career Reasons Setup", "creason", "cre_name");
            d1code.ShowDialog();
        }

        private void button72_Click(object sender, EventArgs e)
        {
            /* glFormOpened=.t. =getAccessLevel(gnCompid,gnaccesslvl) reprnt=Createobject('twocode','Skills Set Definition ','skillset','ski_name',gnCompid) reprnt.Show glFormOpened=.f. */
            twocode d2code = new twocode(cs, "Skills Set Definition", "skillset", "ski_name", ncompid);
            d2code.ShowDialog();
        }

        private void button76_Click(object sender, EventArgs e)
        {
            twocode d2code = new twocode(cs, "Marital Status", "marystat", "mar_name", ncompid);
            d2code.ShowDialog();
        }

        private void button75_Click(object sender, EventArgs e)
        {
            twocode d2code = new twocode(cs, "Exit Reason Setup", "exitreasons", "xit_name", ncompid);
            d2code.ShowDialog();
        }

        private void button68_Click(object sender, EventArgs e)
        {
            twocode d2code = new twocode(cs, "Title Types", "titres", "tit_name", ncompid);
            d2code.ShowDialog();
        }

        private void button64_Click(object sender, EventArgs e)
        {
            /* glFormOpened=.t. =getAccessLevel(gnCompid,gnaccesslvl) reprnt=Createobject('twocodeonly','Leave types setup ','leave_tp','lea_name') reprnt.Show glFormOpened=.f. */
            twocode d2code = new twocode(cs, "Leave Types Setup", "leave_tp", "lea_name", ncompid);
            d2code.ShowDialog();
        }

        private void button60_Click(object sender, EventArgs e)
        {
            JobDefinition jd = new JobDefinition(cs,ncompid,cLocalCaption);
            jd.ShowDialog();
        }

        private void button80_Click(object sender, EventArgs e)
        {
            /*            glFormOpened=.t. =getAccessLevel(gnCompid,gnaccesslvl) reprnt=Createobject('twocode','Designation setup','designation','des_name',gnCompID) reprnt.Show glFormOpened=.f.              */
            twocode d2code = new twocode(cs, "Designation Setup", "designation", "des_name", ncompid);
            d2code.ShowDialog();
        }

        private void button79_Click(object sender, EventArgs e)
        {
            /*              glFormOpened=.t.=getAccessLevel(gnCompid,gnaccesslvl)reprnt=Createobject('twocodeonly','Family Relation Setup','family','fam_relation',gnCompID)reprnt.ShowglFormOpened=.f.             */
            onecode d2code = new onecode(cs, "Family Relation Setup", "family", "fam_relation");
            d2code.ShowDialog();
        }

        private void button78_Click(object sender, EventArgs e)
        {
            ///* glFormOpened=.t. =getAccessLevel(gnCompid,gnaccesslvl) DO FORM holidayn.scx glFormOpened=.f.         */
            //holidayn dhol = new holidayn();
            //dhol.ShowDialog();
        }

        private void button77_Click(object sender, EventArgs e)
        {
            /*              glFormOpened=.t.=getAccessLevel(gnCompid,gnaccesslvl)reprnt=Createobject('twocode','Collateral Setup ','collateral','col_name',gnCompid)reprnt.ShowglFormOpened=.f.             */
            onecode d1code = new onecode(cs,  "Collateral Setup", "collateral", "coll_name");
            d1code.ShowDialog();
        }

        private void button88_Click(object sender, EventArgs e)
        {
            /*
             DO FORM autorizer.scx
             */
        }

        private void button83_Click(object sender, EventArgs e)
        {
            /*              glFormOpened=.t. =getAccessLevel(gnCompid,gnaccesslvl)reprnt=Createobject('twocodeonly','Religion Setup ','relgion','rel_name',gnCompid)reprnt.ShowglFormOpened=.f.             */
            onecode d2code = new onecode(cs, "Religion Setup", "relgion", "rel_name");
            d2code.ShowDialog();
        }

        private void button82_Click(object sender, EventArgs e)
        {
            /*              glFormOpened=.t.=getAccessLevel(gnCompid,gnaccesslvl)reprnt=Createobject('twocodeonly','Ailment Definition ','ailment','ail_name',gnCompid)reprnt.ShowglFormOpened=.f.             */
            onecode d2code = new onecode(cs, "Ailment Definition", "ailment", "ail_name");
            d2code.ShowDialog();
        }

        private void button81_Click(object sender, EventArgs e)
        {
            /*              glFormOpened=.t.=getAccessLevel(gnCompid,gnaccesslvl)reprnt=Createobject('twocodeonly','Exit Questions setup ','exitquestions','xq_name')reprnt.ShowglFormOpened=.f.      */
            onecode d2code = new onecode(cs, "Exit Questions Setup", "exitquestions", "xq_name");
            d2code.ShowDialog();
        }

        private void button88_Click_1(object sender, EventArgs e)
        {
            twocode d2code = new twocode(cs, "Loan Reason setup", "loanReason", "res_name", ncompid);
            d2code.ShowDialog();
        }

        private void button89_Click(object sender, EventArgs e)
        {
            twocode d2code = new twocode(cs, "Source of Funds setup", "sourceFunds", "sou_name", ncompid);
            d2code.ShowDialog();
        }

        private void button90_Click(object sender, EventArgs e)
        {
            /*           reprnt=Createobject('twocode','Department Setup','dept','dep_name',gnCompID) reprnt.Show              */
            twocode d2code = new twocode(cs, "Loan Reject Reason Setup", "loanreject", "rej_name", ncompid);
            d2code.ShowDialog();
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox1.Focused)
            {
                AllClear2Go();
            }
        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox2.Focused)
            {
                AllClear2Go();
            }
        }

        private void textBox1_Validated(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                checkstaff(textBox1.Text.Trim());
                AllClear2Go();
            }
        }

        private void checkstaff(string staffno)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                //                string dusername = string.Empty;
                string dstaffno = staffno.PadLeft(6, '0');
                textBox1.Text = dstaffno;
                string dsqls = "select ltrim(rtrim(firstname))+' '+ltrim(RTRIM(midname))+' '+ltrim(rtrim(lastname)) AS staffname from staff where staffno = " + "'" + dstaffno + "'";
                SqlDataAdapter das = new SqlDataAdapter(dsqls, ndConnHandle);
                DataTable staffview = new DataTable();
                das.Fill(staffview);
                if (staffview.Rows.Count > 0)
                {
                    textBox3.Text = staffview.Rows[0]["staffname"].ToString();
                }
                else
                {
                    MessageBox.Show("Staff no does not exist in HR File , please try another one");
                    textBox1.Text = "";
                    textBox1.Focus();
                }
            }
        }

        private void textBox2_Validated(object sender, EventArgs e)
        {
            checkDuplicates(textBox2.Text.Trim());
            AllClear2Go();
        }

        private void textBox3_Validated(object sender, EventArgs e)
        {
            AllClear2Go();
        }

        private void textBox4_Validated(object sender, EventArgs e)
        {
            if (textBox4.Text != "")
            {
                string dtext = textBox4.Text.Trim();
                dNewpassw = tclassChkpassWord.dpassWord(dtext);
                if (dNewpassw.Trim().Length <= 5)
                {
                    textBox4.Text = textBox5.Text = "";
                    textBox4.Focus();
                }
            }
            AllClear2Go();
        }

        private void textBox5_Validated(object sender, EventArgs e)
        {
            if (textBox5.Text != "")
            {
                dNewpasswConfirmed = tclassChkpassWord.dpassWord(textBox5.Text.Trim());
                if (dNewpassw != "" && dNewpasswConfirmed != "" && dNewpassw == dNewpasswConfirmed)
                {
                    glPassWordMatch = true;
                }
                else
                {
                    MessageBox.Show("Password mismatch, please try again");
                    textBox4.Text = textBox5.Text = "";
                    glPassWordMatch = false;
                    textBox4.Focus();
                }
                AllClear2Go();
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            checkUserType();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            checkUserType();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            checkUserType();
        }

        private void checkDuplicates(string tcuser)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                string dusername = string.Empty;
                string dsqls = "select ltrim(rtrim(username)) AS username from susers where oprcode = " + "'" + tcuser + "'";
                SqlDataAdapter das = new SqlDataAdapter(dsqls, ndConnHandle);
                DataTable userviews = new DataTable();
                das.Fill(userviews);
                if (userviews.Rows.Count > 0)
                {
                    dusername = userviews.Rows[0]["username"].ToString();
                    MessageBox.Show("User ID belongs to <<" + dusername + ">>, please try another one");
                    textBox2.Text = "";
                    textBox2.Focus();
                }
            }
        }

        private void checkUserType()
        {
            comboBox3.SelectedIndex = -1;
            comboBox4.SelectedIndex = -1;
            comboBox3.Enabled = radioButton1.Checked || radioButton3.Checked || radioButton4.Checked ? true : false;
            comboBox4.Enabled = radioButton1.Checked || radioButton3.Checked || radioButton4.Checked ? true : false;
            AllClear2Go();
        }

        private void comboBox3_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox3.Focused)
            {
                AllClear2Go();
            }
        }

        private void comboBox4_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox4.Focused)
            {
                AllClear2Go();
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            checkUserType();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (glNewUser)
            {
                insertUser();
            }
            else
            {
                amendUser();
            }
            initvariables();
            getBasicDetails();
        }

        private void insertUser()
        {
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                string dcashacct = Convert.ToInt16(comboBox3.SelectedIndex) > -1 ? comboBox3.SelectedValue.ToString() : string.Empty;
                DateTime dforcedate = Convert.ToDateTime(forceChange.Value);
                string cquery = "Insert Into susers (compid,oprcode,username,userpassword,dateforce,userphoto,branchid,cashaccont,staffno,accesslvl,debtlimitamt,credlimitamt,loanlimitamt,surpaccont)";
                cquery += "values";
                cquery += "(@lcompid,@loprcode,@lusername,@lpswd,@ldateforce,@luserphoto,@lbranchid,@lcashaccont,@lstaffno,@baserole,@debtamt,@credamt,@loanamt,@surpacct)";

                SqlDataAdapter cuscommand = new SqlDataAdapter();
                cuscommand.InsertCommand = new SqlCommand(cquery, ndConnHandle1);
                cuscommand.InsertCommand.Parameters.Add("@lcompid", SqlDbType.Int).Value = ncompid;
                cuscommand.InsertCommand.Parameters.Add("@loprcode", SqlDbType.VarChar).Value = textBox2.Text.Trim();
                cuscommand.InsertCommand.Parameters.Add("@lusername", SqlDbType.VarChar).Value = textBox3.Text.Trim();
                cuscommand.InsertCommand.Parameters.Add("@lpswd", SqlDbType.VarChar).Value = dNewpasswConfirmed.Trim();
                cuscommand.InsertCommand.Parameters.Add("@ldateforce", SqlDbType.DateTime).Value = forceChange.Value;
                cuscommand.InsertCommand.Parameters.Add("@luserphoto", SqlDbType.VarChar).Value = string.Empty;
                cuscommand.InsertCommand.Parameters.Add("@lbranchid", SqlDbType.Int).Value = Convert.ToInt16(comboBox2.SelectedValue);
                cuscommand.InsertCommand.Parameters.Add("@lcashaccont", SqlDbType.VarChar).Value = dcashacct;
                cuscommand.InsertCommand.Parameters.Add("@lstaffno", SqlDbType.VarChar).Value = textBox1.Text;
                cuscommand.InsertCommand.Parameters.Add("@baserole", SqlDbType.Int).Value = gnUserBaseRole;
                cuscommand.InsertCommand.Parameters.Add("@debtamt", SqlDbType.Decimal).Value = Convert.ToDecimal(maskedTextBox1.Text);
                cuscommand.InsertCommand.Parameters.Add("@credamt", SqlDbType.Decimal).Value = Convert.ToDecimal(maskedTextBox2.Text);
                cuscommand.InsertCommand.Parameters.Add("@loanamt", SqlDbType.Decimal).Value = Convert.ToDecimal(maskedTextBox3.Text);
                cuscommand.InsertCommand.Parameters.Add("@surpacct", SqlDbType.VarChar).Value = comboBox4.SelectedValue; 

                ndConnHandle1.Open();
                cuscommand.InsertCommand.ExecuteNonQuery();
                ndConnHandle1.Close();
            }
        }

        private void amendUser()
        {
            string dcashacct = Convert.ToInt16(comboBox3.SelectedIndex) > -1 ? comboBox3.SelectedValue.ToString() : string.Empty;
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                string cquery = "update susers set username=@username,dateforce=@ldateforce,branchid=@branid,cashaccont=@lcashaccont,";
                cquery += "accesslvl=@baserole,debtlimitamt=@debtamt,credlimitamt=@credamt,loanlimitamt=@loanamt,surpaccont=@surpacct where oprcode=@userid";

                SqlDataAdapter cuscommand = new SqlDataAdapter();
                cuscommand.UpdateCommand = new SqlCommand(cquery, ndConnHandle1);
                cuscommand.UpdateCommand.Parameters.Add("@username", SqlDbType.VarChar).Value = textBox3.Text.Trim();
                cuscommand.UpdateCommand.Parameters.Add("@ldateforce", SqlDbType.DateTime).Value = forceChange.Value;
                cuscommand.UpdateCommand.Parameters.Add("@branid", SqlDbType.Int).Value = Convert.ToInt16(comboBox2.SelectedValue);
                cuscommand.UpdateCommand.Parameters.Add("@lcashaccont", SqlDbType.VarChar).Value = dcashacct;
                cuscommand.UpdateCommand.Parameters.Add("@userid", SqlDbType.VarChar).Value = textBox2.Text.Trim();
                cuscommand.UpdateCommand.Parameters.Add("@baserole", SqlDbType.Int).Value = gnUserBaseRole;
                cuscommand.UpdateCommand.Parameters.Add("@debtamt", SqlDbType.Decimal).Value = Convert.ToDecimal(maskedTextBox1.Text);
                cuscommand.UpdateCommand.Parameters.Add("@credamt", SqlDbType.Decimal).Value = Convert.ToDecimal(maskedTextBox2.Text);
                cuscommand.UpdateCommand.Parameters.Add("@loanamt", SqlDbType.Decimal).Value = Convert.ToDecimal(maskedTextBox3.Text);
                cuscommand.UpdateCommand.Parameters.Add("@surpacct", SqlDbType.VarChar).Value = comboBox4.SelectedValue; 

                ndConnHandle1.Open();
                cuscommand.UpdateCommand.ExecuteNonQuery();
                ndConnHandle1.Close();
            }
        }

        private void initvariables()
        {
            int defPassChangeDays = 40;
            textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text = textBox5.Text = maskedTextBox1.Text = maskedTextBox2.Text = maskedTextBox3.Text = "";
            forceChange.Value = textBox12.Text!=""? DateTime.Today.AddDays(Convert.ToDouble(textBox12.Text)) : DateTime.Today.AddDays(defPassChangeDays);
            glNewUser = true;
            comboBox1.SelectedIndex = comboBox2.SelectedIndex = comboBox3.SelectedIndex = comboBox4.SelectedIndex = comboBox12.SelectedIndex = -1;
            radioButton1.Checked = radioButton11.Checked = textBox4.Enabled = textBox5.Enabled = true;
            radioButton2.Checked = radioButton3.Checked = radioButton4.Checked = radioButton10.Checked = saveButton.Enabled = checkBox1.Checked = checkBox3.Checked = checkBox4.Checked= false;
            saveButton.BackColor = Color.Gainsboro;
        }


        private void getUserDetails(string duserid)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                userview.Clear();
                ndConnHandle.Open();
                string dsql = "exec tsp_GetUserList_One  " + ncompid + ",'" + duserid + "'";
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
//                DataTable userview = new DataTable();
                da.Fill(userview);
                if (userview.Rows.Count > 0)
                {
                    comboBox1.SelectedValue = Convert.ToInt16(userview.Rows[0]["compid"]);
                    comboBox2.SelectedValue = Convert.ToInt16(userview.Rows[0]["branchid"]);
                    comboBox3.SelectedValue = userview.Rows[0]["cashaccont"].ToString();
                    comboBox4.SelectedValue = userview.Rows[0]["surpaccont"].ToString();
                    comboBox12.SelectedValue = userview.Rows[0]["accesslvl"].ToString();
                    gnUserBaseRole = Convert.ToInt16(userview.Rows[0]["accesslvl"]);
                    textBox1.Text = userview.Rows[0]["staffno"].ToString();
                    textBox2.Text = userview.Rows[0]["oprcode"].ToString();
                    textBox3.Text = userview.Rows[0]["username"].ToString();
                    textBox4.Text = userview.Rows[0]["userpassword"].ToString();
                    textBox5.Text = userview.Rows[0]["userpassword"].ToString();
                    maskedTextBox1.Text = Convert.ToDecimal(userview.Rows[0]["debtlimitamt"]).ToString("N2");
                    maskedTextBox2.Text = Convert.ToDecimal(userview.Rows[0]["credlimitamt"]).ToString("N2");
                    maskedTextBox3.Text = Convert.ToDecimal(userview.Rows[0]["loanlimitamt"]).ToString("N2");
                    checkBox1.Checked = Convert.ToDecimal(maskedTextBox1.Text) > 0.00m ? true : false;
                    checkBox3.Checked = Convert.ToDecimal(maskedTextBox2.Text) > 0.00m ? true : false;
                    checkBox4.Checked = Convert.ToDecimal(maskedTextBox3.Text) > 0.00m ? true : false;
                }
                else
                {
                   // MessageBox.Show("Details incomplete, please complete");
                    //   MessageBox.Show("Ooops!, what happened to the staff ?");
                }
            }
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox5.Focused)
            {
                string dstaffno = comboBox5.SelectedValue.ToString().Trim();
                getUserDetails(dstaffno);
            }
        }



        private void userGrid_Click(object sender, EventArgs e)
        {
            if (radioButton10.Checked) //existing user
            {
                button4.Enabled = button5.Enabled = button6.Enabled = true;
                maskedTextBox1.Enabled = maskedTextBox2.Enabled = maskedTextBox3.Enabled = checkBox1.Checked = checkBox3.Checked = checkBox4.Checked = false;
                button4.BackColor = button5.BackColor = button6.BackColor = Color.LawnGreen;
                string dstaffno = userView.Rows[userGrid.CurrentCell.RowIndex]["oprcode"].ToString();
                getUserDetails(dstaffno);
            }
            else //new user
            {
                button4.Enabled = button5.Enabled = button6.Enabled = false;
                maskedTextBox1.Enabled = maskedTextBox2.Enabled = maskedTextBox3.Enabled = checkBox1.Checked = checkBox3.Checked = checkBox4.Checked = true;
                initvariables();
                button4.BackColor = button5.BackColor = button6.BackColor = Color.Gainsboro;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string duser = userView.Rows[userGrid.CurrentCell.RowIndex]["oprcode"].ToString();
            getUserDetails(duser);
            if (MessageBox.Show("Do you want to disable user", "User Disable", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
                {
                    string cquery = "update susers set active=0 where oprcode = @userid ";
                    SqlDataAdapter uscommand = new SqlDataAdapter();
                    uscommand.UpdateCommand = new SqlCommand(cquery, ndConnHandle1);
                    uscommand.UpdateCommand.Parameters.Add("@userid ", SqlDbType.VarChar).Value = duser;
                    ndConnHandle1.Open();
                    uscommand.UpdateCommand.ExecuteNonQuery();
                    ndConnHandle1.Close();
                }
            }
            button4.Enabled = button5.Enabled = button6.Enabled = false;
            initvariables();
            button4.BackColor = button5.BackColor = button6.BackColor = Color.Gainsboro;
            getBasicDetails();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            glNewUser = false;
            maskedTextBox1.Enabled = maskedTextBox2.Enabled = maskedTextBox3.Enabled = checkBox1.Enabled = checkBox3.Enabled = checkBox4.Enabled = comboBox12.Enabled =
            comboBox3.Enabled = comboBox4.Enabled = comboBox2.Enabled = textBox2.Enabled = textBox3.Enabled = radioButton1.Enabled = radioButton2.Enabled = radioButton3.Enabled = radioButton4.Enabled =
            textBox4.Enabled = textBox5.Enabled = comboBox1.Enabled = textBox1.Enabled = true;

//            maskedTextBox1.Enabled = maskedTextBox2.Enabled = maskedTextBox3.Enabled = checkBox1.Enabled = checkBox3.Enabled = checkBox4.Enabled = true;
            string dstaffno = userView.Rows[userGrid.CurrentCell.RowIndex]["oprcode"].ToString();
            getUserDetails(dstaffno);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string duser = userView.Rows[userGrid.CurrentCell.RowIndex]["oprcode"].ToString();
            string dpassw = tclassChkpassWord.dpassWord("default@12345");
            getUserDetails(duser);
            if (MessageBox.Show("Do you want to reset user's password", "User Password Reset", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                MessageBox.Show("We will reset the password to default@12345 which is " + dpassw);
                using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
                {
                    string cquery = "update susers set userpassword = @passw where oprcode = @userid ";
                    SqlDataAdapter uscommand = new SqlDataAdapter();
                    uscommand.UpdateCommand = new SqlCommand(cquery, ndConnHandle1);
                    uscommand.UpdateCommand.Parameters.Add("@userid ", SqlDbType.VarChar).Value = duser;
                    uscommand.UpdateCommand.Parameters.Add("@passw ", SqlDbType.VarChar).Value = dpassw;
                    ndConnHandle1.Open();
                    uscommand.UpdateCommand.ExecuteNonQuery();
                    ndConnHandle1.Close();
                }
            }
            button4.Enabled = button5.Enabled = button6.Enabled = false;
            initvariables();
            button4.BackColor = button5.BackColor = button6.BackColor = Color.Gainsboro;
            getBasicDetails();
        }

        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton11.Checked)
            {
                button4.Enabled = button5.Enabled = button6.Enabled = false;
                maskedTextBox1.Enabled = maskedTextBox2.Enabled = maskedTextBox3.Enabled = checkBox1.Enabled = checkBox3.Enabled = checkBox4.Enabled = comboBox12.Enabled =
                comboBox3.Enabled = comboBox4.Enabled = comboBox2.Enabled = textBox2.Enabled = textBox3.Enabled = radioButton1.Enabled = radioButton2.Enabled = radioButton3.Enabled = radioButton4.Enabled = 
                textBox4.Enabled = textBox5.Enabled = comboBox1.Enabled = textBox1.Enabled = true;
                initvariables();
                button4.BackColor = button5.BackColor = button6.BackColor = Color.Gainsboro;
            }
        }

        private void groupMembers(int groupid)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                grpuview.Clear();
                string dsql4 = "exec tsp_Group_Users_One " + ncompid + "," + groupid;
                SqlDataAdapter dac4 = new SqlDataAdapter(dsql4, ndConnHandle);
                dac4.Fill(grpuview);
                if (grpuview.Rows.Count > 0)
                {
                    groupUserGrid.AutoGenerateColumns = false;
                    groupUserGrid.DataSource = grpuview.DefaultView;
                    groupUserGrid.Columns[0].DataPropertyName = "oprcode";
                    groupUserGrid.Columns[1].DataPropertyName = "username";

                    for (int i = 0; i < 10; i++)
                    {
                        grpuview.Rows.Add();
                    }
                }
            }
        }


        private void UsersInGroup(int groupid)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                usersinview.Clear();
                string dsql4 = "exec tsp_UsersInGroup_One  " + ncompid + "," + groupid;
                SqlDataAdapter dac4 = new SqlDataAdapter(dsql4, ndConnHandle);
                dac4.Fill(usersinview);
                if (usersinview.Rows.Count > 0)
                {
                    comboBox7.DataSource = usersinview.DefaultView;
                    comboBox7.DisplayMember = "username";
                    comboBox7.ValueMember = "usernumb";
                    comboBox7.SelectedIndex = -1;
                }
            }
        }

        private void UsersNotInGroup(int groupid)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                usersninview.Clear();
                string dsql4 = "exec tsp_UsersNotinGroup_One " + ncompid + "," + groupid;
                SqlDataAdapter dac4 = new SqlDataAdapter(dsql4, ndConnHandle);
                dac4.Fill(usersninview);
                if (usersninview.Rows.Count > 0)
                {
                    comboBox7.DataSource = usersninview.DefaultView;
                    comboBox7.DisplayMember = "username";
                    comboBox7.ValueMember = "usernumb";
                    comboBox7.SelectedIndex = -1;
                }
            }
        }


        private void radioButton10_CheckedChanged_1(object sender, EventArgs e)
        {
            maskedTextBox1.Enabled = maskedTextBox2.Enabled = maskedTextBox3.Enabled = checkBox1.Checked = checkBox3.Checked = checkBox4.Checked = false;
            textBox4.Enabled = textBox5.Enabled = comboBox1.Enabled = textBox1.Enabled = false;
            glPassWordMatch = true;
            string dstaffno = userView.Rows[0]["oprcode"].ToString();
            getUserDetails(dstaffno);
        }

        private void tabPage2_Enter(object sender, EventArgs e)
        {
            int grpid = Convert.ToInt16(grpview.Rows[groupGrid.CurrentCell.RowIndex]["groupid"]);
            groupMembers(grpid);
            getgroupRoles();
            getGroupAccessAreas();
            groupAccessAreas(grpid);
        }

        private void groupGrid_Click(object sender, EventArgs e)
        {
            gnGroupID = Convert.ToInt16(grpview.Rows[groupGrid.CurrentCell.RowIndex]["groupid"]);
            groupMembers(gnGroupID);
            getgroupRoles();
            getGroupAccessAreas();
            groupAccessAreas(gnGroupID);
        }



        private void groupRoles(int tngrp)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                newroleview.Clear();
                string dsql5 = "exec tsp_Group_Roles_One " + ncompid + "," + tngrp;
                SqlDataAdapter dac5 = new SqlDataAdapter(dsql5, ndConnHandle);
                dac5.Fill(newroleview);
                if (newroleview.Rows.Count > 0)
                {
                    for (int i = 0; i < newroleview.Rows.Count; i++)
                    {
                        int dqrole = Convert.ToInt16(newroleview.Rows[i]["roleid"]);

                        for (int j = 0; j < grproleview.Rows.Count; j++)
                        {
                            int dvrole = Convert.ToInt16(grproleview.Rows[j]["roleid"]);
                            if (dvrole == dqrole)
                            {
                                groupRolesGrid.Rows[j].Cells["roSelect"].Value = 1;
                            }
                        }
                    }
                }
                else
                {
                    if(grproleview.Rows.Count>0)
                    {
                        for (int k = 0; k < grproleview.Rows.Count; k++)
                        {
                            groupRolesGrid.Rows[k].Cells[0].Value = 0;
                        }
                    }
                }
            }
        }

        private void groupAccessAreas(int tngrp)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                grpAccessView.Clear();
                string grpaccess = "exec tsp_Group_Access_One " + ncompid + "," + tngrp;
                SqlDataAdapter dac51 = new SqlDataAdapter(grpaccess, ndConnHandle);
                dac51.Fill(grpAccessView);
                if (grpAccessView.Rows.Count > 0)
                {
                    for (int i = 0; i < grpAccessView.Rows.Count; i++)
                    {
                        int dqrole = Convert.ToInt16(grpAccessView.Rows[i]["acode"]);

                        for (int j = 0; j < accessview.Rows.Count; j++)
                        {
                            int dvrole = Convert.ToInt16(accessview.Rows[j]["acode"]);
                            if (dvrole == dqrole)
                            {
                                groupAccessGrid.Rows[j].Cells["areaSelect"].Value = 1;
                            }
                        }
                    }
                }
                else
                {
                    if(accessview.Rows.Count >0)
                    {
                        for (int k = 0; k < accessview.Rows.Count; k++)
                        {
                            groupAccessGrid.Rows[k].Cells[0].Value = 0;
                        }
                    }
                }
            }
        }

        private void userRoles(int tnUserNumb)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                newuserroleview.Clear();
                string dsql5 = "exec tsp_User_Roles_One " + ncompid + "," + tnUserNumb;
                SqlDataAdapter dac5 = new SqlDataAdapter(dsql5, ndConnHandle);
                dac5.Fill(newuserroleview);
                if (newuserroleview.Rows.Count > 0)
                {
                    for (int i = 0; i < newuserroleview.Rows.Count; i++)
                    {
                        int dqrole = Convert.ToInt16(newuserroleview.Rows[i]["roleid"]);

                        for (int j = 0; j < userroleview.Rows.Count; j++)
                        {
                            int dvrole = Convert.ToInt16(userroleview.Rows[j]["roleid"]);
                            if (dvrole == dqrole)
                            {
                                userRolesGrid.Rows[j].Cells["usrSelect"].Value = 1;
                            }
                        }
                    }
                }
                else
                {
                    if(userroleview.Rows.Count > 0)
                    {
                        for (int k = 0; k < userroleview.Rows.Count; k++)
                        {
                            userRolesGrid.Rows[k].Cells[0].Value = 0;
                        }
                    }
                }
            }
        }

        private void userAccessAreas(int tngrp)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                usrAccessView.Clear();
                string usraccess1 = "exec tsp_User_Access_One " + ncompid + "," + tngrp;
                SqlDataAdapter dac511 = new SqlDataAdapter(usraccess1, ndConnHandle);
                dac511.Fill(usrAccessView);
                if (usrAccessView.Rows.Count > 0)
                {
                    for (int i = 0; i < usrAccessView.Rows.Count; i++)
                    {
                        int dqrole = Convert.ToInt16(usrAccessView.Rows[i]["acode"]);

                        for (int j = 0; j < useraccessview.Rows.Count; j++)
                        {
                            int dvrole = Convert.ToInt16(useraccessview.Rows[j]["acode"]);
                            if (dvrole == dqrole)
                            {
                                userAccessGrid.Rows[j].Cells["uAreaSelect"].Value = 1;
                            }
                        }
                    }
                }
                else
                {
                    if(useraccessview.Rows.Count>0)
                    {
                        for (int k = 0; k < useraccessview.Rows.Count; k++)
                        {
                            userAccessGrid.Rows[k].Cells[0].Value = 0;
                        }
                    }
                }
            }
        }


        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox6.Focused)
            {
                int grpid = Convert.ToInt16(grpMoview.Rows[groupGrid.CurrentCell.RowIndex]["groupid"]);
                if (radioButton20.Checked) //add user to group
                {
                    UsersNotInGroup(grpid);
                    comboBox7.Enabled = true;
                }
                else                     //remove user from group
                {
                    UsersInGroup(grpid);
                    comboBox7.Enabled = true;
                }
            }
        }


        private void radioButto20_CheckedChanged(object sender, EventArgs e)
        {
            comboBox6.SelectedIndex = comboBox7.SelectedIndex = -1;
            comboBox7.Enabled = false;
        }

        private void radioButton21_CheckedChanged(object sender, EventArgs e)
        {
            comboBox6.SelectedIndex = comboBox7.SelectedIndex = -1;
            comboBox7.Enabled = false;
        }


        private void button9_Click(object sender, EventArgs e)
        {
            int dgrpid = Convert.ToInt16(comboBox6.SelectedValue);
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                if (radioButton20.Checked) //add a user to an existing group
                {
                    string cquery = "insert into group_members (grp_id,usr_id,compid) values (@gnGroupID,@gnUserNumb,@gnCompID)";
                    SqlDataAdapter cuscommand = new SqlDataAdapter();
                    cuscommand.InsertCommand = new SqlCommand(cquery, ndConnHandle1);

                    cuscommand.InsertCommand.Parameters.Add("@gnGroupID", SqlDbType.Int).Value = dgrpid;
                    cuscommand.InsertCommand.Parameters.Add("@gnUserNumb", SqlDbType.Int).Value = Convert.ToInt16(comboBox7.SelectedValue);
                    cuscommand.InsertCommand.Parameters.Add("@gnCompID", SqlDbType.Int).Value = ncompid;

                    ndConnHandle1.Open();
                    cuscommand.InsertCommand.ExecuteNonQuery();
                    ndConnHandle1.Close();
                }

                else
                {
                    string cquery1 = "delete from group_members where grp_id=@gnGroupID and usr_id=@gnUserNumb";
                    SqlDataAdapter coscommand = new SqlDataAdapter();
                    coscommand.DeleteCommand = new SqlCommand(cquery1, ndConnHandle1);

                    coscommand.DeleteCommand.Parameters.Add("@gnGroupID", SqlDbType.Int).Value = Convert.ToInt16(comboBox6.SelectedValue);
                    coscommand.DeleteCommand.Parameters.Add("@gnUserNumb", SqlDbType.Int).Value = Convert.ToInt16(comboBox7.SelectedValue);
                    coscommand.DeleteCommand.Parameters.Add("@gnCompID", SqlDbType.Int).Value = globalvar.gnCompid;

                    ndConnHandle1.Open();
                    coscommand.DeleteCommand.ExecuteNonQuery();
                    ndConnHandle1.Close();
                }
            }
            groupMembers(dgrpid);
            getgroupRoles();
            getGroupAccessAreas();
            groupAccessAreas(dgrpid);

            groupGrid.Focus();
            button9.Enabled = false;
            button9.BackColor = Color.Gainsboro;
            comboBox7.SelectedIndex = -1;
            comboBox7.Enabled = false;
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox7.Focused)
            {
                button9.Enabled = comboBox7.SelectedIndex > -1 ? true : false;
                button9.BackColor = comboBox7.SelectedIndex > -1 ? Color.LawnGreen : Color.Gainsboro;
            }
        }

        private void groupRolesGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (groupRolesGrid.Focused)
            {
              /*  if (groupRolesGrid.Columns[e.ColumnIndex].Name == "roSelect")
                {
                    bool rolestat = groupRolesGrid.Rows[e.RowIndex].Cells["roSelect"].Value.ToString().Trim() != "" ? (Convert.ToBoolean(groupRolesGrid.Rows[e.RowIndex].Cells["roSelect"].Value) ? true : false) : false;
                    int roleID = Convert.ToInt16(groupRolesGrid.Rows[e.RowIndex].Cells["droleID"].Value);
                    int memid = Convert.ToInt16(grpview.Rows[groupGrid.CurrentCell.RowIndex]["groupid"]);
                    if (rolestat)
                    {
                        bool rolecheck = checkFunctions(roleID, true, memid);
                        if (!rolecheck)
                        {
                            addFunctions(roleID, true, memid);
                        }
                    }
                    else
                    {
                        ridFunctions(roleID, true, memid);
                    }
                }*/
            }
        }
        //lnMemid,lnAreaid,lnroleid,lnFunid,true
        private bool checkFunctions(int tnMemid,int tnAreaid,int tnRoleid,int tnFunid, bool tlmemtype)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                bool glRoleExist = false;
                string rolequery = string.Empty;
                if (tlmemtype)
                {
                    rolequery = "SELECT 1 from member_functions where memid = " + tnMemid + " and memtype = 1 and areaid = " + tnAreaid+ " and RoleID = "+tnRoleid+ " and funid = "+tnFunid;
                }
                else
                {
                    rolequery = "SELECT 1 from member_functions where memid = " + tnMemid + " and memtype = 0 and areaid = " + tnAreaid + " and RoleID = " + tnRoleid + " and funid = " + tnFunid;
                }
                SqlDataAdapter daquery = new SqlDataAdapter(rolequery, ndConnHandle);
                DataTable chkroleview = new DataTable();
                daquery.Fill(chkroleview);
                if (chkroleview.Rows.Count > 0)
                {
                    glRoleExist = true;
                    return glRoleExist;
                }
                else
                {
                    glRoleExist = false;
                    return glRoleExist;
                }
            }
        }

        private void addFunctions(int tnMemid, int tnAreaid, int tnRoleid, int tnFunid, bool tlmemtype)
        {
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                string cquery = "insert into member_functions (memid,areaid,roleid,funid,memtype,effRights) values (@nmemid,@nareaid,@nRoleid,@nfunid,@memtype,1)";

                SqlDataAdapter cuscommand = new SqlDataAdapter();
                cuscommand.InsertCommand = new SqlCommand(cquery, ndConnHandle1);
                cuscommand.InsertCommand.Parameters.Add("@nmemID", SqlDbType.Int).Value = tnMemid;
                cuscommand.InsertCommand.Parameters.Add("@nareaid", SqlDbType.Int).Value = tnAreaid;
                cuscommand.InsertCommand.Parameters.Add("@nRoleid", SqlDbType.Int).Value = tnRoleid;
                cuscommand.InsertCommand.Parameters.Add("@nfunid", SqlDbType.Int).Value = tnFunid;
                cuscommand.InsertCommand.Parameters.Add("@memtype", SqlDbType.Bit).Value = tlmemtype;
                ndConnHandle1.Open();
                cuscommand.InsertCommand.ExecuteNonQuery();
                ndConnHandle1.Close();
            }
        }

        private void ridFunctions(int tnMemid, int tnAreaid, int tnRoleid, int tnFunid, bool tlmemtype)
        {
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {

                MessageBox.Show("This is the start 6" + tnMemid + tnAreaid + tnRoleid + tnFunid + tlmemtype);
                string cquery1 = "delete from member_functions where memid=@nmemid and areaid = @nareaid and roleid = @nRoleid and funid = @nfunid and memtype = @memtype";
                SqlDataAdapter coscommand = new SqlDataAdapter();
                coscommand.DeleteCommand = new SqlCommand(cquery1, ndConnHandle1);

                coscommand.DeleteCommand.Parameters.Add("@nmemid", SqlDbType.Int).Value = tnMemid;
                coscommand.DeleteCommand.Parameters.Add("@nareaid", SqlDbType.Int).Value = tnAreaid;
                coscommand.DeleteCommand.Parameters.Add("@nRoleid", SqlDbType.Int).Value = tnRoleid;
                coscommand.DeleteCommand.Parameters.Add("@nfunid", SqlDbType.Int).Value = tnFunid;
                coscommand.DeleteCommand.Parameters.Add("@memtype", SqlDbType.Bit).Value = tlmemtype;

                ndConnHandle1.Open();
                coscommand.DeleteCommand.ExecuteNonQuery();
                ndConnHandle1.Close();
            }
        }
        private void groupRolesGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            groupRolesGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }


        private void tabPage4_Enter(object sender, EventArgs e)
        {
                getUserList();
                int lnUserNumb = Convert.ToInt16(userlistview.Rows[0]["usernumb"]);
                groupsBelong(lnUserNumb);
                getuserRoles();
                getUserAccessAreas();
                getGroupAccessAreas();
                gnGroupID = Convert.ToInt16(grpbelongview.Rows[groupBelongGrid.CurrentCell.RowIndex]["groupid"]);
                UsergroupAccessAreas(gnGroupID);
        }

        private void getUserList()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                userlistview.Clear();
                ndConnHandle.Open();
                string dsql = "exec tsp_getActiveUserList  " + ncompid;
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(userlistview);
                if (userlistview.Rows.Count > 0)
                {
                    userListGrid.AutoGenerateColumns = false;
                    userListGrid.DataSource = userlistview.DefaultView;
                    userListGrid.Columns[0].DataPropertyName = "oprcode";
                    userListGrid.Columns[1].DataPropertyName = "username";

                    for (int i = 0; i < 5; i++)
                    {
                        userlistview.Rows.Add();
                    }
                    userListGrid.Focus();
                }
            }
        }



        private void groupsBelong(int tnUserNumb)
        {
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                grpbelongview.Clear();
                string dsql3g = "exec tsp_GetGroups_Belong " + ncompid + "," + tnUserNumb;
                SqlDataAdapter dac3g = new SqlDataAdapter(dsql3g, ndConnHandle1);
                dac3g.Fill(grpbelongview);
                if (grpbelongview.Rows.Count > 0)
                {
                    groupBelongGrid.AutoGenerateColumns = false;
                    groupBelongGrid.DataSource = grpbelongview.DefaultView;
                    groupBelongGrid.Columns[0].DataPropertyName = "groupname";
                    groupBelongGrid.Columns[1].DataPropertyName = "groupid";

                    for (int i = 0; i < 2; i++)
                    {
                        grpbelongview.Rows.Add();
                    }

                    gnGroupID = Convert.ToInt16(grpbelongview.Rows[0]["groupid"]);
                    UsergroupAccessAreas(gnGroupID);
                    userListGrid.Focus();
                }else { MessageBox.Show("User does not belong to any group, strange. Inform IT DEPT immediately"); }
            }
        }
        private void userRolesGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            userRolesGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void userRolesGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (userRolesGrid.Focused)
            {
                //if (userRolesGrid.Columns[e.ColumnIndex].Name == "usrSelect")
                //{
                //    bool rolestat = userRolesGrid.Rows[e.RowIndex].Cells["usrSelect"].Value.ToString().Trim() != "" ? (Convert.ToBoolean(userRolesGrid.Rows[e.RowIndex].Cells["usrSelect"].Value) ? true : false) : false;
                //    int roleID = Convert.ToInt16(userRolesGrid.Rows[e.RowIndex].Cells["eroleid"].Value);
                //    int memid = Convert.ToInt16(userlistview.Rows[userRolesGrid.CurrentCell.RowIndex]["usernumb"]);
                //    if (rolestat)
                //    {
                //        bool rolecheck = checkFunctions(roleID, false, memid);
                //        if (!rolecheck)
                //        {
                //            addFunctions(roleID, false, memid);
                //        }
                //    }
                //    else
                //    {
                //        ridFunctions(roleID, false, memid);
                //    }
                //}
            }
        }

        private void comboBox9_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox9.Focused)
            {
                if (Convert.ToInt16(comboBox9.SelectedIndex) > -1)
                {
                    getuserRoles();
                    userRolesGrid.Enabled = true;
                    userRoles(Convert.ToInt16(comboBox9.SelectedValue));
                }
                else
                {
                    userRolesGrid.Enabled = false;
                }
            }
        }

        //private void dataGridView1_Click(object sender, EventArgs e)
        //{
        //    gnGroupID = Convert.ToInt16(grpview.Rows[groupGrid.CurrentCell.RowIndex]["groupid"]);
        //    groupMembers(gnGroupID);
        //    getgroupRoles();
        //    groupRoles(gnGroupID);
        //}

        private void userListGrid_Click(object sender, EventArgs e)
        {
            gnUserNumb = Convert.ToInt16(userlistview.Rows[userListGrid.CurrentCell.RowIndex]["usernumb"]);
            groupsBelong(gnUserNumb);
            getuserRoles();
            getUserAccessAreas();
            if(grpbelongview.Rows.Count > 0)
            {
                gnGroupID = Convert.ToInt16(grpbelongview.Rows[groupBelongGrid.CurrentCell.RowIndex]["groupid"]);
                UsergroupAccessAreas(gnGroupID);
            }
        }

        private void groupAccessGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (groupAccessGrid.Focused)
            {
                if (groupAccessGrid.Columns[e.ColumnIndex].Name == "areaSelect")
                {
                    bool areastat = groupAccessGrid.Rows[e.RowIndex].Cells["areaSelect"].Value.ToString().Trim() != "" ? (Convert.ToBoolean(groupAccessGrid.Rows[e.RowIndex].Cells["areaSelect"].Value) ? true : false) : false;
                    int areaID = Convert.ToInt16(groupAccessGrid.Rows[e.RowIndex].Cells["dareaid"].Value);
                    int grpid = Convert.ToInt16(grpview.Rows[groupGrid.CurrentCell.RowIndex]["groupid"]);
                    if (areastat)
                    {
                        bool areacheck = checkAreas(areaID, true, grpid);
                        if (!areacheck)
                        {
                            addAccess(areaID, true, grpid);
                        }
                    }
                    else
                    {
                        ridAccess(areaID, true, grpid);
                    }
                }
            }
        }

        private bool checkAreas(int tnareaid, bool tlgrpusr, int tngrpid)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                bool glGrpExist = false;
                string grpquery = string.Empty;
                if (tlgrpusr)
                {
                    grpquery = "SELECT 1 from user_access where mod_id = " + tnareaid + " and grpind = 1 and grp_id = " + tngrpid;  //group areas
                }
                else
                {
                    grpquery = "SELECT 1 from user_access where mod_id = " + tnareaid + " and grpind = 0 and grp_id = " + tngrpid;  //individual user areas
                }
                SqlDataAdapter daquery = new SqlDataAdapter(grpquery, ndConnHandle);
                DataTable chkareaview = new DataTable();
                daquery.Fill(chkareaview);
                if (chkareaview.Rows.Count > 0)
                {
                    glGrpExist = true;
                    return glGrpExist;
                }
                else
                {
                    glGrpExist = false;
                    return glGrpExist;
                }
            }
        }

        private void addAccess(int dmodid, bool grpusr, int dgrpid)
        {
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                string cquery = "insert into user_access (sol_id,grp_id,mod_id,grpind,compid) values (@gnSolID,@gnGrpID,@gnModID,@dgrpind,@gnCompid)";

                SqlDataAdapter cuscommand = new SqlDataAdapter();
                cuscommand.InsertCommand = new SqlCommand(cquery, ndConnHandle1);
                cuscommand.InsertCommand.Parameters.Add("@gnSolID", SqlDbType.Int).Value = 8;
                cuscommand.InsertCommand.Parameters.Add("@gnModID", SqlDbType.Int).Value = dmodid;
                cuscommand.InsertCommand.Parameters.Add("@gnGrpID", SqlDbType.Int).Value = dgrpid;
                cuscommand.InsertCommand.Parameters.Add("@dgrpind", SqlDbType.Bit).Value = grpusr;
                cuscommand.InsertCommand.Parameters.Add("@gnCompid", SqlDbType.Int).Value = ncompid;
                ndConnHandle1.Open();
                cuscommand.InsertCommand.ExecuteNonQuery();
                ndConnHandle1.Close();
            }
        }

        private void ridAccess(int dmodid, bool grpusr, int dgrpid)
        {
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                string cquery1 = "delete from user_access where grp_id=@gnGrpID and mod_id=@gnModID and grpind=@dgrpind and  compid = @gnCompid";
                SqlDataAdapter coscommand = new SqlDataAdapter();
                coscommand.DeleteCommand = new SqlCommand(cquery1, ndConnHandle1);

                coscommand.DeleteCommand.Parameters.Add("@gnModID", SqlDbType.Int).Value = dmodid;
                coscommand.DeleteCommand.Parameters.Add("@dgrpind", SqlDbType.Bit).Value = grpusr;
                coscommand.DeleteCommand.Parameters.Add("@gnGrpID", SqlDbType.Int).Value = dgrpid;
                coscommand.DeleteCommand.Parameters.Add("@gnCompid", SqlDbType.Int).Value = ncompid;

                ndConnHandle1.Open();
                coscommand.DeleteCommand.ExecuteNonQuery();
                ndConnHandle1.Close();
            }
        }

        private void groupAccessGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            groupAccessGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void userAccessGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void userAccessGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            userAccessGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            int lnPassComp = (radioButton5.Checked ? 1 : (radioButton6.Checked ? 2 : 3));
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                string cquery1 = "update secpara set minpass=@dminpass,passvalid=@dpassvalid,loctrials=@dloctrials,locduration=@dlocdur,passcomp=@dpasscomp,conclog=@docnclog,fprint=@dfinger,faceprint=@dface,mobilecheck=@dmobile";
                SqlDataAdapter coscommand = new SqlDataAdapter();
                coscommand.UpdateCommand = new SqlCommand(cquery1, ndConnHandle1);

                coscommand.UpdateCommand.Parameters.Add("@dminpass", SqlDbType.Int).Value = Convert.ToInt16(textBox11.Text);
                coscommand.UpdateCommand.Parameters.Add("@dpassvalid", SqlDbType.Int).Value = Convert.ToInt16(textBox12.Text);
                coscommand.UpdateCommand.Parameters.Add("@dloctrials", SqlDbType.Int).Value = Convert.ToInt16(textBox13.Text);
                coscommand.UpdateCommand.Parameters.Add("@dlocdur", SqlDbType.Int).Value = Convert.ToInt16(textBox14.Text);
                coscommand.UpdateCommand.Parameters.Add("@dpasscomp", SqlDbType.Int).Value = lnPassComp;
                coscommand.UpdateCommand.Parameters.Add("@docnclog", SqlDbType.Bit).Value = Convert.ToBoolean(checkBox2.Checked);
                coscommand.UpdateCommand.Parameters.Add("@dfinger", SqlDbType.Bit).Value = Convert.ToBoolean(checkBox6.Checked);
                coscommand.UpdateCommand.Parameters.Add("@dface", SqlDbType.Bit).Value = Convert.ToBoolean(checkBox7.Checked);
                coscommand.UpdateCommand.Parameters.Add("@dmobile", SqlDbType.Bit).Value = Convert.ToBoolean(checkBox8.Checked);

                ndConnHandle1.Open();
                coscommand.UpdateCommand.ExecuteNonQuery();
                ndConnHandle1.Close();
            }

        }


        private void tabPage5_Enter(object sender, EventArgs e)
        {
            textBox11.Focus();
            getactiveUsers();
            deptConfiguration();
            secSaveButton.Enabled = true;
            secSaveButton.BackColor = Color.LawnGreen;
        }

        private void deptConfiguration()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string userstringd = "exec tsp_GetDeptconf  " + ncompid;
                SqlDataAdapter useradaptd = new SqlDataAdapter(userstringd, ndConnHandle);
                DataTable depConView = new DataTable();
                useradaptd.Fill(depConView);
                if (depConView.Rows.Count > 0)
                {
                    depConfGrid.AutoGenerateColumns = false;
                    depConfGrid.DataSource = depConView.DefaultView;
                    depConfGrid.Columns[0].DataPropertyName = "dep_name";
                    depConfGrid.Columns[1].DataPropertyName = "hodname";
                    depConfGrid.Columns[2].DataPropertyName = "proxy1";
                    depConfGrid.Columns[3].DataPropertyName = "proxy2";
                    for (int i = 0; i < 10; i++)
                    {
                        depConView.Rows.Add();
                    }
                }
            }
        }
    
  //      sn = SQLExec(gnConnHandle, "exec tsp_GetDeptconf ?gnCompid", "deptCView")

        
        private void button96_Click(object sender, EventArgs e)
        {
            /*
             With Thisform.pageframe1.pAGE4
	gcInputStat='A'
	.combo1.SetFocus
	Thisform.definegrid(4)
	sn=SQLExec(gnConnHandle,"exec tsp_GetDeptconf ?gnCompid","departView")
	If !(sn>0 And Reccount()>0)
		=sysmsg('No Departments Configuration found, Inform IT DEPT')
	Else
		.combo1.RowSource ='allt(departView.dep_name),dep_id'
	Endif
	.Refresh
Endwith
             */
        }

        private void secSaveButton_Click(object sender, EventArgs e)
        {
            int lnPassComp = (radioButton5.Checked ? 1 : (radioButton6.Checked ? 2 : 3));
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                string cquery1 = "update secpara set minpass=@dminpass,passvalid=@dpassvalid,loctrials=@dloctrials,locduration=@dlocdur,passcomp=@dpasscomp,conclog=@docnclog,fprint=@dfinger,faceprint=@dface,mobilecheck=@dmobile";
                SqlDataAdapter coscommand = new SqlDataAdapter();
                coscommand.UpdateCommand = new SqlCommand(cquery1, ndConnHandle1);

                coscommand.UpdateCommand.Parameters.Add("@dminpass", SqlDbType.Int).Value = Convert.ToInt16(textBox11.Text);
                coscommand.UpdateCommand.Parameters.Add("@dpassvalid", SqlDbType.Int).Value = Convert.ToInt16(textBox12.Text);
                coscommand.UpdateCommand.Parameters.Add("@dloctrials", SqlDbType.Int).Value = Convert.ToInt16(textBox13.Text);
                coscommand.UpdateCommand.Parameters.Add("@dlocdur", SqlDbType.Int).Value = Convert.ToInt16(textBox14.Text);
                coscommand.UpdateCommand.Parameters.Add("@dpasscomp", SqlDbType.Int).Value = lnPassComp;
                coscommand.UpdateCommand.Parameters.Add("@docnclog", SqlDbType.Bit).Value = Convert.ToBoolean(checkBox2.Checked);
                coscommand.UpdateCommand.Parameters.Add("@dfinger", SqlDbType.Bit).Value = Convert.ToBoolean(checkBox6.Checked);
                coscommand.UpdateCommand.Parameters.Add("@dface", SqlDbType.Bit).Value = Convert.ToBoolean(checkBox7.Checked);
                coscommand.UpdateCommand.Parameters.Add("@dmobile", SqlDbType.Bit).Value = Convert.ToBoolean(checkBox8.Checked);

                ndConnHandle1.Open();
                coscommand.UpdateCommand.ExecuteNonQuery();
                ndConnHandle1.Close();
                MessageBox.Show("Parameters updated");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox8.Focused )
            {
                if(comboBox8.SelectedIndex > -1 && comboBox9.SelectedIndex > -1)
                {
                    depSaveButton.Enabled = true;
                    depSaveButton.BackColor = Color.LawnGreen;
                }
                else
                {
                    depSaveButton.Enabled = false;
                    depSaveButton.BackColor = Color.Gainsboro;
                }
            }
        }

        private void comboBox9_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (comboBox9.Focused)
            {

                if (comboBox8.SelectedIndex > -1 && comboBox9.SelectedIndex > -1)
                {
                    depSaveButton.Enabled = true;
                    depSaveButton.BackColor = Color.LawnGreen;
                }
                else
                {
                    depSaveButton.Enabled = false;
                    depSaveButton.BackColor = Color.Gainsboro;
                }
            }
        }

        private void depSaveButton_Click(object sender, EventArgs e)
        {
            int lndept = Convert.ToInt16(comboBox8.SelectedValue);
            bool dephere = checkDept(lndept);
            if(dephere )
            {
                updDeptConf(lndept);
            }
            else
            {
                addDeptConf(lndept);
            }
            deptConfiguration();
            comboBox8.SelectedIndex = comboBox9.SelectedIndex = comboBox10.SelectedIndex = comboBox11.SelectedIndex = -1;
            depSaveButton.Enabled = false;
            depSaveButton.BackColor = Color.Gainsboro;
        }

        private bool checkDept(int depid)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                bool glDeptExist = false;
                string deptquery = "SELECT 1 from deptconf where dep_id = " + depid; 
                SqlDataAdapter dequery = new SqlDataAdapter(deptquery, ndConnHandle);
                DataTable chkdeptview = new DataTable();
                dequery.Fill(chkdeptview);
                if (chkdeptview.Rows.Count > 0)
                {
                    glDeptExist = true;
                    return glDeptExist;
                }
                else
                {
                    glDeptExist = false;
                    return glDeptExist;
                }
            }
        }

        private void addDeptConf(int ddept)
        {
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                string cqueryd = "insert into deptconf (dep_id,hodno,proxy1,proxy2,compid) values (@lnDepid,@lcHod,@lcProxy1,@lcProxy2,@gnCompid)";

                SqlDataAdapter depcommand = new SqlDataAdapter();
                depcommand.InsertCommand = new SqlCommand(cqueryd, ndConnHandle1);
                depcommand.InsertCommand.Parameters.Add("@lnDepid", SqlDbType.Int).Value = ddept;
                depcommand.InsertCommand.Parameters.Add("@lcHod", SqlDbType.VarChar).Value = comboBox9.SelectedValue.ToString().Trim()!=""? comboBox9.SelectedValue.ToString() : "";
                depcommand.InsertCommand.Parameters.Add("@lcProxy1", SqlDbType.VarChar).Value = comboBox10.SelectedValue.ToString().Trim() != "" ? comboBox10.SelectedValue.ToString() : "";
                depcommand.InsertCommand.Parameters.Add("@lcProxy2", SqlDbType.VarChar).Value = comboBox10.SelectedValue.ToString().Trim() != "" ? comboBox11.SelectedValue.ToString() : "";
                depcommand.InsertCommand.Parameters.Add("@gnCompid", SqlDbType.Int).Value = ncompid;
                ndConnHandle1.Open();
                depcommand.InsertCommand.ExecuteNonQuery();
                ndConnHandle1.Close();
            }
        }

        private void updDeptConf(int ddept)
        {
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                string cqueryd = "update deptconf set hodno=@lcHod,proxy1=@lcProxy1,proxy2=@lcProxy2 where dep_id=@lnDepid";

                SqlDataAdapter depcommand1 = new SqlDataAdapter();
                depcommand1.UpdateCommand = new SqlCommand(cqueryd, ndConnHandle1);
                depcommand1.UpdateCommand.Parameters.Add("@lnDepid", SqlDbType.Int).Value = ddept;
                depcommand1.UpdateCommand.Parameters.Add("@lcHod", SqlDbType.VarChar).Value = comboBox9.SelectedValue.ToString();
                depcommand1.UpdateCommand.Parameters.Add("@lcProxy1", SqlDbType.VarChar).Value = comboBox10.SelectedValue.ToString();
                depcommand1.UpdateCommand.Parameters.Add("@lcProxy2", SqlDbType.VarChar).Value = comboBox11.SelectedValue.ToString();
                ndConnHandle1.Open();
                depcommand1.UpdateCommand.ExecuteNonQuery();
                ndConnHandle1.Close();
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            //systemRoles sr = new systemRoles(cs, ncompid);
            //sr.ShowDialog();
        }

        private void groupRolesGrid_Click(object sender, EventArgs e)
        {
            gnRoleID = Convert.ToInt16(grproleview.Rows[groupRolesGrid.CurrentCell.RowIndex]["roleid"]);
            roleFunctions(gnRoleID);
        }

        private void roleFunctions(int tnrole)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                newfunview.Clear();
                areaFunctions(gnAreaID);
                string dfunstr = "exec tsp_RoleFunctions_One  " + ncompid + "," + gnAreaID + "," + tnrole;
                SqlDataAdapter rolefunc = new SqlDataAdapter(dfunstr, ndConnHandle);
                rolefunc.Fill(newfunview);
                if (newfunview.Rows.Count > 0)
                {
                    int funrow = newfunview.Rows.Count;
                    areaFunctions(gnAreaID);
                    for (int i = 0; i < funrow; i++)
                    {
                        int dqrole = Convert.ToInt16(newfunview.Rows[i]["funid"]);

                        for (int j = 0; j < funview.Rows.Count; j++)
                        {
                            int dvrole = Convert.ToInt16(funview.Rows[j]["funid"]);
                            if (dvrole == dqrole)
                            {
                                grpFunGrid.Rows[j].Cells["funSelect"].Value = 1;
                            }
                        }
                    }
                }
                else
                {
                    if(funview.Rows.Count>0)
                    {
                        for (int k = 0; k < funview.Rows.Count; k++)
                        {
                            grpFunGrid.Rows[k].Cells[0].Value = 0;
                        }
                    }
                }
            }
        }

        private void groupAccessGrid_Click(object sender, EventArgs e)
        {
            bool accSelected = !Convert.IsDBNull(accessview.Rows[groupAccessGrid.CurrentCell.RowIndex]["accSelect"]) ? Convert.ToBoolean(accessview.Rows[groupAccessGrid.CurrentCell.RowIndex]["accSelect"]) : false;
            groupRolesGrid.Enabled = accSelected ? true : false;
            grpFunGrid.Enabled = accSelected ? true : false; 
            gnAreaID = Convert.ToInt16(accessview.Rows[groupAccessGrid.CurrentCell.RowIndex]["acode"]);
            areaFunctions(gnAreaID);
        }

        private void grpFunGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (grpFunGrid.Focused)
            {
                if (grpFunGrid.Columns[e.ColumnIndex].Name == "funSelect")
                {
//                    MessageBox.Show("Inside grpFunGrid_CellValueChanged step 0 and group rows "+ grpview.Rows.Count);
                    bool rolestat = grpFunGrid.Rows[e.RowIndex].Cells["funSelect"].Value.ToString().Trim() != "" ? (Convert.ToBoolean(grpFunGrid.Rows[e.RowIndex].Cells["funSelect"].Value) ? true : false) : false;
                    int lnMemid = Convert.ToInt16(grpview.Rows[groupGrid.CurrentCell.RowIndex]["groupid"]);

  //                  MessageBox.Show("Inside grpFunGrid_CellValueChanged step 1 and groupid is "+lnMemid);
                    int lnAreaid = Convert.ToInt16(accessview.Rows[groupAccessGrid.CurrentCell.RowIndex]["acode"]);
                    int lnroleid = Convert.ToInt16(grproleview.Rows[groupRolesGrid.CurrentCell.RowIndex]["roleid"]);

    //                MessageBox.Show("Inside grpFunGrid_CellValueChanged step 2");
                    int lnFunid = Convert.ToInt16(funview.Rows[grpFunGrid.CurrentCell.RowIndex]["funid"]); 
                    if (rolestat)
                    {
                        bool rolecheck = checkFunctions(lnMemid,lnAreaid,lnroleid,lnFunid,true);
                        if (!rolecheck)
                        {
                            addFunctions(lnMemid, lnAreaid, lnroleid, lnFunid, true);
                        }
                    }
                    else
                    {
                        ridFunctions(lnMemid, lnAreaid, lnroleid, lnFunid, true);
                    }
                }
            }
        }

        private void grpFunGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            grpFunGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void groupBelongGrid_Click(object sender, EventArgs e)
        {
            effRightsview.Clear();
            gnGroupID = Convert.ToInt16(grpbelongview.Rows[groupBelongGrid.CurrentCell.RowIndex]["groupid"]);
            getuserRoles();
            getUserAccessAreas();
            //            userAccessAreas(gnGroupID);
            //            getGroupAccessAreas();
            UsergroupAccessAreas(gnGroupID);
        }

        private void UsergroupAccessAreas(int tngrp)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string grpaccess1 = "exec tsp_Group_Access_One " + ncompid + "," + tngrp;
                SqlDataAdapter dac511 = new SqlDataAdapter(grpaccess1, ndConnHandle);
                DataTable grpUserAccessView = new DataTable();
                dac511.Fill(grpUserAccessView);
                if (grpUserAccessView.Rows.Count > 0)
                {
//                    MessageBox.Show("something in usergropuaccess areas for group = " + tngrp);
                    for (int i = 0; i < grpUserAccessView.Rows.Count; i++)
                    {
                        int dqrole = Convert.ToInt16(grpUserAccessView.Rows[i]["acode"]);
                        for (int j = 0; j < useraccessview.Rows.Count; j++)
                        {
                            int dvrole = Convert.ToInt16(useraccessview.Rows[j]["acode"]);

                            if (dvrole == dqrole)
                            {
                                userAccessGrid.Rows[j].Cells["uAreaSelect"].Value = 1;
                            }
                        }
                    }
                }
                else
                {
  //                  MessageBox.Show("Nothing in usergropuaccess areas for group = " + tngrp);
                    if(useraccessview.Rows.Count> 0)
                    {
                        for (int k = 0; k < useraccessview.Rows.Count; k++)
                        {
                            userAccessGrid.Rows[k].Cells[0].Value = 0;
                        }
                    }
                }
            }
        }

        private void comboBox12_SelectedValueChanged(object sender, EventArgs e)
        {
            if(comboBox12.Focused )
            {
                gnUserBaseRole = Convert.ToInt16(comboBox12.SelectedValue);
                AllClear2Go();
            }
        }

        private void userAccessGrid_Click(object sender, EventArgs e)
        {
            if(useraccessview.Rows.Count>0 )
            {
                bool accSelected = Convert.ToBoolean(useraccessview.Rows[userAccessGrid.CurrentCell.RowIndex]["accSelect"]);
                effRightsGrid.Enabled = accSelected ? true : false;
                gnAreaID = Convert.ToInt16(useraccessview.Rows[userAccessGrid.CurrentCell.RowIndex]["acode"]);
                int lnMemid = Convert.ToInt16(userlistview.Rows[userListGrid.CurrentCell.RowIndex]["usernumb"]);
                areaFunctions1(gnAreaID);
                inheritedRights(gnAreaID, lnMemid);
                individualRights(gnAreaID, lnMemid);
            }  
        }

        private void areaFunctions1(int tnAreaid)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                effRightsview.Clear();
                string funtring1 = "exec tsp_AreaFunctions_One  " + ncompid + "," + tnAreaid;
                SqlDataAdapter grpadapt1 = new SqlDataAdapter(funtring1, ndConnHandle);
                grpadapt1.Fill(effRightsview);
                if (effRightsview.Rows.Count > 0)
                {
                    effRightsGrid.AutoGenerateColumns = false;
                    effRightsGrid.DataSource = effRightsview.DefaultView;
                    effRightsGrid.Columns[0].DataPropertyName = "rolesel";
                    effRightsGrid.Columns[1].DataPropertyName = "functioname";
                    effRightsGrid.Columns[2].DataPropertyName = "funid";
                }
                for(int j=0;j<effRightsview.Rows.Count;j++)
                {

                }
            }
        }

        private void inheritedRights(int tnAreaid,int tnUserNumb)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {

                string grpaccess1 = "exec tsp_Inherited_Rights_One " + ncompid + "," + tnAreaid;
                SqlDataAdapter dac511 = new SqlDataAdapter(grpaccess1, ndConnHandle);
                DataTable inheritView = new DataTable();
                dac511.Fill(inheritView);
                if (inheritView.Rows.Count > 0)
                {
                    for (int i = 0; i < inheritView.Rows.Count; i++)
                    {
                        int dqrole = Convert.ToInt16(inheritView.Rows[i]["funid"]);
                        for (int j = 0; j < effRightsview.Rows.Count; j++)
                        {
                            int dvrole = Convert.ToInt16(effRightsview.Rows[j]["funid"]);
                            if (dvrole == dqrole)
                            {
//                                MessageBox.Show("we are doing function in inherited rights " + dvrole);
                                int lnroleid = Convert.ToInt16(userlistview.Rows[userListGrid.CurrentCell.RowIndex]["accesslvl"]);
                                bool rolecheck = checkFunctions(tnUserNumb, tnAreaid, lnroleid, dvrole, false);
                                if (!rolecheck)
                                {
                                    addFunctions(tnUserNumb, tnAreaid, lnroleid, dvrole, false);
                                }
                                effRightsGrid.Rows[j].DefaultCellStyle.BackColor = Color.Gold;
                                effRightsGrid.Rows[j].DefaultCellStyle.ForeColor = Color.Black;
                            }
                        }
                    }
                }
                else
                {
                    if(effRightsview.Rows.Count>0)
                    {
                        for (int k = 0; k < effRightsview.Rows.Count; k++)
                        {
                            effRightsGrid.Rows[k].Cells[0].Value = 0;
                        }
                    }
                }
            }

        }

        private void individualRights(int tnAreaid,int tnUserNumb)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
//                string grpaccess2 = "exec tsp_Individual_Rights_One " + ncompid + "," + tnAreaid + "," + tnUserNumb;
                string grpaccess2 = "exec tsp_Individual_Rights_One " + ncompid + "," +tnUserNumb;
                SqlDataAdapter dac512 = new SqlDataAdapter(grpaccess2, ndConnHandle);
                DataTable indiviView = new DataTable();
                dac512.Fill(indiviView);
                if (indiviView.Rows.Count > 0)
                {
//                    MessageBox.Show("we have found individual rights");
                    for (int i = 0; i < indiviView.Rows.Count; i++)
                    {
                        int dqrole = Convert.ToInt16(indiviView.Rows[i]["funid"]);
                        bool effRight = Convert.ToBoolean(indiviView.Rows[i]["effRights"]);
                        int effrows = effRightsview.Rows.Count;
                        int dfirs = i;
//                        MessageBox.Show("The number of effective view rows is " + effrows);
                        for (int j = 0; j < effRightsview.Rows.Count; j++)
                        {
                            int dvrole = Convert.ToInt16(effRightsview.Rows[j]["funid"]);
                            if (dvrole == dqrole)
                            {
//                                MessageBox.Show("we have brought function "+dqrole+" and found function "+dvrole+" and j is "+j+" and i ="+dfirs );
                                //MessageBox.Show("We will get the colors for row = "+j+" and effective rights = "+effRight);
                                effRightsGrid.Rows[j].Cells["effSelect"].Value = true;// effRight;
                                effRightsGrid.Rows[j].DefaultCellStyle.BackColor = Color.BlueViolet;
                                effRightsGrid.Rows[j].DefaultCellStyle.ForeColor = Color.White;
                            }
                        }
                    }
                }
            }
        }

        private void effRightsGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (effRightsGrid.Focused)
            {

                if (effRightsGrid.Columns[e.ColumnIndex].Name == "effSelect")
                {
                    bool rolestat = effRightsGrid.Rows[e.RowIndex].Cells["effSelect"].Value.ToString().Trim() != "" ? (Convert.ToBoolean(effRightsGrid.Rows[e.RowIndex].Cells["effSelect"].Value) ? true : false) : false;
                    int lnMemid = Convert.ToInt16(userlistview.Rows[userListGrid.CurrentCell.RowIndex]["usernumb"]);
                    int lnAreaid = Convert.ToInt16(useraccessview.Rows[userAccessGrid.CurrentCell.RowIndex]["acode"]);
                    int lnroleid = Convert.ToInt16(userlistview.Rows[userListGrid.CurrentCell.RowIndex]["accesslvl"]);
                    int lnFunid = Convert.ToInt16(effRightsview.Rows[effRightsGrid.CurrentCell.RowIndex]["funid"]);
                    bool llInheritedRights = effRightsGrid.Rows[effRightsGrid.CurrentCell.RowIndex].DefaultCellStyle.BackColor == Color.Gold ? true : false ;

                    if (rolestat) 
                    {
                        bool funcheck = checkFunctions(lnMemid, lnAreaid, lnroleid, lnFunid, false);
                        if (!funcheck)
                        {
                            MessageBox.Show("This is the start 1");
                            if (llInheritedRights)    //inherited rights, will just flag it as active rights , set effRights =1 
                            {
                                enableEffRights(lnMemid, lnAreaid, lnroleid, lnFunid, false);
                            }
                            else
                            {                       //personal rights and will be added
                                MessageBox.Show("This is the start 2");
                                addFunctions(lnMemid, lnAreaid, lnroleid, lnFunid, false);
                                effRightsGrid.Rows[effRightsGrid.CurrentCell.RowIndex].DefaultCellStyle.BackColor = Color.BlueViolet;
                                effRightsGrid.Rows[effRightsGrid.CurrentCell.RowIndex].DefaultCellStyle.ForeColor = Color.White;
                            }
                        }
                        else
                        {
                            MessageBox.Show("This is the start 3");
                            enableEffRights(lnMemid, lnAreaid, lnroleid, lnFunid, false);
                        }
                    }
                    else
                    {
                        if (llInheritedRights )    //inherited rights, will just flag it as inactive rights , set effRights=0 
                        {
                            MessageBox.Show("This is the start 4");
                            disableEffRights(lnMemid, lnAreaid, lnroleid, lnFunid, false);
                        }
                        else      //personal rights and will be removed
                        {
                            MessageBox.Show("This is the start 5  " + lnMemid + lnAreaid + lnroleid + lnFunid);
                            ridFunctions(lnMemid, lnAreaid, lnroleid, lnFunid, false);
                            effRightsGrid.Rows[effRightsGrid.CurrentCell.RowIndex].DefaultCellStyle.BackColor = Color.White;
                            effRightsGrid.Rows[effRightsGrid.CurrentCell.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                        }
                    }
                }
            }
        }

        private void enableEffRights(int tnMemid, int tnAreaid, int tnRoleid, int tnFunid, bool tlmemtype)
        {
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                string cquery1 = "update member_functions set effRights = 1 where memid=@nmemid and areaid = @nareaid and roleid = @nRoleid and funid = @nfunid and memtype = @memtype";
                SqlDataAdapter coscommand = new SqlDataAdapter();
                coscommand.DeleteCommand = new SqlCommand(cquery1, ndConnHandle1);

                coscommand.DeleteCommand.Parameters.Add("@nmemid", SqlDbType.Int).Value = tnMemid;
                coscommand.DeleteCommand.Parameters.Add("@nareaid", SqlDbType.Int).Value = tnAreaid;
                coscommand.DeleteCommand.Parameters.Add("@nRoleid", SqlDbType.Int).Value = tnRoleid;
                coscommand.DeleteCommand.Parameters.Add("@nfunid", SqlDbType.Int).Value = tnFunid;
                coscommand.DeleteCommand.Parameters.Add("@memtype", SqlDbType.Bit).Value = tlmemtype;

                ndConnHandle1.Open();
                coscommand.DeleteCommand.ExecuteNonQuery();
                ndConnHandle1.Close();
            }
        }

        private void disableEffRights(int tnMemid, int tnAreaid, int tnRoleid, int tnFunid, bool tlmemtype)
        {
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                string cquery1 = "update member_functions set effRights=0 where memid=@nmemid and areaid = @nareaid and roleid = @nRoleid and funid = @nfunid and memtype = @memtype";
                SqlDataAdapter coscommand = new SqlDataAdapter();
                coscommand.DeleteCommand = new SqlCommand(cquery1, ndConnHandle1);

                coscommand.DeleteCommand.Parameters.Add("@nmemid", SqlDbType.Int).Value = tnMemid;
                coscommand.DeleteCommand.Parameters.Add("@nareaid", SqlDbType.Int).Value = tnAreaid;
                coscommand.DeleteCommand.Parameters.Add("@nRoleid", SqlDbType.Int).Value = tnRoleid;
                coscommand.DeleteCommand.Parameters.Add("@nfunid", SqlDbType.Int).Value = tnFunid;
                coscommand.DeleteCommand.Parameters.Add("@memtype", SqlDbType.Bit).Value = tlmemtype;

                ndConnHandle1.Open();
                coscommand.DeleteCommand.ExecuteNonQuery();
                ndConnHandle1.Close();
            }
        }

        private void effRightsGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            effRightsGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Focused )
            {
                maskedTextBox1.Enabled = checkBox1.Checked;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Focused)
            {
                maskedTextBox2.Enabled = checkBox3.Checked;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Focused)
            {
                maskedTextBox3.Enabled = checkBox4.Checked;
            }
        }

        private void maskedTextBox1_Validated(object sender, EventArgs e)
        {
            if(maskedTextBox1.Text !="")
            {
                maskedTextBox1.Text = Convert.ToDecimal(maskedTextBox1.Text).ToString("N2");
                AllClear2Go();
            }
        }

        private void maskedTextBox2_Validated(object sender, EventArgs e)
        {
            if (maskedTextBox2.Text != "")
            {
                maskedTextBox2.Text = Convert.ToDecimal(maskedTextBox2.Text).ToString("N2");
                AllClear2Go();
            }
        }

        private void maskedTextBox3_Validated(object sender, EventArgs e)
        {
            if (maskedTextBox3.Text != "")
            {
                maskedTextBox3.Text = Convert.ToDecimal(maskedTextBox3.Text).ToString("N2");
                AllClear2Go();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            onecode d1ocode = new onecode(cs, "Loan Status Setup", "loanstatus", "stat_name");
            d1ocode.ShowDialog();
        }

        private void button15_Click_1(object sender, EventArgs e)
        {
            onecode d1code = new onecode(cs, "Disability Acquired time", "disa_time", "acq_time");
            d1code.ShowDialog();
        }

        private void button97_Click(object sender, EventArgs e)
        {
            onecode d1code = new onecode(cs, "Employment types Setup", "empl_type", "emp_name");
            d1code.ShowDialog();
        }

        private void button84_Click(object sender, EventArgs e)
        {
            trn_institute trn = new TclassLibrary.trn_institute(cs, ncompid, cLocalCaption);
            trn.ShowDialog();
        }

        private void button84_Click_1(object sender, EventArgs e)
        {
            onecode d1code = new onecode(cs, "Qualification types Setup", "qual_typ", "qua_name");
            d1code.ShowDialog();
        }

        private void button98_Click(object sender, EventArgs e)
        {

            onecode d1code = new onecode(cs, "Job Functions ", "jobfunction", "fun_name");
            d1code.ShowDialog();
        }

        private void button99_Click(object sender, EventArgs e)
        {
            onecode d1code = new onecode(cs, "Job Types", "jobtype", "typ_name");
            d1code.ShowDialog();
        }

        private void button100_Click(object sender, EventArgs e)
        {
            onecode d1code = new onecode(cs, "Payroll types Setup", "paytype", "pay_name");
            d1code.ShowDialog();
        }

        private void button99_Click_1(object sender, EventArgs e)
        {
            onecode d1code = new onecode(cs, "Payroll period type Setup", "payper_type", "per_name");
            d1code.ShowDialog();
        }

        private void button102_Click(object sender, EventArgs e)
        {
            onecode d1code = new onecode(cs, "Payroll payment type Setup", "pmt_type", "pmt_name");
            d1code.ShowDialog();
        }

        private void button101_Click(object sender, EventArgs e)
        {
            onecode d1code = new onecode(cs, "Supplier Class Setup", "supp_class", "class_name");
            d1code.ShowDialog();

        }

        private void button103_Click(object sender, EventArgs e)
        {
            onecode d1code = new onecode(cs, "Business Nature Setup", "biz_nature", "biz_name");
            d1code.ShowDialog();
        }

        private void button104_Click(object sender, EventArgs e)
        {
            AccountStatus AcctSt = new AccountStatus(cs,ncompid,cLocalCaption);
            AcctSt.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            onecode d1code = new onecode(cs, "District Setup", "biz_nature", "biz_name");
            d1code.ShowDialog();
        }

        private void button91_Click(object sender, EventArgs e)
        {
            onecode d1code = new onecode(cs, "City Setup", "biz_nature", "biz_name");
            d1code.ShowDialog();
        }
    }
}


