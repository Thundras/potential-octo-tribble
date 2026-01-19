using System;

namespace _7D2D_ServerInfo
{
    internal static class ProgramHelpers
    {
        internal static bool IsDebugMode(string[] args)
        {
            foreach (string arg in args)
            {
                if (string.Equals(arg, "/Debug", StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        internal static TimeSpan GetRefreshDelay(double refreshIntervalSeconds)
        {
            if (refreshIntervalSeconds <= 0)
                return TimeSpan.FromSeconds(1);

            return TimeSpan.FromSeconds(refreshIntervalSeconds);
        }
    }
}
