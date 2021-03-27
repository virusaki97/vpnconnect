using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;


namespace VPNClient
{
    public class IPInfo
    {
        public string ip, code;
        public string country = "Unknown";
     
        public IPInfo(string szIP)
        {
            GetCountryByIP(szIP);

        }

        public void LoadFromIP(string szIP)
        {
            GetCountryByIP(szIP);
        }


        private string IPRequestHelper(string url)
        {

            System.Net.HttpWebRequest objRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
            System.Net.HttpWebResponse objResponse = (System.Net.HttpWebResponse)objRequest.GetResponse();

            StreamReader responseStream = new StreamReader(objResponse.GetResponseStream());
            string responseRead = responseStream.ReadToEnd();

            responseStream.Close();
            responseStream.Dispose();

            return responseRead;
        }

        private void GetCountryByIP(string ipAddress)
        {
            string ipResponse = IPRequestHelper("http://ip-api.com/xml/" + ipAddress);

            XmlDocument ipInfoXML = new XmlDocument();
            ipInfoXML.LoadXml(ipResponse);
            XmlNodeList responseXML = ipInfoXML.GetElementsByTagName("query");

            ip = ipAddress;
            country = responseXML.Item(0).ChildNodes[1].InnerText.ToString();
            code = responseXML.Item(0).ChildNodes[2].InnerText.ToString();
        }

    }
}
