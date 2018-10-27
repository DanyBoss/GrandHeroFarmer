using GrandHeroFarmer.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GrandHeroFarmer.Bot
{
    public class FehBot
    {
        private readonly BotConfiguration config;
        private int cicles = 1;

        public FehBot(BotConfiguration config)
        {
            this.config = config;
        }
        
        public void Run(AdbWrapper adb)
        {
            Console.WriteLine();
            ConsoleLogger.WriteTime("Starting cicle ", false);
            ConsoleLogger.Write(string.Format("[{0}]", cicles.ToString()), textColor: ConsoleColor.Cyan);

            // Click Lunatic Button
            adb.Tap(config.startButton.GenerateRandomCoords());

            // Click Fight Button
            adb.Tap(config.fightButton.GenerateRandomCoords());

            // Communicate Server
            Thread.Sleep(config.communicateServerTimer);

            // Click to skip initial animations
            adb.Tap(config.fightButton.GenerateRandomCoords());

            // Wait until Dialog starts
            Thread.Sleep(2000);

            // Skip Dialog
            ConsoleLogger.WriteTime("Skipping initial dialog");
            adb.Tap(config.skipDialogButton.GenerateRandomCoords());

            // Wait for initial animations
            Thread.Sleep(3000);

            // Click Auto Battle
            ConsoleLogger.WriteTime("Initializing Auto-Battle");
            adb.Tap(config.autoBattleButton.GenerateRandomCoords());

            // CLick Accept
            adb.Tap(config.acceptAutoBattle.GenerateRandomCoords());

            // Wait for battle to end
            ConsoleLogger.WriteTime("Waiting for battle to end");
            Thread.Sleep(config.stageTimer);

            // Click again to skip animations
            adb.Tap(config.fightButton.GenerateRandomCoords());

            // All done!
            ConsoleLogger.WriteTime("Finished cicle ", false);
            ConsoleLogger.Write(string.Format("[{0}]", cicles.ToString()), textColor: ConsoleColor.Cyan);

            // Wait to send result to server
            Thread.Sleep(config.communicateServerTimer + 1000);

            cicles++;
        }

    }
}
