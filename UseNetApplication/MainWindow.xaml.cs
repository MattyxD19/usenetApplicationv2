using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
            //terminal.Clear();
            //NewsgroupList.Items.Clear();
            OpenFileDialog selectUser = new OpenFileDialog();

            String path = @"c:\temp";

            selectUser.InitialDirectory = path;
            selectUser.ShowDialog();

            StreamReader readUserFile = new StreamReader(selectUser.FileName);

            createConnection.ServerName = readUserFile.ReadLine();
            createConnection.ServerPort = Int32.Parse(readUserFile.ReadLine());
            createConnection.UserEmail = readUserFile.ReadLine();
            createConnection.UserPassword = readUserFile.ReadLine();
            
            createConnection.startConnection();
           
            
            InputText.IsEnabled = true;
            InputButton.IsEnabled = true;
            CreateAPost.IsEnabled = true;
            ListPopular.IsEnabled = true;
        }

        private void InputButton_Click(object sender, RoutedEventArgs e)
        {
            sendMessageToUsenet = InputText.Text;
            Console.WriteLine(sendMessageToUsenet);
            if (sendMessageToUsenet.Equals("quit"))
            {
                InputText.IsEnabled = false;
                InputButton.IsEnabled = false;
            }
            else if (sendMessageToUsenet.Equals("list"))
            {
                foreach (var item in createConnection.CreateList(sendMessageToUsenet + "\n"))
                {
                    terminal.AppendText(item);
                }
                terminal.Clear();

            }

            if (sendMessageToUsenet.Contains("save"))
            {
                //nothing should be send to the server
                string savedName = sendMessageToUsenet.Remove(0, 5);
                saveGroup.WriteNewsGroupToDoc(savedName);
                Console.WriteLine("saving: " + savedName + " to C:/temp/savedNewsGroup.txt");
            }

            if (sendMessageToUsenet.Contains("remove"))
            {
                //nothing should be send to the server
                string removeName = sendMessageToUsenet.Remove(0, 7);
                saveGroup.RemoveNewsGroup(removeName);
                Console.WriteLine("removing: " + removeName + " from C:/temp/savedNewsGroup.txt");
            }



            if (InputText.Text == "" || InputText.Text == null)
            {
                InputText.BorderBrush = Brushes.Red;
            }
            InputText.Clear();
        }

        private void InputText_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter) {
                
                sendMessageToUsenet = InputText.Text;
                Console.WriteLine(sendMessageToUsenet);
                if (sendMessageToUsenet.Equals("quit"))
                {
                    InputText.IsEnabled = false;
                    InputButton.IsEnabled = false;
                }
                else if (sendMessageToUsenet.Equals("list"))
                {
                    foreach (var item in createConnection.CreateList(sendMessageToUsenet + "\n"))
                    {
                        terminal.AppendText(item);
                    }
                    terminal.Clear();

                }

                if (sendMessageToUsenet.Contains("save"))
                {
                    //nothing should be send to the server
                    string savedName = sendMessageToUsenet.Remove(0, 5);
                    saveGroup.WriteNewsGroupToDoc(savedName);
                    Console.WriteLine("saving: " + savedName + " to C:/temp/savedNewsGroup.txt");
                }

                if (sendMessageToUsenet.Contains("remove"))
                {
                    //nothing should be send to the server
                    string removeName = sendMessageToUsenet.Remove(0, 7);
                    saveGroup.RemoveNewsGroup(removeName);
                    Console.WriteLine("removing: " + removeName + " from C:/temp/savedNewsGroup.txt");
                }
                


                if (InputText.Text == "" || InputText.Text == null)
                {
                    InputText.BorderBrush = Brushes.Red;
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
    }
}
