using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace TclassLibrary
{

    public class genbill
    {
        public static string genvoucher(string cs, DateTime dsysdate)
        {
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
            {
                string susp = "select nvoucherno from client_code";
                string cvoucher = "";
                int vcounter;
                SqlDataAdapter da2 = new SqlDataAdapter(susp, ndConnHandle3);
                ndConnHandle3.Open();
                DataTable ds = new DataTable();
                da2.Fill(ds);
                if (ds != null)
                {
                    vcounter = Convert.ToInt32(ds.Rows[0]["nvoucherno"]);
                    if (Convert.ToUInt32(ds.Rows[0]["nvoucherno"]) >= 9999)
                    {
                        cvoucher = dsysdate.Year.ToString().Substring(2, 2) + dsysdate.Month.ToString().PadLeft(2, '0') + dsysdate.Day.ToString().PadLeft(2, '0') + "0001";
                        return cvoucher;
                    }
                    else
                    {
                        cvoucher = dsysdate.Year.ToString().Substring(2, 2) + dsysdate.Month.ToString().PadLeft(2, '0') + dsysdate.Day.ToString().PadLeft(2, '0') + vcounter.ToString().PadLeft(4, '0');
                        return cvoucher;
                    }
                }
                else { return ""; }
            }
        } //end of genvoucher
    }// end of genbill 

    public class getReportFile
    {
        public static bool dReportfile(string tcPath, string tcReport)
        {
            string dpath = Path.Combine(tcPath, tcReport);
            bool drepexists = File.Exists(dpath);
            if (!drepexists)
            {
                MessageBox.Show("The report file does not exist in startup folder " + tcPath);
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool dLogofile(string tcPath, string tcLogo)
        {
            string dpath = Path.Combine(tcPath, tcLogo);
            bool dlogoexists = File.Exists(dpath);
            if (!dlogoexists)
            {
                MessageBox.Show("The Company header file <<" + tcLogo + ">> does not exist in startup folder " + tcPath);
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    public class LoanAmortizationSchedule
    {
        public void loanAmortization(string cs, int tnloanid, double tnLoanAmt, double tnLoanInt, int tnLoanDur, DateTime tdStartDate)
        {
            DataTable amortview = new DataTable();
            int nyrpay = 12;            //this is the number of payments per year
            double gnOrigLoan = Math.Round(tnLoanAmt, 2);// Math.Round(gnLoanAmt, 2);
            double gnCumuInt = 0.00;
            double totalprin = 0.00;
            double totalint = 0.00;
            double nintpay = 0.00;
            double nprinpay = 0.00;
            double nfactor = tnLoanAmt / tnLoanDur;// gnLoanAmt / loanDur;
            double nPrincipal = tnLoanAmt;// gnLoanAmt;// - nfactor;
            double tnPerPay = 0.00;
            DateTime dStartDate = new DateTime();
            double nPeriodicInt = Math.Pow(1 + (tnLoanInt / 100), 1.0 / 12) - 1;

            for (int j = 1; j <= tnLoanDur; j++)
            {
                double newrate = tnLoanInt / 100 / nyrpay;
                if (tnLoanInt > 0.00)
                {
                    tnPerPay = loanCalculation.pmt(newrate, tnLoanDur, tnLoanAmt, 0.00, 0);  // Fixed Periodic Payment
                    nintpay = loanCalculation.ipmt(newrate, j, tnLoanDur, tnLoanAmt, 0.00, 0); // Interest portion of the Fixed periodic payment
                    nprinpay = loanCalculation.ppmt(newrate, j, tnLoanDur, tnLoanAmt, 0.00, 0);   // Principal payment of the periodic payment 
                }
                else
                {
                    tnPerPay = tnLoanAmt / tnLoanDur;// loanCalculation.pmt(newrate, loanDur, gnLoanAmt, 0.00, 0);  // Fixed Periodic Payment
                    nintpay = 0.00;// loanCalculation.ipmt(newrate, j, loanDur, gnLoanAmt, 0.00, 0); // Interest portion of the Fixed periodic payment
                    nprinpay = tnPerPay;// loanCalculation.ppmt(newrate, j, loanDur, gnLoanAmt, 0.00, 0);   // Principal payment of the periodic payment 
                }
                gnOrigLoan = Math.Round(gnOrigLoan, 2);
                gnCumuInt = Math.Round((gnCumuInt + Math.Abs(nintpay)), 2);
                nPrincipal = Math.Abs(nPrincipal) - Math.Abs(nprinpay);
                totalprin = Math.Abs(totalprin) + Math.Abs(nprinpay);
                totalint = Math.Abs(totalint) + Math.Abs(nintpay);
                dStartDate = tdStartDate.AddMonths(j);//.ToShortDateString();
                tnPerPay = Math.Round(tnPerPay, 2);
                nprinpay = Math.Round(nprinpay, 2);
                nintpay = Math.Round(nintpay, 2);

                insertAmort(cs, dStartDate, tnPerPay, nprinpay, nintpay, gnOrigLoan, nPrincipal, gnCumuInt, tnloanid, j, tnPerPay);
                gnOrigLoan = Math.Round(Math.Abs(gnOrigLoan), 2) - Math.Round(Math.Abs(nprinpay), 2);
            }
            updateAmortFlag(cs, tnloanid);
        }

        public void insertAmort(string cs, DateTime tdDueDate, double tnRePayment, double tnPrinPay, double tnInterestPay, double tnBegBal, double tnEndBal, double tnCumuInt,
            int tnLoanid, int tnOrder, double tnLoanBalance)
        {
            using (SqlConnection nConnHandle2 = new SqlConnection(cs))
            {
                //string cglquery = " insert into amortabl(due_date,npayment,nprinpmnt,nintpmnt,nbegbal,nendbal,ncumint,loanid,norder,nbalance,amort_date)";
                //cglquery += " values ";
                //cglquery += " (@ldue_date,@lnpayment,@lnprinpmnt,@lnintpmnt,@lnbegbal,@lnendbal,@lncumint,@lloanid,@lnorder,@lnbalance,convert(date,getdate()))";

                //     MessageBox.Show("inside insertamort duedate,repayment,prinpmt,intpmt " + tdDueDate + "," + tnRePayment + "," + tnPrinPay + "," + tnInterestPay);
                string cglquery = " insert into amortabl(due_date,npayment,nprinpmnt,nintpmnt)";
                cglquery += " values ";
                cglquery += " (@ldue_date,@lnpayment,@lnprinpmnt,@lnintpmnt)";

                SqlDataAdapter insCommand = new SqlDataAdapter();
                insCommand.InsertCommand = new SqlCommand(cglquery, nConnHandle2);

                nConnHandle2.Open();
                insCommand.InsertCommand.Parameters.Add("@ldue_date", SqlDbType.DateTime).Value = tdDueDate;// drow["duedate"];
                insCommand.InsertCommand.Parameters.Add("@lnpayment", SqlDbType.Decimal).Value = tnRePayment;// Convert.ToDecimal(drow["npayment"]);
                insCommand.InsertCommand.Parameters.Add("@lnprinpmnt", SqlDbType.Decimal).Value = tnPrinPay;// Convert.ToDecimal(drow["nprinpay"]);
                insCommand.InsertCommand.Parameters.Add("@lnintpmnt", SqlDbType.Decimal).Value = tnInterestPay;// Convert.ToDecimal(drow["nintpay"]);
                                                                                                               //insCommand.InsertCommand.Parameters.Add("@lnbegbal", SqlDbType.Decimal).Value = tnBegBal;// Convert.ToDecimal(drow["begbal"]);
                                                                                                               //insCommand.InsertCommand.Parameters.Add("@lnendbal", SqlDbType.Decimal).Value = tnEndBal;// Convert.ToDecimal(drow["namount"]);
                                                                                                               //insCommand.InsertCommand.Parameters.Add("@lncumint", SqlDbType.Decimal).Value = tnCumuInt;// Convert.ToDecimal(drow["cumInt"]);
                                                                                                               //insCommand.InsertCommand.Parameters.Add("@lloanid", SqlDbType.Int).Value = tnLoanid;// gnLoanID; ;
                                                                                                               //insCommand.InsertCommand.Parameters.Add("@lnorder", SqlDbType.Int).Value = tnOrder;// drow["dperiod"];
                                                                                                               //insCommand.InsertCommand.Parameters.Add("@lnbalance", SqlDbType.Decimal).Value = tnRePayment;// Convert.ToDecimal(drow["npayment"]);
                insCommand.InsertCommand.ExecuteNonQuery();
                insCommand.InsertCommand.Parameters.Clear();
                nConnHandle2.Close();
            }
        }

        public void updateAmortFlag(string cs, int tnLoanid)
        {
            using (SqlConnection nConnHandle2 = new SqlConnection(cs))
            {
                string amortquery = " update loan_det set lamort=1 where loan_id =@lloanid";

                SqlDataAdapter updCommand = new SqlDataAdapter();
                updCommand.UpdateCommand = new SqlCommand(amortquery, nConnHandle2);

                updCommand.UpdateCommand.Parameters.Add("@lloanid", SqlDbType.Int).Value = tnLoanid;
                nConnHandle2.Open();
                updCommand.UpdateCommand.ExecuteNonQuery();
                nConnHandle2.Close();
            }
        }

    }


    public class codedPassword
    {
        public static string encryptPassword(string cs, string dpassword)
        {
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
            {
                int i = 0;
                int j = 0;
                int l = 0;
                string ncrptd = "";
                string t = "";
                int k = dpassword.Length;
                string xpswd = dpassword.ToUpper();
                string ypswd = xpswd.Substring(k - 1, 1);

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

    public class getNewAcct
    {
        public static string newAcctNumber(string cs, string tcMemberCode, string acctype)
        {
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
            {
                string sreq = "select cacctnumb from glmast where ccustcode=@tcCustCode and acode = @tcAcode order by cacctnumb desc";
                SqlDataAdapter accountview = new SqlDataAdapter();
                accountview.SelectCommand = new SqlCommand(sreq, ndConnHandle3);
                accountview.SelectCommand.Parameters.Add("@tcCustCode ", SqlDbType.VarChar).Value = tcMemberCode;
                accountview.SelectCommand.Parameters.Add("@tcAcode", SqlDbType.VarChar).Value = acctype;

                ndConnHandle3.Open();
                accountview.SelectCommand.ExecuteNonQuery();
                ndConnHandle3.Close();
                DataTable actView = new DataTable();
                accountview.Fill(actView);
                if (actView != null && actView.Rows.Count > 0)
                {
                    int newSubref = Convert.ToInt16(actView.Rows[0]["cacctnumb"].ToString().Substring(9, 2)) + 1;
                    string newAcct = actView.Rows[0]["cacctnumb"].ToString().Trim().Substring(0, 9) + newSubref.ToString().Trim().PadLeft(2, '0');
                    return newAcct;
                }
                else
                {
                    //                    return "";
                    string newAcct = acctype + tcMemberCode + "01";
                    return newAcct;
                }
            }
        }
    }

    public class updateJournal
    {
        public void updJournal(string cs, string actnumb, string desc, decimal tamt, string tcjvno, string trancode, DateTime dtransDate, string tcUserid, int tncompid)
        {
            using (SqlConnection nConnHandle2 = new SqlConnection(cs))
            {
                int jourtype = (trancode == "01" ? 2 :  //Deposits
                (trancode == "02" ? 3 : //Withdrawals
                (trancode == "07" ? 4 : //Loan Repayment
                (trancode == "09" ? 5 : //"<< Loan Payout/close >>" :
                (trancode == "22" ? 6 : 99)))));// "<< Loan Charge Off >>" : "")))));
                string cglquery = "Insert Into journal (cvoucherno,cuserid,dtrandate,ctrandesc,cacctnumb,dpostdate,cstack,ntranamnt,jvno,bank,cchqno,jcstack,compid,ctrancode,jour_TYPE)";
                cglquery += " VALUES  (@llcVoucherNo,@luserid,@lgdtrans_date,@lgcDesc,@lcActNumb,convert(date,getdate()),@llcStack,@llnTranAmt,@llcjvno,@llcBank,@lgcChqno,@llcStack,@lgnCompID,@lTrancode,@jtype)";
                nConnHandle2.Open();
                SqlDataAdapter insCommand = new SqlDataAdapter();
                insCommand.InsertCommand = new SqlCommand(cglquery, nConnHandle2);
                insCommand.InsertCommand.Parameters.Add("@llcVoucherNo", SqlDbType.VarChar).Value = genbill.genvoucher(cs, dtransDate);
                insCommand.InsertCommand.Parameters.Add("@luserid", SqlDbType.Char).Value = tcUserid;
                insCommand.InsertCommand.Parameters.Add("@lgdtrans_date", SqlDbType.DateTime).Value = dtransDate;
                insCommand.InsertCommand.Parameters.Add("@lgcDesc", SqlDbType.VarChar).Value = desc;
                insCommand.InsertCommand.Parameters.Add("@lcActNumb", SqlDbType.VarChar).Value = actnumb;
                insCommand.InsertCommand.Parameters.Add("@llcStack", SqlDbType.VarChar).Value = genStack.getstack(cs);
                insCommand.InsertCommand.Parameters.Add("@llnTranAmt", SqlDbType.Decimal).Value = tamt;
                insCommand.InsertCommand.Parameters.Add("@llcjvno", SqlDbType.VarChar).Value = tcjvno;
                insCommand.InsertCommand.Parameters.Add("@llcBank", SqlDbType.Int).Value = 1;
                insCommand.InsertCommand.Parameters.Add("@lgcChqno", SqlDbType.VarChar).Value = "000001";
                insCommand.InsertCommand.Parameters.Add("@lgnCompID", SqlDbType.Int).Value = tncompid;
                insCommand.InsertCommand.Parameters.Add("@lTrancode", SqlDbType.Char).Value = trancode;
                insCommand.InsertCommand.Parameters.Add("@jtype", SqlDbType.Int).Value = jourtype;

                insCommand.InsertCommand.ExecuteNonQuery();
                insCommand.InsertCommand.Parameters.Clear();
                nConnHandle2.Close();
            }
        }
    }
    public class phoneNumberFormat
    {
        public static string phoneformat(int telnumb)
        {
            //string phone;
            //string area;
            //string major;
            //string minor;
            //string intl_firstsegment;
            //string intl_secondsegment;
            //string intl_thirdsegment;
            //string intl_fourthsegment;
            //string intl_fifthsegment;

            //switch (telnumb)
            //{
            //    case 7:            //number of digits is 7 such as The Gambia
            //        break;
            //}
            //    if (e.Value.ToString().Length == US_PHONE_FORMAT_LEN)
            //{
            //    phone = e.Value.ToString();
            //    area = phone.Substring(0, 3);
            //    major = phone.Substring(3, 3);
            //    minor = phone.Substring(6);
            //    e.Value = string.Format("{0}-{1}-{2}", area, major, minor);
            //}
            //else if ((e.Value.ToString().Length == UK_PHONE_LEN) && (e.Value.ToString()[0] == '+'))
            //{
            //    phone = e.Value.ToString();
            //    intl_firstsegment = phone.Substring(0, 2);
            //    intl_secondsegment = phone.Substring(2, 3);
            //    intl_thirdsegment = phone.Substring(5);
            //    e.Value = string.Format("+{0}-{1}-{2}", intl_firstsegment, intl_secondsegment, intl_thirdsegment);
            //}
            //else if ((e.Value.ToString().Length == COMMON_INTERNATIONAL_PHONE_LEN) && (e.Value.ToString()[0] == '+'))
            //{
            //    phone = e.Value.ToString();
            //    intl_firstsegment = phone.Substring(0, 2);
            //    intl_secondsegment = phone.Substring(2, 2);
            //    intl_thirdsegment = phone.Substring(4, 3);
            //    intl_fourthsegment = phone.Substring(7, 2);
            //    intl_fifthsegment = phone.Substring(9);
            //    e.Value = string.Format("+{0}-{1}-{2}-{3}-{4}", intl_firstsegment, intl_secondsegment, intl_thirdsegment, intl_fourthsegment, intl_fifthsegment);
            //}
            return " ";
        }
    }

    public class ServiceRequestNumber
    {
        public int myadd(int a, int b)
        {
            return a + b;
        }

        public int mysub(int a, int b)
        {
            return a - b;
        }

        public string reqnumb(string cs, DateTime dsysdate)
        {
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
            {
                string sreq = "select req_numb from client_code";
                string crequest = "";
                int rcounter;
                SqlDataAdapter requestview = new SqlDataAdapter(sreq, ndConnHandle3);
                ndConnHandle3.Open();
                DataTable reqView = new DataTable();
                requestview.Fill(reqView);
                if (reqView != null)
                {
                    rcounter = Convert.ToInt32(reqView.Rows[0]["req_numb"]);
                    if (Convert.ToUInt32(reqView.Rows[0]["req_numb"]) >= 9999)
                    {
                        crequest = dsysdate.Year.ToString().Substring(2, 2) + dsysdate.Month.ToString().PadLeft(2, '0') + dsysdate.Day.ToString().PadLeft(2, '0') + "0001";
                        return crequest;
                    }
                    else
                    {
                        crequest = dsysdate.Year.ToString().Substring(2, 2) + dsysdate.Month.ToString().PadLeft(2, '0') + dsysdate.Day.ToString().PadLeft(2, '0') + rcounter.ToString().PadLeft(4, '0');
                        return crequest;
                    }
                }
                else { return ""; }
            }
        }

    }
    public class genreceipt
    {
        public static string getreceipt(string cs, DateTime dsysdate)
        {
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
            {
                string susp = "select rec_no from client_code";
                string creceipt = "";
                int vcounter;
                SqlDataAdapter da2 = new SqlDataAdapter(susp, ndConnHandle3);
                ndConnHandle3.Open();
                DataTable ds = new DataTable();
                da2.Fill(ds);
                if (ds != null)
                {
                    vcounter = Convert.ToInt32(ds.Rows[0]["rec_no"]);
                    if (Convert.ToUInt32(ds.Rows[0]["rec_no"]) >= 9999)
                    {
                        creceipt = dsysdate.Year.ToString().Substring(2, 2) + dsysdate.Month.ToString().PadLeft(2, '0') + dsysdate.Day.ToString().PadLeft(2, '0') + "0001";
                        return creceipt;
                    }
                    else
                    {
                        creceipt = dsysdate.Year.ToString().Substring(2, 2) + dsysdate.Month.ToString().PadLeft(2, '0') + dsysdate.Day.ToString().PadLeft(2, '0') + vcounter.ToString().PadLeft(4, '0');
                        return creceipt;
                    }
                }
                else { return ""; }
            }
        }
    }

    public class taxCalculation
    {
        public static decimal calcTax(string countrycode, decimal tnSalary)
        {
            decimal nTaxAmt = 0.00m;
            decimal nPerReli = 0.00m;
            switch (countrycode)
            {
                case "220":                      //Tax calculation for The Gambia
                    tnSalary = 12 * tnSalary;
                    //gnIncome=Iif(tnSalary<=11180.33,0.00,;
                    //    IIF(tnSalary<=17500,(tnSalary-7500)*0.10/12,;
                    //    IIF(tnSalary<=27500,((tnSalary-17500)*0.15+1000)/12,;
                    //    IIF(tnSalary<=37500,((tnSalary-27500)*0.20+2500)/12,;
                    //    IIF(tnSalary<=47500,((tnSalary-37500)*0.25+4500)/12,;
                    //    (tnSalary-47500)*0.35/12+583.33)))))
                    //Return gnIncome  

                    nTaxAmt = (tnSalary <= 17500.00m ? (tnSalary - 7500.00m) * 0.10m / 12 :
                    (tnSalary <= 27500.00m ? ((tnSalary - 17500.00m) * 0.15m + 1000.00m) / 12 :
                    (tnSalary <= 37500.00m ? ((tnSalary - 27500.00m) * 0.20m + 2500.00m) / 12 :
                    (tnSalary <= 47500.00m ? ((tnSalary - 37500.00m) * 0.25m + 4500.00m) / 12 :
                    ((tnSalary - 47500) * 0.35m / 12 + 583.33m)))));
                    break;
                case "254":                      //Tax calculation for Kenya
                                                 /*
                                                  Function KTAXCAL
                             Parameters tnSalary
                             gnIncome=Iif(tnSalary<=11180,(tnSalary)*0.10,Iif(tnSalary<=21713,(tnSalary-11180)*0.15+1118,Iif(tnSalary<=32246,((tnSalary-21713)*0.20+2698),;
                                 IIF(tnSalary<=42779,(tnSalary-32246)*0.25+4805,(tnSalary-42779)*0.30+7503))))
                             gnIncome=Iif(gnIncome-gnPerReli>0.00,gnIncome-gnPerReli,0.00)
                             Return gnIncome
                                                  */
                    nTaxAmt = (tnSalary <= 11180.00m ? (tnSalary) * 0.10m :
                        (tnSalary <= 21713.00m ? (tnSalary - 11180.00m) * 0.15m + 1118m :
                        (tnSalary <= 32246.00m ? ((tnSalary - 21713.00m) * 0.20m + 2698.00m) :
                        (tnSalary <= 42779.00m ? (tnSalary - 32246.00m) * 0.25m + 4805.00m :
                        (tnSalary - 42779.00m) * 0.30m + 7503.00m))));
                    nTaxAmt = nTaxAmt > nPerReli ? nTaxAmt - nPerReli : 0.00m;
                    break;
            }

            return nTaxAmt;
        }
    }

    public class countdaysinaMonth
    {
        public static int numberofDaysinMonth(int nyear, int nmonth, DayOfWeek dDayofWeek)
        {
            DateTime startDate = new DateTime(nyear, nmonth, 1);
            int days = DateTime.DaysInMonth(startDate.Year, startDate.Month);
            int weekDayCount = 0;
            for (int day = 0; day < days; ++day)
            {
                weekDayCount += startDate.AddDays(day).DayOfWeek == dDayofWeek ? 1 : 0;
            }
            return weekDayCount;
        }
    }


    public class checkDuplicateClients
    {
        public static bool getduplicate(string cs, string tcClientCode)
        {
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
            {
                string susp1 = "select ccustcode from cusreg";
                SqlDataAdapter da21 = new SqlDataAdapter(susp1, ndConnHandle3);
                ndConnHandle3.Open();
                DataTable ds1 = new DataTable();
                da21.Fill(ds1);
                if (ds1 != null && ds1.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }

    public class getProductControl
    {
        //        private string  getloancontrol(int lprid, out string dacct)
        public static string productControl(string cs, int tnprid)
        {
            string dasql = "select  prod_control,prd_name from  prodtype where prd_id = " + tnprid;
            string dacct = string.Empty;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter daloan = new SqlDataAdapter(dasql, ndConnHandle);
                DataTable dprodview = new DataTable();
                daloan.Fill(dprodview);
                if (dprodview.Rows.Count > 0)
                {
                    dacct = dprodview.Rows[0]["prod_control"].ToString();
                    return dacct;
                }
                else
                {
                    MessageBox.Show("Product Control not defined, inform IT Dept immediately");
                    dacct = string.Empty;
                    return dacct;
                }
            }
        }

        public static string interestControl(string cs, int tnprid)
        {
            string dasql = "select  int_inc,prd_name from  prodtype where prd_id = " + tnprid;
            string dacct = string.Empty;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter daloan = new SqlDataAdapter(dasql, ndConnHandle);
                DataTable dprodview = new DataTable();
                daloan.Fill(dprodview);
                if (dprodview.Rows.Count > 0)
                {

                    dacct = dprodview.Rows[0]["int_inc"].ToString();
                    return dacct;
                }
                else
                {
                    MessageBox.Show("Product Control not defined, inform IT Dept immediately");
                    dacct = string.Empty;
                    return dacct;
                }
            }
        }
        public static string ninterestControl(string cs, int tnprid)
        {
            string dasql = "select  nint_inc,prd_name from  prodtype where prd_id = " + tnprid;
            string dacct = string.Empty;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter daloan = new SqlDataAdapter(dasql, ndConnHandle);
                DataTable dprodview = new DataTable();
                daloan.Fill(dprodview);
                if (dprodview.Rows.Count > 0)
                {
                    dacct = dprodview.Rows[0]["nint_inc"].ToString();
                    return dacct;
                }
                else
                {
                    MessageBox.Show("Product Control not defined, inform IT Dept immediately");
                    dacct = string.Empty;
                    return dacct;
                }
            }
        }

        public static string expenseControl(string cs, int tnprid)
        {
            string dasql = "select  exp_acc,prd_name from  prodtype where prd_id = " + tnprid;
            string dacct = string.Empty;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter daloan = new SqlDataAdapter(dasql, ndConnHandle);
                DataTable dprodview = new DataTable();
                daloan.Fill(dprodview);
                if (dprodview.Rows.Count > 0)
                {
                    dacct = dprodview.Rows[0]["exp_acc"].ToString();
                    return dacct;
                }
                else
                {
                    MessageBox.Show("Product Control not defined, inform IT Dept immediately");
                    dacct = string.Empty;
                    return dacct;
                }
            }
        }

        public static string payableControl(string cs, int tnprid)
        {
            string dasql = "select  acc_pay,prd_name from  prodtype where prd_id = " + tnprid;
            string dacct = string.Empty;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter daloan = new SqlDataAdapter(dasql, ndConnHandle);
                DataTable dprodview = new DataTable();
                daloan.Fill(dprodview);
                if (dprodview.Rows.Count > 0)
                {
                    dacct = dprodview.Rows[0]["acc_pay"].ToString();
                    return dacct;
                }
                else
                {
                    MessageBox.Show("Product Control not defined, inform IT Dept immediately");
                    dacct = string.Empty;
                    return dacct;
                }
            }
        }

        public static string receivableControl(string cs, int tnprid)
        {
            string dasql = "select  acc_rec,prd_name from  prodtype where prd_id = " + tnprid;
            string dacct = string.Empty;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter daloan = new SqlDataAdapter(dasql, ndConnHandle);
                DataTable dprodview = new DataTable();
                daloan.Fill(dprodview);
                if (dprodview.Rows.Count > 0)
                {
                    dacct = dprodview.Rows[0]["acc_rec"].ToString();
                    return dacct;
                }
                else
                {
                    MessageBox.Show("Product Control not defined, inform IT Dept immediately");
                    dacct = string.Empty;
                    return dacct;
                }
            }
        }


        public static string badincControl(string cs, int tnprid)
        {
            string dasql = "select bad_deb_inc,prd_name from  prodtype where prd_id = " + tnprid;
            string dacct = string.Empty;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter daloan = new SqlDataAdapter(dasql, ndConnHandle);
                DataTable dprodview = new DataTable();
                daloan.Fill(dprodview);
                if (dprodview.Rows.Count > 0)
                {
                    dacct = dprodview.Rows[0]["bad_deb_inc"].ToString();
                    return dacct;
                }
                else
                {
                    MessageBox.Show("Product Control not defined, inform IT Dept immediately");
                    dacct = string.Empty;
                    return dacct;
                }
            }
        }


        public static string badexpControl(string cs, int tnprid)
        {
            string dasql = "select  bad_deb_exp,prd_name from  prodtype where prd_id = " + tnprid;
            string dacct = string.Empty;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter daloan = new SqlDataAdapter(dasql, ndConnHandle);
                DataTable dprodview = new DataTable();
                daloan.Fill(dprodview);
                if (dprodview.Rows.Count > 0)
                {
                    dacct = dprodview.Rows[0]["bad_deb_exp"].ToString();
                    return dacct;
                }
                else
                {
                    MessageBox.Show("Product Control not defined, inform IT Dept immediately");
                    dacct = string.Empty;
                    return dacct;
                }
            }
        }
    }

    public class loanCalculation
    {
        public static double oldpmt(double annRate, int nper, int nyrpay, double pv)  //This will return the periodic payment
        {
            double rate = (double)annRate / 100 / nyrpay;           //(double)annRate / 100 / 12;
            double denom = Math.Pow((1 + rate), nper) - 1;
            double pmtn = (rate + (rate / denom)) * pv;
            double paymt = rate / (Math.Pow(1 + rate, nper) - 1) * -(pv * Math.Pow(1 + rate, nper) + 0.00);
            return paymt;
        }

        public static double pmt(double r, int nper, double pv, double fv, int type)
        {
            double pmt = r / (Math.Pow(1 + r, nper) - 1) * -(pv * Math.Pow(1 + r, nper) + fv);
            return pmt;
        }

        public static double intrest(double annRate, int nper, int nyrpay, double pv)    //This will return the total interest
        {
            double rate = (double)annRate / 100 / nyrpay;
            double denom = Math.Pow((1 + rate), nper) - 1;
            double totInt = pv * (nper * (rate + (rate / denom)) - 1);
            return totInt;
        }

        public static double fv(double r, int nper, double c, double pv, int type)  //This will return the future payment
        {
            double fv = -(c * (Math.Pow(1 + r, nper) - 1) / r + pv * Math.Pow(1 + r, nper));
            return fv;
        }

        public static double ipmt(double r, int per, int nper, double pv, double fv, int type) //This will return the interest part of the periodic payment
        {
            double ipmt = loanCalculation.fv(r, per - 1, loanCalculation.pmt(r, nper, pv, fv, type), pv, type) * r;
            if (type == 1) ipmt /= (1 + r);
            return ipmt;
        }

        public static double ppmt(double r, int per, int nper, double pv, double fv, int type) //this will return the principal part of the periodic payment
        {
            double ppmt = loanCalculation.pmt(r, nper, pv, 0.00, 0) - loanCalculation.ipmt(r, per, nper, pv, 0.00, 0);
            return ppmt;
        }
    }

    /*   public class loanpmt
       {
           public static double pmt(double annRate, int nper, int nyrpay, double pv)  //This will return the periodic payment
           {
               double rate = (double)annRate / 100 / nyrpay;            
               double denom = Math.Pow((1 + rate), nper) - 1;
               double pmt = (rate + (rate / denom)) * pv;
               double paymt = rate / (Math.Pow(1 + rate, nper) - 1) * -(pv * Math.Pow(1 + rate, nper) + 0.00);
               MessageBox.Show("old pay = " + pmt + " and new pay =" + paymt);
               return pmt;                                         

           }
         }

       public class loanfuture
       {
           public static double fv(double annRate, int nper, int nyrpay, double c, double pv, int type)
           {
               double rate = (double)annRate / 100 / nyrpay;
               double fv = -(c * (Math.Pow(1 + rate, nper) - 1) / rate + pv * Math.Pow(1 + rate, nper));
               return fv;
           }
       }

       public class Ipaymt
       { public static double ipmt(double annRate, int per, int nper, int nyrpay, double pv, double fv, int type)
           {
               double rate = (double)annRate / 100 / nyrpay;

               double ipmt = loanfuture.fv(r, per - 1, pmt(r, nper, pv, fv, type), pv, type) * r;
        //       double ipmt = -(c * (Math.Pow(1 + rate, nper) - 1) / rate + pv * Math.Pow(1 + rate, nper))
               if (type == 1) ipmt /= (1 + r);

               return ipmt;
           }
       }
       */
    public class genStack
    {
        public static string getstack(string cs)
        {
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
            {
                string susp = "select stackcount from client_code";
                string creceipt = "";
                SqlDataAdapter da2 = new SqlDataAdapter(susp, ndConnHandle3);
                ndConnHandle3.Open();
                DataTable ds = new DataTable();
                da2.Fill(ds);
                if (ds != null)
                {
                    creceipt = ds.Rows[0]["stackcount"].ToString().PadLeft(12, '0');
                    return creceipt;
                }
                else { return ""; }
            }
        }
    }



    public class GetClient_Code
    {
        public static int clientCode_int(string cs, string fieldname)
        {
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
            {
                string susp = "select " + fieldname + " from client_code";
                int retValue = 0;
                SqlDataAdapter da2 = new SqlDataAdapter(susp, ndConnHandle3);
                ndConnHandle3.Open();
                DataTable ds = new DataTable();
                da2.Fill(ds);
                if (ds != null)
                {
                    retValue = Convert.ToInt32(ds.Rows[0][0]);
                    return retValue;
                }
                else { return 0; }
            }
        }


        public static string clientCode_str(string cs, string fieldname)
        {
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
            {
                string susp = "select " + fieldname + " from client_code";
                string retValue = "";
                SqlDataAdapter da2 = new SqlDataAdapter(susp, ndConnHandle3);
                ndConnHandle3.Open();
                DataTable ds = new DataTable();
                da2.Fill(ds);
                if (ds != null)
                {
                    retValue = ds.Rows[0][0].ToString(); //.ToString().PadLeft(12, '0');
                    return retValue;
                }
                else { return ""; }
            }
        }
    }

    public class updateClient_Code
    {
        public void updClient(string cs, string fieldname)
        {
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
            {
                string strpara = fieldname + "=" + fieldname + "+ 1";
                ndConnHandle3.Open();
                string cupdatequry1 = "update client_code set " + strpara;
                SqlDataAdapter updCommand1 = new SqlDataAdapter();
                updCommand1.UpdateCommand = new SqlCommand(cupdatequry1, ndConnHandle3);
                updCommand1.UpdateCommand.ExecuteNonQuery();
                ndConnHandle3.Close();
            }
        }
    }

    public class resetClient_Code
    {
        public void setClient(string cs, string fieldname)
        {
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
            {
                string strpara = fieldname + "= 1";
                ndConnHandle3.Open();
                string cupdatequry1 = "update client_code set " + strpara;
                SqlDataAdapter updCommand1 = new SqlDataAdapter();
                updCommand1.UpdateCommand = new SqlCommand(cupdatequry1, ndConnHandle3);
                updCommand1.UpdateCommand.ExecuteNonQuery();
                ndConnHandle3.Close();
            }
        }
    }

    /*



Function GENRFQ					&&This function generates Request for quote numbers
Parameters tcwhen
sn=SQLExec(gnConnHandle,"select rfq_no from patient_code","updateVouView")
If !(sn>0)
	=sysmsg("something is not right")
Else
	If rfq_no>=9999
		fn=SQLExec(gnConnHandle,"update patient_code set rfq_no=1","updcounter")
		gcVoucher='RF'+Right(Alltrim(Str(Year(gdSysDate))),2)+;
			PADL(Alltrim(Str(Month(gdSysDate))),2,'0')+;
			PADL(Alltrim(Str(Day(gdSysDate))),2,'0')+'0001'
	Else
		gcVoucher='RF'+Right(Alltrim(Str(Year(gdSysDate))),2)+;
			PADL(Alltrim(Str(Month(gdSysDate))),2,'0')+;
			PADL(Alltrim(Str(Day(gdSysDate))),2,'0')+;
			PADL(Alltrim(Str(rfq_no)),4,'0')
		If tcwhen='2'
			sn=SQLExec(gnConnHandle,"update patient_code set rfq_no=rfq_no+1","tupdateview")
			If !(sn>0)
				=Messagebox("something is wrong","Information Centre")
			Endif
		Endif
	Endif
Endif
Return gcVoucher

Function GENPO					&&This function generates PURCHASE ORDER NUMBERS
Parameters tcwhen
sn=SQLExec(gnConnHandle,"select po_no from patient_code","updateVouView")
If !(sn>0)
	=sysmsg("something is not right")
Else
	If po_no>=9999
		fn=SQLExec(gnConnHandle,"update patient_code set po_no=1","updcounter")
		gcVoucher='PO'+Right(Alltrim(Str(Year(gdSysDate))),2)+;
			PADL(Alltrim(Str(Month(gdSysDate))),2,'0')+;
			PADL(Alltrim(Str(Day(gdSysDate))),2,'0')+'0001'
	Else
		gcVoucher='PO'+Right(Alltrim(Str(Year(gdSysDate))),2)+;
			PADL(Alltrim(Str(Month(gdSysDate))),2,'0')+;
			PADL(Alltrim(Str(Day(gdSysDate))),2,'0')+;
			PADL(Alltrim(Str(po_no)),4,'0')
		If tcwhen='2'
			sn=SQLExec(gnConnHandle,"update patient_code set po_no=po_no+1","tupdateview")
			If !(sn>0)
				=Messagebox("something is wrong","Information Centre")
			Endif
		Endif
	Endif
Endif
Return gcVoucher

Function gendn					&&This function generates Delivery note numbers
Parameters tcwhen
sn=SQLExec(gnConnHandle,"select dl_no from patient_code","updateVouView")
If !(sn>0)
	=sysmsg("something is not right")
Else
	If dl_no>=9999
		fn=SQLExec(gnConnHandle,"update patient_code set dl_no=1","updcounter")
		gcVoucher='DL'+Right(Alltrim(Str(Year(gdSysDate))),2)+;
			PADL(Alltrim(Str(Month(gdSysDate))),2,'0')+;
			PADL(Alltrim(Str(Day(gdSysDate))),2,'0')+'0001'
	Else
		gcVoucher='DL'+Right(Alltrim(Str(Year(gdSysDate))),2)+;
			PADL(Alltrim(Str(Month(gdSysDate))),2,'0')+;
			PADL(Alltrim(Str(Day(gdSysDate))),2,'0')+;
			PADL(Alltrim(Str(dl_no)),4,'0')
		If tcwhen='2'
			sn=SQLExec(gnConnHandle,"update patient_code set dl_no=dl_no+1","tupdateview")
			If !(sn>0)
				=Messagebox("something is wrong","Information Centre")
			Endif
		Endif
	Endif
Endif
Return gcVoucher

Function genpay					&&This function generates payment request numbers
Parameters tcwhen
sn=SQLExec(gnConnHandle,"select payr_no from patient_code","updateVouView")
If !(sn>0)
	=sysmsg("something is not right")
Else
	If payr_no>=9999
		fn=SQLExec(gnConnHandle,"update patient_code set payr_no=1","updcounter")
		gcVoucher='PY'+Right(Alltrim(Str(Year(gdSysDate))),2)+;
			PADL(Alltrim(Str(Month(gdSysDate))),2,'0')+;
			PADL(Alltrim(Str(Day(gdSysDate))),2,'0')+'0001'
	Else
		gcVoucher='PY'+Right(Alltrim(Str(Year(gdSysDate))),2)+;
			PADL(Alltrim(Str(Month(gdSysDate))),2,'0')+;
			PADL(Alltrim(Str(Day(gdSysDate))),2,'0')+;
			PADL(Alltrim(Str(payr_no)),4,'0')
		If tcwhen='2'
			sn=SQLExec(gnConnHandle,"update patient_code set payr_no=payr_no+1","tupdateview")
			If !(sn>0)
				=Messagebox("something is wrong","Information Centre")
			Endif
		Endif
	Endif
Endif
Return gcVoucher

Function genpayV					&&This function generates payment voucher numbers
Parameters tcwhen
sn=SQLExec(gnConnHandle,"select payv_no from patient_code","updateVView")
If !(sn>0)
	=sysmsg("something is not right")
Else
	If payv_no>=9999
		fn=SQLExec(gnConnHandle,"update patient_code set payv_no=1","updcounter")
		gcVoucher='PV'+Right(Alltrim(Str(Year(gdSysDate))),2)+;
			PADL(Alltrim(Str(Month(gdSysDate))),2,'0')+;
			PADL(Alltrim(Str(Day(gdSysDate))),2,'0')+'0001'
	Else
		gcVoucher='PV'+Right(Alltrim(Str(Year(gdSysDate))),2)+;
			PADL(Alltrim(Str(Month(gdSysDate))),2,'0')+;
			PADL(Alltrim(Str(Day(gdSysDate))),2,'0')+;
			PADL(Alltrim(Str(payv_no)),4,'0')
		If tcwhen='2'
			sn=SQLExec(gnConnHandle,"update patient_code set payv_no=payv_no+1","tupdateview")
			If !(sn>0)
				=sysmsg("something is wrong, Inform IT DEPT")
			Endif
		Endif
	Endif
Endif
Return gcVoucher
         */



    public class AuditTrail
    {

        //			tcUpdateType C = 'creation', A = 'amendment', D = 'Deletion', R = 'Read/View'
        //          tcAuditDesc     Description of the audit such as 'client registration'
        //			tnOldAmt Old value before amendment
        //          tnNewAmt        New value after amendment
        //			tcAuditScope Area of audit such as Client administration, Laboratory, In patient, Payroll etc use gnModuleID
        //			tcOldLabel Old label/description before amendment
        //          tcNewLabel      New label/description after amendment+
        //          tcAuditRemarks  Any extra remarks
        //          tcCpID          Mac address of the computer used for the process
        //			gcWkStation Domain computer
        //          gcWinUser       Domain user*

        public void upd_audit_trail(string cs, string tcAuditDesc, decimal tnOldAmt, decimal tnNewAmt, string cuserid, string updatetype, string tcOldLabel, string tcNewLabel, string tcAuditRemarks, string tcCpID, string gcWkStation, string gcWinUser)
        {
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
            {
                string cpatsql = "insert into audit_trail (audit_desc,orig_value,new_value,suserid,audit_date,audit_time,audit_type,orig_cvalue,new_cvalue,cremarks,cpid,wkstat,winusr)";
                cpatsql += " values (@tcAuditDesc,@tnOldAmt,@tnNewAmt,@gcUserid,convert(date,getdate()),convert(time,getdate()),@tcUpdateType,@tcOldLabel,@tcNewLabel,@tcAuditRemarks,@tcCpID,@gcWkStation,@gcWinUser)";
                SqlDataAdapter glinsCommand = new SqlDataAdapter();
                glinsCommand.InsertCommand = new SqlCommand(cpatsql, ndConnHandle3);
                glinsCommand.InsertCommand.Parameters.Add("@tcAuditDesc", SqlDbType.Char).Value = tcAuditDesc;
                glinsCommand.InsertCommand.Parameters.Add("@tnOldAmt", SqlDbType.Decimal).Value = tnOldAmt;
                glinsCommand.InsertCommand.Parameters.Add("@tnNewAmt", SqlDbType.Decimal).Value = tnNewAmt;
                glinsCommand.InsertCommand.Parameters.Add("@gcUserid", SqlDbType.Char).Value = cuserid;
                glinsCommand.InsertCommand.Parameters.Add("@tcUpdateType", SqlDbType.Char).Value = updatetype;

                glinsCommand.InsertCommand.Parameters.Add("@tcOldLabel", SqlDbType.Char).Value = tcOldLabel;
                glinsCommand.InsertCommand.Parameters.Add("@tcNewLabel", SqlDbType.Char).Value = tcNewLabel;
                glinsCommand.InsertCommand.Parameters.Add("@tcAuditRemarks", SqlDbType.Char).Value = tcAuditRemarks;
                glinsCommand.InsertCommand.Parameters.Add("@tcCpID", SqlDbType.Char).Value = tcCpID;
                glinsCommand.InsertCommand.Parameters.Add("@gcWkStation", SqlDbType.Char).Value = gcWkStation;
                glinsCommand.InsertCommand.Parameters.Add("@gcWinUser", SqlDbType.Char).Value = gcWinUser;

                ndConnHandle3.Open();
                glinsCommand.InsertCommand.ExecuteNonQuery();
                ndConnHandle3.Close();
            }
        }
    }


    public class CuVoucher
    {
        public static string genCuVoucher(string cs, DateTime dsysdate)
        {
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
            {
                string susp = "select nvoucherno from client_code";
                string cvoucher = "";
                int vcounter;
                SqlDataAdapter da2 = new SqlDataAdapter(susp, ndConnHandle3);
                ndConnHandle3.Open();
                DataTable ds = new DataTable();
                da2.Fill(ds);
                if (ds != null)
                {
                    vcounter = Convert.ToInt32(ds.Rows[0]["nvoucherno"]);
                    if (Convert.ToUInt32(ds.Rows[0]["nvoucherno"]) >= 9999)
                    {
                        cvoucher = dsysdate.Year.ToString().Substring(2, 2) + dsysdate.Month.ToString().PadLeft(2, '0') + dsysdate.Day.ToString().PadLeft(2, '0') + "0001";
                        return cvoucher;
                    }
                    else
                    {
                        cvoucher = dsysdate.Year.ToString().Substring(2, 2) + dsysdate.Month.ToString().PadLeft(2, '0') + dsysdate.Day.ToString().PadLeft(2, '0') + vcounter.ToString().PadLeft(4, '0');
                        return cvoucher;
                    }
                }
                else { return ""; }
            }
        }//end of genCuVoucher

        public static string genJv(string cs, DateTime dsysdate)
        {
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
            {
                string susp = "select jv_no from client_code";
                string cvoucher = "";
                int vcounter;
                SqlDataAdapter da2 = new SqlDataAdapter(susp, ndConnHandle3);
                ndConnHandle3.Open();
                DataTable ds = new DataTable();
                da2.Fill(ds);
                if (ds != null)
                {
                    vcounter = Convert.ToInt32(ds.Rows[0]["jv_no"]);
                    if (Convert.ToUInt32(ds.Rows[0]["jv_no"]) >= 9999)
                    {
                        cvoucher = dsysdate.Year.ToString().Substring(2, 2) + dsysdate.Month.ToString().PadLeft(2, '0') + dsysdate.Day.ToString().PadLeft(2, '0') + "0001";
                        return cvoucher;
                    }
                    else
                    {
                        cvoucher = dsysdate.Year.ToString().Substring(2, 2) + dsysdate.Month.ToString().PadLeft(2, '0') + dsysdate.Day.ToString().PadLeft(2, '0') + vcounter.ToString().PadLeft(4, '0');
                        return cvoucher;
                    }
                }
                else { return ""; }
            }
        }//end of genCuVoucher

    }// end of cuVoucher


    public class ProdServAccounts
    {
        public static string ServiceAccounts(string cs, string tcSrv_code)          //       Procedure ServiceAccounts, Parameters tcSrv_code
        {
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
            {
                string susp = "select inc_acc,exp_acc,acc_pay,acc_rec from servces where srv_code = " + tcSrv_code;
                SqlDataAdapter da2 = new SqlDataAdapter(susp, ndConnHandle3);
                ndConnHandle3.Open();
                DataTable ds = new DataTable();
                da2.Fill(ds);
                if (ds != null)
                {
                    // fn = SQLExec(gnConnHandle, "select inc_acc,exp_acc,acc_pay,acc_rec from servces where srv_code = ?tcSrv_code", "srView")
                    // If fn> 0 And Reccount() > 0
                    string gcIncAcct = ds.Rows[0]["inc_acc"].ToString();
                    string gcExpAcct = ds.Rows[0]["exp_acc"].ToString();
                    string gcAccPay = ds.Rows[0]["acc_pay"].ToString();
                    string gcAccRec = ds.Rows[0]["acc_rec"].ToString();
                    string accts = gcIncAcct + " I " + gcExpAcct + " E " + gcAccPay + " P " + gcAccRec + " R ";
                    return accts;
                }
                else { return ""; }
            }
        }
        public static string ProductAccounts(string cs, string tcProd_code)          //       Procedure ProductAccounts, Parameters tcProd_code
        {
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
            {
                string susp = "select inc_acc,cog_acc,acc_pay,acc_rec,inv_acc from products where prod_code = " + "'" + tcProd_code + "'";
                SqlDataAdapter da2 = new SqlDataAdapter(susp, ndConnHandle3);
                ndConnHandle3.Open();
                DataTable ds = new DataTable();
                da2.Fill(ds);
                if (ds != null)
                {
                    string gcIncAcct = ds.Rows[0]["inc_acc"].ToString();
                    string gcExpAcct = ds.Rows[0]["cog_acc"].ToString();
                    string gcAccPay = ds.Rows[0]["acc_pay"].ToString();
                    string gcAccRec = ds.Rows[0]["acc_rec"].ToString();
                    string gcInvAcc = ds.Rows[0]["inv_acc"].ToString();
                    string accts = gcIncAcct + " I " + gcExpAcct + " E " + gcAccPay + " P " + gcAccRec + " R " + gcIncAcct + " V ";
                    return accts;
                }
                else { MessageBox.Show("we could not find accounts for this product"); return ""; }
            }
        }
    }

    public class CheckLastBalance
    {
        public static decimal lastbalance(string cs, string tcAcctNumb)
        {
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
            {
                decimal lnLastBalance = 0.00m;
                SqlDataReader cUserDetails = null;
                SqlCommand cGetUser = new SqlCommand("select nbookbal from glmast where cacctnumb=@tcAcctNumb", ndConnHandle3);
                cGetUser.Parameters.Add("@tcAcctNumb", SqlDbType.VarChar).Value = tcAcctNumb;
                ndConnHandle3.Open();
                cUserDetails = cGetUser.ExecuteReader();
                cUserDetails.Read();
                if (cUserDetails.HasRows == true)
                {
                    lnLastBalance = cUserDetails.GetDecimal(0);     //  Convert.ToDecimal(cUserDetails. .Rows[0]["nbookbal"]);
                    return lnLastBalance;
                }
                else
                {
                    return lnLastBalance;
                }
            }
        }
    }

    public class deletetables
    {
        public void zaptempfiles(string cs, string dtable, string dclient)
        {
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
            {
                ndConnHandle3.Open();
                string cdisquery1 = "delete from " + dtable + " where ccustcode=@cCustCode";
                SqlDataAdapter delvis = new SqlDataAdapter();
                delvis.DeleteCommand = new SqlCommand(cdisquery1, ndConnHandle3);
                delvis.DeleteCommand.Parameters.Add("@cCustCode", SqlDbType.Char).Value = dclient;
                delvis.DeleteCommand.ExecuteNonQuery();
                ndConnHandle3.Close();
            }
        }
    }
    public class getVisitNumber
    {
        public static int visitno(string cs, int ncompid, string tcCustCode)
        {
            int nvisno = 0;
            string dsql = "exec tsp_GetVisit_new  " + ncompid + ",'" + tcCustCode + "'";
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                DataTable vistable = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(vistable);
                if (vistable.Rows.Count > 0)
                {
                    nvisno = Convert.ToInt16(vistable.Rows[0]["visno"]);
                    return nvisno;
                }
                else
                {
                    return nvisno;
                }
            }
        }
    }


    public class daltext
    {
        public static string inwords(string mnumber)
        {
            string mstring = mnumber.Trim().PadLeft(12, ' ');
            string cmillion = mstring.Substring(0, 3);
            string cthousand = mstring.Substring(3, 3);
            string chundred = mstring.Substring(6, 3);
            string mwords = "";                                             //&& Variable for output text
            string nbututs = "";

            if (mstring.Substring(0, 3).Trim().Length > 0)
            {
                mwords = getdigits.threedigit(mstring.Substring(0, 3)) + " Million ";
            }

            if (mstring.Substring(3, 3).Trim().Length > 0)
            {
                mwords = mwords + getdigits.threedigit(mstring.Substring(3, 3)) + " Thousand ";
            }

            string step3 = mstring.Substring(6, 3).Trim();

            if (mstring.Substring(6, 3).Trim().Length > 0)
            {
                mwords = mwords + getdigits.threedigit(mstring.Substring(6, 3)) + "  ";
            }
            if (Convert.ToDecimal(mstring.Substring(mstring.Length - 2, 2).Trim().ToString()) > 0)    //.Length>0)
            {
                nbututs = getdigits.threedigit('0' + mstring.Substring(mstring.Length - 2, 2).Trim());
            }
            else
            {
                nbututs = "ZERO";
            }


            if (mwords == "")
            {
                mwords = "ZERO";
            }

            string mrest = (nbututs.ToUpper().ToString() == "ZERO" ? " only " : " And " + nbututs + " UNIT " + " Only.");
            mwords = mwords.Trim() + " MAIN " + mrest.Trim();
            return mwords;
        }
    }

    public class getdigits
    {
        public static string threedigit(string m3digit)
        {
            string mthree = "";
            string m3first = m3digit.Substring(0, 1);
            string[] mones = new string[19];
            string[] mtens = new string[8];
            int mnum = 0;
            mones[0] = "One";
            mones[1] = "Two";
            mones[2] = "Three";
            mones[3] = "Four";
            mones[4] = "Five";
            mones[5] = "Six";
            mones[6] = "Seven";
            mones[7] = "Eight";
            mones[8] = "Nine";
            mones[9] = "Ten";
            mones[10] = "Eleven";
            mones[11] = "Twelve";
            mones[12] = "Thirteen";
            mones[13] = "Fourteen";
            mones[14] = "Fifteen";
            mones[15] = "Sixteen";
            mones[16] = "Seventeen";
            mones[17] = "Eighteen";
            mones[18] = "Nineteen";

            mtens[0] = "Twenty";
            mtens[1] = "Thirty";
            mtens[2] = "Forty";
            mtens[3] = "Fifty";
            mtens[4] = "Sixty";
            mtens[5] = "Seventy";
            mtens[6] = "Eighty";
            mtens[7] = "Ninety";

            if (m3digit.Substring(0, 1) != " ")                 //first digit is non empty
            {
                mnum = Convert.ToInt32(m3digit.Substring(0, 1));
                if (mnum > 0)
                {
                    mthree = mones[mnum - 1] + " Hundred ";
                }
            }


            //            if (m3digit.Substring(1, 2) != " ")

            if (Convert.ToInt16(m3digit.Substring(1, 2)) > 0)
            {
                mnum = Convert.ToInt16(m3digit.Substring(1, 2));

                if (mnum > 0 && mnum <= 19)
                {
                    mthree = mthree + mones[mnum - 1];
                }

                if (mnum > 19)
                {
                    int dnum = (int)(mnum / 10) - 2;
                    mthree = mthree + mtens[dnum];
                    mnum = Convert.ToInt16(m3digit.Substring(2, 1));
                    if (mnum > 0)
                    {
                        mthree = mthree + "-" + mones[mnum - 1];
                    }
                }
            }
            else
            {
            }

            return mthree;
        }
    }

    public class updateGlmast
    {
        public updateGlmast()
        {
        }

        public void updGlmast(string cs, string tcAcctNumb, decimal nTranAmt)
        {
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
            {
                //**********update glmast account
                string cpatquery = "update glmast set nbookbal = nbookbal + @tnTranAmt, dlsttrn = convert(date,getdate()) where cacctnumb = @tcAcctNumb";

                SqlDataAdapter glinsCommand = new SqlDataAdapter();

                glinsCommand.UpdateCommand = new SqlCommand(cpatquery, ndConnHandle3);
                glinsCommand.UpdateCommand.Parameters.Add("@tnTranAmt", SqlDbType.Decimal).Value = nTranAmt;
                glinsCommand.UpdateCommand.Parameters.Add("@tcAcctNumb", SqlDbType.Char).Value = tcAcctNumb;

                ndConnHandle3.Open();
                glinsCommand.UpdateCommand.ExecuteNonQuery();
                ndConnHandle3.Close();
                updMinBal(cs, tcAcctNumb, nTranAmt);
            }
        }

        public void updMinBal(string cs, string tcAcct, decimal namt)
        {
            using (SqlConnection dconnhand = new SqlConnection(cs))
            {

                using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
                {
                    //**********update the minimum balance of glmast account
                    string cpatquery = "update glmast set nminbal = (case when nbookbal+@namt > nminbal then nminbal else nbookbal + @namt end) where glmast.cacctnumb = @acct";

                    SqlDataAdapter glinsCommand = new SqlDataAdapter();

                    glinsCommand.UpdateCommand = new SqlCommand(cpatquery, ndConnHandle3);
                    glinsCommand.UpdateCommand.Parameters.Add("@namt", SqlDbType.Decimal).Value = namt;
                    glinsCommand.UpdateCommand.Parameters.Add("@acct", SqlDbType.Char).Value = tcAcct;

                    ndConnHandle3.Open();
                    glinsCommand.UpdateCommand.ExecuteNonQuery();
                    ndConnHandle3.Close();
                }
            }

        }
    }//end of updateGlmast


    public class updateDailyBalance
    {
        public void updDayBal(string cs, DateTime dTrandate, string dacct, decimal ntranAmt, int branid, int compid)
        {
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
            {
                decimal lnUpdAmtPos = (ntranAmt > 0.00m ? ntranAmt : 0.00m);
                decimal lnUpdAmtNeg = (ntranAmt < 0.00m ? ntranAmt : 0.00m);
                decimal lnUpdAmt = (lnUpdAmtPos > 0.00m ? lnUpdAmtPos : lnUpdAmtNeg);
                string sqlqueryint = "select 1 from glmast where intcode=1 and cacctnumb = " + "'" + dacct + "'"; //This process is for internal accounts only
                SqlDataAdapter dabint = new SqlDataAdapter(sqlqueryint, ndConnHandle3);
                DataTable intdayview = new DataTable();
                dabint.Fill(intdayview);
                if (intdayview != null && intdayview.Rows.Count > 0)            //&&This is an internal account 
                {
                    //************Getting account in daybal

                    string sqlquery1 = "select 1 from daybal where cacctnumb = " + "'" + dacct + "'" + " and convert(date,baldate) = '" + dTrandate + "'";// @dtrandate order by baldate desc "; //Check to see if accounts exists for today
                    SqlDataAdapter dab = new SqlDataAdapter(sqlquery1, ndConnHandle3);
                    DataTable dayview = new DataTable();
                    dab.Fill(dayview);
                    if (dayview != null && dayview.Rows.Count > 0)            //&&There is a record for the account for today, we update debit, credit and closing balances
                    {
                        string cupdquery = "exec tsp_UpdateDailyBalance @cAcctNumb,@nUpdAmtPos,@nUpdAmtNeg,@nUpdAmt,@baldate";
                        SqlDataAdapter glupdCommand = new SqlDataAdapter();
                        glupdCommand.UpdateCommand = new SqlCommand(cupdquery, ndConnHandle3);
                        glupdCommand.UpdateCommand.Parameters.Add("@baldate", SqlDbType.DateTime).Value = dTrandate;
                        glupdCommand.UpdateCommand.Parameters.Add("@cAcctNumb", SqlDbType.Char).Value = dacct;
                        glupdCommand.UpdateCommand.Parameters.Add("@nUpdAmtPos", SqlDbType.Decimal).Value = lnUpdAmtPos;
                        glupdCommand.UpdateCommand.Parameters.Add("@nUpdAmtNeg", SqlDbType.Decimal).Value = lnUpdAmtNeg;
                        glupdCommand.UpdateCommand.Parameters.Add("@nUpdAmt", SqlDbType.Decimal).Value = lnUpdAmt;
                        ndConnHandle3.Open();
                        glupdCommand.UpdateCommand.ExecuteNonQuery();
                        ndConnHandle3.Close();
                    }
                    else                                                       //There is no record for this patient for today
                    {
                        string sqlquerybal = "select baldate,nclosbal from daybal where cacctnumb = " + "'" + dacct + "' and convert(date,baldate)<='"+dTrandate+"' order by baldate desc";  //checking to see if there are previouis records for client

                        SqlDataAdapter dab1 = new SqlDataAdapter(sqlquerybal, ndConnHandle3);
                        DataTable preview = new DataTable();
                        dab1.Fill(preview);
                        if (preview != null && preview.Rows.Count > 0)            //&&There are previous records for this account, we need to pick the last closing balance and insert new record
                        {
                            decimal lnClosBal = Convert.ToDecimal(preview.Rows[0]["nclosbal"]);
                            string cpatquery = "exec tsp_InsertDailyBalance @baldate, @cAcctNumb, @nopenbal,@ndebit,@ncredit,@branchid,@ncompid";
                            SqlDataAdapter glinsCommand = new SqlDataAdapter();
                            glinsCommand.InsertCommand = new SqlCommand(cpatquery, ndConnHandle3);
                            glinsCommand.InsertCommand.Parameters.Add("@baldate", SqlDbType.DateTime).Value = dTrandate;
                            glinsCommand.InsertCommand.Parameters.Add("@cAcctNumb", SqlDbType.Char).Value = dacct;
                            glinsCommand.InsertCommand.Parameters.Add("@nopenbal", SqlDbType.Decimal).Value = lnClosBal;
                            glinsCommand.InsertCommand.Parameters.Add("@ndebit", SqlDbType.Decimal).Value = lnUpdAmtNeg;
                            glinsCommand.InsertCommand.Parameters.Add("@ncredit", SqlDbType.Decimal).Value = lnUpdAmtPos;
                            glinsCommand.InsertCommand.Parameters.Add("@branchid", SqlDbType.Int).Value = branid;
                            glinsCommand.InsertCommand.Parameters.Add("@ncompid", SqlDbType.Int).Value = compid;

                            ndConnHandle3.Open();
                            glinsCommand.InsertCommand.ExecuteNonQuery();
                            ndConnHandle3.Close();
                        }
                        else                                                    //There is no previous record for this account, we just insert  a new record
                        {
//                            MessageBox.Show("There is no previous record for this account, we just insert  a new record");
                            string cpatquery12 = "exec tsp_InsertDailyBalance @baldate, @cAcctNumb, @nopenbal,@ndebit,@ncredit,@branchid,@ncompid";
                            SqlDataAdapter glinsCommand = new SqlDataAdapter();
                            glinsCommand.InsertCommand = new SqlCommand(cpatquery12, ndConnHandle3);
                            glinsCommand.InsertCommand.Parameters.Add("@baldate", SqlDbType.DateTime).Value = dTrandate;
                            glinsCommand.InsertCommand.Parameters.Add("@cAcctNumb", SqlDbType.Char).Value = dacct;
                            glinsCommand.InsertCommand.Parameters.Add("@nopenbal", SqlDbType.Decimal).Value = 0.00m;
                            glinsCommand.InsertCommand.Parameters.Add("@ndebit", SqlDbType.Decimal).Value = lnUpdAmtNeg;
                            glinsCommand.InsertCommand.Parameters.Add("@ncredit", SqlDbType.Decimal).Value = lnUpdAmtPos;
                            glinsCommand.InsertCommand.Parameters.Add("@branchid", SqlDbType.Int).Value = branid;
                            glinsCommand.InsertCommand.Parameters.Add("@ncompid", SqlDbType.Int).Value = compid;

                            ndConnHandle3.Open();
                            glinsCommand.InsertCommand.ExecuteNonQuery();
                            ndConnHandle3.Close();
                        }
                    }
                }else { MessageBox.Show("Account does not exist or is not an internal account"); }
            }
        }
    }



    public class updateTranhist
    {
        public updateTranhist()
        {
            //       MessageBox.Show("we are in the public");
        }


        public void updTranhist(string cs, string tcAcctNumb, decimal tnTranAmt, string tcDesc, string tcVoucher, string tcChqno, string tcUserID, decimal tnNewBal, string tcTranCode, int lnServID,
           bool lPaid, string tcContra, decimal lnWaiveAmt, int tnqty, decimal unitprice, string tcReceipt, bool llCashpay, int visno, bool isproduct, int srvid, string tcSrv_code, string tcProd_code,
           bool lFreeBee, string lcCustCode, int tnCompid)
        {
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
            {
                ndConnHandle3.Open();
                SqlDataReader cUser1Details = null;
                string lcStack = "";
                SqlCommand cGetUser = new SqlCommand("select stackcount from client_code", ndConnHandle3);
                SqlCommand updatestack = new SqlCommand("update client_code set stackcount = stackcount + 1", ndConnHandle3);
                cUser1Details = cGetUser.ExecuteReader();
                cUser1Details.Read();
                if (cUser1Details.HasRows == true)
                {
                    lcStack = cUser1Details.GetInt32(0).ToString().PadLeft(12, '0');
                }
                else
                {
                    MessageBox.Show("Patient counter file empty, CRITICAL ERROR!!, inform IT DEPT immediately");
                }
                cUser1Details.Close();


                SqlDataAdapter trinsCommand1 = new SqlDataAdapter();
                SqlDataAdapter trinsCommand2 = new SqlDataAdapter();


                string cpatquery = "Insert Into tranhist (cacctnumb,ntranamnt,ctrandesc,dpostdate,dtrandate,dvaluedate,cvoucherno,receiptno,cchqno,cuserid,nnewbal,cstack,ctrancode,compid,sverified,";
                cpatquery += "serv_id,lpaid,ccontra,nwaiveamt,ctrantime,quantity,unit_price,lcashpay,visno,ccustcode,isproduct,srv_id,srv_code,prod_code,lfreebee) ";
                cpatquery += "VALUES (@tcAcctNumb,@tntranamt,@tcDesc,CONVERT(date,GETDATE()),CONVERT (date, GETDATE()),CONVERT (Date, GETDATE()),@tcVoucher,@tcReceipt,@tcChqno,@tcUserID,@tnNewBal,@lcStack,@tcTranCode,@tnCompid,@lVerified,";
                cpatquery += "@lnServID,@llPaid,@tcContra,@lnWaiveAmt,CONVERT (Time, GETDATE()),@tnqty,@unitprice,@lCashpay,@visno,@lcCustcode,@isproduct,@srvid,@tcSrv_code,@tcProd_Code,@tlFreeBee)";

                string cpatquery1 = "Insert Into todayhist (cacctnumb,ntranamnt,ctrandesc,dpostdate,dtrandate,dvaluedate,cvoucherno,receiptno,cchqno,cuserid,nnewbal,cstack,ctrancode,compid,sverified,";
                cpatquery1 += "serv_id,lpaid,ccontra,nwaiveamt,ctrantime,quantity,unit_price,lcashpay,visno,ccustcode,isproduct,srv_id,srv_code,prod_code,lfreebee) ";
                cpatquery1 += "VALUES (@tcAcctNumb,@tntranamt,@tcDesc,CONVERT(date,GETDATE()),CONVERT (date, GETDATE()),CONVERT (Date, GETDATE()),@tcVoucher,@tcReceipt,@tcChqno,@tcUserID,@tnNewBal,@lcStack,@tcTranCode,@tnCompid,@lVerified,";
                cpatquery1 += "@lnServID,@llPaid,@tcContra,@lnWaiveAmt,CONVERT (Time, GETDATE()),@tnqty,@unitprice,@lCashpay,@visno,@lcCustcode,@isproduct,@srvid,@tcSrv_code,@tcProd_Code,@tlFreeBee)";


                trinsCommand1.InsertCommand = new SqlCommand(cpatquery, ndConnHandle3);
                trinsCommand1.InsertCommand.Parameters.Add("@tcAcctNumb", SqlDbType.Char).Value = tcAcctNumb;
                trinsCommand1.InsertCommand.Parameters.Add("@tnTranAmt", SqlDbType.Decimal).Value = tnTranAmt;
                trinsCommand1.InsertCommand.Parameters.Add("@tcDesc", SqlDbType.Char).Value = tcDesc;
                trinsCommand1.InsertCommand.Parameters.Add("@tcVoucher", SqlDbType.Char).Value = tcVoucher;
                trinsCommand1.InsertCommand.Parameters.Add("@tcReceipt", SqlDbType.Char).Value = tcReceipt;
                trinsCommand1.InsertCommand.Parameters.Add("@tcChqno", SqlDbType.Char).Value = tcChqno;
                trinsCommand1.InsertCommand.Parameters.Add("@tcUserid", SqlDbType.Char).Value = tcUserID;
                trinsCommand1.InsertCommand.Parameters.Add("@tnNewBal", SqlDbType.Decimal).Value = tnNewBal;
                trinsCommand1.InsertCommand.Parameters.Add("@lcStack", SqlDbType.Char).Value = lcStack;
                trinsCommand1.InsertCommand.Parameters.Add("@tcTranCode", SqlDbType.Char).Value = tcTranCode;
                trinsCommand1.InsertCommand.Parameters.Add("@tcContra", SqlDbType.Char).Value = tcContra;
                trinsCommand1.InsertCommand.Parameters.Add("@tnCompid", SqlDbType.Int).Value = tnCompid;
                trinsCommand1.InsertCommand.Parameters.Add("@lnServID", SqlDbType.Char).Value = lnServID;
                trinsCommand1.InsertCommand.Parameters.Add("@llPaid", SqlDbType.Bit).Value = lPaid;
                trinsCommand1.InsertCommand.Parameters.Add("@lnWaiveAmt", SqlDbType.Decimal).Value = lnWaiveAmt;
                trinsCommand1.InsertCommand.Parameters.Add("@tnqty", SqlDbType.Int).Value = tnqty;
                trinsCommand1.InsertCommand.Parameters.Add("@unitprice", SqlDbType.Decimal).Value = unitprice;
                trinsCommand1.InsertCommand.Parameters.Add("@lCashpay", SqlDbType.Bit).Value = llCashpay;
                trinsCommand1.InsertCommand.Parameters.Add("@visno", SqlDbType.Int).Value = visno;
                trinsCommand1.InsertCommand.Parameters.Add("@lcCustcode", SqlDbType.Char).Value = lcCustCode;
                trinsCommand1.InsertCommand.Parameters.Add("@isproduct", SqlDbType.Bit).Value = isproduct;
                trinsCommand1.InsertCommand.Parameters.Add("@srvid", SqlDbType.Int).Value = srvid;
                trinsCommand1.InsertCommand.Parameters.Add("@tcSrv_code", SqlDbType.Char).Value = tcSrv_code;
                trinsCommand1.InsertCommand.Parameters.Add("@tcProd_code", SqlDbType.Char).Value = tcProd_code;
                trinsCommand1.InsertCommand.Parameters.Add("@tlFreeBee", SqlDbType.Bit).Value = false;
                trinsCommand1.InsertCommand.Parameters.Add("@lVerified", SqlDbType.Bit).Value = true;

                trinsCommand2.InsertCommand = new SqlCommand(cpatquery1, ndConnHandle3);
                trinsCommand2.InsertCommand.Parameters.Add("@tcAcctNumb", SqlDbType.Char).Value = tcAcctNumb;
                trinsCommand2.InsertCommand.Parameters.Add("@tnTranAmt", SqlDbType.Decimal).Value = tnTranAmt;
                trinsCommand2.InsertCommand.Parameters.Add("@tcDesc", SqlDbType.Char).Value = tcDesc;
                trinsCommand2.InsertCommand.Parameters.Add("@tcVoucher", SqlDbType.Char).Value = tcVoucher;
                trinsCommand2.InsertCommand.Parameters.Add("@tcReceipt", SqlDbType.Char).Value = tcReceipt;
                trinsCommand2.InsertCommand.Parameters.Add("@tcChqno", SqlDbType.Char).Value = tcChqno;
                trinsCommand2.InsertCommand.Parameters.Add("@tcUserid", SqlDbType.Char).Value = tcUserID;
                trinsCommand2.InsertCommand.Parameters.Add("@tnNewBal", SqlDbType.Decimal).Value = tnNewBal;
                trinsCommand2.InsertCommand.Parameters.Add("@lcStack", SqlDbType.Char).Value = lcStack;
                trinsCommand2.InsertCommand.Parameters.Add("@tcTranCode", SqlDbType.Char).Value = tcTranCode;
                trinsCommand2.InsertCommand.Parameters.Add("@tcContra", SqlDbType.Char).Value = tcContra;
                trinsCommand2.InsertCommand.Parameters.Add("@tnCompid", SqlDbType.Int).Value = tnCompid;
                trinsCommand2.InsertCommand.Parameters.Add("@lnServID", SqlDbType.Char).Value = lnServID;
                trinsCommand2.InsertCommand.Parameters.Add("@llPaid", SqlDbType.Bit).Value = lPaid;
                trinsCommand2.InsertCommand.Parameters.Add("@lnWaiveAmt", SqlDbType.Decimal).Value = lnWaiveAmt;
                trinsCommand2.InsertCommand.Parameters.Add("@tnqty", SqlDbType.Int).Value = tnqty;
                trinsCommand2.InsertCommand.Parameters.Add("@unitprice", SqlDbType.Decimal).Value = unitprice;
                trinsCommand2.InsertCommand.Parameters.Add("@lCashpay", SqlDbType.Bit).Value = llCashpay;
                trinsCommand2.InsertCommand.Parameters.Add("@visno", SqlDbType.Int).Value = visno;
                trinsCommand2.InsertCommand.Parameters.Add("@lcCustcode", SqlDbType.Char).Value = lcCustCode;
                trinsCommand2.InsertCommand.Parameters.Add("@isproduct", SqlDbType.Bit).Value = isproduct;
                trinsCommand2.InsertCommand.Parameters.Add("@srvid", SqlDbType.Int).Value = srvid;
                trinsCommand2.InsertCommand.Parameters.Add("@tcSrv_code", SqlDbType.Char).Value = tcSrv_code;
                trinsCommand2.InsertCommand.Parameters.Add("@tcProd_code", SqlDbType.Char).Value = tcProd_code;
                trinsCommand2.InsertCommand.Parameters.Add("@tlFreeBee", SqlDbType.Bit).Value = false;
                trinsCommand2.InsertCommand.Parameters.Add("@lVerified", SqlDbType.Bit).Value = true;


                trinsCommand1.InsertCommand.ExecuteNonQuery();
                trinsCommand2.InsertCommand.ExecuteNonQuery();
                updatestack.ExecuteNonQuery();
                ndConnHandle3.Close();
            }
        }
    }

    public class updateCuTranhist
    {
        public updateCuTranhist()
        {
            //       MessageBox.Show("we are in the public");
        }

        //Update History File
        public void updCuTranhist(string cs, string tcAcctNumb, decimal tnTranAmt, string tcDesc, string tcVoucher, string tcChqno, string tcUserID, decimal tnNewBal, string tcTranCode, int lnServID,
           bool lPaid, string tcContra, decimal lnWaiveAmt, int tnqty, decimal unitprice, string tcReceipt, bool llCashpay, int visno, bool isproduct, int srvid, string tcSrv_code, string tcProd_code,
           bool lFreeBee, string lcCustCode, int tnCompid, int branchid, int currcode, DateTime dtTranDate, DateTime dtValueDate)
        {
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
            {
                ndConnHandle3.Open();
                SqlDataReader cUser1Details = null;
                string lcStack = "";

                lcStack = GetClient_Code.clientCode_int(cs, "stackcount").ToString().Trim().PadLeft(15, '0');

                SqlDataAdapter trinsCommand1 = new SqlDataAdapter();
                SqlDataAdapter trinsCommand2 = new SqlDataAdapter();

                string cpatquery = "Insert Into tranhist (cacctnumb,ntranamnt,ctrandesc,dpostdate,dtrandate,dvaluedate,cvoucherno,receiptno,cchqno,cuserid,nnewbal,cstack,ctrancode,compid,sverified,";
                cpatquery += "serv_id,lpaid,ccontra,nwaiveamt,ctrantime,quantity,unit_price,lcashpay,visno,ccustcode,isproduct,srv_id,srv_code,prod_code,lfreebee,branchid,ccurrcode) ";
                cpatquery += "VALUES (@tcAcctNumb,@tntranamt,@tcDesc,CONVERT(date,GETDATE()),@dtrandate,@dvaludate,@tcVoucher,@tcReceipt,@tcChqno,@tcUserID,@tnNewBal,@lcStack,@tcTranCode,@tnCompid,@lVerified,";
                cpatquery += "@lnServID,@llPaid,@tcContra,@lnWaiveAmt,CONVERT (Time, GETDATE()),@tnqty,@unitprice,@lCashpay,@visno,@lcCustcode,@isproduct,@srvid,@tcSrv_code,@tcProd_Code,@tlFreeBee,@lbranchid,@lcurrcode)";

                string cpatquery1 = "Insert Into todayhist (cacctnumb,ntranamnt,ctrandesc,dpostdate,dtrandate,dvaluedate,cvoucherno,receiptno,cchqno,cuserid,nnewbal,cstack,ctrancode,compid,sverified,";
                cpatquery1 += "serv_id,lpaid,ccontra,nwaiveamt,ctrantime,quantity,unit_price,lcashpay,visno,ccustcode,isproduct,srv_id,srv_code,prod_code,lfreebee,branchid,ccurrcode) ";
                cpatquery1 += "VALUES (@tcAcctNumb,@tntranamt,@tcDesc,CONVERT(date,GETDATE()),@dtrandate,@dvaludate,@tcVoucher,@tcReceipt,@tcChqno,@tcUserID,@tnNewBal,@lcStack,@tcTranCode,@tnCompid,@lVerified,";
                cpatquery1 += "@lnServID,@llPaid,@tcContra,@lnWaiveAmt,CONVERT (Time, GETDATE()),@tnqty,@unitprice,@lCashpay,@visno,@lcCustcode,@isproduct,@srvid,@tcSrv_code,@tcProd_Code,@tlFreeBee,@lbranchid,@lcurrcode)";


                trinsCommand1.InsertCommand = new SqlCommand(cpatquery, ndConnHandle3);
                trinsCommand1.InsertCommand.Parameters.Add("@tcAcctNumb", SqlDbType.Char).Value = tcAcctNumb;
                trinsCommand1.InsertCommand.Parameters.Add("@tnTranAmt", SqlDbType.Decimal).Value = tnTranAmt;
                trinsCommand1.InsertCommand.Parameters.Add("@tcDesc", SqlDbType.Char).Value = tcDesc;
                trinsCommand1.InsertCommand.Parameters.Add("@tcVoucher", SqlDbType.Char).Value = tcVoucher;
                trinsCommand1.InsertCommand.Parameters.Add("@tcReceipt", SqlDbType.Char).Value = tcReceipt;
                trinsCommand1.InsertCommand.Parameters.Add("@tcChqno", SqlDbType.Char).Value = tcChqno;
                trinsCommand1.InsertCommand.Parameters.Add("@tcUserid", SqlDbType.Char).Value = tcUserID;
                trinsCommand1.InsertCommand.Parameters.Add("@tnNewBal", SqlDbType.Decimal).Value = tnNewBal;
                trinsCommand1.InsertCommand.Parameters.Add("@lcStack", SqlDbType.Char).Value = lcStack;
                trinsCommand1.InsertCommand.Parameters.Add("@tcTranCode", SqlDbType.Char).Value = tcTranCode;
                trinsCommand1.InsertCommand.Parameters.Add("@tcContra", SqlDbType.Char).Value = tcContra;
                trinsCommand1.InsertCommand.Parameters.Add("@tnCompid", SqlDbType.Int).Value = tnCompid;
                trinsCommand1.InsertCommand.Parameters.Add("@lnServID", SqlDbType.Char).Value = lnServID;
                trinsCommand1.InsertCommand.Parameters.Add("@llPaid", SqlDbType.Bit).Value = lPaid;
                trinsCommand1.InsertCommand.Parameters.Add("@lnWaiveAmt", SqlDbType.Decimal).Value = lnWaiveAmt;
                trinsCommand1.InsertCommand.Parameters.Add("@tnqty", SqlDbType.Int).Value = tnqty;
                trinsCommand1.InsertCommand.Parameters.Add("@unitprice", SqlDbType.Decimal).Value = unitprice;
                trinsCommand1.InsertCommand.Parameters.Add("@lCashpay", SqlDbType.Bit).Value = llCashpay;
                trinsCommand1.InsertCommand.Parameters.Add("@visno", SqlDbType.Int).Value = visno;
                trinsCommand1.InsertCommand.Parameters.Add("@lcCustcode", SqlDbType.Char).Value = lcCustCode;
                trinsCommand1.InsertCommand.Parameters.Add("@isproduct", SqlDbType.Bit).Value = isproduct;
                trinsCommand1.InsertCommand.Parameters.Add("@srvid", SqlDbType.Int).Value = srvid;
                trinsCommand1.InsertCommand.Parameters.Add("@tcSrv_code", SqlDbType.Char).Value = tcSrv_code;
                trinsCommand1.InsertCommand.Parameters.Add("@tcProd_code", SqlDbType.Char).Value = tcProd_code;
                trinsCommand1.InsertCommand.Parameters.Add("@tlFreeBee", SqlDbType.Bit).Value = false;
                trinsCommand1.InsertCommand.Parameters.Add("@lVerified", SqlDbType.Bit).Value = true;
                trinsCommand1.InsertCommand.Parameters.Add("@lbranchid", SqlDbType.Int).Value = branchid;
                trinsCommand1.InsertCommand.Parameters.Add("@lcurrcode", SqlDbType.Int).Value = currcode;
                trinsCommand1.InsertCommand.Parameters.Add("@dtrandate", SqlDbType.DateTime).Value = dtTranDate;
                trinsCommand1.InsertCommand.Parameters.Add("@dvaludate", SqlDbType.DateTime).Value = dtValueDate;

                trinsCommand2.InsertCommand = new SqlCommand(cpatquery1, ndConnHandle3);
                trinsCommand2.InsertCommand.Parameters.Add("@tcAcctNumb", SqlDbType.Char).Value = tcAcctNumb;
                trinsCommand2.InsertCommand.Parameters.Add("@tnTranAmt", SqlDbType.Decimal).Value = tnTranAmt;
                trinsCommand2.InsertCommand.Parameters.Add("@tcDesc", SqlDbType.Char).Value = tcDesc;
                trinsCommand2.InsertCommand.Parameters.Add("@tcVoucher", SqlDbType.Char).Value = tcVoucher;
                trinsCommand2.InsertCommand.Parameters.Add("@tcReceipt", SqlDbType.Char).Value = tcReceipt;
                trinsCommand2.InsertCommand.Parameters.Add("@tcChqno", SqlDbType.Char).Value = tcChqno;
                trinsCommand2.InsertCommand.Parameters.Add("@tcUserid", SqlDbType.Char).Value = tcUserID;
                trinsCommand2.InsertCommand.Parameters.Add("@tnNewBal", SqlDbType.Decimal).Value = tnNewBal;
                trinsCommand2.InsertCommand.Parameters.Add("@lcStack", SqlDbType.Char).Value = lcStack;
                trinsCommand2.InsertCommand.Parameters.Add("@tcTranCode", SqlDbType.Char).Value = tcTranCode;
                trinsCommand2.InsertCommand.Parameters.Add("@tcContra", SqlDbType.Char).Value = tcContra;
                trinsCommand2.InsertCommand.Parameters.Add("@tnCompid", SqlDbType.Int).Value = tnCompid;
                trinsCommand2.InsertCommand.Parameters.Add("@lnServID", SqlDbType.Char).Value = lnServID;
                trinsCommand2.InsertCommand.Parameters.Add("@llPaid", SqlDbType.Bit).Value = lPaid;
                trinsCommand2.InsertCommand.Parameters.Add("@lnWaiveAmt", SqlDbType.Decimal).Value = lnWaiveAmt;
                trinsCommand2.InsertCommand.Parameters.Add("@tnqty", SqlDbType.Int).Value = tnqty;
                trinsCommand2.InsertCommand.Parameters.Add("@unitprice", SqlDbType.Decimal).Value = unitprice;
                trinsCommand2.InsertCommand.Parameters.Add("@lCashpay", SqlDbType.Bit).Value = llCashpay;
                trinsCommand2.InsertCommand.Parameters.Add("@visno", SqlDbType.Int).Value = visno;
                trinsCommand2.InsertCommand.Parameters.Add("@lcCustcode", SqlDbType.Char).Value = lcCustCode;
                trinsCommand2.InsertCommand.Parameters.Add("@isproduct", SqlDbType.Bit).Value = isproduct;
                trinsCommand2.InsertCommand.Parameters.Add("@srvid", SqlDbType.Int).Value = srvid;
                trinsCommand2.InsertCommand.Parameters.Add("@tcSrv_code", SqlDbType.Char).Value = tcSrv_code;
                trinsCommand2.InsertCommand.Parameters.Add("@tcProd_code", SqlDbType.Char).Value = tcProd_code;
                trinsCommand2.InsertCommand.Parameters.Add("@tlFreeBee", SqlDbType.Bit).Value = false;
                trinsCommand2.InsertCommand.Parameters.Add("@lVerified", SqlDbType.Bit).Value = true;
                trinsCommand2.InsertCommand.Parameters.Add("@lbranchid", SqlDbType.Int).Value = branchid;
                trinsCommand2.InsertCommand.Parameters.Add("@lcurrcode", SqlDbType.Int).Value = currcode;
                trinsCommand2.InsertCommand.Parameters.Add("@dtrandate", SqlDbType.DateTime).Value = dtTranDate;
                trinsCommand2.InsertCommand.Parameters.Add("@dvaludate", SqlDbType.DateTime).Value = dtValueDate;

                trinsCommand1.InsertCommand.ExecuteNonQuery();
                trinsCommand2.InsertCommand.ExecuteNonQuery();
                updateClient_Code updc = new updateClient_Code();
                updc.updClient(cs, "stackcount");
                ndConnHandle3.Close();
            }
        }
    }

    public class updateDashBoard
    {
        //                public updateDashBoard()
        //          {
        //    }

        public void updDashBoard()
        {

            MessageBox.Show("we will update the dashboard");
            /*
         Function UpdDashBoard			&&This updates the executive dashboard
         Parameters tnDnYear,tnDmonth,tcDuptype,tnDPatients,tnDdoctors,tnDNurses,tnDStaff,tnDIncome,tnDExpense,tnDLabRequest,tnDLabReceipt,tnDLabTests,tnDLabInvalid,tnDLabValid,;
             tnDLabTurn,tnDRadRequest,tnDRadReceipt,tnDRadTest,tnDRadInvalid,tnDRadValid,tnDRadTurn,tnDOtRequest,tnDOtReceipt,tnDOtSurgeries,tnDOtDeaths,tnDOtTurn,tnDDeliveries,tnDTotalDeaths,;
             tnDfemale,tnDmale,tnDchild,tnDpLocal,tnDforeigner,tnDteenager,tnDAdult,tnDprepaid,tnDhas_ins,tnDhas_cor,tnDAcct_rec,tnDAcct_pay,tnDDrugStockValue,tnDhas_nhif
         gnDIncome=0.00		&&Iif(glVatable,gnDIncome*(1+gnVat),gnDIncome)
         gnDAcct_rec=Iif(glVatable,gnDAcct_rec*(1+gnVat),gnDAcct_rec)
         sn=SQLExec(gnConnHandle,"exec tsp_UpdateDashBoard ?gnCompid, ?tnDnYear,?tnDmonth,?tcDuptype,?tnDPatients,?tnDdoctors,?tnDNurses,?tnDStaff,?tnDIncome,"+;
             "?tnDExpense,?tnDLabRequest,?tnDLabReceipt,?tnDLabTests,?tnDLabInvalid,?tnDLabValid,?tnDLabTurn,?tnDRadRequest,?tnDRadReceipt,?tnDRadTest,?tnDRadInvalid,?tnDRadValid,?tnDRadTurn,"+;
             "?tnDOtRequest,?tnDOtReceipt,?tnDOtSurgeries,?tnDOtDeaths,?tnDOtTurn,?tnDDeliveries,?tnDTotalDeaths,?tnDfemale,?tnDmale,?tnDchild,?tnDpLocal,?tnDforeigner,?tnDteenager,?tnDAdult,?tnDprepaid,"+;
             "?tnDhas_ins,?tnDhas_cor,?tnDAcct_rec,?tnDAcct_pay,?tnDDrugStockValue,0,?tnDhas_nhif","Patupd")
         If !(sn>0)
             =sysmsg("Dashboard not updated")
         Endif
                      */
        }
    }//end of updateDashBoard

    public class DiagnosisEngine
    {
        public DiagnosisEngine()
        {
            MessageBox.Show("We are the AI Diagnosis Engine");
        }
        public static string DiagResult()
        {
            string dresult = "This is the diagnostic result";
            return dresult;
        }
    }//end of DiagnosiEngine

    public class tclassChkpassWord
    {
        public tclassChkpassWord()
        {
            MessageBox.Show("We would like to check the password");
        }

        public static string dpassWord(string cpassString)
        {
            //            MessageBox.Show("we are inside dpassword of classchkpassword in tclasslibrary with text = "+cpassString);
            int i = 0;
            int j = 0;
            int l = 0;
            string ncrptd = "";
            string t = "";
            int k = cpassString.Length;
            string xpswd = cpassString.ToUpper();                   // this.Text.ToUpper();
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
                //            MessageBox.Show("The value of T is " + t+" length of t is "+t.Length);
                j = t.Length % 10;
                //          MessageBox.Show("The value of j is " + t);

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
            }
            else
            {
                MessageBox.Show("Password cannot be less than 6 Characters, please try again");
                ncrptd = "";
            }
            return ncrptd;
        }
    }



    public class requestnumber1
    {
        private string reqnumb(string cs, DateTime dsysdate)
        {
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
            {
                string sreq = "select req_numb from client_code";
                string crequest = "";
                int rcounter;
                SqlDataAdapter requestview = new SqlDataAdapter(sreq, ndConnHandle3);
                ndConnHandle3.Open();
                DataTable reqView = new DataTable();
                requestview.Fill(reqView);
                if (reqView != null)
                {
                    rcounter = Convert.ToInt32(reqView.Rows[0]["req_numb"]);
                    if (Convert.ToUInt32(reqView.Rows[0]["req_numb"]) >= 9999)
                    {
                        crequest = dsysdate.Year.ToString().Substring(2, 2) + dsysdate.Month.ToString().PadLeft(2, '0') + dsysdate.Day.ToString().PadLeft(2, '0') + "0001";
                        return crequest;
                    }
                    else
                    {
                        crequest = dsysdate.Year.ToString().Substring(2, 2) + dsysdate.Month.ToString().PadLeft(2, '0') + dsysdate.Day.ToString().PadLeft(2, '0') + rcounter.ToString().PadLeft(4, '0');
                        return crequest;
                    }
                }
                else { return ""; }
            }
        }//end of reqnumb
    }//end of requestnumber

    public class temporary_files
    {
        public void temporary_files_upload(int tntime)
        {
            MessageBox.Show("We will upload the temporary file here");
            /*
                          
             Parameters tntime
Do Case
Case tntime =1			&&complaints and findings
	With Thisform.pageframe1.page10
		sn=SQLExec(gnConnHandle,"select complain,findings from pat_visit_bkp where ccustcode = ?gcCustCode and activesession=1 and visno=?gnVisNo","comV")
		If sn>0 And Reccount()>0
			.edit1.Value=Alltrim(complain)
			.edit5.Value=Alltrim(findings)
		Else
			Store '' To .edit1.Value,.edit5.Value
		Endif
	Endwith
Case tntime =2		&&Laboratory tests
	With Thisform.pageframe1.page1
		Select selTestItems
		Zap
		sn=SQLExec(gnConnHandle,"select itemNo,item_name,total_cost,lcovered,srv_code from labtestitems_bkp where ccustcode = ?gcCustCode and visno=?gnVisNo","tlabv")
		If sn>0 And Reccount()>0
			Do While !Eof()
				lnItemno=itemNo
				lcSrvCode=srv_code
				lcTestName=item_name
				lnTestCost=total_cost
				llcov=lcovered
				Insert Into selTestItems ;
					(itemNo,test_name,total_cost,testsel,lcov,srv_code)	Values (lnItemno,lcTestName,lnTestCost,.T.,llcov,lcSrvCode)
				Update templabtestitems Set tsel=.T. Where  itemNo=lnItemno
				Select tlabv
				Skip
			Enddo
			Select templabtestitems
			Locate
			Select selTestItems
			glLabUpdated=Iif(Reccount()>0,.T.,.F.)
			Locate
		Else
			Delete From selTestItems
			Update templabtestitems Set tsel=.F.
			Select templabtestitems
			Locate
		Endif
		With .selTest
			Select selTestItems
			Sum All total_cost For testsel And lcov To gnLabCoverPayment
			Sum All total_cost For testsel And !lcov To gnLabCashPayment
			Locate
			.RecordSource = 'selTestItems'
			.column1.ControlSource = 'test_name'
			.column2.ControlSource = 'total_cost'
			.column3.ControlSource = 'testsel'
		Endwith
		.selTest.Refresh
	Endwith
Case tntime =3		&&Radiology tests
	With Thisform.pageframe1.page2
*		Thisform.getradexams
		Select selExamItems
		Zap
		sn=SQLExec(gnConnHandle,"select itemNo,item_name,total_cost,lcovered,srv_code from radtestitems_bkp where ccustcode = ?gcCustCode and visno=?gnVisNo","tradv")
		If sn>0 And Reccount()>0
			Do While !Eof()
				lnItemno=itemNo
				lcSrvCode=srv_code
				lcTestName=item_name
				lnTestCost=total_cost
				llcov=lcovered
				Insert Into selExamItems ;
					(itemNo,exam_name,total_cost,examsel,lcov,srv_code)	Values (lnItemno,lcTestName,lnTestCost,.T.,llcov,lcSrvCode)
				Update tempradtestitems Set tsel=.T. Where  itemNo=lnItemno
				Select tradv
				Skip
			Enddo
			Select tempradtestitems
			Locate
			Select selExamItems
			glradUpdated=Iif(Reccount()>0,.T.,.F.)
			Locate
		Else
			Delete From selExamItems
			Update tempradtestitems Set tsel=.F.
			Select tempradtestitems
			Locate
		Endif
		With .selExam
			Select selExamItems
			Sum All total_cost For examsel And lcov To gnradCoverPayment
			Sum All total_cost For examsel And !lcov To gnradCashPayment

			Locate
			.RecordSource = 'selExamItems'
			.column1.ControlSource = 'exam_name'
			.column2.ControlSource = 'total_cost'
			.column3.ControlSource = 'examsel'
		Endwith
		.selExam.Refresh
	Endwith
Case tntime =4		&&Operating Theatre Procedures
	With Thisform.pageframe1.page3
*		Thisform.getoptproc
		Select selProcItems
		Zap
		sn=SQLExec(gnConnHandle,"select itemNo,item_name,total_cost,lcovered,srv_code from opttestitems_bkp where ccustcode = ?gcCustCode and visno=?gnVisNo","toptv")
		If sn>0 And Reccount()>0
			Do While !Eof()
				lnItemno=itemNo
				lcSrvCode=srv_code
				lcTestName=item_name
				lnTestCost=total_cost
				llcov=lcovered
				Insert Into selProcItems ;
					(itemNo,proc_name,total_cost,procsel,lcov,srv_code)	Values (lnItemno,lcTestName,lnTestCost,.T.,llcov,lcSrvCode)
				Update tempopttestitems Set tsel=.T. Where  itemNo=lnItemno
				Select toptv
				Skip
			Enddo
			Select tempopttestitems
			Locate
			Select selProcItems
			gloptUpdated=Iif(Reccount()>0,.T.,.F.)
		Else
			Delete From selProcItems
			Update tempopttestitems Set tsel=.F.
			Select tempopttestitems
			Locate
		Endif
		With .selproc
			Select selProcItems
			Sum All total_cost For procsel And lcov To gnoptCoverPayment
			Sum All total_cost For procsel And !lcov To gnoptCashPayment
			Locate
			.RecordSource = 'selProcItems'
			.column1.ControlSource = 'proc_name'
			.column2.ControlSource = 'total_cost'
			.column3.ControlSource = 'procsel'
		Endwith
		.selproc.Refresh
	Endwith
Case tntime =5		&&Prescription
	With Thisform.pageframe1.page9
*		Thisform.medicinegrid
*		Thisform.getdruglist
		Select selDrugItems
		Zap
*		sn=SQLExec(gnConnHandle,"select products.prod_code,prod_name,drug_dispense_bkp.quantity,* from drug_dispense_bkp,products "+;
			"where drug_dispense_bkp.prod_code=products.prod_code and ccustcode = ?gcCustCode and visno=?gnVisNo","tprev")
		sn=SQLExec(gnConnHandle,"select products.prod_code,prod_name,drug_dispense_bkp.quantity,drug_dispense_bkp.unitprice,drug_dispense_bkp.unitmeas,drug_dispense_bkp.cformula,drug_dispense_bkp.perday, "+;
		"drug_dispense_bkp.lcovered from drug_dispense_bkp,products "+;
			"where drug_dispense_bkp.prod_code=products.prod_code and ccustcode = ?gcCustCode and visno=?gnVisNo","tprev")
		If sn>0 And Reccount()>0
			Do While !Eof()
				lnItemno=0
				lcTestName=prod_name
				lnqty=quantity
				lnPrice=unitprice
				lnTotCost=quantity*unitprice		&&total_cost
				lcProdCode=prod_code
				lnUnitMeas=unitmeas
				lnformula=cformula
				lnPerDay=perday
				llcov=IIF(!ISNULL(lcovered),lcovered,.f.)
				Insert Into selDrugItems ;
					(prod_code,itemNo,prod_name,quantity,unit_price,seldrug,lcov)	Values (lcProdCode,lnItemno,lcTestName,lnqty,lnPrice,.T.,llcov)
				Update pharmaitems Set seldrug=.T.,tot_amt=lnTotCost, quantity=lnqty,unit_meas=lnUnitMeas,formula=lnformula,perday=lnPerDay Where prod_code = lcProdCode
				Skip
			Enddo
		Else
			Delete From selDrugItems
			Update pharmaitems Set seldrug=.F.,quantity=0,unit_meas=0,formula=0,perday=0		&& Where prod_code = lcProdCode
			Select pharmaitems
			Locate
		Endif
		Select pharmaitems
		Locate
		Select selDrugItems
		glPayDrugSelected=Iif(Reccount()>0,.T.,.F.)
		With .selpres
			Select selDrugItems
			Select selDrugItems
			Sum All quantity*unit_price  For lcov To gnPreCoverPayment
			Sum All quantity*unit_price  For !lcov To gnPreCashPayment
			Locate
			.RecordSource = 'selDrugItems'
			.column1.ControlSource = 'prod_name'
			.column2.ControlSource = 'quantity'
			.column3.ControlSource = 'seldrug'
		Endwith
		.selpres.Refresh
	Endwith
Case tntime =6		&&Diagnosis
	With Thisform.pageframe1.page6
		Select selDiagItems
		Zap
		sn=SQLExec(gnConnHandle,"select * from diag_rept_bkp,icd10 where diag_rept_bkp.icd_code= icd10.ICD_CODE and ccustcode = ?gcCustCode and visno=?gnVisNo","tdiagv")
		If sn>0 And Reccount()>0
			Do While !Eof()
				lcCode=icd_code
				lcDiagName=icd_name
				Insert Into selDiagItems ;
					(icdno,icd_name)	Values (lcCode,lcDiagName)
				Update icdview Set icd_sel=.T. Where  icd_code=lcCode
				Select tdiagv
				Skip
			Enddo
			Select icdview
			Count For icd_sel To N
			glPatDisSelected=Iif(N>0,.T.,.F.)
			Locate
		Else
			Delete From selDiagItems
			Update icdview Set icd_sel=.F.
			Select icdview
			Locate
		Endif

		With .selDiag
			Select selDiagItems
			Locate
			.RecordSource = 'selDiagItems'
			.column1.ControlSource = 'icd_name'
			.column2.ControlSource = 'icdno'
		Endwith
		.selDiag.Refresh
	Endwith
Case tntime =7		&&Triage details
*thisfor
	With Thisform.pageframe1.page11
		Store 0.00 To .text18.Value,.text19.Value,.text20.Value,.text22.Value,.text23.Value,.text25.Value,.text26.Value,.text27.Value
		Store .F. To .nav_butt1.command2.Enabled,.nav_butt1.command3.Enabled,.nav_butt1.command4.Enabled,;
			.nav_butt1.command6.Enabled
		sn=SQLExec(gnConnHandle,"select ccustcode,visno,clinician,p_hip,c_pressure,p_pulse,p_temp,p_waist,p_height,p_weight,p_bmi,"+;
			"p_resp from todayvisit where ccustcode=?gcCustCode and visno=?gnVisNo and p_temp>0.00 ","tRiage")
		If sn>0 And Reccount()>0
			glTriageDone=.T.
			.text23.Value=p_hip
			.text24.Value=c_pressure
			.text25.Value=p_pulse
			.text26.Value=p_temp
			.text27.Value=p_waist
			.text18.Value=p_height
			.text19.Value=p_weight
			.text20.Value=p_bmi
			.text22.Value=p_resp 
		Else
			Store 0.00 To .text23.Value,.text25.Value,.text26.Value,.text27.Value,.text18.Value,.text19.Value,.text20.Value,.text22.Value
			.text24.Value=''
			glTriageDone=.F.
		Endif
	Endwith
Endcase
Thisform.Refresh

            */
        }
        public void temporary_files_update(string cs, int tnSource, string tcCode, int nvisno, string proservcode, string proservname, decimal itemcost, bool tlcov, bool inout)
        {
            //       MessageBox.Show("We will work on the temporary files");
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
            {
                switch (tnSource)
                {
                    case 1: //complains, findings and diagnosis


                        /*                    Case tnSource = 1 && Complaints and findings
                                              With Thisform.pageframe1.page10
                                                  sn = SQLExec(gnConnHandle, "select 1 from pat_visit_bkp where ccustcode = ?gcCustCode and activesession=1 and visno=?gnVisNo and sess_id=?gnSessionID", "")
                                                  If sn> 0 And Reccount() > 0
                                                       tn = SQLExec(gnConnHandle, 'update pat_visit_bkp set complain=?.edit1.value,findings=?.edit5.value where ccustcode = ?gcCustCode and activesession=1 and visno=?gnVisNo and sess_id=?gnSessionID', 'updv1')
                                                       If !(tn > 0)
                                                           = sysmsg('Could not update complains and findings in Temporary file')
                                                       Endif
                                                   Else
                                                       fn = SQLExec(gnConnHandle, "insert into pat_visit_bkp (ccustcode,visno,visdate,vistime,compid,complain,findings,dr_id,sess_id)" +;
                                                  "values " +;
                                                  "(?gcCustCode,?gnVisNo,convert(date,getdate()),convert(time,getdate()),?gnCompID,?.edit1.value,?.edit5.value,?gnIntDocID,?gnSessionID)","patin")
                                                      If !(fn > 0)
                                                          = sysmsg('Could not Insert complains and findings in Temporary file')
                                                      Endif
                                                  Endif
                                              Endwith
                                                          Case tnSource=5			&&Diagnosis
                                              sn=SQLExec(gnConnHandle,"select 1 from diag_rept_bkp where ccustcode = ?gcCustCode and visno=?gnVisNo and icd_code=?tcItemName  and sess_id=?gnSessionID","")
                                              If !(sn>0 And Reccount()>0)
                                                  fn=SQLExec(gnConnHandle,"Insert Into diag_rept_bkp (icd_code,ccustcode,visno,sess_id) values (?lcIcdNo,?gcCustCode,?gnVisNo,?gnSessionID)","divw1")
                                                  If !(fn>0 )
                                                      =sysmsg('Could not Insert Diagnoses in Temporary file')
                                                  Endif
                                              Endif

                                                   */
                        break;
                    case 2: //laboratory tests
                        ndConnHandle3.Open();
                        if (inout)
                        {
                            SqlDataAdapter labupd1 = new SqlDataAdapter();  //insert a new record in the backup file
                            string cquery = "Insert Into labtestitems_bkp (srv_code,item_name,total_cost,ccustcode,visno,lcovered) values (@srvCode,@tcItemName,@tnItemCost,@gcCustCode,@gnVisNo,@tlcov)";


                            SqlDataReader cLabDetails = null;
                            SqlCommand cGetUser = new SqlCommand("select 1 from labtestitems_bkp  where ccustcode = @gcCustCode and visno=@gnVisNo and srv_code=@srvCode", ndConnHandle3);
                            cGetUser.Parameters.Add("@gcCustCode", SqlDbType.Char).Value = tcCode;
                            cGetUser.Parameters.Add("@gnVisNo", SqlDbType.Int).Value = nvisno;
                            cGetUser.Parameters.Add("@srvCode", SqlDbType.Char).Value = proservcode;
                            cLabDetails = cGetUser.ExecuteReader();
                            cLabDetails.Read();
                            if (cLabDetails.HasRows == false)  //The backup does not have the labtest so we will insert
                            {
                                cLabDetails.Close();
                                labupd1.InsertCommand = new SqlCommand(cquery, ndConnHandle3);
                                labupd1.InsertCommand.Parameters.Add("@srvCode", SqlDbType.Char).Value = proservcode;
                                labupd1.InsertCommand.Parameters.Add("@tcItemName", SqlDbType.Char).Value = proservname;
                                labupd1.InsertCommand.Parameters.Add("@tnItemCost", SqlDbType.Decimal).Value = itemcost;
                                labupd1.InsertCommand.Parameters.Add("@gcCustCode", SqlDbType.Char).Value = tcCode;
                                labupd1.InsertCommand.Parameters.Add("@gnVisNo", SqlDbType.Int).Value = nvisno;
                                labupd1.InsertCommand.Parameters.Add("@tlcov", SqlDbType.Bit).Value = tlcov;
                                labupd1.InsertCommand.ExecuteNonQuery();
                                ndConnHandle3.Close();
                            }
                        }
                        else
                        {
                            SqlDataAdapter labdel = new SqlDataAdapter();  //remove a record from the backup file
                            string delquery = "delete from labtestitems_bkp where ccustcode =@gcCustCode and visno =@gnVisNo and srv_code =@srvCode";
                            labdel.DeleteCommand = new SqlCommand(delquery, ndConnHandle3);
                            labdel.DeleteCommand.Parameters.Add("@gcCustCode", SqlDbType.Char).Value = tcCode;
                            labdel.DeleteCommand.Parameters.Add("@gnVisNo", SqlDbType.Int).Value = nvisno;
                            labdel.DeleteCommand.Parameters.Add("@srvCode", SqlDbType.Char).Value = proservcode;
                            labdel.DeleteCommand.ExecuteNonQuery();
                            ndConnHandle3.Close();
                        }
                        break;
                    case 3: //Radiology exams
                        ndConnHandle3.Open();
                        if (inout)
                        {
                            SqlDataAdapter radupd1 = new SqlDataAdapter();  //insert a new record in the backup file
                            string cquery = "Insert Into radtestitems_bkp (srv_code,item_name,total_cost,ccustcode,visno,lcovered) values (@srvCode,@tcItemName,@tnItemCost,@gcCustCode,@gnVisNo,@tlcov)";


                            SqlDataReader cLabDetails = null;
                            SqlCommand cGetUser = new SqlCommand("select 1 from radtestitems_bkp  where ccustcode = @gcCustCode and visno=@gnVisNo and srv_code=@srvCode", ndConnHandle3);
                            cGetUser.Parameters.Add("@gcCustCode", SqlDbType.Char).Value = tcCode;
                            cGetUser.Parameters.Add("@gnVisNo", SqlDbType.Int).Value = nvisno;
                            cGetUser.Parameters.Add("@srvCode", SqlDbType.Char).Value = proservcode;
                            cLabDetails = cGetUser.ExecuteReader();
                            cLabDetails.Read();
                            if (cLabDetails.HasRows == false)  //The backup does not have the labtest so we will insert
                            {
                                cLabDetails.Close();
                                radupd1.InsertCommand = new SqlCommand(cquery, ndConnHandle3);
                                radupd1.InsertCommand.Parameters.Add("@srvCode", SqlDbType.Char).Value = proservcode;
                                radupd1.InsertCommand.Parameters.Add("@tcItemName", SqlDbType.Char).Value = proservname;
                                radupd1.InsertCommand.Parameters.Add("@tnItemCost", SqlDbType.Decimal).Value = itemcost;
                                radupd1.InsertCommand.Parameters.Add("@gcCustCode", SqlDbType.Char).Value = tcCode;
                                radupd1.InsertCommand.Parameters.Add("@gnVisNo", SqlDbType.Int).Value = nvisno;
                                radupd1.InsertCommand.Parameters.Add("@tlcov", SqlDbType.Bit).Value = tlcov;
                                radupd1.InsertCommand.ExecuteNonQuery();
                                ndConnHandle3.Close();
                            }
                        }
                        else
                        {
                            SqlDataAdapter raddel = new SqlDataAdapter();  //remove a record from the backup file
                            string delquery = "delete from radtestitems_bkp where ccustcode =@gcCustCode and visno =@gnVisNo and srv_code =@srvCode";
                            raddel.DeleteCommand = new SqlCommand(delquery, ndConnHandle3);
                            raddel.DeleteCommand.Parameters.Add("@gcCustCode", SqlDbType.Char).Value = tcCode;
                            raddel.DeleteCommand.Parameters.Add("@gnVisNo", SqlDbType.Int).Value = nvisno;
                            raddel.DeleteCommand.Parameters.Add("@srvCode", SqlDbType.Char).Value = proservcode;
                            raddel.DeleteCommand.ExecuteNonQuery();
                            ndConnHandle3.Close();
                        }
                        break;
                    case 4: //operating theatre procedures
                            //           MessageBox.Show("We will update the Operating Theatre temporary files");
                        ndConnHandle3.Open();
                        if (inout)
                        {
                            SqlDataAdapter optupd1 = new SqlDataAdapter();  //insert a new record in the backup file
                            string cquery = "Insert Into opttestitems_bkp (srv_code,item_name,total_cost,ccustcode,visno,lcovered) values (@srvCode,@tcItemName,@tnItemCost,@gcCustCode,@gnVisNo,@tlcov)";


                            SqlDataReader cLabDetails = null;
                            SqlCommand cGetUser = new SqlCommand("select 1 from opttestitems_bkp  where ccustcode = @gcCustCode and visno=@gnVisNo and srv_code=@srvCode", ndConnHandle3);
                            cGetUser.Parameters.Add("@gcCustCode", SqlDbType.Char).Value = tcCode;
                            cGetUser.Parameters.Add("@gnVisNo", SqlDbType.Int).Value = nvisno;
                            cGetUser.Parameters.Add("@srvCode", SqlDbType.Char).Value = proservcode;
                            cLabDetails = cGetUser.ExecuteReader();
                            cLabDetails.Read();
                            if (cLabDetails.HasRows == false)  //The backup does not have the labtest so we will insert
                            {
                                cLabDetails.Close();
                                optupd1.InsertCommand = new SqlCommand(cquery, ndConnHandle3);
                                optupd1.InsertCommand.Parameters.Add("@srvCode", SqlDbType.Char).Value = proservcode;
                                optupd1.InsertCommand.Parameters.Add("@tcItemName", SqlDbType.Char).Value = proservname;
                                optupd1.InsertCommand.Parameters.Add("@tnItemCost", SqlDbType.Decimal).Value = itemcost;
                                optupd1.InsertCommand.Parameters.Add("@gcCustCode", SqlDbType.Char).Value = tcCode;
                                optupd1.InsertCommand.Parameters.Add("@gnVisNo", SqlDbType.Int).Value = nvisno;
                                optupd1.InsertCommand.Parameters.Add("@tlcov", SqlDbType.Bit).Value = tlcov;
                                optupd1.InsertCommand.ExecuteNonQuery();
                                ndConnHandle3.Close();
                            }
                        }
                        else
                        {
                            SqlDataAdapter optdel = new SqlDataAdapter();  //remove a record from the backup file
                            string delquery = "delete from opttestitems_bkp where ccustcode =@gcCustCode and visno =@gnVisNo and srv_code =@srvCode";
                            optdel.DeleteCommand = new SqlCommand(delquery, ndConnHandle3);
                            optdel.DeleteCommand.Parameters.Add("@gcCustCode", SqlDbType.Char).Value = tcCode;
                            optdel.DeleteCommand.Parameters.Add("@gnVisNo", SqlDbType.Int).Value = nvisno;
                            optdel.DeleteCommand.Parameters.Add("@srvCode", SqlDbType.Char).Value = proservcode;
                            optdel.DeleteCommand.ExecuteNonQuery();
                            ndConnHandle3.Close();
                        }
                        break;
                        //              case 5: //prescription 
                } //end of switch
            }//end of connect 
        }//end of temporary_files_update

        public void temporary_files_update_pharmacy(string cs, decimal itemcost, string proservcode, int qty, int dissue, int nformula, int pday, string ddos, string tcCode, int nvisno, bool tlcov, bool inout)
        {
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
            {
                ndConnHandle3.Open();
                if (inout)
                {
                    SqlDataAdapter phaupd = new SqlDataAdapter();  //insert a new record in the backup file
                    string cquery = "Insert Into drug_dispense_bkp(unitprice, prod_code, unitmeas, quantity, cformula, perday, dosage, ccustcode, visno,lcovered) ";
                    cquery += " values (@gnProdCost,@gcProdCode,@Qty,@lnIssue,@lnFormula,@lnPerDay,@gcDosage,@gcCustCode,@gnVisNo,@llcov)";


                    SqlDataReader cphaDetails = null;
                    SqlCommand cGetUser = new SqlCommand("select 1 from drug_dispense_bkp  where ccustcode = @gcCustCode and visno=@gnVisNo and prod_code=@gcProdCode", ndConnHandle3);
                    cGetUser.Parameters.Add("@gcCustCode", SqlDbType.Char).Value = tcCode;
                    cGetUser.Parameters.Add("@gnVisNo", SqlDbType.Int).Value = nvisno;
                    cGetUser.Parameters.Add("@gcProdCode", SqlDbType.Char).Value = proservcode;
                    cphaDetails = cGetUser.ExecuteReader();
                    cphaDetails.Read();
                    if (cphaDetails.HasRows == false)  //The backup does not have the labtest so we will insert
                    {
                        cphaDetails.Close();
                        phaupd.InsertCommand = new SqlCommand(cquery, ndConnHandle3);
                        phaupd.InsertCommand.Parameters.Add("@gnProdCost", SqlDbType.Decimal).Value = itemcost;
                        phaupd.InsertCommand.Parameters.Add("@gcProdCode", SqlDbType.Char).Value = proservcode;
                        phaupd.InsertCommand.Parameters.Add("@Qty", SqlDbType.Int).Value = qty;
                        phaupd.InsertCommand.Parameters.Add("@lnIssue", SqlDbType.Int).Value = dissue;
                        phaupd.InsertCommand.Parameters.Add("@lnFormula", SqlDbType.Int).Value = nformula;
                        phaupd.InsertCommand.Parameters.Add("@lnPerDay", SqlDbType.Int).Value = pday;
                        phaupd.InsertCommand.Parameters.Add("@gcDosage", SqlDbType.Char).Value = ddos;
                        phaupd.InsertCommand.Parameters.Add("@gcCustCode", SqlDbType.Char).Value = tcCode;
                        phaupd.InsertCommand.Parameters.Add("@gnVisNo", SqlDbType.Int).Value = nvisno;
                        phaupd.InsertCommand.Parameters.Add("@llcov", SqlDbType.Bit).Value = tlcov;
                        phaupd.InsertCommand.ExecuteNonQuery();
                        ndConnHandle3.Close();
                    }
                }
                else
                {
                    //        MessageBox.Show("We will delete from the prescription temporary files");
                    SqlDataAdapter phadel = new SqlDataAdapter();  //remove a record from the backup file
                    string delquery = "delete from drug_dispense_bkp where ccustcode =@gcCustCode and visno =@gnVisNo and prod_code =@gcProdCode";
                    phadel.DeleteCommand = new SqlCommand(delquery, ndConnHandle3);
                    phadel.DeleteCommand.Parameters.Add("@gcCustCode", SqlDbType.Char).Value = tcCode;
                    phadel.DeleteCommand.Parameters.Add("@gnVisNo", SqlDbType.Int).Value = nvisno;
                    phadel.DeleteCommand.Parameters.Add("@gcProdCode", SqlDbType.Char).Value = proservcode;
                    phadel.DeleteCommand.ExecuteNonQuery();
                    ndConnHandle3.Close();
                }
            }
        }
        public void temporary_files_clear(string cs, string tcCode, int nvisno)
        {
            //            MessageBox.Show("We will clear temporary files here");
            //            string cs = globalvar.cos;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string tempfile0 = "delete from pat_visit_bkp where ccustcode = @gcCustCode and activesession=1 and visno=@gnVisNo";
                string tempfile1 = "delete from labtestitems_bkp where ccustcode=@gcCustCode and visno=@gnVisNo";
                string tempfile2 = "delete from radtestitems_bkp where ccustcode = @gcCustCode and visno=@gnVisNo";
                string tempfile3 = "delete from opttestitems_bkp where ccustcode=@gcCustCode and visno=@gnVisNo";
                string tempfile4 = "delete from diag_rept_bkp where ccustcode = @gcCustCode and visno=@gnVisNo";
                string tempfile5 = "delete from drug_dispense_bkp where ccustcode=@gcCustCode and visno=@gnVisNo";

                SqlDataAdapter tclear0 = new SqlDataAdapter();
                tclear0.UpdateCommand = new SqlCommand(tempfile0, ndConnHandle);

                SqlDataAdapter tclear1 = new SqlDataAdapter();
                tclear1.UpdateCommand = new SqlCommand(tempfile1, ndConnHandle);

                SqlDataAdapter tclear2 = new SqlDataAdapter();
                tclear2.UpdateCommand = new SqlCommand(tempfile2, ndConnHandle);

                SqlDataAdapter tclear3 = new SqlDataAdapter();
                tclear3.UpdateCommand = new SqlCommand(tempfile3, ndConnHandle);

                SqlDataAdapter tclear4 = new SqlDataAdapter();
                tclear4.UpdateCommand = new SqlCommand(tempfile4, ndConnHandle);

                SqlDataAdapter tclear5 = new SqlDataAdapter();
                tclear5.UpdateCommand = new SqlCommand(tempfile5, ndConnHandle);
                ndConnHandle.Open();

                tclear0.UpdateCommand.Parameters.Add("@gcCustCode", SqlDbType.Char).Value = tcCode;
                tclear0.UpdateCommand.Parameters.Add("@gnVisNo", SqlDbType.Int).Value = nvisno;

                tclear1.UpdateCommand.Parameters.Add("@gcCustCode", SqlDbType.Char).Value = tcCode;
                tclear1.UpdateCommand.Parameters.Add("@gnVisNo", SqlDbType.Int).Value = nvisno;

                tclear2.UpdateCommand.Parameters.Add("@gcCustCode", SqlDbType.Char).Value = tcCode;
                tclear2.UpdateCommand.Parameters.Add("@gnVisNo", SqlDbType.Int).Value = nvisno;

                tclear3.UpdateCommand.Parameters.Add("@gcCustCode", SqlDbType.Char).Value = tcCode;
                tclear3.UpdateCommand.Parameters.Add("@gnVisNo", SqlDbType.Int).Value = nvisno;

                tclear4.UpdateCommand.Parameters.Add("@gcCustCode", SqlDbType.Char).Value = tcCode;
                tclear4.UpdateCommand.Parameters.Add("@gnVisNo", SqlDbType.Int).Value = nvisno;

                tclear5.UpdateCommand.Parameters.Add("@gcCustCode", SqlDbType.Char).Value = tcCode;
                tclear5.UpdateCommand.Parameters.Add("@gnVisNo", SqlDbType.Int).Value = nvisno;


                tclear0.UpdateCommand.ExecuteNonQuery();
                tclear1.UpdateCommand.ExecuteNonQuery();
                tclear2.UpdateCommand.ExecuteNonQuery();
                tclear3.UpdateCommand.ExecuteNonQuery();
                tclear4.UpdateCommand.ExecuteNonQuery();
                tclear5.UpdateCommand.ExecuteNonQuery();
                ndConnHandle.Close();
            }
        }// end of temporary_files_clear

    }//end of temporary_files 

}//end of Tclass Library

