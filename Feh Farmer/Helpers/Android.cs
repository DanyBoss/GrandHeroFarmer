﻿using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Feh_Farmer.Helpers
{
    public class Android
    {
        private DeviceData _device;

        public Android()
        {
            AdbServer server = new AdbServer();
            var result = server.StartServer(@"adb\adb.exe", restartServerIfNewer: false);

            ConnectToAndroid();

            var monitor = new DeviceMonitor(new AdbSocket(new IPEndPoint(IPAddress.Loopback, AdbClient.AdbServerPort)));
            monitor.DeviceDisconnected += this.OnDeviceDisconnected;
            monitor.Start();
        }

        private void ConnectToAndroid()
        {
            bool foundDevice = false;
            while (!foundDevice)
            {
                ConsoleLogger.Write("Trying to get Device from adb...", Type.Default);
                if(AdbClient.Instance.GetDevices().Count != 0)
                {
                    _device = AdbClient.Instance.GetDevices().First();
                    foundDevice = true;
                    ConsoleLogger.WriteLine(_device.Name + " connected!", Type.Info, false);
                    return;
                }
                ConsoleLogger.WriteLine("No Device was found! Is it connected?", Type.Error);
                ConsoleLogger.Write("Check the connection and press", Type.Default);
                ConsoleLogger.Write(" Enter ", Type.Info, false);
                ConsoleLogger.WriteLine("to try again.", Type.Default, false);
                Console.ReadLine();
            }
        }

        /// <summary>
        /// Sends a tap event to the connected device with the given coordenates
        /// </summary>
        /// <param name="pX"></param>
        /// <param name="pY"></param>
        public void Tap(Tuple<int, int> coordenates)
        {
            string command = "input tap " + coordenates.Item1 + " " + coordenates.Item2;


            IShellOutputReceiver receiver = null;

            AdbClient.Instance.ExecuteRemoteCommand(command, _device, receiver);
        }
        
        // Monitor Functions
        void OnDeviceDisconnected(object sender, DeviceDataEventArgs e)
        {
            ConsoleLogger.WriteLine("The device " + _device.Name +" has disconnected to this PC", Type.Error);
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
            bottomRight = new Tuple<int, int>(pBottomRightX, pBottomRightY); ;
        }

        public Tuple<int, int> GenerateRandomCoords()
        {
            Random r = new Random();
            int xRand = r.Next(topLeft.Item1, bottomRight.Item1);
            int yRand = r.Next(topLeft.Item2, bottomRight.Item2);

            return new Tuple<int, int>(xRand, yRand);
        }
    }
}