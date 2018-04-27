using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Deployment.Application;
using System.Threading;
using System.Runtime.InteropServices;

namespace _7D2D_ServerInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;


            //new Thread(() => new Form1().ShowDialog()).Start();

            var Version = System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();

            Console.Title = $"7 Days to die - Horde & Airdrop Viewer - {Version}";

            bool debug = false;
            foreach (string arg in args)
            {
                if (arg.ToLower() == "/Debug".ToLower())
                    debug = true;
            }

            _7D2D_ServerInfo i = new _7D2D_ServerInfo(debug);
            IGUI GUI = new GUI_Console(i);

            do
            {
                if (i.Refresh())
                    GUI.Draw();
                else
                    GUI.DrawConnectionError();

                System.Threading.Thread.Sleep((int)(60f / 24f * 1000f));

            } while (true);
        }
    }
}
