using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace _7D2D_ServerInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.Title = "7 Days to die - Horde & Supply Viewer";

            bool debug = false;
            foreach (string arg in args)
            {
                if (arg.ToLower() == "/Debug".ToLower())
                    debug = true;
            }
            
            _7D2D_ServerInfo i = new _7D2D_ServerInfo(debug);

            do
            {
                //Console.Clear();

                i.Refresh();

                for (var j = 0; j < 19; j++)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.SetCursorPosition(j * 6 + 3, 1);
                    Console.Write($" {i.CurrentServerTimeDays + j}  ");

                    if (i.IsBloodMoon(i.CurrentServerTimeDays + j))
                        Console.ForegroundColor = ConsoleColor.Red;
                    else
                        Console.ForegroundColor = ConsoleColor.Gray;

                    if (j == 0)
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                    else
                        Console.BackgroundColor = ConsoleColor.Black;

                    Console.SetCursorPosition(j * 6 + 3, 2);
                    Console.Write("╔═══╗");
                    Console.SetCursorPosition(j * 6 + 3, 3);
                    Console.Write("║   ║");
                    Console.SetCursorPosition(j * 6 + 3, 4);
                    Console.Write("╚═══╝");
                    //╔╗╚╝═║

                    if (i.IsAirdrop(i.CurrentServerTimeDays + j))
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.SetCursorPosition(j * 6 + 1 + 3, 3);
                        Console.Write(" A ");
                    }
                }

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.BackgroundColor = ConsoleColor.Black;

                Console.SetCursorPosition(0, 6);
                Console.WriteLine($"   Server Name : {i.GameHost}");
                Console.WriteLine($"   Beschreibung: {i.ServerDescription}");
                Console.WriteLine($"   Version     : {i.Version}");
                Console.WriteLine($"   Server Zeit : {i.CurrentServerTimeHours:D2}:{i.CurrentServerTimeMins:D2}");
                Console.WriteLine($"   Spieler     : {i.CurrentPlayers}/{i.MaxPlayers}");

                System.Threading.Thread.Sleep((int)(60f / 24f * 1000f));

            } while (true);
        }
    }
}
