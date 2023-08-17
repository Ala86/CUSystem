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
using System.Globalization;

namespace TclassLibrary
{
    public partial class runPayroll : Form
    {
        string cs = string.Empty;
        int ncompid = 0;
        string dloca = string.Empty;
        DataTable payview = new DataTable();
        string gcPeriod = string.Empty;
        decimal gnPartBasic = 0.00m;
        bool glPartSal = false;
        decimal gnBasic = 0.00m;
        decimal gnRotHrs = 0.00m;
        decimal gnRotPay = 0.00m;
        decimal gnHotHrs = 0.00m;
        decimal gnHotPay = 0.00m;
        decimal gnAlwTotal = 0.00m;
        decimal gnalwtaxy = 0.00m;
        decimal gnalwtaxn = 0.00m;
        decimal gnNhifPay = 0.00m;
        decimal gnTotOvt = 0.00m;
        decimal gnSalAdv = 0.00m;
        decimal gnloanTotal = 0.00m;
        decimal lnEmpDed1 = 0.00m;          // &&The amount the employee is supposed to pay when the contribution is a percentage of his salary
        decimal lnEmpDed2 = 0.00m;          // && The amount the employee is supposed to pay when the contribution is an absolute value
        decimal lnEmplDed1 = 0.00m;         // && The amount the employer is supposed to pay when the contribution is a percentage of his salary
        decimal lnEmplDed2 = 0.00m;         // && The amount the employer is supposed to pay when the contribution is an absolute value
        decimal gnstfcont = 0.00m;          // lnEmpDed1 + lnEmpDed2
        decimal gncmpcont = 0.00m;          // lnEmplDed1 + lnEmplDed2
        decimal nbonamt = 0.00m;
        decimal gnNssfStaffPay = 0.00m;


        public runPayroll(string tcCos, int tnCompid, string tcLoca)
        {
            InitializeComponent();
            cs = tcCos;
            ncompid = tnCompid;
            dloca = tcLoca;
        }

        private void runPayroll_Load(object sender, EventArgs e)
        {
            this.Text = dloca + "<< Process Payroll >>";
            getPeriod();
            getStaff();
        }

        private void getPeriod()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                payview.Clear();
                ndConnHandle.Open();
                string dsql1 = "exec tsp_PayrollPeriod " + ncompid;
                SqlDataAdapter da1 = new SqlDataAdapter(dsql1, ndConnHandle);
                DataTable perview = new DataTable();
                da1.Fill(perview);
                if (perview.Rows.Count > 0)
                {
                    gcPeriod = perview.Rows[0]["cperiod"].ToString().Trim();
                    int dmonth = Convert.ToInt16(perview.Rows[0]["cperiod"].ToString().Trim().Substring(0, 2));
                    int dyear = Convert.ToInt16(perview.Rows[0]["cperiod"].ToString().Trim().Substring(2,4));
                    string monthName = new DateTime(dyear,dmonth,1).ToString("MMMM", CultureInfo.InvariantCulture);
                    textBox2.Text = monthName + ", " + dyear.ToString().Trim();// DateTime.Now.Year;
                }
                else { MessageBox.Show("Period has not been set, inform IT DEPT immediately"); }
            }
        }

        private void getStaff()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                payview.Clear();
                ndConnHandle.Open();
                string dsql = "exec tsp_getActiveStaff " + ncompid;
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(payview);
                if (payview.Rows.Count > 0)
                {
                    textBox1.Text = payview.Rows.Count.ToString();
                    SaveButton.Enabled = true;
                    SaveButton.BackColor = Color.LawnGreen;
                }
                else { SaveButton.Enabled = false; SaveButton.BackColor = Color.Gainsboro; MessageBox.Show("No active staff found, inform IT DEPT immediately"); }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to Process Payroll ", "Payroll Processing", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                label3.Text = "updating periodic records";
                int lnCount = 0;
                string tcstaffno = string.Empty;
                decimal tnSalary = 0.00m;
                bool tlTxEmt = false;
                bool tlSocec = false;
