using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerTransfer
{
    class Program
    {
        static void Main(string[] args)
        {
            const int BufferSize = 8192;  //缓存大小  8192字节

            ConsoleKey key;   //用于 向客户端 回发消息

            Console.WriteLine("Server  is  running ...");
            IPAddress ip = new IPAddress(new byte[] { 127, 0, 0, 1 });
            TcpListener listener = new TcpListener(ip, 8500);
            //开始侦听
            listener.Start();
            Console.WriteLine("Start Listening on port 8500...");

            //1. 多个客户端一条请求
            /*
            do
            {
                //获取一个连接
                TcpClient remoteClient = listener.AcceptTcpClient();

                //显示连接
                Console.WriteLine("Client Connected! {0} <-- {1}",
                    remoteClient.Client.LocalEndPoint, remoteClient.Client.RemoteEndPoint);

                //获得流，写入buffer中
                NetworkStream streamToClient = remoteClient.GetStream();
                byte[] buffer = new byte[BufferSize];
                int bytesRead = streamToClient.Read(buffer, 0, BufferSize);
                Console.WriteLine("Reading data,{0} bytes...", bytesRead);

                //获得请求的字符串
                string msg = Encoding.Unicode.GetString(buffer, 0, bytesRead);
                Console.WriteLine("recetved: {0}", msg);
                
            }
            while (true);
             * 
            */

            //2. 一个客户端多条请求

            /*
             
            //获取一个连接
            TcpClient remoteClient = listener.AcceptTcpClient();
            //显示连接
            Console.WriteLine("Client Connected! {0} <-- {1}",
                remoteClient.Client.LocalEndPoint, remoteClient.Client.RemoteEndPoint);

            //获得流，写入buffer中
            NetworkStream streamToClient = remoteClient.GetStream();
            try
            {
                do
                {

                    byte[] buffer = new byte[BufferSize];
                    int bytesRead = streamToClient.Read(buffer, 0, BufferSize);
                    Console.WriteLine("Reading data,{0} bytes...", bytesRead);

                    //获取请求的字符串
                    string msg = Encoding.Unicode.GetString(buffer, 0, bytesRead);
                    Console.WriteLine("received: {0}", msg);

                }
                while (true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.Read();//pause
                return;
            }
           */

            //3. 一个客户端 多条请求 并回复客户端

            //获取一个连接
            TcpClient remoteClient = listener.AcceptTcpClient();

            //显示连接
            Console.WriteLine("Client Connected! {0} <-- {1}",
                remoteClient.Client.LocalEndPoint, remoteClient.Client.RemoteEndPoint);

            //获得流，
            NetworkStream streamToClient = remoteClient.GetStream();

            do
            {
                //写入buffer中
                byte[] buffer = new byte[BufferSize];
                int bytesRead;
                try
                {
                    lock (streamToClient)
                        bytesRead = streamToClient.Read(buffer, 0, BufferSize);
                    if (bytesRead == 0)
                        throw new Exception("读到0字节");
                    Console.WriteLine("Reading data, {0} bytes ...", bytesRead);

                    //获得请求的的字符串
                    string msg = Encoding.Unicode.GetString(buffer, 0, bytesRead);
                    Console.WriteLine("received: {0}", msg);

                    //若为字母 则 转成大写并发送

                    msg = msg.ToUpper();
                    buffer = Encoding.Unicode.GetBytes(msg);
                    lock (streamToClient)
                    {
                        streamToClient.Write(buffer, 0, buffer.Length);
                    }
                    Console.WriteLine("Sentback in upper :{0}", msg);
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    break;
                }
            } while (true);

            streamToClient.Dispose();
            remoteClient.Close();

            //退出
            Console.WriteLine("输入\"Q\"键退出.");//while 循环 此处执行不到

            do
            {
                key = Console.ReadKey(true).Key;
            } while (key != ConsoleKey.Q);
        }
    }
}
