using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net;
using System.IO;

namespace ExtensionView
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        string NowVer = "3.0.1";

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/namegpark");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.facebook.com/exploitforme");
        }

        private void label3_Click(object sender, EventArgs e)
        {
            HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create("http://spark.woobi.co.kr/ExVER.txt");
            HttpWebResponse hwsp = (HttpWebResponse)hwr.GetResponse();
            StreamReader sr = new StreamReader(hwsp.GetResponseStream(), Encoding.Default);
            string ExtensionVer = sr.ReadLine();
            if (ExtensionVer != NowVer)
            {
                MessageBox.Show("업데이트가 필요합니다.\n\n 최신버전 :" + ExtensionVer + "\n 현재버전 :" + NowVer + "\n\n확인을 누르시면 다운로드 페이지로 이동합니다.", "Extension View");
                Process.Start("http://g_park01.blog.me");
            }
            else
                MessageBox.Show("최신 버전입니다 !", "Extension View");
        }
    }
}
