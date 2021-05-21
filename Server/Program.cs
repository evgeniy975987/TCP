using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static string ip = "127.0.0.1";
        static int port = 8080;



        static void Main(string[] args)
        {
            Task.Run(() => Test()).Wait();

        }

        public static void Test()
        {
            
                EndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            socket.Bind(endPoint);
            socket.Listen(5);
            var listener = socket.Accept();
            byte[] buffer = new byte[256];
            int size = 0;
            
            while (true)
            {
                var data = new StringBuilder();
                do
                {
                    size = listener.Receive(buffer);
                    data.Append(Encoding.UTF8.GetString(buffer, 0, size));
                }
                while (listener.Available > 0);
                Console.WriteLine(data);
                listener.Send(Encoding.UTF8.GetBytes("успех"));

            }


        }
    }
}
