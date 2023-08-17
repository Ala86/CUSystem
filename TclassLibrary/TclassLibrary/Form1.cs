using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TclassLibrary
{
    public partial class Form1 : Form
    {
        Parameters tcCaption, tcDbf, tcField, tnCompany, tcModule
Release gcDbf, gcField, gcDesc, gnCompanyID, gcSysModule, gcDComp, gcDmod
Public gcDbf, gcField, gcDesc, gnCompanyID, gcSysModule, gcDComp, gcDmod
gcDesc=''
gnCompanyID=tnCompany
gcSysModule = tcModule
gcDbf=tcDbf
gcField = tcField
This.Icon= gcIconFile
With Thisform
    .definegrid
    .Caption = tcCaption
    .getrdata
    .Refresh
Endwith
        public Form1()
        {
            InitializeComponent();
        }

        private void doform(string cs, string tcCaption, string tcDbf, string tcField, int tnCompany)
        {
            With Thisform

    ln = SQLExec(gnConnHandle, "Select &gcField from &gcDbf where compid=?gnCompID order by &gcField", "Myresults")

    If ln> 0 And Reccount() > 0
         .facgrid.RecordSource = 'MyResults'
         .facgrid.coLUMN1.heADER1.Caption = 'Existing Descriptions'
         .facgrid.coLUMN1.ControlSource = '&gcField'
 
     Endif
     .Refresh
 Endwith
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
