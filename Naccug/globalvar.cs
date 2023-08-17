using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;

namespace WinTcare
{
    public class getStartupFolder
    {
        public static string gcStartUpDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
    }
    public static class globalvar
    {
        /******************global variables used throughout the solution**************************************************/

        // Connection String Name
        public static string gcServername = "";
        public static string gcDatabaseName = "";
        public static string gcspassword = "";
        public static string gcsusername = "";
        public static string gcComname = "";
       //  public static string gcspassword = "";

        public static string cos = "Server = " + gcServername.Trim() + "; Database =" + gcDatabaseName.Trim() + "; User ID = " + gcsusername.Trim() + "; Password= " + gcspassword.Trim() + "; connection Timeout = 3000000"; //connection to local database
        public const string cLocalCaption = "NaccugSoft - Credit Union Management Solution";
        public const string gcCopyRight = "Copyright Naccug System. All Rights Reserved.";
        public const string cUserType = "M";
        public const string ClientAcctPrefix = "104";
        public static string gcCustCode = "";

        public static string gcSaveCtrl_Ind = "";
        public static string gcSaveCtrl_Cor = "";
        public static string gcSaveCtrl_Grp = "";
        public static string gcSaveCtrl_Stf = "";

        public static string gcShareCtrl_Ind = "";
        public static string gcShareCtrl_Cor = "";
        public static string gcShareCtrl_Grp = "";
        public static string gcShareCtrl_Stf = "";

        public static string gcLoanCtrl_Ind = "";
        public static string gcLoanCtrl_Cor = "";
        public static string gcLoanCtrl_Grp = "";
        public static string gcLoanCtrl_Stf = "";
        public static string gcToolbutton = "";

        public static DateTime gdSysDate;
        public static string gcVoucherno;
        public static string gcAcctNumb = "";
        public static string gcIntSuspense = "";
        public static string gcMacAddress = "";
        public static string gcWinUser = "";
        public static string gcWorkStation = "";
        public static string gcFormCaption = "";
        public static int gnServiceCentreID;
        public static int gnCompid; //cos
      //  public static string cos = "";
        public static int gnCou_ID;
        public static int gnDefaultCountry;
        public static int gnSpSelect;
        public static string gcSpSelect = "";
        public static int gnQueueID;
        public static int gnVisno=0;
        public static int gnIntDocID;
        public static int gnChargeoffloor;
        public static int gnWriteoffloor;
        public static decimal gnDebitLimit=0.00m;
        public static decimal gnCreditLimit=0.00m;
        public static decimal gnLoanLimit=0.00m;
        public static decimal gcCashAccontBal = 0.00m;
        public static decimal gnCashierBalance = 0.00m;

        public static bool glSysAdmin;

        //public static string gcCurrCode;
        public static int gnCurrCode;
        public static string gcCurrName;
        public static string gcCurrUnit;   
        public static string gcUserid;
        public static bool glFreeBee;

        public static bool gldread;
        public static bool gldcreate;
        public static bool gldamend;
        public static bool gldhide;

        //user access definition
        public static bool glMemberRegistration;  
        public static bool glMemberAttach;      
        public static bool glMemberPayroll;
        public static bool glLoanApply;
        public static bool glLoanAmort;
        public static bool glLoanAddGrantor;
        public static bool glLoanEdtGrantor;
        public static bool glLoanDelGrantor;
        public static bool glMemberAddAcct;

        public static bool glAccountAdd;
        public static bool glAccountEdit;
        public static bool glAccountDel;

        public static bool glMemberEdit;
        public static bool glMemberclose;
        public static bool glMemberActive;
        public static bool glLoanChargeOff;
        public static bool glLoanActive;
        public static bool glLoanWriteOff;
        public static bool glLoanApproval;
        public static bool glLoanIssue;

        public static bool glDeposit; 
        public static bool glWithDraw;
        public static bool glLoanPay;
        public static bool glJournal;
        public static bool glVerify;
        public static bool glAcctEnq;

        public static bool glCreateUser;
        public static bool glCreateEmplyr;
        public static bool glCreateBank;
        public static bool glCreateBnkBranch;
        public static bool glCreateCmpBranch;
        public static bool glCreateCompany;
        public static bool glCreateSector;
        public static bool glCreateInst;
        public static bool glCreateDept;
        public static bool glCreateCity;
        public static bool glCreateCountry;
        public static bool glCreateLocation;
        public static bool glCreateLoanReject;
        public static bool glCreateProduct;
        public static bool glCreateRegion;
        public static bool glCreateDistrict;
        public static bool glCreateYard;
        public static bool glEditUser;
        public static bool glActivateUser;
        public static bool glHideUser;
        public static bool glEditEmplyr;
        public static bool glActiveEmplyr;
        public static bool glHideEmplyr;
        public static bool glEditDistrict;
        public static bool glActiveDistrict;
        public static bool glHideDistrict;
        public static bool glEditRegion;
        public static bool glActiveRegion;
        public static bool glHideRegion;
        public static bool glActiveProduct;
        public static bool glHideProduct;
        public static bool glEditCity;
        public static bool glActiveCity;
        public static bool glHideCity;
        public static bool glEditLoanReject;
        public static bool glActiveLoanReject;
        public static bool glHideLoanReject;

