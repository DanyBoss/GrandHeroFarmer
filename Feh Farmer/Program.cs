using Feh_Farmer.Helpers;
using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Feh_Farmer
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleLogger.WriteLine("Welcome to Feh Farmer", Helpers.Type.Info, true, ConsoleColor.DarkYellow);
            Android phone = new Android();

            //Initializing Service Configurations
            ConsoleLogger.Write("Loading Configurations from xml... ", Helpers.Type.Default);
            XmlDocument doc = new XmlDocument();
            doc.Load("Configurations/Default.xml");
            ConsoleLogger.WriteLine("OK", Helpers.Type.Info, false);

            Thread thread = new Thread(() => Worker(phone, doc));
            thread.Start();
        }

        static void Worker(Android phone, XmlDocument doc)
        {
            bool Run = true;
            //Initializing Play Areas
            ConsoleLogger.Write("Initializing game configurations... ", Helpers.Type.Default);

            ClickArea startGBHButton = new ClickArea(100, 850, 1000, 1000);
            ClickArea fightButton = new ClickArea(200, 1130, 900, 1230);
            ClickArea skipDialogButton = new ClickArea(750, 50, 1000, 100);
            ClickArea autoBattleButton = new ClickArea(770, 1770, 900, 1900);
            ClickArea acceptAutoBattleButton = new ClickArea(230, 900, 870, 1000);
            ConsoleLogger.WriteLine("OK", Helpers.Type.Info, false);

            int stageLoadTimer = Convert.ToInt32(doc.SelectSingleNode("Settings/Timers/StageLoadTimer").InnerText);
            int initialDialogTimer = Convert.ToInt32(doc.SelectSingleNode("Settings/Timers/InitialDialogTimer").InnerText);
            int stageTimer = Convert.ToInt32(doc.SelectSingleNode("Settings/Timers/StageTimer").InnerText);

            ConsoleLogger.WriteLine("Setup is done, press enter to start earning feathers.", Helpers.Type.Info);
            Console.ReadLine();

            int iterations = 0;

            //main Loop
            while (Run)
            {
                Console.WriteLine();
                ConsoleLogger.Write("Starting iteration ", Helpers.Type.Default);
                ConsoleLogger.WriteLine(iterations.ToString(), Helpers.Type.Info, false);
                phone.Tap(startGBHButton.GenerateRandomCoords());

                phone.Tap(fightButton.GenerateRandomCoords());

                Thread.Sleep(2000);
                phone.Tap(fightButton.GenerateRandomCoords());

                Thread.Sleep(stageLoadTimer * 1000);
                ConsoleLogger.WriteLine("Skipping initial dialog", Helpers.Type.Default);
                phone.Tap(skipDialogButton.GenerateRandomCoords());
                Thread.Sleep(initialDialogTimer * 1000);

                ConsoleLogger.WriteLine("Initializing Auto-Battle", Helpers.Type.Default);
                phone.Tap(autoBattleButton.GenerateRandomCoords());

                phone.Tap(acceptAutoBattleButton.GenerateRandomCoords());

                ConsoleLogger.WriteLine("Waiting for battle to end", Helpers.Type.Default);
                Thread.Sleep(stageTimer * 1000);

                phone.Tap(fightButton.GenerateRandomCoords());
                Thread.Sleep(3000);

                ConsoleLogger.Write("Finished iteration ", Helpers.Type.Default);
                ConsoleLogger.WriteLine(iterations.ToString(), Helpers.Type.Info, false);

                iterations++;
            }

            ConsoleLogger.WriteLine("Exiting", Helpers.Type.Info);
        }
    }

    enum State { Idle, Running };
}
