using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace ClientConsole1
{
    class Client
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Client Running...");
            int i;
            TcpClient client;
            for (i = 0; i < 5; i++)
            {
                try
                {
                    client = new TcpClient();
                    client.Connect("localhost", 8500);
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.Read();
                    return;
                }

                //连接信息
                Console.Write("Server Connected! {0} -->{1}\n",
                    client.Client.LocalEndPoint, client.Client.RemoteEndPoint);
            }

            //按q退出
            Console.WriteLine("\n输入\"Q\"键退出.");
            ConsoleKey key;
            do
            {
                key = Console.ReadKey(true).Key;
            } while (key != ConsoleKey.Q);
        }
    }
}
