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
        static FileWoker fileWoker = new FileWoker();


        static void Main(string[] args)
        {

            Console.WriteLine("ВВедите путь до файла");
            string path = Console.ReadLine();

            try
            {
                fileWoker.SendFile(path);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            Console.Clear();

        }

       

       
    }
}
