using System;
using NetSparkleUpdater;
using NetSparkleUpdater.Enums;
using NetSparkleUpdater.SignatureVerifiers;

namespace _7D2D_ServerInfo
{
    internal static class UpdateBootstrapper
    {
        public static SparkleUpdater? TryStart(RemoteConfig config)
        {
            if (string.IsNullOrWhiteSpace(config.UpdateAppCastUrl))
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(config.UpdatePublicKey))
            {
                return null;
            }

            if (!Uri.TryCreate(config.UpdateAppCastUrl, UriKind.Absolute, out Uri? appCastUri))
            {
                Console.Error.WriteLine($"Update appcast URL '{config.UpdateAppCastUrl}' is invalid.");
                return null;
            }

            if (!string.Equals(appCastUri.Scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase))
            {
                // Enforce HTTPS to prevent downgrade or MITM issues.
                Console.Error.WriteLine("Update appcast URL must use HTTPS.");
                return null;
            }

            try
            {
                var verifier = new Ed25519Checker(SecurityMode.Strict, config.UpdatePublicKey);
                var sparkle = new SparkleUpdater(config.UpdateAppCastUrl, verifier);
                sparkle.StartLoop(true, true);
                return sparkle;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to start updater: {ex.Message}");
                return null;
            }
        }
    }
}
