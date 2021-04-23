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

namespace Ntpvize
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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
                                listBox1.Items.Clear();
                                verileriyaz();
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
                listBox1.Items.Add(veriler);
                veriler = "";
            }
            calis++;
            if (calis == 1)
            {
                veriguncelle();
            }

        }
     

        private void button1_Click(object sender, EventArgs e)
        {
            verileriyaz();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
