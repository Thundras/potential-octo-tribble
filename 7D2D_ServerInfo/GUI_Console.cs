using System;
using System.Collections.Generic;
using System.Drawing;
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
            Console.WindowWidth = 111; // MaxDaysHorizontal * MaxCharactersPerDay -3;
            Console.WindowHeight = 20;
        }

        public void Draw()
        {
            var OffsetY = 0;
            //for (var CurrentDay = 0; CurrentDay < MaxDaysHorizontal; CurrentDay++)
            //{
            //    
            //    Console.ForegroundColor = ConsoleColor.Gray;
            //    Console.BackgroundColor = ConsoleColor.Black;
            //    Console.SetCursorPosition(CurrentDay * MaxCharactersPerDay + 3, 1);
            //    Console.Write($" {_ServerInfo.CurrentServerTimeDays + CurrentDay}  ");

            //    if (_ServerInfo.IsBloodMoon(_ServerInfo.CurrentServerTimeDays + CurrentDay))
            //        Console.ForegroundColor = ConsoleColor.Red;
            //    else
            //        Console.ForegroundColor = ConsoleColor.Gray;

            //    if (CurrentDay == 0)
            //        Console.BackgroundColor = ConsoleColor.DarkGreen;
            //    else
            //        Console.BackgroundColor = ConsoleColor.Black;

            //    Console.SetCursorPosition(CurrentDay * MaxCharactersPerDay + 3, 2);
            //    Console.Write("╔═══╗");
            //    Console.SetCursorPosition(CurrentDay * MaxCharactersPerDay + 3, 3);
            //    Console.Write("║   ║");
            //    Console.SetCursorPosition(CurrentDay * MaxCharactersPerDay + 3, 4);
            //    Console.Write("╚═══╝");
            //    //╔╗╚╝═║

            //    if (_ServerInfo.IsAirdrop(_ServerInfo.CurrentServerTimeDays + CurrentDay))
            //    {
            //        Console.ForegroundColor = ConsoleColor.Yellow;
            //        Console.SetCursorPosition(CurrentDay * MaxCharactersPerDay + 1 + 3, 3);
            //        Console.Write(" A ");
            //    }
            //}

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;

            OffsetY = 1;

            Console.SetCursorPosition(3, OffsetY + 0); Console.WriteLine($"Server Name : {_ServerInfo.GameHost}");
            Console.SetCursorPosition(3, OffsetY + 1); Console.WriteLine($"Beschreibung: {_ServerInfo.ServerDescription}");
            Console.SetCursorPosition(3, OffsetY + 2); Console.WriteLine($"Version     : {_ServerInfo.Version}");
            Console.SetCursorPosition(3, OffsetY + 3); Console.WriteLine($"Server Zeit : {_ServerInfo.CurrentServerTimeHours:D2}:{_ServerInfo.CurrentServerTimeMins:D2}");
            Console.SetCursorPosition(3, OffsetY + 4); Console.WriteLine($"Spieler     : {_ServerInfo.CurrentPlayers}/{_ServerInfo.MaxPlayers}");

            DateTime CalendarStart = _ServerInfo.GetCalendarStart();
            DateTime CalendarEnd = _ServerInfo.GetCalendarEnd(5);


            OffsetY = 7;
            CultureInfo cultureInfo = CultureInfo.CurrentCulture; //new CultureInfo("de-DE");
            var tmp = cultureInfo.Calendar.GetWeekOfYear(new DateTime(2018, 12, 31), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            for (var MonthIndex = 0; MonthIndex < 5; MonthIndex++)
            {
                var curMonthDate = CalendarStart.AddMonths(MonthIndex);

                var MonthName = $"{curMonthDate.ToString("MMMM", cultureInfo)} {curMonthDate.Year - _ServerInfo.CurrentServerTimeDateInitial.Year + 1:D2}";
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.SetCursorPosition(21 * MonthIndex + 3, OffsetY +  0); Console.Write($"╔═══════════════════╗");
                Console.SetCursorPosition(21 * MonthIndex + 3, OffsetY +  1); Console.Write($"║  {MonthName,-15}  ║");
                Console.SetCursorPosition(21 * MonthIndex + 3, OffsetY +  2); Console.Write($"║                   ║");
                Console.SetCursorPosition(21 * MonthIndex + 3, OffsetY +  3); Console.Write($"╠═╦══╦══╦══╦══╦══╦══╣");
                Console.SetCursorPosition(21 * MonthIndex + 3, OffsetY +  4); Console.Write($"║M║  ║  ║  ║  ║  ║  ║");
                Console.SetCursorPosition(21 * MonthIndex + 3, OffsetY +  5); Console.Write($"║D║  ║  ║  ║  ║  ║  ║");
                Console.SetCursorPosition(21 * MonthIndex + 3, OffsetY +  6); Console.Write($"║M║  ║  ║  ║  ║  ║  ║");
                Console.SetCursorPosition(21 * MonthIndex + 3, OffsetY +  7); Console.Write($"║D║  ║  ║  ║  ║  ║  ║");
                Console.SetCursorPosition(21 * MonthIndex + 3, OffsetY +  8); Console.Write($"║F║  ║  ║  ║  ║  ║  ║");
                Console.SetCursorPosition(21 * MonthIndex + 3, OffsetY +  9); Console.Write($"║S║  ║  ║  ║  ║  ║  ║");
                Console.SetCursorPosition(21 * MonthIndex + 3, OffsetY + 10); Console.Write($"║S║  ║  ║  ║  ║  ║  ║");
                Console.SetCursorPosition(21 * MonthIndex + 3, OffsetY + 11); Console.Write($"╚═╩══╩══╩══╩══╩══╩══╝");


                var curWeekDate = curMonthDate;

                for (var WeekIndex = 0; WeekIndex < 6; WeekIndex++)
                {
                    if (curWeekDate.Month != curMonthDate.Month) break;

                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.SetCursorPosition(21 * MonthIndex + 3 + WeekIndex * 3 + 3, 9); Console.Write($"{cultureInfo.Calendar.GetWeekOfYear(curWeekDate, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday),2}");

                    if (WeekIndex == 0)
                    {
                        var curWeekDay = (int)curWeekDate.DayOfWeek;
                        curWeekDay = curWeekDay - 1;
                        if (curWeekDay < 0) curWeekDay += 7;
                        curWeekDate = curWeekDate.AddDays(-curWeekDay);
                    }

                    var Day = curWeekDate;
                    for (var DayIndex = 0; DayIndex < 7; DayIndex++)
                    {
                        if (Day.Month == curMonthDate.Month)
                        {
                            if (_ServerInfo.IsBloodMoon(Day))
                                Console.BackgroundColor = ConsoleColor.Red;
                            else
                                Console.BackgroundColor = ConsoleColor.Black;

                            if (_ServerInfo.IsAirdrop(Day))
                                Console.ForegroundColor = ConsoleColor.Green;
                            else
                                Console.ForegroundColor = ConsoleColor.Gray;

                            if (_ServerInfo.CurrentServerTimeDays == (Day - _ServerInfo.CurrentServerTimeDateInitial).Days + 1 )
                            {
                                ConsoleColor color = Console.BackgroundColor;
                                Console.BackgroundColor = Console.ForegroundColor;
                                Console.ForegroundColor = color;

                            }
                                Console.SetCursorPosition(21 * MonthIndex + 3 + WeekIndex * 3 + 3, 11 + DayIndex); Console.Write($"{Day.Day,2}");
                        }
                        Day = Day.AddDays(1);
                    }
                    curWeekDate = curWeekDate.AddDays(7);
                }

            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;
            String2ASCII($"{_ServerInfo.CurrentServerTimeDays} {_ServerInfo.CurrentServerTimeHours:D2}:{_ServerInfo.CurrentServerTimeMins:D2}", new Point(29,1));

        }

        public void DrawConnectionError()
        {
            //Console.Clear();
            //Console.ResetColor();
            Console.WriteLine($"{DateTime.Now.ToString()}: Fehler beim herstellen der Verbindung.");
            Console.WriteLine($"{DateTime.Now.ToString()}: Neuer Verbindungsversuch.");
        }

        private void String2ASCII(string Text, Point Position)
        {
            var StringLength = 0;
            foreach (char currentChar in Text)
            {
                switch (currentChar.ToString())
                {
                    case "0":
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 0); Console.Write(" ██████╗  ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 1); Console.Write("██╔═████╗ ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 2); Console.Write("██║██╔██║ ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 3); Console.Write("████╔╝██║ ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 4); Console.Write("╚██████╔╝ ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 5); Console.Write(" ╚═════╝  ");
                        StringLength += 10;
                        break;
                    case "1":
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 0); Console.Write("    ██╗   ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 1); Console.Write("   ███║   ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 2); Console.Write("   ╚██║   ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 3); Console.Write("    ██║   ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 4); Console.Write("    ██║   ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 5); Console.Write("    ╚═╝   ");
                        StringLength += 10;
                        break;
                    case "2":
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 0); Console.Write(" ██████╗  ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 1); Console.Write(" ╚════██╗ ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 2); Console.Write("  █████╔╝ ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 3); Console.Write(" ██╔═══╝  ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 4); Console.Write(" ███████╗ ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 5); Console.Write(" ╚══════╝ ");
                        StringLength += 10;
                        break;
                    case "3":
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 0); Console.Write(" ██████╗  ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 1); Console.Write(" ╚════██╗ ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 2); Console.Write("  █████╔╝ ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 3); Console.Write("  ╚═══██╗ ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 4); Console.Write(" ██████╔╝ ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 5); Console.Write(" ╚═════╝  ");
                        StringLength += 10;
                        break;
                    case "4":
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 0); Console.Write(" ██╗  ██╗ ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 1); Console.Write(" ██║  ██║ ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 2); Console.Write(" ███████║ ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 3); Console.Write(" ╚════██║ ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 4); Console.Write("      ██║ ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 5); Console.Write("      ╚═╝ ");
                        StringLength += 10;
                        break;
                    case "5":
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 0); Console.Write(" ███████╗ ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 1); Console.Write(" ██╔════╝ ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 2); Console.Write(" ███████╗ ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 3); Console.Write(" ╚════██║ ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 4); Console.Write(" ███████║ ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 5); Console.Write(" ╚══════╝ ");
                        StringLength += 10;
                        break;
                    case "6":
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 0); Console.Write(" ██████╗  ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 1); Console.Write("██╔════╝  ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 2); Console.Write("███████╗  ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 3); Console.Write("██╔═══██╗ ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 4); Console.Write("╚██████╔╝ ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 5); Console.Write(" ╚═════╝  ");
                        StringLength += 10;
                        break;
                    case "7":
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 0); Console.Write(" ███████╗ ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 1); Console.Write(" ╚════██║ ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 2); Console.Write("     ██╔╝ ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 3); Console.Write("    ██╔╝  ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 4); Console.Write("    ██║   ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 5); Console.Write("    ╚═╝   ");
                        StringLength += 10;
                        break;
                    case "8":
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 0); Console.Write("  █████╗  ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 1); Console.Write(" ██╔══██╗ ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 2); Console.Write(" ╚█████╔╝ ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 3); Console.Write(" ██╔══██╗ ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 4); Console.Write(" ╚█████╔╝ ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 5); Console.Write("  ╚════╝  ");
                        StringLength += 10;
                        break;
                    case "9":
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 0); Console.Write("  █████╗  ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 1); Console.Write(" ██╔══██╗ ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 2); Console.Write(" ╚██████║ ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 3); Console.Write("  ╚═══██║ ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 4); Console.Write("  █████╔╝ ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 5); Console.Write("  ╚════╝  ");
                        StringLength += 10;
                        break;
                    case ":":
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 0); Console.Write("     ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 1); Console.Write(" ██╗ ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 2); Console.Write(" ╚═╝ ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 3); Console.Write(" ██╗ ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 4); Console.Write(" ╚═╝ ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 5); Console.Write("     ");
                        StringLength += 5;
                        break;
                    case " ":
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 0); Console.Write("     ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 1); Console.Write("     ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 2); Console.Write("     ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 3); Console.Write("     ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 4); Console.Write("     ");
                        Console.SetCursorPosition(Position.X + StringLength, Position.Y + 5); Console.Write("     ");
                        StringLength += 5;
                        break;
                    default:
                        break;
                }
            }
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
