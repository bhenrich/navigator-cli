using System.Collections.Generic;
using System.IO;

namespace Navigator.Directories
{
    public class Crawler
    {
        public static List<string> GetDirectories(string path)
        {
            return new List<string>(Directory.GetDirectories(path));
        }

        public static List<string> GetFiles(string path)
        {
            return new List<string>(Directory.GetFiles(path));
        }
    }
}