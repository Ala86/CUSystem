using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RestSharp;
using Newtonsoft.Json;
using System.Collections;


namespace TclassLibrary
{
    public partial class myLocation : Form
    {
        string cs = string.Empty;
        int ncompid = 0;
        string dloca = string.Empty;

        public myLocation(string tcCos, int tnCompid, string tcLoca)
        {
            InitializeComponent();
            cs = tcCos;
            ncompid = tnCompid;
            dloca = tcLoca;
        }

        private void button1_Click(object sender, EventArgs e)
        {
           IpLocationResult.Text = FetchCurrentIpLocation();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
        /*
        private string FetchCurrentIpLocation()
        {
            string strIpLocation = string.Empty;
            var client = new RestClient("https://ipapi.co/json/");
            var request = new RestRequest()
            {
                Method = Method.GET
            };
            var response = client.Execute(request);
            //            strIpLocation = response.Content;
            var dictionary = JsonConvert.DeserializeObject<IDictionary>(response.Content);
            foreach (var key in dictionary.Keys)
            {
                strIpLocation += key.ToString() + ":" + dictionary[key] + "\r\n";
            }
            //            strIpLocation = IpLocationResult.Text;
            return strIpLocation;
        }
        */
        private string FetchCurrentIpLocation()
        {
            string strIpLocation = string.Empty;
            var client = new RestClient("https://ipapi.co/json/");
            var request = new RestRequest()
            {
                Method = Method.GET
            };
           var response =   client.Execute(request);
            //            strIpLocation = response.Content;
        var dictionary = JsonConvert.DeserializeObject<IDictionary>(response.Content); 
            foreach(var key in dictionary.Keys)
            {
             strIpLocation +=  key.ToString() + ":" + dictionary[key]+"\r\n";
            }
//            strIpLocation = IpLocationResult.Text;
            return strIpLocation;
        }

        private void myLocation_Load(object sender, EventArgs e)
        {

        }
    }
}
