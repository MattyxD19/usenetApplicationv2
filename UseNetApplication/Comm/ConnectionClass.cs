using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
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

        public string ServerName
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

        public void startConnection()
        {
            byte[] sendMessage = Encoding.UTF8.GetBytes("" + "\n"); //get From TextBox
            byte[] authUser = Encoding.UTF8.GetBytes("authinfo user " + userEmail + "\n");
            byte[] authpass = Encoding.UTF8.GetBytes("authinfo PASS " + userPassword + "\n");
            byte[] userCommand = Encoding.UTF8.GetBytes("" + "\n");

            socket = new TcpClient(ServerName, ServerPort);
            ns = socket.GetStream();

            reader = new StreamReader(ns, Encoding.UTF8);
            String recieveMessage = reader.ReadLine();

            ns.Write(authUser, 0, authUser.Length);
            recieveMessage = reader.ReadLine();
            Console.WriteLine(recieveMessage);

            ns.Write(authpass, 0, authpass.Length);
            recieveMessage = reader.ReadLine();
            Console.WriteLine(recieveMessage);

            ns.Flush();

            Console.WriteLine("Got this message {0} back from the server", recieveMessage);
            
         
            ns.Close();
        }

        public string CreateMessage(string message)
        {
            socket = new TcpClient(ServerName, ServerPort);
            ns = socket.GetStream();
            reader = new StreamReader(ns, Encoding.UTF8);

            byte[] userCommand = Encoding.UTF8.GetBytes(message + "\n");

            ns.Write(userCommand, 0, userCommand.Length);

            ns.Flush();

            reader.Close();
            socket.Close();
            ns.Close();

            return message;
        }

        
        public string GetReturnMessage()
        {
            socket = new TcpClient(ServerName, ServerPort);
            ns = socket.GetStream();
            reader = new StreamReader(ns, Encoding.UTF8);
           

            String recieveMessage = reader.ReadLine();
            string returnMessage = recieveMessage;
            ns.Flush();

            reader.Close();
            socket.Close();
            ns.Close();

            return returnMessage;
        }

        /*public string closeConnectionMethod(byte[] userCommand)
        {
            socket = new TcpClient(ServerName, ServerPort);
            ns = socket.GetStream();
            reader = new StreamReader(ns, Encoding.UTF8);

            StringBuilder buildByteToString = new StringBuilder();

            for (int i = 0; i < userCommand.Length; i++)
            {
                buildByteToString.Append(userCommand[i].ToString());
            }

            if (buildByteToString.Equals("quit"))
            {
                reader.Close();
                socket.Close();
                ns.Close();
                Console.WriteLine("Connection has been closed");
                
            }
            ns.Flush();
            string closeMessage = buildByteToString.ToString(); 
            return closeMessage;
        }*/

    }
}
