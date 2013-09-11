using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace ServerConsole1
{
    class Server
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Server is running...");
            IPAddress ip = new IPAddress(new byte[] { 127, 0, 0, 1 });
            TcpListener listener = new TcpListener(ip, 8500);

            //监听
            listener.Start();
            Console.WriteLine("Start listening on port 8500...");

            //只获取一个客户端 就中断
            while (true)
            {
                TcpClient remoteClient = listener.AcceptTcpClient();
                Console.WriteLine("Client Connected! {0} <--{1}",
                    remoteClient.Client.LocalEndPoint, remoteClient.Client.RemoteEndPoint);
            }

            Console.WriteLine("输入\"Q\"键退出.");
            ConsoleKey key;
            do
            {
                key = Console.ReadKey(true).Key;
            } while (key != ConsoleKey.Q);
        }
    }
}
