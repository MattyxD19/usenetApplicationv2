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
    /// Interaction logic for NewPostWindow.xaml
    /// </summary>
    public partial class NewPostWindow : Window
    {
        public NewPostWindow()
        {
            InitializeComponent();
        }

        /*
         * When the user is done writing a new article
         * it will be saved in the "articles" folder at c:\temp\Articles
         * 
         * it is not fully complete yet
         */

        private void SavePost_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            String path = @"c:\temp\articles\" + HeaderTitle.Text + ".txt";
            if (!File.Exists(path))
            {
                DirectoryInfo dir = new DirectoryInfo(@"c:\temp\");
                dir.CreateSubdirectory("Articles");
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.Write(PostTextBox.Text);
                }
            }
            else if (File.Exists(path))
            {
                MessageBox.Show("article already exits!");
            }

            using (StreamReader sr = File.OpenText(path))
            {
                string tempString = "";
                while ((tempString = sr.ReadLine()) != null)
                {
                    Console.WriteLine(tempString);
                }
            }

            this.Close();
            mainWindow.Show();
        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();
        }
    }
}
