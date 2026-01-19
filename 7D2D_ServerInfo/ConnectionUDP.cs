using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace _7D2D_ServerInfo
{
    class ConnectionUDP : IConnection
    {
        IPEndPoint ep;
        bool Debug = false;

        public ConnectionUDP()
        {
            Debug = true;
        }
        public ConnectionUDP(string host, int port)
        {
            if (!IPAddress.TryParse(host, out var ipAddress))
            {
                ipAddress = Dns.GetHostAddresses(host).First();
            }

            ep = new IPEndPoint(ipAddress, port + 1);
        }
        public byte[] Refresh()
        {
            byte[] Return;
            if (Debug == false)
            {
                var client = new UdpClient();
                client.Client.SendTimeout = 5000;
                client.Client.ReceiveTimeout = 5000;
                try
                {
                    client.Connect(ep);


                    var b = new byte[] { 0xff, 0xff, 0xff, 0xff, 0x54, 0x53, 0x6f, 0x75, 0x72, 0x63, 0x65, 0x20, 0x45, 0x6e, 0x67, 0x69, 0x6e, 0x65, 0x20, 0x51, 0x75, 0x65, 0x72, 0x79, 0x00 };

                    client.Send(b, b.Length);
                    var receivedData1 = client.Receive(ref ep);

                    b = new byte[] { 0xff, 0xff, 0xff, 0xff, 0x56, 0x00, 0x00, 0x00, 0x00 };

                    client.Send(b, b.Length);

                    var receivedData2 = client.Receive(ref ep);

                    b = new byte[] { 0xff, 0xff, 0xff, 0xff, 0x56, receivedData2[5], receivedData2[6], receivedData2[7], receivedData2[8] };

                    client.Send(b, b.Length);

                    var receivedData3 = client.Receive(ref ep);

                    client.Close();

                    Return = receivedData3;
                }
                catch
                {
                    byte[] tmp = null;
                    Return = tmp;
                }
            }
            else
                Return = Encoding.ASCII.GetBytes("ÿÿÿÿE4\0AirDropFrequency\072\0AirDropMarker\0False\0Architecture64\0True\0BlockDurabilityModifier\0100\0BloodMoonEnemyCount\010\0BuildCreate\0False\0CompatibilityVersion\0Alpha 16.4\0CountryCode\0DE\0CurrentPlayers\00\0CurrentServerTime\03277234\0DayCount\03\0DayLightLength\018\0DayNightLength\060\0DropOnDeath\02\0DropOnQuit\00\0EACEnabled\0True\0EnemyDifficulty\00\0EnemySpawnMode\0True\0GameDifficulty\04\0GameHost\07 days Chaos\0GameMode\0SurvivalMP\0GameName\0reaven\0GameType\07DTD\0IP\095.156.227.89\0IsDedicated\0True\0IsPasswordProtected\0True\0IsPublic\0True\0LandClaimDeadZone\030\0LandClaimDecayMode\00\0LandClaimExpiryTime\03\0LandClaimOfflineDurabilityModifier\00\0LandClaimOnlineDurabilityModifier\00\0LandClaimSize\050\0LevelName\0Random Gen\0LootAbundance\050\0LootRespawnDays\07\0MaxPlayers\010\0MaxSpawnedAnimals\050\0MaxSpawnedZombies\040\0Ping\0-1\0Platform\0LinuxPlayer\0PlayerKillingMode\02\0Port\027260\0RequiresMod\0False\0ServerDescription\0Chaos\0ServerWebsiteURL\0\0ShowFriendPlayerOnMap\0True\0SteamID\090114285007098885\0StockFiles\0False\0StockSettings\0False\0Version\0Alpha 16.4\0ZombiesRun\00\0");

            return Return;
        }
    }
}
