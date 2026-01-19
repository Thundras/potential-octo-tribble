using System;
using System.Threading;

namespace _7D2D_ServerInfo
{
    class Program
    {
        private static readonly Uri RemoteConfigUri = new("https://raw.githubusercontent.com/Thundras/potential-octo-tribble/main/config/server-config.json");

        static async Task Main(string[] args)
        {
            bool debug = false;
            foreach (string arg in args)
            {
                if (arg.ToLower() == "/Debug".ToLower())
                    debug = true;
            }

            RemoteConfig? config = null;
            try
            {
                config = await RemoteConfigLoader.LoadAsync(RemoteConfigUri, CancellationToken.None);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load remote config: {ex.Message}");
                return;
            }

            if (config is null)
            {
                Console.Error.WriteLine("Remote config was empty or invalid.");
                return;
            }

            Console.Title = $"7 Days to Die - Horde & Airdrop Viewer - {config.ServerHost}:{config.ServerPort}";

            _ = UpdateBootstrapper.TryStart(config);

            IConnection connection = debug
                ? new ConnectionUDP()
                : new ConnectionUDP(config.ServerHost, config.ServerPort);

            _7D2D_ServerInfo i = new _7D2D_ServerInfo(connection, debug);
            IGUI GUI = new GUI_Console(i);

            do
            {
                if (i.Refresh())
                    GUI.Draw();
                else
                    GUI.DrawConnectionError();

                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(config.RefreshIntervalSeconds));
            } while (true);
        }
    }
}
