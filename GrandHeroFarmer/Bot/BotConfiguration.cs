using GrandHeroFarmer.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GrandHeroFarmer.Bot
{
    public class BotConfiguration
    {
        public readonly ClickArea startButton;
        public readonly ClickArea fightButton;
        public readonly ClickArea skipDialogButton;
        public readonly ClickArea autoBattleButton;
        public readonly ClickArea acceptAutoBattle;

        public readonly int communicateServerTimer;
        public readonly int stageTimer;

        public BotConfiguration(string configFolder)
        {
            XDocument xmlConfiguration = GetXml(configFolder);

            startButton = new ClickArea(xmlConfiguration.Descendants("StartGHBButton").FirstOrDefault());
            fightButton = new ClickArea(xmlConfiguration.Descendants("FightButton").FirstOrDefault());
            skipDialogButton = new ClickArea(xmlConfiguration.Descendants("SkipDialogButton").FirstOrDefault());
            autoBattleButton = new ClickArea(xmlConfiguration.Descendants("AutoBattleButton").FirstOrDefault());
            acceptAutoBattle = new ClickArea(xmlConfiguration.Descendants("AcceptAutoBattleButton").FirstOrDefault());

            communicateServerTimer = (int)xmlConfiguration.Descendants("CommunicateServerTimer").FirstOrDefault() * 1000;
            stageTimer = (int)xmlConfiguration.Descendants("StageTimer").FirstOrDefault() * 1000;
        }

        private XDocument GetXml(string folderPath)
        {
            var currentDay = DateTime.Today.DayOfWeek;
            try
            {
                switch (currentDay)
                {
                    case DayOfWeek.Friday:
                        {
                            return XDocument.Load(String.Format("{0}/Friday.xml", folderPath));
                        }
                    case DayOfWeek.Saturday:
                        {
                            return XDocument.Load(String.Format("{0}/Saturday.xml", folderPath));
                        }
                    case DayOfWeek.Sunday:
                        {
                            return XDocument.Load(String.Format("{0}/Sunday.xml", folderPath));
                        }
                    case DayOfWeek.Monday:
                        {
                            return XDocument.Load(String.Format("{0}/Monday.xml", folderPath));
                        }
                    case DayOfWeek.Tuesday:
                        {
                            return XDocument.Load(String.Format("{0}/Tuesday.xml", folderPath));
                        }
                    case DayOfWeek.Wednesday:
                        {
                            return XDocument.Load(String.Format("{0}/Wednesday.xml", folderPath));
                        }
                    case DayOfWeek.Thursday:
                        {
                            return XDocument.Load(String.Format("{0}/Thursday.xml", folderPath));
                        }
                    default:
                        {
                            return XDocument.Load(String.Format("{0}/Default.xml", folderPath));
                        }
                }
            }
            catch
            {
                return XDocument.Load(String.Format("{0}/Default.xml", folderPath));
            }
            
        }
    }
}
