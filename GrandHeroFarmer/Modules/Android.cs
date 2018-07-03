using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GrandHeroFarmer.Modules
{
    public class Android
    {
        private readonly AdbServer _server;
        private DeviceData _device;

        public Android()
        {
            if (File.Exists(@"adb\adb.exe"))
            {
                _server = new AdbServer();
                _server.StartServer(@"adb\adb.exe", restartServerIfNewer: false);
                ConnectToAndroid();

                var monitor = new DeviceMonitor(new AdbSocket(new IPEndPoint(IPAddress.Loopback, AdbClient.AdbServerPort)));
                monitor.DeviceDisconnected += this.OnDeviceDisconnected;
                monitor.Start();
            }
            else
            {
                ConsoleLogger.Write("adb.exe not found. ", textColor: ConsoleColor.Red);
                ConsoleLogger.Write("Did you extract everything from the zip?");
                ConsoleLogger.Write("Press Enter to exit.");

                Console.ReadLine();
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Tries to connect to an android device by using the ADB interface.
        /// </summary>
        private void ConnectToAndroid()
        {
            bool foundDevice = false;
            while (!foundDevice)
            {
                ConsoleLogger.WriteTime("Trying to get device information... ", false);
                if(AdbClient.Instance.GetDevices().Count != 0)
                {
                    
                    _device = AdbClient.Instance.GetDevices().First();
                    foundDevice = true;
                    ConsoleLogger.Write(_device.Name + " connected!", textColor: ConsoleColor.Cyan);
                    return;
                }
                ConsoleLogger.Write("Failed! No Device was found!", textColor: ConsoleColor.Red);
                ConsoleLogger.WriteTime("Check the connection and press", false);
                ConsoleLogger.Write(" Enter ", false, ConsoleColor.Cyan);
                ConsoleLogger.Write("to try again.");
                Console.ReadLine();
            }
        }

        /// <summary>
        /// Sends a tap event to the connected device with the given coordenates
        /// </summary>
        /// <param name="pX">X Coordinate</param>
        /// <param name="pY">Y Coordinate</param>
        public void Tap(Tuple<int, int> coordenates)
        {
            string command = "input tap " + coordenates.Item1 + " " + coordenates.Item2;

            IShellOutputReceiver receiver = null;

            AdbClient.Instance.ExecuteRemoteCommand(command, _device, receiver);
        }
        
        // Monitor Functions
        void OnDeviceDisconnected(object sender, DeviceDataEventArgs e)
        {
            ConsoleLogger.WriteTime("The device " + _device.Name +" has disconnected to this PC", textColor: ConsoleColor.Red);
            ConsoleLogger.WriteTime("Please restart the application and go back to the Mission Select Menu.");
            ConnectToAndroid();
        }
    }

    /// <summary>
    /// Represents a Click Area in a Mobile App.
    /// </summary>
    public class ClickArea
    {
        private readonly Tuple<int, int> topLeft;
        private readonly Tuple<int, int> bottomRight;

        public ClickArea(int pTopLeftX, int pTopLeftY, int pBottomRightX, int pBottomRightY)
        {
            topLeft = new Tuple<int, int>(pTopLeftX, pTopLeftY);
            bottomRight = new Tuple<int, int>(pBottomRightX, pBottomRightY);
        }

        /// <summary>
        /// Creates a click area from a XElement in a XML File.
        /// </summary>
        /// <param name="element"></param>
        public ClickArea(XElement element) 
            : this
            (
                (int) element.Descendants("TopLeftX").FirstOrDefault(),
                (int) element.Descendants("TopLeftY").FirstOrDefault(),
                (int) element.Descendants("BottomRightX").FirstOrDefault(),
                (int) element.Descendants("BottomRightY").FirstOrDefault()
            ) { }

        public Tuple<int, int> GenerateRandomCoords()
        {
            Random r = new Random();
            int xRand = r.Next(topLeft.Item1, bottomRight.Item1);
            int yRand = r.Next(topLeft.Item2, bottomRight.Item2);

            return new Tuple<int, int>(xRand, yRand);
        }
    }
}
