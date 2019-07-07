using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using RailSiteDataGrabber.rwmms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RailSiteDataGrabber
{
    public partial class frm_rwmms : Form
    {
        public frm_rwmms()
        {
            InitializeComponent();
        }

        private void frm_rwmms_Load(object sender, EventArgs e)
        {
            this.treeView1.ExpandAll();
        }
        private void btn_getInfo_Click(object sender, EventArgs e)
        {
            
            string str = GetSha256Hash("SubscriberStatus" + "ShahreKalameh09105340027");
            using (webpageWagonRepairCurrent pg = new webpageWagonRepairCurrent("http://rwmms.rai.ir/CurrentRepair1.aspx"))
            {
                if (pg.fnc_load())
                {
                    if (pg.fnc_save(true, false, false))
                    {
                        MessageBox.Show("Saved!!!");
                    }
                }
            }
        }

        public static string GetSha256Hash(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            SHA256Managed hashstring = new SHA256Managed();

            byte[] hash = hashstring.ComputeHash(bytes);
            string hashString = string.Empty;
            foreach (byte x in hash)
            {
                hashString += String.Format("{0:x2}", x);
            }
            return hashString;
        }
        public object fnc_getColumnValue(object value, string prp_regexPattern, int prp_matchGroupIndex)
        {

            if (!string.IsNullOrEmpty(prp_regexPattern) && value != null && value != Convert.DBNull && !string.IsNullOrEmpty(value.ToString()))
            {
                string valueStr = value.ToString().Replace("&nbsp;", " ");
                Match mtch = Regex.Match(value.ToString(), prp_regexPattern);
                if (mtch.Success
                    && mtch.Groups.Count >= prp_matchGroupIndex)
                {
                    return mtch.Groups[prp_matchGroupIndex].Value;
                }
            }


            return value;
        }
    }

    public class temp
    {
        public Type prp_type { get; set; }

    }
    public class cls
    {
        public bool fnc_save()
        {
            return true;
        }
    }

    public class clsChild : cls
    {
        public int prp_id { get; set; }
        public clsChild(int id)
        {
            this.prp_id = id;
        }
        public bool fnc_save()
        {
            return false;
        }
    }
}
