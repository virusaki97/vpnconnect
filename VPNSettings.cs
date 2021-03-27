using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace VPNClient
{
    public static class VPNSettings
    {
        public static string ConnectionEntry = "VPNClient";
        public static string VPNRasPhoneBook = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VPNClient\VPNClient.pbk";
        public static string VPNListPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VPNClient\vpnlist.txt";

        static public Image GetFlag(string code)
        {
            var resourceName = "VPNClient.CountryFlags." + code.ToLower() + ".png";
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
            return Image.FromStream(stream);

        }

        static public List<VPNServer> LoadVPNEntries(string path)
        {
            List<VPNServer> list = new List<VPNServer>();

            if (path.Equals(VPNSettings.VPNListPath) && !File.Exists(path))
            {
                
                    var assembly = Assembly.GetExecutingAssembly();
                    var resourceName = "VPNClient.VPNData.default_list.txt";

                    var stream = assembly.GetManifestResourceStream(resourceName);

                    Directory.CreateDirectory(Path.GetDirectoryName(path));

                    var fileStream = File.Create(path);
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.CopyTo(fileStream);
                    fileStream.Close();
            }
            else
            {
                if (!File.Exists(path))
                    return list;
                else if (!path.Equals(VPNSettings.VPNListPath))
                    File.Copy(path, VPNSettings.VPNListPath, true);
            }

          
            var lines = File.ReadLines(path);

            foreach (var line in lines)
            {
                try
                {
                    list.Add(new VPNServer(line));
                }
                catch (Exception) { }
            }

            return list;
        }
  
    }
}
