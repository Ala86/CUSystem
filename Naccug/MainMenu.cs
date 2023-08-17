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
    public partial class MainMenu : Form
    {

        string cs = globalvar.cos;
        int ncompid = globalvar.gnCompid;
        string cloc = globalvar.cLocalCaption;
        string cuser = globalvar.gcUserid;
        decimal gnCashBalance = globalvar.gcCashAccontBal;
        DataTable cashview = new DataTable();
        // private object toolStripStatusLable ;


        //string cloca = globalvar.clocation;

        //cs = cos;
        //    ncompid = dcompid;
        //    cLocalCaption = dloca;

        //        private object clientBillView;
        //      private object newClientAcct;
        ///    private object execDashBoard;
        //  private object userMgt;
        //  private object staffMgt;

//        public static class DataEvents
//        {
//            public static event EventHandler DataChanged;
//            internal static void RaiseDataChanged()
//            {
//                var handler = DataChanged;
//                if (handler != null)
//                {
//                    handler();
////                    RefreshData();
//                }
                    
////                    handler(object sender, EventArgs e);
//            }
//        }
        public MainMenu()
        {
            InitializeComponent();
//            DataEvents.DataChanged += (s, e) => RefreshData();
        }


        private void RefreshData(object sender, EventArgs e)
        {
            toolStripStatusLabel3.Text = globalvar.gcCashAccontBal.ToString(); //gnCashBalance.ToString("N2");// gnCashBalance;//  globalvar.gcToolbutton;
        }

        private void bodyReleaseClaimsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        //private void getCashiers()
        //{
        //    using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
        //    {
        //        cashview.Clear();
        //        ndConnHandle1.Open();
        //        //************Getting accounts  exec tsp_getBranchCashiers 30,16              
        //        decimal curbal = 0.00m;
        //        string casSql = " exec tsp_getBranchCashiers " + ncompid + "," + 16;
        //        SqlDataAdapter dalCas = new SqlDataAdapter(casSql, ndConnHandle1);
        //        dalCas.Fill(cashview);
        //        if (cashview != null && cashview.Rows.Count > 0)
        //        {
        //            string ala;
        //            ala = cashview.Rows[0]["oprcode"].ToString();
        //           // MessageBox.Show("ala" + ala);

        //            if (ala.ToString().Trim() == globalvar.gcUserid.ToString().Trim())
        //            {
        //              //  dAcctNumb = Processedclientview.Rows[j]["acctnumb"].ToString();    
        //                decimal bkbal = Convert.ToDecimal(cashview.Rows[0]["nbookbal"]);
        //                curbal = curbal + bkbal;
        //                MessageBox.Show("This is the balance" + curbal);
        //                toolStripStatusLabel3.Text = curbal.ToString();
        //            }
        //            else
        //            {
        //             //   MessageBox.Show("the Query is empty"+ globalvar.gcUserid);
        //            }
        //         }
        //        ndConnHandle1.Close();
        //    }
        //}

        private void MainMenu_Load(object sender, EventArgs e)
        {
            //    this.Text = globalvar.cLocalCaption + "<< Main Menu >>";
            //DateTime timenow = DateTime.Now;
            //label2.Text = DateTime.Now.ToLongDateString();

            this.Text = globalvar.cLocalCaption + "<< Main Menu >>";
            DateTime timenow = DateTime.Now;
            label2.Text = DateTime.Now.ToLongDateString();
            toolStripStatusLabel1.Text = "Current User";
             toolStripStatusLabel2.Text = globalvar.gcUserid;
            globalvar.gcToolbutton = globalvar.gcCashAccontBal.ToString();
            updateBalanceBar();
//            toolStripStatusLabel3.Text = globalvar.gcCashAccontBal.ToString(); //gnCashBalance.ToString("N2");// gnCashBalance;//  globalvar.gcToolbutton;
           // label3.Text = globalvar.gcToolbutton;
            //getCashiers();
            //updateAge1();
            //updateAge2();

            //            MessageBox.Show("The transfer flag is " + globalvar.glTransfers);toolStripStatusLable1.T

            //***************** Member Administration Access Control
            toolStripButton28.Enabled = globalvar.glMemberRegistration;
            toolStripButton3.Enabled = globalvar.glMemberAttach;
            toolStripButton14.Enabled = globalvar.glMemberPayroll;
            toolStripButton66.Enabled = globalvar.glMemberclose;
            toolStripButton63.Enabled = globalvar.glMemberActive;
            toolStripButton104.Enabled = globalvar.glLoanApply;
            toolStripButton105.Enabled = globalvar.glLoanAddGrantor;
            toolStripButton93.Enabled = globalvar.glLoanApproval;
            toolStripButton32.Enabled = globalvar.glLoanAmort;
            toolStripButton4.Enabled = globalvar.glLoanIssue;
          //  toolStripButton10.Enabled = globalvar.glLoanChargeOff;
            toolStripButton106.Enabled = globalvar.glLoanActive;
            toolStripButton5.Enabled = globalvar.glMemberAddAcct;

            //***************** Periodic Processing Access Control
            toolStripButton62.Enabled = globalvar.glBatchProcess;
            toolStripButton31.Enabled = globalvar.glBatchLoanApply;
            toolStripButton95.Enabled = globalvar.glBatchLoanAppr;
            toolStripButton1.Enabled = globalvar.glBatchLoanIssue;
            toolStripButton19.Enabled = globalvar.glBatchSetup;
            toolStripButton94.Enabled = globalvar.glLoanApply;

            //***************** Accounts  Access Control
            toolStripButton16.Enabled = globalvar.glDeposit;
            toolStripButton17.Enabled = globalvar.glWithDraw;
            toolStripButton15.Enabled = globalvar.glLoanPay;
            toolStripButton12.Enabled = globalvar.glLoanPayout;
            toolStripButton10.Enabled = globalvar.glLoanChargeOff;
            toolStripButton29.Enabled = globalvar.glLoanWriteOff;
            toolStripButton13.Enabled = globalvar.glCashManager;
            toolStripButton30.Enabled = globalvar.glJournal;
            toolStripButton74.Enabled = globalvar.glVerify;
            toolStripButton11.Enabled = globalvar.glTransfers;
            toolStripButton6.Enabled = globalvar.glRevAdjust;
            toolStripButton21.Enabled = globalvar.glAcctEnq;
            toolStripButton22.Enabled = globalvar.glGenLedger;
            toolStripButton23.Enabled = globalvar.glAcctPay;
            //toolStripButton24.Enabled = globalvar.glAcctRec;
            toolStripButton75.Enabled = globalvar.glAcctRecon;
            toolStripButton25.Enabled = globalvar.glAccountAdd;
            toolStripButton26.Enabled = globalvar.glAssetMgt;
            toolStripButton33.Enabled = globalvar.glBudget;
            toolStripButton5.Enabled = globalvar.glMemberAddAcct;

            //***************** Payroll  Access Control
            toolStripButton35.Enabled = globalvar.glPartSal;
            toolStripButton36.Enabled = globalvar.glAllo;
            toolStripButton37.Enabled = globalvar.glLoans;
            toolStripButton38.Enabled = globalvar.glOvtime;
            toolStripButton39.Enabled = globalvar.glSalAdv;
            toolStripButton61.Enabled = globalvar.glSubscript;
            //            toolStripButton40.Enabled = globalvar.glp9;  // this is applicable only for Kenya
            toolStripButton41.Enabled = globalvar.glRunPay;
            toolStripButton42.Enabled = globalvar.glPostPay;
            toolStripButton43.Enabled = globalvar.glClosPay;
            toolStripButton44.Enabled = globalvar.glPayPara;
            //            toolStripButton56.Enabled = globalvar.glRevAdjust;


            //***************** HRM  Access Control
            toolStripButton45.Enabled = globalvar.glBasicData;
            toolStripButton46.Enabled = globalvar.glEmpBio;
            toolStripButton47.Enabled = globalvar.glDepData;
            toolStripButton48.Enabled = globalvar.glMedCover;
            toolStripButton49.Enabled = globalvar.glLivAppr;
            toolStripButton50.Enabled = globalvar.glDiscipl;
            toolStripButton51.Enabled = globalvar.glAbsence;
            toolStripButton52.Enabled = globalvar.glApprSal;
            toolStripButton53.Enabled = globalvar.glTraining;
            toolStripButton54.Enabled = globalvar.glAccident;
            toolStripButton55.Enabled = globalvar.glSuccess;
            toolStripButton57.Enabled = globalvar.glCareer;
            toolStripButton58.Enabled = globalvar.glGrievance;
            toolStripButton60.Enabled = globalvar.glExitServ;


            //***************** Procurement  Access Control
            toolStripButton9.Enabled = globalvar.glRfq;
            toolStripButton64.Enabled = globalvar.glPo;
            toolStripButton65.Enabled = globalvar.glDelivery;
            toolStripButton67.Enabled = globalvar.glMainStore;
            toolStripButton69.Enabled = globalvar.glItemSetup;
            toolStripButton70.Enabled = globalvar.glSupplMgt;
            toolStripButton59.Enabled = globalvar.glStockUpd;

            //***************** executive  Access Control
            toolStripButton8.Enabled = globalvar.glReqAppr;
            toolStripButton18.Enabled = globalvar.glPoAppr;
            toolStripButton76.Enabled = globalvar.glLivAppr;
            toolStripButton77.Enabled = globalvar.glAttendance;
            toolStripButton82.Enabled = globalvar.glDashBoard;
            toolStripButton83.Enabled = globalvar.glAcctRept;
            toolStripButton84.Enabled = globalvar.glAudRept;

            // System Admin
            toolStripButton71.Enabled = globalvar.glSysAdmin;
            updateAge();
           // updateAge2();
         //   updateAge3();
        }

        private void updateBalanceBar()
        {
            toolStripStatusLabel3.Text = globalvar.gcCashAccontBal.ToString(); //gnCashBalance.ToString("N2");// gnCashBalance;//  globalvar.gcToolbutton;
        }

        void updateAge()
        {
            SqlConnection ndConnHandle1 = new SqlConnection(cs);
            ndConnHandle1.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "UPDATE CUSREG SET nage = datediff(yy,ddatebirth,getdate())";
            cmd.Connection = ndConnHandle1;
            cmd.ExecuteNonQuery();
            ndConnHandle1.Close();
        }
        //void updateAge1()
        //{
        //    SqlConnection ndConnHandle1 = new SqlConnection(cs);
        //    ndConnHandle1.Open();
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandText = "UPDATE CUSREG SET mempict = null, memsign = null where ccustcode = '000950'";
        //    cmd.Connection = ndConnHandle1;
        //    cmd.ExecuteNonQuery();
        //    ndConnHandle1.Close();
        //}
        //void updateAge2()
        //{
        //    SqlConnection ndConnHandle1 = new SqlConnection(cs);
        //    ndConnHandle1.Open();
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandText = "UPDATE glmast SET  caccstat = 'A' where ccustcode = '000372'";
        //    cmd.Connection = ndConnHandle1;
        //    cmd.ExecuteNonQuery();
        //    ndConnHandle1.Close();
        //}
        //void updateAge3()
        //{
        //    SqlConnection ndConnHandle1 = new SqlConnection(cs);
        //    ndConnHandle1.Open();
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandText = "UPDATE glmast SET  caccstat = 'A' where ccustcode = '000379'";
        //    cmd.Connection = ndConnHandle1;
        //    cmd.ExecuteNonQuery();
        //    ndConnHandle1.Close();
        //}
        public class myRenderer : ToolStripProfessionalRenderer
        {
            public myRenderer() : base(new myColorTable())
            {

            }
        }

        public class myColorTable : ProfessionalColorTable
        {
            public override Color ButtonCheckedGradientBegin
            {
                get { return ButtonPressedGradientBegin; }
            }
            public override Color ButtonCheckedGradientEnd
            {
                get { return ButtonPressedGradientEnd; }
            }
            public override Color ButtonCheckedGradientMiddle
            {
                get { return ButtonPressedGradientMiddle; }
            }
        }
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

 
        private void deviceManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainSwitch myswitch = new MainSwitch();
            myswitch.ShowDialog();
        }

 
        private void consultationToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void triageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            app_q myqueue = new app_q();
            myqueue.ShowDialog();

        }

        private void consultationToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            app_q myqueue = new app_q();
            myqueue.ShowDialog();
        }

        private void specialClinicsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            app_q myqueue = new app_q();
            myqueue.ShowDialog();
        }

        private void todaysClientListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            app_q myqueue = new app_q();
            myqueue.ShowDialog();
        }

        private void medicalCoverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            app_q myqueue = new app_q();
            myqueue.ShowDialog();

        }

        private void admissionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            app_q myqueue = new app_q();
            myqueue.ShowDialog();

        }

        private void dischargeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            app_q myqueue = new app_q();
            myqueue.ShowDialog();

        }

        private void clientBillToolStripMenuItem_Click(object sender, EventArgs e)
        {
            app_q myqueue = new app_q();
            myqueue.ShowDialog();
        }

        private void clientWaitingListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            app_q myqueue = new app_q();
            myqueue.ShowDialog();

        }

        private void processTestRequestsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            app_q myqueue = new app_q();
            myqueue.ShowDialog();

        }

        private void updateTestResultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            app_q myqueue = new app_q();
            myqueue.ShowDialog();

        }

        private void validateTestResultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            app_q myqueue = new app_q();
            myqueue.ShowDialog();
        }

        private void clientWaitingListToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            app_q myqueue = new app_q();
            myqueue.ShowDialog();
        }

        private void processExaminationRequestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            app_q myqueue = new app_q();
            myqueue.ShowDialog();
        }

        private void updateExaminationResultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            app_q myqueue = new app_q();
            myqueue.ShowDialog();
        }

        private void validateExaminationResultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            app_q myqueue = new app_q();
            myqueue.ShowDialog();
        }

        private void clientWaitingListToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            app_q myqueue = new app_q();
            myqueue.ShowDialog();
        }

        private void clientCheckinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            app_q myqueue = new app_q();
            myqueue.ShowDialog();
        }

        private void updateProcedureRequestsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            app_q myqueue = new app_q();
            myqueue.ShowDialog();
        }

        private void validateResultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            app_q myqueue = new app_q();
            myqueue.ShowDialog();
        }

        private void clientWaitingListToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            app_q myqueue = new app_q();
            myqueue.ShowDialog();
        }

        private void clientAdministrationToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

   

  

        private void button1_Click(object sender, EventArgs e)
        {
            InventoryControl newrec = new InventoryControl();
            newrec.ShowDialog();
//            ClientTransfer newtrans = new ClientTransfer();
  //          newtrans.ShowDialog();
            //DeathConfirm newcover = new DeathConfirm();
            //newcover.ShowDialog();
//            Consultation newcover = new Consultation();
  //          newcover.ShowDialog();
        }

        private void clientReceivingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            app_q myqueue = new app_q();
            myqueue.ShowDialog();
        }

   

        private void receivingDiceasedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            app_q myqueue = new app_q();
            myqueue.ShowDialog();
        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            app_q myqueue = new app_q();
            myqueue.ShowDialog();
        }

        private void sendDeceasedToMortuaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            app_q myqueue = new app_q();
            myqueue.ShowDialog();
        }
        private void sendDeceasedToMortuaryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            app_q myqueue = new app_q();
            myqueue.ShowDialog();
        }

        private void toolStripMenuItem9_Click_1(object sender, EventArgs e)
        {
            app_q myqueue = new app_q();
            myqueue.ShowDialog();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
     //       ClientBill DisplayBill = new ClientBill();
       //     DisplayBill.ShowDialog();
        }

        private void transferToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void transferNotificationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            app_q myqueue = new app_q();
            myqueue.ShowDialog();
        }

        private void userManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //userManagement newUser = new userManagement();
            //newUser.ShowDialog();
        }

        private void userMgt_Click(object sender, EventArgs e)
        {
            //userManagement newrec = new userManagement();
            //newrec.ShowDialog();
        }

         private void featuresSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddRoomFeatures newrec = new AddRoomFeatures();
            newrec.ShowDialog();
        }

        private void stockTakeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InventoryControl newrec = new InventoryControl();
            newrec.ShowDialog();
        }

        private void itemSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InventoryControl newrec = new InventoryControl();
            newrec.ShowDialog();
        }

        private void supplierSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InventoryControl newrec = new InventoryControl();
            newrec.ShowDialog();
        }
        private void assetManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InventoryControl newrec = new InventoryControl();
            newrec.ShowDialog();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            InventoryControl newrec = new InventoryControl();
            newrec.ShowDialog();
        }

        private void drugsCategorySetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
