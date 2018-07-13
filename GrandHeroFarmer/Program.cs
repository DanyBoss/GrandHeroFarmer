using GrandHeroFarmer.Modules;
using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace GrandHeroFarmer
{
    class Program
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

        static void Main(string[] args)
        {
            Console.Title = "Grand Hero Farmer v" + Helpers.GetProgramVersion();
            Console.Clear();

            Console.WriteLine();
            ConsoleLogger.WriteCenter("Grand Hero Farmer", textColor: ConsoleColor.Cyan, backgroundColor: ConsoleColor.DarkCyan);

            ConsoleLogger.WriteCenter(puns[rnd.Next(puns.Count)], true, textColor: ConsoleColor.Magenta);

            ConsoleLogger.WriteTime("Starting program...");
            try
            {
                Android phone = new Android();

                //Initializing Service Configurations
                ConsoleLogger.WriteTime("Loading service configurations from xml... ", false);
                XDocument doc = XDocument.Load("Configurations/Default.xml");
                ClickArea startGBHButton = new ClickArea(doc.Descendants("StartGHBButton").FirstOrDefault());
                ClickArea fightButton = new ClickArea(doc.Descendants("FightButton").FirstOrDefault());
                ClickArea skipDialogButton = new ClickArea(doc.Descendants("SkipDialogButton").FirstOrDefault());
                ClickArea autoBattleButton = new ClickArea(doc.Descendants("AutoBattleButton").FirstOrDefault());
                ClickArea acceptAutoBattleButton = new ClickArea(doc.Descendants("AcceptAutoBattleButton").FirstOrDefault());

                int communicateServerTimer = ((int)(doc.Descendants("CommunicateServerTimer").FirstOrDefault()) * 1000);
                int stageTimer = ((int)(doc.Descendants("StageTimer").FirstOrDefault()) * 1000);
                ConsoleLogger.Write("OK", textColor: ConsoleColor.Cyan);

                ConsoleLogger.WriteTime("Setup is done, press enter to start earning feathers.", textColor: ConsoleColor.Cyan);
                ConsoleLogger.WriteTime("Press 'Ctrl + C' to exit the application.", newLine: false, textColor: ConsoleColor.Cyan);
                Console.ReadLine();

                int cicles = 1;

                Thread thread = new Thread(() => {

                    Console.Title = "Grand Hero Farmer - Farming";

                    while (true)
                    {
                        Console.WriteLine();
                        ConsoleLogger.WriteTime("Starting cicle ", false);
                        ConsoleLogger.Write("[" + cicles.ToString() + "]", textColor: ConsoleColor.Cyan);

                        // Click Lunatic Button
                        phone.Tap(startGBHButton.GenerateRandomCoords());

                        // Click Fight Button
                        phone.Tap(fightButton.GenerateRandomCoords());

                        // Communicate Server
                        Thread.Sleep(communicateServerTimer);

                        // Click to skip initial animations
                        phone.Tap(fightButton.GenerateRandomCoords());

                        // Wait until Dialog starts
                        Thread.Sleep(2000);

                        // Skip Dialog
                        ConsoleLogger.WriteTime("Skipping initial dialog");
                        phone.Tap(skipDialogButton.GenerateRandomCoords());

                        // Wait for initial animations
                        Thread.Sleep(3000);

                        // Click Auto Battle
                        ConsoleLogger.WriteTime("Initializing Auto-Battle");
                        phone.Tap(autoBattleButton.GenerateRandomCoords());

                        // CLick Accept
                        phone.Tap(acceptAutoBattleButton.GenerateRandomCoords());

                        // Wait for battle to end
                        ConsoleLogger.WriteTime("Waiting for battle to end");
                        Thread.Sleep(stageTimer);

                        // Click again to skip animations
                        phone.Tap(fightButton.GenerateRandomCoords());

                        // All done!
                        ConsoleLogger.WriteTime("Finished cicle ", false);
                        ConsoleLogger.Write("[" + cicles.ToString() + "]", textColor: ConsoleColor.Cyan);

                        // Wait to send result to server
                        Thread.Sleep(communicateServerTimer + 1000);

                        cicles++;
                    }
                });

                thread.Start();

            }
            catch (System.IO.FileNotFoundException)
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