//                gcPeriod = DateTime.Today.Month.ToString().Trim().PadLeft(2, '0') + DateTime.Today.Year.ToString().Trim().PadLeft(4, '0');

                foreach (DataRow dr in payview.Rows)
                {
                    lnCount++;
                    textBox3.Text = lnCount.ToString().Trim();
                    tcstaffno = dr["staffno"].ToString();
                    tnSalary = Convert.ToDecimal(dr["nbasic"]);
                    tlTxEmt = Convert.ToBoolean(dr["ltaxfree"]) ? true : false;         //tax exempt
                    tlSocec = Convert.ToBoolean(dr["csocex"]) ? true : false;           // social security exemption
                    partSalary(tcstaffno);                                              //if part salary, then take what is in saldet
                    checksaldet(tcstaffno, tnSalary, tlTxEmt, tlSocec);                    //Check if records exist in saldet,otherwise add
                    updovt(tcstaffno);                                                  //get overtime details from ovtime.dbf
                    saladv(tcstaffno);                                                  //get salary advance details from saladv.dbf
                    getallowance(tcstaffno);                                            //get allwaonces
                    getloans(tcstaffno);                                                //get loans
                    getcontributions(tcstaffno, tnSalary);                               //get other contributions
                    updatesaldet(tcstaffno);                                            //Calculate and update salaries
                    reinitvar();
                }
                MessageBox.Show("Salary processing Successful");
                this.Close();                
            }
            else
            {
                MessageBox.Show("You can come back and run payroll as many times as necessary");
            }
            /*
				If glBioLink
					lnOrigSal=nbasic/52
					lnHrlySal=lnOrigSal/gnWeekHrs
					lnHrsNumb=Thisform.getattendance(gcStaffNo,lnHrlySal)
					lnAttenBasic=lnHrlySal*lnHrsNumb*12
					gnBasic=Iif(glPartSal,gnPartBasic,lnAttenBasic)
					gnNHIFBasic=gnBasic/12						&&Monthly basic
				Else
					gnBasic =Iif(glPartSal,gnPartBasic,nbasic)
					gnNHIFBasic=gnBasic/12						&&Monthly basic
				Endif

				gnNssfStaffPay =Iif(gcLocalCountry='KENYA',200,Iif(gcLocalCountry='UGANDA',(gnBasic/12)*0.05,0.00))
				gnNssfCompPay  =Iif(gcLocalCountry='KENYA',200,Iif(gcLocalCountry='UGANDA',(gnBasic/12)*0.15,0.00))

				gnNhifPay=0.00
				If(gcLocalCountry='KENYA')
					If gnNHIFBasic >0.00
						.getnhif									&&NHIF
					Endif
				Endif
				Select atvstaff
				Skip
			Enddo
		Endif
		Thisform.Release	Endif
	.Refresh
Endwith
             */
        }

        private void partSalary(string tcStaffno)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {

                string cquery = "exec tsp_PartSal_One   @tcPeriod,@tStaffNo,@tnCompid";
                DataTable partsalview = new DataTable();
                SqlDataAdapter cuscommand = new SqlDataAdapter();
                cuscommand.SelectCommand = new SqlCommand(cquery, ndConnHandle);

                cuscommand.SelectCommand.Parameters.Add("@tstaffno", SqlDbType.VarChar).Value = tcStaffno;
                cuscommand.SelectCommand.Parameters.Add("@tcperiod", SqlDbType.Char).Value = gcPeriod;
                cuscommand.SelectCommand.Parameters.Add("@tncompid", SqlDbType.Int).Value = ncompid;

                ndConnHandle.Open();
                cuscommand.SelectCommand.ExecuteNonQuery();
                cuscommand.Fill(partsalview);
                if (partsalview.Rows.Count > 0)
                {
                    gnPartBasic = Convert.ToDecimal(partsalview.Rows[0]["nnewsal"]);
                    glPartSal = true;
                }
                else { glPartSal = false; }
                ndConnHandle.Close();
            }
        }



        private void checksaldet(string tcStaffno, decimal tnBasic, bool tltax, bool tlsoc)
        {

            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string cquery = "select 1 from saldet where staffno=@tStaffNo and cperiod=@tcPeriod and compid=@tnCompid";

                DataTable saldetview = new DataTable();
                SqlDataAdapter cuscommand = new SqlDataAdapter();
                cuscommand.SelectCommand = new SqlCommand(cquery, ndConnHandle);

                cuscommand.SelectCommand.Parameters.Add("@tstaffno", SqlDbType.Char).Value = tcStaffno;
                cuscommand.SelectCommand.Parameters.Add("@tcperiod", SqlDbType.Char).Value = gcPeriod;
                cuscommand.SelectCommand.Parameters.Add("@tncompid", SqlDbType.Int).Value = ncompid;
                ndConnHandle.Open();
                cuscommand.SelectCommand.ExecuteNonQuery();
                cuscommand.Fill(saldetview);
                if (saldetview.Rows.Count <= 0)
                {
                    gnBasic = glPartSal ? gnPartBasic : tnBasic;
                    string insSalQuery = "Insert Into saldet (staffno,csocex,cperiod,nnewsal,noldsal,ltxexemt,compid) ";
                    insSalQuery += "values (@tstaffno,@tcsocex,@tcperiod,@tnnewsal,@tnoldsal,@tltxexemt,@tncompid)";
                    SqlDataAdapter cuscommand1 = new SqlDataAdapter();
                    cuscommand1.InsertCommand = new SqlCommand(cquery, ndConnHandle);

                    cuscommand1.InsertCommand.Parameters.Add("@tstaffno", SqlDbType.Char).Value = tcStaffno;
                    cuscommand1.InsertCommand.Parameters.Add("@tcsocex", SqlDbType.Int).Value = tlsoc;
                    cuscommand1.InsertCommand.Parameters.Add("@tcperiod", SqlDbType.Char).Value = gcPeriod;
                    cuscommand1.InsertCommand.Parameters.Add("@tnnewsal", SqlDbType.Decimal).Value = gnBasic;
                    cuscommand1.InsertCommand.Parameters.Add("@tnoldsal", SqlDbType.Decimal).Value = gnBasic;
                    cuscommand1.InsertCommand.Parameters.Add("@tltxexemt", SqlDbType.Bit).Value = tltax;
                    cuscommand1.InsertCommand.Parameters.Add("@tncompid", SqlDbType.Int).Value = ncompid;
                    cuscommand1.InsertCommand.ExecuteNonQuery();
                }
                else
                {
                    gnBasic = glPartSal ? gnPartBasic : tnBasic;
                    string updSalQuery = "update saldet set csocex= @tcSosec,ltxexemt=@tlTxexemt,nnewsal=@tnBasic,noldsal=@tnBasic ";
                    updSalQuery += "where staffno=@tstaffno and cperiod=@tcperiod and compid=@tnCompid";
                    SqlDataAdapter cuscommand2 = new SqlDataAdapter();
                    cuscommand2.UpdateCommand = new SqlCommand(cquery, ndConnHandle);

                    cuscommand2.UpdateCommand.Parameters.Add("@tstaffno", SqlDbType.Char).Value = tcStaffno;
                    cuscommand2.UpdateCommand.Parameters.Add("@tcsocex", SqlDbType.Int).Value = tlsoc;
                    cuscommand2.UpdateCommand.Parameters.Add("@tltxexemt", SqlDbType.Bit).Value = tltax;
                    cuscommand2.UpdateCommand.Parameters.Add("@tnnewsal", SqlDbType.Decimal).Value = gnBasic;
                    cuscommand2.UpdateCommand.Parameters.Add("@tnoldsal", SqlDbType.Decimal).Value = gnBasic;
                    cuscommand2.UpdateCommand.Parameters.Add("@tcperiod", SqlDbType.Char).Value = gcPeriod;
                    cuscommand2.UpdateCommand.Parameters.Add("@tncompid", SqlDbType.Int).Value = ncompid;
                    cuscommand2.UpdateCommand.ExecuteNonQuery();
                }
                ndConnHandle.Close();

            }
        }


        private void updovt(string tcStaffno)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string cquery = "select nstaffid,rothrs,rotpay,hothrs,hotpay from ovtime where cperiod=@tcPeriod and staffno=@tStaffNo and compid = @tncompid";

                DataTable ovtview = new DataTable();
                SqlDataAdapter cuscommand = new SqlDataAdapter();
                cuscommand.SelectCommand = new SqlCommand(cquery, ndConnHandle);

                cuscommand.SelectCommand.Parameters.Add("@tstaffno", SqlDbType.Char).Value = tcStaffno;
                cuscommand.SelectCommand.Parameters.Add("@tcperiod", SqlDbType.Char).Value = gcPeriod;
                cuscommand.SelectCommand.Parameters.Add("@tncompid", SqlDbType.Int).Value = ncompid;
                ndConnHandle.Open();
                cuscommand.SelectCommand.ExecuteNonQuery();
                cuscommand.Fill(ovtview);
                if (ovtview.Rows.Count > 0)
                {
                    foreach (DataRow or in ovtview.Rows)
                    {
                        gnRotHrs = gnRotHrs + Convert.ToDecimal(or["rothrs"]);
                        gnRotPay = gnRotPay + Convert.ToDecimal(or["rotpay"]);
                        gnHotHrs = gnHotHrs + Convert.ToDecimal(or["hothrs"]);
                        gnHotPay = gnHotPay + Convert.ToDecimal(or["hotpay"]);
                    }
                    gnTotOvt = gnRotPay + gnHotPay;
                }
            }
        }

        private void saladv(string tcStaffno)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string cquery = "select nsaladv from saladv where cperiod=@tcPeriod and staffno=@tStaffNo and compid = @tncompid";
                DataTable saladview = new DataTable();
                SqlDataAdapter cuscommand = new SqlDataAdapter();
                cuscommand.SelectCommand = new SqlCommand(cquery, ndConnHandle);
                cuscommand.SelectCommand.Parameters.Add("@tstaffno", SqlDbType.Char).Value = tcStaffno;
                cuscommand.SelectCommand.Parameters.Add("@tcperiod", SqlDbType.Char).Value = gcPeriod;
                cuscommand.SelectCommand.Parameters.Add("@tncompid", SqlDbType.Int).Value = ncompid;
                ndConnHandle.Open();
                cuscommand.SelectCommand.ExecuteNonQuery();
                cuscommand.Fill(saladview);
                if (saladview.Rows.Count > 0)
                {
                    foreach (DataRow cr in saladview.Rows)
                    {
                        gnSalAdv = gnSalAdv + Convert.ToDecimal(cr["nsaladv"]);
                    }
                }
            }
        }

        private void getallowance(string tcStaffno)     //                               &&get allwaonces
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string cquery = "exec tsp_getAllowance @tnCompid, @tStaffNo";

                DataTable alloview = new DataTable();
                SqlDataAdapter cuscommand = new SqlDataAdapter();
                cuscommand.SelectCommand = new SqlCommand(cquery, ndConnHandle);

                cuscommand.SelectCommand.Parameters.Add("@tstaffno", SqlDbType.Char).Value = tcStaffno;
                cuscommand.SelectCommand.Parameters.Add("@tncompid", SqlDbType.Int).Value = ncompid;
                ndConnHandle.Open();
                cuscommand.SelectCommand.ExecuteNonQuery();
                cuscommand.Fill(alloview);
                if (alloview.Rows.Count > 0)
                {
                    foreach (DataRow cr in alloview.Rows)
                    {
                        gnalwtaxy = gnalwtaxy + (Convert.ToBoolean(cr["taxable"]) ? Convert.ToDecimal(cr["amount"]) : 0.00m);
                        gnalwtaxn = gnalwtaxn + (!Convert.ToBoolean(cr["taxable"]) ? Convert.ToDecimal(cr["amount"]) : 0.00m);
                    }
                    gnAlwTotal = gnalwtaxy + gnalwtaxn;
                }
            }
        }

        private void getloans(string tcStaffno)                 //                                  &&get loans
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string cquery = "exec tsp_getLoans @tnCompid, @tStaffNo";

                DataTable loanview = new DataTable();
                SqlDataAdapter cuscommand = new SqlDataAdapter();
                cuscommand.SelectCommand = new SqlCommand(cquery, ndConnHandle);

                cuscommand.SelectCommand.Parameters.Add("@tstaffno", SqlDbType.Char).Value = tcStaffno;
                cuscommand.SelectCommand.Parameters.Add("@tncompid", SqlDbType.Int).Value = ncompid;
                ndConnHandle.Open();
                cuscommand.SelectCommand.ExecuteNonQuery();
                cuscommand.Fill(loanview);
                if (loanview.Rows.Count > 0)
                {
                    foreach (DataRow lr in loanview.Rows)
                    {
                        gnloanTotal = gnloanTotal + (Convert.ToBoolean(lr["npayment"]) ? Convert.ToDecimal(lr["npayment"]) : 0.00m);
                    }
                }
            }
        }

        private void getcontributions(string tcStaffno, decimal tnSal)         //                           &&get other contributions
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string cquery = "exec tsp_getStaffCon @tcPeriod,@tnCompID,@tStaffno";
                DataTable contview = new DataTable();
                SqlDataAdapter cuscommand4 = new SqlDataAdapter();
                cuscommand4.SelectCommand = new SqlCommand(cquery, ndConnHandle);

                cuscommand4.SelectCommand.Parameters.Add("@tstaffno", SqlDbType.Char).Value = tcStaffno;
                cuscommand4.SelectCommand.Parameters.Add("@tcPeriod", SqlDbType.Char).Value = gcPeriod;
                cuscommand4.SelectCommand.Parameters.Add("@tncompid", SqlDbType.Int).Value = ncompid;
                ndConnHandle.Open();
                cuscommand4.SelectCommand.ExecuteNonQuery();
                cuscommand4.Fill(contview);
                if (contview.Rows.Count > 0)
                {
                    foreach (DataRow cr in contview.Rows)
                    {
                        if (Convert.ToDecimal(cr["mployee"]) > 0.00m)
                        {
                            lnEmpDed1 = lnEmpDed1 + tnSal * Convert.ToDecimal(cr["mployee"]) / 100; // && percentage contribution by employee
                        }

                        if (Convert.ToDecimal(cr["mployeepay"]) > 0.00m)                            // && absolute value contribution by employee
                        {
                            lnEmpDed2 = lnEmpDed2 + Convert.ToDecimal(cr["mployeepay"]);
                        }

                        if (Convert.ToDecimal(cr["mployer"]) > 0.00m)
                        {
                            lnEmplDed1 = lnEmplDed1 + tnSal * Convert.ToDecimal(cr["mployer"]) / 100;  // && percentage contribution by employer
                        }

                        if (Convert.ToDecimal(cr["mployerpay"]) > 0.00m)                               // && absolute value contribution by employee
                        {
                            lnEmplDed2 = lnEmplDed2 + Convert.ToDecimal(cr["mployerpay"]);
                        }
                        gnstfcont = lnEmpDed1 + lnEmpDed2;
                        gncmpcont = lnEmplDed1 + lnEmplDed2;
                    }
                }
            }
        }

        private void updatesaldet(string tcStaffno)                 //                               &&Calculate and update salaries
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
//                MessageBox.Show("inside update saldet with period = " + gcPeriod);
                //     execute tsp_getSalDet ?gcPeriod, ?gnCompID, ?gcStaffNo
                string cquery = "exec tsp_getSalDet @tnCompid, @tStaffNo,@tcPeriod";
                DataTable saldetview = new DataTable();
                SqlDataAdapter cuscommand = new SqlDataAdapter();
                cuscommand.SelectCommand = new SqlCommand(cquery, ndConnHandle);
                cuscommand.SelectCommand.Parameters.Add("@tstaffno", SqlDbType.Char).Value = tcStaffno;
                cuscommand.SelectCommand.Parameters.Add("@tcPeriod", SqlDbType.Char).Value = gcPeriod;
                cuscommand.SelectCommand.Parameters.Add("@tncompid", SqlDbType.Int).Value = ncompid;
                ndConnHandle.Open();
                cuscommand.SelectCommand.ExecuteNonQuery();
                cuscommand.Fill(saldetview);
                if (saldetview.Rows.Count > 0)
                {
                    decimal lnovt = Convert.ToDecimal(saldetview.Rows[0]["overtimep"]);
                    decimal lnTaxAble = gnBasic / 12 + gnalwtaxy + lnovt + nbonamt - gnNssfStaffPay;
                    decimal lnTaxAmt = taxCalculation.calcTax("220", lnTaxAble);// KTAXCAL(lnTaxAble) - gnPersonalRelief
                    lnTaxAmt = lnTaxAmt > 0.00m ? lnTaxAmt : 0.00m;

                    string sd = "update saldet set nstfcont=@tgnstfcont,ncompcont=@tgncmpcont,nalwtaxy=@tgnalwtaxy,nalwtaxn=@tgnalwtaxn,nalwtot=@tgnAlwTotal,";
                    sd += "nloantot=@tgnloanTotal,otwk=@tgnRotHrs,otwkpay=@tgnRotPay,otwknhol=@tgnHothrs,otwknholp=@tgnHotPay,overtimep=@tgnTotOvt,ntxblamt=@tlnTaxable,ntotovt=@tlnOvt,";
                    sd += "lprocess=1,nicmtax=@tlnTaxAmt,nsaladv=@tgnSalAdv,nhif=@tgnNhifPay,nstfss=@tgnNssfStaffPay,ncompss=@tgnNssfCompPay where staffno=@tStaffNo and compid=@tnCompid AND ";
                    sd += "cperiod=@tcPeriod";

                    DataTable saldetview1 = new DataTable();
                    SqlDataAdapter cuscommand1 = new SqlDataAdapter();
                    cuscommand1.UpdateCommand = new SqlCommand(sd, ndConnHandle);
                    cuscommand1.UpdateCommand.Parameters.Add("@tstaffno", SqlDbType.Char).Value = tcStaffno;
                    cuscommand1.UpdateCommand.Parameters.Add("@tgnstfcont", SqlDbType.Decimal).Value = gnstfcont;
                    cuscommand1.UpdateCommand.Parameters.Add("@tgncmpcont", SqlDbType.Decimal).Value = gncmpcont;
                    cuscommand1.UpdateCommand.Parameters.Add("@tgnalwtaxy", SqlDbType.Decimal).Value = gnalwtaxy;
                    cuscommand1.UpdateCommand.Parameters.Add("@tgnalwtaxn", SqlDbType.Decimal).Value = gnalwtaxn;
                    cuscommand1.UpdateCommand.Parameters.Add("@tgnAlwTotal", SqlDbType.Decimal).Value = gnAlwTotal;
                    cuscommand1.UpdateCommand.Parameters.Add("@tgnloanTotal", SqlDbType.Decimal).Value = gnloanTotal;
                    cuscommand1.UpdateCommand.Parameters.Add("@tgnRotHrs", SqlDbType.Decimal).Value = gnRotHrs;
                    cuscommand1.UpdateCommand.Parameters.Add("@tgnRotPay", SqlDbType.Decimal).Value = gnRotPay;
                    cuscommand1.UpdateCommand.Parameters.Add("@tgnHothrs", SqlDbType.Decimal).Value = gnHotHrs;
                    cuscommand1.UpdateCommand.Parameters.Add("@tgnHotPay", SqlDbType.Decimal).Value = gnHotPay;
                    cuscommand1.UpdateCommand.Parameters.Add("@tgnTotOvt", SqlDbType.Decimal).Value = gnTotOvt;
                    cuscommand1.UpdateCommand.Parameters.Add("@tlnTaxable", SqlDbType.Decimal).Value = lnTaxAble;
                    cuscommand1.UpdateCommand.Parameters.Add("@tlnOvt", SqlDbType.Decimal).Value = lnovt;
                    cuscommand1.UpdateCommand.Parameters.Add("@tlnTaxAmt", SqlDbType.Decimal).Value = lnTaxAmt;
                    cuscommand1.UpdateCommand.Parameters.Add("@tgnSalAdv", SqlDbType.Decimal).Value = gnSalAdv;
                    cuscommand1.UpdateCommand.Parameters.Add("@tgnNhifPay", SqlDbType.Decimal).Value = 0.00m;
                    cuscommand1.UpdateCommand.Parameters.Add("@tgnNssfStaffPay", SqlDbType.Decimal).Value = 0.00m;
                    cuscommand1.UpdateCommand.Parameters.Add("@tgnNssfCompPay", SqlDbType.Decimal).Value = 0.00m;
                    cuscommand1.UpdateCommand.Parameters.Add("@tcPeriod", SqlDbType.Char).Value = gcPeriod;
                    cuscommand1.UpdateCommand.Parameters.Add("@tncompid", SqlDbType.Int).Value = ncompid;
                    cuscommand1.UpdateCommand.ExecuteNonQuery();
                }
            }
        }

        private void reinitvar()
        {
             glPartSal = false;
            gnPartBasic = gnBasic = gnRotHrs = gnRotPay = gnHotHrs = gnHotPay = gnAlwTotal = gnalwtaxy = gnalwtaxn = gnNhifPay = gnTotOvt = 
             gnSalAdv = gnloanTotal = lnEmpDed1 = lnEmpDed2 = lnEmplDed1 = lnEmplDed2 = gnstfcont = gncmpcont = nbonamt = gnNssfStaffPay = 0.00m;
        }
    }
}
