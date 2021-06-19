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

           
            
            
            Task T = Task.Run(() => { GetFile(); });
             tasks.Add(T);

            Task.WaitAll(tasks.ToArray());
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
