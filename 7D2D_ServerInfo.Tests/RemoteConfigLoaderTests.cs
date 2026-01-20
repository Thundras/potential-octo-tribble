using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using _7D2D_ServerInfo;
using Xunit;

namespace _7D2D_ServerInfo.Tests
{
    public class RemoteConfigLoaderTests
    {
        [Fact]
        public async Task LoadFromFileAsync_ReturnsNullWhenFileMissing()
        {
            string missingPath = Path.Combine(Path.GetTempPath(), $"missing-{Guid.NewGuid():N}.json");

            RemoteConfig? result = await RemoteConfigLoader.LoadFromFileAsync(missingPath, CancellationToken.None);

            Assert.Null(result);
        }

        [Fact]
        public async Task LoadFromFileAsync_ReturnsConfigForValidJson()
        {
            string path = Path.Combine(Path.GetTempPath(), $"config-{Guid.NewGuid():N}.json");
            string json = """
            {
              "serverHost": "localhost",
              "serverPort": 8080,
              "refreshIntervalSeconds": 2.5,
              "updateAppCastUrl": "https://example.com/appcast.xml",
              "updatePublicKey": "public-key"
            }
            """;

            await File.WriteAllTextAsync(path, json);

            try
            {
                RemoteConfig? result = await RemoteConfigLoader.LoadFromFileAsync(path, CancellationToken.None);

                Assert.NotNull(result);
                Assert.Equal("localhost", result!.ServerHost);
                Assert.Equal(8080, result.ServerPort);
                Assert.Equal(2.5, result.RefreshIntervalSeconds);
                Assert.Equal("https://example.com/appcast.xml", result.UpdateAppCastUrl);
                Assert.Equal("public-key", result.UpdatePublicKey);
            }
            finally
            {
                File.Delete(path);
            }
        }

        [Fact]
        public async Task LoadFromFileAsync_ThrowsForInvalidJson()
        {
            string path = Path.Combine(Path.GetTempPath(), $"config-{Guid.NewGuid():N}.json");

            await File.WriteAllTextAsync(path, "not-json");

            try
            {
                await Assert.ThrowsAsync<JsonException>(() =>
                    RemoteConfigLoader.LoadFromFileAsync(path, CancellationToken.None));
            }
            finally
            {
                File.Delete(path);
            }
        }

        [Fact]
        public async Task LoadAsync_ReturnsConfigFromEndpoint()
        {
            string json = """
            {
              "ServerHost": "example.test",
              "ServerPort": 9000,
              "RefreshIntervalSeconds": 1.25,
              "UpdateAppCastUrl": "https://example.com/appcast.xml",
              "UpdatePublicKey": "public-key"
            }
            """;

            Uri uri = StartHttpListener(json, out HttpListener listener, out Task handlerTask);

            try
            {
                RemoteConfig? result = await RemoteConfigLoader.LoadAsync(uri, CancellationToken.None);

                Assert.NotNull(result);
                Assert.Equal("example.test", result!.ServerHost);
                Assert.Equal(9000, result.ServerPort);
                Assert.Equal(1.25, result.RefreshIntervalSeconds);
                Assert.Equal("https://example.com/appcast.xml", result.UpdateAppCastUrl);
                Assert.Equal("public-key", result.UpdatePublicKey);
            }
            finally
            {
                listener.Stop();
                await handlerTask;
            }
        }

        [Fact]
        public async Task LoadAsync_ThrowsForInvalidJson()
        {
            Uri uri = StartHttpListener("invalid-json", out HttpListener listener, out Task handlerTask);

            try
            {
                await Assert.ThrowsAsync<JsonException>(() =>
                    RemoteConfigLoader.LoadAsync(uri, CancellationToken.None));
            }
            finally
            {
                listener.Stop();
                await handlerTask;
            }
        }

        [Fact]
        public async Task LoadAsync_ThrowsWhenCanceled()
        {
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
                RemoteConfigLoader.LoadAsync(new Uri("http://localhost:12345"), cts.Token));
        }

        private static Uri StartHttpListener(string responseBody, out HttpListener listener, out Task handlerTask)
        {
            int port = GetFreeTcpPort();
            string prefix = $"http://localhost:{port}/";
            listener = new HttpListener();
            listener.Prefixes.Add(prefix);
            listener.Start();

            handlerTask = Task.Run(async () =>
            {
                HttpListenerContext context = await listener.GetContextAsync();
                context.Response.StatusCode = 200;
                context.Response.ContentType = "application/json";
                await using var writer = new StreamWriter(context.Response.OutputStream);
                await writer.WriteAsync(responseBody);
                context.Response.Close();
            });

            return new Uri($"{prefix}config");
        }

        private static int GetFreeTcpPort()
        {
            TcpListener listener = new(IPAddress.Loopback, 0);
            listener.Start();
            int port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }
    }
}
