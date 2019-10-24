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
        public List<string> articleList = new List<string>();
        public List<string> messageList = new List<string>();

        public string startConnection()
        {
            
            socket = new TcpClient(serverName, serverPort);
            ns = socket.GetStream();
            reader = new StreamReader(ns, Encoding.UTF8);

            string recieveMessage = "";
            recieveMessage = reader.ReadLine();
            Console.WriteLine(recieveMessage);
            

            byte[] authUser = Encoding.UTF8.GetBytes("authinfo user " + userEmail + "\n");
            byte[] authpass = Encoding.UTF8.GetBytes("authinfo PASS " + userPassword + "\n");
           

            ns.Write(authUser, 0, authUser.Length);
            recieveMessage = reader.ReadLine();
            Console.WriteLine("Server says: " + recieveMessage);
            

            ns.Write(authpass, 0, authpass.Length);
            recieveMessage = reader.ReadLine();
            Console.WriteLine("Server says: " + recieveMessage);

            return recieveMessage;
        }

        public List<String> CreateMessage(string message)
        {
            ns = socket.GetStream();
            string recieveMessage = "";
            byte[] userCommand = Encoding.UTF8.GetBytes(message + "\n");

            ns.Write(userCommand, 0, userCommand.Length);

            if (userCommand.Equals("quit"))
            {
                Console.WriteLine("Connection is now closed");
                Console.WriteLine(reader.ReadLine());
                socket.Close();
                ns.Flush();
                ns.Close();
                reader.Close();
            }

            while (reader.Peek() >= 0)
            {
                if ((recieveMessage = reader.ReadLine()) != null)
                {
                    Console.WriteLine(recieveMessage.ToString());
                    ns.Flush();
                    messageList.Add(recieveMessage.ToString());
                }

            }

            return messageList;
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
                
            while (reader.Peek() >= 0)
            {
                if ((recieveMessage = reader.ReadLine()) != null)
                {
                    Console.WriteLine(recieveMessage.ToString());
                    ns.Flush();
                    listNews.Add(recieveMessage.ToString());
                }
                
            }
            return listNews;
        }

        public List<String> ReadArticle(string message)
        {
            string recieveMessage = "";
            ns = socket.GetStream();
            reader = new StreamReader(ns, Encoding.UTF8);
            byte[] userCommand = Encoding.UTF8.GetBytes(message + "\n");

            ns.Write(userCommand, 0, userCommand.Length);

            recieveMessage = reader.ReadLine();
            Console.WriteLine(recieveMessage);
            ns.Flush();

            while (reader.Peek() >= 0)
            {
                if ((recieveMessage = reader.ReadLine()) != null)
                {
                    Console.WriteLine(recieveMessage.ToString());
                    ns.Flush();
                    articleList.Add(recieveMessage.ToString());
                }

            }
            return articleList;

        }

        public void createPost()
        {

        }

    }
}
