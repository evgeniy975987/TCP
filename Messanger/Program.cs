using System;
using System.Threading.Tasks;
using Client;

namespace Messanger
{
    class Program
    {
        static void Main(string[] args)
        {
            FileWoker fileWoker = new FileWoker();
            Client.Messanger messanger = new Client.Messanger();
           

            Console.WriteLine("1. отослать сообщение");
            Console.WriteLine("2. отослать файла");

            if (Console.ReadLine() == "1")
            {
                Console.Clear();
                Console.WriteLine("ВВедите сообщение");

                messanger.SendMessage(Console.ReadLine());
            }
            if (Console.ReadLine() == "2") fileWoker.SendFile(@"C:\Users\админ\Desktop\файлы для тестов программ\2.JPG");


        }
    }
}
