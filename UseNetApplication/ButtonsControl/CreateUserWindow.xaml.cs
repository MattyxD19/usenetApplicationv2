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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace UseNetApplication.ButtonsControl
{
    /// <summary>
    /// Interaction logic for CreateUserWindow.xaml
    /// </summary>
    public partial class CreateUserWindow : Window
    {
        public CreateUserWindow()
        {
            InitializeComponent();
        }

        /*
         * before the user can use the newsreader, a new user is required
         * when the user has typed their information inside the newly opened window
         * and pressed "save user" the information will be stored in a textfile at:
         * c:\temp\username.txt
         * 
         * the textfile is also used when the user wants to connect to the usenet
         * where the following is read
         * server name
         * server port
         * email
         * password
         * 
         * if a user creates a new user with the same username an error will say
         * that the current username already exits, it is done to avoid duplicates
         */

        private void SaveNewUser_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            String path = @"c:\temp\" + UsernameTextBox.Text + ".txt";
            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(NewsServerNameTextBox.Text);
                    sw.WriteLine(ServerPortTextBox.Text);
                    sw.WriteLine(EmailTextBox.Text);
                    sw.WriteLine(PasswordTextBox.Text);
                    sw.WriteLine(UsernameTextBox.Text);
                }
            }
            else if (File.Exists(path))
            {
                MessageBox.Show("User already exits!");
            }

            using (StreamReader sr = File.OpenText(path))
            {
                string tempString = "";
                while ((tempString = sr.ReadLine()) != null)
                {
                    Console.WriteLine(tempString);
                }
            }

            mainWindow.Show();
            this.Close();
        }
    }
}
