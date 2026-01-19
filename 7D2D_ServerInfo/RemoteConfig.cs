using System.Net.Http;
using System.Text.Json;

namespace _7D2D_ServerInfo
{
    internal sealed record RemoteConfig(
        string ServerHost,
        int ServerPort,
        double RefreshIntervalSeconds,
        string UpdateAppCastUrl,
        string UpdatePublicKey
    );

    internal static class RemoteConfigLoader
    {
        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public static async Task<RemoteConfig?> LoadAsync(Uri configUri, CancellationToken cancellationToken)
        {
            using var httpClient = new HttpClient();
            await using var stream = await httpClient.GetStreamAsync(configUri, cancellationToken);
            return await JsonSerializer.DeserializeAsync<RemoteConfig>(stream, SerializerOptions, cancellationToken);
        }
    }
}
