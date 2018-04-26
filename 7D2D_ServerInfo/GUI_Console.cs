using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7D2D_ServerInfo
{
    class GUI_Console : IGUI
    {
        private const int MaxDaysHorizontal = 19;
        private const int MaxCharactersPerDay = 6;
        _7D2D_ServerInfo _ServerInfo;

        public GUI_Console(_7D2D_ServerInfo _ServerInfo)
        {
            this._ServerInfo = _ServerInfo;
        }

        public void Draw()
        {
            for (var CurrentDay = 0; CurrentDay < MaxDaysHorizontal; CurrentDay++)
            {
                Console.WindowWidth = MaxDaysHorizontal * MaxCharactersPerDay + 3 + 3;
                Console.WindowHeight = 25;
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(CurrentDay * MaxCharactersPerDay + 3, 1);
                Console.Write($" {_ServerInfo.CurrentServerTimeDays + CurrentDay}  ");

                if (_ServerInfo.IsBloodMoon(_ServerInfo.CurrentServerTimeDays + CurrentDay))
                    Console.ForegroundColor = ConsoleColor.Red;
                else
                    Console.ForegroundColor = ConsoleColor.Gray;

                if (CurrentDay == 0)
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                else
                    Console.BackgroundColor = ConsoleColor.Black;

                Console.SetCursorPosition(CurrentDay * MaxCharactersPerDay + 3, 2);
                Console.Write("╔═══╗");
                Console.SetCursorPosition(CurrentDay * MaxCharactersPerDay + 3, 3);
                Console.Write("║   ║");
                Console.SetCursorPosition(CurrentDay * MaxCharactersPerDay + 3, 4);
                Console.Write("╚═══╝");
                //╔╗╚╝═║

                if (_ServerInfo.IsAirdrop(_ServerInfo.CurrentServerTimeDays + CurrentDay))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.SetCursorPosition(CurrentDay * MaxCharactersPerDay + 1 + 3, 3);
                    Console.Write(" A ");
                }
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;

            Console.SetCursorPosition(3, 6);
            Console.WriteLine($"Server Name : {_ServerInfo.GameHost}");
            Console.WriteLine($"Beschreibung: {_ServerInfo.ServerDescription}");
            Console.WriteLine($"Version     : {_ServerInfo.Version}");
            Console.WriteLine($"Server Zeit : {_ServerInfo.CurrentServerTimeHours:D2}:{_ServerInfo.CurrentServerTimeMins:D2}");
            Console.WriteLine($"Spieler     : {_ServerInfo.CurrentPlayers}/{_ServerInfo.MaxPlayers}");

            /*
            //│┌┐└┘├┤┬┴┼═║╒╓╔╕╖╗╘╙╚╛╜╝╞╟╠╡╢╣╤╥╦╧╨╩╪╫╬
            Console.WriteLine($"   ╔═══════════════════╗╔═══════════════════╗╔═══════════════════╗╔═══════════════════╗");
            Console.WriteLine($"   ║     Monthname     ║║     Monthname     ║║     Monthname     ║║     Monthname     ║");
            Console.WriteLine($"   ║  ❶  ❷  ❸  ❹  ❺  ❻ ║║  ❶  ❷  ❸  ❹  ❺  ❻ ║║  ❶  ❷  ❸  ❹  ❺  ❻ ║║  ❶  ❷  ❸  ❹  ❺  ❻ ║");
            Console.WriteLine($"   ╠═╤══╤══╤══╤══╤══╤══╣╠═╤══╤══╤══╤══╤══╤══╣╠═╤══╤══╤══╤══╤══╤══╣╠═╤══╤══╤══╤══╤══╤══╣");
            Console.WriteLine($"   ║M│ 1│ 8│15│22│29│  ║║M│ 1│ 8│15│22│29│  ║║M│ 1│ 8│15│22│29│  ║║M│ 1│ 8│15│22│29│  ║");
            Console.WriteLine($"   ║D│ 2│ 9│16│23│30│  ║║D│ 2│ 9│16│23│30│  ║║D│ 2│ 9│16│23│30│  ║║D│ 2│ 9│16│23│30│  ║");
            Console.WriteLine($"   ║M│ 3│10│17│24│31│  ║║M│ 3│10│17│24│31│  ║║M│ 3│10│17│24│31│  ║║M│ 3│10│17│24│31│  ║");
            Console.WriteLine($"   ║D│ 4│11│18│25│  │  ║║D│ 4│11│18│25│  │  ║║D│ 4│11│18│25│  │  ║║D│ 4│11│18│25│  │  ║");
            Console.WriteLine($"   ║F│ 5│12│19│26│  │  ║║F│ 5│12│19│26│  │  ║║F│ 5│12│19│26│  │  ║║F│ 5│12│19│26│  │  ║");
            Console.WriteLine($"   ║S│ 6│13│20│27│  │  ║║S│ 6│13│20│27│  │  ║║S│ 6│13│20│27│  │  ║║S│ 6│13│20│27│  │  ║");
            Console.WriteLine($"   ║S│ 7│14│21│28│  │  ║║S│ 7│14│21│28│  │  ║║S│ 7│14│21│28│  │  ║║S│ 7│14│21│28│  │  ║");
            Console.WriteLine($"   ╚═╧══╧══╧══╧══╧══╧══╝╚═╧══╧══╧══╧══╧══╧══╝╚═╧══╧══╧══╧══╧══╧══╝╚═╧══╧══╧══╧══╧══╧══╝");
            */
            ///*
            DateTime CalendarStart = _ServerInfo.GetCalendarStart();// new DateTime(CurrentServerTimeDate.Year, CurrentServerTimeDate.Month, 1);
            DateTime CalendarEnd = _ServerInfo.GetCalendarEnd(5);// (new DateTime(CurrentServerTimeDate.AddMonths(3).Year, CurrentServerTimeDate.AddMonths(3).Month, 1)).AddDays(-1);

            CultureInfo xy = CultureInfo.CurrentCulture; //new CultureInfo("de-DE");
            var tmp = xy.Calendar.GetWeekOfYear(new DateTime(2018, 12, 31), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            for (var j = 0; j < 5; j++)
            {
                var curMonthDate = CalendarStart.AddMonths(j);

                var MonthName = curMonthDate.ToString("MMMM", xy);
                Console.SetCursorPosition(21 * j + 3, 12); Console.Write($"╔═══════════════════╗");
                Console.SetCursorPosition(21 * j + 3, 13); Console.Write($"║{MonthName,-19    }║");
                Console.SetCursorPosition(21 * j + 3, 14); Console.Write($"║                   ║");
                Console.SetCursorPosition(21 * j + 3, 15); Console.Write($"╠═╦══╦══╦══╦══╦══╦══╣");
                Console.SetCursorPosition(21 * j + 3, 16); Console.Write($"║M║  ║  ║  ║  ║  ║  ║");
                Console.SetCursorPosition(21 * j + 3, 17); Console.Write($"║D║  ║  ║  ║  ║  ║  ║");
                Console.SetCursorPosition(21 * j + 3, 18); Console.Write($"║M║  ║  ║  ║  ║  ║  ║");
                Console.SetCursorPosition(21 * j + 3, 19); Console.Write($"║D║  ║  ║  ║  ║  ║  ║");
                Console.SetCursorPosition(21 * j + 3, 20); Console.Write($"║F║  ║  ║  ║  ║  ║  ║");
                Console.SetCursorPosition(21 * j + 3, 21); Console.Write($"║S║  ║  ║  ║  ║  ║  ║");
                Console.SetCursorPosition(21 * j + 3, 22); Console.Write($"║S║  ║  ║  ║  ║  ║  ║");
                Console.SetCursorPosition(21 * j + 3, 23); Console.Write($"╚═╩══╩══╩══╩══╩══╩══╝");

                
                
                for (var k = 0; k < 6; k++)
                {
                    var curWeekDate = curMonthDate.AddDays(k * 7);
                    
                }

            }





            //Console.SetCursorPosition(0, 13);
            //for (DateTime x = CalendarStart; x <= CalendarEnd; x = x.AddDays(1))
            //{
            //    TimeSpan ts = x - _ServerInfo.CurrentServerTimeDateInitial.AddDays(-1);
            //    Console.WriteLine($"   {ts.Days,4} {x.DayOfYear,4} {x.DayOfWeek.ToString().Substring(0, 2)} {x.Day:D2}.{x.Month:D2}.{x.Year - _ServerInfo.CurrentServerTimeDateInitial.Year + 1:D2}");
            //}
            //*/
        }

        #region IDisposable Support
        private bool disposedValue = false; // Dient zur Erkennung redundanter Aufrufe.

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: verwalteten Zustand (verwaltete Objekte) entsorgen.
                }

                // TODO: nicht verwaltete Ressourcen (nicht verwaltete Objekte) freigeben und Finalizer weiter unten überschreiben.
                // TODO: große Felder auf Null setzen.

                disposedValue = true;
            }
        }

        // TODO: Finalizer nur überschreiben, wenn Dispose(bool disposing) weiter oben Code für die Freigabe nicht verwalteter Ressourcen enthält.
        // ~GUI_Console() {
        //   // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in Dispose(bool disposing) weiter oben ein.
        //   Dispose(false);
        // }

        // Dieser Code wird hinzugefügt, um das Dispose-Muster richtig zu implementieren.
        public void Dispose()
        {
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in Dispose(bool disposing) weiter oben ein.
            Dispose(true);
            // TODO: Auskommentierung der folgenden Zeile aufheben, wenn der Finalizer weiter oben überschrieben wird.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
