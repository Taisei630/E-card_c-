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
    public partial class KOUTEI : Form
    {
        PictureBox[] pic = new PictureBox[12];//コントロール配列
        int Count = 0;//勝敗の数を格納     if(Count==3)になったら勝ちでそれも送信
        int Next = 0; //続行するかしないか
        string Re;
        string IP;
        bool[] v = new bool[5];
        public KOUTEI()
        {
            InitializeComponent();
        }

        private void KOUTEI_Load(object sender, EventArgs e)
        {
            pic[0] = pictureBox1;//配列にpictureBoxを格納
            pic[1] = pictureBox2;
            pic[2] = pictureBox3;
            pic[3] = pictureBox4;
            pic[4] = pictureBox5;
            pic[5] = pictureBox11;
            pic[6] = pictureBox12;
            pic[7] = pictureBox6;
            pic[8] = pictureBox7;
            pic[9] = pictureBox8;
            pic[10] = pictureBox9;
            pic[11] = pictureBox10;


            for (int i = 0; i < pic.Length; i++)
            {
                pic[i].Enabled = false;     //最初はクリック出来ない①
            }

            pic[0].Image.Tag = "K";//タグの設定
            pic[1].Image.Tag = "S1";
            pic[2].Image.Tag = "S2";
            pic[3].Image.Tag = "S3";
            pic[4].Image.Tag = "S4";
            pic[5].Image.Tag = "N";
            pic[6].Image.Tag = "N";
            pic[7].Image.Tag = "U1";
            pic[8].Image.Tag = "U2";
            pic[9].Image.Tag = "U3";
            pic[10].Image.Tag = "U4";
            pic[11].Image.Tag = "U5";

            v[0] = false;
            v[1] = false;
            v[2] = false;
            v[3] = false;
            v[4] = false;//バグ対策

            IP = (Owner as Form1).EnemyIp.Text;

            timerGameStart.Start();
        }

        private void timerGameStart_Tick(object sender, EventArgs e)//ゲーム開始カウントダウン
        {
            labelGameStartCount.Text = (int.Parse(labelGameStartCount.Text) - 1).ToString();
            if (int.Parse(labelGameStartCount.Text) == 0)
            {
                labelGameStart.Visible = false;
                labelGameStartCount.Visible = false;//GameStart非表示
                labelGameSecond.Visible = false;
                labelSeigen.Visible = true; ;
                labelSeigenCount.Visible = true;
                labelSeigenSecond.Visible = true;

                labelSeigen.Text = "制限時間";
                labelSeigenCount.Text = "5";//label表示
                labelSeigenSecond.Text = "秒";
                for (int i = 0; i < pic.Length; i++)
                {
                    pic[i].Enabled = true;//カード選択出来るようにする。②
                }
                timerSeigenJikan.Start();
                timerGameStart.Stop();
            }

        }

        private void timerSeigenJikan_Tick(object sender, EventArgs e)//制限時間
        {
            labelSeigenCount.Text = (int.Parse(labelSeigenCount.Text) - 1).ToString();
            if (int.Parse(labelSeigenCount.Text) == 0)
            {
                labelSeigen.Visible = false;
                labelSeigenCount.Visible = false;//Seigenを非表示
                labelSeigenSecond.Visible = false;
                labelHantei.Text = "2";
                timerSyouhai.Start();
                timerSeigenJikan.Stop();

                var udp = new UdpClient();//大文字を送信し、小文字を受信する。
                if (pic[5].Image.Tag.ToString() == "K")
                {
                    Byte[] sendBytes = Encoding.GetEncoding("Shift-JIS").GetBytes("K");
                    udp.Send(sendBytes, sendBytes.Length, IP, 10000);//皇帝を召喚した場合Form3へ"K"を送信
                    v[0] = true;
                }//10000～50000
                else if (pic[5].Image.Tag.ToString() == "S1")
                {
                    Byte[] sendBytes = Encoding.GetEncoding("Shift-JIS").GetBytes("S1");
                    udp.Send(sendBytes, sendBytes.Length, IP, 20000);
                    v[1] = true;
                }
                else if (pic[5].Image.Tag.ToString() == "S2")
                {
                    Byte[] sendBytes = Encoding.GetEncoding("Shift-JIS").GetBytes("S2");
                    udp.Send(sendBytes, sendBytes.Length, IP, 30000);//市民を召喚した場合Form3へ"S"を送信
                    v[2] = true;
                }
                else if (pic[5].Image.Tag.ToString() == "S3")
                {
                    Byte[] sendBytes = Encoding.GetEncoding("Shift-JIS").GetBytes("S3");
                    udp.Send(sendBytes, sendBytes.Length, IP, 40000);//市民を召喚した場合Form3へ"S"を送信
                    v[3] = true;
                }
                else if (pic[5].Image.Tag.ToString() == "S4")
                {
                    Byte[] sendBytes = Encoding.GetEncoding("Shift-JIS").GetBytes("S4");
                    udp.Send(sendBytes, sendBytes.Length, (Owner as Form1).EnemyIp.Text, 50000);//市民を召喚した場合Form3へ"S"を送信
                    v[4] = true;
                }
                else if (pic[5].Image.Tag.ToString() == "N")
                {
                    int i;
                    do
                    {
                        var rnd = new Random();//何も召喚されてない場合
                        i = rnd.Next(5);

                        if (v[i])
                        {
                            i = 5;
                        }

                    } while (i == 5);

                    if (i == 0)
                    {
                        pic[5].Image = Properties.Resources.S;
                        pic[5].Image.Tag = "S1";
                        pic[1].Image = Properties.Resources.N;
                        pic[1].Image.Tag = "N";
                        Byte[] sendBytes = Encoding.GetEncoding("Shift-JIS").GetBytes("S1");
                        udp.Send(sendBytes, sendBytes.Length, IP, 20000);

                        v[0] = true;
                    }
                    if (i == 1)
                    {
                        pic[5].Image = Properties.Resources.S;
                        pic[5].Image.Tag = "S2";
                        pic[2].Image = Properties.Resources.N;
                        pic[2].Image.Tag = "N";
                        Byte[] sendBytes = Encoding.GetEncoding("Shift-JIS").GetBytes("S2");
                        udp.Send(sendBytes, sendBytes.Length, IP, 30000);

                        v[1] = true;
                    }
                    if (i == 2)
                    {
                        pic[5].Image = Properties.Resources.S;
                        pic[5].Image.Tag = "S3";
                        pic[3].Image = Properties.Resources.N;
                        pic[3].Image.Tag = "N";
                        Byte[] sendBytes = Encoding.GetEncoding("Shift-JIS").GetBytes("S3");
                        udp.Send(sendBytes, sendBytes.Length, IP, 40000);

                        v[2] = true;
                    }
                    if (i == 3)
                    {
                        pic[5].Image = Properties.Resources.S;
                        pic[5].Image.Tag = "S4";
                        pic[4].Image = Properties.Resources.N;
                        pic[4].Image.Tag = "N";
                        Byte[] sendBytes = Encoding.GetEncoding("Shift-JIS").GetBytes("S4");
                        udp.Send(sendBytes, sendBytes.Length, IP, 50000);

                        v[3] = true;
                    }
                    if (i == 4)
                    {
                        pic[5].Image = Properties.Resources.K;
                        pic[5].Image.Tag = "K";
                        pic[0].Image = Properties.Resources.N;
                        pic[0].Image.Tag = "N";
                        Byte[] sendBytes = Encoding.GetEncoding("Shift-JIS").GetBytes("K");
                        udp.Send(sendBytes, sendBytes.Length, (Owner as Form1).EnemyIp.Text, 54321);

                        v[4] = true;
                    }
                }
            }
        }

        private void timerSyouhai_Tick(object sender, EventArgs e)//勝敗判定(Reで判定)
        {
            labelHantei.Text = (int.Parse(labelHantei.Text) - 1).ToString();
            if ((int.Parse(labelHantei.Text) == 0))//タイマーが0
            {
                switch (pic[5].Image.Tag)//勝敗判定
                {
                    case "K"://自分が皇帝をだして
                        switch (Re)
                        {
                            case "D"://相手が奴隷
                                labelSyouhai.Text = "奴隷の勝ち";
                                pic[6].Image = Properties.Resources._2D;
                                pic[6].Image.Tag = "2D";
                                break;
                            case "s1":
                            case "s2":
                            case "s3":
                            case "s4":
                                labelSyouhai.Text = "皇帝の勝ち";
                                pic[6].Image = Properties.Resources._2S;
                                pic[6].Image.Tag = "2S";
                                Count++;
                                break;
                            default: break;

                        }

                        break;
                    case "S1"://自分が市民を出して
                    case "S2":
                    case "S3":
                    case "S4":
                        switch (Re)
                        {
                            case "D":
                                labelSyouhai.Text = "市民の勝ち";
                                pic[6].Image = Properties.Resources._2D;
                                pic[6].Image.Tag = "2D";
                                Count++;
                                break;//相手が皇帝
                            case "s1":
                            case "s2":
                            case "s3":
                            case "s4":
                                labelSyouhai.Text = "引き分け";
                                pic[6].Image = Properties.Resources._2S;
                                pic[6].Image.Tag = "2S";
                                Next = 1;
                                break;//相手が市民
                            case "win":
                                MessageBox.Show("貴方の負け");
                                break;//相手が勝利したら相手からWINが送られて来る

                            default:break;
                        }
                        break;
                    default: break;
                }
                if (Next == 1)//引き分け
                {
                    if (pic[5].Image.Tag.ToString() == "S1")
                    {
                        pic[5].Image = Properties.Resources.N;
                        pic[5].Image.Tag = "N";
                        pic[6].Image = Properties.Resources.N;
                        pic[6].Image.Tag = "N";
                        pic[7].Image = Properties.Resources.N;
                        pic[7].Image.Tag = "N";
                        pic[1].Enabled = false;

                    }//手札消す
                    else if (pic[5].Image.Tag.ToString() == "S2")
                    {
                        pic[5].Image = Properties.Resources.N;
                        pic[5].Image.Tag = "N";
                        pic[6].Image = Properties.Resources.N;
                        pic[6].Image.Tag = "N";
                        pic[8].Image = Properties.Resources.N;
                        pic[8].Image.Tag = "N";
                        pic[2].Enabled = false;
                    }
                    else if (pic[5].Image.Tag.ToString() == "S3")
                    {
                        pic[5].Image = Properties.Resources.N;
                        pic[5].Image.Tag = "N";
                        pic[6].Image = Properties.Resources.N;
                        pic[6].Image.Tag = "N";
                        pic[9].Image = Properties.Resources.N;
                        pic[9].Image.Tag = "N";
                        pic[3].Enabled = false;
                    }
                    else if (pic[5].Image.Tag.ToString() == "S4")
                    {
                        pic[5].Image = Properties.Resources.N;
                        pic[5].Image.Tag = "N";
                        pic[6].Image = Properties.Resources.N;
                        pic[6].Image.Tag = "N";
                        pic[10].Image = Properties.Resources.N;
                        pic[10].Image.Tag = "N";
                        pic[4].Enabled = false;
                    }
                    labelSeigen.Visible = true;
                    labelSeigenCount.Visible = true;
                    labelSeigenSecond.Visible = true;
                    labelSeigen.Text = "制限時間";
                    labelSeigenCount.Text = "5";//label表示
                    labelSeigenSecond.Text = "秒";

                    timerSeigenJikan.Start();//制限時間へ
                }
                else//次のラウンド
                {
                    if (Count == 3)
                    {
                        var udp = new UdpClient();
                        Byte[] sendBytes = Encoding.GetEncoding("Shift-JIS").GetBytes("WIN");
                        udp.Send(sendBytes, sendBytes.Length, IP, 60000);
                        this.Close();// ゲーム終了
                    }
                    else
                    {
                        //初期化
                        pic[0].Image = Properties.Resources.K;
                        pic[0].Image.Tag = "K";//最初に戻す。
                        pic[1].Image = Properties.Resources.S;
                        pic[1].Image.Tag = "S1";
                        pic[2].Image = Properties.Resources.S;
                        pic[2].Image.Tag = "S2";
                        pic[3].Image = Properties.Resources.S;
                        pic[3].Image.Tag = "S3";
                        pic[4].Image = Properties.Resources.S;
                        pic[4].Image.Tag = "S4";
                        pic[5].Image = Properties.Resources.N;
                        pic[5].Image.Tag = "N";
                        pic[6].Image = Properties.Resources.N;
                        pic[6].Image.Tag = "N";
                        pic[7].Image = Properties.Resources.U;
                        pic[7].Image.Tag = "U1";
                        pic[8].Image = Properties.Resources.U;
                        pic[8].Image.Tag = "U2";
                        pic[9].Image = Properties.Resources.U;
                        pic[9].Image.Tag = "U3";
                        pic[10].Image = Properties.Resources.U;
                        pic[10].Image.Tag = "U4";
                        pic[11].Image = Properties.Resources.U;
                        pic[11].Image.Tag = "U5";
                        for (int i = 0; i < pic.Length; i++)//クリック不可(timerGameStartで可能に)
                        {
                            pic[i].Enabled = false;
                        }

                        labelGameStart.Visible = true;
                        labelGameStartCount.Visible = true;//ゲーム開始表示
                        labelGameSecond.Visible = true;
                        labelGameStart.Text = "ゲーム開始まで";
                        labelGameStartCount.Text = "6";
                        labelGameSecond.Text = "秒";


                        Next = 0;
                        bool[] v = new bool[5];

                        timerGameStart.Start();//ゲーム開始カウントダウンへ
                    }
                }
                timerSyouhai.Stop();

            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)//皇帝
        {
            if (pic[5].Image.Tag.ToString() != "K")                 //奴隷以外が召喚されてた場合
            {
                if (pic[5].Image.Tag.ToString() == "S1" && !v[1])
                {
                    pic[1].Image = Properties.Resources.S;//すでに市民が召喚されてたら元の手札に戻す。
                    pic[1].Image.Tag = "S1";
                }
                if (pic[5].Image.Tag.ToString() == "S2" && !v[2])
                {
                    pic[2].Image = Properties.Resources.S;
                    pic[2].Image.Tag = "S2";
                }
                if (pic[5].Image.Tag.ToString() == "S3" && !v[3])
                {
                    pic[3].Image = Properties.Resources.S;
                    pic[3].Image.Tag = "S3";
                }
                else if (pic[5].Image.Tag.ToString() == "S4" && !v[4])
                {
                    pic[4].Image = Properties.Resources.S;
                    pic[4].Image.Tag = "S4";
                }
            }
            pic[0].Image = Properties.Resources.N;//奴隷を召喚する。
            pic[0].Image.Tag = "N";
            pic[5].Image = Properties.Resources.K;
            pic[5].Image.Tag = "K";
            for (int i = 0; i < pic.Length; i++)//二度クリック回避
            {
                pic[i].Enabled = true;
            }
            pic[0].Enabled = false;
        }

        private void pictureBox2_Click(object sender, EventArgs e)//市民1枚目
        {
            if (pic[5].Image.Tag.ToString() != "S1")                 //市民1枚目以外が召喚されてた場合
            {
                if (pic[5].Image.Tag.ToString() == "K")
                {
                    pic[0].Image = Properties.Resources.K;//すでに奴隷が召喚されてたら元の手札に戻す。
                    pic[0].Image.Tag = "K";
                }
                if (pic[5].Image.Tag.ToString() == "S2" && !v[2])
                {
                    pic[2].Image = Properties.Resources.S;
                    pic[2].Image.Tag = "S2";
                }
                if (pic[5].Image.Tag.ToString() == "S3" && !v[3])
                {
                    pic[3].Image = Properties.Resources.S;
                    pic[3].Image.Tag = "S3";
                }
                else if (pic[5].Image.Tag.ToString() == "S4" && !v[4])
                {
                    pic[4].Image = Properties.Resources.S;
                    pic[4].Image.Tag = "S4";
                }
            }
            pic[1].Image = Properties.Resources.N;//奴隷を召喚する。
            pic[1].Image.Tag = "N";
            pic[5].Image = Properties.Resources.S;
            pic[5].Image.Tag = "S1";
            for (int i = 0; i < pic.Length; i++)//二度クリック回避
            {
                pic[i].Enabled = true;
            }
            pic[1].Enabled = false;
        }

        private void pictureBox3_Click(object sender, EventArgs e)//市民２枚目
        {
            if (pic[5].Image.Tag.ToString() != "S2")                 //市民1枚目以外が召喚されてた場合
            {
                if (pic[5].Image.Tag.ToString() == "K")
                {
                    pic[0].Image = Properties.Resources.K;//すでに奴隷が召喚されてたら元の手札に戻す。
                    pic[0].Image.Tag = "K";
                }
                if (pic[5].Image.Tag.ToString() == "S1" && !v[1])
                {
                    pic[1].Image = Properties.Resources.S;
                    pic[1].Image.Tag = "S2";
                }
                if (pic[5].Image.Tag.ToString() == "S3" && !v[3])
                {
                    pic[3].Image = Properties.Resources.S;
                    pic[3].Image.Tag = "S3";
                }
                else if (pic[5].Image.Tag.ToString() == "S4" && !v[4])
                {
                    pic[4].Image = Properties.Resources.S;
                    pic[4].Image.Tag = "S4";
                }
            }
            pic[2].Image = Properties.Resources.N;//奴隷を召喚する。
            pic[2].Image.Tag = "N";
            pic[5].Image = Properties.Resources.S;
            pic[5].Image.Tag = "S2";
            for (int i = 0; i < pic.Length; i++)//二度クリック回避
            {
                pic[i].Enabled = true;
            }
            pic[2].Enabled = false;
        }

        private void pictureBox4_Click(object sender, EventArgs e)//市民３枚目
        {
            if (pic[5].Image.Tag.ToString() != "S3")                 //市民1枚目以外が召喚されてた場合
            {
                if (pic[5].Image.Tag.ToString() == "K")
                {
                    pic[0].Image = Properties.Resources.K;//すでに奴隷が召喚されてたら元の手札に戻す。
                    pic[0].Image.Tag = "K";
                }
                if (pic[5].Image.Tag.ToString() == "S1" && !v[1])
                {
                    pic[1].Image = Properties.Resources.S;
                    pic[1].Image.Tag = "S1";
                }
                if (pic[5].Image.Tag.ToString() == "S2" && !v[2])
                {
                    pic[2].Image = Properties.Resources.S;
                    pic[2].Image.Tag = "S2";
                }
                else if (pic[5].Image.Tag.ToString() == "S4" && !v[4])
                {
                    pic[4].Image = Properties.Resources.S;
                    pic[4].Image.Tag = "S4";
                }
            }
            pic[3].Image = Properties.Resources.N;//奴隷を召喚する。
            pic[3].Image.Tag = "N";
            pic[5].Image = Properties.Resources.S;
            pic[5].Image.Tag = "S3";
            for (int i = 0; i < pic.Length; i++)//二度クリック回避
            {
                pic[i].Enabled = true;
            }
            pic[3].Enabled = false;
        }

        private void pictureBox5_Click(object sender, EventArgs e)//市民４枚目
        {
            if (pic[5].Image.Tag.ToString() != "S4")                 //市民1枚目以外が召喚されてた場合
            {
                if (pic[5].Image.Tag.ToString() == "K")
                {
                    pic[0].Image = Properties.Resources.K;//すでに奴隷が召喚されてたら元の手札に戻す。
                    pic[0].Image.Tag = "K";
                }
                if (pic[5].Image.Tag.ToString() == "S1" && !v[1])
                {
                    pic[1].Image = Properties.Resources.S;
                    pic[1].Image.Tag = "S1";
                }
                if (pic[5].Image.Tag.ToString() == "S2" && !v[2])
                {
                    pic[2].Image = Properties.Resources.S;
                    pic[2].Image.Tag = "S2";
                }
                else if (pic[5].Image.Tag.ToString() == "S3" && !v[3])
                {
                    pic[3].Image = Properties.Resources.S;
                    pic[3].Image.Tag = "S3";
                }
            }
            pic[4].Image = Properties.Resources.N;
            pic[4].Image.Tag = "N";
            pic[5].Image = Properties.Resources.S;
            pic[5].Image.Tag = "S4";
            for (int i = 0; i < pic.Length; i++)//二度クリック回避
            {
                pic[i].Enabled = true;
            }
            pic[4].Enabled = false;
        }

        private void pictureBox11_Click(object sender, EventArgs e)//召喚場所
        {
            if (((PictureBox)sender).Image.Tag.ToString() == "K")
            {
                pic[5].Image = Properties.Resources.N;
                pic[5].Image.Tag = "N";
                pic[0].Image = Properties.Resources.K;
                pic[0].Image.Tag = "K";
            }
            else
            {
                if (pic[5].Image.Tag.ToString() == "S1")
                {
                    pic[5].Image = Properties.Resources.N;
                    pic[5].Image.Tag = "N";
                    pic[1].Image = Properties.Resources.S;
                    pic[1].Image.Tag = "S1";
                }
                else if (pic[5].Image.Tag.ToString() == "S2")
                {
                    pic[5].Image = Properties.Resources.N;
                    pic[5].Image.Tag = "N";
                    pic[2].Image = Properties.Resources.S;
                    pic[2].Image.Tag = "S2";
                }
                else if (pic[5].Image.Tag.ToString() == "S3")
                {
                    pic[5].Image = Properties.Resources.N;
                    pic[5].Image.Tag = "N";
                    pic[3].Image = Properties.Resources.S;
                    pic[3].Image.Tag = "S3";
                }
                else if (pic[5].Image.Tag.ToString() == "S4")
                {
                    pic[5].Image = Properties.Resources.N;
                    pic[5].Image.Tag = "N";
                    pic[4].Image = Properties.Resources.S;
                    pic[4].Image.Tag = "S4";
                }
            }
        }

        private void KOUTEI_Shown(object sender, EventArgs e)//受信
        {
            DoreiTask();
            shimin1Task();
            shimin2Task();
            shimin3Task();
            shimin4Task();
            WinTask();
        }
        private async Task DoreiTask()//皇帝
        {
            var udp = new UdpClient(1);
            while (true)
            {
                var result = await udp.ReceiveAsync();
                string message = Encoding.GetEncoding("Shift-JIS").GetString(result.Buffer);
                Re = message;//変数Reにmessageを格納
            }
        }
        private async Task shimin1Task()//市民１枚目
        {
            var udp = new UdpClient(2);
            while (true)
            {
                var result = await udp.ReceiveAsync();
                string message = Encoding.GetEncoding("Shift-JIS").GetString(result.Buffer);
                Re = message;
            }
        }
        private async Task shimin2Task()//市民2枚目
        {
            var udp = new UdpClient(3);
            while (true)
            {
                var result = await udp.ReceiveAsync();
                string message = Encoding.GetEncoding("Shift-JIS").GetString(result.Buffer);
                Re = message;
            }
        }
        private async Task shimin3Task()//市民3枚目
        {
            var udp = new UdpClient(4);
            while (true)
            {
                var result = await udp.ReceiveAsync();
                string message = Encoding.GetEncoding("Shift-JIS").GetString(result.Buffer);
                Re = message;
            }
        }
        private async Task shimin4Task()//市民4枚目
        {
            var udp = new UdpClient(5);
            while (true)
            {
                var result = await udp.ReceiveAsync();
                string message = Encoding.GetEncoding("Shift-JIS").GetString(result.Buffer);
                Re = message;
            }
        }
        private async Task WinTask()//勝利
        {
            var udp = new UdpClient(6);
            while (true)
            {
                var result = await udp.ReceiveAsync();
                string message = Encoding.GetEncoding("Shift-JIS").GetString(result.Buffer);
                Re = message;
            }
        }

    }
}
