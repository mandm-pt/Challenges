using System;

namespace AoC.Solutions
{
    internal static class ConsoleUtils
    {
        public static void Write(string text, ConsoleColor color) => WriteToConsole(Console.Write, text, color);
        public static void WriteLine(string text, ConsoleColor color) => WriteToConsole(Console.WriteLine, text, color);

        public static void WriteLineSuccess(string text) => WriteLine(text, ConsoleColor.Green);
        public static void WriteLineError(string text) => WriteLine(text, ConsoleColor.Red);
        public static void WriteLineInfo(string text) => WriteLine(text, ConsoleColor.Cyan);
        public static void WriteLineWarning(string text) => WriteLine(text, ConsoleColor.Yellow);

        public static void WriteSuccess(string text) => Write(text, ConsoleColor.Green);
        public static void WriteError(string text) => Write(text, ConsoleColor.Red);
        public static void WriteInfo(string text) => Write(text, ConsoleColor.Cyan);
        public static void WriteWarning(string text) => Write(text, ConsoleColor.Yellow);

        public static void WriteMenuOption(int option, string text)
        {
            Write(option.ToString(), ConsoleColor.White);
            WriteLine($" - {text}", ConsoleColor.Gray);
        }

        private static void WriteToConsole(Action<string> write, string text, ConsoleColor color)
        {
            var currentColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            write(text);
            Console.ForegroundColor = currentColor;
        }
    }
}
