using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
namespace Client
{
    class Server
    {
        public static string MessageCurrent = "Idle";
        public static void SendFile(string fName)
        {
            try
            {
                IPAddress ip = IPAddress.Parse("192.168.1.50");//IP lấy làm server
                IPEndPoint end = new IPEndPoint(ip, 2014);
                Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

                string path = "";
                fName = fName.Replace("\\", "/");
                while (fName.IndexOf("/") > -1)
                {
                    path += fName.Substring(0, fName.IndexOf("/") + 1);
                    fName = fName.Substring(fName.IndexOf("/") + 1);
                }
                //Chuyển đổi
                byte[] fNameByte = Encoding.ASCII.GetBytes(fName);

               MessageCurrent  = "Buffering...";
                byte[] fileData = File.ReadAllBytes(path + fName);
                byte[] clientData = new byte[4 + fNameByte.Length + fileData.Length];
                byte[] fNameLen = BitConverter.GetBytes(fNameByte.Length);
                fNameLen.CopyTo(clientData, 0);
                fNameByte.CopyTo(clientData, 4);
                fileData.CopyTo(clientData, 4 + fNameByte.Length);
                MessageCurrent = "Connect to Server....";
                sock.Connect (end);
                //Gửi
                MessageCurrent = "The file is being sent....." ;
                sock .Send(clientData );
                //Đóng kn
                sock.Close (); 
                MessageCurrent ="The file was sent...";
            }
            catch(Exception ex)
            {

            }

        }
    }
}
