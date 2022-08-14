using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    class Program
    {
        static int port = 8005; //port for receiving incoming requests 
        static void Main(string[] args)
        {
            //get socket start addresses
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);

            //create socket
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                //we connect the socket with a local point on which we will receive data
                listenSocket.Bind(ipPoint);

                //start listening
                listenSocket.Listen(10);

                Console.WriteLine("The server is running. Waiting for connections...");


                while (true)
                {
                    Socket handler = listenSocket.Accept();
                    //get message
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0; //number of bytes received
                    byte[] data = new byte[256];//buffer for received data

                    do
                    {
                        bytes = handler.Receive(data);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (handler.Available > 0);

                    Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + builder.ToString());

                    //send answer
                    string message = "Your message has been delivered";
                    data = Encoding.Unicode.GetBytes(message);
                    handler.Send(data);
                    //close socket
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}