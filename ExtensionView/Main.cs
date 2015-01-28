using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;

namespace ExtensionView
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            listView1.AllowDrop = true;
        }
        string path = null;
        int i,j; // i=FileCount j=
        string extension = "";
        string pFilename,filedate,LastAccess = "";
        string filename = string.Empty;
        string file2,checksum = "";
        string subpath = "";
        int z = 0;
        int count = 0; //시그니처 갯수 데이터 변수
        int count1 = 0; //시그니처 갯수 체크데이터와 비교하는 변수
        int tmp = 0; // 임시적으로 카운트하는 변수
        int x = 0;

        public class share
        {
            public static string url = "http://spark.woobi.co.kr/ExtensionData.txt";
            public static string[] Save_Data = new string[100];
        }

        //File Get Size Function is OpenSource
        private string GetFileSize(double byteCount)
        {
            string size = "0 Bytes";
            if (byteCount >= 1073741824.0)
                size = String.Format("{0:##.##}", byteCount / 1073741824.0) + " GB";
            else if (byteCount >= 1048576.0)
                size = String.Format("{0:##.##}", byteCount / 1048576.0) + " MB";
            else if (byteCount >= 1024.0)
                size = String.Format("{0:##.##}", byteCount / 1024.0) + " KB";
            else if (byteCount > 0 && byteCount < 1024.0)
                size = byteCount.ToString() + " Bytes";

            return size;
        }

        private void Main_Load(object sender, EventArgs e)
        {
 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            About about1 = new About();
            about1.Show();
        }

        private void listView1_DragDrop(object sender, DragEventArgs e)
        {
            label1.Hide();
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string get in files)
            {
                if (Directory.Exists(get))
                {
                    path = get;
                }
                else
                {
                    MessageBox.Show("폴더를 드래그하십시오.","Extension View");
                }
            }
            label1.Text = path;
            try
            {
                string[] fl = Directory.GetFiles(path);


                listView1.Items.Clear();
                j = 1;
                foreach (string file in fl)
                {
                    try
                    {
                        checksum = "";
                        subpath = file;
                        FileInfo filecheck = new FileInfo(subpath);
                        extension = filecheck.Extension;
                        filedate = filecheck.CreationTime.ToString();
                        LastAccess = filecheck.LastAccessTime.ToString();
                        filename = subpath.Substring(file.LastIndexOf('\\') + 1);
                        pFilename = filename.Substring(0, filename.LastIndexOf('.'));
                        // extension = file.Substring(file.LastIndexOf('.'));
                    }
                    catch (ArgumentOutOfRangeException o)
                    {
                        if (MessageBox.Show("폴더 내 파일들중 확장자가 없는 파일이 발견되었습니다. 헤더체크를 하시겠습니까? \nFileCheck : " + subpath + "", "Extension View", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                                try
                                {
                                    tmp = 0;
                                    z = 0;
                                    HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(share.url);
                                    HttpWebResponse hwsp = (HttpWebResponse)hwr.GetResponse();
                                    StreamReader sr = new StreamReader(hwsp.GetResponseStream(), Encoding.Default);
                                    BinaryReader br = new BinaryReader(File.OpenRead(subpath));

                                        while (sr.Peek() > -1)
                                        {
                                            string Rev_Data = sr.ReadLine();
                                           // MessageBox.Show(Rev_Data);
                                            share.Save_Data[z] = Rev_Data;
                                            z++;
                                        }
                                       // MessageBox.Show(z.ToString());
                                        share.Save_Data[z + 1] = " ";

                                        for (x = 0; x <= z; x++)
                                        {
                                            br.BaseStream.Position = 0;
                                            count1 = 0;
                                            count = 0;
                                           // MessageBox.Show(x.ToString());
                                          //  MessageBox.Show(share.Save_Data[x]);
                                          //  MessageBox.Show("시그니처 카운트 : " + count.ToString());
                                            string[] Ex_Def = share.Save_Data[x].Split(' ');

                                            /*
                                             * 시그니처 갯수 체크 
                                             */

                                            foreach (string ct in Ex_Def)
                                            {
                                                if (ct == "=")
                                                    break;

                                                count++;
                                            }
                                            string Extension = Ex_Def[count + 1]; //서버 확장자 데이터 보관 변수


                                            /*
                                             * 서버데이터와 파일 바이너리 오프셋 비교
                                             */

                                            foreach (string sh in Ex_Def)
                                            {
                                                //int pp = 0;
                                                //pp++;
                                                if (sh != "=")
                                                {
                                                    string b1 = ((byte)br.ReadByte()).ToString("X2");
                                                    if (b1 == sh)
                                                    {
                                                        count1++;
                                                        if (count1 == count)
                                                        {
                                                            br.Close();
                                                            file2 = Path.ChangeExtension(subpath, Extension);
                                                            File.Move(subpath, file2);
                                                            checksum = "O";
                                                            subpath = file2;
                                                            br.BaseStream.Position = 0;
                                                            break;
                                                        }
                                                    } //if
                                                }
                                            } //foreach문
                                            tmp++;
                                          //  MessageBox.Show("tmp: " + tmp.ToString());
                                            if (tmp == z)
                                            {
                                               // MessageBox.Show("Break!!!");
                                                break;
                                            }
                                        } //for문
                                        tmp = 0;
                                        br.Close();
                                        file2 = Path.ChangeExtension(subpath, " ");
                                        File.Move(subpath, file2);
                                        checksum = "X";
                                        subpath = file2;
                                    }
                                catch
                                {

                                }
                        }
                    }   
                    FileInfo filecheck2 = new FileInfo(subpath);
                    extension = filecheck2.Extension;
                    filename = subpath.Substring(file.LastIndexOf('\\') + 1);
                    pFilename = filename.Substring(0, filename.LastIndexOf('.'));
                    string strFileSize = GetFileSize(filecheck2.Length);
                    ListViewItem item = new ListViewItem("" + j);
                    item.SubItems.Add(pFilename); //FileName
                    item.SubItems.Add(extension); //Extension
                    item.SubItems.Add(strFileSize); // FileSize
                    item.SubItems.Add(filedate); // FileCreationDate
                    item.SubItems.Add(LastAccess); // File Last Access Date
                    item.SubItems.Add(checksum); // Hd Check
                    listView1.Items.Add(item);
                    j++;
                }
                i = listView1.Items.Count;

                label2.Text = "파일갯수 : " + i;
            }
            catch { }

        }

        private void listView1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void listView1_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.Cancel = true;
            e.NewWidth = listView1.Columns[e.ColumnIndex].Width;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AdvancedMode adm = new AdvancedMode();
            adm.Show();
        }
    }
}
