using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Client : Form
    {
        public Client()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            connet();
        }
        TcpClient tcpclient;
        NetworkStream ns;
        IPEndPoint ipe;
        public void connet()
        {
            ipe = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 995);
            tcpclient = new TcpClient();
            tcpclient.Connect(ipe);
            ns = tcpclient.GetStream();
            gui_key("Send_Key");
            Thread t = new Thread(Receive);
            t.IsBackground = true;
            t.Start();
        }
        public void Receive()
        {
            byte[] bytes = new byte[1024];

            while (true)
            {

                int bytesRead = ns.Read(bytes, 0, bytes.Length);
                string s = Encoding.UTF8.GetString(bytes, 0, bytesRead);
                string[] s1 = s.Split(';');
                switch (s1[0])
                {
                    case "Send_Key":
                        send_key_public(s1);
                        break;

                    case "Receive_Key":
                        receive_key_public(s1);
                        break;
                    case "Receive":
                        if (check_md5(s1[1], s1[4]))
                        {
                            decrypt_mess(s1);
                            session = true;
                            dem = 0;
                        }
                        else
                        {
                            MessageList("Message da bi thay doi", "--");
                        }
                        break;
                }
            }
        }
        private void MessageList(string text, string type)
        {
            if (listBoxclient.InvokeRequired)
            {
                listBoxclient.Invoke(new MethodInvoker(delegate
                {
                    listBoxclient.Items.Add(type + ": " + text);
                }));

            }
        }
        private void insert(string text, string type)
        {
            listBoxclient.Items.Add(type + ": " + text);
        }

        private void butsend_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            timer1.Interval = 1000;
            dem = 0;
            if (session == false)
            {
                gui_key("Send_Key");
            }
            else
            {
                if (txtmess.Text != "")
                {
                    encryptmess(txtmess.Text);
                    txtmess.Clear();
                }
            }
        }

        private void butsendnoise_Click(object sender, EventArgs e)
        {
            senddata(txtmessnoise.Text);
        }

        //tinh p, g
        BigInteger p, g;
        Diffie_hellman dh;
        public void Tao_p_g()
        {
            do
            {
                p = Songaunhien();
                g = (p - 1) / 2;
            } while (!KTnguyento((int)p));
        }

        //randdom p
        private int Songaunhien()
        {

            Random rd = new Random();
            return rd.Next(1, 10000);
        }
        //kt nguyen to
        private bool KTnguyento(int i)
        {
            bool kiemtra = true;
            for (int j = 2; j < i; j++)
                if (i % j == 0)
                    kiemtra = false;
            return kiemtra;
        }


        public string getdatime()
        {
            return DateTime.Now.ToString("h:mm:ss tt");
        }

        public byte[] create_init_vector(string iv)
        {
            SHA256 mySHA256 = SHA256Managed.Create();
            byte[] tmp = mySHA256.ComputeHash(Encoding.ASCII.GetBytes(iv));
            byte[] ivc = new byte[16];
            for (int i = 0; i < 16; i++)
            {
                ivc[i] = tmp[i];
            }
            return ivc;
        }
        public void senddata(string text)
        {
            byte[] byteTime = Encoding.UTF8.GetBytes(text);
            ns.Write(byteTime, 0, byteTime.Length);
        }

        bool session = false;
        int dem = 0;

        private void timer1_Tick(object sender, EventArgs e)
        {
            dem++;

            if (dem == 20)
            {
                session = false;
                MessageBox.Show("Change Key");
                gui_key("Send_Key");
                timer1.Start();
                dem = 0;
            }
        }

        public void gui_key(string x)
        {
            Tao_p_g();
            dh = new Diffie_hellman(p, g);
            string s = x + ";" + p + ";" + g + ";" + dh.Publickey;
            senddata(s);
        }
        byte[] key;
        byte[] iv;
        public void encryptmess(string mess)
        {
            if (mess.Length > 0)
            {
                md5 md5 = new md5();
                listBoxclient.Items.Add("Me: " + mess);
                Aes256 aes256 = new Aes256();
                SHA256 sha256 = SHA256Managed.Create();
                byte[] mess1 = Encoding.UTF8.GetBytes(mess);
                byte[] key1 = sha256.ComputeHash(Encoding.ASCII.GetBytes(Convert.ToString(dh.Key_Chung)));
                txtkey.Text = Convert.ToBase64String(key1);
                string ivc = getdatime();
                byte[] iv = create_init_vector(ivc);
                txtiv.Text = Convert.ToBase64String(iv);
                byte[] dayhash = sha256.ComputeHash(Encoding.ASCII.GetBytes(ivc));
                string encrypt = aes256.EncryptString(mess1, key1, iv, dayhash);
                string[] enc1 = encrypt.Split(';');
                string mess_md5 = md5.GetMD5(enc1[0] + Convert.ToString(dh.Key_Chung));

                string messsend = "Receive;" + encrypt + ";" + ivc + ";" + mess_md5;
                //string[] s1 = messsend.Split(';');
                //textBoxmessnoise.Text = s1[1].ToString() + ";" + s1[2].ToString() + ";" + s1[3].ToString();
                txtmessnoise.Text = messsend;
                senddata(messsend);
            }
        }
        public void decrypt_mess(string[] mess)
        {
            Aes256 aes256 = new Aes256();
            string encrypt = mess[1];
            string ivc = mess[3];
            int count = Convert.ToInt32(mess[2]);
            iv = create_init_vector(ivc);
            txtiv.Text = Convert.ToBase64String(iv);
            string decrypt = aes256.DecryptString(encrypt, key, iv, count);
            MessageList(Convert.ToString(decrypt), "Server");
            timer1.Enabled = true;
            timer1.Start();
        }
        public void send_key_public(string[] mess)
        {
            SHA256 sha256 = SHA256Managed.Create();
            p = Convert.ToInt32(mess[1]);
            g = Convert.ToInt32(mess[2]);
            dh = new Diffie_hellman(p, g);
            dh.Tao_key_chung(Convert.ToInt32(mess[3]));
            key = sha256.ComputeHash(Encoding.ASCII.GetBytes(Convert.ToString(dh.Key_Chung)));
            senddata(Convert.ToString("Receive_Key" + ";" + dh.Publickey));
            txtkey.Text = Convert.ToBase64String(key);
            session = true;
        }
        public void receive_key_public(string[] mess)
        {
            SHA256 sha256 = SHA256Managed.Create();
            dh.Tao_key_chung(Convert.ToInt32(mess[1]));
            key = sha256.ComputeHash(Encoding.ASCII.GetBytes(Convert.ToString(dh.Key_Chung)));
            txtkey.Text = Convert.ToBase64String(key);
            session = true;
        }
        //Check MD5
        public bool check_md5(string encrypt, string md5_encrypt)
        {
            bool check = false;
            md5 md5 = new md5();
            string result = md5.GetMD5(encrypt + Convert.ToString(dh.Key_Chung));
            if (result == md5_encrypt)
            {
                check = true;
            }
            return check;
        }

        private void txtmess_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                butsend.PerformClick();
            }
        }
        


    }
}
