using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UseNetApplication.Comm
{
    class ConnectionClass
    {

        private string serverName;
        private int serverPort;
        private string userEmail;
        private string userPassword;
        private string returnMessage;

        public  string ServerName
        {
            get
            {
                return serverName;
            }
            set
            {
                serverName = value;
            }
        }
        public int ServerPort
        {
            get
            {
                return serverPort;
            }
            set
            {
                serverPort = value;
            }
        }

        public string UserEmail
        {
            get
            {
                return userEmail;
            }
            set
            {
                userEmail = value;
            }
        }

        public string UserPassword
        {
            get
            {
                return userPassword;
            }
            set
            {
                userPassword = value;
            }
        }

        public string ReturnMessage
        {
            get
            {
                return returnMessage;
            }
            set
            {
                returnMessage = value;
            }
        }


  
        public TcpClient socket = null;
        public NetworkStream ns = null;
        public StreamReader reader = null;
        public StringBuilder buildByteToString = new StringBuilder();
        public List<string> listNews = new List<string>();

        public void startConnection()
        {
            MainWindow main = new MainWindow();
            socket = new TcpClient(serverName, serverPort);
            ns = socket.GetStream();
            reader = new StreamReader(ns, Encoding.UTF8);

            string recieveMessage = "";
            recieveMessage = reader.ReadLine();
            Console.WriteLine(recieveMessage);
            main.terminal.Text = recieveMessage;

            byte[] authUser = Encoding.UTF8.GetBytes("authinfo user " + userEmail + "\n");
            
            ns.Flush();

            byte[] authpass = Encoding.UTF8.GetBytes("authinfo PASS " + userPassword + "\n");
           
            ns.Flush();

            ns.Write(authUser, 0, authUser.Length);
            main.terminal.AppendText(reader.ReadLine());
            Console.WriteLine("Server says: " + main.RecieveMessageUsenet);
            ns.Flush();
            ns.Write(authpass, 0, authpass.Length);
            main.terminal.AppendText(reader.ReadLine());
            Console.WriteLine("Server says: " + main.RecieveMessageUsenet);
            
            
            //while(ns.DataAvailable)
            //{
            //    Thread.Sleep(25);
            //    recieveMessage = reader.ReadLine();
            //    main.RecieveMessageUsenet = recieveMessage;
            //    Console.WriteLine(main.RecieveMessageUsenet);
            //}
            ns.Flush();
            recieveMessage = main.RecieveMessageUsenet;
            //return recieveMessage;
        }

        public string CreateMessage(string message)
        {
            ns = socket.GetStream();
            string recieveMessage = "";
            byte[] userCommand = Encoding.UTF8.GetBytes(message + "\n");

            ns.Write(userCommand, 0, userCommand.Length);

            if (ns.DataAvailable)
            {
                if (ns.CanRead)
                {
                    recieveMessage = reader.ReadLine();
                    Console.WriteLine("Server says: " + recieveMessage);
                    ns.Flush();
                }
                
            }

            if (userCommand.Equals("quit"))
            {
                Console.WriteLine("Connection is now closed");
                Console.WriteLine(reader.ReadLine());
                socket.Close();
                ns.Flush();
                ns.Close();
                reader.Close();

            }

            Console.WriteLine(recieveMessage);
            return recieveMessage;
        }

        public List<String> CreateList(string message)
        {
            String recieveMessage = "";
            ns = socket.GetStream();
            reader = new StreamReader(ns, Encoding.UTF8);
            byte[] userCommand = Encoding.UTF8.GetBytes(message + "\n");

            ns.Write(userCommand, 0, userCommand.Length);

            recieveMessage = reader.ReadLine();
            Console.WriteLine("Server says: " + recieveMessage);
            ns.Flush();
                
            while ((recieveMessage = reader.ReadLine()).Split('\n') != null)
            {
                //buildByteToString.Append(recieveMessage);
                Console.WriteLine(recieveMessage.ToString());
                ns.Flush();
                listNews.Add(recieveMessage.ToString());
            }
               

            return listNews;

        }

        public void createPost()
        {

        }

    }
}
