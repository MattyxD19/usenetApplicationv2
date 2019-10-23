using System;
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

        //private string sendMessageToUsenet;

        //public string SendMessageToUsenet
        //{
        //    get
        //    {
        //        return sendMessageToUsenet;
        //    }
        //    set
        //    {
        //        sendMessageToUsenet = value;
        //    }
        //}

        public MainWindow()
        {
            InitializeComponent();
        }

        private string sendMessageToUsenet = "";

        ConnectionClass createConnection = new ConnectionClass();
        NewPostWindow newPost = new NewPostWindow();

        public StringBuilder sb = new StringBuilder();
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
            
            String status = createConnection.startConnection();
            if (terminal.Text.Length > 0)
            {
                terminal.AppendText(Environment.NewLine);
            }

            terminal.AppendText(status);
            InputText.IsEnabled = true;
            InputButton.IsEnabled = true;
            CreateAPost.IsEnabled = true;
        }

        private void InputButton_Click(object sender, RoutedEventArgs e)
        {
            terminal.Clear();
            sendMessageToUsenet = terminal.Text;
            if (sendMessageToUsenet.Equals("quit"))
            {
                InputText.IsEnabled = false;
                InputButton.IsEnabled = false;
            }
            else if (sendMessageToUsenet.Equals("save") || sendMessageToUsenet.Equals("Save"))
            {
                //nothing should be send to the server
                Console.WriteLine("saving: ", terminal.Text.Substring(4, terminal.Text.Length) + " to C:/temp/favorites/savedNewsGroup.txt");
            }
            else
            {
                String serverResponse = createConnection.CreateMessage(sendMessageToUsenet + "\n");
                if (terminal.Text.Length > 0)
                {
                    terminal.AppendText(Environment.NewLine);
                }
                terminal.AppendText(serverResponse);
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
                terminal.Clear();
                sendMessageToUsenet = terminal.Text;
                if (sendMessageToUsenet.Equals("quit"))
                {
                    InputText.IsEnabled = false;
                    InputButton.IsEnabled = false;
                }
                if (sendMessageToUsenet.Equals("save") || sendMessageToUsenet.Equals("Save"))
                {
                    //nothing should be send to the server
                    Console.WriteLine("saving: ", terminal.Text.Substring(4, terminal.Text.Length) + " to C:/temp/favorites/savedNewsGroup.txt");
                }
                else
                {
                    String serverResponse = createConnection.CreateMessage(sendMessageToUsenet + "\n");
                    if (terminal.Text.Length > 0)
                    {
                        terminal.AppendText(Environment.NewLine);
                    }
                    terminal.AppendText(serverResponse);
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

        public string SaveSingleNewsGroup()
        {
            String savedNewsGroup = "";
            if (terminal.Text.Substring(4, terminal.Text.Length) == "save" || terminal.Text.Substring(4, terminal.Text.Length) == "Save")
            {
                savedNewsGroup = terminal.Text.Substring(4, terminal.Text.Length);

            }

            return savedNewsGroup;
        }

    }
}
