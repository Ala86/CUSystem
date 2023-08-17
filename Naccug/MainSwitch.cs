using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;



namespace WinTcare
{
    public partial class MainSwitch : Form
    {
        #region constructor
        public MainSwitch()
        {
//            Exception ex = new Exception("There was a problem");
  //          string ln = "";

            InitializeComponent();
        }    
        #endregion 
        #region Private Variables
        SerialPort port = new SerialPort();
        SerialPort comm = new SerialPort();
        clsSMS objclsSMS = new clsSMS();
//        MessageCollection objMessageCollection = new MessageCollection();
        ShortMessageCollection objShortMessageCollection = new ShortMessageCollection();
        #endregion
        #region Private Methods

        #region Write StatusBar
        private void WriteStatusBar(string status)
        {
            try
            {
                statusBar1.Text = "Message: " + status;
            }
            catch (Exception)
            {
                
            }
        }
        #endregion
        
        #endregion
        #region Private Events
        private void MainSwitch_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + "<<Main Switch>>";
            try
            {
                #region Display all available COM Ports
                string[] ports = SerialPort.GetPortNames();
             //   bool ln = false;
            // Add all port names to the combo box:
                foreach (string dport in ports)
                {
                   this.cboPortName.Items.Add(dport);
                   this.CommPortList.Items.Add(dport);
  
             /*               comm.PortName = dport;                          //COM21
                            comm.BaudRate = 9600;
                            comm.DataBits = 8;
                            comm.StopBits = StopBits.One;
                            comm.Parity =  Parity.None;
                            comm.ReadTimeout = 300;
                            comm.WriteTimeout = 300;
                            comm.Encoding = Encoding.GetEncoding ("iso-8859-1");
                            comm.Open();
                            comm.DtrEnable = true;
                            comm.RtsEnable = true; 
                    commok=comm.IsOpen;
                        if (commok == true) 
                        {
                            MessageBox.Show("Connected Successfully to Port = " + dport);
                        }
                        else;
                        {
                            MessageBox.Show("Not Connected Successfully to Port = " + dport);
                        }
              /*     modemok = port.DsrHolding ;*/
                }
                #endregion
                this.btnDisconn.Enabled = false;
            }
            catch ( Exception)
            {
//               ErrorLog(Message.Create); 
            }
        }    
  /*      private void btnDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                this.gboPortSettings.Enabled = true;
                objclsSMS.ClosePort(this.port);
                this.lblConnectionStatus.Text = "Not Connected";
                this.btnDisconn.Enabled = false;

            }
            catch (Exception ex)
            {
                ErrorLog(ex.Message);
            }
        }*/
        private void btnReadSMS_Click(object sender, EventArgs e)
        {
           try
            {
                //count SMS 
               int uCountSMS = objclsSMS.CountSMSmessages(this.port);
             string  mycount = "The Number of messages = "+Convert.ToString(uCountSMS);  
                WriteStatusBar(mycount);
                if (uCountSMS > 0)
                {
               
                    #region Command
                    string strCommand = "AT+CMGL=\"ALL\"";

                    if (this.rbReadAll.Checked)
                    {
                        strCommand = "AT+CMGL=\"ALL\"";
                    }
                    else if (this.rbReadUnRead.Checked)
                    {
                        strCommand = "AT+CMGL=\"REC UNREAD\"";
                    }
                    else if (this.rbReadStoreSent.Checked)
                    {
                        strCommand = "AT+CMGL=\"STO SENT\"";
                    }
                    else if (this.rbReadStoreUnSent.Checked)
                    {
                        strCommand = "AT+CMGL=\"STO UNSENT\"";
                    }
                    #endregion

                    // If SMS exist then read SMS
                    #region Read SMS
                    //.............................................. Read all SMS ....................................................
                    objShortMessageCollection = objclsSMS.ReadSMS(this.port, strCommand);
                    foreach (ShortMessage msg in objShortMessageCollection)
                    {

                        ListViewItem item = new ListViewItem(new string[] { msg.Index, msg.Sent, msg.Sender, msg.Message });
                        item.Tag = msg;
                        lvwMessages.Items.Add(item);

                    }
                    #endregion

                }
                else
                {
                    lvwMessages.Clear();
                    //MessageBox.Show("There is no message in SIM");
                    this.statusBar1.Text = "There is no message in SIM, something is not happening at port ";
                    
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex.Message);
            }
        }    
        private void btnDeleteSMS_Click(object sender, EventArgs e)
        {
            try
            {
                //Count SMS 
                int uCountSMS = objclsSMS.CountSMSmessages(this.port);
                if (uCountSMS > 0)
                {
                    DialogResult dr = MessageBox.Show("Are u sure u want to delete the SMS?", "Delete confirmation", MessageBoxButtons.YesNo);

                    if (dr.ToString() == "Yes")
                    {
                        #region Delete SMS

                        if (this.rbDeleteAllSMS.Checked)
                        {                           
                            //...............................................Delete all SMS ....................................................

                            #region Delete all SMS
                            string strCommand = "AT+CMGD=1,4";
                            if (objclsSMS.DeleteMsg(this.port, strCommand))
                            {
                                //MessageBox.Show("Messages has deleted successfuly ");
                                this.statusBar1.Text = "Messages has deleted successfuly";
                            }
                            else
                            {
                                //MessageBox.Show("Failed to delete messages ");
                                this.statusBar1.Text = "Failed to delete messages";
                            }
                            #endregion
                            
                        }
                        else if (this.rbDeleteReadSMS.Checked)
                        {                          
                            //...............................................Delete Read SMS ....................................................

                            #region Delete Read SMS
                            string strCommand = "AT+CMGD=1,3";
                            if (objclsSMS.DeleteMsg(this.port, strCommand))
                            {
                                //MessageBox.Show("Messages has deleted successfuly");
                                this.statusBar1.Text = "Messages has deleted successfuly";
                            }
                            else
                            {
                                //MessageBox.Show("Failed to delete messages ");
                                this.statusBar1.Text = "Failed to delete messages";
                            }
                            #endregion

                        }

                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex.Message);
            }

        }
        private void btnCountSMS_Click(object sender, EventArgs e)
        {
            try
            {
                //Count SMS
                int uCountSMS = objclsSMS.CountSMSmessages(this.port);
                this.txtCountSMS.Text = uCountSMS.ToString();
            }
            catch (Exception ex)
            {
                ErrorLog(ex.Message);
            }
        }
        #endregion
        #region Error Log
        public void ErrorLog(string Message)
        {
            StreamWriter sw = null;

            try
            {
                WriteStatusBar(Message);

                string sLogFormat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";
                //string sPathName = @"E:\";
                string sPathName = @"SMSapplicationErrorLog_";

                string sYear = DateTime.Now.Year.ToString();
                string sMonth = DateTime.Now.Month.ToString();
                string sDay = DateTime.Now.Day.ToString();

                string sErrorTime = sDay + "-" + sMonth + "-" + sYear;

                sw = new StreamWriter(sPathName + sErrorTime + ".txt", true);

                sw.WriteLine(sLogFormat + Message);
                sw.Flush();

            }
            catch (Exception )
            {
                //ErrorLog(ex.ToString());
            }
            finally
            {
                if (sw != null)
                {
                    sw.Dispose();
                    sw.Close();
                }
            }
            
        }
        #endregion 
        #region Switch Selection
        private void SwitchItems_AfterSelect(object sender, TreeViewEventArgs e)
        {
              SelectSwitch(SwitchItems.SelectedNode.Name); 
          }
         public void SelectSwitch(string SwitchNode) {
             switch (SwitchNode) {
                 case "SmsNode":
                     SmsSwitch.Visible = true;
                     ShariaPage.Visible = false;
                     PaymentDevices.Visible = false;
                     ScanAndBars.Visible = false;
                     OrderManagement.Visible = false;
                     GpsServices.Visible = false;
                     DcmManagement.Visible = false;
                     LogManagement.Visible = false; 
                     BioManagement.Visible = false;
                     statusBar1.Text = "SMS Switch Configuration";
                     break;
                 case "PmtNode":
                     PaymentDevices.Visible = true;
                     ShariaPage.Visible = false;
                    SmsSwitch.Visible = false;
                     ScanAndBars.Visible = false;
                     OrderManagement.Visible = false;
                     GpsServices.Visible = false;
                     DcmManagement.Visible = false;
                     LogManagement.Visible = false; 
                     BioManagement.Visible = false;
                     statusBar1.Text = "Payment Device Configuration";
                   break;
                 case "ScanBarNode":
                     ScanAndBars.Visible = true;
                     ShariaPage.Visible = false;
                     SmsSwitch.Visible = false;
                     PaymentDevices.Visible = false;
                     OrderManagement.Visible = false;
                     GpsServices.Visible = false;
                     DcmManagement.Visible = false;
                     LogManagement.Visible = false; 
                     BioManagement.Visible = false;
                     statusBar1.Text = "Scanners and Barcode Readers Configuration";
                     break;
                 case "ShariaNode":
                     ShariaPage.Visible = true;
                     SmsSwitch.Visible = false;
                     ScanAndBars.Visible = false;
                     PaymentDevices.Visible = false;
                     OrderManagement.Visible = false;
                     GpsServices.Visible = false;
                     DcmManagement.Visible = false;
                     LogManagement.Visible = false; 
                     BioManagement.Visible = false;
                     statusBar1.Text = "Islamic Banking Configuration";
                     break;
                 case "OrderNode":
                     OrderManagement.Visible = true;
                     ShariaPage.Visible = false;
                     SmsSwitch.Visible = false;
                     ScanAndBars.Visible = false;
                     GpsServices.Visible = false;
                     DcmManagement.Visible = false;
                     LogManagement.Visible = false; 
                     BioManagement.Visible = false;
                     statusBar1.Text = "Order Management Configuration";
                     break;
                 case "CdmNode":
                     DcmManagement.Visible = true;
                     OrderManagement.Visible = false;
                     ShariaPage.Visible = false;
                     SmsSwitch.Visible = false;
                     ScanAndBars.Visible = false;
                     GpsServices.Visible = false;
                     LogManagement.Visible = false; 
                     BioManagement.Visible = false;
                     statusBar1.Text = "Connected Device Management";
                     break;
                 case "SysNode":
                     LogManagement.Visible = true; 
                     DcmManagement.Visible = false;
                     OrderManagement.Visible = false;
                     ShariaPage.Visible = false;
                     SmsSwitch.Visible = false;
                     ScanAndBars.Visible = false;
                     GpsServices.Visible = false;
                     BioManagement.Visible = false;
                     statusBar1.Text = "Sytem Logs and Audit Trail";
                     break;
                 case "BioNode":
                     BioManagement.Visible = true;
                     LogManagement.Visible = false; 
                     DcmManagement.Visible = false;
                     OrderManagement.Visible = false;
                     ShariaPage.Visible = false;
                     SmsSwitch.Visible = false;
                     ScanAndBars.Visible = false;
                     GpsServices.Visible = false;
                     statusBar1.Text = "Biometric Device Configuration";
                     break;
                 case "GpsNode":
                     GpsServices.Visible = true;
                     LogManagement.Visible = false; 
                     OrderManagement.Visible = false;
                     ShariaPage.Visible = false;
                     SmsSwitch.Visible = false;
                     ScanAndBars.Visible = false;
                     DcmManagement.Visible = false;
                     BioManagement.Visible = false;
                     statusBar1.Text = "Location Services Configuration";
                     break;
                 default:
          //           SmsSwitch.Visible = true;
            //         ShariaPage.Visible = false;
                     break;
             }
         }
        #endregion
        private void btnConnect_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("In the click of connect");
            {
                try
                {
                    //Open communication port 
                    this.port = objclsSMS.OpenPort(this.cboPortName.Text, Convert.ToInt32(this.cboBaudRate.Text), Convert.ToInt32(this.cboDataBits.Text), Convert.ToInt32(this.txtReadTimeOut.Text), Convert.ToInt32(this.txtWriteTimeOut.Text));
                    if (this.port != null)
                    {
                        this.gboPortSettings.Enabled = false;
                        this.statusBar1.Text = "Modem is connected at PORT " + this.cboPortName.Text;
                        this.lblConnectionStatus.Text = "Connected at " + this.cboPortName.Text;
                        this.btnDisconn.Enabled = true;
                        this.btnConnect.Enabled = false;
                    }

                    else
                    {
                        this.statusBar1.Text = "Invalid port settings";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An exception has occured, which we will find out");
                    ErrorLog(ex.Message);
                }

            }
        }
        private void tstButton_Click(object sender, EventArgs e)
        {
            if (objclsSMS.sendMsg(this.port,"0708083651","We are using the test button"))
            {
                //MessageBox.Show("Message has sent successfully");
                this.statusBar1.Text = "Message sent successful";
            }
            else
            {
                //MessageBox.Show("Failed to send message");
                this.statusBar1.Text = "Failed to send message";
            }
  
        }
        private void statusBar1_PanelClick(object sender, StatusBarPanelClickEventArgs e)
        {

        }
        private void btnSMSSend_Click(object sender, EventArgs e)
         {
             {
             //    MessageBox.Show("second send button");
                 //.............................................. Send SMS ....................................................
                 try
                 {

                     if (objclsSMS.sendMsg(this.port, "0708083651", this.txtMessage.Text))
                     {
                         //MessageBox.Show("Message has sent successfully");
                         this.statusBar1.Text = "Message sent successful";
                     }
                     else
                     {
                            this.statusBar1.Text = "Failed to send message";
                     }

                 }
                 catch (Exception ex)
                 {
                      ErrorLog(ex.Message);
                 }
             }

         }
         #region Communication port disconnection
         private void btnDisconn_Click(object sender, EventArgs e)
         {
             try
             {
                 this.gboPortSettings.Enabled = true;
                 objclsSMS.ClosePort(this.port);
                 this.lblConnectionStatus.Text = "Not Connected";
                 this.btnDisconn.Enabled = false;
                 this.btnConnect.Enabled = true;
                 this.statusBar1.Text = "Modem Disconnected";

             }
             catch (Exception ex)
             {
                 ErrorLog(ex.Message);
             }
         }
        #endregion 
         // After detecting a Read() timeout, call here
         // The trick turned out to be actually calling Close() on the port even though it's already dead.
         public void TrySafeDisconnect()
         {
             GC.SuppressFinalize(port.BaseStream);
             try { port.Close(); }
             catch { }
             port = null;
         }

         private void OrderCompConfig_Click(object sender, EventArgs e)
         {

         }

         private void label32_Click(object sender, EventArgs e)
         {

         }

         private void checkBox1_CheckedChanged(object sender, EventArgs e)
         {
             if (this.checkBox1.Checked)
             {
                 this.SmsUserDelCheck.Checked=true;
                 this.SmsUserReadCheck.Checked=true;
                 this.SmsUserAmdCheck.Checked = true;
                 this.SmsUserWritCheck.Checked = true;
             }
             else if(!this.checkBox1.Checked)
             {
       //          this.checkBox2.Checked = false;
         //        this.SmsUserReadCheck.Checked = false;
           //      this.checkBox4.Checked = false;
             //    this.checkBox5.Checked = false;
             }
         }

         private void CheckBoxvalidation(object sender, EventArgs e)
         {
             if (!this.SmsUserReadCheck.Checked || !this.SmsUserWritCheck.Checked || !this.SmsUserDelCheck.Checked || !this.SmsUserAmdCheck.Checked)
             {
                 this.checkBox1.Checked = false;
             }
         }

         private void SmsUserReadCheck_CheckedChanged(object sender, EventArgs e)
         {

         }

         private void gboPortSettings_Enter(object sender, EventArgs e)
         {

         }

         private void Check_ports_Click(object sender, EventArgs e)
         {
             #region Display all available COM Ports
             string[] nports = SerialPort.GetPortNames();
             bool modemok = false;
             // Add all port names to the combo box:
             foreach (string dport in nports)
             {
                 this.cboPortName.Items.Add(dport);
                 this.CommPortList.Items.Add(dport);
                 modemok = this.port.DsrHolding;
                 if (modemok == true)
                 {
                     MessageBox.Show("Connected Successfully to Port" + dport, "Connection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                 }
                 else
                 {
                     MessageBox.Show("Something is not right to Port" + dport, "Connection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                 }
             }
             #endregion

         }

         private void Check_ports_Click_1(object sender, EventArgs e)
         {

         }

         private void statusBar1_PanelClick_1(object sender, StatusBarPanelClickEventArgs e)
         {

         }

         private void tstButton_Click_1(object sender, EventArgs e)
         {

         }

         private void txtMessage_TextChanged(object sender, EventArgs e)
         {

         }

         private void txtSIM_TextChanged(object sender, EventArgs e)
         {

         }
        
      }
}
