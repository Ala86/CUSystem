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
using System.Windows.Forms.DataVisualization.Charting;

namespace TclassLibrary
{
    public partial class dashBoard : Form
    {
        string cs = string.Empty;
        int ncompid = 0;
        string dloca = string.Empty;
        DataTable pieview = new DataTable();

        public dashBoard(string tcCos, int tnCompid, string tcLoca)
        {
            InitializeComponent();
            cs = tcCos;
            ncompid = tnCompid;
            dloca = tcLoca;
        }
        private void dashBoard_Load(object sender, EventArgs e)
        {
         this.Text = dloca + "<< Dash Board >>";
            //        getbudget();
            getStatus();
            Getgender();
            loanCount();
//            getPie();
        }


        private void getPie()
        {
            string sqlgender = "select distinct dgender = case when gender = 1 then  'male'  else 'female' end, dquantity = case when gender = 1 then(select count(gender) ";
            sqlgender += "from cusreg m where m.gender = 1) else (select count(gender) from cusreg m where m.gender = 0) end  from cusreg o where  gender in (1,0)";
            using (SqlConnection dconn = new SqlConnection(cs))
            {
                SqlDataAdapter dag = new SqlDataAdapter(sqlgender, dconn);
                dag.Fill(pieview);
                if (pieview != null && pieview.Rows.Count > 0)
                {
//                    double x = pieview.Rows[0]["dgen"]
                    MessageBox.Show("we are about to show the grapyh");
                    chart2.DataSource = pieview.DefaultView;
//                    gendChart.Series["gender"].Points.AddXY( ((object)"dgender", "dquantity");
                    chart2.Series["gender"].BackHatchStyle = ChartHatchStyle.LargeConfetti;
                    chart2.Series[0].ChartType = SeriesChartType.Pie;
                    chart2.Legends[0].Enabled = true;
                    chart2.ChartAreas[0].Area3DStyle.Enable3D = true;
                    /*
                    //Get the males
                    int[] x = (from p in pieview.AsEnumerable()
                                  orderby p.Field<int>("male") 
                                  select p.Field<int>("male")).ToArray();

                    //Get the females
                    int[] y = (from p in pieview.AsEnumerable()
                                   orderby p.Field<int>("female") 
                                   select p.Field<int>("female")).ToArray();

                    gendChart.Series[0].ChartType = SeriesChartType.Pie;
                    gendChart.Series[0].Points.DataBindXY(x, y);
                    gendChart.Legends[0].Enabled = true;
                    gendChart.ChartAreas[0].Area3DStyle.Enable3D = true;
                    */
                }
            }
        }

    private void Getgender()
        {
            using (SqlConnection dconn = new SqlConnection(cs))
            {
                string sqlgender = "select distinct dgender = case when gender = 1 then  'male'  else 'female' end, dquantity = case when gender = 1 then(select count(gender) ";
                sqlgender += "from cusreg m where m.gender = 1) else (select count(gender) from cusreg m where m.gender = 0) end  from cusreg o where  gender in (1,0)";
                dconn.Open();
                //                DataTable dt = GetData(sqlgender, cs);
                SqlDataAdapter da1 = new SqlDataAdapter(sqlgender, dconn);
                DataTable dt = new DataTable();
                da1.Fill(dt);
                if(dt!=null && dt.Rows.Count>0)
                {

                    //Get the x axis
                    string[] x = (from p in dt.AsEnumerable()
                                  orderby p.Field<string>("dgender") ascending
                                  select p.Field<string>("dgender")).ToArray();

                    //Get the y axis
                    int[] y = (from p in dt.AsEnumerable()
                               orderby p.Field<int>("dquantity") ascending
                               select p.Field<int>("dquantity")).ToArray();

                    chart2.Series[0].ChartType = SeriesChartType.Pie;
                    chart2.Series[0].Points.DataBindXY(x, y);
                    chart2.Legends[0].Enabled = true;
                    chart2.Series["gender"].BackHatchStyle = ChartHatchStyle.LargeConfetti;
                    chart2.ChartAreas[0].Area3DStyle.Enable3D = true;
                }
                dconn.Close();
            }
        }


        private void loanCount()
        {
            using (SqlConnection dconn = new SqlConnection(cs))
            {
                string sqlgender = "exec tsp_loanstatus "+ncompid;
                dconn.Open();
                SqlDataAdapter da1c = new SqlDataAdapter(sqlgender, dconn);
                DataTable dtc = new DataTable();
                da1c.Fill(dtc);
                if (dtc != null && dtc.Rows.Count > 0)
                {
                    
                    //Get the x axis
                    string[] x = (from p in dtc.AsEnumerable()
                                  orderby p.Field<string>("dstatus") ascending
                                  select p.Field<string>("dstatus")).ToArray();

                    //Get the y axis
                    int[] y = (from p in dtc.AsEnumerable()
                               orderby p.Field<int>("dcount") ascending
                               select p.Field<int>("dcount")).ToArray();

                    chart3.Series[0].ChartType = SeriesChartType.Doughnut;
                    chart3.Series[0].Points.DataBindXY(x, y);
                    chart3.Legends[0].Enabled = true;
                    chart3.Series["loancount"].IsValueShownAsLabel = true;
                    chart3.Series["loancount"].BackHatchStyle = ChartHatchStyle.LargeConfetti;
                    chart3.ChartAreas[0].Area3DStyle.Enable3D = true;

  /*                  loanStatusGrid.DataSource = dtc;
                    loanStatusGrid.Series["loanCnt"].XValueMember = "dstatus";
                    loanStatusGrid.Series["loanCnt"].YValueMembers = "dcount";
                    loanStatusGrid.Titles.Add("Loan Count");*/

                }
                dconn.Close();
            }
        }



        private void getStatus()
        {
            using (SqlConnection dconn = new SqlConnection(cs))
            {
                string sqlquery = "exec tsp_loanstatus "+ncompid;
                SqlDataAdapter das = new SqlDataAdapter(sqlquery,dconn);
                DataTable graview = new DataTable();
                das.Fill(graview);
                if(graview !=null && graview.Rows.Count>0)
                {
                    chart1.DataSource = graview;
                    chart1.Series["loanAmt"].XValueMember = "dstatus";
                    chart1.Series["loanAmt"].IsValueShownAsLabel = true;
                    chart1.Series["loanAmt"].YValueMembers = "damt";
                    chart1.Titles.Add("Loan Amounts");
                    //                    chart1.Series[x].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
                    chart1.Series["loanAmt"].ChartType = SeriesChartType.Bar;
        /*            
          //          loanStatusGrid.DataSource = graview;
                    loanStatusGrid.Series["loanCnt"].XValueMember = "dstatus";
                    loanStatusGrid.Series["loanCnt"].YValueMembers = "dcount";
                    loanStatusGrid.Titles.Add("Loan Count");*/
                }
            }
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
