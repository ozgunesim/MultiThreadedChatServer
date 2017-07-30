using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;


namespace MultiThreadChatServer
{
    public partial class Form1 : Form
    {
        Thread th;
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            th = new Thread(new ThreadStart(dinle));
            try
            {
                th.Start();
                th.Priority = ThreadPriority.Highest;
                this.Text = "Server açıldı";
            }
            catch (ThreadAbortException hata)
            {
                th.Abort();
                MessageBox.Show("Bekleyen paket yok");
            } 
        }

        const int port = 10000;
        private void dinle()
        {
            
            string ip;
            string gelen;
            TcpListener dinleyici = new TcpListener(port);
            byte[] gelenveri_dizi = new byte[1024];
            
            while (true)
            {
                dinleyici.Start();
                Socket soket = dinleyici.AcceptSocket();
                soket.Receive(gelenveri_dizi, gelenveri_dizi.Length, 0);
                gelen = Encoding.ASCII.GetString(gelenveri_dizi);
                ip = soket.RemoteEndPoint.ToString();
                ListViewItem itm = new ListViewItem();
                itm.Text = ip;
                itm.SubItems.Add(gelen);
                itm.SubItems.Add(DateTime.Now.ToString());
                listView1.Items.Add(itm);

                string cevap = "Mesajiniz basariyla alinmistir." + Environment.NewLine +
                    "Gelen: "  + gelen + Environment.NewLine;
                byte[] gonderveri_dizi = Encoding.ASCII.GetBytes(cevap);
                soket.Send(gonderveri_dizi, gonderveri_dizi.Length, 0);

                gelen = "";
                dinleyici.Stop();
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            //TcpListener dinleyici = new TcpListener(port);
            //dinleyici.Start();

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            th.Abort();   
        }


    }
}
