using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    public class FileWoker
    {
        public  static int portClient = 8080;
        private int portServer = 8010;
        //private string IPserver = "192.168.1.72";
        static string IPserver = "127.0.0.1";
        IPEndPoint ReceiveEndPoint ;
        public static UdpClient udpSender;

        

        
       
        public FileWoker() {


            ReceiveEndPoint = new IPEndPoint(IPAddress.Parse(IPserver), portServer);
            udpSender = new UdpClient(portClient);


        }
        public  void SendFile(string path)
        {
            int packetSize = 8192;
            byte[] packetSend;
            byte[] packetRecive;

            try
            {

                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    udpSender.Client.ReceiveTimeout = 10000;

                    int numBytesRead = 0;
                    int BytesToRead = (int)fs.Length;

                    String fileName = Path.GetFileName(path);
                    String format = Path.GetExtension(path);

                    packetSend = Encoding.Unicode.GetBytes(fileName);
                    udpSender.Send(packetSend, packetSend.Length, IPserver, portServer);
                    packetRecive = udpSender.Receive(ref ReceiveEndPoint);

                    packetSend = Encoding.Unicode.GetBytes(format);
                    udpSender.Send(packetSend, packetSend.Length, IPserver, portServer);
                    packetRecive = udpSender.Receive(ref ReceiveEndPoint);

                    int parts = (int)fs.Length / packetSize;

                    if ((int)fs.Length % packetSize != 0) parts++;

                    packetSend = BitConverter.GetBytes(parts);

                    udpSender.Send(packetSend, packetSend.Length, IPserver, portServer);

                    packetRecive = udpSender.Receive(ref ReceiveEndPoint);

                    packetSend = new byte[packetSize];
                    int n = 0;
                    for (int i = 0; i < parts - 1; i++)
                    {
                        n = fs.Read(packetSend, 0, packetSize);
                        if (n == 0) break;
                        numBytesRead += n;
                        BytesToRead -= n;

                        udpSender.Send(packetSend, packetSend.Length, IPserver, portServer);
                        packetRecive = udpSender.Receive(ref ReceiveEndPoint);
                        Console.WriteLine(i);
                        Thread.Sleep(1);
                    }
                    packetSend = new byte[BytesToRead];
                    n = fs.Read(packetSend, 0, BytesToRead);
                    udpSender.Send(packetSend, packetSend.Length, IPserver, portServer);
                    packetRecive = udpSender.Receive(ref ReceiveEndPoint);
                }
                Console.WriteLine("файл отправлен");
            }

            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }
    }
}
