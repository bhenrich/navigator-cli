using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Drawing;
using Navigator.Config;
using Navigator.Directories;

namespace Navigator.Rendering
{
    public static class Renderer
    {
        private static int selectedIndex = 0;
        private static int firstVisibleIndex = 0;
        private static int maxVisibleItems = Console.WindowHeight - 7; // Reserve an extra line for spacing

        public static void DisplayCurrentDirectory(string currentDirectory)
        {
            Console.Clear();
            Console.WriteLine($"Current Directory: {currentDirectory}\n");
        }

        public static void DisplayDirectoryContents(string currentDirectory)
        {
            List<string> directories = Crawler.GetDirectories(currentDirectory).OrderBy(d => d).ToList();
            List<string> files = Crawler.GetFiles(currentDirectory).OrderBy(f => f).ToList();

            var config = ConfigManager.LoadConfig();

            int totalItems = directories.Count + files.Count;

            if (selectedIndex >= totalItems) selectedIndex = totalItems - 1;
            if (selectedIndex < 0) selectedIndex = 0;

            AdjustFirstVisibleIndex(totalItems);

            for (int i = firstVisibleIndex; i < firstVisibleIndex + maxVisibleItems && i < totalItems; i++)
            {
                if (i < directories.Count)
                {
                    if (i == selectedIndex)
                        HighlightText(Path.GetFileName(directories[i]), config.Colors);
                    else
                        PrintDirectory(Path.GetFileName(directories[i]), config.Colors);
                }
                else
                {
                    if (i == selectedIndex)
                        HighlightText(Path.GetFileName(files[i - directories.Count]), config.Colors);
                    else
                        PrintFile(Path.GetFileName(files[i - directories.Count]), config.Colors);
                }
            }

            // Leave an extra line for spacing before displaying controls
            Console.WriteLine();
            DisplayControls(config);
        }

        public static string HandleKeyPress(ConsoleKeyInfo keyInfo, string currentDirectory, KeybindsConfig keybinds)
        {
            List<string> directories = Crawler.GetDirectories(currentDirectory).OrderBy(d => d).ToList();
            List<string> files = Crawler.GetFiles(currentDirectory).OrderBy(f => f).ToList();

            int totalItems = directories.Count + files.Count;

            switch (keyInfo.Key.ToString())
            {
                case var key when key == keybinds.EntryUp:
                    if (selectedIndex > 0) selectedIndex--;
                    break;
                case var key when key == keybinds.EntryDown:
                    if (selectedIndex < totalItems - 1) selectedIndex++;
                    break;
                /*
                case var key when key == keybinds.Open:
                    if (selectedIndex < directories.Count)
                        return directories[selectedIndex];
                    else
                        OpenFilePrompt(files[selectedIndex - directories.Count]);
                    break; */
                case var key when key == keybinds.DirDown:
                    if (selectedIndex < directories.Count)
                        return directories[selectedIndex];
                    break;
                case var key when key == keybinds.DirUp:
                    return Directory.GetParent(currentDirectory)?.FullName ?? currentDirectory;
                case var key when key == keybinds.Exit:
                    ExitApp(currentDirectory);
                    break;
            }

            return currentDirectory;
        }

        private static void AdjustFirstVisibleIndex(int totalItems)
        {
            if (selectedIndex < firstVisibleIndex)
            {
                firstVisibleIndex = selectedIndex;
            }
            else if (selectedIndex >= firstVisibleIndex + maxVisibleItems)
            {
                firstVisibleIndex = selectedIndex - maxVisibleItems + 1;
            }
        }

        private static void HighlightText(string text, ColorsConfig colors)
        {
            Console.BackgroundColor = ConvertHexToConsoleColor(colors.SelectedBackgroundColor);
            Console.ForegroundColor = ConvertHexToConsoleColor(colors.SelectedForegroundColor);
            Console.WriteLine(text);
            Console.ResetColor();
        }

        private static void PrintDirectory(string directory, ColorsConfig colors)
        {
            Console.ForegroundColor = ConvertHexToConsoleColor(colors.DirectoryColor);
            Console.WriteLine(directory);
            Console.ResetColor();
        }

        private static void PrintFile(string file, ColorsConfig colors)
        {
            Console.ForegroundColor = ConvertHexToConsoleColor(colors.FileColor);
            Console.WriteLine(file);
            Console.ResetColor();
        }

