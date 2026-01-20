using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace _7D2D_ServerInfo
{
    /// <summary>
    /// UDP implementation for querying a 7DTD server using the Source Engine protocol.
    /// </summary>
    class ConnectionUDP : IConnection
    {
        // The UDP endpoint resolved from the configured host/port.
        private IPEndPoint endpoint;
        private readonly bool debug;

        /// <summary>
        /// Initializes a debug-only connection that returns static sample data.
        /// </summary>
        public ConnectionUDP()
        {
            // In debug mode, no remote endpoint is required because we return
            // hardcoded sample data.
            debug = true;
            endpoint = new IPEndPoint(IPAddress.Loopback, 0);
        }
        /// <summary>
        /// Initializes a live connection for the specified host and port.
        /// </summary>
        /// <param name="host">Hostname or IP address of the server.</param>
        /// <param name="port">Base port for the server (query uses port + 1).</param>
        /// <exception cref="InvalidOperationException">Thrown when the host cannot be resolved.</exception>
        public ConnectionUDP(string host, int port)
        {
            if (string.IsNullOrWhiteSpace(host))
            {
                throw new ArgumentException("Host must be provided.", nameof(host));
            }

            if (port <= 0 || port >= 65535)
            {
                throw new ArgumentOutOfRangeException(nameof(port), "Port must be between 1 and 65534.");
            }

            // Prefer direct IP parsing, and fall back to DNS resolution if needed.
            if (!IPAddress.TryParse(host, out var ipAddress))
            {
                // DNS resolution can return IPv6, but we prefer IPv4 when available.
                var addresses = Dns.GetHostAddresses(host);
                // Prefer IPv4 addresses when available to match typical 7DTD server setups.
                ipAddress = addresses.FirstOrDefault(address => address.AddressFamily == AddressFamily.InterNetwork)
                    ?? addresses.FirstOrDefault();
            }

            if (ipAddress is null)
            {
                // Fail fast when DNS resolution yields no usable address.
                throw new InvalidOperationException($"Could not resolve host '{host}'.");
            }

            endpoint = new IPEndPoint(ipAddress, port + 1);
        }
        /// <summary>
        /// Executes the UDP query sequence and returns the raw response payload.
        /// </summary>
        /// <returns>Byte array containing the response payload, or <c>null</c> on failure.</returns>
        public byte[]? Refresh()
        {
            byte[]? Return;
            if (debug == false)
            {
                try
                {
                    using var client = new UdpClient();
                    client.Client.SendTimeout = 5000;
                    client.Client.ReceiveTimeout = 5000;
                    // Use a local endpoint variable because Receive mutates the ref parameter.
                    var remoteEndpoint = endpoint;
                    client.Connect(remoteEndpoint);


                    var b = new byte[] { 0xff, 0xff, 0xff, 0xff, 0x54, 0x53, 0x6f, 0x75, 0x72, 0x63, 0x65, 0x20, 0x45, 0x6e, 0x67, 0x69, 0x6e, 0x65, 0x20, 0x51, 0x75, 0x65, 0x72, 0x79, 0x00 };

                    client.Send(b, b.Length);
                    // Handshake round-trip: the first response contains a challenge token.
                    _ = client.Receive(ref remoteEndpoint);

                    b = new byte[] { 0xff, 0xff, 0xff, 0xff, 0x56, 0x00, 0x00, 0x00, 0x00 };

                    client.Send(b, b.Length);

                    // The second response provides the challenge token for the query.
                    var receivedData2 = client.Receive(ref remoteEndpoint);
                    if (receivedData2.Length < 9)
                    {
                        Console.Error.WriteLine("UDP query failed: challenge response was too short.");
                        return null;
                    }

                    b = new byte[] { 0xff, 0xff, 0xff, 0xff, 0x56, receivedData2[5], receivedData2[6], receivedData2[7], receivedData2[8] };

                    client.Send(b, b.Length);

                    // Final response contains the server status payload.
                    var receivedData3 = client.Receive(ref remoteEndpoint);

                    Return = receivedData3;
                }
                catch (Exception ex)
                {
                    // Log errors to stderr so operators can troubleshoot connection issues.
                    Console.Error.WriteLine($"UDP query failed: {ex.Message}");
                    Return = null;
                }
            }
            else
            {
                // Debug path returns static sample data so the UI can render without a live server.
                Return = Encoding.Latin1.GetBytes("ÿÿÿÿE4\0AirDropFrequency\072\0AirDropMarker\0False\0Architecture64\0True\0BlockDurabilityModifier\0100\0BloodMoonEnemyCount\010\0BuildCreate\0False\0CompatibilityVersion\0Alpha 16.4\0CountryCode\0DE\0CurrentPlayers\00\0CurrentServerTime\03277234\0DayCount\03\0DayLightLength\018\0DayNightLength\060\0DropOnDeath\02\0DropOnQuit\00\0EACEnabled\0True\0EnemyDifficulty\00\0EnemySpawnMode\0True\0GameDifficulty\04\0GameHost\07 days Chaos\0GameMode\0SurvivalMP\0GameName\0reaven\0GameType\07DTD\0IP\095.156.227.89\0IsDedicated\0True\0IsPasswordProtected\0True\0IsPublic\0True\0LandClaimDeadZone\030\0LandClaimDecayMode\00\0LandClaimExpiryTime\03\0LandClaimOfflineDurabilityModifier\00\0LandClaimOnlineDurabilityModifier\00\0LandClaimSize\050\0LevelName\0Random Gen\0LootAbundance\050\0LootRespawnDays\07\0MaxPlayers\010\0MaxSpawnedAnimals\050\0MaxSpawnedZombies\040\0Ping\0-1\0Platform\0LinuxPlayer\0PlayerKillingMode\02\0Port\027260\0RequiresMod\0False\0ServerDescription\0Chaos\0ServerWebsiteURL\0\0ShowFriendPlayerOnMap\0True\0SteamID\090114285007098885\0StockFiles\0False\0StockSettings\0False\0Version\0Alpha 16.4\0ZombiesRun\00\0");
            }

            return Return;
        }
    }
}
