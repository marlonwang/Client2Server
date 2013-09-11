using System;
using System.Text;
using System.Net.Sockets;

namespace ClientTransfer
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Client running ...");
            TcpClient client;
            try
            {
                client = new TcpClient();
                client.Connect("localhost", 8500);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.Read();//pause
                return;
            }

            //打印链接的服务信息
            Console.WriteLine("Server Connected! {0} --> {1}",
                client.Client.LocalEndPoint, client.Client.RemoteEndPoint);
            //string msg = "\"Welcome To TraceFact.Net\"";
            
            string msg;
            //1. 多个客户端一条请求
            /*
               msg=Console.ReadLine();
            */

            //2. 一个客户端多条请求
            Console.Write("=============================\n");
            Console.Write("Menu : S - Send, X - Exit  ：\n");
            Console.Write("=============================\n");
            Console.Write("[S / X]:");
            NetworkStream streamToServer = client.GetStream();
            ConsoleKey key; //判断输入 S或 E
            try
            {
                do
                {
                    key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.S)
                    {
                        Console.Write("\nInput the Message : "); //获取收入字符串
                        msg = Console.ReadLine();

                        byte[] buffer = Encoding.Unicode.GetBytes(msg); //获取缓存
                        streamToServer.Write(buffer, 0, buffer.Length);  //发给服务器

                        Console.Write("Sent :{0}", msg);
                        Console.Write("\nPress s to continue sending message:");
                    }
                }
                while (key != ConsoleKey.X); //X 退出
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.Read();//pause
                return;
            }
            /*
            byte[] buffer = Encoding.Unicode.GetBytes(msg); //获得缓存
            streamToServer.Write(buffer, 0, buffer.Length);//发给服务器
            Console.Write("Sent :{0}", msg);
            */
            
            Console.Read();
        }
    }
}
