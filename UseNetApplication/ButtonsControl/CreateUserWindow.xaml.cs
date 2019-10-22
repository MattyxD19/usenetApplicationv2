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

        public MainWindow mainWindow = new MainWindow();

        private void SaveNewUser_Click(object sender, RoutedEventArgs e)
        {
            String path = @"c:\temp\" + UsernameTextBox.Text + ".txt";
            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(NewsServerNameTextBox.Text);
                    sw.WriteLine(ServerPortTextBox.Text);
                    sw.WriteLine(UsernameTextBox.Text);
                    sw.WriteLine(PasswordTextBox.Text);
                    sw.WriteLine(EmailTextBox.Text);
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
