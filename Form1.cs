using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Sockets;


namespace E_CARD2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            buttonGameStart.Enabled = false;

            string hostname = Dns.GetHostName();
            IPAddress[] adrList = Dns.GetHostAddresses(hostname);
            foreach (IPAddress address in adrList)
            {
               labelIP.Text = address.ToString();
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            receiveNameTask();
            GameTask();
        }

        private void Send_Name(object sender, EventArgs e)//名前
        {
            var udp = new System.Net.Sockets.UdpClient();
            Byte[] sendBytes = Encoding.GetEncoding("Shift-JIS").GetBytes(textBoxMyName.Text);//ゲーム名を送信
            udp.Send(sendBytes, sendBytes.Length, EnemyIp.Text, 10);
            
            if(textBoxMyName.Text==" "||textBoxMyName.Text==null)
            {
                textBoxMyName.Text = "Gest";
            }
            labelMyName.Text = textBoxMyName.Text;//自分の名前
        }

       
        private async Task receiveNameTask()
        {
            var udp = new UdpClient(10);
            while (true)
            {
                //送信された文字列の受信
                var Nameresult = await udp.ReceiveAsync();
                string Name = Encoding.GetEncoding("Shift-JIS").GetString(Nameresult.Buffer);

                labelEnemyName.Text = Name.ToString();
            }
        }
        private async Task GameTask()
        {
            var udp = new UdpClient(11);
            while (true)
            {
                //送信された文字列の受信
                var Gameresult = await udp.ReceiveAsync();
                string Game = Encoding.GetEncoding("Shift-JIS").GetString(Gameresult.Buffer);
                if(Game=="KOUTEI")
                {
                    var f3 = new KOUTEI();
                    f3.Show(this);
                }
                else if(Game=="DOREI")
                {
                    var f2 = new DOREI();
                    f2.Show(this);
                }
            }
        }

        private void labelMyName_TextAlignChanged(object sender, EventArgs e)
        {
            labelMember.Text = (int.Parse(labelMyName.Text) + 1).ToString();
            if(int.Parse(labelMyName.Text)==2)
            {
                buttonGameStart.Enabled = true;
            }
        }

        private void buttonGameStart_Click(object sender, EventArgs e)
        {

            int i; var rnd = new Random();
            i = rnd.Next(2);
            if(i==0)//自分が奴隷
            {
                var udp = new System.Net.Sockets.UdpClient();
                Byte[] sendBytes = Encoding.GetEncoding("Shift-JIS").GetBytes("KOUTEI");
                udp.Send(sendBytes, sendBytes.Length, EnemyIp.Text, 11);

                var f2 = new DOREI();//相手に皇帝側にいかせる文字列を送信して自分は奴隷側へ
                f2.Show(this);
            }
            else//自分が皇帝
            {
                var udp = new System.Net.Sockets.UdpClient();
                Byte[] sendBytes = Encoding.GetEncoding("Shift-JIS").GetBytes("DOREI");
                udp.Send(sendBytes, sendBytes.Length, EnemyIp.Text, 11);

                var f3 = new KOUTEI();
                f3.Show(this);
            }
           
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            var f2 = new DOREI();
            f2.Show(this);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            var f3 = new KOUTEI();
            f3.Show(this);
        }
    }
    
}
