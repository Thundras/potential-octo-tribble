using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace _7D2D_ServerInfo
{
    class _7D2D_ServerInfo
    {
        public enum enumDropOnDeath
        {
            Everything = 0,
            Toolbelt_Only = 1,
            Backpack_Only = 2,
            Delete_All = 3
        }
        public enum enumDropOnQuit
        {
            Nothing = 0,
            Everything = 1,
            Toolbelt_Only = 2,
            Backpack_Only = 3
        }
        public enum enumGameDifficulty
        {
            Scavenger = 0, //(0): Easiest setting designed for noobs. - Easiest
            Adventurer = 1, //(1): Not just a new recruit any more. - Easy
            Nomad = 2, //(2): Designed for experienced FPS players. - Normal
            Warrior = 3, //(3): Now you're starting to impress. - Hard
            Survivalist = 4, //(4) : Kids don't try this at home! - Harder
            Insane = 5  //(5) : You're one brave Mother! - Hardest
        }
        public enum enumPlayerKillingMode
        {
            No_Killing = 0, //No Killing: Players cannot damage one another under any circumstances.
            Kill_Allies_Only = 1, //Kill Allies Only: Players can only damage each other if both agree to it by adding each other to their Allies list using the ingame players menu.
            Kill_Strangers_Only = 2, //Kill Strangers Only: Players can damage eachother UNLESS they are Allies.
            Kill_Everyone = 3 //Kill Everyone: Players can damage everyone, regardless of Allied status
        }
        public enum enumZombieRun
        {
            Default = 0, //Default - This setting will make the Zombies walk in the daytime and run at night.
            Never_Run = 1, //Never Run - This setting will make the Zombies walk all the time regardless of the time of day.
            Always_Run = 2  //Always Run - This setting will make the Zombies run all the time regardless of the time of day.
        }

        public int AirDropFrequency { get; private set; }
        public bool AirDropMarker { get; private set; }
        public bool Architecture64 { get; private set; }
        public int BlockDurabilityModifier { get; private set; }
        public int BloodMoonEnemyCount { get; private set; }
        public bool BuildCreate { get; private set; }
        public string CompatibilityVersion { get; private set; }
        public string CountryCode { get; private set; }
        public int CurrentPlayers { get; private set; }
        public int CurrentServerTime { get; private set; }
        public int DayCount { get; private set; }
        public int DayLightLength { get; private set; }
        public int DayNightLength { get; private set; }
        public enumDropOnDeath DropOnDeath { get; private set; }
        public enumDropOnQuit DropOnQuit { get; private set; }
        public bool EACEnabled { get; private set; }
        public int EnemyDifficulty { get; private set; }
        public bool EnemySpawnMode { get; private set; }
        public enumGameDifficulty GameDifficulty { get; private set; }
        public string GameHost { get; private set; }
        public string GameMode { get; private set; }
        public string GameName { get; private set; }
        public string GameType { get; private set; }
        public string IP { get; private set; }
        public bool IsDedicated { get; private set; }
        public bool IsPasswordProtected { get; private set; }
        public bool IsPublic { get; private set; }
        public int LandClaimDeadZone { get; private set; }//LandClaimDeadZone: 30
        public int LandClaimDecayMode { get; private set; }//LandClaimDecayMode: 0
        public int LandClaimExpiryTime { get; private set; }//LandClaimExpiryTime: 3
        public int LandClaimOfflineDurabilityModifier { get; private set; }//LandClaimOfflineDurabilityModifier: 0
        public int LandClaimOnlineDurabilityModifier { get; private set; }//LandClaimOnlineDurabilityModifier: 0
        public int LandClaimSize { get; private set; }//LandClaimSize: 50
        public string LevelName { get; private set; }//LevelName: Random Gen
        public int LootAbundance { get; private set; }//LootAbundance: 50
        public int LootRespawnDays { get; private set; }//LootRespawnDays: 7
        public int MaxPlayers { get; private set; }//MaxPlayers: 10
        public int MaxSpawnedAnimals { get; private set; }//MaxSpawnedAnimals: 50
        public int MaxSpawnedZombies { get; private set; }//MaxSpawnedZombies: 40
        public int Ping { get; private set; }//Ping: -1
        public string Platform { get; private set; }//Platform: LinuxPlayer
        public enumPlayerKillingMode PlayerKillingMode { get; private set; }//PlayerKillingMode: 2
        public int Port { get; private set; }//Port: 27260
        public bool RequiresMod { get; private set; }//RequiresMod: False
        public string ServerDescription { get; private set; }//ServerDescription: Chaos
        public string ServerWebsiteURL { get; private set; }//ServerWebsiteURL:
        public bool ShowFriendPlayerOnMap { get; private set; }//ShowFriendPlayerOnMap: True
        public string SteamID { get; private set; }//SteamID: 90114274196415493
        public bool StockFiles { get; private set; }//StockFiles: False
        public bool StockSettings { get; private set; }//StockSettings: False
        public string Version { get; private set; }//Version: Alpha 16.4
        public enumZombieRun ZombiesRun { get; private set; }//ZombiesRun: 0

        public int CurrentServerTimeYear { get; private set; }
        public int CurrentServerTimeMonth { get; private set; }
        public int CurrentServerTimeDay { get; private set; }
        public int CurrentServerTimeDays { get; private set; }
        public int CurrentServerTimeHours { get; private set; }
        public int CurrentServerTimeMins { get; private set; }
        public int CurrentServerTimeNextBloodMoon { get; private set; }
        public int CurrentServerTimeNextSupply { get; private set; }
        public DateTime CurrentServerTimeDate { get; private set; }
        public DateTime CurrentServerTimeDateInitial { get; private set; }

        public DateTime LastUpdate { get; private set; }

        private IConnection Con;

        bool debug = false;
        public _7D2D_ServerInfo(): this(false) {}
        public _7D2D_ServerInfo(bool Debug)
        {
            CurrentServerTimeDateInitial = new DateTime(2018, 1, 1);

            debug = Debug;
            if (Debug == false)
                Con = new ConnectionUDP( "185.239.237.61", 37018);
            else
                Con = new ConnectionUDP();
            Refresh();
        }

        public void Refresh()
        {
            if ((DateTime.Now - LastUpdate).TotalMilliseconds > 30000)
            {
                Fill();
                LastUpdate = DateTime.Now;
            }
            else
            {
                if (CurrentPlayers > 0) CurrentServerTime += 16;
            }
            CalcCurrentServerTime();
        }

        private void Fill()
        {
            byte[] receivedData = Con.Refresh();
            if (receivedData is null) return;
            string Info = System.Text.Encoding.Default.GetString(receivedData);
            string[] list = Info.Split(new Char[] { (char)0 });

            for (var i = 1; i < list.Length - 1; i += 2)
            {
                PropertyInfo _propertyInfo = GetType().GetProperty(list[i]);
                _propertyInfo.SetValue(this, CastPropertyValue(_propertyInfo, list[i + 1]), null);
            }
            if (debug == true) CurrentPlayers = 1;
        }

        private void CalcCurrentServerTime()
        {
            //CurrentServerTime = 13172166;
            CurrentServerTimeDays = (int)((float)((CurrentServerTime / 24000) + 1));
            CurrentServerTimeHours = (int)((float)((CurrentServerTime % 24000) / 1000));
            CurrentServerTimeMins = (int)((float)((float)(CurrentServerTime % 1000) * 60) / 1000);
            CurrentServerTimeNextBloodMoon = (7 - (CurrentServerTimeDays % 7)) % 7;
            CurrentServerTimeNextSupply = ((AirDropFrequency / 24) - CurrentServerTimeDays % (AirDropFrequency / 24) + 1) % (AirDropFrequency / 24);
            CurrentServerTimeDate = CurrentServerTimeDateInitial.AddDays(CurrentServerTimeDays);

            CurrentServerTimeYear = CurrentServerTimeDate.Year - CurrentServerTimeDateInitial.Year + 1;
            CurrentServerTimeMonth = CurrentServerTimeDate.Month;
            CurrentServerTimeDay = CurrentServerTimeDate.Day;
        }

        public DateTime GetCalendarStart()
        {
            return new DateTime(CurrentServerTimeDate.Year, CurrentServerTimeDate.Month, 1);
        }
        public DateTime GetCalendarEnd()
        {
            return GetCalendarEnd(5);
        }

        public DateTime GetCalendarEnd(int MonthDuration)
        {
            return (new DateTime(CurrentServerTimeDate.AddMonths(MonthDuration).Year, CurrentServerTimeDate.AddMonths(MonthDuration).Month, 1)).AddDays(-1);
        }

        public bool IsBloodMoon(int Day)
        {
            return ((7 - (Day % 7)) % 7) == 0;
        }

        public bool IsAirdrop(int Day)
        {
            return (((AirDropFrequency / 24) - Day % (AirDropFrequency / 24) + 1) % (AirDropFrequency / 24)) == 0;
        }

        public object CastPropertyValue(PropertyInfo property, string value)
        {
            if (property == null || String.IsNullOrEmpty(value))
                return null;
            if (property.PropertyType.IsEnum)
            {
                Type enumType = property.PropertyType;
                int tmp;
                if (Enum.IsDefined(enumType, value))
                    return Enum.Parse(enumType, value);
                else if (Int32.TryParse(value, out tmp))
                    return tmp;
            }
            if (property.PropertyType == typeof(bool))
                return value == "1" || value.ToLower() == "True".ToLower();
            else if (property.PropertyType == typeof(Uri))
                return new Uri(Convert.ToString(value));
            else
                return Convert.ChangeType(value, property.PropertyType);
        }
    }
}
