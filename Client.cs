using Socket_Manager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UDPServer_Test3
{
    public partial class Client : Form
    {
        SocketManager socket;
        public Client()
        {
            InitializeComponent();
            socket = new SocketManager();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (txtIP.Text == "" || txtPort.Text == "")
            {
                MessageBox.Show("Chưa nhập IP/Port!");
                return;
            }

            socket.PORT = txtPort.Text;
            socket.IP = txtIP.Text;
            ketNoiServer();
        }
        private void ketNoiServer()
        {
            try
            {
                socket.connectServer();
                //label3.Text = "Tạo server thành công! Sẵn sàng kết nối!";
                MessageBox.Show("Kết nối thành công!");
                //Lắng nghe từ server
                Thread listenThread = new Thread(() =>
                {
                    while (true)
                    {
                        Thread.Sleep(50);//sleep 50 để giản lệnh while ra
                        try
                        {
                            Listen();
                            break;
                        }
                        catch
                        {

                        }

                    }

                });
                listenThread.IsBackground = true;
                listenThread.Start();
                
            }
            catch
            {
                MessageBox.Show("Kết nối server thất bại!\nVui lòng kiểm tra IP & PORT");
                //label3.Text = "Tao server that bai!";
                return;
            }
        }
        private void Listen()
        {
            string chuoiNhan = (string)socket.receive();
            MessageBox.Show(chuoiNhan);
            //rtxtShowMessage.Text += chuoiNhan + "\n";
        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                socket.send(txtSend.Text);
                txtSend.Text = "";
            }
            catch
            {
                MessageBox.Show("Chưa có kết nối tới server!");
            }
        }
    }
}
