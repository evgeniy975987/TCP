using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {

        //static string ipClient = "192.168.1.70";

        static string ipClient = "127.0.0.1";
       
        static void Main(string[] args)
        {
            List<Task> tasks = new List<Task>();

            //tasks.Add(new Task(() => { Messenger(); }));
            Task task = Task.Run(() => { Messenger(); });
            tasks.Add(task);
            
            Task T = Task.Run(() => { GetFile(); });
             tasks.Add(T);

            Task.WaitAll(tasks.ToArray());
        }

        public static void Messenger()
        {
             
             int port = 8085;
            
            EndPoint endPoint = new IPEndPoint(IPAddress.Parse(ipClient), port);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            socket.Bind(endPoint);
            socket.Listen(5);
            var listener = socket.Accept();
            byte[] buffer = new byte[64];
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

        public static void GetFile()
        {
            
            int portClient = 8080; 
           int portServer = 8010;
            UdpClient udpReciever = new UdpClient(portServer);

            IPEndPoint recieveEndPoint = new IPEndPoint(IPAddress.Parse(ipClient), portClient);
            
            byte[] packetRecieve = new byte[1];
            while (true)
            {

                byte[] packetSend = new byte[1];
                
                packetRecieve = udpReciever.Receive(ref recieveEndPoint);
                string name = Encoding.Unicode.GetString(packetRecieve);
                udpReciever.Send(packetSend, 1, recieveEndPoint);

                packetRecieve = udpReciever.Receive(ref recieveEndPoint);
                string format = Encoding.Unicode.GetString(packetRecieve);
                udpReciever.Send(packetSend, 1, recieveEndPoint);


                packetRecieve = udpReciever.Receive(ref recieveEndPoint);
                int parts = BitConverter.ToInt32(packetRecieve, 0);
                
                
                
                using (FileStream fs = new FileStream($"{name + format}", FileMode.Create, FileAccess.Write))
                {

                    for (int i = 0; i < parts; i++)
                    {
                        udpReciever.Send(packetSend, 1, ipClient, portClient);
                        packetRecieve = udpReciever.Receive(ref recieveEndPoint);
                        fs.Write(packetRecieve, 0, packetRecieve.Length);
                        udpReciever.Send(new byte[1], 1, ipClient,portClient);
                        Console.WriteLine(i);
                    }

                    Console.WriteLine("файл получен");
                }
            }

        }
    }
}
