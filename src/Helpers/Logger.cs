using System;

namespace Navigator.Helpers
{
    public class Logger
    {
        public static void Info(string input)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"[LOG] {input}");
            Console.ResetColor();
        }

        public static void Debug(string input)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"[DBG] {input}");
            Console.ResetColor();
        }

        public static void Warn(string input)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[WRN] {input}");
            Console.ResetColor();
        }

        public static void Error(string input)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERR] {input}");
            Console.ResetColor();
        }
    }
}