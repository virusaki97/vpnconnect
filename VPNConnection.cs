using DotRas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VPNClient
{
    public class VPNConnection
    {
        public VPNServer current_server;

        public VPNConnection(){}

        public VPNConnection(VPNServer server)
        {
            Connect(server);
        }

        public bool Connect(VPNServer server)
        {
            Disconnect();
            current_server = server;
            try
            {
                using (RasPhoneBook phonebook = new RasPhoneBook())
                {             
                    phonebook.Open(VPNSettings.VPNRasPhoneBook);

                    string name = VPNSettings.ConnectionEntry;

                    var devices = RasDevice.GetDevices();
                    RasDevice device = null;

                    foreach (RasDevice modem in devices)
                    {
                        if (modem.Name.ToLower().Contains("(pptp)"))
                        {
                            device = modem;
                            break;
                        }
                    }

                    RasEntry entry = RasEntry.CreateVpnEntry(name, server.ipinfo.ip, RasVpnStrategy.Default, device);

                    phonebook.Entries.Clear();
                    phonebook.Entries.Add(entry);      
                }

                RasDialer dialer = new RasDialer();
                dialer.EntryName = VPNSettings.ConnectionEntry;
                dialer.PhoneBookPath = VPNSettings.VPNRasPhoneBook;
                dialer.Credentials = server.credentials;

                dialer.DialAsync();
                

                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return false;
            }
        }

        public void Disconnect()
        {
            var conn = RasConnection.GetActiveConnections().Where(c => c.EntryName == VPNSettings.ConnectionEntry).FirstOrDefault();
            if (conn != null)
            {
                conn.HangUp();
            }
        }
    }
}
