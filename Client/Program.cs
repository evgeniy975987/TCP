using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {

        static string ip = "127.0.0.1";
        static int port = 8080;
        static void Main(string[] args)
        {

            //Task.Run(() => Test()).Wait();
            try
            {
                Task.Run(() => Test2()).Wait();
                Console.ReadLine();
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
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

            }



            catch (Exception ex) { Console.WriteLine(ex.Message); }



        }

        public static void Test2() {

           
            
            int portClient = 8080;


            
            string IPserver = "192.168.1.72";
            int portServer = 8010;

            IPEndPoint ReceiveEndPoint = new IPEndPoint(IPAddress.Parse(IPserver), portServer);

            

            UdpClient udpSender = new UdpClient( portClient);
          
            

            string path = @"C:\Users\админ\Desktop\файлы для тестов программ\1.JPG";
            int packetSize = 8192;
            byte[] packetSend;
            byte[] packetRecive;

            try
            {

                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read)) {
                    udpSender.Client.ReceiveTimeout = 10000;
                    
                    int numBytesRead = 0;
                    int BytesToRead = (int)fs.Length;

                    String fileName = Path.GetFileName(path);
                    String format = Path.GetExtension(path);
                    packetSend = Encoding.Unicode.GetBytes(fileName);
                    
                    udpSender.Send(packetSend, packetSend.Length, IPserver, portServer);


                    packetRecive = udpSender.Receive(ref ReceiveEndPoint);
                    
                    int parts = (int) fs.Length / packetSize;

                    if ((int)fs.Length % packetSize != 0) parts++;

                    packetSend = BitConverter.GetBytes(parts);

                    udpSender.Send(packetSend, packetSend.Length, IPserver, portServer);

                    packetRecive = udpSender.Receive(ref ReceiveEndPoint);

                    packetSend = new byte[packetSize];
                    int n = 0;
                    
                    
                    for (int i = 0; i < parts - 1; i++) {
                        n = fs.Read(packetSend, 0, packetSize);
                        if (n == 0) break;
                        numBytesRead += n;
                        BytesToRead -= n;
                        
                        udpSender.Send(packetSend, packetSend.Length, IPserver, portServer);
                        packetRecive = udpSender.Receive(ref ReceiveEndPoint);
                        Console.WriteLine(i);
                        Thread.Sleep(5);
                    }
                    packetSend = new byte[BytesToRead];
                    n = fs.Read(packetSend, 0, BytesToRead);
                    udpSender.Send(packetSend, packetSend.Length, IPserver, portServer);
                    packetRecive = udpSender.Receive(ref ReceiveEndPoint);
                }
                Console.WriteLine("файл отправлен");
            }

            catch (Exception ex) {

                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }
    }
}
