using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace TTDH
{
    public class ClientAPI
    {
        private string ServerName;

        public ClientAPI(string server)
        {
            ServerName = server;            
        }
        //public string GetData()
        //{
        //    TcpClient client = new TcpClient(ServerName, 3000);
        //    String data = null;
        //    try
        //    {
        //        NetworkStream stream = client.GetStream();
        //        byte[] bytes = new byte[1024];                
        //        int readByte;
        //        System.Collections.Generic.List<byte> lst = new System.Collections.Generic.List<byte>();
        //        while ((readByte = stream.Read(bytes, 0, bytes.Length)) != 0)
        //        {
        //            var copy = new byte[readByte];
        //            Array.Copy(bytes, 0, copy, 0, readByte);
        //            lst.AddRange(copy);
        //        }
        //        // Translate data bytes to a ASCII string.
        //        data = System.Text.Encoding.UTF8.GetString(lst.ToArray());
        //        lst = null;
        //    }
        //    finally
        //    {
        //        client.Close();
        //    }
        //    return data;
        //}
        //public int SendCommand(string msg)
        //{
        //    TcpClient client = new TcpClient(ServerName, 3000);
        //    try
        //    {
        //        // Translate the passed message into ASCII and store it as a Byte array.
        //        Byte[] data = System.Text.Encoding.UTF8.GetBytes(msg);
        //        // Get a client stream for reading and writing. 
        //        client.ReceiveBufferSize = data.Length;
        //        NetworkStream stream = client.GetStream();
        //        // Send the message to the connected TcpServer. 
        //        stream.Write(data, 0, data.Length);
        //        Console.WriteLine("Sent data to server with " + data.Length.ToString() + " bytes.");
        //        stream.Close();
        //    }
           
        //    finally
        //    {
        //        client.Close();
        //    }
        //    Console.WriteLine("\n Press Enter to continue...");
        //    Console.Read();
        //    return 1;

        //}
        public string GetDataFromCmd(string msg)
        {
            TcpClient client = new TcpClient(ServerName, 3000);
            string dataRecieve = null;
            try
            {
                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.UTF8.GetBytes(msg);
                // Get a client stream for reading and writing. 
                client.ReceiveBufferSize = data.Length;
                NetworkStream stream = client.GetStream();
                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);
                //Get data                
                byte[] bytes = new byte[1024];
                int readByte;
                System.Collections.Generic.List<byte> lst = new System.Collections.Generic.List<byte>();
                while ((readByte = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    var copy = new byte[readByte];
                    Array.Copy(bytes, 0, copy, 0, readByte);
                    lst.AddRange(copy);
                }                
                // Translate data bytes to a ASCII string.
                dataRecieve = System.Text.Encoding.UTF8.GetString(lst.ToArray());
                lst = null;
                //close
                stream.Close();
            }
            catch
            {

            }           
            
            finally
            {
                client.Close();
            }
            return dataRecieve;
        }
    }
}
