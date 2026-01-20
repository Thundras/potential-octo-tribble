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

            var verifier = new Ed25519Checker(SecurityMode.Strict, config.UpdatePublicKey);
            var sparkle = new SparkleUpdater(config.UpdateAppCastUrl, verifier);
            sparkle.StartLoop(true, true);
            return sparkle;
        }
    }
}
