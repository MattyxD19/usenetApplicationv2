using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using UseNetApplication.ButtonsControl;
using UseNetApplication.Comm;

namespace UseNetApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }
        // A global string used all over in this class
        private string sendMessageToUsenet = "";

        
        ConnectionClass createConnection = new ConnectionClass();
        NewPostWindow newPost = new NewPostWindow();
        SaveNewsGroupClass saveGroup = new SaveNewsGroupClass();

        private void CreateUser_Click(object sender, RoutedEventArgs e)
        {
            CreateUserWindow objCreationWindow = new CreateUserWindow();
            this.Close();
            objCreationWindow.Show();

        }

        private void ViewUserFiles_click(object sender, RoutedEventArgs e)
        {
            String path = @"c:\temp";
            OpenFileDialog ofd = new OpenFileDialog();
            if (Directory.Exists(path))
            {
                ofd.InitialDirectory = path;
            }
            ofd.ShowDialog();
        }

        /*
         * The user has to click the "connect" button before a connection can be made
         * when the button is clicked, a filedialog will open and the user has to select
         * the textfile with the username they want to use
         * 
         * the selected textfile is then read, and the information is passed to the connection class
         * 
         * The terminal display will the show "281 Ok." if they are connected
         * 
         * The disabled buttons and textbox will then be enabled as well
         */

        private void ConnectUsenetButton_Click(object sender, RoutedEventArgs e)
        {
            terminal.Clear();
            NewsgroupList.Items.Clear();
            OpenFileDialog selectUser = new OpenFileDialog();

            String path = @"c:\temp";

            selectUser.InitialDirectory = path;
            selectUser.ShowDialog();

            StreamReader readUserFile = new StreamReader(selectUser.FileName);

            createConnection.ServerName = readUserFile.ReadLine();
            createConnection.ServerPort = Int32.Parse(readUserFile.ReadLine());
            createConnection.UserEmail = readUserFile.ReadLine();
            createConnection.UserPassword = readUserFile.ReadLine();

            string pendingMessage = createConnection.startConnection();
            terminal.Clear();
            terminal.AppendText(pendingMessage);


            InputText.IsEnabled = true;
            InputButton.IsEnabled = true;
            CreateAPost.IsEnabled = true;
            ListPopular.IsEnabled = true;
        }
        
        /*
         * Since the user can decide to press the "send" button to send their command
         * instead of pressing enter, two methods has to be created with the same code
         * 
         * they both read the input textbox for a command, where it then decides what to do
         * 
         * some commands have been defined to do specific operations so that the user
         * can recieve all the data which the server will send
         */

        private void InputButton_Click(object sender, RoutedEventArgs e)
        {
            terminal.Clear();
            sendMessageToUsenet = InputText.Text;
            Console.WriteLine(sendMessageToUsenet);
            if (sendMessageToUsenet.Equals("quit"))
            {
                InputText.IsEnabled = false;
                InputButton.IsEnabled = false;
            }
            if (sendMessageToUsenet.Equals("list"))
            {
                terminal.Clear();
                NewsgroupList.Items.Clear();
                createConnection.CreateList(sendMessageToUsenet + "\n");
                Thread.Sleep(250);
                foreach (var item in createConnection.listNews)
                {

                    NewsgroupList.Items.Add(item);
                }
                terminal.Clear();
            }
            if (sendMessageToUsenet.Contains("save"))
            {
                TerminalCommands(sendMessageToUsenet);
            }
            if (sendMessageToUsenet.Contains("remove"))
            {
                TerminalCommands(sendMessageToUsenet);
            }
            if (sendMessageToUsenet.Contains("article"))
            {
                TerminalCommands(sendMessageToUsenet);
            }
            if (sendMessageToUsenet.Contains("search"))
            {
                TerminalCommands(sendMessageToUsenet);
            }

            if (InputText.Text == "" || InputText.Text == null)
            {
                InputText.BorderBrush = Brushes.Red;
            }
            else
            {
                createConnection.listNews.Clear();
                createConnection.CreateMessage(sendMessageToUsenet + "\n");
                terminal.Clear();
                foreach (var item in createConnection.messageList)
                {
                    terminal.AppendText("\n" + item);
                }
                createConnection.messageList.Clear();
            }
            InputText.Clear();
        }

        /*
        * Since the user can decide to press the "send" button to send their command
        * instead of pressing enter, two methods has to be created with the same code
        * 
        * they both read the input textbox for a command, where it then decides what to do
        * 
        * some commands have been defined to do specific operations so that the user
        * can recieve all the data which the server will send
        * 
        * this eventhandler has also been modified to use keyevent.Enter as a valid 
        * keypress instead of the "up" arrowkey
        */

        private void InputText_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter) {
                terminal.Clear();

                sendMessageToUsenet = InputText.Text;
                Console.WriteLine(sendMessageToUsenet);

                if (sendMessageToUsenet.Equals("quit"))
                {
                    InputText.IsEnabled = false;
                    InputButton.IsEnabled = false;
                }
                if (sendMessageToUsenet.Equals("list"))
                {
                    terminal.Clear();
                    NewsgroupList.Items.Clear();
                    createConnection.CreateList(sendMessageToUsenet + "\n");
                    Thread.Sleep(250);
                    foreach (var item in createConnection.listNews)
                    {
                        NewsgroupList.Items.Add(item);
                    }
                    createConnection.listNews.Clear();
                    terminal.Clear();
                }
                if (sendMessageToUsenet.Contains("save"))
                {
                    TerminalCommands(sendMessageToUsenet);
                }
                if (sendMessageToUsenet.Contains("remove"))
                {
                    TerminalCommands(sendMessageToUsenet);
                }
                if (sendMessageToUsenet.Contains("article"))
                {
                    TerminalCommands(sendMessageToUsenet);
                }
                if (sendMessageToUsenet.Contains("search"))
                {
                    TerminalCommands(sendMessageToUsenet);
                }
                if (InputText.Text == "" || InputText.Text == null)
                {
                    InputText.BorderBrush = Brushes.Red;
                }
                else
                {
                    createConnection.listNews.Clear();
                    createConnection.CreateMessage(sendMessageToUsenet + "\n");
                    terminal.Clear();
                    foreach (var item in createConnection.messageList)
                    {
                        terminal.AppendText("\n" + item);
                    }
                    createConnection.messageList.Clear();
                    
                }
                InputText.Clear();
            }
        }

        private void CreateNewPost_click(object sender, RoutedEventArgs e)
        {
            newPost.Show();
            this.Close();
        }

        /*
         * the user can decide to have the NewsGroupList populated
         * with their favorite newsgroup
         * 
         * they just have to click the "show favorites" button
         * and then the textfile called "savedNewsGroup" will be
         * read for information, whereafter the NewsGroupList is 
         * populated
         */

        private void ListPopular_Click(object sender, RoutedEventArgs e)
        {
            List<string> favoriteNewsGroup = new List<string>();
            NewsgroupList.Items.Clear();
            string path = @"c:\temp\savedNewsGroup.txt";
            using (StreamReader sr = File.OpenText(path))
            {
                string tempString = "";
                while ((tempString = sr.ReadLine()) != null)
                {
                    favoriteNewsGroup.Add(tempString);
                }
            }

            foreach (var item in favoriteNewsGroup)
            {
                NewsgroupList.Items.Add(item);
            }
        }


        /*
         * This method contains predetermined commands that the user can send to the server
         * 
         */

        public void TerminalCommands(string command)
        {
            if (command.Contains("save"))
            {
                //nothing should be send to the server
                string savedName = command.Remove(0, 5);
                saveGroup.WriteNewsGroupToDoc(savedName);
                Console.WriteLine("saving: " + savedName + " to C:/temp/savedNewsGroup.txt");
            }
            if (command.Contains("remove"))
            {
                //nothing should be send to the server
                string removeName = command.Remove(0, 7);
                saveGroup.RemoveNewsGroup(removeName);
                Console.WriteLine("removing: " + removeName + " from C:/temp/savedNewsGroup.txt");
            }
            if (command.Contains("article"))
            {
                createConnection.ReadArticle(command);
                foreach (var item in createConnection.articleList)
                {
                    string pendingText = item;
                    terminal.Clear();
                    terminal.AppendText("\n" + pendingText);
                    
                }
            }

            if (command.Contains("search"))
            {
                string searchName = command.Remove(0, 7);
                //not functional yet...
                foreach (string item in NewsgroupList.Items)
                {
                    if (item.Contains(searchName))
                    {
                        NewsgroupList.Items.Add(item);
                    }                  
                }
            }

        }

        /*
         * the user should also be able to search the NewsGroupList for newsgroups
         * so they easily can find them for whatever reason they see fit
         */

        private void NewsgroupList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            
            Console.WriteLine("item clicked: " + NewsgroupList.SelectedItem.ToString());
            string selectedNewsGroup = NewsgroupList.SelectedItems.ToString();
            Console.WriteLine(selectedNewsGroup.ToString());
            string message = "group " + selectedNewsGroup.ToString();
            createConnection.CreateMessage(message + "\n");
            Console.WriteLine(message);
        }
    }
}
