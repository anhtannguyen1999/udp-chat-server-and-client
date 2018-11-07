using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Socket_Manager
{
    class SocketManager
    {
        //Client
        #region client
        Socket client;
        
        public bool connectServer()
        {
            try
            {
                IPAddress.Parse(IP);
                int.Parse(PORT);
            }
            catch
            {
                return false ;
            }
            
            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(IP),int.Parse(PORT));

            //Thiết lập socket client có kết nối UDP
            client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            try
            {
                client.Connect(ipe);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        //Server
        #region server
        Socket server;
        public bool CreateServer()
        {
            
            try
            {
                //IPAddress.Parse(ip);
                int.Parse(PORT);
            }
            catch
            {
                return false;
            }
            IPEndPoint ipe = new IPEndPoint(IPAddress.Any, int.Parse(PORT));
            server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            server.Bind(ipe);
            
            return true;
        }
        #endregion

//Client and server****************************************************************
        #region both
        public string IP;
        public string PORT;
        public bool isServer = true;
        public const int BUFFER = 1024;

        //Phân tích data thành 1 mảng byte 010101010111...
        public byte[] SerializeData(Object o)
        {
            MemoryStream ms = new MemoryStream();//stream phân tích
            BinaryFormatter bf1 = new BinaryFormatter(); //
            bf1.Serialize(ms, o);//Phân tích dữ liệu 'o' theo dạng của stream
            return ms.ToArray();
        }
        //Biến mảng byte 011010101... thành data
        public object DeserializeData(byte[] theByteArray)
        {
            MemoryStream ms = new MemoryStream(theByteArray);//đưa dl vào stream
            BinaryFormatter bf1 = new BinaryFormatter();
            ms.Position = 0;//đưa về vtri 0
            return bf1.Deserialize(ms);
        }
        //Lấy ra IPv4 của card mạng đang dùng
        public string GetLocalIPv4(NetworkInterfaceType _type)
        {
            string output = "";
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            output = ip.Address.ToString();
                        }
                    }
                }
            }
            return output;
        }

        //Gửi và nhận content
        public bool send(object data)
        {
            byte[] sendData = SerializeData(data);
            return SendData(client, sendData); //Gửi hết

        }
        public object receive()
        {
            byte[] receiveData = new byte[1024];
            bool isOK = ReceiveData(server,receiveData );//Nhận từ bất kỳ cái nào 
            return DeserializeData(receiveData);
        }
        private bool SendData(Socket target,byte[]data) //Gửi data cho target
        {
            return target.Send(data)==1?true:false;//Nếu gửi thành công  trả về 1=> true
        }
        private bool ReceiveData(Socket target,byte[]data) //Nhận data từ target
        {
            return target.Receive(data) == 1 ? true : false;
        }
        #endregion
    }
}