        public static bool glEditLocation;
        public static bool glActiveLocation;
        public static bool glHideLocation;

        public static bool glEditDept;
        public static bool glActiveDept;
        public static bool glHideDept;

        public static bool glEditCountry;
        public static bool glActiveCountry;
        public static bool glHideCountry;

        public static bool glEditInsType;
        public static bool glActiveInsType;
        public static bool glHideInsType;

        public static bool glEditBnkBranch;
        public static bool glActiveBnkBranch;
        public static bool glHideBnkBranch;

        public static bool glEditCompany;
        public static bool glActiveCompany;
        public static bool glHideCompany;

        public static bool glEditBank;
        public static bool glActiveBank;
        public static bool glHideBank;

        public static bool glEditBranch;
        public static bool glActiveBranch;
        public static bool glHideBranch;

        public static bool glEditSector;
        public static bool glActiveSector;
        public static bool glHideSector;

        public static bool glBatchSetup;
        public static bool glBatchProcess;
        public static bool glBatchLoanApply;

        public static bool glBatchLoanAppr;
        public static bool glBatchLoanIssue;

        public static bool glLoanPayout;
        public static bool glCashManager;
        public static bool glAcctPay;
        public static bool glAcctRec;
        public static bool glBudget;
        public static bool glAttendance;

        public static bool glStatements;
        public static bool glAcctRecon;
        public static bool glInterestCalc;

        public static bool glTransfers;
        public static bool glRevAdjust;
        public static bool glGenLedger;

        public static bool glOpsRept;
        public static bool glMemRept;
        public static bool glLonRept;
        public static bool glPorRept;
        public static bool glGlRept;
        public static bool glAudRept;
        public static bool glSocRept;

        public static bool glPartSal;
        public static bool glRunPay;
        public static bool glAllo;
        public static bool glLoans;
        public static bool glOvtime;
        public static bool glSalAdv;

        public static bool glSubscript;
        public static bool glPostPay;
        public static bool glClosPay;
        public static bool glPayPara;

        public static bool glBasicData;
        public static bool glEmpBio;
        public static bool glDepData;
        public static bool glMedCover;
        public static bool glAbsence;
        public static bool glTraining;

        public static bool glAccident;
        public static bool glLivAppr;
        public static bool glDiscipl;
        public static bool glApprSal;
        public static bool glCareer;
        public static bool glSuccess;

        public static bool glGrievance;
        public static bool glExitServ;

        public static bool glRfq;
        public static bool glPo;
        public static bool glDelivery;
        public static bool glSupplMgt;

        public static bool glItemSetup;
        public static bool glMainStore;
        public static bool glStockUpd;
        public static bool glAssetMgt;
        public static bool glReqAppr;
        public static bool glPoAppr;

        public static bool glWavAppr;
        public static bool glDashBoard;
        public static bool glStatistics;

        public static bool glPayRept;
        public static bool glHrmRept;
        public static bool glProcRept;
        public static bool glAcctRept;

        //other security levels
        public static int gnAccessLevel;
        public static int gnGroupID;
        public static string gcUserName;
        public static int gnBranchid;
        public static string gcCashAccont;
        public static string gcMobileCashAccont;
        public static string gcCashAccontName;
        public static bool glIscasier;

        //company details
        public static string gcIDDCODE;
        public static string gcCompName ;
        public static string gcCompanyHeader;
        public static string gcCompAddr;
        public static string gcCompPobox;
        public static string gcCompTel;
        public static string gcCompFax;  
        public static decimal gnCorpTax ;  
        public static int gnAnonComp ;  
        public static string gcLocalCountry;  
        public static decimal  gnVat ;  
        public static decimal gnNhifRebate ; 
        public static bool glBioLink ;  
        public static int gnToddlerAge ;  
        public static int gnChildAge ;  
        public static decimal gnXchRate ;  
        public static string gcCompMail; 
        public static int gnWeekHrs ; 
        public static decimal gnCompInj ;  
        public static decimal gnCompCont ;  
        public static decimal gnCompInd ;  
        public static decimal gnOvt50 ;  
        public static decimal gnOvt100 ;  
        public static string gcCurrSym;  
        public static bool gl4Profit ;  
        public static string gcPayPer;  
        public static decimal gnMaxSalAdv ;  
        public static decimal gnSalCap ;  
        public static decimal gnPerReli ;  
        public static int gnRetireAge ;
        public static string gcSavingsCode = "250";
        public static string gcShariaSavingsCode = "251";
        public static string gcSharesCode = "270";
        public static string gcShariaSharesCode = "271";
        public static string gcLoansCode = "130";
        public static string gcShariaLoansCode = "131";
        public static bool glGrantee2Sav = false;
        public static decimal gnGuarPercent = 0.00m;
        public static int gnLoanChargeOff = 0;
    }


    public class Calculator
    {
        public double Add(double num1, double num2)
        {
            return num1 + num2;
        }
    }

    public class ScientificCalculator:Calculator
    {
        public double Power(double num, double power )
        {
            return Math.Pow(num, power);
        }
    }

}
