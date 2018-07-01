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

        public static void WriteLine(string value, Type type, bool showTimestamp = true, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            Write(value, type, showTimestamp, backgroundColor);
            Console.WriteLine();
        }

        public static void Write(string value, Type type, bool showTimestamp = true, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            string timeLog = (showTimestamp ? DateTime.UtcNow.ToString("[HH:mm:ss] >> ") : String.Empty);

            ConsoleColor color;
            switch (type)
            {
                case Type.Info:
                    color = ConsoleColor.Cyan;
                    break;
                case Type.Error:
                    color = ConsoleColor.Red;
                    break;
                default:
                    color = ConsoleColor.Gray;
                    break;
            }

            // Print stuff
            Console.Write(timeLog);
            Console.ForegroundColor = color;
            Console.BackgroundColor = backgroundColor;
            Console.Write(value);
            Console.ResetColor();
        }

    }
}
