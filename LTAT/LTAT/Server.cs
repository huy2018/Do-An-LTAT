using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Numerics;
using System.Security.Cryptography;

namespace LTAT
{
    public partial class Server : Form
    {
        public Server()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            connet();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        TcpListener listener;
        TcpClient tcpclient;
        NetworkStream ns;
        IPEndPoint ipe;
        public void connet()
        {
            try
            {
                ipe = new IPEndPoint(IPAddress.Any, 995);
                listener = new TcpListener(ipe);
                listener.Start();
                tcpclient = listener.AcceptTcpClient();
                ns = tcpclient.GetStream();
                Thread t = new Thread(Receive);
                t.IsBackground = true;
                t.Start();
            }
            catch { }
        }

        //Nhận Key và check có Đúng Hay Không
        public void Receive()
        {
            md5 md5 = new md5();
            byte[] bytes = new byte[1024];
            while (true)
            {
                int bytesRead = ns.Read(bytes, 0, bytes.Length);
                string text = Encoding.UTF8.GetString(bytes, 0, bytesRead);
                string[] s1 = text.Split(';');
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
            if (listBoxserver.InvokeRequired)
            {
                listBoxserver.Invoke(new MethodInvoker(delegate
                {
                    listBoxserver.Items.Add(type + ": " + text);
                }));
            }
        }
        private void insert(string text, string type)
        {
            listBoxserver.Items.Add(type + ": " + text);
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
                    this.insert(txtmess.Text, "Me");
                    txtmess.Clear();
                }
            }
        }
        
        private void butsendnoise_Click(object sender, EventArgs e)
        {
            string[] s1 = txtmessnoise.Text.Split(';');
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
        public void senddata(string text)
        {
            byte[] byteTime = Encoding.UTF8.GetBytes(text);
            ns.Write(byteTime, 0, byteTime.Length);
        }

        //lay time trong may tinh
        public string getdatime()
        {
            return DateTime.Now.ToString("h:mm:ss tt");
        }

        //thoi gian doi key
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

        //tao IV = 16 byte
        public byte[] create_init_vector(string iv)
        {
            SHA256 sha256 = SHA256Managed.Create();
            byte[] tmp = sha256.ComputeHash(Encoding.ASCII.GetBytes(iv));
            byte[] ivc = new byte[16];
            for (int i = 0; i < 16; i++)
            {
                ivc[i] = tmp[i];
            }
            return ivc;
        }
        //gui key trong tung truong hop tren Hàm receive ở trên
        public void gui_key(string x)
        {
            Tao_p_g();
            dh = new Diffie_hellman(p, g);
            string s = x + ";" + p + ";" + g + ";" + dh.Publickey;
            senddata(s);
        }

        byte[] key;
        byte[] iv;

        //ma hoa tin nhan
        public void encryptmess(string mess)
        {
            md5 md5 = new md5();
            Aes256 aes256 = new Aes256();
            SHA256 sha256 = SHA256Managed.Create();
            //array bytes message
            byte[] mess1 = Encoding.UTF8.GetBytes(mess);
            ////array bytes key chung
            byte[] key1 = sha256.ComputeHash(Encoding.ASCII.GetBytes(Convert.ToString(dh.Key_Chung)));
            //get daytime
            string ivc = getdatime();
            //array bytes iv from daytime , start index 2
            byte[] iv = create_init_vector(ivc);
            txtiv.Text = Convert.ToBase64String(iv);
            //get 32 byte from datime for padding
            byte[] dayhash = sha256.ComputeHash(Encoding.ASCII.GetBytes(ivc));
            //encrypt message
            string encrypt = aes256.EncryptString(mess1, key1, iv, dayhash);
            string[] enc1 = encrypt.Split(';');
            string mess_md5 = md5.GetMD5(enc1[0] + Convert.ToString(dh.Key_Chung));
            //byte[] enc = Encoding.ASCII.GetBytes(encrypt);
            string messsend = "Receive;" + encrypt + ";" + ivc + ";" + mess_md5;
            //string[] s1 = messsend.Split(';');
            //textBoxmessnoise.Text = s1[1].ToString() + ";" + s1[2].ToString() + ";" + s1[3].ToString();
            txtmessnoise.Text = messsend;
            senddata(messsend);
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
            MessageList(Convert.ToString(decrypt), "Client");
            timer1.Enabled = true;
            timer1.Start();
        }

        //Alice Gửi p ,g ,Key Public --> Bob Nhận public_key, p ,g ,private_key random -->Tạo Key Chung
        public void send_key_public(string[] mess)
        {
            SHA256 sha256 = SHA256Managed.Create();
            p = Convert.ToInt32(mess[1]);
            g = Convert.ToInt32(mess[2]);
            dh = new Diffie_hellman(p, g);          
            dh.Tao_key_chung(Convert.ToInt32(mess[3]));
            key = sha256.ComputeHash(Encoding.ASCII.GetBytes(Convert.ToString(dh.Key_Chung))); 
            txtkey.Text = Convert.ToBase64String(key);
            //Bob Gửi public_key qua Alice
            senddata(Convert.ToString("Receive_Key" + ";" + dh.Publickey));
            session = true;
        }

        //Alice Nhận public_key Từ Bob --> Tạo Key Chung
        public void receive_key_public(string[] mess)
        {
            SHA256 sha256 = SHA256Managed.Create();
            dh.Tao_key_chung(Convert.ToInt32(mess[1])); 
            key = sha256.ComputeHash(Encoding.ASCII.GetBytes(Convert.ToString(dh.Key_Chung)));
            txtkey.Text = Convert.ToBase64String(key);
            session = true;
        }
        //Check MD5,phat hien thay doi key
        public bool check_md5(string encrypt, string md5_encrypt)
        {
            bool check = false;//sai hien thong bao.
            md5 md5 = new md5();
            string result = md5.GetMD5(encrypt + Convert.ToString(dh.Key_Chung));
            if (result == md5_encrypt)
            {
                check = true;//dung tiep tuc gui.
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
