using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
//using Microsoft.VisualBasic;
using System.Security.Cryptography;
using TclassLibrary;

namespace WinTcare
{
    public partial class LoginScreen : Form
    {
        string cs = globalvar.cos;
        int ncompid = 0;// globalvar.gnCompid;
        DataTable indRitesView = new DataTable();
        DataTable grpRitesView = new DataTable();
        public LoginScreen()
        {

            InitializeComponent();
        }


        private void LoginScreen_Load(object sender, EventArgs e)
        {
            //            this.ControlBox = false;
            this.Text = globalvar.cLocalCaption + "<<Login Screen>>";
            this.UserID.Focus();
            this.label1.BackColor = Color.Transparent;
            this.label2.BackColor = Color.Transparent;
//            this.label24.Text = globalvar.gcCopyRight;
            getmacaddress();
            string mymac = globalvar.gcMacAddress;
     //       MessageBox.Show("My mac address is " + mymac);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Down)
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
            //      string newpass=UserPassWord.Text.ToUpper(); 
            if (UserID.Text != "" && UserPassword.Text != "")
            {
                okButton.Enabled = true;
                okButton.BackColor = Color.LawnGreen;
                okButton.Select();
                //                okButton.BackColor.ToArgb = "";   
            }
            else
            {
                okButton.Enabled = false;
                //*            MessageBox.Show("Invalid User or Password");
            }

        }
        #endregion 

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void okButton_Click(object sender, EventArgs e)
        {
            getmacaddress();
            using (SqlConnection nConnHandle = new SqlConnection(cs))
            {
                nConnHandle.Open();
                string duser = this.UserID.Text.Trim().ToUpper();
                string dpass = this.chkpassword(this.UserPassword.Text.Trim().ToUpper());
                string dnewpass = tclassChkpassWord.dpassWord(UserPassword.Text.Trim().ToUpper());
                SqlDataReader cUserDetails = null;
                SqlCommand cGetUser = new SqlCommand("exec tsp_getUser @myUserID,@myUserPASS", nConnHandle);
                cGetUser.Parameters.Add("@myUserID", SqlDbType.VarChar).Value = duser;      // this.UserID.Text;
                cGetUser.Parameters.Add("@myUserPASS", SqlDbType.VarChar).Value = dpass;    // this.UserPassword.Text;
                cUserDetails = cGetUser.ExecuteReader();
                cUserDetails.Read();
                if (cUserDetails.HasRows == true)
                {
                    string finUserID = Convert.ToString(cUserDetails["oprcode"]).Trim().ToUpper(); 
                    string finUserPASS = Convert.ToString(cUserDetails["userpassword"]).Trim().ToUpper(); 
                    int nmodule = 8;// cUserDetails.GetInt32(2);
                    int nUserNumb = Convert.ToInt16(cUserDetails["usernumb"]);
                    if (duser == finUserID && dpass == finUserPASS)
                    {
                        globalvar.gcUserid = finUserID;
                        globalvar.gnCompid = Convert.ToInt32(cUserDetails["compid"]);
                        ncompid = Convert.ToInt32(cUserDetails["compid"]);
                        globalvar.gcUserName = cUserDetails["username"].ToString();
                        globalvar.gnBranchid = Convert.ToInt32(cUserDetails["branchid"]);
                        globalvar.gcCashAccont = cUserDetails["cashaccont"].ToString();
                        globalvar.gnAccessLevel = Convert.ToInt32(cUserDetails["accesslvl"]);
                        globalvar.glIscasier = Convert.ToBoolean(cUserDetails["iscasier"]);
                        companyDetails();

                        if(globalvar.gcCashAccont!=null && globalvar.gcCashAccont!=string.Empty)
                        {
                            cashAccount(globalvar.gcCashAccont);
                        }

                        initPersonalRights();
                        getGroupbelong(nUserNumb);
                        personalRights(nUserNumb);
                        SuspenseAccount();
                        Todaydate();
                        getmedoc(globalvar.gcUserid);


                        //initPersonalRights();
                        //personalRights(nUserNumb);   
                        //SuspenseAccount();
                        //Todaydate();
                        //getmedoc(globalvar.gnCompid, globalvar.gcUserid);
                        this.Hide();
                        MainMenu mymenu = new MainMenu();
                        mymenu.WindowState = FormWindowState.Maximized;
                        mymenu.ShowDialog();
                        this.Show();
                    }
                    else
                    {
                        MessageBox.Show("Invalid User Details");
                        this.UserID.Text = "";
                        this.UserPassword.Text = "";
                        okButton.Enabled = false;
                        okButton.BackColor = Color.LightGray;
                    }
                }
                else
                {
                    MessageBox.Show("Invalid User Details");
                    this.UserID.Text = "";
                    this.UserPassword.Text = "";
                    okButton.Enabled = false;
                    okButton.BackColor = Color.LightGray;
                    this.UserID.Focus();
                }
                nConnHandle.Close();
            }
            this.UserID.Text = "";
            this.UserPassword.Text = "";
            okButton.Enabled = false;
            okButton.BackColor = Color.LightGray;
            this.UserID.Focus();
        }

        private void getmedoc(string duserid)
        {
////            string cs2 = globalvar.cos;
//            using (SqlConnection nConnHandle = new SqlConnection(cs))
//            {
//                string susp = "exec tsp_GetMeDoctors "+ncompid+","+"'"+duserid+"'";
//                SqlDataAdapter da2 = new SqlDataAdapter(susp, nConnHandle);
//                nConnHandle.Open();
//                DataTable ds = new DataTable();
//                da2.Fill(ds);
////                if (ds != null)
//                if(ds.Rows.Count>0)
//                {
////                    MessageBox.Show("at least we have found something");
//                    globalvar.gnIntDocID = Convert.ToInt16(ds.Rows[0]["dr_id"]);
//  //                  MessageBox.Show("The dr_id is " + globalvar.gnIntDocID);
//                }
//            }
        }

