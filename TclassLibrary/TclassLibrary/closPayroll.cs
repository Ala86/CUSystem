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
    public partial class closPayroll : Form
    {
        string cs = string.Empty;
        int ncompid = 0;
        string dloca = string.Empty;
        DataTable payview = new DataTable();
        DataTable npayview = new DataTable();
        string gcPeriod = string.Empty;
        string gcNewPeriod = string.Empty;

        public closPayroll(string tcCos, int tnCompid, string tcLoca)
        {
            InitializeComponent();
            cs = tcCos;
            ncompid = tnCompid;
            dloca = tcLoca;
        }

        private void closPayroll_Load(object sender, EventArgs e)
        {
            this.Text = dloca + "<< Payroll Period Closure >>";
            getPeriod();
            getStaff();
        }

        //private void getPeriod()
        //{
        //    using (SqlConnection ndConnHandle = new SqlConnection(cs))
        //    {
        //        payview.Clear();
        //        ndConnHandle.Open();
        //        string dsql1 = "exec tsp_PayrollPeriod " + ncompid;
        //        SqlDataAdapter da1 = new SqlDataAdapter(dsql1, ndConnHandle);
        //        DataTable perview = new DataTable();
        //        da1.Fill(perview);
        //        if (perview.Rows.Count > 0)
        //        {
        //            string monthName = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).ToString("MMMM", CultureInfo.InvariantCulture);
        //            textBox2.Text = monthName + ", " + DateTime.Now.Year;
        //        }
        //        else { MessageBox.Show("Period has not been set, inform IT DEPT immediately"); }
        //    }
        //}

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
                    int dyear = Convert.ToInt16(perview.Rows[0]["cperiod"].ToString().Trim().Substring(2, 4));
                    string monthName = new DateTime(dyear, dmonth, 1).ToString("MMMM", CultureInfo.InvariantCulture);
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
                string dsql = "select staffno from saldet where lprocess = 0 and lposted=0 and compid = @tnCompid and cperiod = @tcperiod"; 
                SqlDataAdapter selcommand = new SqlDataAdapter();
                selcommand.SelectCommand = new SqlCommand(dsql, ndConnHandle);
                selcommand.SelectCommand.Parameters.Add("@tcperiod", SqlDbType.Char).Value = gcPeriod;
                selcommand.SelectCommand.Parameters.Add("@tncompid", SqlDbType.Int).Value = ncompid;
                selcommand.SelectCommand.ExecuteNonQuery();
                selcommand.Fill(npayview);
                if (npayview.Rows.Count >0)  //There is at least 1 unprocessed staff, and posting has not been done cannot close period
                {
                    SaveButton.Enabled = false;
                    SaveButton.BackColor = Color.Gainsboro;
                    MessageBox.Show("Period has not been processed or not posted, Please process period, post payroll before closing");
                }
                else                         //All staff have been processed for this period, we can close 
                {
                    SaveButton.Enabled = true;
                    SaveButton.BackColor = Color.LawnGreen;
                }
            }
        }


        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Do you want to CLOSE this period?   \n Please remember that this process is irreversible, so \n make sure all processes are completed for the current period \n and a backup taken", "Payroll Closure",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                getnewperiod();
                updateSalaryDetails();
                this.Close();