//            OneCode newcode = new OneCode();
  //          newcode.ShowDialog();
        }

        private void drugsSubcategorySetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ThreeCode newcode = new ThreeCode();
            newcode.ShowDialog();
        }

        private void stockIssueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StockTransfer newstock = new StockTransfer();
            newstock.ShowDialog();
        }

        private void newRadExam_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton6_Click_1(object sender, EventArgs e)
        {

        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void toolStripContainer1_TopToolStripPanel_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem84_Click(object sender, EventArgs e)
        {

        }


        private void clientAdministrtionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExecutiveToolStrip.Visible = false;
            SystemAdministrationToolStrip.Visible = false;
            ClientAdminToolStrip.Visible = true;
            ClientAdminToolStrip.Location = new Point(0, 24);
            ClinicManToolStrip.Visible = false;
            AccountsToolStrip.Visible = false;
            HrToolStrip.Visible = false;
            PayrollToolStrip.Visible = false;
            ProcurementtoolStrip.Visible = false;
            ReportingtoolStrip1.Visible = false;
            inpatienttoolstrip.Visible = false;
        }

        private void toolStripButton28_Click(object sender, EventArgs e)
        {
            Members nmem = new WinTcare.Members();
            nmem.ShowDialog();
        }

        private void clinicalManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SystemAdministrationToolStrip.Visible = false;
            ClinicManToolStrip.Visible = true;
            ClinicManToolStrip.Location= new Point (0,24);
            PayrollToolStrip.Visible = false;
            AccountsToolStrip.Visible = false;
            HrToolStrip.Visible = false;
            ProcurementtoolStrip.Visible = false;
            ClientAdminToolStrip.Visible = false;
            ExecutiveToolStrip.Visible = false;
            ReportingtoolStrip1.Visible = false;
            inpatienttoolstrip.Visible = false;
        }

        private void toolStripButton26_Click(object sender, EventArgs e)
        {

        }

        private void accountsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SystemAdministrationToolStrip.Visible = false;
            AccountsToolStrip.Visible = true;
            AccountsToolStrip.Location = new Point(0, 24);
            ClinicManToolStrip.Visible = false;
            PayrollToolStrip.Visible = false;
            HrToolStrip.Visible = false;
            ProcurementtoolStrip.Visible = false;
            ClientAdminToolStrip.Visible = false;
            ExecutiveToolStrip.Visible = false;
            ReportingtoolStrip1.Visible = false;
            inpatienttoolstrip.Visible = false;
        }

        private void payrollToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SystemAdministrationToolStrip.Visible = false;
            ClientAdminToolStrip.Visible = false;
            PayrollToolStrip.Visible = true;
            PayrollToolStrip.Location = new Point(0, 24);
            ClinicManToolStrip.Visible = false;
            AccountsToolStrip.Visible = false;
            HrToolStrip.Visible = false;
            ProcurementtoolStrip.Visible = false;
            ExecutiveToolStrip.Visible = false;
            ReportingtoolStrip1.Visible = false;
            inpatienttoolstrip.Visible = false;
        }

        private void humanResourcesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SystemAdministrationToolStrip.Visible = false;
            ClientAdminToolStrip.Visible = false;
            PayrollToolStrip.Visible = false;
            HrToolStrip.Location = new Point(0, 24);
            ClinicManToolStrip.Visible = false;
            AccountsToolStrip.Visible = false;
            HrToolStrip.Visible = true;
            ProcurementtoolStrip.Visible = false;
            ExecutiveToolStrip.Visible = false;
            ReportingtoolStrip1.Visible = false;
            inpatienttoolstrip.Visible = false;
        }

        private void exitToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }


        private void toolStripButton12_Click_2(object sender, EventArgs e)
        {
            PayrollSetup pays = new PayrollSetup();
            pays.ShowDialog();
        }


        private void toolStripButton29_Click(object sender, EventArgs e)
        {
           
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click_2(object sender, EventArgs e)
        {
           
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TclassLibrary.chgPassword ala = new TclassLibrary.chgPassword(cs, cuser);
            ala.ShowDialog();
        }

        private void leaveApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            livApply dlapp = new livApply();
            dlapp.ShowDialog();
        }

        private void requisitionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Requisition dreq = new Requisition();
            dreq.ShowDialog();
        }

        private void toolStripButton19_Click(object sender, EventArgs e)
        {
            BatchSetup bat = new BatchSetup();
            bat.ShowDialog();
        }

        private void toolStripButton62_Click(object sender, EventArgs e)
        {
            PeriodicDues due = new PeriodicDues();
            due.ShowDialog();
        }

        private void procurementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SystemAdministrationToolStrip.Visible = false;
            ClientAdminToolStrip.Visible = false;
            PayrollToolStrip.Visible = false;
            ProcurementtoolStrip.Visible = true;
            ProcurementtoolStrip.Location = new Point(0, 24);
            ClinicManToolStrip.Visible = false;
            AccountsToolStrip.Visible = false;
            HrToolStrip.Visible = false;
            ExecutiveToolStrip.Visible = false;
            ReportingtoolStrip1.Visible = false;
            inpatienttoolstrip.Visible = false;
        }

        private void ClinicManToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripButton14_Click(object sender, EventArgs e)
        {
            memPayroll mp = new memPayroll();
            mp.ShowDialog();
        }



        private void toolStripButton13_Click(object sender, EventArgs e)
        {
            CashManager cm = new CashManager();
            cm.Show();
        }


        private void toolStripButton4_Click(object sender, EventArgs e)
        {
//            globalvar.gcSpSelect = "S";
//            globalvar.gnSpSelect = 6;
  //          spcselect qselect = new spcselect();
    //        qselect.ShowDialog();

//            Orders dord = new Orders();
  //          dord.ShowDialog();
        }

        private void toolStripButton30_Click_1(object sender, EventArgs e)
        {
            Journal djo = new Journal();
            djo.ShowDialog();
        }

        private void systemAdminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SystemAdministrationToolStrip.Visible = true;
            ClinicManToolStrip.Visible = false;
            SystemAdministrationToolStrip.Location = new Point(0, 24);
            PayrollToolStrip.Visible = false;
            AccountsToolStrip.Visible = false;
            HrToolStrip.Visible = false;
            ProcurementtoolStrip.Visible = false;
            ClientAdminToolStrip.Visible = false;
            ExecutiveToolStrip.Visible = false;
            ReportingtoolStrip1.Visible = false;
            inpatienttoolstrip.Visible = false;
        }

        private void toolStripButton71_Click(object sender, EventArgs e)
        {
            userManagement duser = new userManagement(cs,ncompid,cloc);
            duser.ShowDialog();

        }

         private void toolStripButton21_Click(object sender, EventArgs e)
        {
           // MessageBox.Show("This is the connection string " + cs);
            TclassLibrary.acctEnquiry dac = new TclassLibrary.acctEnquiry(cs, ncompid, cloc);
            dac.Show();

        }

        private void toolStripButton25_Click(object sender, EventArgs e)
        {
            EndOfYear dact = new EndOfYear();
            dact.Show();
        }

        private void toolStripButton16_Click(object sender, EventArgs e)
        {
            deposits ddep = new deposits("01");
            ddep.RefreshNeeded += new EventHandler(RefreshData);
           ddep.Show();
        }

        private void toolStripButton74_Click(object sender, EventArgs e)
        {
            TclassLibrary.tranupdate ver = new TclassLibrary.tranupdate(cs, ncompid, cloc, globalvar.gcUserid,globalvar.gnBranchid,globalvar.gnCurrCode,globalvar.gcWorkStation,globalvar.gcWinUser);
            ver.Show();
        }

        private void toolStripButton23_Click(object sender, EventArgs e)
        {
            transfers trs = new transfers(2);
            trs.Show();
            //deposits ddep = new deposits("03");
            //ddep.ShowDialog();
        }

        private void toolStripButton24_Click(object sender, EventArgs e)
        {
            acRec drec = new acRec();
            drec.Show();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ExecutiveToolStrip.Visible = true;
            ExecutiveToolStrip.Location = new Point(0, 24);
            SystemAdministrationToolStrip.Visible = false;
            ClientAdminToolStrip.Visible = false;
            PayrollToolStrip.Visible = false;
            ProcurementtoolStrip.Visible = false;
            ClinicManToolStrip.Visible = false;
            AccountsToolStrip.Visible = false;
            HrToolStrip.Visible = false;
            ReportingtoolStrip1.Visible = false;
            inpatienttoolstrip.Visible = false;
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            Reqappr dreqap = new Reqappr();
            dreqap.ShowDialog();
        }

        private void toolStripButton18_Click(object sender, EventArgs e)
        {
            lpoAppr dlpoapp = new lpoAppr();
            dlpoapp.ShowDialog();
        }

        private void toolStripButton76_Click(object sender, EventArgs e)
        {
            LivAppr dlap = new LivAppr();
            dlap.ShowDialog();
        }

        private void toolStripButton77_Click(object sender, EventArgs e)
        {
            Attendance datt = new Attendance();
            datt.ShowDialog();
        }

        private void toolStripButton75_Click(object sender, EventArgs e)
        {
            reconcile drec = new reconcile();
            drec.ShowDialog();
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            rfq drfq = new rfq();
            drfq.ShowDialog();
        }

        private void toolStripButton64_Click(object sender, EventArgs e)
        {
            PurchaseOrder dpo = new PurchaseOrder();
            dpo.ShowDialog();
        }

        private void toolStripButton65_Click(object sender, EventArgs e)
        {
            PurchaseDelivery dpdel = new PurchaseDelivery();
            dpdel.ShowDialog();
        }

        private void toolStripButton45_Click(object sender, EventArgs e)
        {
            emplyee dbas = new emplyee();
            dbas.ShowDialog();
        }

        private void toolStripButton3_Click_1(object sender, EventArgs e)
        {
            Scorders dord = new Scorders();
            dord.ShowDialog();
        }

        private void toolStripButton67_Click(object sender, EventArgs e)
        {
            Scorders dmord = new Scorders();
            dmord.ShowDialog();
        }

        private void toolStripButton22_Click(object sender, EventArgs e)
        {

           GenLedgerMgt duser = new GenLedgerMgt(cs, ncompid, cloc, globalvar.gnBranchid, getStartupFolder.gcStartUpDirectory, globalvar.gcCompanyHeader,globalvar.gcUserid);
            duser.ShowDialog();
        }

        private void toolStripButton32_Click(object sender, EventArgs e)
        {
            loanAmort lam = new loanAmort();
            lam.Show();
        }

        private void toolStripButton53_Click(object sender, EventArgs e)
        {
            staffTrain dstrn = new staffTrain();
            dstrn.ShowDialog();
        }

        private void toolStripButton50_Click(object sender, EventArgs e)
        {
            Discipline ddis = new Discipline();
            ddis.ShowDialog();
        }

        private void toolStripButton51_Click(object sender, EventArgs e)
        {
            Absence dab = new Absence();
            dab.ShowDialog();
        }

        private void toolStripButton58_Click(object sender, EventArgs e)
        {
            grievance dgri = new grievance();
            dgri.ShowDialog();
        }

        private void toolStripButton54_Click(object sender, EventArgs e)
        {
            accidents dacc = new accidents();
            dacc.ShowDialog();
        }

        private void toolStripButton52_Click(object sender, EventArgs e)
        {
            Apraisal dapr = new Apraisal();
            dapr.ShowDialog();
        }

        private void toolStripButton57_Click(object sender, EventArgs e)
        {
            career dcar = new career();
            dcar.ShowDialog();
        }

        private void toolStripButton47_Click(object sender, EventArgs e)
        {
            dependents ddep = new dependents();
            ddep.ShowDialog();
        }

        private void toolStripButton60_Click(object sender, EventArgs e)
        {
            ExitService dxit = new ExitService();
            dxit.ShowDialog();
        }

        private void toolStripButton49_Click(object sender, EventArgs e)
        {
            LivAppr dliv = new LivAppr();
            dliv.ShowDialog();
        }

        private void toolStripButton48_Click(object sender, EventArgs e)
        {
            medRegister dmreg = new medRegister();
            dmreg.ShowDialog();
        }

        private void toolStripButton46_Click(object sender, EventArgs e)
        {
            bioDetails dbio = new bioDetails();
            dbio.ShowDialog();
        }

        private void toolStripButton55_Click(object sender, EventArgs e)
        {
            succPlan dsuc = new succPlan();
            dsuc.ShowDialog();
        }

        private void toolStripButton34_Click(object sender, EventArgs e)
        {
            staff dst = new staff();
            dst.ShowDialog();
        }

        private void updatePersonalDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            employees demp = new employees();
            demp.ShowDialog();
        }

        private void toolStripButton35_Click(object sender, EventArgs e)
        {
            partSalary dpatsal = new partSalary();
            dpatsal.ShowDialog();
        }

        private void toolStripButton36_Click(object sender, EventArgs e)
        {
            staffAllowance dstaffall = new staffAllowance();
            dstaffall.ShowDialog();
        }

        private void toolStripButton37_Click(object sender, EventArgs e)
        {
            staffLoans dloans = new staffLoans();
            dloans.ShowDialog();
        }

        private void toolStripButton38_Click(object sender, EventArgs e)
        {
            overTime dovt = new overTime();
            dovt.ShowDialog();
        }

        private void toolStripButton39_Click(object sender, EventArgs e)
        {
            salAdvance dsalv = new salAdvance();
            dsalv.ShowDialog();
        }

        private void toolStripButton61_Click(object sender, EventArgs e)
        {
            staffSubs dsubs = new staffSubs();
            dsubs.ShowDialog();
        }

        private void toolStripButton41_Click(object sender, EventArgs e)
        {
            runPayroll dpay = new runPayroll();
            dpay.ShowDialog();
        }

        private void toolStripButton43_Click(object sender, EventArgs e)
        {
            closPayroll dcls = new closPayroll();
            dcls.ShowDialog();
        }

        private void todayListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selecCU su = new selecCU();
            su.ShowDialog();
            //            todayList dtlist = new todayList();
  //          dtlist.ShowDialog();
        }

        private void reportingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReportingtoolStrip1.Visible = false;
            ReportingtoolStrip1.Location = new Point(0, 24);
            SystemAdministrationToolStrip.Visible = false;
            AccountsToolStrip.Visible = false;
            ClinicManToolStrip.Visible = false;
            PayrollToolStrip.Visible = false;
            HrToolStrip.Visible = false;
            ProcurementtoolStrip.Visible = false;
            ClientAdminToolStrip.Visible = false;
            ExecutiveToolStrip.Visible = false;
            inpatienttoolstrip.Visible = false;
        }

        private void toolStripButton93_Click(object sender, EventArgs e)
        {
            loanApproval lap = new loanApproval();
            lap.Show();
        }

        private void MainMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            foreach (ToolStripMenuItem item in ((ToolStrip)sender).Items)
            {
                if (item != e.ClickedItem)
                    item.BackColor = Color.White;
                else
                    item.BackColor = Color.FromArgb(217, 244, 253);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void SystemAdministrationToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripButton69_Click(object sender, EventArgs e)
        {

        }

        private void AccountsToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripButton31_Click_1(object sender, EventArgs e)
        {

        }

        private void toolStripButton10_Click_2(object sender, EventArgs e)
        {

        }

        private void inpatientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SystemAdministrationToolStrip.Visible = false;
            ClientAdminToolStrip.Visible = false;
            inpatienttoolstrip.Visible = true;
            inpatienttoolstrip.Location = new Point(0, 24);
            ClinicManToolStrip.Visible = false;
            AccountsToolStrip.Visible = false;
            HrToolStrip.Visible = false;
            ProcurementtoolStrip.Visible = false;
            ExecutiveToolStrip.Visible = false;
            ReportingtoolStrip1.Visible = false;
        }

        private void toolStripButton95_Click_1(object sender, EventArgs e)
        {
            InterestCalculation bat = new InterestCalculation();
            bat.ShowDialog();
        }

        private void toolStripButton105_Click(object sender, EventArgs e)
        {
            loanGuarantor lag = new loanGuarantor();
            lag.Show();
        }

        private void toolStripButton104_Click(object sender, EventArgs e)
        {
            LoanApplication lapp = new WinTcare.LoanApplication();
            lapp.Show();
        }

        private void toolStripButton94_Click(object sender, EventArgs e)
        {

        }

        private void printerSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton2_Click_1(object sender, EventArgs e)
        {
            myLocation myl = new WinTcare.myLocation();
            myl.ShowDialog();
        }

        private void toolStripButton3_Click_2(object sender, EventArgs e)
        {
            memberAttach ma = new WinTcare.memberAttach();
            ma.Show();
        }

        private void toolStripButton4_Click_1(object sender, EventArgs e)
        {
            LoanDisburse ld = new LoanDisburse();
            ld.Show();
        }

        private void transactionEnquiryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            REPTransactionEnquiry trans = new REPTransactionEnquiry();
            trans.ShowDialog();
        }

        private void transactionListingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            REPTransactionListing tranl = new REPTransactionListing();
            tranl.ShowDialog();
        }

        private void membersEnquiryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            REPMemberReport mem = new REPMemberReport();
            mem.ShowDialog();
        }

        private void loansReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            REPLoans loa = new REPLoans();
            loa.ShowDialog();
        }

        private void chequeEnquiryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            REPChequeEnquiry che = new REPChequeEnquiry();
            che.ShowDialog();
        }

        private void loanAgingReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            REPTrialBalance trl = new REPTrialBalance();
            trl.ShowDialog();
        }

        private void arreasProvissionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            REPIncomeStatement inc = new REPIncomeStatement();
            inc.ShowDialog();
        }

        private void balanceSheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            REPBalanceSheet bal = new REPBalanceSheet();
            bal.ShowDialog();
        }

        private void memberByAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            REPMemberByAccount mem = new REPMemberByAccount();
            mem.ShowDialog();
        }

        private void ageAnalysisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            REPAgeAnalysis age = new REPAgeAnalysis();
            age.ShowDialog();
        }

        private void payrollBatchReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            REPBatchReport bat = new REPBatchReport();
            bat.ShowDialog();
        }

        private void memberStatementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            REPMemberStatement sta = new REPMemberStatement();
            sta.ShowDialog();

        }

        private void accountByGenderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            REPAgeByAccount ade = new REPAgeByAccount();
            ade.ShowDialog();
        }

        private void agingReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            REPDetailedAging aging = new REPDetailedAging();
            aging.ShowDialog();
        }

        private void accountListingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            REPAccounts acct = new REPAccounts();
            acct.ShowDialog();
        }

        private void loansRejectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            REPLoans lrej = new REPLoans();
            lrej.ShowDialog();
        }

        private void guarantorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            REPGuarantors gua = new REPGuarantors();
            gua.ShowDialog();
        }

        private void toolStripButton17_Click(object sender, EventArgs e)
        {
            deposits ddep = new deposits("02");
            ddep.RefreshNeeded += new EventHandler(RefreshData);
            ddep.Show();
        }

        private void toolStripButton15_Click(object sender, EventArgs e)
        {
            deposits ddep = new deposits("07");
            ddep.RefreshNeeded += new EventHandler(RefreshData);
            ddep.Show();
        }

        private void savingsByValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            REPSavingsByValue sav = new REPSavingsByValue();
            sav.ShowDialog();
        }

        private void loanByValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            REPLoanByValue lon = new REPLoanByValue();
            lon.ShowDialog();
        }

        private void savingsBalancesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            REPSavingsBalances bal = new REPSavingsBalances();
            bal.ShowDialog();
        }

        private void loanBalancesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            REPLoanBalances lon = new REPLoanBalances();
            lon.ShowDialog();
        }

        private void toolStripButton5_Click_2(object sender, EventArgs e)
        {
            dashBoard das = new dashBoard();
            das.ShowDialog();
        }

        private void dashBoardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dashBoard das = new dashBoard();
            das.ShowDialog();
        }

        private void toolStripButton27_Click_1(object sender, EventArgs e)
        {
            chargeoff ch = new chargeoff(1);
            ch.ShowDialog();
        }

        private void toolStripButton11_Click_1(object sender, EventArgs e)
        {
            transfers trf = new transfers(1);
            trf.ShowDialog();
        }

        private void toolStripButton6_Click_3(object sender, EventArgs e)
        {
            TclassLibrary.Reversal rv = new TclassLibrary.Reversal(cs, ncompid, cloc, globalvar.gdSysDate, globalvar.gcUserid, globalvar.gnBranchid, globalvar.gnCurrCode, "N");
            rv.ShowDialog();

            //Reversal rv = new Reversal(cs, ncompid, gcLocalCaption, globalvar.gdSysDate, globalvar.gcUserid, globalvar.gnBranchid, globalvar.gnCurrCode);
            //rv.ShowDialog();
        }

        private void journalReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            REPJournalEnquiry jou = new REPJournalEnquiry();
            jou.ShowDialog();
        }

        private void toolStripButton63_Click_1(object sender, EventArgs e)
        {
            MemberClose clo = new MemberClose("A");
            clo.ShowDialog();
        }

        private void loanSheduleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FRMLoanShedule lo = new FRMLoanShedule();
            lo.ShowDialog();
        }

        private void toolStripButton66_Click_1(object sender, EventArgs e)
        {
            MemberClose clo = new MemberClose("C");
            clo.ShowDialog();
        }

        private void auditTrailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            REPauditTrail aud = new REPauditTrail();
            aud.ShowDialog();
        }

        private void socialFinancialDataDashboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SocialData soc = new SocialData();
            soc.ShowDialog();
        }

        private void toolStripButton106_Click(object sender, EventArgs e)
        {
            chargeoff ch = new chargeoff(2);
            ch.ShowDialog();
        }

        private void toolStripButton29_Click_1(object sender, EventArgs e)
        {
            chargeoff ch = new chargeoff(3);
            ch.ShowDialog();

        }

        private void dashBoardToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            dashBoard das = new dashBoard();
            das.ShowDialog();
        }

        private void toolStripButton20_Click_1(object sender, EventArgs e)
        {

        }

        private void toolStripButton106_Click_1(object sender, EventArgs e)
        {
            chargeoff ch = new chargeoff(2);
            ch.ShowDialog();
        }

        private void toolStripButton10_Click_3(object sender, EventArgs e)
        {
            chargeoff ch = new chargeoff(1);
            ch.ShowDialog();
        }

        private void toolStripButton5_Click_3(object sender, EventArgs e)
        {
            MemberAcct ma = new MemberAcct();
            ma.ShowDialog();
        }

        private void toolStripButton104_Click_1(object sender, EventArgs e)
        {
            LoanApplication lapp = new WinTcare.LoanApplication();
            lapp.ShowDialog();
        }

        private void toolStripButton93_Click_1(object sender, EventArgs e)
        {
            loanApproval lap = new loanApproval();
            lap.ShowDialog();
        }

        private void toolStripButton105_Click_1(object sender, EventArgs e)
        {
            loanGuarantor lag = new loanGuarantor();
            lag.ShowDialog();
        }

        private void toolStripButton32_Click_1(object sender, EventArgs e)
        {
            loanAmort lam = new loanAmort();
            lam.ShowDialog();
        }

        private void toolStripButton4_Click_2(object sender, EventArgs e)
        {
            LoanDisburse ld = new LoanDisburse();
            ld.ShowDialog();
        }

        private void toolStripButton28_Click_1(object sender, EventArgs e)
        {
            Members nmem = new WinTcare.Members();
            nmem.Show();
        }

        private void toolStripButton3_Click_3(object sender, EventArgs e)
        {
            memberMsg ma = new memberMsg();
            ma.ShowDialog();
        }

        private void toolStripButton14_Click_1(object sender, EventArgs e)
        {
            memPayroll mp = new memPayroll();
            mp.ShowDialog();
        }

        private void toolStripButton63_Click_2(object sender, EventArgs e)
        {
            MemberClose mc = new WinTcare.MemberClose("A");
            mc.ShowDialog();
        }

        private void toolStripButton12_Click_3(object sender, EventArgs e)
        {

        }

        private void toolStripButton72_Click(object sender, EventArgs e)
        {
            //ProductDef dpr = new ProductDef(cs, ncompid, cLocalCaption);
            //dpr.ShowDialog();
        }

        private void toolStripButton107_Click(object sender, EventArgs e)
        {
            InterestCalculation bat = new InterestCalculation();
            bat.ShowDialog();
        }

        private void toolStripButton108_Click(object sender, EventArgs e)
        {
            TclassLibrary.fixrbal fxb = new TclassLibrary.fixrbal(cs, ncompid, cloc, globalvar.gdSysDate, globalvar.gcUserid, globalvar.gnBranchid);
            fxb.ShowDialog();
        }

        private void toolStripButton66_Click_2(object sender, EventArgs e)
        {
            MemberClose mc = new WinTcare.MemberClose("C");
            mc.ShowDialog();
        }

        private void toolStripButton72_Click_1(object sender, EventArgs e)
        {
          //  WhatsAppForm mc = new WinTcare.WhatsAppForm();
           // mc.ShowDialog();
        }

        private void toolStripButton73_Click_1(object sender, EventArgs e)
        {
            //PeriodicDues due = new PeriodicDues();
            //due.ShowDialog();
        }

        private void toolStripButton110_Click(object sender, EventArgs e)
        {
            AccountClose mc = new WinTcare.AccountClose("C");
            mc.ShowDialog();
        }

        private void toolStripButton109_Click(object sender, EventArgs e)
        {
            AccountClose mc = new WinTcare.AccountClose("A");
            mc.ShowDialog();
        }

        private void toolStripButton2_Click_2(object sender, EventArgs e)
        {

        }

        private void toolStripStatusLabel3_Click(object sender, EventArgs e)
        {

        }

        private void MainMenu_Paint(object sender, PaintEventArgs e)
        {
            toolStripStatusLabel3.Text = globalvar.gcCashAccontBal.ToString();  
        }

        private void toolStripButton24_Click_1(object sender, EventArgs e)
        {
            ReprintForm rpf = new ReprintForm();
            rpf.ShowDialog();
        }

        private void transactionSummaryReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            REPTransactionSummary reptransaction = new REPTransactionSummary();
            reptransaction.ShowDialog();
        }

        private void lockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FRMLock frmlock = new FRMLock();
            frmlock.ShowDialog();
        }

        private void arreasProvisionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            REPLoanProvision frmpro = new REPLoanProvision();
            frmpro.ShowDialog();
        }

        private void detailedJournalReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            REPGLStatement frm = new REPGLStatement();
            frm.Show();
        }

        private void cashManagerReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            REPCashManager frm = new REPCashManager();
            frm.Show();
        }

        private void receiptReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            REPReceiptListing al = new REPReceiptListing();
            al.Show();
        }
    }
}
    internal class Doctors
    {
        internal void ShowDialog()
        {
      //      throw new NotImplementedException();
        }
    }

