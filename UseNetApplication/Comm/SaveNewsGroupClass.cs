using System;
using System.IO;

namespace UseNetApplication.Comm
{
    class SaveNewsGroupClass
    {
        public string WriteNewsGroupToDoc(String group)
        {
            try
            {
                String path = @"c:\temp\savedNewsGroup.txt";
                if (!File.Exists(path))
                {
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        
                        sw.WriteLine(group + "\n");
                      
                        sw.Close();
                    }
                }
                else if (File.Exists(path))
                {
                    using (TextWriter tw = new StreamWriter(@"c:\temp\savedNewsGroup.txt", append:true))
                    {
                        tw.WriteLine(group.ToString());
                    }
                }
            }
            catch
            {
                Console.WriteLine("Error: Nothing was written check SaveNewsGroupClass");
            }
            return "Document saved";
        }

        public string RemoveNewsGroup(string name)
        {

            String path = @"c:\temp\savedNewsGroup.txt";
            string tempFile = Path.GetTempFileName();

            using (var sr = new StreamReader(path))
            using (var sw = new StreamWriter(tempFile))
            {
                string line = "";

                while ((line = sr.ReadLine()) != null)
                {
                    if (line != name)
                        sw.WriteLine(line);
                }
            }

            File.Delete(path);
            File.Move(tempFile, path);

            return "news group has been removed";
        }

    }
}
