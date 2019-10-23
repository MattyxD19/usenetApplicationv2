using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseNetApplication.Comm
{
    class SaveNewsGroupClass
    {
        public void WriteNewsGroupToDoc()
        {
            MainWindow mainWindow = new MainWindow();

            DirectoryInfo dir = new DirectoryInfo(@"c:\temp\");
            dir.CreateSubdirectory("favorites");
            String path = @"c:\temp\favorites\" + "savedNewsGroup" + ".txt";
            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(mainWindow.SaveSingleNewsGroup());
                    sw.Close();
                }
            }
            else if (File.Exists(path))
            {
                using (StreamWriter sw2 = new StreamWriter(path))
                {
                    sw2.WriteLine(mainWindow.SaveSingleNewsGroup());
                    sw2.Close();
                }
            }

            
        }
    }
}
