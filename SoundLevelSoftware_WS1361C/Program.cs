using System;
using LibUsbDotNet;
using LibUsbDotNet.Main;
using System.Threading;

namespace SoundLevelSoftware_WS1361C
{
    class Program
    {
        public static UsbDevice MyUsbDevice;

        public static UsbDeviceFinder MyUsbFinder = new UsbDeviceFinder(0x16c0, 0x5dc);
        static void Main(string[] args)
        {
            MyUsbDevice = UsbDevice.OpenUsbDevice(MyUsbFinder);
            if (MyUsbDevice == null) 
                throw new Exception("WS1361C Noise Level Meter Not Connected.");

            UsbSetupPacket usbSetupPacket = new UsbSetupPacket(0xC0, 4, 0, 10, 200);
            byte[] Values = new byte[4];
            int Length;
            double LastValue = 0;
            while (true)
            {
                Thread.Sleep(10);
                MyUsbDevice.ControlTransfer(ref usbSetupPacket, Values, Values.Length, out Length);
                double db = (Values[0] + ((Values[1] & 3) * 256)) * 0.1 + 30;
                if (LastValue == db)
                    continue;
                LastValue = db;
                Console.WriteLine(LastValue);
            }
        }
    }
}
