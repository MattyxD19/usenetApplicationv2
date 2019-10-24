using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

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
        public List<string> listNews = new List<string>();
        public List<string> articleList = new List<string>();
        public List<string> messageList = new List<string>();


        /**
         *when the user clicks on the "connect" button in the main window
         * and have selected the desired file, the fil will then be read for
         * servername
         * serverport
         * email
         * password
         * then each value is assigned in this method
         * by doing this the user doesnt have to enter a single line in the inputtextBox
         */

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

        /**
         * since there was huge problems with recieving multiple lines from the server
         * a list was created so that the main window can iterate throug the list
         * and add each item to the termnial display
         */

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


            recieveMessage = reader.ReadLine();
            Console.WriteLine(recieveMessage);
            messageList.Add(recieveMessage);

            while (reader.Peek() >= 0)
            {
                
                if ((recieveMessage = reader.ReadLine()) != null)
                {
                    
                    Console.WriteLine(recieveMessage);
                    messageList.Add(recieveMessage);
                    ns.Flush();
                }

            }
            
            return messageList;
        }

        /**
         * when the user types "list" the listview on the left will be populated
         * a command is send to the server, where all the newsgroups then are iterated
         * and added to the listnews list
         * and in return send the main window
         */

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

        /**
         * To make sure that every line of text is returned
         * the whole article is added to yet another list
         * which is also iterated through like the previous two lists
         */

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

        /*
         * the user is not yet able to post a new article
         * but the code would be entered here
         */
        public void createPost()
        {

        }

    }
}
