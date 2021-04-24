using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.Xml;
using System.Threading;
using System.IO;

namespace Ntpvize
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        void uyarisesi()
        {
            Console.Beep(874, 400);
            Console.Beep(340, 400);
            Console.Beep(274, 400);
        }



        string yol = @"veriler.txt";
        void dosyayenile() // yeni veri geldiğinde eski veriler temizlenip yenileri alındı
        {
            if (System.IO.File.Exists(yol))
            {
                {
                    System.IO.File.Delete(yol);
                    FileStream fs = new FileStream(yol, FileMode.Create, FileAccess.ReadWrite);
                    fs.Close();
                }
            }
            else
            {
                FileStream fs = new FileStream(yol, FileMode.Create, FileAccess.ReadWrite);
                fs.Close();
            }
        }
        void veriekle(string veri)
        {
            string dosya_yolu = yol;
            StreamReader sr = new StreamReader(dosya_yolu);
            string eskiveri = sr.ReadToEnd();
            sr.Close();
            FileStream fs = new FileStream(dosya_yolu, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(eskiveri + veri);
            sw.Flush();
            sw.Close();
            fs.Close();
        }


        void veriguncelle()
        {
            XmlTextReader xmloku;
            int i = 0;
            bool item = false;
            while (true)
            {
                xmloku = new XmlTextReader("https://www.webtekno.com/rss.xml");
                i = 0;
                item = false;
                while (xmloku.Read())
                {

                    if (xmloku.Name == "item")
                    {
                        item = true;
                    }
                    if (item == false)
                    {
                        continue;
                    }
                    i++;
                    if (xmloku.Name == "id")
                    {

                        if (i == 3)
                        {
                            if (Convert.ToInt32(xmloku.ReadString()) != enguncelveriid)
                            {
                                uyarisesi();
                                listBox1.Items.Clear();
                                thread1 = new Thread(verileriyaz);
                                thread1.Start();
                                Thread.Sleep(20000);
                            }
                            else
                            {
                                Thread.Sleep(1000);
                                
                            }
                        }
                    }
                }
                Thread.Sleep(20000);
            }
        }

        int enguncelveriid = 0;
        void verileriyaz()
        {
          
            XmlTextReader xmloku = new XmlTextReader("https://www.webtekno.com/rss.xml");
            string veriler = "";
            bool item = false;
            int i = 0;
            int calis = 0;
            while (xmloku.Read())
            {
                if (xmloku.Name == "item")
                {
                    item = true;
                }
                if (item == false)
                {
                    continue;
                }
                i++;
                if (xmloku.Name == "id")
                {
                    if (i == 3)
                    {
                        enguncelveriid = Convert.ToInt32(xmloku.ReadString());
                    }
                    veriler += xmloku.ReadString() + Environment.NewLine;
                }
                if (xmloku.Name == "title")
                { 
                    veriler += xmloku.ReadString() + Environment.NewLine;
                }
                if (xmloku.Name == "description")
                {
                    veriler += xmloku.ReadString() + Environment.NewLine;
                }
                veriekle(veriler);
             //   listBox1.Items.Add(veriler); //Thread Kullandığımız için formla etkileşime giremedik
                veriler = "";
            }
            calis++;
            if (calis == 1)
            {
                thread2 = new Thread(veriguncelle);
                thread2.Start();
            }

        }
     
          Thread thread1,thread2;
        private void button1_Click(object sender, EventArgs e)
        {
            thread1 = new Thread(verileriyaz);
            thread1.Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                thread1.Abort();
                thread2.Abort();
            }
            catch (Exception)
            {

              
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dosyayenile();
        }
    }
}
