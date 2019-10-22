using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UseNetApplication.ButtonsControl;
using UseNetApplication.Comm;

namespace UseNetApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private string sendMessageToUsenet;

        public string SendMessageToUsenet
        {
            get
            {
                return sendMessageToUsenet;
            }
            set
            {
                sendMessageToUsenet = value;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        ConnectionClass createConnection = new ConnectionClass();

        private void CreateUser_Click(object sender, RoutedEventArgs e)
        {
            CreateUserWindow objCreationWindow = new CreateUserWindow();
            this.Close();
            objCreationWindow.Show();

        }

        private void DeleteUserFileBrowser_Click(object sender, RoutedEventArgs e)
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
            
            terminal.AppendText(createConnection.GetReturnMessage());
        }

        private void InputButton_Click(object sender, RoutedEventArgs e)
        {
            sendMessageToUsenet = InputText.Text;
            createConnection.CreateMessage(sendMessageToUsenet);
            InputText.Clear();
            terminal.AppendText(createConnection.GetReturnMessage());
        }

        private void InputText_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter) {
                sendMessageToUsenet = InputText.Text;
                createConnection.CreateMessage(sendMessageToUsenet);
                InputText.Clear();
                terminal.AppendText(createConnection.GetReturnMessage());
            }
        }
    }
}
