using Socket_Manager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UDPServer_Test3
{
    public partial class Server : Form
    {
        SocketManager socket;
        public Server()
        {
            InitializeComponent();
            socket = new SocketManager();
            showMyIP();
        }
        private void showMyIP()
        {
            string localIPv4 = socket.GetLocalIPv4(NetworkInterfaceType.Wireless80211);//Lấy IPv4 wifi
            if (string.IsNullOrEmpty(localIPv4))//Nếu null thì lấy của Ethernet
            {
                localIPv4 = socket.GetLocalIPv4(NetworkInterfaceType.Ethernet);
            }
            txtIP.Text = localIPv4;
        }
        private void btnCreate_Click(object sender, EventArgs e)
        {
            if(txtIP.Text==""||txtPort.Text=="")
            {
                MessageBox.Show("Chưa nhập Port!");
                return;
            }

            socket.PORT = txtPort.Text;
            socket.IP = txtIP.Text;
            
            taoServer();            
        }
                
        private void taoServer()
        {
            try
            {
                socket.CreateServer();
                label3.Text = "Tạo server thành công! Sẵn sàng kết nối!";
                MessageBox.Show("Tạo server thành công! Sẵn sàng kết nối!");
            }
            catch
            {
                MessageBox.Show("Tạo server thất bại!\nServer đã tồn tại hoặc Port nhập vào không đúng định dạng.");
                label3.Text = "Tao server that bai!";
                return;
            }


            Thread listen = new Thread(new ThreadStart(NhanDL));
            listen.Start();
        }
        private void NhanDL()
        {
            while (true)
            {
                while (true)
                {
                    //thread.sleep(50);//sleep 50 để giản lệnh while ra                    
                    try
                    {
                        Listen();
                    }
                    catch
                    {
                    }
                }
                
                
            }
        }
        private string chuoiNhan="";
        private delegate void ShowOnRTBX1Callback(string message);
        // InvokeRequired required compares the thread ID of the
        // calling thread to the thread ID of the creating thread.
        // If these threads are different, it returns true.
        //Đã dùng delegate để xử lý bug này
        //https://stackoverflow.com/questions/10775367/cross-thread-operation-not-valid-control-textbox1-accessed-from-a-thread-othe
        //http://diendan.congdongcviet.com/threads/t5794::xu-ly-loi-cross-thread-cross-thread-operation-not-valid.cpp
        //...
        private void Listen()
        {
            chuoiNhan = (string)socket.receive();
            ShowOnRTBX1(chuoiNhan);
            //MessageBox.Show(chuoiNhan);            
        }
        private void ShowOnRTBX1(string message)
        {
            if (this.rtbx1.InvokeRequired)
            {
                ShowOnRTBX1Callback d = new ShowOnRTBX1Callback(ShowOnRTBX1);
                this.Invoke(d, new object[] { message });
            }
            else
            {
                this.rtbx1.Text += "Đã nhận: "+message+"\n";
            }
        }
        private void Server_Load(object sender, EventArgs e)
        {
            (new Client()).Show();
        }
    }
}
