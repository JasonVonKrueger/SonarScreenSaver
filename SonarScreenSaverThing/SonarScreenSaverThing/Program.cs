using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Diagnostics;

namespace SonarScreenSaverThing
{
    class Program
    {
        private static SerialPort IO;
        static bool _continue = true;

        static void ListSerPorts()
        {
            string[] ports = SerialPort.GetPortNames();

            Console.WriteLine("The following serial ports were found:");

            foreach (string port in ports) { Console.WriteLine(port); }            
        }

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n*******************************************************");
            Console.WriteLine("*          Screen Saver Sonar Thingy                  *");
            Console.WriteLine("*******************************************************\n");
            Console.ForegroundColor = ConsoleColor.Green;

            Thread readThread = new Thread(ReadPort);

            ListSerPorts();
            Console.Write("Enter serial port (COMX): ");
            string comport = Console.ReadLine();

            if (!initSerialPort(comport, 9600))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nCould not init the serial port!");
                Console.ForegroundColor = ConsoleColor.Green;
            }

            readThread.Start();

            while (_continue)
            {
                string message = Console.ReadLine();

                if (message == "quit")
                {
                    _continue = false;                    
                    readThread.Join();
                    IO.Close();
                    Environment.Exit(0);
                }
            }
        }


        private static bool initSerialPort(string serialPortNum, int serialPortRate)
        {
            try
            {
                if (IO != null)
                {
                    IO.Close();  //Just in case port is already taken
                }

                IO = new SerialPort(serialPortNum, serialPortRate, Parity.None, 8, StopBits.One);
                IO.DtrEnable = false;
                IO.Handshake = Handshake.None;
                IO.RtsEnable = false;

                IO.Open();
                Console.WriteLine("COM port initialized");

                return true;
            }
            catch
            {
                serialPortNum = String.Empty;
                IO.Close();

                return false;
            }
        }

        public static void ReadPort()
        {
            while (_continue)
            {
                try
                {
                    // see if data has been sent
                    if (IO.BytesToRead == 0)
                    {
                        string message = IO.ReadLine();

                        // start screen saver if user is more than 30 inches away
                        if (int.Parse(message) >= 30)
                        {
                            Console.WriteLine(message);

                            ProcessStartInfo startInfo = new ProcessStartInfo("rundll32.exe");
                            startInfo.Arguments = "user32.dll, LockWorkStation";
                            Process.Start(startInfo);
                        }
                    }
                }
                catch (TimeoutException)
                { }
            }
        }
    }
}
