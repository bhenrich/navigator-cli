using Navigator.Config;
using Navigator.Directories;
using Navigator.Helpers;
using Navigator.Rendering;

namespace Navigator
{
    class Program
    {
        private static string currentDirectory = Environment.CurrentDirectory;
        private static NavigatorConfig config;

        static void Main(string[] args)
        {
            Console.Clear();
            config = ConfigManager.LoadConfig();

            while (true)
            {
                Renderer.DisplayCurrentDirectory(currentDirectory);
                Renderer.DisplayDirectoryContents(currentDirectory);

                ConsoleKeyInfo keyInfo = Console.ReadKey(true); // true to prevent showing key press
                currentDirectory = Renderer.HandleKeyPress(keyInfo, currentDirectory, config.Keybinds);
            }
        }
    }
}