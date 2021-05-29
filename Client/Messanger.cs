using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Messanger
    {

        //string IPserver = "192.168.1.72";
        string IPserver = "127.0.0.1";
        int portServer = 8031;


        IPEndPoint ReceiveEndPoint;
        UdpClient udpSender;

        public Messanger()
        {


            ReceiveEndPoint = new IPEndPoint(IPAddress.Parse(IPserver), portServer); ;

            udpSender = FileWoker.udpSender;


        }
        public void SendMessage(string message)
        {
            string ip = "127.0.0.1";
            int port = 8085;
            try
            {

                EndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


                socket.Connect(endPoint);

                int size;

                


                    var data = Encoding.UTF8.GetBytes(message);
                    socket.Send(data);
                    byte[] buffer = new byte[32];

                    var answer = new StringBuilder();
                    do
                    {
                        size = socket.Receive(buffer);
                        answer.Append(Encoding.UTF8.GetString(buffer, 0, size));
                    }
                    while (socket.Available > 0);

                    Console.WriteLine(answer);
                }

            



            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
