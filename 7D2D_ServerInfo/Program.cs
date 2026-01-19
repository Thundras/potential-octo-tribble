using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace _7D2D_ServerInfo
{
    /// <summary>
    /// Application entry point that orchestrates configuration loading and UI refresh.
    /// </summary>
    class Program
    {
        // The remote JSON configuration is hosted on GitHub, allowing the app to
        // bootstrap without a dedicated backend service.
        private static readonly Uri RemoteConfigUri = new("https://raw.githubusercontent.com/Thundras/potential-octo-tribble/main/config/server-config.json");
        private static readonly string LocalConfigPath = Path.Combine(AppContext.BaseDirectory, "config", "server-config.json");

        /// <summary>
        /// Main entry for the console application.
        /// </summary>
        /// <param name="args">Command-line arguments passed to the process.</param>
        /// <returns>A task representing the asynchronous startup work.</returns>
        static async Task Main(string[] args)
        {
            bool debug = ProgramHelpers.IsDebugMode(args);
            // Protect startup from hanging network calls by applying a hard timeout
            // to the initial configuration fetch.
            using var configCts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            RemoteConfig? config = await TryLoadConfigAsync(configCts.Token);
            if (config is null)
            {
                Console.Error.WriteLine("Remote config was empty or invalid.");
                return;
            }

            // Create a linked cancellation source that is triggered on Ctrl+C to
            // allow the refresh loop to exit gracefully.
            using var cancellation = new CancellationTokenSource();
            Console.CancelKeyPress += (_, eventArgs) =>
            {
                eventArgs.Cancel = true;
                cancellation.Cancel();
            };

            await RunAsync(config, debug, cancellation.Token);
        }

        /// <summary>
        /// Attempts to load the remote configuration, returning <c>null</c> on failure.
        /// </summary>
        /// <param name="cancellationToken">Token used to cancel the HTTP request.</param>
        /// <returns>The remote configuration if loaded successfully; otherwise <c>null</c>.</returns>
        private static async Task<RemoteConfig?> TryLoadConfigAsync(CancellationToken cancellationToken)
        {
            try
            {
                // Delegate the actual fetch and JSON parsing to the loader so we can
                // centralize HTTP settings (timeouts, shared HttpClient, etc.).
                var remoteConfig = await RemoteConfigLoader.LoadAsync(RemoteConfigUri, cancellationToken);
                if (remoteConfig is not null)
                {
                    return remoteConfig;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load remote config: {ex.Message}");
            }

            try
            {
                var localConfig = await RemoteConfigLoader.LoadFromFileAsync(LocalConfigPath, cancellationToken);
                if (localConfig is not null)
                {
                    Console.Error.WriteLine("Loaded local config fallback.");
                    return localConfig;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load local config: {ex.Message}");
            }

            return null;
        }

        /// <summary>
        /// Runs the main refresh loop until cancellation is requested.
        /// </summary>
        /// <param name="config">Remote configuration values.</param>
        /// <param name="debug">Whether to use debug/sample data instead of live UDP.</param>
        /// <param name="cancellationToken">Token to stop the loop.</param>
        /// <returns>A task representing the asynchronous loop.</returns>
        private static async Task RunAsync(RemoteConfig config, bool debug, CancellationToken cancellationToken)
        {
            Console.Title = $"7 Days to Die - Horde & Airdrop Viewer - {config.ServerHost}:{config.ServerPort}";

            _ = UpdateBootstrapper.TryStart(config);

            IConnection connection;
            try
            {
                connection = debug
                    ? new ConnectionUDP()
                    : new ConnectionUDP(config.ServerHost, config.ServerPort);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to initialize connection: {ex.Message}");
                return;
            }

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