        private void getGroupbelong(int nusernumb)
        {
            //            MessageBox.Show("inside getgroupbelow with "+ncompid+" and " + nusernumb);
            string dsqlBelong = "exec tsp_UserBelongInGroup  " + ncompid + "," + nusernumb;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter dabelong = new SqlDataAdapter(dsqlBelong, ndConnHandle);
                DataTable belongview = new DataTable();
                dabelong.Fill(belongview);
                if (belongview.Rows.Count > 0)
                {
                    for (int i = 0; i < belongview.Rows.Count; i++)
                    {
                        int grpid = Convert.ToInt16(belongview.Rows[i]["groupid"]);
                        grpRitesView.Clear();
                        inheritedRights(grpid, globalvar.gnAccessLevel, nusernumb);
                    }
                }
                else { MessageBox.Show("user does not belong to any group with access rights, inform IT DEPT immediately"); }
            }
        }

        private void inheritedRights(int tngrpid, int tnroleid, int tnUserNumb)
        {
            string dsqlinhRites = "EXEC Tsp_getUserInheritedRights " + ncompid + "," + tngrpid + "," + tnroleid + "," + tnUserNumb;// @dCOMPID, @dgroupid,@dareaid,@dusernumb";
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter dainhRites = new SqlDataAdapter(dsqlinhRites, ndConnHandle);
                dainhRites.Fill(grpRitesView);
                if (grpRitesView.Rows.Count > 0)
                {
                    int inhrites = grpRitesView.Rows.Count;

                    for (int i = 0; i < grpRitesView.Rows.Count; i++)
                    {
                        int dfunid = Convert.ToInt16(grpRitesView.Rows[i]["funid"]);

                        if (dfunid == 1) { globalvar.glCreateCountry = true; }
                        if (dfunid == 3) { globalvar.glCreateLocation = true; }
                        if (dfunid == 5) { globalvar.glCreateLoanReject = true; }
                        if (dfunid == 7) { globalvar.glCreateProduct = true; }
                        if (dfunid == 9) { globalvar.glCreateRegion = true; }
                        if (dfunid == 10) { globalvar.glCreateYard = true; }
                        if (dfunid == 11) { globalvar.glCreateDistrict = true; }


                        if (dfunid == 13) { globalvar.glEditUser = true; }
                        if (dfunid == 14) { globalvar.glActivateUser = true; }
                        if (dfunid == 15) { globalvar.glHideUser = true; }

                        if (dfunid == 16) { globalvar.glMemberRegistration = true; }

                        if (dfunid == 17) { globalvar.glMemberAttach = true; }
                        if (dfunid == 18) { globalvar.glMemberPayroll = true; }
                        if (dfunid == 19) { globalvar.glLoanApply = true; }
                        if (dfunid == 20) { globalvar.glLoanAmort = true; }
                        if (dfunid == 21) { globalvar.glLoanAddGrantor = true; }
                        if (dfunid == 22) { globalvar.glAccountAdd = true; }
                        if (dfunid == 23) { globalvar.glMemberEdit = true; }
                        if (dfunid == 24) { globalvar.glMemberclose = true; }
                        if (dfunid == 25) { globalvar.glMemberActive = true; }
                        if (dfunid == 26) { globalvar.glLoanChargeOff = true; }
                        if (dfunid == 27) { globalvar.glLoanActive = true; }
                        if (dfunid == 28) { globalvar.glAccountEdit = true; }
                        if (dfunid == 29) { globalvar.glAccountDel = true; }


                        if (dfunid == 30) { globalvar.glEditEmplyr = true; }
                        if (dfunid == 31) { globalvar.glActiveEmplyr = true; }
                        if (dfunid == 32) { globalvar.glHideEmplyr = true; }
                        if (dfunid == 33) { globalvar.glEditDistrict = true; }
                        if (dfunid == 34) { globalvar.glActiveDistrict = true; }
                        if (dfunid == 35) { globalvar.glHideDistrict = true; }
                        if (dfunid == 36) { globalvar.glEditRegion = true; }
                        if (dfunid == 37) { globalvar.glActiveRegion = true; }
                        if (dfunid == 38) { globalvar.glHideRegion = true; }

                        if (dfunid == 40) { globalvar.glActiveProduct = true; }
                        if (dfunid == 41) { globalvar.glHideProduct = true; }
                        if (dfunid == 42) { globalvar.glEditCity = true; }
                        if (dfunid == 43) { globalvar.glActiveCity = true; }
                        if (dfunid == 44) { globalvar.glHideCity = true; }
                        if (dfunid == 45) { globalvar.glEditLoanReject = true; }
                        if (dfunid == 46) { globalvar.glActiveLoanReject = true; }
                        if (dfunid == 47) { globalvar.glHideLoanReject = true; }
                        if (dfunid == 48) { globalvar.glEditLocation = true; }
                        if (dfunid == 49) { globalvar.glActiveLocation = true; }
                        if (dfunid == 50) { globalvar.glHideLocation = true; }

                        if (dfunid == 51) { globalvar.glEditDept = true; }
                        if (dfunid == 52) { globalvar.glActiveDept = true; }
                        if (dfunid == 53) { globalvar.glHideDept = true; }

                        if (dfunid == 54) { globalvar.glEditCountry = true; }
                        if (dfunid == 55) { globalvar.glActiveCountry = true; }
                        if (dfunid == 56) { globalvar.glHideCountry = true; }
                        if (dfunid == 57) { globalvar.glEditInsType = true; }
                        if (dfunid == 58) { globalvar.glActiveInsType = true; }
                        if (dfunid == 59) { globalvar.glHideInsType = true; }

                        if (dfunid == 60) { globalvar.glEditBnkBranch = true; }
                        if (dfunid == 61) { globalvar.glActiveBnkBranch = true; }
                        if (dfunid == 62) { globalvar.glHideBnkBranch = true; }
                        if (dfunid == 63) { globalvar.glEditCompany = true; }
                        if (dfunid == 64) { globalvar.glActiveCompany = true; }
                        if (dfunid == 65) { globalvar.glHideCompany = true; }

                        if (dfunid == 66) { globalvar.glEditBank = true; }
                        if (dfunid == 67) { globalvar.glActiveBank = true; }
                        if (dfunid == 68) { globalvar.glHideBank = true; }
                        if (dfunid == 69) { globalvar.glEditBranch = true; }
                        if (dfunid == 70) { globalvar.glActiveBranch = true; }
                        if (dfunid == 71) { globalvar.glHideBranch = true; }
                        if (dfunid == 72) { globalvar.glEditSector = true; }
                        if (dfunid == 73) { globalvar.glActiveSector = true; }
                        if (dfunid == 74) { globalvar.glHideSector = true; }


                        if (dfunid == 75) { globalvar.glBatchSetup = true; }
                        if (dfunid == 76) { globalvar.glBatchProcess = true; }
                        if (dfunid == 77) { globalvar.glBatchLoanApply = true; }
                        if (dfunid == 78) { globalvar.glBatchLoanAppr = true; }
                        if (dfunid == 79) { globalvar.glBatchLoanIssue = true; }

                        if (dfunid == 80) { globalvar.glJournal = true; }
                        if (dfunid == 81) { globalvar.glAcctEnq = true; }
                        if (dfunid == 82) { globalvar.glDeposit = true; }
                        if (dfunid == 83) { globalvar.glWithDraw = true; }
                        if (dfunid == 84) { globalvar.glCreateCmpBranch = true; }
                        if (dfunid == 85) { globalvar.glCreateBnkBranch = true; }


                        if (dfunid == 88) { globalvar.glCreateCompany = true; }
                        if (dfunid == 90) { globalvar.glCreateSector = true; }
                        if (dfunid == 92) { globalvar.glCreateInst = true; }
                        if (dfunid == 94) { globalvar.glCreateDept = true; }
                        if (dfunid == 96) { globalvar.glCreateCity = true; }

                        if (dfunid == 98) { globalvar.glLoanEdtGrantor = true; }
                        if (dfunid == 99) { globalvar.glLoanDelGrantor = true; }

                        if (dfunid == 100) { globalvar.glLoanApproval = true; }
                        if (dfunid == 101) { globalvar.glLoanIssue = true; }
                        if (dfunid == 102) { globalvar.glLoanWriteOff = true; }
                        if (dfunid == 108) { globalvar.glLoanPay = true; }
                        if (dfunid == 109) { globalvar.glLoanPayout = true; }
                        if (dfunid == 111) { globalvar.glVerify = true; }

                        if (dfunid == 103) { globalvar.glCreateUser = true; }
                        if (dfunid == 104) { globalvar.glCreateUser = true; }
                        if (dfunid == 105) { globalvar.glCreateEmplyr = true; }
                        if (dfunid == 106) { globalvar.glBudget = true; }
                        if (dfunid == 107) { globalvar.glCreateBank = true; }

                        if (dfunid == 110) { globalvar.glCashManager = true; }
                        if (dfunid == 112) { globalvar.glAcctPay = true; }
                        if (dfunid == 113) { globalvar.glAcctRec = true; }

                        if (dfunid == 114) { globalvar.glStatements = true; }
                        if (dfunid == 115) { globalvar.glAcctRecon = true; }
                        if (dfunid == 116) { globalvar.glInterestCalc = true; }

                        if (dfunid == 117) { globalvar.glTransfers = true; }
                        if (dfunid == 118) { globalvar.glRevAdjust = true; }
                        if (dfunid == 119) { globalvar.glGenLedger = true; }
                        //                        MessageBox.Show("The transfer flag in loginscreen is " + globalvar.glTransfers);

                        //****** Payroll Processing
                        if (dfunid == 154) { globalvar.glPartSal = true; }
                        if (dfunid == 155) { globalvar.glRunPay = true; }
                        if (dfunid == 156) { globalvar.glAllo = true; }
                        if (dfunid == 157) { globalvar.glLoans = true; }
                        if (dfunid == 158) { globalvar.glOvtime = true; }
                        if (dfunid == 159) { globalvar.glSalAdv = true; }

                        if (dfunid == 160) { globalvar.glSubscript = true; }
                        if (dfunid == 161) { globalvar.glPostPay = true; }
                        if (dfunid == 162) { globalvar.glClosPay = true; }
                        if (dfunid == 163) { globalvar.glPayPara = true; }

                        //******* HRM
                        if (dfunid == 164) { globalvar.glBasicData = true; }
                        if (dfunid == 165) { globalvar.glEmpBio = true; }
                        if (dfunid == 166) { globalvar.glDepData = true; }
                        if (dfunid == 167) { globalvar.glMedCover = true; }
                        if (dfunid == 168) { globalvar.glAbsence = true; }
                        if (dfunid == 169) { globalvar.glTraining = true; }
                        if (dfunid == 170) { globalvar.glAccident = true; }
                        if (dfunid == 171) { globalvar.glLivAppr = true; }
                        if (dfunid == 172) { globalvar.glDiscipl = true; }
                        if (dfunid == 173) { globalvar.glApprSal = true; }
                        if (dfunid == 174) { globalvar.glCareer = true; }
                        if (dfunid == 175) { globalvar.glSuccess = true; }
                        if (dfunid == 176) { globalvar.glGrievance = true; }
                        if (dfunid == 177) { globalvar.glExitServ = true; }

                        //********* Procurement
                        if (dfunid == 178) { globalvar.glRfq = true; }
                        if (dfunid == 179) { globalvar.glPo = true; }
                        if (dfunid == 180) { globalvar.glDelivery = true; }
                        if (dfunid == 181) { globalvar.glSupplMgt = true; }
                        if (dfunid == 182) { globalvar.glItemSetup = true; }
                        if (dfunid == 183) { globalvar.glMainStore = true; }
                        if (dfunid == 184) { globalvar.glStockUpd = true; }
                        if (dfunid == 185) { globalvar.glAssetMgt = true; }
                        if (dfunid == 186) { globalvar.glReqAppr = true; }
                        if (dfunid == 187) { globalvar.glPoAppr = true; }

                        //********** Executive
                        if (dfunid == 188) { globalvar.glWavAppr = true; }
                        if (dfunid == 189) { globalvar.glDashBoard = true; }
                        if (dfunid == 190) { globalvar.glStatistics = true; }
                        if (dfunid == 191) { globalvar.glAttendance = true; }

                        //*********** Reporting 
                        if (dfunid == 120) { globalvar.glOpsRept = true; }
                        if (dfunid == 121) { globalvar.glMemRept = true; }
                        if (dfunid == 122) { globalvar.glLonRept = true; }
                        if (dfunid == 123) { globalvar.glPorRept = true; }
                        if (dfunid == 124) { globalvar.glGlRept = true; }
                        if (dfunid == 125) { globalvar.glAudRept = true; }
                        if (dfunid == 126) { globalvar.glSocRept = true; }

                        if (dfunid == 127) { globalvar.glPayRept = true; }
                        if (dfunid == 128) { globalvar.glHrmRept = true; }
                        if (dfunid == 129) { globalvar.glProcRept = true; }
                        if (dfunid == 130) { globalvar.glAcctRept = true; }
                        //********************************************************************************************
                        //********************************************************************************************
                    }
                }else { MessageBox.Show("No inherited rites for this user"); }
            }
        }


        private static string getmacaddress()
        {
            string maddr = string.Empty;
            foreach(NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus==OperationalStatus.Up)
                {
                maddr = nic.GetPhysicalAddress().ToString();
                    globalvar.gcMacAddress = maddr;
                    break;
                }
            }