//                getStaff();
            }
        }

        private void getnewperiod()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                gcPeriod = DateTime.Today.Month.ToString().Trim().PadLeft(2, '0') + DateTime.Today.Year.ToString().Trim().PadLeft(4, '0');
                gcNewPeriod = nextprd(cs,ncompid);
                string cquery = "update period set lcurrent=0,lclose=1";
                SqlDataAdapter cuscommand = new SqlDataAdapter();
                cuscommand.UpdateCommand = new SqlCommand(cquery, ndConnHandle);
                ndConnHandle.Open();
                cuscommand.UpdateCommand.ExecuteNonQuery();

                string insquery = "insert into period (cperiod,lcurrent,lclose,compid) values (@tcperiod,1,0,@tncompid)";
                SqlDataAdapter inscommand = new SqlDataAdapter();
                inscommand.InsertCommand = new SqlCommand(insquery, ndConnHandle);
                inscommand.InsertCommand.Parameters.Add("@tcperiod", SqlDbType.Char).Value = gcNewPeriod;
                inscommand.InsertCommand.Parameters.Add("@tncompid", SqlDbType.Int).Value = ncompid;
                inscommand.InsertCommand.ExecuteNonQuery();
                ndConnHandle.Close();
            }
        }

        private void updateSalaryDetails()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                npayview.Clear();
                string salquery = "exec tsp_getActiveStaff @tncompid";
                SqlDataAdapter selcommand = new SqlDataAdapter();
                selcommand.SelectCommand = new SqlCommand(salquery, ndConnHandle);
                selcommand.SelectCommand.Parameters.Add("@tcperiod", SqlDbType.Char).Value = gcPeriod;
                selcommand.SelectCommand.Parameters.Add("@tncompid", SqlDbType.Int).Value = ncompid;
                ndConnHandle.Open();
                selcommand.SelectCommand.ExecuteNonQuery();
                selcommand.Fill(npayview);
                if (npayview.Rows.Count > 0)
                {
                    string insquery1 = "Insert Into saldet (staffno,cperiod,nnewsal,noldsal,lprocess,compid)  ";
                    insquery1 += " values (@tstaffno,@tcperiod,@tnnewsal,@tnoldsal,0,@tncompid)";
                    SqlDataAdapter inscommand1 = new SqlDataAdapter();
                    inscommand1.InsertCommand = new SqlCommand(insquery1, ndConnHandle);
                    foreach (DataRow srow in npayview.Rows)
                    {
                        string tcStaffno = srow["staffno"].ToString().Trim();
                        decimal lnnewsal = Convert.ToDecimal(srow["nbasic"]);
                        decimal lnoldsal = Convert.ToDecimal(srow["nbasic"]);

                        inscommand1.InsertCommand.Parameters.Add("@tstaffno", SqlDbType.VarChar).Value = tcStaffno;
                        inscommand1.InsertCommand.Parameters.Add("@tcperiod", SqlDbType.Char).Value = gcNewPeriod;
                        inscommand1.InsertCommand.Parameters.Add("@tnnewsal", SqlDbType.Decimal).Value = lnnewsal;
                        inscommand1.InsertCommand.Parameters.Add("@tnoldsal", SqlDbType.Decimal).Value = lnoldsal;
                        inscommand1.InsertCommand.Parameters.Add("@tncompid", SqlDbType.Int).Value = ncompid;
                        inscommand1.InsertCommand.ExecuteNonQuery();
                        inscommand1.InsertCommand.Parameters.Clear();
                    }
                    ndConnHandle.Close();
                    MessageBox.Show("Period closed successfully");
                }
            }
        }

        private static string nextprd(string tcs,int tncompid)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(tcs))
            {
                string salquery = "select * from period where lcurrent=1 and lclose=0 and compid = @tnCompid";
                SqlDataAdapter selcommand = new SqlDataAdapter();
                DataTable prdview = new DataTable();
                selcommand.SelectCommand = new SqlCommand(salquery, ndConnHandle);
                selcommand.SelectCommand.Parameters.Add("@tncompid", SqlDbType.Int).Value = tncompid;
                ndConnHandle.Open();
                selcommand.SelectCommand.ExecuteNonQuery();
                selcommand.Fill(prdview);
                ndConnHandle.Close();
                if (prdview.Rows.Count > 0)
                {
                    string lcmonth = prdview.Rows[0]["cperiod"].ToString().Trim().Substring(0, 2);
                    string lcyear = prdview.Rows[0]["cperiod"].ToString().Trim().Substring(2, 4);
                    if (Convert.ToInt16(lcmonth) == 12)//            If Val(lcmonth) = 12
                    {
                        lcmonth = "01";
                        lcyear = (Convert.ToInt16(lcyear) + 1).ToString().Trim().PadLeft(4, '0'); //Padl(Allt(Str(Val(lcyear) + 1)), 4, '0')
                    }
                    else
                    {
                        lcmonth = (Convert.ToInt16(lcmonth) + 1).ToString().Trim().PadLeft(2, '0');// Padl(Allt(Str(Val(lcmonth) + 1)), 2, '0')
                    }
                    return lcmonth + lcyear;
                }
                else
                {
                    string lcmonth = string.Empty;
                    string lcyear = string.Empty;
                    return lcmonth + lcyear;
                }
            }
        }

    }
}

