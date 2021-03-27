using DotRas;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace VPNClient
{
    public partial class Form1 : Form
    {
        List<VPNServer> VPNList;
        VPNConnection vpncon;
        Random random;

        bool Connected = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            VPNList = VPNSettings.LoadVPNEntries(VPNSettings.VPNListPath);
            vpncon = new VPNConnection();
            random = new Random();
            button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            UpdateElements();
          
        }

        private void reloadElements()
        {
            label3.Text = string.Empty;
            vpncon.Disconnect();
            button1.BackgroundImage = Properties.Resources.offbutton;
            Connected = false;
            UpdateElements();

        }

        private void UpdateElements()
        {
         
            comboBox1.Items.Clear();
            for( int i = 0; i < VPNList.Count; i++)
            {
               
                var tmp = VPNList[i].ipinfo;

                if (comboBox1.FindString(tmp.country) == -1)
                {               
                    comboBox1.Items.Add(new FlagCountryItem(tmp.code, tmp.country));
                }
            }
         
            comboBox1.SelectedIndex = 0;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            vpncon.Disconnect();
        }

        private void ComboBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 ) return;

     
            Color foreColor = e.ForeColor;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            if (e.State.HasFlag(DrawItemState.Focus) && !e.State.HasFlag(DrawItemState.ComboBoxEdit))
            {
                e.DrawBackground();
                e.DrawFocusRectangle();
            }
            else
            {
                using (Brush backgbrush = new SolidBrush(Color.WhiteSmoke))
                {
                    e.Graphics.FillRectangle(backgbrush, e.Bounds);
                    foreColor = Color.Black;
                }
            }
            using (Brush textbrush = new SolidBrush(foreColor))
            {
                e.Graphics.DrawString(comboBox1.Items[e.Index].ToString(),
                                      e.Font, textbrush, e.Bounds.Height + 10, e.Bounds.Y,
                                      StringFormat.GenericTypographic);
            }
            var item = (FlagCountryItem) comboBox1.Items[e.Index];

            e.Graphics.DrawImage(VPNSettings.GetFlag(item.code),
                                 new Rectangle(e.Bounds.Location,
                                 new Size(e.Bounds.Height - 2, e.Bounds.Height - 2)));
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if(!Connected)
            {
                label3.Text = "Connecting to VPN server...";
                var comboitem = (FlagCountryItem)comboBox1.SelectedItem;
                List<VPNServer> tmplist = new List<VPNServer>(VPNList.FindAll(c => c.ipinfo.code == comboitem.code));

                int id = random.Next(tmplist.Count);

                try
                {
                    vpncon.Connect(tmplist[id]);
                }catch(Exception)
                {
                    label3.Text = "Error connecting to VPN server.";
                }

                button1.BackgroundImage = Properties.Resources.onbutton;
                Connected = true;
                label3.Text = "Connected to " + vpncon.current_server.ipinfo.ip;
            }
            else
            {
                label3.Text = string.Empty;
                vpncon.Disconnect();
                button1.BackgroundImage = Properties.Resources.offbutton;
                Connected = false;
            }
            if (Connected) comboBox1.Enabled = false; else comboBox1.Enabled = true;

        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ResetDefaultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            File.Delete(VPNSettings.VPNListPath);
            VPNList = VPNSettings.LoadVPNEntries(VPNSettings.VPNListPath);
            reloadElements();
            MessageBox.Show("VPN List Configuration has been reset to default.", "Configuration", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void LoadVPNListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "txt Files (*.txt)|*.txt";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var tmplist = VPNSettings.LoadVPNEntries(openFileDialog.FileName);
                    var count = tmplist.Count;

                    if(count > 0)
                    {
                        VPNList = tmplist;
                        reloadElements();
                    }
                    string tmpmsg = string.Format("Loaded {0} VPN Entries from file!", count);
                    MessageBox.Show(tmpmsg, "VPN List", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Don't step on my private property nigger!", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
