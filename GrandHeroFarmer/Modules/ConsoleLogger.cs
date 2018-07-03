using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandHeroFarmer.Modules
{
    enum Type { Info = ConsoleColor.Cyan, Error = ConsoleColor.Red, Default = ConsoleColor.Gray };

    static class ConsoleLogger
    {
        public static void Write(string value, bool newLine = true, ConsoleColor textColor = ConsoleColor.Gray, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            Console.ForegroundColor = textColor;
            Console.BackgroundColor = backgroundColor;
            Console.Write(value);
            Console.ResetColor();

            if (newLine)
            {
                Console.WriteLine();
            }
        }

        public static void WriteTime(string value, bool newLine = true, ConsoleColor textColor = ConsoleColor.Gray, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            string timeLog = DateTime.UtcNow.ToString("[HH:mm:ss] >> ");

            Console.Write(timeLog);
            Write(value, newLine, textColor, backgroundColor);
        }

        public static void WriteCenter(string value, bool newLine = true, ConsoleColor textColor = ConsoleColor.Gray, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            Console.SetCursorPosition((Console.WindowWidth - value.Length) / 2, Console.CursorTop);

            Write(value, newLine, textColor, backgroundColor);
        }

    }
}
