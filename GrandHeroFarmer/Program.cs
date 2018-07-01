using GrandHeroFarmer.Helpers;
using SharpAdbClient;
using System;
using System.Collections.Generic;
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
        static void Main(string[] args)
        {
            ConsoleLogger.WriteLine("Welcome to Feh Farmer", Helpers.Type.Info, true, ConsoleColor.DarkCyan);
            try
            {
                Android phone = new Android();

                //Initializing Service Configurations
                ConsoleLogger.Write("Loading service configurations from xml... ", Helpers.Type.Default);
                XDocument doc = XDocument.Load("Configurations/Default.xml");
                ClickArea startGBHButton = new ClickArea(doc.Descendants("StartGHBButton").FirstOrDefault());
                ClickArea fightButton = new ClickArea(doc.Descendants("FightButton").FirstOrDefault());
                ClickArea skipDialogButton = new ClickArea(doc.Descendants("SkipDialogButton").FirstOrDefault());
                ClickArea autoBattleButton = new ClickArea(doc.Descendants("AutoBattleButton").FirstOrDefault());
                ClickArea acceptAutoBattleButton = new ClickArea(doc.Descendants("AcceptAutoBattleButton").FirstOrDefault());

                int communicateServerTimer = ((int)(doc.Descendants("CommunicateServerTimer").FirstOrDefault()) * 1000);
                int stageTimer = ((int)(doc.Descendants("StageTimer").FirstOrDefault()) * 1000);

                ConsoleLogger.WriteLine("OK", Helpers.Type.Info, false);

                ConsoleLogger.WriteLine("Setup is done, press enter to start earning feathers.", Helpers.Type.Info);
                ConsoleLogger.Write("Press 'Ctrl + C' to exit the application.", Helpers.Type.Info);
                Console.ReadLine();

                int iterations = 1;

                Thread thread = new Thread(() => {
                    while (true)
                    {
                        Console.WriteLine();
                        ConsoleLogger.Write("Starting iteration ", Helpers.Type.Default, true);
                        ConsoleLogger.WriteLine(iterations.ToString(), Helpers.Type.Info, false);

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
                        ConsoleLogger.WriteLine("Skipping initial dialog", Helpers.Type.Default);
                        phone.Tap(skipDialogButton.GenerateRandomCoords());

                        // Wait for initial animations
                        Thread.Sleep(3000);

                        // Click Auto Battle
                        ConsoleLogger.WriteLine("Initializing Auto-Battle", Helpers.Type.Default);
                        phone.Tap(autoBattleButton.GenerateRandomCoords());

                        // CLick Accept
                        phone.Tap(acceptAutoBattleButton.GenerateRandomCoords());

                        // Wait for battle to end
                        ConsoleLogger.WriteLine("Waiting for battle to end", Helpers.Type.Default);
                        Thread.Sleep(stageTimer);

                        // Click again to skip animations
                        phone.Tap(fightButton.GenerateRandomCoords());

                        // Wait to send result to server
                        Thread.Sleep(communicateServerTimer + 1000);

                        // All done!
                        ConsoleLogger.Write("Finished iteration ", Helpers.Type.Default);
                        ConsoleLogger.WriteLine(iterations.ToString(), Helpers.Type.Info, false);

                        iterations++;
                    }

                });
                thread.Start();

            }
            catch (System.IO.FileNotFoundException)
            {
                ConsoleLogger.WriteLine("FAILED", Helpers.Type.Error, false);
                ConsoleLogger.WriteLine("Couldn't Load XML File. Press Enter to exit the application.", Helpers.Type.Error, true);
            }
            catch (NullReferenceException)
            {
                ConsoleLogger.WriteLine("FAILED", Helpers.Type.Error, false);
                ConsoleLogger.WriteLine("Looks like your XML file is malformed. Press Enter to exit the application.", Helpers.Type.Error, true);
            }
            catch (Exception ex)
            {
                ConsoleLogger.WriteLine(ex.ToString(), Helpers.Type.Error, false);
            }
            finally
            {
                Console.ReadLine();
            }
        }
    }
}
