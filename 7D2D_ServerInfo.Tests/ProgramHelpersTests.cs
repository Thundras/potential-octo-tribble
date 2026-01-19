using System;
using _7D2D_ServerInfo;
using Xunit;

namespace _7D2D_ServerInfo.Tests
{
    public class ProgramHelpersTests
    {
        [Fact]
        public void IsDebugMode_ReturnsTrueWhenDebugArgumentPresent()
        {
            string[] args = { "/Debug" };

            bool result = ProgramHelpers.IsDebugMode(args);

            Assert.True(result);
        }

        [Fact]
        public void IsDebugMode_ReturnsTrueForDifferentCasing()
        {
            string[] args = { "/debug" };

            bool result = ProgramHelpers.IsDebugMode(args);

            Assert.True(result);
        }

        [Fact]
        public void IsDebugMode_ReturnsFalseWhenMissing()
        {
            string[] args = { "--other" };

            bool result = ProgramHelpers.IsDebugMode(args);

            Assert.False(result);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void GetRefreshDelay_ReturnsMinimumWhenNonPositive(double refreshIntervalSeconds)
        {
            TimeSpan delay = ProgramHelpers.GetRefreshDelay(refreshIntervalSeconds);

            Assert.Equal(TimeSpan.FromSeconds(1), delay);
        }

        [Fact]
        public void GetRefreshDelay_ReturnsConfiguredDelay()
        {
            TimeSpan delay = ProgramHelpers.GetRefreshDelay(2.5);

            Assert.Equal(TimeSpan.FromSeconds(2.5), delay);
        }
    }
}