//            MessageBox.Show("The address in getmacaddress here is " + maddr);
            return maddr;
            /*            var macAddr =
    (
    from nic in NetworkInterface.GetAllNetworkInterfaces()
    where nic.OperationalStatus == OperationalStatus.Up
    select nic.GetPhysicalAddress().ToString()
    ).FirstOrDefault();
            globalvar.gcMacAddress = macAddr;
            MessageBox.Show("The MacAddress is " + macAddr);
            return macAddr;
            */
            //    string lcComputerName = ".";
            //    string loWMIService = "";
            //    string loItems = "";
            //    string loItem = "";
            //    string lcMACAddress = "";
            //    lcComputerName = "."
        }

        private void SuspenseAccount()
        {
            using (SqlConnection nConnHandle = new SqlConnection(cs))
            {
                string susp = "select int_suspense from client_code";
                SqlDataAdapter da2 = new SqlDataAdapter(susp, nConnHandle);
                nConnHandle.Open();
                DataTable ds = new DataTable();
                da2.Fill(ds);
                if (ds != null && ds.Rows.Count>0)
                {
                    globalvar.gcIntSuspense = ds.Rows[0]["int_suspense"].ToString();
                }
            }
        }

//        private void 
        private void cashAccount(string actnumb)
        {
            using (SqlConnection nConnHandle = new SqlConnection(cs))
            {
                string cashap = "select cacctname from glmast where cacctnumb ="+"'"+actnumb+"'";
                SqlDataAdapter da21 = new SqlDataAdapter(cashap, nConnHandle);
                nConnHandle.Open();
                DataTable ds1 = new DataTable();
                da21.Fill(ds1);
                if (ds1 != null & ds1.Rows.Count>0)
                {
                    globalvar.gcCashAccontName  = ds1.Rows[0]["cacctname"].ToString();
                }
            }
        }

        private void Todaydate()
        {
//            string cs2 = globalvar.cos;
            using (SqlConnection nConnHandle = new SqlConnection(cs))
            {
                string susp = "select convert(date,GETDATE()) as dsysdate ";
                SqlDataAdapter da2 = new SqlDataAdapter(susp, nConnHandle);
                nConnHandle.Open();
                DataTable ds = new DataTable();
                da2.Fill(ds);
                if (ds != null)
                {
                    globalvar.gdSysDate =Convert.ToDateTime(ds.Rows[0]["dsysdate"]);
                }
            }
        }

  

        private void companyDetails()
        {
            using (SqlConnection nConnHandle0 = new SqlConnection(cs))
            {
                nConnHandle0.Open();
                SqlDataReader cCompDetails = null;
                SqlCommand cGetUser = new SqlCommand("exec tsp_getComp @ncompid", nConnHandle0);
                cGetUser.Parameters.Add("@ncompid", SqlDbType.Int).Value = ncompid;
                cCompDetails = cGetUser.ExecuteReader();
                cCompDetails.Read();
                if (cCompDetails.HasRows == true)
                {
//                    MessageBox.Show("The company has been found and now we are going for the details");
                    globalvar.gcCompName = cCompDetails["com_name"].ToString();
                    globalvar.gcCompanyHeader = cCompDetails["com_logo"].ToString();
                    globalvar.gcCompAddr = cCompDetails["caddress"].ToString();
                    globalvar.gcCompPobox = cCompDetails["pobox"].ToString();
                    globalvar.gcCompTel = cCompDetails["tel"].ToString();
                    globalvar.gcCompFax = cCompDetails["fax"].ToString();
                    globalvar.gnCorpTax = Convert.ToDecimal(cCompDetails["corp_tax"]);
                    globalvar.gnAnonComp = Convert.ToInt32(cCompDetails["anoncomp"]);
                    globalvar.gcIDDCODE = cCompDetails["idd_code"].ToString();   
                    globalvar.gcLocalCountry = (Convert.ToBoolean(cCompDetails["localCountry"]) ? cCompDetails["cou_name"] : "").ToString();
                    globalvar.gnVat = Convert.ToDecimal(cCompDetails["vat"]);
                    globalvar.gnNhifRebate = Convert.ToDecimal(cCompDetails["nhifRefund"]);
                    globalvar.glBioLink = Convert.ToBoolean(cCompDetails["biolink"]);
                    globalvar.gnCou_ID = Convert.ToInt32(cCompDetails["cou_id"]);
                    globalvar.gnToddlerAge = Convert.ToInt32(cCompDetails["toddlerage"]);
                    globalvar.gnChildAge = Convert.ToInt32(cCompDetails["childage"]);
                    globalvar.gnXchRate = Convert.ToDecimal(cCompDetails["xchrate"]);
                    globalvar.gcCompMail = cCompDetails["email"].ToString();
                    globalvar.gnWeekHrs = Convert.ToInt32(cCompDetails["nweekHrs"]);

//                    MessageBox.Show("The company has been found step 1");
                    globalvar.gnCompInj = Convert.ToDecimal(cCompDetails["injcomp"]);
                    globalvar.gnCompCont = Convert.ToDecimal(cCompDetails["nemployer"]);
                    globalvar.gnCompInd = Convert.ToDecimal(cCompDetails["nemployee"]);
                    globalvar.gnOvt50 = Convert.ToDecimal(cCompDetails["ovt50"]);
                    globalvar.gnOvt100 = Convert.ToDecimal(cCompDetails["ovt100"]);
                    globalvar.gcCurrSym = cCompDetails["CurrSym"].ToString();
                    globalvar.gcCurrName = cCompDetails["curr_name"].ToString().ToLower();
                    globalvar.gcCurrUnit = cCompDetails["unit_name"].ToString().ToLower();
                    globalvar.gnCurrCode = Convert.ToInt32(cCompDetails["curr_id"]);//.ToString().ToLower();
                    globalvar.gl4Profit = Convert.ToBoolean(cCompDetails["forprofit"]);
                    globalvar.gcPayPer = cCompDetails["pay_period"].ToString();
                    globalvar.gnMaxSalAdv = Convert.ToDecimal(cCompDetails["maxsaladv"]);
                    globalvar.gnSalCap = Convert.ToDecimal(cCompDetails["salary_cap"]);
                    globalvar.gnPerReli = Convert.ToDecimal(cCompDetails["per_relief"]);
                    globalvar.gnRetireAge = Convert.ToInt32(cCompDetails["retireage"]);
                    globalvar.gnDefaultCountry = Convert.ToInt32(cCompDetails["cou_ID"]);
                    globalvar.glGrantee2Sav = Convert.ToBoolean(cCompDetails["grantee2sav"]);
                    globalvar.gnGuarPercent = Convert.ToDecimal(cCompDetails["guarpercent"]);

//                    MessageBox.Show("The company has been found step 2");
                    globalvar.gcSaveCtrl_Ind  = cCompDetails["savectrl_ind"].ToString();
                    globalvar.gcSaveCtrl_Cor  = cCompDetails["savectrl_cor"].ToString();
                    globalvar.gcSaveCtrl_Grp  = cCompDetails["savectrl_grp"].ToString();
                    globalvar.gcSaveCtrl_Stf  = cCompDetails["savectrl_stf"].ToString();
                    globalvar.gcShareCtrl_Ind = cCompDetails["sharectrl_ind"].ToString();
                    globalvar.gcShareCtrl_Cor = cCompDetails["sharectrl_cor"].ToString();
                    globalvar.gcShareCtrl_Grp = cCompDetails["sharectrl_grp"].ToString();
                    globalvar.gcShareCtrl_Stf = cCompDetails["sharectrl_stf"].ToString();
                    globalvar.gcLoanCtrl_Ind = cCompDetails["loanctrl_ind"].ToString();

//                    MessageBox.Show("The company has been found step 3");
                    globalvar.gcLoanCtrl_Cor = cCompDetails["loanctrl_cor"].ToString();
                    globalvar.gcLoanCtrl_Grp = cCompDetails["loanctrl_grp"].ToString();
                    globalvar.gcLoanCtrl_Stf = cCompDetails["loanctrl_stf"].ToString();
                    globalvar.gnLoanChargeOff =Convert.ToInt16(cCompDetails["prd_id"]);
  //                  MessageBox.Show("The company has been found step 4");
                    //        MessageBox.Show("Company id and name, local country  " + tncompid + " , " + globalvar.gcCompName+", "+globalvar.gcLocalCountry);
                }
            }
        }
        private void label3_Click(object sender, EventArgs e)
        {

        }

   private void initPersonalRights()
        {
 globalvar.glCreateCountry = globalvar.glCreateLocation = globalvar.glCreateLoanReject = globalvar.glCreateProduct = globalvar.glCreateRegion = globalvar.glCreateYard = globalvar.glCreateDistrict = globalvar.glEditUser =
globalvar.glActivateUser = globalvar.glHideUser = globalvar.glMemberRegistration = globalvar.glMemberRegistration =  globalvar.glMemberAttach = globalvar.glMemberPayroll = globalvar.glLoanApply = globalvar.glLoanAmort =
globalvar.glLoanAddGrantor =  globalvar.glAccountAdd = globalvar.glMemberEdit = globalvar.glMemberclose = globalvar.glMemberActive = globalvar.glLoanChargeOff = globalvar.glLoanActive = globalvar.glAccountEdit = globalvar.glAccountDel =
globalvar.glEditEmplyr = globalvar.glActiveEmplyr = globalvar.glHideEmplyr =globalvar.glEditDistrict =globalvar.glActiveDistrict =globalvar.glHideDistrict =globalvar.glEditRegion =globalvar.glActiveRegion =globalvar.glHideRegion =
globalvar.glActiveProduct =globalvar.glHideProduct =globalvar.glEditCity =globalvar.glActiveCity =globalvar.glHideCity =globalvar.glEditLoanReject =globalvar.glActiveLoanReject =globalvar.glHideLoanReject =globalvar.glEditLocation =
globalvar.glActiveLocation =globalvar.glHideLocation =globalvar.glEditDept =globalvar.glActiveDept =globalvar.glHideDept =globalvar.glEditCountry =globalvar.glActiveCountry =globalvar.glHideCountry =globalvar.glEditInsType =globalvar.glActiveInsType =
globalvar.glHideInsType =globalvar.glEditBnkBranch =globalvar.glActiveBnkBranch =globalvar.glHideBnkBranch =globalvar.glEditCompany =globalvar.glActiveCompany =globalvar.glHideCompany =globalvar.glEditBank =globalvar.glActiveBank =globalvar.glHideBank =
globalvar.glEditBranch =globalvar.glActiveBranch =globalvar.glHideBranch =globalvar.glEditSector =globalvar.glActiveSector =globalvar.glHideSector =globalvar.glBatchSetup =globalvar.glBatchProcess =globalvar.glBatchLoanApply =globalvar.glBatchLoanAppr =
globalvar.glBatchLoanIssue =globalvar.glJournal =globalvar.glAcctEnq =globalvar.glDeposit =globalvar.glWithDraw =globalvar.glCreateCmpBranch =globalvar.glCreateBnkBranch =globalvar.glCreateCompany =globalvar.glCreateSector =globalvar.glCreateInst =
globalvar.glCreateDept =globalvar.glCreateCity =globalvar.glLoanEdtGrantor =globalvar.glLoanDelGrantor =globalvar.glLoanApproval =globalvar.glLoanIssue =globalvar.glLoanWriteOff =globalvar.glLoanPay =globalvar.glLoanPayout =globalvar.glVerify =
globalvar.glCreateUser =globalvar.glCreateEmplyr =globalvar.glCreateBank =globalvar.glCashManager =globalvar.glAcctPay =globalvar.glAcctRec =globalvar.glStatements =globalvar.glAcctRecon =globalvar.glInterestCalc =globalvar.glTransfers =globalvar.glRevAdjust =
globalvar.glGenLedger =globalvar.glPartSal =globalvar.glRunPay =globalvar.glAllo =globalvar.glLoans =globalvar.glOvtime =globalvar.glSalAdv =globalvar.glSubscript =globalvar.glPostPay =globalvar.glClosPay =globalvar.glPayPara =globalvar.glBasicData =
globalvar.glEmpBio =globalvar.glDepData =globalvar.glMedCover =globalvar.glAbsence =globalvar.glTraining =globalvar.glAccident =globalvar.glLivAppr =globalvar.glDiscipl =globalvar.glApprSal =globalvar.glCareer =globalvar.glSuccess =globalvar.glGrievance =
globalvar.glExitServ =globalvar.glRfq =globalvar.glPo =globalvar.glDelivery =globalvar.glSupplMgt =globalvar.glItemSetup =globalvar.glMainStore =globalvar.glStockUpd =globalvar.glAssetMgt =globalvar.glReqAppr =globalvar.glPoAppr =globalvar.glWavAppr =
globalvar.glDashBoard =globalvar.glStatistics =globalvar.glOpsRept =globalvar.glMemRept =globalvar.glLonRept =globalvar.glPorRept =globalvar.glGlRept =globalvar.glAudRept =globalvar.glSocRept =globalvar.glPayRept =globalvar.glHrmRept =globalvar.glProcRept =
globalvar.glAcctRept = globalvar.glAttendance= false; 

        }
        private void personalRights(int tnUserNumb)
        {
//            MessageBox.Show("Inside personal rights step 0");
            string dsqlRites = "exec Tsp_getUserPersonalRights  "+ncompid+","+ tnUserNumb;
            indRitesView.Clear();
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter daRites = new SqlDataAdapter(dsqlRites, ndConnHandle);
                daRites.Fill(indRitesView);
                if (indRitesView.Rows.Count > 0)
                {
                    for (int i = 0; i < indRitesView.Rows.Count; i++)
                    {
                        int dfunid = Convert.ToInt16(indRitesView.Rows[i]["funid"]);

                        if (dfunid == 1) { globalvar.glCreateCountry = true; }
                        if (dfunid == 3) { globalvar.glCreateLocation = true; }
                        if (dfunid == 5) { globalvar.glCreateLoanReject = true; }
                        if (dfunid == 7) { globalvar.glCreateProduct = true; }
                        if (dfunid == 9) { globalvar.glCreateRegion = true; }
                        if (dfunid == 10) { globalvar.glCreateYard = true; }
                        if (dfunid == 11) { globalvar.glCreateDistrict = true; }


                        if (dfunid == 13) { globalvar.glEditUser = true; }
                        if (dfunid == 14) { globalvar.glActivateUser = true; }
                        if (dfunid == 15) { globalvar.glHideUser = true; }

                        if (dfunid == 16) { globalvar.glMemberRegistration = true; }

                        if (dfunid == 17) { globalvar.glMemberAttach = true; }
                        if (dfunid == 18) { globalvar.glMemberPayroll = true; }
                        if (dfunid == 19) { globalvar.glLoanApply = true; }
                        if (dfunid == 20) { globalvar.glLoanAmort = true; }
                        if (dfunid == 21) { globalvar.glLoanAddGrantor = true; }
                        if (dfunid == 22) { globalvar.glAccountAdd = true; }
                        if (dfunid == 23) { globalvar.glMemberEdit = true; }
                        if (dfunid == 24) { globalvar.glMemberclose = true; }
                        if (dfunid == 25) { globalvar.glMemberActive = true; }
                        if (dfunid == 26) { globalvar.glLoanChargeOff = true; }
                        if (dfunid == 27) { globalvar.glLoanActive = true; }
                        if (dfunid == 28) { globalvar.glAccountEdit = true; }
                        if (dfunid == 29) { globalvar.glAccountDel = true; }


                        if (dfunid == 30) { globalvar.glEditEmplyr = true; }
                        if (dfunid == 31) { globalvar.glActiveEmplyr = true; }
                        if (dfunid == 32) { globalvar.glHideEmplyr = true; }
                        if (dfunid == 33) { globalvar.glEditDistrict = true; }
                        if (dfunid == 34) { globalvar.glActiveDistrict = true; }
                        if (dfunid == 35) { globalvar.glHideDistrict = true; }
                        if (dfunid == 36) { globalvar.glEditRegion = true; }
                        if (dfunid == 37) { globalvar.glActiveRegion = true; }
                        if (dfunid == 38) { globalvar.glHideRegion = true; }

                        if (dfunid == 40) { globalvar.glActiveProduct = true; }
                        if (dfunid == 41) { globalvar.glHideProduct = true; }
                        if (dfunid == 42) { globalvar.glEditCity = true; }
                        if (dfunid == 43) { globalvar.glActiveCity = true; }
                        if (dfunid == 44) { globalvar.glHideCity = true; }
                        if (dfunid == 45) { globalvar.glEditLoanReject = true; }
                        if (dfunid == 46) { globalvar.glActiveLoanReject = true; }
                        if (dfunid == 47) { globalvar.glHideLoanReject = true; }
                        if (dfunid == 48) { globalvar.glEditLocation = true; }
                        if (dfunid == 49) { globalvar.glActiveLocation = true; }
                        if (dfunid == 50) { globalvar.glHideLocation = true; }

                        if (dfunid == 51) { globalvar.glEditDept = true; }
                        if (dfunid == 52) { globalvar.glActiveDept = true; }
                        if (dfunid == 53) { globalvar.glHideDept = true; }

                        if (dfunid == 54) { globalvar.glEditCountry = true; }
                        if (dfunid == 55) { globalvar.glActiveCountry = true; }
                        if (dfunid == 56) { globalvar.glHideCountry = true; }
                        if (dfunid == 57) { globalvar.glEditInsType = true; }
                        if (dfunid == 58) { globalvar.glActiveInsType = true; }
                        if (dfunid == 59) { globalvar.glHideInsType = true; }

                        if (dfunid == 60) { globalvar.glEditBnkBranch = true; }
                        if (dfunid == 61) { globalvar.glActiveBnkBranch = true; }
                        if (dfunid == 62) { globalvar.glHideBnkBranch = true; }
                        if (dfunid == 63) { globalvar.glEditCompany = true; }
                        if (dfunid == 64) { globalvar.glActiveCompany = true; }
                        if (dfunid == 65) { globalvar.glHideCompany = true; }

                        if (dfunid == 66) { globalvar.glEditBank = true; }
                        if (dfunid == 67) { globalvar.glActiveBank = true; }
                        if (dfunid == 68) { globalvar.glHideBank = true; }
                        if (dfunid == 69) { globalvar.glEditBranch = true; }
                        if (dfunid == 70) { globalvar.glActiveBranch = true; }
                        if (dfunid == 71) { globalvar.glHideBranch = true; }
                        if (dfunid == 72) { globalvar.glEditSector = true; }
                        if (dfunid == 73) { globalvar.glActiveSector = true; }
                        if (dfunid == 74) { globalvar.glHideSector = true; }


                        if (dfunid == 75) { globalvar.glBatchSetup = true; }
                        if (dfunid == 76) { globalvar.glBatchProcess = true; }
                        if (dfunid == 77) { globalvar.glBatchLoanApply = true; }
                        if (dfunid == 78) { globalvar.glBatchLoanAppr = true; }
                        if (dfunid == 79) { globalvar.glBatchLoanIssue = true; }

                        if (dfunid == 80) { globalvar.glJournal = true; }
                        if (dfunid == 81) { globalvar.glAcctEnq = true; }
                        if (dfunid == 82) { globalvar.glDeposit = true; }
                        if (dfunid == 83) { globalvar.glWithDraw = true; }
                        if (dfunid == 84) { globalvar.glCreateCmpBranch = true; }
                        if (dfunid == 85) { globalvar.glCreateBnkBranch = true; }


                        if (dfunid == 88) { globalvar.glCreateCompany = true; }
                        if (dfunid == 90) { globalvar.glCreateSector = true; }
                        if (dfunid == 92) { globalvar.glCreateInst = true; }
                        if (dfunid == 94) { globalvar.glCreateDept = true; }
                        if (dfunid == 96) { globalvar.glCreateCity = true; }

                        if (dfunid == 98) { globalvar.glLoanEdtGrantor = true; }
                        if (dfunid == 99) { globalvar.glLoanDelGrantor = true; }

                        if (dfunid == 100) { globalvar.glLoanApproval = true; }
                        if (dfunid == 101) { globalvar.glLoanIssue = true; }
                        if (dfunid == 102) { globalvar.glLoanWriteOff = true; }
                        if (dfunid == 108) { globalvar.glLoanPay = true; }
                        if (dfunid == 109) { globalvar.glLoanPayout = true; }
                        if (dfunid == 111) { globalvar.glVerify = true; }

                        if (dfunid == 103) { globalvar.glCreateUser = true; }
                        if (dfunid == 104) { globalvar.glCreateUser = true; }
                        if (dfunid == 105) { globalvar.glCreateEmplyr = true; }
                        if (dfunid == 106) { globalvar.glBudget = true; }
                        if (dfunid == 107) { globalvar.glCreateBank = true; }

                        if (dfunid == 110) { globalvar.glCashManager = true;}
                        if (dfunid == 112) { globalvar.glAcctPay = true;}
                        if (dfunid == 113) { globalvar.glAcctRec = true;}

                        if (dfunid == 114) { globalvar.glStatements = true;}
                        if (dfunid == 115) { globalvar.glAcctRecon = true;}
                        if (dfunid == 116) { globalvar.glInterestCalc = true;}

                        if (dfunid == 117) { globalvar.glTransfers = true;}
                        if (dfunid == 118) { globalvar.glRevAdjust = true;}
                        if (dfunid == 119) { globalvar.glGenLedger = true;}
//                        MessageBox.Show("The transfer flag in loginscreen is " + globalvar.glTransfers);

                        //****** Payroll Processing
                        if (dfunid == 154) { globalvar.glPartSal = true;}
                        if (dfunid == 155) { globalvar.glRunPay = true;}
                        if (dfunid == 156) { globalvar.glAllo = true;}
                        if (dfunid == 157) { globalvar.glLoans = true;}
                        if (dfunid == 158) { globalvar.glOvtime = true;}
                        if (dfunid == 159) { globalvar.glSalAdv = true;}

                        if (dfunid == 160) { globalvar.glSubscript = true;}
                        if (dfunid == 161) { globalvar.glPostPay = true;}
                        if (dfunid == 162) { globalvar.glClosPay = true;}
                        if (dfunid == 163) { globalvar.glPayPara = true;}

                        //******* HRM
                        if (dfunid == 164) { globalvar.glBasicData = true;}
                        if (dfunid == 165) { globalvar.glEmpBio = true;}
                        if (dfunid == 166) { globalvar.glDepData = true;}
                        if (dfunid == 167) { globalvar.glMedCover = true;}
                        if (dfunid == 168) { globalvar.glAbsence = true;}
                        if (dfunid == 169) { globalvar.glTraining = true;}
                        if (dfunid == 170) { globalvar.glAccident = true;}
                        if (dfunid == 171) { globalvar.glLivAppr = true;}
                        if (dfunid == 172) { globalvar.glDiscipl = true;}
                        if (dfunid == 173) { globalvar.glApprSal = true;}
                        if (dfunid == 174) { globalvar.glCareer = true;}
                        if (dfunid == 175) { globalvar.glSuccess = true;}
                        if (dfunid == 176) { globalvar.glGrievance = true;}
                        if (dfunid == 177) { globalvar.glExitServ = true;}

                        //********* Procurement
                        if (dfunid == 178) { globalvar.glRfq = true;}
                        if (dfunid == 179) { globalvar.glPo = true;}
                        if (dfunid == 180) { globalvar.glDelivery = true;}
                        if (dfunid == 181) { globalvar.glSupplMgt = true;}
                        if (dfunid == 182) { globalvar.glItemSetup = true;}
                        if (dfunid == 183) { globalvar.glMainStore = true;}
                        if (dfunid == 184) { globalvar.glStockUpd = true;}
                        if (dfunid == 185) { globalvar.glAssetMgt = true;}
                        if (dfunid == 186) { globalvar.glReqAppr = true;}
                        if (dfunid == 187) { globalvar.glPoAppr = true;}

                        //********** Executive
                        if (dfunid == 188) { globalvar.glWavAppr = true;}
                        if (dfunid == 189) { globalvar.glDashBoard = true;}
                        if (dfunid == 190) { globalvar.glStatistics = true;}
                        if (dfunid == 191) { globalvar.glAttendance = true; }

                        //*********** Reporting 
                        if (dfunid == 120) { globalvar.glOpsRept = true;}
                        if (dfunid == 121) { globalvar.glMemRept = true;}
                        if (dfunid == 122) { globalvar.glLonRept = true;}
                        if (dfunid == 123) { globalvar.glPorRept = true;}
                        if (dfunid == 124) { globalvar.glGlRept = true;}
                        if (dfunid == 125) { globalvar.glAudRept = true;}
                        if (dfunid == 126) { globalvar.glSocRept = true;}

                        if (dfunid == 127) { globalvar.glPayRept = true; }
                        if (dfunid == 128) { globalvar.glHrmRept = true; }
                        if (dfunid == 129) { globalvar.glProcRept = true; }
                        if (dfunid == 130) { globalvar.glAcctRept = true; }
                        //********************************************************************************************
                    }
                }
            }
        }

        private void UserID_TextChanged(object sender, EventArgs e)
        {
            //    MessageBox.Show("user id text changed");
        }

        private void UserID_TextChanged_1(object sender, EventArgs e)
        {
            //    MessageBox.Show("user id text changed");
        }


        private void UserPassWord_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {
            MessageBox.Show("mask input rejected first time");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void UserPassWord_TextChanged(object sender, EventArgs e)
        {
            MessageBox.Show("password changed here");
        }


        private void UserPassWord_MaskInputRejected_1(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label24_Click(object sender, EventArgs e)
        {

        }

        private void MyPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void UserPassword_TextChanged_1(object sender, EventArgs e)
        {
        }
  
    private string chkpassword(string dpassword)
            {
            int i = 0;
            int j = 0;
            int l = 0;
            string ncrptd = "";
            string t = "";
            int k = this.UserPassword.Text.Length;
            string xpswd = this.UserPassword.Text.ToUpper();        // this.Text.ToUpper();
            string ypswd = xpswd.Substring(k - 1, 1);                 //This is the last character

            for (i = 1; i < k; i++)
            {
                ypswd = ypswd + xpswd.Substring(k - (i + 1), 1);
            }
            if (ypswd.Length > 5)
            {
                t = ypswd.Substring(0, 1);
                for (j = 1; j < ypswd.Length; j++)
                {
                    if (j % 2 > 0)
                    {
                        t = t + ypswd.Substring(j, 1);
                    }
                    else
                    {
                        t = ypswd.Substring(j, 1) + t;
                    }
                }
            }
            j = t.Length % 10;
            for (i = 0; i < t.Length; i++)
            {
                int d = Convert.ToChar(t.Substring(i, 1));
                l = d + j;
                if (l > 90)
                {
                    j = (l) % 90;                                
                }
                ncrptd = ncrptd + Convert.ToString(l);
                j = (i + d + j) % 60;                           
            }
            return ncrptd;
        }
    }
}

