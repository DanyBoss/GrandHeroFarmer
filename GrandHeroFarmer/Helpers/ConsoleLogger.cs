using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandHeroFarmer.Helpers
{
    enum Type { Info, Error, Default };

    static class ConsoleLogger
    {

        public static void WriteSeparator()
        {
            Console.WriteLine();
            Console.Write(@"\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\");
            Console.WriteLine();
        }

        public static void WriteLine(string value, Type type, bool showTimestamp = true, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            Write(value, type, showTimestamp, backgroundColor);
            Console.WriteLine();
        }

        public static void Write(string value, Type type, bool showTimestamp = true, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            string timeLog = (showTimestamp ? DateTime.UtcNow.ToString("[HH:mm:ss] >> ") : String.Empty);

            switch (type)
            {
                case Type.Info:
                    Console.Write(timeLog);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.BackgroundColor = backgroundColor;
                    Console.Write(value);
                    Console.ResetColor();
                    break;
                case Type.Error:
                    Console.Write(timeLog);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.BackgroundColor = backgroundColor;
                    Console.Write(value);
                    Console.ResetColor();
                    break;
                default:
                    Console.Write(timeLog);
                    Console.Write(value);
                    break;
            }
        }

    }
}
