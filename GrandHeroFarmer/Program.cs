using GrandHeroFarmer.Bot;
using GrandHeroFarmer.Modules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Linq;

namespace GrandHeroFarmer
{
    internal class Program
    {
        private static readonly List<string> puns = new List<string>
        {
            "Just for feathers and love",
            "Just one more +10 and I'm done",
            "We need more Armads in the game",
            "Just Tiki.",
            "r/OrderOfHeroes is pretty neat",
            "/feg/ is cozy sometimes"
        };

        public static readonly Random rnd = new Random();

        private static void Main(string[] args)
        {
            Console.Title = string.Format("Grand Hero Farmer v{0}", Helpers.GetProgramVersion());
            Console.Clear();

            Console.WriteLine();
            ConsoleLogger.WriteCenter("Grand Hero Farmer", textColor: ConsoleColor.Cyan, backgroundColor: ConsoleColor.DarkCyan);
            ConsoleLogger.WriteCenter(puns[rnd.Next(puns.Count)], newLine: true, textColor: ConsoleColor.Magenta);
            ConsoleLogger.WriteTime("Starting program...");

            try
            {
                AdbWrapper adb = new AdbWrapper();

                // Initializing Bot Configurations
                ConsoleLogger.WriteTime("Loading bot configurations from xml... ", false);
                BotConfiguration botConfig = new BotConfiguration("configs");
                ConsoleLogger.Write("OK", textColor: ConsoleColor.Cyan);

                ConsoleLogger.WriteTime("Initializing bot instance... ", false);
                FehBot bot = new FehBot(botConfig);
                ConsoleLogger.Write("OK", textColor: ConsoleColor.Cyan);

                ConsoleLogger.WriteTime("Setup is done, press enter to start earning feathers.", textColor: ConsoleColor.Cyan);
                ConsoleLogger.WriteTime("Press 'Ctrl + C' to exit the application.", newLine: false, textColor: ConsoleColor.Cyan);
                Console.ReadLine();

                Thread thread = new Thread(() =>
                {
                    Console.Title = string.Format("{0} - Farming", Console.Title);

                    while (true)
                    {
                        bot.Run(adb);
                    }
                });

                thread.Start();
            }
            catch (FileNotFoundException)
            {
                ConsoleLogger.Write("FAILED", textColor: ConsoleColor.Red);
                ConsoleLogger.WriteTime("Couldn't Load XML File. Press Enter to exit the application.");
            }
            catch (NullReferenceException)
            {
                ConsoleLogger.Write("FAILED", textColor: ConsoleColor.Red);
                ConsoleLogger.WriteTime("Looks like your XML file is malformed. Press Enter to exit the application.");
            }
            catch (Exception ex)
            {
                ConsoleLogger.Write(ex.ToString(), textColor: ConsoleColor.Red);
            }
            finally
            {
                Console.ReadLine();
            }
        }
    }
}