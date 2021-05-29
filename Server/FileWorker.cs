using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class FileWorker
    {
        static string ip = "127.0.0.1";
        static StringBuilder fileName;
        static StringBuilder fileFormat;
        
        

        public FileWorker() {


            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint iPEndPointFileName = new IPEndPoint(IPAddress.Parse(ip), 8081);
            IPEndPoint iPEndPointFileFormat = new IPEndPoint(IPAddress.Parse(ip), 8082);
            IPEndPoint iPEndPointSaveFile = new IPEndPoint(IPAddress.Parse(ip), 8083);

            while (true) {
                TEST();
            }
        }


        public static void SaveFile(Socket socket, EndPoint endPoint, int bufferSize)
        {

            

            socket.Bind(endPoint);
            socket.Listen(5);

            var listener = socket.Accept();
            byte[] buffer = new byte[32];
            int size = 0;

            List<byte> fileBytes = new List<byte>();
            var data = new StringBuilder();
                do
                {
                    size = listener.Receive(buffer);
                data.Append(Encoding.UTF8.GetString(buffer, 0, size));
                fileBytes.AddRange(buffer);
                    
                }
                while (listener.Available > 0);
            File.WriteAllBytes($"C:\\Users\\админ\\Desktop\\файлы для тестов программ\\Новая папка\\{fileName}.{fileFormat}", fileBytes.ToArray());
            listener.Send(Encoding.UTF8.GetBytes("успех"));
            
        }


        

        public string  GetFileInfo(Socket socket, EndPoint endPoint, int bufferSize)
        {
            socket.Bind(endPoint);
            socket.Listen(5);
            var listener = socket.Accept();
            byte [] buffer = new byte[bufferSize];
            int size;
                var data = new StringBuilder();
                do
                {
                    size = listener.Receive(buffer);
                    data.Append(Encoding.UTF8.GetString(buffer, 0, size));
                }
                while (listener.Available > 0);

                listener.Send(Encoding.UTF8.GetBytes($"инфа файла {data} пелучена"));
            return data.ToString();
        }


        public string TEST() {

            int port = 8084;

            EndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            socket.Bind(endPoint);
            socket.Listen(5);

            var listener = socket.Accept();
            byte[] buffer = new byte[512];
            int size = 0;
            List<byte> fileBytes = new List<byte>();
            int count = 0;
            
            while (true)
            {
                var data = new StringBuilder();
                do
                {
                    size = listener.Receive(buffer);
                    if (count == 2) fileBytes.AddRange(buffer);
                    else data.Append(Encoding.UTF8.GetString(buffer, 0, size));
                    
                    


                }
                while (listener.Available > 0);
                Console.WriteLine(data);


                if (count == 2 && listener.Available == 0)
                {
                    File.WriteAllBytes($"C:\\Users\\админ\\Desktop\\файлы для тестов программ\\Новая папка\\{fileName}.JPG", buffer);
                    data = new StringBuilder();
                    count = 0;
                }
                
                if (count == 1)
                {
                    fileFormat = data;
                    data = new StringBuilder();
                    count++;
                }
                if (count == 0) {
                    fileName = data;
                    data = new StringBuilder(); ;
                    count++;
                }

                Console.WriteLine("всего байт" + fileBytes.Count);
            }


        }


    }
    }

