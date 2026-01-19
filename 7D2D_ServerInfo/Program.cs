using System;
using System.Threading;
using System.Threading.Tasks;

namespace _7D2D_ServerInfo
{
    class Program
    {
        private static readonly Uri RemoteConfigUri = new("https://raw.githubusercontent.com/Thundras/potential-octo-tribble/main/config/server-config.json");

        static async Task Main(string[] args)
        {
            bool debug = ProgramHelpers.IsDebugMode(args);
            RemoteConfig? config = await TryLoadConfigAsync();
            if (config is null)
            {
                Console.Error.WriteLine("Remote config was empty or invalid.");
                return;
            }

            using var cancellation = new CancellationTokenSource();
            Console.CancelKeyPress += (_, eventArgs) =>
            {
                eventArgs.Cancel = true;
                cancellation.Cancel();
            };

            await RunAsync(config, debug, cancellation.Token);
        }

        private static async Task<RemoteConfig?> TryLoadConfigAsync()
        {
            try
            {
                return await RemoteConfigLoader.LoadAsync(RemoteConfigUri, CancellationToken.None);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load remote config: {ex.Message}");
                return null;
            }
        }

        private static async Task RunAsync(RemoteConfig config, bool debug, CancellationToken cancellationToken)
        {
            Console.Title = $"7 Days to Die - Horde & Airdrop Viewer - {config.ServerHost}:{config.ServerPort}";

            _ = UpdateBootstrapper.TryStart(config);

            IConnection connection = debug
                ? new ConnectionUDP()
                : new ConnectionUDP(config.ServerHost, config.ServerPort);

            _7D2D_ServerInfo serverInfo = new _7D2D_ServerInfo(connection, debug);
            IGUI gui = new GUI_Console(serverInfo);
            TimeSpan refreshDelay = ProgramHelpers.GetRefreshDelay(config.RefreshIntervalSeconds);

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    if (serverInfo.Refresh())
                        gui.Draw();
                    else
                        gui.DrawConnectionError();

                    await Task.Delay(refreshDelay, cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }
    }
}
