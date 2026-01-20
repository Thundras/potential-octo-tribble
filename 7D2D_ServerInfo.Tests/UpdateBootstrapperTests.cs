using _7D2D_ServerInfo;
using Xunit;

namespace _7D2D_ServerInfo.Tests
{
    public class UpdateBootstrapperTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void TryStart_ReturnsNullWhenAppCastUrlMissing(string appCastUrl)
        {
            var config = new RemoteConfig("localhost", 8080, 1, appCastUrl, "public-key");

            var result = UpdateBootstrapper.TryStart(config);

            Assert.Null(result);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void TryStart_ReturnsNullWhenPublicKeyMissing(string publicKey)
        {
            var config = new RemoteConfig("localhost", 8080, 1, "https://example.com/appcast.xml", publicKey);

            var result = UpdateBootstrapper.TryStart(config);

            Assert.Null(result);
        }
    }
}