        private static void DisplayControls(NavigatorConfig config)
        {
            int currentLineCursor = Console.CursorTop;
            int windowHeight = Console.WindowHeight;

            Console.SetCursorPosition(0, windowHeight - 4);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Controls:");
            Console.WriteLine($"Select Up: {config.Keybinds.EntryUp}\t|\tAscend Directory: {config.Keybinds.DirUp}");
            Console.WriteLine($"Select Down: {config.Keybinds.EntryDown}\t|\tEnter Directory: {config.Keybinds.DirDown}\t|\tExit: {config.Keybinds.Exit}");

            Console.ResetColor();
            Console.SetCursorPosition(0, currentLineCursor);
        }

        private static void OpenFilePrompt(string filePath)
        {
            int windowHeight = Console.WindowHeight;
            int windowWidth = Console.WindowWidth;

            string prompt = "Enter command to open file: ";
            int promptLength = prompt.Length;
            int boxWidth = Math.Max(promptLength, 30) + 4;

            int boxTop = windowHeight / 2 - 2;
            int boxLeft = (windowWidth - boxWidth) / 2;

            DrawBox(boxTop, boxLeft, 5, boxWidth);

            Console.SetCursorPosition(boxLeft + 2, boxTop + 1);
            Console.Write(prompt);

            Console.SetCursorPosition(boxLeft + 2, boxTop + 2);
            string? command = Console.ReadLine();

            if (!string.IsNullOrEmpty(command))
            {
                ExitAppAndExecuteCommand(command, filePath);
            }
            else
            {
                Console.SetCursorPosition(0, boxTop + 5);
                Console.ResetColor();
            }
        }

        private static void DrawBox(int top, int left, int height, int width)
        {
            for (int i = 0; i < height; i++)
            {
                Console.SetCursorPosition(left, top + i);
                Console.BackgroundColor = ConsoleColor.White;
                Console.Write(new string(' ', width));
                Console.ResetColor();
            }

            Console.SetCursorPosition(left + 1, top + 1);
            Console.Write(new string('-', width - 2));

            Console.SetCursorPosition(left + 1, top + height - 1);
            Console.Write(new string('-', width - 2));

            for (int i = 1; i < height - 1; i++)
            {
                Console.SetCursorPosition(left + 1, top + i);
                Console.Write('|');
                Console.SetCursorPosition(left + width - 2, top + i);
                Console.Write('|');
            }
        }

        private static void ExitAppAndExecuteCommand(string command, string filePath)
        {
            Console.Clear();
            Console.WriteLine($"Executing command: {command} {filePath}");
            Process.Start(new ProcessStartInfo
            {
                FileName = "sh",
                Arguments = $"-c \"{command} '{filePath}'\"",
                UseShellExecute = true,
                RedirectStandardOutput = false,
                RedirectStandardInput = false,
                RedirectStandardError = false,
                CreateNoWindow = false
            });
            Environment.Exit(0); 
        }

        private static void ExitApp(string currentDirectory)
        {
            Console.Clear();
            
            // found out that one does not simply change the parent shell's environment using a child process
            //Console.WriteLine($"exit to {currentDirectory}");
            
            Environment.Exit(0); 
        }

        private static ConsoleColor ConvertHexToConsoleColor(string hex)
        {
            var color = ColorTranslator.FromHtml(hex);
            return GetNearestConsoleColor(color);
        }

        private static ConsoleColor GetNearestConsoleColor(Color color)
        {
            ConsoleColor[] consoleColors = (ConsoleColor[])ConsoleColor.GetValues(typeof(ConsoleColor));
            ConsoleColor nearestColor = ConsoleColor.Black;
            double smallestDistance = double.MaxValue;

            foreach (var consoleColor in consoleColors)
            {
                var cc = ColorFromConsoleColor(consoleColor);
                var distance = ColorDistance(color, cc);

                if (distance < smallestDistance)
                {
                    smallestDistance = distance;
                    nearestColor = consoleColor;
                }
            }

            return nearestColor;
        }

        private static Color ColorFromConsoleColor(ConsoleColor consoleColor)
        {
            int[] cColors = {
                0x000000, // Black
                0x000080, // DarkBlue
                0x008000, // DarkGreen
                0x008080, // DarkCyan
                0x800000, // DarkRed
                0x800080, // DarkMagenta
                0x808000, // DarkYellow
                0xC0C0C0, // Gray
                0x808080, // DarkGray
                0x0000FF, // Blue
                0x00FF00, // Green
                0x00FFFF, // Cyan
                0xFF0000, // Red
                0xFF00FF, // Magenta
                0xFFFF00, // Yellow
                0xFFFFFF  // White
            };

            return Color.FromArgb(cColors[(int)consoleColor]);
        }

        private static double ColorDistance(Color c1, Color c2)
        {
            return Math.Sqrt(Math.Pow(c1.R - c2.R, 2) + Math.Pow(c1.G - c2.G, 2) + Math.Pow(c1.B - c2.B, 2));
        }
    }
}