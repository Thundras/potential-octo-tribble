using System.IO;
using System.Net.Http;
using System.Text.Json;

namespace _7D2D_ServerInfo
{
    /// <summary>
    /// Configuration values fetched from a remote JSON document.
    /// </summary>
    /// <param name="ServerHost">Hostname or IP of the 7DTD server.</param>
    /// <param name="ServerPort">Base port for the server (query uses port + 1).</param>
    /// <param name="RefreshIntervalSeconds">UI refresh cadence in seconds.</param>
    /// <param name="UpdateAppCastUrl">URL to the NetSparkle appcast feed.</param>
    /// <param name="UpdatePublicKey">Ed25519 public key used for update verification.</param>
    internal sealed record RemoteConfig(
        string ServerHost,
        int ServerPort,
        double RefreshIntervalSeconds,
        string UpdateAppCastUrl,
        string UpdatePublicKey
    );

    /// <summary>
    /// Helper responsible for downloading and deserializing remote configuration.
    /// </summary>
    internal static class RemoteConfigLoader
    {
        // Reuse a single HttpClient instance to avoid socket exhaustion and to keep
        // DNS/connection pooling efficient across multiple config refreshes.
        private static readonly HttpClient HttpClient = new()
        {
            // Fail fast on stalled network calls so the app can surface errors quickly.
            Timeout = TimeSpan.FromSeconds(10)
        };
        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            // Accept case-insensitive JSON field names to reduce coupling to the
            // config provider's exact casing.
            PropertyNameCaseInsensitive = true
        };

        /// <summary>
        /// Downloads the configuration JSON from the provided URI and deserializes it.
        /// </summary>
        /// <param name="configUri">URI of the JSON configuration.</param>
        /// <param name="cancellationToken">Token to cancel the download.</param>
        /// <returns>The deserialized <see cref="RemoteConfig"/> or <c>null</c> if deserialization fails.</returns>
        public static async Task<RemoteConfig?> LoadAsync(Uri configUri, CancellationToken cancellationToken)
        {
            // Download the raw JSON stream and deserialize it directly to reduce
            // memory overhead from intermediate string allocations.
            await using var stream = await HttpClient.GetStreamAsync(configUri, cancellationToken);
            return await JsonSerializer.DeserializeAsync<RemoteConfig>(stream, SerializerOptions, cancellationToken);
        }

        /// <summary>
        /// Loads configuration from a local JSON file if available.
        /// </summary>
        /// <param name="configPath">Path to the local configuration JSON.</param>
        /// <param name="cancellationToken">Token to cancel the file read.</param>
        /// <returns>The deserialized <see cref="RemoteConfig"/> or <c>null</c> if deserialization fails.</returns>
        public static async Task<RemoteConfig?> LoadFromFileAsync(string configPath, CancellationToken cancellationToken)
        {
            if (!File.Exists(configPath))
            {
                return null;
            }

            await using var stream = File.OpenRead(configPath);
            return await JsonSerializer.DeserializeAsync<RemoteConfig>(stream, SerializerOptions, cancellationToken);
        }
    }
}
