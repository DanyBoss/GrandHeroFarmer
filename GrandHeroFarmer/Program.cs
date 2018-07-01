using Feh_Farmer.Helpers;
using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Feh_Farmer
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleLogger.WriteLine("Welcome to Feh Farmer", Helpers.Type.Info, true, ConsoleColor.DarkYellow);
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
            int stageLoadTimer = ((int)(doc.Descendants("StageLoadTimer").FirstOrDefault()) * 1000);
            int initialDialogTimer = ((int)(doc.Descendants("InitialDialogTimer").FirstOrDefault()) * 1000);
            int stageTimer = ((int)(doc.Descendants("StageTimer").FirstOrDefault()) * 1000);

            ConsoleLogger.WriteLine("OK", Helpers.Type.Info, false);

            ConsoleLogger.WriteLine("Setup is done, press enter to start earning feathers.", Helpers.Type.Info);
            ConsoleLogger.WriteLine("Press 'Ctrl + C' to exit the application.", Helpers.Type.Info);
            Console.ReadLine();

            int iterations = 0;

            Thread thread = new Thread(() => {
                while (true)
                {
                    Console.WriteLine();
                    ConsoleLogger.Write("Starting iteration ", Helpers.Type.Default);
                    ConsoleLogger.WriteLine(iterations.ToString(), Helpers.Type.Info, false);
                    phone.Tap(startGBHButton.GenerateRandomCoords());

                    phone.Tap(fightButton.GenerateRandomCoords());

                    Thread.Sleep(communicateServerTimer);
                    phone.Tap(fightButton.GenerateRandomCoords());

                    Thread.Sleep(stageLoadTimer);
                    ConsoleLogger.WriteLine("Skipping initial dialog", Helpers.Type.Default);
                    phone.Tap(skipDialogButton.GenerateRandomCoords());
                    Thread.Sleep(initialDialogTimer);

                    ConsoleLogger.WriteLine("Initializing Auto-Battle", Helpers.Type.Default);
                    phone.Tap(autoBattleButton.GenerateRandomCoords());

                    phone.Tap(acceptAutoBattleButton.GenerateRandomCoords());

                    ConsoleLogger.WriteLine("Waiting for battle to end", Helpers.Type.Default);
                    Thread.Sleep(stageTimer);

                    phone.Tap(fightButton.GenerateRandomCoords());
                    Thread.Sleep(communicateServerTimer);

                    ConsoleLogger.Write("Finished iteration ", Helpers.Type.Default);
                    ConsoleLogger.WriteLine(iterations.ToString(), Helpers.Type.Info, false);

                    iterations++;
                }

            });
            thread.Start();
        }
    }
}
