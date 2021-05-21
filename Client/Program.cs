using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
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

            try
            {

                EndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                
                
                socket.Connect(endPoint);
               
                int size;

                while (true) {
                    Console.WriteLine("ВВедите сообщение");
                    string message = Console.ReadLine();
                    var data = Encoding.UTF8.GetBytes(message);
                    socket.Send(data);
                    byte[] buffer = new byte[256];
                    
                    var answer = new StringBuilder();
                    do
                    {
                        size = socket.Receive(buffer);
                        answer.Append(Encoding.UTF8.GetString(buffer, 0, size));
                    }
                    while (socket.Available > 0);

                    Console.WriteLine(answer);
                }

            }



            catch (Exception ex) { Console.WriteLine(ex.Message); }



        }
    }
}
