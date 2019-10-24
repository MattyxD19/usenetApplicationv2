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

        private string recieveMessageUsenet;
        public string RecieveMessageUsenet
        {
            get
            {
                return recieveMessageUsenet;
            }
            set
            {
                recieveMessageUsenet = value;
            }
        }

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

        private void ConnectUsenetButton_Click(object sender, RoutedEventArgs e)
        {
            terminal.AcceptsReturn = true;
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

            string currentText = terminal.Text;
            string pendingMessage = createConnection.startConnection();
            terminal.Clear();
            terminal.AppendText(currentText + "\n" + pendingMessage);


            InputText.IsEnabled = true;
            InputButton.IsEnabled = true;
            CreateAPost.IsEnabled = true;
            ListPopular.IsEnabled = true;
        }

        private void InputButton_Click(object sender, RoutedEventArgs e)
        {
            terminal.AcceptsReturn = true;
            sendMessageToUsenet = InputText.Text;
            Console.WriteLine(sendMessageToUsenet);
            if (sendMessageToUsenet.Equals("quit"))
            {
                InputText.IsEnabled = false;
                InputButton.IsEnabled = false;
            }
            if (sendMessageToUsenet.Equals("list"))
            {
                NewsgroupList.Items.Clear();
                createConnection.CreateList(sendMessageToUsenet + "\n");
                Thread.Sleep(250);
                foreach (var item in createConnection.listNews)
                {
                    NewsgroupList.Items.Add(item);
                }


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
                createConnection.CreateMessage(sendMessageToUsenet + "\n");
                foreach (var item in createConnection.messageList)
                {
                    string currentText = terminal.Text;
                    string pendingItem = item;
                    terminal.AppendText(currentText + "\n" + pendingItem);
                }
            }
            InputText.Clear();
        }

        private void InputText_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter) {
                terminal.AcceptsReturn = true;
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
                    createConnection.CreateMessage(sendMessageToUsenet + "\n");

                    foreach (var item in createConnection.messageList)
                    {
                        //string currentText = terminal.Text;
                        string pendingItem = item.ToString();
                        terminal.AppendText(pendingItem);
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
                    string currentText = InputText.Text;
                    string pendingText = item;
                    terminal.Clear();
                    terminal.AppendText(currentText + "\n" + pendingText);
                    
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

        private void NewsgroupList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("item clicked: " + NewsgroupList.SelectedItems);
            string selectedNewsGroup = NewsgroupList.SelectedItems.ToString();
            string[] groupSelected = selectedNewsGroup.Split(' ');
            Console.WriteLine(groupSelected);
            
        }
    }
}
